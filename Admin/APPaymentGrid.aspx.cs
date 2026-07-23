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
            this.BindFinancialYearDropdown();
            this.BindAPPaymentGrid();
        }
    }

    private void BindFinancialYearDropdown()
    {
        ddlFinancialYear.Items.Clear();
        int currentYear = DateTime.Now.Year;
        int currentMonth = DateTime.Now.Month;
        int startYear = currentMonth >= 4 ? currentYear : currentYear - 1;

        for (int y = startYear; y >= 2020; y--)
        {
            string fyText = "FY " + y + "-" + (y + 1).ToString().Substring(2, 2);
            string fyValue = y.ToString();
            ddlFinancialYear.Items.Add(new System.Web.UI.WebControls.ListItem(fyText, fyValue));
        }
    }

    protected void ddlFinancialYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindAPPaymentGrid();
    }

    private void GetFinancialYearDates(out DateTime startDate, out DateTime endDate)
    {
        int startYear = Convert.ToInt32(ddlFinancialYear.SelectedValue);
        startDate = new DateTime(startYear, 4, 1);
        endDate = new DateTime(startYear + 1, 3, 31, 23, 59, 59);
    }

    private void BindAPPaymentGrid()
    {
        DateTime fyStart, fyEnd;
        GetFinancialYearDates(out fyStart, out fyEnd);

        string query = @"
        SELECT 
            a.AP_Id,
            b.ClientName AS AP_VendorName,
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
        LEFT JOIN IT_ClientDetails b ON a.AP_VendorId = b.ClientKey
        LEFT JOIN IT_PayableInvoices c ON a.AP_InvoiceId = c.PayableInvoiceKey
        WHERE ISNULL(a.AP_InvoiceDate, a.AP_CreatedOn) >= @FYStart 
          AND ISNULL(a.AP_InvoiceDate, a.AP_CreatedOn) <= @FYEnd
        ORDER BY a.AP_CreatedOn DESC";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@FYStart", fyStart);
        cmd.Parameters.AddWithValue("@FYEnd", fyEnd);
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
