using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WEB_EmployeeRegister : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_userky = "";
    string str_EmployeeStatus = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        this.DA = new DataAccess();
        this.SC = new SessionCustom();

        if (!IsPostBack)
        {

            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Employee Register";

            this.loaddivision();
            this.loaddepartment();
            this.loaddesignation();
            this.loadstate();
           
        }
        this.str_userky = SC.Userid;
    }

    private void loadstate()
    {
        string str_state = "select Stateid,Statename from IT_State order by Statename ASC";
        SqlCommand cmd = new SqlCommand(str_state);
        DataSet ds = this.DA.GetDataSet(cmd);
        if (ds != null && ds.Tables.Count > 0)
        {
            ddl_state.DataSource = ds.Tables[0];
            ddl_state.DataValueField = "Stateid";
            ddl_state.DataTextField = "Statename";
            ddl_state.DataBind();
            ddl_state.Items.Add(new ListItem("Select  State ", "0"));
            ddl_state.SelectedValue = "0";

        }
    }

    private void loaddesignation()
    {
        string str_des = "select Destinationid,Destinationname from IT_Destination order by Destinationname ASC";
        SqlCommand cmd = new SqlCommand(str_des);
        DataSet ds1 = this.DA.GetDataSet(cmd);
        if (ds1 != null && ds1.Tables.Count > 0)
        {
            ddl_dest.DataSource = ds1.Tables[0];
            ddl_dest.DataValueField = "Destinationid";
            ddl_dest.DataTextField = "Destinationname";
            ddl_dest.DataBind();
            ddl_dest.Items.Add(new ListItem(" Select  Destination Name ", "0"));
            ddl_dest.SelectedValue = "0";

        }
    }

    private void loaddepartment()
    {
        string str_dep = "select Departmentid,Departmentname from IT_Department order by Departmentname ASC";
        SqlCommand cmd = new SqlCommand(str_dep);
        DataSet ds2 = this.DA.GetDataSet(cmd);
        if (ds2 != null && ds2.Tables.Count > 0)
        {
            ddl_depart.DataSource = ds2.Tables[0];
            ddl_depart.DataValueField = "Departmentid";
            ddl_depart.DataTextField = "Departmentname";
            ddl_depart.DataBind();
            ddl_depart.Items.Add(new ListItem("Select  Department Name", "0"));
            ddl_depart.SelectedValue = "0";

        }
    }

    private void loaddivision()
    {
        string str_URL = "select Divisionid,Divisionname from IT_Division order by Divisionname ASC";
        SqlCommand cmd = new SqlCommand(str_URL);
        DataSet ds3 = this.DA.GetDataSet(cmd);
        if (ds3 != null && ds3.Tables.Count > 0)
        {
            ddl_division.DataSource = ds3.Tables[0];
            ddl_division.DataValueField = "Divisionid";
            ddl_division.DataTextField = "Divisionname";
            ddl_division.DataBind();
            ddl_division.Items.Add(new ListItem("Select  Division Name", "0"));
            ddl_division.SelectedValue = "0";

        }
    }

    protected void btn_register_Click(object sender, EventArgs e)
    {
        // ===== DOB 18 Years Validation START =====
        DateTime dob;

        if (!DateTime.TryParseExact(
                txt_dob.Text,
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out dob))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Internal Tool",
                "<script>alert('Please enter a valid Date of Birth');</script>");
            return;
        }


        int age = DateTime.Now.Year - dob.Year;

        if (DateTime.Now < dob.AddYears(age))
        {
            age--;
        }

        if (age < 18)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Internal Tool",
                "<script>alert('Employee must be at least 18 years old to register');</script>");
            return;
        }
        // ===== DOB 18 Years Validation END =====

        string str_employeeid = Txt_Employeeid.Text;
        string str_chkempid = "select Employeeid from IT_EmployeeRegister where Employeeid='" + str_employeeid + "'";
        DataTable dt_chkempid = DA.GetDataTable(str_chkempid);
        if (dt_chkempid.Rows.Count > 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Internal Tool", "<script>alert('Employeeid already registered try with new Employeeid');</script>");
            return;
        }

        string str_Username = txt_username.Text;
        string str_chkUsername = "select Username from IT_EmployeeRegister where Username='" + str_Username + "'";
        DataTable dt_chkUsername = DA.GetDataTable(str_chkUsername);
        if (dt_chkUsername.Rows.Count > 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Internal Tool", "<script>alert('Username already registered try with new Username');</script>");
            return;
        }


        this.str_EmployeeStatus = "1";
        string str_userid = Guid.NewGuid().ToString();
        string str_sql = ("Insert into IT_EmployeeRegister(Employeekey,Employeeid,Username,Firstname,Lastname,Email,Phonenumber,Password,Address,State,City,Zipcode,Image,Gender,DOB,Destination,Qualification,Division,Department,Createdby,EmployeeStatus,roles)values(@Employeekey,@Employeeid,@Username,@Firstname,@Lastname,@Email,@Phonenumber,@Password,@Address,@State,@City,@Zipcode,@Image,@Gender,@DOB,@Destination,@Qualification,@Division,@Department,@Createdby,@EmployeeStatus,@roles)");
        SqlCommand cmd = new SqlCommand(str_sql);

      

        string filename = Path.GetFileName(up_img.FileName);
        string extension = Path.GetExtension(filename);
        string str_newid = str_userid + extension;
        string str_path = Server.MapPath("~/images/AdminPRofilePictures/") + str_newid;
        up_img.SaveAs(str_path);

        cmd.Parameters.AddWithValue("@Employeekey", str_userid);
        cmd.Parameters.AddWithValue("@Employeeid", Txt_Employeeid.Text);
        cmd.Parameters.AddWithValue("@Username", txt_username.Text);
        cmd.Parameters.AddWithValue("@Firstname", txt_fname.Text);
        cmd.Parameters.AddWithValue("@Lastname", txt_lname.Text);
        cmd.Parameters.AddWithValue("@Email", txt_email.Text);
        cmd.Parameters.AddWithValue("@Phonenumber", txt_phone.Text);
        cmd.Parameters.AddWithValue("@Password", txt_pwd.Text);
        cmd.Parameters.AddWithValue("@Address", txt_address.Text);
        cmd.Parameters.AddWithValue("@State", ddl_state.SelectedValue);

        cmd.Parameters.AddWithValue("@City", txt_city.Text);
        cmd.Parameters.AddWithValue("@Zipcode", txt_zipcode.Text);
        cmd.Parameters.AddWithValue("@Image", str_newid);
        cmd.Parameters.AddWithValue("@Gender", rd_gander.SelectedValue);
        cmd.Parameters.AddWithValue("@DOB", txt_dob.Text);
        cmd.Parameters.AddWithValue("@Destination", ddl_dest.SelectedValue);

        cmd.Parameters.AddWithValue("@Qualification", txt_qualification.Text);
        cmd.Parameters.AddWithValue("@Division", ddl_division.SelectedValue);
        cmd.Parameters.AddWithValue("@Department", ddl_depart.SelectedValue);
        cmd.Parameters.AddWithValue("@Createdby", this.SC.Userid);
        cmd.Parameters.AddWithValue("@EmployeeStatus", str_EmployeeStatus);
cmd.Parameters.AddWithValue("@roles", str_EmployeeStatus);
        DA.ExecuteNonQuery(cmd);
        ClientScript.RegisterStartupScript(this.GetType(), "Internal Tool", "<script>alert('Register  Successfully');</script>");
        Response.Redirect("~/Admin/EmployeeView.aspx");
        
    }

}