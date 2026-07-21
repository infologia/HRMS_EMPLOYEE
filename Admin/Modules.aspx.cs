using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.Services;

public partial class Admin_Modules : System.Web.UI.Page
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
                control1.Text = "Modules";
        }

        string str_query = "SELECT ModuleId, ModuleName, Description, IsActive FROM IT_Modules ORDER BY CreatedOn DESC";
        SqlCommand cmd = new SqlCommand(str_query);
        DataTable dt_modules = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_modules);

        if (dt_modules.Rows.Count > 0)
        {
            ds.Tables[0].Columns.Add("ActiveText");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                bool isActive = Convert.ToBoolean(dr["IsActive"]);
                dr["ActiveText"] = isActive ? "<span class='label label-success'>Yes</span>" : "<span class='label label-default'>No</span>";
            }
            this.PH.LoadGridItem(ds, PH_Modules, "Modules.txt", "");
        }
    }

    [WebMethod]
    public static string SaveModule(string moduleName, string description, bool isActive)
    {
        try
        {
            SessionCustom SC = new SessionCustom();
            string userId = SC.Userid;

            SqlCommand cmd = new SqlCommand("INSERT INTO IT_Modules(ModuleName, Description, IsActive, CreatedBy, CreatedOn) VALUES(@ModuleName, @Description, @IsActive, @CreatedBy, @CreatedOn)");
            cmd.Parameters.AddWithValue("@ModuleName", moduleName);
            cmd.Parameters.AddWithValue("@Description", string.IsNullOrEmpty(description) ? (object)DBNull.Value : description);
            cmd.Parameters.AddWithValue("@IsActive", isActive);
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
    public static string UpdateModule(string moduleId, string moduleName, string description, bool isActive)
    {
        try
        {
            SessionCustom SC = new SessionCustom();
            string userId = SC.Userid;

            SqlCommand cmd = new SqlCommand("UPDATE IT_Modules SET ModuleName=@ModuleName, Description=@Description, IsActive=@IsActive, ModifiedBy=@ModifiedBy, ModifiedOn=@ModifiedOn WHERE ModuleId=@ModuleId");
            cmd.Parameters.AddWithValue("@ModuleName", moduleName);
            cmd.Parameters.AddWithValue("@Description", string.IsNullOrEmpty(description) ? (object)DBNull.Value : description);
            cmd.Parameters.AddWithValue("@IsActive", isActive);
            cmd.Parameters.AddWithValue("@ModifiedBy", userId);
            cmd.Parameters.Add("@ModifiedOn", SqlDbType.DateTime).Value = DateTime.Now;
            cmd.Parameters.AddWithValue("@ModuleId", moduleId);

            new DataAccess().ExecuteNonQuery(cmd);
            return "true";
        }
        catch (Exception)
        {
            return "false";
        }
    }

    [WebMethod]
    public static string DeleteModule(string str_moduleid)
    {
        string str_Response = "0";
        try
        {
            DataAccess DA1 = new DataAccess();
            SqlCommand cmd = new SqlCommand("DELETE FROM IT_Modules WHERE ModuleId=@ModuleId");
            cmd.Parameters.AddWithValue("@ModuleId", str_moduleid);
            DA1.ExecuteNonQuery(cmd);
            return str_Response = "1";
        }
        catch (Exception)
        {
            return str_Response;
        }
    }
}