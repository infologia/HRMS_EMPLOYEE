using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_Latepermissionrequest : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_userkey = "";
    string str_responsestatus = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Late Permission";

            string today = DateTime.Now.ToString("yyyy-MM-dd");
            txt_date.Attributes["min"] = today;
            txt_date.Attributes["max"] = today;
            txt_date.Text = today;
        }
    }

protected void btn_perm_Click(object sender, EventArgs e)
{

        DateTime requestDate;
        string[] acceptedFormats = { "yyyy-MM-dd", "dd/MM/yyyy" };

        bool isParsed = DateTime.TryParseExact(
            txt_date.Text.Trim(),
            acceptedFormats,
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out requestDate
        );

        if (!isParsed)
        {
            ShowError("Invalid date format.");
            return;
        }

        requestDate = requestDate.Date;

        if (requestDate != DateTime.Now.Date)
        {
            ShowError("Late Permission can only be applied for today.");
            return;
        }


     
        DateTime fromTime, toTime;

    bool fromOk = DateTime.TryParse(txt_fromtime.Text.Trim(), out fromTime);
    bool toOk = DateTime.TryParse(txt_totime.Text.Trim(), out toTime);

    if (!fromOk || !toOk)
    {
        ShowError("Invalid From Time or To Time.");
        return;
    }

    if (toTime <= fromTime)
    {
        ShowError("To Time must be greater than From Time.");
        return;
    }
    TimeSpan permissionHours = toTime - fromTime;
    this.str_userkey = SC.Userid.ToString();
    this.str_responsestatus = "1"; 
        string sql = @"
        INSERT INTO IT_LatePermissionDetails
        (
            Employeekey,
            permissionhourse,
            Createdby,
            Requestdate,
            Fromtime,
            Totime,
            Reason,
Responsestatus
        )
        VALUES
        (
            @Employeekey,
            @permissionhourse,
            @Createdby,
            @Requestdate,
            @Fromtime,
            @Totime,
            @Reason,
@Responsestatus
        )";

    SqlCommand cmd = new SqlCommand(sql);
    cmd.Parameters.AddWithValue("@Employeekey", str_userkey);
    cmd.Parameters.AddWithValue("@Createdby", str_userkey);
    cmd.Parameters.AddWithValue("@Requestdate", requestDate);
    cmd.Parameters.AddWithValue("@Fromtime", fromTime.ToString("hh:mm tt"));
    cmd.Parameters.AddWithValue("@Totime", toTime.ToString("hh:mm tt"));
    cmd.Parameters.AddWithValue("@permissionhourse", permissionHours);
    cmd.Parameters.AddWithValue("@Reason", txt_reasons.InnerText.Trim());
    cmd.Parameters.Add("@Responsestatus", SqlDbType.NVarChar).Value = str_responsestatus;
        DA.ExecuteNonQuery(cmd);
    ScriptManager.RegisterStartupScript(
        this,
        this.GetType(),
        "toastr_redirect",
        "showToastr('success','Request created successfully!');" +
        "setTimeout(function(){ window.location.href = '/Admin/Latepermissionrequestview.aspx'; }, 2000);",
        true
    );
}
    private void ShowError(string message)
    {
        message = message.Replace("'", "\\'");

        ScriptManager.RegisterStartupScript(
            this,
            this.GetType(),
            "toastr_error",
            string.Format("showToastr('error','{0}');", message),
            true
        );
    }

    //protected void btn_perm_Click(object sender, EventArgs e)
    //{
    //    // ✅ Basic required field check
    //    if (string.IsNullOrWhiteSpace(txt_date.Text) ||
    //        string.IsNullOrWhiteSpace(txt_fromtime.Text) ||
    //        string.IsNullOrWhiteSpace(txt_totime.Text))
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "infologia",
    //            "<script>alert('Please select all required fields.');</script>");
    //        return;
    //    }

    //    // ✅ Date validation
    //    DateTime requestDate;
    //    string[] acceptedFormat = { "dd/MM/yyyy" };

    //    bool isParsed = DateTime.TryParseExact(
    //        txt_date.Text.Trim(),
    //        acceptedFormat,
    //        CultureInfo.InvariantCulture,
    //        DateTimeStyles.None,
    //        out requestDate
    //    );

    //    //if (!isParsed)
    //    if (!DateTime.TryParseExact(txt_date.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out requestDate))
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "infologia",
    //            "<script>alert('Invalid date format. Please select a valid date.');</script>");
    //        return;
    //    }

    //    // ✅ Convert to date only
    //    requestDate = requestDate.Date;

    //    // ✅ Late Permission Allowed Only For Today
    //    if (requestDate != DateTime.Now.Date)
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "infologia",
    //            "<script>alert('Late Permission can only be applied for today. Previous or future dates are not allowed.');</script>");
    //        return;
    //    }

    //    // ✅ Allowed submission window (Today between 09:30 AM and 10:00 AM)
    //    TimeSpan allowedStart = TimeSpan.Parse("09:30");
    //    TimeSpan allowedEnd = TimeSpan.Parse("10:00");
    //    TimeSpan nowTime = DateTime.Now.TimeOfDay;

    //    if (nowTime < allowedStart || nowTime > allowedEnd)
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "infologia",
    //            "<script>alert('Late Permission requests are allowed only between 09:30 AM and 10:00 AM.');</script>");
    //        return;
    //    }

    //    // ✅ Parse user-entered time
    //    TimeSpan fromTime, toTime;
    //    try
    //    {
    //        fromTime = DateTime.Parse(txt_fromtime.Text.Trim()).TimeOfDay;
    //        toTime = DateTime.Parse(txt_totime.Text.Trim()).TimeOfDay;
    //    }
    //    catch
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "infologia",
    //            "<script>alert('Please enter a valid time.');</script>");
    //        return;
    //    }

    //    // ✅ User selected time must also fall within allowed range
    //    if (fromTime < allowedStart || toTime > allowedEnd)
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "infologia",
    //            "<script>alert('Please select time only between 09:30 AM and 10:00 AM.');</script>");
    //        return;
    //    }

    //    if (fromTime >= toTime)
    //    {
    //        ClientScript.RegisterStartupScript(this.GetType(), "infologia",
    //            "<script>alert('To Time must be greater than From Time.');</script>");
    //        return;
    //    }

    //    // ✅ Calculate permission duration
    //    TimeSpan permissionHours = toTime - fromTime;

    //    // ✅ Insert into database
    //    this.str_userkey = SC.Userid.ToString();

    //    string sql = @"INSERT INTO IT_LatePermissionDetails
    //               (Employeekey, permissionhourse, Createdby, Requestdate, Fromtime, Totime, Reason)
    //               VALUES (@Employeekey, @permissionhourse, @Createdby, @Requestdate, @Fromtime, @Totime, @Reason)";

    //    SqlCommand cmd = new SqlCommand(sql);
    //    cmd.Parameters.AddWithValue("@Employeekey", str_userkey);
    //    cmd.Parameters.AddWithValue("@Createdby", str_userkey);
    //    cmd.Parameters.AddWithValue("@Requestdate", requestDate);
    //    cmd.Parameters.AddWithValue("@Fromtime", txt_fromtime.Text.Trim());
    //    cmd.Parameters.AddWithValue("@Totime", txt_totime.Text.Trim());
    //    cmd.Parameters.AddWithValue("@Reason", txt_reasons.InnerText.Trim());
    //    cmd.Parameters.AddWithValue("@permissionhourse", permissionHours);

    //    DA.ExecuteNonQuery(cmd);

    //    // ✅ Redirect after success
    //    Response.Redirect("~/Employee/LatePermissionRequestView.aspx");
    //}

}
