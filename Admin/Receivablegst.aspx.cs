using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_gst : System.Web.UI.Page
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
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Receivable GST";
            
            BindFinancialYearDropdown();
            LoadInvoiceGrid();
        }
        if (Request["__EVENTTARGET"] == "PayInvoice")
        {
            PayInvoice();
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
        LoadInvoiceGrid();
    }

    private void GetFinancialYearDates(out DateTime startDate, out DateTime endDate)
    {
        int startYear = Convert.ToInt32(ddlFinancialYear.SelectedValue);
        startDate = new DateTime(startYear, 4, 1);
        endDate = new DateTime(startYear + 1, 3, 31, 23, 59, 59);
    }

    private void LoadInvoiceGrid()
    {
        DateTime fyStart, fyEnd;
        GetFinancialYearDates(out fyStart, out fyEnd);

        string str_query = @"
    SELECT 
        i.InvoiceKey,
        i.InvoiceNumber,
        CONVERT(varchar(10), i.InvoiceDate, 23) AS InvoiceDate, 
        i.GSTAmount,
        CONVERT(varchar(10), i.GSTpaiddate, 23) AS GSTpaiddate, 
        i.InvoiceAmount,
        i.Status,
        i.GSTstatus
    FROM IT_Invoices i
    INNER JOIN IT_ClientDetails c ON i.ClientKey = c.ClientKey
    LEFT JOIN IT_Countries cnt ON cnt.CountryKey = c.Country
    WHERE ISNULL(i.InvoiceDate, i.CreatedOn) >= @FYStart 
      AND ISNULL(i.InvoiceDate, i.CreatedOn) <= @FYEnd
      AND cnt.Country = 'India'
    ORDER BY i.CreatedOn DESC";

        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@FYStart", fyStart);
        cmd.Parameters.AddWithValue("@FYEnd", fyEnd);

        DataTable dt_invoice = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_invoice);

        if (dt_invoice.Rows.Count > 0)
        {
            if (!ds.Tables[0].Columns.Contains("StatusText"))
                ds.Tables[0].Columns.Add("StatusText");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int status = Convert.ToInt32(dr["GSTstatus"]); // 0 = Unpaid, 1 = Paid
               
                if (status == 1)
                {
                    dr["StatusText"] = "<span class='label label-success'>Paid</span>";
                }
                else
                {
                    dr["StatusText"] = "<span class='label label-warning' style='cursor:pointer' " +
                        "onclick=\"fn_PayInvoice('" + dr["InvoiceKey"] + "')\">Unpaid</span>";
                }
            }
            this.PH.LoadGridItem(ds, PH_Project, "gst.txt", "");
        }
    }

    private void PayInvoice()
    {
        int invoiceKey = Convert.ToInt32(hdnInvoiceKey.Value);
        string description = txtDescription.Text.Trim();

        string str_query = @"
UPDATE IT_Invoices
SET
    GSTstatus = 1,
    GSTpaiddate = GETDATE(),
    GSTDescription = @Description,
    ModifiedOn = GETDATE()
WHERE InvoiceKey = @InvoiceKey";

        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@InvoiceKey", invoiceKey);
        cmd.Parameters.AddWithValue("@Description", description);

        DA.ExecuteNonQuery(cmd);

        // Optional: clear textbox after save
        txtDescription.Text = string.Empty;

        LoadInvoiceGrid();
    }

}