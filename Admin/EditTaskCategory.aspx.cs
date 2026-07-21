using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_EditTaskCategory : System.Web.UI.Page
{


    DataAccess DA;
    SessionCustom SC;
    string str_key = "";
    string str_userkey = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.str_userkey = this.SC.Userid;
        if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
        {

            this.str_key = Request.QueryString["id"].ToString();
        }
        if (!IsPostBack)
        {
            this.loadedit();

        }
    }
    private void loadedit()
    {


        string str_query = "select categoryname,Description,Status from TT_TaskCategory where Taskcategorykey=@Taskcategorykey ";

        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@Taskcategorykey", str_key);
        DataTable dt_preview = DA.GetDataTable(cmd);

        if (dt_preview.Rows.Count > 0)
        {

            txt_category.Text = dt_preview.Rows[0]["Categoryname"].ToString();
            txt_description.InnerText = dt_preview.Rows[0]["Description"].ToString();
            Rd_Status.SelectedValue = dt_preview.Rows[0]["Status"].ToString();





        }
    }





    protected void Rd_Status_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (this.Rd_Status.SelectedValue == "1")
        {
            txt_active.Visible = true;
        }
        else if (this.Rd_Status.SelectedValue == "0")
        {
            txt_active.Visible = true;
        }
    }
    protected void btn_update_Click(object sender, EventArgs e)
    {
        
        string date = DateTime.Now.ToString();
  string str_project = "update TT_TaskCategory SET Categoryname=@Categoryname,description=@description,Status=@Status,Modifiedon=@Modifiedon,Modifiedby=@Modifiedby where TaskCategorykey=@TaskCategorykey";
      SqlCommand cmd =new SqlCommand(str_project);
        cmd.Parameters.AddWithValue("@TaskCategorykey", str_key);
        cmd.Parameters.AddWithValue("@Categoryname", txt_category.Text);
        cmd.Parameters.AddWithValue("@description", txt_description.InnerText);
        cmd.Parameters.AddWithValue("@Status", Rd_Status.SelectedValue);
        cmd.Parameters.AddWithValue("@Modifiedon", date);
        cmd.Parameters.AddWithValue("@Modifiedby", str_userkey);
        DA.ExecuteNonQuery(cmd);
        Response.Redirect("~/Admin/TaskCategoryView.aspx");
    }
}