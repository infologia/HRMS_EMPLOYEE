using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Web.UI.HtmlControls;

public partial class Admin_ParentMenus : BasePage
{
    PhTemplate PH;

    // Set the menu name for permission checking
    protected override string PageMenuName
    {
        get { return "Parent Menu"; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.PH = new PhTemplate();

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Parent Menus";
        }

        LoadParentMenus();
    }

    private void LoadParentMenus()
    {
        string str_query = @"SELECT m.MenuKey, m.MenuName, m.PageName, m.MenuListNo, mod.ModuleName, m.MenuIcon, m.FolderName 
                            FROM IT_Menus m 
                            LEFT JOIN IT_Modules mod ON m.ModuleId = mod.ModuleId 
                            WHERE m.MenuType = 0
                            ORDER BY m.CreatedOn DESC";
        SqlCommand cmd = new SqlCommand(str_query);
        DataTable dt_menus = DA.GetDataTable(cmd);

        if (dt_menus.Rows.Count > 0)
        {
            bool hasEditPerm = CanEdit();
            bool hasDeletePerm = CanDelete();

            string html = "";
            foreach (DataRow row in dt_menus.Rows)
            {
                string menuKey = row["MenuKey"].ToString();
                string menuName = row["MenuName"].ToString();
                string moduleName = row["ModuleName"].ToString();
                string menuListNo = row["MenuListNo"].ToString();

                html += "<tr>";
                html += "<td>" + menuName + "</td>";
                html += "<td>" + moduleName + "</td>";
                html += "<td>" + menuListNo + "</td>";
                
                html += "<td class='text-center'>";
                html += "<ul class='icons-list'>";
                
                // Edit Button
                if (hasEditPerm)
                {
                    html += "<li><a href='CreateParentMenu.aspx?id=" + menuKey + "' class='text-primary' data-popup='tooltip' title='Edit'><i class='icon-pencil7'></i></a></li>";
                }
                else
                {
                    html += "<li><a class='text-muted' style='cursor:not-allowed;' data-popup='tooltip' title='Edit (No Permission)'><i class='icon-pencil7'></i></a></li>";
                }

                // Delete Button
                if (hasDeletePerm)
                {
                    html += "<li><a href='javascript:void(0);' class='text-danger' onclick='fn_DeleteMenu(\"" + menuKey + "\")' data-popup='tooltip' title='Delete'><i class='icon-trash'></i></a></li>";
                }
                else
                {
                    html += "<li><a class='text-muted' style='cursor:not-allowed;' data-popup='tooltip' title='Delete (No Permission)'><i class='icon-trash'></i></a></li>";
                }
                
                html += "</ul>";
                html += "</td>";
                html += "</tr>";
            }

            PH_ParentMenus.Controls.Add(new Literal { Text = html });
        }
    }

    [WebMethod]
    public static string DeleteMenu(string str_menukey)
    {
        string str_Response = "0";
        try
        {
            SessionCustom SC = new SessionCustom();
            
            // Check Delete Permission
            if (!SC.HasPermission("Parent Menu", "delete"))
            {
                return "0";
            }

            DataAccess DA1 = new DataAccess();
            SqlCommand cmd = new SqlCommand("DELETE FROM IT_Menus WHERE MenuKey=@MenuKey AND MenuType=0");
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
