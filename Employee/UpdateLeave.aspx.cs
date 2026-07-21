using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
public partial class WEB_Employee_UpdateLeave : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_queryid = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Leaves";
        }

        if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
        {

            this.str_queryid = Request.QueryString["id"].ToString();
        }
        if (!IsPostBack)
        {
            
            this.loaddropdown();
            this.LoadValue();
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

    private void LoadValue()
    {
        string str_query = "SELECT Employeekey, Reason, Responsereason, Responsestatus, CONVERT(varchar(10), Fromdate, 103) AS Fromdate, CONVERT(varchar(10), Todate, 103)   AS Todate,LeaveType FROM IT_EmployeeLeaveDetails  where employeeleavedetailskey=@employeeleavedetailskey";
        SqlCommand sc = new SqlCommand(str_query);
        sc.Parameters.AddWithValue("@employeeleavedetailskey", str_queryid);
        DataTable dt_loadvalue = this.DA.GetDataTable(sc);
        if (dt_loadvalue.Rows.Count > 0)
        {
            DataRow dr = dt_loadvalue.Rows[0];
            string str_leavestus = dr["responsestatus"].ToString();
            if (str_leavestus == "1")
            {
                div_Reson.Visible = false;
                ddl_leavetype.SelectedValue = dr["LeaveType"].ToString();
                txt_fromdate.Text = dr["Fromdate"].ToString();
                txt_todate.Text = dr["Todate"].ToString();
                txt_reason.InnerText = dr["Reason"].ToString();
                btn_request.Visible = true;
            }
            else
            {
                div_Reson.Visible = true;
                ddl_leavetype.SelectedValue = dr["LeaveType"].ToString();
                txt_fromdate.Text = dr["Fromdate"].ToString();
                txt_todate.Text = dr["Todate"].ToString();
                txt_reason.InnerText = dr["Reason"].ToString();


                txt_reason1.Attributes.Add("Readonly", "Readonly");
                ddl_category.Attributes.Add("disabled", "disabled");
                txt_fromdate.Attributes.Add("Readonly", "Readonly");
                txt_todate.Attributes.Add("Readonly", "Readonly");
                txt_reason.Attributes.Add("Readonly", "Readonly");


                ddl_category.SelectedValue = str_leavestus;
                txt_reason1.InnerText = dt_loadvalue.Rows[0]["responsereason"].ToString();
                btn_request.Visible = false;
            }

           
        }
    }

protected void btn_request_Click(object sender, EventArgs e)
{
    DateTime dt_fromdate, dt_todate;

    string format = "dd/MM/yyyy";
    CultureInfo culture = CultureInfo.InvariantCulture;

    if (!DateTime.TryParseExact(txt_fromdate.Text.Trim(), format, culture,
                                DateTimeStyles.None, out dt_fromdate) ||
        !DateTime.TryParseExact(txt_todate.Text.Trim(), format, culture,
                                DateTimeStyles.None, out dt_todate))
    {
        ClientScript.RegisterStartupScript(this.GetType(), "alert",
            "<script>alert('Invalid date format. Please use dd/MM/yyyy');</script>");
        return;
    }
        if (ddl_leavetype.SelectedValue == "0" || ddl_leavetype.SelectedValue == "1")
        {
            if (dt_fromdate != dt_todate)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert",
                    "<script>alert('For Forenoon/Afternoon leave, From Date and To Date must be the same.');</script>");
                return;
            }
        }

        // Normalize dates
        dt_fromdate = dt_fromdate.Date;
    dt_todate = dt_todate.Date;

        decimal NoOfDays = 0;

        if (ddl_leavetype.SelectedValue == "0" || ddl_leavetype.SelectedValue == "1")
        {
            NoOfDays = 0.5m;
        }
        else
        {
            NoOfDays = (dt_todate.AddDays(1) - dt_fromdate).Days;
        }


        DateTime dt_current = DateTime.Now.Date;

    // Validations
    if (dt_current > dt_fromdate)
    {
        ShowAlert("Please select a valid From date");
        return;
    }

    if (dt_current > dt_todate)
    {
        ShowAlert("Please select a valid To date");
        return;
    }

    // Allow same date, block only greater
    if (dt_fromdate > dt_todate)
    {
        ShowAlert("From date should not be greater than To date");
        return;
    }

    // DB update
    string str_userid = SC.Userid.ToString();

    SqlCommand cmd = new SqlCommand(@"
        UPDATE IT_EmployeeLeaveDetails
        SET Fromdate = @Fromdate,
            Todate = @Todate,
            Reason = @Reason,
            Modifiedon = @Modifiedon,
            Modifiedby = @Modifiedby,
            leavedays = @leavedays,LeaveType=@LeaveType
        WHERE Employeeleavedetailskey = @Employeeleavedetailskey");

    cmd.Parameters.AddWithValue("@Employeeleavedetailskey", this.str_queryid);
    cmd.Parameters.AddWithValue("@Fromdate", dt_fromdate);
    cmd.Parameters.AddWithValue("@Todate", dt_todate);
    cmd.Parameters.AddWithValue("@leavedays", NoOfDays);
    cmd.Parameters.AddWithValue("@Reason", txt_reason.InnerText);
    cmd.Parameters.AddWithValue("@Modifiedon", DateTime.UtcNow);
    cmd.Parameters.AddWithValue("@Modifiedby", str_userid);
    cmd.Parameters.AddWithValue("@LeaveType", ddl_leavetype.SelectedValue);
    DA.ExecuteNonQuery(cmd);
    Response.Redirect("~/Employee/LeaveRequestView.aspx");
}
private void ShowAlert(string message)
{
    ClientScript.RegisterStartupScript(this.GetType(), "alert",
        "<script>alert('" + message + "');</script>");
}
}