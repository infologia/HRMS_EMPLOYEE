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
            LoadTotalAmounts();
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
        LoadTotalAmounts();
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
        i.InvoiceStatus,
        s.name as StatusName,
        CONVERT(VARCHAR(10), i.InvoiceDate, 23) AS InvoiceDate
    FROM IT_Invoices i
    INNER JOIN IT_ClientDetails c ON i.ClientKey = c.ClientKey
    LEFT JOIN IT_Countries cnt ON cnt.CountryKey = c.Country
    LEFT JOIN IT_InvoiceStatus s ON i.InvoiceStatus = s.id
    WHERE i.TDSAmount > 0 
      AND ISNULL(i.InvoiceDate, i.CreatedOn) >= @FYStart 
      AND ISNULL(i.InvoiceDate, i.CreatedOn) <= @FYEnd
      AND cnt.Country = 'India'
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

            // 🔹 Status logic (Given / Received / Cancelled)
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int invoiceStatus = dr["InvoiceStatus"] != DBNull.Value ? Convert.ToInt32(dr["InvoiceStatus"]) : 0;
                string statusName = dr["StatusName"] != DBNull.Value ? dr["StatusName"].ToString() : "Pending";

                string labelClass = "label-warning"; 
                if (invoiceStatus == 1)
                {
                    labelClass = "label-primary"; 
                }
                else if (invoiceStatus == 2)
                {
                    labelClass = "label-success"; 
                }
                else if (invoiceStatus == 3)
                {
                    labelClass = "label-danger"; 
                }
                
                if(string.IsNullOrEmpty(statusName)) statusName = "Pending";

                dr["StatusText"] = "<span class='label label-sm " + labelClass + "'>" + statusName + "</span>";
            }

            // 🔹 Load to grid
            PH.LoadGridItem(ds, PH_TDSInvoices, "tds.txt", "");
        }
    }


    private void LoadTotalAmounts()
    {
        DateTime fyStart, fyEnd;
        GetFinancialYearDates(out fyStart, out fyEnd);

        string query = @"SELECT 
                ISNULL(SUM(i.TDSAmount), 0) AS TotalTDS, 
                ISNULL(SUM(i.InvoiceAmount), 0) AS TotalInvoiceAmount,
                ISNULL(SUM(i.TotalAmount), 0) AS TotalAmount 
            FROM IT_Invoices i
            INNER JOIN IT_ClientDetails c ON i.ClientKey = c.ClientKey
            LEFT JOIN IT_Countries cnt ON cnt.CountryKey = c.Country
            WHERE i.TDSAmount > 0 
              AND ISNULL(i.InvoiceDate, i.CreatedOn) >= @FYStart 
              AND ISNULL(i.InvoiceDate, i.CreatedOn) <= @FYEnd
              AND cnt.Country = 'India'";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@FYStart", fyStart);
        cmd.Parameters.AddWithValue("@FYEnd", fyEnd);

        DataTable dt = DA.GetDataTable(cmd);
        if (dt != null && dt.Rows.Count > 0)
        {
            lblTotalTDS.Text = Convert.ToDecimal(dt.Rows[0]["TotalTDS"]).ToString("0.00");
            lblTotalInvoiceAmount.Text = Convert.ToDecimal(dt.Rows[0]["TotalInvoiceAmount"]).ToString("0.00");
            lblTotalAmount.Text = Convert.ToDecimal(dt.Rows[0]["TotalAmount"]).ToString("0.00");
        }
        else
        {
            lblTotalTDS.Text = "0.00";
            lblTotalInvoiceAmount.Text = "0.00";
            lblTotalAmount.Text = "0.00";
        }
    }
}