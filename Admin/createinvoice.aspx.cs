using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;


public partial class Admin_createinvoice : System.Web.UI.Page, ICallbackEventHandler
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;

    string Id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();

        // Register Callback for real-time validation
        String cbReference = Page.ClientScript.GetCallbackEventReference(this, "arg", "ReceiveServerData", "context");
        String callbackScript = "function CallServer(arg, context) {" + cbReference + "; }";
        Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "CallServer", callbackScript, true);

        if (!IsPostBack)
        {
            // Disable browser caching for these controls so that a soft refresh clears them
            ddlClient.Attributes.Add("autocomplete", "off");
            ddProjectName.Attributes.Add("autocomplete", "off");
            InvoiceNumber.Attributes.Add("autocomplete", "off");

            try { DA.ExecuteNonQuery(new SqlCommand("IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[IT_Invoices]') AND name = 'TdsPercentage') BEGIN ALTER TABLE IT_Invoices ADD TdsPercentage DECIMAL(18,2) NULL END")); } catch {}
            BindClients();
            BindStatus();
            SGSTViews.Visible = false;
            CGSTViews.Visible = false;
            IGSTViews.Visible = false;
            ReceivedViews.Visible = false;
            ReceiveddateViews.Visible = false;
            BalanceViews.Visible = false;
            TDSViews.Visible = true;
            Label lblBread = Master.FindControl("lbl_bread") as Label;
            if (lblBread != null)
            {
                lblBread.Text = "Receivable Invoice";
            }

            if (Request.QueryString["id"] != null)
            {
                // UPDATE MODE
                lgTitle.InnerHtml ="<i class='icon-reading position-left'></i> Update Receivable Invoice";
                btnSave.Visible = false;
                btnUpdate.Visible = true;

                int invoiceId = Convert.ToInt32(Request.QueryString["id"]);
                hfInvoiceKey.Value = invoiceId.ToString();
                ReceivedViews.Visible = true;
                ReceiveddateViews.Visible = true;
                TDSViews.Visible = true;
                PopulateProjectData(invoiceId);
                Invoicegrid();
                BindLastInvoiceNumber();
            }
            else
            {
                // CREATE MODE
                lgTitle.InnerHtml =
                           "<i class='icon-reading position-left'></i> Create Receivable Invoice";
                btnSave.Visible = true;
                btnUpdate.Visible = false;
                BindLastInvoiceNumber();
            }
        }
    }
    private void Invoicegrid()
    {
        string clientId = ddlClient.SelectedValue;
        string projectId = ddProjectName.SelectedValue;

        if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(projectId))
        {
            divGrid.Visible = false;
            return;
        }

        DateTime currentDate = DateTime.Now;
        DateTime fyStart, fyEnd;
        if (currentDate.Month >= 4)
        {
            fyStart = new DateTime(currentDate.Year, 4, 1);
            fyEnd = new DateTime(currentDate.Year + 1, 3, 31, 23, 59, 59);
        }
        else
        {
            fyStart = new DateTime(currentDate.Year - 1, 4, 1);
            fyEnd = new DateTime(currentDate.Year, 3, 31, 23, 59, 59);
        }

        string str_view = @"SELECT b.ProjectName As ProjectName, a.InvoiceKey, a.InvoiceNumber, a.TotalAmount, CONVERT(VARCHAR(10), a.InvoiceDate, 105) AS InvoiceDate, CONVERT(VARCHAR(10), a.ReceivedDate, 105) AS ReceivedDate, CONVERT(VARCHAR(10), a.CreatedOn, 105) AS CreatedOn FROM IT_Invoices a left join IT_Projects b on a.ProjectKey = b.ProjectKey where a.ClientKey=@ClientKey and a.ProjectKey=@ProjectKey AND ISNULL(a.InvoiceDate, a.CreatedOn) >= @FYStart AND ISNULL(a.InvoiceDate, a.CreatedOn) <= @FYEnd";

        SqlCommand cmd = new SqlCommand(str_view);
        cmd.Parameters.AddWithValue("@ClientKey", clientId);
        cmd.Parameters.AddWithValue("@ProjectKey", projectId);
        cmd.Parameters.AddWithValue("@FYStart", fyStart);
        cmd.Parameters.AddWithValue("@FYEnd", fyEnd);
        DataTable dt = DA.GetDataTable(cmd);
        PH_invoice.Controls.Clear();

        if (dt.Rows.Count > 0)
        {
            DataSet ds = new DataSet();
            ds.Merge(dt);
            this.PH.LoadGridItem(ds, PH_invoice, "Invoicelist.txt", "");
            divGrid.Visible = true;
        }
        else
        {
            divGrid.Visible = false;
        }

    }
    private void PopulateProjectData(int InvoiceId)
    {
        string query = @" SELECT InvoiceKey, ClientKey, ProjectKey, InvoiceNumber, Currency, InvoiceAmount, SGSTAmount, CGSTAmount, IGSTAmount, TotalAmount, TDSAmount, TdsPercentage, GstPercentage, conversionAmount, InvoiceStatus, Notes, CONVERT(varchar(10), InvoiceDate, 103)   AS InvoiceDate, CONVERT(varchar(10), ReceivedOn, 103)    AS ReceivedOn, CONVERT(varchar(10), ReceivedDate, 103)  AS ReceivedDate FROM IT_Invoices WHERE InvoiceKey = @InvoiceKey";
        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@InvoiceKey", InvoiceId);
        DataSet ds = this.DA.GetDataSet(cmd);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            string clientId = row["ClientKey"].ToString();
            
            if (ddlClient.Items.FindByValue(clientId) != null)
                ddlClient.SelectedValue = clientId;
            else
            {
                SqlCommand cmdClient = new SqlCommand("SELECT CompanyName FROM IT_ClientDetails WHERE ClientKey=@ClientKey");
                cmdClient.Parameters.AddWithValue("@ClientKey", clientId);
                DataSet dsClient = this.DA.GetDataSet(cmdClient);
                if (dsClient != null && dsClient.Tables.Count > 0 && dsClient.Tables[0].Rows.Count > 0)
                {
                    string cName = dsClient.Tables[0].Rows[0]["CompanyName"].ToString();
                    if (!string.IsNullOrEmpty(cName))
                    {
                        ddlClient.Items.Add(new ListItem(cName, clientId));
                        ddlClient.SelectedValue = clientId;
                    }
                }
            }

            // txtProjectName.Text = row["InvoiceNumber"].ToString();
            IT_InvoiceDate.Text = row["InvoiceDate"].ToString();
            IT_ReceivedDate.Text = row["ReceivedOn"].ToString();
            Receiveddate.Text = row["ReceivedDate"].ToString();
            BindCurrency(clientId);
            GSTViews(clientId);

            if (DD_Currency.Items.FindByValue(row["Currency"].ToString()) != null)
                DD_Currency.SelectedValue = row["Currency"].ToString();
            SubTotal.Text = row["InvoiceAmount"].ToString();
            SGSTAmount.Text = row["SGSTAmount"].ToString();
            CGSTAmount.Text = row["CGSTAmount"].ToString();
            IGSTAmount.Text = row["IGSTAmount"].ToString();
            TotalAmount.Text = row["TotalAmount"].ToString();
            string Project_Key = row["ProjectKey"].ToString();
            if (ddProjectName.Items.FindByValue(Project_Key) != null)
                ddProjectName.SelectedValue = Project_Key;
            else
            {
                SqlCommand cmdProject = new SqlCommand("SELECT ProjectName FROM IT_Projects WHERE ProjectKey=@ProjectKey");
                cmdProject.Parameters.AddWithValue("@ProjectKey", Project_Key);
                DataSet dsProject = this.DA.GetDataSet(cmdProject);
                if (dsProject != null && dsProject.Tables.Count > 0 && dsProject.Tables[0].Rows.Count > 0)
                {
                    string pName = dsProject.Tables[0].Rows[0]["ProjectName"].ToString();
                    if (!string.IsNullOrEmpty(pName))
                    {
                        ddProjectName.Items.Add(new ListItem(pName, Project_Key));
                        ddProjectName.SelectedValue = Project_Key;
                    }
                }
            }
            
            InvoiceNumber.Text = row["InvoiceNumber"].ToString();
            TdsAmount.Text = row["TDSAmount"].ToString();
            if (ds.Tables[0].Columns.Contains("TdsPercentage") && row["TdsPercentage"] != DBNull.Value)
                TdsPercentage.Text = row["TdsPercentage"].ToString();
            if (ds.Tables[0].Columns.Contains("GstPercentage") && row["GstPercentage"] != DBNull.Value)
                GstPercentage.Text = row["GstPercentage"].ToString();
            ReceivedAmount.Text = row["conversionAmount"].ToString();
            
            if (ds.Tables[0].Columns.Contains("InvoiceStatus") && row["InvoiceStatus"] != DBNull.Value)
            {
                string iStatus = row["InvoiceStatus"].ToString();
                if (DD_Status.Items.FindByValue(iStatus) != null) DD_Status.SelectedValue = iStatus;
            }
            if (ds.Tables[0].Columns.Contains("Notes") && row["Notes"] != DBNull.Value)
                txtNotes.Text = row["Notes"].ToString();

            BindBalnce(Project_Key);
        }


        // Populate Participants
        BindInvoiceDescriptionRows(InvoiceId);


    }

    private void BindInvoiceDescriptionRows(int invoiceKey)
    {
        string query = @"SELECT Description, Amount 
                     FROM IT_InvoiceDescription 
                     WHERE InvoiceKey = @InvoiceKey";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@InvoiceKey", invoiceKey);

        DataSet ds = this.DA.GetDataSet(cmd);

        if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
        {
            // At least one empty row
            tblBody.InnerHtml = @"
        <tr>
            <td><textarea name='txtName' class='form-control txtName' rows='2' style='resize:vertical;'></textarea></td>
            <td><input type='number' name='txtAmount' class='form-control txtAmount' /></td>
            <td><button type='button' class='btn btn-danger btn-sm removeRow'>Remove</button></td>
        </tr>";
            return;
        }

        StringBuilder sb = new StringBuilder();

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            sb.Append(@"
        <tr>
            <td>
                <textarea name='txtName' class='form-control txtName' rows='2' style='resize:vertical;'>" + dr["Description"].ToString() + @"</textarea>
            </td>
            <td>
                <input type='number' name='txtAmount' class='form-control txtAmount'
                       value='" + dr["Amount"].ToString() + @"' />
            </td>
            <td>
                <button type='button' class='btn btn-danger btn-sm removeRow'>Remove</button>
            </td>
        </tr>");
        }

        tblBody.InnerHtml = sb.ToString();
    }

    private void GSTViews(string InvoiceId) {
        string sql = @"SELECT TaxType
                       FROM IT_ClientDetails
                       WHERE ClientKey=@ClientKey";

        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.AddWithValue("@ClientKey", InvoiceId);

        DataSet ds = DA.GetDataSet(cmd);

        if (ds.Tables[0].Rows.Count > 0)
        {
            string taxType = ds.Tables[0].Rows[0]["TaxType"] != DBNull.Value ? ds.Tables[0].Rows[0]["TaxType"].ToString() : "";

            hfGstStateCode.Value = taxType;

            if (taxType == "1") // Same State
            {
                SGSTViews.Visible = true;
                CGSTViews.Visible = true;
                IGSTViews.Visible = false;
                GSTPercentViews.Visible = true;
                TDSPercentViews.Visible = true;
                TDSViews.Visible = true;
            }
            else if (taxType == "2") // Other State
            {
                IGSTViews.Visible = true;
                SGSTViews.Visible = false;
                CGSTViews.Visible = false;
                GSTPercentViews.Visible = true;
                TDSPercentViews.Visible = true;
                TDSViews.Visible = true;
            }
            else // Overseas or None
            {
                SGSTViews.Visible = false;
                CGSTViews.Visible = false;
                IGSTViews.Visible = false;
                GSTPercentViews.Visible = false;
                TDSPercentViews.Visible = false;
                TDSViews.Visible = false;
            }
        }
    }
    protected void btn_send_Click(object sender, EventArgs e)
    {
        if (CheckDuplicateInvoiceServer(InvoiceNumber.Text.Trim(), ""))
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "duplicate", "showToastr('error','This invoice number already exists');", true);
            return;
        }

        if (!string.IsNullOrEmpty(IT_InvoiceDate.Text) &&
               !string.IsNullOrEmpty(IT_ReceivedDate.Text))
        {
            DateTime startDate, endDate;

            bool isStartValid = DateTime.TryParseExact(
                IT_InvoiceDate.Text.Trim(),
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out startDate
            );

            bool isEndValid = DateTime.TryParseExact(IT_ReceivedDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate
            );


            if (endDate <= startDate)
            {
                ScriptManager.RegisterStartupScript(
                    this, this.GetType(), "dateerror",
                    "showToastr('error','Due Date must be greater than Invoice Date');",
                    true);
                return;
            }
        }

        string subtotal = hdnSubTotal.Value;
        string SGST = hdnSGST.Value;
        string CGST = hdnCGST.Value;
        string IGST = hdnIGST.Value;
        string TAmount = hdnTotal.Value;
        try
        {
            string sql = @"
        INSERT INTO IT_Invoices
        (
            ClientKey, InvoiceNumber, InvoiceAmount, GSTAmount,TotalAmount,
            Currency, InvoiceDate, ReceivedOn, Status,
            CreatedBy,SGSTAmount,CGSTAmount,IGSTAmount,GSTstatus,ReceivedDate,ProjectKey,conversionAmount,TDSAmount,TdsPercentage,GstPercentage,InvoiceStatus,Notes
        )
        VALUES
        (
            @ClientKey, @InvoiceNumber, @InvoiceAmount, @GSTAmount,@TotalAmount,
            @Currency, @InvoiceDate, @ReceivedOn, @Status,
             @CreatedBy,@SGSTAmount,@CGSTAmount,@IGSTAmount,@GSTstatus,@ReceivedDate,@ProjectKey,@conversionAmount,@TDSAmount,@TdsPercentage,@GstPercentage,@InvoiceStatus,@Notes
        );
        SELECT SCOPE_IDENTITY();";

            SqlCommand cmd = new SqlCommand(sql);

            cmd.Parameters.AddWithValue("@ClientKey", ddlClient.SelectedValue);
            cmd.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber.Text);
            cmd.Parameters.AddWithValue("@InvoiceAmount", subtotal);
            cmd.Parameters.AddWithValue("@GSTAmount",
                Convert.ToDecimal(SGST) +
                Convert.ToDecimal(CGST) +
                Convert.ToDecimal(IGST));
            cmd.Parameters.AddWithValue("@SGSTAmount", SGST);
            cmd.Parameters.AddWithValue("@CGSTAmount", CGST);
            cmd.Parameters.AddWithValue("@IGSTAmount", IGST);

            cmd.Parameters.AddWithValue("@TotalAmount", TAmount);
            cmd.Parameters.AddWithValue("@Currency", DD_Currency.SelectedValue);
            cmd.Parameters.AddWithValue("@ProjectKey", ddProjectName.SelectedValue);
            cmd.Parameters.AddWithValue("@InvoiceDate", string.IsNullOrEmpty(IT_InvoiceDate.Text) ? (object)DBNull.Value : DateTime.ParseExact(IT_InvoiceDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture));
            cmd.Parameters.AddWithValue("@ReceivedOn", string.IsNullOrEmpty(IT_ReceivedDate.Text) ? (object)DBNull.Value : DateTime.ParseExact(IT_ReceivedDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture));


            if (string.IsNullOrWhiteSpace(Receiveddate.Text))
            {
                cmd.Parameters.AddWithValue("@Status", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Status", 1);
            }

            
            cmd.Parameters.AddWithValue("@GSTstatus", 0);
            if (string.IsNullOrWhiteSpace(ReceivedAmount.Text))
            {
                cmd.Parameters.AddWithValue("@conversionAmount", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue(
                    "@conversionAmount",
                    Convert.ToDecimal(ReceivedAmount.Text)
                );
            }
            

            if (string.IsNullOrWhiteSpace(Receiveddate.Text))
            {
                cmd.Parameters.AddWithValue("@ReceivedDate", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("@ReceivedDate", DateTime.ParseExact(Receiveddate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture));
            }

            if (string.IsNullOrWhiteSpace(TdsAmount.Text))
            {
                cmd.Parameters.AddWithValue("@TDSAmount", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue(
                    "@TDSAmount",
                    Convert.ToDecimal(TdsAmount.Text)
                );
            }

            cmd.Parameters.AddWithValue("@TdsPercentage", string.IsNullOrWhiteSpace(TdsPercentage.Text) ? (object)DBNull.Value : Convert.ToDecimal(TdsPercentage.Text));
            cmd.Parameters.AddWithValue("@GstPercentage", string.IsNullOrWhiteSpace(GstPercentage.Text) ? (object)DBNull.Value : Convert.ToDecimal(GstPercentage.Text));

            cmd.Parameters.AddWithValue("@InvoiceStatus", DD_Status.SelectedValue);
            cmd.Parameters.AddWithValue("@Notes", txtNotes.Text);

            cmd.Parameters.AddWithValue("@CreatedBy", SC.Userid);


            this.DA.ExecuteNonQuery(cmd);

            // Get last inserted ProjectKey
            string getProjectKey = "SELECT MAX(InvoiceKey) FROM IT_Invoices";
            SqlCommand cmdGetKey = new SqlCommand(getProjectKey);
            DataSet ds = this.DA.GetDataSet(cmdGetKey);
            int projectKey = Convert.ToInt32(ds.Tables[0].Rows[0][0]);



            // INSERT DESCRIPTION

            // 🔹 INSERT DESCRIPTION ROWS
            InsertInvoiceDescriptions(projectKey);

            //  pdfgenerator(projectKey);

            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "toastr_redirect",
                "showToastr('success','Receivable InvoiceGrid saved successfully!');" +
                "setTimeout(function(){ window.location.href = '/Admin/ReceivableInvoiceGrid.aspx'; }, 2000);",
                true
            );

        }
        catch (Exception ex)
        {
            throw ex;
        }


    }


    protected void Btn_update_Click(object sender, EventArgs e)
    {
        try
        {
            if (CheckDuplicateInvoiceServer(InvoiceNumber.Text.Trim(), hfInvoiceKey.Value))
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "duplicate", "showToastr('error','This invoice number already exists');", true);
                return;
            }

            if (!string.IsNullOrEmpty(IT_InvoiceDate.Text) &&
            !string.IsNullOrEmpty(IT_ReceivedDate.Text))
            {
                DateTime startDate, endDate;

                bool isStartValid = DateTime.TryParseExact(
                    IT_InvoiceDate.Text.Trim(),
                    "dd/MM/yyyy",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out startDate
                );

                bool isEndValid = DateTime.TryParseExact(IT_ReceivedDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate
                );


                if (endDate <= startDate)
                {
                    ScriptManager.RegisterStartupScript(
                        this, this.GetType(), "dateerror",
                        "showToastr('error','Due Date must be greater than Invoice Date');",
                        true);
                    return;
                }
            }

            string subtotal = hdnSubTotal.Value;
            if (subtotal=="")
            {
                subtotal = SubTotal.Text;
            }
            string SGST = hdnSGST.Value;
            if (SGST=="")
            {
                SGST = SGSTAmount.Text;
            }
            string CGST = hdnCGST.Value;
            if (CGST=="")
            {
                CGST = CGSTAmount.Text;
            }
            string IGST = hdnIGST.Value;
            if (IGST=="")
            {
                IGST = IGSTAmount.Text;
            }
            
            
            string TAmount = hdnTotal.Value;

            if (TAmount=="")
            {
                TAmount = TotalAmount.Text;
            }

            int invoiceKey = int.Parse(hfInvoiceKey.Value);



            string sql = @"
        UPDATE IT_Invoices
SET
    ClientKey        = @ClientKey,
    InvoiceNumber    = @InvoiceNumber,
    InvoiceAmount    = @InvoiceAmount,
    GSTAmount        = @GSTAmount,
    TotalAmount      = @TotalAmount,
    Currency         = @Currency,
    InvoiceDate      = @InvoiceDate,
    ReceivedOn       = @ReceivedOn,
    Status           = @Status,
    SGSTAmount       = @SGSTAmount,
    CGSTAmount       = @CGSTAmount,
    IGSTAmount       = @IGSTAmount,
    ReceivedDate     = @ReceivedDate,
    ProjectKey       = @ProjectKey,
    conversionAmount = @conversionAmount,
    TDSAmount        = @TDSAmount,
    TdsPercentage    = @TdsPercentage,
    GstPercentage    = @GstPercentage,
    InvoiceStatus    = @InvoiceStatus,
    Notes            = @Notes,
    ModifiedOn       = GETDATE(),
    ModifiedBy       = @ModifiedBy
WHERE
    InvoiceKey = @InvoiceKey;
";

            SqlCommand cmd = new SqlCommand(sql);

            cmd.Parameters.AddWithValue("@InvoiceKey", invoiceKey);
            cmd.Parameters.AddWithValue("@ClientKey", ddlClient.SelectedValue);
            cmd.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber.Text);
            cmd.Parameters.AddWithValue("@InvoiceAmount", subtotal);
            cmd.Parameters.AddWithValue("@GSTAmount",
                Convert.ToDecimal(SGST) +
                Convert.ToDecimal(CGST) +
                Convert.ToDecimal(IGST));
            cmd.Parameters.AddWithValue("@SGSTAmount", SGST);
            cmd.Parameters.AddWithValue("@CGSTAmount", CGST);
            cmd.Parameters.AddWithValue("@IGSTAmount", IGST);

            cmd.Parameters.AddWithValue("@TotalAmount", TAmount);
            cmd.Parameters.AddWithValue("@Currency", DD_Currency.SelectedValue);
            cmd.Parameters.AddWithValue("@ProjectKey", ddProjectName.SelectedValue);
            cmd.Parameters.AddWithValue("@InvoiceDate", string.IsNullOrEmpty(IT_InvoiceDate.Text) ? (object)DBNull.Value : DateTime.ParseExact(IT_InvoiceDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture));
            cmd.Parameters.AddWithValue("@ReceivedOn", string.IsNullOrEmpty(IT_ReceivedDate.Text) ? (object)DBNull.Value : DateTime.ParseExact(IT_ReceivedDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture));

            if (string.IsNullOrWhiteSpace(Receiveddate.Text))
            {
                cmd.Parameters.AddWithValue("@Status", 0);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Status", 1);
            }


            if (string.IsNullOrWhiteSpace(ReceivedAmount.Text))
            {
                cmd.Parameters.AddWithValue("@conversionAmount", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue(
                    "@conversionAmount",
                    Convert.ToDecimal(ReceivedAmount.Text)
                );
            }
             cmd.Parameters.AddWithValue("@ReceivedDate", string.IsNullOrEmpty(Receiveddate.Text) ? (object)DBNull.Value : DateTime.ParseExact(Receiveddate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture));
          

            if (string.IsNullOrWhiteSpace(TdsAmount.Text))
            {
                cmd.Parameters.AddWithValue("@TDSAmount", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue(
                    "@TDSAmount",
                    Convert.ToDecimal(TdsAmount.Text)
                );
            }
            
            cmd.Parameters.AddWithValue("@TdsPercentage", string.IsNullOrWhiteSpace(TdsPercentage.Text) ? (object)DBNull.Value : Convert.ToDecimal(TdsPercentage.Text));
            cmd.Parameters.AddWithValue("@GstPercentage", string.IsNullOrWhiteSpace(GstPercentage.Text) ? (object)DBNull.Value : Convert.ToDecimal(GstPercentage.Text));

            cmd.Parameters.AddWithValue("@InvoiceStatus", DD_Status.SelectedValue);
            cmd.Parameters.AddWithValue("@Notes", txtNotes.Text);

            cmd.Parameters.AddWithValue("@ModifiedBy", SC.Userid);



            this.DA.ExecuteNonQuery(cmd);

            // 🔹 DELETE OLD DESCRIPTION
            SqlCommand del = new SqlCommand(
                "DELETE FROM IT_InvoiceDescription WHERE InvoiceKey=@InvoiceKey");
            del.Parameters.AddWithValue("@InvoiceKey", invoiceKey);
            this.DA.ExecuteNonQuery(del);

            // 🔹 INSERT NEW DESCRIPTION
            InsertInvoiceDescriptions(invoiceKey);

            ScriptManager.RegisterStartupScript(
               this,
               this.GetType(),
               "toastr_redirect",
               "showToastr('success','Receivable InvoiceGrid Update successfully!');" +
               "setTimeout(function(){ window.location.href = '/Admin/ReceivableInvoiceGrid.aspx'; }, 2000);",
               true
           );


        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void InsertInvoiceDescriptions(int invoiceKey)
    {
        string[] descriptions = Request.Form.GetValues("txtName");
        string[] amounts = Request.Form.GetValues("txtAmount");

        if (descriptions == null || amounts == null) return;

        for (int i = 0; i < descriptions.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(descriptions[i])) continue;

            SqlCommand cmd = new SqlCommand(@"
        INSERT INTO IT_InvoiceDescription
        (InvoiceKey, Description, Amount)
        VALUES
        (@InvoiceKey, @Description, @Amount)");

            cmd.Parameters.AddWithValue("@InvoiceKey", invoiceKey);
            cmd.Parameters.AddWithValue("@Description", descriptions[i]);
            cmd.Parameters.AddWithValue("@Amount", amounts[i]);

            this.DA.ExecuteNonQuery(cmd);
        }
    }
    private void BindClients()
    {
        string str_clients = "SELECT ClientKey, CompanyName FROM IT_ClientDetails WHERE Status='1' AND PartyType='2' ORDER BY CompanyName";
        SqlCommand cmd = new SqlCommand(str_clients);
        DataSet ds = this.DA.GetDataSet(cmd);

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlClient.DataSource = ds.Tables[0];
            ddlClient.DataTextField = "CompanyName";
            ddlClient.DataValueField = "ClientKey";
            ddlClient.DataBind();
            ddlClient.Items.Insert(0, new ListItem("-- Select Client --", "")); // matches validator InitialValue
        }
        else
        {
            ddlClient.Items.Clear();
            ddlClient.DataSource = null;
            ddlClient.DataBind();

            ddlClient.Items.Insert(0, new ListItem("-- No Client Found --", ""));
        }




    }
    private void BindCurrency(string client_Id)
    {
        if (string.IsNullOrEmpty(client_Id))
        {
            SGSTViews.Visible = false;
            CGSTViews.Visible = false;
            IGSTViews.Visible = false;
            ddProjectName.Items.Clear();
            ddProjectName.Items.Insert(0, new ListItem("-- No Projects Found --", ""));
            return;
        }

        GSTViews(client_Id);


        string str_project = "select ProjectName,ProjectKey from IT_Projects where ClientKey='" + client_Id + "'";
        SqlCommand cmdproject = new SqlCommand(str_project);
        DataSet ds_project = this.DA.GetDataSet(cmdproject);

        if (ds_project != null && ds_project.Tables.Count > 0 && ds_project.Tables[0].Rows.Count > 0)
        {
            ddProjectName.DataSource = ds_project.Tables[0];
            ddProjectName.DataTextField = "ProjectName";
            ddProjectName.DataValueField = "ProjectKey";
            ddProjectName.DataBind();
            ddProjectName.Items.Insert(0, new ListItem("-- Select Projects --", "")); // matches validator InitialValue
        }
        else
        {
            ddProjectName.Items.Clear();
            ddProjectName.DataSource = null;
            ddProjectName.DataBind();

            ddProjectName.Items.Insert(0, new ListItem("-- No Projects Found --", ""));
        }





        string str_currency = "select c.CurrencyCode,c.LocalCurrencyID from IT_ClientDetails a inner join IT_Countries b on a.Country=b.CountryKey inner join IT_Currency c on b.Country=c.CountryName where a.ClientKey='" + client_Id + "'";
        SqlCommand cmd_str_currency = new SqlCommand(str_currency);
        DataSet ds_currency = this.DA.GetDataSet(cmd_str_currency);

        if (ds_currency != null && ds_currency.Tables.Count > 0 && ds_currency.Tables[0].Rows.Count > 0)
        {
            DD_Currency.DataSource = ds_currency.Tables[0];
            DD_Currency.DataTextField = "CurrencyCode";
            DD_Currency.DataValueField = "LocalCurrencyID";
            DD_Currency.DataBind();
            DD_Currency.Items.Insert(0, new ListItem("-- Select Currency --", "")); // matches validator InitialValue
        }
        else
        {
            DD_Currency.Items.Clear();
            DD_Currency.DataSource = null;
            DD_Currency.DataBind();

            DD_Currency.Items.Insert(0, new ListItem("-- No Currency Found --", ""));
        }


    }

    private void BindBalnce(string client_Id)
    {
        decimal valuBudget = 0;
        decimal paidamount = 0;
        int invoiceCount = 0;

        string str_project = "select sum(Budget) as Budget from IT_Projects where ProjectKey = @ProjectKey";
        SqlCommand cmdproject = new SqlCommand(str_project);
        cmdproject.Parameters.AddWithValue("@ProjectKey", client_Id);
        DataSet ds_project = this.DA.GetDataSet(cmdproject);

        DateTime currentDate = DateTime.Now;
        DateTime fyStart, fyEnd;
        if (currentDate.Month >= 4)
        {
            fyStart = new DateTime(currentDate.Year, 4, 1);
            fyEnd = new DateTime(currentDate.Year + 1, 3, 31, 23, 59, 59);
        }
        else
        {
            fyStart = new DateTime(currentDate.Year - 1, 4, 1);
            fyEnd = new DateTime(currentDate.Year, 3, 31, 23, 59, 59);
        }

        string str_projectAmount = "select count(InvoiceKey) as invoiceCount, sum(InvoiceAmount) as amount from IT_Invoices where ProjectKey = @ProjectKey AND ISNULL(InvoiceDate, CreatedOn) >= @FYStart AND ISNULL(InvoiceDate, CreatedOn) <= @FYEnd";
        SqlCommand cmdprojectAmount = new SqlCommand(str_projectAmount);
        cmdprojectAmount.Parameters.AddWithValue("@ProjectKey", client_Id);
        cmdprojectAmount.Parameters.AddWithValue("@FYStart", fyStart);
        cmdprojectAmount.Parameters.AddWithValue("@FYEnd", fyEnd);
        DataSet ds_projectAmount = this.DA.GetDataSet(cmdprojectAmount);

        if (ds_project.Tables[0].Rows.Count > 0 &&
            ds_project.Tables[0].Rows[0]["Budget"] != DBNull.Value)
        {
            valuBudget = Convert.ToDecimal(ds_project.Tables[0].Rows[0]["Budget"]);
        }

        if (ds_projectAmount.Tables[0].Rows.Count > 0)
        {
            if (ds_projectAmount.Tables[0].Rows[0]["amount"] != DBNull.Value)
                paidamount = Convert.ToDecimal(ds_projectAmount.Tables[0].Rows[0]["amount"]);
            if (ds_projectAmount.Tables[0].Rows[0]["invoiceCount"] != DBNull.Value)
                invoiceCount = Convert.ToInt32(ds_projectAmount.Tables[0].Rows[0]["invoiceCount"]);
        }

        lblInvoiceCount.InnerText = invoiceCount.ToString();
        lblTotalAmount.InnerText = paidamount.ToString("0.00");
        Balance.InnerText = (valuBudget - paidamount).ToString("0.00");
        BalanceViews.Visible = true;
    }




    protected void Rd_Project_SelectedIndexChanged(object sender, EventArgs e)
    {
        string client_Id = ddProjectName.SelectedValue;

        if (string.IsNullOrEmpty(client_Id))
        {
            BalanceViews.Visible = false;
            divGrid.Visible = false;
            return;
        }


        BindBalnce(client_Id);
        Invoicegrid();
    }
    protected void Rd_Status_SelectedIndexChanged(object sender, EventArgs e)
    {
        string client_Id = ddlClient.SelectedValue;

        if (string.IsNullOrEmpty(client_Id))
        {
            SGSTViews.Visible = false;
            CGSTViews.Visible = false;
            IGSTViews.Visible = false;
            ddProjectName.Items.Clear();
            ddProjectName.Items.Insert(0, new ListItem("-- No Projects Found --", ""));
            return;
        }

        GSTViews(client_Id);


        string str_project = "select ProjectName,ProjectKey from IT_Projects where ClientKey='" + client_Id + "'";
        SqlCommand cmdproject = new SqlCommand(str_project);
        DataSet ds_project = this.DA.GetDataSet(cmdproject);

        if (ds_project != null && ds_project.Tables.Count > 0 && ds_project.Tables[0].Rows.Count > 0)
        {
            ddProjectName.DataSource = ds_project.Tables[0];
            ddProjectName.DataTextField = "ProjectName";
            ddProjectName.DataValueField = "ProjectKey";
            ddProjectName.DataBind();
            ddProjectName.Items.Insert(0, new ListItem("-- Select Projects --", "")); // matches validator InitialValue
        }
        else
        {
            ddProjectName.Items.Clear();
            ddProjectName.DataSource = null;
            ddProjectName.DataBind();

            ddProjectName.Items.Insert(0, new ListItem("-- No Projects Found --", ""));
        }





        string str_currency = "select c.CurrencyCode,c.LocalCurrencyID from IT_ClientDetails a inner join IT_Countries b on a.Country=b.CountryKey inner join IT_Currency c on b.Country=c.CountryName where a.ClientKey='" + client_Id + "'";
        SqlCommand cmd_str_currency = new SqlCommand(str_currency);
        DataSet ds_currency = this.DA.GetDataSet(cmd_str_currency);

        if (ds_currency != null && ds_currency.Tables.Count > 0 && ds_currency.Tables[0].Rows.Count > 0)
        {
            DD_Currency.DataSource = ds_currency.Tables[0];
            DD_Currency.DataTextField = "CurrencyCode";
            DD_Currency.DataValueField = "LocalCurrencyID";
            DD_Currency.DataBind();
            DD_Currency.Items.Insert(0, new ListItem("-- Select Currency --", "")); // matches validator InitialValue
        }
        else
        {
            DD_Currency.Items.Clear();
            DD_Currency.DataSource = null;
            DD_Currency.DataBind();

            DD_Currency.Items.Insert(0, new ListItem("-- No Currency Found --", ""));
        }

    }

    private void BindStatus()
    {
        string str_status = "SELECT id, name FROM it_invoicestatus ORDER BY id";
        SqlCommand cmd = new SqlCommand(str_status);
        DataSet ds = this.DA.GetDataSet(cmd);

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DD_Status.DataSource = ds.Tables[0];
            DD_Status.DataTextField = "name";
            DD_Status.DataValueField = "id";
            DD_Status.DataBind();
            DD_Status.Items.Insert(0, new ListItem("-- Select Status --", ""));
        }
        else
        {
            DD_Status.Items.Clear();
            DD_Status.DataSource = null;
            DD_Status.DataBind();
            DD_Status.Items.Insert(0, new ListItem("-- No Status Found --", ""));
        }
    }

    private void BindLastInvoiceNumber()
    {
        string sql = "SELECT TOP 1 InvoiceNumber FROM IT_Invoices ORDER BY InvoiceKey DESC";
        SqlCommand cmd = new SqlCommand(sql);
        DataSet ds = this.DA.GetDataSet(cmd);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            lblLastInvoiceNo.Text = "Previous Invoice No: " + ds.Tables[0].Rows[0]["InvoiceNumber"].ToString();
        }
    }

    private bool CheckDuplicateInvoiceServer(string invoiceNumber, string invoiceKey)
    {
        string sql = "SELECT COUNT(1) FROM IT_Invoices WHERE InvoiceNumber = @InvoiceNumber";
        if (!string.IsNullOrEmpty(invoiceKey))
        {
            sql += " AND InvoiceKey != @InvoiceKey";
        }
        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.AddWithValue("@InvoiceNumber", invoiceNumber);
        if (!string.IsNullOrEmpty(invoiceKey))
        {
            cmd.Parameters.AddWithValue("@InvoiceKey", invoiceKey);
        }
        DataSet ds = this.DA.GetDataSet(cmd);
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            int count = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            return count > 0;
        }
        return false;
    }

    // Callback event handling for duplicate checking
    private string callbackResult = "";

    public void RaiseCallbackEvent(string eventArgument)
    {
        string invoiceNo = eventArgument;
        string invoiceKey = hfInvoiceKey.Value;
        bool exists = CheckDuplicateInvoiceServer(invoiceNo, invoiceKey);
        callbackResult = exists ? "1" : "0";
    }

    public string GetCallbackResult()
    {
        return callbackResult;
    }
}