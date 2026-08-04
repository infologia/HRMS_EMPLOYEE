using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;

public partial class Employee_taskgrids : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Tasks";

            if (Request.QueryString["action"] == "delete" && !string.IsNullOrEmpty(Request.QueryString["taskkey"]))
            {
                DeleteTask(Request.QueryString["taskkey"]);
                return;
            }

            bool comingFromDetails = false;
            if (Request.UrlReferrer != null && Request.UrlReferrer.AbsolutePath.IndexOf("createtasknew.aspx", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                comingFromDetails = true;
            }

            if (!comingFromDetails)
            {
                Session["SelectedEmployeeFilter_NewTask"] = null;
                Session["SelectedMonthFilter_NewTask"] = null;
                Session["SelectedYearFilter_NewTask"] = null;
                Session["SelectedWidget_NewTask"] = null;
            }
            
            if (Session["SelectedWidget_NewTask"] != null)
            {
                hfActiveStatus.Value = Session["SelectedWidget_NewTask"].ToString();
            }
            else
            {
                hfActiveStatus.Value = "0";
            }
            
            BindMonthYear(ddlDate, ddlYear);
            CheckAndShowEmployeeFilter();
            LoadProjectHeader();
            LoadDashboard();

            Session.Remove("SelectedEmployeeFilter_NewTask");
            Session.Remove("SelectedMonthFilter_NewTask");
            Session.Remove("SelectedYearFilter_NewTask");
            Session.Remove("SelectedWidget_NewTask");
        }
    }

    // ── UI helpers: enhanced cell rendering (avatar + status badges) ──
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

        // Build full path — same pattern used across the project
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
        // variant: purple | blue | green | red
        return "<span class='count-badge badge-" + variant + "'>" + value + "</span>";
    }

    protected void CardClick(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        hfActiveStatus.Value = btn.CommandArgument;
        int status = int.Parse(hfActiveStatus.Value);
        
        if (divEmployeeFilter.Visible)
        {
            Session["SelectedEmployeeFilter_NewTask"] = ddlEmployee.SelectedValue;
        }
        Session["SelectedMonthFilter_NewTask"] = ddlDate.SelectedValue;
        Session["SelectedYearFilter_NewTask"] = ddlYear.SelectedValue;
        Session["SelectedWidget_NewTask"] = hfActiveStatus.Value;

        ScriptManager.RegisterStartupScript(this, this.GetType(), "reinitDT", "initDataTable();", true);
        LoadProjectHeader();
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
            
            // Verify user is the creator
            SqlCommand cmdCheck = new SqlCommand("SELECT CreatedBy FROM IT_TaskCreation WHERE TaskKey = @TaskKey");
            cmdCheck.Parameters.AddWithValue("@TaskKey", taskKeyInt);
            DataTable dtCheck = DA.GetDataTable(cmdCheck);
            
            if (dtCheck != null && dtCheck.Rows.Count > 0)
            {
                string createdBy = dtCheck.Rows[0]["CreatedBy"].ToString();
                
                if (createdBy == userid)
                {
                    // Delete the task
                    SqlCommand cmdDelete = new SqlCommand("DELETE FROM IT_TaskCreation WHERE TaskKey = @TaskKey");
                    cmdDelete.Parameters.AddWithValue("@TaskKey", taskKeyInt);
                    DA.ExecuteNonQuery(cmdDelete);
                    
                    string projectKey = Request.QueryString["id"];
                    string redirectUrl = string.IsNullOrEmpty(projectKey) ? "newtaskgrids.aspx" : "newtaskgrids.aspx?id=" + projectKey;
                    Response.Redirect(redirectUrl + "&msg=deleted");
                }
                else
                {
                    Response.Redirect("newtaskgrids.aspx?msg=unauthorized");
                }
            }
        }
        catch
        {
            Response.Redirect("newtaskgrids.aspx?msg=error");
        }
    }

    private void LoadProjectHeader()
    {
        string projectKey = Request.QueryString["id"];
        string userid = this.SC.Userid;
        
        bool showCreateButton = false;
        
        if (!string.IsNullOrEmpty(userid))
        {
            if (!string.IsNullOrEmpty(projectKey))
            {
                // Check if the user is explicitly assigned as a Team Lead for this specific project
                SqlCommand cmdCheckTL = new SqlCommand("SELECT 1 FROM IT_ProjectTeamLeads WHERE ProjectKey = @ProjectKey AND EmployeeKey = @EmpId");
                cmdCheckTL.Parameters.AddWithValue("@ProjectKey", projectKey);
                cmdCheckTL.Parameters.AddWithValue("@EmpId", userid);
                DataTable dtCheckTL = DA.GetDataTable(cmdCheckTL);
                
                if (dtCheckTL != null && dtCheckTL.Rows.Count > 0)
                {
                    showCreateButton = true;
                }
            }
            else
            {
                // If viewing All Projects, check if they are a team lead in ANY project
                SqlCommand cmdCheckTL = new SqlCommand("SELECT 1 FROM IT_ProjectTeamLeads WHERE EmployeeKey = @EmpId");
                cmdCheckTL.Parameters.AddWithValue("@EmpId", userid);
                DataTable dtCheckTL = DA.GetDataTable(cmdCheckTL);
                
                if (dtCheckTL != null && dtCheckTL.Rows.Count > 0)
                {
                    showCreateButton = true;
                }
            }
        }
        
        // Set Create Task button visibility
        lnk_CreateTask.Visible = showCreateButton;
        
        if (!string.IsNullOrEmpty(projectKey))
        {
            // Get project name from database
            SqlCommand cmd = new SqlCommand("SELECT ProjectName FROM IT_Projects WHERE ProjectKey = @ProjectKey");
            cmd.Parameters.AddWithValue("@ProjectKey", projectKey);
            DataTable dt = DA.GetDataTable(cmd);
            
            if (dt != null && dt.Rows.Count > 0)
            {
                lbl_ProjectHeader.Text = dt.Rows[0]["ProjectName"].ToString();
                // Pass project key to CreateTask page
                if (showCreateButton)
                {
                    lnk_CreateTask.NavigateUrl = "createtasknew.aspx?project=" + Server.UrlEncode(projectKey);
                }
            }
            else
            {
                lbl_ProjectHeader.Text = "Project Not Found";
                if (showCreateButton)
                {
                    lnk_CreateTask.NavigateUrl = "createtasknew.aspx";
                }
            }
        }
        else
        {
            lbl_ProjectHeader.Text = "All Projects";
            if (showCreateButton)
            {
                lnk_CreateTask.NavigateUrl = "createtasknew.aspx";
            }
        }
    }

    private void CheckAndShowEmployeeFilter()
    {
        string str_userid = this.SC.Userid;
        string projectKey = Request.QueryString["id"];
        
        bool isTeamLead = IsUserTeamLead(str_userid, projectKey);
        
        if (isTeamLead)
        {
            divEmployeeFilter.Visible = true;
            BindEmployeeDropdown();
        }
        else
        {
            divEmployeeFilter.Visible = false;
        }
    }

    private void BindEmployeeDropdown()
    {
        string query = @"SELECT EmployeeKey, (Firstname + ' ' + Lastname) AS EmployeeName 
                        FROM IT_EmployeeRegister 
                        WHERE Employeestatus = 1 
                        ORDER BY Firstname";
        
        DataTable dt = DA.GetDataTable(query);
        
        ddlEmployee.Items.Clear();
        ddlEmployee.Items.Add(new ListItem("-- All Employees --", "0"));
        
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
            {
                ddlEmployee.Items.Add(new ListItem(dr["EmployeeName"].ToString(), dr["EmployeeKey"].ToString()));
            }
        }
        
        if (Session["SelectedEmployeeFilter_NewTask"] != null)
        {
            if (ddlEmployee.Items.FindByValue(Session["SelectedEmployeeFilter_NewTask"].ToString()) != null)
            {
                ddlEmployee.SelectedValue = Session["SelectedEmployeeFilter_NewTask"].ToString();
            }
        }
        else if (ddlEmployee.Items.FindByValue(this.SC.Userid) != null)
        {
            ddlEmployee.SelectedValue = this.SC.Userid;
        }
    }

    private void BindMonthYear(DropDownList ddlMonth, DropDownList ddlYr)
    {
        ddlMonth.Items.Clear();
        ddlMonth.Items.Add(new ListItem("All", "0"));
        for (int m = 1; m <= 12; m++)
            ddlMonth.Items.Add(new ListItem(new DateTime(2025, m, 1).ToString("MMMM"), (m + 1).ToString()));
            
        if (Session["SelectedMonthFilter_NewTask"] != null)
        {
            ddlMonth.SelectedValue = Session["SelectedMonthFilter_NewTask"].ToString();
        }
        else
        {
            ddlMonth.SelectedValue = (DateTime.Now.Month + 1).ToString();
        }

        ddlYr.Items.Clear();
        int currentYear = DateTime.Now.Year;
        for (int y = currentYear - 5; y <= currentYear + 5; y++)
            ddlYr.Items.Add(new ListItem(y.ToString(), y.ToString()));
            
        if (Session["SelectedYearFilter_NewTask"] != null)
        {
            ddlYr.SelectedValue = Session["SelectedYearFilter_NewTask"].ToString();
        }
        else
        {
            ddlYr.SelectedValue = currentYear.ToString();
        }
    }

    private void LoadDashboard()
    {
        // Load counts for all statuses
        LoadAllTasksCount(lbl_AllTaskCount);
        LoadCount(lbl_YetToStartCount, 1);   // StatusID 1 = Yet to Start
        LoadCount(lbl_InProgressCount, 2);   // StatusID 2 = In Progress
        LoadOverdueCount(lbl_OverDueCount);  // Overdue = StartDate old + subtask not completed
        LoadCompletedCount(lbl_CompletedCount); // ALL subtasks completed
        
        // Load grid based on active status
        int status = int.Parse(hfActiveStatus.Value);
        switch (status)
        {
            case 0: BindAllTasksGrid(); break;
            case 1: BindGrid(1); break;   // Yet to Start
            case 2: BindGrid(2); break;   // In Progress
            case 3: BindOverdueGrid(); break;
            case 4: BindCompletedGrid(); break; // All subtasks completed
            case 5: BindGrid(3); break;   // Pending (StatusID=3)
            default: BindAllTasksGrid(); break;
        }
    }

    private void LoadAllTasksCount(Label lblCard)
    {
        string userid = this.SC.Userid;
        if (string.IsNullOrEmpty(userid)) return;

        int monthVal = int.Parse(ddlDate.SelectedValue);
        int month = (monthVal >= 2) ? monthVal - 1 : 0;
        int year = int.Parse(ddlYear.SelectedValue);

        bool isTeamLead = IsUserTeamLead(userid, Request.QueryString["id"]);

        bool showAllEmployees = isTeamLead && divEmployeeFilter.Visible && ddlEmployee.SelectedValue == "0";
        string filterUserId = userid;
        if (isTeamLead && divEmployeeFilter.Visible && ddlEmployee.SelectedValue != "0")
            filterUserId = ddlEmployee.SelectedValue;

        string where = " WHERE 1=1";
        if (!showAllEmployees)
            where += " AND a.EmployeeList = @UserId";
        
        string projectKey = Request.QueryString["id"];
        if (!string.IsNullOrEmpty(projectKey))
            where += " AND a.ProjectName = @ProjectKey";
        if (month > 0) where += " AND MONTH(a.StartDate) = @Month";
        if (year > 0)  where += " AND YEAR(a.StartDate) = @Year";

        string query = @"SELECT COUNT(d.TaskDetailID) 
                         FROM IT_TaskCreation a
                         LEFT JOIN IT_TaskDescriptiondetails d ON a.TaskKey = d.TaskKey" + where;

        SqlCommand cmd = new SqlCommand(query);
        if (!showAllEmployees)
            cmd.Parameters.AddWithValue("@UserId", filterUserId);
        if (!string.IsNullOrEmpty(projectKey))
            cmd.Parameters.AddWithValue("@ProjectKey", projectKey);
        if (month > 0) cmd.Parameters.AddWithValue("@Month", month);
        if (year > 0)  cmd.Parameters.AddWithValue("@Year", year);

        DataTable dtCount = DA.GetDataTable(cmd);
        string count = (dtCount != null && dtCount.Rows.Count > 0) ? dtCount.Rows[0][0].ToString() : "0";
        lblCard.Text = count;
    }

    private void LoadCount(Label lblCard, int statusId)
    {
        string userid = this.SC.Userid;
        if (string.IsNullOrEmpty(userid)) return;

        int monthVal = int.Parse(ddlDate.SelectedValue);
        int month = (monthVal >= 2) ? monthVal - 1 : 0;
        int year = int.Parse(ddlYear.SelectedValue);

        bool isTeamLead = IsUserTeamLead(userid, Request.QueryString["id"]);

        bool showAllEmployees = isTeamLead && divEmployeeFilter.Visible && ddlEmployee.SelectedValue == "0";
        string filterUserId = userid;
        if (isTeamLead && divEmployeeFilter.Visible && ddlEmployee.SelectedValue != "0")
            filterUserId = ddlEmployee.SelectedValue;

        string where = " WHERE d.Status = @StatusId";
        if (!showAllEmployees)
            where += " AND a.EmployeeList = @UserId";
        
        string projectKey = Request.QueryString["id"];
        if (!string.IsNullOrEmpty(projectKey))
            where += " AND a.ProjectName = @ProjectKey";
        if (month > 0) where += " AND MONTH(a.StartDate) = @Month";
        if (year > 0)  where += " AND YEAR(a.StartDate) = @Year";

        string query = @"SELECT COUNT( a.TaskKey) 
                        FROM IT_TaskCreation a
                        LEFT JOIN IT_TaskDescriptiondetails d ON a.TaskKey = d.TaskKey" + where;

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@StatusId", statusId);
        if (!showAllEmployees)
            cmd.Parameters.AddWithValue("@UserId", filterUserId);
        if (!string.IsNullOrEmpty(projectKey))
            cmd.Parameters.AddWithValue("@ProjectKey", projectKey);
        if (month > 0) cmd.Parameters.AddWithValue("@Month", month);
        if (year > 0)  cmd.Parameters.AddWithValue("@Year", year);

        DataTable dtCount = DA.GetDataTable(cmd);
        string count = (dtCount != null && dtCount.Rows.Count > 0) ? dtCount.Rows[0][0].ToString() : "0";
        lblCard.Text = count;
    }

    private void LoadOverdueCount(Label lblCard)
    {
        string userid = this.SC.Userid;
        if (string.IsNullOrEmpty(userid)) return;

        int monthVal = int.Parse(ddlDate.SelectedValue);
        int month = (monthVal >= 2) ? monthVal - 1 : 0;
        int year = int.Parse(ddlYear.SelectedValue);

        bool isTeamLead = IsUserTeamLead(userid, Request.QueryString["id"]);

        bool showAllEmployees = isTeamLead && divEmployeeFilter.Visible && ddlEmployee.SelectedValue == "0";
        string filterUserId = userid;
        if (isTeamLead && divEmployeeFilter.Visible && ddlEmployee.SelectedValue != "0")
            filterUserId = ddlEmployee.SelectedValue;

        string where = " WHERE a.StartDate < @CurrentDate AND d.Status != 4";
        if (!showAllEmployees)
            where += " AND a.EmployeeList = @UserId";
        
        string projectKey = Request.QueryString["id"];
        if (!string.IsNullOrEmpty(projectKey))
            where += " AND a.ProjectName = @ProjectKey";
        if (month > 0) where += " AND MONTH(a.StartDate) = @Month";
        if (year > 0)  where += " AND YEAR(a.StartDate) = @Year";

        string query = @"SELECT COUNT(DISTINCT a.TaskKey) 
                        FROM IT_TaskCreation a
                        LEFT JOIN IT_TaskDescriptiondetails d ON a.TaskKey = d.TaskKey" + where;

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@CurrentDate", DateTime.Now.Date);
        if (!showAllEmployees)
            cmd.Parameters.AddWithValue("@UserId", filterUserId);
        if (!string.IsNullOrEmpty(projectKey))
            cmd.Parameters.AddWithValue("@ProjectKey", projectKey);
        if (month > 0) cmd.Parameters.AddWithValue("@Month", month);
        if (year > 0)  cmd.Parameters.AddWithValue("@Year", year);

        DataTable dtCount = DA.GetDataTable(cmd);
        string count = (dtCount != null && dtCount.Rows.Count > 0) ? dtCount.Rows[0][0].ToString() : "0";
        lblCard.Text = count;
    }

    // Count tasks where ALL subtasks are completed (Status = 4)
    private void LoadCompletedCount(Label lblCard)
    {
        string userid = this.SC.Userid;
        if (string.IsNullOrEmpty(userid)) return;

        int monthVal = int.Parse(ddlDate.SelectedValue);
        int month = (monthVal >= 2) ? monthVal - 1 : 0;
        int year = int.Parse(ddlYear.SelectedValue);

        bool isTeamLead = IsUserTeamLead(userid, Request.QueryString["id"]);

        bool showAllEmployees = isTeamLead && divEmployeeFilter.Visible && ddlEmployee.SelectedValue == "0";
        string filterUserId = userid;
        if (isTeamLead && divEmployeeFilter.Visible && ddlEmployee.SelectedValue != "0")
            filterUserId = ddlEmployee.SelectedValue;

        string projectKey = Request.QueryString["id"];

        string baseWhere = " WHERE 1=1";
        if (!showAllEmployees) baseWhere += " AND a.EmployeeList = @UserId";
        if (!string.IsNullOrEmpty(projectKey)) baseWhere += " AND a.ProjectName = @ProjectKey";
        if (month > 0) baseWhere += " AND MONTH(a.StartDate) = @Month";
        if (year > 0)  baseWhere += " AND YEAR(a.StartDate) = @Year";

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
        if (!showAllEmployees) cmd.Parameters.AddWithValue("@UserId", filterUserId);
        if (!string.IsNullOrEmpty(projectKey)) cmd.Parameters.AddWithValue("@ProjectKey", projectKey);
        if (month > 0) cmd.Parameters.AddWithValue("@Month", month);
        if (year > 0)  cmd.Parameters.AddWithValue("@Year", year);

        DataTable dtCount = DA.GetDataTable(cmd);
        lblCard.Text = (dtCount != null && dtCount.Rows.Count > 0) ? dtCount.Rows[0][0].ToString() : "0";
    }

    // Helper: get sub-task status counts for a given TaskKey
    private void GetSubTaskCounts(int taskKey, out int total, out int completed, out int yetToStart, out int inProgress, out int overdue)
    {
        total = 0; completed = 0; yetToStart = 0; inProgress = 0; overdue = 0;
        SqlCommand cmd = new SqlCommand(@"
            SELECT 
                COUNT(*) AS Total,
                SUM(CASE WHEN d.Status = 4 THEN 1 ELSE 0 END) AS Completed,
                SUM(CASE WHEN d.Status = 1 THEN 1 ELSE 0 END) AS YetToStart,
                SUM(CASE WHEN d.Status = 2 THEN 1 ELSE 0 END) AS InProgress,
                SUM(CASE WHEN d.Status != 4 AND a.StartDate < @CurrentDate THEN 1 ELSE 0 END) AS Overdue
            FROM IT_TaskDescriptiondetails d
            JOIN IT_TaskCreation a ON d.TaskKey = a.TaskKey
            WHERE d.TaskKey = @TaskKey");
        cmd.Parameters.AddWithValue("@TaskKey", taskKey);
        cmd.Parameters.AddWithValue("@CurrentDate", DateTime.Now.Date);
        DataTable dt = DA.GetDataTable(cmd);
        if (dt != null && dt.Rows.Count > 0)
        {
            total      = dt.Rows[0]["Total"]      != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["Total"])      : 0;
            completed  = dt.Rows[0]["Completed"]  != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["Completed"])  : 0;
            yetToStart = dt.Rows[0]["YetToStart"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["YetToStart"]) : 0;
            inProgress = dt.Rows[0]["InProgress"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["InProgress"]) : 0;
            overdue    = dt.Rows[0]["Overdue"]    != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["Overdue"])    : 0;
        }
    }

    private void BindGrid(int statusId)
    {
        string userid = this.SC.Userid;
        if (string.IsNullOrEmpty(userid)) return;

        int monthVal = int.Parse(ddlDate.SelectedValue);
        int month = (monthVal >= 2) ? monthVal - 1 : 0;
        int year = int.Parse(ddlYear.SelectedValue);

        bool isTeamLead = IsUserTeamLead(userid, Request.QueryString["id"]);

        bool showAllEmployees = isTeamLead && divEmployeeFilter.Visible && ddlEmployee.SelectedValue == "0";
        string filterUserId = userid;
        if (isTeamLead && divEmployeeFilter.Visible && ddlEmployee.SelectedValue != "0")
            filterUserId = ddlEmployee.SelectedValue;

        string where = " WHERE 1=1";
        if (!showAllEmployees)
            where += " AND a.EmployeeList = @UserId";
        
        string projectKey = Request.QueryString["id"];
        if (!string.IsNullOrEmpty(projectKey))
            where += " AND a.ProjectName = @ProjectKey";
        if (month > 0) where += " AND MONTH(a.StartDate) = @Month";
        if (year > 0)  where += " AND YEAR(a.StartDate) = @Year";

        string query = @"SELECT 
                            CAST(a.StartDate AS DATE) AS StartDate,
                            MAX(a.TaskKey) AS TaskKey, 
                            MAX(a.CreatedBy) AS CreatedBy,
                            (e.Firstname + ' ' + e.Lastname) AS AssignedTo,
                            e.Image AS EmpImage,
                            SUM(ISNULL(d.AssignedHours, 0)) AS AssignedHours, 
                            SUM(ISNULL(d.ActualHours, 0)) AS ActualHours, 
                            COUNT(d.TaskDetailID) AS stTotal,
                            SUM(CASE WHEN d.Status = 1 THEN 1 ELSE 0 END) AS stYetToStart,
                            SUM(CASE WHEN d.Status = 2 THEN 1 ELSE 0 END) AS stInProgress,
                            SUM(CASE WHEN d.Status = 4 THEN 1 ELSE 0 END) AS stCompleted,
                            SUM(CASE WHEN d.Status != 4 AND a.StartDate < @CurrentDate THEN 1 ELSE 0 END) AS stOverdue,
                            MIN(d.Status) AS Status
                        FROM IT_TaskCreation a
                        LEFT JOIN IT_EmployeeRegister e ON a.EmployeeList = e.EmployeeKey
                        LEFT JOIN IT_TaskDescriptiondetails d ON a.TaskKey = d.TaskKey" + where + @"
                        GROUP BY CAST(a.StartDate AS DATE), a.EmployeeList, e.Firstname, e.Lastname, e.Image
                        HAVING SUM(CASE WHEN d.Status = @StatusId THEN 1 ELSE 0 END) > 0
                        ORDER BY StartDate DESC";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@CurrentDate", DateTime.Now.Date);
        if (!showAllEmployees)
            cmd.Parameters.AddWithValue("@UserId", filterUserId);
        if (!string.IsNullOrEmpty(projectKey))
            cmd.Parameters.AddWithValue("@ProjectKey", projectKey);
        if (month > 0) cmd.Parameters.AddWithValue("@Month", month);
        if (year > 0)  cmd.Parameters.AddWithValue("@Year", year);
        cmd.Parameters.AddWithValue("@StatusId", statusId);
        
        DataTable dt = DA.GetDataTable(cmd);

        PH_Tasks.Controls.Clear();
        if (dt == null || dt.Rows.Count == 0)
        {
            PH_Tasks.Controls.Add(new Literal { Text = "<tr><td colspan='14' class='text-center'>No records found</td></tr>" });
            return;
        }

        foreach (DataRow dr in dt.Rows)
        {
            string startDate  = dr["StartDate"]  != DBNull.Value ? Convert.ToDateTime(dr["StartDate"]).ToString("dd/MM/yyyy") : "-";
            string assignedTo = dr["AssignedTo"] != DBNull.Value ? dr["AssignedTo"].ToString() : "-";
            string empImage   = dr["EmpImage"]   != DBNull.Value ? dr["EmpImage"].ToString() : "";
            int assignedHours = dr["AssignedHours"] != DBNull.Value ? Convert.ToInt32(dr["AssignedHours"]) : 0;
            int actualHours   = dr["ActualHours"]   != DBNull.Value ? Convert.ToInt32(dr["ActualHours"])   : 0;
            
            int stTotal       = dr["stTotal"]      != DBNull.Value ? Convert.ToInt32(dr["stTotal"])      : 0;
            int stYetToStart  = dr["stYetToStart"] != DBNull.Value ? Convert.ToInt32(dr["stYetToStart"]) : 0;
            int stInProgress  = dr["stInProgress"] != DBNull.Value ? Convert.ToInt32(dr["stInProgress"]) : 0;
            int stCompleted   = dr["stCompleted"]  != DBNull.Value ? Convert.ToInt32(dr["stCompleted"])  : 0;
            int stOverdue     = dr["stOverdue"]    != DBNull.Value ? Convert.ToInt32(dr["stOverdue"])    : 0;

            int taskKey = Convert.ToInt32(dr["TaskKey"]);

            // View button always shown
            string viewButton = "<a href='createtasknew.aspx?id=" + taskKey + "&view=1' class='btn btn-xs btn-info' title='View'><i class='glyphicon glyphicon-eye-open'></i></a>";

            string buttonHtml = "";
            if (statusId == 4 || (stTotal > 0 && stCompleted == stTotal))
            {
                buttonHtml = "<button type='button' class='btn btn-xs btn-default' disabled title='Completed task cannot be edited'><i class='glyphicon glyphicon-edit'></i></button>";
            }
            else
            {
                buttonHtml = "<a href='createtasknew.aspx?id=" + taskKey + "' class='btn btn-xs btn-primary'><i class='glyphicon glyphicon-edit'></i></a>";
            }
            
            string createdBy = dr["CreatedBy"] != DBNull.Value ? dr["CreatedBy"].ToString() : "";
            string removeButton = "";
            if (statusId == 4 || (stTotal > 0 && stCompleted > 0))
            {
                removeButton = "<button type='button' class='btn btn-xs btn-default' disabled title='Task with completed subtasks cannot be deleted'><i class='glyphicon glyphicon-trash'></i></button>";
            }
            else if (createdBy == userid)
            {
                removeButton = "<button type='button' class='btn btn-xs btn-danger' onclick='confirmDelete(" + taskKey + ")'><i class='glyphicon glyphicon-trash'></i></button>";
            }
            else
            {
                removeButton = "<button type='button' class='btn btn-xs btn-default' disabled><i class='glyphicon glyphicon-trash'></i></button>";
            }

            string row =
                "<tr>" +
                    "<td>" + startDate + "</td>" +
                    "<td>" + BuildAssigneeCell(assignedTo, empImage) + "</td>" +
                    "<td class='text-center'>" + assignedHours + "</td>" +
                    "<td class='text-center'>" + actualHours + "</td>" +
                    "<td class='text-center'>" + stTotal + "</td>" +
                    "<td class='text-center'>" + BuildBadge(stYetToStart, "purple") + "</td>" +
                    "<td class='text-center'>" + BuildBadge(stInProgress, "blue") + "</td>" +
                    "<td class='text-center'>" + BuildBadge(stCompleted, "green") + "</td>" +
                    "<td class='text-center'>" + BuildBadge(stOverdue, "red") + "</td>" +
                    "<td class='action-cell'>" + viewButton + " " + buttonHtml + " " + removeButton + "</td>" +
                "</tr>";
            PH_Tasks.Controls.Add(new Literal { Text = row });
        }
    }

    // Grid: tasks where ALL subtasks are Status=4 (Completed)
    private void BindCompletedGrid()
    {
        string userid = this.SC.Userid;
        if (string.IsNullOrEmpty(userid)) return;

        int monthVal = int.Parse(ddlDate.SelectedValue);
        int month = (monthVal >= 2) ? monthVal - 1 : 0;
        int year = int.Parse(ddlYear.SelectedValue);

        bool isTeamLead = IsUserTeamLead(userid, Request.QueryString["id"]);

        bool showAllEmployees = isTeamLead && divEmployeeFilter.Visible && ddlEmployee.SelectedValue == "0";
        string filterUserId = userid;
        if (isTeamLead && divEmployeeFilter.Visible && ddlEmployee.SelectedValue != "0")
            filterUserId = ddlEmployee.SelectedValue;

        string projectKey = Request.QueryString["id"];

        string baseWhere = " WHERE 1=1";
        if (!showAllEmployees) baseWhere += " AND a.EmployeeList = @UserId";
        if (!string.IsNullOrEmpty(projectKey)) baseWhere += " AND a.ProjectName = @ProjectKey";
        if (month > 0) baseWhere += " AND MONTH(a.StartDate) = @Month";
        if (year > 0)  baseWhere += " AND YEAR(a.StartDate) = @Year";

        string query = @"SELECT 
                            CAST(a.StartDate AS DATE) AS StartDate,
                            MAX(a.TaskKey) AS TaskKey,
                            MAX(a.CreatedBy) AS CreatedBy,
                            (e.Firstname + ' ' + e.Lastname) AS AssignedTo,
                            e.Image AS EmpImage,
                            SUM(ISNULL(d.AssignedHours, 0)) AS AssignedHours,
                            SUM(ISNULL(d.ActualHours, 0)) AS ActualHours,
                            COUNT(d.TaskDetailID) AS stTotal,
                            SUM(CASE WHEN d.Status = 1 THEN 1 ELSE 0 END) AS stYetToStart,
                            SUM(CASE WHEN d.Status = 2 THEN 1 ELSE 0 END) AS stInProgress,
                            SUM(CASE WHEN d.Status = 4 THEN 1 ELSE 0 END) AS stCompleted,
                            SUM(CASE WHEN d.Status != 4 AND a.StartDate < @CurrentDate THEN 1 ELSE 0 END) AS stOverdue
                        FROM IT_TaskCreation a
                        LEFT JOIN IT_EmployeeRegister e ON a.EmployeeList = e.EmployeeKey
                        LEFT JOIN IT_TaskDescriptiondetails d ON a.TaskKey = d.TaskKey"
                        + baseWhere + @"
                        GROUP BY CAST(a.StartDate AS DATE), a.EmployeeList, e.Firstname, e.Lastname, e.Image
                        HAVING COUNT(d.TaskDetailID) > 0 AND SUM(CASE WHEN d.Status != 4 THEN 1 ELSE 0 END) = 0
                        ORDER BY StartDate DESC";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@CurrentDate", DateTime.Now.Date);
        if (!showAllEmployees) cmd.Parameters.AddWithValue("@UserId", filterUserId);
        if (!string.IsNullOrEmpty(projectKey)) cmd.Parameters.AddWithValue("@ProjectKey", projectKey);
        if (month > 0) cmd.Parameters.AddWithValue("@Month", month);
        if (year > 0)  cmd.Parameters.AddWithValue("@Year", year);

        DataTable dt = DA.GetDataTable(cmd);

        PH_Tasks.Controls.Clear();
        if (dt == null || dt.Rows.Count == 0)
        {
            PH_Tasks.Controls.Add(new Literal { Text = "<tr><td colspan='12' class='text-center'>No completed tasks found</td></tr>" });
            return;
        }

        foreach (DataRow dr in dt.Rows)
        {
            string startDate  = dr["StartDate"]  != DBNull.Value ? Convert.ToDateTime(dr["StartDate"]).ToString("dd/MM/yyyy") : "-";
            string assignedTo = dr["AssignedTo"] != DBNull.Value ? dr["AssignedTo"].ToString() : "-";
            string empImage   = dr["EmpImage"]   != DBNull.Value ? dr["EmpImage"].ToString() : "";
            int assignedHours = dr["AssignedHours"] != DBNull.Value ? Convert.ToInt32(dr["AssignedHours"]) : 0;
            int actualHours   = dr["ActualHours"]   != DBNull.Value ? Convert.ToInt32(dr["ActualHours"])   : 0;
            
            int stTotal       = dr["stTotal"]      != DBNull.Value ? Convert.ToInt32(dr["stTotal"])      : 0;
            int stYetToStart  = dr["stYetToStart"] != DBNull.Value ? Convert.ToInt32(dr["stYetToStart"]) : 0;
            int stInProgress  = dr["stInProgress"] != DBNull.Value ? Convert.ToInt32(dr["stInProgress"]) : 0;
            int stCompleted   = dr["stCompleted"]  != DBNull.Value ? Convert.ToInt32(dr["stCompleted"])  : 0;
            int stOverdue     = dr["stOverdue"]    != DBNull.Value ? Convert.ToInt32(dr["stOverdue"])    : 0;

            int taskKey = Convert.ToInt32(dr["TaskKey"]);

            string viewButton   = "<a href='createtasknew.aspx?id=" + taskKey + "&view=1' class='btn btn-xs btn-info' title='View'><i class='glyphicon glyphicon-eye-open'></i></a>";
            string updateButton = "<button type='button' class='btn btn-xs btn-default' disabled title='Completed task cannot be edited'><i class='glyphicon glyphicon-edit'></i></button>";
            string removeButton = "<button type='button' class='btn btn-xs btn-default' disabled title='Completed task cannot be deleted'><i class='glyphicon glyphicon-trash'></i></button>";

            string row =
                "<tr>" +
                    "<td>" + startDate   + "</td>" +
                    "<td>" + BuildAssigneeCell(assignedTo, empImage)  + "</td>" +
                    "<td class='text-center'>" + assignedHours + "</td>" +
                    "<td class='text-center'>" + actualHours   + "</td>" +
                    "<td class='text-center'>" + stTotal       + "</td>" +
                    "<td class='text-center'>" + BuildBadge(stYetToStart, "purple")  + "</td>" +
                    "<td class='text-center'>" + BuildBadge(stInProgress, "blue")  + "</td>" +
                    "<td class='text-center'>" + BuildBadge(stCompleted, "green")   + "</td>" +
                    "<td class='text-center'>" + BuildBadge(stOverdue, "red")     + "</td>" +
                    "<td class='action-cell'>" + viewButton + " " + updateButton + " " + removeButton + "</td>" +
                "</tr>";
            PH_Tasks.Controls.Add(new Literal { Text = row });
        }
    }

    private void BindOverdueGrid()
    {
        string userid = this.SC.Userid;
        if (string.IsNullOrEmpty(userid)) return;

        int monthVal = int.Parse(ddlDate.SelectedValue);
        int month = (monthVal >= 2) ? monthVal - 1 : 0;
        int year = int.Parse(ddlYear.SelectedValue);

        bool isTeamLead = IsUserTeamLead(userid, Request.QueryString["id"]);

        bool showAllEmployees = isTeamLead && divEmployeeFilter.Visible && ddlEmployee.SelectedValue == "0";
        string filterUserId = userid;
        if (isTeamLead && divEmployeeFilter.Visible && ddlEmployee.SelectedValue != "0")
            filterUserId = ddlEmployee.SelectedValue;

        string where = " WHERE 1=1";
        if (!showAllEmployees)
            where += " AND a.EmployeeList = @UserId";
        
        string projectKey = Request.QueryString["id"];
        if (!string.IsNullOrEmpty(projectKey))
            where += " AND a.ProjectName = @ProjectKey";
        if (month > 0) where += " AND MONTH(a.StartDate) = @Month";
        if (year > 0)  where += " AND YEAR(a.StartDate) = @Year";

        string query = @"SELECT 
                            CAST(a.StartDate AS DATE) AS StartDate,
                            MAX(a.TaskKey) AS TaskKey, 
                            MAX(a.CreatedBy) AS CreatedBy,
                            (e.Firstname + ' ' + e.Lastname) AS AssignedTo,
                            e.Image AS EmpImage,
                            SUM(ISNULL(d.AssignedHours, 0)) AS AssignedHours, 
                            SUM(ISNULL(d.ActualHours, 0)) AS ActualHours, 
                            COUNT(d.TaskDetailID) AS stTotal,
                            SUM(CASE WHEN d.Status = 1 THEN 1 ELSE 0 END) AS stYetToStart,
                            SUM(CASE WHEN d.Status = 2 THEN 1 ELSE 0 END) AS stInProgress,
                            SUM(CASE WHEN d.Status = 4 THEN 1 ELSE 0 END) AS stCompleted,
                            SUM(CASE WHEN d.Status != 4 AND a.StartDate < @CurrentDate THEN 1 ELSE 0 END) AS stOverdue,
                            MIN(d.Status) AS Status
                        FROM IT_TaskCreation a
                        LEFT JOIN IT_EmployeeRegister e ON a.EmployeeList = e.EmployeeKey
                        LEFT JOIN IT_TaskDescriptiondetails d ON a.TaskKey = d.TaskKey" + where + @"
                        GROUP BY CAST(a.StartDate AS DATE), a.EmployeeList, e.Firstname, e.Lastname, e.Image
                        HAVING SUM(CASE WHEN d.Status != 4 AND a.StartDate < @CurrentDate THEN 1 ELSE 0 END) > 0
                        ORDER BY StartDate DESC";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@CurrentDate", DateTime.Now.Date);
        if (!showAllEmployees)
            cmd.Parameters.AddWithValue("@UserId", filterUserId);
        if (!string.IsNullOrEmpty(projectKey))
            cmd.Parameters.AddWithValue("@ProjectKey", projectKey);
        if (month > 0) cmd.Parameters.AddWithValue("@Month", month);
        if (year > 0)  cmd.Parameters.AddWithValue("@Year", year);

        DataTable dt = DA.GetDataTable(cmd);

        PH_Tasks.Controls.Clear();
        if (dt == null || dt.Rows.Count == 0)
        {
            PH_Tasks.Controls.Add(new Literal { Text = "<tr><td colspan='14' class='text-center'>No overdue tasks found</td></tr>" });
            return;
        }

        foreach (DataRow dr in dt.Rows)
        {
            string startDate  = dr["StartDate"]  != DBNull.Value ? Convert.ToDateTime(dr["StartDate"]).ToString("dd/MM/yyyy") : "-";
            string assignedTo = dr["AssignedTo"] != DBNull.Value ? dr["AssignedTo"].ToString() : "-";
            string empImage   = dr["EmpImage"]   != DBNull.Value ? dr["EmpImage"].ToString() : "";
            int assignedHours = dr["AssignedHours"] != DBNull.Value ? Convert.ToInt32(dr["AssignedHours"]) : 0;
            int actualHours   = dr["ActualHours"]   != DBNull.Value ? Convert.ToInt32(dr["ActualHours"])   : 0;
            
            int stTotal       = dr["stTotal"]      != DBNull.Value ? Convert.ToInt32(dr["stTotal"])      : 0;
            int stYetToStart  = dr["stYetToStart"] != DBNull.Value ? Convert.ToInt32(dr["stYetToStart"]) : 0;
            int stInProgress  = dr["stInProgress"] != DBNull.Value ? Convert.ToInt32(dr["stInProgress"]) : 0;
            int stCompleted   = dr["stCompleted"]  != DBNull.Value ? Convert.ToInt32(dr["stCompleted"])  : 0;
            int stOverdue     = dr["stOverdue"]    != DBNull.Value ? Convert.ToInt32(dr["stOverdue"])    : 0;

            int taskKey = Convert.ToInt32(dr["TaskKey"]);

            // View button always present
            string viewButton = "<a href='createtasknew.aspx?id=" + taskKey + "&view=1' class='btn btn-xs btn-info' title='View'><i class='glyphicon glyphicon-eye-open'></i></a>";

            string buttonHtml = "";
            if (stTotal > 0 && stCompleted == stTotal)
            {
                buttonHtml = "<button type='button' class='btn btn-xs btn-default' disabled title='Completed task cannot be edited'><i class='glyphicon glyphicon-edit'></i></button>";
            }
            else
            {
                buttonHtml = "<a href='createtasknew.aspx?id=" + taskKey + "' class='btn btn-xs btn-primary'><i class='glyphicon glyphicon-edit'></i></a>";
            }
            
            string createdBy = dr["CreatedBy"] != DBNull.Value ? dr["CreatedBy"].ToString() : "";
            string removeButton = "";
            if (stTotal > 0 && stCompleted > 0)
            {
                removeButton = "<button type='button' class='btn btn-xs btn-default' disabled title='Task with completed subtasks cannot be deleted'><i class='glyphicon glyphicon-trash'></i></button>";
            }
            else if (createdBy == userid)
            {
                removeButton = "<button type='button' class='btn btn-xs btn-danger' onclick='confirmDelete(" + taskKey + ")'><i class='glyphicon glyphicon-trash'></i></button>";
            }
            else
            {
                removeButton = "<button type='button' class='btn btn-xs btn-default' disabled><i class='glyphicon glyphicon-trash'></i></button>";
            }
            
            string rowStyle = " style='background-color: #ffffff;'";
            
            string row =
                "<tr" + rowStyle + ">" +
                    "<td>" + startDate + "</td>" +
                    "<td>" + BuildAssigneeCell(assignedTo, empImage) + "</td>" +
                    "<td class='text-center'>" + assignedHours + "</td>" +
                    "<td class='text-center'>" + actualHours + "</td>" +
                    "<td class='text-center'>" + stTotal + "</td>" +
                    "<td class='text-center'>" + BuildBadge(stYetToStart, "purple") + "</td>" +
                    "<td class='text-center'>" + BuildBadge(stInProgress, "blue") + "</td>" +
                    "<td class='text-center'>" + BuildBadge(stCompleted, "green") + "</td>" +
                    "<td class='text-center'>" + BuildBadge(stOverdue, "red") + "</td>" +
                    "<td class='action-cell'>" + viewButton + " " + buttonHtml + " " + removeButton + "</td>" +
                "</tr>";
            PH_Tasks.Controls.Add(new Literal { Text = row });
        }
    }

    private void BindAllTasksGrid()
    {
        string userid = this.SC.Userid;
        if (string.IsNullOrEmpty(userid)) return;

        int monthVal = int.Parse(ddlDate.SelectedValue);
        int month = (monthVal >= 2) ? monthVal - 1 : 0;
        int year = int.Parse(ddlYear.SelectedValue);

        bool isTeamLead = IsUserTeamLead(userid, Request.QueryString["id"]);

        bool showAllEmployees = isTeamLead && divEmployeeFilter.Visible && ddlEmployee.SelectedValue == "0";
        string filterUserId = userid;
        if (isTeamLead && divEmployeeFilter.Visible && ddlEmployee.SelectedValue != "0")
            filterUserId = ddlEmployee.SelectedValue;

        string where = " WHERE 1=1";
        if (!showAllEmployees)
            where += " AND a.EmployeeList = @UserId";
        
        string projectKey = Request.QueryString["id"];
        if (!string.IsNullOrEmpty(projectKey))
            where += " AND a.ProjectName = @ProjectKey";
        if (month > 0) where += " AND MONTH(a.StartDate) = @Month";
        if (year > 0)  where += " AND YEAR(a.StartDate) = @Year";

        string query = @"SELECT 
                            CAST(a.StartDate AS DATE) AS StartDate,
                            MAX(a.TaskKey) AS TaskKey, 
                            MAX(a.CreatedBy) AS CreatedBy,
                            (e.Firstname + ' ' + e.Lastname) AS AssignedTo,
                            e.Image AS EmpImage,
                            SUM(ISNULL(d.AssignedHours, 0)) AS AssignedHours, 
                            SUM(ISNULL(d.ActualHours, 0)) AS ActualHours, 
                            COUNT(d.TaskDetailID) AS stTotal,
                            SUM(CASE WHEN d.Status = 1 THEN 1 ELSE 0 END) AS stYetToStart,
                            SUM(CASE WHEN d.Status = 2 THEN 1 ELSE 0 END) AS stInProgress,
                            SUM(CASE WHEN d.Status = 4 THEN 1 ELSE 0 END) AS stCompleted,
                            SUM(CASE WHEN d.Status != 4 AND a.StartDate < @CurrentDate THEN 1 ELSE 0 END) AS stOverdue,
                            MIN(d.Status) AS Status
                        FROM IT_TaskCreation a
                        LEFT JOIN IT_EmployeeRegister e ON a.EmployeeList = e.EmployeeKey
                        LEFT JOIN IT_TaskDescriptiondetails d ON a.TaskKey = d.TaskKey" + where + @"
                        GROUP BY CAST(a.StartDate AS DATE), a.EmployeeList, e.Firstname, e.Lastname, e.Image
                        ORDER BY StartDate DESC";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@CurrentDate", DateTime.Now.Date);
        if (!showAllEmployees)
            cmd.Parameters.AddWithValue("@UserId", filterUserId);
        if (!string.IsNullOrEmpty(projectKey))
            cmd.Parameters.AddWithValue("@ProjectKey", projectKey);
        if (month > 0) cmd.Parameters.AddWithValue("@Month", month);
        if (year > 0)  cmd.Parameters.AddWithValue("@Year", year);

        DataTable dt = DA.GetDataTable(cmd);

        PH_Tasks.Controls.Clear();
        if (dt == null || dt.Rows.Count == 0)
        {
            PH_Tasks.Controls.Add(new Literal { Text = "<tr><td colspan='14' class='text-center'>No records found</td></tr>" });
            return;
        }

        foreach (DataRow dr in dt.Rows)
        {
            string startDate  = dr["StartDate"]  != DBNull.Value ? Convert.ToDateTime(dr["StartDate"]).ToString("dd/MM/yyyy") : "-";
            string assignedTo = dr["AssignedTo"] != DBNull.Value ? dr["AssignedTo"].ToString() : "-";
            string empImage   = dr["EmpImage"]   != DBNull.Value ? dr["EmpImage"].ToString() : "";
            int assignedHours = dr["AssignedHours"] != DBNull.Value ? Convert.ToInt32(dr["AssignedHours"]) : 0;
            int actualHours   = dr["ActualHours"]   != DBNull.Value ? Convert.ToInt32(dr["ActualHours"])   : 0;
            
            int stTotal       = dr["stTotal"]      != DBNull.Value ? Convert.ToInt32(dr["stTotal"])      : 0;
            int stYetToStart  = dr["stYetToStart"] != DBNull.Value ? Convert.ToInt32(dr["stYetToStart"]) : 0;
            int stInProgress  = dr["stInProgress"] != DBNull.Value ? Convert.ToInt32(dr["stInProgress"]) : 0;
            int stCompleted   = dr["stCompleted"]  != DBNull.Value ? Convert.ToInt32(dr["stCompleted"])  : 0;
            int stOverdue     = dr["stOverdue"]    != DBNull.Value ? Convert.ToInt32(dr["stOverdue"])    : 0;

            bool isOverdue = false;
            
            // Overdue: StartDate is more than 7 days ago and not all subtasks completed
            if (dr["StartDate"] != DBNull.Value && Convert.ToDateTime(dr["StartDate"]) < DateTime.Now.Date && stCompleted < stTotal)
            {
                isOverdue = true;
            }

            int taskKey = Convert.ToInt32(dr["TaskKey"]);

            // View button always present
            string viewButton = "<a href='createtasknew.aspx?id=" + taskKey + "&view=1' class='btn btn-xs btn-info' title='View'><i class='glyphicon glyphicon-eye-open'></i></a>";

            string buttonHtml = "";
            if (stTotal > 0 && stCompleted == stTotal)
            {
                buttonHtml = "<button type='button' class='btn btn-xs btn-default' disabled title='Completed task cannot be edited'><i class='glyphicon glyphicon-edit'></i></button>";
            }
            else
            {
                string btnClass = isOverdue ? "btn-danger" : "btn-primary";
                buttonHtml = "<a href='createtasknew.aspx?id=" + taskKey + "' class='btn btn-xs " + btnClass + "'><i class='glyphicon glyphicon-edit'></i></a>";
            }
            
            string createdBy = dr["CreatedBy"] != DBNull.Value ? dr["CreatedBy"].ToString() : "";
            string removeButton = "";
            if (stTotal > 0 && stCompleted > 0)
            {
                removeButton = "<button type='button' class='btn btn-xs btn-default' disabled title='Task with completed subtasks cannot be deleted'><i class='glyphicon glyphicon-trash'></i></button>";
            }
            else if (createdBy == userid)
            {
                removeButton = "<button type='button' class='btn btn-xs btn-danger' onclick='confirmDelete(" + taskKey + ")'><i class='glyphicon glyphicon-trash'></i></button>";
            }
            else
            {
                removeButton = "<button type='button' class='btn btn-xs btn-default' disabled><i class='glyphicon glyphicon-trash'></i></button>";
            }
            
            string rowStyle = isOverdue ? " style='background-color: #ffffff;'" : "";
            
            string row =
                "<tr" + rowStyle + ">" +
                    "<td>" + startDate + "</td>" +
                    "<td>" + BuildAssigneeCell(assignedTo, empImage) + "</td>" +
                    "<td class='text-center'>" + assignedHours + "</td>" +
                    "<td class='text-center'>" + actualHours + "</td>" +
                    "<td class='text-center'>" + stTotal + "</td>" +
                    "<td class='text-center'>" + BuildBadge(stYetToStart, "purple") + "</td>" +
                    "<td class='text-center'>" + BuildBadge(stInProgress, "blue") + "</td>" +
                    "<td class='text-center'>" + BuildBadge(stCompleted, "green") + "</td>" +
                    "<td class='text-center'>" + BuildBadge(stOverdue, "red") + "</td>" +
                    "<td class='action-cell'>" + viewButton + " " + buttonHtml + " " + removeButton + "</td>" +
                "</tr>";
            PH_Tasks.Controls.Add(new Literal { Text = row });
        }
    }
    protected void Filter_Changed(object sender, EventArgs e) 
    { 
        if (divEmployeeFilter.Visible)
        {
            Session["SelectedEmployeeFilter_NewTask"] = ddlEmployee.SelectedValue;
        }
        Session["SelectedMonthFilter_NewTask"] = ddlDate.SelectedValue;
        Session["SelectedYearFilter_NewTask"] = ddlYear.SelectedValue;
        Session["SelectedWidget_NewTask"] = hfActiveStatus.Value;
        
        ScriptManager.RegisterStartupScript(this, this.GetType(), "reinitDT", "initDataTable();", true);
        LoadDashboard(); 
    }

    private bool IsUserTeamLead(string userid, string projectKey)
    {
        if (string.IsNullOrEmpty(userid)) return false;
        
        if (!string.IsNullOrEmpty(projectKey))
        {
            SqlCommand cmdCheckTL = new SqlCommand("SELECT 1 FROM IT_ProjectTeamLeads WHERE ProjectKey = @ProjectKey AND EmployeeKey = @EmpId");
            cmdCheckTL.Parameters.AddWithValue("@ProjectKey", projectKey);
            cmdCheckTL.Parameters.AddWithValue("@EmpId", userid);
            DataTable dtCheckTL = DA.GetDataTable(cmdCheckTL);
            return dtCheckTL != null && dtCheckTL.Rows.Count > 0;
        }
        else
        {
            SqlCommand cmdCheckTL = new SqlCommand("SELECT 1 FROM IT_ProjectTeamLeads WHERE EmployeeKey = @EmpId");
            cmdCheckTL.Parameters.AddWithValue("@EmpId", userid);
            DataTable dtCheckTL = DA.GetDataTable(cmdCheckTL);
            return dtCheckTL != null && dtCheckTL.Rows.Count > 0;
        }
    }
}
