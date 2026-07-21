using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;


public partial class WEB_MailTemplate : System.Web.UI.Page
{

    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;

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

        //String str_query = "SELECT username,fromdate,Todate,reason,approvedstatus,employeekey FROM IT_EmployeeLeaveDetails INNER JOIN IT_EmployeeRegister ON IT_EmployeeLeaveDetails.createdby = IT_EmployeeRegister.employeekey order by IT_EmployeeLeaveDetails.createdon ASC";
        string str_id = this.SC.Userid;
        string str_query = "select Header,MailSubject,Content,Image,Footer,Mailtemplatekey from IT_Mailtemplate order by createdon ASC ";

        SqlCommand cmd = new SqlCommand(str_query);
      
        DataTable dt_dashboard = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_dashboard);

        if (dt_dashboard.Rows.Count > 0)
           

            {
                this.PH.LoadGridItem(ds, PH_mail, "mail.txt", "");

            }


        }


    [WebMethod] //Delete
    public static string DeleteProject(string Productkey)
    {
        string str_Response = "0";
        try
        {
            DataAccess DA1;
            DA1 = new DataAccess();
            SaveQuery SAQ = new SaveQuery();
            SessionCustom SC = new SessionCustom();
            string str_Sql = "delete from IT_Mailtemplate where Mailtemplatekey=@Mailtemplatekey";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@Mailtemplatekey", Productkey);
            DA1.ExecuteNonQuery(cmd);

            return str_Response = "1";

        }
        catch (Exception ex)
        {
            return str_Response;
        }
    }
  
}
   
