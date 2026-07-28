using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_EmployeeMonthlyReport : System.Web.UI.Page
{
    DataAccess DA;
    
    protected void Page_Load(object sender, EventArgs e)
    {
        DA = new DataAccess();
        if (!IsPostBack)
        {
            LoadEmployees();
            ddlMonth.SelectedValue = "0";
            ddlYear.SelectedValue = DateTime.Now.Year.ToString();
            LoadAllData();
        }
    }

    private void LoadEmployees()
    {
        string sql = "SELECT Employeekey, firstname + ' ' + lastname + ' (' + employeeid + ')' AS EmpName FROM IT_EmployeeRegister WHERE Employeestatus=1 ORDER BY firstname";
        DataTable dt = DA.GetDataTable(sql);
        ddlEmployee.DataSource = dt;
        ddlEmployee.DataTextField = "EmpName";
        ddlEmployee.DataValueField = "Employeekey";
        ddlEmployee.DataBind();
        ddlEmployee.Items.Insert(0, new ListItem("-- All Employees --", ""));
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        LoadAllData();
    }

    private void LoadAllData()
    {
        divReportContent.Visible = true;
        
        if (!string.IsNullOrEmpty(ddlEmployee.SelectedValue))
        {
            divEmpInfo.Visible = true;
            divKpiCards.Visible = true;
            gvPerformanceReport.Visible = false;
            pnlCharts.Visible = true;
            lblGridSectionTitle.Text = "Employee Task Performance Analytics";
            
            LoadEmployeeInfo();
            LoadKpiCards();
            LoadEmployeeChartsData();
        }
        else
        {
            divEmpInfo.Visible = false;
            divKpiCards.Visible = false;
            gvPerformanceReport.Visible = true;
            pnlCharts.Visible = false;
            lblGridSectionTitle.Text = "Monthly Performance Overview (All Employees)";
            
            LoadPerformanceReportGrid();
        }
    }

    private void LoadEmployeeChartsData()
    {
        string empKey = ddlEmployee.SelectedValue;
        int month = Convert.ToInt32(ddlMonth.SelectedValue);
        int year = Convert.ToInt32(ddlYear.SelectedValue);

        // 1. Task Status Count (Completed vs Pending)
        string sqlStatus = @"
            SELECT 
                SUM(CASE WHEN td.Status = 4 THEN 1 ELSE 0 END) AS Completed,
                SUM(CASE WHEN td.Status <> 4 OR td.Status IS NULL THEN 1 ELSE 0 END) AS Pending
            FROM IT_TaskCreation tc
            LEFT JOIN IT_TaskDescriptiondetails td ON td.TaskKey = tc.TaskKey
            WHERE tc.EmployeeList = @EmpKey AND (@Month = 0 OR MONTH(tc.StartDate) = @Month) AND YEAR(tc.StartDate) = @Year";
            
        SqlCommand cmdStatus = new SqlCommand(sqlStatus);
        cmdStatus.Parameters.AddWithValue("@EmpKey", empKey);
        cmdStatus.Parameters.AddWithValue("@Month", month);
        cmdStatus.Parameters.AddWithValue("@Year", year);
        DataTable dtStatus = DA.GetDataTable(cmdStatus);
        
        int completed = 0, pending = 0;
        if (dtStatus.Rows.Count > 0)
        {
            completed = dtStatus.Rows[0]["Completed"] != DBNull.Value ? Convert.ToInt32(dtStatus.Rows[0]["Completed"]) : 0;
            pending = dtStatus.Rows[0]["Pending"] != DBNull.Value ? Convert.ToInt32(dtStatus.Rows[0]["Pending"]) : 0;
        }
        hfTaskStatusValues.Value = completed + "," + pending;

        // 2. Project Task Count
        string sqlProjects = @"
            SELECT 
                ISNULL((SELECT ProjectName FROM IT_Projects WHERE ProjectKey = tc.ProjectName), 'No Project') AS ProjectName,
                COUNT(td.TaskDetailID) AS TaskCount
            FROM IT_TaskCreation tc
            LEFT JOIN IT_TaskDescriptiondetails td ON td.TaskKey = tc.TaskKey
            WHERE tc.EmployeeList = @EmpKey AND (@Month = 0 OR MONTH(tc.StartDate) = @Month) AND YEAR(tc.StartDate) = @Year
            GROUP BY tc.ProjectName";
            
        SqlCommand cmdProjects = new SqlCommand(sqlProjects);
        cmdProjects.Parameters.AddWithValue("@EmpKey", empKey);
        cmdProjects.Parameters.AddWithValue("@Month", month);
        cmdProjects.Parameters.AddWithValue("@Year", year);
        DataTable dtProjects = DA.GetDataTable(cmdProjects);
        
        List<string> projects = new List<string>();
        List<string> counts = new List<string>();
        foreach (DataRow row in dtProjects.Rows)
        {
            projects.Add(row["ProjectName"].ToString().Replace(",", "")); // Avoid comma splitting issue
            counts.Add(row["TaskCount"].ToString());
        }
        hfProjectLabels.Value = string.Join(",", projects);
        hfProjectValues.Value = string.Join(",", counts);
    }

    private void LoadEmployeeInfo()
    {
        string empKey = ddlEmployee.SelectedValue;
        string sql = @"
            SELECT firstname + ' ' + lastname AS EmpName, employeeid, 
                   Image, 
                   (SELECT Departmentname FROM IT_Department WHERE Departmentid = a.Department) AS DeptName,
                   (SELECT RoleName FROM IT_Roles WHERE RoleId = a.Role) AS DesigName
            FROM IT_EmployeeRegister a WHERE Employeekey=@EmpKey";
            
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.AddWithValue("@EmpKey", empKey);
        DataTable dt = DA.GetDataTable(cmd);
        
        if(dt.Rows.Count > 0)
        {
            lblName.Text = dt.Rows[0]["EmpName"].ToString();
            lblEmpId.Text = dt.Rows[0]["employeeid"].ToString();
            lblDept.Text = dt.Rows[0]["DeptName"].ToString();
            lblDesignation.Text = dt.Rows[0]["DesigName"].ToString();
            lblReportMonth.Text = ddlMonth.SelectedItem.Text + " " + ddlYear.SelectedItem.Text;
            
            string img = dt.Rows[0]["Image"].ToString();
            imgProfile.ImageUrl = string.IsNullOrEmpty(img) ? "~/Images/nopicture.jpg" : "~/Images/Adminprofilepictures/" + img;
        }
    }

    private void LoadPerformanceReportGrid()
    {
        string empKey = ddlEmployee.SelectedValue;
        int month = Convert.ToInt32(ddlMonth.SelectedValue);
        int year = Convert.ToInt32(ddlYear.SelectedValue);

        // Calculate standard working days from official config table
        int workingDays = 0;
        string sqlWorkingDays = "SELECT ISNULL(SUM(CAST(Numberofworkdaysinmonth AS INT)), 0) FROM IT_EmployeeWorkingDayDetails WHERE [Year] = @Year AND (@Month = 0 OR monthvalue = @Month)";
        SqlCommand cmdWD = new SqlCommand(sqlWorkingDays);
        cmdWD.Parameters.AddWithValue("@Year", year);
        cmdWD.Parameters.AddWithValue("@Month", month);
        DataTable dtWD = DA.GetDataTable(cmdWD);
        if (dtWD.Rows.Count > 0 && dtWD.Rows[0][0] != DBNull.Value)
        {
            workingDays = Convert.ToInt32(dtWD.Rows[0][0]);
        }

        if (workingDays == 0)
        {
            int startMonth = month == 0 ? 1 : month;
            int endMonth = month == 0 ? 12 : month;
            for (int m = startMonth; m <= endMonth; m++)
            {
                int daysInMonth = DateTime.DaysInMonth(year, m);
                for (int i = 1; i <= daysInMonth; i++)
                {
                    DateTime dateVal = new DateTime(year, m, i);
                    if (dateVal.DayOfWeek != DayOfWeek.Saturday && dateVal.DayOfWeek != DayOfWeek.Sunday)
                        workingDays++;
                }
            }
        }

        string sql = @"
            WITH EmployeeBase AS (
                SELECT Employeekey, firstname + ' ' + lastname AS EmployeeName 
                FROM IT_EmployeeRegister 
                WHERE Employeestatus = 1
            ),
            TaskData AS (
                SELECT 
                    tc.EmployeeList AS Employeekey,
                    COUNT(td.TaskDetailID) AS TotalTasks,
                    SUM(CASE WHEN td.Status = 4 THEN 1 ELSE 0 END) AS CompletedTasks
                FROM IT_TaskCreation tc
                LEFT JOIN IT_TaskDescriptiondetails td ON td.TaskKey = tc.TaskKey
                WHERE (@Month = 0 OR MONTH(tc.StartDate) = @Month) AND YEAR(tc.StartDate) = @Year
                GROUP BY tc.EmployeeList
            ),
            LeaveData AS (
                SELECT 
                    Employeekey,
                    SUM(CAST(ISNULL(LeaveDays, 0) AS DECIMAL(5,2))) AS TotalLeavesTaken
                FROM IT_EmployeeLeaveDetails
                WHERE Responsestatus = 2
                  AND (@Month = 0 OR MONTH(FromDate) = @Month) AND YEAR(FromDate) = @Year
                GROUP BY Employeekey
            ),
            AttendanceData AS (
                SELECT 
                    Employeekey,
                    COUNT(DISTINCT WorkDate) AS TotalWorkingDays
                FROM IT_V_EmployeeDailyWorkSummary
                WHERE (@Month = 0 OR MONTH(WorkDate) = @Month) AND YEAR(WorkDate) = @Year
                GROUP BY Employeekey
            ),
            PermissionData AS (
                SELECT 
                    Employeekey,
                    SUM(DATEDIFF(MINUTE, Fromtime, Totime)) / 60.0 AS TotalPermissionHours
                FROM IT_EmployeePermissionDetails
                WHERE Responsestatus = 2
                  AND (@Month = 0 OR MONTH(Requestdate) = @Month) AND YEAR(Requestdate) = @Year
                GROUP BY Employeekey
            ),
            LateLoginData AS (
                SELECT 
                    ws.Employeekey,
                    COUNT(DISTINCT ws.WorkDate) AS LateLoginDays
                FROM IT_V_EmployeeDailyWorkSummary ws
                WHERE (@Month = 0 OR MONTH(ws.WorkDate) = @Month) AND YEAR(ws.WorkDate) = @Year
                  AND ws.InTime IS NOT NULL
                  AND CAST(DATEADD(minute, 330, ws.InTime) AS TIME) > '09:45:00'
                  AND NOT (
                        EXISTS (
                            SELECT 1 
                            FROM IT_EmployeePermissionDetails epd
                            WHERE epd.Employeekey = ws.Employeekey
                              AND CAST(epd.Requestdate AS DATE) = ws.WorkDate
                              AND epd.Responsestatus = 2
                              AND TRY_CAST(epd.Fromtime AS TIME) < '12:00:00'
                        )
                        OR
                        EXISTS (
                            SELECT 1 
                            FROM IT_EmployeeLeaveDetails ld
                            WHERE ld.Employeekey = ws.Employeekey
                              AND ws.WorkDate BETWEEN CAST(ld.Fromdate AS DATE) AND CAST(ld.Todate AS DATE)
                              AND ld.Responsestatus = 2
                              AND CAST(ld.LeaveType AS VARCHAR) = '0'
                        )
                  )
                GROUP BY ws.Employeekey
            )
            SELECT 
                eb.EmployeeName,
                @WorkingDays AS WorkingDays,
                ISNULL(a.TotalWorkingDays, 0) AS PresentDays,
                ISNULL(l.TotalLeavesTaken, 0) AS LeavesTaken,
                ISNULL(p.TotalPermissionHours, 0) AS PermissionHours,
                ISNULL(ll.LateLoginDays, 0) AS LateLogins,
                ISNULL(t.TotalTasks, 0) AS TotalTasks,
                ISNULL(t.CompletedTasks, 0) AS CompletedTasks
            FROM EmployeeBase eb
            LEFT JOIN TaskData t ON eb.Employeekey = t.Employeekey
            LEFT JOIN LeaveData l ON eb.Employeekey = l.Employeekey
            LEFT JOIN AttendanceData a ON eb.Employeekey = a.Employeekey
            LEFT JOIN PermissionData p ON eb.Employeekey = p.Employeekey
            LEFT JOIN LateLoginData ll ON eb.Employeekey = ll.Employeekey
            WHERE 1=1 " +
            (!string.IsNullOrEmpty(empKey) ? "AND eb.Employeekey = @EmpKey " : "") +
            "ORDER BY eb.EmployeeName";

        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.AddWithValue("@Month", month);
        cmd.Parameters.AddWithValue("@Year", year);
        cmd.Parameters.AddWithValue("@WorkingDays", workingDays);
        if (!string.IsNullOrEmpty(empKey))
            cmd.Parameters.AddWithValue("@EmpKey", empKey);

        DataTable dt = DA.GetDataTable(cmd);
        
        // Add calculated columns
        dt.Columns.Add("TaskCompletionRate", typeof(decimal));
        dt.Columns.Add("OverallScore", typeof(decimal));

        foreach (DataRow row in dt.Rows)
        {
            int totalT = Convert.ToInt32(row["TotalTasks"]);
            int completedT = Convert.ToInt32(row["CompletedTasks"]);
            decimal presentD = Convert.ToDecimal(row["PresentDays"]);
            int lateL = Convert.ToInt32(row["LateLogins"]);

            // Apply Late Login Penalty based on policy
            decimal penaltyDays = 0;
            if (lateL >= 6)
                penaltyDays = 1.0m;
            else if (lateL >= 3)
                penaltyDays = 0.5m;

            decimal adjustedPresent = presentD - penaltyDays;
            if (adjustedPresent < 0) adjustedPresent = 0;

            decimal taskRate = totalT > 0 ? ((decimal)completedT / totalT) * 100 : 0;
            row["TaskCompletionRate"] = taskRate;

            decimal attendanceRate = workingDays > 0 ? (adjustedPresent / workingDays) * 100 : 100;
            if (attendanceRate > 100) attendanceRate = 100;

            decimal finalScore = (taskRate * 0.7m) + (attendanceRate * 0.3m);
            row["OverallScore"] = finalScore;
        }

        gvPerformanceReport.DataSource = dt;
        gvPerformanceReport.DataBind();
    }

    protected void gvPerformanceReport_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblScore = (Label)e.Row.FindControl("lblScore");
            if (lblScore != null)
            {
                string scoreText = lblScore.Text.Replace("%", "");
                decimal score;
                if (decimal.TryParse(scoreText, out score))
                {
                    if (score >= 80)
                        lblScore.CssClass += " badge-excellent";
                    else if (score >= 50)
                        lblScore.CssClass += " badge-good";
                    else
                        lblScore.CssClass += " badge-poor";
                }
            }
        }
    }

    private void LoadKpiCards()
    {
        string empKey = ddlEmployee.SelectedValue;
        int month = Convert.ToInt32(ddlMonth.SelectedValue);
        int year = Convert.ToInt32(ddlYear.SelectedValue);

        // 1. Working Days
        int workingDays = 0;
        string sqlWorkingDays = "SELECT ISNULL(SUM(CAST(Numberofworkdaysinmonth AS INT)), 0) FROM IT_EmployeeWorkingDayDetails WHERE [Year] = @Year AND (@Month = 0 OR monthvalue = @Month)";
        SqlCommand cmdWD = new SqlCommand(sqlWorkingDays);
        cmdWD.Parameters.AddWithValue("@Year", year);
        cmdWD.Parameters.AddWithValue("@Month", month);
        DataTable dtWD = DA.GetDataTable(cmdWD);
        if (dtWD.Rows.Count > 0 && dtWD.Rows[0][0] != DBNull.Value)
        {
            workingDays = Convert.ToInt32(dtWD.Rows[0][0]);
        }

        if (workingDays == 0)
        {
            int startMonth = month == 0 ? 1 : month;
            int endMonth = month == 0 ? 12 : month;
            for (int m = startMonth; m <= endMonth; m++)
            {
                int daysInMonth = DateTime.DaysInMonth(year, m);
                for (int i = 1; i <= daysInMonth; i++)
                {
                    DateTime dateVal = new DateTime(year, m, i);
                    if (dateVal.DayOfWeek != DayOfWeek.Saturday && dateVal.DayOfWeek != DayOfWeek.Sunday)
                        workingDays++;
                }
            }
        }
        lblWorkingDaysKpi.Text = workingDays.ToString();

        // 2. Present Days
        string sqlPresent = "SELECT COUNT(DISTINCT WorkDate) FROM IT_V_EmployeeDailyWorkSummary WHERE Employeekey=@EmpKey AND (@Month = 0 OR MONTH(WorkDate)=@Month) AND YEAR(WorkDate)=@Year";
        SqlCommand cmdPresent = new SqlCommand(sqlPresent);
        cmdPresent.Parameters.AddWithValue("@EmpKey", empKey);
        cmdPresent.Parameters.AddWithValue("@Month", month);
        cmdPresent.Parameters.AddWithValue("@Year", year);
        DataTable dtPresent = DA.GetDataTable(cmdPresent);
        int presentDays = (dtPresent.Rows.Count > 0 && dtPresent.Rows[0][0] != DBNull.Value) ? Convert.ToInt32(dtPresent.Rows[0][0]) : 0;
        lblPresentDaysKpi.Text = presentDays.ToString();

        // 3. Tasks Completed
        string sqlTasks = @"
            SELECT 
                COUNT(td.TaskDetailID) AS Total,
                SUM(CASE WHEN td.Status = 4 THEN 1 ELSE 0 END) AS Completed
            FROM IT_TaskCreation tc
            LEFT JOIN IT_TaskDescriptiondetails td ON td.TaskKey = tc.TaskKey
            WHERE tc.EmployeeList = @EmpKey AND (@Month = 0 OR MONTH(tc.StartDate)=@Month) AND YEAR(tc.StartDate)=@Year";
        SqlCommand cmdTasks = new SqlCommand(sqlTasks);
        cmdTasks.Parameters.AddWithValue("@EmpKey", empKey);
        cmdTasks.Parameters.AddWithValue("@Month", month);
        cmdTasks.Parameters.AddWithValue("@Year", year);
        DataTable dtTasks = DA.GetDataTable(cmdTasks);
        int totalTasks = 0, completedTasks = 0;
        if (dtTasks.Rows.Count > 0)
        {
            totalTasks = dtTasks.Rows[0]["Total"] != DBNull.Value ? Convert.ToInt32(dtTasks.Rows[0]["Total"]) : 0;
            completedTasks = dtTasks.Rows[0]["Completed"] != DBNull.Value ? Convert.ToInt32(dtTasks.Rows[0]["Completed"]) : 0;
        }
        lblCompletedTasksKpi.Text = completedTasks + "/" + totalTasks;

        // 4. Leaves Taken
        string sqlLeaves = "SELECT SUM(CAST(ISNULL(LeaveDays, 0) AS DECIMAL(5,2))) FROM IT_EmployeeLeaveDetails WHERE Employeekey=@EmpKey AND (@Month = 0 OR MONTH(FromDate)=@Month) AND YEAR(FromDate)=@Year AND Responsestatus=2";
        SqlCommand cmdLeaves = new SqlCommand(sqlLeaves);
        cmdLeaves.Parameters.AddWithValue("@EmpKey", empKey);
        cmdLeaves.Parameters.AddWithValue("@Month", month);
        cmdLeaves.Parameters.AddWithValue("@Year", year);
        DataTable dtLeaves = DA.GetDataTable(cmdLeaves);
        decimal leaves = (dtLeaves.Rows.Count > 0 && dtLeaves.Rows[0][0] != DBNull.Value) ? Convert.ToDecimal(dtLeaves.Rows[0][0]) : 0;
        lblLeavesKpi.Text = leaves.ToString("0.##");

        // 5. Permission Taken
        string sqlPermission = "SELECT SUM(DATEDIFF(MINUTE, Fromtime, Totime)) / 60.0 FROM IT_EmployeePermissionDetails WHERE Employeekey=@EmpKey AND (@Month = 0 OR MONTH(Requestdate)=@Month) AND YEAR(Requestdate)=@Year AND Responsestatus=2";
        SqlCommand cmdPermission = new SqlCommand(sqlPermission);
        cmdPermission.Parameters.AddWithValue("@EmpKey", empKey);
        cmdPermission.Parameters.AddWithValue("@Month", month);
        cmdPermission.Parameters.AddWithValue("@Year", year);
        DataTable dtPermission = DA.GetDataTable(cmdPermission);
        decimal permissionHours = (dtPermission.Rows.Count > 0 && dtPermission.Rows[0][0] != DBNull.Value) ? Convert.ToDecimal(dtPermission.Rows[0][0]) : 0;
        lblPermissionKpi.Text = permissionHours.ToString("0.##") + " hrs";

        // 6. Late Logins
        string sqlLate = @"
            SELECT COUNT(DISTINCT ws.WorkDate) 
            FROM IT_V_EmployeeDailyWorkSummary ws 
            WHERE ws.Employeekey=@EmpKey 
              AND (@Month = 0 OR MONTH(ws.WorkDate)=@Month) 
              AND YEAR(ws.WorkDate)=@Year 
              AND ws.InTime IS NOT NULL
              AND CAST(DATEADD(minute, 330, ws.InTime) AS TIME) > '09:45:00'
              AND NOT (
                    EXISTS (
                        SELECT 1 
                        FROM IT_EmployeePermissionDetails epd
                        WHERE epd.Employeekey = ws.Employeekey
                          AND CAST(epd.Requestdate AS DATE) = ws.WorkDate
                          AND epd.Responsestatus = 2
                          AND TRY_CAST(epd.Fromtime AS TIME) < '12:00:00'
                    )
                    OR
                    EXISTS (
                        SELECT 1 
                        FROM IT_EmployeeLeaveDetails ld
                        WHERE ld.Employeekey = ws.Employeekey
                          AND ws.WorkDate BETWEEN CAST(ld.Fromdate AS DATE) AND CAST(ld.Todate AS DATE)
                          AND ld.Responsestatus = 2
                          AND CAST(ld.LeaveType AS VARCHAR) = '0'
                    )
              )";
        SqlCommand cmdLate = new SqlCommand(sqlLate);
        cmdLate.Parameters.AddWithValue("@EmpKey", empKey);
        cmdLate.Parameters.AddWithValue("@Month", month);
        cmdLate.Parameters.AddWithValue("@Year", year);
        DataTable dtLate = DA.GetDataTable(cmdLate);
        int lateLogins = (dtLate.Rows.Count > 0 && dtLate.Rows[0][0] != DBNull.Value) ? Convert.ToInt32(dtLate.Rows[0][0]) : 0;
        lblLateKpi.Text = lateLogins.ToString();

        // 7. Performance Score
        decimal penaltyDays = 0;
        if (lateLogins >= 6)
            penaltyDays = 1.0m;
        else if (lateLogins >= 3)
            penaltyDays = 0.5m;

        decimal adjustedPresent = (decimal)presentDays - penaltyDays;
        if (adjustedPresent < 0) adjustedPresent = 0;

        decimal taskRate = totalTasks > 0 ? ((decimal)completedTasks / totalTasks) * 100 : 0;
        decimal attendanceRate = workingDays > 0 ? (adjustedPresent / workingDays) * 100 : 100;
        if (attendanceRate > 100) attendanceRate = 100;
        decimal finalScore = (taskRate * 0.7m) + (attendanceRate * 0.3m);
        lblPerformanceScoreKpi.Text = finalScore.ToString("0.##") + "%";
    }
}
