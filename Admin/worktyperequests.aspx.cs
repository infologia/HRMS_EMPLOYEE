using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Admin_worktyperequests : System.Web.UI.Page
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
            if (Request.QueryString["delete"] != null)
            {
                DeleteWorkTypeRequest(Request.QueryString["delete"]);
                return;
            }

            BindDateDropdown();
            PopulateYearDropdown();

            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Work Type Requests";

            LoadWorkTypeRequests();
        }
    }

    private void DeleteWorkTypeRequest(string requestId)
    {
        try
        {
            string query = "DELETE FROM IT_WorkTypeRequest WHERE WR_Id = @RequestId";
            SqlCommand cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@RequestId", requestId);
            DA.ExecuteNonQuery(cmd);

            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_success", "showToastr('success','Request removed successfully!');setTimeout(function(){ window.location.href = 'worktyperequests.aspx'; }, 2000);", true);
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "toastr_error", "showToastr('error','Error: " + ex.Message.Replace("'", "\\'" ) + "');", true);
        }
    }

    private void BindDateDropdown()
    {
        ddlDate.Items.Clear();
        ddlDate.Items.Add(new ListItem("All", "0"));
        ddlDate.Items.Add(new ListItem("Today", "1"));

        for (int m = 1; m <= 12; m++)
        {
            string monthName = new DateTime(2025, m, 1).ToString("MMMM");
            ddlDate.Items.Add(new ListItem(monthName, (m + 1).ToString()));
        }

        int currentMonthValue = DateTime.Now.Month + 1;
        ddlDate.SelectedValue = currentMonthValue.ToString();
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

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadWorkTypeRequests();
    }

    protected void ddlDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadWorkTypeRequests();
    }

    private void LoadWorkTypeRequests()
    {
        string query = @"SELECT 
    r.WR_Id,
    e.Employeeid,
    e.Firstname + ' ' + e.Lastname AS username,
    w.WT_TypeName AS WorkTypeName,
    CONVERT(VARCHAR(10), r.FromDate, 103) AS fromdate,
    CONVERT(VARCHAR(10), r.ToDate, 103) AS Todate,
    r.Reason,
    CONVERT(VARCHAR(10), r.CreatedOn, 103) AS CreatedDate
FROM IT_WorkTypeRequest r
LEFT JOIN IT_EmployeeRegister e ON r.EmployeeKey = e.Employeekey
LEFT JOIN IT_WorkType w ON r.WorkTypeId = w.WT_Id
WHERE 1 = 1";

        SqlCommand cmd = new SqlCommand();

        int selectedYear = int.Parse(ddlYear.SelectedValue);
        query += " AND YEAR(r.CreatedOn) = @YearValue";
        cmd.Parameters.AddWithValue("@YearValue", selectedYear);

        string selected = ddlDate.SelectedValue;

        if (selected == "1")
        {
            query += " AND CAST(r.CreatedOn AS DATE) = CAST(GETDATE() AS DATE)";
        }
        else if (int.Parse(selected) >= 2)
        {
            int month = int.Parse(selected) - 1;
            query += " AND MONTH(r.CreatedOn) = @MonthValue";
            cmd.Parameters.AddWithValue("@MonthValue", month);
        }

        query += " ORDER BY r.CreatedOn DESC";

        cmd.CommandText = query;

        DataTable dt = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt);

        PH.LoadGridItem(ds, PH_WorkType, "worktyperequest.txt", "");
    }
}
