using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WEB_Employee_Profile : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    String str_userid = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();

        if (!IsPostBack)
        {
            this.loaddivision();
            this.loaddepartment();
            this.loaddesignation();
            this.loadstate();

        }
        this.LoadProfile();
    }
    protected void Btn_Update_Click(object sender, EventArgs e)
    {

    }

    protected void btn_Resgister_Click(object sender, EventArgs e)
    {

    }

    private void LoadProfile()
    {
        this.str_userid = SC.Userid;
        String str_key = "select  * from IT_EmployeeRegister where Employeekey=@Employeekey";

        SqlCommand cmd = new SqlCommand(str_key);
        cmd.Parameters.AddWithValue("@employeekey", this.str_userid);
        DataTable dt_dashboard = DA.GetDataTable(cmd);
        if (dt_dashboard != null && dt_dashboard.Rows.Count > 0)
        {
            txt_empid.Text = dt_dashboard.Rows[0]["Employeeid"].ToString();
            string str_username = dt_dashboard.Rows[0]["Username"].ToString();
            txt_username.Text = str_username;
            label_UserName.Text = str_username;
            txt_fname.Text = dt_dashboard.Rows[0]["Firstname"].ToString();
            txt_lname.Text = dt_dashboard.Rows[0]["Lastname"].ToString();
            txt_email.Text = dt_dashboard.Rows[0]["Email"].ToString();
            txt_pwd.Attributes["value"] = dt_dashboard.Rows[0]["Password"].ToString();
            txt_phone.Text = dt_dashboard.Rows[0]["Phonenumber"].ToString();
            txt_address.Text = dt_dashboard.Rows[0]["Address"].ToString();
            ddl_depart.Attributes.Add("disabled", "disabled");
            ddl_dest.Attributes.Add("disabled", "disabled");
            ddl_division.Attributes.Add("disabled", "disabled");
            ddl_state.Attributes.Add("disabled", "disabled");
            ddl_dest.SelectedValue = dt_dashboard.Rows[0]["Destination"].ToString();
            ddl_depart.SelectedValue = dt_dashboard.Rows[0]["Department"].ToString();
            ddl_state.SelectedValue = dt_dashboard.Rows[0]["State"].ToString();
            ddl_division.SelectedValue = dt_dashboard.Rows[0]["Division"].ToString();
            rd_gander.Text = dt_dashboard.Rows[0]["Gender"].ToString();
            txt_dob.Text = dt_dashboard.Rows[0]["DOB"].ToString();
            txt_city.Text = dt_dashboard.Rows[0]["City"].ToString();
            txt_zipcode.Text = dt_dashboard.Rows[0]["Zipcode"].ToString();
            txt_qualification.Text = dt_dashboard.Rows[0]["Qualification"].ToString();
            string str_UserProfileImage = Convert.ToString(dt_dashboard.Rows[0]["Image"]);
            if (str_UserProfileImage == "")
            {
                Img_Profile.ImageUrl = "~/images/nopicture.jpg";
            }
            else
            {
                Img_Profile.ImageUrl = "~/images/EmployeePRofilePictures/" + str_UserProfileImage;
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
            ddl_dest.Items.Add(new ListItem("Select  Department Name ", "0"));
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
            ddl_depart.Items.Add(new ListItem("Select  Desingation Nmae ", "0"));
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
            ddl_division.Items.Add(new ListItem("Select  Divisionname ", "0"));
            ddl_division.SelectedValue = "0";
        }
    }

}