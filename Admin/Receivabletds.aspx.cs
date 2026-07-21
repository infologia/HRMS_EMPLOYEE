using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Receivabletds : System.Web.UI.Page
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
                control1.Text = "Payable TDS";
            LoadTds();
        }

    }

    private void LoadTds()
    {
        string str_query = @"SELECT
        c.VendorName,
        i.InvoiceNumber,
        i.InvoiceAmount,
        i.TDSAmount,
        i.TotalPayableAmount,
        i.PaymentStatus,
        CONVERT(VARCHAR(10), i.InvoiceDate, 103) AS InvoiceDate
    FROM IT_PayableInvoices i
    INNER JOIN IT_Vendors c
        ON i.VendorName = c.VendorKey
    WHERE i.TDSAmount > 0
    ORDER BY i.InvoiceDate DESC;
    ";

        using (SqlCommand cmd = new SqlCommand(str_query))
        {
            DataTable dtTds = DA.GetDataTable(cmd);

            DataSet ds = new DataSet();
            ds.Tables.Add(dtTds);

            // 🔹 Add PaymentStatusText column
            if (!ds.Tables[0].Columns.Contains("PaymentStatusText"))
                ds.Tables[0].Columns.Add("PaymentStatusText");

            // 🔹 Status mapping: 0 = Pending, 1 = Completed
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int paymentStatus = Convert.ToInt32(dr["PaymentStatus"]);

                if (paymentStatus == 1)
                    dr["PaymentStatusText"] =
                        "<span class='label label-success'>Completed</span>";
                else
                    dr["PaymentStatusText"] =
                        "<span class='label label-warning'>Pending</span>";
            }

            // 🔹 Load grid
            PH.LoadGridItem(ds, PH_TDSInvoices, "receivabletds.txt", "");
        }
    }



}