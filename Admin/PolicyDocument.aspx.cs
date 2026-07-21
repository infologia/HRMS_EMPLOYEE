using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_PolicyDocument : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    string str_key = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();


        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Policy Documents";

           
        }

        if (!IsPostBack)
        {
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                int id = Convert.ToInt32(Request.QueryString["id"]);

                this.delete();
            }

        }

        string str_docu = "select PolicyDocumentkey,DocumentName,PolicyDocument,Createdby,Status,CONVERT(Varchar,CreatedOn,103)as createdOn from IT_PolicyDocument";
        SqlCommand cmd = new SqlCommand(str_docu);

        DataTable dt_document = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_document);
        if (dt_document.Rows.Count > 0)
        {
            if (ds.Tables[0].Columns.Contains("Status"))
                ds.Tables[0].Columns.Add("ActiveText");
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int activetype = Convert.ToInt16(dr["Status"].ToString());
                if (activetype == 1)
                    dr["ActiveText"] = "<span class='label label-sm label-success'>Active</span>";
                else if (activetype == 2)
                    dr["ActiveText"] = "<span class='label label-sm label-danger'>InActive</span>";
            }

            this.PH.LoadGridItem(ds, PH_DocumentView, "PolicyDocument.txt", "");

        }

    }

    private void delete()
    {
        string str_query = "DELETE FROM IT_PolicyDocument WHERE PolicyDocumentKey = @Id";
        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@Id", Convert.ToInt32(Request.QueryString["id"]));
        DA.ExecuteNonQuery(cmd);

    }
}