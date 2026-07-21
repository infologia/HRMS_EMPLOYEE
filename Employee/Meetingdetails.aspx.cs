using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_Meetingdetails : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;

    private string key = "";
    string str_id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();

        Label control1 = this.Master.FindControl("lbl_bread") as Label;
        if (control1 != null)
            control1.Text = "Meeting Details";

        // Always check if QueryString has id
        if (!string.IsNullOrEmpty(Request.QueryString["id"]))
        {
            this.str_id = Request.QueryString["id"].ToString();

            int viewId = 0;

            if (!string.IsNullOrEmpty(Request.QueryString["Viewid"]))
            {
                string decrypted =
        UrlCrypto.Decrypt(Request.QueryString["Viewid"]);

                viewId = Convert.ToInt32(decrypted);
            }

            if (viewId == 0)
            {
                btn_update.Visible = false;
                btn_request.Visible = false;
            }
            else
            {
                btn_update.Visible = true;
                btn_request.Visible = false;
            }

            if (!IsPostBack)
            {
                Loadclient();
                LoadEmp();
                LoadLeads();
                assignvalues();
  if (viewId == 0)
  {
      // VIEW MODE
      DisableControlsSimple();
  }
  else
  {
      // UPDATE MODE
      EnableControlsForEdit();
  }
            }

            
        }
        else
        {
            if (!IsPostBack)
            {
                Loadclient();
                LoadEmp();
                LoadLeads();
            }

            btn_request.Visible = true;
            btn_update.Visible = false;
        }
    }
    private void LoadEmp()
    {
        string str_lead = "select employeekey,(firstname) name,employeekey from  IT_EmployeeRegister where Employeestatus=1";

        {
            SqlCommand cmd = new SqlCommand(str_lead);
            DataSet reader = this.DA.GetDataSet(cmd);
            ddl_employee.DataSource = reader;
            ddl_employee.DataTextField = "name";
            ddl_employee.DataValueField = "employeekey";
            ddl_employee.Style.Add("padding-top", "0px");
            ddl_employee.DataBind();
           // ddl_employee.Items.Insert(0, new ListItem("-- Select Participation --", ""));
        }
    }
    private void Loadclient()
    {
        string str_lead = "select ClientKey,CompanyName from IT_ClientDetails where status=1";
        {
            SqlCommand cmd = new SqlCommand(str_lead);
            DataSet reader = this.DA.GetDataSet(cmd);
            ddl_Client.DataSource = reader;
            ddl_Client.DataTextField = "CompanyName";
            ddl_Client.DataValueField = "ClientKey";
            ddl_Client.DataBind();
            ddl_Client.Items.Insert(0, new ListItem("-- Select Client --", ""));
        }
    }
    private void LoadLeads()
    {
        string str_lead = "select LeadKey,Name from IT_Leads where Status Is NOT NULL";
        {
            SqlCommand cmd = new SqlCommand(str_lead);
            DataSet reader = this.DA.GetDataSet(cmd);
            ddl_Leads.DataSource = reader;
            ddl_Leads.DataTextField = "Name";
            ddl_Leads.DataValueField = "LeadKey";
            ddl_Leads.DataBind();
            ddl_Leads.Items.Insert(0, new ListItem("-- Select Leads --", ""));
        }
    }

    private void LoadUpdateLeads(string leadskey)
    {
        string str_lead = "SELECT Name, LeadKey FROM IT_Leads WHERE CreatedBy = @CreatedBy";

        SqlCommand cmd = new SqlCommand(str_lead);
        cmd.Parameters.AddWithValue("@CreatedBy", SC.Userid);

        DataSet ds = this.DA.GetDataSet(cmd);

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddl_Leads.DataSource = ds.Tables[0];
            ddl_Leads.DataTextField = "Name";
            ddl_Leads.DataValueField = "LeadKey";
            ddl_Leads.DataBind();

            ddl_Leads.Items.Insert(0, new ListItem("-- Select Leads --", ""));
        }
        else
        {
            string str_leadupdate = "select Name,LeadKey from IT_Leads where LeadKey='" + leadskey + "'";
            {
                SqlCommand cmdupdate = new SqlCommand(str_leadupdate);
                DataSet reader = this.DA.GetDataSet(cmdupdate);
                ddl_Leads.DataSource = reader;
                ddl_Leads.DataTextField = "Name";
                ddl_Leads.DataValueField = "LeadKey";
                ddl_Leads.DataBind();
                ddl_Leads.Items.Insert(0, new ListItem("-- Select Leads --", ""));
            }
        }

    }
    protected void btn_request_Click(object sender, EventArgs e)
    {
        Guid userId = new Guid(SC.Userid.ToString());

        string[] dateFormats = { "dd/MM/yyyy", "dd-MM-yyyy", "MM/dd/yyyy", "yyyy-MM-dd", "d/M/yyyy", "d-M-yyyy" };
        string[] timeFormats = {
            "dd/MM/yyyy h:mm tt", "dd/MM/yyyy hh:mm tt", "dd-MM-yyyy h:mm tt", "dd-MM-yyyy hh:mm tt",
            "dd/MM/yyyy H:mm", "dd-MM-yyyy H:mm", "MM/dd/yyyy h:mm tt", "MM/dd/yyyy hh:mm tt",
            "d/M/yyyy h:mm tt", "d/M/yyyy hh:mm tt", "d-M-yyyy h:mm tt", "d-M-yyyy hh:mm tt"
        };

        DateTime meetingDate;
        if (!DateTime.TryParseExact(txt_MeetingDate.Text.Trim(), dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out meetingDate))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "err", "showToastr('error','Invalid meeting date format.');", true);
            return;
        }

        DateTime startTime;
        if (!DateTime.TryParseExact(txt_MeetingDate.Text.Trim() + " " + txt_starttime.Text.Trim(), timeFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out startTime))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "err", "showToastr('error','Invalid start time format.');", true);
            return;
        }

        DateTime endTime;
        if (!DateTime.TryParseExact(txt_MeetingDate.Text.Trim() + " " + txt_endtime.Text.Trim(), timeFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out endTime))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "err", "showToastr('error','Invalid end time format.');", true);
            return;
        }

        string sqlMeetingInsert = @"
        DECLARE @MeetingKey TABLE (MeetingKey INT);

        INSERT INTO IT_Meetings
        (
            MeetingTitle, MeetingDescription, MeetingDate, StartTime, EndTime,
            MeetingType, MeetingLink, ClientKey, CreatedBy, Status, Fanthomlink,leads,ProjectKey
        )
        OUTPUT INSERTED.MeetingKey INTO @MeetingKey
        VALUES
        (
            @MeetingTitle, @MeetingDescription, @MeetingDate, @StartTime, @EndTime,
            @MeetingType, @MeetingLink, @ClientKey, @CreatedBy, @Status, @Fanthomlink,@leads,@ProjectKey
        );

        SELECT MeetingKey FROM @MeetingKey;
    ";

        SqlCommand cmdMeeting = new SqlCommand(sqlMeetingInsert);
        cmdMeeting.Parameters.AddWithValue("@MeetingTitle", txt_MeetingTitle.Text.Trim());
        cmdMeeting.Parameters.AddWithValue("@MeetingDescription", txt_Description.InnerText.Trim());
        cmdMeeting.Parameters.AddWithValue("@MeetingDate", meetingDate);
        cmdMeeting.Parameters.AddWithValue("@StartTime", startTime);
        cmdMeeting.Parameters.AddWithValue("@EndTime", endTime);
        cmdMeeting.Parameters.AddWithValue("@MeetingType", ddl_meetingtype.SelectedValue);
        cmdMeeting.Parameters.AddWithValue("@MeetingLink", txt_MeetingLink.Text.Trim());
        cmdMeeting.Parameters.AddWithValue("@ClientKey",
            ddl_meetingtype.SelectedValue == "3"? new Guid(ddl_Client.SelectedValue): (object)DBNull.Value);

        object projectKey = DBNull.Value;
        int projId;
        if (ddl_meetingtype.SelectedValue == "3" &&
            !string.IsNullOrEmpty(hfProjectKey.Value) &&
            int.TryParse(hfProjectKey.Value, out projId))
        {
            projectKey = projId;
        }
        cmdMeeting.Parameters.AddWithValue("@ProjectKey", projectKey);

        object clientKey = DBNull.Value;

        int leadId;
        if (ddl_meetingtype.SelectedValue == "5" &&
            int.TryParse(ddl_Leads.SelectedValue, out leadId))
        {
            clientKey = leadId;
        }
        cmdMeeting.Parameters.AddWithValue("@leads", clientKey);


        cmdMeeting.Parameters.AddWithValue("@CreatedBy", userId);
        cmdMeeting.Parameters.AddWithValue("@Status", ddl_status.SelectedValue);
        cmdMeeting.Parameters.AddWithValue("@Fanthomlink", txt_FathomDetails.InnerText.Trim());

        DataTable dtMeeting = DA.GetDataTable(cmdMeeting);
        if (dtMeeting.Rows.Count == 0) return;

        int meetingKey = Convert.ToInt32(dtMeeting.Rows[0]["MeetingKey"]);

        bool anyParticipantAdded = false;

        // Insert PARTICIPANTS (SERVER-SIDE CONFLICT CHECK)
        foreach (ListItem item in ddl_employee.Items)
        {
            if (!item.Selected) continue;

            Guid employeeKey = new Guid(item.Value);

            // Conflict check (MANDATORY)
            SqlCommand cmdCheck = new SqlCommand(@"
SELECT 1
FROM IT_Meetings M
INNER JOIN IT_MeetingParticipants P ON M.MeetingKey = P.MeetingKey
WHERE P.EmployeeKey = @EmployeeKey
  AND M.StartTime < @EndTime
  AND M.EndTime > @StartTime
  AND M.Status = 1");
            cmdCheck.Parameters.AddWithValue("@EmployeeKey", employeeKey);
            cmdCheck.Parameters.AddWithValue("@MeetingDate", meetingDate);
            cmdCheck.Parameters.AddWithValue("@StartTime", startTime);
            cmdCheck.Parameters.AddWithValue("@EndTime", endTime);

            DataTable dtConflict = DA.GetDataTable(cmdCheck);

            if (dtConflict.Rows.Count > 0)
            {
                // Skip conflicted employee
                continue;
            }

            // Insert participant
            SqlCommand cmdParticipant = new SqlCommand(@"
            INSERT INTO IT_MeetingParticipants (MeetingKey, EmployeeKey, AddedOn)
            VALUES (@MeetingKey, @EmployeeKey, GETDATE())");

            cmdParticipant.Parameters.AddWithValue("@MeetingKey", meetingKey);
            cmdParticipant.Parameters.AddWithValue("@EmployeeKey", employeeKey);

            DA.ExecuteNonQuery(cmdParticipant);
            anyParticipantAdded = true;
        }

        // Final validation
        if (!anyParticipantAdded)
        {

            ScriptManager.RegisterStartupScript(
            this,
            this.GetType(),
            "toastr_redirect",
            "showToastr('success','No participants added. All selected employees have conflicts.');" +
            "setTimeout(function(){ window.location.href = '/Employee/Meetings.aspx'; }, 2000);",
            true
        );

          
            return;
        }
        ScriptManager.RegisterStartupScript(
             this,
             this.GetType(),
             "toastr_redirect",
             "showToastr('success','Meetings Save successfully!');" +
             "setTimeout(function(){ window.location.href = '/Employee/Meetings.aspx'; }, 2000);",
             true
         );
        
    }

    private void LoadProjectsByClient(string clientKey)
    {
        string query = "SELECT ProjectKey, ProjectName FROM IT_Projects WHERE ClientKey = @ClientKey";
        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@ClientKey", new Guid(clientKey));
        DataTable dt = DA.GetDataTable(cmd);
        ddl_Projects.DataSource = dt;
        ddl_Projects.DataTextField = "ProjectName";
        ddl_Projects.DataValueField = "ProjectKey";
        ddl_Projects.DataBind();
        ddl_Projects.Items.Insert(0, new ListItem("-- Select Project --", ""));
    }

    [WebMethod]
    public static List<ProjectItem> GetProjectsByClient(string clientKey)
    {
        List<ProjectItem> projects = new List<ProjectItem>();
        string query = "SELECT ProjectKey, ProjectName FROM IT_Projects WHERE ClientKey = @ClientKey";
        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@ClientKey", new Guid(clientKey));
        DataTable dt = new DataAccess().GetDataTable(cmd);
        foreach (DataRow row in dt.Rows)
        {
            projects.Add(new ProjectItem
            {
                ProjectKey = row["ProjectKey"].ToString(),
                ProjectName = row["ProjectName"].ToString()
            });
        }
        return projects;
    }

    public class ProjectItem
    {
        public string ProjectKey { get; set; }
        public string ProjectName { get; set; }
    }

    [WebMethod]
    public static object CheckMeetingConflict(
        string employeeKey,
        string meetingDate,
        string startTime,
        string endTime,
        int meetingKey)
    {
        CultureInfo culture = CultureInfo.InvariantCulture;

        string[] dateFormats = { "dd/MM/yyyy", "dd-MM-yyyy", "MM/dd/yyyy", "yyyy-MM-dd", "d/M/yyyy", "d-M-yyyy" };
        string[] timeFormats = {
            "dd/MM/yyyy h:mm tt", "dd/MM/yyyy hh:mm tt", "dd-MM-yyyy h:mm tt", "dd-MM-yyyy hh:mm tt",
            "dd/MM/yyyy H:mm", "dd-MM-yyyy H:mm", "MM/dd/yyyy h:mm tt", "MM/dd/yyyy hh:mm tt",
            "d/M/yyyy h:mm tt", "d/M/yyyy hh:mm tt", "d-M-yyyy h:mm tt", "d-M-yyyy hh:mm tt"
        };
        
        DateTime meetingDateOnly = DateTime.ParseExact(meetingDate.Trim(), dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None);
        DateTime startDateTime = DateTime.ParseExact(startTime.Trim(), timeFormats, CultureInfo.InvariantCulture, DateTimeStyles.None);
        DateTime endDateTime = DateTime.ParseExact(endTime.Trim(), timeFormats, CultureInfo.InvariantCulture, DateTimeStyles.None);
        SqlCommand cmd = new SqlCommand(@"
        SELECT TOP 1 
            M.MeetingTitle,
            M.MeetingDate,
            M.StartTime,
            M.EndTime,
            c.Firstname + ' ' + c.Lastname AS UserName,m.MeetingKey
        FROM IT_Meetings M
        INNER JOIN IT_MeetingParticipants P ON M.MeetingKey = P.MeetingKey
        INNER JOIN IT_EmployeeRegister c ON P.EmployeeKey = c.EmployeeKey
        WHERE P.EmployeeKey = @EmployeeKey
          AND M.StartTime < @EndTime
          AND M.EndTime > @StartTime
          AND M.MeetingDate = @MeetingDate
          AND M.Status = 1
          AND (@MeetingKey = 0 OR M.MeetingKey <> @MeetingKey)");

        cmd.Parameters.Add("@EmployeeKey", SqlDbType.UniqueIdentifier)
            .Value = new Guid(employeeKey);

        cmd.Parameters.Add("@StartTime", SqlDbType.DateTime)
            .Value = startDateTime;

        cmd.Parameters.Add("@EndTime", SqlDbType.DateTime)
            .Value = endDateTime;

        cmd.Parameters.Add("@MeetingDate", SqlDbType.Date)
            .Value = meetingDateOnly;

        cmd.Parameters.Add("@MeetingKey", SqlDbType.Int).Value = meetingKey;

        DataTable dt = new DataAccess().GetDataTable(cmd);

        if (dt.Rows.Count > 0)
        {
            return new
            {
                conflict = true,
                title = dt.Rows[0]["MeetingTitle"].ToString(),
                date = meetingDateOnly.ToString("dd-MM-yyyy"),
                employeeName = dt.Rows[0]["UserName"].ToString(),
                MeetingKey = dt.Rows[0]["MeetingKey"].ToString(),
                time =
                    Convert.ToDateTime(dt.Rows[0]["StartTime"]).ToString("hh:mm tt")
                    + " - " +
                    Convert.ToDateTime(dt.Rows[0]["EndTime"]).ToString("hh:mm tt")
            };
        }

        return new { conflict = false };
    }

  

    private static string GetEmployeeName(string empKey)
    {
        string name = "";

        SqlCommand cmd = new SqlCommand(
            "SELECT FirstName FROM IT_EmployeeRegister WHERE EmployeeKey = @Key");
        cmd.Parameters.AddWithValue("@Key", new Guid(empKey));

        DataTable dt = new DataAccess().GetDataTable(cmd);

        if (dt.Rows.Count > 0)
        {
            name = dt.Rows[0]["FirstName"].ToString();
        }

        return name;
    }

    private void ClearForm()
    {
        //txt_name.Text = "";
        //ddl_status.SelectedIndex = 0;
    }
    public void assignvalues()
    {
        string str_assign = "SELECT * FROM IT_Meetings WHERE MeetingKey = @MeetingKey";
        SqlCommand cmd = new SqlCommand(str_assign);
        cmd.Parameters.AddWithValue("@MeetingKey", this.str_id);

        DataTable dt_meeting = this.DA.GetDataTable(cmd);

      



        if (dt_meeting.Rows.Count > 0)
        {

            hfMeetingKey.Value = this.str_id;

            txt_MeetingTitle.Text = dt_meeting.Rows[0]["MeetingTitle"].ToString();
            txt_Description.InnerText = dt_meeting.Rows[0]["MeetingDescription"].ToString();
            txt_MeetingDate.Text = Convert.ToDateTime(dt_meeting.Rows[0]["MeetingDate"]).ToString("dd/MM/yyyy");

          

            txt_starttime.Text = dt_meeting.Rows[0]["StartTime"] == DBNull.Value
    ? ""
    : Convert.ToDateTime(dt_meeting.Rows[0]["StartTime"])
        .ToString("hh:mm tt");

            txt_endtime.Text = dt_meeting.Rows[0]["EndTime"] == DBNull.Value
                ? ""
                : Convert.ToDateTime(dt_meeting.Rows[0]["EndTime"])
                    .ToString("hh:mm tt");


            ddl_meetingtype.SelectedValue = dt_meeting.Rows[0]["MeetingType"].ToString();
          string meetingtype = dt_meeting.Rows[0]["MeetingType"].ToString();
          string leadskey = dt_meeting.Rows[0]["leads"].ToString();
            txt_MeetingLink.Text = dt_meeting.Rows[0]["MeetingLink"].ToString();

            if (meetingtype == "3")
            {
                div_client.Visible = true;
                div_projects.Visible = true;
                div_leads.Visible = false;
                Loadclient();
                if (dt_meeting.Rows[0]["ClientKey"] != DBNull.Value)
                {
                    ddl_Client.SelectedValue = dt_meeting.Rows[0]["ClientKey"].ToString();
                }
                if (dt_meeting.Rows[0]["ProjectKey"] != DBNull.Value)
                {
                    string clientKeyVal = dt_meeting.Rows[0]["ClientKey"].ToString();
                    LoadProjectsByClient(clientKeyVal);
                    ddl_Projects.SelectedValue = dt_meeting.Rows[0]["ProjectKey"].ToString();
                    hfProjectKey.Value = dt_meeting.Rows[0]["ProjectKey"].ToString();
                }
            }
            else if (meetingtype == "5")
            {
                div_leads.Visible = true;
                div_client.Visible = false;
                div_projects.Visible = false;
                LoadUpdateLeads(leadskey);
            }
            else
            {
                div_leads.Visible = false;
                div_client.Visible = false;
                div_projects.Visible = false;
            }

            if (dt_meeting.Rows[0]["leads"] != DBNull.Value)
                ddl_Leads.SelectedValue = dt_meeting.Rows[0]["leads"].ToString();

            ddl_status.SelectedValue = dt_meeting.Rows[0]["Status"].ToString();
            txt_FathomDetails.InnerText = dt_meeting.Rows[0]["fanthomlink"].ToString();
        }
        string str_Employee = "SELECT EmployeeKey FROM IT_MeetingParticipants WHERE MeetingKey = @MeetingKey";
        SqlCommand cmd_Employee = new SqlCommand(str_Employee);
        cmd_Employee.Parameters.AddWithValue("@MeetingKey", this.str_id);

        DataTable dt_employee = this.DA.GetDataTable(cmd_Employee);

        
        if (dt_employee.Rows.Count > 0)
        {
            ddl_employee.ClearSelection();

            List<string> userKeyList = new List<string>();

            foreach (DataRow row in dt_employee.Rows)
            {
                string empKey = row["EmployeeKey"].ToString();

                ListItem item = ddl_employee.Items.FindByValue(empKey);
                if (item != null)
                {
                    item.Selected = true;
                    userKeyList.Add(empKey); // 🔹 add to array
                }
            }

            // 🔹 store as comma separated values
            UserKeys.Value = string.Join(",", userKeyList);
        }


    }
    private void DisableControlsSimple()
    {
        // TextBoxes → ReadOnly ONLY
        txt_MeetingTitle.ReadOnly = true;
        txt_starttime.ReadOnly = true;
        txt_endtime.ReadOnly = true;
        txt_MeetingDate.ReadOnly = true;
        txt_MeetingLink.ReadOnly = true;

        // Dropdowns → Enabled false (safe)
        ddl_meetingtype.Enabled = false;
        ddl_status.Enabled = false;
        ddl_Client.Enabled = false;
        ddl_Leads.Enabled = false;

        // Multiselect → DO NOT disable here
        // ddl_employee.Enabled = false; 

        // TextAreas
        txt_Description.Disabled = true;
        txt_FathomDetails.Disabled = true;

        // Buttons
        btn_request.Visible = false;
        btn_update.Visible = false;

        // Add view-mode class
        Page.Form.Attributes["class"] = "view-mode";
    }
    private void EnableControlsForEdit()
    {
        txt_MeetingTitle.ReadOnly = false;
        txt_starttime.ReadOnly = false;
        txt_endtime.ReadOnly = false;
        txt_MeetingDate.ReadOnly = false;
        txt_MeetingLink.ReadOnly = false;

        ddl_meetingtype.Enabled = true;
        ddl_status.Enabled = true;
        ddl_Client.Enabled = true;
        ddl_Leads.Enabled = true;

        txt_Description.Disabled = false;
        txt_FathomDetails.Disabled = false;

        btn_update.Visible = true;

        Page.Form.Attributes.Remove("class"); 
    }


    protected void btn_update_Click(object sender, EventArgs e)
    {
        Guid userId = new Guid(SC.Userid.ToString());

        // Parse MeetingKey as int
        int meetingKey;
        if (!int.TryParse(this.str_id, out meetingKey))
        {
            // Stop execution if MeetingKey is invalid
            throw new Exception("Invalid MeetingKey value: " + this.str_id);
        }

        string[] dateFormats = { "dd/MM/yyyy", "dd-MM-yyyy", "MM/dd/yyyy", "yyyy-MM-dd", "d/M/yyyy", "d-M-yyyy" };
        string[] timeFormats = {
            "dd/MM/yyyy h:mm tt", "dd/MM/yyyy hh:mm tt", "dd-MM-yyyy h:mm tt", "dd-MM-yyyy hh:mm tt",
            "dd/MM/yyyy H:mm", "dd-MM-yyyy H:mm", "MM/dd/yyyy h:mm tt", "MM/dd/yyyy hh:mm tt",
            "d/M/yyyy h:mm tt", "d/M/yyyy hh:mm tt", "d-M-yyyy h:mm tt", "d-M-yyyy hh:mm tt"
        };

        // Parse Meeting Date
        DateTime meetingDate;
        if (!DateTime.TryParseExact(txt_MeetingDate.Text.Trim(), dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out meetingDate))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "err", "showToastr('error','Invalid meeting date format.');", true);
            return;
        }

        // Parse Start Time (AM / PM)
        DateTime startTime;
        if (!DateTime.TryParseExact(txt_MeetingDate.Text.Trim() + " " + txt_starttime.Text.Trim(), timeFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out startTime))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "err", "showToastr('error','Invalid start time format.');", true);
            return;
        }

        // Parse End Time (AM / PM)
        DateTime endTime;
        if (!DateTime.TryParseExact(txt_MeetingDate.Text.Trim() + " " + txt_endtime.Text.Trim(), timeFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out endTime))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "err", "showToastr('error','Invalid end time format.');", true);
            return;
        }

        SqlCommand cmdUpdateMeeting = new SqlCommand(@"
        UPDATE IT_Meetings
        SET
            MeetingTitle       = @MeetingTitle,
            MeetingDescription = @MeetingDescription,
            MeetingDate        = @MeetingDate,
            StartTime          = @StartTime,
            EndTime            = @EndTime,
            MeetingType        = @MeetingType,
            MeetingLink        = @MeetingLink,
            ClientKey          = @ClientKey,
            Status             = @Status,
            Fanthomlink        = @Fanthomlink,
            ModifiedBy         = @ModifiedBy,
            ModifiedOn         = GETDATE(),
leads=@leads,
ProjectKey=@ProjectKey
        WHERE MeetingKey = @MeetingKey
    ");

        cmdUpdateMeeting.Parameters.AddWithValue("@MeetingTitle", txt_MeetingTitle.Text.Trim());
        cmdUpdateMeeting.Parameters.AddWithValue("@MeetingDescription", txt_Description.InnerText.Trim());
        cmdUpdateMeeting.Parameters.AddWithValue("@MeetingDate", meetingDate);
        cmdUpdateMeeting.Parameters.AddWithValue("@StartTime", startTime);
        cmdUpdateMeeting.Parameters.AddWithValue("@EndTime", endTime);
        cmdUpdateMeeting.Parameters.AddWithValue("@MeetingType", ddl_meetingtype.SelectedValue);
        cmdUpdateMeeting.Parameters.AddWithValue("@MeetingLink", txt_MeetingLink.Text.Trim());

        cmdUpdateMeeting.Parameters.AddWithValue("@ClientKey",
            ddl_meetingtype.SelectedValue == "3"
                ? new Guid(ddl_Client.SelectedValue)
                : (object)DBNull.Value);

        object projectKey = DBNull.Value;
        int projId;
        if (ddl_meetingtype.SelectedValue == "3" &&
            !string.IsNullOrEmpty(hfProjectKey.Value) &&
            int.TryParse(hfProjectKey.Value, out projId))
        {
            projectKey = projId;
        }
        cmdUpdateMeeting.Parameters.AddWithValue("@ProjectKey", projectKey);

        object clientKey = DBNull.Value;

        int leadId;
        if (ddl_meetingtype.SelectedValue == "5" &&
            int.TryParse(ddl_Leads.SelectedValue, out leadId))
        {
            clientKey = leadId;
        }
        cmdUpdateMeeting.Parameters.AddWithValue("@leads", clientKey);
        cmdUpdateMeeting.Parameters.AddWithValue("@Status", Convert.ToInt32(ddl_status.SelectedValue));
        cmdUpdateMeeting.Parameters.AddWithValue("@Fanthomlink", txt_FathomDetails.InnerText.Trim());
        cmdUpdateMeeting.Parameters.AddWithValue("@ModifiedBy", userId);
        cmdUpdateMeeting.Parameters.AddWithValue("@MeetingKey", meetingKey);

        DA.ExecuteNonQuery(cmdUpdateMeeting);

        SqlCommand cmdDeleteParticipants = new SqlCommand(@"
        DELETE FROM IT_MeetingParticipants
        WHERE MeetingKey = @MeetingKey
    ");
        cmdDeleteParticipants.Parameters.AddWithValue("@MeetingKey", meetingKey);
        DA.ExecuteNonQuery(cmdDeleteParticipants);

        bool anyParticipantAdded = false;

        foreach (ListItem item in ddl_employee.Items)
        {
            if (!item.Selected) continue;

            Guid employeeKey = new Guid(item.Value);

            SqlCommand cmdCheck = new SqlCommand(@"
            SELECT 1
            FROM IT_Meetings M
            INNER JOIN IT_MeetingParticipants P ON M.MeetingKey = P.MeetingKey
            WHERE P.EmployeeKey = @EmployeeKey
              AND M.StartTime < @EndTime
              AND M.EndTime > @StartTime
              AND M.Status = 1
              AND M.MeetingKey <> @MeetingKey
        ");

            cmdCheck.Parameters.AddWithValue("@EmployeeKey", employeeKey);
            cmdCheck.Parameters.AddWithValue("@StartTime", startTime);
            cmdCheck.Parameters.AddWithValue("@EndTime", endTime);
            cmdCheck.Parameters.AddWithValue("@MeetingKey", meetingKey);

            DataTable dtConflict = DA.GetDataTable(cmdCheck);

            if (dtConflict.Rows.Count > 0)
            {
                continue;
            }

            SqlCommand cmdParticipant = new SqlCommand(@"
            INSERT INTO IT_MeetingParticipants (MeetingKey, EmployeeKey, AddedOn)
            VALUES (@MeetingKey, @EmployeeKey, GETDATE())
        ");

            cmdParticipant.Parameters.AddWithValue("@MeetingKey", meetingKey);
            cmdParticipant.Parameters.AddWithValue("@EmployeeKey", employeeKey);

            DA.ExecuteNonQuery(cmdParticipant);
            anyParticipantAdded = true;
        }

        if (!anyParticipantAdded)
        {


            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "toastr_redirect",
                "showToastr('success','No participants added. All selected employees have conflicts.');",
                true
            );
          
            return;
        }
        ScriptManager.RegisterStartupScript(
               this,
               this.GetType(),
               "toastr_redirect",
               "showToastr('success','Meetings Update successfully!');" +
               "setTimeout(function(){ window.location.href = '/Employee/Meetings.aspx'; }, 2000);",
               true
           );
        
    }


    protected void ddl_meetingtype_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_meetingtype.SelectedValue == "3")
        {
            div_client.Visible = true;
            div_projects.Visible = true;
            div_leads.Visible = false;
            Loadclient();
        }
        else if (ddl_meetingtype.SelectedValue == "5")
        {
            div_leads.Visible= true;
            div_client.Visible = false;
            div_projects.Visible = false;
            LoadLeads();
        }
        else
        {
            div_leads.Visible = false;
            div_client.Visible = false;
            div_projects.Visible = false;
        }
    }


    [WebMethod]
    public static object UpdateConflict(
     List<string> employeeKey,
     string meetingDate,
     string startTime,
     string endTime)
    {
        CultureInfo culture = CultureInfo.InvariantCulture;

        string[] dateFormats = { "dd/MM/yyyy", "dd-MM-yyyy", "MM/dd/yyyy", "yyyy-MM-dd", "d/M/yyyy", "d-M-yyyy" };
        string[] timeFormats = {
            "dd/MM/yyyy h:mm tt", "dd/MM/yyyy hh:mm tt", "dd-MM-yyyy h:mm tt", "dd-MM-yyyy hh:mm tt",
            "dd/MM/yyyy H:mm", "dd-MM-yyyy H:mm", "MM/dd/yyyy h:mm tt", "MM/dd/yyyy hh:mm tt",
            "d/M/yyyy h:mm tt", "d/M/yyyy hh:mm tt", "d-M-yyyy h:mm tt", "d-M-yyyy hh:mm tt"
        };

        DateTime meetingDateOnly = DateTime.ParseExact(meetingDate.Trim(), dateFormats, culture, DateTimeStyles.None);
        DateTime startDateTime = DateTime.ParseExact(startTime.Trim(), timeFormats, culture, DateTimeStyles.None);
        DateTime endDateTime = DateTime.ParseExact(endTime.Trim(), timeFormats, culture, DateTimeStyles.None);

        foreach (string empKey in employeeKey)
        {
            SqlCommand cmd = new SqlCommand(@"
            SELECT TOP 1 
                M.MeetingTitle,
                M.MeetingDate,
                M.StartTime,
                M.EndTime,
                c.Firstname + ' ' + c.Lastname AS UserName,
                M.MeetingKey
            FROM IT_Meetings M
            INNER JOIN IT_MeetingParticipants P ON M.MeetingKey = P.MeetingKey
            INNER JOIN IT_EmployeeRegister c ON P.EmployeeKey = c.EmployeeKey
            WHERE P.EmployeeKey = @EmployeeKey
              AND M.StartTime < @EndTime
              AND M.EndTime > @StartTime
              AND M.MeetingDate = @MeetingDate
              AND M.Status = 1");

            cmd.Parameters.Add("@EmployeeKey", SqlDbType.UniqueIdentifier)
                .Value = new Guid(empKey);

            cmd.Parameters.Add("@StartTime", SqlDbType.DateTime)
                .Value = startDateTime;

            cmd.Parameters.Add("@EndTime", SqlDbType.DateTime)
                .Value = endDateTime;

            cmd.Parameters.Add("@MeetingDate", SqlDbType.Date)
                .Value = meetingDateOnly;

            DataTable dt = new DataAccess().GetDataTable(cmd);

            // ✅ Only return when conflict exists
            if (dt.Rows.Count > 0)
            {
                return new
                {
                    conflict = true,
                    title = dt.Rows[0]["MeetingTitle"].ToString(),
                    date = meetingDateOnly.ToString("dd-MM-yyyy"),
                    employeeName = dt.Rows[0]["UserName"].ToString(),
                    MeetingKey = dt.Rows[0]["MeetingKey"].ToString(),
                    time =
                        Convert.ToDateTime(dt.Rows[0]["StartTime"]).ToString("hh:mm tt")
                        + " - " +
                        Convert.ToDateTime(dt.Rows[0]["EndTime"]).ToString("hh:mm tt")
                };
            }
        }

        // ✅ No conflicts for any employee
        return new { conflict = false };
    }

    

    [WebMethod]
       public static object Removemeeting(string meetingKey, List<string> employeeKeys)
    {
        SessionCustom SC = new SessionCustom();
        DataAccess DA = new DataAccess();

        Guid currentUser = new Guid(SC.Userid);

        //  Permission check
        SqlCommand cmdCheck = new SqlCommand(@"
        SELECT 1
        FROM IT_Meetings M
        INNER JOIN IT_EmployeeRegister E ON E.EmployeeKey = @UserId
        WHERE M.MeetingKey = @MeetingKey
          AND (
                M.CreatedBy = @UserId
                OR (E.Division = 1 AND E.EmployeeStatus = 1)
              )
    ");

        cmdCheck.Parameters.AddWithValue("@MeetingKey", meetingKey);
        cmdCheck.Parameters.AddWithValue("@UserId", currentUser);

        DataTable dtPermission = DA.GetDataTable(cmdCheck);

        if (dtPermission.Rows.Count == 0)
        {
            //  Not allowed
            return new { success = false, message = "You are not authorized to modify this meeting." };
        }

        //  Allowed – remove participants
        foreach (string empKey in employeeKeys)
        {
            SqlCommand cmd = new SqlCommand(@"
            DELETE FROM IT_MeetingParticipants
            WHERE MeetingKey = @MeetingKey AND EmployeeKey = @EmployeeKey
        ");

            cmd.Parameters.AddWithValue("@MeetingKey", meetingKey);
            cmd.Parameters.AddWithValue("@EmployeeKey", new Guid(empKey));

            DA.ExecuteNonQuery(cmd);
        }

        return new { success = true };
    }

}