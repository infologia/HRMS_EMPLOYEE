using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WEB_EditDocument : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_key = "";
    string str_userkey = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.str_userkey= this.SC.Userid;


        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Document & Maintanence";
        }

        if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
        {

            this.str_key = Request.QueryString["id"].ToString();
        }

        if (!IsPostBack)
        {
            this.loadid();
            this.loadedit();
        }

    }
    private void loadedit()
    {

        string str_query = "select Username,Employeekey,Document,DocumentName,Createdon from IT_Document where Documentkey=@Documentkey ";

        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@Documentkey", str_key);
        DataTable dt_preview = DA.GetDataTable(cmd);

        if (dt_preview.Rows.Count > 0)
        {

            ddl_id.SelectedValue = dt_preview.Rows[0]["Employeekey"].ToString();
            txt_user.Text = dt_preview.Rows[0]["Username"].ToString();
            txt_user.Attributes.Add("readonly", "readonly");
            txt_letter.Text = dt_preview.Rows[0]["DocumentName"].ToString();
            string docPath = dt_preview.Rows[0]["Document"].ToString();

            if (!string.IsNullOrEmpty(docPath))
            {
                pnlOldDoc.Visible = true;

                lnkViewDoc.NavigateUrl = ResolveUrl("~/Document/" + docPath);
                lnkViewDoc.Text = "View Document";
                hdnOldDocument.Value = docPath;
            }
        }


    }

    protected void btn_edit_Click(object sender, EventArgs e)
    {
        string str_newid = hdnOldDocument.Value;
        if (up_document.HasFile)
        {

            string filename = Path.GetFileName(up_document.FileName);
            string extension = Path.GetExtension(filename);
            str_newid = str_key + extension;
            string str_path = Server.MapPath("~/Document/") + str_newid;
            up_document.SaveAs(str_path);
        }

        string date = DateTime.Now.ToString();
        string str_Sql = "update IT_Document SET Employeekey=@Employeekey,Username=@Username,Document=@Document,DocumentName=@DocumentName,Modifiedon=@Modifiedon,Modifiedby=@Modifiedby Where Documentkey=@Documentkey";
        SqlCommand cmd = new SqlCommand(str_Sql);
        cmd.Parameters.AddWithValue("@Username", txt_user.Text);
        cmd.Parameters.AddWithValue("@Documentkey", str_key);
        cmd.Parameters.AddWithValue("@Document", str_newid);
        cmd.Parameters.AddWithValue("@DocumentName", txt_letter.Text);
        cmd.Parameters.AddWithValue("@Employeekey", ddl_id.SelectedValue);
        cmd.Parameters.AddWithValue("@Modifiedby", str_userkey);
        cmd.Parameters.Add("@Modifiedon", SqlDbType.DateTime).Value = DateTime.Now;
        DA.ExecuteNonQuery(cmd);
        Response.Redirect("~/Admin/Documents.aspx");
    }
    private void loadid()
    {
        string str_id = "select Employeeid,Employeekey from IT_EmployeeRegister where Employeestatus = '1' and Division in (1,2,3)";
        SqlCommand cmd = new SqlCommand(str_id);
        DataSet ds = this.DA.GetDataSet(cmd);
        if (ds != null && ds.Tables.Count > 0)
        {
            ddl_id.DataSource = ds.Tables[0];
            ddl_id.DataTextField = "Employeeid";
            ddl_id.DataValueField = "Employeekey";
            ddl_id.DataBind();
            ddl_id.Items.Add(new ListItem("Select  Employeeid ", "0"));
            ddl_id.SelectedValue = "0";
        }
    }    
    protected void ddl_id_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str_empdatils = "select Username from IT_EmployeeRegister where Employeekey='"+str_key+"'";
        SqlCommand cmd = new SqlCommand(str_empdatils);
        cmd.Parameters.AddWithValue("@Employeekey", ddl_id.SelectedValue);
        DataTable dt_empdetails = DA.GetDataTable(cmd);
        if (dt_empdetails.Rows.Count > 0)
        {
            txt_user.Text = dt_empdetails.Rows[0]["Username"].ToString();
            txt_user.Attributes.Add("readonly", "readonly");
        }
    }
}
