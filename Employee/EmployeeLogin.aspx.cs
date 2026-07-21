using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class WEB_Employee_EmployeeLogin : System.Web.UI.Page
{
    DataAccess Da;
    SessionCustom Sc;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.Sc = new SessionCustom();
        this.Da = new DataAccess();
    }
    protected void btn_Submit_Click(object sender, EventArgs e)
    {
        string str_login = "select * from  IT_EmployeeRegister where Username=@Username COLLATE SQL_Latin1_General_CP1_CS_AS and Password=@Password COLLATE SQL_Latin1_General_CP1_CS_AS ";
        SqlCommand sc = new SqlCommand(str_login);
        sc.Parameters.AddWithValue("@Username", txt_Uname.Text);
        sc.Parameters.AddWithValue("@Password", txt_Pwd.Text);
        DataTable dt_login = this.Da.GetDataTable(sc);

        if (dt_login != null && dt_login.Rows.Count > 0)
        {
            string str_userststus = dt_login.Rows[0]["EmployeeStatus"].ToString();
            string str_stusreson = dt_login.Rows[0]["Employeereason"].ToString();
            if (str_userststus == "0")
            {
                lbl_error.Text = str_stusreson.ToString();
                lbl_error.ForeColor = System.Drawing.Color.Red;
                div_error.Visible = true;
                lbl_error.Visible = true;
            }
            else
            {
                Sc.Userid = dt_login.Rows[0]["Employeekey"].ToString();
                string str_Admin = dt_login.Rows[0]["Username"].ToString();
                Sc.UserImage = dt_login.Rows[0]["image"].ToString();
                Sc.username = str_Admin;
                Sc.UserRecordTable = dt_login;
                Response.Redirect(@"~/Employee/Timings.aspx");
            }
           
        }
        else
        {
                lbl_error.Text="Incorrect Username/Password";
                lbl_error.ForeColor=System.Drawing.Color.Red;
                div_error.Visible = true;
                lbl_error.Visible = true;
        }

      
    }
}