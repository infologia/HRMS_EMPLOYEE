using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;

public partial class WEB_Employee_PermissionRequest : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_responsestatus = "";
    string str_userkey = "";
    string str_requestdate = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Permissions";
        }
    }

    protected void btn_perm_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txt_date.Text) ||
            string.IsNullOrWhiteSpace(txt_fromtime.Text) ||
            string.IsNullOrWhiteSpace(txt_totime.Text))
        {
           
            ShowError("Please select all required fields");
            return;
        }

        DateTime dt_date;
        try
        {
          
            dt_date = DateTime.ParseExact(txt_date.Text.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }
        catch
        {
            
            ShowError("Invalid date format. Please select a valid date.");
            return;
        }

        if (DateTime.Now.Date > dt_date.Date)
        {
           

            ShowError("Please select a valid (future or current) date");
            return;
        }

        DateTime fromTimeDT, toTimeDT;
        try
        {
            DateTime.TryParse(txt_fromtime.Text.ToString(), out fromTimeDT);
            DateTime.TryParse(txt_totime.Text.ToString(), out toTimeDT);
          
        }
        catch
        {
          
            ShowError("Invalid time format. Please select proper From and To time.");
            return;
        }

        TimeSpan fromTime = fromTimeDT.TimeOfDay;
        TimeSpan toTime = toTimeDT.TimeOfDay;

        TimeSpan companyStart = DateTime.ParseExact("09:30 AM", "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;
        TimeSpan companyEnd = DateTime.ParseExact("06:30 PM", "hh:mm tt", CultureInfo.InvariantCulture).TimeOfDay;

        if (fromTime < companyStart || toTime > companyEnd || fromTime >= toTime)
        {
           
            ShowError("Please select valid time range within company hours (09:30 AM - 06:30 PM)");
            return;
        }

        TimeSpan duration = toTime - fromTime;
        if (duration > TimeSpan.FromHours(1))
        {
           
            ShowError("Permission should be maximum 01:00 hr per day. Please apply leave instead.");
            return;
        }

        // Check monthly limit (3 hours)
        int currentMonth = DateTime.Now.Month;
        int currentYear = DateTime.Now.Year;

        string monthlyCheckQuery = @"
            SELECT ISNULL(SUM(DATEDIFF(MINUTE, Fromtime, Totime)), 0) AS UsedMinutes
            FROM IT_EmployeePermissionDetails
            WHERE Employeekey = @Employeekey
              AND Responsestatus IN (1, 2)
              AND MONTH(Requestdate) = @Month
              AND YEAR(Requestdate) = @Year";

        SqlCommand cmdMonthly = new SqlCommand(monthlyCheckQuery);
        cmdMonthly.Parameters.AddWithValue("@Employeekey", SC.Userid);
        cmdMonthly.Parameters.AddWithValue("@Month", currentMonth);
        cmdMonthly.Parameters.AddWithValue("@Year", currentYear);

        DataTable dtMonthly = DA.GetDataTable(cmdMonthly);
        int usedMinutes = 0;
        if (dtMonthly.Rows.Count > 0)
        {
            usedMinutes = Convert.ToInt32(dtMonthly.Rows[0]["UsedMinutes"]);
        }

        int monthlyLimitMinutes = 3 * 60; // 3 hours
        int requestedMinutes = (int)duration.TotalMinutes;
        int totalMinutes = usedMinutes + requestedMinutes;

        if (totalMinutes > monthlyLimitMinutes)
        {
            int remainingMinutes = monthlyLimitMinutes - usedMinutes;
            int remainingHours = remainingMinutes / 60;
            int remainingMins = remainingMinutes % 60;

            ShowError("Monthly permission limit exceeded. You have " + remainingHours + "h " + remainingMins + "m remaining this month.");
            return;
        }

        string str_request = "SELECT Requestdate FROM IT_EmployeePermissionDetails WHERE Requestdate = @Requestdate AND Employeekey = @Employeekey";
        SqlCommand cmd_check = new SqlCommand(str_request);
        cmd_check.Parameters.AddWithValue("@Requestdate", dt_date);
        cmd_check.Parameters.AddWithValue("@Employeekey", SC.Userid);

        DataTable dt_requestdate = DA.GetDataTable(cmd_check);
        if (dt_requestdate.Rows.Count > 0)
        {
           
            ShowError("Permission already taken for this date.");
            return;
        }

        string str_leavecheck = @"
            SELECT 1 
            FROM IT_EmployeeLeaveDetails 
            WHERE Employeekey = @Employeekey 
              AND Responsestatus IN ('1', '2')
              AND CAST(@Requestdate AS DATE) BETWEEN CAST(Fromdate AS DATE) AND CAST(Todate AS DATE)";
        SqlCommand cmd_leavecheck = new SqlCommand(str_leavecheck);
        cmd_leavecheck.Parameters.AddWithValue("@Employeekey", SC.Userid);
        cmd_leavecheck.Parameters.AddWithValue("@Requestdate", dt_date);

        DataTable dt_leavecheck = DA.GetDataTable(cmd_leavecheck);
        if (dt_leavecheck.Rows.Count > 0)
        {
            ShowError("You are on leave");
            return;
        }

        this.str_responsestatus = "1";
        this.str_userkey = SC.Userid.ToString();
        this.str_requestdate = dt_date.ToString("yyyy-MM-dd");

        string str_sql = @"
            INSERT INTO IT_EmployeePermissionDetails
            (Employeekey, permissionhourse, Createdby, Requestdate, Fromtime, Totime, Reason, Responsestatus, Createdon)
            VALUES (@Employeekey, @permissionhourse, @Createdby, @Requestdate, @Fromtime, @Totime, @Reason, @Responsestatus, @Createdon)";

        SqlCommand cmd = new SqlCommand(str_sql);
        cmd.Parameters.AddWithValue("@Employeekey", str_userkey);
        cmd.Parameters.AddWithValue("@Createdby", str_userkey);
        cmd.Parameters.AddWithValue("@Responsestatus", str_responsestatus);
        cmd.Parameters.AddWithValue("@Requestdate", dt_date); 
        cmd.Parameters.AddWithValue("@Createdon", DateTime.Now);

        cmd.Parameters.AddWithValue("@Fromtime", fromTimeDT.ToString("hh:mm tt"));
        cmd.Parameters.AddWithValue("@Totime", toTimeDT.ToString("hh:mm tt"));

        cmd.Parameters.AddWithValue("@Reason", txt_reasons.InnerText.Trim());
        cmd.Parameters.AddWithValue("@permissionhourse", duration.ToString(@"hh\:mm"));

        DA.ExecuteNonQuery(cmd);

        ShowSuccessAndRedirect(
       "Request updated successfully!",
       "/Employee/PermissionRequestView.aspx"



       );
    }
    private void ShowError(string message)
    {
        message = message.Replace("'", "\\'");
        ScriptManager.RegisterStartupScript(
            this,
            this.GetType(),
            "toastr_error",
            "showToastr('error','" + message + "');",
            true
        );
    }

    private void ShowSuccessAndRedirect(string message, string redirectUrl)
    {
        message = message.Replace("'", "\\'");
        ScriptManager.RegisterStartupScript(
            this,
            this.GetType(),
            "toastr_success",
            "showToastr('success','" + message + "');" +
            "setTimeout(function(){ window.location.href = '" + redirectUrl + "'; }, 2000);",
            true
        );
    }

    [WebMethod]
    public static string CheckLeaveForDate(string dateStr)
    {
        try
        {
            DateTime dt_date;
            if (DateTime.TryParseExact(dateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt_date))
            {
                SessionCustom SC = new SessionCustom();
                DataAccess DA1 = new DataAccess();

                string str_leavecheck = @"
                    SELECT 1 
                    FROM IT_EmployeeLeaveDetails 
                    WHERE Employeekey = @Employeekey 
                      AND Responsestatus IN ('1', '2')
                      AND CAST(@Requestdate AS DATE) BETWEEN CAST(Fromdate AS DATE) AND CAST(Todate AS DATE)";
                SqlCommand cmd_leavecheck = new SqlCommand(str_leavecheck);
                cmd_leavecheck.Parameters.AddWithValue("@Employeekey", SC.Userid);
                cmd_leavecheck.Parameters.AddWithValue("@Requestdate", dt_date);

                DataTable dt_leavecheck = DA1.GetDataTable(cmd_leavecheck);
                if (dt_leavecheck.Rows.Count > 0)
                {
                    return "true";
                }
            }
        }
        catch { }
        return "false";
    }
}
