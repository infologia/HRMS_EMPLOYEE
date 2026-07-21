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


public partial class Admin_ProjectCategoryView : System.Web.UI.Page
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
            string str_Sql = "delete from TT_PjtCategory where PjtCategorykey=@PjtCategorykey";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@PjtCategorykey", str_PjtCategorykey);
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
        string str_project = "select PjtCategoryKey, categoryname,description,CONVERT(Varchar,createdon,103)as createdon,status from TT_PjtCategory  ";
        SqlCommand cmd = new SqlCommand(str_project);

        DataTable dt_project = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_project);
        if (dt_project.Rows.Count > 0)
        {
            if (ds.Tables[0].Columns.Contains("Status"))
                ds.Tables[0].Columns.Add("ActiveText");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int activetype = Convert.ToInt16(dr["Status"].ToString());
                if (activetype == 1)
                    dr["ActiveText"] = "<span class='label label-sm label-success'>Active</span>";
                else if (activetype == 0)
                    dr["ActiveText"] = "<span class='label label-sm label-danger'>InActive</span>";
            }

            this.PH.LoadGridItem(ds, PH_ProjectCategoryView, "ProjectCategoryView.txt", "");


        }

    }

}   //if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
        //{
        //    this.str_key = Request.QueryString["id"].ToString();
        //    this.delete();
        //}
        


    

    

   