using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_Latepermissionrequestview : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    protected void Page_Load(object sender, EventArgs e)
    {


        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();


        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Self Services";
        }

        string str_query = "SELECT b.firstname+''+b.lastname as name,CONVERT(varchar,a.Requestdate,103)as request,a.Fromtime,a.Totime,a.responsereason,a.LatePermissionDetailskey,a.Permissionhourse,a.responsestatus FROM IT_LatePermissionDetails a left outer join IT_EmployeeRegister b ON a.createdby = b.Employeekey  where a.createdby=@createdby order by a.createdon ASC";

        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@createdby", SC.Userid);

        DataTable dt_dashboard = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_dashboard);
        if (dt_dashboard.Rows.Count > 0)
        {
            if (ds.Tables[0].Columns.Contains("responsestatus"))
                ds.Tables[0].Columns.Add("ActiveText");
            ds.Tables[0].Columns.Add("ViewText");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                String str_reason = dr["responsereason"].ToString();
                int activetype = Convert.ToInt16(dr["responsestatus"].ToString());
                if (activetype == 1)
                {
                    dr["ActiveText"] = "<span class='label label-info' title='" + str_reason + "'>Pending</span>";
                    dr["ViewText"] = "";
                }
                else if (activetype == 2)
                {
                    dr["ActiveText"] = "<span class='label label-success' title='" + str_reason + "'>Approved</span>";
                    dr["ViewText"] = "hidden";
                }
                else if (activetype == 3)
                {
                    dr["ActiveText"] = "<span class='label label-danger' title='" + str_reason + "'>Rejected</span>";
                    dr["ViewText"] = "hidden";
                }
            }
            this.PH.LoadGridItem(ds, PH_Permission, "Laterecordviewemp.txt", "");

        }
    }
    [WebMethod] //Delete
    public static string DeleteProject(string str_employeepermissiondetailskey)
    {
        string str_Response = "0";
        try
        {
            DataAccess DA1;
            DA1 = new DataAccess();
            SaveQuery SAQ = new SaveQuery();
            SessionCustom SC = new SessionCustom();
            string str_Sql = "delete from IT_LatePermissionDetails where LatePermissionDetailskey=@LatePermissionDetailskey";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@LatePermissionDetailskey", str_employeepermissiondetailskey);
            DA1.ExecuteNonQuery(cmd);

            return str_Response = "1";

        }
        catch (Exception ex)
        {
            return str_Response;
        }
    }
}