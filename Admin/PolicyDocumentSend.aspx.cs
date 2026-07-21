using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IdentityModel.Protocols.WSTrust;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_PolicyDocumentSend : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_userkey = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.str_userkey = SC.Userid;


        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Policy Documents";

        }
        if (Request.QueryString["id"] != null)
        {
            int id;
            if (int.TryParse(Request.QueryString["id"], out id))
            {
                if (!Page.IsPostBack)
                {
                    this.loadedit(id);
                    btn_send.Text = "Update";
                    create.Text = "Update Policy Documents";
                }
            }
        }
        else
        {
            if (!Page.IsPostBack)
            {
                rblStatus.SelectedValue = "1";
                btn_send.Text = "Submit";
                create.Text = "Create Policy Documents";
            }
        }

    }

    private void loadedit(int id)
    {


        string str_query = "select PolicyDocument,DocumentName,CreatedOn,Status from IT_PolicyDocument where PolicyDocumentkey='" + id + "'" ;

        SqlCommand cmd = new SqlCommand(str_query);
       
        DataTable dt_preview = DA.GetDataTable(cmd);

        if (dt_preview.Rows.Count > 0)
        {

            txt_letter.Text = dt_preview.Rows[0]["DocumentName"].ToString();
            string docPath = dt_preview.Rows[0]["PolicyDocument"].ToString();

            if (!string.IsNullOrEmpty(docPath))
            {
                pnlOldDoc.Visible = true;

                lnkViewDoc.NavigateUrl = ResolveUrl("~/Document/" + docPath);
                lnkViewDoc.Text = "View Document";
                hdnOldDocument.Value = docPath;
            }
        }
        string status = dt_preview.Rows[0]["Status"].ToString();
        if (rblStatus.Items.FindByValue(status) != null)
        {
            rblStatus.SelectedValue = status; 
        }


    }

    protected void btn_send_Click(object sender, EventArgs e)
    {
        SqlCommand cmd;
        string query;
        bool isEdit = Request.QueryString["id"] != null;
        int id = Convert.ToInt32(Request.QueryString["id"]);
        string fileName = "";
        if (up_document.HasFile)
        {
            string ext = Path.GetExtension(up_document.FileName);
            fileName = Guid.NewGuid().ToString() + ext;
            string path = Server.MapPath("~/Document");
            up_document.SaveAs(path + fileName);
        }

        if (isEdit)
        {
            id = Convert.ToInt32(Request.QueryString["id"]);
            query = @"update IT_PolicyDocument SET PolicyDocument=@PolicyDocument,DocumentName=@DocumentName,Status=@Status,Modifiedon=@Modifiedon,Modifiedby=@Modifiedby Where PolicyDocumentkey='"+id+"'" ;
            cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@Modifiedby", str_userkey);
            cmd.Parameters.AddWithValue("@Modifiedon", DateTime.Now);
            cmd.Parameters.AddWithValue("@PolicyDocumentkey", id);

        }
        else
        {
            query = @"insert into IT_PolicyDocument(PolicyDocument, DocumentName, Status, createdby)values(@PolicyDocument, @DocumentName, @Status, @createdby)";
            cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@Createdby", str_userkey);

            
        }


        string str_userky = Guid.NewGuid().ToString();
        string filename = Path.GetFileName(up_document.FileName);
        string extension = Path.GetExtension(filename);
        string str_newid = str_userky + extension;
        string str_path = Server.MapPath("~/Document/") + str_newid;
        up_document.SaveAs(str_path);
        //status
        
       
       

        
        cmd.Parameters.AddWithValue("@PolicyDocument", str_newid);
        cmd.Parameters.AddWithValue("@DocumentName", txt_letter.Text);
        cmd.Parameters.AddWithValue("@Status", Convert.ToInt32(rblStatus.SelectedValue));


        DA.ExecuteNonQuery(cmd);
        Response.Redirect("~/Admin/PolicyDocument.aspx");

    }

   
}