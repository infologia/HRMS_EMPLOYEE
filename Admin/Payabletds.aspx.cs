using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Payabletds : System.Web.UI.Page
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
                control1.Text = "Payable TDS";
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
        c.VendorName,
        i.InvoiceNumber,
        i.InvoiceAmount,
        i.TDSAmount,
        i.TotalPayableAmount,
        i.PaymentStatus,
        CONVERT(VARCHAR(10), i.InvoiceDate, 103) AS InvoiceDate
    FROM IT_PayableInvoices i
    INNER JOIN IT_Vendors c
        ON i.VendorName = c.VendorKey
    WHERE i.TDSAmount > 0
      AND ISNULL(i.InvoiceDate, i.CreatedOn) >= @FYStart 
      AND ISNULL(i.InvoiceDate, i.CreatedOn) <= @FYEnd
    ORDER BY i.InvoiceDate DESC;
    ";

        using (SqlCommand cmd = new SqlCommand(str_query))
        {
            cmd.Parameters.AddWithValue("@FYStart", fyStart);
            cmd.Parameters.AddWithValue("@FYEnd", fyEnd);
            DataTable dtTds = DA.GetDataTable(cmd);

            DataSet ds = new DataSet();
            ds.Tables.Add(dtTds);

            // 🔹 Add PaymentStatusText column
            if (!ds.Tables[0].Columns.Contains("PaymentStatusText"))
                ds.Tables[0].Columns.Add("PaymentStatusText");

            // 🔹 Status mapping: 0 = Pending, 1 = Completed
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int paymentStatus = Convert.ToInt32(dr["PaymentStatus"]);

                if (paymentStatus == 1)
                    dr["PaymentStatusText"] =
                        "<span class='label label-success'>Completed</span>";
                else
                    dr["PaymentStatusText"] =
                        "<span class='label label-warning'>Pending</span>";
            }

            // 🔹 Load grid
            PH.LoadGridItem(ds, PH_TDSInvoices, "payabletds.txt", "");
        }
    }



}