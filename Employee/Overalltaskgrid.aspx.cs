using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_Overalltaskgrid : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();

        if (!IsPostBack)
        {
            if (Request.QueryString["action"] == "delete" && !string.IsNullOrEmpty(Request.QueryString["taskkey"]))
            {
                DeleteTask(Request.QueryString["taskkey"]);
                return;
            }

            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Overall Task ";

            hfActiveStatus.Value = "0";

            CheckAndShowEmployeeFilter();
            BindMonthDropdown();
            BindYearDropdown();
            
            LoadDashboard();
        }
    }

    private void CheckAndShowEmployeeFilter()
    {
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
        ddlMonth.Items.Add(new ListItem("All", "0"));
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

    protected void Filter_Changed(object sender, EventArgs e)
    {
        LoadDashboard();
    }

    protected void CardClick(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        hfActiveStatus.Value = btn.CommandArgument;
        LoadDashboard();
    }

    public string GetStatusTitle()
    {
        int status = int.Parse(hfActiveStatus.Value);
        switch (status)
        {
            case 0: return "All Tasks";
            case 1: return "Yet to Start";
            case 2: return "In Progress";
            case 3: return "Overdue";
            case 4: return "Completed";
            case 5: return "Pending";
            default: return "All Tasks";
        }
    }

    private void DeleteTask(string taskKey)
    {
        try
        {
            string userid = this.SC.Userid;
            int taskKeyInt = Convert.ToInt32(taskKey);
            
            SqlCommand cmdCheck = new SqlCommand("SELECT CreatedBy FROM IT_TaskCreation WHERE TaskKey = @TaskKey");
            cmdCheck.Parameters.AddWithValue("@TaskKey", taskKeyInt);
            DataTable dtCheck = DA.GetDataTable(cmdCheck);
            
            if (dtCheck != null && dtCheck.Rows.Count > 0)
            {
                string createdBy = dtCheck.Rows[0]["CreatedBy"].ToString();
                if (createdBy == userid)
                {
                    SqlCommand cmdDelete = new SqlCommand("DELETE FROM IT_TaskCreation WHERE TaskKey = @TaskKey");
                    cmdDelete.Parameters.AddWithValue("@TaskKey", taskKeyInt);
                    DA.ExecuteNonQuery(cmdDelete);
                    Response.Redirect("Overalltaskgrid.aspx?msg=deleted");
                }
                else
                {
                    Response.Redirect("Overalltaskgrid.aspx?msg=unauthorized");
                }
            }
        }
        catch
        {
            Response.Redirect("Overalltaskgrid.aspx?msg=error");
        }
    }

    private void LoadDashboard()
    {
        LoadChartData();

        LoadAllTasksCount(lbl_AllTaskCount);
        LoadCount(lbl_YetToStartCount, 1);
        LoadCount(lbl_InProgressCount, 2);
        LoadOverdueCount(lbl_OverDueCount);
        LoadCompletedCount(lbl_CompletedCount);
        
        lbl_SchMeeting.Text = GetMeetingCount("1").ToString();
        lbl_CmpMeeting.Text = GetMeetingCount("2").ToString();
        
        int status = int.Parse(hfActiveStatus.Value);
        switch (status)
        {
            case 0: BindAllTasksGrid(); break;
            case 1: BindGrid(1); break;
            case 2: BindGrid(2); break;
            case 3: BindOverdueGrid(); break;
            case 4: BindCompletedGrid(); break;
            case 5: BindGrid(3); break;
            default: BindAllTasksGrid(); break;
        }

        ScriptManager.RegisterStartupScript(this, this.GetType(), "reinitDT", "initDataTable(); renderChart();", true);
    }

    protected string GetOverdueClass()
    {
        int count;
        if (int.TryParse(lbl_OverDueCount.Text, out count) && count > 0)
        {
            return "has-value";
        }
        return "";
    }

    private void LoadChartData()
    {
        string str_userid = SC.Userid;
        if (ddlEmployee.SelectedValue != "0")
        {
            str_userid = ddlEmployee.SelectedValue;
        }
        
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
        
        string monthCond = selectedMonth > 0 ? " AND MONTH(m.MeetingDate) = @Month" : "";
        string taskMonthCond = selectedMonth > 0 ? " AND MONTH(t.StartDate) = @Month" : "";

        string query = @"
        SELECT 
            (SELECT ISNULL(SUM(DATEDIFF(MINUTE, m.StartTime, m.EndTime)) / 60.0, 0)
             FROM IT_MeetingParticipants mp
             INNER JOIN IT_Meetings m ON m.MeetingKey = mp.MeetingKey
             WHERE mp.EmployeeKey = @EmployeeKey AND YEAR(m.MeetingDate) = @Year" + monthCond + @") AS MeetingsHours,
               
            (SELECT ISNULL(SUM(CAST(ISNULL(td.AssignedHours, 0) AS DECIMAL(10,2))), 0)
             FROM IT_TaskCreation t
             INNER JOIN IT_TaskDescriptiondetails td ON td.TaskKey = t.TaskKey
             WHERE t.EmployeeList = @EmployeeKey AND YEAR(t.StartDate) = @Year" + taskMonthCond + @"
               AND ISNULL(t.Role, 0) != 3) AS TaskHours,
             
            (SELECT ISNULL(SUM(CAST(ISNULL(td.AssignedHours, 0) AS DECIMAL(10,2))), 0)
             FROM IT_TaskCreation t
             INNER JOIN IT_TaskDescriptiondetails td ON td.TaskKey = t.TaskKey
             WHERE t.EmployeeList = @EmployeeKey AND YEAR(t.StartDate) = @Year" + taskMonthCond + @"
               AND t.Role = 3) AS TestingHours";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@EmployeeKey", str_userid);
        if (selectedMonth > 0)
            cmd.Parameters.AddWithValue("@Month", selectedMonth);
        cmd.Parameters.AddWithValue("@Year", selectedYear);

        DataTable dt = DA.GetDataTable(cmd);
        if (dt != null && dt.Rows.Count > 0)
        {
            decimal mHours = Convert.ToDecimal(dt.Rows[0]["MeetingsHours"]);
            decimal tHours = Convert.ToDecimal(dt.Rows[0]["TaskHours"]);
            decimal testHours = Convert.ToDecimal(dt.Rows[0]["TestingHours"]);

            hf_meetings.Value = mHours.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
            hf_task.Value     = tHours.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
            hf_testing.Value  = testHours.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);

            bool hasData = (mHours + tHours + testHours) > 0;
            chartSectionContainer.Visible = hasData;
            noDataState.Visible = !hasData;
        }
        else
        {
            hf_meetings.Value = "0";
            hf_task.Value     = "0";
            hf_testing.Value  = "0";

            chartSectionContainer.Visible = false;
            noDataState.Visible = true;
        }
    }

    private string BuildAssigneeCell(string name, string imagePath = "")
    {
        if (string.IsNullOrEmpty(name) || name == "-")
            return "<span class='text-muted'>-</span>";

        string initials = "";
        string[] parts = name.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string p in parts)
        {
            if (initials.Length >= 2) break;
            initials += char.ToUpper(p[0]);
        }
        if (initials.Length == 0) initials = "?";

        string fullPath = string.IsNullOrEmpty(imagePath)
            ? "../Images/nopicture.jpg"
            : "../Images/Adminprofilepictures/" + imagePath;

        string avatarInner =
            "<img src='" + fullPath + "' alt='" + initials + "' " +
            "onerror=\"this.onerror=null; this.src='../Images/nopicture.jpg';\" />";

        return "<span class='assignee-cell'>" +
                    "<span class='assignee-avatar'>" + avatarInner + "</span>" +
                    "<span>" + name + "</span>" +
               "</span>";
    }

    private string BuildBadge(int value, string variant)
    {
        return "<span class='count-badge badge-" + variant + "'>" + value + "</span>";
    }

    private string GetCommonWhereClause(bool includeStatusWhere)
    {
        string userid = this.SC.Userid;
        string filterUserId = ddlEmployee.SelectedValue != "0" ? ddlEmployee.SelectedValue : userid;
        bool showAllEmployees = (ddlEmployee.SelectedValue == "0");
        int month = int.Parse(ddlMonth.SelectedValue);
        int year = int.Parse(ddlYear.SelectedValue);

        string where = includeStatusWhere ? "" : " WHERE 1=1";
        if (!showAllEmployees)
            where += " AND a.EmployeeList = @UserId";
        
        if (month > 0) where += " AND MONTH(a.StartDate) = @Month";
        if (year > 0)  where += " AND YEAR(a.StartDate) = @Year";

        return where;
    }

    private void AddCommonParameters(SqlCommand cmd)
    {
        string userid = this.SC.Userid;
        string filterUserId = ddlEmployee.SelectedValue != "0" ? ddlEmployee.SelectedValue : userid;
        bool showAllEmployees = (ddlEmployee.SelectedValue == "0");
        int month = int.Parse(ddlMonth.SelectedValue);
        int year = int.Parse(ddlYear.SelectedValue);

        if (!showAllEmployees)
            cmd.Parameters.AddWithValue("@UserId", filterUserId);
        if (month > 0) 
            cmd.Parameters.AddWithValue("@Month", month);
        if (year > 0)  
            cmd.Parameters.AddWithValue("@Year", year);
            
        cmd.Parameters.AddWithValue("@CurrentDate", DateTime.Now.Date);
    }

    private int GetMeetingCount(string statusCondition)
    {
        string userid = this.SC.Userid;
        string filterUserId = ddlEmployee.SelectedValue != "0" ? ddlEmployee.SelectedValue : userid;
        bool showAllEmployees = (ddlEmployee.SelectedValue == "0");
        int month = int.Parse(ddlMonth.SelectedValue);
        int year = int.Parse(ddlYear.SelectedValue);

        string where = " WHERE 1=1";
        if (!showAllEmployees)
            where += " AND mp.EmployeeKey = @UserId";
        
        if (month > 0) where += " AND MONTH(m.MeetingDate) = @Month";
        if (year > 0)  where += " AND YEAR(m.MeetingDate) = @Year";
        
        if (!string.IsNullOrEmpty(statusCondition))
            where += " AND m.Status IN (" + statusCondition + ")";

        string query = @"SELECT COUNT(DISTINCT m.MeetingKey)
                         FROM IT_Meetings m
                         INNER JOIN IT_MeetingParticipants mp ON m.MeetingKey = mp.MeetingKey" + where;

        SqlCommand cmd = new SqlCommand(query);
        if (!showAllEmployees)
            cmd.Parameters.AddWithValue("@UserId", filterUserId);
        if (month > 0) 
            cmd.Parameters.AddWithValue("@Month", month);
        if (year > 0)  
            cmd.Parameters.AddWithValue("@Year", year);

        DataTable dt = DA.GetDataTable(cmd);
        return (dt != null && dt.Rows.Count > 0) ? Convert.ToInt32(dt.Rows[0][0]) : 0;
    }

    private void LoadAllTasksCount(Label lblCard)
    {
        string where = GetCommonWhereClause(false);
        string query = @"SELECT COUNT(d.TaskDetailID) 
                        FROM IT_TaskCreation a
                        LEFT JOIN IT_TaskDescriptiondetails d ON a.TaskKey = d.TaskKey" + where;
        SqlCommand cmd = new SqlCommand(query);
        AddCommonParameters(cmd);
        DataTable dtCount = DA.GetDataTable(cmd);
        int taskCount = (dtCount != null && dtCount.Rows.Count > 0) ? Convert.ToInt32(dtCount.Rows[0][0]) : 0;
        lblCard.Text = taskCount.ToString();
    }

    private void LoadCount(Label lblCard, int statusId)
    {
        string where = " WHERE d.Status = @StatusId" + GetCommonWhereClause(true);
        string query = @"SELECT COUNT(a.TaskKey) 
                        FROM IT_TaskCreation a
                        LEFT JOIN IT_TaskDescriptiondetails d ON a.TaskKey = d.TaskKey" + where;
        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@StatusId", statusId);
        AddCommonParameters(cmd);
        DataTable dtCount = DA.GetDataTable(cmd);
        int taskCount = (dtCount != null && dtCount.Rows.Count > 0) ? Convert.ToInt32(dtCount.Rows[0][0]) : 0;
        lblCard.Text = taskCount.ToString();
    }

    private void LoadOverdueCount(Label lblCard)
    {
        string where = " WHERE a.StartDate < DATEADD(day, -7, @CurrentDate) AND d.Status != 4" + GetCommonWhereClause(true);
        string query = @"SELECT COUNT(a.TaskKey) 
                        FROM IT_TaskCreation a
                        LEFT JOIN IT_TaskDescriptiondetails d ON a.TaskKey = d.TaskKey" + where;
        SqlCommand cmd = new SqlCommand(query);
        AddCommonParameters(cmd);
        DataTable dtCount = DA.GetDataTable(cmd);
        lblCard.Text = (dtCount != null && dtCount.Rows.Count > 0) ? dtCount.Rows[0][0].ToString() : "0";
    }

    private void LoadCompletedCount(Label lblCard)
    {
        string baseWhere = GetCommonWhereClause(false);
        string query = @"SELECT COUNT(DISTINCT a.TaskKey)
                        FROM IT_TaskCreation a"
                        + baseWhere + @"
                        AND NOT EXISTS (
                            SELECT 1 FROM IT_TaskDescriptiondetails d 
                            WHERE d.TaskKey = a.TaskKey AND d.Status != 4
                        )
                        AND EXISTS (
                            SELECT 1 FROM IT_TaskDescriptiondetails d 
                            WHERE d.TaskKey = a.TaskKey
                        )";
        SqlCommand cmd = new SqlCommand(query);
        AddCommonParameters(cmd);
        DataTable dtCount = DA.GetDataTable(cmd);
        lblCard.Text = (dtCount != null && dtCount.Rows.Count > 0) ? dtCount.Rows[0][0].ToString() : "0";
    }

    private void BindAllTasksGrid()
    {
        string where = GetCommonWhereClause(false);
        string query = @"SELECT CAST(a.StartDate AS DATE) AS StartDate, a.EmployeeList AS EmployeeKey,
                        (e.Firstname + ' ' + e.Lastname) AS AssignedTo, e.Image AS EmpImage,
                        (SUM(CAST(ISNULL(d.AssignedHours, 0) AS DECIMAL(10,2))) + 
                         ISNULL((SELECT SUM(DATEDIFF(MINUTE, m.StartTime, m.EndTime)) / 60.0 
                                 FROM IT_Meetings m 
                                 JOIN IT_MeetingParticipants mp ON m.MeetingKey = mp.MeetingKey 
                                 WHERE mp.EmployeeKey = a.EmployeeList 
                                   AND CAST(m.MeetingDate AS DATE) = CAST(a.StartDate AS DATE) 
                                   AND m.Status IN (1, 2)), 0)) AS AssignedHours, 
                        SUM(CAST(ISNULL(d.ActualHours, 0) AS DECIMAL(10,2))) AS ActualHours, 
                        COUNT(d.TaskKey) AS SubTaskCount,
                        ISNULL((SELECT COUNT(DISTINCT m.MeetingKey) 
                                FROM IT_Meetings m 
                                JOIN IT_MeetingParticipants mp ON m.MeetingKey = mp.MeetingKey 
                                WHERE mp.EmployeeKey = a.EmployeeList 
                                  AND CAST(m.MeetingDate AS DATE) = CAST(a.StartDate AS DATE) 
                                  AND m.Status IN (1, 2)), 0) AS MeetingCount,
                        SUM(CASE WHEN d.Status = 1 THEN 1 ELSE 0 END) AS YetToStartCount,
                        SUM(CASE WHEN d.Status = 2 THEN 1 ELSE 0 END) AS InProgressCount,
                        SUM(CASE WHEN d.Status = 4 THEN 1 ELSE 0 END) AS CompletedCount,
                        SUM(CASE WHEN d.Status != 4 AND a.StartDate < DATEADD(day, -7, @CurrentDate) THEN 1 ELSE 0 END) AS OverdueCount
                        FROM (
                            SELECT CAST(StartDate AS DATE) AS StartDate, EmployeeList
                            FROM IT_TaskCreation
                            UNION
                            SELECT CAST(m.MeetingDate AS DATE) AS StartDate, mp.EmployeeKey AS EmployeeList
                            FROM IT_Meetings m
                            JOIN IT_MeetingParticipants mp ON m.MeetingKey = mp.MeetingKey
                        ) a
                        LEFT JOIN IT_TaskCreation tc ON CAST(tc.StartDate AS DATE) = a.StartDate AND tc.EmployeeList = a.EmployeeList
                        LEFT JOIN IT_EmployeeRegister e ON a.EmployeeList = e.EmployeeKey
                        LEFT JOIN IT_TaskDescriptiondetails d ON tc.TaskKey = d.TaskKey" + where + @"
                        GROUP BY CAST(a.StartDate AS DATE), a.EmployeeList, e.Firstname, e.Lastname, e.Image
                        ORDER BY CAST(a.StartDate AS DATE) DESC";

        SqlCommand cmd = new SqlCommand(query);
        AddCommonParameters(cmd);
        DataTable dt = DA.GetDataTable(cmd);
        
        RenderGrid(dt);
    }

    private void BindGrid(int statusId)
    {
        string baseWhere = GetCommonWhereClause(false);
        string query = @"SELECT a.StartDate, a.EmployeeList AS EmployeeKey,
                        (e.Firstname + ' ' + e.Lastname) AS AssignedTo, e.Image AS EmpImage,
                        (SUM(CAST(ISNULL(d.AssignedHours, 0) AS DECIMAL(10,2))) + 
                         ISNULL((SELECT SUM(DATEDIFF(MINUTE, m.StartTime, m.EndTime)) / 60.0 
                                 FROM IT_Meetings m 
                                 JOIN IT_MeetingParticipants mp ON m.MeetingKey = mp.MeetingKey 
                                 WHERE mp.EmployeeKey = a.EmployeeList 
                                   AND CAST(m.MeetingDate AS DATE) = a.StartDate 
                                   AND m.Status IN (1, 2)), 0)) AS AssignedHours, 
                        SUM(CAST(ISNULL(d.ActualHours, 0) AS DECIMAL(10,2))) AS ActualHours, 
                        COUNT(d.TaskKey) AS SubTaskCount,
                        ISNULL((SELECT COUNT(DISTINCT m.MeetingKey) 
                                FROM IT_Meetings m 
                                JOIN IT_MeetingParticipants mp ON m.MeetingKey = mp.MeetingKey 
                                WHERE mp.EmployeeKey = a.EmployeeList 
                                  AND CAST(m.MeetingDate AS DATE) = a.StartDate 
                                  AND m.Status IN (1, 2)), 0) AS MeetingCount,
                        SUM(CASE WHEN d.Status = 1 THEN 1 ELSE 0 END) AS YetToStartCount,
                        SUM(CASE WHEN d.Status = 2 THEN 1 ELSE 0 END) AS InProgressCount,
                        SUM(CASE WHEN d.Status = 4 THEN 1 ELSE 0 END) AS CompletedCount,
                        SUM(CASE WHEN d.Status != 4 AND a.StartDate < DATEADD(day, -7, @CurrentDate) THEN 1 ELSE 0 END) AS OverdueCount
                        FROM (
                            SELECT CAST(StartDate AS DATE) AS StartDate, EmployeeList
                            FROM IT_TaskCreation
                            UNION
                            SELECT CAST(m.MeetingDate AS DATE) AS StartDate, mp.EmployeeKey AS EmployeeList
                            FROM IT_Meetings m
                            JOIN IT_MeetingParticipants mp ON m.MeetingKey = mp.MeetingKey
                        ) a
                        LEFT JOIN IT_TaskCreation tc ON CAST(tc.StartDate AS DATE) = a.StartDate AND tc.EmployeeList = a.EmployeeList
                        LEFT JOIN IT_EmployeeRegister e ON a.EmployeeList = e.EmployeeKey
                        LEFT JOIN IT_TaskDescriptiondetails d ON tc.TaskKey = d.TaskKey"
                        + baseWhere + @"
                        AND EXISTS (
                            SELECT 1 FROM IT_TaskDescriptiondetails d2 
                            WHERE d2.TaskKey = tc.TaskKey AND d2.Status = @StatusId
                        )
                        GROUP BY CAST(a.StartDate AS DATE), a.EmployeeList, e.Firstname, e.Lastname, e.Image
                        ORDER BY CAST(a.StartDate AS DATE) DESC";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@StatusId", statusId);
        AddCommonParameters(cmd);
        DataTable dt = DA.GetDataTable(cmd);
        
        RenderGrid(dt);
    }

    private void BindCompletedGrid()
    {
        string baseWhere = GetCommonWhereClause(false);
        string query = @"SELECT CAST(a.StartDate AS DATE) AS StartDate, a.EmployeeList AS EmployeeKey,
                        (e.Firstname + ' ' + e.Lastname) AS AssignedTo, e.Image AS EmpImage,
                        (SUM(CAST(ISNULL(d.AssignedHours, 0) AS DECIMAL(10,2))) + 
                         ISNULL((SELECT SUM(DATEDIFF(MINUTE, m.StartTime, m.EndTime)) / 60.0 
                                 FROM IT_Meetings m 
                                 JOIN IT_MeetingParticipants mp ON m.MeetingKey = mp.MeetingKey 
                                 WHERE mp.EmployeeKey = a.EmployeeList 
                                   AND CAST(m.MeetingDate AS DATE) = CAST(a.StartDate AS DATE) 
                                   AND m.Status IN (1, 2)), 0)) AS AssignedHours, 
                        SUM(CAST(ISNULL(d.ActualHours, 0) AS DECIMAL(10,2))) AS ActualHours, 
                        COUNT(d.TaskKey) AS SubTaskCount,
                        ISNULL((SELECT COUNT(DISTINCT m.MeetingKey) 
                                FROM IT_Meetings m 
                                JOIN IT_MeetingParticipants mp ON m.MeetingKey = mp.MeetingKey 
                                WHERE mp.EmployeeKey = a.EmployeeList 
                                  AND CAST(m.MeetingDate AS DATE) = CAST(a.StartDate AS DATE) 
                                  AND m.Status IN (1, 2)), 0) AS MeetingCount,
                        SUM(CASE WHEN d.Status = 1 THEN 1 ELSE 0 END) AS YetToStartCount,
                        SUM(CASE WHEN d.Status = 2 THEN 1 ELSE 0 END) AS InProgressCount,
                        SUM(CASE WHEN d.Status = 4 THEN 1 ELSE 0 END) AS CompletedCount,
                        SUM(CASE WHEN d.Status != 4 AND a.StartDate < DATEADD(day, -7, @CurrentDate) THEN 1 ELSE 0 END) AS OverdueCount
                        FROM (
                            SELECT CAST(StartDate AS DATE) AS StartDate, EmployeeList
                            FROM IT_TaskCreation
                            UNION
                            SELECT CAST(m.MeetingDate AS DATE) AS StartDate, mp.EmployeeKey AS EmployeeList
                            FROM IT_Meetings m
                            JOIN IT_MeetingParticipants mp ON m.MeetingKey = mp.MeetingKey
                        ) a
                        LEFT JOIN IT_TaskCreation tc ON CAST(tc.StartDate AS DATE) = a.StartDate AND tc.EmployeeList = a.EmployeeList
                        LEFT JOIN IT_EmployeeRegister e ON a.EmployeeList = e.EmployeeKey
                        LEFT JOIN IT_TaskDescriptiondetails d ON tc.TaskKey = d.TaskKey"
                        + baseWhere + @"
                        AND NOT EXISTS (
                            SELECT 1 FROM IT_TaskDescriptiondetails d2 
                            WHERE d2.TaskKey = tc.TaskKey AND d2.Status != 4
                        )
                        AND EXISTS (
                            SELECT 1 FROM IT_TaskDescriptiondetails d3 WHERE d3.TaskKey = tc.TaskKey
                        )
                        GROUP BY CAST(a.StartDate AS DATE), a.EmployeeList, e.Firstname, e.Lastname, e.Image
                        ORDER BY CAST(a.StartDate AS DATE) DESC";

        SqlCommand cmd = new SqlCommand(query);
        AddCommonParameters(cmd);
        DataTable dt = DA.GetDataTable(cmd);
        
        RenderGrid(dt);
    }

    private void BindOverdueGrid()
    {
        string baseWhere = GetCommonWhereClause(false);
        string query = @"SELECT a.StartDate, a.EmployeeList AS EmployeeKey,
                        (e.Firstname + ' ' + e.Lastname) AS AssignedTo, e.Image AS EmpImage,
                        (SUM(CAST(ISNULL(d.AssignedHours, 0) AS DECIMAL(10,2))) + 
                         ISNULL((SELECT SUM(DATEDIFF(MINUTE, m.StartTime, m.EndTime)) / 60.0 
                                 FROM IT_Meetings m 
                                 JOIN IT_MeetingParticipants mp ON m.MeetingKey = mp.MeetingKey 
                                 WHERE mp.EmployeeKey = a.EmployeeList 
                                   AND CAST(m.MeetingDate AS DATE) = a.StartDate 
                                   AND m.Status IN (1, 2)), 0)) AS AssignedHours, 
                        SUM(CAST(ISNULL(d.ActualHours, 0) AS DECIMAL(10,2))) AS ActualHours, 
                        COUNT(d.TaskKey) AS SubTaskCount,
                        ISNULL((SELECT COUNT(DISTINCT m.MeetingKey) 
                                FROM IT_Meetings m 
                                JOIN IT_MeetingParticipants mp ON m.MeetingKey = mp.MeetingKey 
                                WHERE mp.EmployeeKey = a.EmployeeList 
                                  AND CAST(m.MeetingDate AS DATE) = a.StartDate 
                                  AND m.Status IN (1, 2)), 0) AS MeetingCount,
                        SUM(CASE WHEN d.Status = 1 THEN 1 ELSE 0 END) AS YetToStartCount,
                        SUM(CASE WHEN d.Status = 2 THEN 1 ELSE 0 END) AS InProgressCount,
                        0 AS CompletedCount,
                        SUM(CASE WHEN d.Status != 4 AND a.StartDate < DATEADD(day, -7, @CurrentDate) THEN 1 ELSE 0 END) AS OverdueCount
                        FROM (
                            SELECT CAST(StartDate AS DATE) AS StartDate, EmployeeList
                            FROM IT_TaskCreation
                            UNION
                            SELECT CAST(m.MeetingDate AS DATE) AS StartDate, mp.EmployeeKey AS EmployeeList
                            FROM IT_Meetings m
                            JOIN IT_MeetingParticipants mp ON m.MeetingKey = mp.MeetingKey
                        ) a
                        LEFT JOIN IT_TaskCreation tc ON CAST(tc.StartDate AS DATE) = a.StartDate AND tc.EmployeeList = a.EmployeeList
                        LEFT JOIN IT_EmployeeRegister e ON a.EmployeeList = e.EmployeeKey
                        LEFT JOIN IT_TaskDescriptiondetails d ON tc.TaskKey = d.TaskKey"
                        + baseWhere + @"
                        AND a.StartDate < DATEADD(day, -7, @CurrentDate)
                        AND EXISTS (
                            SELECT 1 FROM IT_TaskDescriptiondetails d2 
                            WHERE d2.TaskKey = tc.TaskKey AND d2.Status != 4
                        )
                        GROUP BY CAST(a.StartDate AS DATE), a.EmployeeList, e.Firstname, e.Lastname, e.Image
                        ORDER BY CAST(a.StartDate AS DATE) DESC";

        SqlCommand cmd = new SqlCommand(query);
        AddCommonParameters(cmd);
        DataTable dt = DA.GetDataTable(cmd);
        
        RenderGrid(dt);
    }

    private void RenderGrid(DataTable dt)
    {
        PH_Tasks.Controls.Clear();
        if (dt == null || dt.Rows.Count == 0)
        {
            PH_Tasks.Controls.Add(new Literal { Text = "<tr><td colspan='11' class='text-center'>No records found</td></tr>" });
            return;
        }

        foreach (DataRow dr in dt.Rows)
        {
            string startDate = dr["StartDate"] != DBNull.Value ? Convert.ToDateTime(dr["StartDate"]).ToString("dd/MM/yyyy") : "-";
            string rawDateForUrl = dr["StartDate"] != DBNull.Value ? Convert.ToDateTime(dr["StartDate"]).ToString("yyyy-MM-dd") : "";
            string empKey = dr["EmployeeKey"] != DBNull.Value ? dr["EmployeeKey"].ToString() : "";
            
            string assignedTo = dr["AssignedTo"] != DBNull.Value ? dr["AssignedTo"].ToString() : "-";
            string empImage   = dr["EmpImage"]   != DBNull.Value ? dr["EmpImage"].ToString() : "";
            decimal assignedHours = dr["AssignedHours"] != DBNull.Value ? Convert.ToDecimal(dr["AssignedHours"]) : 0;
            decimal actualHours   = dr["ActualHours"]   != DBNull.Value ? Convert.ToDecimal(dr["ActualHours"])   : 0;
            
            int meetingCount = dr["MeetingCount"] != DBNull.Value ? Convert.ToInt32(dr["MeetingCount"]) : 0;
            int stTotal = dr["SubTaskCount"] != DBNull.Value ? Convert.ToInt32(dr["SubTaskCount"]) : 0;
            
            int stCompleted = dr["CompletedCount"] != DBNull.Value ? Convert.ToInt32(dr["CompletedCount"]) : 0;
            int stYetToStart = dr["YetToStartCount"] != DBNull.Value ? Convert.ToInt32(dr["YetToStartCount"]) : 0;
            int stInProgress = dr["InProgressCount"] != DBNull.Value ? Convert.ToInt32(dr["InProgressCount"]) : 0;
            int stOverdue = dr["OverdueCount"] != DBNull.Value ? Convert.ToInt32(dr["OverdueCount"]) : 0;

            string viewButton = "<a href='DailyTaskDetails.aspx?date=" + rawDateForUrl + "&emp=" + empKey + "' class='btn btn-xs btn-info' title='View Details'><i class='glyphicon glyphicon-eye-open'></i></a>";

            string row =
                "<tr>" +
                    "<td>" + startDate + "</td>" +
                    "<td>" + BuildAssigneeCell(assignedTo, empImage) + "</td>" +
                    "<td class='text-center'>" + assignedHours.ToString("0.##") + "</td>" +
                    "<td class='text-center'>" + actualHours.ToString("0.##") + "</td>" +
                    "<td class='text-center'>" + stTotal + "</td>" +
                    "<td class='text-center'>" + meetingCount + "</td>" +
                    "<td class='text-center'>" + BuildBadge(stYetToStart, "purple") + "</td>" +
                    "<td class='text-center'>" + BuildBadge(stInProgress, "blue") + "</td>" +
                    "<td class='text-center'>" + BuildBadge(stCompleted, "green") + "</td>" +
                    "<td class='text-center'>" + BuildBadge(stOverdue, "red") + "</td>" +
                    "<td class='action-cell'>" + viewButton + "</td>" +
                "</tr>";
            PH_Tasks.Controls.Add(new Literal { Text = row });
        }
    }
}
