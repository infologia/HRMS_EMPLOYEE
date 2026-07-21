using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_WorkedHoursDetails : System.Web.UI.Page
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
            BindYearDropdown();
            Load_WorkedHoursData();
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Working days";
        }
       
        String str_userid = this.SC.Userid;
        string str_query = " SELECT 'SUM' outtime, COUNT(a.outtime) workingday,b.Numberofworkdaysinmonth,b.year,b.monthvalue,b.Numberofworkdaysinmonth-count(a.outtime) as workeddays,c.Firstname+' '+ c.Lastname name FROM IT_InOutTime a left outer join  IT_EmployeeWorkingDayDetails b on (month(a.intime))=b.monthvalue left outer join IT_EmployeeRegister c " +
                            "on a.Employeekey = c.Employeekey where a.Employeekey= @Employeekey and year='"+ddlYear.SelectedValue+"' and a.OutTime is not null group by b.Numberofworkdaysinmonth,b.year,b.monthvalue,c.Firstname,c.Lastname";
        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@employeekey", str_userid);

        DataTable dt_dashboard = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();

        ds.Merge(dt_dashboard);
        if (dt_dashboard.Rows.Count > 0)
        {
            this.PH.LoadGridItem(ds, PH_leave, "Workedhoursview.txt", "");
        }

    }
    private void BindYearDropdown()
    {
        DataTable dt_year = DA.GetDataTable("SELECT DISTINCT (b.year) year FROM IT_InOutTime a left outer join  IT_EmployeeWorkingDayDetails b on (month(a.intime))=b.monthvalue where a.Employeekey='"+this.SC.Userid+"' order by b.Year desc");
        if (dt_year != null && dt_year.Rows.Count > 0)
        {
            ddlYear.DataSource = dt_year;
            ddlYear.DataTextField = "year";
            ddlYear.DataValueField = "year";
            ddlYear.DataBind();        
        }      
    }
    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        Load_WorkedHoursData();
       
    }

    private void Load_WorkedHoursData()
    {
        string selectedYear = ddlYear.SelectedValue;

        string query = "SELECT DISTINCT (b.year) FROM IT_InOutTime a left outer join  IT_EmployeeWorkingDayDetails b on (month(a.intime))=b.monthvalue where a.Employeekey= @Employeekey order by b.Year desc";

        if (selectedYear != "0")
        {
            query += " AND Year = @Year";
        }

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@Employeekey", this.SC.Userid);

        if (selectedYear != "0")
        {
            cmd.Parameters.AddWithValue("@Year", selectedYear);
        }


    }


}