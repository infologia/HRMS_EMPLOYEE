using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Text;

public partial class Admin_Roles : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    string str_userid = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();
        str_userid = this.SC.Userid;

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Roles";

            LoadModuleDropdown();
        }

        LoadRoles();
    }

    private void LoadModuleDropdown()
    {
        SqlCommand cmd = new SqlCommand("SELECT ModuleId, ModuleName FROM IT_Modules WHERE IsActive=1 ORDER BY ModuleName");
        DataTable dt = DA.GetDataTable(cmd);
        lstModules.DataSource = dt;
        lstModules.DataTextField = "ModuleName";
        lstModules.DataValueField = "ModuleId";
        lstModules.DataBind();
    }

    private void LoadRoles()
    {
        string str_query = @"SELECT RoleId, RoleName, ModuleIds, Description FROM IT_Roles ORDER BY CreatedOn DESC";
        SqlCommand cmd = new SqlCommand(str_query);
        DataTable dt_roles = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_roles);

        if (dt_roles.Rows.Count > 0)
        {
            ds.Tables[0].Columns.Add("ModuleNames");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string moduleIds = dr["ModuleIds"].ToString();
                if (!string.IsNullOrEmpty(moduleIds))
                {
                    SqlCommand cmdModules = new SqlCommand("SELECT ModuleName FROM IT_Modules WHERE ModuleId IN (" + moduleIds + ")");
                    DataTable dtModules = DA.GetDataTable(cmdModules);
                    StringBuilder sbModules = new StringBuilder();
                    foreach (DataRow drModule in dtModules.Rows)
                    {
                        sbModules.Append(drModule["ModuleName"].ToString() + ", ");
                    }
                    dr["ModuleNames"] = sbModules.ToString().TrimEnd(',', ' ');
                }
                else
                {
                    dr["ModuleNames"] = "-";
                }
            }
            this.PH.LoadGridItem(ds, PH_Roles, "Roles.txt", "");
        }
    }

    [WebMethod]
    public static string SaveRole(string roleName, string moduleIds, string description)
    {
        try
        {
            SessionCustom SC = new SessionCustom();
            string userId = SC.Userid;

            SqlCommand cmd = new SqlCommand("INSERT INTO IT_Roles(RoleName, ModuleIds, Description, CreatedBy, CreatedOn) VALUES(@RoleName, @ModuleIds, @Description, @CreatedBy, @CreatedOn)");
            cmd.Parameters.AddWithValue("@RoleName", roleName);
            cmd.Parameters.AddWithValue("@ModuleIds", string.IsNullOrEmpty(moduleIds) ? (object)DBNull.Value : moduleIds);
            cmd.Parameters.AddWithValue("@Description", string.IsNullOrEmpty(description) ? (object)DBNull.Value : description);
            cmd.Parameters.AddWithValue("@CreatedBy", userId);
            cmd.Parameters.Add("@CreatedOn", SqlDbType.DateTime).Value = DateTime.Now;

            new DataAccess().ExecuteNonQuery(cmd);
            return "true";
        }
        catch (Exception)
        {
            return "false";
        }
    }

    [WebMethod]
    public static string UpdateRole(string roleId, string roleName, string moduleIds, string description)
    {
        try
        {
            SessionCustom SC = new SessionCustom();
            string userId = SC.Userid;

            SqlCommand cmd = new SqlCommand("UPDATE IT_Roles SET RoleName=@RoleName, ModuleIds=@ModuleIds, Description=@Description, ModifiedBy=@ModifiedBy, ModifiedOn=@ModifiedOn WHERE RoleId=@RoleId");
            cmd.Parameters.AddWithValue("@RoleName", roleName);
            cmd.Parameters.AddWithValue("@ModuleIds", string.IsNullOrEmpty(moduleIds) ? (object)DBNull.Value : moduleIds);
            cmd.Parameters.AddWithValue("@Description", string.IsNullOrEmpty(description) ? (object)DBNull.Value : description);
            cmd.Parameters.AddWithValue("@ModifiedBy", userId);
            cmd.Parameters.Add("@ModifiedOn", SqlDbType.DateTime).Value = DateTime.Now;
            cmd.Parameters.AddWithValue("@RoleId", roleId);

            new DataAccess().ExecuteNonQuery(cmd);
            return "true";
        }
        catch (Exception)
        {
            return "false";
        }
    }

    [WebMethod]
    public static string DeleteRole(string str_roleid)
    {
        string str_Response = "0";
        try
        {
            DataAccess DA1 = new DataAccess();
            SqlCommand cmd = new SqlCommand("DELETE FROM IT_Roles WHERE RoleId=@RoleId");
            cmd.Parameters.AddWithValue("@RoleId", str_roleid);
            DA1.ExecuteNonQuery(cmd);
            return str_Response = "1";
        }
        catch (Exception)
        {
            return str_Response;
        }
    }
}
