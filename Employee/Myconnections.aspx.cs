using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_Myconnections : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_id = "";
    string str_userid = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        DA = new DataAccess();
        SC = new SessionCustom();

        Label control1 = this.Master.FindControl("lbl_bread") as Label;
        if (control1 != null)
            control1.Text = "My Connections";

        if (!string.IsNullOrEmpty(Request.QueryString["id"]))
        {
            this.str_id = Request.QueryString["id"].ToString();
            int viewId = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["Viewid"]))
            {
                string view_id = Request.QueryString["Viewid"].ToString();
                viewId = Convert.ToInt32(view_id);

            }
            if (!IsPostBack)
            {
                BindLeadType();
                AssignValues();
            }
            if (viewId == 0)
            {
                btn_update.Visible = false;
                btn_request.Visible = false;
            }
            else
            {
                btn_update.Visible = true;
                btn_request.Visible = false;
            }
        }
        else
        {
            if (!IsPostBack)
            {
                BindLeadType();
            }

            btn_request.Visible = true;
            btn_update.Visible = false;
        }
    }




    private void BindLeadType()
    {
        string query = "SELECT LeadTypeKey, LeadType FROM IT_LeadType";
        SqlCommand cmd = new SqlCommand(query);

        DataSet ds = DA.GetDataSet(cmd);
        ddl_leadtype.DataSource = ds;
        ddl_leadtype.DataTextField = "LeadType";
        ddl_leadtype.DataValueField = "LeadTypeKey";
        ddl_leadtype.DataBind();

        ddl_leadtype.Items.Insert(0, new ListItem("-- Select Lead Type --", ""));
    }

     private void AssignValues()
    {
        int connectionId;
        if (!int.TryParse(str_id, out connectionId))
            return;

        string query = "SELECT * FROM MyConnections WHERE ConnectionKey=@ConnectionKey";
        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@ConnectionKey", connectionId);
        DataTable dt = DA.GetDataTable(cmd);

        if (dt.Rows.Count > 0)
        {
            txt_name.Text = dt.Rows[0]["Name"].ToString();
            txt_company.Text = dt.Rows[0]["Company"].ToString();
            txt_position.Text = dt.Rows[0]["Position"].ToString();
            txt_email.Text = dt.Rows[0]["Email"].ToString();
            txt_mobile.Text = dt.Rows[0]["Mobile"].ToString();
            txt_source.Text = dt.Rows[0]["Source"].ToString();
            txt_description.Text = dt.Rows[0]["Description"].ToString();
            ddl_leadtype.SelectedValue = dt.Rows[0]["LeadType"].ToString();
        }

        // Load existing followup rows
        SqlCommand fuCmd = new SqlCommand("SELECT Description, CONVERT(varchar(10), FollowUpDate, 103) AS FollowUpDate FROM MyConnectionFollowUp WHERE ConnectionKey=@ConnectionKey");
        fuCmd.Parameters.AddWithValue("@ConnectionKey", connectionId);
        DataTable dtFu = DA.GetDataTable(fuCmd);

        if (dtFu != null && dtFu.Rows.Count > 0)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (DataRow fuRow in dtFu.Rows)
            {
                sb.Append("<tr>");
                sb.AppendFormat("<td style='padding:7px 10px; border:1px solid #dde3ea;'><input type='text' class='form-control' name='fuDesc[]' value='{0}' placeholder='Enter Description' /></td>", fuRow["Description"].ToString().Replace("'", "&#39;"));
                sb.Append("<td style='padding:7px 10px; border:1px solid #dde3ea;'><div class='input-group'><span class='input-group-addon'><i class='icon-calendar22'></i></span>");
                sb.AppendFormat("<input type='text' class='form-control pickadate' name='fuDate[]' value='{0}' placeholder='DD/MM/YYYY' readonly='readonly' /></div></td>", fuRow["FollowUpDate"].ToString());
                sb.Append("<td style='text-align:center; padding:7px 10px; border:1px solid #dde3ea;'><button type='button' class='btn btn-danger btn-xs removeFuRow'><i class='icon-trash'></i></button></td>");
                sb.Append("</tr>");
            }
            followupBody.InnerHtml = sb.ToString();
        }
    }

    protected void btn_request_Click(object sender, EventArgs e)
    {
        InsertOrUpdateConnection(false);
    }

    protected void btn_update_Click(object sender, EventArgs e)
    {
        InsertOrUpdateConnection(true);
    }

    private void InsertOrUpdateConnection(bool isUpdate)
    {
        Guid userId = new Guid(SC.Userid);
        int connectionKey = 0;

        if (isUpdate)
        {
            if (!int.TryParse(str_id, out connectionKey))
                return;

            SqlCommand cmd = new SqlCommand(@"
                UPDATE MyConnections
                SET Name=@Name, Company=@Company, Position=@Position,
                    Email=@Email, Mobile=@Mobile, Source=@Source,
                    LeadType=@LeadType, Description=@Description,
                    ModifiedBy=@ModifiedBy, ModifiedOn=GETDATE()
                WHERE ConnectionKey=@ConnectionKey
            ");

            cmd.Parameters.AddWithValue("@ConnectionKey", connectionKey);
            cmd.Parameters.AddWithValue("@ModifiedBy", userId);

            cmd.Parameters.AddWithValue("@Name", txt_name.Text.Trim());
            cmd.Parameters.AddWithValue("@Company", txt_company.Text.Trim());
            cmd.Parameters.AddWithValue("@Position", txt_position.Text.Trim());
            cmd.Parameters.AddWithValue("@Email", txt_email.Text.Trim());
            cmd.Parameters.AddWithValue("@Mobile", txt_mobile.Text.Trim());
            cmd.Parameters.AddWithValue("@Source", txt_source.Text.Trim());
            cmd.Parameters.AddWithValue("@LeadType", ddl_leadtype.SelectedValue);
            cmd.Parameters.AddWithValue("@Description", txt_description.Text.Trim());

        DA.ExecuteNonQuery(cmd);


        }
        else
        {
            SqlCommand cmd = new SqlCommand(@"
                INSERT INTO MyConnections
                (Name, Company, Position, Email, Mobile, Source, LeadType, Description, Status, CreatedBy)
                OUTPUT INSERTED.ConnectionKey
                VALUES
                (@Name, @Company, @Position, @Email, @Mobile, @Source, @LeadType, @Description, 1, @CreatedBy)
            ");

            cmd.Parameters.AddWithValue("@Name", txt_name.Text.Trim());
            cmd.Parameters.AddWithValue("@Company", txt_company.Text.Trim());
            cmd.Parameters.AddWithValue("@Position", txt_position.Text.Trim());
            cmd.Parameters.AddWithValue("@Email", txt_email.Text.Trim());
            cmd.Parameters.AddWithValue("@Mobile", txt_mobile.Text.Trim());
            cmd.Parameters.AddWithValue("@Source", txt_source.Text.Trim());
            cmd.Parameters.AddWithValue("@LeadType", ddl_leadtype.SelectedValue);
            cmd.Parameters.AddWithValue("@Description", txt_description.Text.Trim());
            cmd.Parameters.AddWithValue("@CreatedBy", userId);

            DataTable dt = DA.GetDataTable(cmd);
            connectionKey = Convert.ToInt32(dt.Rows[0]["ConnectionKey"]);
        }

        SaveFollowUps(connectionKey, userId, isUpdate);
        Response.Redirect("Myconnectionsdetails.aspx");
    }

    private void SaveFollowUps(int connectionKey, Guid userId, bool isUpdate)
    {
        if (isUpdate)
        {
            SqlCommand del = new SqlCommand("DELETE FROM MyConnectionFollowUp WHERE ConnectionKey=@ConnectionKey");
            del.Parameters.AddWithValue("@ConnectionKey", connectionKey);
            DA.ExecuteNonQuery(del);
        }

        string[] descs = Request.Form.GetValues("fuDesc[]");
        string[] dates = Request.Form.GetValues("fuDate[]");

        if (descs == null) return;

        for (int i = 0; i < descs.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(descs[i])) continue;

            DateTime fuDate;
            bool hasDate = DateTime.TryParseExact(
                dates != null && dates.Length > i ? dates[i] : "",
                "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out fuDate);

            SqlCommand cmd = new SqlCommand(@"INSERT INTO MyConnectionFollowUp
                (ConnectionKey, Description, FollowUpDate, CreatedBy, CreatedOn)
                VALUES (@ConnectionKey, @Description, @FollowUpDate, @CreatedBy, GETDATE())");
            cmd.Parameters.AddWithValue("@ConnectionKey", connectionKey);
            cmd.Parameters.AddWithValue("@Description", descs[i].Trim());
            cmd.Parameters.AddWithValue("@FollowUpDate", hasDate ? (object)fuDate : DBNull.Value);
            cmd.Parameters.AddWithValue("@CreatedBy", userId);
            DA.ExecuteNonQuery(cmd);
        }
    }

    [WebMethod]
    public static string GetCompanyNames(string query)
    {
        DataAccess DA = new DataAccess();
        SqlCommand cmd = new SqlCommand("SELECT DISTINCT TOP 10 Company FROM IT_Leads WHERE Company LIKE @query UNION SELECT DISTINCT TOP 10 Company FROM MyConnections WHERE Company LIKE @query ORDER BY Company");
        cmd.Parameters.AddWithValue("@query", "%" + query + "%");
        
        DataTable dt = DA.GetDataTable(cmd);
        List<string> companies = new List<string>();
        
        foreach (DataRow row in dt.Rows)
        {
            companies.Add(row["Company"].ToString());
        }
        
        return Newtonsoft.Json.JsonConvert.SerializeObject(companies);
    }
}
