using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class WEB_Admin_Mailtemplateedit : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_id = "";
    string str_userid = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();


        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Document & Maintanence";

            //HtmlAnchor control = this.Master.FindControl("li_dashboard") as HtmlAnchor;
            //if (control != null)
            //    control.Attributes.Add("class", "active");
        }

        if (Request.QueryString["key"] != null && Request.QueryString["key"] != "")
        {

            this.str_id = Request.QueryString["key"].ToString();
        }
     
        str_userid = this.SC.Userid;

        string str_query = "select Header,MailSubject,Content,Image,Footer from IT_Mailtemplate where Mailtemplatekey=@Mailtemplatekey ";

        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@Mailtemplatekey", str_id);
        DataTable dt_preview = DA.GetDataTable(cmd);

        if (dt_preview.Rows.Count > 0)
        {


            if (Convert.ToString(dt_preview.Rows[0]["image"]) != "")
                Img_1.ImageUrl = "~/images/" + dt_preview.Rows[0]["image"];
            else
                Img_1.ImageUrl = "~/images/Koala.jpg";

            txt_header.Text = dt_preview.Rows[0]["Header"].ToString();
        txt_subject.Text= dt_preview.Rows[0]["MailSubject"].ToString();


        txt_content.Text = dt_preview.Rows[0]["Content"].ToString();
        txt_footer.Text = dt_preview.Rows[0]["Footer"].ToString();

        }

    }
  
    protected void btn_edit_Click(object sender, EventArgs e)
    {
        try
        {


            //this.str_userid = Guid.NewGuid().ToString();
            string filename = Path.GetFileName(up_image.FileName);
            string extension = Path.GetExtension(filename);
            string str_newid = str_id + extension;
            string str_path = Server.MapPath("~/images/") + str_newid;
            up_image.SaveAs(str_path);

            string date = DateTime.Now.ToString();
            string str_Sql = "update IT_Mailtemplate SET Header=@Header,MailSubject=@MailSubject,Content=@Content,Image=@Image,Footer=@Footer,modifiedon=@modifiedon,modifiedby=@modifiedby where Mailtemplatekey=@Mailtemplatekey";
            SqlCommand cmd = new SqlCommand(str_Sql);

            cmd.Parameters.AddWithValue("@Mailtemplatekey", str_id);

            cmd.Parameters.AddWithValue("@Header", txt_header.Text);
            cmd.Parameters.AddWithValue("@MailSubject", txt_subject.Text.Trim());
            cmd.Parameters.AddWithValue("@Content", txt_content.Text);
            cmd.Parameters.AddWithValue("@Image", str_newid);
        
            cmd.Parameters.AddWithValue("@Footer", txt_footer.Text.Trim());
            cmd.Parameters.AddWithValue("@Modifiedby", this.str_userid);
            cmd.Parameters.AddWithValue("@Modifiedon", date);
            DA.ExecuteNonQuery(cmd);
        }
        catch (Exception ex)
        {
        }
    }
}