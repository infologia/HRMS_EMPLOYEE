using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_UpdateWorkingDayDetails : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    CommonFunction CF;
    string str_id = "";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.CF = new CommonFunction();


        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Employee Pay";

            //HtmlAnchor control = this.Master.FindControl("li_dashboard") as HtmlAnchor;
            //if (control != null)
            //    control.Attributes.Add("class", "active");
        }


        if(!IsPostBack)
        {
            CF.LoadYearToDropdown(ddl_year);
            CF.LoadMonthToDropdown(ddl_month);
        }
      

        if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
        {

            this. str_id = Request.QueryString["id"].ToString();
        }

      
         this.Loadlanguage();
    }
 private void Loadlanguage()
    
 {
        string str_query = "select * from IT_EmployeeWorkingDayDetails  where Employeeworkingdaydetailskey=@Employeeworkingdaydetailskey";
        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@Employeeworkingdaydetailskey", str_id);
        DataTable dt_dashboard = DA.GetDataTable(cmd);
        if (dt_dashboard.Rows.Count > 0)
        {
            ddl_year.SelectedValue = dt_dashboard.Rows[0]["year"].ToString();
            ddl_month.SelectedValue = dt_dashboard.Rows[0]["monthvalue"].ToString();
            txt_days.Text = dt_dashboard.Rows[0]["numberofdaysinmonth"].ToString();
            txt_work.Text = dt_dashboard.Rows[0]["numberofworkdaysinmonth"].ToString();
           

        }
    }

 

    protected void btn_update_Click(object sender, EventArgs e)
    {
        string str_sql = "update IT_EmployeeWorkingDayDetails set createdby=@createdby,createdon=@createdon,year=@year,monthvalue=@monthvalue,numberofdaysinmonth=@numberofdaysinmonth,numberofworkdaysinmonth=@numberofworkdaysinmonth,numberofleavedaysinmonth=@numberofleavedaysinmonth";
        SqlCommand cmd = new SqlCommand(str_sql);
       cmd.Parameters.AddWithValue("@year", ddl_year.SelectedValue);
        cmd.Parameters.AddWithValue("@monthvalue", ddl_month.SelectedValue);
        cmd.Parameters.AddWithValue("@numberofdaysinmonth", txt_days.Text);
        cmd.Parameters.AddWithValue("@numberofworkdaysinmonth", txt_work.Text);
         DA.ExecuteNonQuery(cmd);
        Response.Redirect("~/admin/WorkingDayViewDetails.aspx");
    }


}
