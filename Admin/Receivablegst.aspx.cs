using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_gst : System.Web.UI.Page
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
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Receivable GST";

            LoadInvoiceGrid();
        }
        if (Request["__EVENTTARGET"] == "PayInvoice")
        {
            PayInvoice();
        }
    }
    private void LoadInvoiceGrid()
    {
        string str_query = @"
    SELECT 
        InvoiceKey,
        InvoiceNumber,
 CONVERT(varchar(10), InvoiceDate, 23) AS InvoiceDate, 
GSTAmount,
 CONVERT(varchar(10), GSTpaiddate, 23) AS GSTpaiddate, 
        InvoiceAmount,
        Status,
        GSTstatus
    FROM IT_Invoices
    ORDER BY CreatedOn DESC";

        SqlCommand cmd = new SqlCommand(str_query);

        DataTable dt_invoice = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_invoice);

        if (dt_invoice.Rows.Count > 0)
        {
            if (!ds.Tables[0].Columns.Contains("StatusText"))
                ds.Tables[0].Columns.Add("StatusText");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int status = Convert.ToInt32(dr["GSTstatus"]); // 0 = Unpaid, 1 = Paid
               
                if (status == 1)
                {
                    dr["StatusText"] = "<span class='label label-success'>Paid</span>";
                }
                else
                {
                    dr["StatusText"] = "<span class='label label-warning' style='cursor:pointer' " +
                        "onclick=\"fn_PayInvoice('" + dr["InvoiceKey"] + "')\">Unpaid</span>";
                }
            }
            this.PH.LoadGridItem(ds, PH_Project, "gst.txt", "");
        }
    }

    private void PayInvoice()
    {
        int invoiceKey = Convert.ToInt32(hdnInvoiceKey.Value);
        string description = txtDescription.Text.Trim();

        string str_query = @"
UPDATE IT_Invoices
SET
    GSTstatus = 1,
    GSTpaiddate = GETDATE(),
    GSTDescription = @Description,
    ModifiedOn = GETDATE()
WHERE InvoiceKey = @InvoiceKey";

        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@InvoiceKey", invoiceKey);
        cmd.Parameters.AddWithValue("@Description", description);

        DA.ExecuteNonQuery(cmd);

        // Optional: clear textbox after save
        txtDescription.Text = string.Empty;

        LoadInvoiceGrid();
    }

}