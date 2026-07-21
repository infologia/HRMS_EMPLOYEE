using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IdentityModel.Protocols.WSTrust;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_Myleadsdetails : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_responsestatus = "";
    string str_userkey = "";
    string str_requestleave = "";
    string str_requestleave1 = "";
    private string key = "";
    string str_id = "";


    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();

        Label control1 = this.Master.FindControl("lbl_bread") as Label;
        if (control1 != null)
            control1.Text = "Leads";

        // Always check if QueryString has id
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
                assignvalues();
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
                LoadEmptyContactRow();
            }

            btn_request.Visible = true;
            btn_update.Visible = false;
        }
    }


    private void BindLeadType()
    {
        string str_lead = "select * from IT_LeadType";


        {
            SqlCommand cmd = new SqlCommand(str_lead);
            DataSet reader = this.DA.GetDataSet(cmd);

            ddl_leadtype.DataSource = reader;
            ddl_leadtype.DataTextField = "LeadType";
            ddl_leadtype.DataValueField = "LeadTypeKey";
            ddl_leadtype.DataBind();

            ddl_leadtype.Items.Insert(0, new ListItem("-- Select Lead Type --", ""));
        }
    }
    public string GetSalesStatusOptions(string selected)
    {
        DataTable dt = DA.GetDataTable(
            "SELECT SalesStatusKey, SalesStatus FROM IT_SalesStatus ORDER BY SalesStatusKey");

        StringBuilder sb = new StringBuilder();
        sb.Append("<option value=''>Select</option>");

        foreach (DataRow dr in dt.Rows)
        {
            string sel = dr["SalesStatusKey"].ToString() == selected ? "selected='selected'" : "";
            sb.Append("<option value='" + dr["SalesStatusKey"].ToString() + "' " + sel + ">"
                      + dr["SalesStatus"].ToString() + "</option>");
        }
        return sb.ToString();
    }

    protected void btn_request_Click(object sender, EventArgs e)
    {
        Guid userId = new Guid(SC.Userid.ToString());

        SqlCommand cmd = new SqlCommand(@"INSERT INTO IT_Leads (Company,Email,Mobile,Source,Description,LeadType,CreatedBy) OUTPUT INSERTED.LeadKey VALUES (@Company,@Email,@Mobile,@Source,@Description,@LeadType,@CreatedBy)");
        cmd.Parameters.AddWithValue("@Company", txt_company.Text.Trim());
        cmd.Parameters.AddWithValue("@Email", txt_email.Text.Trim());
        cmd.Parameters.AddWithValue("@Mobile", txt_mobile.Text.Trim());
        cmd.Parameters.AddWithValue("@Source", txt_source.Text.Trim());
        cmd.Parameters.AddWithValue("@Description", txt_description.Text.Trim());
        cmd.Parameters.AddWithValue("@LeadType", ddl_leadtype.SelectedValue);
        cmd.Parameters.Add("@CreatedBy", SqlDbType.UniqueIdentifier).Value = userId;


        DataTable dt = DA.GetDataTable(cmd);
        if (dt.Rows.Count == 0)
        {
            // Handle error
            Response.Write("<script>alert('Lead insertion failed');</script>");
            return;
        }

        int leadKey = Convert.ToInt32(dt.Rows[0]["LeadKey"]);
        // Insert Contacts
        string[] names = Request.Form.GetValues("contact_name");
        string[] positions = Request.Form.GetValues("contact_position");
        string[] contacts = Request.Form.GetValues("contact_no");
        string[] emails = Request.Form.GetValues("contact_email");
        string[] descriptions = Request.Form.GetValues("contact_desc");
        string[] statuses = Request.Form.GetValues("contact_status");

        if (names != null && names.Length > 0)
        {
            for (int i = 0; i < names.Length; i++)
            {
                string query = @"
                INSERT INTO IT_LeadContacts 
                (LeadKey,Name, Position, ContactNo, Email, Description, Status)
                VALUES
                (@LeadKey,@Name, @Position, @ContactNo, @Email, @Description, @Status)";

                using (SqlCommand cmdContact = new SqlCommand(query))
                {
                    cmdContact.Parameters.AddWithValue("@LeadKey", leadKey);
                    cmdContact.Parameters.AddWithValue("@Name", names[i]);
                    cmdContact.Parameters.AddWithValue("@Position", positions[i]);
                    cmdContact.Parameters.AddWithValue("@ContactNo", contacts[i]);
                    cmdContact.Parameters.AddWithValue("@Email", emails[i]);
                    cmdContact.Parameters.AddWithValue("@Description", descriptions[i]);
                    cmdContact.Parameters.AddWithValue("@Status", statuses[i]);

                    DA.ExecuteNonQuery(cmdContact);
                }
            }
            Response.Redirect("Myleads.aspx");
            ClearForm();
        }
    }

    public void assignvalues()
    {
        string str_assing = "select * from IT_Leads a left outer join IT_SalesStatus b on a.Status=b.SalesStatusKey left outer join IT_LeadType c on a.LeadType=c.LeadTypeKey where a.LeadKey=@LeadKey";
        SqlCommand cmd = new SqlCommand(str_assing);
        cmd.Parameters.AddWithValue("@LeadKey", this.str_id);
        DataTable dt_leadvalue = this.DA.GetDataTable(cmd);
        if (dt_leadvalue.Rows.Count > 0)
        {

            txt_company.Text = dt_leadvalue.Rows[0]["Company"].ToString();
            txt_mobile.Text = dt_leadvalue.Rows[0]["Mobile"].ToString();
            txt_email.Text = dt_leadvalue.Rows[0]["Email"].ToString();
            txt_description.Text = dt_leadvalue.Rows[0]["Description"].ToString();
            txt_source.Text = dt_leadvalue.Rows[0]["Source"].ToString();
            ddl_leadtype.SelectedValue = dt_leadvalue.Rows[0]["LeadType"].ToString();
        }
        LoadContacts();
    }
    private void LoadContacts()
    {
        string sql = @"SELECT ContactKey, Name, Position, ContactNo, Email, Description, Status
                   FROM IT_LeadContacts
                   WHERE LeadKey=@LeadKey";

        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.Add("@LeadKey", SqlDbType.Int).Value = this.str_id;

        DataTable dt = DA.GetDataTable(cmd);

        if (dt.Rows.Count == 0)
        {
            LoadEmptyContactRow();
            return;
        }

        StringBuilder sb = new StringBuilder();

        foreach (DataRow dr in dt.Rows)
        {
            sb.Append("<tr>");

            sb.Append("<td>");
            sb.Append("<input type='hidden' name='contact_id' value='" + dr["ContactKey"] + "' />");
            sb.Append("<input type='text' class='form-control' name='contact_name' value='" + dr["Name"] + "' />");
            sb.Append("</td>");

            sb.Append("<td><input type='text' class='form-control' name='contact_position' value='" + dr["Position"] + "' /></td>");
            sb.Append("<td><input type='text' class='form-control' name='contact_no' value='" + dr["ContactNo"] + "' /></td>");
            sb.Append("<td><input type='email' class='form-control' name='contact_email' value='" + dr["Email"] + "' /></td>");
            sb.Append("<td><input type='text' class='form-control' name='contact_desc' value='" + dr["Description"] + "' /></td>");

            sb.Append("<td>");
            sb.Append("<select class='form-control contact_status' name='contact_status'>");
            sb.Append(GetSalesStatusOptions(dr["Status"].ToString()));
            sb.Append("</select>");
            sb.Append("</td>");

            sb.Append("<td class='text-center'>");
            sb.Append("<button type='button' class='btn btn-danger' onclick='removeRow(this)'>Remove</button>");
            sb.Append("</td>");

            sb.Append("</tr>");
        }

        ltContacts.Text = sb.ToString();
    }

    private void LoadEmptyContactRow()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append("<tr>");

        sb.Append("<td><input type='text' class='form-control' name='contact_name' /></td>");
        sb.Append("<td><input type='text' class='form-control' name='contact_position' /></td>");
        sb.Append("<td><input type='text' class='form-control' name='contact_no' /></td>");
        sb.Append("<td><input type='email' class='form-control' name='contact_email' /></td>");
        sb.Append("<td><input type='text' class='form-control' name='contact_desc' /></td>");

        sb.Append("<td>");
        sb.Append("<select class='form-control contact_status' name='contact_status'>");
        sb.Append(GetSalesStatusOptions(""));
        sb.Append("</select>");
        sb.Append("</td>");

        sb.Append("<td class='text-center'>");
        sb.Append("<button type='button' class='btn btn-danger' onclick='removeRow(this)'>Remove</button>");
        sb.Append("</td>");

        sb.Append("</tr>");

        ltContacts.Text = sb.ToString();
    }

    protected void btn_update_Click(object sender, EventArgs e)
    {
        Guid userId = new Guid(SC.Userid.ToString());
        int leadKey;
        if (!int.TryParse(this.str_id, out leadKey))
        {
            return;
        }
        int leadType = 0;
        int.TryParse(ddl_leadtype.SelectedValue, out leadType);

        SqlCommand cmd = new SqlCommand(@"UPDATE IT_Leads SET Company=@Company,Email=@Email,Mobile=@Mobile,Source=@Source,Description=@Description,LeadType=@LeadType, ModifiedBy=@ModifiedBy,ModifiedOn=GETDATE() WHERE LeadKey=@LeadKey ");
        cmd.Parameters.Add("@LeadKey", SqlDbType.Int).Value = leadKey;
        cmd.Parameters.Add("@Company", SqlDbType.NVarChar, 100).Value = txt_company.Text.Trim();
        cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 100).Value = txt_email.Text.Trim();
        cmd.Parameters.Add("@Mobile", SqlDbType.NVarChar, 15).Value = txt_mobile.Text.Trim();
        cmd.Parameters.Add("@Source", SqlDbType.NVarChar, 50).Value = txt_source.Text.Trim();
        cmd.Parameters.Add("@Description", SqlDbType.NVarChar, -1).Value = txt_description.Text.Trim();
        cmd.Parameters.Add("@LeadType", SqlDbType.Int).Value = leadType;
        cmd.Parameters.Add("@ModifiedBy", SqlDbType.UniqueIdentifier).Value = userId;
        DA.ExecuteNonQuery(cmd);
        //  Contact Arrays
        string[] contactIds = Request.Form.GetValues("contact_id");
        string[] names = Request.Form.GetValues("contact_name");
        string[] positions = Request.Form.GetValues("contact_position");
        string[] contacts = Request.Form.GetValues("contact_no");
        string[] emails = Request.Form.GetValues("contact_email");
        string[] descriptions = Request.Form.GetValues("contact_desc");
        string[] statuses = Request.Form.GetValues("contact_status");
        string[] deletedIds = Request.Form.GetValues("deleted_contact_id");

        if (deletedIds != null)
        {
            foreach (string id in deletedIds)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    SqlCommand del = new SqlCommand(
                        "DELETE FROM IT_LeadContacts WHERE ContactKey=@ContactKey");
                    del.Parameters.Add("@ContactKey", SqlDbType.Int).Value = id;
                    DA.ExecuteNonQuery(del);
                }
            }
        }

        if (names != null)
        {
            for (int i = 0; i < names.Length; i++)
            {
                string contactId = null;

                if (contactIds != null && i < contactIds.Length)
                {
                    contactId = contactIds[i];
                }

                if (!string.IsNullOrEmpty(contactId))
                {
                    // 🔹 UPDATE
                    SqlCommand updateContact = new SqlCommand(@"
                UPDATE IT_LeadContacts 
                SET Name=@Name,
                    Position=@Position,
                    ContactNo=@ContactNo,
                    Email=@Email,
                    Description=@Description,
                    Status=@Status
                WHERE ContactKey=@ContactKey");

                    updateContact.Parameters.AddWithValue("@ContactKey", contactId);
                    updateContact.Parameters.AddWithValue("@Name", names[i]);
                    updateContact.Parameters.AddWithValue("@Position", positions[i]);
                    updateContact.Parameters.AddWithValue("@ContactNo", contacts[i]);
                    updateContact.Parameters.AddWithValue("@Email", emails[i]);
                    updateContact.Parameters.AddWithValue("@Description", descriptions[i]);
                    updateContact.Parameters.Add("@Status", SqlDbType.Int)
                                 .Value = Convert.ToInt32(statuses[i]);
                    DA.ExecuteNonQuery(updateContact);
                }
                else
                {
                    //  INSERT
                    SqlCommand insertContact = new SqlCommand(@"
                INSERT INTO IT_LeadContacts
                (LeadKey, Name, Position, ContactNo, Email, Description, Status)
                VALUES
                (@LeadKey, @Name, @Position, @ContactNo, @Email, @Description, @Status)");

                    insertContact.Parameters.AddWithValue("@LeadKey", leadKey);
                    insertContact.Parameters.AddWithValue("@Name", names[i]);
                    insertContact.Parameters.AddWithValue("@Position", positions[i]);
                    insertContact.Parameters.AddWithValue("@ContactNo", contacts[i]);
                    insertContact.Parameters.AddWithValue("@Email", emails[i]);
                    insertContact.Parameters.AddWithValue("@Description", descriptions[i]);
                    insertContact.Parameters.Add("@Status", SqlDbType.Int)
                                 .Value = Convert.ToInt32(statuses[i]);
                    DA.ExecuteNonQuery(insertContact);
                }
            }
        }
        Response.Redirect("Myleads.aspx");
    }

    private void ClearForm()
    {
        txt_company.Text = "";
        txt_email.Text = "";
        txt_mobile.Text = "";
        txt_source.Text = "";
        txt_description.Text = "";
        ddl_leadtype.SelectedIndex = 0;
    }

    [WebMethod]
    public static string GetCompanyNames(string query)
    {
        DataAccess DA = new DataAccess();
        SqlCommand cmd = new SqlCommand("SELECT DISTINCT TOP 10 Company FROM IT_Leads WHERE Company LIKE @query ORDER BY Company");
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