using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_ARPaymentEntry : System.Web.UI.Page
{
    SessionCustom SC;
    DataAccess da;
    string Userid = "";
    int id;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.SC = new SessionCustom();
        this.da = new DataAccess();
        Userid = this.SC.Userid;

        if (!IsPostBack)
        {
            BindClients();
            Label lblBread = Master.FindControl("lbl_bread") as Label;
            if (lblBread != null)
            {
                lblBread.Text = "AR Payment Entry";
            }

            if (Request.QueryString["id"] != null &&
                int.TryParse(Request.QueryString["id"], out id))
            {
                btnSave.Text         = "Update";
                headcreate.InnerText = "Edit AR Payment";
                PopulateFormForUpdate(id);
            }
            else
            {
                btnSave.Text         = "Save";
                headcreate.InnerText = "Create AR Payment";
            }
        }
    }

    private void BindClients()
    {
        string query = "SELECT ClientKey, CompanyName FROM IT_ClientDetails ORDER BY CompanyName";
        SqlCommand cmd = new SqlCommand(query);
        DataTable dt = da.GetDataTable(cmd);

        ddlClientName.DataSource      = dt;
        ddlClientName.DataTextField   = "CompanyName";
        ddlClientName.DataValueField  = "ClientKey";
        ddlClientName.DataBind();
        ddlClientName.Items.Insert(0, new ListItem(" -- Select Client Name -- ", ""));
    }

    private void BindInvoices(string clientId)
    {
        ddlInvoiceNo.Items.Clear();
        ddlInvoiceNo.Items.Insert(0, new ListItem(" -- Select Invoice No -- ", ""));

        if (string.IsNullOrEmpty(clientId)) return;

        string query = @"SELECT InvoiceKey, InvoiceNumber
                         FROM IT_Invoices
                         WHERE ClientKey = @ClientId
                           AND InvoiceNumber IS NOT NULL
                           AND InvoiceStatus = '2'
                         ORDER BY InvoiceNumber;";
        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@ClientId", clientId);
        DataTable dt = da.GetDataTable(cmd);

        ddlInvoiceNo.DataSource     = dt;
        ddlInvoiceNo.DataTextField  = "InvoiceNumber";
        ddlInvoiceNo.DataValueField = "InvoiceKey";
        ddlInvoiceNo.DataBind();
        ddlInvoiceNo.Items.Insert(0, new ListItem(" -- Select Invoice No -- ", ""));
    }

    protected void ddlClientName_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindInvoices(ddlClientName.SelectedValue);
        ClearInvoiceFields();
    }

    protected void ddlInvoiceNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(ddlInvoiceNo.SelectedValue)) return;

        string query = @"SELECT InvoiceAmount, GSTAmount, TotalAmount, InvoiceDate, TDSAmount, TdsPercentage, GstPercentage
                         FROM IT_Invoices
                         WHERE InvoiceKey = @InvoiceId";
        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@InvoiceId", ddlInvoiceNo.SelectedValue);
        DataTable dt = da.GetDataTable(cmd);

        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];

            txtSubTotal.Text   = dr["InvoiceAmount"] != DBNull.Value ? dr["InvoiceAmount"].ToString() : "0";
            txtGST.Text        = dr["GSTAmount"] != DBNull.Value ? dr["GSTAmount"].ToString() : "0";
            txtGrandTotal.Text = dr["TotalAmount"] != DBNull.Value ? dr["TotalAmount"].ToString() : "0";
            
            decimal subTotal = dr["InvoiceAmount"] != DBNull.Value ? Convert.ToDecimal(dr["InvoiceAmount"]) : 0;
            decimal tdsAmt = 0;
            if (dt.Columns.Contains("TDSAmount") && dr["TDSAmount"] != DBNull.Value)
            {
                tdsAmt = Convert.ToDecimal(dr["TDSAmount"]);
            }
            
            txtAmount.Text = tdsAmt.ToString("0.##");
            hfAmount.Value = tdsAmt.ToString("0.##");
            
            if (dt.Columns.Contains("TdsPercentage") && dr["TdsPercentage"] != DBNull.Value)
            {
                txtPercent.Text = dr["TdsPercentage"].ToString();
            }
            else
            {
                txtPercent.Text = "0";
            }

            if (dt.Columns.Contains("GstPercentage") && dr["GstPercentage"] != DBNull.Value)
            {
                txtGSTPercent.Text = dr["GstPercentage"].ToString();
            }
            else
            {
                txtGSTPercent.Text = "0";
            }
            
            decimal grandTotal = dr["TotalAmount"] != DBNull.Value ? Convert.ToDecimal(dr["TotalAmount"]) : 0;
            decimal netDue = grandTotal - tdsAmt;
            txtNetDue.Text = netDue.ToString("0.##");
            hfNetDue.Value = netDue.ToString("0.##");
            
            if (dr["InvoiceDate"] != DBNull.Value)
            {
                if (dr["InvoiceDate"] is DateTimeOffset)
                {
                    DateTimeOffset issueDate = (DateTimeOffset)dr["InvoiceDate"];
                    txtInvoiceDate.Text = issueDate.DateTime.ToString("yyyy-MM-dd");
                }
                else
                {
                    DateTime invDate = Convert.ToDateTime(dr["InvoiceDate"]);
                    txtInvoiceDate.Text = invDate.ToString("yyyy-MM-dd");
                }
            }
            else
            {
                txtInvoiceDate.Text = "";
            }
        }
    }

    private void ClearInvoiceFields()
    {
        txtSubTotal.Text      = "";
        txtGST.Text           = "";
        txtGSTPercent.Text    = "";
        txtGrandTotal.Text    = "";
        txtAmount.Text        = "";
        txtNetDue.Text        = "";
        txtPayment.Text       = "";
        txtBalanceAmount.Text = "";
        txtPercent.Text       = "";
    }


    private void PopulateFormForUpdate(int recordId)
    {
        string query = @"SELECT AR_ClientId, AR_InvoiceId, AR_InvoiceDate,
                                AR_SubTotal, AR_GST, AR_GrandTotal,
                                AR_Percent, AR_Amount,
                                AR_NetDue, AR_Payment, AR_BalanceAmount
                         FROM IT_ARPaymentEntry
                         WHERE AR_Id = @AR_Id";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@AR_Id", recordId);
        DataTable dt = da.GetDataTable(cmd);

        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];

            BindInvoices(dr["AR_ClientId"].ToString());

            ddlClientName.SelectedValue = dr["AR_ClientId"].ToString();
            ddlInvoiceNo.SelectedValue  = dr["AR_InvoiceId"].ToString();
            
            if (dr["AR_InvoiceDate"] != DBNull.Value)
            {
                if (dr["AR_InvoiceDate"] is DateTimeOffset)
                {
                    DateTimeOffset dto = (DateTimeOffset)dr["AR_InvoiceDate"];
                    txtInvoiceDate.Text = dto.DateTime.ToString("yyyy-MM-dd");
                }
                else
                {
                    DateTime invDate = Convert.ToDateTime(dr["AR_InvoiceDate"]);
                    txtInvoiceDate.Text = invDate.ToString("yyyy-MM-dd");
                }
            }
            
            txtSubTotal.Text            = dr["AR_SubTotal"].ToString();
            txtGST.Text                 = dr["AR_GST"].ToString();
            txtGrandTotal.Text          = dr["AR_GrandTotal"].ToString();
            txtPercent.Text             = dr["AR_Percent"].ToString();
            txtAmount.Text              = dr["AR_Amount"].ToString();
            txtNetDue.Text              = dr["AR_NetDue"].ToString();
            txtPayment.Text             = dr["AR_Payment"].ToString();
            txtBalanceAmount.Text       = dr["AR_BalanceAmount"].ToString();
            hfARId.Value                = recordId.ToString();
            
            decimal arSubTotal = dr["AR_SubTotal"] != DBNull.Value ? Convert.ToDecimal(dr["AR_SubTotal"]) : 0;
            decimal arGstAmt = dr["AR_GST"] != DBNull.Value ? Convert.ToDecimal(dr["AR_GST"]) : 0;
            if (arSubTotal > 0 && arGstAmt > 0)
            {
                decimal arGstPercent = (arGstAmt / arSubTotal) * 100;
                txtGSTPercent.Text = arGstPercent.ToString("0.##");
            }
            else
            {
                txtGSTPercent.Text = "0";
            }
            
            hfAmount.Value     = dr["AR_Amount"].ToString();
            hfNetDue.Value     = dr["AR_NetDue"].ToString();
            hfPayment.Value    = dr["AR_Payment"].ToString();
            hfBalanceAmt.Value = dr["AR_BalanceAmount"].ToString();

            // Load payment details from sub table
            LoadPaymentDetails(recordId);
            
            // Trigger client-side calculations
            ScriptManager.RegisterStartupScript(this, this.GetType(), "triggerCalc",
                "setTimeout(function(){ recalcTotals(); }, 600);", true);
        }
    }

    private void LoadPaymentDetails(int arId)
    {
        string query = @"SELECT PD_TransactionNo, PD_PaymentDate, PD_PaymentAmount
                         FROM IT_ARPaymentDetails
                         WHERE AR_Id = @AR_Id
                         ORDER BY PD_Id";
        
        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@AR_Id", arId);
        DataTable dt = da.GetDataTable(cmd);

        if (dt.Rows.Count > 0)
        {
            var paymentRows = new List<PaymentRow>();
            foreach (DataRow dr in dt.Rows)
            {
                paymentRows.Add(new PaymentRow
                {
                    txnNo = dr["PD_TransactionNo"].ToString(),
                    payDate = dr["PD_PaymentDate"] != DBNull.Value ? Convert.ToDateTime(dr["PD_PaymentDate"]).ToString("yyyy-MM-dd") : "",
                    payAmt = dr["PD_PaymentAmount"].ToString()
                });
            }
            hfPaymentRows.Value = Newtonsoft.Json.JsonConvert.SerializeObject(paymentRows);
            
            // Register script to populate rows on client side
            ScriptManager.RegisterStartupScript(this, this.GetType(), "loadPaymentRows",
                "setTimeout(function(){ loadExistingPaymentRows(); }, 500);", true);
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(hfARId.Value))
            {
                // UPDATE
                string updateQuery = @"UPDATE IT_ARPaymentEntry SET
                                        AR_ClientId      = @AR_ClientId,
                                        AR_InvoiceId     = @AR_InvoiceId,
                                        AR_InvoiceDate   = @AR_InvoiceDate,
                                        AR_SubTotal      = @AR_SubTotal,
                                        AR_GST           = @AR_GST,
                                        AR_GrandTotal    = @AR_GrandTotal,
                                        AR_Percent       = @AR_Percent,
                                        AR_Amount        = @AR_Amount,
                                        AR_NetDue        = @AR_NetDue,
                                        AR_Payment       = @AR_Payment,
                                        AR_BalanceAmount = @AR_BalanceAmount,
                                        AR_ModifiedOn    = GETDATE(),
                                        AR_ModifiedBy    = @AR_ModifiedBy
                                       WHERE AR_Id = @AR_Id";

                SqlCommand cmd = new SqlCommand(updateQuery);
                AddCommonParams(cmd);
                cmd.Parameters.AddWithValue("@AR_ModifiedBy", SC.Userid);
                cmd.Parameters.AddWithValue("@AR_Id",         hfARId.Value);
                da.ExecuteNonQuery(cmd);

                SavePaymentDetails(Convert.ToInt32(hfARId.Value));

                ScriptManager.RegisterStartupScript(this, this.GetType(), "toast",
                    "showToastr('success', 'AR Payment Updated Successfully'); setTimeout(function(){ window.location.href='ARPaymentGrid.aspx'; }, 2000);", true);
            }
            else
            {
                // Duplicate check
                string checkQuery = @"SELECT COUNT(1) FROM IT_ARPaymentEntry
                                      WHERE AR_ClientId  = @AR_ClientId
                                        AND AR_InvoiceId = @AR_InvoiceId";
                SqlCommand chkCmd = new SqlCommand(checkQuery);
                chkCmd.Parameters.AddWithValue("@AR_ClientId",  ddlClientName.SelectedValue);
                chkCmd.Parameters.AddWithValue("@AR_InvoiceId", ddlInvoiceNo.SelectedValue);
                DataTable dtChk = da.GetDataTable(chkCmd);

                if (dtChk.Rows.Count > 0 && Convert.ToInt32(dtChk.Rows[0][0]) > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toast",
                        "showToastr('error', 'Payment entry already exists for this Invoice.');", true);
                    return;
                }

                // INSERT
                string insertQuery = @"INSERT INTO IT_ARPaymentEntry
                                       (AR_ClientId, AR_InvoiceId, AR_InvoiceDate,
                                        AR_SubTotal, AR_GST, AR_GrandTotal,
                                        AR_Percent, AR_Amount,
                                        AR_NetDue, AR_Payment, AR_BalanceAmount,
                                        AR_CreatedOn, AR_CreatedBy)
                                       VALUES
                                       (@AR_ClientId, @AR_InvoiceId, @AR_InvoiceDate,
                                        @AR_SubTotal, @AR_GST, @AR_GrandTotal,
                                        @AR_Percent, @AR_Amount,
                                        @AR_NetDue, @AR_Payment, @AR_BalanceAmount,
                                        GETDATE(), @AR_CreatedBy)";

                SqlCommand cmd = new SqlCommand(insertQuery);
                AddCommonParams(cmd);
                cmd.Parameters.AddWithValue("@AR_CreatedBy", SC.Userid);
                da.ExecuteNonQuery(cmd);

                // Get the newly inserted ID
                string getIdQuery = "SELECT MAX(AR_Id) FROM IT_ARPaymentEntry WHERE AR_CreatedBy = @AR_CreatedBy";
                SqlCommand idCmd = new SqlCommand(getIdQuery);
                idCmd.Parameters.AddWithValue("@AR_CreatedBy", SC.Userid);
                DataTable dtId = da.GetDataTable(idCmd);
                int newARId = Convert.ToInt32(dtId.Rows[0][0]);

                SavePaymentDetails(newARId);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "toast",
                    "showToastr('success', 'AR Payment Created Successfully'); setTimeout(function(){ window.location.href='ARPaymentGrid.aspx'; }, 2000);", true);
            }
        }
        catch (Exception ex)
        {
            string errorMsg = ex.Message.Replace("'", "\\'").Replace("\r\n", " ");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "toast",
                "showToastr('error', 'Error: " + errorMsg + "');", true);
        }
    }

    private void SavePaymentDetails(int arId)
    {
        string deleteQuery = "DELETE FROM IT_ARPaymentDetails WHERE AR_Id = @AR_Id";
        SqlCommand delCmd = new SqlCommand(deleteQuery);
        delCmd.Parameters.AddWithValue("@AR_Id", arId);
        da.ExecuteNonQuery(delCmd);

        if (!string.IsNullOrEmpty(hfPaymentRows.Value))
        {
            var paymentRows = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PaymentRow>>(hfPaymentRows.Value);
            
            foreach (var row in paymentRows)
            {
                if (!string.IsNullOrEmpty(row.payAmt))
                {
                    string insertDetail = @"INSERT INTO IT_ARPaymentDetails
                                           (AR_Id, PD_TransactionNo, PD_PaymentDate, PD_PaymentAmount, PD_CreatedOn, PD_CreatedBy)
                                           VALUES (@AR_Id, @PD_TransactionNo, @PD_PaymentDate, @PD_PaymentAmount, GETDATE(), @PD_CreatedBy)";
                    
                    SqlCommand detailCmd = new SqlCommand(insertDetail);
                    detailCmd.Parameters.AddWithValue("@AR_Id", arId);
                    detailCmd.Parameters.AddWithValue("@PD_TransactionNo", row.txnNo ?? "");
                    detailCmd.Parameters.AddWithValue("@PD_PaymentDate", string.IsNullOrEmpty(row.payDate) ? (object)DBNull.Value : row.payDate);
                    detailCmd.Parameters.AddWithValue("@PD_PaymentAmount", ParseDecimal(row.payAmt));
                    detailCmd.Parameters.AddWithValue("@PD_CreatedBy", SC.Userid);
                    da.ExecuteNonQuery(detailCmd);
                }
            }
        }
    }

    private class PaymentRow
    {
        public string txnNo { get; set; }
        public string payDate { get; set; }
        public string payAmt { get; set; }
    }

    private void AddCommonParams(SqlCommand cmd)
    {
        cmd.Parameters.AddWithValue("@AR_ClientId",      ddlClientName.SelectedValue);
        cmd.Parameters.AddWithValue("@AR_InvoiceId",     ddlInvoiceNo.SelectedValue);
        cmd.Parameters.AddWithValue("@AR_InvoiceDate",   string.IsNullOrEmpty(txtInvoiceDate.Text) ? (object)DBNull.Value : txtInvoiceDate.Text.Trim());
        cmd.Parameters.AddWithValue("@AR_SubTotal",      ParseDecimal(txtSubTotal.Text));
        cmd.Parameters.AddWithValue("@AR_GST",           ParseDecimal(txtGST.Text));
        cmd.Parameters.AddWithValue("@AR_GrandTotal",    ParseDecimal(txtGrandTotal.Text));
        cmd.Parameters.AddWithValue("@AR_Percent",       ParseDecimal(txtPercent.Text));
        cmd.Parameters.AddWithValue("@AR_Amount",        ParseDecimal(hfAmount.Value));
        cmd.Parameters.AddWithValue("@AR_NetDue",        ParseDecimal(hfNetDue.Value));
        cmd.Parameters.AddWithValue("@AR_Payment",       ParseDecimal(hfPayment.Value));
        cmd.Parameters.AddWithValue("@AR_BalanceAmount", ParseDecimal(hfBalanceAmt.Value));
    }

    private decimal ParseDecimal(string val)
    {
        decimal result;
        return decimal.TryParse(val, out result) ? result : 0;
    }
}
