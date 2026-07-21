using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_ParentMenu : System.Web.UI.Page
{
    SessionCustom SC;
    DataAccess DA;
    string str_id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.SC = new SessionCustom();
        this.DA = new DataAccess();


        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Menu";

            //HtmlAnchor control = this.Master.FindControl("li_dashboard") as HtmlAnchor;
            //if (control != null)
            //    control.Attributes.Add("class", "active");
        }


        if (Request.QueryString["id"] == "" || Request.QueryString["id"] == null)
        {
            if (!IsPostBack)
            {
                this.Destination();
                btn_submit.Text = "Submit";
            }
        }
        else
        {
            this.str_id = Request.QueryString["id"].ToString();
            if (!IsPostBack)
            {
                this.Destination();
                assignvalues();
                btn_submit.Text = "Update";
            }
        }


      

    }
         private void Destination()
    {
        string str_desg = "select * from it_destination";
        SqlCommand cmd3 = new SqlCommand(str_desg);
        DataSet ds = this.DA.GetDataSet(cmd3);
        if (ds != null && ds.Tables.Count > 0)
        {
            ddl_desg.DataSource = ds.Tables[0];
            ddl_desg.DataTextField = "Destinationname";
            ddl_desg.DataValueField = "Destinationkey";
            ddl_desg.DataBind();
            ddl_desg.Items.Add(new ListItem("Select Designation ", "0"));
            ddl_desg.SelectedValue = "0";
        }


    }

    
    protected void btn_submit_Click(object sender, EventArgs e)
    {
        try
        {

            if (this.str_id == "")
            {
                int partype = 0;
                //string str_userky = Guid.NewGuid().ToString();
                string str_int = "insert into TT_Menus(MenuName,Menudescription,Menutype,Menulist,Menuicon,Createdby,Destinationkey,Status)values(@ParentMenuName,@Menudescription,@Menutype,@Menulist,@Menuicon,@Createdby,@Destinationkey,@Status)";
                SqlCommand cmd = new SqlCommand(str_int);
                cmd.Parameters.AddWithValue("Status", Convert.ToInt32(rblStatus.SelectedValue));
                cmd.Parameters.AddWithValue("ParentMenuName", txt_menuname.Text);
                cmd.Parameters.AddWithValue("Menutype", partype);
                cmd.Parameters.AddWithValue("Menulist", txt_menuno.Text);
                cmd.Parameters.AddWithValue("Menuicon", txt_icons.Text);
                cmd.Parameters.AddWithValue("Menudescription", txt_menudesc.InnerText);
                cmd.Parameters.AddWithValue("Createdby",this.SC.Userid);
                cmd.Parameters.AddWithValue("Destinationkey", ddl_desg.SelectedValue);
                DA.ExecuteNonQuery(cmd);
            }
            else
            {
                string date = DateTime.Now.ToString();
                string str_int = "UPDATE TT_Menus SET MenuName=@ParentMenuName,Menuicon=@Menuicon,Menulist=@Menulist,Menudescription=@Menudescription,Modifiedby=@Modifiedby,Modifiedon=@Modifiedon,Destinationkey=@Destinationkey,Status=@Status WHERE MenuKey=@ParentMenuKey;";
                SqlCommand cmd = new SqlCommand(str_int);
                cmd.Parameters.AddWithValue("Status", Convert.ToInt32(rblStatus.SelectedValue));
                cmd.Parameters.AddWithValue("ParentMenuName", txt_menuname.Text);
                cmd.Parameters.AddWithValue("Menudescription", txt_menudesc.InnerText);
                cmd.Parameters.AddWithValue("ParentMenuKey", this.str_id);
                cmd.Parameters.AddWithValue("Menuicon", txt_icons.Text);
                cmd.Parameters.AddWithValue("Menulist", txt_menuno.Text);
                cmd.Parameters.AddWithValue("Modifiedby", this.SC.Userid);
                cmd.Parameters.Add("Modifiedon", SqlDbType.DateTime).Value = DateTime.Now;

                cmd.Parameters.AddWithValue("Destinationkey", ddl_desg.SelectedValue);
                DA.ExecuteNonQuery(cmd);
            }
        }
        catch
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Zeesta", "<script>alert('Action failed please contact Team');</script>");
            return;
        }

        Response.Redirect("~/Admin/ParentMenuGrid.aspx");
    }

    public void assignvalues()
    {
        string str_assing = "select * from TT_Menus where MenuKey=@ParentMenuKey";
        SqlCommand cmd = new SqlCommand(str_assing);
        cmd.Parameters.AddWithValue("ParentMenuKey", this.str_id);
        DataTable dt_parentmenu = this.DA.GetDataTable(cmd);
        if (dt_parentmenu.Rows.Count > 0)
        {
            txt_menuname.Text = dt_parentmenu.Rows[0]["MenuName"].ToString();
            txt_menudesc.InnerText = dt_parentmenu.Rows[0]["Menudescription"].ToString();
            txt_icons.Text = dt_parentmenu.Rows[0]["Menuicon"].ToString();
            txt_menuno.Text = dt_parentmenu.Rows[0]["Menulist"].ToString();
            ddl_desg.SelectedValue = dt_parentmenu.Rows[0]["Destinationkey"].ToString();

            rblStatus.SelectedValue = dt_parentmenu.Rows[0]["Status"].ToString();
        }

    }

    [WebMethod] // Check Numbers
    public static string Checkemployeeid(string str_menuno)
    {
        try
        {

            DataAccess DA1 = new DataAccess();
            SessionCustom SC = new SessionCustom();
            string strusg = SC.Userdesg;
            string str_userdesgn = "";
            string str_desg = "select * from it_destination where destinationid='" + strusg + "'";
            SqlCommand cmd1 = new SqlCommand(str_desg);
            //cmd.Parameters.AddWithValue("@Employeekey", str_userid);
            DataTable DT_desg = DA1.GetDataTable(cmd1);
            if (DT_desg.Rows.Count > 0)
            {
                str_userdesgn = DT_desg.Rows[0]["Destinationkey"].ToString();

            }

            SqlCommand sc_username = new SqlCommand("select * from TT_Menus where menulist=@menulist where menutype=1 and  Destinationkey='" + str_userdesgn + "'");
            sc_username.Parameters.Add(new SqlParameter("@menulist", SqlDbType.NVarChar)).Value = str_menuno;
            DataTable dt_username = new DataAccess().GetDataTable(sc_username);

            if (dt_username.Rows.Count > 0)
            {
                return "This number already assigned try with new row number";
            }
            return "";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }

    }
}