using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WEB_Employee_PermissionRequestView : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;

    // Variables for new permission request
    string str_responsestatus = "";
    string str_userkey = "";
    string str_requestdate = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();
        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Permissions";

            // Populate Year DropDown
            int currentYear = DateTime.Now.Year;
            ddl_year.Items.Add(new ListItem("All Years", "0"));
            for (int y = currentYear; y >= 2020; y--)
            {
                ddl_year.Items.Add(new ListItem(y.ToString(), y.ToString()));
            }

            // Default to current year & current month
            ddl_year.SelectedValue = currentYear.ToString();
            ddl_month.SelectedValue = DateTime.Now.Month.ToString();

            LoadUpdateDropdown();
        }

        // Approved By = the manager who last responded to the request (Modifiedby), joined to IT_EmployeeRegister
        string str_query = @"SELECT
                                CONVERT(varchar,a.Requestdate,103) as request,
                                a.Fromtime,
                                a.Totime,
                                a.Reason,
                                a.responsereason,
                                a.Responsestatus,
                                a.employeepermissiondetailskey,
                                a.Permissionhourse,
                                CONVERT(varchar,a.Createdon,103) as appliedraw
                              FROM IT_EmployeePermissionDetails a
                              WHERE a.createdby=@createdby";

        if (ddl_year.SelectedValue != "0")
        {
            str_query += " AND YEAR(a.Requestdate) = @year";
        }
        if (ddl_month.SelectedValue != "0")
        {
            str_query += " AND MONTH(a.Requestdate) = @month";
        }

        str_query += " ORDER BY a.createdon DESC";

        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@createdby", SC.Userid);
        if (ddl_year.SelectedValue != "0")
        {
            cmd.Parameters.AddWithValue("@year", Convert.ToInt32(ddl_year.SelectedValue));
        }
        if (ddl_month.SelectedValue != "0")
        {
            cmd.Parameters.AddWithValue("@month", Convert.ToInt32(ddl_month.SelectedValue));
        }

        DataTable dt_dashboard = DA.GetDataTable(cmd);

        // ---- Summary counts (computed from the same dataset, no extra DB round trip) ----
        int total = dt_dashboard.Rows.Count;
        int pendingCount = 0, approvedCount = 0, rejectedCount = 0;
        foreach (DataRow row in dt_dashboard.Rows)
        {
            int status = Convert.ToInt16(row["responsestatus"].ToString());
            if (status == 1) pendingCount++;
            else if (status == 2) approvedCount++;
            else if (status == 3) rejectedCount++;
        }
        lbl_total.Text = total.ToString();
        lbl_pending.Text = pendingCount.ToString();
        lbl_approved.Text = approvedCount.ToString();
        lbl_rejected.Text = rejectedCount.ToString();

        DataSet ds = new DataSet();
        ds.Merge(dt_dashboard);
        if (dt_dashboard.Rows.Count > 0)
        {
            if (ds.Tables[0].Columns.Contains("Responsestatus"))
                ds.Tables[0].Columns.Add("ActiveText");
            ds.Tables[0].Columns.Add("ViewText");
            ds.Tables[0].Columns.Add("HideViewBtn");
            ds.Tables[0].Columns.Add("ReasonDisplay");
            ds.Tables[0].Columns.Add("ReasonFull");
            ds.Tables[0].Columns.Add("AppliedDate");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                String str_reason = dr["responsereason"].ToString();
                int activetype = Convert.ToInt16(dr["responsestatus"].ToString());
                if (activetype == 1)
                {
                    dr["ActiveText"] = "<span class='pr-pill pending' title='" + Server.HtmlEncode(str_reason) + "'><i class='icon-history'></i>Pending</span>";
                    dr["ViewText"] = "";
                    dr["HideViewBtn"] = "display: none;";
                }
                else if (activetype == 2)
                {
                    dr["ActiveText"] = "<span class='pr-pill approved' title='" + Server.HtmlEncode(str_reason) + "'><i class='icon-checkmark-circle'></i>Approved</span>";
                    dr["ViewText"] = "display: none;";
                    dr["HideViewBtn"] = "";
                }
                else if (activetype == 3)
                {
                    dr["ActiveText"] = "<span class='pr-pill rejected' title='" + Server.HtmlEncode(str_reason) + "'><i class='icon-cancel-circle2'></i>Rejected</span>";
                    dr["ViewText"] = "";
                    dr["HideViewBtn"] = "display: none;";
                }

                // Reason column: truncate for the grid, keep full text for the hover tooltip
                string fullReason = dr["Reason"] == null ? "" : dr["Reason"].ToString();
                string encodedFull = Server.HtmlEncode(fullReason);
                string displayReason = fullReason.Length > 40 ? fullReason.Substring(0, 40) + "..." : fullReason;
                dr["ReasonFull"] = encodedFull;
                dr["ReasonDisplay"] = Server.HtmlEncode(displayReason);

                dr["AppliedDate"] = dr["appliedraw"];
            }

            // Bind All
            this.PH.LoadGridItem(ds, PH_All, "PermissionRequestView.txt", "");

            // Bind Pending (Responsestatus = '1')
            DataView dvPending = new DataView(ds.Tables[0]);
            dvPending.RowFilter = "Responsestatus = '1'";
            DataSet dsPending = new DataSet();
            dsPending.Tables.Add(dvPending.ToTable());
            this.PH.LoadGridItem(dsPending, PH_Pending, "PermissionRequestView.txt", "");

            // Bind Approved (Responsestatus = '2')
            DataView dvApproved = new DataView(ds.Tables[0]);
            dvApproved.RowFilter = "Responsestatus = '2'";
            DataSet dsApproved = new DataSet();
            dsApproved.Tables.Add(dvApproved.ToTable());
            this.PH.LoadGridItem(dsApproved, PH_Approved, "PermissionRequestView.txt", "");

            // Bind Rejected (Responsestatus = '3')
            DataView dvRejected = new DataView(ds.Tables[0]);
            dvRejected.RowFilter = "Responsestatus = '3'";
            DataSet dsRejected = new DataSet();
            dsRejected.Tables.Add(dvRejected.ToTable());
            this.PH.LoadGridItem(dsRejected, PH_Rejected, "PermissionRequestView.txt", "");
        }
    }

    private void LoadUpdateDropdown()
    {
        string str_URL = "select * from StatusResponse order by StatusResponsekey";
        SqlCommand cmd = new SqlCommand(str_URL);
        DataSet ds1 = this.DA.GetDataSet(cmd);
        if (ds1 != null && ds1.Tables.Count > 0)
        {
            upd_ddl_category.DataSource = ds1.Tables[0];
            upd_ddl_category.DataTextField = "status";
            upd_ddl_category.DataValueField = "StatusResponseId";
            upd_ddl_category.DataBind();
            upd_ddl_category.Items.Insert(0, new ListItem("Select Status", "0"));
        }
    }
    [WebMethod] //Delete
    public static string DeleteProject(string str_employeepermissiondetailskey)
    {
        string str_Response = "0";
        try
        {
            DataAccess DA1;
            DA1 = new DataAccess();
            SaveQuery SAQ = new SaveQuery();
            SessionCustom SC = new SessionCustom();
            string str_Sql = "delete from IT_EmployeePermissionDetails where employeepermissiondetailskey=@employeepermissiondetailskey";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@employeepermissiondetailskey", str_employeepermissiondetailskey);
            DA1.ExecuteNonQuery(cmd);

            return str_Response = "1";

        }
        catch (Exception ex)
        {
            return str_Response;
        }
    }

    // --- NEW REQUEST LOGIC ---
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
            dt_date = DateTime.ParseExact(txt_date.Text.ToString(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
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

        TimeSpan companyStart = DateTime.ParseExact("09:30 AM", "hh:mm tt", System.Globalization.CultureInfo.InvariantCulture).TimeOfDay;
        TimeSpan companyEnd = DateTime.ParseExact("06:30 PM", "hh:mm tt", System.Globalization.CultureInfo.InvariantCulture).TimeOfDay;

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

        cmd.Parameters.AddWithValue("@Reason", txt_reasons.Text.Trim());
        cmd.Parameters.AddWithValue("@permissionhourse", duration.ToString(@"hh\:mm"));

        DA.ExecuteNonQuery(cmd);

        ShowSuccessAndRedirect("Request updated successfully!", "PermissionRequestView.aspx");
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
            "setTimeout(function(){ window.location.href = '" + redirectUrl + "'; }, 1500);",
            true
        );
    }

    [WebMethod]
    public static string CheckLeaveForDate(string dateStr)
    {
        try
        {
            DateTime dt_date;
            if (DateTime.TryParseExact(dateStr, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dt_date))
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

    // --- UPDATE / VIEW REQUEST LOGIC ---
    public class PermissionData
    {
        public string Requestdate { get; set; }
        public string Fromtime { get; set; }
        public string Totime { get; set; }
        public string Reason { get; set; }
        public string Responsestatus { get; set; }
        public string responsereason { get; set; }
    }

    [WebMethod]
    public static PermissionData GetPermissionDetails(string id)
    {
        PermissionData data = new PermissionData();
        try
        {
            DataAccess DA1 = new DataAccess();
            string str_query = "select CONVERT(varchar(10), Requestdate, 103) AS Requestdate,Fromtime,Totime,Reason,Responsestatus,responsereason from IT_EmployeePermissionDetails where employeepermissiondetailskey=@id";
            SqlCommand sc = new SqlCommand(str_query);
            sc.Parameters.AddWithValue("@id", id);
            DataTable ds = DA1.GetDataTable(sc);
            if (ds.Rows.Count > 0)
            {
                data.Requestdate = ds.Rows[0]["Requestdate"].ToString();
                data.Fromtime = ds.Rows[0]["Fromtime"].ToString();
                data.Totime = ds.Rows[0]["Totime"].ToString();
                data.Reason = ds.Rows[0]["Reason"].ToString();
                data.Responsestatus = ds.Rows[0]["Responsestatus"].ToString();
                data.responsereason = ds.Rows[0]["responsereason"].ToString();
            }
        }
        catch { }
        return data;
    }

    protected void btn_update_Click(object sender, EventArgs e)
    {
        string str_id = hdn_update_id.Value;
        if (string.IsNullOrEmpty(str_id))
        {
            ShowError("Invalid request ID.");
            return;
        }

        if (string.IsNullOrWhiteSpace(upd_txt_date.Text) || string.IsNullOrWhiteSpace(upd_txt_fromtime.Text) || string.IsNullOrWhiteSpace(upd_txt_totime.Text))
        {
            ShowError("Please select Required Field");
            return;
        }

        DateTime dt_current = DateTime.Now.Date;
        DateTime dt_date;

        bool isValid = DateTime.TryParseExact(
            upd_txt_date.Text.Trim(),
            new string[] { "dd/MM/yyyy", "yyyy-MM-dd", "dd-MM-yyyy" },
            System.Globalization.CultureInfo.InvariantCulture,
            System.Globalization.DateTimeStyles.None,
            out dt_date
        );

        if (!isValid)
        {
            ShowError("Please enter a valid date in DD/MM/YYYY format");
            return;
        }

        dt_date = dt_date.Date;
        if (dt_current > dt_date)
        {
            ShowError("Please select valid (future or current) date");
            return;
        }

        // Validate time
        TimeSpan tf = DateTime.Parse("09:30 AM").TimeOfDay;
        TimeSpan tt = DateTime.Parse("06:30 PM").TimeOfDay;

        string str_fromtime = upd_txt_fromtime.Text;
        string str_totime = upd_txt_totime.Text;

        TimeSpan tf1, tt1;
        try
        {
            tf1 = DateTime.Parse(str_fromtime).TimeOfDay;
            tt1 = DateTime.Parse(str_totime).TimeOfDay;
        }
        catch
        {
            ShowError("Invalid time format.");
            return;
        }

        if (tf > tf1 || tt < tt1 || tf > tt1 || tt < tf1 || tt1 < tf || tf1 > tt || tf1 == tt1 || tf1 > tt1 || tt1 < tf1)
        {
            ShowError("Please select valid time range within company hours (09:30 AM - 06:30 PM)");
            return;
        }

        // Check 1 hr duration
        TimeSpan duration = tt1 - tf1;
        if (duration > TimeSpan.FromHours(1))
        {
            ShowError("Permission should be maximum 01:00 hr per day. Please apply leave instead.");
            return;
        }

        DateTime modifiedOn = DateTime.Now;
        Guid permissionKey = Guid.Parse(str_id);

        string str_sql = "Update IT_EmployeePermissionDetails SET Fromtime=@Fromtime,permissionhourse=@permissionhourse,Totime=@Totime,Reason=@Reason,Requestdate=@Requestdate,modifiedon=@modifiedon,Modifiedby=@Modifiedby, Responsestatus='1' where employeepermissiondetailskey=@employeepermissiondetailskey";
        SqlCommand cmd = new SqlCommand(str_sql);
        cmd.Parameters.AddWithValue("@employeepermissiondetailskey", permissionKey);
        cmd.Parameters.AddWithValue("@Modifiedby", SC.Userid);
        cmd.Parameters.AddWithValue("@modifiedon", modifiedOn);
        cmd.Parameters.AddWithValue("@Requestdate", dt_date);
        cmd.Parameters.AddWithValue("@Fromtime", upd_txt_fromtime.Text);
        cmd.Parameters.AddWithValue("@permissionhourse", duration.ToString(@"hh\:mm"));
        cmd.Parameters.AddWithValue("@Totime", upd_txt_totime.Text);
        cmd.Parameters.AddWithValue("@Reason", upd_txt_reasons.Text.Trim());
        
        DA.ExecuteNonQuery(cmd);
        
        ShowSuccessAndRedirect("Request updated successfully!", "PermissionRequestView.aspx");
    }

    protected void ddl_filter_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Handled automatically on Page_Load on postback
    }
}
