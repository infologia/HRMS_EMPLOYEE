using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_Assigntesting : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();

        if (!IsPostBack)
        {
            BindProjects();
            BindRole();
            BindStatus();
            BindTaskName();
            BindEmployees();

            if (Request.QueryString["id"] != null)
            {
                int testingKey = Convert.ToInt32(Request.QueryString["id"]);
                PopulateTestingData(testingKey);
                btnSaveTesting.Text = "Update Testing";
            }
            else
            {
                btnSaveTesting.Text = "Save Testing";
            }
        }
    }

    private void BindTaskName()
    {
        ddlTaskName.Items.Clear();
        ddlTaskName.Items.Insert(0, new ListItem("-- Select Task --", ""));
    }

    private void BindEmployees()
    {
        ddlEmployee.Items.Clear();
        ddlEmployee.Items.Insert(0, new ListItem("-- Select Employee --", ""));
    }

    private void PopulateTestingData(int testingKey)
    {
        string sql = @"SELECT projectkey, taskkey, TaskName, assignedto, 
                       CONVERT(VARCHAR(10), StartDate, 103) AS StartDate, 
                       CONVERT(VARCHAR(10), EndDate, 103) AS EndDate, 
                       AssignedHours, ActualHours, taskstatus, testdescription, CreatedBy 
                       FROM IT_TaskTesting WHERE TaskTestingkey = @TestingKey";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add("@TestingKey", SqlDbType.Int).Value = testingKey;
        DataTable dt = DA.GetDataTable(cmd);

        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            int projectKey = Convert.ToInt32(row["projectkey"]);
            int taskKey = Convert.ToInt32(row["taskkey"]);
            string assignedTo = row["assignedto"].ToString();
            string createdBy = row["CreatedBy"].ToString();
            string currentUser = SC.Userid;

            ddlProject.SelectedValue = projectKey.ToString();
            hfTaskKey.Value = taskKey.ToString();
            hfTaskName.Value = row["TaskName"].ToString();
            hfEmployeeKey.Value = assignedTo;
            txtStartDate.Text = row["StartDate"].ToString();
            txtEndDate.Text = row["EndDate"].ToString();
            hfEndDate.Value = row["EndDate"].ToString(); // Store in hidden field too
            txtHours.Text = row["AssignedHours"].ToString();
            txtActualHours.Text = row["ActualHours"] != DBNull.Value ? row["ActualHours"].ToString() : "";
            ddlTaskStatus.SelectedValue = row["taskstatus"].ToString();
            txtTestDescription.Text = row["testdescription"].ToString();
            hfTestingKey.Value = testingKey.ToString();

            if (createdBy != currentUser)
            {
                ddlProject.Enabled = false;
                ddlTaskName.Enabled = false;
                ddlEmployee.Enabled = false;
                ddlRole.Enabled = false;
                txtStartDate.Enabled = false;
                txtEndDate.Enabled = false;
                txtHours.Enabled = false;
                txtTaskDescription.Enabled = false;
                txtTestDescription.Enabled = false;
            }

            ClientScript.RegisterStartupScript(this.GetType(), "LoadDropdowns", 
                "loadTasksAndEmployees(" + projectKey + ", " + taskKey + ", '" + assignedTo + "');", true);
        }
    }

    private void BindProjects()
    {
        string userid = this.SC.Userid;
        
        // Get projects where the current user is assigned (either as participant or lead)
        string sql = @"SELECT DISTINCT p.ProjectKey, p.ProjectName 
                       FROM IT_Projects p
                       WHERE p.ProjectKey IN (
                           SELECT DISTINCT t.ProjectName 
                           FROM IT_TaskCreation t 
                           WHERE t.EmployeeList = @UserId
                           UNION
                           SELECT pp.ProjectKey 
                           FROM IT_ProjectsParticipants pp 
                           WHERE pp.EmployeeKey = @UserId
                           UNION
                           SELECT p2.ProjectKey 
                           FROM IT_Projects p2 
                           WHERE p2.LeadBy = @UserId
                       )
                       ORDER BY p.ProjectName";
                       
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add("@UserId", SqlDbType.NVarChar).Value = userid;
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
    public static string GetTaskDescription(int taskKey)
    {
        DataAccess DA = new DataAccess();

        string sql = "SELECT TaskDescription FROM IT_TaskCreation WHERE TaskKey = @TaskKey";
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add("@TaskKey", SqlDbType.Int).Value = taskKey;

        DataTable dt = DA.GetDataTable(cmd);
        if (dt.Rows.Count > 0)
            return dt.Rows[0]["TaskDescription"].ToString();

        return null;
    }

    private void BindRole()
    {
        string sql = "SELECT RoleID, RoleName FROM IT_TaskRole WHERE RoleID = 3";
        SqlCommand cmd = new SqlCommand(sql);
        DataSet ds = DA.GetDataSet(cmd);

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlRole.DataSource = ds.Tables[0];
            ddlRole.DataTextField = "RoleName";
            ddlRole.DataValueField = "RoleID";
            ddlRole.DataBind();
        }
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
            ddlTaskStatus.DataTextField = "StatusName";
            ddlTaskStatus.DataValueField = "StatusID";
            ddlTaskStatus.DataBind();
        }

        ddlTaskStatus.Items.Insert(0, new ListItem("-- Select Status --", ""));
    }

    [WebMethod]
    public static string CheckHours(string employeeKey, string startDate, int hours, int testingKey)
    {
        DataAccess DA = new DataAccess();
        Guid empId;
        DateTime date;
        if (!Guid.TryParse(employeeKey, out empId)) return null;
        if (!DateTime.TryParseExact(startDate, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out date)) return null;

        SqlCommand cmd1 = new SqlCommand(@"
            SELECT ISNULL(SUM(AssignedHours), 0) AS TotalHours
            FROM IT_TaskTesting
            WHERE assignedto = @EmployeeKey
              AND CAST(StartDate AS DATE) = @TaskDate
              AND (@TestingKey = 0 OR TaskTestingkey <> @TestingKey)");
        cmd1.Parameters.Add("@EmployeeKey", SqlDbType.UniqueIdentifier).Value = empId;
        cmd1.Parameters.Add("@TaskDate", SqlDbType.Date).Value = date;
        cmd1.Parameters.Add("@TestingKey", SqlDbType.Int).Value = testingKey;
        DataTable dt1 = DA.GetDataTable(cmd1);
        int testingHours = (dt1 != null && dt1.Rows.Count > 0) ? Convert.ToInt32(dt1.Rows[0]["TotalHours"]) : 0;

        SqlCommand cmd2 = new SqlCommand(@"
            SELECT ISNULL(SUM(AssignedHours), 0) AS TotalHours
            FROM IT_TaskCreation
            WHERE EmployeeList = @EmployeeKey
              AND CAST(StartDate AS DATE) = @TaskDate");
        cmd2.Parameters.Add("@EmployeeKey", SqlDbType.UniqueIdentifier).Value = empId;
        cmd2.Parameters.Add("@TaskDate", SqlDbType.Date).Value = date;
        DataTable dt2 = DA.GetDataTable(cmd2);
        int taskHours = (dt2 != null && dt2.Rows.Count > 0) ? Convert.ToInt32(dt2.Rows[0]["TotalHours"]) : 0;

        int totalExisting = testingHours + taskHours;
        if (totalExisting + hours > 8)
            return "Already Assigned: " + totalExisting + " hrs (Task: " + taskHours + ", Testing: " + testingHours + ") | Entered: " + hours + " hrs | Max: 8 hrs/day";
        return null;
    }

    [WebMethod]
    public static List<object> GetTasks(int projectKey)
    {
        DataAccess DA = new DataAccess();

        // Get tasks only from the selected project
        string sql = @"SELECT TaskKey, TaskName 
                       FROM IT_TaskCreation 
                       WHERE ProjectName = @ProjectKey 
                       AND TaskName IS NOT NULL AND TaskName <> ''
                       AND Status = 4
                       ORDER BY TaskName";

        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add("@ProjectKey", SqlDbType.Int).Value = projectKey;

        DataTable dt = DA.GetDataTable(cmd);
        var result = new List<object>();
        foreach (DataRow row in dt.Rows)
            result.Add(new { Value = row["TaskKey"].ToString(), Text = row["TaskName"].ToString() });

        return result;
    }

    [WebMethod]
    public static List<object> GetEmployees(int projectKey)
    {
        DataAccess DA = new DataAccess();

        string sql = @"SELECT e.EmployeeKey, (e.Firstname + ' ' + e.Lastname) AS EmployeeName
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

    protected void btnSaveTesting_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(ddlProject.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_error", "toastr.error('Please select Project');", true);
                return;
            }

            string selectedTaskKey = hfTaskKey.Value;
            if (string.IsNullOrEmpty(selectedTaskKey))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_error", "toastr.error('Please select Task Name');", true);
                return;
            }

            if (string.IsNullOrEmpty(hfEmployeeKey.Value))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_error", "toastr.error('Please select Team Member');", true);
                return;
            }

            if (string.IsNullOrEmpty(txtHours.Text.Trim()))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_error", "toastr.error('Please enter Assigned Hours');", true);
                return;
            }

            if (string.IsNullOrEmpty(ddlTaskStatus.SelectedValue))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_error", "toastr.error('Please select Status');", true);
                return;
            }

            if (hfHoursValid.Value == "false")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_block", "toastr.warning('Cannot save. Employee already has 8 hours for this date!');", true);
                return;
            }

            Guid userId = Guid.Parse(SC.Userid.ToString());
            Guid employeeList = Guid.Parse(hfEmployeeKey.Value);
            int projectKey = Convert.ToInt32(ddlProject.SelectedValue);
            int taskKey = Convert.ToInt32(hfTaskKey.Value);

            DateTime startDate = DateTime.ParseExact(txtStartDate.Text.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            
            // Get EndDate from hidden field since txtEndDate is disabled
            string endDateValue = string.IsNullOrEmpty(txtEndDate.Text) ? hfEndDate.Value : txtEndDate.Text;
            DateTime endDate = DateTime.ParseExact(endDateValue.Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
            
            int assignedHours = int.Parse(txtHours.Text.Trim());

            string sql;
            SqlCommand cmd;

            if (!string.IsNullOrEmpty(hfTestingKey.Value))
            {
                sql = @"UPDATE IT_TaskTesting SET 
                        projectkey = @ProjectKey, 
                        taskkey = @TaskKey, 
                        TaskName = @TaskName, 
                        assignedto = @AssignedTo, 
                        StartDate = @StartDate, 
                        EndDate = @EndDate, 
                        AssignedHours = @AssignedHours, 
                        ActualHours = @ActualHours, 
                        taskstatus = @Status, 
                        testdescription = @TestDescription, 
                        ModifiedOn = GETDATE(), 
                        ModifiedBy = @ModifiedBy 
                        WHERE TaskTestingkey = @TestingKey";

                cmd = new SqlCommand(sql);
                cmd.Parameters.Add("@TestingKey", SqlDbType.Int).Value = Convert.ToInt32(hfTestingKey.Value);
                cmd.Parameters.Add("@ModifiedBy", SqlDbType.UniqueIdentifier).Value = userId;
            }
            else
            {
                sql = @"INSERT INTO IT_TaskTesting
                        (projectkey, taskkey, TaskName, assignedto, StartDate, EndDate, AssignedHours, ActualHours, taskstatus, testdescription, CreatedOn, CreatedBy)
                        VALUES
                        (@ProjectKey, @TaskKey, @TaskName, @AssignedTo, @StartDate, @EndDate, @AssignedHours, @ActualHours, @Status, @TestDescription, GETDATE(), @CreatedBy)";

                cmd = new SqlCommand(sql);
                cmd.Parameters.Add("@CreatedBy", SqlDbType.UniqueIdentifier).Value = userId;
            }

            cmd.Parameters.Add("@ProjectKey", SqlDbType.Int).Value = projectKey;
            cmd.Parameters.Add("@TaskKey", SqlDbType.Int).Value = taskKey;
            cmd.Parameters.Add("@TaskName", SqlDbType.NVarChar).Value = hfTaskName.Value;
            cmd.Parameters.Add("@AssignedTo", SqlDbType.UniqueIdentifier).Value = employeeList;
            cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = startDate;
            cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = endDate;
            cmd.Parameters.Add("@AssignedHours", SqlDbType.Int).Value = assignedHours;
            cmd.Parameters.Add("@ActualHours", SqlDbType.Int).Value = string.IsNullOrEmpty(txtActualHours.Text.Trim()) ? (object)DBNull.Value : Convert.ToInt32(txtActualHours.Text.Trim());
            cmd.Parameters.Add("@Status", SqlDbType.Int).Value = Convert.ToInt32(ddlTaskStatus.SelectedValue);
            cmd.Parameters.Add("@TestDescription", SqlDbType.NVarChar).Value = txtTestDescription.Text.Trim();

            DA.ExecuteNonQuery(cmd);

            string message = string.IsNullOrEmpty(hfTestingKey.Value) ? "Testing assigned successfully!" : "Testing updated successfully!";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_redirect", "showToastr('success','" + message + "');setTimeout(function(){ window.location.href = '/Employee/Assigntestings.aspx'; }, 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_error", "toastr.error('" + ex.Message.Replace("'", "\\'") + "');", true);
        }
    }
}
