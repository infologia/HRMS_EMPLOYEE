using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class TicketingTool_Project_sSummary : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    string str_userkey="";
    string str_key = "";
    
    protected void Page_Load(object sender, EventArgs e)
    {

        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();

        if (Request.QueryString["key"] != null && Request.QueryString["key"] != "")
        {
            this.str_key = Request.QueryString["key"].ToString();
        }


        this.LoadSummary();
       
    }

    private void LoadSummary()
    {
        this.str_userkey = SC.Userid.ToString();
        string str_sql = "select b.firstname,a.projectkey,a.Pjname,CONVERT(varchar,a.Createdon,103)as createdon,a.progressstatus from TT_project a  left outer join IT_EmployeeRegister b   on a.Createdby =b.Employeekey where projectkey=@projectkey";
        SqlCommand cmd = new SqlCommand(str_sql);
        cmd.Parameters.AddWithValue("@projectkey", this.str_key);
        DataTable dt_task = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
       
        ds.Merge(dt_task);
        if (dt_task.Rows.Count > 0)
        {
            if (ds.Tables[0].Columns.Contains("ProgressStatus"))
                ds.Tables[0].Columns.Add("ActiveCategory");
            foreach (DataRow dr1 in ds.Tables[0].Rows)
            {
                int activetype = Convert.ToInt16(dr1["ProgressStatus"].ToString());
                if (activetype == 1)
                    dr1["ActiveCategory"] = "<span class='label label-primary'>Pending</span>";
                if (activetype == 3)
                    dr1["ActiveCategory"] = "<span class='label label-Green'>Done</span>";
                if (activetype == 2)
                    dr1["ActiveCategory"] = "<span class='label label-danger'>InProgress</span>";


            }
            this.PH.LoadGridItem(ds, PH_EmployeeView, "Summary.txt", "");
        }

       
    }
}