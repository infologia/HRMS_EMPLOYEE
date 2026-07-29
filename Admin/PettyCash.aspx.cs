using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_PettyCash : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();

        string str_userid = this.SC.Userid;

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Petty Cash";

            BindDateDropdown();
            PopulateYearDropdown();
            BindFinancialYearDropdown();

            ddlFinancialYear.SelectedValue = "0";
            ddlDate.SelectedValue = "0";
            ddlYear.SelectedValue = DateTime.Now.Year.ToString();

            LoadCashByMonthYear();
        }

        if (SC.UserRole == "0")
        {
            id_pettycash.Visible = true;
        }
    }

    private void BindFinancialYearDropdown()
    {
        ddlFinancialYear.Items.Clear();
        ddlFinancialYear.Items.Add(new ListItem("All FY", "0"));
        int currentYear  = DateTime.Now.Year;
        int currentMonth = DateTime.Now.Month;
        int fyStartYear  = currentMonth >= 4 ? currentYear : currentYear - 1;
        for (int y = fyStartYear; y >= 2020; y--)
            ddlFinancialYear.Items.Add(new ListItem("FY " + y + "-" + (y + 1).ToString().Substring(2, 2), y.ToString()));
    }

    private void GetFYDates(out DateTime? fyStart, out DateTime? fyEnd)
    {
        string fyVal = ddlFinancialYear.SelectedValue;
        if (fyVal == "0") { fyStart = null; fyEnd = null; return; }
        int startYear = Convert.ToInt32(fyVal);
        fyStart = new DateTime(startYear, 4, 1);
        fyEnd   = new DateTime(startYear + 1, 3, 31, 23, 59, 59);
    }

    private void LoadCash(int month, int year, int type)
    {
        DateTime? fyStart, fyEnd;
        GetFYDates(out fyStart, out fyEnd);

        string str_query = @"
    SELECT
    a.PC_CashKey,
    a.PC_Description,
    a.PC_Amount,
    a.PC_BalanceAmount,
    a.PC_Status,
    a.PC_Date,
    ISNULL(b.Firstname + ' ' + b.Lastname, '') AS Username
FROM TT_PettyCash a
LEFT JOIN IT_EmployeeRegister b
    ON a.CreatedBy = b.EmployeeKey
WHERE
    (@FYStart IS NULL OR a.PC_Date >= @FYStart)
    AND (@FYEnd IS NULL OR a.PC_Date <= @FYEnd)
    AND (@Type = 0 OR a.PC_Status = @Type)
    AND (
        (@Year = 0 AND @Month = 0)
        OR (@Year = 0 AND @Month BETWEEN 1 AND 12 AND MONTH(a.PC_Date) = @Month)
        OR (@Year > 0 AND @Month = 0 AND YEAR(a.PC_Date) = @Year)
        OR (@Year > 0 AND @Month BETWEEN 1 AND 12 AND MONTH(a.PC_Date) = @Month AND YEAR(a.PC_Date) = @Year)
        OR (@Month = -1 AND CAST(a.PC_Date AS DATE) = CAST(GETDATE() AS DATE))
    )
ORDER BY a.PC_Date DESC;";

        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@Month", month);
        cmd.Parameters.AddWithValue("@Year", year);
        cmd.Parameters.AddWithValue("@Type", type);
        cmd.Parameters.AddWithValue("@FYStart", fyStart.HasValue ? (object)fyStart.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("@FYEnd",   fyEnd.HasValue   ? (object)fyEnd.Value   : DBNull.Value);

        DataTable dt = DA.GetDataTable(cmd);

        if (dt.Rows.Count > 0)
        {

            if (!dt.Columns.Contains("ActiveText"))
                dt.Columns.Add("ActiveText");
            if (!dt.Columns.Contains("PC_DateText"))
                dt.Columns.Add("PC_DateText", typeof(string));
            if (!dt.Columns.Contains("ActionText"))
                dt.Columns.Add("ActionText", typeof(string));

            int curMonth = DateTime.Now.Month;
            int curYear  = DateTime.Now.Year;

            foreach (DataRow dr in dt.Rows)
            {
                if (dr["PC_Date"] != DBNull.Value)
                {
                    DateTime pcDate = Convert.ToDateTime(dr["PC_Date"]);
                    dr["PC_DateText"] = pcDate.ToString("dd/MM/yyyy");

                    bool isCurrentMonth = (pcDate.Month == curMonth && pcDate.Year == curYear);
                    string key = dr["PC_CashKey"].ToString();

                    if (isCurrentMonth)
                    {
                        dr["ActionText"] = "<a href='CreatePettyCash.aspx?id=" + key + "' title='Update'><i class='icon-pencil7 text-primary'></i></a>" +
                                           "&nbsp;&nbsp;<a href='javascript:void(0);' title='Remove' onclick='fn_DeleteProject(" + key + ")'><i class='icon-trash text-danger'></i></a>";
                    }
                    else
                    {
                        dr["ActionText"] = "<a title='Edit disabled for past months' style='opacity:0.4; cursor:not-allowed; pointer-events:none;'><i class='icon-pencil7 text-muted'></i></a>" +
                                           "&nbsp;&nbsp;<a title='Delete disabled for past months' style='opacity:0.4; cursor:not-allowed; pointer-events:none;'><i class='icon-trash text-muted'></i></a>";
                    }
                }
                int status = Convert.ToInt32(dr["PC_Status"]);
                dr["ActiveText"] = status == 1
                    ? "<span class='label label-sm label-success'>CR</span>"
                    : "<span class='label label-sm label-danger'>DT</span>";
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            PH.LoadGridItem(ds, PH_PettyCash, "PettyCash.txt", "");
        }
    }

    private void LoadPettyCashTotals(int month, int year, int type)
    {
        DateTime? fyStart, fyEnd;
        GetFYDates(out fyStart, out fyEnd);

        string query = @"
SELECT
    ISNULL(SUM(CASE WHEN PC_Status = 1 THEN PC_Amount ELSE 0 END), 0) AS CRAmount,
    ISNULL(SUM(CASE WHEN PC_Status = 2 THEN PC_Amount ELSE 0 END), 0) AS DTAmount,
    ISNULL(
        (SELECT TOP 1 PC_BalanceAmount FROM TT_PettyCash
         WHERE (@FYStart IS NULL OR PC_Date >= @FYStart)
           AND (@FYEnd IS NULL OR PC_Date <= @FYEnd)
           AND (@Type = 0 OR PC_Status = @Type)
           AND (
               (@Year = 0 AND @Month = 0)
               OR (@Year = 0 AND @Month BETWEEN 1 AND 12 AND MONTH(PC_Date) = @Month)
               OR (@Year > 0 AND @Month = 0 AND YEAR(PC_Date) = @Year)
               OR (@Year > 0 AND @Month BETWEEN 1 AND 12 AND MONTH(PC_Date) = @Month AND YEAR(PC_Date) = @Year)
               OR (@Month = -1 AND CAST(PC_Date AS DATE) = CAST(GETDATE() AS DATE))
           )
         ORDER BY PC_Date DESC, PC_CashKey DESC), 0
    ) AS BalanceAmount
FROM TT_PettyCash
WHERE
    (@FYStart IS NULL OR PC_Date >= @FYStart)
    AND (@FYEnd IS NULL OR PC_Date <= @FYEnd)
    AND (@Type = 0 OR PC_Status = @Type)
    AND (
        (@Year = 0 AND @Month = 0)
        OR (@Year = 0 AND @Month BETWEEN 1 AND 12 AND MONTH(PC_Date) = @Month)
        OR (@Year > 0 AND @Month = 0 AND YEAR(PC_Date) = @Year)
        OR (@Year > 0 AND @Month BETWEEN 1 AND 12 AND MONTH(PC_Date) = @Month AND YEAR(PC_Date) = @Year)
        OR (@Month = -1 AND CAST(PC_Date AS DATE) = CAST(GETDATE() AS DATE))
    );";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@Month", month);
        cmd.Parameters.AddWithValue("@Year", year);
        cmd.Parameters.AddWithValue("@Type", type);
        cmd.Parameters.AddWithValue("@FYStart", fyStart.HasValue ? (object)fyStart.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("@FYEnd",   fyEnd.HasValue   ? (object)fyEnd.Value   : DBNull.Value);

        DataTable dt = DA.GetDataTable(cmd);

        if (dt.Rows.Count > 0)
        {
            lblCR.Text = Convert.ToDecimal(dt.Rows[0]["CRAmount"]).ToString("0.00");
            lblDT.Text = Convert.ToDecimal(dt.Rows[0]["DTAmount"]).ToString("0.00");
            lblBalance.Text = Convert.ToDecimal(dt.Rows[0]["BalanceAmount"]).ToString("0.00");
        }
    }




    [System.Web.Services.WebMethod]
    public static string DeleteCash(int CashKey)
    {
        try
        {
            DataAccess DA = new DataAccess();

            string query = "DELETE FROM TT_PettyCash WHERE PC_CashKey = @PC_CashKey";

            SqlCommand cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@PC_CashKey", CashKey);

            DA.ExecuteNonQuery(cmd);

            return "SUCCESS";
        }
        catch (Exception)
        {
            return "ERROR";
        }
    }


    private void BindDateDropdown()
    {
        ddlDate.Items.Clear();

        ddlDate.Items.Add(new ListItem("All", "0"));
        ddlDate.Items.Add(new ListItem("Today", "-1"));

        for (int m = 1; m <= 12; m++)
        {
            string monthName = new DateTime(2000, m, 1).ToString("MMMM");
            ddlDate.Items.Add(new ListItem(monthName, m.ToString()));
        }

        // Page load default → All
        ddlDate.SelectedValue = "0";
    }
    private void PopulateYearDropdown()
    {
        ddlYear.Items.Clear();
        int currentYear = DateTime.Now.Year;

        ddlYear.Items.Add(new ListItem("All", "0"));

        for (int year = currentYear - 5; year <= currentYear + 5; year++)
            ddlYear.Items.Add(new ListItem(year.ToString(), year.ToString()));

        ddlYear.SelectedValue = currentYear.ToString();
    }


    private void LoadCashByMonthYear()
    {
        int month = Convert.ToInt32(ddlDate.SelectedValue);
        int year  = Convert.ToInt32(ddlYear.SelectedValue);
        int type  = Convert.ToInt32(ddlType.SelectedValue);

        LoadCash(month, year, type);
        LoadPettyCashTotals(month, year, type);
    }


    protected void btnApplyFilter_Click(object sender, EventArgs e)
    {
        LoadCashByMonthYear();
    }

    protected void ddlType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCashByMonthYear();
    }

    protected void ddlFinancialYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCashByMonthYear();
    }

    protected void ddlDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCashByMonthYear();
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCashByMonthYear();
    }

}