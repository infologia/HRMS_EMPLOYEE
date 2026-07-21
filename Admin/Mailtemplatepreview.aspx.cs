using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class WEB_Admin_Mailtemplatepreview : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    string str_id = "";

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


        if (Request.QueryString["key"] != null && Request.QueryString["key"] != "")
        {

            this.str_id = Request.QueryString["key"].ToString();
        
        }

        string str_query = "select Header,MailSubject,Content,Image,Footer from IT_Mailtemplate where Mailtemplatekey=@Mailtemplatekey ";

        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@Mailtemplatekey", str_id);
        DataTable dt_dashboard = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_dashboard);
        if (dt_dashboard.Rows.Count > 0)
        {
            //string str_image = ds.Tables[0].Rows[0]["Images"].ToString();
           

           
            this.PH.LoadGridItem(ds, PH_Preview, "EmailForRegistration.txt", "");

        }
    }
    protected void btn_Submit_Click(object sender, EventArgs e)
    {
        Response.Redirect(@"~/Admin/Mailtemplatesend.aspx?key="+ this.str_id +"");
    }
}