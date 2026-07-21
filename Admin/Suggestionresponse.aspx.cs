using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WEB_Admin_Suggestionresponse : System.Web.UI.Page
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
                control1.Text = "Employee Monitoring";
            Load_WorkedHoursData();
           
        }
        //String str_query = "SELECT username,fromdate,Todate,reason,approvedstatus,employeekey FROM IT_EmployeeLeaveDetails INNER JOIN IT_EmployeeRegister ON IT_EmployeeLeaveDetails.createdby = IT_EmployeeRegister.employeekey order by IT_EmployeeLeaveDetails.createdon ASC";
       // string str_query = "SELECT b.Employeeid,b.Firstname+' '+b.lastname as username,a.suggestioncategory,a.Suggestionresponse,a.reason,a.employeekey,a.SuggestionId,a.suggestionstatus,a.Suggestionkey FROM IT_Suggestion a left outer join IT_EmployeeRegister b ON a.createdby = b.Employeekey where roles='1' order by a.createdon ASC";filter
    }
    protected void ddlDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        Load_WorkedHoursData();
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        Load_WorkedHoursData();
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

    private void Load_WorkedHoursData()
    {
        string str_query = "SELECT b.Employeeid,b.Firstname+' '+b.lastname as username,a.suggestioncategory,a.Suggestionresponse,a.reason,a.employeekey,a.SuggestionId,a.suggestionstatus,a.Suggestionkey FROM IT_Suggestion a left outer join IT_EmployeeRegister b ON a.createdby = b.Employeekey where roles='1' and 1 = 1";
        
        SqlCommand cmd = new SqlCommand();

        int selectedYear = int.Parse(ddlYear.SelectedValue);
        str_query += " AND YEAR(a.createdon) = @YearValue";
        cmd.Parameters.AddWithValue("@YearValue", selectedYear);

        string selected = ddlDate.SelectedValue;

        if (selected == "1")
        {
            str_query += " AND CAST(a.createdon AS DATE) = CAST(GETDATE() AS DATE)";
        }
        else if (int.Parse(selected) >= 2)
        {
            int month = int.Parse(selected) - 1;
            str_query += " AND MONTH(a.createdon) = @MonthValue";
            cmd.Parameters.AddWithValue("@MonthValue", month);
        }
        
        str_query += " ORDER BY a.createdon ASC";
        
        cmd.CommandText = str_query;
        DataTable dt_dashboard = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_dashboard);
        if (dt_dashboard.Rows.Count > 0)
        {
            if (ds.Tables[0].Columns.Contains("suggestioncategory"))
                ds.Tables[0].Columns.Add("ActiveText");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int activetype = Convert.ToInt16(dr["suggestioncategory"].ToString());
                if (activetype == 1)
                    dr["ActiveText"] = "<span class='label label-info'>Management</span>";
                else if (activetype == 3)
                    dr["ActiveText"] = "<span class='label label-info'>Student Welfare</span>";

            }
            if (ds.Tables[0].Columns.Contains("suggestionstatus"))
                ds.Tables[0].Columns.Add("ActiveCategory");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                String str_reason = dr["Suggestionresponse"].ToString();
                int activetype = Convert.ToInt16(dr["suggestionstatus"].ToString());
                if (activetype == 1)
                    dr["ActiveCategory"] = "<span class='label label-info' title='" + str_reason + "'>Pending</span>";
                else if (activetype == 2)
                    dr["ActiveCategory"] = "<span class='label label-success' title='" + str_reason + "'>Approved</span>";
                else if (activetype == 3)
                    dr["ActiveCategory"] = "<span class='label label-danger' title='" + str_reason + "'>Rejected</span>";


            }
            this.PH.LoadGridItem(ds, PH_Suggestion, "Suggestion.txt", "");
        }
    }
}
