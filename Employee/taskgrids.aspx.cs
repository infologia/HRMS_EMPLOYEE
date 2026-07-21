using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;

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
            if (Request.QueryString["action"] == "delete" && !string.IsNullOrEmpty(Request.QueryString["taskkey"]))
            {
                DeleteTask(Request.QueryString["taskkey"]);
                return;
            }
            
            hfActiveStatus.Value = "1";
            
            BindMonthYear(ddlDate, ddlYear);
            CheckAndShowEmployeeFilter();
            LoadProjectHeader();
            LoadDashboard();
        }
    }

    protected void CardClick(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        hfActiveStatus.Value = btn.CommandArgument;
        int status = int.Parse(hfActiveStatus.Value);
        
        // Only load grid, skip count reload
        if (status == 3)
            BindOverdueGrid();
        else
            BindGrid(status);
    }

    public string GetStatusTitle()
    {
        int status = int.Parse(hfActiveStatus.Value);
        switch (status)
        {
            case 1: return "Yet to Start";
            case 2: return "In Progress";
            case 3: return "Overdue";
            case 4: return "Completed";
            default: return "Yet to Start";
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
                    string redirectUrl = string.IsNullOrEmpty(projectKey) ? "taskgrids.aspx" : "taskgrids.aspx?id=" + projectKey;
                    Response.Redirect(redirectUrl + "&msg=deleted");
                }
                else
                {
                    Response.Redirect("taskgrids.aspx?msg=unauthorized");
                }
            }
        }
        catch
        {
            Response.Redirect("taskgrids.aspx?msg=error");
        }
    }

    private void LoadProjectHeader()
    {
        string projectKey = Request.QueryString["id"];
        string userid = this.SC.Userid;
        
        // Check if user is Division 1 (Team Lead) or Destination 24/11
        bool showCreateButton = false;
        
        if (!string.IsNullOrEmpty(userid))
        {
            SqlCommand cmdCheck = new SqlCommand("SELECT Division, Destination FROM IT_EmployeeRegister WHERE Employeekey = @EmpId AND Employeestatus = 1");
            cmdCheck.Parameters.AddWithValue("@EmpId", userid);
            DataTable dtCheck = DA.GetDataTable(cmdCheck);
            
            if (dtCheck != null && dtCheck.Rows.Count > 0)
            {
                int userDivision = Convert.ToInt32(dtCheck.Rows[0]["Division"]);
                int destination = dtCheck.Rows[0]["Destination"] != DBNull.Value ? Convert.ToInt32(dtCheck.Rows[0]["Destination"]) : 0;
                
                // Show button for Division 1 OR Destination 11, 23, 24
                showCreateButton = (userDivision == 1) || (destination == 11) || (destination == 23) || (destination == 24);
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
                    lnk_CreateTask.NavigateUrl = "CreateTask.aspx?project=" + Server.UrlEncode(projectKey);
                }
            }
            else
            {
                lbl_ProjectHeader.Text = "Project Not Found";
                if (showCreateButton)
                {
                    lnk_CreateTask.NavigateUrl = "CreateTask.aspx";
                }
            }
        }
        else
        {
            lbl_ProjectHeader.Text = "All Projects";
            if (showCreateButton)
            {
                lnk_CreateTask.NavigateUrl = "CreateTask.aspx";
            }
        }
    }

    private void CheckAndShowEmployeeFilter()
    {
        string str_userid = this.SC.Userid;
        string checkQuery = @"SELECT Division FROM IT_EmployeeRegister WHERE Employeekey = @EmpId AND Employeestatus = 1";
        
        SqlCommand cmd = new SqlCommand(checkQuery);
        cmd.Parameters.AddWithValue("@EmpId", str_userid);
        
        DataTable dt = DA.GetDataTable(cmd);
        
        if (dt != null && dt.Rows.Count > 0)
        {
            int division = Convert.ToInt32(dt.Rows[0]["Division"]);
            
            if (division == 1)
            {
                divEmployeeFilter.Visible = true;
                BindEmployeeDropdown();
            }
        }
    }

    private void BindEmployeeDropdown()
    {
        string query = @"SELECT EmployeeKey, (Firstname + ' ' + Lastname) AS EmployeeName 
                        FROM IT_EmployeeRegister 
                        WHERE Employeestatus = 1 AND Destination IN (11, 12, 23, 24)
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
    }

    private void BindMonthYear(DropDownList ddlMonth, DropDownList ddlYr)
    {
        ddlMonth.Items.Clear();
        ddlMonth.Items.Add(new ListItem("All", "0"));
        for (int m = 1; m <= 12; m++)
            ddlMonth.Items.Add(new ListItem(new DateTime(2025, m, 1).ToString("MMMM"), (m + 1).ToString()));
        ddlMonth.SelectedValue = (DateTime.Now.Month + 1).ToString();

        ddlYr.Items.Clear();
        int currentYear = DateTime.Now.Year;
        for (int y = currentYear - 5; y <= currentYear + 5; y++)
            ddlYr.Items.Add(new ListItem(y.ToString(), y.ToString()));
        ddlYr.SelectedValue = currentYear.ToString();
    }

    private void LoadDashboard()
    {
        // Load counts for all statuses
        LoadCount(lbl_YetToStartCount, 1);
        LoadCount(lbl_InProgressCount, 2);
        LoadOverdueCount(lbl_OverDueCount);
        LoadCount(lbl_CompletedCount, 4);
        
        // Load grid based on active status
        int status = int.Parse(hfActiveStatus.Value);
        if (status == 3)
            BindOverdueGrid();
        else
            BindGrid(status);
    }

    private void LoadCount(Label lblCard, int statusId)
    {
        string userid = this.SC.Userid;
        if (string.IsNullOrEmpty(userid)) return;

        int monthVal = int.Parse(ddlDate.SelectedValue);
        int month = (monthVal >= 2) ? monthVal - 1 : 0;
        int year = int.Parse(ddlYear.SelectedValue);

        SqlCommand cmdTL = new SqlCommand("SELECT 1 FROM IT_EmployeeRegister WHERE Employeekey = @EmpId AND Division = 1 AND Employeestatus = 1");
        cmdTL.Parameters.AddWithValue("@EmpId", userid);
        DataTable dtTL = DA.GetDataTable(cmdTL);
        bool isTeamLead = dtTL != null && dtTL.Rows.Count > 0;

        bool showAllEmployees = isTeamLead && divEmployeeFilter.Visible && ddlEmployee.SelectedValue == "0";
        string filterUserId = userid;
        if (isTeamLead && divEmployeeFilter.Visible && ddlEmployee.SelectedValue != "0")
            filterUserId = ddlEmployee.SelectedValue;

        string where = " WHERE a.Status = @StatusId";
        if (!showAllEmployees)
            where += " AND a.EmployeeList = @UserId";
        
        string projectKey = Request.QueryString["id"];
        if (!string.IsNullOrEmpty(projectKey))
            where += " AND a.ProjectName = @ProjectKey";
        if (month > 0) where += " AND MONTH(a.StartDate) = @Month";
        if (year > 0)  where += " AND YEAR(a.StartDate) = @Year";

        string query = "SELECT COUNT(*) FROM IT_TaskCreation a" + where;

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

        SqlCommand cmdTL = new SqlCommand("SELECT 1 FROM IT_EmployeeRegister WHERE Employeekey = @EmpId AND Division = 1 AND Employeestatus = 1");
        cmdTL.Parameters.AddWithValue("@EmpId", userid);
        DataTable dtTL = DA.GetDataTable(cmdTL);
        bool isTeamLead = dtTL != null && dtTL.Rows.Count > 0;

        bool showAllEmployees = isTeamLead && divEmployeeFilter.Visible && ddlEmployee.SelectedValue == "0";
        string filterUserId = userid;
        if (isTeamLead && divEmployeeFilter.Visible && ddlEmployee.SelectedValue != "0")
            filterUserId = ddlEmployee.SelectedValue;

        string where = " WHERE a.EndDate < @CurrentDate AND a.Status != 4";
        if (!showAllEmployees)
            where += " AND a.EmployeeList = @UserId";
        
        string projectKey = Request.QueryString["id"];
        if (!string.IsNullOrEmpty(projectKey))
            where += " AND a.ProjectName = @ProjectKey";
        if (month > 0) where += " AND MONTH(a.StartDate) = @Month";
        if (year > 0)  where += " AND YEAR(a.StartDate) = @Year";

        string query = "SELECT COUNT(*) FROM IT_TaskCreation a" + where;

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

    private void BindGrid(int statusId)
    {
        string userid = this.SC.Userid;
        if (string.IsNullOrEmpty(userid)) return;

        int monthVal = int.Parse(ddlDate.SelectedValue);
        int month = (monthVal >= 2) ? monthVal - 1 : 0;
        int year = int.Parse(ddlYear.SelectedValue);

        SqlCommand cmdTL = new SqlCommand("SELECT 1 FROM IT_EmployeeRegister WHERE Employeekey = @EmpId AND Division = 1 AND Employeestatus = 1");
        cmdTL.Parameters.AddWithValue("@EmpId", userid);
        DataTable dtTL = DA.GetDataTable(cmdTL);
        bool isTeamLead = dtTL != null && dtTL.Rows.Count > 0;

        bool showAllEmployees = isTeamLead && divEmployeeFilter.Visible && ddlEmployee.SelectedValue == "0";
        string filterUserId = userid;
        if (isTeamLead && divEmployeeFilter.Visible && ddlEmployee.SelectedValue != "0")
            filterUserId = ddlEmployee.SelectedValue;

        string where = " WHERE a.Status = @StatusId";
        if (!showAllEmployees)
            where += " AND a.EmployeeList = @UserId";
        
        string projectKey = Request.QueryString["id"];
        if (!string.IsNullOrEmpty(projectKey))
            where += " AND a.ProjectName = @ProjectKey";
        if (month > 0) where += " AND MONTH(a.StartDate) = @Month";
        if (year > 0)  where += " AND YEAR(a.StartDate) = @Year";

        string query = @"SELECT a.TaskKey, a.TaskName, a.TaskDescription, a.StartDate, a.EndDate, a.AssignedHours, a.ActualHours, 
                        (a.ActualHours - a.AssignedHours) AS OvertimeHours, a.Status, a.CreatedBy,
                        (e.Firstname + ' ' + e.Lastname) AS AssignedTo
                        FROM IT_TaskCreation a
                        LEFT JOIN IT_EmployeeRegister e ON a.EmployeeList = e.EmployeeKey" + where + " ORDER BY a.StartDate DESC";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@StatusId", statusId);
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
            PH_Tasks.Controls.Add(new Literal { Text = "<tr><td colspan='9' class='text-center'>No records found</td></tr>" });
            return;
        }

        foreach (DataRow dr in dt.Rows)
        {
            string startDate = dr["StartDate"] != DBNull.Value ? Convert.ToDateTime(dr["StartDate"]).ToString("dd/MM/yyyy") : "-";
            string endDate   = dr["EndDate"]   != DBNull.Value ? Convert.ToDateTime(dr["EndDate"]).ToString("dd/MM/yyyy")   : "-";
            string assignedTo = dr["AssignedTo"] != DBNull.Value ? dr["AssignedTo"].ToString() : "-";
            
            string overtimeHours = "N/A";
            if (dr["ActualHours"] != DBNull.Value && Convert.ToInt32(dr["ActualHours"]) > 0)
            {
                overtimeHours = dr["OvertimeHours"].ToString();
            }
            
            string statusText = "";
            int status = Convert.ToInt32(dr["Status"]);
            switch(status)
            {
                case 1: statusText = "<span class='label label-warning'>Yet to Start</span>"; break;
                case 2: statusText = "<span class='label label-info'>In Progress</span>"; break;
                case 4: statusText = "<span class='label label-success'>Completed</span>"; break;
                default: statusText = "<span class='label label-default'>Unknown</span>"; break;
            }
            
            string buttonHtml = "";
            if (statusId == 4)
            {
                buttonHtml = "<a href='CreateTask.aspx?id=" + dr["TaskKey"] + "&view=1' class='btn btn-xs btn-success'>View</a>";
            }
            else
            {
                buttonHtml = "<a href='CreateTask.aspx?id=" + dr["TaskKey"] + "' class='btn btn-xs btn-primary'>Update</a>";
            }
            
            string createdBy = dr["CreatedBy"] != DBNull.Value ? dr["CreatedBy"].ToString() : "";
            string removeButton = "";
            if (statusId == 4)
            {
                removeButton = "<button type='button' class='btn btn-xs btn-default' disabled>Remove</button>";
            }
            else if (createdBy == userid)
            {
                removeButton = "<button type='button' class='btn btn-xs btn-danger' onclick='confirmDelete(" + dr["TaskKey"] + ")'>Remove</button>";
            }
            else
            {
                removeButton = "<button type='button' class='btn btn-xs btn-default' disabled>Remove</button>";
            }
            
            string row =
                "<tr>" +
                    "<td><span title='" + dr["TaskDescription"].ToString().Replace("'", "&apos;") + "'>" + dr["TaskName"] + "</span></td>" +
                    "<td>" + assignedTo + "</td>" +
                    "<td>" + startDate + "</td>" +
                    "<td>" + endDate + "</td>" +
                    "<td>" + dr["AssignedHours"] + "</td>" +
                    "<td>" + overtimeHours + "</td>" +
                    "<td>" + statusText + "</td>" +
                    "<td>" + buttonHtml + "</td>" +
                    "<td>" + removeButton + "</td>" +
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

        SqlCommand cmdTL = new SqlCommand("SELECT 1 FROM IT_EmployeeRegister WHERE Employeekey = @EmpId AND Division = 1 AND Employeestatus = 1");
        cmdTL.Parameters.AddWithValue("@EmpId", userid);
        DataTable dtTL = DA.GetDataTable(cmdTL);
        bool isTeamLead = dtTL != null && dtTL.Rows.Count > 0;

        bool showAllEmployees = isTeamLead && divEmployeeFilter.Visible && ddlEmployee.SelectedValue == "0";
        string filterUserId = userid;
        if (isTeamLead && divEmployeeFilter.Visible && ddlEmployee.SelectedValue != "0")
            filterUserId = ddlEmployee.SelectedValue;

        string where = " WHERE a.EndDate < @CurrentDate AND a.Status != 4";
        if (!showAllEmployees)
            where += " AND a.EmployeeList = @UserId";
        
        string projectKey = Request.QueryString["id"];
        if (!string.IsNullOrEmpty(projectKey))
            where += " AND a.ProjectName = @ProjectKey";
        if (month > 0) where += " AND MONTH(a.StartDate) = @Month";
        if (year > 0)  where += " AND YEAR(a.StartDate) = @Year";

        string query = @"SELECT a.TaskKey, a.TaskName, a.TaskDescription, a.StartDate, a.EndDate, a.AssignedHours, a.ActualHours, 
                        (a.ActualHours - a.AssignedHours) AS OvertimeHours, a.Status, a.CreatedBy,
                        (e.Firstname + ' ' + e.Lastname) AS AssignedTo
                        FROM IT_TaskCreation a
                        LEFT JOIN IT_EmployeeRegister e ON a.EmployeeList = e.EmployeeKey" + where + " ORDER BY a.EndDate DESC";

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
            PH_Tasks.Controls.Add(new Literal { Text = "<tr><td colspan='9' class='text-center'>No overdue tasks found</td></tr>" });
            return;
        }

        foreach (DataRow dr in dt.Rows)
        {
            string startDate = dr["StartDate"] != DBNull.Value ? Convert.ToDateTime(dr["StartDate"]).ToString("dd/MM/yyyy") : "-";
            string endDate   = dr["EndDate"]   != DBNull.Value ? Convert.ToDateTime(dr["EndDate"]).ToString("dd/MM/yyyy")   : "-";
            string assignedTo = dr["AssignedTo"] != DBNull.Value ? dr["AssignedTo"].ToString() : "-";
            
            string overtimeHours = "N/A";
            if (dr["ActualHours"] != DBNull.Value && Convert.ToInt32(dr["ActualHours"]) > 0)
            {
                overtimeHours = dr["OvertimeHours"].ToString();
            }
            
            string statusText = "<span class='label label-danger'>Overdue</span>";
            
            string createdBy = dr["CreatedBy"] != DBNull.Value ? dr["CreatedBy"].ToString() : "";
            string removeButton = "";
            if (createdBy == userid)
            {
                removeButton = "<button type='button' class='btn btn-xs btn-danger' onclick='confirmDelete(" + dr["TaskKey"] + ")'>Remove</button>";
            }
            else
            {
                removeButton = "<button type='button' class='btn btn-xs btn-default' disabled>Remove</button>";
            }
            
            string row =
                "<tr style='background-color: #ffebee;'>" +
                    "<td><span title='" + dr["TaskDescription"].ToString().Replace("'", "&apos;") + "'>" + dr["TaskName"] + "</span></td>" +
                    "<td>" + assignedTo + "</td>" +
                    "<td>" + startDate + "</td>" +
                    "<td style='color: #d32f2f; font-weight: bold;'>" + endDate + "</td>" +
                    "<td>" + dr["AssignedHours"] + "</td>" +
                    "<td>" + overtimeHours + "</td>" +
                    "<td>" + statusText + "</td>" +
                    "<td><a href='CreateTask.aspx?id=" + dr["TaskKey"] + "' class='btn btn-xs btn-danger'>Update</a></td>" +
                    "<td>" + removeButton + "</td>" +
                "</tr>";
            PH_Tasks.Controls.Add(new Literal { Text = row });
        }
    }
    protected void Filter_Changed(object sender, EventArgs e) { LoadDashboard(); }
}
