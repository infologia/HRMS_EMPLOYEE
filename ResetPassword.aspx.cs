using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Cryptography;
using System.Text;
using System.IO;

public partial class ResetPassword : System.Web.UI.Page
{
    DataAccess DA;
   // private Decrypt de;
    string str_userkey = "";
    string str_Key = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
      //  de = new Decrypt();


        if (Request.QueryString["id"] == null || Request.QueryString["id"] == "") return;
        str_Key = Request.QueryString["id"];
        DateTime now = DateTime.UtcNow;
        string str_datetime = now.ToString();
        string str_Sql = "select * from IT_Logdetail where logkey=@logkey";
        SqlCommand cmd = new SqlCommand(str_Sql);
        cmd.Parameters.AddWithValue("@logkey", str_Key);
        DataTable dt_email = DA.GetDataTable(cmd);
        if (dt_email != null && dt_email.Rows.Count > 0)
        {
            string str_createdon = Convert.ToString(dt_email.Rows[0]["createdon"]);
            this.str_userkey = Convert.ToString(dt_email.Rows[0]["createdby"]);
            var createdon = DateTime.Parse(str_createdon).AddMinutes(30);
            if (now > createdon)
            {
                Response.Redirect("~/LinkExpired.aspx");
            }
        }
    }
 
    protected void btn_Submit_Click(object sender, EventArgs e)
    {
        string val = BCrypt.Net.BCrypt.HashPassword(txt_newpass.Text.Trim());

        string str_Sqlupdate = "UPDATE IT_EmployeeRegister SET password=@password where employeekey=@employeekey";
        SqlCommand cmd = new SqlCommand(str_Sqlupdate);
        cmd.Parameters.AddWithValue("@password", val);
        cmd.Parameters.AddWithValue("@employeekey", this.str_userkey);
        this.DA.ExecuteNonQuery(cmd);

        Response.Redirect("~/login.aspx?id=1");
    }
}