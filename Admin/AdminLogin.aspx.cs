using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;

public partial class WEB_AdminLogin : System.Web.UI.Page
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
        string str_login = "select * from  IT_AdminLogin where Username=@Username and Userpassword=@Userpassword COLLATE SQL_Latin1_General_CP1_CS_AS ";
        SqlCommand sc = new SqlCommand(str_login);
        sc.Parameters.AddWithValue("@Username", txt_Uname.Text);
        sc.Parameters.AddWithValue("@Userpassword", txt_Pwd.Text);
        DataTable dt_login = this.DA.GetDataTable(sc);

        if (dt_login != null && dt_login.Rows.Count > 0)
        {
            SC.Userid = dt_login.Rows[0]["Userkey"].ToString();
            string str_Admin = dt_login.Rows[0]["Username"].ToString();
           
            SC.username = str_Admin;
            SC.UserRecordTable = dt_login;
            Response.Redirect(@"~/Admin/Dashboard.aspx");
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
