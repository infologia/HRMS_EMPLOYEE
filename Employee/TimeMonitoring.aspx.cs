using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class WEB_TimeMonitoring : System.Web.UI.Page
{

    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    CommonFunction CF;

    string str_userkey = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();
        this.CF = new CommonFunction();
        this.str_userkey = SC.Userid;

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Time Tracking";
        }

      
        string str_view = "select Employeekey,Employeeid,EmployeeName,WorkDate,InTime,OutTime,GrossWorkingHours,LunchDuration,BreakDuration,NetWorkingDuration from IT_V_EmployeeDailyWorkSummary where employeekey = @employeekey";
        SqlCommand cmd = new SqlCommand(str_view);
        cmd.Parameters.AddWithValue("@employeekey", str_userkey);
        DataTable dt_dashboard = DA.GetDataTable(cmd);
        if (dt_dashboard.Rows.Count > 0)
        {
            DataTable final = new DataTable();
            final.Columns.Add("Employeeid");
            final.Columns.Add("EmployeeName");
            final.Columns.Add("WorkDate");
            final.Columns.Add("InTime");
            final.Columns.Add("OutTime");
            final.Columns.Add("GrossWorkingHours");
            final.Columns.Add("LunchDuration");
            final.Columns.Add("BreakDuration");
            final.Columns.Add("NetWorkingDuration");

            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            foreach (DataRow row in dt_dashboard.Rows)
            {
                final.Rows.Add(
                    row["Employeeid"],
                    row["EmployeeName"],
                    Convert.ToDateTime(row["WorkDate"]).ToString("dd/MM/yyyy"),
                    row["InTime"] == DBNull.Value
                        ? "-"
                        : TimeZoneInfo.ConvertTimeFromUtc((DateTime)row["InTime"], istZone).ToString("hh:mm tt"),
                    row["OutTime"] == DBNull.Value
                        ? "-"
                        : TimeZoneInfo.ConvertTimeFromUtc((DateTime)row["OutTime"], istZone).ToString("hh:mm tt"),
                    row["GrossWorkingHours"],
                    row["LunchDuration"],
                    row["BreakDuration"],
                    row["NetWorkingDuration"]
                );
            }

            
                DataSet ds = new DataSet();
                ds.Merge(final);
                this.PH.LoadGridItem(ds, PH_TimemonitoringView, "TimeMonitoringView.txt", "");
            }

        }

    
}