using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.UI.WebControls;

public partial class Admin_Clientsdetails : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;

    private string key = "";
    string str_id = "";

    // Folder where uploaded documents will be saved (relative to site root)
    private readonly string UploadFolder = "~/Uploads/ClientDocs/";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();

        Label control1 = this.Master.FindControl("lbl_bread") as Label;
        if (control1 != null)
            control1.Text = "Organization";

        if (!string.IsNullOrEmpty(Request.QueryString["id"]))
        {
            this.str_id = Request.QueryString["id"].ToString();
            if (!IsPostBack)
            {
                Loadcountry();
                LoadEmp();
                Loadsales();
                LoadPartyType();
                LoadTaxType();
                assignvalues();
            }

            SqlCommand cmdRole = new SqlCommand("SELECT role FROM IT_EmployeeRegister WHERE Employeekey = @Employeekey AND role = 11");
            cmdRole.Parameters.AddWithValue("@Employeekey", SC.Userid);
            bool isRole11 = DA.GetDataTable(cmdRole).Rows.Count > 0;
            btn_update.Visible = isRole11;
            btn_request.Visible = false;
        }
        else
        {
            if (!IsPostBack)
            {
                Loadcountry();
                LoadEmp();
                Loadsales();
                LoadPartyType();
                LoadTaxType();
            }

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

        // Handle file uploads
        string contractCopyPath = SaveUploadedFile(fu_ContractCopy, "ContractCopy");
        string ndaPath          = SaveUploadedFile(fu_NDA, "NDA");
        string sowPath          = SaveUploadedFile(fu_SOW, "SOW");
        string otherDocsPath    = SaveUploadedFile(fu_OtherDocs, "OtherDocs");

        SqlCommand cmd = new SqlCommand(@"
            INSERT INTO IT_ClientDetails 
            (
                ClientCode, ClientName, CompanyName, ContactPerson, Designation,
                Email, AlternateEmail, Mobile, AlternateMobile, PartyType, Industry,
                Website, AddressLine1, AddressLine2, Country, Source, Status,
                Description, OnboardBy, CreatedBy, TaxType, SalesPerson,
                -- Bank Details
                BankName, AccountHolderName, AccountNumber, IFSCCode, Branch, BankAddress,
                -- Contact Details
                ContactName, ContactTitle, Department, Telephone, ContactMobile, ContactEmail,
                -- Account Manager
                AccMgrName, AccMgrEmail, AccMgrMobile, AssignedDate, LastFollowUpDate,
                -- Documents
                ContractCopyPath, NDAPath, SOWPath, OtherDocsPath,
                -- Contract Information
                ContractNumber, ContractStartDate, ContractEndDate, ContractType,
                RenewalDate, NoticePeriod, ContractStatus, SLADetails
            )
            VALUES
            (
                @ClientCode, @ClientName, @CompanyName, @ContactPerson, @Designation,
                @Email, @AlternateEmail, @Mobile, @AlternateMobile, @PartyType, @Industry,
                @Website, @AddressLine1, @AddressLine2, @Country, @Source, @Status,
                @Description, @OnboardBy, @CreatedBy, @TaxType, @SalesPerson,
                -- Bank Details
                @BankName, @AccountHolderName, @AccountNumber, @IFSCCode, @Branch, @BankAddress,
                -- Contact Details
                @ContactName, @ContactTitle, @Department, @Telephone, @ContactMobile, @ContactEmail,
                -- Account Manager
                @AccMgrName, @AccMgrEmail, @AccMgrMobile, @AssignedDate, @LastFollowUpDate,
                -- Documents
                @ContractCopyPath, @NDAPath, @SOWPath, @OtherDocsPath,
                -- Contract Information
                @ContractNumber, @ContractStartDate, @ContractEndDate, @ContractType,
                @RenewalDate, @NoticePeriod, @ContractStatus, @SLADetails
            )");

        AddBasicParams(cmd, userId);
        AddBankParams(cmd);
        AddContactParams(cmd);
        AddAccMgrParams(cmd);
        AddDocumentParams(cmd, contractCopyPath, ndaPath, sowPath, otherDocsPath);
        AddContractParams(cmd);

        DA.ExecuteNonQuery(cmd);
        Response.Redirect("Clients.aspx");
    }
    protected void btn_update_Click(object sender, EventArgs e)
    {
        Guid userId = new Guid(SC.Userid.ToString());

        // Use existing path if no new file uploaded, else save new file
        string contractCopyPath = fu_ContractCopy.HasFile  ? SaveUploadedFile(fu_ContractCopy, "ContractCopy") : hf_ContractCopyPath.Value;
        string ndaPath          = fu_NDA.HasFile           ? SaveUploadedFile(fu_NDA, "NDA")                   : hf_NDAPath.Value;
        string sowPath          = fu_SOW.HasFile           ? SaveUploadedFile(fu_SOW, "SOW")                   : hf_SOWPath.Value;
        string otherDocsPath    = fu_OtherDocs.HasFile     ? SaveUploadedFile(fu_OtherDocs, "OtherDocs")       : hf_OtherDocsPath.Value;

        SqlCommand cmd = new SqlCommand(@"
            UPDATE IT_ClientDetails SET
                ClientCode=@ClientCode, ClientName=@ClientName, CompanyName=@CompanyName,
                ContactPerson=@ContactPerson, Designation=@Designation, Email=@Email,
                AlternateEmail=@AlternateEmail, Mobile=@Mobile, AlternateMobile=@AlternateMobile,
                PartyType=@PartyType, Industry=@Industry, Website=@Website,
                AddressLine1=@AddressLine1, AddressLine2=@AddressLine2, Country=@Country,
                Source=@Source, Status=@Status, Description=@Description,
                OnboardBy=@OnboardBy, ModifiedBy=@ModifiedBy, ModifiedOn=GETDATE(), TaxType=@TaxType, SalesPerson=@SalesPerson,
                -- Bank Details
                BankName=@BankName, AccountHolderName=@AccountHolderName, AccountNumber=@AccountNumber,
                IFSCCode=@IFSCCode, Branch=@Branch, BankAddress=@BankAddress,
                -- Contact Details
                ContactName=@ContactName, ContactTitle=@ContactTitle, Department=@Department,
                Telephone=@Telephone, ContactMobile=@ContactMobile, ContactEmail=@ContactEmail,
                -- Account Manager
                AccMgrName=@AccMgrName, AccMgrEmail=@AccMgrEmail, AccMgrMobile=@AccMgrMobile,
                AssignedDate=@AssignedDate, LastFollowUpDate=@LastFollowUpDate,
                -- Documents
                ContractCopyPath=@ContractCopyPath, NDAPath=@NDAPath,
                SOWPath=@SOWPath, OtherDocsPath=@OtherDocsPath,
                -- Contract Information
                ContractNumber=@ContractNumber, ContractStartDate=@ContractStartDate,
                ContractEndDate=@ContractEndDate, ContractType=@ContractType,
                RenewalDate=@RenewalDate, NoticePeriod=@NoticePeriod,
                ContractStatus=@ContractStatus, SLADetails=@SLADetails
            WHERE ClientKey=@ClientKey");

        AddBasicParams(cmd, userId, isUpdate: true);
        AddBankParams(cmd);
        AddContactParams(cmd);
        AddAccMgrParams(cmd);
        AddDocumentParams(cmd, contractCopyPath, ndaPath, sowPath, otherDocsPath);
        AddContractParams(cmd);
        cmd.Parameters.Add("@ClientKey", SqlDbType.UniqueIdentifier).Value = new Guid(this.str_id);

        DA.ExecuteNonQuery(cmd);
        Response.Redirect("Clients.aspx");
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

            // Documents – store paths in hidden fields and show View links
            hf_ContractCopyPath.Value   = r["ContractCopyPath"].ToString();
            hf_NDAPath.Value            = r["NDAPath"].ToString();
            hf_SOWPath.Value            = r["SOWPath"].ToString();
            hf_OtherDocsPath.Value      = r["OtherDocsPath"].ToString();

            SetViewLink(hl_ContractCopy, r["ContractCopyPath"].ToString());
            SetViewLink(hl_NDA, r["NDAPath"].ToString());
            SetViewLink(hl_SOW, r["SOWPath"].ToString());
            SetViewLink(hl_OtherDocs, r["OtherDocsPath"].ToString());

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
    protected void lnk_RemoveContractCopy_Click(object sender, EventArgs e)  { RemoveDocument("ContractCopyPath", hf_ContractCopyPath, hl_ContractCopy); }
    protected void lnk_RemoveNDA_Click(object sender, EventArgs e)            { RemoveDocument("NDAPath", hf_NDAPath, hl_NDA); }
    protected void lnk_RemoveSOW_Click(object sender, EventArgs e)            { RemoveDocument("SOWPath", hf_SOWPath, hl_SOW); }
    protected void lnk_RemoveOtherDocs_Click(object sender, EventArgs e)      { RemoveDocument("OtherDocsPath", hf_OtherDocsPath, hl_OtherDocs); }

    private void RemoveDocument(string columnName, HiddenField hf, HyperLink hl)
    {
        if (!string.IsNullOrEmpty(this.str_id) && !string.IsNullOrEmpty(hf.Value))
        {
            // Delete physical file
            string physicalPath = Server.MapPath(hf.Value);
            if (File.Exists(physicalPath))
                File.Delete(physicalPath);

            // Clear DB column
            SqlCommand cmd = new SqlCommand(
                "UPDATE IT_ClientDetails SET " + columnName + " = NULL WHERE ClientKey = @ClientKey"
            );
            cmd.Parameters.Add("@ClientKey", SqlDbType.UniqueIdentifier).Value = new Guid(this.str_id);
            DA.ExecuteNonQuery(cmd);
        }

        hf.Value = "";
        hl.Visible = false;
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
        cmd.Parameters.Add("@OnboardBy", SqlDbType.UniqueIdentifier).Value = new Guid(ddl_OnboardBy.SelectedValue);
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

    private void AddDocumentParams(SqlCommand cmd, string contractCopyPath, string ndaPath, string sowPath, string otherDocsPath)
    {
        cmd.Parameters.AddWithValue("@ContractCopyPath", string.IsNullOrEmpty(contractCopyPath) ? (object)DBNull.Value : contractCopyPath);
        cmd.Parameters.AddWithValue("@NDAPath",          string.IsNullOrEmpty(ndaPath)          ? (object)DBNull.Value : ndaPath);
        cmd.Parameters.AddWithValue("@SOWPath",          string.IsNullOrEmpty(sowPath)          ? (object)DBNull.Value : sowPath);
        cmd.Parameters.AddWithValue("@OtherDocsPath",    string.IsNullOrEmpty(otherDocsPath)    ? (object)DBNull.Value : otherDocsPath);
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
