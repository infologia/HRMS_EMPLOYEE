using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


public partial class WEB_Admin_Suggestionview : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_id = "";
    string str_userid = "";
    string str_suggestionresponse = "";
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
            this.loadstatus();
            this.assginvalue();

        }

    }
    private void assginvalue()
    {
        str_userid = this.SC.Userid;
        string str_leave = "select Suggestioncategory,reason,suggestionresponse,suggestionstatus from IT_Suggestion where Suggestionkey=@Suggestionkey";
        SqlCommand cmd = new SqlCommand(str_leave);
        cmd.Parameters.AddWithValue("@Suggestionkey", str_id);
        DataTable dt_complaint = this.DA.GetDataTable(cmd);

        if (dt_complaint.Rows.Count > 0)
        {
            txt_reason.InnerText = dt_complaint.Rows[0]["Reason"].ToString();
            ddl_category.SelectedValue = dt_complaint.Rows[0]["Suggestioncategory"].ToString();
            ddl_status.SelectedValue = dt_complaint.Rows[0]["suggestionstatus"].ToString();

            this.str_suggestionresponse = dt_complaint.Rows[0]["suggestionresponse"].ToString();
            if (str_suggestionresponse == "")
            {
                btn_request.Visible = true;
                txt_response.InnerText = str_suggestionresponse;
            }
            else
            {
                btn_request.Visible = false;
                txt_response.Attributes.Add("Readonly", "Readonly");
                ddl_status.Attributes.Add("disabled", "disabled");
                txt_response.InnerText = str_suggestionresponse;
            }


        }
    }

    public void loaddropdown()
    {

        string str_URL = "   select * from IT_SuggestionCategory order by SuggestionCategorykey";
        SqlCommand cmd = new SqlCommand(str_URL);
        DataSet ds1 = this.DA.GetDataSet(cmd);
        if (ds1 != null && ds1.Tables.Count > 0)
        {
            ddl_category.DataSource = ds1.Tables[0];
            ddl_category.DataTextField = "SuggestionCategoryName";
            ddl_category.DataValueField = "SuggestionCategoryid";

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
                ClientScript.RegisterStartupScript(this.GetType(), "Internal Tool", "<script>alert('Please Select Reason');</script>");
                return;
            }

            str_userid = this.SC.Userid;
            string date = DateTime.Now.ToString();
            string str_Sql = "update  IT_Suggestion SET suggestionresponse=@suggestionresponse,modifiedon=@modifiedon,modifiedby=@modifiedby,suggestionstatus=@suggestionstatus where Suggestionkey=@Suggestionkey";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@Suggestionkey", str_id);
            cmd.Parameters.AddWithValue("@suggestionresponse", txt_response.InnerHtml.Trim());
            cmd.Parameters.AddWithValue("@Modifiedby", this.str_userid);
            

            cmd.Parameters.AddWithValue("@suggestionstatus", ddl_status.SelectedValue);
            cmd.Parameters.AddWithValue("@Modifiedon", date);
            DA.ExecuteNonQuery(cmd);
            Response.Redirect("~/admin/Suggestionresponse.aspx");
        }
        catch (Exception ex)
        {
        }

    }
}