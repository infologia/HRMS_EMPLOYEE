using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class WEB_Admin_Adminmailtemplate : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string userid = "";
    string str_userid = "";
  
 
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        string date = DateTime.Now.ToString();
        userid = this.SC.Userid;
        str_userid = Guid.NewGuid().ToString();


        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Document & Maintanence";

            //HtmlAnchor control = this.Master.FindControl("li_dashboard") as HtmlAnchor;
            //if (control != null)
            //    control.Attributes.Add("class", "active");
        }

    }
    protected void btn_sendto_Click(object sender, EventArgs e)
    {
        try
        {

            this.str_userid = Guid.NewGuid().ToString();
            string filename = Path.GetFileName(up_image.FileName);
            string extension = Path.GetExtension(filename);
            string str_newid = str_userid + extension;
            string str_path = Server.MapPath("~/images/") + str_newid;
            up_image.SaveAs(str_path);

            int status = 1;
            string str_Sql = ("insert into IT_Mailtemplate(Mailtemplatekey,Header,MailSubject,Content,Image,Footer,Createdby,status)values(@Mailtemplatekey,@Header,@Subject,@Content,@Image,@Footer,@Createdby,@status)");
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@Mailtemplatekey", str_userid);
            cmd.Parameters.AddWithValue("@Header", txt_header.Text);
            cmd.Parameters.AddWithValue("@Subject", txt_subject.Text);
            cmd.Parameters.AddWithValue("@Content", txt_content.Text);
            cmd.Parameters.AddWithValue("@Image", str_newid);
            cmd.Parameters.AddWithValue("@Footer", txt_footer.Text);
            cmd.Parameters.AddWithValue("@Createdby", userid);
            cmd.Parameters.AddWithValue("@status", status);

            DA.ExecuteNonQuery(cmd);
            Response.Redirect(@"~/Admin/mailtemplatesend.aspx?key=" + this.str_userid + "");
          
        }
        catch (Exception ex)
        {
        }
    }
    protected void btn_preview_Click(object sender, EventArgs e)
    {
        try
        {
            this.str_userid = Guid.NewGuid().ToString();
            string filename = Path.GetFileName(up_image.FileName);
            string extension = Path.GetExtension(filename);
            string str_newid = str_userid + extension;
            string str_path = Server.MapPath("~/images/") + str_newid;
            up_image.SaveAs(str_path);

            int status = 1;
            string str_Sql = "insert into IT_Mailtemplate(Mailtemplatekey,Header,MailSubject,Content,Image,Footer,Createdby,Status)values(@Mailtemplatekey,@Header,@Subject,@Content,@Image,@Footer,@Createdby,@Status)";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@Mailtemplatekey",str_userid);
            cmd.Parameters.AddWithValue("@Header", txt_header.Text);
            cmd.Parameters.AddWithValue("@Subject", txt_subject.Text);
            cmd.Parameters.AddWithValue("@Content", txt_content.Text);
            cmd.Parameters.AddWithValue("@Image",str_newid);
            cmd.Parameters.AddWithValue("@Footer", txt_footer.Text);
            cmd.Parameters.AddWithValue("@Createdby",userid);
             cmd.Parameters.AddWithValue("@Status", status);
       

            DA.ExecuteNonQuery(cmd);
            Response.Redirect(@"~//Admin/mailtemplatepreview.aspx?key=" + this.str_userid + "");
        }
        catch (Exception ex)
        {
        }
    }
}
