using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WEB_Admin_LeaveView : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_id = "";
    string str_userid = "";
    string str_responsereason = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();


        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Employee Monitoring";
       }

        if (Request.QueryString["key"] != null && Request.QueryString["key"] != "")
        {

            this.str_id = Request.QueryString["key"].ToString();
        }
        if (!IsPostBack)
        {
            this.loaddropdown();
            this.assignvalue();
        }
    }

    private void assignvalue()
    {
        str_userid = this.SC.Userid;

        string str_leave = "select Responsestatus, CONVERT(varchar(10), fromdate, 103) AS fromdate, CONVERT(varchar(10), todate, 103) AS todate, reason, responsestatus, responsereason, LeaveType, LeaveCategoryId from IT_EmployeeLeaveDetails where Employeeleavedetailskey=@Employeeleavedetailskey";
        SqlCommand cmd = new SqlCommand(str_leave);
        cmd.Parameters.AddWithValue("@Employeeleavedetailskey", str_id);
        DataTable dt_leave = this.DA.GetDataTable(cmd);

        if (dt_leave.Rows.Count > 0)
        {

            txt_fromdate.Text = dt_leave.Rows[0]["Fromdate"].ToString();
            txt_todate.Text = dt_leave.Rows[0]["Todate"].ToString();
            txt_reason.InnerText = dt_leave.Rows[0]["Reason"].ToString();
            ddl_category.SelectedValue = dt_leave.Rows[0]["responsestatus"].ToString();

            // Leave Type
            string leaveTypeVal = dt_leave.Rows[0]["LeaveType"] != DBNull.Value ? dt_leave.Rows[0]["LeaveType"].ToString() : "";
            switch (leaveTypeVal)
            {
                case "0": txt_leavetype.Text = "Half Day (Forenoon)"; break;
                case "1": txt_leavetype.Text = "Half Day (Afternoon)"; break;
                case "2": txt_leavetype.Text = "Full Day"; break;
                default:  txt_leavetype.Text = "-"; break;
            }

            // Leave Category
            if (dt_leave.Rows[0]["LeaveCategoryId"] != DBNull.Value)
            {
                SqlCommand cmdCat = new SqlCommand("SELECT Name FROM LeaveCategory WHERE Id = @Id");
                cmdCat.Parameters.AddWithValue("@Id", dt_leave.Rows[0]["LeaveCategoryId"]);
                DataTable dtCat = this.DA.GetDataTable(cmdCat);
                txt_leavecategory.Text = (dtCat != null && dtCat.Rows.Count > 0) ? dtCat.Rows[0]["Name"].ToString() : "-";
            }
            else
            {
                txt_leavecategory.Text = "-";
            }

            this.str_responsereason = dt_leave.Rows[0]["responsereason"].ToString();
            if (str_responsereason == "")
            {
                btn_request.Visible = true;
                txt_reason1.InnerText = str_responsereason;
            }
            else
            {
                btn_request.Visible = false;
                txt_reason1.Attributes.Add("Readonly", "Readonly");
                ddl_category.Attributes.Add("disabled", "disabled");
                txt_reason1.InnerText = str_responsereason;
            }

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

    protected void btn_request_Click(object sender, EventArgs e)
    {

        try
        {
            if (ddl_category.SelectedValue == "1")
            {
                ClientScript.RegisterStartupScript(this.GetType(), "Internal Tool", "<script>alert('Please Select Response');</script>");
                return;
            }
            str_userid = this.SC.Userid;
            string date = DateTime.Now.ToString();
            //DateTime dt_datetime= DateTime.ParseExact(date, "dd-MM-yyyy hh:mm:ss", CultureInfo.InvariantCulture);

            DateTime dateTime10 = Convert.ToDateTime(date);
            string str_Sql = "update  IT_EmployeeLeaveDetails SET responsestatus=@responsestatus,responsereason=@responsereason,modifiedon=@modifiedon,modifiedby=@modifiedby where Employeeleavedetailskey=@Employeeleavedetailskey";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@Employeeleavedetailskey", str_id);
            cmd.Parameters.AddWithValue("@responsestatus", ddl_category.SelectedValue);
            cmd.Parameters.AddWithValue("@responsereason", txt_reason1.InnerText.Trim());
            cmd.Parameters.AddWithValue("@Modifiedby", this.str_userid);
            cmd.Parameters.AddWithValue("@Modifiedon", dateTime10);
            DA.ExecuteNonQuery(cmd);
            Response.Redirect("~/Admin/LeaveResponse.aspx");
        }
        catch (Exception ex)
        {
        }

    }
}
