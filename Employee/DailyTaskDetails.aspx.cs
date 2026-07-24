using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_DailyTaskDetails : System.Web.UI.Page
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
                control1.Text = "Daily Task Details";

            if (!string.IsNullOrEmpty(Request.QueryString["date"]) && !string.IsNullOrEmpty(Request.QueryString["emp"]))
            {
                LoadDailyTasks(Request.QueryString["date"], Request.QueryString["emp"]);
            }
            else
            {
                Response.Redirect("Overalltaskgrid.aspx");
            }
        }
    }

    private void LoadDailyTasks(string dateStr, string empKey)
    {
        DateTime date;
        if (!DateTime.TryParse(dateStr, out date))
        {
            Response.Redirect("Overalltaskgrid.aspx");
            return;
        }

        string formattedDate = date.ToString("dd/MM/yyyy");
        lblDate.Text = formattedDate;

        // Get Employee Name
        string empQuery = "SELECT Firstname + ' ' + Lastname AS EmpName FROM IT_EmployeeRegister WHERE EmployeeKey = @EmpKey";
        SqlCommand empCmd = new SqlCommand(empQuery);
        empCmd.Parameters.AddWithValue("@EmpKey", empKey);
        DataTable dtEmp = DA.GetDataTable(empCmd);
        if (dtEmp != null && dtEmp.Rows.Count > 0)
        {
            lblEmployeeName.Text = dtEmp.Rows[0]["EmpName"].ToString();
        }

        // Get tasks
        string query = @"SELECT t.TaskKey, p.ProjectName, tr.RoleName AS WorkTypeName, d.TaskDetailID, d.TaskName, d.TaskDescription, 
                        d.AssignedHours, d.ActualHours, d.Status, d.Remarks 
                        FROM IT_TaskCreation t
                        LEFT JOIN IT_Projects p ON t.ProjectName = p.ProjectKey
                        LEFT JOIN IT_TaskDescriptiondetails d ON t.TaskKey = d.TaskKey
                        LEFT JOIN IT_TaskRole tr ON d.WorkType = tr.RoleID
                        WHERE CAST(t.StartDate AS DATE) = @Date AND t.EmployeeList = @EmpKey
                        ORDER BY p.ProjectName, d.TaskName";
                        
        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@Date", date.Date);
        cmd.Parameters.AddWithValue("@EmpKey", empKey);
        
        DataTable dt = DA.GetDataTable(cmd);
        
        decimal totalHours = 0;
        decimal totalActualHours = 0;

        if (dt == null || dt.Rows.Count == 0)
        {
            phProjects.Controls.Add(new Literal { Text = "<div class='alert alert-info'>No tasks found for this date.</div>" });
        }
        else
        {
            StringBuilder html = new StringBuilder();

            html.Append("<div class='table-responsive'>");
            html.Append("<table class='table table-bordered table-striped task-table'>");
            html.Append("<thead><tr><th>Project Name</th><th>Work Type</th><th style='width: 20%;'>Task Name</th><th style='width: 30%;'>Task Description</th><th style='text-align: center;'>Notes</th><th>Asgn (h)</th><th>Act (h)</th><th style='width: 10%; text-align: center;'>Status</th></tr></thead>");
            html.Append("<tbody>");

            foreach (DataRow row in dt.Rows)
            {
                string projectName = row["ProjectName"].ToString();
                if (string.IsNullOrEmpty(projectName)) projectName = "Unassigned Project";
                
                string workType = row["WorkTypeName"].ToString();
                string taskName = row["TaskName"].ToString();
                string taskDesc = row["TaskDescription"].ToString();
                string remarks = row["Remarks"].ToString();
                decimal assignedHours = row["AssignedHours"] != DBNull.Value ? Convert.ToDecimal(row["AssignedHours"]) : 0;
                decimal actualHours = row["ActualHours"] != DBNull.Value ? Convert.ToDecimal(row["ActualHours"]) : 0;
                int status = row["Status"] != DBNull.Value ? Convert.ToInt32(row["Status"]) : 1;

                totalHours += assignedHours;
                totalActualHours += actualHours;
                
                string notesHtml = "-";
                if (!string.IsNullOrWhiteSpace(remarks))
                {
                    notesHtml = "<i class='glyphicon glyphicon-info-sign text-info' title='" + Server.HtmlEncode(remarks) + "' style='font-size: 14px;'></i>";
                }

                string statusHtml = "";
                switch (status)
                {
                    case 1: statusHtml = "<span class='status-badge bg-purple'>Yet to start</span>"; break;
                    case 2: statusHtml = "<span class='status-badge bg-blue'>In Progress</span>"; break;
                    case 3: statusHtml = "<span class='status-badge bg-red'>Overdue</span>"; break;
                    case 4: statusHtml = "<span class='status-badge bg-green'>Completed</span>"; break;
                    case 5: statusHtml = "<span class='status-badge bg-purple'>Pending</span>"; break;
                    default: statusHtml = "<span class='status-badge bg-purple'>Yet to start</span>"; break;
                }

                html.Append("<tr>");
                html.Append("<td>" + projectName + "</td>");
                html.Append("<td>" + workType + "</td>");
                html.Append("<td>" + taskName + "</td>");
                html.Append("<td>" + taskDesc + "</td>");
                html.Append("<td class='text-center'>" + notesHtml + "</td>");
                html.Append("<td>" + assignedHours + "</td>");
                html.Append("<td>" + actualHours + "</td>");
                html.Append("<td class='text-center'>" + statusHtml + "</td>");
                html.Append("</tr>");
            }

            html.Append("</tbody></table>");
            html.Append("</div>");

            phProjects.Controls.Add(new Literal { Text = html.ToString() });
        }

        decimal meetingHours = LoadMeetings(date, empKey);
        lblTotalHours.Text = (totalHours + meetingHours).ToString("0.##");
        lblTotalActualHours.Text = totalActualHours.ToString("0.##");
    }

    private decimal LoadMeetings(DateTime date, string empKey)
    {
        string meetingQuery = @"
            SELECT DISTINCT
                a.MeetingTitle,
                a.MeetingDescription,
                FORMAT(a.StartTime, 'hh:mm tt') AS StartTime,
                FORMAT(a.EndTime, 'hh:mm tt') AS EndTime,
                a.Status,
                CAST(DATEDIFF(MINUTE, a.StartTime, a.EndTime) / 60.0 AS DECIMAL(10,2)) AS Hours
            FROM IT_Meetings a
            LEFT JOIN IT_MeetingParticipants c ON a.MeetingKey = c.MeetingKey
            WHERE CAST(a.MeetingDate AS DATE) = @Date 
            AND (c.EmployeeKey = @EmpKey OR a.CreatedBy = @EmpKey)";

        SqlCommand cmdMeeting = new SqlCommand(meetingQuery);
        cmdMeeting.Parameters.AddWithValue("@Date", date.Date);
        cmdMeeting.Parameters.AddWithValue("@EmpKey", empKey);

        DataTable dtMeetings = DA.GetDataTable(cmdMeeting);

        decimal totalMeetingHours = 0;

        if (dtMeetings == null || dtMeetings.Rows.Count == 0)
        {
            phMeetings.Controls.Add(new Literal { Text = "<div class='alert alert-info' style='font-size: 12px;'>No meetings scheduled for this date.</div>" });
            return totalMeetingHours;
        }

        StringBuilder htmlMeetings = new StringBuilder();
        htmlMeetings.Append("<div class='table-responsive'>");
        htmlMeetings.Append("<table class='table table-bordered table-striped task-table'>");
        htmlMeetings.Append("<thead><tr><th>Meeting Title</th><th>Meeting Description</th><th>Timings</th><th>Hours</th><th style='text-align: center;'>Status</th></tr></thead>");
        htmlMeetings.Append("<tbody>");

        foreach (DataRow row in dtMeetings.Rows)
        {
            string title = row["MeetingTitle"].ToString();
            string desc = row["MeetingDescription"].ToString();
            string start = row["StartTime"].ToString();
            string end = row["EndTime"].ToString();
            string hours = row["Hours"].ToString();
            int status = row["Status"] != DBNull.Value ? Convert.ToInt32(row["Status"]) : 1;

            string timings = start + " - " + end;

            decimal mHours = 0;
            if (decimal.TryParse(hours, out mHours))
            {
                totalMeetingHours += mHours;
            }

            string statusHtml = "";
            switch (status)
            {
                case 1: statusHtml = "<span class='status-badge' style='background-color: #337ab7;'>Scheduled</span>"; break;
                case 2: statusHtml = "<span class='status-badge bg-green'>Completed</span>"; break;
                case 3: statusHtml = "<span class='status-badge bg-red'>Cancelled</span>"; break;
                case 4: statusHtml = "<span class='status-badge' style='background-color: #f0ad4e;'>Postponed</span>"; break;
                default: statusHtml = "<span class='status-badge' style='background-color: #337ab7;'>Scheduled</span>"; break;
            }

            htmlMeetings.Append("<tr>");
            htmlMeetings.Append("<td>" + title + "</td>");
            htmlMeetings.Append("<td>" + desc + "</td>");
            htmlMeetings.Append("<td>" + timings + "</td>");
            htmlMeetings.Append("<td>" + hours + "</td>");
            htmlMeetings.Append("<td class='text-center'>" + statusHtml + "</td>");
            htmlMeetings.Append("</tr>");
        }

        htmlMeetings.Append("</tbody></table>");
        htmlMeetings.Append("</div>");

        phMeetings.Controls.Add(new Literal { Text = htmlMeetings.ToString() });
        return totalMeetingHours;
    }
}
