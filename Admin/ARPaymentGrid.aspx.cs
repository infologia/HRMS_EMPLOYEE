using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_ARPaymentGrid : System.Web.UI.Page
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
                lblBread.Text = "AR Payment Grid";
            }
            this.BindARPaymentGrid();
        }
    }

    private void BindARPaymentGrid()
    {
        string query = @"
        SELECT 
            a.AR_Id,
            b.CompanyName AS AR_ClientName,
            c.InvoiceNumber AS AR_InvoiceNo,
            CONVERT(VARCHAR(10), a.AR_InvoiceDate, 120) AS AR_InvoiceDate,
            a.AR_GrandTotal,
            a.AR_Payment,
            a.AR_BalanceAmount,
            CASE 
                WHEN a.AR_BalanceAmount <= 0 THEN 'Completed'
                ELSE 'Pending'
            END AS AR_Status,
            CASE 
                WHEN a.AR_BalanceAmount <= 0 THEN 'success'
                ELSE 'warning'
            END AS AR_Status_Class
        FROM IT_ARPaymentEntry a 
        LEFT JOIN IT_ClientDetails b ON a.AR_ClientId = b.ClientKey
        LEFT JOIN IT_Invoices c ON a.AR_InvoiceId = c.InvoiceKey
        ORDER BY a.AR_CreatedOn DESC";

        SqlCommand cmd = new SqlCommand(query);
        DataTable dt = da.GetDataTable(cmd);

        if (dt.Rows.Count > 0)
        {
            DataSet ds = new DataSet();
            ds.Merge(dt);
            this.PH.LoadGridItem(ds, PH_ARPayment, "ARPaymentGrid.txt", "");
        }
    }

    [System.Web.Services.WebMethod]
    public static string DeleteARPayment(int id)
    {
        try
        {
            DataAccess DA = new DataAccess();
            string query = "DELETE FROM IT_ARPaymentEntry WHERE AR_Id = @AR_Id";
            SqlCommand cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@AR_Id", id);
            DA.ExecuteNonQuery(cmd);
            return "Success";
        }
        catch (Exception ex)
        {
            return "Error: " + ex.Message.Replace("'", "\\'").Replace("\r\n", " ");
        }
    }
}
