using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_Salesdocs : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    CommonFunction CF;
    PhTemplate PH;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();

        if (!IsPostBack)
        {
            Label lbl = this.Master.FindControl("lbl_bread") as Label;
            if (lbl != null)
                lbl.Text = "Sales Documents";

            LoadSalesDocs();

        }
    }

    private void LoadSalesDocs()
    {
        string query = @"SELECT a.filename,a.SalesDocumentsKey, a.FileTitle,a.CreatedOn,a.ModifiedOn,b.username,a.Description FROM IT_SalesDocuments a left join IT_EmployeeRegister b on a.CreatedBy=b.Employeekey ORDER BY CreatedOn DESC";

        SqlCommand cmd = new SqlCommand(query);
        DataTable dt_docs = DA.GetDataTable(cmd);

        if (dt_docs.Rows.Count > 0)
        {
            DataSet ds = new DataSet();
            ds.Merge(dt_docs);
            ds.Tables[0].Columns.Add("ViewDocument");
            ds.Tables[0].Columns.Add("Download");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string fileName = dr["FileName"].ToString();
                string filePath = ResolveUrl("../Document/Uploads/SalesDocs/" + fileName);

                dr["ViewDocument"] = "<a href='" + filePath + "' target='_blank'><button type='button' class='label label-info'>View</button></a>";
                dr["Download"] = "<a href='" + filePath + "' download><button type='button' class='label label-sm label-success'>Download</button></a>";
            }

            PH.LoadGridItem(ds, PH_SalesDocs, "SalesDocsemployee.txt", "");
        }
        else
        {
            PH_SalesDocs.Controls.Clear();
            PH_SalesDocs.Controls.Add(new LiteralControl("<tr><td colspan='4'>No documents found</td></tr>"));
        }
    }

}