using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_TaskCategory : System.Web.UI.Page
{

    DataAccess DA;
    SessionCustom SC;
    string str_userkey = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.str_userkey = SC.Userid.ToString();

    }
    protected void btn_send_Click(object sender, EventArgs e)
    {
        DataTable dt_check = DA.GetDataTable("select * from TT_TaskCategory where categoryname='" + txt_category.Text + "'");
        if (dt_check.Rows.Count > 0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "internal tool", "<script>alert('This Catecory name already created');</script>");
            txt_category.Text = "";
        }

        if (txt_category.Text == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "internal tool", "<script>alert('Please fill Catecory');</script>");
            return;

        }

        if (txt_description.InnerText == "")
        {
            ClientScript.RegisterStartupScript(this.GetType(), "internal tool", "<script>alert('Please fill descriptions');</script>");
            return;
        }

        try
        {


            string str_project = "insert into TT_TaskCategory(Categoryname,Description,status,createdby)values(@Categoryname,@Description,@status,@createdby)";
            SqlCommand cmd = new SqlCommand(str_project);
            cmd.Parameters.AddWithValue("Categoryname", txt_category.Text);
            cmd.Parameters.AddWithValue("Description", txt_description.InnerText);
            cmd.Parameters.AddWithValue("status", Rd_Status.SelectedValue);
            cmd.Parameters.AddWithValue("createdby", str_userkey);
            DA.ExecuteNonQuery(cmd);
            Response.Redirect("~/Admin/TaskCategoryView.aspx");
        }
   
    catch(Exception ex)
{
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
}