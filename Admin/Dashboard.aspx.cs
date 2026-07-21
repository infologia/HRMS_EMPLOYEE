using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WEB_Dashboard : System.Web.UI.Page
{
    SessionCustom SC;
    DataAccess DA;
    PhTemplate PH;
    string str_CurrentDate = "";
    string str_TODate = "";
    int empkey;
    int leavedetails;


    protected void Page_Load(object sender, EventArgs e)
    
    {
        this.SC = new SessionCustom();
        this.DA = new DataAccess();
        this.PH = new PhTemplate();

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Dashboard";

            HtmlAnchor control = this.Master.FindControl("li_dashboard") as HtmlAnchor;
            if (control != null)
                control.Attributes.Add("class", "active");
        }

        

        string str_dashboard = "select count(Employeekey) as Employeekey from IT_EmployeeRegister where roles='1' and Employeestatus='1'";
        DataTable dt_ashboard = DA.GetDataTable(str_dashboard);
        if (dt_ashboard.Rows.Count > 0)
        {
          string str_usercount = dt_ashboard.Rows[0]["Employeekey"].ToString();
            this.empkey=int.Parse(str_usercount);
            lbl_Totalmembers.Text =empkey.ToString();

        }
        DateTime date = DateTime.UtcNow;
        string date1 = date.ToString("yyyy-MM-dd");
        this.str_CurrentDate = date1 + " " + "00:00:01";
        this.str_TODate = date1 + " " + " 23:59:59";

        // ONLINE = InTime iruku & OutTime illa
        string str_online = @"SELECT COUNT(*) FROM IT_InOutTime WHERE InTime IS NOT NULL AND OutTime IS NULL AND Createdon BETWEEN @createdfrom AND @createdto";

        SqlCommand cmdOnline = new SqlCommand(str_online);
        cmdOnline.Parameters.AddWithValue("@createdfrom", str_CurrentDate);
        cmdOnline.Parameters.AddWithValue("@createdto", str_TODate);

        DataTable dtOnline = DA.GetDataTable(cmdOnline);
        int onlineCount = 0;

        if (dtOnline.Rows.Count > 0)
        {
            onlineCount = Convert.ToInt32(dtOnline.Rows[0][0]);
        }

        // OFFLINE = Total Employees - Online
        int offlineCount = this.empkey - onlineCount;

        // SET LABELS
        lb_intime.Text = onlineCount.ToString();   // Online
        lb_outtime.Text = offlineCount.ToString(); // Offline

        // ===== ONLINE / OFFLINE COUNT END =====

        string CurrentMonth = String.Format("{0:MMMM}", DateTime.Now);

        string str_dashboard1 = "select count(Responsestatus) as Responsestatus from IT_EmployeeLeaveDetails where Responsestatus='1' and month(Todate) = month(getdate()) and year(Todate)=Year(getdate()) ";
        DataTable dt_ashboard1 = DA.GetDataTable(str_dashboard1);
        if (dt_ashboard1.Rows.Count > 0)
        {
            string str_statuscount = dt_ashboard1.Rows[0]["Responsestatus"].ToString();
            this.leavedetails = int.Parse(str_statuscount);
            lbl_leave.Text = leavedetails.ToString();
            lbl_mon.Text = CurrentMonth;

        }
        DateTime date2 = DateTime.UtcNow;
        string date3 = date2.ToString("yyyy-MM-dd");
        this.str_CurrentDate = date3 + " " + "00:00:01";
        this.str_TODate = date3 + " " + " 23:59:59";


        string CurrentMonth1 = String.Format("{0:MMMM}", DateTime.Now);
        string str_dashboard2 = "select count(Responsestatus) as Responsestatus from IT_EmployeePermissionDetails where Responsestatus='1' and month(Createdon) = month(getdate()) and year(Createdon)=year(getdate())";
        DataTable dt_dashboard2 = DA.GetDataTable(str_dashboard2);
        if (dt_ashboard1.Rows.Count > 0)
        {
            string str_statuscount = dt_dashboard2.Rows[0]["Responsestatus"].ToString();
            this.leavedetails = int.Parse(str_statuscount);
            lbl_perm.Text = leavedetails.ToString();
            lbl_month.Text = CurrentMonth1;

        }
        DateTime date4 = DateTime.UtcNow;
        string date5 = date2.ToString("yyyy-MM-dd");
        this.str_CurrentDate = date5 + " " + "00:00:01";
        this.str_TODate = date5 + " " + " 23:59:59";


        string str_query = "Select CONVERT(VARCHAR(10), Createdon, 105) AS Createdon,Reason,Complaintstatus  from IT_Complaint where Complaintstatus='1' order by createdon DESC";
        SqlCommand cmd = new SqlCommand(str_query);

        DataTable dt_dashboard = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_dashboard);
        if (dt_dashboard.Rows.Count > 0)
        {
            if (ds.Tables[0].Columns.Contains("Complaintstatus"))
                ds.Tables[0].Columns.Add("ActiveText");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int activetype = Convert.ToInt16(dr["Complaintstatus"].ToString());
                if (activetype == 1)
                    dr["ActiveText"] = "<span class='label label-info'>Pending</span>";
                else if (activetype == 2)
                    dr["ActiveText"] = "<span class='label label-sm label-success'>Approved</span>";
                else if (activetype == 3)
                    dr["ActiveText"] = "<span class='label label-danger'>Rejected</span>";
            }
            this.PH.LoadGridItem(ds, PH_Leave, "dashboardleave.txt", "");

        }


        string str_querys = "Select Reason, CONVERT(VARCHAR(10), Createdon, 105) AS Createdon,SuggestionStatus  from IT_Suggestion where SuggestionStatus='1' order by createdon DESC";
        SqlCommand cmd1 = new SqlCommand(str_querys);

        DataTable dt_sugg = DA.GetDataTable(cmd1);
        DataSet ds1 = new DataSet();
        ds1.Merge(dt_sugg);
        if (dt_sugg.Rows.Count > 0)
        {
            if (ds1.Tables[0].Columns.Contains("SuggestionStatus"))
                ds1.Tables[0].Columns.Add("ActiveText");
            foreach (DataRow dr in ds1.Tables[0].Rows)
            {
                int activetype = Convert.ToInt16(dr["SuggestionStatus"].ToString());
                if (activetype == 1)
                    dr["ActiveText"] = "<span class='label label-info'>Pending</span>";
                else if (activetype == 2)
                    dr["ActiveText"] = "<span class='label label-sm label-success'>Approved</span>";
                else if (activetype == 3)
                    dr["ActiveText"] = "<span class='label label-danger'>Rejected</span>";
            }
            this.PH.LoadGridItem(ds1, PH_Suggestion, "dashboardsuggestion.txt", "");

        }
    }
}
