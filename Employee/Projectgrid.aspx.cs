using iTextSharp.tool.xml.html;
using iTextSharp.tool.xml.html.head;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_Projectgrid : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();

        if (!IsPostBack)
        {
            BindDateDropdown();
            PopulateYearDropdown();
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Project";
            Load_Projectdata();
        }
        
        string userRoleId = this.SC.UserRecordTable != null && this.SC.UserRecordTable.Rows.Count > 0
            ? this.SC.UserRecordTable.Rows[0]["Role"].ToString() : "";
        Create_Project.Visible = userRoleId == "11";
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        Load_Projectdata();
    }

    protected void ddlDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        Load_Projectdata();
    }

    private void BindDateDropdown()
    {
        ddlDate.Items.Clear();
        ddlDate.Items.Add(new ListItem("All", "0"));
        ddlDate.Items.Add(new ListItem("Today", "1"));

        for (int m = 1; m <= 12; m++)
        {
            int currentYear = DateTime.Now.Year;
            string monthName = new DateTime(currentYear, m, 1).ToString("MMMM");
            ddlDate.Items.Add(new ListItem(monthName, (m + 1).ToString()));
        }

        ddlDate.SelectedValue = "0";
    }

    private void PopulateYearDropdown()
    {
        ddlYear.Items.Clear();
        int currentYear = DateTime.Now.Year;

        for (int year = currentYear - 5; year <= currentYear + 5; year++)
        {
            ddlYear.Items.Add(new ListItem(year.ToString(), year.ToString()));
        }

        ListItem defaultYearItem = ddlYear.Items.FindByValue(currentYear.ToString());
        if (defaultYearItem != null)
            defaultYearItem.Selected = true;
    }

    private void Load_Projectdata()
    {
        string str_userid = this.SC.Userid;

        // Base query
        string baseQuery = "SELECT p.ProjectKey, p.ProjectCode, p.ProjectName, c.ClientName, " +
                           "CONVERT(VARCHAR(10), p.StartDate, 105) AS StartDate, " +
                           "CONVERT(VARCHAR(10), p.EndDate, 105) AS EndDate, p.Status " +
                           "FROM IT_projects p " +
                           "INNER JOIN IT_ClientDetails c ON p.ClientKey = c.ClientKey " +
                           "WHERE 1 = 1";

        // Build shared filter parameters
        int selectedYear = int.Parse(ddlYear.SelectedValue);
        string selected = ddlDate.SelectedValue;

        string dateFilter = " AND YEAR(p.StartDate) = @YearValue";
        if (selected == "1")
            dateFilter += " AND CAST(p.StartDate AS DATE) = CAST(GETDATE() AS DATE)";
        else if (int.Parse(selected) >= 2)
            dateFilter += " AND MONTH(p.StartDate) = @MonthValue";

        string activeQuery = baseQuery + dateFilter +
                             " AND p.Status IN ('Planned', 'In Progress')" +
                             " ORDER BY p.ProjectName";

        SqlCommand cmdActive = new SqlCommand(activeQuery);
        cmdActive.Parameters.AddWithValue("@YearValue", selectedYear);
        if (int.Parse(selected) >= 2)
            cmdActive.Parameters.AddWithValue("@MonthValue", int.Parse(selected) - 1);
        cmdActive.Parameters.AddWithValue("@createdby", str_userid);

        DataTable dtActive = DA.GetDataTable(cmdActive);

        DataSet dsActive = new DataSet();
        dsActive.Merge(dtActive);
        if (!dsActive.Tables[0].Columns.Contains("ActionText"))
            dsActive.Tables[0].Columns.Add("ActionText");

        string userRoleId = this.SC.UserRecordTable != null && this.SC.UserRecordTable.Rows.Count > 0
            ? this.SC.UserRecordTable.Rows[0]["Role"].ToString() : "";

        foreach (DataRow dr in dsActive.Tables[0].Rows)
        {
            string statusId = dr["Status"].ToString();
            dr["Status"] = "<span class=\"label label-info\">" + statusId + "</span>";
            
            if (userRoleId == "11")
            {
                dr["ActionText"] = "<ul class=\"icons-list\">" +
                                   "<li><a href=\"Project.aspx?id=" + dr["ProjectKey"].ToString() + "\" class=\"text-primary\" data-popup=\"tooltip\" title=\"Edit\"><i class=\"icon-pencil7\"></i></a></li>" +
                                   "<li><a href=\"javascript:void(0);\" class=\"text-danger\" onclick=\"fn_DeleteProject('" + dr["ProjectKey"].ToString() + "')\" data-popup=\"tooltip\" title=\"Delete\"><i class=\"icon-trash\"></i></a></li>" +
                                   "</ul>";
            }
            else
            {
                dr["ActionText"] = "<ul class=\"icons-list\">" +
                                   "<li><a href=\"Project.aspx?id=" + dr["ProjectKey"].ToString() + "\" class=\"text-primary\" data-popup=\"tooltip\" title=\"View\"><i class=\"icon-eye\"></i></a></li>" +
                                   "</ul>";
            }
        }

        this.PH.LoadGridItem(dsActive, PH_ActiveProjects, "Projectgrid.txt", "");

        // ── Grid 2: Completed ───────────────────────────────────────────────
        string completedQuery = baseQuery + dateFilter +
                                " AND p.Status = 'Completed'" +
                                " ORDER BY p.ProjectName";

        SqlCommand cmdCompleted = new SqlCommand(completedQuery);
        cmdCompleted.Parameters.AddWithValue("@YearValue", selectedYear);
        if (int.Parse(selected) >= 2)
            cmdCompleted.Parameters.AddWithValue("@MonthValue", int.Parse(selected) - 1);
        cmdCompleted.Parameters.AddWithValue("@createdby", str_userid);

        DataTable dtCompleted = DA.GetDataTable(cmdCompleted);

        DataSet dsCompleted = new DataSet();
        dsCompleted.Merge(dtCompleted);
        if (!dsCompleted.Tables[0].Columns.Contains("ActionText"))
            dsCompleted.Tables[0].Columns.Add("ActionText");

        foreach (DataRow dr in dsCompleted.Tables[0].Rows)
        {
            dr["Status"] = "<span class=\"label label-success\">Completed</span>";
            
            if (userRoleId == "11")
            {
                dr["ActionText"] = "<ul class=\"icons-list\">" +
                                   "<li><a href=\"Project.aspx?id=" + dr["ProjectKey"].ToString() + "\" class=\"text-primary\" data-popup=\"tooltip\" title=\"Edit\"><i class=\"icon-pencil7\"></i></a></li>" +
                                   "<li><a href=\"javascript:void(0);\" class=\"text-danger\" style=\"cursor:not-allowed; opacity:0.6;\" data-popup=\"tooltip\" title=\"Delete Disabled\"><i class=\"icon-trash\"></i></a></li>" +
                                   "</ul>";
            }
            else
            {
                dr["ActionText"] = "<ul class=\"icons-list\">" +
                                   "<li><a href=\"Project.aspx?id=" + dr["ProjectKey"].ToString() + "\" class=\"text-primary\" data-popup=\"tooltip\" title=\"View\"><i class=\"icon-eye\"></i></a></li>" +
                                   "</ul>";
            }
        }

        this.PH.LoadGridItem(dsCompleted, PH_CompletedProjects, "Projectgrid.txt", "");
    }


    [WebMethod]
    public static string DeleteProject(string str_projectkey)
    {
        try
        {
            DataAccess DA1 = new DataAccess();

            string q1 = "DELETE FROM IT_ProjectsParticipants WHERE ProjectKey=@ProjectKey";
            SqlCommand cmd1 = new SqlCommand(q1);
            cmd1.Parameters.AddWithValue("@ProjectKey", str_projectkey);
            DA1.ExecuteNonQuery(cmd1);

            string q2 = "DELETE FROM IT_projects WHERE ProjectKey=@ProjectKey";
            SqlCommand cmd2 = new SqlCommand(q2);
            cmd2.Parameters.AddWithValue("@ProjectKey", str_projectkey);
            DA1.ExecuteNonQuery(cmd2);

            return "1";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}
