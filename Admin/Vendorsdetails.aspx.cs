using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
public partial class Admin_Vendorsdetails : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;

    private string key = "";
    string str_id = "";
    string Vendorkey;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();

        Label control1 = this.Master.FindControl("lbl_bread") as Label;
        if (control1 != null)
            control1.Text = "Vendors";

        // Always check if QueryString has id
        if (!string.IsNullOrEmpty(Request.QueryString["id"]))
        {
            this.str_id = Request.QueryString["id"].ToString();
            if (!IsPostBack)
            {
                Loadcountry();
               
                assignvalues();
            }

            btn_update.Visible = true;
            btn_request.Visible = false;
        }
        else
        {
            if (!IsPostBack)
            {
                Loadcountry();
                
            }

            btn_request.Visible = true;
            btn_update.Visible = false;
        }
    }
    private void Loadcountry()
    {
        string str_lead = "select CountryKey,Country from it_countries";

        {
            SqlCommand cmd = new SqlCommand(str_lead);
            DataSet reader = this.DA.GetDataSet(cmd);
            ddl_Country.DataSource = reader;
            ddl_Country.DataTextField = "Country";
            ddl_Country.DataValueField = "CountryKey";
            ddl_Country.DataBind();
            ddl_Country.Items.Insert(0, new ListItem("-- Select Country --", ""));
        }
    }
   

    protected void btn_request_Click(object sender, EventArgs e)
    {
        Guid userId = new Guid(SC.Userid.ToString());

        SqlCommand cmd = new SqlCommand("INSERT INTO IT_Vendors (VendorCode,VendorName,ContactPerson,Email,Mobile,GSTNumber,PANNumber,Address,Country,BankName,AccountNumber,IFSCCode,PaymentTerms,Status,Remarks,CreatedBy) VALUES (@VendorCode,@VendorName,@ContactPerson,@Email,@Mobile,@GSTNumber,@PANNumber,@Address,@Country,@BankName,@BankName,@IFSCCode,@PaymentTerms,@Status,@Remarks,@CreatedBy)");
        cmd.Parameters.AddWithValue("@VendorCode", txt_VendorCode.Text.Trim());
        cmd.Parameters.AddWithValue("@VendorName", txt_VendorName.Text.Trim());
        cmd.Parameters.AddWithValue("@ContactPerson", txt_ContactPerson.Text.Trim());
        cmd.Parameters.AddWithValue("@Email", txt_email.Text.Trim());
        cmd.Parameters.AddWithValue("@Mobile", txt_mobile.Text.Trim());
        cmd.Parameters.AddWithValue("@GSTNumber", txt_gst.Text.Trim());
        cmd.Parameters.AddWithValue("@PANNumber", txt_pan.Text.Trim());
        cmd.Parameters.AddWithValue("@Address", txt_Address.Text.Trim());
        cmd.Parameters.AddWithValue("@Country", ddl_Country.SelectedValue);
        cmd.Parameters.AddWithValue("@BankName", txt_bank.Text.Trim());
        cmd.Parameters.AddWithValue("@AccountNumber", txt_Accountno.Text.Trim());
        cmd.Parameters.AddWithValue("@IFSCCode", txt_ifsc.Text.Trim());
        cmd.Parameters.AddWithValue("@PaymentTerms", txt_payment.Text.Trim());
        cmd.Parameters.AddWithValue("@Status", ddl_Clientstatus.SelectedValue);
        cmd.Parameters.AddWithValue("@Remarks", txt_remarks.InnerText);
        cmd.Parameters.Add("@CreatedBy", SqlDbType.UniqueIdentifier).Value = userId;
        //cmd.Parameters.Add("@VendorKey", SqlDbType.UniqueIdentifier).Value = this.str_id;

        DA.ExecuteNonQuery(cmd);
        Response.Redirect("Vendor.aspx");
        ClearForm();
    }
    private void ClearForm()
    {
        
    }

    public void assignvalues()
    {
        

        string str_assign = "SELECT * FROM IT_Vendors WHERE VendorKey = @VendorKey";
        SqlCommand cmd = new SqlCommand(str_assign);
        cmd.Parameters.AddWithValue("@VendorKey", this.str_id);

        DataTable dt_vendor = this.DA.GetDataTable(cmd);

        if (dt_vendor.Rows.Count > 0)
        {
            txt_VendorCode.Text = dt_vendor.Rows[0]["VendorCode"].ToString();
            txt_VendorName.Text = dt_vendor.Rows[0]["VendorName"].ToString();
            txt_ContactPerson.Text = dt_vendor.Rows[0]["ContactPerson"].ToString();
            txt_email.Text = dt_vendor.Rows[0]["Email"].ToString();
            txt_mobile.Text = dt_vendor.Rows[0]["Mobile"].ToString();
            txt_gst.Text = dt_vendor.Rows[0]["GSTNumber"].ToString();
            txt_pan.Text = dt_vendor.Rows[0]["PANNumber"].ToString();
            txt_Address.Text = dt_vendor.Rows[0]["Address"].ToString();
            ddl_Country.SelectedValue = dt_vendor.Rows[0]["Country"].ToString();
            txt_bank.Text = dt_vendor.Rows[0]["BankName"].ToString();
            txt_Accountno.Text = dt_vendor.Rows[0]["AccountNumber"].ToString();
            txt_ifsc.Text = dt_vendor.Rows[0]["IFSCCode"].ToString();
            txt_payment.Text = dt_vendor.Rows[0]["PaymentTerms"].ToString();
            ddl_Clientstatus.SelectedValue = dt_vendor.Rows[0]["Status"].ToString();
            txt_remarks.InnerText = dt_vendor.Rows[0]["Remarks"].ToString();
            
        }

    }
    protected void btn_update_Click(object sender, EventArgs e)
    {
        Guid userId = new Guid(SC.Userid.ToString());



        SqlCommand cmd = new SqlCommand("UPDATE IT_Vendors SET VendorCode=@VendorCode,VendorName=@VendorName,ContactPerson=@ContactPerson,Email=@Email,Mobile=@Mobile,GSTNumber=@GSTNumber,PANNumber=@PANNumber,Address=@Address,Country=@Country,BankName=@BankName,AccountNumber=@AccountNumber,IFSCCode=@IFSCCode,PaymentTerms=@PaymentTerms,Status=@Status,Remarks=@Remarks,ModifiedBy=@ModifiedBy,ModifiedOn=GETDATE() WHERE VendorKey=@VendorKey");

        cmd.Parameters.AddWithValue("@VendorCode", txt_VendorCode.Text.Trim());
        cmd.Parameters.AddWithValue("@VendorName", txt_VendorName.Text.Trim());
        cmd.Parameters.AddWithValue("@ContactPerson", txt_ContactPerson.Text.Trim());
        cmd.Parameters.AddWithValue("@Email", txt_email.Text.Trim());
        cmd.Parameters.AddWithValue("@Mobile", txt_mobile.Text.Trim());
        cmd.Parameters.AddWithValue("@GSTNumber", txt_gst.Text.Trim());
        cmd.Parameters.AddWithValue("@PANNumber", txt_pan.Text.Trim());
        cmd.Parameters.AddWithValue("@Address", txt_Address.Text.Trim());
        cmd.Parameters.AddWithValue("@Country", ddl_Country.SelectedValue);
        cmd.Parameters.AddWithValue("@BankName", txt_bank.Text.Trim());
        cmd.Parameters.AddWithValue("@AccountNumber", txt_Accountno.Text.Trim());
        cmd.Parameters.AddWithValue("@IFSCCode", txt_ifsc.Text.Trim());
        cmd.Parameters.AddWithValue("@PaymentTerms", txt_payment.Text.Trim());
        cmd.Parameters.AddWithValue("@Status", ddl_Clientstatus.SelectedValue);
        cmd.Parameters.AddWithValue("@Remarks", txt_remarks.InnerText);
        cmd.Parameters.Add("@ModifiedBy", SqlDbType.UniqueIdentifier).Value = userId;
        cmd.Parameters.AddWithValue("@VendorKey", this.str_id);
        


        DA.ExecuteNonQuery(cmd);
        Response.Redirect("Vendor.aspx");
    }
}