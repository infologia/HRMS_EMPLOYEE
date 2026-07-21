using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_ProjectIssue : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    string str_id = "";
    string str_key = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();
        str_id = this.SC.Userid;


        string str_query = "select a.Taskname,a.TaskKey,b.pjname,CASE WHEN DATEDIFF(SECOND, a.Createdon, GETDATE()) < 60 THEN ' Just Posted'"+ 
                            "WHEN DATEDIFF(MINUTE, a.Createdon, GETDATE()) < 60 THEN CAST(DATEDIFF(MINUTE, a.Createdon, GETDATE()) AS VARCHAR(10)) + ' Minutes ago'"+
                             "WHEN DATEDIFF(MINUTE, a.Createdon, GETDATE()) < 24 * 60 THEN CAST(FLOOR(DATEDIFF(MINUTE, a.Createdon, GETDATE())/60) AS VARCHAR(10)) + ' Hours ago'"+
                        " WHEN DATEDIFF(dd,a.Createdon, GetDate()) <= 7 THEN 'One Week Ago' WHEN DATEDIFF(dd,a.Createdon, GetDate()) > 7 AND DATEDIFF(dd,a.Createdon, GetDate()) < 30 THEN 'One Month Ago'"+
                          "ELSE CAST(FLOOR(DATEDIFF(HOUR,a.Createdon, GETDATE())/24) AS VARCHAR(10)) + ' Days ago'"+
                          "END AS TimeAgo from TT_CreateTask a left outer join TT_Project b on a.Projectid=b.Projectkey";

        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@Createdy", str_id);

        DataTable dt_dashboard = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_dashboard);
        if (dt_dashboard.Rows.Count > 0)
        {

            this.PH.LoadGridItem(ds, ph_view, "projectview.txt", "");

        }


        if (!IsPostBack)
        {

            if (Request.QueryString["key"] != null && Request.QueryString["key"] != "")
            {

                this.str_key = Request.QueryString["key"].ToString();
                this.task();
            }


         
        }
        
    }

    public void task()
    {
        string str_query1 = "select a.pjname,b.Taskname,DATEADD(dd, 0, DATEDIFF(dd, 0, b.Createdon)) as Created,b.Description,c.Email,d.IssueTypeName,e.PriorityName from TT_Project a left outer join TT_CreateTask b on a.Projectkey=b.ProjectId left outer join IT_EmployeeRegister c on c.Employeekey=b.Assignee left outer join TT_IssueType d on b.Issuetype=d.IssueTypekey left outer join TT_Priority e on b.priority=e.Priortykey where b.Taskkey=@Taskkey";

        SqlCommand cmd1 = new SqlCommand(str_query1);
        cmd1.Parameters.AddWithValue("@Taskkey", str_key);

        DataTable dt_dashboard1 = DA.GetDataTable(cmd1);
        DataSet ds1 = new DataSet();
        ds1.Merge(dt_dashboard1);
        if (dt_dashboard1.Rows.Count > 0)
        {

            this.PH.LoadGridItem(ds1, ph_taskview, "Taskview.txt", "");

        }


    }
 
  
   
}