using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class WEB_ChangePassword : System.Web.UI.Page
{
    SessionCustom sc;
    DataAccess da;
    string userkey = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.sc = new SessionCustom();
        this.da = new DataAccess();
        this.userkey = sc.Userid;
        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Change Password";
        }
    }
    protected void btn_Submit_Click(object sender, EventArgs e)
    {
        DateTime date = DateTime.Now;
        string getdate = date.ToString();
        string str_changepass = "Update IT_EmployeeRegister set Password=@Password,modifiedon=@modifiedon where Employeekey=@Employeekey";
        SqlCommand cmd = new SqlCommand(str_changepass);
        cmd.Parameters.AddWithValue("@Password", txt_password.Text.Trim());
        cmd.Parameters.AddWithValue("@modifiedon", getdate);
        cmd.Parameters.AddWithValue("@Employeekey", userkey);
        da.ExecuteNonQuery(cmd);
        lbl_error.Text = "Password Updated Successfully";
   div_error.Visible = true;
        lbl_error.Visible = true;
        txt_password.Text = "";
        txt_cnfrmPassword.Text = "";
    }
}