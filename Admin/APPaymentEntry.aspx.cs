using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_APPaymentEntry : System.Web.UI.Page
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
            BindVendors();
            Label lblBread = Master.FindControl("lbl_bread") as Label;
            if (lblBread != null)
            {
                lblBread.Text = "AP Payment Entry";
            }

            if (Request.QueryString["id"] != null &&
                int.TryParse(Request.QueryString["id"], out id))
            {
                btnSave.Text         = "Update";
                headcreate.InnerText = "Edit AP Payment";
                PopulateFormForUpdate(id);
            }
            else
            {
                btnSave.Text         = "Save";
                headcreate.InnerText = "Create AP Payment";
            }
        }
    }

    private void BindVendors()
    {
        string query = "SELECT ClientKey, ClientName FROM IT_ClientDetails WHERE PartyType = 1 AND Status = 1 ORDER BY ClientName";
        SqlCommand cmd = new SqlCommand(query);
        DataTable dt = da.GetDataTable(cmd);

        ddlVendorName.DataSource      = dt;
        ddlVendorName.DataTextField   = "ClientName";
        ddlVendorName.DataValueField  = "ClientKey";
        ddlVendorName.DataBind();
        ddlVendorName.Items.Insert(0, new ListItem(" -- Select Vendor Name -- ", ""));
    }

    private void BindInvoices(string vendorId)
    {
        ddlInvoiceNo.Items.Clear();
        ddlInvoiceNo.Items.Insert(0, new ListItem(" -- Select Invoice No -- ", ""));

        if (string.IsNullOrEmpty(vendorId)) return;

        string query = @"SELECT PayableInvoiceKey, InvoiceNumber
                         FROM IT_PayableInvoices
                         WHERE VendorNameNew = @VendorId
                           AND InvoiceNumber IS NOT NULL
                         ORDER BY InvoiceNumber;";
        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@VendorId", vendorId);
        DataTable dt = da.GetDataTable(cmd);

        ddlInvoiceNo.DataSource     = dt;
        ddlInvoiceNo.DataTextField  = "InvoiceNumber";
        ddlInvoiceNo.DataValueField = "PayableInvoiceKey";
        ddlInvoiceNo.DataBind();
        ddlInvoiceNo.Items.Insert(0, new ListItem(" -- Select Invoice No -- ", ""));
    }

    protected void ddlVendorName_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindInvoices(ddlVendorName.SelectedValue);
        ClearInvoiceFields();
    }

    protected void ddlInvoiceNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(ddlInvoiceNo.SelectedValue)) return;

        string query = @"SELECT InvoiceAmount, GSTPercentage, GSTAmount, TotalPayableAmount, InvoiceDate, TDSPercentage, TDSAmount
                         FROM IT_PayableInvoices
                         WHERE PayableInvoiceKey = @InvoiceId";
        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@InvoiceId", ddlInvoiceNo.SelectedValue);
        DataTable dt = da.GetDataTable(cmd);

        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];

            txtSubTotal.Text   = dr["InvoiceAmount"] != DBNull.Value ? dr["InvoiceAmount"].ToString() : "0";
            txtGSTPercent.Text = dr["GSTPercentage"] != DBNull.Value ? dr["GSTPercentage"].ToString() : "0";
            txtGST.Text        = dr["GSTAmount"] != DBNull.Value ? dr["GSTAmount"].ToString() : "0";
            txtGrandTotal.Text = dr["TotalPayableAmount"] != DBNull.Value ? dr["TotalPayableAmount"].ToString() : "0";
            txtPercent.Text    = dr["TDSPercentage"] != DBNull.Value ? dr["TDSPercentage"].ToString() : "0";
            
            decimal tdsAmt = dr["TDSAmount"] != DBNull.Value ? Convert.ToDecimal(dr["TDSAmount"]) : 0;
            
            txtAmount.Text = tdsAmt.ToString("0.##");
            hfAmount.Value = tdsAmt.ToString("0.##");
            
            decimal grandTotal = dr["TotalPayableAmount"] != DBNull.Value ? Convert.ToDecimal(dr["TotalPayableAmount"]) : 0;
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
        txtGSTPercent.Text    = "";
        txtGST.Text           = "";
        txtGrandTotal.Text    = "";
        txtPercent.Text       = "";
        txtAmount.Text        = "";
        txtNetDue.Text        = "";
        txtPayment.Text       = "";
        txtBalanceAmount.Text = "";
    }


    private void PopulateFormForUpdate(int recordId)
    {
        string query = @"SELECT AP_VendorId, AP_InvoiceId, AP_InvoiceDate,
                                AP_SubTotal, AP_GST, AP_GrandTotal,
                                AP_Percent, AP_Amount,
                                AP_NetDue, AP_Payment, AP_BalanceAmount
                         FROM IT_APPaymentEntry
                         WHERE AP_Id = @AP_Id";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@AP_Id", recordId);
        DataTable dt = da.GetDataTable(cmd);

        if (dt.Rows.Count > 0)
        {
            DataRow dr = dt.Rows[0];

            BindInvoices(dr["AP_VendorId"].ToString());

            ddlVendorName.SelectedValue = dr["AP_VendorId"].ToString();
            ddlInvoiceNo.SelectedValue  = dr["AP_InvoiceId"].ToString();
            
            if (dr["AP_InvoiceDate"] != DBNull.Value)
            {
                if (dr["AP_InvoiceDate"] is DateTimeOffset)
                {
                    DateTimeOffset dto = (DateTimeOffset)dr["AP_InvoiceDate"];
                    txtInvoiceDate.Text = dto.DateTime.ToString("yyyy-MM-dd");
                }
                else
                {
                    DateTime invDate = Convert.ToDateTime(dr["AP_InvoiceDate"]);
                    txtInvoiceDate.Text = invDate.ToString("yyyy-MM-dd");
                }
            }
            
            txtSubTotal.Text            = dr["AP_SubTotal"].ToString();
            txtGST.Text                 = dr["AP_GST"].ToString();
            txtGrandTotal.Text          = dr["AP_GrandTotal"].ToString();
            txtPercent.Text             = dr["AP_Percent"].ToString();
            txtAmount.Text              = dr["AP_Amount"].ToString();
            txtNetDue.Text              = dr["AP_NetDue"].ToString();
            txtPayment.Text             = dr["AP_Payment"].ToString();
            txtBalanceAmount.Text       = dr["AP_BalanceAmount"].ToString();
            hfAPId.Value                = recordId.ToString();
            
            // Fetch GSTPercentage since it's not stored in IT_APPaymentEntry
            string gstPercentQuery = "SELECT GSTPercentage FROM IT_PayableInvoices WHERE PayableInvoiceKey = @InvoiceId";
            SqlCommand gstCmd = new SqlCommand(gstPercentQuery);
            gstCmd.Parameters.AddWithValue("@InvoiceId", dr["AP_InvoiceId"].ToString());
            DataTable dtGst = da.GetDataTable(gstCmd);
            if (dtGst.Rows.Count > 0 && dtGst.Rows[0]["GSTPercentage"] != DBNull.Value)
            {
                txtGSTPercent.Text = dtGst.Rows[0]["GSTPercentage"].ToString();
            }
            else
            {
                txtGSTPercent.Text = "0";
            }
            
            hfAmount.Value     = dr["AP_Amount"].ToString();
            hfNetDue.Value     = dr["AP_NetDue"].ToString();
            hfPayment.Value    = dr["AP_Payment"].ToString();
            hfBalanceAmt.Value = dr["AP_BalanceAmount"].ToString();

            // Load payment details from sub table
            LoadPaymentDetails(recordId);
            
            // Trigger client-side calculations
            ScriptManager.RegisterStartupScript(this, this.GetType(), "triggerCalc",
                "setTimeout(function(){ recalcTotals(); }, 600);", true);
        }
    }

    private void LoadPaymentDetails(int apId)
    {
        string query = @"SELECT PD_TransactionNo, PD_PaymentDate, PD_PaymentAmount
                         FROM IT_APPaymentDetails
                         WHERE AP_Id = @AP_Id
                         ORDER BY PD_Id";
        
        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@AP_Id", apId);
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
            if (!string.IsNullOrEmpty(hfAPId.Value))
            {
                // UPDATE
                string updateQuery = @"UPDATE IT_APPaymentEntry SET
                                        AP_VendorId      = @AP_VendorId,
                                        AP_InvoiceId     = @AP_InvoiceId,
                                        AP_InvoiceDate   = @AP_InvoiceDate,
                                        AP_SubTotal      = @AP_SubTotal,
                                        AP_GST           = @AP_GST,
                                        AP_GrandTotal    = @AP_GrandTotal,
                                        AP_Percent       = @AP_Percent,
                                        AP_Amount        = @AP_Amount,
                                        AP_NetDue        = @AP_NetDue,
                                        AP_Payment       = @AP_Payment,
                                        AP_BalanceAmount = @AP_BalanceAmount,
                                        AP_ModifiedOn    = GETDATE(),
                                        AP_ModifiedBy    = @AP_ModifiedBy
                                       WHERE AP_Id = @AP_Id";

                SqlCommand cmd = new SqlCommand(updateQuery);
                AddCommonParams(cmd);
                cmd.Parameters.AddWithValue("@AP_ModifiedBy", SC.Userid);
                cmd.Parameters.AddWithValue("@AP_Id",         hfAPId.Value);
                da.ExecuteNonQuery(cmd);

                SavePaymentDetails(Convert.ToInt32(hfAPId.Value));

                ScriptManager.RegisterStartupScript(this, this.GetType(), "toast",
                    "showToastr('success', 'AP Payment Updated Successfully'); setTimeout(function(){ window.location.href='APPaymentGrid.aspx'; }, 2000);", true);
            }
            else
            {
                // Duplicate check
                string checkQuery = @"SELECT COUNT(1) FROM IT_APPaymentEntry
                                      WHERE AP_VendorId  = @AP_VendorId
                                        AND AP_InvoiceId = @AP_InvoiceId";
                SqlCommand chkCmd = new SqlCommand(checkQuery);
                chkCmd.Parameters.AddWithValue("@AP_VendorId",  ddlVendorName.SelectedValue);
                chkCmd.Parameters.AddWithValue("@AP_InvoiceId", ddlInvoiceNo.SelectedValue);
                DataTable dtChk = da.GetDataTable(chkCmd);

                if (dtChk.Rows.Count > 0 && Convert.ToInt32(dtChk.Rows[0][0]) > 0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toast",
                        "showToastr('error', 'Payment entry already exists for this Invoice.');", true);
                    return;
                }

                // INSERT
                string insertQuery = @"INSERT INTO IT_APPaymentEntry
                                       (AP_VendorId, AP_InvoiceId, AP_InvoiceDate,
                                        AP_SubTotal, AP_GST, AP_GrandTotal,
                                        AP_Percent, AP_Amount,
                                        AP_NetDue, AP_Payment, AP_BalanceAmount,
                                        AP_CreatedOn, AP_CreatedBy)
                                       VALUES
                                       (@AP_VendorId, @AP_InvoiceId, @AP_InvoiceDate,
                                        @AP_SubTotal, @AP_GST, @AP_GrandTotal,
                                        @AP_Percent, @AP_Amount,
                                        @AP_NetDue, @AP_Payment, @AP_BalanceAmount,
                                        GETDATE(), @AP_CreatedBy)";

                SqlCommand cmd = new SqlCommand(insertQuery);
                AddCommonParams(cmd);
                cmd.Parameters.AddWithValue("@AP_CreatedBy", SC.Userid);
                da.ExecuteNonQuery(cmd);

                // Get the newly inserted ID
                string getIdQuery = "SELECT MAX(AP_Id) FROM IT_APPaymentEntry WHERE AP_CreatedBy = @AP_CreatedBy";
                SqlCommand idCmd = new SqlCommand(getIdQuery);
                idCmd.Parameters.AddWithValue("@AP_CreatedBy", SC.Userid);
                DataTable dtId = da.GetDataTable(idCmd);
                int newAPId = Convert.ToInt32(dtId.Rows[0][0]);

                SavePaymentDetails(newAPId);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "toast",
                    "showToastr('success', 'AP Payment Created Successfully'); setTimeout(function(){ window.location.href='APPaymentGrid.aspx'; }, 2000);", true);
            }
        }
        catch (Exception ex)
        {
            string errorMsg = ex.Message.Replace("'", "\\'").Replace("\r\n", " ");
            ScriptManager.RegisterStartupScript(this, this.GetType(), "toast",
                "showToastr('error', 'Error: " + errorMsg + "');", true);
        }
    }

    private void SavePaymentDetails(int apId)
    {
        string deleteQuery = "DELETE FROM IT_APPaymentDetails WHERE AP_Id = @AP_Id";
        SqlCommand delCmd = new SqlCommand(deleteQuery);
        delCmd.Parameters.AddWithValue("@AP_Id", apId);
        da.ExecuteNonQuery(delCmd);

        if (!string.IsNullOrEmpty(hfPaymentRows.Value))
        {
            var paymentRows = Newtonsoft.Json.JsonConvert.DeserializeObject<List<PaymentRow>>(hfPaymentRows.Value);
            
            foreach (var row in paymentRows)
            {
                if (!string.IsNullOrEmpty(row.payAmt))
                {
                    string insertDetail = @"INSERT INTO IT_APPaymentDetails
                                           (AP_Id, PD_TransactionNo, PD_PaymentDate, PD_PaymentAmount, PD_CreatedOn, PD_CreatedBy)
                                           VALUES (@AP_Id, @PD_TransactionNo, @PD_PaymentDate, @PD_PaymentAmount, GETDATE(), @PD_CreatedBy)";
                    
                    SqlCommand detailCmd = new SqlCommand(insertDetail);
                    detailCmd.Parameters.AddWithValue("@AP_Id", apId);
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
        cmd.Parameters.AddWithValue("@AP_VendorId",      ddlVendorName.SelectedValue);
        cmd.Parameters.AddWithValue("@AP_InvoiceId",     ddlInvoiceNo.SelectedValue);
        cmd.Parameters.AddWithValue("@AP_InvoiceDate",   string.IsNullOrEmpty(txtInvoiceDate.Text) ? (object)DBNull.Value : txtInvoiceDate.Text.Trim());
        cmd.Parameters.AddWithValue("@AP_SubTotal",      ParseDecimal(txtSubTotal.Text));
        cmd.Parameters.AddWithValue("@AP_GST",           ParseDecimal(txtGST.Text));
        cmd.Parameters.AddWithValue("@AP_GrandTotal",    ParseDecimal(txtGrandTotal.Text));
        cmd.Parameters.AddWithValue("@AP_Percent",       ParseDecimal(txtPercent.Text));
        cmd.Parameters.AddWithValue("@AP_Amount",        ParseDecimal(hfAmount.Value));
        cmd.Parameters.AddWithValue("@AP_NetDue",        ParseDecimal(hfNetDue.Value));
        cmd.Parameters.AddWithValue("@AP_Payment",       ParseDecimal(hfPayment.Value));
        cmd.Parameters.AddWithValue("@AP_BalanceAmount", ParseDecimal(hfBalanceAmt.Value));
    }

    private decimal ParseDecimal(string val)
    {
        decimal result;
        return decimal.TryParse(val, out result) ? result : 0;
    }
}
