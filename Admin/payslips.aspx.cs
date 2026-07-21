using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_payslips : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    string str_month = "";
    string str_year = "";
    private DataTable _dt_grid = null;
    private bool is_view_mode = false; // true when arriving from Admin/viewpayroll.aspx "View" link (?view=1)

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();

        is_view_mode = (Request.QueryString["view"] == "1");

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = is_view_mode ? "Payroll - Saved Record" : "Payroll Grid";
            this.LoadMonthDropdown();
            this.LoadYearDropdown();
        }
        if (Request.QueryString["key"] != null && Request.QueryString["key"] != "")
            str_month = Request.QueryString["key"].ToString();

        if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            str_year = Request.QueryString["id"].ToString();

        if (is_view_mode)
        {
            // Viewing an already-saved payroll batch: trust the month/year passed in,
            // no "previous month only" restriction (that rule only applies to generating new payroll).
            if (str_month == "" || str_year == "")
            {
                DateTime prevMonthVM = DateTime.Now.AddMonths(-1);
                str_month = prevMonthVM.Month.ToString();
                str_year = prevMonthVM.Year.ToString();
            }
        }
        else
        {
            // Default to previous month (current month & future months are restricted)
            DateTime prevMonth = DateTime.Now.AddMonths(-1);
            if (str_month == "" || str_year == "")
            {
                str_month = prevMonth.Month.ToString();
                str_year  = prevMonth.Year.ToString();
            }
            else
            {
                // If query-string passes current/future month, fall back to previous month
                int qs_month = int.Parse(str_month);
                int qs_year  = int.Parse(str_year);
                DateTime selectedDate = new DateTime(qs_year, qs_month, 1);
                DateTime currentMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                if (selectedDate >= currentMonthStart)
                {
                    str_month = prevMonth.Month.ToString();
                    str_year  = prevMonth.Year.ToString();
                }
            }
        }

        if (!IsPostBack)
        {
            // Try setting selected value; if not in list (e.g. future year), leave as default
            if (ddl_month.Items.FindByValue(str_month) != null)
                ddl_month.SelectedValue = str_month;
            if (ddl_year.Items.FindByValue(str_year) != null)
                ddl_year.SelectedValue = str_year;
        }
        DateTime labelDate = new DateTime(int.Parse(str_year), int.Parse(str_month), 1);
        lbl_monthyear.Text = labelDate.ToString("MMMM yyyy");

        if (is_view_mode)
        {
            // Compact, read-only header: Month / Year / Load / Save / Export controls are not
            // relevant when looking at a specific saved payroll record, so hide them.
            div_controls.Visible = false;
            div_header.Attributes["class"] = "panel-heading ledger-header ledger-header-compact";
            btn_saveall.Visible = false;
            btn_export.Visible = false;
        }

        if (!IsPostBack)
        {
            this.LoadGrid();
        }
    }

    private void LoadMonthDropdown()
    {
        ddl_month.Items.Clear();
        for (int m = 1; m <= 12; m++)
        {
            string monthName = new DateTime(2025, m, 1).ToString("MMMM");
            ddl_month.Items.Add(new ListItem(monthName, m.ToString()));
        }
        ddl_month.SelectedValue = DateTime.Now.Month.ToString();
    }
    private void LoadYearDropdown()
    {
        ddl_year.Items.Clear();
        int currentYear = DateTime.Now.Year;
        // Show only past years up to current year (no future years)
        for (int year = currentYear - 5; year <= currentYear; year++)
        {
            ddl_year.Items.Add(new ListItem(year.ToString(), year.ToString()));
        }
        ListItem defaultYear = ddl_year.Items.FindByValue(currentYear.ToString());
        if (defaultYear != null)
            defaultYear.Selected = true;
    }
    protected void btn_load_Click(object sender, EventArgs e)
    {
        str_month = ddl_month.SelectedValue;
        str_year  = ddl_year.SelectedValue;

        int sel_month = int.Parse(str_month);
        int sel_year  = int.Parse(str_year);

        // Restrict: only previous months allowed; current month & future months blocked
        DateTime selectedDate    = new DateTime(sel_year, sel_month, 1);
        DateTime currentMonthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        if (selectedDate >= currentMonthStart)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert_restrict",
                "showToastr('error','Payroll can only be loaded for previous months. Current month and future months are not allowed.');", true);
            return;
        }

        DateTime labelDate = new DateTime(sel_year, sel_month, 1);
        lbl_monthyear.Text = labelDate.ToString("MMMM yyyy");

        this.LoadGrid();
    }
    protected void btn_export_Click(object sender, EventArgs e)
    {
        str_month = ddl_month.SelectedValue;
        str_year = ddl_year.SelectedValue;

        int month = int.Parse(str_month);
        int year = int.Parse(str_year);

        DataTable dt_export = GetPayrollDataTable(month, year);

        if (dt_export.Rows.Count == 0)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert_noexport",
                "showToastr('error','No payroll data found to export.');", true);
            return;
        }

        DateTime labelDate = new DateTime(year, month, 1);
        string fileName = "Payroll_" + labelDate.ToString("MMM_yyyy") + ".xls";

        // Columns shown in the grid, in display order: Key = source DataTable column, Value = header text
        var exportColumns = new System.Collections.Generic.KeyValuePair<string, string>[]
        {
            new System.Collections.Generic.KeyValuePair<string, string>("EmployeeName", "Employee Name"),
            new System.Collections.Generic.KeyValuePair<string, string>("Employeeid", "Employee ID"),
            new System.Collections.Generic.KeyValuePair<string, string>("NoOfDaysInMonth", "Days In Month"),
            new System.Collections.Generic.KeyValuePair<string, string>("NoOfWorkingDaysInMonth", "Working Days"),
            new System.Collections.Generic.KeyValuePair<string, string>("NoOfPaidHolidays", "Paid Holidays"),
            new System.Collections.Generic.KeyValuePair<string, string>("InformedLeave", "Informed Leave"),
            new System.Collections.Generic.KeyValuePair<string, string>("LeaveDaysInYear", "Leave Days In Year"),
            new System.Collections.Generic.KeyValuePair<string, string>("CurrentMonthLeaveDays", "Month Leave Days"),
            new System.Collections.Generic.KeyValuePair<string, string>("LOPLeaveDays", "LOP Leave Days"),
            new System.Collections.Generic.KeyValuePair<string, string>("UninformedLeave", "Uninformed Leave"),
            new System.Collections.Generic.KeyValuePair<string, string>("HRMSHalfDayCount", "Half Day Count"),
            new System.Collections.Generic.KeyValuePair<string, string>("HRMSHalfDayDeduction", "Half Day Deduction"),
            new System.Collections.Generic.KeyValuePair<string, string>("HRMSFullDayDeduction", "Full Day Deduction"),
            new System.Collections.Generic.KeyValuePair<string, string>("LateLoginCount", "Late Login Count"),
            new System.Collections.Generic.KeyValuePair<string, string>("OutTimeNullCount", "OutTime Missing Count"),
            new System.Collections.Generic.KeyValuePair<string, string>("TotalDeductionDays", "Total Deduction"),
            new System.Collections.Generic.KeyValuePair<string, string>("MonthlySalary", "Monthly Salary"),
            new System.Collections.Generic.KeyValuePair<string, string>("PerDaySalary", "Per Day Salary"),
            new System.Collections.Generic.KeyValuePair<string, string>("LeaveDaysSalary", "Leave Days Salary"),
            new System.Collections.Generic.KeyValuePair<string, string>("TotalEligibleDays", "Eligible Days"),
            new System.Collections.Generic.KeyValuePair<string, string>("EligibleSalaryAmount", "Eligible Amount"),
            new System.Collections.Generic.KeyValuePair<string, string>("NetPay", "Net Pay"),
            new System.Collections.Generic.KeyValuePair<string, string>("AnnualCTC", "Annual CTC"),
            new System.Collections.Generic.KeyValuePair<string, string>("InTimeAvailableInHRMS", "InTime Days"),
            new System.Collections.Generic.KeyValuePair<string, string>("FinalNetPay", "Final Net Pay")
        };

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("<html><head><meta charset=\"utf-8\" /></head><body>");
        sb.Append("<table border='1'>");

        sb.Append("<tr>");
        foreach (var col in exportColumns)
            sb.Append("<th>" + System.Web.HttpUtility.HtmlEncode(col.Value) + "</th>");
        sb.Append("</tr>");

        foreach (DataRow row in dt_export.Rows)
        {
            sb.Append("<tr>");
            foreach (var col in exportColumns)
            {
                string val = row.Table.Columns.Contains(col.Key) && row[col.Key] != DBNull.Value
                    ? row[col.Key].ToString()
                    : "";
                sb.Append("<td>" + System.Web.HttpUtility.HtmlEncode(val) + "</td>");
            }
            sb.Append("</tr>");
        }

        sb.Append("</table></body></html>");

        Response.Clear();
        Response.Buffer = true;
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
        Response.Charset = "";
        Response.Write(sb.ToString());
        Response.End();
    }
    private void LoadGrid()
    {
        if (str_month == "" || str_year == "")
        {
            str_month = ddl_month.SelectedValue;
            str_year = ddl_year.SelectedValue;
        }

        int month = int.Parse(str_month);
        int year = int.Parse(str_year);

        DataTable dt_grid = is_view_mode
            ? GetSavedPayrollDataTable(month, year)
            : GetPayrollDataTable(month, year);
        _dt_grid = dt_grid;

        if (dt_grid.Rows.Count > 0)
        {
            DataSet ds = new DataSet();
            ds.Merge(dt_grid);
            PH.LoadGridItem(ds, PH_payslip, "paysliadvance.txt", "");
            btn_saveall.Visible = !is_view_mode;
            btn_export.Visible = !is_view_mode;
        }
        else
        {
            btn_saveall.Visible = false;
            btn_export.Visible = false;
        }

        this.LoadKpiSummary(dt_grid, month, year);
    }

    /// <summary>
    /// Returns the data exactly as it was SAVED for the given month/year
    /// (used when the page is opened in read-only "view" mode). No live recomputation.
    /// Column names are aliased to match the ones GetPayrollDataTable() produces,
    /// so the same paysliadvance.txt template / KPI summary code can be reused as-is.
    /// </summary>
    private DataTable GetSavedPayrollDataTable(int month, int year)
    {
        string str_query = @"
            SELECT
                EmployeeName, Employeeid,
                NoOfDaysInMonth, NoOfWorkingDays AS NoOfWorkingDaysInMonth, NoOfPaidHolidays,
                InformedLeave, LeaveDaysInYear, CurrentMonthLeaveDays, LOPLeaveDays, UninformedLeave,
                HRMSHalfDayCount, HRMSHalfDayDeduction, HRMSFullDayDeduction,
                LateLoginCount, OutTimeNullCount, TotalDeductionDays,
                MonthlySalary, PerDaySalary, LeaveDaysSalary, TotalEligibleDays, EligibleSalaryAmount,
                NetPay, AnnualCTC, InTimeAvailableInHRMS, FinalNetPay
            FROM IT_EmployeePayrollDetails
            WHERE PayrollMonth = @Month AND PayrollYear = @Year
            ORDER BY EmployeeName";

        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@Month", month);
        cmd.Parameters.AddWithValue("@Year", year);

        DataTable dt = DA.GetDataTable(cmd);
        return dt ?? new DataTable();
    }

    private DataTable GetPayrollDataTable(int month, int year)
    {
        string str_query = @"
;WITH 

-- 1. LEAVE YEAR TOTAL (informed leave only - uninformed leave has its own LOP logic below)
CTE_LeaveYearTotal AS (
    SELECT 
        ld.Employeekey,
        ISNULL(SUM(CAST(ld.LeaveDays AS DECIMAL(10,2))), 0) AS LeaveYearTotal
    FROM IT_EmployeeLeaveDetails ld
    WHERE ld.Responsestatus = 2
      AND ld.Createdby != '1987DF80-F1A7-4EFE-A6BB-AF04AD6AA9BD'
      AND YEAR(ld.Fromdate) = @Year
    GROUP BY ld.Employeekey
),

-- 2. CURRENT MONTH LEAVE TOTAL (informed leave only)
CTE_MonthLeave AS (
    SELECT 
        ld.Employeekey,
        ISNULL(SUM(CAST(ld.LeaveDays AS DECIMAL(10,2))), 0) AS MonthLeaveDays
    FROM IT_EmployeeLeaveDetails ld
    WHERE ld.Responsestatus = 2
      AND ld.Createdby != '1987DF80-F1A7-4EFE-A6BB-AF04AD6AA9BD'
      AND MONTH(ld.Fromdate) = @Month
      AND YEAR(ld.Fromdate) = @Year
    GROUP BY ld.Employeekey
),

-- 3. INFORMED LEAVE
CTE_InformedLeave AS (
    SELECT 
        ld.Employeekey,
        ISNULL(SUM(CAST(ld.LeaveDays AS DECIMAL(10,2))), 0) AS InformedLeaveDays
    FROM IT_EmployeeLeaveDetails ld
    WHERE ld.Responsestatus = 2
      AND ld.Createdby != '1987DF80-F1A7-4EFE-A6BB-AF04AD6AA9BD'
      AND MONTH(ld.Fromdate) = @Month
      AND YEAR(ld.Fromdate) = @Year
    GROUP BY ld.Employeekey
),

-- 4. UNINFORMED LEAVE
CTE_UninformedLeave AS (
    SELECT 
        ld.Employeekey,
        ISNULL(SUM(CAST(ld.LeaveDays AS DECIMAL(10,2))), 0) AS UninformedLeaveDays
    FROM IT_EmployeeLeaveDetails ld
    WHERE ld.Responsestatus = 2
      AND ld.Createdby = '1987DF80-F1A7-4EFE-A6BB-AF04AD6AA9BD'
      AND MONTH(ld.Fromdate) = @Month
      AND YEAR(ld.Fromdate) = @Year
    GROUP BY ld.Employeekey
),

-- 5. LEAVE TILL PREVIOUS MONTH (informed leave only)
CTE_PrevMonthLeave AS (
    SELECT 
        ld.Employeekey,
        ISNULL(SUM(CAST(ld.LeaveDays AS DECIMAL(10,2))), 0) AS PrevMonthLeaveDays
    FROM IT_EmployeeLeaveDetails ld
    WHERE ld.Responsestatus = 2
      AND ld.Createdby != '1987DF80-F1A7-4EFE-A6BB-AF04AD6AA9BD'
      AND YEAR(ld.Fromdate) = @Year
      AND MONTH(ld.Fromdate) < @Month
    GROUP BY ld.Employeekey
),

-- 6. PERMISSION DETAILS
CTE_Permission AS (
    SELECT 
        pd.Employeekey,
        CAST(pd.Requestdate AS DATE) AS PermissionDate,
        ISNULL(DATEDIFF(MINUTE, '00:00:00', pd.Permissionhourse), 0) AS PermissionMinutes
    FROM IT_EmployeePermissionDetails pd
    WHERE pd.Responsestatus = 2
      AND MONTH(pd.Requestdate) = @Month
      AND YEAR(pd.Requestdate) = @Year
),

-- 7. ATTENDANCE PER DAY
CTE_Attendance AS (
    SELECT 
        ws.Employeekey,
        ws.WorkDate,
        ws.InTime,
        ws.OutTime,
        CASE
            WHEN ws.NetWorkingDuration IS NOT NULL
              AND ws.NetWorkingDuration NOT LIKE '%Out Time is null%'
              AND CHARINDEX(' hours', ws.NetWorkingDuration) > 0
              AND CHARINDEX('hours ', ws.NetWorkingDuration) > 0
              AND CHARINDEX(' minutes', ws.NetWorkingDuration) > CHARINDEX('hours ', ws.NetWorkingDuration) + 6
            THEN
                CAST(SUBSTRING(ws.NetWorkingDuration, 1, CHARINDEX(' hours', ws.NetWorkingDuration) - 1) AS INT) * 60
                +
                CAST(SUBSTRING(ws.NetWorkingDuration, CHARINDEX('hours ', ws.NetWorkingDuration) + 6,
                     CHARINDEX(' minutes', ws.NetWorkingDuration) - CHARINDEX('hours ', ws.NetWorkingDuration) - 6) AS INT)
            ELSE 0
        END AS NetWorkingMinutes
    FROM IT_V_EmployeeDailyWorkSummary ws
    WHERE MONTH(ws.WorkDate) = @Month
      AND YEAR(ws.WorkDate) = @Year
),

-- 8. HRMS HALF DAY COUNT
CTE_HalfDay AS (
    SELECT 
        a.Employeekey,
        COUNT(*) AS HRMSHalfDayCount
    FROM CTE_Attendance a
    LEFT JOIN CTE_Permission p 
        ON a.Employeekey = p.Employeekey 
        AND a.WorkDate = p.PermissionDate
    WHERE (a.NetWorkingMinutes + ISNULL(p.PermissionMinutes, 0)) < 480
      AND (a.NetWorkingMinutes + ISNULL(p.PermissionMinutes, 0)) > 240
    GROUP BY a.Employeekey
),

-- 9. HRMS FULL DAY DEDUCTION
CTE_FullDay AS (
    SELECT 
        a.Employeekey,
        COUNT(*) AS HRMSFullDayDeduction
    FROM CTE_Attendance a
    WHERE a.NetWorkingMinutes < 240
      AND a.OutTime IS NOT NULL
      AND NOT EXISTS (
            SELECT 1 
            FROM IT_EmployeeLeaveDetails ld
            WHERE ld.Employeekey = a.Employeekey
              AND CAST(ld.Fromdate AS DATE) = a.WorkDate
              AND ld.Responsestatus = 2
      )
    GROUP BY a.Employeekey
),

-- 10. IN TIME AVAILABLE
CTE_InTime AS (
    SELECT 
        ws.Employeekey,
        COUNT(*) AS InTimeAvailableInHRMS
    FROM IT_V_EmployeeDailyWorkSummary ws
    WHERE ws.InTime IS NOT NULL
      AND MONTH(ws.WorkDate) = @Month
      AND YEAR(ws.WorkDate) = @Year
    GROUP BY ws.Employeekey
),

-- 11. LATE LOGIN COUNT (InTime crosses 09:45 AM).
--     A late day is EXCLUDED from the count only when BOTH are true for that date:
--       a) IT_LatePermissionDetails has an approved (Responsestatus = 2) record, AND
--       b) IT_EmployeePermissionDetails has an approved morning permission (Fromtime before 12:00 PM)
CTE_LateLogin AS (
    SELECT 
        a.Employeekey,
        COUNT(*) AS LateLoginCount
    FROM CTE_Attendance a
    WHERE a.InTime IS NOT NULL
      AND CAST(a.InTime AS TIME) > '09:45:00'
      AND NOT (
            EXISTS (
                SELECT 1 
                FROM IT_LatePermissionDetails lp
                WHERE lp.Employeekey = a.Employeekey
                  AND CAST(lp.Requestdate AS DATE) = a.WorkDate
                  AND lp.Responsestatus = 2
            )
            AND EXISTS (
                SELECT 1 
                FROM IT_EmployeePermissionDetails epd
                WHERE epd.Employeekey = a.Employeekey
                  AND CAST(epd.Requestdate AS DATE) = a.WorkDate
                  AND epd.Responsestatus = 2
                  AND TRY_CAST(epd.Fromtime AS TIME) < '12:00:00'
            )
      )
    GROUP BY a.Employeekey
),

-- 12. DAYS WITH OUT TIME MISSING (NULL) IN DAILY WORK SUMMARY -> Half day deduction each
--     Excludes only days already covered by an approved leave.
--     Note: CTE_FullDay requires OutTime IS NOT NULL, so a day with OutTime = NULL
--     can never be picked up by the Full Day rule -> no double-deduction risk here,
--     so no NetWorkingMinutes filter is needed (that filter previously made this
--     CTE always return 0, since NetWorkingMinutes is 0, never NULL, whenever
--     NetWorkingDuration = 'Out Time is null').
CTE_OutTimeNull AS (
    SELECT 
        a.Employeekey,
        COUNT(*) AS OutTimeNullCount
    FROM CTE_Attendance a
    WHERE a.OutTime IS NULL
      AND NOT EXISTS (
            SELECT 1 
            FROM IT_EmployeeLeaveDetails ld
            WHERE ld.Employeekey = a.Employeekey
              AND CAST(ld.Fromdate AS DATE) = a.WorkDate
              AND ld.Responsestatus = 2
      )
    GROUP BY a.Employeekey
)

SELECT 
    ROW_NUMBER() OVER (ORDER BY er.Employeeid ASC)          AS Sno,
    er.Employeekey                                          AS Employeekey,
    ISNULL(er.Firstname,'') + ' ' + ISNULL(er.Lastname,'') AS EmployeeName,
    er.Employeeid                                           AS Employeeid,
    base.DaysInMonth                                        AS NoOfDaysInMonth,
    base.WorkDays                                           AS NoOfWorkingDaysInMonth,
    base.PaidHolidays                                       AS NoOfPaidHolidays,
    ISNULL(il.InformedLeaveDays, 0)                        AS InformedLeave,
    ISNULL(lyt.LeaveYearTotal, 0)                          AS LeaveDaysInYear,
    ISNULL(ml.MonthLeaveDays, 0)                           AS CurrentMonthLeaveDays,
    CAST(ROUND(calc.LOPDays, 2) AS DECIMAL(10,2))          AS LOPLeaveDays,
    ISNULL(ul.UninformedLeaveDays, 0)                      AS UninformedLeave,
    ISNULL(hd.HRMSHalfDayCount, 0)                        AS HRMSHalfDayCount,
    finalded.FinalHalfDayDed                                AS HRMSHalfDayDeduction,
    finalded.FinalFullDayDed                                AS HRMSFullDayDeduction,
    ISNULL(ll.LateLoginCount, 0)                           AS LateLoginCount,
    ISNULL(ot.OutTimeNullCount, 0)                         AS OutTimeNullCount,
    ded.TotalDed                                            AS TotalDeductionDays,
    base.MonthlySal                                         AS MonthlySalary,
    calc.PerDay                                             AS PerDaySalary,
    CAST(ROUND(calc.PerDay * base.PaidHolidays, 2) AS DECIMAL(10,2))                                AS LeaveDaysSalary,
    elig.EligDays                                           AS TotalEligibleDays,
    CAST(ROUND(calc.PerDay * elig.EligDays, 2) AS DECIMAL(10,2))                                    AS EligibleSalaryAmount,
    CAST(ROUND((calc.PerDay * elig.EligDays) + (calc.PerDay * base.PaidHolidays), 2) AS DECIMAL(10,2)) AS NetPay,
    base.MonthlySal * 12                                    AS AnnualCTC,
    ISNULL(it.InTimeAvailableInHRMS, 0)                    AS InTimeAvailableInHRMS,
    CAST(ROUND(calc.PerDay * elig.EligDays, 2) AS DECIMAL(10,2))                                    AS FinalNetPay,
    sd.EmployeeDOJ                                          AS EmployeeDOJ

FROM (
    SELECT *,
        ROW_NUMBER() OVER (PARTITION BY Employeeid ORDER BY Employeekey) AS rn
    FROM IT_EmployeeRegister
    WHERE Employeestatus = 1
) er
INNER JOIN IT_EmployeeSalaryDetails sd 
    ON sd.Employeekey = er.Employeekey
    AND er.rn = 1
    -- Skip employees who had not yet joined by the end of the payroll month
    AND CONVERT(DATE, sd.EmployeeDOJ, 107) <= EOMONTH(DATEFROMPARTS(@Year, @Month, 1))
INNER JOIN it_employeeworkingdaydetails wd 
    ON CAST(wd.Year AS INT) = @Year 
    AND wd.monthvalue = @Month
LEFT JOIN CTE_LeaveYearTotal lyt 
    ON lyt.Employeekey = er.Employeekey
LEFT JOIN CTE_MonthLeave ml 
    ON ml.Employeekey = er.Employeekey
LEFT JOIN CTE_InformedLeave il 
    ON il.Employeekey = er.Employeekey
LEFT JOIN CTE_UninformedLeave ul 
    ON ul.Employeekey = er.Employeekey
LEFT JOIN CTE_PrevMonthLeave pm
    ON pm.Employeekey = er.Employeekey
LEFT JOIN CTE_HalfDay hd 
    ON hd.Employeekey = er.Employeekey
LEFT JOIN CTE_FullDay fd 
    ON fd.Employeekey = er.Employeekey
LEFT JOIN CTE_InTime it 
    ON it.Employeekey = er.Employeekey
LEFT JOIN CTE_LateLogin ll 
    ON ll.Employeekey = er.Employeekey
LEFT JOIN CTE_OutTimeNull ot 
    ON ot.Employeekey = er.Employeekey

CROSS APPLY (
    SELECT
        CAST(ISNULL(sd.Empoloyeemonthlysalary,'0') AS DECIMAL(10,2))                                AS MonthlySal,
        CAST(wd.Numberofdaysinmonth AS DECIMAL(10,2))                                                AS DaysInMonth,
        CAST(wd.Numberofworkdaysinmonth AS DECIMAL(10,2))                                            AS WorkDays,
        CAST(wd.Numberofdaysinmonth AS DECIMAL(10,2)) - CAST(wd.Numberofworkdaysinmonth AS DECIMAL(10,2)) AS PaidHolidays,
        CASE
            WHEN YEAR(CONVERT(DATE, sd.EmployeeDOJ, 107)) < @Year THEN 12
            ELSE (13 - MONTH(CONVERT(DATE, sd.EmployeeDOJ, 107)))
        END AS LeaveEntitlement
) AS base

CROSS APPLY (
    SELECT
        CASE
            WHEN ISNULL(lyt.LeaveYearTotal,0) > base.LeaveEntitlement
            THEN
                CASE
                    WHEN (base.LeaveEntitlement - ISNULL(pm.PrevMonthLeaveDays,0)) <= 0
                    THEN ISNULL(ml.MonthLeaveDays,0)
                    ELSE
                        CASE
                            WHEN (ISNULL(ml.MonthLeaveDays,0) - (base.LeaveEntitlement - ISNULL(pm.PrevMonthLeaveDays,0))) > 0
                            THEN CAST(ROUND(ISNULL(ml.MonthLeaveDays,0) - (base.LeaveEntitlement - ISNULL(pm.PrevMonthLeaveDays,0)), 2) AS DECIMAL(10,2))
                            ELSE 0
                        END
                END
            ELSE 0
        END                                                                                          AS LOPDays,
        CAST(ROUND(base.MonthlySal / base.DaysInMonth, 2) AS DECIMAL(10,2))                        AS PerDay,
        -- Base half day deduction (partial hours worked that day)
        CAST(ROUND(ISNULL(hd.HRMSHalfDayCount,0) / 2.0, 2) AS DECIMAL(10,2))                      AS BaseHalfDayDed,
        -- Late login: every 6 late logins = 1 full day; leftover 3-5 late logins = 0.5 day
        CAST(FLOOR(ISNULL(ll.LateLoginCount,0) / 6.0) AS DECIMAL(10,2))                           AS LateLoginFullDayPortion,
        CAST(ROUND(FLOOR((ISNULL(ll.LateLoginCount,0) % 6) / 3.0) * 0.5, 2) AS DECIMAL(10,2))     AS LateLoginHalfDayPortion,
        -- OutTime missing: 0.5 day deduction for every day OutTime is NULL, folded into half day
        CAST(ROUND(ISNULL(ot.OutTimeNullCount,0) * 0.5, 2) AS DECIMAL(10,2))                      AS OutTimeNullHalfDayPortion
) AS calc

CROSS APPLY (
    SELECT
        CAST(ROUND(calc.BaseHalfDayDed + calc.LateLoginHalfDayPortion + calc.OutTimeNullHalfDayPortion, 2) AS DECIMAL(10,2)) AS FinalHalfDayDed,
        CAST(ROUND(ISNULL(fd.HRMSFullDayDeduction,0) + calc.LateLoginFullDayPortion, 2) AS DECIMAL(10,2))                    AS FinalFullDayDed
) AS finalded

CROSS APPLY (
    SELECT
        CAST(ROUND(calc.LOPDays + ISNULL(ul.UninformedLeaveDays,0) + finalded.FinalHalfDayDed + finalded.FinalFullDayDed, 2) AS DECIMAL(10,2)) AS TotalDed
) AS ded

CROSS APPLY (
    SELECT
        CASE 
            WHEN base.WorkDays - ded.TotalDed < 0 THEN CAST(0 AS DECIMAL(10,2))
            ELSE CAST(ROUND(base.WorkDays - ded.TotalDed, 2) AS DECIMAL(10,2))
        END AS EligDays
) AS elig

ORDER BY er.Employeeid ASC";

        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@Month", month);
        cmd.Parameters.AddWithValue("@Year", year);

        return DA.GetDataTable(cmd);
    }
    private void LoadKpiSummary(DataTable dt_grid, int month, int year)
    {
        int totalEmployees = dt_grid.Rows.Count;
        decimal totalLOPDays = 0m;
        decimal totalUninformed = 0m;
        decimal totalDedDays = 0m;
        decimal totalDedAmount = 0m;
        decimal grossSalary = 0m;
        decimal lopAmount = 0m;
        decimal netPayOutlay = 0m;

        foreach (DataRow row in dt_grid.Rows)
        {
            decimal lop = ToDecimal(row["LOPLeaveDays"]);
            decimal uninformed = ToDecimal(row["UninformedLeave"]);
            decimal dedDays = ToDecimal(row["TotalDeductionDays"]);
            decimal workDays = ToDecimal(row["NoOfWorkingDaysInMonth"]);
            decimal monthlySal = ToDecimal(row["MonthlySalary"]);
            decimal perDay = ToDecimal(row["PerDaySalary"]);
            decimal netPay = ToDecimal(row["NetPay"]);
            decimal cappedDedDays = dedDays > workDays ? workDays : dedDays;
            decimal dedAmount = cappedDedDays * perDay;

            totalLOPDays += lop;
            totalUninformed += uninformed;
            totalDedDays += dedDays;
            totalDedAmount += dedAmount;
            grossSalary += monthlySal;
            lopAmount += lop * perDay;
            netPayOutlay += netPay;
        }

        lbl_kpi_employees.Text = totalEmployees.ToString();
        lbl_kpi_deddayscount.Text = totalDedDays.ToString("0.##");
        lbl_kpi_lopdays.Text = totalLOPDays.ToString("0.##");
        lbl_kpi_uninformed.Text = totalUninformed.ToString("0.##");
        lbl_total_netpay.Text = netPayOutlay.ToString("N2");

    }
    protected void btn_saveall_Click(object sender, EventArgs e)
    {
        str_month = ddl_month.SelectedValue;
        str_year  = ddl_year.SelectedValue;

        int month = int.Parse(str_month);
        int year  = int.Parse(str_year);

        if (CheckPayrollExists(month, year))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert_exists",
                "showToastr('error','Payroll already generated for this month!');", true);

            this.LoadGrid();
            return;
        }
        this.LoadGrid();

        if (_dt_grid == null || _dt_grid.Rows.Count == 0)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert_nodata",
                "showToastr('error','No payroll data found to save. Please load the grid first.');", true);
            return;
        }

        bool success = SavePayroll(_dt_grid, month, year);

        if (success)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert_success",
                "showToastr('success','Payroll Generated Successfully!');", true);
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert_fail",
                "showToastr('error','An error occurred while saving payroll. Please try again.');", true);
        }
    }
    private bool CheckPayrollExists(int month, int year)
    {
        string query = @"
            SELECT COUNT(1) AS RecordCount
            FROM IT_EmployeePayrollDetails 
            WHERE PayrollMonth = @Month 
              AND PayrollYear  = @Year";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@Month", month);
        cmd.Parameters.AddWithValue("@Year",  year);

        DataTable dt = DA.GetDataTable(cmd);
        if (dt != null && dt.Rows.Count > 0)
            return Convert.ToInt32(dt.Rows[0]["RecordCount"]) > 0;

        return false;
    }

    private bool SavePayroll(DataTable dt, int month, int year)
    {
        // Retrieve the session user key for audit columns
        Guid createdBy = Guid.Empty;
        try { createdBy = new Guid(SC.Userid); } catch { }

        string insertQuery = @"INSERT INTO IT_EmployeePayrollDetails (
PayrollDetailskey, Employeekey, Employeeid, EmployeeName, PayrollMonth, PayrollYear,
NoOfDaysInMonth, NoOfWorkingDays, NoOfPaidHolidays, InformedLeave, LeaveDaysInYear,
CurrentMonthLeaveDays, LOPLeaveDays, UninformedLeave, HRMSHalfDayCount,
HRMSHalfDayDeduction, HRMSFullDayDeduction, LateLoginCount,
OutTimeNullCount, TotalDeductionDays, MonthlySalary,
PerDaySalary, LeaveDaysSalary, TotalEligibleDays, EligibleSalaryAmount, NetPay,
AnnualCTC, InTimeAvailableInHRMS, FinalNetPay, Createdon, Createdby
) VALUES (
NEWID(), @Employeekey, @Employeeid, @EmployeeName, @PayrollMonth, @PayrollYear,
@NoOfDaysInMonth, @NoOfWorkingDays, @NoOfPaidHolidays, @InformedLeave, @LeaveDaysInYear,
@CurrentMonthLeaveDays, @LOPLeaveDays, @UninformedLeave, @HRMSHalfDayCount,
@HRMSHalfDayDeduction, @HRMSFullDayDeduction, @LateLoginCount,
@OutTimeNullCount, @TotalDeductionDays, @MonthlySalary,
@PerDaySalary, @LeaveDaysSalary, @TotalEligibleDays, @EligibleSalaryAmount, @NetPay,
@AnnualCTC, @InTimeAvailableInHRMS, @FinalNetPay, GETDATE(), @Createdby)";
        try
        {
            foreach (DataRow row in dt.Rows)
            {
                SqlCommand cmd = new SqlCommand(insertQuery);
               
                cmd.Parameters.AddWithValue("@Employeekey", row["Employeekey"]);
                cmd.Parameters.AddWithValue("@Employeeid", row["Employeeid"].ToString());
                cmd.Parameters.AddWithValue("@EmployeeName", row["EmployeeName"].ToString());
                cmd.Parameters.AddWithValue("@PayrollMonth", month);
                cmd.Parameters.AddWithValue("@PayrollYear", year);
                cmd.Parameters.AddWithValue("@NoOfDaysInMonth", ToDecimal(row["NoOfDaysInMonth"]));
                cmd.Parameters.AddWithValue("@NoOfWorkingDays", ToDecimal(row["NoOfWorkingDaysInMonth"]));
                cmd.Parameters.AddWithValue("@NoOfPaidHolidays", ToDecimal(row["NoOfPaidHolidays"]));
                cmd.Parameters.AddWithValue("@InformedLeave", ToDecimal(row["InformedLeave"]));
                cmd.Parameters.AddWithValue("@LeaveDaysInYear", ToDecimal(row["LeaveDaysInYear"]));
                cmd.Parameters.AddWithValue("@CurrentMonthLeaveDays", ToDecimal(row["CurrentMonthLeaveDays"]));
                cmd.Parameters.AddWithValue("@LOPLeaveDays", ToDecimal(row["LOPLeaveDays"]));
                cmd.Parameters.AddWithValue("@UninformedLeave", ToDecimal(row["UninformedLeave"]));
                cmd.Parameters.AddWithValue("@HRMSHalfDayCount", ToDecimal(row["HRMSHalfDayCount"]));
                cmd.Parameters.AddWithValue("@HRMSHalfDayDeduction", ToDecimal(row["HRMSHalfDayDeduction"]));
                cmd.Parameters.AddWithValue("@HRMSFullDayDeduction", ToDecimal(row["HRMSFullDayDeduction"]));
                cmd.Parameters.AddWithValue("@LateLoginCount", ToInt(row["LateLoginCount"]));
                cmd.Parameters.AddWithValue("@OutTimeNullCount", ToInt(row["OutTimeNullCount"]));
                cmd.Parameters.AddWithValue("@TotalDeductionDays", ToDecimal(row["TotalDeductionDays"]));
                cmd.Parameters.AddWithValue("@MonthlySalary", ToDecimal(row["MonthlySalary"]));
                cmd.Parameters.AddWithValue("@PerDaySalary", ToDecimal(row["PerDaySalary"]));
                cmd.Parameters.AddWithValue("@LeaveDaysSalary", ToDecimal(row["LeaveDaysSalary"]));
                cmd.Parameters.AddWithValue("@TotalEligibleDays", ToDecimal(row["TotalEligibleDays"]));
                cmd.Parameters.AddWithValue("@EligibleSalaryAmount", ToDecimal(row["EligibleSalaryAmount"]));
                cmd.Parameters.AddWithValue("@NetPay", ToDecimal(row["NetPay"]));
                cmd.Parameters.AddWithValue("@AnnualCTC", ToDecimal(row["AnnualCTC"]));
                cmd.Parameters.AddWithValue("@InTimeAvailableInHRMS", ToInt(row["InTimeAvailableInHRMS"]));
                cmd.Parameters.AddWithValue("@FinalNetPay", ToDecimal(row["FinalNetPay"]));
                cmd.Parameters.AddWithValue("@Createdby", createdBy == Guid.Empty ? (object)DBNull.Value : createdBy);

                DA.ExecuteNonQuery(cmd);
            }

            return true;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("SavePayroll error: " + ex.Message);
            return false;
        }
    }
    private decimal ToDecimal(object val)
    {
        if (val == null || val == DBNull.Value) return 0m;
        decimal d;
        return decimal.TryParse(val.ToString(), out d) ? d : 0m;
    }
    private int ToInt(object val)
    {
        if (val == null || val == DBNull.Value) return 0;
        int i;
        return int.TryParse(val.ToString(), out i) ? i : 0;
    }
}
