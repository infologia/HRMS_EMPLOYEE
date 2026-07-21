using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_CreateSubMenu : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_id = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Sub Menus";

            LoadModules();

            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                this.str_id = Request.QueryString["id"].ToString();
                LoadSubMenuData();
                btn_submit.Text = "Update";
            }
            else
            {
                btn_submit.Text = "Submit";
            }
        }
        else
        {
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                this.str_id = Request.QueryString["id"].ToString();
            }
        }
    }

    private void LoadModules()
    {
        SqlCommand cmd = new SqlCommand("SELECT ModuleId, ModuleName FROM IT_Modules WHERE IsActive=1 ORDER BY ModuleName");
        DataSet ds = DA.GetDataSet(cmd);
        if (ds != null && ds.Tables.Count > 0)
        {
            ddl_module.DataSource = ds.Tables[0];
            ddl_module.DataTextField = "ModuleName";
            ddl_module.DataValueField = "ModuleId";
            ddl_module.DataBind();
            ddl_module.Items.Insert(0, new ListItem("Select Module", "0"));
        }
    }

    protected void ddl_module_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadParentMenus();
    }

    private void LoadParentMenus()
    {
        ddl_parentmenu.Items.Clear();
        ddl_parentmenu.Items.Insert(0, new ListItem("Select Parent Menu", "0"));
        
        if (ddl_module.SelectedValue != "0")
        {
            int currentMenuId = string.IsNullOrEmpty(str_id) ? 0 : Convert.ToInt32(str_id);
            
            SqlCommand cmd = new SqlCommand(@"
                SELECT MenuKey AS Id, MenuName AS Name, MenuType, MenuListNo
                FROM IT_Menus 
                WHERE ModuleId=@ModuleId 
                AND MenuKey <> @CurrentMenuId
                ORDER BY MenuType, MenuListNo");
                
            cmd.Parameters.AddWithValue("@ModuleId", ddl_module.SelectedValue);
            cmd.Parameters.AddWithValue("@CurrentMenuId", currentMenuId);
            
            DataTable dt = DA.GetDataTable(cmd);
            
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string prefix = Convert.ToInt32(row["MenuType"]) == 1 ? "-- " : "";
                    ddl_parentmenu.Items.Add(new ListItem(prefix + row["Name"].ToString(), row["Id"].ToString()));
                }
            }
        }
    }

    private void LoadSubMenuData()
    {
        SqlCommand cmd = new SqlCommand("SELECT * FROM IT_Menus WHERE MenuKey=@MenuKey AND MenuType=1");
        cmd.Parameters.AddWithValue("@MenuKey", str_id);
        DataTable dt = DA.GetDataTable(cmd);
        if (dt.Rows.Count > 0)
        {
            txt_menuname.Text = dt.Rows[0]["MenuName"].ToString();
            txt_pagename.Text = dt.Rows[0]["PageName"].ToString();
            txt_menulistno.Text = dt.Rows[0]["MenuListNo"].ToString();
            ddl_module.SelectedValue = dt.Rows[0]["ModuleId"].ToString();
            LoadParentMenus();
            if (dt.Rows[0]["ParentMenuKey"] != DBNull.Value)
                ddl_parentmenu.SelectedValue = dt.Rows[0]["ParentMenuKey"].ToString();
            txt_foldername.Text = dt.Rows[0]["FolderName"].ToString();
            txt_menuicon.Text = dt.Rows[0]["MenuIcon"].ToString();
            txt_menudesc.InnerText = dt.Rows[0]["MenuDescription"].ToString();
            
            if (dt.Rows[0]["Status"] != DBNull.Value)
                rblStatus.SelectedValue = dt.Rows[0]["Status"].ToString();
        }
    }

    protected void btn_submit_Click(object sender, EventArgs e)
    {
        try
        {
            if (str_id == "")
            {
                SqlCommand cmd = new SqlCommand("INSERT INTO IT_Menus(MenuName, PageName, MenuListNo, ModuleId, ParentMenuKey, FolderName, MenuIcon, MenuDescription, MenuType, Status, CreatedBy, CreatedOn) VALUES(@MenuName, @PageName, @MenuListNo, @ModuleId, @ParentMenuKey, @FolderName, @MenuIcon, @MenuDescription, 1, @Status, @CreatedBy, @CreatedOn)");
                cmd.Parameters.AddWithValue("@MenuName", txt_menuname.Text.Trim());
                cmd.Parameters.AddWithValue("@PageName", txt_pagename.Text.Trim());
                cmd.Parameters.AddWithValue("@MenuListNo", txt_menulistno.Text.Trim());
                cmd.Parameters.AddWithValue("@ModuleId", ddl_module.SelectedValue);
                cmd.Parameters.AddWithValue("@ParentMenuKey", ddl_parentmenu.SelectedValue == "0" ? (object)DBNull.Value : ddl_parentmenu.SelectedValue);
                cmd.Parameters.AddWithValue("@FolderName", txt_foldername.Text.Trim());
                cmd.Parameters.AddWithValue("@MenuIcon", txt_menuicon.Text.Trim());
                cmd.Parameters.AddWithValue("@MenuDescription", string.IsNullOrEmpty(txt_menudesc.InnerText) ? (object)DBNull.Value : txt_menudesc.InnerText.Trim());
                cmd.Parameters.AddWithValue("@Status", Convert.ToInt32(rblStatus.SelectedValue));
                cmd.Parameters.AddWithValue("@CreatedBy", SC.Userid);
                cmd.Parameters.Add("@CreatedOn", SqlDbType.DateTime).Value = DateTime.Now;
                DA.ExecuteNonQuery(cmd);
                ShowSuccessAndRedirect("Sub menu created successfully!", "/Admin/SubMenus.aspx");
            }
            else
            {
                SqlCommand cmd = new SqlCommand("UPDATE IT_Menus SET MenuName=@MenuName, PageName=@PageName, MenuListNo=@MenuListNo, ModuleId=@ModuleId, ParentMenuKey=@ParentMenuKey, FolderName=@FolderName, MenuIcon=@MenuIcon, MenuDescription=@MenuDescription, Status=@Status, ModifiedBy=@ModifiedBy, ModifiedOn=@ModifiedOn WHERE MenuKey=@MenuKey AND MenuType=1");
                cmd.Parameters.AddWithValue("@MenuName", txt_menuname.Text.Trim());
                cmd.Parameters.AddWithValue("@PageName", txt_pagename.Text.Trim());
                cmd.Parameters.AddWithValue("@MenuListNo", txt_menulistno.Text.Trim());
                cmd.Parameters.AddWithValue("@ModuleId", ddl_module.SelectedValue);
                cmd.Parameters.AddWithValue("@ParentMenuKey", ddl_parentmenu.SelectedValue == "0" ? (object)DBNull.Value : ddl_parentmenu.SelectedValue);
                cmd.Parameters.AddWithValue("@FolderName", txt_foldername.Text.Trim());
                cmd.Parameters.AddWithValue("@MenuIcon", txt_menuicon.Text.Trim());
                cmd.Parameters.AddWithValue("@MenuDescription", string.IsNullOrEmpty(txt_menudesc.InnerText) ? (object)DBNull.Value : txt_menudesc.InnerText.Trim());
                cmd.Parameters.AddWithValue("@Status", Convert.ToInt32(rblStatus.SelectedValue));
                cmd.Parameters.AddWithValue("@ModifiedBy", SC.Userid);
                cmd.Parameters.Add("@ModifiedOn", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.AddWithValue("@MenuKey", str_id);
                DA.ExecuteNonQuery(cmd);
                ShowSuccessAndRedirect("Sub menu updated successfully!", "/Admin/SubMenus.aspx");
            }
        }
        catch (Exception ex)
        {
            ShowError("Action failed. Please try again.");
        }
    }

    private void ShowError(string message)
    {
        message = message.Replace("'", "\\'");
        ScriptManager.RegisterStartupScript(this, GetType(), "toastr_error", "showToastr('error','" + message + "');", true);
    }

    private void ShowSuccessAndRedirect(string message, string redirectUrl)
    {
        message = message.Replace("'", "\\'");
        ScriptManager.RegisterStartupScript(this, GetType(), "toastr_success", "showToastr('success','" + message + "');" + "setTimeout(function(){ window.location.href = '" + redirectUrl + "'; }, 2000);", true);
    }
}
