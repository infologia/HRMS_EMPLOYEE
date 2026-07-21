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

public partial class Admin_Watches : System.Web.UI.Page
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
        string str_show = "select a.Taskkey,a.Taskname,a.Description,b.Pjname from TT_Createtask a left outer join TT_project b on a.Projectkey=b.Projectkey where Taskkey=@Taskkey";
     SqlCommand sc = new SqlCommand(str_show);
        sc.Parameters.AddWithValue("@Taskkey", this.str_key);
        DataTable dt_show = DA.GetDataTable(sc);
        if (dt_show.Rows.Count > 0)
        {
            string str_name = dt_show.Rows[0]["Taskkey"].ToString();
            hfkey.Value = str_name;
            
            txt_taskname.Attributes.Add("Readonly", "Readonly");
            txt_description.Attributes.Add("Readonly", "Readonly");
            txt_pjtname.Text = dt_show.Rows[0]["pjname"].ToString();
            txt_taskname.Text = dt_show.Rows[0]["Taskname"].ToString();
            txt_description.InnerText = dt_show.Rows[0]["Description"].ToString();
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
                cmd.CommandText = "Select Firstname,Employeekey from IT_EmployeeRegister where Employeekey NOT IN (SELECT Employeekey FROM TT_TaskWatcher WHERE Taskkey=@Taskkey)  and Firstname like @searchtext+ '%'";
                cmd.Parameters.AddWithValue("@SearchText", prefix);
                cmd.Parameters.AddWithValue("@Taskkey", prefix2);

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


    protected void btn_submit_Click(object sender, EventArgs e)
    {
        try
        {      string str_addpjt = "insert into TT_TaskWatcher(Employeekey,Taskkey,Createdby)values(@Employeekey,@Taskkey,@Createdby)";
                    SqlCommand cmd2 = new SqlCommand(str_addpjt);
                    cmd2.Parameters.AddWithValue("@Taskkey", hfkey.Value);
                    cmd2.Parameters.AddWithValue("@Employeekey", hfCustomerId.Value);
                    cmd2.Parameters.AddWithValue("@Createdby", str_userkey);
                    DA.ExecuteNonQuery(cmd2);

                    string str_username = "Select username,email from IT_EmployeeRegister where Employeekey='" + hfCustomerId.Value + "' ";
                    SqlCommand cmd3 = new SqlCommand(str_username);
                    DataTable dt_mail = DA.GetDataTable(cmd3);
                    if (dt_mail.Rows.Count > 0)
                    {
                        string str_email = dt_mail.Rows[0]["Email"].ToString();
                        string str_user = dt_mail.Rows[0]["Username"].ToString();
                        string str_user1 = txt_pjtname.Text;
                        string str_user3 = txt_taskname.Text;

                        string email_fun = this.CF.assignemployee(str_email, "Employee", str_user, str_user1, str_user3);

                    }
                 Response.Redirect("~/admin/WatchesView.aspx?id=" + this.str_key + "");

                }
                

      
        catch (Exception ex)
        {

        }

    }

    }
