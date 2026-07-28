using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Services;

public partial class Employee_Createtask : System.Web.UI.Page
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
            {
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                    control1.Text = (!string.IsNullOrEmpty(Request.QueryString["view"]) && Request.QueryString["view"] == "1") ? "View Task" : "Edit Task";
                else
                    control1.Text = "Create Task";
            }

            bool hasFullAccess = CheckFullAccess();
            hfHasFullAccess.Value = hasFullAccess ? "1" : "0";
            BindRoles();
            LoadStatusOptions();
            LoadHoursOptions();
            
            // Auto-bind project if coming from newtaskgrids.aspx
            string projectParam = Request.QueryString["project"];
            if (!string.IsNullOrEmpty(projectParam))
            {
                AutoBindProject(projectParam);
            }
            else
            {
                CheckTeamLead(); // Only call if no project param
            }
            LoadStatusOptions();
            
            string idValue = Request.QueryString["id"];
            string backId = Request.QueryString["backid"];
            string viewMode = Request.QueryString["view"];
            
            if (!string.IsNullOrEmpty(idValue))
            {
                int taskKey = 0;
                try
                {
                    taskKey = Convert.ToInt32(idValue);
                }
                catch
                {
                    lblError.Text = "Invalid Task ID.";
                    return;
                }

                hfTaskKey.Value = taskKey.ToString();

                if (!string.IsNullOrEmpty(viewMode) && viewMode == "1")
                {
                    hfViewMode.Value = "1";
                    btnSaveTask.Visible = false;
                    btnUpdateTask.Visible = false;
                    btnAddRow.Visible = false;
                    ddlProject.Enabled = false;
                    ddlEmployee.Enabled = false;
                    
                    txtStartDate.Enabled = false;
                    PopulateTaskData(taskKey);
                }
                else
                {
                    PopulateTaskData(taskKey);
                    btnSaveTask.Visible = false;
                    btnUpdateTask.Visible = true;
                    
                    // In edit mode, these fields must always be read-only
                    ddlProject.Enabled = false;
                    ddlEmployee.Enabled = false;
                    txtStartDate.Enabled = false;
                }
                
                // Set back button URL based on task's project
                SetBackButtonFromTask(taskKey);
            }
            else
            {
                btnSaveTask.Visible = true;
                btnUpdateTask.Visible = false;
                LoadEmptyTaskRow();
                
                // Set Add Row button visibility based on access
                btnAddRow.Visible = CheckCreateTaskAccess();
                
                // Set back button for new task
                if (!string.IsNullOrEmpty(backId))
                {
                    btnBack.HRef = "Viewtask.aspx?id=" + backId;
                }
                else if (!string.IsNullOrEmpty(projectParam))
                {
                    btnBack.HRef = "newtaskgrids.aspx?id=" + Server.UrlEncode(projectParam);
                }
                else
                {
                    btnBack.HRef = "newtaskgrids.aspx";
                }
            }

        }
    }

    private void LoadHoursOptions()
    {
        string sql = "SELECT AH_Id, AH_Hours FROM IT_AssignedHours WHERE AH_IsActive = 1 ORDER BY AH_Hours";
        SqlCommand cmd = new SqlCommand(sql);
        DataTable dt = DA.GetDataTable(cmd);

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("<option value=''>Select</option>");

        foreach (DataRow dr in dt.Rows)
        {
            sb.Append("<option value='" + dr["AH_Hours"].ToString() + "'>" + dr["AH_Hours"].ToString() + "</option>");
        }
        ltHoursOptions.Text = sb.ToString();
    }

    private void LoadStatusOptions()
    {
        bool hasFullAccess = CheckFullAccess();
        string sql = "SELECT StatusID, StatusName FROM IT_StatusMaster WHERE StatusID NOT IN (5,3,6) ORDER BY StatusOrder";
        SqlCommand cmd = new SqlCommand(sql);
        DataTable dt = DA.GetDataTable(cmd);

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("<option value=''>Select</option>");

        foreach (DataRow dr in dt.Rows)
        {
            string sid = dr["StatusID"].ToString();
            string colorClass = GetStatusColorClass(sid);
            string disabledAttr = (!hasFullAccess && sid != "2") ? " disabled='disabled'" : "";
            sb.Append("<option value='" + sid + "' class='" + colorClass + "'" + disabledAttr + ">" + dr["StatusName"].ToString() + "</option>");
        }
        ltStatusOptions.Text = sb.ToString();
    }

    private string GetStatusColorClass(string statusId)
    {
        switch (statusId)
        {
            case "1": return "status-assigned";
            case "2": return "status-ongoing";
            case "4": return "status-completed";
            default:  return "";
        }
    }

    private string GetStatusColorHex(string statusId)
    {
        switch (statusId)
        {
            case "1": return "#17a2b8";
            case "2": return "#2196f3";
            case "4": return "#4caf50";
            default:  return "";
        }
    }

    private void SetBackButtonFromTask(int taskKey)
    {
        try
        {
            string query = "SELECT ProjectName FROM IT_TaskCreation WHERE TaskKey = @TaskKey";
            SqlCommand cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@TaskKey", taskKey);
            DataTable dt = DA.GetDataTable(cmd);
            
            if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["ProjectName"] != DBNull.Value)
            {
                string projectKey = dt.Rows[0]["ProjectName"].ToString();
                btnBack.HRef = "newtaskgrids.aspx?id=" + Server.UrlEncode(projectKey);
            }
            else
            {
                btnBack.HRef = "newtaskgrids.aspx";
            }
        }
        catch
        {
            btnBack.HRef = "newtaskgrids.aspx";
        }
    }

    private void LoadEmptyTaskRow()
    {
        bool hasFullAccess = CheckFullAccess();
        bool isViewMode = hfViewMode.Value == "1";
        string disabledAttr = isViewMode ? "readonly" : "";
        
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(isViewMode ? "<tr class='row-view-mode'>" : "<tr class='row-edit-mode'>");
        sb.Append("<td><textarea class='form-control editable-field' name='task_name' rows='1' placeholder='Enter Task Name' style='resize:vertical;' " + disabledAttr + "></textarea><textarea class='form-control display-field' name='task_name_display' rows='1' style='resize:vertical;background:#f5f5f5;' readonly></textarea></td>");
        sb.Append("<td><textarea class='form-control editable-field' name='task_description' rows='1' " + disabledAttr + "></textarea><textarea class='form-control display-field' rows='1' style='resize:vertical;background:#f5f5f5;' readonly></textarea></td>");
        sb.Append("<td><select class='form-control editable-field' name='task_work_type'>");
        sb.Append(ltWorkTypeOptions.Text);
        sb.Append("</select><span class='display-field'></span></td>");
        sb.Append("<td><select class='form-control editable-field' name='task_assigned_hours' " + ((hasFullAccess && !isViewMode) ? "" : "style='pointer-events:none;opacity:0.6;'") + ">");
        sb.Append(ltHoursOptions.Text);
        sb.Append("</select><span class='display-field'></span></td>");
        sb.Append("<td><input type='number' class='form-control always-on-field' name='task_actual_hours' min='0' step='1' oninput='validateActualHoursField(this)' onchange='validateActualHoursField(this)' onkeyup='validateActualHoursField(this)' />");
        sb.Append("<div class='actual-hours-error' style='color:red; font-size:10px; display:none; font-weight:bold; margin-top:2px;'>Required</div></td>");
        if (isViewMode)
        {
            // View mode: disabled dropdown
            sb.Append("<td><select class='form-control always-on-field status-select' name='task_status' disabled style='pointer-events:none;opacity:1;'>");
            sb.Append(ltStatusOptions.Text);
            sb.Append("</select><input type='hidden' name='task_status' value='' /></td>");
        }
        else
        {
            sb.Append("<td><select class='form-control always-on-field status-select' name='task_status'>");
            sb.Append(ltStatusOptions.Text);
            sb.Append("</select></td>");
        }
        sb.Append("<td><textarea class='form-control always-on-field' name='task_remarks' rows='1' placeholder='Notes' style='resize:vertical;'></textarea>");
        sb.Append("</td>");
        if (!isViewMode)
        {
            sb.Append("<td class='text-center' style='white-space:nowrap;'>");
            sb.Append("<button type='button' class='btn btn-success btn-xs btn-edit-row' onclick='editTaskRow(this)' title='Save Row' style='margin-right:2px;'><i class='glyphicon glyphicon-ok'></i></button>");
            if (hasFullAccess)
                sb.Append("<button type='button' class='btn btn-danger btn-xs' onclick='removeTaskRow(this)' title='Delete Row'><i class='icon-trash'></i></button>");
            else
                sb.Append("<button type='button' class='btn btn-danger btn-xs' disabled style='opacity:0.5; cursor:not-allowed;' title='Delete Row'><i class='icon-trash'></i></button>");
            sb.Append("</td>");
        }
        else
        {
            sb.Append("<td class='text-center' style='white-space:nowrap;'>");
            sb.Append("<button type='button' class='btn btn-success btn-xs' disabled style='margin-right:2px; opacity:0.5; cursor:not-allowed;'><i class='glyphicon glyphicon-ok'></i></button>");
            sb.Append("<button type='button' class='btn btn-danger btn-xs' disabled style='opacity:0.5; cursor:not-allowed;'><i class='icon-trash'></i></button>");
            sb.Append("</td>");
        }
        sb.Append("</tr>");
        ltTaskDetails.Text = sb.ToString();
    }

    private bool CheckCreateTaskAccess()
    {
        string userid = this.SC.Userid;
        if (string.IsNullOrEmpty(userid)) return false;
        
        string projectParam = Request.QueryString["project"];
        if (!string.IsNullOrEmpty(projectParam))
        {
            SqlCommand cmdCheckTL = new SqlCommand("SELECT 1 FROM IT_ProjectTeamLeads WHERE ProjectKey = @ProjectKey AND EmployeeKey = @EmpId");
            cmdCheckTL.Parameters.AddWithValue("@ProjectKey", projectParam);
            cmdCheckTL.Parameters.AddWithValue("@EmpId", userid);
            DataTable dtCheckTL = DA.GetDataTable(cmdCheckTL);
            
            if (dtCheckTL != null && dtCheckTL.Rows.Count > 0)
            {
                return true;
            }
        }
        else
        {
            SqlCommand cmdCheckTL = new SqlCommand("SELECT 1 FROM IT_ProjectTeamLeads WHERE EmployeeKey = @EmpId");
            cmdCheckTL.Parameters.AddWithValue("@EmpId", userid);
            DataTable dtCheckTL = DA.GetDataTable(cmdCheckTL);
            
            if (dtCheckTL != null && dtCheckTL.Rows.Count > 0)
            {
                return true;
            }
        }
        
        return false;
    }

    private bool CheckFullAccess()
    {
        string str_userid = this.SC.Userid;
        string query = @"SELECT Division, Destination FROM IT_EmployeeRegister WHERE Employeekey = @EmpId AND Employeestatus = 1";
        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@EmpId", str_userid);
        DataTable dt = DA.GetDataTable(cmd);

        if (dt.Rows.Count > 0)
        {
            int division = Convert.ToInt32(dt.Rows[0]["Division"]);
            int destination = dt.Rows[0]["Destination"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["Destination"]) : 0;
            return (division == 1 || destination == 24 || destination == 11);
        }
        return false;
    }

    private bool CheckIsProjectTeamLead(int projectKey)
    {
        string userid = this.SC.Userid;
        if (string.IsNullOrEmpty(userid)) return false;

        string qTL = "SELECT 1 FROM IT_ProjectTeamLeads WHERE ProjectKey = @ProjectKey AND EmployeeKey = @UserId";
        SqlCommand cTL = new SqlCommand(qTL);
        cTL.Parameters.AddWithValue("@ProjectKey", projectKey);
        cTL.Parameters.AddWithValue("@UserId", userid);
        DataTable dtTL = DA.GetDataTable(cTL);
        return (dtTL.Rows.Count > 0);
    }

    private void CheckTeamLead()
    {
        string str_userid = this.SC.Userid;
        string projectParam = Request.QueryString["project"];
        bool isProjectAutoBound = !string.IsNullOrEmpty(projectParam);

        string query = @"SELECT Division, Destination FROM IT_EmployeeRegister WHERE Employeekey = @EmpId AND Employeestatus = 1";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@EmpId", str_userid);

        DataTable dt = DA.GetDataTable(cmd);

        if (dt.Rows.Count > 0)
        {
            int division = Convert.ToInt32(dt.Rows[0]["Division"]);
            int destination = dt.Rows[0]["Destination"] != DBNull.Value ? Convert.ToInt32(dt.Rows[0]["Destination"]) : 0;
            
            if (division == 1 || destination == 24 || destination == 11)
            {
                ddlProject.Enabled = !isProjectAutoBound;
                ddlEmployee.Enabled = true;
                
                txtStartDate.Enabled = true;
                btnSaveTask.Enabled = true;
                btnUpdateTask.Enabled = true;
                BindProjects();
                BindEmployeesOnLoad();
            }
            else if (division == 2)
            {
                ddlProject.Enabled = false;
                ddlEmployee.Enabled = false;
                
                txtStartDate.Enabled = false;
                btnSaveTask.Enabled = false;
                btnUpdateTask.Enabled = true;
                BindupdateProjects();
            }
        }
        else if (str_userid == "1987df80-f1a7-4efe-a6bb-af04ad6aa9bd")
        {
            ddlProject.Enabled = false;
            ddlEmployee.Enabled = false;
            
            txtStartDate.Enabled = false;
            btnSaveTask.Enabled = false;
            btnUpdateTask.Enabled = true;
            BindupdateProjects();
        }
        else
        {
            ddlProject.Enabled = false;
            ddlEmployee.Enabled = false;
            
            txtStartDate.Enabled = false;
            btnSaveTask.Enabled = false;
            btnUpdateTask.Enabled = true;
            BindupdateProjects();
        }
    }
    private void BindupdateProjects()
    {
        string sql = "SELECT ProjectKey, ProjectName FROM IT_Projects ORDER BY ProjectName ";
        SqlCommand cmd = new SqlCommand(sql);
        DataSet ds = DA.GetDataSet(cmd);

        if (ds != null && ds.Tables.Count > 0)
        {
            ddlProject.DataSource = ds.Tables[0];
            ddlProject.DataTextField = "ProjectName";
            ddlProject.DataValueField = "ProjectKey";
            ddlProject.DataBind();
        }

        ddlProject.Items.Insert(0, new ListItem(" Select Project ", ""));
    }
    private void BindProjects()
    {
        string sql = @"SELECT ProjectKey, ProjectName FROM IT_Projects 
                       WHERE EXISTS (SELECT 1 FROM IT_ProjectTeamLeads WHERE ProjectKey = IT_Projects.ProjectKey AND EmployeeKey = @UserId)
                       ORDER BY ProjectName";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.AddWithValue("@UserId", this.SC.Userid);
        DataSet ds = DA.GetDataSet(cmd);

        if (ds != null && ds.Tables.Count > 0)
        {
            ddlProject.DataSource = ds.Tables[0];
            ddlProject.DataTextField = "ProjectName";
            ddlProject.DataValueField = "ProjectKey";
            ddlProject.DataBind();
        }

        ddlProject.Items.Insert(0, new ListItem(" Select Project ", ""));
    }

    [WebMethod]
    public static string CheckHours(string employeeKey, string startDate, int hours, int taskKey)
    {
        // 8 hours per day restriction removed
        return null;
    }

    [WebMethod]
    public static string CheckDuplicateTask(int projectKey, string startDate, string employeeKey, int taskKey)
    {
        try
        {
            DateTime dtStart;
            if (!DateTime.TryParseExact(startDate.Trim(),
                new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "d/M/yyyy", "d-M-yyyy", "yyyy/MM/dd", "yyyy-MM-dd" },
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out dtStart))
            {
                return "0";
            }

            Guid empGuid;
            if (!Guid.TryParse(employeeKey, out empGuid))
                return "0";

            string checkExisting = "SELECT COUNT(1) FROM IT_TaskCreation WHERE ProjectName = @ProjectName AND StartDate = @StartDate AND EmployeeList = @EmployeeList AND TaskKey != @TaskKey";
            DataAccess da = new DataAccess();
            SqlCommand cmdCheck = new SqlCommand(checkExisting);
            cmdCheck.Parameters.Add("@ProjectName", SqlDbType.Int).Value = projectKey;
            cmdCheck.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = dtStart.Date;
            cmdCheck.Parameters.Add("@EmployeeList", SqlDbType.UniqueIdentifier).Value = empGuid;
            cmdCheck.Parameters.Add("@TaskKey", SqlDbType.Int).Value = taskKey;
            
            DataTable dt = da.GetDataTable(cmdCheck);
            if(dt != null && dt.Rows.Count > 0)
            {
                if(Convert.ToInt32(dt.Rows[0][0]) > 0)
                {
                    return "1";
                }
            }
        }
        catch { }
        return "0";
    }

    [WebMethod]
    public static List<object> GetEmployees(int projectKey)
    {
        DataAccess DA = new DataAccess();
        SessionCustom SC = new SessionCustom();

        string sql = @"
        SELECT DISTINCT e.EmployeeKey, (e.Firstname + ' ' + e.Lastname) AS EmployeeName, e.Firstname
        FROM IT_EmployeeRegister e
        WHERE e.Employeestatus = 1 
        AND e.Destination IN (11, 12, 23, 24)
        ORDER BY e.Firstname";

        SqlCommand cmd = new SqlCommand(sql);

        DataTable dt = DA.GetDataTable(cmd);
        var result = new List<object>();
        foreach (DataRow row in dt.Rows)
            result.Add(new { Value = row["EmployeeKey"].ToString(), Text = row["EmployeeName"].ToString() });

        return result;
    }

    private void BindTeamLead(int projectKey)
    {
        string sql = @"
        SELECT DISTINCT
            e.EmployeeKey, -- GUID column
            (e.Firstname + ' ' + e.Lastname) AS EmployeeName
        FROM IT_ProjectsParticipants p
        INNER JOIN IT_EmployeeRegister e
            ON e.EmployeeKey = p.TeamLead
        WHERE p.ProjectKey = @ProjectKey
          AND p.TeamLead IS NOT NULL";

        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add("@ProjectKey", SqlDbType.Int).Value = projectKey;

        DataSet ds = DA.GetDataSet(cmd);


    }

    private void BindEmployeesOnLoad()
    {
        string sql = @"
        SELECT DISTINCT
            e.EmployeeKey,
            (e.Firstname + ' ' + e.Lastname) AS EmployeeName,
            e.Firstname
        FROM IT_EmployeeRegister e
        WHERE e.Employeestatus = 1
        AND e.Destination IN (11, 12, 23, 24)
        ORDER BY e.Firstname";

        SqlCommand cmd = new SqlCommand(sql);
        DataSet ds = DA.GetDataSet(cmd);

        ddlEmployee.Items.Clear();

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlEmployee.DataSource = ds.Tables[0];
            ddlEmployee.DataTextField = "EmployeeName";
            ddlEmployee.DataValueField = "EmployeeKey";
            ddlEmployee.DataBind();
        }

        ddlEmployee.Items.Insert(0, new ListItem("Select Employee", ""));
    }

    private void BindEmployees(int projectKey)
    {
        string sql = @"
        SELECT DISTINCT
            e.EmployeeKey,
            (e.Firstname + ' ' + e.Lastname) AS EmployeeName,
            e.Firstname
        FROM IT_EmployeeRegister e
        WHERE e.Employeestatus = 1
        AND e.Destination IN (11, 12, 23, 24)
        ORDER BY e.Firstname";

        SqlCommand cmd = new SqlCommand(sql);

        DataSet ds = DA.GetDataSet(cmd);

        ddlEmployee.Items.Clear();

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlEmployee.DataSource = ds.Tables[0];
            ddlEmployee.DataTextField = "EmployeeName";
            ddlEmployee.DataValueField = "EmployeeKey";
            ddlEmployee.DataBind();
        }

        ddlEmployee.Items.Insert(0, new ListItem(" Select Employee ", ""));
    }

    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(ddlProject.SelectedValue))
        {
            int projectKey = Convert.ToInt32(ddlProject.SelectedValue);
            ddlEmployee.Enabled = true;
            BindTeamLead(projectKey);
            BindEmployees(projectKey);
        }
        else
        {
            ddlEmployee.Enabled = false;
            ddlEmployee.Items.Clear();
        }
    }

    private void BindRoles()
    {
        string sql = "SELECT RoleID, RoleName FROM IT_TaskRole;";
        SqlCommand cmd = new SqlCommand(sql);
        DataSet ds = DA.GetDataSet(cmd);

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<option value=''>Select Work Type</option>");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                sb.Append("<option value='" + dr["RoleID"].ToString() + "'>" + dr["RoleName"].ToString() + "</option>");
            }
            ltWorkTypeOptions.Text = sb.ToString();
        }
    }


    protected void btnSaveTask_Click(object sender, EventArgs e)
    {
        try
        {
            Guid userId = Guid.Parse(SC.Userid.ToString());
            Guid employeeList = Guid.Parse(hfEmployeeKey.Value);

            DateTime startDate;
            if (!DateTime.TryParseExact(txtStartDate.Text.Trim(),
                new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "d/M/yyyy", "d-M-yyyy" },
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out startDate))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_warning", "toastr.warning('Invalid date format. Please select a valid Work Day.');", true);
                return;
            }

            // Check if task already exists for this project, employee and date
            string checkExisting = "SELECT COUNT(1) FROM IT_TaskCreation WHERE ProjectName = @ProjectName AND StartDate = @StartDate AND EmployeeList = @EmployeeList";
            SqlCommand cmdCheck = new SqlCommand(checkExisting, new SqlConnection(DA.ConnectionString));
            cmdCheck.Parameters.Add("@ProjectName", SqlDbType.Int).Value = Convert.ToInt32(ddlProject.SelectedValue);
            cmdCheck.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = startDate.Date;
            cmdCheck.Parameters.Add("@EmployeeList", SqlDbType.UniqueIdentifier).Value = employeeList;
            
            cmdCheck.Connection.Open();
            int existingCount = Convert.ToInt32(cmdCheck.ExecuteScalar());
            cmdCheck.Connection.Close();

            if (existingCount > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_warning", "toastr.warning('A task is already created for this Employee in this Project on the selected date.');", true);
                return;
            }

            // Get task details from table
            string[] taskNames = Request.Form.GetValues("task_name");
            string[] descriptions = Request.Form.GetValues("task_description");
            string[] workTypes = Request.Form.GetValues("task_work_type");
            string[] assignedHours = Request.Form.GetValues("task_assigned_hours");
            string[] actualHours = Request.Form.GetValues("task_actual_hours");
            string[] statuses = Request.Form.GetValues("task_status");
            string[] remarks = Request.Form.GetValues("task_remarks");

            // Insert main task
            string insertMainTask = @"
            INSERT INTO IT_TaskCreation
            (ProjectName, EmployeeList, StartDate, Role, CreatedOn, CreatedBy)
            VALUES
            (@ProjectName, @EmployeeList, @StartDate, @Role, GETDATE(), @CreatedBy);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

            SqlCommand cmdMain = new SqlCommand(insertMainTask);
            cmdMain.Parameters.Add("@ProjectName", SqlDbType.Int).Value = Convert.ToInt32(ddlProject.SelectedValue);
            cmdMain.Parameters.Add("@EmployeeList", SqlDbType.UniqueIdentifier).Value = employeeList;
            cmdMain.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = startDate;
            cmdMain.Parameters.Add("@Role", SqlDbType.Int).Value = 0;
            cmdMain.Parameters.Add("@CreatedBy", SqlDbType.UniqueIdentifier).Value = userId;

            cmdMain.Connection = new SqlConnection(DA.ConnectionString);
            cmdMain.Connection.Open();
            int newTaskKey = Convert.ToInt32(cmdMain.ExecuteScalar());
            cmdMain.Connection.Close();

            // Insert task details
            if (taskNames != null && taskNames.Length > 0)
            {
                for (int i = 0; i < taskNames.Length; i++)
                {
                    int assignedHrs = 0;
                    int actualHrs = 0;
                    int statusVal = 0;
                    int workTypeVal = 0;
                    if (assignedHours != null && i < assignedHours.Length) int.TryParse(assignedHours[i], out assignedHrs);
                    if (actualHours != null && i < actualHours.Length) int.TryParse(actualHours[i], out actualHrs);
                    if (statuses != null && i < statuses.Length) int.TryParse(statuses[i], out statusVal);
                    if (workTypes != null && i < workTypes.Length) int.TryParse(workTypes[i], out workTypeVal);
                    string remarksVal = (remarks != null && i < remarks.Length) ? remarks[i] : "";

                    string insertDetail = @"
                    INSERT INTO IT_TaskDescriptiondetails
                    (TaskKey, TaskName, TaskDescription, WorkType, AssignedHours, ActualHours, Status, Remarks, CreatedOn, CreatedBy)
                    VALUES
                    (@TaskKey, @TaskName, @TaskDescription, @WorkType, @AssignedHours, @ActualHours, @Status, @Remarks, GETDATE(), @CreatedBy)";

                    SqlCommand cmd = new SqlCommand(insertDetail);
                    cmd.Parameters.Add("@TaskKey", SqlDbType.Int).Value = newTaskKey;
                    cmd.Parameters.Add("@TaskName", SqlDbType.NVarChar).Value = taskNames[i];
                    cmd.Parameters.Add("@TaskDescription", SqlDbType.NVarChar).Value = descriptions != null && i < descriptions.Length ? descriptions[i] : "";
                    cmd.Parameters.Add("@WorkType", SqlDbType.Int).Value = workTypeVal;
                    cmd.Parameters.Add("@AssignedHours", SqlDbType.Int).Value = assignedHrs;
                    cmd.Parameters.Add("@ActualHours", SqlDbType.Int).Value = actualHrs;
                    cmd.Parameters.Add("@Status", SqlDbType.Int).Value = statusVal;
                    cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = remarksVal;
                    cmd.Parameters.Add("@CreatedBy", SqlDbType.UniqueIdentifier).Value = userId;

                    DA.ExecuteNonQuery(cmd);
                }
            }

            ScriptManager.RegisterStartupScript(
              this,
              this.GetType(),
              "toastr_redirect",
              "showToastr('success','Task saved successfully!');" +
              "setTimeout(function(){ " + GetRedirectScript() + " }, 2000);",
              true
          );
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(
         this,
         this.GetType(),
         "toastr_error",
         "toastr.error('" + ex.Message.Replace("'", "\\'") + "');",
         true
     );
        }
    }

    protected void btnUpdateTask_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(ddlProject.SelectedValue) ||
            string.IsNullOrEmpty(ddlEmployee.SelectedValue))
        {
            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "toastr_warning",
                "toastr_warning('Please select all required fields!');",
                true
            );
            return;
        }

        Guid employeeList, createdBy;
        if (!Guid.TryParse(hfEmployeeKey.Value, out employeeList))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_warning", "toastr_warning('Invalid Employee GUID!');", true);
            return;
        }
        if (!Guid.TryParse(SC.Userid, out createdBy))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_warning", "toastr_warning('Invalid session user GUID!');", true);
            return;
        }

        int projectKey, taskKey;
        int roleID = 0;
        DateTime startDate;

        if (!int.TryParse(ddlProject.SelectedValue, out projectKey) ||
            !int.TryParse(hfTaskKey.Value, out taskKey) ||
            !DateTime.TryParseExact(txtStartDate.Text.Trim(),
                new string[] { "dd/MM/yyyy", "dd-MM-yyyy", "d/M/yyyy", "d-M-yyyy" },
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None, out startDate))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_warning", "toastr_warning('One or more fields are invalid!');", true);
            return;
        }

        try
        {
            // Check if task already exists for this project, employee and date (excluding current task)
            string checkExisting = "SELECT COUNT(1) FROM IT_TaskCreation WHERE ProjectName = @ProjectName AND StartDate = @StartDate AND EmployeeList = @EmployeeList AND TaskKey != @TaskKey";
            SqlCommand cmdCheck = new SqlCommand(checkExisting, new SqlConnection(DA.ConnectionString));
            cmdCheck.Parameters.Add("@ProjectName", SqlDbType.Int).Value = projectKey;
            cmdCheck.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = startDate.Date;
            cmdCheck.Parameters.Add("@EmployeeList", SqlDbType.UniqueIdentifier).Value = employeeList;
            cmdCheck.Parameters.Add("@TaskKey", SqlDbType.Int).Value = taskKey;
            
            cmdCheck.Connection.Open();
            int existingCount = Convert.ToInt32(cmdCheck.ExecuteScalar());
            cmdCheck.Connection.Close();

            if (existingCount > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_warning", "toastr.warning('A task is already created for this Employee in this Project on the selected date.');", true);
                return;
            }

            // Update main task
            string updateMain = @"
            UPDATE IT_TaskCreation
            SET ProjectName = @ProjectName,
                EmployeeList = @EmployeeList,
                StartDate = @StartDate,
                Role = @Role,
                ModifiedBy = @ModifiedBy,
                ModifiedOn = GETDATE()
            WHERE TaskKey = @TaskKey";

            SqlCommand cmdMain = new SqlCommand(updateMain);
            cmdMain.Parameters.Add("@ProjectName", SqlDbType.Int).Value = projectKey;
            cmdMain.Parameters.Add("@EmployeeList", SqlDbType.UniqueIdentifier).Value = employeeList;
            cmdMain.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = startDate;
            cmdMain.Parameters.Add("@Role", SqlDbType.Int).Value = roleID;
            cmdMain.Parameters.Add("@ModifiedBy", SqlDbType.UniqueIdentifier).Value = createdBy;
            cmdMain.Parameters.Add("@TaskKey", SqlDbType.Int).Value = taskKey;

            DA.ExecuteNonQuery(cmdMain);

            // Delete existing task details
            string deleteDetails = "DELETE FROM IT_TaskDescriptiondetails WHERE TaskKey = @TaskKey";
            SqlCommand cmdDelete = new SqlCommand(deleteDetails);
            cmdDelete.Parameters.Add("@TaskKey", SqlDbType.Int).Value = taskKey;
            DA.ExecuteNonQuery(cmdDelete);

            // Get task details from table
            string[] taskNames = Request.Form.GetValues("task_name");
            string[] descriptions = Request.Form.GetValues("task_description");
            string[] workTypes = Request.Form.GetValues("task_work_type");
            string[] assignedHours = Request.Form.GetValues("task_assigned_hours");
            string[] actualHours = Request.Form.GetValues("task_actual_hours");
            string[] statuses = Request.Form.GetValues("task_status");
            string[] remarks = Request.Form.GetValues("task_remarks");

            // Insert new task details
            if (taskNames != null && taskNames.Length > 0)
            {
                for (int i = 0; i < taskNames.Length; i++)
                {
                    int assignedHrs = 0;
                    int actualHrs = 0;
                    int statusVal = 0;
                    int workTypeVal = 0;
                    if (assignedHours != null && i < assignedHours.Length) int.TryParse(assignedHours[i], out assignedHrs);
                    if (actualHours != null && i < actualHours.Length) int.TryParse(actualHours[i], out actualHrs);
                    if (statuses != null && i < statuses.Length) int.TryParse(statuses[i], out statusVal);
                    if (workTypes != null && i < workTypes.Length) int.TryParse(workTypes[i], out workTypeVal);
                    string remarksVal = (remarks != null && i < remarks.Length) ? remarks[i] : "";

                    string insertDetail = @"
                    INSERT INTO IT_TaskDescriptiondetails
                    (TaskKey, TaskName, TaskDescription, WorkType, AssignedHours, ActualHours, Status, Remarks, CreatedOn, CreatedBy)
                    VALUES
                    (@TaskKey, @TaskName, @TaskDescription, @WorkType, @AssignedHours, @ActualHours, @Status, @Remarks, GETDATE(), @CreatedBy)";

                    SqlCommand cmd = new SqlCommand(insertDetail);
                    cmd.Parameters.Add("@TaskKey", SqlDbType.Int).Value = taskKey;
                    cmd.Parameters.Add("@TaskName", SqlDbType.NVarChar).Value = taskNames[i];
                    cmd.Parameters.Add("@TaskDescription", SqlDbType.NVarChar).Value = descriptions != null && i < descriptions.Length ? descriptions[i] : "";
                    cmd.Parameters.Add("@WorkType", SqlDbType.Int).Value = workTypeVal;
                    cmd.Parameters.Add("@AssignedHours", SqlDbType.Int).Value = assignedHrs;
                    cmd.Parameters.Add("@ActualHours", SqlDbType.Int).Value = actualHrs;
                    cmd.Parameters.Add("@Status", SqlDbType.Int).Value = statusVal;
                    cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar).Value = remarksVal;
                    cmd.Parameters.Add("@CreatedBy", SqlDbType.UniqueIdentifier).Value = createdBy;

                    DA.ExecuteNonQuery(cmd);
                }
            }

            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "toastr_redirect",
                "showToastr('success','Task updated successfully!');" +
                "setTimeout(function(){ " + GetRedirectScript() + " }, 2000);",
                true
            );
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_error", "toastr.error('" + ex.Message.Replace("'", "\\'") + "');", true);
        }
    }

    private void PopulateTaskData(int taskKey)
    {
        string query = @"
    SELECT 
        ProjectName,
        EmployeeList,
        StartDate,
        Role
    FROM IT_TaskCreation
    WHERE TaskKey = @TaskKey";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@TaskKey", taskKey);
        DataSet ds = DA.GetDataSet(cmd);

        if (ds.Tables[0].Rows.Count == 0)
            return;

        DataRow row = ds.Tables[0].Rows[0];

        int projectKey = Convert.ToInt32(row["ProjectName"]);
        string projectStr = projectKey.ToString();
        
        if (ddlProject.Items.FindByValue(projectStr) == null)
        {
            string pQuery = "SELECT ProjectName FROM IT_Projects WHERE ProjectKey = @PKey";
            SqlCommand pCmd = new SqlCommand(pQuery);
            pCmd.Parameters.AddWithValue("@PKey", projectKey);
            DataTable pDt = DA.GetDataTable(pCmd);
            if(pDt.Rows.Count > 0)
            {
                ddlProject.Items.Add(new ListItem(pDt.Rows[0]["ProjectName"].ToString(), projectStr));
            }
        }
        
        ddlProject.SelectedValue = projectStr;

        BindTeamLead(projectKey);
        BindEmployees(projectKey);

        string employee = row["EmployeeList"].ToString();
        if (ddlEmployee.Items.FindByValue(employee) != null)
            ddlEmployee.SelectedValue = employee;
        hfEmployeeKey.Value = employee;



        if (row["StartDate"] != DBNull.Value)
            txtStartDate.Text = Convert.ToDateTime(row["StartDate"]).ToString("dd/MM/yyyy");

        // Load task details from subtable
        string detailQuery = "SELECT TaskDetailID, TaskName, TaskDescription, WorkType, AssignedHours, ActualHours, Status, Remarks FROM IT_TaskDescriptiondetails WHERE TaskKey = @TaskKey";
        SqlCommand cmdDetails = new SqlCommand(detailQuery);
        cmdDetails.Parameters.AddWithValue("@TaskKey", taskKey);
        DataTable dtDetails = DA.GetDataTable(cmdDetails);

        bool hasFullAccess = CheckIsProjectTeamLead(projectKey);
        bool isViewMode = hfViewMode.Value == "1";
        
        if (!isViewMode) {
            btnAddRow.Visible = hasFullAccess;
        }

        string disabledAttr = hasFullAccess ? "" : "readonly";
        
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        
        string sql = "SELECT StatusID, StatusName FROM IT_StatusMaster WHERE StatusID NOT IN (5,3,6) ORDER BY StatusOrder";
        SqlCommand cmdStatus = new SqlCommand(sql);
        DataTable dtStatus = DA.GetDataTable(cmdStatus);

        string hoursQuery = "SELECT AH_Hours FROM IT_AssignedHours WHERE AH_IsActive = 1 ORDER BY AH_Hours";
        SqlCommand cmdHours = new SqlCommand(hoursQuery);
        DataTable dtHours = DA.GetDataTable(cmdHours);
        
        string workTypeQuery = "SELECT RoleID, RoleName FROM IT_TaskRole";
        SqlCommand cmdWorkType = new SqlCommand(workTypeQuery);
        DataTable dtWorkTypes = DA.GetDataTable(cmdWorkType);
        
        foreach (DataRow detailRow in dtDetails.Rows)
        {
            string selectedStatus = detailRow["Status"].ToString();
            string statusColorClass = GetStatusColorClass(selectedStatus);

            sb.Append("<tr class='row-view-mode'>");
            string taskName = detailRow["TaskName"] != DBNull.Value ? detailRow["TaskName"].ToString() : "";
            sb.Append("<td><textarea class='form-control editable-field' name='task_name' rows='1' style='resize:vertical;' " + disabledAttr + ">" + taskName + "</textarea><textarea class='form-control display-field' name='task_name_display' rows='1' style='resize:vertical;background:#f5f5f5;' readonly title='" + taskName.Replace("'", "&#39;") + "'>" + taskName + "</textarea></td>");

            string descVal = detailRow["TaskDescription"].ToString();
            sb.Append("<td><textarea class='form-control editable-field' name='task_description' rows='1' title='" + descVal.Replace("'", "&#39;") + "' " + disabledAttr + ">" + descVal + "</textarea>");
            sb.Append("<textarea class='form-control display-field' rows='1' style='resize:vertical;background:#f5f5f5;' title='" + descVal.Replace("'", "&#39;") + "' readonly>" + descVal + "</textarea></td>");

            string selectedWorkType = detailRow["WorkType"] != DBNull.Value ? detailRow["WorkType"].ToString() : "";
            string workTypeName = "";
            sb.Append("<td><select class='form-control editable-field' name='task_work_type' " + (hasFullAccess ? "" : "style='pointer-events:none;opacity:0.6;'") + ">");
            sb.Append("<option value=''>Select Work Type</option>");
            foreach (DataRow drWt in dtWorkTypes.Rows)
            {
                string sel = drWt["RoleID"].ToString() == selectedWorkType ? "selected='selected'" : "";
                if (sel != "") workTypeName = drWt["RoleName"].ToString();
                sb.Append("<option value='" + drWt["RoleID"].ToString() + "' " + sel + ">" + drWt["RoleName"].ToString() + "</option>");
            }
            sb.Append("</select><span class='display-field' style='font-size:12px;'>" + workTypeName + "</span></td>");

            string selectedHours = detailRow["AssignedHours"] != DBNull.Value ? detailRow["AssignedHours"].ToString() : "";
            string hoursDisabled = hasFullAccess ? "" : "disabled";
            sb.Append("<td><select class='form-control editable-field' name='task_assigned_hours' " + (hasFullAccess ? "" : "style='pointer-events:none;opacity:0.6;'") + ">");
            sb.Append("<option value=''>Select</option>");
            foreach (DataRow drHours in dtHours.Rows)
            {
                string sel = drHours["AH_Hours"].ToString() == selectedHours ? "selected='selected'" : "";
                sb.Append("<option value='" + drHours["AH_Hours"].ToString() + "' " + sel + ">" + drHours["AH_Hours"].ToString() + "</option>");
            }
            sb.Append("</select><span class='display-field' style='font-size:12px;'>" + selectedHours + "</span></td>");

            string actualHours = detailRow["ActualHours"] != DBNull.Value ? detailRow["ActualHours"].ToString() : "";
            sb.Append("<td><input type='number' class='form-control always-on-field' name='task_actual_hours' min='0' step='1' value='" + actualHours + "' oninput='validateActualHoursField(this)' onchange='validateActualHoursField(this)' onkeyup='validateActualHoursField(this)' />");
            sb.Append("<div class='actual-hours-error' style='color:red; font-size:10px; display:none; font-weight:bold; margin-top:2px;'>Required</div></td>");

            // Status - get name for badge
            string statusName = "";
            foreach (DataRow drStatus in dtStatus.Rows)
            {
                if (drStatus["StatusID"].ToString() == selectedStatus)
                { statusName = drStatus["StatusName"].ToString(); break; }
            }
            string badgeStyle = !string.IsNullOrEmpty(statusColorClass)
                ? "display:inline-block; padding:5px 12px; border-radius:4px; font-weight:600; color:white; background-color:" + GetStatusColorHex(selectedStatus) + "; width:100%; text-align:center;"
                : "display:inline-block; width:100%;";

            // Status column: always show dropdown with color
            sb.Append("<td>");
            sb.Append("<select class='form-control status-select always-on-field " + statusColorClass + "' name='task_status'>");
            sb.Append("<option value=''>Select</option>");
            foreach (DataRow drStatus in dtStatus.Rows)
            {
                string sid = drStatus["StatusID"].ToString();
                string sel = sid == selectedStatus ? "selected='selected'" : "";
                string optClass = GetStatusColorClass(sid);
                string statusDisabledAttr = (!hasFullAccess && sid != "2") ? " disabled='disabled'" : "";
                sb.Append("<option value='" + sid + "' " + sel + " class='" + optClass + "'" + statusDisabledAttr + ">" + drStatus["StatusName"].ToString() + "</option>");
            }
            sb.Append("</select>");
            sb.Append("</td>");

            string remarksVal = detailRow["Remarks"] != DBNull.Value ? detailRow["Remarks"].ToString() : "";
            sb.Append("<td><textarea class='form-control always-on-field' name='task_remarks' rows='1' style='resize:vertical;' title='" + remarksVal.Replace("'", "&#39;") + "'>" + remarksVal + "</textarea></td>");
            if (!isViewMode)
            {
                sb.Append("<td class='text-center' style='white-space:nowrap;'>");
                sb.Append("<button type='button' class='btn btn-primary btn-xs btn-edit-row' onclick='editTaskRow(this)' title='Edit Row' style='margin-right:2px;'><i class='glyphicon glyphicon-pencil'></i></button>");
                if (hasFullAccess)
                {
                    if (selectedStatus == "4")
                        sb.Append("<button type='button' class='btn btn-danger btn-xs' disabled style='opacity:0.5; cursor:not-allowed;' title='Completed subtask cannot be deleted'><i class='icon-trash'></i></button>");
                    else
                        sb.Append("<button type='button' class='btn btn-danger btn-xs' onclick='removeTaskRow(this)' title='Delete Row'><i class='icon-trash'></i></button>");
                }
                else
                {
                    sb.Append("<button type='button' class='btn btn-danger btn-xs' disabled style='opacity:0.5; cursor:not-allowed;' title='Delete Row'><i class='icon-trash'></i></button>");
                }
                sb.Append("</td>");
            }
            else
            {
                sb.Append("<td class='text-center' style='white-space:nowrap;'>");
                sb.Append("<button type='button' class='btn btn-primary btn-xs' disabled style='margin-right:2px; opacity:0.5; cursor:not-allowed;'><i class='glyphicon glyphicon-pencil'></i></button>");
                sb.Append("<button type='button' class='btn btn-danger btn-xs' disabled style='opacity:0.5; cursor:not-allowed;'><i class='icon-trash'></i></button>");
                sb.Append("</td>");
            }
            sb.Append("</tr>");
        }
        
        ltTaskDetails.Text = sb.ToString();
    }

    private void AutoBindProject(string projectKey)
    {
        try
        {
            // First bind projects to populate dropdown
            BindProjects();
            
            // Set the project dropdown to the passed project key
            if (ddlProject.Items.FindByValue(projectKey) != null)
            {
                ddlProject.SelectedValue = projectKey;
                ddlProject.Enabled = false; // Make it readonly
                
                // Bind employees
                ddlEmployee.Enabled = true;
                BindEmployeesOnLoad();
            }
        }
        catch (Exception ex)
        {
            // Log error but don't break the page
            lblError.Text = "Error auto-selecting project: " + ex.Message;
        }
    }

   
    private string GetRedirectScript()
    {
        // Check if project parameter exists (new task creation)
        string projectParam = Request.QueryString["project"];
        if (!string.IsNullOrEmpty(projectParam))
        {
            return "window.location.href = '/Employee/newtaskgrids.aspx?id=" + Server.UrlEncode(projectParam) + "';";
        }
        
        // Check if task id exists (edit/update mode) - get project from selected dropdown or DB
        string taskId = Request.QueryString["id"];
        if (!string.IsNullOrEmpty(taskId) && !string.IsNullOrEmpty(ddlProject.SelectedValue))
        {
            return "window.location.href = '/Employee/newtaskgrids.aspx?id=" + Server.UrlEncode(ddlProject.SelectedValue) + "';";
        }
        
        // Default: all projects
        return "window.location.href = '/Employee/newtaskgrids.aspx';";
    }

}
