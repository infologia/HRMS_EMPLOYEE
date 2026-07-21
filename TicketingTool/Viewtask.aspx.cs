using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TicketingTool_Viewtask : System.Web.UI.Page
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



        String str_prj = "select a.Taskname,a.TaskKey,left(a.Description, 150)as [Descriptions],a.Duedate,a.Status," +
                           "b.pjname,b.projectkey,c.Categoryname,d.Priorityname,e.Username,d.Priorityname from TT_CreateTask a left outer join TT_Project b on a.Projectkey=b.Projectkey  left outer join TT_taskcategory c on a.Issuetype=c.Taskcategorykey    left outer join TT_Priority" +
                         " d on d.priortykey=a.priority left outer join IT_EmployeeRegister e on e.Employeekey=a.Createdby  where a.assignee='"+this.SC.Userid+"' group by a.TaskKey,a.Description, a.Taskname," +
                         "a.Duedate,a.Status,c.Categoryname,d.Priorityname,e.Username,d.Priorityname,b.pjname,b.projectkey,a.Createdon";


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
                    dr["ActiveCategory"] = "<span class='label label-primary'>Pending</span>";
                else if (activetype == 2)
                    dr["ActiveCategory"] = "<span class='label label-danger'>Inprogress</span>";
                else if (activetype == 3)
                    dr["ActiveCategory"] = "<span class='label label-sm label-success'>Completed</span>";
                else if (activetype == 4)
                    dr["ActiveCategory"] = "<span class='label label-sm label-success'>Accepted</span>";


            }


            this.PH.LoadGridItem(ds, PH_task, "usertask.txt", "");
        }
    }

}
