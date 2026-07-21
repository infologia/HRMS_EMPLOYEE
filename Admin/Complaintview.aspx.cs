using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WEB_Admin_Complaintview : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_id = "";
    string str_userid = "";
    string str_response = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();


        if (Request.QueryString["key"] != null && Request.QueryString["key"] != "")
        {

            this.str_id = Request.QueryString["key"].ToString();
        }


        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Employee Monitoring";

            //HtmlAnchor control = this.Master.FindControl("li_dashboard") as HtmlAnchor;
            //if (control != null)
            //    control.Attributes.Add("class", "active");
        }

        if (!IsPostBack)
        {
            this.loaddropdown();
            this.loadstatus();
            this.assgnvalue();

        }



    }

    private void assgnvalue()
    {
        str_userid = this.SC.Userid;
        string str_leave = "select complaintcategory,reason,complaintstatus,complaintresponse from IT_Complaint where Complaintkey=@Complaintkey";
        SqlCommand cmd = new SqlCommand(str_leave);
        cmd.Parameters.AddWithValue("@Complaintkey", str_id);
        DataTable dt_complaint = this.DA.GetDataTable(cmd);

        if (dt_complaint.Rows.Count > 0)
        {
            txt_reason.InnerText = dt_complaint.Rows[0]["Reason"].ToString();
            ddl_category.SelectedValue = dt_complaint.Rows[0]["complaintcategory"].ToString();
            ddl_status.SelectedValue = dt_complaint.Rows[0]["complaintstatus"].ToString();

            this.str_response = dt_complaint.Rows[0]["complaintresponse"].ToString();

            if (str_response == "")
            {
                btn_request.Visible = true;
                txt_response.InnerText = str_response;
            }
            else
            {
                btn_request.Visible = false;
                txt_response.Attributes.Add("Readonly", "Readonly");
                ddl_status.Attributes.Add("disabled", "disabled");
                txt_response.InnerText = str_response;
            }

        }
    }

    public void loaddropdown()
    {

        string str_URL = "   select * from IT_ComplaintCategory order by ComplaintCategorykey";
        SqlCommand cmd = new SqlCommand(str_URL);
        DataSet ds1 = this.DA.GetDataSet(cmd);
        if (ds1 != null && ds1.Tables.Count > 0)
        {
            ddl_category.DataSource = ds1.Tables[0];
            ddl_category.DataTextField = "ComplaintCategoryName";
            ddl_category.DataValueField = "ComplaintCategoryid";

            ddl_category.DataBind();
            ddl_category.Items.Add(new ListItem("Select Your Category", "0"));
            ddl_category.SelectedValue = "0";

        }
    }

    public void loadstatus()
    {

        string str_URL = "   select * from StatusResponse order by StatusResponsekey";
        SqlCommand cmd = new SqlCommand(str_URL);
        DataSet ds1 = this.DA.GetDataSet(cmd);
        if (ds1 != null && ds1.Tables.Count > 0)
        {
            ddl_status.DataSource = ds1.Tables[0];
            ddl_status.DataTextField = "status";
            ddl_status.DataValueField = "StatusResponseId";

            ddl_status.DataBind();
            ddl_status.Items.Add(new ListItem("Select Status", "0"));
            ddl_status.SelectedValue = "0";

        }
    }

    protected void btn_request_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddl_status.SelectedValue == "1")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "infologia", "<script>alert('Please Select Response');</script>");
                return;
            }

            str_userid = this.SC.Userid;
            string date = DateTime.Now.ToString();
            string str_Sql = "update  IT_Complaint SET complaintresponse=@complaintresponse,modifiedon=@modifiedon,modifiedby=@modifiedby,complaintstatus=@complaintstatus where Complaintkey=@Complaintkey";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@Complaintkey", str_id);
            cmd.Parameters.AddWithValue("@complaintresponse", txt_response.InnerText.Trim());
            cmd.Parameters.AddWithValue("@complaintstatus", ddl_status.SelectedValue);
            cmd.Parameters.AddWithValue("@Modifiedby", this.str_userid);
            cmd.Parameters.AddWithValue("@Modifiedon", date);
            DA.ExecuteNonQuery(cmd);
            Response.Redirect("~/Admin/ComplaintResponse.aspx");
        }
        catch (Exception ex)
        {
        }

    }
}