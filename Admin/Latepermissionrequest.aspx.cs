using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
public partial class Employee_Latepermissionrequest : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_userkey = "";
    string str_requestdate = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Self Services";

        }
        DateTime date = DateTime.Now;
        txt_date.Text = date.ToString("MM/dd/yyyy");
    }
    protected void btn_perm_Click(object sender, EventArgs e)
    {
        if (txt_date.Text == "" && txt_fromtime.Text == "" && txt_totime.Text == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "infologia", "<script>alert('Please select Required Field');</script>");
            return;
        }
        if (txt_date.Text == "" || txt_fromtime.Text == "" || txt_totime.Text == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "infologia", "<script>alert('Please select Required Field');</script>");
            return;
        }

        //check//
        DateTime dt_current = DateTime.Now;
        dt_current = dt_current.Date;
        DateTime dt_date = Convert.ToDateTime(txt_date.Text);
        dt_date = dt_date.Date;
        if (dt_current > dt_date)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "infologia", "<script>alert('Please select valid date');</script>");
            return;

        }

        //Validatetime For compnay times//
        TimeSpan tf = DateTime.Parse("09:00 AM").TimeOfDay;
        TimeSpan tt = DateTime.Parse("07:00 PM").TimeOfDay;

        string str_fromtime = txt_fromtime.Text;
        string str_totime = txt_totime.Text;

        TimeSpan tf1 = DateTime.Parse(str_fromtime).TimeOfDay;
        TimeSpan tt1 = DateTime.Parse(str_totime).TimeOfDay;

        if (tf > tf1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "infologia", "<script>alert('Please select valid From Time');</script>");
            return;
        }
        else if (tt < tt1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "infologia", "<script>alert('Please select valid To Time');</script>");
            return;
        }

        else if (tf > tt1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "infologia", "<script>alert('Please select valid To Time');</script>");
            return;
        }
        else if (tt < tf1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "infologia", "<script>alert('Please select valid To Time');</script>");
            return;
        }
        else if (tt1 < tf)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "infologia", "<script>alert('Please select valid To Time');</script>");
            return;
        }
        else if (tf1 > tt)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "infologia", "<script>alert('Please select valid To Time');</script>");
            return;
        }

        else if (tf1 == tt1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "infologia", "<script>alert('Please select valid To Time');</script>");
            return;
        }
        else if (tf1 > tt1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "infologia", "<script>alert('Please select valid To Time');</script>");
            return;
        }

        else if (tt1 < tf1)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "infologia", "<script>alert('Please select valid To Time');</script>");
            return;
        }




        //hours Calculation
        DateTime dt = Convert.ToDateTime(txt_fromtime.Text);
        DateTime dt1 = Convert.ToDateTime(txt_totime.Text);
        TimeSpan ts = dt1 - dt;
        //TimeSpan ts1 = DateTime.Parse("05:00:00").TimeOfDay;
        //if (ts >= ts1)
        //{
        //    ClientScript.RegisterStartupScript(this.GetType(), "infologia", "<script>alert('Permission should be below 05:00 hrs...');</script>");
        //    return;
        //}

        //Insert into Table


        DateTime getdate12 = Convert.ToDateTime(txt_date.Text);
        this.str_userkey = SC.Userid.ToString();
        String str_sql = ("insert into IT_LatePermissionDetails(Employeekey,permissionhourse,Createdby,Requestdate,Fromtime,Totime,Reason)values(@Employeekey,@permissionhourse,@Createdby,@Requestdate,@Fromtime,@Totime,@Reason)");
        SqlCommand cmd = new SqlCommand(str_sql);
        cmd.Parameters.AddWithValue("@Employeekey", str_userkey);
        cmd.Parameters.AddWithValue("@Createdby", str_userkey);
        cmd.Parameters.AddWithValue("@Requestdate", getdate12);
        cmd.Parameters.AddWithValue("@Fromtime", txt_fromtime.Text);
        cmd.Parameters.AddWithValue("@Totime", txt_totime.Text);
        cmd.Parameters.AddWithValue("@Reason", txt_reasons.InnerText);
        cmd.Parameters.AddWithValue("@permissionhourse", ts);
        DA.ExecuteNonQuery(cmd);
        Response.Redirect("~/Employee/LatePermissionRequestView.aspx");
    }
}