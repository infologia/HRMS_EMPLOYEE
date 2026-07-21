using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;

public partial class WEB_Admin_Mailtemplatesend : System.Web.UI.Page
{
    DataAccess DA;
    CommonFunction CF;
    string str_id="";
    string str_img="";
    string str_head="";
      string str_sub="";
      string str_content="";
      string str_footer="";
    protected void Page_Load(object sender, EventArgs e)
    {

        this.DA = new DataAccess();
        this.CF = new CommonFunction();


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


        if (!IsPostBack)
        {
            this.grid();
           
        }
        this.mail();
    }
    public void grid()
    {
        DataTable DT_GRIDDATA = new DataTable();
        SqlCommand cmd = new SqlCommand("select Employeeid,firstname+lastname as UserName,Email,Phonenumber from IT_EmployeeRegister where roles='1'");
        DT_GRIDDATA = DA.GetDataTable(cmd);
        gvEmp.DataSource = DT_GRIDDATA;
        gvEmp.DataBind();

    }
    public void mail()
    {
       
        string str_query = "select Header,MailSubject,Content,Image,Footer from IT_Mailtemplate where Mailtemplatekey=@Mailtemplatekey ";

        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@Mailtemplatekey", str_id);
        DataTable dt_preview = DA.GetDataTable(cmd);

        if (dt_preview.Rows.Count > 0)
        {


            //if (Convert.ToString(dt_preview.Rows[0]["image"]) != "")
        this.str_img =dt_preview.Rows[0]["image"].ToString();
           

          this.str_head = dt_preview.Rows[0]["Header"].ToString();
        this.str_sub= dt_preview.Rows[0]["MailSubject"].ToString();


        this.str_content = dt_preview.Rows[0]["Content"].ToString();
        this.str_footer = dt_preview.Rows[0]["Footer"].ToString();

        }

    }

 



    protected void btn_send_Click(object sender, EventArgs e)
    {
        string str = string.Empty;
        string strname = string.Empty;
        foreach (GridViewRow gvrow in gvEmp.Rows)
        {
            CheckBox chk = (CheckBox)gvrow.FindControl("chkSelect");
            if (chk != null & chk.Checked)
            {


                string str_name = gvrow.Cells[3].Text;
                string str_head = this.str_head;
                string str_sub = this.str_sub;
                string str_content = this.str_content;
                string str_img =  this.str_img ;
                string str_footer = this.str_footer;
                string email_fun = this.CF.PasswordRecover(str_name, "response",str_head,str_sub,str_content,str_img,str_footer);

             

                    SqlCommand cmd = new SqlCommand("Update IT_Mailtemplate set Status=2 where Mailtemplatekey='" + str_id + "'");
                    cmd.Parameters.AddWithValue("@Mailtemplatekey", str_id);
                    DA.ExecuteNonQuery(cmd);
                    Response.Redirect(@"~/Admin/Mailtemplate.aspx");
              
                this.grid();
                //  Txt_Area.InnerText = "";
                ClientScript.RegisterStartupScript(this.GetType(), "Global Requirement", "<script>alert('Email sent successfully');</script>");


            }
        }
    }
}
