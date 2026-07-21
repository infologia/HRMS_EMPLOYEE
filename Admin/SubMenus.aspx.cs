using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.Services;

public partial class Admin_SubMenus : System.Web.UI.Page
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
                control1.Text = "Sub Menus";
        }

        string str_query = @"SELECT sm.MenuKey AS SubMenuKey, sm.MenuName, sm.PageName, sm.MenuListNo, 
                            mod.ModuleName, pm.MenuName as ParentMenuName, sm.FolderName 
                            FROM IT_Menus sm 
                            LEFT JOIN IT_Menus pm ON sm.ParentMenuKey = pm.MenuKey 
                            LEFT JOIN IT_Modules mod ON sm.ModuleId = mod.ModuleId 
                            WHERE sm.MenuType = 1
                            ORDER BY sm.CreatedOn DESC";
        SqlCommand cmd = new SqlCommand(str_query);
        DataTable dt_submenus = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_submenus);

        if (dt_submenus.Rows.Count > 0)
        {
            this.PH.LoadGridItem(ds, PH_SubMenus, "SubMenus.txt", "");
        }
    }

    [WebMethod]
    public static string DeleteSubMenu(string str_menukey)
    {
        string str_Response = "0";
        try
        {
            DataAccess DA1 = new DataAccess();
            SqlCommand cmd = new SqlCommand("DELETE FROM IT_Menus WHERE MenuKey=@MenuKey AND MenuType=1");
            cmd.Parameters.AddWithValue("@MenuKey", str_menukey);
            DA1.ExecuteNonQuery(cmd);
            return str_Response = "1";
        }
        catch (Exception)
        {
            return str_Response;
        }
    }
}
