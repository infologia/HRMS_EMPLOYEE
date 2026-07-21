using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Sockets;
using System.Globalization;

public partial class Admin_Payableinvoice1 :  System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string Id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        if (!IsPostBack)
        {
            BindClients();
            Label lblBread = Master.FindControl("lbl_bread") as Label;
            if (lblBread != null)
            {
                lblBread.Text = "Payable Invoice";
            }
            if (!IsPostBack && Request.QueryString["id"] != null)
            {
                Id = Request.QueryString["id"];
                btnSave.Visible = false;
                btnUpdate.Visible = true;

                int InvoiceId = Convert.ToInt32(Id);
                hfInvoiceKey.Value = InvoiceId.ToString();
                PopulateProjectData(InvoiceId);
            }
        }
    }
    private void PopulateProjectData(int InvoiceId)
    {
        // string query = "SELECT PayableInvoiceKey, VendorName, InvoiceNumber, CONVERT(varchar(10), InvoiceDate, 111) AS InvoiceDate, CONVERT(varchar(10), DueDate, 111) AS DueDate, CONVERT(varchar(10), PaymentDate, 111) AS PaymentDate, Currency, PaymentStatus, InvoiceAmount, TDSAmount, GSTAmount, TotalPayableAmount, PaymentMode, Description, TotalAmount FROM IT_PayableInvoices WHERE PayableInvoiceKey = @InvoiceKey";
        string query = "SELECT PayableInvoiceKey, VendorNameNew, InvoiceNumber, CONVERT(varchar(10), InvoiceDate, 111) AS InvoiceDate, CONVERT(varchar(10), DueDate, 111) AS DueDate, CONVERT(varchar(10), PaymentDate, 111) AS PaymentDate, Currency, PaymentStatus, InvoiceAmount, TDSAmount, GSTAmount, TotalPayableAmount, PaymentMode, Description, TotalAmount, GSTPercentage, TDSPercentage FROM IT_PayableInvoices WHERE PayableInvoiceKey = @InvoiceKey";
        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@InvoiceKey", InvoiceId);
        DataSet ds = this.DA.GetDataSet(cmd);

        if (ds.Tables[0].Rows.Count > 0)
        {
            DataRow row = ds.Tables[0].Rows[0];
            // string clientId = row["VendorName"].ToString();
            // ddlVendor.SelectedValue = row["VendorName"].ToString();
            string clientId = row["VendorNameNew"] != DBNull.Value ? row["VendorNameNew"].ToString() : "";
            ddlVendor.SelectedValue = clientId;
            if (!string.IsNullOrEmpty(clientId))
                BindCurrency(clientId);
            // txtProjectName.Text = row["InvoiceNumber"].ToString();
            IT_InvoiceDate.Text = row["InvoiceDate"].ToString();
            IT_ReceivedDate.Text = row["DueDate"].ToString();
            //PaymentDate.Text = row["PaymentDate"] != DBNull.Value ? Convert.ToDateTime(row["PaymentDate"]).ToString("yyyy-MM-dd") : "";
            PaymentDate.Text =row["PaymentDate"].ToString();
            DD_Currency.SelectedValue = row["Currency"].ToString();
            DD_Status.SelectedValue = row["PaymentStatus"].ToString();
            InvoiceAmount.Text = row["InvoiceAmount"].ToString();
            TDSAmount.Text = row["TDSAmount"].ToString();
            GSTAmount.Text = row["GSTAmount"].ToString();
            TotalPayableAmount.Text = row["TotalPayableAmount"].ToString();
            if (ds.Tables[0].Columns.Contains("GSTPercentage"))
            {
                GSTPercentage.Text = row["GSTPercentage"].ToString();
            }
            if (ds.Tables[0].Columns.Contains("TDSPercentage"))
            {
                TDSPercentage.Text = row["TDSPercentage"].ToString();
            }
            PaymentMode.Text = row["PaymentMode"].ToString();
            Description.Text = row["Description"].ToString();
            InvoiceNumber.Text = row["InvoiceNumber"].ToString();
            TotalAmount.Text = row["TotalAmount"].ToString();

        }

        // Populate Participants


    }
    protected void btn_send_Click(object sender, EventArgs e)
    {
        string subtotal = hdnSubTotal.Value;
        string TDSAmount = hdnTDS.Value;
        string GST = hdnGST.Value;
        string TAmount = hdnTotal.Value;
        string TotalAmount = hdnTotalAmount.Value;
       

        if (!string.IsNullOrEmpty(IT_InvoiceDate.Text) &&
            !string.IsNullOrEmpty(IT_ReceivedDate.Text))
        {
            DateTime startDate, endDate;

            bool isStartValid = DateTime.TryParseExact(
                IT_InvoiceDate.Text.Trim(),
                "yyyy/MM/dd",          
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out startDate
            );

            bool isEndValid = DateTime.TryParseExact( IT_ReceivedDate.Text.Trim(), "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate
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


        try
        {
            string sql = @"
        INSERT INTO IT_PayableInvoices
        (
            -- VendorName, 
            VendorNameNew,
            InvoiceNumber,
    InvoiceDate,
    DueDate,
    InvoiceAmount,
    GSTAmount,
    TDSAmount,
    TotalPayableAmount,
    Currency,
    PaymentStatus,
    PaymentDate,
    PaymentMode,
    Description,
TotalAmount,
 CreatedBy,
 GSTPercentage,
 TDSPercentage
        )
        VALUES
        (
            -- @VendorName, 
            @VendorNameNew,
            @InvoiceNumber,
    @InvoiceDate,
    @DueDate,
    @InvoiceAmount,
    @GSTAmount,
    @TDSAmount,
    @TotalPayableAmount,
    @Currency,
    @PaymentStatus,
    @PaymentDate,
    @PaymentMode,
    @Description,@TotalAmount,
 @CreatedBy,
 @GSTPercentage,
 @TDSPercentage
        );
        SELECT SCOPE_IDENTITY();"
            ;
            SqlCommand cmd = new SqlCommand(sql);
            
            // cmd.Parameters.AddWithValue("@VendorName", ddlVendor.SelectedValue);
            Guid vendorGuid1;
            cmd.Parameters.AddWithValue("@VendorNameNew", Guid.TryParse(ddlVendor.SelectedValue, out vendorGuid1) ? (object)vendorGuid1 : DBNull.Value);
            cmd.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber.Text);
            cmd.Parameters.AddWithValue("@InvoiceDate", string.IsNullOrEmpty(IT_InvoiceDate.Text) ? (object)DBNull.Value : DateTime.Parse(IT_InvoiceDate.Text));
            cmd.Parameters.AddWithValue("@DueDate", string.IsNullOrEmpty(IT_ReceivedDate.Text) ? (object)DBNull.Value : DateTime.Parse(IT_ReceivedDate.Text));
            cmd.Parameters.AddWithValue("@InvoiceAmount", Convert.ToDecimal(subtotal));
            cmd.Parameters.AddWithValue("@GSTAmount", Convert.ToDecimal(GST));
            cmd.Parameters.AddWithValue("@TDSAmount", Convert.ToDecimal(TDSAmount));
            cmd.Parameters.AddWithValue("@TotalPayableAmount", Convert.ToDecimal(TAmount));
            cmd.Parameters.AddWithValue("@Currency", DD_Currency.SelectedValue);
            cmd.Parameters.AddWithValue("@PaymentStatus", DD_Status.SelectedValue);
            cmd.Parameters.AddWithValue("@PaymentDate", string.IsNullOrEmpty(PaymentDate.Text) ? (object)DBNull.Value : DateTime.Parse(PaymentDate.Text));
            cmd.Parameters.AddWithValue("@PaymentMode", PaymentMode.Text);
            cmd.Parameters.AddWithValue("@Description", Description.Text);
            cmd.Parameters.AddWithValue("@TotalAmount", Convert.ToDecimal(TotalAmount));
            cmd.Parameters.AddWithValue("@CreatedBy", SC.Userid);
            cmd.Parameters.AddWithValue("@GSTPercentage", string.IsNullOrEmpty(GSTPercentage.Text) ? (object)DBNull.Value : Convert.ToDecimal(GSTPercentage.Text));
            cmd.Parameters.AddWithValue("@TDSPercentage", string.IsNullOrEmpty(TDSPercentage.Text) ? (object)DBNull.Value : Convert.ToDecimal(TDSPercentage.Text));


            this.DA.ExecuteNonQuery(cmd);
            ScriptManager.RegisterStartupScript(
               this,
               this.GetType(),
               "toastr_redirect",
               "showToastr('success','Payable Invoice saved successfully!');" +
               "setTimeout(function(){ window.location.href = '/Admin/PayableinvoiceGrid.aspx'; }, 2000);",
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
        string subtotal = hdnSubTotal.Value;
        string TDSAmount = hdnTDS.Value;
        string GST = hdnGST.Value;
        string TAmount = hdnTotal.Value;
        string TotalAmount = hdnTotalAmount.Value;
        if (!string.IsNullOrEmpty(IT_InvoiceDate.Text) &&
             !string.IsNullOrEmpty(IT_ReceivedDate.Text))
        {
            DateTime startDate, endDate;

            bool isStartValid = DateTime.TryParseExact(
                IT_InvoiceDate.Text.Trim(),
                "yyyy/MM/dd",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out startDate
            );

            bool isEndValid = DateTime.TryParseExact(IT_ReceivedDate.Text.Trim(), "yyyy/MM/dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate
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


        int invoiceKey = Convert.ToInt32(hfInvoiceKey.Value);

        try
        {
            string sql = @"
        UPDATE IT_PayableInvoices
        SET
            -- VendorName = @VendorName,
            VendorNameNew = @VendorNameNew,
            InvoiceNumber = @InvoiceNumber,
            InvoiceDate = @InvoiceDate,
            DueDate = @DueDate,
            InvoiceAmount = @InvoiceAmount,
            GSTAmount = @GSTAmount,
            TDSAmount = @TDSAmount,
            TotalPayableAmount = @TotalPayableAmount,
            Currency = @Currency,
            PaymentStatus = @PaymentStatus,
            PaymentDate = @PaymentDate,
            PaymentMode = @PaymentMode,
            Description = @Description,
            ModifiedOn = GETDATE(),
            ModifiedBy = @ModifiedBy,TotalAmount=@TotalAmount,
            GSTPercentage = @GSTPercentage,
            TDSPercentage = @TDSPercentage
        WHERE PayableInvoiceKey = @PayableInvoiceKey";

            SqlCommand cmd = new SqlCommand(sql);

            cmd.Parameters.AddWithValue("@PayableInvoiceKey", invoiceKey);

            // cmd.Parameters.AddWithValue("@VendorName", ddlVendor.SelectedValue);
            Guid vendorGuid2;
            cmd.Parameters.AddWithValue("@VendorNameNew", Guid.TryParse(ddlVendor.SelectedValue, out vendorGuid2) ? (object)vendorGuid2 : DBNull.Value);
            cmd.Parameters.AddWithValue("@InvoiceNumber", InvoiceNumber.Text);
            cmd.Parameters.AddWithValue("@InvoiceDate", string.IsNullOrEmpty(IT_InvoiceDate.Text) ? (object)DBNull.Value : DateTime.Parse(IT_InvoiceDate.Text));
            cmd.Parameters.AddWithValue("@DueDate", string.IsNullOrEmpty(IT_ReceivedDate.Text) ? (object)DBNull.Value : DateTime.Parse(IT_ReceivedDate.Text));
            cmd.Parameters.AddWithValue("@InvoiceAmount", Convert.ToDecimal(subtotal));
            cmd.Parameters.AddWithValue("@GSTAmount", Convert.ToDecimal(GST));
            cmd.Parameters.AddWithValue("@TDSAmount", Convert.ToDecimal(TDSAmount));
            cmd.Parameters.AddWithValue("@TotalPayableAmount", Convert.ToDecimal(TAmount));
            cmd.Parameters.AddWithValue("@Currency", DD_Currency.SelectedValue);
            cmd.Parameters.AddWithValue("@PaymentStatus", DD_Status.SelectedValue);
            cmd.Parameters.AddWithValue("@PaymentDate", string.IsNullOrEmpty(PaymentDate.Text) ? (object)DBNull.Value : DateTime.Parse(PaymentDate.Text));
            cmd.Parameters.AddWithValue("@PaymentMode", PaymentMode.Text);
            cmd.Parameters.AddWithValue("@Description", Description.Text);
            cmd.Parameters.AddWithValue("@TotalAmount", Convert.ToDecimal(TotalAmount));
            cmd.Parameters.AddWithValue("@ModifiedBy", SC.Userid);
            cmd.Parameters.AddWithValue("@GSTPercentage", string.IsNullOrEmpty(GSTPercentage.Text) ? (object)DBNull.Value : Convert.ToDecimal(GSTPercentage.Text));
            cmd.Parameters.AddWithValue("@TDSPercentage", string.IsNullOrEmpty(TDSPercentage.Text) ? (object)DBNull.Value : Convert.ToDecimal(TDSPercentage.Text));
            

            this.DA.ExecuteNonQuery(cmd);

            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "toastr_redirect",
                "showToastr('success','Payable Invoice updated successfully!');" +
                "setTimeout(function(){ window.location.href = '/Admin/PayableinvoiceGrid.aspx'; }, 2000);",
                true
            );
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    private void BindClients()
    {
        string str_clients = "SELECT ClientKey, ClientName FROM IT_ClientDetails WHERE PartyType = 1 AND Status = 1";
        SqlCommand cmd = new SqlCommand(str_clients);
        DataSet ds = this.DA.GetDataSet(cmd);

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            ddlVendor.DataSource = ds.Tables[0];
            ddlVendor.DataTextField = "ClientName";
            ddlVendor.DataValueField = "ClientKey";
            ddlVendor.DataBind();
        }

        ddlVendor.Items.Insert(0, new ListItem("-- Select Client --", ""));
    }

    private void BindCurrency(string client_Id)
    {
        string str_clients = "SELECT c.CurrencyCode, c.LocalCurrencyID FROM IT_ClientDetails a INNER JOIN IT_Countries b ON a.Country = b.CountryKey INNER JOIN IT_Currency c ON b.Country = c.CountryName WHERE a.ClientKey = @ClientKey";
        SqlCommand cmd = new SqlCommand(str_clients);
        cmd.Parameters.AddWithValue("@ClientKey", client_Id);
        DataSet ds = this.DA.GetDataSet(cmd);

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DD_Currency.DataSource = ds.Tables[0];
            DD_Currency.DataTextField = "CurrencyCode";
            DD_Currency.DataValueField = "LocalCurrencyID";
            DD_Currency.DataBind();
        }

        DD_Currency.Items.Insert(0, new ListItem("-- Select Currency --", ""));
    }

    protected void Rd_Status_SelectedIndexChanged(object sender, EventArgs e)
    {
        string client_Id = ddlVendor.SelectedValue;
        string str_clients = "SELECT c.CurrencyCode, c.LocalCurrencyID FROM IT_ClientDetails a INNER JOIN IT_Countries b ON a.Country = b.CountryKey INNER JOIN IT_Currency c ON b.Country = c.CountryName WHERE a.ClientKey = @ClientKey";
        SqlCommand cmd = new SqlCommand(str_clients);
        cmd.Parameters.AddWithValue("@ClientKey", client_Id);
        DataSet ds = this.DA.GetDataSet(cmd);

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            DD_Currency.DataSource = ds.Tables[0];
            DD_Currency.DataTextField = "CurrencyCode";
            DD_Currency.DataValueField = "LocalCurrencyID";
            DD_Currency.DataBind();
        }

        DD_Currency.Items.Insert(0, new ListItem("-- Select Currency --", ""));
    }
}