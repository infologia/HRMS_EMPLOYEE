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
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Assigned Projects";
            LoadProjectDashboard();
        }
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

        string query = @"
SELECT
    p.ProjectKey,
    p.ProjectName,
    p.StartDate,
    p.EndDate,
    p.EstimatedHours,
    p.Status AS ProjectStatus,
    ISNULL(pt.name, 'N/A') AS ProjectType,

    -- All Task counts (from TaskDescriptiondetails)
    (SELECT COUNT(*) FROM IT_TaskDescriptiondetails td 
     INNER JOIN IT_TaskCreation tc ON td.TaskKey = tc.TaskKey 
     WHERE tc.ProjectName = p.ProjectKey) AS AllOverall,
    (SELECT COUNT(DISTINCT pp.EmployeeKey) 
     FROM IT_ProjectsParticipants pp 
     WHERE pp.ProjectKey = p.ProjectKey) AS AllAssignedEmployees,
    (SELECT COUNT(*) FROM IT_TaskDescriptiondetails td 
     INNER JOIN IT_TaskCreation tc ON td.TaskKey = tc.TaskKey 
     WHERE tc.ProjectName = p.ProjectKey AND td.Status = 1) AS AllAssigned,
    (SELECT COUNT(*) FROM IT_TaskDescriptiondetails td 
     INNER JOIN IT_TaskCreation tc ON td.TaskKey = tc.TaskKey 
     WHERE tc.ProjectName = p.ProjectKey AND td.Status = 2) AS AllInProgress,
    (SELECT COUNT(*) FROM IT_TaskDescriptiondetails td 
     INNER JOIN IT_TaskCreation tc ON td.TaskKey = tc.TaskKey 
     WHERE tc.ProjectName = p.ProjectKey AND td.Status = 4) AS AllCompleted,
    (SELECT COUNT(*) FROM IT_TaskDescriptiondetails td 
     INNER JOIN IT_TaskCreation tc ON td.TaskKey = tc.TaskKey 
     WHERE tc.ProjectName = p.ProjectKey AND td.Status <> 4 AND tc.StartDate < CAST(GETDATE() AS DATE)) AS AllOverdue,

    -- My Task counts (from TaskDescriptiondetails for specific employee)
    (SELECT COUNT(*) FROM IT_TaskDescriptiondetails td 
     INNER JOIN IT_TaskCreation tc ON td.TaskKey = tc.TaskKey 
     WHERE tc.ProjectName = p.ProjectKey AND tc.EmployeeList = @UserId) AS MyOverall,
    (SELECT COUNT(*) FROM IT_TaskDescriptiondetails td 
     INNER JOIN IT_TaskCreation tc ON td.TaskKey = tc.TaskKey 
     WHERE tc.ProjectName = p.ProjectKey AND tc.EmployeeList = @UserId AND td.Status = 2) AS MyInProgress,
    (SELECT COUNT(*) FROM IT_TaskDescriptiondetails td 
     INNER JOIN IT_TaskCreation tc ON td.TaskKey = tc.TaskKey 
     WHERE tc.ProjectName = p.ProjectKey AND tc.EmployeeList = @UserId AND td.Status = 4) AS MyCompleted,
    (SELECT COUNT(*) FROM IT_TaskDescriptiondetails td 
     INNER JOIN IT_TaskCreation tc ON td.TaskKey = tc.TaskKey 
     WHERE tc.ProjectName = p.ProjectKey AND tc.EmployeeList = @UserId AND td.Status <> 4 AND tc.StartDate < CAST(GETDATE() AS DATE)) AS MyOverdue,
    (SELECT COUNT(*) FROM IT_TaskDescriptiondetails td 
     INNER JOIN IT_TaskCreation tc ON td.TaskKey = tc.TaskKey 
     WHERE tc.ProjectName = p.ProjectKey AND tc.EmployeeList = @UserId AND td.Status = 1) AS MyAssigned,

    -- Total Hours Spent (from TaskDescriptiondetails)
    ISNULL((SELECT SUM(CASE WHEN td.ActualHours IS NOT NULL AND td.ActualHours > 0 THEN td.ActualHours ELSE ISNULL(td.AssignedHours,0) END)
            FROM IT_TaskDescriptiondetails td 
            INNER JOIN IT_TaskCreation tc ON td.TaskKey = tc.TaskKey 
            WHERE tc.ProjectName = p.ProjectKey), 0) AS TotalHoursSpent,

    -- Current Month Hours Spent (for Monthly Support Contract)
    ISNULL((SELECT SUM(CASE WHEN td.ActualHours IS NOT NULL AND td.ActualHours > 0 THEN td.ActualHours ELSE ISNULL(td.AssignedHours,0) END)
            FROM IT_TaskDescriptiondetails td 
            INNER JOIN IT_TaskCreation tc ON td.TaskKey = tc.TaskKey 
            WHERE tc.ProjectName = p.ProjectKey 
            AND MONTH(tc.StartDate) = MONTH(GETDATE()) 
            AND YEAR(tc.StartDate) = YEAR(GETDATE())), 0) AS CurrentMonthHoursSpent,

    -- My Logged Hours (from TaskDescriptiondetails for specific employee)
    ISNULL((SELECT SUM(CASE WHEN td.ActualHours IS NOT NULL AND td.ActualHours > 0 THEN td.ActualHours ELSE ISNULL(td.AssignedHours,0) END)
            FROM IT_TaskDescriptiondetails td 
            INNER JOIN IT_TaskCreation tc ON td.TaskKey = tc.TaskKey 
            WHERE tc.ProjectName = p.ProjectKey AND tc.EmployeeList = @UserId), 0) AS MyLoggedHours,

    -- My Current Month Hours (for Monthly Support Contract)
    ISNULL((SELECT SUM(CASE WHEN td.ActualHours IS NOT NULL AND td.ActualHours > 0 THEN td.ActualHours ELSE ISNULL(td.AssignedHours,0) END)
            FROM IT_TaskDescriptiondetails td 
            INNER JOIN IT_TaskCreation tc ON td.TaskKey = tc.TaskKey 
            WHERE tc.ProjectName = p.ProjectKey AND tc.EmployeeList = @UserId 
            AND MONTH(tc.StartDate) = MONTH(GETDATE()) 
            AND YEAR(tc.StartDate) = YEAR(GETDATE())), 0) AS MyCurrentMonthHours,

    (SELECT TOP 1 FilePath FROM IT_ProjectDocuments WHERE ProjectKey = p.ProjectKey AND DocumentName LIKE '%workflow%') AS WorkflowDocumentPath,
    (SELECT COUNT(*) FROM IT_EmployeeRegister er WHERE er.Employeekey = @UserId AND er.Division = 1) AS IsTeamLead

FROM IT_Projects p
LEFT JOIN ProjectType pt ON p.ProjectTypeId = pt.id
WHERE EXISTS (SELECT 1 FROM IT_ProjectsParticipants pp WHERE pp.ProjectKey = p.ProjectKey AND pp.EmployeeKey = @UserId)
   OR EXISTS (SELECT 1 FROM IT_ProjectTeamLeads tl WHERE tl.ProjectKey = p.ProjectKey AND tl.EmployeeKey = @UserId)
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

    private void BindCommonFields(RepeaterItemEventArgs e, DataRowView row, bool isOngoing)
    {
        string projectName = row["ProjectName"].ToString();
        string projectType = row["ProjectType"].ToString();
        Label lblProjectName = (Label)e.Item.FindControl("lbl_ProjectName");
        lblProjectName.Text = projectName;
        lblProjectName.ToolTip = projectName;

        Label lblProjectType = (Label)e.Item.FindControl("lbl_ProjectType");
        if (lblProjectType != null)
        {
            lblProjectType.Text = projectType;
        }

        // Hours Warning
        Panel pnlHoursWarning = (Panel)e.Item.FindControl("pnl_HoursWarning");
        if (pnlHoursWarning != null)
        {
            decimal estimatedHours = row["EstimatedHours"] != DBNull.Value ? Convert.ToDecimal(row["EstimatedHours"]) : 0;
            decimal usedHours = 0;
            
            if (projectType == "Monthly Support Contract")
            {
                usedHours = row["CurrentMonthHoursSpent"] != DBNull.Value ? Convert.ToDecimal(row["CurrentMonthHoursSpent"]) : 0;
            }
            else
            {
                usedHours = row["TotalHoursSpent"] != DBNull.Value ? Convert.ToDecimal(row["TotalHoursSpent"]) : 0;
            }
            
            if (usedHours > estimatedHours && estimatedHours > 0)
            {
                pnlHoursWarning.Visible = true;
            }
        }

        string startDate = row["StartDate"] != DBNull.Value ? Convert.ToDateTime(row["StartDate"]).ToString("dd/MM/yyyy") : "N/A";
        string endDate   = row["EndDate"]   != DBNull.Value ? Convert.ToDateTime(row["EndDate"]).ToString("dd/MM/yyyy")   : "N/A";

        ((Label)e.Item.FindControl("lbl_StartDate")).Text = startDate;
        ((Label)e.Item.FindControl("lbl_EndDate")).Text   = endDate;

        if (isOngoing)
        {
            // Second pair of date labels inside pnl_OngoingIndicator
            Label lbl2 = (Label)e.Item.FindControl("lbl_StartDate2");
            Label lbl3 = (Label)e.Item.FindControl("lbl_EndDate2");
            if (lbl2 != null) lbl2.Text = startDate;
            if (lbl3 != null) lbl3.Text = endDate;
        }

        string workflowDocPath = row["WorkflowDocumentPath"] != DBNull.Value ? row["WorkflowDocumentPath"].ToString() : "";
        
        if (!string.IsNullOrEmpty(workflowDocPath))
        {
            // Ensure path starts with ~/ if it's a relative path without it
            if (!workflowDocPath.StartsWith("~") && !workflowDocPath.StartsWith("/") && !workflowDocPath.StartsWith("http"))
            {
                workflowDocPath = "~/" + workflowDocPath;
            }
            // Replace spaces with %20 to avoid 404 errors in IIS
            workflowDocPath = workflowDocPath.Replace(" ", "%20");
        }

        bool isTeamLead = Convert.ToInt32(row["IsTeamLead"]) > 0;

        HyperLink lnkWorkflow = (HyperLink)e.Item.FindControl("lnk_Workflow");
        if (lnkWorkflow != null)
        {
            if (!string.IsNullOrEmpty(workflowDocPath) && isTeamLead)
            {
                lnkWorkflow.NavigateUrl = workflowDocPath;
                lnkWorkflow.Visible = true;
            }
            else
            {
                lnkWorkflow.Visible = false;
            }
        }

        ((Label)e.Item.FindControl("lbl_EstimatedHours")).Text   = row["EstimatedHours"] != DBNull.Value ? row["EstimatedHours"].ToString() : "0";
        
        if (projectType == "Monthly Support Contract")
        {
            ((Label)e.Item.FindControl("lbl_UsedHours")).Text = row["CurrentMonthHoursSpent"].ToString();
            ((Label)e.Item.FindControl("lbl_myhours")).Text = row["MyCurrentMonthHours"].ToString();
        }
        else
        {
            ((Label)e.Item.FindControl("lbl_UsedHours")).Text = row["TotalHoursSpent"].ToString();
            ((Label)e.Item.FindControl("lbl_myhours")).Text = row["MyLoggedHours"].ToString();
        }

        // All Task Details
        ((Label)e.Item.FindControl("lbl_AllOverall")).Text   = row["AllOverall"].ToString();
        ((Label)e.Item.FindControl("lbl_AllAssignedEmpCount")).Text = row["AllAssignedEmployees"].ToString();
        ((Label)e.Item.FindControl("lbl_AllAssignedCount")).Text = row["AllAssigned"].ToString();
        ((Label)e.Item.FindControl("lbl_InProgress")).Text   = row["AllInProgress"].ToString();
        ((Label)e.Item.FindControl("lbl_Completed")).Text    = row["AllCompleted"].ToString();
        ((Label)e.Item.FindControl("lbl_Pending")).Text      = row["AllOverdue"].ToString();

        // My Task Details
        ((Label)e.Item.FindControl("lbl_MyOverall")).Text    = row["MyOverall"].ToString();
        ((Label)e.Item.FindControl("lbl_MyOngoing")).Text    = row["MyInProgress"].ToString();
        ((Label)e.Item.FindControl("lbl_MyCompleted")).Text  = row["MyCompleted"].ToString();
        ((Label)e.Item.FindControl("lbl_MyOverdue")).Text    = row["MyOverdue"].ToString();
        ((Label)e.Item.FindControl("lbl_MyAssignedCount")).Text = row["MyAssigned"].ToString();

        string projectKey = row["ProjectKey"] != DBNull.Value ? row["ProjectKey"].ToString() : "";
        ((HyperLink)e.Item.FindControl("lnk_Project")).NavigateUrl = "newtaskgrids.aspx?id=" + Server.UrlEncode(projectKey);
    }

    protected void rptOngoing_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;

        DataRowView row = (DataRowView)e.Item.DataItem;
        BindCommonFields(e, row, true);

        Panel overduePanel = (Panel)e.Item.FindControl("pnl_OverdueIndicator");
        Panel ongoingPanel = (Panel)e.Item.FindControl("pnl_OngoingIndicator");
        System.Web.UI.HtmlControls.HtmlGenericControl projectCard =
            (System.Web.UI.HtmlControls.HtmlGenericControl)e.Item.FindControl("divProjectCard");

        if (row["EndDate"] != DBNull.Value && Convert.ToDateTime(row["EndDate"]) < DateTime.Now)
        {
            overduePanel.Visible = true;
            if (projectCard != null)
                projectCard.Attributes["class"] += " border-overdue";
        }
        else
        {
            ongoingPanel.Visible = true;
        }
    }

    protected void rptCompleted_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem) return;
        DataRowView row = (DataRowView)e.Item.DataItem;
        BindCommonFields(e, row, false);
    }
}
