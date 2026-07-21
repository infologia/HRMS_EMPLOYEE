using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_APPaymentGrid : System.Web.UI.Page
{
    SessionCustom sc;
    DataAccess da;
    PhTemplate PH;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.sc = new SessionCustom();
        this.da = new DataAccess();
        PH = new PhTemplate();
        
        if (!Page.IsPostBack)
        {
            Label lblBread = Master.FindControl("lbl_bread") as Label;
            if (lblBread != null)
            {
                lblBread.Text = "Payable Payment Details ";
            }
            this.BindAPPaymentGrid();
        }
    }

    private void BindAPPaymentGrid()
    {
        string query = @"
        SELECT 
            a.AP_Id,
            b.VendorName AS AP_VendorName,
            c.InvoiceNumber AS AP_InvoiceNo,
            CONVERT(VARCHAR(10), a.AP_InvoiceDate, 120) AS AP_InvoiceDate,
            a.AP_GrandTotal,
            a.AP_Payment,
            a.AP_BalanceAmount,
            CASE 
                WHEN a.AP_BalanceAmount <= 0 THEN 'Completed'
                ELSE 'Pending'
            END AS AP_Status,
            CASE 
                WHEN a.AP_BalanceAmount <= 0 THEN 'success'
                ELSE 'warning'
            END AS AP_Status_Class
        FROM IT_APPaymentEntry a 
        LEFT JOIN IT_Vendors b ON a.AP_VendorId = b.VendorKey
        LEFT JOIN IT_PayableInvoices c ON a.AP_InvoiceId = c.PayableInvoiceKey
        ORDER BY a.AP_CreatedOn DESC";

        SqlCommand cmd = new SqlCommand(query);
        DataTable dt = da.GetDataTable(cmd);

        if (dt.Rows.Count > 0)
        {
            DataSet ds = new DataSet();
            ds.Merge(dt);
            this.PH.LoadGridItem(ds, PH_APPayment, "APPaymentGrid.txt", "");
        }
    }

    [System.Web.Services.WebMethod]
    public static string DeleteAPPayment(int id)
    {
        try
        {
            DataAccess DA = new DataAccess();
            string query = "DELETE FROM IT_APPaymentEntry WHERE AP_Id = @AP_Id";
            SqlCommand cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@AP_Id", id);
            DA.ExecuteNonQuery(cmd);
            return "Success";
        }
        catch (Exception ex)
        {
            return "Error: " + ex.Message.Replace("'", "\\'").Replace("\r\n", " ");
        }
    }
}
