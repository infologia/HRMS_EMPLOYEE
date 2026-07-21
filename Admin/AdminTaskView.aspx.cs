using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AdminTaskView : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    string str_userkey = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();
        this.str_userkey = SC.Userid;
        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "File Maintanence";
        }
        string indiantime = "05:25:00.000";
        string str_query = "select a.status,DATEDIFF(minute, a.Taskstarttime, a.Taskendtime) / (24*60) 'Days',(DATEDIFF(minute, a.Taskstarttime, a.Taskendtime) / 60) % 24 'Hours',DATEDIFF(minute, a.Taskstarttime, a.Taskendtime) % 60 'Minutes' ,b.PjName,a.Taskname,a.Duedate,a.Taskstarttime +'" + indiantime + "' Taskstarttime,a.Taskendtime +'" + indiantime + "' Taskendtime,a.worktiming from tt_createtask a left outer join TT_Project b on a.projectkey=b.projectkey";
        SqlCommand cmd = new SqlCommand(str_query);
        //cmd.Parameters.AddWithValue("@Employeekey", str_userkey);
        DataTable dt_document = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_document);


        if (dt_document.Rows.Count > 0)
        {
            if (ds.Tables[0].Columns.Contains("status"))
                ds.Tables[0].Columns.Add("ActiveText");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                String str_reason = dr["status"].ToString();
                int activetype = Convert.ToInt16(dr["status"].ToString());
                if (activetype == 1)
                    dr["ActiveText"] = "<span class='label label-info' title='" + str_reason + "'>Pending</span>";
                else if (activetype == 2)
                    dr["ActiveText"] = "<span class='label label-sm label-warning' title='" + str_reason + "'>Inprogress</span>";
                else if (activetype == 3)
                    dr["ActiveText"] = "<span class='label label-success' title='" + str_reason + "'>completed</span>";
                else if (activetype == 4)
                    dr["ActiveText"] = "<span class='label label-sm label-success' title='" + str_reason + "'>Accepted</span>";
            }
            this.PH.LoadGridItem(ds, PH_admintaskview, "AdminTaskView.txt", "");

        }



    }
}