using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WEB_ProfilePage : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_userid = "";
    string str_newid = "";
    string str_UserProfileImage = "";
    string userroles;
    private object str_modifiedon;
    

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.userroles=this.SC.UserRole;


        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Profile Settings";
if (this.userroles == "2") // Admin
{
    btnBack.HRef = "Dashboard.aspx";
}
else // Employee
{
    btnBack.HRef = "~/Employee/Timings.aspx";
}
 }


        if (!IsPostBack)
        {
            this.loaddivision();
            this.loaddepartment();
            this.loaddesignation();
            this.loadstate();

        }

        if (this.userroles != "0")
        {
            txt_dob.ReadOnly = true;
            txt_dob.Attributes.Add("style", "background-color: #eee; cursor: not-allowed; pointer-events: none;");
            ddl_depart.Attributes.Add("disabled", "disabled");
            ddl_dest.Attributes.Add("disabled", "disabled");
            ddl_division.Attributes.Add("disabled", "disabled");
            ddl_state.Attributes.Add("disabled", "disabled");
        }

        this.str_userid = SC.Userid;
        String str_Profile = "select * from it_employeeregister where employeekey=@employeekey";

        SqlCommand cmd = new SqlCommand(str_Profile);
        cmd.Parameters.AddWithValue("@employeekey", this.str_userid);
        DataTable dt_dashboard = DA.GetDataTable(cmd);
        if (dt_dashboard != null && dt_dashboard.Rows.Count > 0)
        {
            string str_adminid = dt_dashboard.Rows[0]["Employeeid"].ToString();
            if (str_adminid == "")
            {
                btn_Resgister.Visible = true;
            }
            else
            {
                if (!IsPostBack)
                {

                    div_Password.Visible = true;
                    div_username.Visible = true;
                    Btn_Update.Visible = true;
                    this.LoadProfile();
                }
            }
        }


    }

    private void LoadProfile()
    {
        this.str_userid = SC.Userid;
        String str_key = "select * from it_employeeregister where employeekey=@employeekey";

        SqlCommand cmd = new SqlCommand(str_key);
        cmd.Parameters.AddWithValue("@employeekey", this.str_userid);
        DataTable dt_dashboard = DA.GetDataTable(cmd);
        if (dt_dashboard != null && dt_dashboard.Rows.Count > 0)
        {
            txt_admid.Text = dt_dashboard.Rows[0]["Employeeid"].ToString();
            string str_username = dt_dashboard.Rows[0]["username"].ToString();
            txt_username.Text = str_username;
            label_UserName.Text = str_username;
            txt_fname.Text = dt_dashboard.Rows[0]["Firstname"].ToString();
            txt_lname.Text = dt_dashboard.Rows[0]["Lastname"].ToString();
            txt_email.Text = dt_dashboard.Rows[0]["Email"].ToString();
            txt_pwd.Attributes["value"] = dt_dashboard.Rows[0]["password"].ToString();
            txt_phone.Text = dt_dashboard.Rows[0]["Phonenumber"].ToString();
            if (dt_dashboard.Rows[0]["DOB"] != DBNull.Value && !string.IsNullOrEmpty(dt_dashboard.Rows[0]["DOB"].ToString()))
            {
                string rawDob = dt_dashboard.Rows[0]["DOB"].ToString();
                DateTime parsedDob;
                string[] parseFormats = { "dd/MM/yyyy", "yyyy-MM-dd", "MM/dd/yyyy", "yyyy-MM-dd HH:mm:ss", "dd/MM/yyyy HH:mm:ss" };
                if (DateTime.TryParseExact(rawDob.Trim(), parseFormats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out parsedDob))
                {
                    txt_dob.Text = parsedDob.ToString("dd/MM/yyyy");
                }
                else if (DateTime.TryParse(rawDob, out parsedDob))
                {
                    txt_dob.Text = parsedDob.ToString("dd/MM/yyyy");
                }
                else
                {
                    txt_dob.Text = rawDob;
                }
            }
            else
            {
                txt_dob.Text = "";
            }
            txt_address.Text = dt_dashboard.Rows[0]["Address"].ToString();
            ddl_dest.SelectedValue = dt_dashboard.Rows[0]["Destination"].ToString();
            ddl_depart.SelectedValue = dt_dashboard.Rows[0]["Department"].ToString();
            ddl_state.SelectedValue = dt_dashboard.Rows[0]["State"].ToString();
            ddl_division.SelectedValue = dt_dashboard.Rows[0]["Division"].ToString();
            rd_gander.Text = dt_dashboard.Rows[0]["Gender"].ToString();
            
            txt_city.Text = dt_dashboard.Rows[0]["City"].ToString();
            txt_zipcode.Text = dt_dashboard.Rows[0]["Zipcode"].ToString();
            txt_qualification.Text = dt_dashboard.Rows[0]["Qualification"].ToString();
            this.str_UserProfileImage = Convert.ToString(dt_dashboard.Rows[0]["Image"]);
            if (str_UserProfileImage == "")
            {
                Img_Profile.ImageUrl = "../images/nopicture.jpg";
            }
            else
            {
                Img_Profile.ImageUrl = "~/images/AdminPRofilePictures/" + str_UserProfileImage;
            }
            ddl_dest.Attributes.Add("disabled", "disabled");
            ddl_depart.Attributes.Add("disabled", "disabled");
            ddl_division.Attributes.Add("disabled", "disabled");
        }
    }

    private void loadstate()
    {
        string str_state = "select Stateid,Statename from IT_State";
        SqlCommand cmd = new SqlCommand(str_state);
        DataSet ds = this.DA.GetDataSet(cmd);
        if (ds != null && ds.Tables.Count > 0)
        {
            ddl_state.DataSource = ds.Tables[0];
            ddl_state.DataTextField = "Statename";
            ddl_state.DataValueField = "Stateid";
            ddl_state.DataBind();
            ddl_state.Items.Add(new ListItem("Select  Statename ", "0"));
            ddl_state.SelectedValue = "0";
        }
    }

    private void loaddesignation()
    {
        string str_dest = "select Destinationid,Destinationname from IT_Destination";
        SqlCommand cmd = new SqlCommand(str_dest);
        DataSet ds = this.DA.GetDataSet(cmd);
        if (ds != null && ds.Tables.Count > 0)
        {
            ddl_dest.DataSource = ds.Tables[0];
            ddl_dest.DataTextField = "Destinationname";
            ddl_dest.DataValueField = "Destinationid";
            ddl_dest.DataBind();
            ddl_dest.Items.Add(new ListItem("Select  Designation Name ", "0"));
            ddl_dest.SelectedValue = "0";
        }
    }

    private void loaddepartment()
    {
        string str_depat = "select Departmentid,Departmentname from IT_Department";
        SqlCommand cmd = new SqlCommand(str_depat);
        DataSet ds = this.DA.GetDataSet(cmd);
        if (ds != null && ds.Tables.Count > 0)
        {
            ddl_depart.DataSource = ds.Tables[0];
            ddl_depart.DataTextField = "Departmentname";
            ddl_depart.DataValueField = "Departmentid";
            ddl_depart.DataBind();
            ddl_depart.Items.Add(new ListItem("Select  Department Name ", "0"));
            ddl_depart.SelectedValue = "0";
        }
    }

    private void loaddivision()
    {
        string str_division = "select Divisionid,Divisionname from IT_Division";
        SqlCommand cmd = new SqlCommand(str_division);
        DataSet ds = this.DA.GetDataSet(cmd);
        if (ds != null && ds.Tables.Count > 0)
        {
            ddl_division.DataSource = ds.Tables[0];
            ddl_division.DataTextField = "Divisionname";
            ddl_division.DataValueField = "Divisionid";
            ddl_division.DataBind();
            ddl_division.Items.Add(new ListItem("Select  Division Name ", "0"));
            ddl_division.SelectedValue = "0";
        }
    }


    protected void btn_Resgister_Click(object sender, EventArgs e)
    {
        this.str_userid = SC.Userid.ToString();
        string str_sql = ("update it_employeeregister set Employeeid=@Employeeid,Firstname=@Firstname,Lastname=@Lastname,Email=@Email,DOB=@DOB,Phonenumber=@Phonenumber,Address=@Address,State=@State,City=@City,Zipcode=@Zipcode,Image=@Image,Gender=@Gender,Destination=@Destination,Qualification=@Qualification,Division=@Division,Department=@Department,Createdby=@Createdby where employeekey=@employeekey");
        SqlCommand cmd = new SqlCommand(str_sql);



        string filename = Path.GetFileName(Fi_Updatepicture.FileName);
        if (filename != "")
        {
            string extension = Path.GetExtension(filename);
            this.str_newid = str_userid + extension;
            string str_path = Server.MapPath("~/images/AdminProfilePictures/") + str_newid;
            Fi_Updatepicture.SaveAs(str_path);
        }

        cmd.Parameters.AddWithValue("@Employeekey", str_userid);
        cmd.Parameters.AddWithValue("@Employeeid", txt_admid.Text);

        cmd.Parameters.AddWithValue("@Firstname", txt_fname.Text);
        cmd.Parameters.AddWithValue("@Lastname", txt_lname.Text);
        cmd.Parameters.AddWithValue("@Email", txt_email.Text);
        DateTime dob;
        string[] formats = { "dd/MM/yyyy", "yyyy-MM-dd", "MM/dd/yyyy" };
        if (DateTime.TryParseExact(txt_dob.Text.Trim(),
                                   formats,
                                   System.Globalization.CultureInfo.InvariantCulture,
                                   System.Globalization.DateTimeStyles.None,
                                   out dob))
        {
            cmd.Parameters.Add("@DOB", SqlDbType.Date).Value = dob;
        }
        else if (DateTime.TryParse(txt_dob.Text, out dob))
        {
            cmd.Parameters.Add("@DOB", SqlDbType.Date).Value = dob;
        }
        else
        {
            cmd.Parameters.Add("@DOB", SqlDbType.Date).Value = DBNull.Value;
        }
        cmd.Parameters.AddWithValue("@Phonenumber", txt_phone.Text);

        cmd.Parameters.AddWithValue("@Address", txt_address.Text);
        cmd.Parameters.AddWithValue("@State", ddl_state.SelectedValue);

        cmd.Parameters.AddWithValue("@City", txt_city.Text);
        cmd.Parameters.AddWithValue("@Zipcode", txt_zipcode.Text);
        if (this.str_newid != "")
        {

            cmd.Parameters.AddWithValue("@Image", str_newid);
        }
        else
        {
            cmd.Parameters.AddWithValue("@Image", SC.UserImage);
        }
        cmd.Parameters.AddWithValue("@Gender", rd_gander.SelectedValue);
        
        

        cmd.Parameters.AddWithValue("@Destination", ddl_dest.SelectedValue);

        cmd.Parameters.AddWithValue("@Qualification", txt_qualification.Text);
        cmd.Parameters.AddWithValue("@Division", ddl_division.SelectedValue);
        cmd.Parameters.AddWithValue("@Department", ddl_depart.SelectedValue);
        cmd.Parameters.AddWithValue("@Createdby", this.SC.Userid);
        DA.ExecuteNonQuery(cmd);

        ScriptManager.RegisterStartupScript(
            this,
            GetType(),
            "toastr_success",
            "showToastr('success', 'Profile updated successfully'); setTimeout(function(){ window.location.href = '/Admin/Dashboard.aspx'; }, 2000);",
            true
        );


    }
    protected void Btn_Update_Click(object sender, EventArgs e)
    {
        this.str_userid = SC.Userid.ToString();
        string str_sql = "update  It_employeeregister set Employeeid=@Employeeid,Firstname=@Firstname,Lastname=@Lastname,Email=@email,DOB=@DOB,Phonenumber=@Phonenumber,Address=@Address,City=@City,Zipcode=@Zipcode,Image=@Image,Gender=@Gender,Modifiedby=@Modifiedby,Modifiedon=@Modifiedon where Employeekey=@Employeekey";

        SqlCommand cmd = new SqlCommand(str_sql);
        cmd.Parameters.AddWithValue("@Modifiedon", DateTime.UtcNow);     
        cmd.Parameters.AddWithValue("@Employeekey", str_userid);
        string filename = Path.GetFileName(Fi_Updatepicture.FileName);
        if (filename != "")
        {
            string extension = Path.GetExtension(filename);
            this.str_newid = str_userid + extension;
            string str_path = Server.MapPath("~/images/AdminProfilePictures/") + this.str_newid;
            Fi_Updatepicture.SaveAs(str_path);
        }

        cmd.Parameters.AddWithValue("@Employeeid", txt_admid.Text);
        cmd.Parameters.AddWithValue("@Firstname", txt_fname.Text);
        cmd.Parameters.AddWithValue("@Lastname", txt_lname.Text);
        cmd.Parameters.AddWithValue("@Email", txt_email.Text);
        cmd.Parameters.AddWithValue("@Phonenumber", txt_phone.Text);
        DateTime dob;
        string[] formats = { "dd/MM/yyyy", "yyyy-MM-dd", "MM/dd/yyyy" };
        if (DateTime.TryParseExact(txt_dob.Text.Trim(),
                                   formats,
                                   System.Globalization.CultureInfo.InvariantCulture,
                                   System.Globalization.DateTimeStyles.None,
                                   out dob))
        {
            cmd.Parameters.Add("@DOB", SqlDbType.Date).Value = dob;
        }
        else if (DateTime.TryParse(txt_dob.Text, out dob))
        {
            cmd.Parameters.Add("@DOB", SqlDbType.Date).Value = dob;
        }
        else
        {
            cmd.Parameters.Add("@DOB", SqlDbType.Date).Value = DBNull.Value;
        }
        cmd.Parameters.AddWithValue("@Address", txt_address.Text);
        //cmd.Parameters.AddWithValue("@State", ddl_state.SelectedValue);

        cmd.Parameters.AddWithValue("@City", txt_city.Text);
        cmd.Parameters.AddWithValue("@Zipcode", txt_zipcode.Text);
        
        string currentImage = "";

        SqlCommand getImgCmd = new SqlCommand("select Image from It_employeeregister where Employeekey=@Employeekey");
        getImgCmd.Parameters.AddWithValue("@Employeekey", str_userid);
        DataTable dtImg = DA.GetDataTable(getImgCmd);

        if (dtImg.Rows.Count > 0)
        {
            currentImage = dtImg.Rows[0]["Image"].ToString();
        }

        if (this.str_newid != "")
        {
            cmd.Parameters.Add("@Image", SqlDbType.NVarChar).Value = str_newid;
        }
        else
        {
            cmd.Parameters.Add("@Image", SqlDbType.NVarChar).Value = currentImage;
        }

        cmd.Parameters.AddWithValue("@Gender", rd_gander.SelectedValue);   
        cmd.Parameters.AddWithValue("@Modifiedby", this.SC.Userid);
        
        DA.ExecuteNonQuery(cmd);
        string str_role = this.userroles.ToString();

        //if (str_role == "0") {
        //    Response.Redirect("~/Admin/Dashboard.aspx");
        //}
        //else if(str_role == "1")
        //{
        //    Response.Redirect("~/Employee/Timings.aspx");

        //}
        string redirectUrl = "";

        if (str_role == "0")
        {
            redirectUrl = "/Admin/Dashboard.aspx";
        }
        else if (str_role == "1")
        {
            redirectUrl = "/Employee/Timings.aspx";
        }

        ScriptManager.RegisterStartupScript(
            this,
            GetType(),
            "toastr_success",
            "showToastr('success', 'Profile updated successfully'); setTimeout(function(){ window.location.href = '" + redirectUrl + "'; }, 2000);",
            true
        );



    }
}