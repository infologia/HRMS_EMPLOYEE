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
            this.BindFinancialYearDropdown();
            this.BindARPaymentGrid();
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
        BindARPaymentGrid();
    }

    private void GetFinancialYearDates(out DateTime startDate, out DateTime endDate)
    {
        int startYear = Convert.ToInt32(ddlFinancialYear.SelectedValue);
        startDate = new DateTime(startYear, 4, 1);
        endDate = new DateTime(startYear + 1, 3, 31, 23, 59, 59);
    }

    private void BindARPaymentGrid()
    {
        DateTime fyStart, fyEnd;
        GetFinancialYearDates(out fyStart, out fyEnd);

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
        WHERE ISNULL(a.AR_InvoiceDate, a.AR_CreatedOn) >= @FYStart 
          AND ISNULL(a.AR_InvoiceDate, a.AR_CreatedOn) <= @FYEnd
        ORDER BY a.AR_CreatedOn DESC";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@FYStart", fyStart);
        cmd.Parameters.AddWithValue("@FYEnd", fyEnd);
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
