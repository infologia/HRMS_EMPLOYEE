using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_WorkingDayViewDetails : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    CommonFunction CF;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();
        this.CF = new CommonFunction();


        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Employee Pay";
            BindYearDropdown();
            Load_WorkedHoursData();
            

            //HtmlAnchor control = this.Master.FindControl("li_dashboard") as HtmlAnchor;
            //if (control != null)
            //    control.Attributes.Add("class", "active");
        }


       

    }
    private void BindYearDropdown()
    {
        string query = @"SELECT DISTINCT [Year]  FROM IT_EmployeeWorkingDayDetails ORDER BY [Year] DESC";
        DataTable dtYear = DA.GetDataTable(query);

        ddlYear.Items.Clear();

        if (dtYear != null && dtYear.Rows.Count > 0)
        {
            ddlYear.DataSource = dtYear;
            ddlYear.DataTextField = "Year";
            ddlYear.DataValueField = "Year";
            ddlYear.DataBind();   
        }

        ddlYear.Items.Insert(0, new ListItem("All", "0")); 
        ddlYear.SelectedIndex = 0; 
    }


    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        Load_WorkedHoursData();
    }
    private void Load_WorkedHoursData()
    {
        string str_query;
        SqlCommand cmd = new SqlCommand();
        if (ddlYear.SelectedValue == "0")
        {
            str_query = "Select createdon,year,DateName( month , DateAdd( month , monthvalue , 0 ) - 1 ) as month,Numberofdaysinmonth,Numberofworkdaysinmonth,Numberofleavedaysinmonth,employeeworkingdaydetailskey from IT_EmployeeWorkingDayDetails order by createdon";

        }
        else
        {
            str_query = "Select createdon,year,DateName( month , DateAdd( month , monthvalue , 0 ) - 1 ) as month,Numberofdaysinmonth,Numberofworkdaysinmonth,Numberofleavedaysinmonth,employeeworkingdaydetailskey from IT_EmployeeWorkingDayDetails  WHERE [Year] = @Year order by createdon";
            cmd.Parameters.AddWithValue("@Year",
           Convert.ToInt32(ddlYear.SelectedValue));
        }

        cmd.CommandText = str_query;

        DataTable dt_dashboard = DA.GetDataTable(cmd);

        if (dt_dashboard != null && dt_dashboard.Rows.Count > 0)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(dt_dashboard.Copy());
            this.PH.LoadGridItem(ds, PH_EmployeeView, "WorkingDayViewDetails.txt", "");
        }
    }
}

