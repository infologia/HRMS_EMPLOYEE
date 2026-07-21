using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class WEB_Employee_DashBoard : System.Web.UI.Page
{
    SessionCustom SC;
    DataAccess DA;
    PhTemplate PH;
    CommonFunction CF;
    string str_userkey = "";
    string str_CurrentDate = "";
    string str_addoneday = "";
	string str_userid = "";
	string str_intime = "";
	string str_intimecheck;

	protected void Page_Load(object sender, EventArgs e)
    {
      
        this.SC = new SessionCustom();
        this.DA = new DataAccess();
        this.PH = new PhTemplate();
        this.CF = new CommonFunction();
        this.str_userkey = SC.Userid;

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Dashboard";
        }

        DateTime date = DateTime.UtcNow;
        string date1 = date.ToString("yyyy-MM-dd");
		string dateorchart = date.ToString("dd/MM/yyyy");
		this.str_CurrentDate = date1 + " " + "00:00:00";
        this.str_addoneday = DateTime.UtcNow.AddDays(+1).ToString("yyyy-MM-dd");
        this.str_addoneday = str_addoneday + " " + "00:00:00";



		var month = DateTime.Now.Month.ToString();
        string str_dashboard = "select  Numberofworkdaysinmonth from IT_EmployeeWorkingDayDetails where Year=Year(GETDATE()) and monthvalue=Month(GETDATE()) ";
        SqlCommand cmd = new SqlCommand(str_dashboard);
       
        DataTable dt_ashboard = DA.GetDataTable(cmd);
        if (dt_ashboard.Rows.Count > 0)
        {
            string str_leave1 = dt_ashboard.Rows[0]["Numberofworkdaysinmonth"].ToString();
            decimal dec_leave = Convert.ToDecimal(str_leave1);
            decimal dec_avg = Math.Round(dec_leave / 30, 2);
            lbl_Workingdays.Text = str_leave1;

        }

        DataTable dt_workeddaysmonth = DA.GetDataTable("select * from it_inouttime where employeekey='"+this.str_userkey+"' and Month(OutTime) = Month(getdate()) ");
        if (dt_workeddaysmonth.Rows.Count > 0)
        {
            lbl_Workingdaysavg.Text = dt_workeddaysmonth.Rows.Count.ToString();
        }

        DataTable dt_workeddaysyear = DA.GetDataTable("select * from it_inouttime where employeekey='" + this.str_userkey + "' and Year(OutTime) = Year(getdate()) ");
        if (dt_workeddaysyear.Rows.Count > 0)
        {
            lbl_WorkedDaysyear.Text = dt_workeddaysyear.Rows.Count.ToString();
        }


        DataTable dt_hours = DA.GetDataTable("select sum(cast(numberofworkdaysinmonth as int)) as numberofworkdaysinmonth from IT_EmployeeWorkingDayDetails where  year = year(getdate())");
        if (dt_hours.Rows.Count > 0)
        {
            lbl_workday.Text = dt_hours.Rows[0]["numberofworkdaysinmonth"].ToString();
        }

        var minutes = DateTime.Now.Month.ToString();

        string str_mhours = "select sum(cast(Numberofworkdaysinmonth as int)) as Numberofworkdaysinmonth from IT_EmployeeWorkingDayDetails where monthvalue = month(getdate()) and year = year(getdate())";

        SqlCommand sc = new SqlCommand(str_mhours);

        DataTable dt_mhours = DA.GetDataTable(sc);
        if (dt_mhours.Rows.Count > 0)
        {
            string str_leave1 = dt_mhours.Rows[0]["Numberofworkdaysinmonth"].ToString();
            decimal dec_leave = Convert.ToDecimal(str_leave1);
            decimal result = dec_leave * 8;
            string str_workhours = Convert.ToString(result);
            lbl_whours.Text = str_workhours;
        }


        var hoursecount = DateTime.Now.Month.ToString();

        string qMonthWorkingDays = @"SELECT ISNULL(Numberofworkdaysinmonth,0) FROM IT_EmployeeWorkingDayDetails WHERE monthvalue = MONTH(GETDATE()) AND year = YEAR(GETDATE())";
        SqlCommand cmdMonthWD = new SqlCommand(qMonthWorkingDays);
        DataTable dtMonthWD = DA.GetDataTable(cmdMonthWD);

        int monthWorkingDays = 0;
        if (dtMonthWD.Rows.Count > 0)
        {
            monthWorkingDays = Convert.ToInt32(dtMonthWD.Rows[0][0]);
        }

        lbl_whours.Text = (monthWorkingDays * 8).ToString();

        string qMonthHours = @"SELECT ISNULL(SUM(DATEDIFF(MINUTE,0,WorkingHours)),0)/60 AS Hours,ISNULL(SUM(DATEDIFF(MINUTE,0,WorkingHours)),0)%60 AS Minutes FROM IT_inouttime WHERE Employeekey=@Employeekey AND MONTH(Intime)=MONTH(GETDATE()) AND YEAR(Intime)=YEAR(GETDATE())";
        SqlCommand cmdMonthHours = new SqlCommand(qMonthHours);
        cmdMonthHours.Parameters.AddWithValue("@Employeekey", str_userkey);

        DataTable dtMonth = DA.GetDataTable(cmdMonthHours);

        string actualMonthHours = "0 hours 0 minutes";
        if (dtMonth.Rows.Count > 0)
        {
            actualMonthHours =
                dtMonth.Rows[0]["Hours"] + " hours " +
                dtMonth.Rows[0]["Minutes"] + " minutes";
        }

        lbl_test.Text = actualMonthHours;
  
        string qYearWorkingDays = @"SELECT ISNULL(SUM(CAST(Numberofworkdaysinmonth AS INT)),0) FROM IT_EmployeeWorkingDayDetails WHERE year = YEAR(GETDATE())";

        SqlCommand cmdYearWD = new SqlCommand(qYearWorkingDays);
        DataTable dtYearWD = DA.GetDataTable(cmdYearWD);

        int yearWorkingDays = 0;
        if (dtYearWD.Rows.Count > 0)
        {
            yearWorkingDays = Convert.ToInt32(dtYearWD.Rows[0][0]);
        }

        lbl_totalhours.Text = (yearWorkingDays * 8).ToString(); 

        string qYearHours = @"SELECT ISNULL(SUM(DATEDIFF(MINUTE,0,WorkingHours)),0)/60 AS Hours,ISNULL(SUM(DATEDIFF(MINUTE,0,WorkingHours)),0)%60 AS Minutes FROM IT_inouttime WHERE Employeekey=@Employeekey AND YEAR(Intime)=YEAR(GETDATE())";

        SqlCommand cmdYearHours = new SqlCommand(qYearHours);
        cmdYearHours.Parameters.AddWithValue("@Employeekey", str_userkey);

        DataTable dtYear = DA.GetDataTable(cmdYearHours);
        if (dtYear.Rows.Count > 0)
        {
            Label4.Text = dtYear.Rows[0]["Hours"] + " hours " +
                          dtYear.Rows[0]["Minutes"] + " minutes";
        }


        var month1 = DateTime.Now.Month.ToString();
        string str_dashboard1 = @"
    SELECT ISNULL(SUM(CAST(LeaveDays AS DECIMAL(5,2))), 0) AS LeaveDays, Employeekey
    FROM IT_EmployeeLeaveDetails
    WHERE Responsestatus = '2'
      AND Employeekey = @Employeekey
      AND MONTH(Todate) = MONTH(GETDATE())
      AND YEAR(Todate) = YEAR(GETDATE())
    GROUP BY Employeekey";

        SqlCommand cmd1 = new SqlCommand(str_dashboard1);
        cmd1.Parameters.AddWithValue("@Employeekey", str_userkey);

        DataTable dt_ashboard1 = DA.GetDataTable(cmd1);

        if (dt_ashboard1.Rows.Count > 0)
        {
            decimal dec_leave = Convert.ToDecimal(dt_ashboard1.Rows[0]["LeaveDays"]);
            lbl_Totalleave.Text = dec_leave.ToString("0.##"); // displays 4.5 instead of 4
        }
        else
        {
            lbl_Totalleave.Text = "0";
        }

        string str_intime = "00:00:00";
        string str_outtime = "00:00:00";
        string str_dashboard2 = "select FORMAT(InTime,'hh:mm tt') AS InTime , FORMAT(OutTime,'hh:mm tt') AS OutTime,CONVERT(Varchar,createdon,103)as Date from IT_InOutTime where Createdby=@Createdby and Createdon between @createdonfrom and @createdonto";
        SqlCommand cmd2 = new SqlCommand(str_dashboard2);
        cmd2.Parameters.AddWithValue("@Createdby", str_userkey);
        cmd2.Parameters.AddWithValue("@createdonfrom", this.str_CurrentDate);
        cmd2.Parameters.AddWithValue("@createdonto", this.str_addoneday);
        DataTable dt_ashboard2 = DA.GetDataTable(cmd2);
        if (dt_ashboard2.Rows.Count > 0)
        {
          
            string str_date = dt_ashboard2.Rows[0]["Date"].ToString();
            string str_showintime = dt_ashboard2.Rows[0]["InTime"].ToString();
         
            
            if (str_showintime == "")
            {
                lb_intime.Text = str_intime;
            }
            else
            {
                DateTime Time = CF.currentdatetime(str_showintime);
                var time1 = Time.ToString("H:mm tt");
                str_intime = time1.ToString();

                lb_intime.Text = str_intime;

            }

            string str_showouttime = dt_ashboard2.Rows[0]["OutTime"].ToString();
          
            if (str_showouttime == "")
            {
                lb_outtime.Text = str_outtime;
            }
            else
            {
                DateTime Outtime = CF.currentdatetime(str_showouttime);
                var time2 = Outtime.ToString("H:mm tt");
                str_outtime = time2.ToString();
                lb_outtime.Text = str_outtime;
            }
            lb_date.Text = str_date;

        }

        string str_detail = "select top 5 CONVERT(Varchar,Fromdate,103)as Invalue,Reason,Responsestatus,responsereason from IT_EmployeeLeaveDetails where Employeekey=@Employeekey Order by Todate DESC";
        SqlCommand cmd3 = new SqlCommand(str_detail);
        cmd3.Parameters.AddWithValue("@Employeekey", str_userkey);
        DataTable dt_leave = DA.GetDataTable(cmd3);
        DataSet ds = new DataSet();
        ds.Merge(dt_leave);
        if (dt_leave.Rows.Count > 0)
        {

            if (ds.Tables[0].Columns.Contains("Responsestatus"))
                ds.Tables[0].Columns.Add("ActiveText");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                String str_reason = dr["responsereason"].ToString();
                int activetype = Convert.ToInt16(dr["responsestatus"].ToString());
                if (activetype == 1)
                    dr["ActiveText"] = "<span class='label label-info' title='" + str_reason + "'>Pending</span>";
                else if (activetype == 2)
                    dr["ActiveText"] = "<span class='label label-sm label-success' title='" + str_reason + "'>Approved</span>";
                else if (activetype == 3)
                    dr["ActiveText"] = "<span class='label label-danger' title='" + str_reason + "'>Rejected</span>";
            }
            this.PH.LoadGridItem(ds, PH_LeaveRequest, "LeaveRequest.txt", "");
        }
        string str_details = "Select top 5 Requestdate,Reason,Responsestatus,responsereason from IT_EmployeePermissionDetails where Employeekey=@Employeekey Order by Requestdate DESC";
         SqlCommand cmd4 = new SqlCommand(str_details);
         cmd4.Parameters.AddWithValue("@Employeekey", str_userkey);
             DataTable dt_leave1 = DA.GetDataTable(cmd4);
               DataSet ds1 = new DataSet();
               ds1.Merge(dt_leave1);
        if (dt_leave1.Rows.Count > 0)
         {

            if (ds1.Tables[0].Columns.Contains("Responsestatus"))
                ds1.Tables[0].Columns.Add("ActiveText");
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                String str_reason = dr["responsereason"].ToString();
                int activetype = Convert.ToInt16(dr["responsestatus"].ToString());
                if (activetype == 1)
                    dr["ActiveText"] = "<span class='label label-info' title='" + str_reason + "'>Pending</span>";
                else if (activetype == 2)
                    dr["ActiveText"] = "<span class='label label-sm label-success' title='" + str_reason + "'>Approved</span>";
                else if (activetype == 3)
                    dr["ActiveText"] = "<span class='label label-danger' title='" + str_reason + "'>Rejected</span>";
            }
            this.PH.LoadGridItem(ds1, PH_PermissionRequest, "PermissionRequest.txt", "");
        }

    }

}
   