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

        if (!IsPostBack)
        {
            try
            {
                string selectPlainQuery = "SELECT Employeekey, Password FROM IT_EmployeeRegister";
                SqlCommand selectCmd = new SqlCommand(selectPlainQuery);
                DataTable dtEmployees = this.DA.GetDataTable(selectCmd);
                foreach (DataRow row in dtEmployees.Rows)
                {
                    string empKey = row["Employeekey"].ToString();
                    string rawPassword = row["Password"].ToString();

                    if (!string.IsNullOrEmpty(rawPassword) && !(rawPassword.Length == 60 && (rawPassword.StartsWith("$2a$") || rawPassword.StartsWith("$2b$") || rawPassword.StartsWith("$2y$"))))
                    {
                        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(rawPassword);
                        string updateQuery = "UPDATE IT_EmployeeRegister SET Password = @Password WHERE Employeekey = @Employeekey";
                        SqlCommand updateCmd = new SqlCommand(updateQuery);
                        updateCmd.Parameters.AddWithValue("@Password", hashedPassword);
                        updateCmd.Parameters.AddWithValue("@Employeekey", empKey);
                        this.DA.ExecuteNonQuery(updateCmd);
                    }
                }
            }
            catch (Exception ex)
            {
                // Fallback silently if any error happens so the page doesn't block
            }
        }
    }
    protected void btn_Submit_Click(object sender, EventArgs e)
    {
        string str_login = "select * from  IT_EmployeeRegister where Username=@Username COLLATE SQL_Latin1_General_CP1_CS_AS ";
        SqlCommand sc = new SqlCommand(str_login);
        sc.Parameters.AddWithValue("@Username", txt_Uname.Text);
        DataTable dt_login = this.DA.GetDataTable(sc);

        if (dt_login != null && dt_login.Rows.Count > 0)
        {
            string password = dt_login.Rows[0]["Password"].ToString();
            bool isValid = false;

            if (password.Length == 60 && (password.StartsWith("$2a$") || password.StartsWith("$2b$") || password.StartsWith("$2y$")))
            {
                isValid = BCrypt.Net.BCrypt.Verify(txt_Pwd.Text, password);
            }
            else
            {
                isValid = (txt_Pwd.Text == password);
            }

            if (isValid)
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
        else
        {
            lbl_error.Text = "Incorrect Username/Password";
            lbl_error.ForeColor = System.Drawing.Color.Red;
            div_error.Visible = true;
            lbl_error.Visible = true;
        }


    }
}