using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WEB_Employee_Complaintstatus : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_id = "";
    string str_userid = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.str_userid = this.SC.Userid;
        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Self Services";
        }

        if (Request.QueryString["key"] != null && Request.QueryString["key"] != "")
        {

            this.str_id = Request.QueryString["key"].ToString();
        }
        if (!IsPostBack)
        {
            this.loaddropdown();
            this.loadstatus();
            this.AssignValusToControls();
            this.complaint();
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
    public void complaint()
    {
        string str_leave = "select complaintstatus from IT_Complaint where complaintkey=@complaintkey";
        SqlCommand cmd = new SqlCommand(str_leave);
        cmd.Parameters.AddWithValue("@complaintkey", str_id);
        DataTable dt_complaint = this.DA.GetDataTable(cmd);

        if (dt_complaint.Rows.Count > 0)
        {
            string day = dt_complaint.Rows[0]["complaintstatus"].ToString();
            int comp1 = Convert.ToInt32(day);
            if (comp1 == 1)
            {
                comp.Visible = false;

            }
            else
            {
                comp.Visible = true;
                btn_update.Visible = false;
                txt_reason1.Attributes.Add("readonly", "readonly");
                ddl_category.Attributes.Add("disabled", "disabled");
            }
        }
    }
    public void AssignValusToControls()
    {
        string str_leave = "select complaintcategory,reason,Complaintresponse,complaintstatus from IT_Complaint where complaintkey=@complaintkey";
        SqlCommand cmd = new SqlCommand(str_leave);
        cmd.Parameters.AddWithValue("@complaintkey", str_id);
        DataTable dt_complaint = this.DA.GetDataTable(cmd);

        if (dt_complaint.Rows.Count > 0)
        {
            txt_reason1.InnerText = dt_complaint.Rows[0]["Reason"].ToString();
            ddl_category.SelectedValue = dt_complaint.Rows[0]["complaintcategory"].ToString();
            ddl_status.SelectedValue = dt_complaint.Rows[0]["complaintstatus"].ToString();
            txt_response.InnerText = dt_complaint.Rows[0]["Complaintresponse"].ToString();

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
    protected void btn_update_Click(object sender, EventArgs e)
    {
        try
        {
            int status = 1;
            string date = DateTime.Now.ToString();
            string str_Sql = "update  IT_Complaint SET ComplaintCategory=@ComplaintCategory,Reason=@Reason,modifiedon=@modifiedon,modifiedby=@modifiedby,complaintstatus=@complaintstatus where complaintkey=@complaintkey";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@ComplaintCategory", ddl_category.SelectedValue);
            cmd.Parameters.AddWithValue("@Reason", txt_reason1.InnerText.Trim());
            cmd.Parameters.AddWithValue("@complaintstatus", status);
            cmd.Parameters.AddWithValue("@complaintkey", this.str_id);
            cmd.Parameters.AddWithValue("@Modifiedby", this.str_userid);
            cmd.Parameters.AddWithValue("@Modifiedon", date);
            DA.ExecuteNonQuery(cmd);
       
            Response.Redirect("~/employee/complaintresponseview.aspx");
        }
        catch (Exception ex)
        {
        }
    }
}