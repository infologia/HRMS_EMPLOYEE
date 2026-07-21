using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class Admin_Addmembers : System.Web.UI.Page
{

    DataAccess DA;
    SessionCustom SC;
    CommonFunction CF;
    string str_userkey = "";
    string str_key = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.CF = new CommonFunction();
        this.str_userkey = SC.Userid.ToString();

        if (!IsPostBack)
        {
            this.project();

        }

        if (Request.QueryString["id"] == null || Request.QueryString["id"] == "")
        {

        }
        else
        {
            this.str_key = Request.QueryString["id"].ToString();
            if (!IsPostBack)
            {
                this.assgnvalue();
            }

        }

    }

    private void assgnvalue()
    {
        string str_show = "select a.Projectkey,b.categoryname,a.PjDescription from TT_Project a left outer join TT_pjtcategory b on a.pjcategory=b.pjtcategorykey where Projectkey=@Projectkey";
        SqlCommand sc = new SqlCommand(str_show);
        sc.Parameters.AddWithValue("@Projectkey", str_key);
        DataTable dt_show = DA.GetDataTable(sc);
        if (dt_show.Rows.Count > 0)
        {
            ddl_pjtname.Attributes.Add("Readonly", "Readonly");
            ddl_pjtname.SelectedValue = dt_show.Rows[0]["Projectkey"].ToString();
            txt_category.Attributes.Add("Readonly", "Readonly");
            txt_description.Attributes.Add("Readonly", "Readonly");
            txt_category.Text = dt_show.Rows[0]["categoryname"].ToString();
            txt_description.InnerText = dt_show.Rows[0]["PjDescription"].ToString();
        }

    }



    [WebMethod]
    public static string[] GetCustomers(string prefix, string prefix2)
    {
        AppVar AP = new AppVar();
        string str_con = AP.DatabaseConnectionString;

        List<string> customers = new List<string>();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = str_con;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "Select Firstname,Employeekey from IT_EmployeeRegister where  Firstname like @searchtext+ '%'";
                cmd.Parameters.AddWithValue("@SearchText", prefix);
                cmd.Parameters.AddWithValue("@projectkey", prefix2);

                cmd.Connection = conn;
                conn.Open();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        customers.Add(string.Format("{0},{1}", sdr["firstname"], sdr["Employeekey"]));
                    }
                }
                conn.Close();
            }
        }
        return customers.ToArray();

    }

    private void project()
    {
        string str_des = "select PjName,Projectkey from TT_Project ";
        SqlCommand cmd = new SqlCommand(str_des);
        DataSet ds = this.DA.GetDataSet(cmd);
        if (ds != null && ds.Tables.Count > 0)
        {
            ddl_pjtname.DataSource = ds.Tables[0];
            ddl_pjtname.DataTextField = "PjName";
            ddl_pjtname.DataValueField = "projectkey";
            ddl_pjtname.DataBind();
            ddl_pjtname.Items.Add(new ListItem("Select Project ", "0"));
            ddl_pjtname.SelectedValue = "0";
        }


    }


    protected void txt_pjtname_SelectedIndexChanged(object sender, EventArgs e)
    {
        string str_project = "select * from TT_project a left outer join TT_pjtcategory b on a.pjcategory=b.pjtcategorykey where a.projectkey=@projectkey";
        SqlCommand cmd1 = new SqlCommand(str_project);
        cmd1.Parameters.AddWithValue("@Projectkey", ddl_pjtname.SelectedValue);
        DataTable dt_project = DA.GetDataTable(cmd1);
        if (dt_project.Rows.Count > 0)
        {

            hfkey.Value = ddl_pjtname.SelectedValue;
            txt_category.Attributes.Add("Readonly", "Readonly");
            txt_description.Attributes.Add("Readonly", "Readonly");
            txt_category.Text = dt_project.Rows[0]["Categoryname"].ToString();
            txt_description.InnerText = dt_project.Rows[0]["PjDescription"].ToString();
        }
    }


    protected void btn_submit_Click(object sender, EventArgs e)
    {
        try
        {

            string str_chk = "select ProgressStatus from TT_Project where projectkey=@projectkey";
            SqlCommand sc = new SqlCommand(str_chk);
            sc.Parameters.AddWithValue("@projectkey", ddl_pjtname.SelectedValue);
            DataTable dt_chk = DA.GetDataTable(sc);
            if (dt_chk.Rows.Count > 0)
            {
                string str_stus = dt_chk.Rows[0]["ProgressStatus"].ToString();
                if (str_stus == "1")
                {
                    string str_addpjt = "insert into [TT_AddMember](Projectkey,Employeekey,Createdby)values(@Projectkey,@Employeekey,@Createdby)";
                    SqlCommand cmd2 = new SqlCommand(str_addpjt);
                    cmd2.Parameters.AddWithValue("@Projectkey", ddl_pjtname.SelectedValue);
                    cmd2.Parameters.AddWithValue("@Employeekey", hfCustomerId.Value);
                    cmd2.Parameters.AddWithValue("@Createdby", str_userkey);
                    DA.ExecuteNonQuery(cmd2);
                    string str_username = "Select username,email from IT_EmployeeRegister where  Employeekey='" + hfCustomerId.Value + "' ";
                    SqlCommand cmd3 = new SqlCommand(str_username);
                    DataTable dt_mail = DA.GetDataTable(cmd3);
                    if (dt_mail.Rows.Count > 0)
                    {
                        string str_email = dt_mail.Rows[0]["Email"].ToString();
                        string str_user = dt_mail.Rows[0]["Username"].ToString();
                        string str_user1 = ddl_pjtname.SelectedItem.Text;
                        string str_user3 = txt_category.Text;

                        string email_fun = this.CF.assignemployee(str_email, "Employee", str_user, str_user1, str_user3);

                    }

                }
                else if (str_stus == "2")
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "Internal Tool", "<script>alert('Your Project Was Inactive..Please active and update');</script>");
                    return;
                }


            }
            //Response.Redirect("~/admin/AddUserView.aspx?id=" + this.str_key + "");
            ClientScript.RegisterStartupScript(this.GetType(), "Internal Tool", "<script>alert('Add members successfully');</script>");
        }
        catch (Exception ex)
        {

        }

    }
}