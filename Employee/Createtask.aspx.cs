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
            BindRoles();
            BindStatus();
            
            // Auto-bind project if coming from taskgrids.aspx
            string projectParam = Request.QueryString["project"];
            if (!string.IsNullOrEmpty(projectParam))
            {
                AutoBindProject(projectParam);
            }
            else
            {
                CheckTeamLead(); // Only call if no project param
            }
            
            string idValue = Request.QueryString["id"];
            string backId = Request.QueryString["backid"];
            string viewMode = Request.QueryString["view"];
            
            if (!string.IsNullOrEmpty(backId))
            {
                btnBack.HRef = "Viewtask.aspx?id=" + backId;
            }
            else if (!string.IsNullOrEmpty(projectParam))
            {
                btnBack.HRef = "taskgrids.aspx?id=" + Server.UrlEncode(projectParam);
            }
            else
            {
                // If no project param, try to get from selected project dropdown
                btnBack.HRef = "taskgrids.aspx";
            }
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

                PopulateTaskData(taskKey);

                // Check if view mode (from completed grid)
                if (!string.IsNullOrEmpty(viewMode) && viewMode == "1")
                {
                    // View only mode - hide all buttons
                    btnSaveTask.Visible = false;
                    btnUpdateTask.Visible = false;
                    
                    // Disable all fields
                    txtTaskName.Enabled = false;
                    ddlProject.Enabled = false;
                    ddlEmployee.Enabled = false;
                    ddlRole.Enabled = false;
                    txtStartDate.Enabled = false;
                    txtEndDate.Enabled = false;
                    txtHours.Enabled = false;
                    txtActualHours.Enabled = false;
                    ddlTaskStatus.Enabled = false;
                    txtTaskDescription.Enabled = false;
                }
                else
                {
                    btnSaveTask.Visible = false;
                    btnUpdateTask.Visible = true;
                }
            }
            else
            {
                btnSaveTask.Visible = true;
                btnUpdateTask.Visible = false;
            }

        }
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
            
            if (division == 1 || destination == 24 || destination == 11) // Division 1 or Destination 24/11
            {
                // Enable all fields
                txtTaskName.Enabled = true;
                ddlProject.Enabled = !isProjectAutoBound; // Readonly if auto-bound
                ddlEmployee.Enabled = true;
                ddlRole.Enabled = true;
                txtStartDate.Enabled = true;
                txtEndDate.Enabled = true;
                txtHours.Enabled = true;
                txtActualHours.Enabled = true;
                ddlTaskStatus.Enabled = true;
                txtTaskDescription.Enabled = true;
                btnSaveTask.Enabled = true;
                btnUpdateTask.Enabled = true;
                BindProjects();
                BindEmployeesOnLoad();
            }
            else if (division == 2) // Division 2 - Regular Employee
            {
                // Disable all fields except actual hours and status
                txtTaskName.Enabled = false;
                ddlProject.Enabled = false;
                ddlEmployee.Enabled = false;
                ddlRole.Enabled = false;
                txtStartDate.Enabled = false;
                txtEndDate.Enabled = false;
                txtHours.Enabled = false;
                txtActualHours.Enabled = true;
                ddlTaskStatus.Enabled = true;
                txtTaskDescription.Enabled = false;
                btnSaveTask.Enabled = false;
                btnUpdateTask.Enabled = true;
                BindupdateProjects();
            }
        }
        else if (str_userid == "1987df80-f1a7-4efe-a6bb-af04ad6aa9bd")
        {
            // Special user - all disabled
            txtTaskName.Enabled = false;
            ddlProject.Enabled = false;
            ddlEmployee.Enabled = false;
            ddlRole.Enabled = false;
            txtStartDate.Enabled = false;
            txtEndDate.Enabled = false;
            txtHours.Enabled = false;
            txtActualHours.Enabled = false;
            ddlTaskStatus.Enabled = false;
            txtTaskDescription.Enabled = false;
            btnSaveTask.Enabled = false;
            btnUpdateTask.Enabled = false;
            BindupdateProjects();
        }
        else
        {
            // Default - all disabled
            txtTaskName.Enabled = false;
            ddlProject.Enabled = false;
            ddlEmployee.Enabled = false;
            ddlRole.Enabled = false;
            txtStartDate.Enabled = false;
            txtEndDate.Enabled = false;
            txtHours.Enabled = false;
            txtActualHours.Enabled = false;
            ddlTaskStatus.Enabled = false;
            txtTaskDescription.Enabled = false;
            btnSaveTask.Enabled = false;
            btnUpdateTask.Enabled = false;
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

        ddlProject.Items.Insert(0, new ListItem("-- Select Project --", ""));
    }
    private void BindProjects()
    {
        string sql = @"SELECT ProjectKey, ProjectName FROM IT_Projects 
                       WHERE leadby = @UserId 
                       OR EXISTS (SELECT 1 FROM IT_ProjectsParticipants WHERE ProjectKey = IT_Projects.ProjectKey AND EmployeeKey = @UserId)
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

        ddlProject.Items.Insert(0, new ListItem("-- Select Project --", ""));
    }

    [WebMethod]
    public static string CheckHours(string employeeKey, string startDate, int hours, int taskKey)
    {
        // 8 hours per day restriction removed
        return null;
        
        //DataAccess DA = new DataAccess();
        //Guid empId;
        //DateTime date;
        //if (!Guid.TryParse(employeeKey, out empId)) return null;
        //if (!DateTime.TryParseExact(startDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date)) return null;

        //SqlCommand cmd = new SqlCommand(@"
        //SELECT @TotalHours = ISNULL(SUM(AssignedHours), 0)
        //FROM IT_TaskCreation
        //WHERE EmployeeList = @EmployeeKey
        //  AND CAST(StartDate AS DATE) = @TaskDate
        //  AND (@TaskKey = 0 OR TaskKey <> @TaskKey)");
        //cmd.Parameters.Add("@EmployeeKey", SqlDbType.UniqueIdentifier).Value = empId;
        //cmd.Parameters.Add("@TaskDate", SqlDbType.Date).Value = date;
        //cmd.Parameters.Add("@TaskKey", SqlDbType.Int).Value = taskKey;
        //SqlParameter outParam = new SqlParameter("@TotalHours", SqlDbType.Int) { Direction = ParameterDirection.Output };
        //cmd.Parameters.Add(outParam);
        //DA.ExecuteNonQuery(cmd);

        //int existing = Convert.ToInt32(outParam.Value);
        //if (existing + hours > 8)
        //    return "Already Assigned: " + existing + " hrs | Entered: " + hours + " hrs | Max: 8 hrs/day";
        //return null;
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

        ddlEmployee.Items.Insert(0, new ListItem("-- Select Employee --", ""));
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

        ddlEmployee.Items.Insert(0, new ListItem("-- Select Employee --", ""));
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
        string sql = "SELECT RoleID, RoleName FROM IT_TaskRole WHERE RoleID NOT IN (3) ORDER BY RoleOrder;";
        SqlCommand cmd = new SqlCommand(sql);
        DataSet ds = DA.GetDataSet(cmd);

        ddlRole.Items.Clear();

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlRole.DataSource = ds.Tables[0];
            ddlRole.DataTextField = "RoleName";   // user sees
            ddlRole.DataValueField = "RoleID";    // INT to save
            ddlRole.DataBind();
        }

        ddlRole.Items.Insert(0, new ListItem("-- Select Role --", "0"));
    }
    private void BindStatus()
    {
        string sql = "SELECT StatusID, StatusName FROM IT_StatusMaster WHERE StatusID NOT IN (5) ORDER BY StatusOrder";
        SqlCommand cmd = new SqlCommand(sql);
        DataSet ds = DA.GetDataSet(cmd);

        ddlTaskStatus.Items.Clear();

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlTaskStatus.DataSource = ds.Tables[0];
            ddlTaskStatus.DataTextField = "StatusName"; // user sees
            ddlTaskStatus.DataValueField = "StatusID";  // INT to save
            ddlTaskStatus.DataBind();
        }

        ddlTaskStatus.Items.Insert(0, new ListItem("-- Select Status --", "0"));
    }


    protected void btnSaveTask_Click(object sender, EventArgs e)
    {
        try
        {
            // Clear previous error
            lblActualHoursError.Visible = false;
            
            int statusID = Convert.ToInt32(ddlTaskStatus.SelectedValue);
            
            // Status = 4 (Completed) - Actual Hours mandatory
            if (statusID == 4)
            {
                string actualHoursValue = txtActualHours.Text.Trim();
                if (string.IsNullOrEmpty(actualHoursValue) || actualHoursValue == "0")
                {
                    lblActualHoursError.Text = "Actual Hours is required for Completed status";
                    lblActualHoursError.Visible = true;
                    txtActualHours.Focus();
                    return;
                }
            }
            
            Guid userId = Guid.Parse(SC.Userid.ToString());
            Guid employeeList = Guid.Parse(hfEmployeeKey.Value);

            DateTime startDate = DateTime.ParseExact(
                txtStartDate.Text.Trim(),
                "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture
            );

            // Get EndDate from hidden field since txtEndDate is disabled
            string endDateValue = string.IsNullOrEmpty(txtEndDate.Text) ? hfEndDate.Value : txtEndDate.Text;
            DateTime endDate = DateTime.ParseExact(
                endDateValue.Trim(),
                "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture
            );


            int assignedHours = int.Parse(txtHours.Text.Trim());
            // 8 hours per day restriction removed
            //if (!CheckAssignedHours(employeeList, startDate, assignedHours, 0))
            //{
            //    return;
            //}

            int actualHours = 0;
            if (!string.IsNullOrEmpty(txtActualHours.Text.Trim()))
            {
                actualHours = int.Parse(txtActualHours.Text.Trim());
            }

            string taskDescription = txtTaskDescription.Text.Trim();

            string sql = @"
        INSERT INTO IT_TaskCreation
        (ProjectName, TaskName, TaskDescription, EmployeeList, StartDate, EndDate, AssignedHours, ActualHours, Role, CreatedOn, CreatedBy, Status)
        VALUES
        (@ProjectName, @TaskName, @TaskDescription, @EmployeeList, @StartDate, @EndDate, @AssignedHours, @ActualHours, @Role, GETDATE(), @CreatedBy, @Status)";

            SqlCommand cmd = new SqlCommand(sql);

            cmd.Parameters.Add("@ProjectName", SqlDbType.Int).Value = Convert.ToInt32(ddlProject.SelectedValue);
            cmd.Parameters.Add("@TaskName", SqlDbType.NVarChar).Value = txtTaskName.Text.Trim();
            cmd.Parameters.Add("@TaskDescription", SqlDbType.NVarChar).Value = taskDescription;
            cmd.Parameters.Add("@EmployeeList", SqlDbType.UniqueIdentifier).Value = employeeList;
            cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = startDate;
            cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = endDate;
            cmd.Parameters.Add("@AssignedHours", SqlDbType.Int).Value = assignedHours;
            cmd.Parameters.Add("@ActualHours", SqlDbType.Int).Value = actualHours;
            cmd.Parameters.Add("@Role", SqlDbType.Int).Value = Convert.ToInt32(ddlRole.SelectedValue);
            cmd.Parameters.Add("@CreatedBy", SqlDbType.UniqueIdentifier).Value = userId;
            cmd.Parameters.Add("@Status", SqlDbType.Int).Value = Convert.ToInt32(ddlTaskStatus.SelectedValue);

            DA.ExecuteNonQuery(cmd);


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
        // Clear previous error
        lblActualHoursError.Visible = false;
        
        // Validate selections
        if (string.IsNullOrEmpty(ddlProject.SelectedValue) ||

            string.IsNullOrEmpty(ddlEmployee.SelectedValue) ||
            ddlRole.SelectedValue == "0" ||
            ddlTaskStatus.SelectedValue == "0")
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

        int statusID = Convert.ToInt32(ddlTaskStatus.SelectedValue);
        
        // Status = 4 (Completed) - Actual Hours mandatory
        if (statusID == 4)
        {
            string actualHoursValue = txtActualHours.Text.Trim();
            if (string.IsNullOrEmpty(actualHoursValue) || actualHoursValue == "0")
            {
                lblActualHoursError.Text = "Actual Hours is required for Completed status";
                lblActualHoursError.Visible = true;
                txtActualHours.Focus();
                return;
            }
        }

        // Parse GUIDs safely
        Guid employeeList, createdBy;

        if (!Guid.TryParse(hfEmployeeKey.Value, out employeeList))
        {
            ScriptManager.RegisterStartupScript(
    this,
    this.GetType(),
    "toastr_warning",
    "toastr_warning('Invalid Employee GUID!');",
    true
);

            return;
        }
        if (!Guid.TryParse(SC.Userid, out createdBy))
        {
            ScriptManager.RegisterStartupScript(
  this,
  this.GetType(),
  "toastr_warning",
  "toastr_warning('Invalid session user GUID!');",
  true
);


            return;
        }

        // Parse other fields
        int projectKey, roleID, statusIDForUpdate, taskKey, assignedHours;
        DateTime startDate, endDate;

        if (!int.TryParse(ddlProject.SelectedValue, out projectKey) ||
            !int.TryParse(ddlRole.SelectedValue, out roleID) ||
            !int.TryParse(ddlTaskStatus.SelectedValue, out statusIDForUpdate) ||
            !int.TryParse(hfTaskKey.Value, out taskKey) ||
            !int.TryParse(txtHours.Text, out assignedHours) ||
           
            !DateTime.TryParseExact(
                   txtStartDate.Text.Trim(),
                   "dd/MM/yyyy",
                   System.Globalization.CultureInfo.InvariantCulture,
                   System.Globalization.DateTimeStyles.None,
                   out startDate
            ))


        {
            ScriptManager.RegisterStartupScript(
this,
this.GetType(),
"toastr_warning",
"toastr_warning('One or more fields are invalid!');",
true
);

            return;
        }

        // Get EndDate from hidden field since txtEndDate is disabled
        string endDateValue = string.IsNullOrEmpty(txtEndDate.Text) ? hfEndDate.Value : txtEndDate.Text;
        if (!DateTime.TryParseExact(
                   endDateValue.Trim(),
                   "dd/MM/yyyy",
                   System.Globalization.CultureInfo.InvariantCulture,
                   System.Globalization.DateTimeStyles.None,
                   out endDate
            ))
        {
            ScriptManager.RegisterStartupScript(
this,
this.GetType(),
"toastr_warning",
"toastr_warning('Invalid End Date!');",
true
);
            return;
        }

        string taskDescription = txtTaskDescription.Text.Trim();
        // 8 hours per day restriction removed
        //if (!CheckAssignedHours(employeeList, startDate, assignedHours, taskKey))
        //{
        //    ScriptManager.RegisterStartupScript(
        //        this,
        //        this.GetType(),
        //        "toastr_block",
        //        "toastr.warning('Cannot update. Assigned hours exceed 8 for this employee on selected date!');",
        //        true
        //    );
        //    return;
        //}

        int actualHours = 0;
        if (!string.IsNullOrEmpty(txtActualHours.Text.Trim()))
        {
            actualHours = int.Parse(txtActualHours.Text.Trim());
        }

        try
        {
            string sql = @"
            UPDATE IT_TaskCreation
            SET ProjectName     = @ProjectName,
                TaskName        = @TaskName,
                TaskDescription = @TaskDescription,
                EmployeeList    = @EmployeeList,
                StartDate       = @StartDate,
                EndDate         = @EndDate,
                AssignedHours   = @AssignedHours,
                ActualHours     = @ActualHours,
                Role            = @Role,
                Status          = @Status,
                ModifiedBy      = @ModifiedBy,
                ModifiedOn      = GETDATE()
            WHERE TaskKey = @TaskKey";

            SqlCommand cmd = new SqlCommand(sql);

            cmd.Parameters.Add("@ProjectName", SqlDbType.Int).Value = projectKey;
            cmd.Parameters.Add("@TaskName", SqlDbType.NVarChar).Value = txtTaskName.Text.Trim();
            cmd.Parameters.Add("@TaskDescription", SqlDbType.NVarChar).Value = taskDescription;
            cmd.Parameters.Add("@EmployeeList", SqlDbType.UniqueIdentifier).Value = employeeList;
            cmd.Parameters.Add("@StartDate", SqlDbType.Date).Value = startDate;
            cmd.Parameters.Add("@EndDate", SqlDbType.Date).Value = endDate;
            cmd.Parameters.Add("@AssignedHours", SqlDbType.Int).Value = assignedHours;
            cmd.Parameters.Add("@ActualHours", SqlDbType.Int).Value = actualHours;
            cmd.Parameters.Add("@Role", SqlDbType.Int).Value = roleID;
            cmd.Parameters.Add("@Status", SqlDbType.Int).Value = statusIDForUpdate;
            cmd.Parameters.Add("@ModifiedBy", SqlDbType.UniqueIdentifier).Value = createdBy;
            cmd.Parameters.Add("@TaskKey", SqlDbType.Int).Value = taskKey;

            DA.ExecuteNonQuery(cmd);

            //            ScriptManager.RegisterStartupScript(
            //    this,
            //    this.GetType(),
            //    "toastr_success",
            //    "toastr.success('Task updated successfully!');",
            //    true
            //);
            //            Response.Redirect("~/Employee/Taskgrid.aspx");
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
            ScriptManager.RegisterStartupScript(
          this,
          this.GetType(),
          "toastr_error",
          "toastr.error('" + ex.Message.Replace("'", "\\'") + "');",
          true
      );

        }
    }

    private void PopulateTaskData(int taskKey)
    {
        string query = @"
    SELECT 
        ProjectName,
        AssignedBy,
        TaskName,
        TaskDescription,
        EmployeeList,
        StartDate,
        EndDate,
        AssignedHours,
        ActualHours,
        Role,
        Status
    FROM IT_TaskCreation
    WHERE TaskKey = @TaskKey";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@TaskKey", taskKey);

        DataSet ds = DA.GetDataSet(cmd);

        if (ds.Tables[0].Rows.Count == 0)
            return;

        DataRow row = ds.Tables[0].Rows[0];

        // Project
        ddlProject.SelectedValue = row["ProjectName"].ToString();
        int projectKey = Convert.ToInt32(row["ProjectName"]);

        // MUST bind first
        BindTeamLead(projectKey);
        BindEmployees(projectKey);



        // Employee
        string employee = row["EmployeeList"].ToString();
        if (ddlEmployee.Items.FindByValue(employee) != null)
            ddlEmployee.SelectedValue = employee;
        hfEmployeeKey.Value = employee;

        // Role & Status
        ddlRole.SelectedValue = row["Role"].ToString();
        ddlTaskStatus.SelectedValue = row["Status"].ToString();

        // Dates
        if (row["StartDate"] != DBNull.Value)
            txtStartDate.Text = Convert.ToDateTime(row["StartDate"]).ToString("dd/MM/yyyy");

        if (row["EndDate"] != DBNull.Value)
        {
            string endDateStr = Convert.ToDateTime(row["EndDate"]).ToString("dd/MM/yyyy");
            txtEndDate.Text = endDateStr;
            hfEndDate.Value = endDateStr; // Store in hidden field too
        }

        // Hours
        txtHours.Text = row["AssignedHours"] != DBNull.Value
            ? row["AssignedHours"].ToString()
            : "";

        txtActualHours.Text = row["ActualHours"] != DBNull.Value
            ? row["ActualHours"].ToString()
            : "";

        txtTaskName.Text = row["TaskName"].ToString();
        txtTaskDescription.Text = row["TaskDescription"].ToString();
    }

    // 8 hours per day restriction removed
    //private bool CheckAssignedHours(Guid employeeId, DateTime taskDate, int assignedHours, int taskKey)
    //{
    //    SqlCommand cmd = new SqlCommand(@"
    //    SELECT @TotalHours = ISNULL(SUM(AssignedHours), 0)
    //    FROM IT_TaskCreation
    //    WHERE EmployeeList = @EmployeeKey
    //      AND CAST(StartDate AS DATE) = @TaskDate
    //      AND (@TaskKey = 0 OR TaskKey <> @TaskKey)
    //");

    //    cmd.Parameters.Add("@EmployeeKey", SqlDbType.UniqueIdentifier).Value = employeeId;
    //    cmd.Parameters.Add("@TaskDate", SqlDbType.Date).Value = taskDate;
    //    cmd.Parameters.Add("@TaskKey", SqlDbType.Int).Value = taskKey;

    //    SqlParameter outParam = new SqlParameter("@TotalHours", SqlDbType.Int);
    //    outParam.Direction = ParameterDirection.Output;
    //    cmd.Parameters.Add(outParam);

    //    DA.ExecuteNonQuery(cmd);

    //    int existingHours = Convert.ToInt32(outParam.Value);

    //    if (existingHours + assignedHours > 8)
    //    {
    //        lblTotalHours.Visible = true;
    //        lblTotalHours.ForeColor = System.Drawing.Color.Red;
    //        lblTotalHours.Text =
    //            "Already Assigned: " + existingHours +
    //            " hrs | Entered: " + assignedHours +
    //            " hrs | Max: 8 hrs/day";

    //        return false;
    //    }

    //    lblTotalHours.Visible = false;
    //    return true;
    //}

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

    // 8 hours per day restriction removed
    //private void CheckAssignedHours()
    //{
    //    if (string.IsNullOrEmpty(ddlEmployee.SelectedValue) ||
    //        string.IsNullOrEmpty(txtStartDate.Text) ||
    //        string.IsNullOrEmpty(txtHours.Text))
    //    {
    //        lblTotalHours.Visible = false;
    //        return;
    //    }

    //    Guid employeeId;
    //    DateTime taskDate;
    //    int assignedHours;
    //    int taskKey = 0;

    //    if (!Guid.TryParse(ddlEmployee.SelectedValue, out employeeId))
    //        return;

    //    if (!DateTime.TryParseExact(
    //           txtStartDate.Text.Trim(),
    //           "dd/MM/yyyy",
    //           System.Globalization.CultureInfo.InvariantCulture,
    //           System.Globalization.DateTimeStyles.None,
    //           out taskDate))
    //       return;

    //    if (!int.TryParse(txtHours.Text, out assignedHours))
    //        return;

    //    // Update case-la taskKey irukkum
    //    if (!string.IsNullOrEmpty(hfTaskKey.Value))
    //    {
    //        int.TryParse(hfTaskKey.Value, out taskKey);
    //    }

    //    CheckAssignedHours(employeeId, taskDate, assignedHours, taskKey);
    //}

    private string GetRedirectScript()
    {
        string projectParam = Request.QueryString["project"];
        if (!string.IsNullOrEmpty(projectParam))
        {
            return "window.location.href = '/Employee/taskgrids.aspx?id=" + Server.UrlEncode(projectParam) + "';";
        }
        else
        {
            return "window.location.href = '/Employee/taskgrids.aspx';";
        }
    }

    // 8 hours per day restriction removed
    //protected void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    CheckAssignedHours();
    //}

    //protected void txtStartDate_TextChanged(object sender, EventArgs e)
    //{
    //    CheckAssignedHours();
    //}

    //protected void txtHours_TextChanged(object sender, EventArgs e)
    //{
    //    CheckAssignedHours();
    //}
}