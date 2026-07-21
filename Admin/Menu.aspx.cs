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


public partial class Admin_Menu : System.Web.UI.Page
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
        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Menu";
        }
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
            string str_Sql = "delete from TT_Menus where MenuKey=@MenuKey";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@MenuKey", str_PjtCategorykey);
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
        string str_project = "select MenuKey,MenuName,Pagename,menulist,CONVERT(Varchar,createdon,103)as createdon from TT_Menus where menutype=1";
        SqlCommand cmd = new SqlCommand(str_project);

        DataTable dt_project = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_project);
        if (dt_project.Rows.Count > 0)
        {

            this.PH.LoadGridItem(ds, PH_Menu, "menu.txt", "");


        }

    }
}