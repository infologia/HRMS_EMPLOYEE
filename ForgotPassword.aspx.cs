using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.IO;

public partial class ForgotPassword : System.Web.UI.Page
{
    DataAccess DA;
    CommonFunction CF;
    PhTemplate PH;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.CF = new CommonFunction();
        this.PH = new PhTemplate();
    }

    protected void btn_Submit_Click(object sender, EventArgs e)
    {
        DataAccess DA = new DataAccess();
        CommonFunction CF= new CommonFunction();
        PhTemplate PH = new PhTemplate();

        string str_Sql = "select Password,Email,Employeekey,Username from IT_EmployeeRegister where Email=@Email";
        SqlCommand cmd = new SqlCommand(str_Sql);
        cmd.Parameters.AddWithValue("@Email", txt_email.Text);
        DataTable dt_email = DA.GetDataTable(cmd);

        if (dt_email != null && dt_email.Rows.Count > 0)
        {
            string str_password = Convert.ToString(dt_email.Rows[0]["Password"]);
            string str_name = Convert.ToString(dt_email.Rows[0]["Email"]);
            string str_userkey = Convert.ToString(dt_email.Rows[0]["Employeekey"]);
            string str_firstname = Convert.ToString(dt_email.Rows[0]["Username"]);
            SqlCommand SC_Log = CF.CreateLogKey(str_userkey);
            DA.ExecuteNonQuery(SC_Log);

            string str_Query = "SELECT top 1 * FROM IT_Logdetail where createdby=@createdby ORDER BY createdon DESC";
            SqlCommand sc = new SqlCommand(str_Query);
            sc.Parameters.AddWithValue("@createdby", str_userkey);
            DataTable dt_log = DA.GetDataTable(sc);

            string str_logkey = Convert.ToString(dt_log.Rows[0]["LogKey"]);
            string str_link = "http://employee.infologia.in/ResetPassword.aspx?id=" + str_logkey + "";
            string email_fun = this.CF.PasswordRecovery(str_name, "password", "Infologia Password Recovery", str_link, str_firstname);
            txt_email.Text = "";
            div_suscc.Visible = true;
            Lbl_Sucs.Text = "Recovery instructions sent to your email please check your email";
            Div_fail.Visible = false;
        }
        else
        {
            div_suscc.Visible = false;
            Div_fail.Visible = true;
            Lbl_Fail.Text = "Please Provide valid email address";
        }
    }
}