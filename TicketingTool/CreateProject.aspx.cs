using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;


public partial class TicketingTool_CreateProject : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    string str_queryid = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();

        if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
        {

            this.str_queryid = Request.QueryString["id"].ToString();
            Btn_update.Visible = true;

            if (!IsPostBack)
            {
                this.Loadcatgaroy();
                this.assignvalues();
            }
        }
        else
        {
            if (!IsPostBack)
            {
                this.Loadcatgaroy();

            }
            btn_send.Visible = true;
        }
    }

    private void assignvalues()
    {
        string str_table = "select pjname,PjCategory,PjDescription from TT_Project Where projectkey='" + this.str_queryid + "' ";
        SqlCommand cmd = new SqlCommand(str_table);
        DataTable dt_assginue = DA.GetDataTable(cmd);
        if (dt_assginue.Rows.Count > 0)
        {
            txt_prjname.Text = dt_assginue.Rows[0]["pjname"].ToString();
            txt_des.InnerText = dt_assginue.Rows[0]["PjDescription"].ToString();
            ddl_prjtype.SelectedValue = dt_assginue.Rows[0]["PjCategory"].ToString();
        }

    }
    private void Loadcatgaroy()
    {
        string str_cat = "select Pjtcategorykey,Categoryname from TT_PjtCategory where Status='1'";
        SqlCommand sc = new SqlCommand(str_cat);
        DataSet ds = this.DA.GetDataSet(sc);
        if (ds != null && ds.Tables.Count > 0)
        {
            ddl_prjtype.DataSource = ds.Tables[0];
            ddl_prjtype.DataTextField = "Categoryname";
            ddl_prjtype.DataValueField = "Pjtcategorykey";
            ddl_prjtype.DataBind();
            ddl_prjtype.Items.Add(new ListItem("Select  Project Category ", "0"));
            ddl_prjtype.SelectedValue = "0";
        }
    }
    protected void btn_send_Click(object sender, EventArgs e)
    {

        string str_staus = "1";
        string str_insert = "insert into TT_Project (PjName,PjCategory,PjDescription,ProgressStatus,Createdby) values(@PjName,@PjCategory,@PjDescription,@ProgressStatus,@Createdby)";
        SqlCommand cmd = new SqlCommand(str_insert);
        cmd.Parameters.AddWithValue("@PjName", txt_prjname.Text);
        cmd.Parameters.AddWithValue("@PjCategory", ddl_prjtype.SelectedValue);
        cmd.Parameters.AddWithValue("@PjDescription", txt_des.InnerText);
        cmd.Parameters.AddWithValue("@ProgressStatus", str_staus);
        cmd.Parameters.AddWithValue("@Createdby", SC.Userid);
        DA.ExecuteNonQuery(cmd);
        Response.Redirect("~/TicketingTool/Projects.aspx?=1");
    }
    protected void Btn_update_Click(object sender, EventArgs e)
    {
        string str_modifiedon = DateTime.Now.ToString();
        string str_update = "update TT_Project set PjName=@PjName,PjCategory=@PjCategory,PjDescription=@PjDescription,Modifiedon=@Modifiedon,Modifiedby=@Modifiedby Where projectkey='" + this.str_queryid + "'";
        SqlCommand sc = new SqlCommand(str_update);
        sc.Parameters.AddWithValue("@PjName",txt_prjname.Text);
        sc.Parameters.AddWithValue("@PjDescription", txt_des.InnerText);
        sc.Parameters.AddWithValue("@PjCategory",ddl_prjtype.SelectedValue);
        sc.Parameters.AddWithValue("@Modifiedon", str_modifiedon);
        sc.Parameters.AddWithValue("Modifiedby",SC.Userid);
        DA.ExecuteNonQuery(sc);
        Response.Redirect("~/TicketingTool/Projects.aspx");
    }
}