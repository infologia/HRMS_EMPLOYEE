using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_tds : System.Web.UI.Page
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
                control1.Text = "Receivable TDS";
            LoadTds();
        }
    }

    private void LoadTds()
    {
        string str_query = @"SELECT 
        c.ClientName,
        i.InvoiceNumber,
        i.InvoiceAmount,
        i.TDSAmount,
        i.TotalAmount,
        i.Status,
        CONVERT(VARCHAR(10), i.InvoiceDate, 103) AS InvoiceDate
    FROM IT_Invoices i
    INNER JOIN IT_ClientDetails c ON i.ClientKey = c.ClientKey
    WHERE i.TDSAmount > 0
    ORDER BY i.InvoiceDate DESC;";

        using (SqlCommand cmd = new SqlCommand(str_query))
        {
            DataTable dtTds = DA.GetDataTable(cmd);

            DataSet ds = new DataSet();
            ds.Tables.Add(dtTds);

            // 🔹 Add StatusText column for Paid / Unpaid
            if (!ds.Tables[0].Columns.Contains("StatusText"))
                ds.Tables[0].Columns.Add("StatusText");

            // 🔹 Status logic (Paid / Unpaid)
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int status = Convert.ToInt32(dr["Status"]);

                if (status == 0) // Pending
                    dr["StatusText"] = "<span class='label label-danger'>Pending</span>";
                else // Received
                    dr["StatusText"] = "<span class='label label-success'>Received</span>";
            }

            // 🔹 Load to grid
            PH.LoadGridItem(ds, PH_TDSInvoices, "tds.txt", "");
        }
    }

}