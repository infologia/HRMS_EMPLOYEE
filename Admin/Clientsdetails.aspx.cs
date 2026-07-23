using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.UI.WebControls;

public partial class Admin_Clientsdetails : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;

    public string DocOptionsHtml { get; set; }
    private string key = "";
    string str_id = "";

    // Folder where uploaded documents will be saved (relative to site root)
    private readonly string UploadFolder = "~/Uploads/ClientDocs/";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        Form.Enctype = "multipart/form-data";

        if (!string.IsNullOrEmpty(Request.QueryString["id"]))
        {
            this.str_id = Request.QueryString["id"].ToString();
        }

        Label control1 = this.Master.FindControl("lbl_bread") as Label;
        if (control1 != null)
            control1.Text = "Organization";

        LoadDocOptions();

        if (!IsPostBack)
        {
            Loadcountry();
            LoadEmp();
            Loadsales();
            LoadPartyType();
            LoadTaxType();

            if (!string.IsNullOrEmpty(this.str_id))
            {
                assignvalues();
                LoadClientDocuments();
            }
            else
            {
                AddDefaultDocRow();
            }
        }

        if (!string.IsNullOrEmpty(this.str_id))
        {
            SqlCommand cmdRole = new SqlCommand("SELECT role FROM IT_EmployeeRegister WHERE Employeekey = @Employeekey AND role = 11");
            cmdRole.Parameters.AddWithValue("@Employeekey", SC.Userid);
            bool isRole11 = DA.GetDataTable(cmdRole).Rows.Count > 0;
            btn_update.Visible = isRole11;
            btn_request.Visible = false;
        }
        else
        {
            btn_request.Visible = true;
            btn_update.Visible = false;
        }
    }
    private void LoadPartyType()
    {
        string str_pt = "select PT_ID, PT_Name from IT_PartyType";
        SqlCommand cmd = new SqlCommand(str_pt);
        DataSet ds = this.DA.GetDataSet(cmd);

        ddl_Type.DataSource = ds.Tables[0];
        ddl_Type.DataTextField = "PT_Name";
        ddl_Type.DataValueField = "PT_ID";
        ddl_Type.DataBind();
        ddl_Type.Items.Insert(0, new ListItem("-- Select Party Type --", ""));
    }
    
    private void LoadTaxType()
    {
        string str_tt = "select id, name from it_taxtype";
        SqlCommand cmd = new SqlCommand(str_tt);
        DataSet ds = this.DA.GetDataSet(cmd);

        DD_TaxType.DataSource = ds.Tables[0];
        DD_TaxType.DataTextField = "name";
        DD_TaxType.DataValueField = "id";
        DD_TaxType.DataBind();
        DD_TaxType.Items.Insert(0, new ListItem("-- Select Tax Type --", ""));
    }
    private void Loadcountry()
    {
        string str_lead = "select CountryKey,Country from it_countries";
        SqlCommand cmd = new SqlCommand(str_lead);
        DataSet reader = this.DA.GetDataSet(cmd);
        ddl_Country.DataSource = reader;
        ddl_Country.DataTextField = "Country";
        ddl_Country.DataValueField = "CountryKey";
        ddl_Country.DataBind();
        ddl_Country.Items.Insert(0, new ListItem("-- Select Country --", ""));
    }
    private void LoadEmp()
    {
        string str_sts = "select employeekey,CONCAT(Firstname, ' ', Lastname) AS name from IT_EmployeeRegister where Employeestatus=1 and Division in (1,2,3)";
        SqlCommand cmd = new SqlCommand(str_sts);
        DataSet reader = this.DA.GetDataSet(cmd);

        ddl_OnboardBy.DataSource = reader;
        ddl_OnboardBy.DataTextField = "name";
        ddl_OnboardBy.DataValueField = "employeekey";
        ddl_OnboardBy.DataBind();
        ddl_OnboardBy.Items.Insert(0, new ListItem("-- Select Employee --", ""));
    }
    private void Loadsales()
    {
        string query = @"SELECT EmployeeKey,
                            CONCAT(FirstName, ' ', LastName) AS Name
                     FROM IT_EmployeeRegister
                     WHERE Destination IN (24,11) and Employeestatus = 1";

        SqlCommand cmd = new SqlCommand(query);
        DataSet ds = this.DA.GetDataSet(cmd);

        if (ds != null && ds.Tables.Count > 0)
        {
            ddlsalesperson.DataSource = ds.Tables[0];
            ddlsalesperson.DataTextField = "Name";
            ddlsalesperson.DataValueField = "EmployeeKey";
            ddlsalesperson.DataBind();
        }

        ddlsalesperson.Items.Insert(0, new ListItem("-- Select Sales Person --", ""));
    }
    protected void btn_request_Click(object sender, EventArgs e)
    {
        Guid userId = new Guid(SC.Userid.ToString());
        Guid newClientKey = Guid.NewGuid();

        SqlCommand cmd = new SqlCommand(@"
            INSERT INTO IT_ClientDetails 
            (
                ClientKey, ClientCode, ClientName, CompanyName, ContactPerson, Designation,
                Email, AlternateEmail, Mobile, AlternateMobile, PartyType, Industry,
                Website, AddressLine1, AddressLine2, Country, Source, Status,
                Description, OnboardBy, CreatedBy, TaxType, SalesPerson,
                BankName, AccountHolderName, AccountNumber, IFSCCode, Branch, BankAddress,
                ContactName, ContactTitle, Department, Telephone, ContactMobile, ContactEmail,
                AccMgrName, AccMgrEmail, AccMgrMobile, AssignedDate, LastFollowUpDate,
                ContractNumber, ContractStartDate, ContractEndDate, ContractType,
                RenewalDate, NoticePeriod, ContractStatus, SLADetails
            )
            VALUES
            (
                @ClientKey, @ClientCode, @ClientName, @CompanyName, @ContactPerson, @Designation,
                @Email, @AlternateEmail, @Mobile, @AlternateMobile, @PartyType, @Industry,
                @Website, @AddressLine1, @AddressLine2, @Country, @Source, @Status,
                @Description, @OnboardBy, @CreatedBy, @TaxType, @SalesPerson,
                @BankName, @AccountHolderName, @AccountNumber, @IFSCCode, @Branch, @BankAddress,
                @ContactName, @ContactTitle, @Department, @Telephone, @ContactMobile, @ContactEmail,
                @AccMgrName, @AccMgrEmail, @AccMgrMobile, @AssignedDate, @LastFollowUpDate,
                @ContractNumber, @ContractStartDate, @ContractEndDate, @ContractType,
                @RenewalDate, @NoticePeriod, @ContractStatus, @SLADetails
            )");

        cmd.Parameters.Add("@ClientKey", SqlDbType.UniqueIdentifier).Value = newClientKey;
        AddBasicParams(cmd, userId);
        AddBankParams(cmd);
        AddContactParams(cmd);
        AddAccMgrParams(cmd);
        AddContractParams(cmd);

        DA.ExecuteNonQuery(cmd);
        UpsertClientDocuments(newClientKey);

        Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_redirect",
            "showToastr('success','Organization Created Successfully!');" +
            "setTimeout(function(){ window.location.href = 'Clients.aspx'; }, 2000);", true);
    }
    protected void btn_update_Click(object sender, EventArgs e)
    {
        Guid userId = new Guid(SC.Userid.ToString());
        Guid clientKey = new Guid(this.str_id);

        SqlCommand cmd = new SqlCommand(@"
            UPDATE IT_ClientDetails SET
                ClientCode=@ClientCode, ClientName=@ClientName, CompanyName=@CompanyName,
                ContactPerson=@ContactPerson, Designation=@Designation, Email=@Email,
                AlternateEmail=@AlternateEmail, Mobile=@Mobile, AlternateMobile=@AlternateMobile,
                PartyType=@PartyType, Industry=@Industry, Website=@Website,
                AddressLine1=@AddressLine1, AddressLine2=@AddressLine2, Country=@Country,
                Source=@Source, Status=@Status, Description=@Description,
                OnboardBy=@OnboardBy, ModifiedBy=@ModifiedBy, ModifiedOn=GETDATE(), TaxType=@TaxType, SalesPerson=@SalesPerson,
                BankName=@BankName, AccountHolderName=@AccountHolderName, AccountNumber=@AccountNumber,
                IFSCCode=@IFSCCode, Branch=@Branch, BankAddress=@BankAddress,
                ContactName=@ContactName, ContactTitle=@ContactTitle, Department=@Department,
                Telephone=@Telephone, ContactMobile=@ContactMobile, ContactEmail=@ContactEmail,
                AccMgrName=@AccMgrName, AccMgrEmail=@AccMgrEmail, AccMgrMobile=@AccMgrMobile,
                AssignedDate=@AssignedDate, LastFollowUpDate=@LastFollowUpDate,
                ContractNumber=@ContractNumber, ContractStartDate=@ContractStartDate,
                ContractEndDate=@ContractEndDate, ContractType=@ContractType,
                RenewalDate=@RenewalDate, NoticePeriod=@NoticePeriod,
                ContractStatus=@ContractStatus, SLADetails=@SLADetails
            WHERE ClientKey=@ClientKey");

        AddBasicParams(cmd, userId, isUpdate: true);
        AddBankParams(cmd);
        AddContactParams(cmd);
        AddAccMgrParams(cmd);
        AddContractParams(cmd);
        cmd.Parameters.Add("@ClientKey", SqlDbType.UniqueIdentifier).Value = clientKey;
        DA.ExecuteNonQuery(cmd);
        UpsertClientDocuments(clientKey);

        Page.ClientScript.RegisterStartupScript(this.GetType(), "toastr_redirect",
            "showToastr('success','Organization Updated Successfully!');" +
            "setTimeout(function(){ window.location.href = 'Clients.aspx'; }, 2000);", true);
    }
    public void assignvalues()
    {
        string str_assign = "SELECT * FROM IT_ClientDetails WHERE ClientKey = @ClientKey";
        SqlCommand cmd = new SqlCommand(str_assign);
        cmd.Parameters.AddWithValue("@ClientKey", this.str_id);
        DataTable dt = this.DA.GetDataTable(cmd);

        if (dt.Rows.Count > 0)
        {
            DataRow r = dt.Rows[0];

            // Basic Info
            txt_ClientCode.Text         = r["ClientCode"].ToString();
            txt_ClientName.Text         = r["ClientName"].ToString();
            txt_CompanyName.Text        = r["CompanyName"].ToString();
            txt_ContactPerson.Text      = r["ContactPerson"].ToString();
            txt_Designation.Text        = r["Designation"].ToString();
            txt_email.Text              = r["Email"].ToString();
            txt_AlternateEmail.Text     = r["AlternateEmail"].ToString();
            txt_mobile.Text             = r["Mobile"].ToString();
            txt_AlternateMobile.Text    = r["AlternateMobile"].ToString();
            if (ddl_Type.Items.FindByValue(r["PartyType"].ToString()) != null)
                ddl_Type.SelectedValue  = r["PartyType"].ToString();
            txt_Industry.Text           = r["Industry"].ToString();
            txt_Website.Text            = r["Website"].ToString();
            txt_AddressLine1.Text       = r["AddressLine1"].ToString();
            txt_AddressLine2.Text       = r["AddressLine2"].ToString();
            ddl_Country.SelectedValue   = r["Country"].ToString();
            txt_Source.Text             = r["Source"].ToString();
            ddl_Clientstatus.SelectedValue = r["Status"].ToString();
            txt_Description.Text        = r["Description"].ToString();
            if (r["TaxType"] != DBNull.Value && DD_TaxType.Items.FindByValue(r["TaxType"].ToString()) != null)
                DD_TaxType.SelectedValue = r["TaxType"].ToString();
            ddl_OnboardBy.SelectedValue = r["OnboardBy"].ToString();
            if (!string.IsNullOrEmpty(r["SalesPerson"].ToString()))
                ddlsalesperson.SelectedValue = r["SalesPerson"].ToString();

            // Bank Details
            txt_BankName.Text           = r["BankName"].ToString();
            txt_AccountHolderName.Text  = r["AccountHolderName"].ToString();
            txt_AccountNumber.Text      = r["AccountNumber"].ToString();
            txt_IFSCCode.Text           = r["IFSCCode"].ToString();
            txt_Branch.Text             = r["Branch"].ToString();
            txt_BankAddress.Text        = r["BankAddress"].ToString();

            // Contact Details
            txt_ContactName.Text        = r["ContactName"].ToString();
            txt_ContactTitle.Text       = r["ContactTitle"].ToString();
            txt_Department.Text         = r["Department"].ToString();
            txt_Telephone.Text          = r["Telephone"].ToString();
            txt_ContactMobile.Text      = r["ContactMobile"].ToString();
            txt_ContactEmail.Text       = r["ContactEmail"].ToString();

            // Account Manager
            txt_AccMgrName.Text         = r["AccMgrName"].ToString();
            txt_AccMgrEmail.Text        = r["AccMgrEmail"].ToString();
            txt_AccMgrMobile.Text       = r["AccMgrMobile"].ToString();
            if (r["AssignedDate"] != DBNull.Value)
                txt_AssignedDate.Text   = Convert.ToDateTime(r["AssignedDate"]).ToString("yyyy-MM-dd");
            if (r["LastFollowUpDate"] != DBNull.Value)
                txt_LastFollowUpDate.Text = Convert.ToDateTime(r["LastFollowUpDate"]).ToString("yyyy-MM-dd");

            // Documents - loaded separately via LoadClientDocuments()



            // Contract Information
            txt_ContractNumber.Text     = r["ContractNumber"].ToString();
            if (r["ContractStartDate"] != DBNull.Value)
                txt_ContractStartDate.Text = Convert.ToDateTime(r["ContractStartDate"]).ToString("yyyy-MM-dd");
            if (r["ContractEndDate"] != DBNull.Value)
                txt_ContractEndDate.Text   = Convert.ToDateTime(r["ContractEndDate"]).ToString("yyyy-MM-dd");
            if (ddl_ContractType.Items.FindByValue(r["ContractType"].ToString()) != null)
                ddl_ContractType.SelectedValue = r["ContractType"].ToString();
            if (r["RenewalDate"] != DBNull.Value)
                txt_RenewalDate.Text       = Convert.ToDateTime(r["RenewalDate"]).ToString("yyyy-MM-dd");
            txt_NoticePeriod.Text       = r["NoticePeriod"].ToString();
            if (ddl_ContractStatus.Items.FindByValue(r["ContractStatus"].ToString()) != null)
                ddl_ContractStatus.SelectedValue = r["ContractStatus"].ToString();
            txt_SLADetails.Text         = r["SLADetails"].ToString();
        }
    }
    private void LoadDocOptions()
    {
        SqlCommand cmd = new SqlCommand("SELECT CM_ID, DocName FROM IT_ClientDocMaster ORDER BY CM_ID");
        DataTable dt = DA.GetDataTable(cmd);
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("<option value=''>-- Select Document --</option>");
        foreach (DataRow dr in dt.Rows)
        {
            sb.AppendFormat("<option value='{0}'>{1}</option>", dr["CM_ID"], dr["DocName"]);
        }
        DocOptionsHtml = sb.ToString();
        string js = "var docOptionsHtml = \"" + System.Web.HttpUtility.JavaScriptStringEncode(DocOptionsHtml) + "\";";
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "docOptionsJs", js, true);
    }

    private void AddDefaultDocRow()
    {
        tBodyClientDocs.InnerHtml =
            "<tr class='doc-row'>" +
            "<td><select class='form-control' name='clientDocId_0'>" + DocOptionsHtml + "</select></td>" +
            "<td><input type='hidden' name='clientExistingPath_0' value='' /><input type='file' class='form-control' name='clientDocFile_0' accept='.pdf,.jpg,.jpeg,.png,.gif,.webp' /></td>" +
            "<td style='text-align:center;'><input type='hidden' name='clientDocRowIndex[]' value='0' /><button type='button' class='btn-remove-inv removeClientDocRow' title='Remove' onclick='removeClientDocRow(this)'><i class='icon-cross2'></i></button></td>" +
            "</tr>";
        Page.ClientScript.RegisterStartupScript(this.GetType(), "setRowIdx", "clientDocRowIdx = 1;", true);
    }

    private string GetDocOptionsWithSelected(string selectedId)
    {
        SqlCommand cmd = new SqlCommand("SELECT CM_ID, DocName FROM IT_ClientDocMaster ORDER BY CM_ID");
        DataTable dt = DA.GetDataTable(cmd);
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("<option value=''>-- Select Document --</option>");
        foreach (DataRow dr in dt.Rows)
        {
            string selectedStr = dr["CM_ID"].ToString() == selectedId ? " selected='selected'" : "";
            sb.AppendFormat("<option value='{0}'{1}>{2}</option>", dr["CM_ID"], selectedStr, dr["DocName"]);
        }
        return sb.ToString();
    }

    private void LoadClientDocuments()
    {
        if (string.IsNullOrEmpty(this.str_id)) return;

        SqlCommand cmd = new SqlCommand("SELECT Docid, OtherDocsPath FROM IT_ClientDocuments WHERE ClientKey = @ClientKey");
        cmd.Parameters.Add("@ClientKey", SqlDbType.UniqueIdentifier).Value = new Guid(this.str_id);
        DataTable dt = DA.GetDataTable(cmd);

        if (dt == null || dt.Rows.Count == 0)
        {
            AddDefaultDocRow();
            return;
        }

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        int idx = 0;
        foreach (DataRow dr in dt.Rows)
        {
            string docid = dr["Docid"].ToString();
            string existingPath = dr["OtherDocsPath"].ToString();

            string viewLink = !string.IsNullOrEmpty(existingPath)
                ? string.Format("<a href='{0}' target='_blank' style='font-size:12px;color:#3a7bd5;margin-left:10px;display:inline-flex;align-items:center;vertical-align:middle;gap:3px;'><i class='icon-eye'></i> View</a>", ResolveUrl(existingPath))
                : "";

            sb.AppendFormat(
                "<tr class='doc-row'>" +
                "<td><select class='form-control' name='clientDocId_{0}'>{1}</select></td>" +
                "<td><input type='hidden' name='clientExistingPath_{0}' value='{2}' /><input type='file' class='form-control' name='clientDocFile_{0}' accept='.pdf,.jpg,.jpeg,.png,.gif,.webp' />{3}</td>" +
                "<td style='text-align:center;'><input type='hidden' name='clientDocRowIndex[]' value='{0}' /><button type='button' class='btn-remove-inv removeClientDocRow' title='Remove' onclick='removeClientDocRow(this)'><i class='icon-cross2'></i></button></td>" +
                "</tr>",
                idx, GetDocOptionsWithSelected(docid), existingPath, viewLink);
            idx++;
        }
        tBodyClientDocs.InnerHtml = sb.ToString();
        Page.ClientScript.RegisterStartupScript(this.GetType(), "setRowIdx", "clientDocRowIdx = " + idx + ";", true);
    }

    private void UpsertClientDocuments(Guid clientKey)
    {
        SqlCommand cmdDel = new SqlCommand("DELETE FROM IT_ClientDocuments WHERE ClientKey = @ClientKey");
        cmdDel.Parameters.Add("@ClientKey", SqlDbType.UniqueIdentifier).Value = clientKey;
        DA.ExecuteNonQuery(cmdDel);

        string[] rowIndices = Request.Form.GetValues("clientDocRowIndex[]");
        if (rowIndices == null || rowIndices.Length == 0) return;

        string folderPath = Server.MapPath(UploadFolder);
        if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

        Guid userId = new Guid(SC.Userid.ToString());

        foreach (string idxStr in rowIndices)
        {
            int idx = int.Parse(idxStr);
            string docIdVal = Request.Form["clientDocId_" + idx];
            if (string.IsNullOrEmpty(docIdVal)) continue;

            int docId = int.Parse(docIdVal);
            string existingPath = Request.Form["clientExistingPath_" + idx] ?? "";
            string savedPath = existingPath;

            HttpPostedFile file = Request.Files["clientDocFile_" + idx];
            if (file != null && file.ContentLength > 0)
            {
                string uniqueName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                file.SaveAs(Path.Combine(folderPath, uniqueName));
                savedPath = UploadFolder.TrimEnd('/') + "/" + uniqueName;
            }

            SqlCommand cmdIns = new SqlCommand(@"
                INSERT INTO IT_ClientDocuments 
                (CD_ID, ClientKey, OtherDocsPath, Docid, CreatedBy, CreatedDate) 
                VALUES 
                (NEWID(), @ClientKey, @OtherDocsPath, @Docid, @CreatedBy, GETDATE())");

            cmdIns.Parameters.Add("@ClientKey", SqlDbType.UniqueIdentifier).Value = clientKey;
            cmdIns.Parameters.AddWithValue("@OtherDocsPath", string.IsNullOrEmpty(savedPath) ? (object)DBNull.Value : savedPath);
            cmdIns.Parameters.AddWithValue("@Docid", docId);
            cmdIns.Parameters.Add("@CreatedBy", SqlDbType.UniqueIdentifier).Value = userId;

            DA.ExecuteNonQuery(cmdIns);
        }
    }
    private void AddBasicParams(SqlCommand cmd, Guid userId, bool isUpdate = false)
    {
        cmd.Parameters.AddWithValue("@ClientCode",      txt_ClientCode.Text.Trim());
        cmd.Parameters.AddWithValue("@ClientName",      txt_ClientName.Text.Trim());
        cmd.Parameters.AddWithValue("@CompanyName",     txt_CompanyName.Text.Trim());
        cmd.Parameters.AddWithValue("@ContactPerson",   txt_ContactPerson.Text.Trim());
        cmd.Parameters.AddWithValue("@Designation",     txt_Designation.Text.Trim());
        cmd.Parameters.AddWithValue("@Email",           txt_email.Text.Trim());
        cmd.Parameters.AddWithValue("@AlternateEmail",  txt_AlternateEmail.Text.Trim());
        cmd.Parameters.AddWithValue("@Mobile",          txt_mobile.Text.Trim());
        cmd.Parameters.AddWithValue("@AlternateMobile", txt_AlternateMobile.Text.Trim());
        cmd.Parameters.Add("@PartyType", SqlDbType.Int).Value = !string.IsNullOrEmpty(ddl_Type.SelectedValue) ? (object)int.Parse(ddl_Type.SelectedValue) : DBNull.Value;
        cmd.Parameters.AddWithValue("@Industry",        txt_Industry.Text.Trim());
        cmd.Parameters.AddWithValue("@Website",         txt_Website.Text.Trim());
        cmd.Parameters.AddWithValue("@AddressLine1",    txt_AddressLine1.Text.Trim());
        cmd.Parameters.AddWithValue("@AddressLine2",    txt_AddressLine2.Text.Trim());
        cmd.Parameters.AddWithValue("@Country",         ddl_Country.SelectedValue);
        cmd.Parameters.AddWithValue("@Source",          txt_Source.Text.Trim());
        cmd.Parameters.AddWithValue("@Status",          ddl_Clientstatus.SelectedValue);
        cmd.Parameters.AddWithValue("@Description",     txt_Description.Text);
        if (!string.IsNullOrEmpty(DD_TaxType.SelectedValue))
            cmd.Parameters.AddWithValue("@TaxType", DD_TaxType.SelectedValue);
        else
            cmd.Parameters.AddWithValue("@TaxType", DBNull.Value);
        Guid onboardBy;
        if (!Guid.TryParse(ddl_OnboardBy.SelectedValue.Trim(), out onboardBy))
        {
            throw new Exception("Invalid GUID: " + ddl_OnboardBy.SelectedValue);
        }
        cmd.Parameters.Add("@OnboardBy", SqlDbType.UniqueIdentifier).Value = onboardBy;
        cmd.Parameters.Add("@SalesPerson", SqlDbType.UniqueIdentifier).Value = !string.IsNullOrEmpty(ddlsalesperson.SelectedValue) && ddlsalesperson.SelectedValue != "" ? (object)new Guid(ddlsalesperson.SelectedValue) : DBNull.Value;
        if (isUpdate)
            cmd.Parameters.Add("@ModifiedBy", SqlDbType.UniqueIdentifier).Value = userId;
        else
            cmd.Parameters.Add("@CreatedBy", SqlDbType.UniqueIdentifier).Value = userId;
    }

    private void AddBankParams(SqlCommand cmd)
    {
        cmd.Parameters.AddWithValue("@BankName",          txt_BankName.Text.Trim());
        cmd.Parameters.AddWithValue("@AccountHolderName", txt_AccountHolderName.Text.Trim());
        cmd.Parameters.AddWithValue("@AccountNumber",     txt_AccountNumber.Text.Trim());
        cmd.Parameters.AddWithValue("@IFSCCode",          txt_IFSCCode.Text.Trim());
        cmd.Parameters.AddWithValue("@Branch",            txt_Branch.Text.Trim());
        cmd.Parameters.AddWithValue("@BankAddress",       txt_BankAddress.Text.Trim());
    }
    private void AddContactParams(SqlCommand cmd)
    {
        cmd.Parameters.AddWithValue("@ContactName",   txt_ContactName.Text.Trim());
        cmd.Parameters.AddWithValue("@ContactTitle",  txt_ContactTitle.Text.Trim());
        cmd.Parameters.AddWithValue("@Department",    txt_Department.Text.Trim());
        cmd.Parameters.AddWithValue("@Telephone",     txt_Telephone.Text.Trim());
        cmd.Parameters.AddWithValue("@ContactMobile", txt_ContactMobile.Text.Trim());
        cmd.Parameters.AddWithValue("@ContactEmail",  txt_ContactEmail.Text.Trim());
    }
    private void AddAccMgrParams(SqlCommand cmd)
    {
        cmd.Parameters.AddWithValue("@AccMgrName",      txt_AccMgrName.Text.Trim());
        cmd.Parameters.AddWithValue("@AccMgrEmail",     txt_AccMgrEmail.Text.Trim());
        cmd.Parameters.AddWithValue("@AccMgrMobile",    txt_AccMgrMobile.Text.Trim());
        cmd.Parameters.AddWithValue("@AssignedDate",    string.IsNullOrEmpty(txt_AssignedDate.Text)    ? (object)DBNull.Value : DateTime.Parse(txt_AssignedDate.Text));
        cmd.Parameters.AddWithValue("@LastFollowUpDate",string.IsNullOrEmpty(txt_LastFollowUpDate.Text)? (object)DBNull.Value : DateTime.Parse(txt_LastFollowUpDate.Text));
    }

    private void AddContractParams(SqlCommand cmd)
    {
        cmd.Parameters.AddWithValue("@ContractNumber",    txt_ContractNumber.Text.Trim());
        cmd.Parameters.AddWithValue("@ContractStartDate", string.IsNullOrEmpty(txt_ContractStartDate.Text) ? (object)DBNull.Value : DateTime.Parse(txt_ContractStartDate.Text));
        cmd.Parameters.AddWithValue("@ContractEndDate",   string.IsNullOrEmpty(txt_ContractEndDate.Text)   ? (object)DBNull.Value : DateTime.Parse(txt_ContractEndDate.Text));
        cmd.Parameters.AddWithValue("@ContractType",      ddl_ContractType.SelectedValue);
        cmd.Parameters.AddWithValue("@RenewalDate",       string.IsNullOrEmpty(txt_RenewalDate.Text)       ? (object)DBNull.Value : DateTime.Parse(txt_RenewalDate.Text));
        cmd.Parameters.AddWithValue("@NoticePeriod",      txt_NoticePeriod.Text.Trim());
        cmd.Parameters.AddWithValue("@ContractStatus",    ddl_ContractStatus.SelectedValue);
        cmd.Parameters.AddWithValue("@SLADetails",        txt_SLADetails.Text.Trim());
    }
    private string SaveUploadedFile(FileUpload fu, string prefix)
    {
        if (!fu.HasFile) return string.Empty;

        string folderPath  = Server.MapPath(UploadFolder);
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string ext       = Path.GetExtension(fu.FileName);
        string fileName = string.Format(
            "{0}_{1}{2}",
            prefix,
            DateTime.Now.ToString("yyyyMMddHHmmss"),
            ext
        );

        string fullPath  = Path.Combine(folderPath, fileName);
        fu.SaveAs(fullPath);

        // Return relative path for DB storage
        return string.Format("~/Uploads/ClientDocs/{0}", fileName);
    }

    private void SetViewLink(HyperLink hl, string path)
    {
        if (!string.IsNullOrEmpty(path))
        {
            hl.NavigateUrl = ResolveUrl(path);
            hl.Visible = true;
        }
        else
        {
            hl.Visible = false;
        }
    }

    private void ClearForm()
    {
        // Reserved for future use
    }
}
