using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Taskview : System.Web.UI.Page
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

        

        String str_prj = " select a.Taskname,a.TaskKey,left(a.Description, 150)as [Descriptions],a.Duedate,a.Status,"+
                           "b.pjname,b.projectkey,c.Categoryname,d.Priorityname,e.Username,count(f.employeekey)as watchers,d.Priorityname,CASE WHEN DATEDIFF(SECOND, a.Createdon, GETDATE()) < 60 THEN ' Just Posted'"+
                             "WHEN DATEDIFF(MINUTE, a.Createdon, GETDATE()) < 60 THEN CAST(DATEDIFF(MINUTE, a.Createdon, GETDATE()) AS VARCHAR(10)) + ' Minutes ago'"+
                             "WHEN DATEDIFF(MINUTE, a.Createdon, GETDATE()) < 24 * 60 THEN CAST(FLOOR(DATEDIFF(MINUTE, a.Createdon, GETDATE())/60) AS VARCHAR(10)) + ' Hours ago'"+
                                "WHEN DATEDIFF(HOUR, a.Createdon, GETDATE()) <24* 7  THEN CAST(FLOOR(DATEDIFF(HOUR,a.Createdon, GETDATE())/24) AS VARCHAR(10)) + ' Days ago'"+
                         "WHEN DATEDIFF(dd,a.Createdon, GetDate()) <= 7 THEN 'One Week Ago' WHEN DATEDIFF(dd,a.Createdon, GetDate()) > 7 AND DATEDIFF(dd,a.Createdon, GetDate()) < 30 THEN 'One Month Ago'"+
                         "END AS TimeAgo from TT_CreateTask a left outer join TT_Project b on a.Projectkey=b.Projectkey  left outer join TT_taskcategory c on a.Issuetype=c.Taskcategorykey    left outer join TT_Priority"+
                         " d on d.priortykey=a.priority left outer join IT_EmployeeRegister e on e.Employeekey=a.Createdby  left outer join TT_TaskWatcher f on f.taskkey=a.taskkey group by a.TaskKey,a.Description, a.Taskname,"+
                         "a.Duedate,a.Status,c.Categoryname,d.Priorityname,e.Username,f.employeekey,d.Priorityname,b.pjname,b.projectkey,a.Createdon";


        SqlCommand cmd = new SqlCommand(str_prj);


        DataTable dt_dashboard = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_dashboard);
        if (dt_dashboard.Rows.Count > 0)
        {

            if (ds.Tables[0].Columns.Contains("status"))
                ds.Tables[0].Columns.Add("ActiveCategory");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int activetype = Convert.ToInt16(dr["status"].ToString());
                if (activetype == 1)
                    dr["ActiveCategory"] = "<span class='label label-primary'>Todo</span>";
                else if (activetype == 2)
                    dr["ActiveCategory"] = "<span class='label label-danger'>Inprogress</span>";
                else if (activetype == 3)
                    dr["ActiveCategory"] = "<span class='label label-sm label-success'>Done</span>";

            }


            this.PH.LoadGridItem(ds, PH_task, "Taskgrid.txt", "");
        }
    }

    
}