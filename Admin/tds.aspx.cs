using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_tds : System.Web.UI.Page
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
                control1.Text = "Receivable TDS";
            BindFinancialYearDropdown();
            LoadTds();
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
        LoadTds();
    }

    private void GetFinancialYearDates(out DateTime startDate, out DateTime endDate)
    {
        int startYear = Convert.ToInt32(ddlFinancialYear.SelectedValue);
        startDate = new DateTime(startYear, 4, 1);
        endDate = new DateTime(startYear + 1, 3, 31, 23, 59, 59);
    }

    private void LoadTds()
    {
        DateTime fyStart, fyEnd;
        GetFinancialYearDates(out fyStart, out fyEnd);

        string str_query = @"SELECT 
        c.ClientName,
        i.InvoiceNumber,
        i.InvoiceAmount,
        i.TDSAmount,
        i.TotalAmount,
        i.Status,
        CONVERT(VARCHAR(10), i.InvoiceDate, 103) AS InvoiceDate
    FROM IT_Invoices i
    INNER JOIN IT_ClientDetails c ON i.ClientKey = c.ClientKey
    WHERE i.TDSAmount > 0 AND ISNULL(i.InvoiceDate, i.CreatedOn) >= @FYStart AND ISNULL(i.InvoiceDate, i.CreatedOn) <= @FYEnd
    ORDER BY i.InvoiceDate DESC;";

        using (SqlCommand cmd = new SqlCommand(str_query))
        {
            cmd.Parameters.AddWithValue("@FYStart", fyStart);
            cmd.Parameters.AddWithValue("@FYEnd", fyEnd);
            DataTable dtTds = DA.GetDataTable(cmd);

            DataSet ds = new DataSet();
            ds.Tables.Add(dtTds);

            // 🔹 Add StatusText column for Paid / Unpaid
            if (!ds.Tables[0].Columns.Contains("StatusText"))
                ds.Tables[0].Columns.Add("StatusText");

            // 🔹 Status logic (Paid / Unpaid)
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int status = Convert.ToInt32(dr["Status"]);

                if (status == 0) // Pending
                    dr["StatusText"] = "<span class='label label-danger'>Pending</span>";
                else // Received
                    dr["StatusText"] = "<span class='label label-success'>Received</span>";
            }

            // 🔹 Load to grid
            PH.LoadGridItem(ds, PH_TDSInvoices, "tds.txt", "");
        }
    }

}