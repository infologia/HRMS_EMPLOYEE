using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class Admin_MenuRole : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();
        this.project();
    }



    [WebMethod] //Delete
    public static string DeleteProject(string str_PjtCategorykey)
    {
        string str_Response = "0";
        try
        {
            DataAccess DA1;
            DA1 = new DataAccess();
            SaveQuery SAQ = new SaveQuery();
            SessionCustom SC = new SessionCustom();
            string str_Sql = "delete from TT_MenuRole where MenuroleKey=@MenuroleKey";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@MenuroleKey", str_PjtCategorykey);
            DA1.ExecuteNonQuery(cmd);

            return str_Response = "1";

        }
        catch (Exception ex)
        {
            return str_Response;
        }
    }

    private void project()
    {
         string str_project = "select a.menurolekey,a.rolename,CONVERT(Varchar,a.createdon,103)as createdon,b.Firstname from  TT_MenuRole a left outer join IT_EmployeeRegister b on a.CreatedBy=b.employeekey";
        SqlCommand cmd = new SqlCommand(str_project);

        DataTable dt_project = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_project);
        if (dt_project.Rows.Count > 0)
        {

            this.PH.LoadGridItem(ds, PH_MenuRole, "menurole.txt", "");


        }

    }
}