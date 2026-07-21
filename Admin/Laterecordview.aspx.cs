using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;
using System.Web.UI.HtmlControls;
using System.Globalization;



public partial class Laterecordview : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    int activetype;
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
                control1.Text = "Employee Monitoring";

            HtmlAnchor control = this.Master.FindControl("li_dashboard") as HtmlAnchor;
            if (control != null)
                control.Attributes.Add("class", "active");

            Load_WorkedHoursData();
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
        Load_WorkedHoursData();
    }

    protected void ddlDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        Load_WorkedHoursData();
    }
  
    private void Load_WorkedHoursData()
    {
        string str_query = @"SELECT b.Employeeid, b.Firstname+' '+b.lastname AS username, CONVERT(varchar(20), a.Requestdate, 103) AS Requestdate, a.Fromtime, a.ToTime, a.reason, a.employeekey, a.Permissionhourse, a.LatePermissionDetailskey, a.responsereason, a.Responsestatus FROM IT_LatePermissionDetails a LEFT JOIN IT_EmployeeRegister b ON a.createdby = b.Employeekey WHERE 1 = 1";

        SqlCommand cmd = new SqlCommand();

        int selectedYear = int.Parse(ddlYear.SelectedValue);
        str_query += " AND YEAR(a.Requestdate) = @YearValue";
        cmd.Parameters.AddWithValue("@YearValue", selectedYear);

        string selected = ddlDate.SelectedValue;

        if (selected == "1")
        {
            str_query += " AND CAST(a.Requestdate AS DATE) = CAST(GETDATE() AS DATE)";
        }
        else if (int.Parse(selected) >= 2)
        {
            int month = int.Parse(selected) - 1;
            str_query += " AND MONTH(a.Requestdate) = @MonthValue";
            cmd.Parameters.AddWithValue("@MonthValue", month);
        }
       
        str_query += " ORDER BY a.createdon ASC";

        cmd.CommandText = str_query;
        DataTable dt_dashboard = DA.GetDataTable(cmd);

        DataSet ds = new DataSet();
        ds.Merge(dt_dashboard);

        if (dt_dashboard.Rows.Count > 0)
        {
            if (!ds.Tables[0].Columns.Contains("ActiveText"))
                ds.Tables[0].Columns.Add("ActiveText");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int activetype = 0;
                string str_reason = dr["responsereason"].ToString();
                string statusString = dr["Responsestatus"].ToString();

                if (!string.IsNullOrEmpty(statusString))
                    activetype = Convert.ToInt32(statusString);

                if (activetype == 1)
                    dr["ActiveText"] = "<span class='label label-info' title='" + str_reason + "'>Pending</span>";
                else if (activetype == 2)
                    dr["ActiveText"] = "<span class='label label-success' title='" + str_reason + "'>Approved</span>";
                else if (activetype == 3)
                    dr["ActiveText"] = "<span class='label label-danger' title='" + str_reason + "'>Rejected</span>";
            }

            PH.LoadGridItem(ds, PH_Permission, "Laterecords.txt", "");
        }
    }

}

