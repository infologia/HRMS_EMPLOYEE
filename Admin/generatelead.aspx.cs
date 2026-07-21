using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI.WebControls;

public partial class Admin_generatelead : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;

    protected void Page_Load(object sender, EventArgs e)
    {
        DA = new DataAccess();
        SC = new SessionCustom();

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Generate/Update Lead";

            if (Request.QueryString["id"] != null)
            {
                btnSave.Text = "Update";
                BindLeadDetails(Request.QueryString["id"].ToString());
            }
            else
            {
                btnSave.Text = "Submit";
            }

            // Show success/error message from redirect
            if (Request.QueryString["msg"] == "updated")
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Lead updated successfully.";
                lblMessage.CssClass = "text-success";
            }
            else if (Request.QueryString["msg"] == "saved")
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Lead saved successfully.";
                lblMessage.CssClass = "text-success";
            }
        }
    }

    // Safely encodes a value for use inside an HTML attribute (used by the
    // contacts Repeater markup: value='<%# AttrEncode(Eval("FirstName")) %>').
    protected string AttrEncode(object value)
    {
        string s = (value == null || value == DBNull.Value) ? "" : value.ToString();
        return HttpUtility.HtmlAttributeEncode(s);
    }

    // Formats a DB date value into the yyyy-MM-dd format required by <input type="date">.
    protected string FormatContactDate(object value)
    {
        if (value == null || value == DBNull.Value) return "";
        DateTime dt;
        if (value is DateTime)
            dt = (DateTime)value;
        else if (!DateTime.TryParse(value.ToString(), out dt))
            return "";
        return dt.ToString("yyyy-MM-dd");
    }

    private void BindLeadDetails(string companyKey)
    {
        string query = "SELECT * FROM IT_GenerateLeads WHERE CompanyKey = @CompanyKey";
        try
        {
            SqlCommand cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@CompanyKey", Convert.ToInt64(companyKey));
            DataTable dt = DA.GetDataTable(cmd);

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                txtCompanyName.Text      = dr["CompanyName"].ToString();
                txtLegalCompanyName.Text = dr["LegalCompanyName"].ToString();
                txtCompanyCode.Text      = dr["CompanyCode"].ToString();
                txtWebsite.Text          = dr["Website"].ToString();
                txtLinkedInURL.Text      = dr["LinkedInURL"].ToString();
                txtIndustry.Text         = dr["Industry"].ToString();
                txtBusinessType.Text     = dr["BusinessType"].ToString();
                txtCompanySize.Text      = dr["CompanySize"].ToString();
                txtEmployeeCount.Text    = dr["EmployeeCount"].ToString();
                txtAnnualRevenue.Text    = dr["AnnualRevenue"].ToString();
                txtCountry.Text          = dr["Country"].ToString();
                txtState.Text            = dr["State"].ToString();
                txtCity.Text             = dr["City"].ToString();
                txtTimeZone.Text         = dr["TimeZone"].ToString();
                txtAddress.Text          = dr["Address"].ToString();
                txtPostalCode.Text       = dr["PostalCode"].ToString();
                txtCompanyPhone.Text     = dr["CompanyPhone"].ToString();
                txtCompanyEmail.Text     = dr["CompanyEmail"].ToString();
                txtContactPageURL.Text   = dr["ContactPageURL"].ToString();
                txtSource.Text           = dr["Source"].ToString();
                txtSourceURL.Text        = dr["SourceURL"].ToString();
                txtNotes.Text            = dr["Notes"].ToString();

                if (ddlLeadStatus.Items.FindByValue(dr["LeadStatus"].ToString()) != null)
                    ddlLeadStatus.SelectedValue = dr["LeadStatus"].ToString();
                if (ddlPriority.Items.FindByValue(dr["Priority"].ToString()) != null)
                    ddlPriority.SelectedValue = dr["Priority"].ToString();
                bool isActive = dr["IsActive"] != DBNull.Value && Convert.ToInt32(dr["IsActive"]) == 1;
                string isActiveVal = isActive ? "1" : "0";
                string script = "document.getElementById('" + (isActive ? "rblActive" : "rblInactive") + "').checked=true; document.getElementById('hfIsActive').value='" + isActiveVal + "';";
                Page.ClientScript.RegisterStartupScript(this.GetType(), "setIsActive", script, true);

                // Load contact persons - bind directly to the Repeater so the already
                // inserted rows render server-side on the very first page load.
                // (Previously this JSON'd the data into a hidden field and relied on a
                // client-side $(document).ready() script to parse + rebuild the rows,
                // which is why the "Add Row" rows never came back populated on Update.)
                SqlCommand cpCmd = new SqlCommand("SELECT FirstName, LastName, Designation, Department, Email, MobileNumber, LinkedInProfile, NextFollowUpDate FROM IT_GeneratedLeadContacts WHERE CompanyKey = @CompanyKey");
                cpCmd.Parameters.AddWithValue("@CompanyKey", Convert.ToInt64(companyKey));
                DataTable dtCp = DA.GetDataTable(cpCmd);

                if (dtCp != null && dtCp.Rows.Count > 0)
                {
                    rptContacts.DataSource = dtCp;
                    rptContacts.DataBind();
                    phBlankRow.Visible = false; // hide the empty starter row, real rows are now rendered
                }
                else
                {
                    phBlankRow.Visible = true; // no saved contacts yet, keep one blank row for entry
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Visible = true;
            lblMessage.Text = "Error loading lead details: " + ex.Message;
            lblMessage.CssClass = "text-danger";
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            bool isUpdate = Request.QueryString["id"] != null;
            SqlCommand cmd = new SqlCommand();
            object userId = string.IsNullOrEmpty(SC.Userid) ? (object)DBNull.Value : Guid.Parse(SC.Userid);

            if (isUpdate)
            {
                cmd.CommandText = @"UPDATE IT_GenerateLeads SET 
                            CompanyName = @CompanyName, LegalCompanyName = @LegalCompanyName, CompanyCode = @CompanyCode, 
                            Website = @Website, LinkedInURL = @LinkedInURL, Industry = @Industry, BusinessType = @BusinessType, 
                            CompanySize = @CompanySize, EmployeeCount = @EmployeeCount, AnnualRevenue = @AnnualRevenue, 
                            Country = @Country, State = @State, City = @City, TimeZone = @TimeZone, Address = @Address, 
                            PostalCode = @PostalCode, CompanyPhone = @CompanyPhone, CompanyEmail = @CompanyEmail, 
                            ContactPageURL = @ContactPageURL, Source = @Source, SourceURL = @SourceURL, 
                            LeadStatus = @LeadStatus, Priority = @Priority, Notes = @Notes,
                            IsActive = @IsActive, ModifiedOn = GETDATE(), ModifiedBy = @UserId
                          WHERE CompanyKey = @CompanyKey";
                cmd.Parameters.AddWithValue("@CompanyKey", Convert.ToInt64(Request.QueryString["id"]));
            }
            else
            {
                cmd.CommandText = @"INSERT INTO IT_GenerateLeads 
                            (CompanyName, LegalCompanyName, CompanyCode, Website, LinkedInURL, Industry, BusinessType, 
                             CompanySize, EmployeeCount, AnnualRevenue, Country, State, City, TimeZone, Address, 
                             PostalCode, CompanyPhone, CompanyEmail, ContactPageURL, Source, SourceURL, 
                             LeadStatus, Priority, Notes, CreatedBy, IsActive)
                          VALUES 
                            (@CompanyName, @LegalCompanyName, @CompanyCode, @Website, @LinkedInURL, @Industry, @BusinessType, 
                             @CompanySize, @EmployeeCount, @AnnualRevenue, @Country, @State, @City, @TimeZone, @Address, 
                             @PostalCode, @CompanyPhone, @CompanyEmail, @ContactPageURL, @Source, @SourceURL, 
                             @LeadStatus, @Priority, @Notes, @UserId, @IsActive);
                          SELECT SCOPE_IDENTITY();";

            }

            cmd.Parameters.AddWithValue("@CompanyName",      txtCompanyName.Text.Trim());
            cmd.Parameters.AddWithValue("@LegalCompanyName", txtLegalCompanyName.Text.Trim());
            cmd.Parameters.AddWithValue("@CompanyCode",      txtCompanyCode.Text.Trim());
            cmd.Parameters.AddWithValue("@Website",          txtWebsite.Text.Trim());
            cmd.Parameters.AddWithValue("@LinkedInURL",      txtLinkedInURL.Text.Trim());
            cmd.Parameters.AddWithValue("@Industry",         txtIndustry.Text.Trim());
            cmd.Parameters.AddWithValue("@BusinessType",     txtBusinessType.Text.Trim());
            cmd.Parameters.AddWithValue("@CompanySize",      txtCompanySize.Text.Trim());
            cmd.Parameters.AddWithValue("@EmployeeCount",    string.IsNullOrEmpty(txtEmployeeCount.Text) ? (object)DBNull.Value : Convert.ToInt32(txtEmployeeCount.Text));
            cmd.Parameters.AddWithValue("@AnnualRevenue",    string.IsNullOrEmpty(txtAnnualRevenue.Text) ? (object)DBNull.Value : Convert.ToDecimal(txtAnnualRevenue.Text));
            cmd.Parameters.AddWithValue("@Country",          txtCountry.Text.Trim());
            cmd.Parameters.AddWithValue("@State",            txtState.Text.Trim());
            cmd.Parameters.AddWithValue("@City",             txtCity.Text.Trim());
            cmd.Parameters.AddWithValue("@TimeZone",         txtTimeZone.Text.Trim());
            cmd.Parameters.AddWithValue("@Address",          txtAddress.Text.Trim());
            cmd.Parameters.AddWithValue("@PostalCode",       txtPostalCode.Text.Trim());
            cmd.Parameters.AddWithValue("@CompanyPhone",     txtCompanyPhone.Text.Trim());
            cmd.Parameters.AddWithValue("@CompanyEmail",     txtCompanyEmail.Text.Trim());
            cmd.Parameters.AddWithValue("@ContactPageURL",   txtContactPageURL.Text.Trim());
            cmd.Parameters.AddWithValue("@Source",           txtSource.Text.Trim());
            cmd.Parameters.AddWithValue("@SourceURL",        txtSourceURL.Text.Trim());
            cmd.Parameters.AddWithValue("@LeadStatus",       ddlLeadStatus.SelectedValue);
            cmd.Parameters.AddWithValue("@Priority",         ddlPriority.SelectedValue);
            cmd.Parameters.AddWithValue("@Notes",            txtNotes.Text.Trim());
            cmd.Parameters.AddWithValue("@IsActive", Request.Form["hfIsActive"] == "1" ? 1 : 0);
            cmd.Parameters.AddWithValue("@UserId",           userId);

            string companyKey;
            if (isUpdate)
            {
                DA.ExecuteNonQuery(cmd);
                companyKey = Request.QueryString["id"].ToString();
            }
            else
            {
                DataTable dtKey = DA.GetDataTable(cmd);
                companyKey = dtKey.Rows[0][0].ToString();
            }

            SaveContactPersons(companyKey, isUpdate, userId);

            if (isUpdate)
                Response.Redirect("generatelead.aspx?id=" + Request.QueryString["id"] + "&msg=updated");
            else
                Response.Redirect("generateleads.aspx?msg=saved");
        }
        catch (Exception ex)
        {
            lblMessage.Visible = true;
            lblMessage.Text = "An error occurred: " + ex.Message;
            lblMessage.CssClass = "text-danger";
        }
    }

    private void SaveContactPersons(string companyKey, bool isUpdate, object userId)
    {
        if (isUpdate)
        {
            SqlCommand del = new SqlCommand("DELETE FROM IT_GeneratedLeadContacts WHERE CompanyKey = @CompanyKey");
            del.Parameters.AddWithValue("@CompanyKey", Convert.ToInt64(companyKey));
            DA.ExecuteNonQuery(del);
        }

        // Read contacts JSON from hidden field (set by client-side JS before form submit)
        string contactsJson = Request.Form["hfContactPersons"];
        if (string.IsNullOrWhiteSpace(contactsJson)) return;

        var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        var persons = serializer.Deserialize<System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, string>>>(contactsJson);
        if (persons == null || persons.Count == 0) return;

        foreach (var p in persons)
        {
            string firstName = p.ContainsKey("FirstName") ? p["FirstName"].Trim() : "";
            if (string.IsNullOrWhiteSpace(firstName)) continue;

            SqlCommand ins = new SqlCommand(@"INSERT INTO IT_GeneratedLeadContacts 
                (CompanyKey, FirstName, LastName, Designation, Department, Email, MobileNumber, LinkedInProfile, NextFollowUpDate, CreatedBy)
                VALUES (@CompanyKey, @FirstName, @LastName, @Designation, @Department, @Email, @Mobile, @LinkedIn, @FollowUp, @UserId)");
            ins.Parameters.AddWithValue("@CompanyKey",  Convert.ToInt64(companyKey));
            ins.Parameters.AddWithValue("@FirstName",   firstName);
            ins.Parameters.AddWithValue("@LastName",    p.ContainsKey("LastName")        ? p["LastName"].Trim()        : "");
            ins.Parameters.AddWithValue("@Designation", p.ContainsKey("Designation")     ? p["Designation"].Trim()     : "");
            ins.Parameters.AddWithValue("@Department",  p.ContainsKey("Department")      ? p["Department"].Trim()      : "");
            ins.Parameters.AddWithValue("@Email",       p.ContainsKey("Email")           ? p["Email"].Trim()           : "");
            ins.Parameters.AddWithValue("@Mobile",      p.ContainsKey("MobileNumber")    ? p["MobileNumber"].Trim()    : "");
            ins.Parameters.AddWithValue("@LinkedIn",    p.ContainsKey("LinkedInProfile") ? p["LinkedInProfile"].Trim() : "");
            string fu = p.ContainsKey("NextFollowUpDate") ? p["NextFollowUpDate"] : "";
            ins.Parameters.AddWithValue("@FollowUp",    !string.IsNullOrEmpty(fu) ? (object)Convert.ToDateTime(fu) : DBNull.Value);
            ins.Parameters.AddWithValue("@UserId",      userId);
            DA.ExecuteNonQuery(ins);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect("generateleads.aspx");
    }

    private void ClearFields()
    {
        txtCompanyName.Text = "";
        txtLegalCompanyName.Text = "";
        txtCompanyCode.Text = "";
        txtWebsite.Text = "";
        txtLinkedInURL.Text = "";
        txtIndustry.Text = "";
        txtBusinessType.Text = "";
        txtCompanySize.Text = "";
        txtEmployeeCount.Text = "";
        txtAnnualRevenue.Text = "";
        txtCountry.Text = "";
        txtState.Text = "";
        txtCity.Text = "";
        txtTimeZone.Text = "";
        txtAddress.Text = "";
        txtPostalCode.Text = "";
        txtCompanyPhone.Text = "";
        txtCompanyEmail.Text = "";
        txtContactPageURL.Text = "";
        txtSource.Text = "";
        txtSourceURL.Text = "";
        ddlLeadStatus.SelectedIndex = 0;
        ddlPriority.SelectedIndex = 0;
        txtNotes.Text = "";
    }
}
