using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_Taskdashboard : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        if (!IsPostBack)
            LoadProjectDashboard();
    }

    public string GetBorderClass(object status)
    {
        if (status == null || status == DBNull.Value) return "border-default";
        return status.ToString() == "Completed" ? "border-completed" : "border-other";
    }

    public string GetPanelColor(int index)
    {
        return "bg-color-" + (index % 10);
    }

    private void LoadProjectDashboard()
    {
        string userid = this.SC.Userid;
        if (string.IsNullOrEmpty(userid)) return;
        string query = @"SELECT p.ProjectKey,p.ProjectName,p.StartDate,p.EndDate,p.EstimatedHours,p.Status AS ProjectStatus,

    -- Assigned Tasks
    (SELECT COUNT(*) FROM IT_TaskCreation tc WHERE tc.ProjectName = p.ProjectKey AND tc.EmployeeList = @UserId) AS AssignedTasks,

    -- In Progress
    (SELECT COUNT(*) FROM IT_TaskCreation tc WHERE tc.ProjectName = p.ProjectKey AND tc.EmployeeList = @UserId AND tc.Status = 2) AS InProgress,

    -- Pending
    (SELECT COUNT(*) FROM IT_TaskCreation tc WHERE tc.ProjectName = p.ProjectKey AND tc.EmployeeList = @UserId AND tc.Status <> 4) AS Pending,

    -- Completed
    (SELECT COUNT(*) FROM IT_TaskCreation tc WHERE tc.ProjectName = p.ProjectKey AND tc.EmployeeList = @UserId AND tc.Status = 4) AS Completed,

    -- Total Hours Spent (All Employees)
    ISNULL
    ((SELECT SUM (CASE WHEN tc.ActualHours IS NOT NULL AND tc.ActualHours > 0 THEN tc.ActualHours ELSE ISNULL(tc.AssignedHours,0) END) FROM IT_TaskCreation tc WHERE tc.ProjectName = p.ProjectKey), 0
    )
    +
    ISNULL
    (
        (
            SELECT SUM(ISNULL(tt.AssignedHours,0))
            FROM IT_Tasktesting tt
            WHERE tt.projectkey = p.ProjectKey
        ),
        0
    ) AS TotalHoursSpent,

    -- My Logged Hours
    ISNULL
    (
        (
            SELECT SUM
            (
                CASE
                    WHEN tc.ActualHours IS NOT NULL
                         AND tc.ActualHours > 0
                    THEN tc.ActualHours
                    ELSE ISNULL(tc.AssignedHours,0)
                END
            )
            FROM IT_TaskCreation tc
            WHERE tc.ProjectName = p.ProjectKey
              AND tc.EmployeeList = @UserId
        ),
        0
    )
    +
    ISNULL
    (
        (
            SELECT SUM(ISNULL(tt.AssignedHours,0))
            FROM IT_Tasktesting tt
            WHERE tt.projectkey = p.ProjectKey
              AND tt.assignedto = @UserId
        ),
        0
    ) AS MyLoggedHours

FROM IT_Projects p
WHERE EXISTS
(
    SELECT 1
    FROM IT_ProjectsParticipants pp
    WHERE pp.ProjectKey = p.ProjectKey
      AND pp.EmployeeKey = @UserId
)
OR EXISTS
(
    SELECT 1
    FROM IT_ProjectTeamLeads tl
    WHERE tl.ProjectKey = p.ProjectKey
      AND tl.EmployeeKey = @UserId
)
ORDER BY p.StartDate DESC";


        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@UserId", userid);
        DataTable dt = DA.GetDataTable(cmd);

        if (dt != null && dt.Rows.Count > 0)
        {
            DataView dvOngoing = new DataView(dt);
            dvOngoing.RowFilter = "ProjectStatus <> 'Completed'";
            rptOngoing.DataSource = dvOngoing;
            rptOngoing.DataBind();

            DataView dvCompleted = new DataView(dt);
            dvCompleted.RowFilter = "ProjectStatus = 'Completed'";
            rptCompleted.DataSource = dvCompleted;
            rptCompleted.DataBind();
        }
    }

    protected void rptOngoing_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

        DataRowView row = (DataRowView)e.Item.DataItem;

        Label lblProjectName = (Label)e.Item.FindControl("lbl_ProjectName");
        string projectName = row["ProjectName"].ToString();
        lblProjectName.Text = projectName;
        lblProjectName.ToolTip = projectName;

        ((Label)e.Item.FindControl("lbl_StartDate")).Text = row["StartDate"] != DBNull.Value ? Convert.ToDateTime(row["StartDate"]).ToString("dd/MM/yyyy") : "N/A";
        ((Label)e.Item.FindControl("lbl_EndDate")).Text = row["EndDate"] != DBNull.Value ? Convert.ToDateTime(row["EndDate"]).ToString("dd/MM/yyyy") : "N/A";
        ((Label)e.Item.FindControl("lbl_AssignedTasks")).Text = row["AssignedTasks"].ToString();
        ((Label)e.Item.FindControl("lbl_InProgress")).Text = row["InProgress"].ToString();
        ((Label)e.Item.FindControl("lbl_Pending")).Text = row["Pending"].ToString();
        ((Label)e.Item.FindControl("lbl_Completed")).Text = row["Completed"].ToString();
        ((Label)e.Item.FindControl("lbl_EstimatedHours")).Text = row["EstimatedHours"] != DBNull.Value ? row["EstimatedHours"].ToString() : "0";
        ((Label)e.Item.FindControl("lbl_UsedHours")).Text = row["TotalHoursSpent"].ToString();
        ((Label)e.Item.FindControl("lbl_myhours")).Text = row["MyLoggedHours"].ToString();

        Panel overduePanel = (Panel)e.Item.FindControl("pnl_OverdueIndicator");
        Panel ongoingPanel = (Panel)e.Item.FindControl("pnl_OngoingIndicator");
        System.Web.UI.HtmlControls.HtmlGenericControl projectCard = (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("divProjectCard");
        
        if (row["EndDate"] != DBNull.Value)
        {
            DateTime endDate = Convert.ToDateTime(row["EndDate"]);
            if (endDate < DateTime.Now)
            {
                overduePanel.Visible = true;
                if (projectCard != null)
                    projectCard.Attributes["class"] = projectCard.Attributes["class"] + " border-overdue";
            }
            else
            {
                ongoingPanel.Visible = true;
            }
        }
        else
        {
            ongoingPanel.Visible = true;
        }

        string projectKey = row["ProjectKey"] != DBNull.Value ? row["ProjectKey"].ToString() : "";
        ((HyperLink)e.Item.FindControl("lnk_Project")).NavigateUrl = "taskgrids.aspx?id=" + Server.UrlEncode(projectKey);
    }

    protected void rptCompleted_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

        DataRowView row = (DataRowView)e.Item.DataItem;

        Label lblProjectName = (Label)e.Item.FindControl("lbl_ProjectName");
        string projectName = row["ProjectName"].ToString();
        lblProjectName.Text = projectName;
        lblProjectName.ToolTip = projectName;

        ((Label)e.Item.FindControl("lbl_StartDate")).Text = row["StartDate"] != DBNull.Value ? Convert.ToDateTime(row["StartDate"]).ToString("dd/MM/yyyy") : "N/A";
        ((Label)e.Item.FindControl("lbl_EndDate")).Text = row["EndDate"] != DBNull.Value ? Convert.ToDateTime(row["EndDate"]).ToString("dd/MM/yyyy") : "N/A";
        ((Label)e.Item.FindControl("lbl_AssignedTasks")).Text = row["AssignedTasks"].ToString();
        ((Label)e.Item.FindControl("lbl_InProgress")).Text = row["InProgress"].ToString();
        ((Label)e.Item.FindControl("lbl_Pending")).Text = row["Pending"].ToString();
        ((Label)e.Item.FindControl("lbl_Completed")).Text = row["Completed"].ToString();
        ((Label)e.Item.FindControl("lbl_EstimatedHours")).Text = row["EstimatedHours"] != DBNull.Value ? row["EstimatedHours"].ToString() : "0";
        ((Label)e.Item.FindControl("lbl_UsedHours")).Text = row["TotalHoursSpent"].ToString();
        ((Label)e.Item.FindControl("lbl_myhours")).Text = row["MyLoggedHours"].ToString();

        string projectKey = row["ProjectKey"] != DBNull.Value ? row["ProjectKey"].ToString() : "";
        ((HyperLink)e.Item.FindControl("lnk_Project")).NavigateUrl = "taskgrids.aspx?id=" + Server.UrlEncode(projectKey);
    }
}
