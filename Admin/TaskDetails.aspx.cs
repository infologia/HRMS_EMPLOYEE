using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_TaskDetails : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    string str_id = "";
    string str_key = "";
      string str_attach = "";
    string str_name = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();
        str_id = this.SC.Userid;
        str_name = this.SC.username;


        if (Request.QueryString["id"] != null && Request.QueryString["id"] != "" )
        {

            if (!IsPostBack)
            {
                this.str_attach = Request.QueryString["id"].ToString();

                this.Attachments();
            }
        }

        else
        {
        }

       if(Request.QueryString["key"] != null )
       {
           this.str_key = Request.QueryString["key"].ToString();

           if (!IsPostBack)
           {
               this.TaskDetails();
               this.attach();
               this.Assignee();
           }
       }
        else
       {
       }
    }

    public void Assignee()
    {
        string str_assignee = "select a.assignee,b.Firstname,b.image,c.destinationname,b.city,b.phonenumber,b.Email,b.Qualification from tt_createtask a left outer join it_employeeregister b on a.assignee=b.employeekey left outer join it_destination c on b.destination=c.destinationid where Taskkey=@Taskkey";
        SqlCommand cmd1 = new SqlCommand(str_assignee);
        cmd1.Parameters.AddWithValue("@Taskkey", str_key);
        //cmd.Parameters.AddWithValue("@employeekey", SC.Userid);
        DataTable dt_tables = DA.GetDataTable(cmd1);
        DataSet ds = new DataSet();
        ds.Merge(dt_tables);
        if (dt_tables.Rows.Count > 0)
        {
          
            this.PH.LoadGridItem(ds,PH_assignee, "Taskassignee.txt", "");
        }

    }

    public void attach()
    {
        string str_achmnts = "select Attachments,Taskkey from TT_Createtask where  Taskkey=@Taskkey";
        SqlCommand cmd = new SqlCommand(str_achmnts);
        cmd.Parameters.AddWithValue("@Taskkey", str_key);
        //cmd.Parameters.AddWithValue("@employeekey", SC.Userid);
        DataTable dt_table = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_table);
        if (dt_table.Rows.Count > 0)
        {
            string str_attcid = dt_table.Rows[0]["Attachments"].ToString();
            if (str_attcid != "")
            {

                this.PH.LoadGridItem(ds, PH_attach, "taskattachment.txt", "");
            }
            else
            {
            }
        }
    }


    public void TaskDetails()
    {
        string str_task = "select a.taskname,a.description,b.pjname,b.pjdescription from tt_createtask a left outer join   tt_Project b on a.projectkey=b.projectkey where taskkey='"+this.str_key+"'";
        DataTable dt_task = DA.GetDataTable(str_task);
        DataSet ds = new DataSet();
        ds.Merge(dt_task);
        if (dt_task.Rows.Count > 0)
        {
            headtitle.InnerText = dt_task.Rows[0]["taskname"].ToString();
            taskdesc.InnerText = dt_task.Rows[0]["description"].ToString();
            pjdesc.InnerText = dt_task.Rows[0]["pjdescription"].ToString();
        }

      }


    public void Attachments()
    {

        string str_Doc = "select Attachments from TT_Createtask where  Taskkey=@Taskkey";
        SqlCommand cmd = new SqlCommand(str_Doc);
        cmd.Parameters.AddWithValue("@Taskkey", str_attach);
        //cmd.Parameters.AddWithValue("@employeekey", SC.Userid);
        DataTable dt_table = DA.GetDataTable(cmd);
        if (dt_table.Rows.Count > 0)
        {
            string str_file = dt_table.Rows[0]["Attachments"].ToString();
            string fullfilename = Server.MapPath("~/Document/" + str_file);
            Response.Clear();
            Response.BufferOutput = false;
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Length", fullfilename);
            Response.AddHeader("content-disposition", "attachment; filename=" + str_file);
            Response.TransmitFile(fullfilename);
            Response.Flush();
            Response.End();
        }
    }

}



