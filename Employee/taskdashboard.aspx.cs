using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_taskdashboard : System.Web.UI.Page
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
                control1.Text = "Task Dashboard";

            CheckAndShowEmployeeFilter();
            BindMonthDropdown();
            BindYearDropdown();
        }

        LoadDashboard();
    }

    private void CheckAndShowEmployeeFilter()
    {
        // Show employee filter for all users
        divEmployeeFilter.Visible = true;
        BindEmployeeDropdown();
    }

    private void BindEmployeeDropdown()
    {
        string query = @"SELECT EmployeeKey, (Firstname + ' ' + Lastname) AS EmployeeName 
                        FROM IT_EmployeeRegister 
                        WHERE Employeestatus = 1 AND Destination IN (11, 12, 23, 24)
                        ORDER BY Firstname";
        
        DataTable dt = DA.GetDataTable(query);
        
        ddlEmployee.Items.Clear();
        ddlEmployee.Items.Add(new ListItem("-- Select Employee --", "0"));
        
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                ddlEmployee.Items.Add(new ListItem(dr["EmployeeName"].ToString(), dr["EmployeeKey"].ToString()));
            }
        }
        
        ddlEmployee.SelectedValue = SC.Userid;
    }

    private void BindMonthDropdown()
    {
        ddlMonth.Items.Clear();
        int currentYear = DateTime.Now.Year;
        for (int m = 1; m <= 12; m++)
            ddlMonth.Items.Add(new ListItem(new DateTime(currentYear, m, 1).ToString("MMMM"), m.ToString()));
        ddlMonth.SelectedValue = DateTime.Now.Month.ToString();
    }

    private void BindYearDropdown()
    {
        ddlYear.Items.Clear();
        int currentYear = DateTime.Now.Year;
        for (int y = currentYear - 5; y <= currentYear + 1; y++)
            ddlYear.Items.Add(new ListItem(y.ToString(), y.ToString()));
        ddlYear.SelectedValue = currentYear.ToString();
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e) { }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e) { }
    protected void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e) { }

    private string GetMeetingTooltip(string dateStr, string userId, int month, int year)
    {
        DateTime taskDate;
        if (!DateTime.TryParseExact(dateStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out taskDate))
            return "";

        string query = @"SELECT m.MeetingTitle, DATEDIFF(MINUTE, m.StartTime, m.EndTime) / 60.0 AS Hours
                        FROM IT_MeetingParticipants mp
                        INNER JOIN IT_Meetings m ON m.MeetingKey = mp.MeetingKey
                        WHERE mp.EmployeeKey = @EmployeeKey
                        AND CAST(m.MeetingDate AS DATE) = @TaskDate
                        AND m.Status IN (1, 2)
                        AND ISNULL(m.MeetingType, 0) != 6
                        ORDER BY m.StartTime";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@EmployeeKey", userId);
        cmd.Parameters.AddWithValue("@TaskDate", taskDate);

        DataTable dt = DA.GetDataTable(cmd);
        if (dt == null || dt.Rows.Count == 0)
            return "";

        System.Text.StringBuilder tooltip = new System.Text.StringBuilder();
        foreach (DataRow row in dt.Rows)
        {
            string title = row["MeetingTitle"].ToString();
            decimal hours = Convert.ToDecimal(row["Hours"]);
            tooltip.Append(title + ": " + hours.ToString("0.##") + " hrs\n");
        }

        return tooltip.ToString().TrimEnd('\n').Replace("'", "\\'").Replace("\n", "&#10;");
    }

    private string GetTaskTooltip(string dateStr, string userId)
    {
        DateTime taskDate;
        if (!DateTime.TryParseExact(dateStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out taskDate))
            return "";

        string query = @"SELECT td.TaskDescription, 
                               CAST(ISNULL(td.AssignedHours, 0) AS DECIMAL(10,2)) AS Hours,
                               CAST(ISNULL(td.ActualHours, 0) AS DECIMAL(10,2)) AS ActualHours
                        FROM IT_TaskCreation t
                        INNER JOIN IT_TaskDescriptiondetails td ON td.TaskKey = t.TaskKey
                        WHERE t.EmployeeList = @EmployeeKey
                        AND CAST(t.StartDate AS DATE) = @TaskDate
                        ORDER BY td.TaskDescription";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@EmployeeKey", userId);
        cmd.Parameters.AddWithValue("@TaskDate", taskDate);

        DataTable dt = DA.GetDataTable(cmd);
        if (dt == null || dt.Rows.Count == 0)
            return "";

        System.Text.StringBuilder tooltip = new System.Text.StringBuilder();
        foreach (DataRow row in dt.Rows)
        {
            string desc = row["TaskDescription"].ToString();
            decimal hours = row["Hours"] == DBNull.Value ? 0 : Convert.ToDecimal(row["Hours"]);
            decimal actualHours = row["ActualHours"] == DBNull.Value ? 0 : Convert.ToDecimal(row["ActualHours"]);
            tooltip.Append(desc + ": " + hours.ToString("0.##") + " hrs (Actual: " + actualHours.ToString("0.##") + " hrs)\n");
        }

        return tooltip.ToString().TrimEnd('\n').Replace("'", "\\'").Replace("\n", "&#10;");
    }

    private string GetTestingTooltip(string dateStr, string userId)
    {
        DateTime taskDate;
        if (!DateTime.TryParseExact(dateStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out taskDate))
            return "";

        string query = @"SELECT tt.testdescription, CAST(ISNULL(tt.ActualHours, tt.AssignedHours) AS DECIMAL(10,2)) AS Hours
                        FROM IT_TaskTesting tt
                        WHERE tt.assignedto = @EmployeeKey
                        AND CAST(tt.StartDate AS DATE) = @TaskDate
                        ORDER BY tt.testdescription";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@EmployeeKey", userId);
        cmd.Parameters.AddWithValue("@TaskDate", taskDate);

        DataTable dt = DA.GetDataTable(cmd);
        if (dt == null || dt.Rows.Count == 0)
            return "";

        System.Text.StringBuilder tooltip = new System.Text.StringBuilder();
        foreach (DataRow row in dt.Rows)
        {
            string desc = row["testdescription"].ToString();
            decimal hours = row["Hours"] == DBNull.Value ? 0 : Convert.ToDecimal(row["Hours"]);
            tooltip.Append(desc + ": " + hours.ToString("0.##") + " hrs\n");
        }

        return tooltip.ToString().TrimEnd('\n').Replace("'", "\\'").Replace("\n", "&#10;");
    }

    private string GetBusinessMeetingTooltip(string dateStr, string userId)
    {
        DateTime taskDate;
        if (!DateTime.TryParseExact(dateStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out taskDate))
            return "";

        string query = @"SELECT m.MeetingTitle, DATEDIFF(MINUTE, m.StartTime, m.EndTime) / 60.0 AS Hours
                        FROM IT_MeetingParticipants mp
                        INNER JOIN IT_Meetings m ON m.MeetingKey = mp.MeetingKey
                        WHERE mp.EmployeeKey = @EmployeeKey
                        AND CAST(m.MeetingDate AS DATE) = @TaskDate
                        AND m.Status IN (1, 2)
                        AND m.MeetingType = 6
                        ORDER BY m.StartTime";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@EmployeeKey", userId);
        cmd.Parameters.AddWithValue("@TaskDate", taskDate);

        DataTable dt = DA.GetDataTable(cmd);
        if (dt == null || dt.Rows.Count == 0)
            return "";

        System.Text.StringBuilder tooltip = new System.Text.StringBuilder();
        foreach (DataRow row in dt.Rows)
        {
            string title = row["MeetingTitle"].ToString();
            decimal hours = Convert.ToDecimal(row["Hours"]);
            tooltip.Append(title + ": " + hours.ToString("0.##") + " hrs\n");
        }

        return tooltip.ToString().TrimEnd('\n').Replace("'", "\\'").Replace("\n", "&#10;");
    }

    private void LoadDashboard()
    {
        string str_userid = SC.Userid;
        
        // If employee is selected in dropdown, use that employee's data
        if (ddlEmployee.SelectedValue != "0")
        {
            str_userid = ddlEmployee.SelectedValue;
        }
        
        // Fetch gender from IT_EmployeeRegister
        string genderQuery = "SELECT Gender FROM IT_EmployeeRegister WHERE EmployeeKey = @EmployeeKey";
        SqlCommand genderCmd = new SqlCommand(genderQuery);
        genderCmd.Parameters.AddWithValue("@EmployeeKey", str_userid);
        DataTable genderDt = DA.GetDataTable(genderCmd);
        if (genderDt != null && genderDt.Rows.Count > 0)
        {
            hf_gender.Value = genderDt.Rows[0]["Gender"].ToString();
        }
        else
        {
            hf_gender.Value = "0";
        }
        
        int selectedMonth = int.Parse(ddlMonth.SelectedValue);
        int selectedYear  = int.Parse(ddlYear.SelectedValue);

        string query = @"
       ;WITH MonthDays AS
(
    SELECT CAST(DATEADD(DAY, number, DATEFROMPARTS(@Year, @Month, 1)) AS DATE) AS TaskDate,
           DATENAME(WEEKDAY, DATEADD(DAY, number, DATEFROMPARTS(@Year, @Month, 1))) AS DayName
    FROM master..spt_values
    WHERE type = 'P'
      AND DATEADD(DAY, number, DATEFROMPARTS(@Year, @Month, 1)) < DATEADD(MONTH, 1, DATEFROMPARTS(@Year, @Month, 1))
),
HolidayDates AS
(
    SELECT CAST(Holidays AS DATE) AS HolidayDate,
           Description AS HolidayName
    FROM IT_Holidays
    WHERE MONTH(Holidays) = @Month
      AND YEAR(Holidays) = @Year
),
FilteredDays AS
(
    SELECT d.TaskDate, d.DayName, h.HolidayName
    FROM MonthDays d
    LEFT JOIN HolidayDates h ON d.TaskDate = h.HolidayDate
    -- Remove the WHERE clause that was filtering out second Saturdays
),
MeetingHours AS
(
    SELECT
        mp.EmployeeKey,
        CAST(m.MeetingDate AS DATE) AS TaskDate,
        SUM(DATEDIFF(MINUTE, m.StartTime, m.EndTime)) / 60.0 AS MeetingHours
    FROM IT_MeetingParticipants mp
    INNER JOIN IT_Meetings m ON m.MeetingKey = mp.MeetingKey
    WHERE mp.EmployeeKey = @EmployeeKey
      AND MONTH(m.MeetingDate) = @Month
      AND YEAR(m.MeetingDate) = @Year
      AND m.Status IN (1, 2)
      AND ISNULL(m.MeetingType, 0) != 6
    GROUP BY mp.EmployeeKey, CAST(m.MeetingDate AS DATE)
),
OutsideMeetingHours AS
(
    SELECT
        mp.EmployeeKey,
        CAST(m.MeetingDate AS DATE) AS TaskDate,
        SUM(DATEDIFF(MINUTE, m.StartTime, m.EndTime)) / 60.0 AS OutsideHours
    FROM IT_MeetingParticipants mp
    INNER JOIN IT_Meetings m ON m.MeetingKey = mp.MeetingKey
    WHERE mp.EmployeeKey = @EmployeeKey
      AND MONTH(m.MeetingDate) = @Month
      AND YEAR(m.MeetingDate) = @Year
      AND m.Status IN (1, 2)
      AND m.MeetingType = 6
    GROUP BY mp.EmployeeKey, CAST(m.MeetingDate AS DATE)
),
TaskHours AS
(
    SELECT
        CAST(t.StartDate AS DATE) AS TaskDate,
        SUM(CAST(ISNULL(td.AssignedHours, 0) AS DECIMAL(10,2))) AS TaskHours,
        SUM(CAST(ISNULL(td.ActualHours, 0) AS DECIMAL(10,2))) AS ActualTaskHours
    FROM IT_TaskCreation t
    INNER JOIN IT_TaskDescriptiondetails td ON td.TaskKey = t.TaskKey
    WHERE t.EmployeeList = @EmployeeKey
      AND MONTH(t.StartDate) = @Month
      AND YEAR(t.StartDate) = @Year
    GROUP BY CAST(t.StartDate AS DATE)
),
TestingHours AS
(
    SELECT
        CAST(tt.StartDate AS DATE) AS TaskDate,
        SUM(CAST(ISNULL(tt.ActualHours, tt.AssignedHours) AS DECIMAL(10,2))) AS TestingHours
    FROM IT_TaskTesting tt
    WHERE tt.assignedto = @EmployeeKey
      AND MONTH(tt.StartDate) = @Month
      AND YEAR(tt.StartDate) = @Year
    GROUP BY CAST(tt.StartDate AS DATE)
),
LeaveData AS
(
    SELECT
        d.TaskDate,
        CAST(1 AS BIT) AS IsLeave,
        l.LeaveType
    FROM FilteredDays d
    INNER JOIN IT_EmployeeLeaveDetails l
        ON l.Employeekey = @EmployeeKey
       AND d.TaskDate BETWEEN CAST(l.Fromdate AS DATE) AND CAST(l.Todate AS DATE)
    WHERE ISNULL(l.Responsestatus, 0) = 2
    GROUP BY d.TaskDate, l.LeaveType
)
SELECT
    CONVERT(VARCHAR(10), d.TaskDate, 103) AS TaskDate,
    LEFT(d.DayName, 3) AS DayName,
    ISNULL(d.HolidayName, '') AS HolidayName,
    CAST(ISNULL(m.MeetingHours, 0) AS DECIMAL(10,2)) AS MeetingsHours,
    CAST(ISNULL(t.TaskHours, 0) AS DECIMAL(10,2)) AS TaskHours,
    CAST(ISNULL(t.ActualTaskHours, 0) AS DECIMAL(10,2)) AS ActualTaskHours,
    CAST(ISNULL(th.TestingHours, 0) AS DECIMAL(10,2)) AS TestingHours,
    CAST(ISNULL(om.OutsideHours, 0) AS DECIMAL(10,2)) AS OutsideMeetingHours,
    CAST(ISNULL(l.IsLeave, 0) AS BIT) AS IsLeave,
    ISNULL(CAST(l.LeaveType AS VARCHAR(10)), '') AS LeaveType
FROM FilteredDays d
LEFT JOIN MeetingHours m ON d.TaskDate = m.TaskDate
LEFT JOIN OutsideMeetingHours om ON d.TaskDate = om.TaskDate
LEFT JOIN TaskHours t ON d.TaskDate = t.TaskDate
LEFT JOIN TestingHours th ON d.TaskDate = th.TaskDate
LEFT JOIN LeaveData l ON d.TaskDate = l.TaskDate
ORDER BY
    CASE
        WHEN (@Month >= MONTH(GETDATE()) AND @Year >= YEAR(GETDATE()))
        THEN CONVERT(DATE, d.TaskDate)
    END ASC,
    CASE
        WHEN (@Month < MONTH(GETDATE()) OR (@Month = MONTH(GETDATE()) AND @Year < YEAR(GETDATE())))
        THEN CONVERT(DATE, d.TaskDate)
    END DESC;";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@EmployeeKey", str_userid);
        cmd.Parameters.AddWithValue("@Month", selectedMonth);
        cmd.Parameters.AddWithValue("@Year", selectedYear);

        DataTable dt = DA.GetDataTable(cmd);

        PH_Dashboard.Controls.Clear();

        decimal totalMeetings = 0, totalTask = 0, totalTesting = 0, totalOutside = 0;

        if (dt != null && dt.Rows.Count > 0)
        {
            dt.Columns.Add("TotalHours",    typeof(decimal));
            dt.Columns.Add("BorderClass",   typeof(string));
            dt.Columns.Add("StatusHtml",    typeof(string));
            dt.Columns.Add("NormalContent", typeof(string));
            dt.Columns.Add("DateWithDay",   typeof(string));
            dt.Columns.Add("MeetingTooltip", typeof(string));
            dt.Columns.Add("TaskTooltip", typeof(string));
            dt.Columns.Add("TestingTooltip", typeof(string));
            dt.Columns.Add("BusinessMeetingTooltip", typeof(string));

            foreach (DataRow dr in dt.Rows)
            {
                string dayName = dr["DayName"].ToString().ToUpper();
                string taskDate = dr["TaskDate"].ToString();
                string holidayName = dr["HolidayName"].ToString();
                bool isHoliday = !string.IsNullOrEmpty(holidayName);
                dr["DateWithDay"] = taskDate + "<span style='float:right;font-size:10px;'>" + dayName + "</span>";

                bool isLeave = dr["IsLeave"] != DBNull.Value && Convert.ToBoolean(dr["IsLeave"]);
                bool isSunday = dayName == "SUN";
                
                // Parse date more safely for second Saturday check
                DateTime currentDate;
                bool isSecondSaturday = false;
                if (DateTime.TryParseExact(dr["TaskDate"].ToString(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out currentDate))
                {
                    isSecondSaturday = dayName == "SAT" && currentDate.Day >= 8 && currentDate.Day <= 14;
                }

                decimal meetings = dr["MeetingsHours"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["MeetingsHours"]);
                decimal task     = dr["TaskHours"]     == DBNull.Value ? 0 : Convert.ToDecimal(dr["TaskHours"]);
                decimal actualTask = dr["ActualTaskHours"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["ActualTaskHours"]);
                decimal testing  = dr["TestingHours"]  == DBNull.Value ? 0 : Convert.ToDecimal(dr["TestingHours"]);
                decimal outside  = dr["OutsideMeetingHours"] == DBNull.Value ? 0 : Convert.ToDecimal(dr["OutsideMeetingHours"]);

                string meetingTooltip = meetings > 0 ? GetMeetingTooltip(taskDate, str_userid, selectedMonth, selectedYear) : "";
                dr["MeetingTooltip"] = meetingTooltip;

                string taskTooltip = task > 0 ? GetTaskTooltip(taskDate, str_userid) : "";
                dr["TaskTooltip"] = taskTooltip;

                string testingTooltip = testing > 0 ? GetTestingTooltip(taskDate, str_userid) : "";
                dr["TestingTooltip"] = testingTooltip;

                string businessMeetingTooltip = outside > 0 ? GetBusinessMeetingTooltip(taskDate, str_userid) : "";
                dr["BusinessMeetingTooltip"] = businessMeetingTooltip;

                bool hasData = meetings > 0 || task > 0 || testing > 0 || outside > 0;

                if (isHoliday)
                {
                    dr["TotalHours"]    = 0;
                    dr["BorderClass"]   = "leave-card";
                    dr["StatusHtml"]    = "";
                    dr["NormalContent"] = "<div class='leave-center'>GOVERNMENT HOLIDAY<br/>" + holidayName + "</div>";
                }
                else if (isSunday && !hasData)
                {
                    dr["TotalHours"]    = 0;
                    dr["BorderClass"]   = "leave-card";
                    dr["StatusHtml"]    = "";
                    dr["NormalContent"] = "<div class='leave-center'>SUNDAY</div>";
                }
                else if (isSecondSaturday && !hasData)
                {
                    dr["TotalHours"]    = 0;
                    dr["BorderClass"]   = "leave-card";
                    dr["StatusHtml"]    = "";
                    dr["NormalContent"] = "<div class='leave-center'>SECOND SATURDAY</div>";
                }
                else if (isSecondSaturday && hasData)
                {
                    decimal total = meetings + task + testing + outside;
                    int pct = (int)Math.Min((total / 8) * 100, 100);

                    totalMeetings += meetings;
                    totalTask     += task;
                    totalTesting  += testing;
                    totalOutside  += outside;

                    dr["TotalHours"]    = total;
                    dr["BorderClass"]   = "leave-card";
                    dr["StatusHtml"]    = "";
                    string taskTooltipAttr = !string.IsNullOrEmpty(dr["TaskTooltip"].ToString()) ? dr["TaskTooltip"].ToString() : "";
                    string testingTooltipAttr = !string.IsNullOrEmpty(dr["TestingTooltip"].ToString()) ? dr["TestingTooltip"].ToString() : "";
                    string businessMeetingTooltipAttr = !string.IsNullOrEmpty(dr["BusinessMeetingTooltip"].ToString()) ? dr["BusinessMeetingTooltip"].ToString() : "";
                    dr["NormalContent"] = 
                        "<div style='text-align:center;font-weight:700;color:#d97706;margin-bottom:8px;'>SECOND SATURDAY</div>" +
                        "<div class='dash-row'><span class='meeting-label' title='" + meetingTooltip + "'>&#129309; Meetings</span><span class='val'>" + meetings.ToString("0.##") + " hrs</span></div>" +
                        "<div class='dash-row'><span class='meeting-label' title='" + taskTooltipAttr + "'>&#128203; Task</span><span class='val'>" + task.ToString("0.##") + " hrs</span></div>" +
                        (actualTask > 0 ? "<div class='dash-row' style='padding-left:20px;font-size:11px;color:#666;'><span>&#9201; Actual</span><span class='val'>" + actualTask.ToString("0.##") + " hrs</span></div>" : "") +
                        "<div class='dash-row'><span class='meeting-label' title='" + testingTooltipAttr + "'>&#129514; Testing</span><span class='val'>" + testing.ToString("0.##") + " hrs</span></div>" +
                        "<div class='dash-row'><span class='meeting-label' title='" + businessMeetingTooltipAttr + "'>&#128188; Business</span><span class='val'>" + outside.ToString("0.##") + " hrs</span></div>" +
                        "<div class='progress-wrap'><div class='progress-bar' style='--bar-width:" + pct + "%;'></div></div>" +
                        "<div class='dash-total'><span>Total</span><span>" + total.ToString("0.##") + " hrs</span></div>";
                }
                else if (isLeave)
                {
                    string leaveType  = dr["LeaveType"].ToString();
                    bool   isHalfDay  = leaveType == "0" || leaveType == "1";
                    string leaveLabel = leaveType == "0" ? "HALF DAY LEAVE\n(Forenoon)"
                                      : leaveType == "1" ? "HALF DAY LEAVE\n(Afternoon)"
                                      : "FULL DAY LEAVE";

                    dr["BorderClass"] = isHalfDay ? "halfday-card" : "leave-card";

                    if (isHalfDay)
                    {
                        decimal total    = meetings + task + testing + outside;
                        int pct = (int)Math.Min((total / 4) * 100, 100);

                        totalMeetings += meetings;
                        totalTask     += task;
                        totalTesting  += testing;
                        totalOutside  += outside;

                        dr["TotalHours"]    = total;
                        dr["StatusHtml"]    = "";
                        string taskTooltipAttr = !string.IsNullOrEmpty(dr["TaskTooltip"].ToString()) ? dr["TaskTooltip"].ToString() : "";
                        string testingTooltipAttr = !string.IsNullOrEmpty(dr["TestingTooltip"].ToString()) ? dr["TestingTooltip"].ToString() : "";
                        string businessMeetingTooltipAttr = !string.IsNullOrEmpty(dr["BusinessMeetingTooltip"].ToString()) ? dr["BusinessMeetingTooltip"].ToString() : "";
                        dr["NormalContent"] =
                            "<div class='dash-row'><span class='meeting-label' title='" + meetingTooltip + "'>&#129309; Meetings</span><span class='val'>" + meetings.ToString("0.##") + " hrs</span></div>" +
                            "<div class='dash-row'><span class='meeting-label' title='" + taskTooltipAttr + "'>&#128203; Task</span><span class='val'>"     + task.ToString("0.##")     + " hrs</span></div>" +
                            (actualTask > 0 ? "<div class='dash-row' style='padding-left:20px;font-size:11px;color:#666;'><span>&#9201; Actual</span><span class='val'>" + actualTask.ToString("0.##") + " hrs</span></div>" : "") +
                            "<div class='dash-row'><span class='meeting-label' title='" + testingTooltipAttr + "'>&#129514; Testing</span><span class='val'>"  + testing.ToString("0.##")  + " hrs</span></div>" +
                            "<div class='dash-row'><span class='meeting-label' title='" + businessMeetingTooltipAttr + "'>&#128188; Business</span><span class='val'>"  + outside.ToString("0.##")  + " hrs</span></div>" +
                            "<div class='progress-wrap'><div class='progress-bar' style='--bar-width:" + pct + "%;'></div></div>" +
                            "<div class='dash-total'><span>Total</span><span>" + total.ToString("0.##") + " / 4 hrs</span></div>";
                    }
                    else
                    {
                        dr["TotalHours"]    = 0;
                        dr["StatusHtml"]    = "<div class='leave-center'>" + leaveLabel.Replace("\n", "<br/>") + "</div>";
                        dr["NormalContent"] = "";
                    }
                }
                else
                {
                    decimal total    = meetings + task + testing + outside;
                    int pct = (int)Math.Min((total / 8) * 100, 100);

                    totalMeetings += meetings;
                    totalTask     += task;
                    totalTesting  += testing;
                    totalOutside  += outside;

                    dr["TotalHours"]    = total;
                    dr["BorderClass"]   = total >= 8 ? "green-card" : "red-card";
                    dr["StatusHtml"]    = "";
                    string taskTooltipAttr = !string.IsNullOrEmpty(dr["TaskTooltip"].ToString()) ? dr["TaskTooltip"].ToString() : "";
                    string testingTooltipAttr = !string.IsNullOrEmpty(dr["TestingTooltip"].ToString()) ? dr["TestingTooltip"].ToString() : "";
                    string businessMeetingTooltipAttr = !string.IsNullOrEmpty(dr["BusinessMeetingTooltip"].ToString()) ? dr["BusinessMeetingTooltip"].ToString() : "";
                    dr["NormalContent"] =
                        "<div class='dash-row'><span class='meeting-label' title='" + meetingTooltip + "'>&#129309; Meetings</span><span class='val'>" + meetings.ToString("0.##") + " hrs</span></div>" +
                        "<div class='dash-row'><span class='meeting-label' title='" + taskTooltipAttr + "'>&#128203; Task</span><span class='val'>"     + task.ToString("0.##")     + " hrs</span></div>" +
                        (actualTask > 0 ? "<div class='dash-row' style='padding-left:20px;font-size:11px;color:#666;'><span>&#9201; Actual</span><span class='val'>" + actualTask.ToString("0.##") + " hrs</span></div>" : "") +
                        "<div class='dash-row'><span class='meeting-label' title='" + testingTooltipAttr + "'>&#129514; Testing</span><span class='val'>"  + testing.ToString("0.##")  + " hrs</span></div>" +
                        "<div class='dash-row'><span class='meeting-label' title='" + businessMeetingTooltipAttr + "'>&#128188; Business</span><span class='val'>"  + outside.ToString("0.##")  + " hrs</span></div>" +
                        "<div class='progress-wrap'><div class='progress-bar' style='--bar-width:" + pct + "%;'></div></div>" +
                        "<div class='dash-total'><span>Total</span><span>" + total.ToString("0.##") + " / 8 hrs</span></div>";
                }
            }

            hf_meetings.Value = totalMeetings.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
            hf_task.Value     = totalTask.ToString("0.##",     System.Globalization.CultureInfo.InvariantCulture);
            hf_testing.Value  = totalTesting.ToString("0.##",  System.Globalization.CultureInfo.InvariantCulture);
            hf_outside.Value  = totalOutside.ToString("0.##",  System.Globalization.CultureInfo.InvariantCulture);

            string templatePath = Server.MapPath("~/DivTemplate/TaskDashboard.txt");
            string template     = System.IO.File.ReadAllText(templatePath);

            var rows      = dt.Rows.Cast<DataRow>().ToList();
            int totalRows = (int)Math.Ceiling(rows.Count / 6.0);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (int rowIndex = 0; rowIndex < totalRows; rowIndex++)
            {
                sb.Append("<div class='dash-row-group'>");
                for (int col = 0; col < 6; col++)
                {
                    int idx = rowIndex * 6 + col;
                    if (idx < rows.Count)
                    {
                        DataRow dr    = rows[idx];
                        string  delay = (col * 0.08).ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
                        string  card  = template
                            .Replace("%%BorderClass%%",   dr["BorderClass"].ToString())
                            .Replace("%%TaskDate%%",      dr["DateWithDay"].ToString())
                            .Replace("%%StatusHtml%%",    dr["StatusHtml"].ToString())
                            .Replace("%%NormalContent%%", dr["NormalContent"].ToString())
                            .Replace("%%AnimDelay%%",     delay);
                        sb.Append(card);
                    }
                }
                sb.Append("</div>");
            }

            PH_Dashboard.Controls.Add(new LiteralControl(sb.ToString()));
        }
        else
        {
            hf_meetings.Value = "0";
            hf_task.Value     = "0";
            hf_testing.Value  = "0";
            hf_outside.Value  = "0";
            PH_Dashboard.Controls.Add(new LiteralControl("<div class='no-data'>No records found.</div>"));
        }
    }
}
