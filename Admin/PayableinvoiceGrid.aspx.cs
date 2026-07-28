using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

public partial class Admin_PayableinvoiceGrid : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();
        if (!IsPostBack)
        {
            BindFinancialYearDropdown();
            BindVendorDropdown();
            BindGrid();
            LoadTotalAmount();
            Label lblBread = Master.FindControl("lbl_bread") as Label;
            lblBread.Text = "Payable Invoices";

        }
    }

    private void BindVendorDropdown()
    {
        string query = "SELECT ClientKey, ClientName FROM IT_ClientDetails WHERE PartyType = 1 AND Status = 1 ORDER BY ClientName";
        DataTable dt = DA.GetDataTable(new SqlCommand(query));

        ddlVendor.Items.Clear();
        ddlVendor.Items.Add(new ListItem("All Vendors", "0"));

        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
                ddlVendor.Items.Add(new ListItem(dr["ClientName"].ToString(), dr["ClientKey"].ToString()));
        }
    }

    protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
        LoadTotalAmount();
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
            ddlFinancialYear.Items.Add(new ListItem(fyText, fyValue));
        }
    }

    protected void ddlFinancialYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
        LoadTotalAmount();
    }

    private void GetFinancialYearDates(out DateTime startDate, out DateTime endDate)
    {
        int startYear = Convert.ToInt32(ddlFinancialYear.SelectedValue);
        startDate = new DateTime(startYear, 4, 1);
        endDate = new DateTime(startYear + 1, 3, 31, 23, 59, 59);
    }

    private void LoadTotalAmount()
    {
        DateTime fyStart, fyEnd;
        GetFinancialYearDates(out fyStart, out fyEnd);

        string vendorFilter = ddlVendor.SelectedValue != "0" ? " AND a.VendorNameNew = @VendorKey" : "";
        string query = "SELECT ISNULL(SUM(a.InvoiceAmount), 0) AS TotalAmount FROM IT_PayableInvoices a WHERE ISNULL(a.InvoiceDate, a.CreatedOn) >= @FYStart AND ISNULL(a.InvoiceDate, a.CreatedOn) <= @FYEnd" + vendorFilter;
        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@FYStart", fyStart);
        cmd.Parameters.AddWithValue("@FYEnd", fyEnd);

        if (ddlVendor.SelectedValue != "0")
            cmd.Parameters.AddWithValue("@VendorKey", ddlVendor.SelectedValue);

        DataTable dt = DA.GetDataTable(cmd);
        if (dt.Rows.Count > 0)
            lblTotalAmount.Text = Convert.ToDecimal(dt.Rows[0]["TotalAmount"]).ToString("0.00");
    }

    private void BindGrid()
    {
        DateTime fyStart, fyEnd;
        GetFinancialYearDates(out fyStart, out fyEnd);

        string vendorFilter = ddlVendor.SelectedValue != "0" ? " AND a.VendorNameNew = @VendorKey" : "";

        string query1 = @"SELECT a.PayableInvoiceKey, b.ClientName AS VendorName, a.InvoiceNumber, a.InvoiceDate, a.DueDate, a.InvoiceAmount, a.PaymentStatus, a.CreatedOn
                          FROM IT_PayableInvoices a
                          LEFT JOIN IT_ClientDetails b ON a.VendorNameNew = b.ClientKey
                          WHERE ISNULL(a.InvoiceDate, a.CreatedOn) >= @FYStart AND ISNULL(a.InvoiceDate, a.CreatedOn) <= @FYEnd" + vendorFilter + " ORDER BY a.InvoiceDate DESC";
        SqlCommand cmd1 = new SqlCommand(query1);
        cmd1.Parameters.AddWithValue("@FYStart", fyStart);
        cmd1.Parameters.AddWithValue("@FYEnd", fyEnd);

        if (ddlVendor.SelectedValue != "0")
            cmd1.Parameters.AddWithValue("@VendorKey", ddlVendor.SelectedValue);

        DataTable dt_dashboard = DA.GetDataTable(cmd1);
        DataSet ds = new DataSet();
        ds.Merge(dt_dashboard);

        if (dt_dashboard.Rows.Count > 0)
        {
            ds.Tables[0].Columns.Add("ActiveText");
            ds.Tables[0].Columns.Add("Company_Name");
            ds.Tables[0].Columns.Add("Invoice_Number");
            ds.Tables[0].Columns.Add("Invoice_Date");
            ds.Tables[0].Columns.Add("Due_Date");
            ds.Tables[0].Columns.Add("Created_Date");
            ds.Tables[0].Columns.Add("Remove");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string str_Status = dr["PaymentStatus"].ToString();
                string InvoiceKey = dr["PayableInvoiceKey"].ToString();

                dr["Company_Name"] = dr["VendorName"].ToString();
                dr["Invoice_Number"] = dr["InvoiceNumber"].ToString();
                dr["Invoice_Date"] = dr["InvoiceDate"] != DBNull.Value ? Convert.ToDateTime(dr["InvoiceDate"]).ToString("yyyy-MM-dd") : "";
                dr["Due_Date"] = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]).ToString("yyyy-MM-dd") : "";
                dr["Created_Date"] = dr["CreatedOn"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedOn"]).ToString("yyyy-MM-dd") : "";

                if (str_Status == "1" || str_Status.ToLower() == "paid" || str_Status.ToLower() == "completed")
                {
                    dr["ActiveText"] = "<span class='label label-success'>Completed</span>";
                    dr["Remove"] = "<a href='javascript:void(0);' class='text-muted' style='cursor:not-allowed; opacity:0.6;' title='Remove'><i class='icon-trash'></i></a>";
                }
                else
                {
                    dr["ActiveText"] = "<span class='label label-sm label-warning'>Pending</span>";
                    dr["Remove"] = "<a href='javascript:void(0);' class='text-danger' style='cursor:pointer;' onclick=\"fn_DeleteProject('" + InvoiceKey + "')\" title='Remove'><i class='icon-trash'></i></a>";
                }
            }

            this.PH.LoadGridItem(ds, PH_PAYABLEINVOICE, "Payableinvoice.txt", "");
        }
    }

    [WebMethod]
    public static string DeleteProject(string str_InvoiceKey)
    {
        string str_Response = "0";
        try
        {
            DataAccess DA1 = new DataAccess();
            SaveQuery SAQ = new SaveQuery();
            SessionCustom SC = new SessionCustom();
            string str_Sql = "delete from IT_PayableInvoices where PayableInvoiceKey=@InvoiceKey";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@InvoiceKey", str_InvoiceKey);
            DA1.ExecuteNonQuery(cmd);

           


            return str_Response = "1";

        }
        catch (Exception ex)
        {
            return str_Response;
        }
    }
}