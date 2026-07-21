using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class WEB_Admin_Permissionview : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_id = "";
    string str_userid = "";
    string str_responsereson = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();


        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Employee Monitoring";

            //HtmlAnchor control = this.Master.FindControl("li_dashboard") as HtmlAnchor;
            //if (control != null)
            //    control.Attributes.Add("class", "active");
        }


        if (Request.QueryString["key"] != null && Request.QueryString["key"] != "")
        {

            this.str_id = Request.QueryString["key"].ToString();
        }

        if (!IsPostBack)
        {
            this.loaddropdown();
            this.assignvalues();

        }
        
    }
    public void loaddropdown()
    {

        string str_URL = "   select * from StatusResponse order by StatusResponsekey";
        SqlCommand cmd = new SqlCommand(str_URL);
        DataSet ds1 = this.DA.GetDataSet(cmd);
        if (ds1 != null && ds1.Tables.Count > 0)
        {
            ddl_category.DataSource = ds1.Tables[0];
            ddl_category.DataTextField = "status";
            ddl_category.DataValueField = "StatusResponseId";

            ddl_category.DataBind();
            ddl_category.Items.Add(new ListItem("Select Status", "0"));
            ddl_category.SelectedValue = "0";

        }
    }
    private void assignvalues()
    {
        str_userid = this.SC.Userid;
        string str_leave = "select CONVERT(varchar(10), Requestdate, 103) AS Requestdate,Fromtime,toTime,reason,responsestatus,responsereason from IT_EmployeePermissionDetails where EmployeePermissionDetailskey=@EmployeePermissionDetailskey";
        SqlCommand cmd = new SqlCommand(str_leave);
        cmd.Parameters.AddWithValue("@EmployeePermissionDetailskey", str_id);
        DataTable dt_permission = this.DA.GetDataTable(cmd);

        if (dt_permission.Rows.Count > 0)
        {
            txt_date.Text = dt_permission.Rows[0]["Requestdate"].ToString();
            //txt_date.Text = dt_permission.Rows[0]["Requestdate"].ToString();
            txt_fromtime.Text = dt_permission.Rows[0]["Fromtime"].ToString();
            txt_totime.Text = dt_permission.Rows[0]["Totime"].ToString();
            txt_reasons.InnerText = dt_permission.Rows[0]["Reason"].ToString();
            ddl_category.SelectedValue = dt_permission.Rows[0]["responsestatus"].ToString();
            this.str_responsereson = dt_permission.Rows[0]["responsereason"].ToString();
            if (str_responsereson == "")
            {
                btn_update.Visible = true;
                txt_reason1.InnerText = str_responsereson;
            }
            else
            {
                btn_update.Visible = false;
                txt_reason1.Attributes.Add("Readonly","Readonly");
                ddl_category.Attributes.Add("disabled", "disabled");
                txt_reason1.InnerText = str_responsereson;
            }

            
        }
    }

   
   
  
    protected void btn_update_Click(object sender, EventArgs e)
    {

        try
        {
            if (ddl_category.SelectedValue=="1")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "infologia", "<script>alert('Please Fill Required Field');</script>");
                return;
            }



            str_userid = this.SC.Userid;
            string date = DateTime.Now.ToString();
            string str_Sql = "update  IT_EmployeePermissionDetails SET responsestatus=@responsestatus,responsereason=@responsereason,modifiedon=@modifiedon,modifiedby=@modifiedby where EmployeePermissionDetailskey=@EmployeePermissionDetailskey";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@EmployeePermissionDetailskey", str_id);
            if (ddl_category.SelectedValue == "1")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Internal Tool", "<script>alert('Please Select Reason');</script>");
                return;
            }
            cmd.Parameters.AddWithValue("@responsestatus", ddl_category.SelectedValue);
            cmd.Parameters.AddWithValue("@responsereason", txt_reason1.InnerText.Trim());
            
            cmd.Parameters.AddWithValue("@Modifiedby", this.str_userid);
            cmd.Parameters.AddWithValue("@Modifiedon", date);
            DA.ExecuteNonQuery(cmd);
          
            Response.Redirect("~/Admin/PermissionResponse.aspx");
        }
        catch (Exception ex)
        {
        }
    }
}