using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_CreateParentMenu : BasePage
{
    string str_id = "";

    // Set the menu name for permission checking
    protected override string PageMenuName
    {
        get { return "Parent Menu"; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        // Check create/edit permission based on mode
        if (Request.QueryString["id"] != null)
        {
            // Edit mode - check edit permission
            if (!CanEdit())
            {
                Response.Redirect("~/Admin/Dashboard.aspx?error=no_edit_permission");
                return;
            }
        }
        else
        {
            // Create mode - check create permission
            if (!CanCreate())
            {
                Response.Redirect("~/Admin/Dashboard.aspx?error=no_create_permission");
                return;
            }
        }

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Parent Menus";

            LoadModules();

            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                this.str_id = Request.QueryString["id"].ToString();
                LoadMenuData();
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

    private void LoadMenuData()
    {
        SqlCommand cmd = new SqlCommand("SELECT * FROM IT_Menus WHERE MenuKey=@MenuKey AND MenuType=0");
        cmd.Parameters.AddWithValue("@MenuKey", str_id);
        DataTable dt = DA.GetDataTable(cmd);
        if (dt.Rows.Count > 0)
        {
            txt_menuname.Text = dt.Rows[0]["MenuName"].ToString();
            txt_pagename.Text = dt.Rows[0]["PageName"].ToString();
            txt_menulistno.Text = dt.Rows[0]["MenuListNo"].ToString();
            ddl_module.SelectedValue = dt.Rows[0]["ModuleId"].ToString();
            txt_menuicon.Text = dt.Rows[0]["MenuIcon"].ToString();
            txt_foldername.Text = dt.Rows[0]["FolderName"].ToString();
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
                SqlCommand cmd = new SqlCommand("INSERT INTO IT_Menus(MenuName, PageName, MenuListNo, ModuleId, MenuIcon, FolderName, MenuDescription, MenuType, Status, CreatedBy, CreatedOn) VALUES(@MenuName, @PageName, @MenuListNo, @ModuleId, @MenuIcon, @FolderName, @MenuDescription, 0, @Status, @CreatedBy, @CreatedOn)");
                cmd.Parameters.AddWithValue("@MenuName", txt_menuname.Text.Trim());
                cmd.Parameters.AddWithValue("@PageName", txt_pagename.Text.Trim());
                cmd.Parameters.AddWithValue("@MenuListNo", txt_menulistno.Text.Trim());
                cmd.Parameters.AddWithValue("@ModuleId", ddl_module.SelectedValue);
                cmd.Parameters.AddWithValue("@MenuIcon", txt_menuicon.Text.Trim());
                cmd.Parameters.AddWithValue("@FolderName", txt_foldername.Text.Trim());
                cmd.Parameters.AddWithValue("@MenuDescription", txt_menudesc.InnerText.Trim());
                cmd.Parameters.AddWithValue("@Status", Convert.ToInt32(rblStatus.SelectedValue));
                cmd.Parameters.AddWithValue("@CreatedBy", SC.Userid);
                cmd.Parameters.Add("@CreatedOn", SqlDbType.DateTime).Value = DateTime.Now;
                DA.ExecuteNonQuery(cmd);
                ShowSuccessAndRedirect("Parent menu created successfully!", "/Admin/ParentMenus.aspx");
            }
            else
            {
                SqlCommand cmd = new SqlCommand("UPDATE IT_Menus SET MenuName=@MenuName, PageName=@PageName, MenuListNo=@MenuListNo, ModuleId=@ModuleId, MenuIcon=@MenuIcon, FolderName=@FolderName, MenuDescription=@MenuDescription, Status=@Status, ModifiedBy=@ModifiedBy, ModifiedOn=@ModifiedOn WHERE MenuKey=@MenuKey AND MenuType=0");
                cmd.Parameters.AddWithValue("@MenuName", txt_menuname.Text.Trim());
                cmd.Parameters.AddWithValue("@PageName", txt_pagename.Text.Trim());
                cmd.Parameters.AddWithValue("@MenuListNo", txt_menulistno.Text.Trim());
                cmd.Parameters.AddWithValue("@ModuleId", ddl_module.SelectedValue);
                cmd.Parameters.AddWithValue("@MenuIcon", txt_menuicon.Text.Trim());
                cmd.Parameters.AddWithValue("@FolderName", txt_foldername.Text.Trim());
                cmd.Parameters.AddWithValue("@MenuDescription", txt_menudesc.InnerText.Trim());
                cmd.Parameters.AddWithValue("@Status", Convert.ToInt32(rblStatus.SelectedValue));
                cmd.Parameters.AddWithValue("@ModifiedBy", SC.Userid);
                cmd.Parameters.Add("@ModifiedOn", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.AddWithValue("@MenuKey", str_id);
                DA.ExecuteNonQuery(cmd);
                ShowSuccessAndRedirect("Parent menu updated successfully!", "/Admin/ParentMenus.aspx");
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
