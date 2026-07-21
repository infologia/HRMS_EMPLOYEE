using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Menuroleupdate : System.Web.UI.Page
{
    SessionCustom SC;
    DataAccess DA;
    string str_id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.SC = new SessionCustom();
        this.DA = new DataAccess();

        if (Request.QueryString["id"] == "" || Request.QueryString["id"] == null)
        {

        }
        else
        {
            this.str_id = Request.QueryString["id"].ToString();
            if (!IsPostBack)
            {
                assignvalues();
            }
        }
    }
    protected void btn_submit_Click(object sender, EventArgs e)
    {
        try
        {

            if (this.str_id == "")
            {
                string str_int = "insert into TT_MenuRole (roleName,createdby)values(@roleName,@createdby)";
                SqlCommand cmd = new SqlCommand(str_int);
                cmd.Parameters.AddWithValue("roleName", txt_menuname.Text);
                cmd.Parameters.AddWithValue("createdby", this.SC.Userid);
                DA.ExecuteNonQuery(cmd);
            }
            else
            {
                string str_int = "UPDATE TT_MenuRole SET roleName=@roleName,modifiedon=getdate(),modifiedby=@modifiedby WHERE MenuroleKey=@MenuroleKey;";
                SqlCommand cmd = new SqlCommand(str_int);
                cmd.Parameters.AddWithValue("roleName", txt_menuname.Text);
                cmd.Parameters.AddWithValue("MenuroleKey", this.str_id);
                cmd.Parameters.AddWithValue("modifiedby", this.SC.Userid);
                DA.ExecuteNonQuery(cmd);
            }
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Zeesta", "<script>alert('Update failed please contact Team');</script>");
            return;
        }

        Response.Redirect("~/Admin/Menurole.aspx");
    }

    public void assignvalues()
    {
        string str_assing = "select * from TT_MenuRole where MenuroleKey=@MenuroleKey;";
        SqlCommand cmd = new SqlCommand(str_assing);
        cmd.Parameters.AddWithValue("MenuroleKey", this.str_id);
        DataTable dt_parentmenu = this.DA.GetDataTable(cmd);
        if (dt_parentmenu.Rows.Count > 0)
        {
            txt_menuname.Text = dt_parentmenu.Rows[0]["roleName"].ToString();

        }

    }
}