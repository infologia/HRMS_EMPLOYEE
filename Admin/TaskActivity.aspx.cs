using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_TaskActivity : System.Web.UI.Page
{

    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    string str_id = "";
    string str_key = "";
    string str_name = "";
    string str_tab = "";

    protected void Page_Load(object sender, EventArgs e)
    {

        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();
        str_id = this.SC.Userid;
        str_name = this.SC.username;
        alltab.Visible = false;
        coms.Visible = false;

        if (Request.QueryString["key"] == "" || Request.QueryString["key"] == null)
        {

        }

        else
        {

            this.str_key = Request.QueryString["key"].ToString();

        }


    }
    protected void allactivity_Click(object sender, EventArgs e)
    {
        //coms.Visible = false;
        alltab.Visible = true;
        string alldate = DateTime.Now.ToString();
         string str_all = "select a.taskkey,a.createdon,a.taskname,b.username,c.Comment from TT_CreateTask a left outer join  IT_EmployeeRegister b on a.createdby=b.Employeekey " +
                          "left outer join TT_Taskdetails c on c.Taskid=a.taskkey  where taskkey='" + this.str_key + "'";
        SqlCommand sc = new SqlCommand(str_all);
        //sc.Parameters.AddWithValue("@createdon", alldate);

        DataTable dt_dashboard1 = DA.GetDataTable(sc);
        DataSet ds1 = new DataSet();
        ds1.Merge(dt_dashboard1);
        if (dt_dashboard1.Rows.Count > 0)
        {

            this.PH.LoadGridItem(ds1, all, "allview.txt", "");
        }
    }
    protected void cmd_Click(object sender, EventArgs e)
    {
        //alltab.Visible = false;
        coms.Visible = true;
       
        string str_chat = "select a.taskkey,a.taskname,b.username,c.Comment, LEFT(b.username, 1) AS ExtractString,CASE WHEN DATEDIFF(SECOND, b.Createdon, GETDATE()) < 60 THEN ' Just Posted'" +
                            "WHEN DATEDIFF(MINUTE, c.Createdon, GETDATE()) < 60 THEN CAST(DATEDIFF(MINUTE, c.Createdon, GETDATE()) AS VARCHAR(10)) + ' Minutes ago'" +
                            "WHEN DATEDIFF(MINUTE, c.Createdon, GETDATE()) < 24 * 60 THEN CAST(FLOOR(DATEDIFF(MINUTE, c.Createdon, GETDATE())/60) AS VARCHAR(10)) + ' Hours ago'" +
                            "WHEN DATEDIFF(HOUR, c.Createdon, GETDATE()) <24* 7  THEN CAST(FLOOR(DATEDIFF(HOUR,c.Createdon, GETDATE())/24) AS VARCHAR(10)) + ' Days ago'" +
                            "WHEN DATEDIFF(dd,c.Createdon, GetDate()) <= 7 THEN 'One Week Ago' WHEN DATEDIFF(dd,c.Createdon, GetDate()) > 7 AND DATEDIFF(dd,c.Createdon, GetDate()) < 30 THEN 'One Month Ago'" +

                        "END AS TimeAgo FROM  TT_CreateTask a left outer join  IT_EmployeeRegister b on b.createdby=b.Employeekey " +
                       "left outer join TT_Taskdetails c on c.Taskid=a.taskkey where taskkey='" + this.str_key + "'";
        SqlCommand sc = new SqlCommand(str_chat);
        sc.Parameters.AddWithValue("@Taskid", this.str_tab);
        DataTable dt_chat = this.DA.GetDataTable(sc);
        DataSet ds_chat = new DataSet();
        ds_chat.Merge(dt_chat);
        if (dt_chat.Rows.Count > 0)
        {

            PH.LoadGridItem(ds_chat, Ph_chat, "Command.txt", "");
        }
    }
    protected void lb_activity_Click(object sender, EventArgs e)
    {
        string alldate = DateTime.Now.ToString();
        string str_all = "select a.taskkey,a.Modifiedon,b.username from TT_CreateTask a left outer join  IT_EmployeeRegister b on a.assignee=b.Employeekey where a.modifiedon IS NOT NULL";
        SqlCommand sc = new SqlCommand(str_all);
        //sc.Parameters.AddWithValue("@createdon", alldate);

        DataTable dt_dashboard1 = DA.GetDataTable(sc);
        DataSet ds1 = new DataSet();
        ds1.Merge(dt_dashboard1);
        if (dt_dashboard1.Rows.Count > 0)
        {
            this.PH.LoadGridItem(ds1, ph_taskview, "History.txt", "");
        }
    }
    protected void lb_history_Click(object sender, EventArgs e)
    {
        string alldate = DateTime.Now.ToString();
        string str_all = "select a.taskkey,a.Modifiedon,b.username from TT_CreateTask a left outer join  IT_EmployeeRegister b on a.assignee=b.Employeekey where a.modifiedon IS NOT NULL";
        SqlCommand sc = new SqlCommand(str_all);
        //sc.Parameters.AddWithValue("@createdon", alldate);

        DataTable dt_dashboard1 = DA.GetDataTable(sc);
        DataSet ds1 = new DataSet();
        ds1.Merge(dt_dashboard1);
        if (dt_dashboard1.Rows.Count > 0)
        {
            this.PH.LoadGridItem(ds1, ph_taskview, "History.txt", "");
        }
    }
    protected void btn_submit_Click(object sender, EventArgs e)
    {
        string str_userky = Guid.NewGuid().ToString();
        string filename = Path.GetFileName(comment.FileName);
        string extension = Path.GetExtension(filename);
        string str_newid = str_userky + extension;
        string str_path = Server.MapPath("~/Comments/") + str_newid;
        comment.SaveAs(str_path);
        SqlCommand cmd = new SqlCommand("insert into TT_Taskdetails(Comment,Attachmentid,createdby,Taskid)values(@Comment,@Attachmentid,@createdby,@Taskid)");
        cmd.Parameters.AddWithValue("@Comment", txt_reason.InnerText);
        cmd.Parameters.AddWithValue("@Attachmentid", str_newid);
        cmd.Parameters.AddWithValue("@createdby", str_id);
        cmd.Parameters.AddWithValue("@Taskid", str_key);
        DA.ExecuteNonQuery(cmd);
        Response.Redirect(@"~/Admin/taskactivity.aspx");
    }
}