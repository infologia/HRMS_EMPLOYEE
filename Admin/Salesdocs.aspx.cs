using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Salesdocs : System.Web.UI.Page
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

        if (Request.QueryString["delFile"] != null)
        {
            string fileName = Request.QueryString["delFile"].ToString();

            SqlCommand cmd = new SqlCommand(
                "DELETE FROM IT_SalesDocuments WHERE FileName = @FileName");
            cmd.Parameters.AddWithValue("@FileName", fileName);
            DA.ExecuteNonQuery(cmd);
            string path = Server.MapPath("~/Document/Uploads/SalesDocs/" + fileName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            Response.Redirect("Salesdocs.aspx");
        }

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
        string query = @"SELECT SD.FileTitle, ER.Username AS CreatedBy, SD.CreatedOn, FileName
                     FROM IT_SalesDocuments SD LEFT JOIN IT_EmployeeRegister ER ON SD.CreatedBy = ER.Employeekey
                     ORDER BY SD.CreatedOn DESC";

        SqlCommand cmd = new SqlCommand(query);
        DataTable dt_docs = DA.GetDataTable(cmd);

        if (dt_docs.Rows.Count > 0)
        {
            DataSet ds = new DataSet();
            ds.Merge(dt_docs);

            ds.Tables[0].Columns.Add("ViewDocument");
            ds.Tables[0].Columns.Add("Download");
            ds.Tables[0].Columns.Add("Delete");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string fileName = dr["FileName"].ToString();
                string filePath = "../Document/Uploads/SalesDocs/" + fileName;
                dr["ViewDocument"] = "<a href='" + filePath + "' target='_blank'><button type='button' class='label label-info'>View</button></a>";
                dr["Download"] = "<a href='" + filePath + "' download><button type='button' class='label label-sm label-success'>Download</button></a>";
                dr["Delete"] =
                        "<a href='javascript:void(0);' " +
                        "onclick=\"deleteDoc('" + fileName + "')\" " +
                        "class='label label-danger'>Delete</a>";
            }
            this.PH.LoadGridItem(ds, PH_SalesDocs, "SalesDocs.txt", "");
        }
        
    }

}