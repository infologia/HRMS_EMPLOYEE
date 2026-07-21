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


public partial class Admin_ParentMenuGrid : System.Web.UI.Page
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
                control1.Text = "Menu";

            //HtmlAnchor control = this.Master.FindControl("li_dashboard") as HtmlAnchor;
            //if (control != null)
            //    control.Attributes.Add("class", "active");
        }


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

            String str_checks = "select * from TT_Menus where  parentmenuid='" + str_PjtCategorykey + " '";
            DataTable dts = DA1.GetDataTable(str_checks);
            if (dts.Rows.Count > 0)
            {
               string par_id = dts.Rows[0]["parentmenuid"].ToString();


                string str_del = "delete from TT_Menus where MenuKey=@MenuKey;" + "delete from TT_Menus where parentmenuid=@parentmenuid;";
                SqlCommand cmd = new SqlCommand(str_del);
                cmd.Parameters.AddWithValue("@MenuKey", str_PjtCategorykey);
                cmd.Parameters.AddWithValue("@parentmenuid", par_id);
                DA1.ExecuteNonQuery(cmd);
            }
            else
            {
                string str_del2 = "delete from TT_Menus where MenuKey=@MenuKey";
                SqlCommand cmd1 = new SqlCommand(str_del2);

                cmd1.Parameters.AddWithValue("@MenuKey", str_PjtCategorykey);
                DA1.ExecuteNonQuery(cmd1);
            }



            return str_Response = "1";

        }
        catch (Exception ex)
        {
            return str_Response;
        }
    }

    private void project()
    {
        string str_project = "select MenuKey,Menuname,Menudescription,CONVERT(Varchar,createdon,103)as createdon from TT_Menus where menutype=0";
        SqlCommand cmd = new SqlCommand(str_project);

        DataTable dt_project = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_project);
        if (dt_project.Rows.Count > 0)
        {

            this.PH.LoadGridItem(ds, PH_ParentMenu, "parentmenu.txt", "");


        }

    }

} 
