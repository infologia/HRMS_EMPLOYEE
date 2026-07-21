using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WEB_Documents : System.Web.UI.Page
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
                control1.Text = "Document & Maintanence";

            //HtmlAnchor control = this.Master.FindControl("li_dashboard") as HtmlAnchor;
            //if (control != null)
            //    control.Attributes.Add("class", "active");
        }

        if (!IsPostBack)
        {
            if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            {
                this.str_key = Request.QueryString["id"].ToString();
                this.delete();


            }

        }

        string str_docu = "select b.employeeid,b.Firstname+' '+b.lastname as name,a.DocumentName,a.Document,a.Documentkey,CONVERT(Varchar,a.Createdon,103)as createdon,a.Username from IT_Document a  inner join IT_EmployeeRegister b   on a.Employeekey =b.Employeekey where roles='1' ";
        SqlCommand cmd = new SqlCommand(str_docu);

        DataTable dt_document = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_document);
        if (dt_document.Rows.Count > 0)
        {

            this.PH.LoadGridItem(ds, PH_DocumentView, "DocumentView.txt", "");

        }

    }

    private void delete()
    {
        string str_query = "Delete from IT_Document where Documentkey=@Documentkey";
        SqlCommand cmd = new SqlCommand(str_query);

        cmd.Parameters.AddWithValue("@Documentkey", str_key);
        DA.ExecuteNonQuery(cmd);
    }

}