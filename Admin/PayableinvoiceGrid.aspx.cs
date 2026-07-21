using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

public partial class Admin_PayableinvoiceGrid : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();
        if (!IsPostBack)
        {
            BindVendorDropdown();
            BindGrid();
            LoadTotalAmount();
            Label lblBread = Master.FindControl("lbl_bread") as Label;
            lblBread.Text = "Payable Invoices";

        }
    }

    private void BindVendorDropdown()
    {
        string query = "SELECT ClientKey, ClientName FROM IT_ClientDetails WHERE PartyType = 1 AND Status = 1 ORDER BY ClientName";
        DataTable dt = DA.GetDataTable(new SqlCommand(query));

        ddlVendor.Items.Clear();
        ddlVendor.Items.Add(new ListItem("All Vendors", "0"));

        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow dr in dt.Rows)
                ddlVendor.Items.Add(new ListItem(dr["ClientName"].ToString(), dr["ClientKey"].ToString()));
        }
    }

    protected void ddlVendor_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindGrid();
        LoadTotalAmount();
    }

    private void LoadTotalAmount()
    {
        string vendorFilter = ddlVendor.SelectedValue != "0" ? " AND a.VendorNameNew = @VendorKey" : "";
        string query = "SELECT ISNULL(SUM(a.InvoiceAmount), 0) AS TotalAmount FROM IT_PayableInvoices a WHERE 1=1" + vendorFilter;
        SqlCommand cmd = new SqlCommand(query);
        if (ddlVendor.SelectedValue != "0")
            cmd.Parameters.AddWithValue("@VendorKey", ddlVendor.SelectedValue);

        DataTable dt = DA.GetDataTable(cmd);
        if (dt.Rows.Count > 0)
            lblTotalAmount.Text = Convert.ToDecimal(dt.Rows[0]["TotalAmount"]).ToString("0.00");
    }

    private void BindGrid()
    {
        string vendorFilter = ddlVendor.SelectedValue != "0" ? " AND a.VendorNameNew = @VendorKey" : "";

        string query1 = @"SELECT a.PayableInvoiceKey, b.ClientName AS VendorName, a.InvoiceNumber, a.InvoiceDate, a.DueDate, a.InvoiceAmount, a.PaymentStatus, a.CreatedOn
                          FROM IT_PayableInvoices a
                          LEFT JOIN IT_ClientDetails b ON a.VendorNameNew = b.ClientKey
                          WHERE 1=1" + vendorFilter + " ORDER BY a.CreatedOn DESC";
        SqlCommand cmd1 = new SqlCommand(query1);
        if (ddlVendor.SelectedValue != "0")
            cmd1.Parameters.AddWithValue("@VendorKey", ddlVendor.SelectedValue);

        DataTable dt_dashboard = DA.GetDataTable(cmd1);
        DataSet ds = new DataSet();
        ds.Merge(dt_dashboard);

        if (dt_dashboard.Rows.Count > 0)
        {
            ds.Tables[0].Columns.Add("ActiveText");
            ds.Tables[0].Columns.Add("Company_Name");
            ds.Tables[0].Columns.Add("Invoice_Number");
            ds.Tables[0].Columns.Add("Invoice_Date");
            ds.Tables[0].Columns.Add("Due_Date");
            ds.Tables[0].Columns.Add("Created_Date");
            ds.Tables[0].Columns.Add("Remove");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string str_Status = dr["PaymentStatus"].ToString();
                string InvoiceKey = dr["PayableInvoiceKey"].ToString();

                dr["Company_Name"] = dr["VendorName"].ToString();
                dr["Invoice_Number"] = dr["InvoiceNumber"].ToString();
                dr["Invoice_Date"] = dr["InvoiceDate"] != DBNull.Value ? Convert.ToDateTime(dr["InvoiceDate"]).ToString("yyyy-MM-dd") : "";
                dr["Due_Date"] = dr["DueDate"] != DBNull.Value ? Convert.ToDateTime(dr["DueDate"]).ToString("yyyy-MM-dd") : "";
                dr["Created_Date"] = dr["CreatedOn"] != DBNull.Value ? Convert.ToDateTime(dr["CreatedOn"]).ToString("yyyy-MM-dd") : "";

                if (str_Status == "1" || str_Status.ToLower() == "paid" || str_Status.ToLower() == "completed")
                {
                    dr["ActiveText"] = "<span class='label label-success'>Completed</span>";
                    dr["Remove"] = "<td><span class='label label-default' style='cursor:not-allowed; opacity:0.6;'>Remove</span></td>";
                }
                else
                {
                    dr["ActiveText"] = "<span class='label label-sm label-warning'>Pending</span>";
                    dr["Remove"] = "<td><a href='javascript:void(0);'><span class='label label-danger' style='cursor:pointer;' onclick=\"fn_DeleteProject('" + InvoiceKey + "')\">Remove</span></a></td>";
                }
            }

            this.PH.LoadGridItem(ds, PH_PAYABLEINVOICE, "Payableinvoice.txt", "");
        }
    }

    [WebMethod]
    public static string DeleteProject(string str_InvoiceKey)
    {
        string str_Response = "0";
        try
        {
            DataAccess DA1 = new DataAccess();
            SaveQuery SAQ = new SaveQuery();
            SessionCustom SC = new SessionCustom();
            string str_Sql = "delete from IT_PayableInvoices where PayableInvoiceKey=@InvoiceKey";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@InvoiceKey", str_InvoiceKey);
            DA1.ExecuteNonQuery(cmd);

           


            return str_Response = "1";

        }
        catch (Exception ex)
        {
            return str_Response;
        }
    }
}