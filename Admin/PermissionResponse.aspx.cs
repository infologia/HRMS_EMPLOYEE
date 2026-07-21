using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;

public partial class WEB_Admin_PermissionResponse : System.Web.UI.Page
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
    }
    private void BindDateDropdown()
    {
        ddlDate.Items.Clear();
        ddlDate.Items.Add(new ListItem("All", "0"));
        ddlDate.Items.Add(new ListItem("Today", "1"));
        int currentYear = DateTime.Now.Year;
        for (int m = 1; m <= 12; m++)
        {
            string monthName = new DateTime(currentYear, m, 1).ToString("MMMM");
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
        //int selectedYear = int.Parse(ddlYear.SelectedValue);
        Load_WorkedHoursData();


    }



    protected void ddlDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        Load_WorkedHoursData();   
    }

    private void Load_WorkedHoursData()
    {
        string query = @"SELECT b.Employeeid,b.Firstname + ' ' + b.lastname AS username,CONVERT(VARCHAR(10), a.Requestdate, 103) AS Requestdate,a.Fromtime,a.ToTime,a.responsestatus,a.reason,a.employeekey,a.Permissionhourse,a.Employeepermissiondetailskey,a.responsereason,CONVERT(VARCHAR(10), a.CreatedOn, 103) AS CreatedDate FROM IT_EmployeePermissionDetails a LEFT JOIN IT_EmployeeRegister b ON a.createdby = b.Employeekey WHERE 1 = 1 ";

        SqlCommand cmd = new SqlCommand();

        int selectedYear = int.Parse(ddlYear.SelectedValue);
        query += " AND YEAR(a.CreatedOn) = @YearValue";
        cmd.Parameters.AddWithValue("@YearValue", selectedYear);

        string selected = ddlDate.SelectedValue;

        if (selected == "1")
        {
            query += " AND CAST(a.CreatedOn AS DATE) = CAST(GETDATE() AS DATE)";
        }
       
        else if (int.Parse(selected) >= 2)
        {
            int month = int.Parse(selected) - 1;
            int year = DateTime.Now.Year;
            query += " AND MONTH(a.CreatedOn) = @MonthValue AND YEAR(a.CreatedOn)=@Year";
            cmd.Parameters.AddWithValue("@MonthValue", month);
            cmd.Parameters.AddWithValue("@Year", year);
        }

        query += " ORDER BY a.CreatedOn DESC";
        cmd.CommandText = query;

        DataTable dt = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt);

        
        if (!ds.Tables[0].Columns.Contains("ActiveText"))
            ds.Tables[0].Columns.Add("ActiveText");

       
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            int status = Convert.ToInt32(dr["responsestatus"]);
            string reason = dr["responsereason"].ToString();

            if (status == 1)
                dr["ActiveText"] = "<span class='label label-info' title='" + reason + "'>Pending</span>";
            else if (status == 2)
                dr["ActiveText"] = "<span class='label label-success' title='" + reason + "'>Approved</span>";
            else if (status == 3)
                dr["ActiveText"] = "<span class='label label-danger' title='" + reason + "'>Rejected</span>";
        }

        this.PH.LoadGridItem(ds, PH_Permission, "Permissionresponse.txt", "");
    }



}
