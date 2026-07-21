using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;


public partial class Login : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.SC = new SessionCustom();
        this.DA = new DataAccess();
    }
    protected void btn_Submit_Click(object sender, EventArgs e)
    {
        string str_login = "select * from  IT_EmployeeRegister where Username=@Username COLLATE SQL_Latin1_General_CP1_CS_AS and Password=@Password COLLATE SQL_Latin1_General_CP1_CS_AS ";
        SqlCommand sc = new SqlCommand(str_login);
        sc.Parameters.AddWithValue("@Username", txt_Uname.Text);
        sc.Parameters.AddWithValue("@Password", txt_Pwd.Text);
        DataTable dt_login = this.DA.GetDataTable(sc);

        if (dt_login != null && dt_login.Rows.Count > 0)
        {
            string str_role = dt_login.Rows[0]["roles"].ToString();
            if (str_role == "0")
            {
                SC.Userid = dt_login.Rows[0]["Employeekey"].ToString();
                string str_Admin = dt_login.Rows[0]["Username"].ToString();
                SC.username = str_Admin;
                SC.UserRole = dt_login.Rows[0]["roles"].ToString();
                SC.Userdesg = dt_login.Rows[0]["Destination"].ToString();
                SC.UserRecordTable = dt_login;
                Response.Redirect(@"~/Admin/Dashboard.aspx");
            }
            
            else if(str_role=="1")
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
                    SC.Userid = dt_login.Rows[0]["Employeekey"].ToString();
                    string str_Admin = dt_login.Rows[0]["Username"].ToString();
                    SC.UserImage = dt_login.Rows[0]["image"].ToString();
                    SC.UserRole = dt_login.Rows[0]["roles"].ToString();

                    SC.username = str_Admin;
                    SC.Userdesg = dt_login.Rows[0]["Destination"].ToString();
                    SC.UserRecordTable = dt_login;
                    Response.Redirect(@"~/Employee/Timings.aspx");
                }
            }
            else if (str_role == "2")
            {
                SC.Userid = dt_login.Rows[0]["Employeekey"].ToString();
                string str_Admin = dt_login.Rows[0]["Username"].ToString();
                SC.username = str_Admin;
                SC.UserRole = dt_login.Rows[0]["roles"].ToString();
                SC.Userdesg = dt_login.Rows[0]["Destination"].ToString();
                SC.UserRecordTable = dt_login;
                Response.Redirect(@"~/Admin/Dashboard.aspx");
            }
        }
        else
        {
            lbl_error.Text = "Incorrect Username/Password";
            lbl_error.ForeColor = System.Drawing.Color.Red;
            div_error.Visible = true;
            lbl_error.Visible = true;
        }


    }
}