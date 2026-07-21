using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_AddUserView : System.Web.UI.Page
{

    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    string str_key = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();

        if (Request.QueryString["id"] == null || Request.QueryString["id"] == "")
        {

        }
        else
        {
            this.str_key = Request.QueryString["id"].ToString();
            this.lable();
            this.Assignvalues();
        }

    }

    private void lable()
    {
        string str_show = "select a.pjname,b.categoryname from TT_Project a left outer join TT_pjtcategory b on a.pjcategory=b.pjtcategorykey where Projectkey=@Projectkey";
        SqlCommand sc = new SqlCommand(str_show);
        sc.Parameters.AddWithValue("@Projectkey", str_key);
        DataTable dt_show = DA.GetDataTable(sc);
        if (dt_show.Rows.Count > 0)
        {

            string str_show2 = dt_show.Rows[0]["pjname"].ToString();
            string str_show3 = dt_show.Rows[0]["categoryname"].ToString();
            lb_show.Text = str_show2;
            lb_show2.Text = str_show3;
        }
    }
    private void Assignvalues()
    {
        string str_query = "select a.AddUserkey,CONVERT(VARCHAR,a.createdon,103)as Createdon,b.Username,c.Departmentname,d.Divisionname,e.Destinationname from TT_AddUser a  left outer join IT_EmployeeRegister b on b.Employeekey=a.Employeekey left outer join IT_department c on c.Departmentid=b.Department left outer join IT_division d on d.Divisionid=b.division left outer join IT_destination e on e.destinationid=b.destination where Projectkey=@Projectkey";
        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@Projectkey", this.str_key);
        DataTable dt_Prj = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt_Prj);
        if (dt_Prj.Rows.Count > 0)
        {

            this.PH.LoadGridItem(ds, PH_AddUserView, "AddUserView.txt", "");

        }

    }

    [WebMethod] //Delete
    public static string DeleteProject(string str_AddUserkey)
    {
        string str_Response = "0";
        try
        {
            DataAccess DA1;
            DA1 = new DataAccess();
            SaveQuery SAQ = new SaveQuery();
            SessionCustom SC = new SessionCustom();
            string str_Sql = "delete from TT_AddUser where AddUserkey=@AddUserkey";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@AddUserkey", str_AddUserkey);
            DA1.ExecuteNonQuery(cmd);

            return str_Response = "1";

        }
        catch (Exception ex)
        {
            return str_Response;
        }
    }
    protected void btn_add_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/admin/adduser.aspx?id=" + this.str_key + "");
    }
}

