using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WEB_Employee_LeaveRequestView : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Leaves";

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
                
            LoadLeaveCategory();
            LoadStatusDropdown();
        }

        BindLeaveGrids();
    }

    private void LoadLeaveCategory()
    {
        string query = "SELECT Id, Name FROM LeaveCategory ORDER BY Id";
        SqlCommand cmd = new SqlCommand(query);
        DataTable dt = DA.GetDataTable(cmd);
        
        ddl_leavecategory.Items.Clear();
        ddl_leavecategory.Items.Add(new ListItem("Select Leave Category", "0"));

        upd_ddl_leavecategory.Items.Clear();
        upd_ddl_leavecategory.Items.Add(new ListItem("Select Leave Category", "0"));
        
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                ddl_leavecategory.Items.Add(new ListItem(row["Name"].ToString(), row["Id"].ToString()));
                upd_ddl_leavecategory.Items.Add(new ListItem(row["Name"].ToString(), row["Id"].ToString()));
            }
        }
    }

    private void LoadStatusDropdown()
    {
        string str_URL = "SELECT * FROM StatusResponse ORDER BY StatusResponsekey";
        SqlCommand cmd = new SqlCommand(str_URL);
        DataSet ds1 = this.DA.GetDataSet(cmd);
        if (ds1 != null && ds1.Tables.Count > 0)
        {
            upd_ddl_category.DataSource = ds1.Tables[0];
            upd_ddl_category.DataTextField = "status";
            upd_ddl_category.DataValueField = "StatusResponseId";
            upd_ddl_category.DataBind();
            upd_ddl_category.Items.Insert(0, new ListItem("Select Status", "0"));
            upd_ddl_category.SelectedValue = "0";
        }
    }

    private void BindLeaveGrids()
    {
        string str_userid = this.SC.Userid;
        string str_query = @"
        SELECT b.firstname + ' ' + b.lastname AS name,
               CONVERT(Varchar, a.Fromdate, 103) AS Invalue,
               CONVERT(Varchar, a.Todate, 103) AS Outvalue,
               a.Responsestatus,
               a.employeeleavedetailskey,
               a.leavedays,
               a.responsereason
        FROM IT_EmployeeLeaveDetails a
        LEFT JOIN IT_EmployeeRegister b ON a.createdby = b.Employeekey
        WHERE a.Employeekey = @EmpKey";

        if (ddl_year.SelectedValue != "0")
        {
            str_query += " AND (YEAR(a.Fromdate) = @year OR YEAR(a.Todate) = @year)";
        }
        if (ddl_month.SelectedValue != "0")
        {
            str_query += " AND (MONTH(a.Fromdate) = @month OR MONTH(a.Todate) = @month)";
        }

        str_query += " ORDER BY a.createdon DESC";

        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@EmpKey", str_userid);
        if (ddl_year.SelectedValue != "0")
        {
            cmd.Parameters.AddWithValue("@year", Convert.ToInt32(ddl_year.SelectedValue));
        }
        if (ddl_month.SelectedValue != "0")
        {
            cmd.Parameters.AddWithValue("@month", Convert.ToInt32(ddl_month.SelectedValue));
        }

        DataTable dt_dashboard = DA.GetDataTable(cmd);
        
        if (!dt_dashboard.Columns.Contains("ActiveText"))
            dt_dashboard.Columns.Add("ActiveText");
        if (!dt_dashboard.Columns.Contains("ViewText"))
            dt_dashboard.Columns.Add("ViewText");
        if (!dt_dashboard.Columns.Contains("HideViewBtn"))
            dt_dashboard.Columns.Add("HideViewBtn");

        int pendingCount = 0, approvedCount = 0, rejectedCount = 0, overviewCount = 0;
        
        DataTable dt_Pending = dt_dashboard.Clone();
        DataTable dt_Approved = dt_dashboard.Clone();
        DataTable dt_Rejected = dt_dashboard.Clone();
        
        if (dt_dashboard.Rows.Count > 0)
        {
            foreach (DataRow dr in dt_dashboard.Rows)
            {
                string str_reason = dr["responsereason"].ToString();
                int activetype = Convert.ToInt16(dr["responsestatus"].ToString());

                if (activetype == 1)
                {
                    dr["ActiveText"] = "<span class='label label-info' title='" + Server.HtmlEncode(str_reason) + "'>Pending</span>";
                    dr["ViewText"] = "";
                    dr["HideViewBtn"] = "display: none;";
                    dt_Pending.ImportRow(dr);
                    pendingCount++;
                }
                else if (activetype == 2)
                {
                    dr["ActiveText"] = "<span class='label label-success' title='" + Server.HtmlEncode(str_reason) + "'>Approved</span>";
                    dr["ViewText"] = "display: none;";
                    dr["HideViewBtn"] = "";
                    dt_Approved.ImportRow(dr);
                    approvedCount++;
                }
                else if (activetype == 3)
                {
                    dr["ActiveText"] = "<span class='label label-danger' title='" + Server.HtmlEncode(str_reason) + "'>Rejected</span>";
                    dr["ViewText"] = "";
                    dr["HideViewBtn"] = "display: none;";
                    dt_Rejected.ImportRow(dr);
                    rejectedCount++;
                }
                overviewCount++;
            }

            DataSet ds_Pending = new DataSet(); ds_Pending.Tables.Add(dt_Pending);
            DataSet ds_Approved = new DataSet(); ds_Approved.Tables.Add(dt_Approved);
            DataSet ds_Rejected = new DataSet(); ds_Rejected.Tables.Add(dt_Rejected);
            DataSet ds_Overview = new DataSet(); ds_Overview.Tables.Add(dt_dashboard);

            this.PH.LoadGridItem(ds_Pending, PH_Pending, "LeaveRequestView.txt", "");
            this.PH.LoadGridItem(ds_Approved, PH_Approved, "LeaveRequestView.txt", "");
            this.PH.LoadGridItem(ds_Rejected, PH_Rejected, "LeaveRequestView.txt", "");
            this.PH.LoadGridItem(ds_Overview, PH_Overview, "LeaveRequestView.txt", "");
        }

        badge_pending.Text = pendingCount.ToString();
        badge_approved.Text = approvedCount.ToString();
        badge_rejected.Text = rejectedCount.ToString();
        badge_overview.Text = overviewCount.ToString();
    }

    [WebMethod]
    public static string DeleteProject(string str_employeeleavedetailskey)
    {
        string str_Response = "0";
        try
        {
            DataAccess DA1 = new DataAccess();
            string str_Sql = "delete from IT_EmployeeLeaveDetails where employeeleavedetailskey=@employeeleavedetailskey";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@employeeleavedetailskey", str_employeeleavedetailskey);
            DA1.ExecuteNonQuery(cmd);

            return str_Response = "1";
        }
        catch (Exception ex)
        {
            return str_Response;
        }
    }

    [WebMethod]
    public static object GetLeaveDetails(string id)
    {
        try
        {
            DataAccess DA1 = new DataAccess();
            string query = "SELECT Employeekey, Reason, Responsereason, Responsestatus, CONVERT(varchar(10), Fromdate, 103) AS Fromdate, CONVERT(varchar(10), Todate, 103) AS Todate, LeaveType, LeaveCategoryId FROM IT_EmployeeLeaveDetails WHERE employeeleavedetailskey = @id";
            SqlCommand cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@id", id);
            DataTable dt = DA1.GetDataTable(cmd);

            if (dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                return new
                {
                    Fromdate = dr["Fromdate"].ToString(),
                    Todate = dr["Todate"].ToString(),
                    Reason = dr["Reason"].ToString(),
                    LeaveType = dr["LeaveType"].ToString(),
                    LeaveCategoryId = dr["LeaveCategoryId"] != DBNull.Value ? dr["LeaveCategoryId"].ToString() : "0",
                    Responsestatus = dr["Responsestatus"].ToString(),
                    Responsereason = dr["Responsereason"].ToString()
                };
            }
        }
        catch { }
        return null;
    }

    [WebMethod]
    public static string CheckLeaveDateExists(string dateStr, string type)
    {
        try
        {
            DateTime dt;
            if (DateTime.TryParseExact(dateStr, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                DataAccess DA1 = new DataAccess();
                SessionCustom SC1 = new SessionCustom();
                string empKey = SC1.Userid.ToString();

                string query = "";
                if (type == "from")
                {
                    query = "SELECT 1 FROM IT_EmployeeLeaveDetails WHERE Fromdate = @date AND Employeekey = @empKey";
                }
                else
                {
                    query = "SELECT 1 FROM IT_EmployeeLeaveDetails WHERE Todate = @date AND Employeekey = @empKey";
                }

                SqlCommand cmd = new SqlCommand(query);
                cmd.Parameters.AddWithValue("@date", dt.Date);
                cmd.Parameters.AddWithValue("@empKey", empKey);
                
                DataTable dtResult = DA1.GetDataTable(cmd);
                if (dtResult != null && dtResult.Rows.Count > 0)
                {
                    return "true";
                }
            }
        }
        catch { }
        return "false";
    }

    protected void btn_update_Click(object sender, EventArgs e)
    {
        DateTime dt_fromdate, dt_todate;
        string format = "dd/MM/yyyy";
        CultureInfo culture = CultureInfo.InvariantCulture;

        if (!DateTime.TryParseExact(upd_txt_fromdate.Text.Trim(), format, culture, DateTimeStyles.None, out dt_fromdate) ||
            !DateTime.TryParseExact(upd_txt_todate.Text.Trim(), format, culture, DateTimeStyles.None, out dt_todate))
        {
            ShowAlert("Invalid date format. Please use dd/MM/yyyy");
            return;
        }
        if (upd_ddl_leavetype.SelectedValue == "0" || upd_ddl_leavetype.SelectedValue == "1")
        {
            if (dt_fromdate != dt_todate)
            {
                ShowAlert("For Forenoon/Afternoon leave, From Date and To Date must be the same.");
                return;
            }
        }

        dt_fromdate = dt_fromdate.Date;
        dt_todate = dt_todate.Date;

        decimal NoOfDays = 0;
        if (upd_ddl_leavetype.SelectedValue == "0" || upd_ddl_leavetype.SelectedValue == "1")
            NoOfDays = 0.5m;
        else
            NoOfDays = (dt_todate.AddDays(1) - dt_fromdate).Days;

        DateTime dt_current = DateTime.Now.Date;

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
        if (dt_fromdate > dt_todate)
        {
            ShowAlert("From date should not be greater than To date");
            return;
        }

        string str_userid = SC.Userid.ToString();
        SqlCommand cmd = new SqlCommand(@"
            UPDATE IT_EmployeeLeaveDetails
            SET Fromdate = @Fromdate,
                Todate = @Todate,
                Reason = @Reason,
                Modifiedon = @Modifiedon,
                Modifiedby = @Modifiedby,
                leavedays = @leavedays,
                LeaveType = @LeaveType,
                LeaveCategoryId = @LeaveCategoryId,
                Responsestatus = '1'
            WHERE Employeeleavedetailskey = @Employeeleavedetailskey");

        cmd.Parameters.AddWithValue("@Employeeleavedetailskey", hdn_update_id.Value);
        cmd.Parameters.AddWithValue("@Fromdate", dt_fromdate);
        cmd.Parameters.AddWithValue("@Todate", dt_todate);
        cmd.Parameters.AddWithValue("@leavedays", NoOfDays);
        cmd.Parameters.AddWithValue("@Reason", upd_txt_reason.InnerText);
        cmd.Parameters.AddWithValue("@Modifiedon", DateTime.UtcNow);
        cmd.Parameters.AddWithValue("@Modifiedby", str_userid);
        cmd.Parameters.AddWithValue("@LeaveType", upd_ddl_leavetype.SelectedValue);
        cmd.Parameters.AddWithValue("@LeaveCategoryId", int.Parse(upd_ddl_leavecategory.SelectedValue));
        DA.ExecuteNonQuery(cmd);

        ShowSuccessAndRedirect("Leave Request Updated Successfully!", "LeaveRequestView.aspx");
    }

    public List<DateTime> GetDatesBetween(DateTime startDate, DateTime endDate)
    {
        List<DateTime> allDates = new List<DateTime>();
        for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
        {
            allDates.Add(date.Date);
        }
        return allDates;
    }

    protected void btn_request_Click(object sender, EventArgs e)
    {
        DateTime dt_date;
        DateTime dtToInclusive;
        Guid myGuid = Guid.Parse(SC.Userid);

        if (!DateTime.TryParseExact(txt_fromdate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt_date))
        {
            ShowAlert("Invalid From date");
            return;
        }

        if (!DateTime.TryParseExact(txt_todate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtToInclusive))
        {
            ShowAlert("Invalid To date");
            return;
        }
        if (ddl_leavetype.SelectedValue == "0" || ddl_leavetype.SelectedValue == "1")
        {
            if (dt_date.Date != dtToInclusive.Date)
            {
                ShowAlert("For Forenoon/Afternoon leave, From Date and To Date must be the same.");
                return;
            }
        }

        dt_date = dt_date.Date;
        dtToInclusive = dtToInclusive.Date;
        DateTime dtExclusive = dtToInclusive.AddDays(1);
        
        TimeSpan ts = dtExclusive - dt_date;
        int totalDaysBetween = ts.Days;
        int sundays = 0;

        for (DateTime d = dt_date; d < dtExclusive; d = d.AddDays(1))
        {
            if (d.DayOfWeek == DayOfWeek.Sunday)
                sundays++;
        }

        DataTable dtHolidays = new DataTable();
        List<DateTime> holidayDates = new List<DateTime>();
        int holidayCount = 0;

        using (SqlConnection conn = new SqlConnection(DA.ConnectionString))
        {
            conn.Open();

            using (SqlCommand cmdH = new SqlCommand("SELECT Holidays FROM IT_Holidays WHERE Holidays >= @fromDate AND Holidays < @toDate", conn))
            {
                cmdH.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = dt_date;
                cmdH.Parameters.Add("@toDate", SqlDbType.DateTime).Value = dtExclusive;
                using (SqlDataAdapter da = new SqlDataAdapter(cmdH))
                {
                    da.Fill(dtHolidays);
                }
            }

            if (dtHolidays.Rows.Count > 0)
            {
                holidayCount = dtHolidays.Rows.Count;
                foreach (DataRow r in dtHolidays.Rows)
                {
                    DateTime h;
                    if (DateTime.TryParse(r["Holidays"].ToString(), out h))
                    {
                        holidayDates.Add(h.Date);
                    }
                }
            }
            int secondSaturdayCount = 0;

            for (DateTime d = dt_date; d < dtExclusive; d = d.AddDays(1))
            {
                if (d.DayOfWeek == DayOfWeek.Saturday && d.Day >= 8 && d.Day <= 14)
                    secondSaturdayCount++;
            }
            decimal NoOfDays = 0;

            if (ddl_leavetype.SelectedValue == "0" || ddl_leavetype.SelectedValue == "1")
            {
                NoOfDays = 0.5m;
            }
            else
            {
                NoOfDays = totalDaysBetween - holidayCount - sundays - secondSaturdayCount;
                if (NoOfDays < 0) NoOfDays = 0;
            }

            string str_responsestatus = "1";
            string str_userkey = SC.Userid.ToString();

            DateTime dt_current = DateTime.Now.Date;
            DateTime dt_fromdate = dt_date;
            DateTime dt_todate = dtToInclusive;

            if (dt_fromdate < dt_current)
            {
                ShowAlert("Please select a valid from date");
                return;
            }
            if (dt_todate < dt_current)
            {
                ShowAlert("Please select a valid to date");
                return;
            }
            if (dt_fromdate > dt_todate)
            {
                ShowAlert("From date cannot be greater than to date");
                return;
            }

            using (SqlCommand cmdChkFrom = new SqlCommand("SELECT 1 FROM IT_EmployeeLeaveDetails WHERE Fromdate = @fromDate AND Employeekey = @empKey", conn))
            {
                cmdChkFrom.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = dt_fromdate;
                cmdChkFrom.Parameters.Add("@empKey", SqlDbType.VarChar).Value = SC.Userid.ToString();
                object o = cmdChkFrom.ExecuteScalar();
                if (o != null)
                {
                    ShowAlert("Leave has already been applied for this From Date.");
                    return;
                }
            }

            using (SqlCommand cmdChkTo = new SqlCommand("SELECT 1 FROM IT_EmployeeLeaveDetails WHERE Todate = @toDate AND Employeekey = @empKey", conn))
            {
                cmdChkTo.Parameters.Add("@toDate", SqlDbType.DateTime).Value = dt_todate;
                cmdChkTo.Parameters.Add("@empKey", SqlDbType.VarChar).Value = SC.Userid.ToString();
                object o = cmdChkTo.ExecuteScalar();
                if (o != null)
                {
                    ShowAlert("Leave has already been applied for this To Date.");
                    return;
                }
            }

            using (SqlTransaction transaction = conn.BeginTransaction("Leavedays"))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.Transaction = transaction;
                try
                {
                    cmd.CommandText = @"
                    INSERT INTO IT_EmployeeLeaveDetails
                    (Employeekey, Createdby, Fromdate, Todate, Reason, Responsestatus, leavedays, LeaveType, LeaveCategoryId)
                    OUTPUT INSERTED.Employeeleavedetailskey
                    VALUES (@Employeekey, @Createdby, @Fromdate, @Todate, @Reason, @Responsestatus, @leavedays, @LeaveType, @LeaveCategoryId);";

                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@Employeekey", SqlDbType.UniqueIdentifier).Value = Guid.Parse(str_userkey);
                    cmd.Parameters.Add("@Createdby", SqlDbType.UniqueIdentifier).Value = Guid.Parse(str_userkey);
                    cmd.Parameters.Add("@Fromdate", SqlDbType.DateTime).Value = dt_fromdate;
                    cmd.Parameters.Add("@Todate", SqlDbType.DateTime).Value = dt_todate;
                    cmd.Parameters.Add("@Reason", SqlDbType.NVarChar).Value = txt_reason.InnerText.Trim();
                    cmd.Parameters.Add("@Responsestatus", SqlDbType.NVarChar).Value = str_responsestatus;
                    cmd.Parameters.Add("@leavedays", SqlDbType.Decimal).Value = NoOfDays;
                    cmd.Parameters.Add("@LeaveType", SqlDbType.NVarChar, 50).Value = ddl_leavetype.SelectedValue;
                    cmd.Parameters.Add("@LeaveCategoryId", SqlDbType.Int).Value = int.Parse(ddl_leavecategory.SelectedValue);
                    
                    object newIdObj = cmd.ExecuteScalar();
                    if (newIdObj == null || newIdObj == DBNull.Value)
                        throw new Exception("Failed to insert leave record.");

                    Guid leaveKey = (Guid)newIdObj;
                    List<DateTime> dates = GetDatesBetween(dt_date, dtExclusive);
                    
                    foreach (DateTime dtdate in dates)
                    {
                        DateTime dateOnly = dtdate.Date;
                        if (holidayDates.Contains(dateOnly)) continue;
                        if (dateOnly.DayOfWeek == DayOfWeek.Sunday) continue;

                        if (dateOnly.DayOfWeek == DayOfWeek.Saturday)
                        {
                            if (dateOnly.Day >= 8 && dateOnly.Day <= 14)
                                continue;  
                        }

                        using (SqlCommand cmdInsertDate = conn.CreateCommand())
                        {
                            cmdInsertDate.Transaction = transaction;
                            cmdInsertDate.CommandText = @"INSERT INTO Leavedates (Leavekey, Leavedays, createdby) VALUES (@Leavekey, @Leavedays, @createdby);";
                            cmdInsertDate.Parameters.Add("@Leavekey", SqlDbType.UniqueIdentifier).Value = leaveKey;
                            cmdInsertDate.Parameters.Add("@Leavedays", SqlDbType.Date).Value = dateOnly;
                            cmdInsertDate.Parameters.Add("@createdby", SqlDbType.UniqueIdentifier).Value = myGuid;
                            cmdInsertDate.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    try { transaction.Rollback(); } catch { }
                    ShowAlert("Error: " + ex.Message.Replace("'", ""));
                    return;
                }
            }
        }
        ShowSuccessAndRedirect("Leave Request Created Successfully!", "LeaveRequestView.aspx");
    }

    private void ShowAlert(string message)
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

    protected void ddl_filter_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Handled automatically on Page_Load on postback
    }
}



