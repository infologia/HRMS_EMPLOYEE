using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_accesscontrols : System.Web.UI.Page
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
                control1.Text = "Access Controls";

            LoadAccessControls();
        }
    }

    private void LoadAccessControls()
    {
        string query = @"SELECT AC_Id, AC_Status, AC_IPOffice, CONVERT(VARCHAR(10), AC_CreatedOn, 103) AS CreatedOn
            FROM IT_AccessControl
            ORDER BY AC_CreatedOn DESC";

        SqlCommand cmd = new SqlCommand(query);
        DataTable dt = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt);

        if (!ds.Tables[0].Columns.Contains("StatusText"))
            ds.Tables[0].Columns.Add("StatusText");

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            int status = Convert.ToInt32(dr["AC_Status"]);
            if (status == 0)
                dr["StatusText"] = "<span class='label label-info'>Office IP</span>";
            else
                dr["StatusText"] = "<span class='label label-success'>Own IP</span>";
        }

        PH.LoadGridItem(ds, PH_AccessControl, "accesscontrol.txt", "");

        string checkQuery = "SELECT COUNT(*) FROM IT_AccessControl";
        SqlCommand checkCmd = new SqlCommand(checkQuery);
        DataTable checkDt = DA.GetDataTable(checkCmd);
        
        if (checkDt.Rows.Count > 0 && Convert.ToInt32(checkDt.Rows[0][0]) > 0)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "disableCreate", "$('#createBtn').prop('disabled', true).addClass('disabled');", true);
        }
    }

    protected void btn_create_Click(object sender, EventArgs e)
    {
        string countQuery = "SELECT COUNT(*) AS RecordCount FROM IT_AccessControl";
        SqlCommand countCmd = new SqlCommand(countQuery);
        DataTable countDt = DA.GetDataTable(countCmd);
        int existingCount = Convert.ToInt32(countDt.Rows[0]["RecordCount"]);

        if (existingCount > 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "infologia",
                "<script>alert('Access Control already exists. Please use Edit option.');</script>");
            return;
        }

        string ipType = Request.Form["createIpType"];
        string officeIP = txt_createOfficeIP.Text.Trim();

        if (string.IsNullOrEmpty(ipType))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "infologia",
                "<script>alert('Please select IP type');</script>");
            return;
        }

        if (string.IsNullOrEmpty(officeIP))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "infologia",
                "<script>alert('Please enter Office IP');</script>");
            return;
        }

        Guid userGuid;
        if (!Guid.TryParse(SC.Userid, out userGuid))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "infologia",
                "<script>alert('Invalid User');</script>");
            return;
        }

        using (SqlConnection conn = new SqlConnection(DA.ConnectionString))
        {
            conn.Open();
            try
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO IT_AccessControl (AC_Status, AC_IPOffice, AC_CreatedBy, AC_CreatedOn)
                        VALUES (@Status, @IPOffice, @CreatedBy, @CreatedOn)";

                    cmd.Parameters.Add("@Status", SqlDbType.Int).Value = Convert.ToInt32(ipType);
                    cmd.Parameters.Add("@IPOffice", SqlDbType.NVarChar).Value = officeIP;
                    cmd.Parameters.Add("@CreatedBy", SqlDbType.UniqueIdentifier).Value = userGuid;
                    cmd.Parameters.Add("@CreatedOn", SqlDbType.DateTime).Value = DateTime.Now;

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "infologia",
                    "<script>alert('Error: " + ex.Message.Replace("'", "") + "');</script>");
                return;
            }
        }

        Response.Redirect("~/Admin/accesscontrols.aspx");
    }

    protected void btn_update_Click(object sender, EventArgs e)
    {
        string id = hf_editId.Value;
        string ipType = Request.Form["editIpType"];
        string officeIP = txt_editOfficeIP.Text.Trim();

        if (string.IsNullOrEmpty(id))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "infologia",
                "<script>alert('Invalid ID');</script>");
            return;
        }
        
        if (string.IsNullOrEmpty(ipType))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "infologia",
                "<script>alert('Please select IP type');</script>");
            return;
        }

        if (string.IsNullOrEmpty(officeIP))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "infologia",
                "<script>alert('Please enter Office IP');</script>");
            return;
        }

        int parsedId;
        if (!int.TryParse(id, out parsedId))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "infologia",
                "<script>alert('Invalid ID format');</script>");
            return;
        }

        Guid userGuid;
        if (!Guid.TryParse(SC.Userid, out userGuid))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "infologia",
                "<script>alert('Invalid User');</script>");
            return;
        }

        using (SqlConnection conn = new SqlConnection(DA.ConnectionString))
        {
            conn.Open();
            try
            {
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE IT_AccessControl 
                        SET AC_Status = @Status, AC_IPOffice = @IPOffice, AC_ModifiedBy = @ModifiedBy, AC_ModifiedOn = @ModifiedOn
                        WHERE AC_Id = @Id";

                    cmd.Parameters.Add("@Status", SqlDbType.Int).Value = Convert.ToInt32(ipType);
                    cmd.Parameters.Add("@IPOffice", SqlDbType.NVarChar).Value = officeIP;
                    cmd.Parameters.Add("@ModifiedBy", SqlDbType.UniqueIdentifier).Value = userGuid;
                    cmd.Parameters.Add("@ModifiedOn", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = parsedId; 

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "infologia",
                    "<script>alert('Error: " + ex.Message.Replace("'", "") + "');</script>");
                return;
            }
        }

        Response.Redirect("~/Admin/accesscontrols.aspx");
    }

    [System.Web.Services.WebMethod]
    public static string GetOfficeIP(int id)
    {
        DataAccess DA = new DataAccess();
        string query = "SELECT AC_IPOffice FROM IT_AccessControl WHERE AC_Id = @Id";
        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@Id", id);
        
        DataTable dt = DA.GetDataTable(cmd);
        if (dt.Rows.Count > 0)
        {
            return dt.Rows[0]["AC_IPOffice"].ToString();
        }
        return "";
    }
}
