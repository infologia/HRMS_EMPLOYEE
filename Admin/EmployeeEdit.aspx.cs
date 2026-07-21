using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class WEB_EmployeeEdit : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_id = "";
    string str_UserProfileImage = "";
    string str_userid = "";
    string str_newid = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();



        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Profile Settings";

        }

        if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
        {
            this.str_id = Request.QueryString["id"].ToString();
        }
        if (!IsPostBack)
        {
            this.loaddivision();
            this.loaddepartment();
            this.loaddesignation();
            this.loadstate();
            this.assignvalues();

        }
    }

    private void assignvalues()
    {
        String str_key = "select  * from IT_EmployeeRegister where Employeekey=@Employeekey";

        SqlCommand cmd = new SqlCommand(str_key);
        cmd.Parameters.AddWithValue("@employeekey", this.str_id);
        DataTable dt_dashboard = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_dashboard);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            txt_empid.Text = ds.Tables[0].Rows[0]["Employeeid"].ToString();
            txt_username.Text = ds.Tables[0].Rows[0]["Username"].ToString();
            txt_fname.Text = ds.Tables[0].Rows[0]["Firstname"].ToString();
            txt_lname.Text = ds.Tables[0].Rows[0]["Lastname"].ToString();
            txt_email.Text = ds.Tables[0].Rows[0]["Email"].ToString();
            txt_pwd.Attributes["value"] = ds.Tables[0].Rows[0]["Password"].ToString();
            txt_phone.Text = ds.Tables[0].Rows[0]["Phonenumber"].ToString();
            txt_address.Text = ds.Tables[0].Rows[0]["Address"].ToString();
            ddl_dest.SelectedValue = ds.Tables[0].Rows[0]["Role"].ToString();
            ddl_depart.SelectedValue = ds.Tables[0].Rows[0]["Department"].ToString();
            ddl_state.SelectedValue = ds.Tables[0].Rows[0]["State"].ToString();
            ddl_division.SelectedValue = ds.Tables[0].Rows[0]["Division"].ToString();
            rd_gander.Text = ds.Tables[0].Rows[0]["Gender"].ToString();
            txt_dob.Text = ds.Tables[0].Rows[0]["DOB"].ToString();
            txt_city.Text = ds.Tables[0].Rows[0]["City"].ToString();
            Rd_Status.SelectedValue = ds.Tables[0].Rows[0]["EmployeeStatus"].ToString();

            txt_zipcode.Text = ds.Tables[0].Rows[0]["Zipcode"].ToString();

            txt_qualification.Text = ds.Tables[0].Rows[0]["Qualification"].ToString();
            this.str_UserProfileImage = Convert.ToString(dt_dashboard.Rows[0]["Image"]);
            if (str_UserProfileImage == "")
            {
                Img_Profile.ImageUrl = "~/images/nopicture.jpg";
            }
            else
            {
                Img_Profile.ImageUrl = "~/images/AdminPRofilePictures/" + str_UserProfileImage;
            }

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
            ddl_state.DataValueField = "Stateid";
            ddl_state.DataTextField = "Statename";
            ddl_state.DataBind();
            ddl_state.Items.Add(new ListItem("Select  State ", "0"));
            ddl_state.SelectedValue = "0";

        }
    }

    private void loaddesignation()
    {
        string str_des = "SELECT RoleId, RoleName FROM IT_Roles ORDER BY CreatedOn DESC";
        SqlCommand cmd = new SqlCommand(str_des);
        DataSet ds1 = this.DA.GetDataSet(cmd);
        if (ds1 != null && ds1.Tables.Count > 0)
        {
            ddl_dest.DataSource = ds1.Tables[0];
            ddl_dest.DataValueField = "RoleId";
            ddl_dest.DataTextField = "RoleName";
            ddl_dest.DataBind();
            ddl_dest.Items.Add(new ListItem(" Select Role ", "0"));
            ddl_dest.SelectedValue = "0";

        }
    }

    private void loaddepartment()
    {
        string str_dep = "select Departmentid,Departmentname from IT_Department";
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
        string str_URL = "select Divisionid,Divisionname from IT_Division";
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

    protected void Rd_Status_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.Rd_Status.SelectedValue == "0")
        {
            txt_active.Visible = true;
        }
        else if (this.Rd_Status.SelectedValue == "1")
        {
            txt_active.Visible = true;
        }
    }
    protected void btn_update_Click(object sender, EventArgs e)
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

        this.str_userid = SC.Userid.ToString();
        string str_modifiedon = DateTime.UtcNow.ToString();

        string str_sql = "update  IT_EmployeeRegister set Employeeid=@Employeeid,Firstname=@Firstname,Lastname=@Lastname,Email=@email,Phonenumber=@Phonenumber,Address=@Address,State=@State,City=@City,Zipcode=@Zipcode,EmployeeStatus=@EmployeeStatus,Employeereason=@Employeereason,Gender=@Gender,Image=@Image,DOB=@DOB,Role=@Role,Qualification=@Qualification,Division=@Division,Department=@Department,Modifiedby=@Modifiedby,Modifiedon=@Modifiedon where Employeekey=@Employeekey";
        SqlCommand cmd = new SqlCommand(str_sql);

        cmd.Parameters.Add("@Employeekey", SqlDbType.UniqueIdentifier).Value = new Guid(str_id);


        string filename = Path.GetFileName(Fi_Updatepicture.FileName);
        if (filename != "")
        {
            string extension = Path.GetExtension(filename);
            this.str_newid = str_id + extension;
            string str_path = Server.MapPath("~/images/AdminProfilePictures/") + this.str_newid;
            Fi_Updatepicture.SaveAs(str_path);
        }

        cmd.Parameters.AddWithValue("@Employeeid", txt_empid.Text);
        if (this.str_newid != "")
        {

            cmd.Parameters.AddWithValue("@Image", str_newid);
        }
        else
        {
            cmd.Parameters.AddWithValue("@Image", SC.UserImage);
        }

        cmd.Parameters.AddWithValue("@Firstname", txt_fname.Text);
        cmd.Parameters.AddWithValue("@Lastname", txt_lname.Text);
        cmd.Parameters.AddWithValue("@Email", txt_email.Text);
        cmd.Parameters.AddWithValue("@Phonenumber", txt_phone.Text);
        cmd.Parameters.AddWithValue("@Employeereason", txt_areamessage.InnerText);
        cmd.Parameters.AddWithValue("@EmployeeStatus", Rd_Status.SelectedValue);
        cmd.Parameters.AddWithValue("@Address", txt_address.Text);
        cmd.Parameters.AddWithValue("@State", ddl_state.SelectedValue);

        cmd.Parameters.AddWithValue("@City", txt_city.Text);
        cmd.Parameters.AddWithValue("@Zipcode", txt_zipcode.Text);

        cmd.Parameters.AddWithValue("@Gender", rd_gander.SelectedValue);       
        cmd.Parameters.Add("@DOB", SqlDbType.NVarChar).Value = txt_dob.Text.Trim();
        cmd.Parameters.AddWithValue("@Role", Convert.ToInt32(ddl_dest.SelectedValue));
        cmd.Parameters.AddWithValue("@Qualification", txt_qualification.Text);
        cmd.Parameters.AddWithValue("@Division", ddl_division.SelectedValue);
        cmd.Parameters.AddWithValue("@Department", ddl_depart.SelectedValue);
        cmd.Parameters.Add("@Modifiedby", SqlDbType.UniqueIdentifier).Value = new Guid(str_userid);
        cmd.Parameters.Add("@Modifiedon", SqlDbType.DateTime).Value = DateTime.UtcNow;
        DA.ExecuteNonQuery(cmd);
        Response.Redirect("~/Admin/Employeeview.aspx");
    }
}

