using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_WatchesView : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    string str_key = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();


        if (Request.QueryString["id"] == null || Request.QueryString["id"] == "")
        {

        }
        else
        {
            this.str_key = Request.QueryString["id"].ToString();
           
            this.Assignvalues();
        }
    }
   
    private void Assignvalues()
    {
        string str_query = "select a.TaskWatcherkey,CONVERT(VARCHAR,a.createdon,103)as createdon,b.Username,c.departmentname,d.divisionname,e.destinationname from TT_TaskWatcher a  left outer join IT_EmployeeRegister b on b.Employeekey=a.Employeekey left outer join IT_department c on c.Departmentid=b.Department left outer join IT_division d on d.Divisionid=b.division left outer join IT_destination e on e.destinationid=b.destination where Taskkey=@Taskkey";
        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@Taskkey", this.str_key);
        DataTable dt_Prj = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_Prj);
        if (dt_Prj.Rows.Count > 0)
        {

            this.PH.LoadGridItem(ds, PH_WatchesView, "WatchesView.txt", "");

        }

    }
    [WebMethod] //Delete
    public static string DeleteProject(string str_TaskWatcherkey)
    {
        string str_Response = "0";
        try
        {
            DataAccess DA1;
            DA1 = new DataAccess();
            SaveQuery SAQ = new SaveQuery();
            SessionCustom SC = new SessionCustom();
            string str_Sql = "delete from TT_TaskWatcher where TaskWatcherkey=@TaskWatcherkey";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@TaskWatcherkey", str_TaskWatcherkey);
            DA1.ExecuteNonQuery(cmd);

            return str_Response = "1";

        }
        catch (Exception ex)
        {
            return str_Response;
        }
    }
   
    protected void addwatch_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/admin/watches.aspx?id='" + this.str_key + "'");
  
    }
}