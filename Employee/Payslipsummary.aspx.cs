using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_Payslipsummary : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_userkey = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.str_userkey = SC.Userid;

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Payslip Summary";

            string month = Request.QueryString["month"];
            string year  = Request.QueryString["year"];

            if (string.IsNullOrEmpty(month) || string.IsNullOrEmpty(year))
            {
                Response.Redirect("payslipdetails.aspx");
                return;
            }

            int iMonth = Convert.ToInt32(month);
            int iYear  = Convert.ToInt32(year);

            lbl_monthyear.Text = new System.DateTime(iYear, iMonth, 1).ToString("MMMM yyyy");

            LoadPayrollSummary(iMonth, iYear);
            LoadAttendance(iMonth, iYear);
        }
    }

    private void LoadPayrollSummary(int month, int year)
    {
        string sql = @"SELECT p.*,
                               ISNULL(e.Firstname + ' ' + e.Lastname, 'Admin') AS GeneratedBy
                        FROM IT_EmployeePayrollDetails p
                        LEFT JOIN IT_EmployeeRegister e ON p.Createdby = e.Employeekey
                        WHERE p.Employeekey = @Employeekey
                          AND p.PayrollMonth = @Month
                          AND p.PayrollYear  = @Year";

        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.AddWithValue("@Employeekey", str_userkey);
        cmd.Parameters.AddWithValue("@Month", month);
        cmd.Parameters.AddWithValue("@Year",  year);

        DataTable dt = DA.GetDataTable(cmd);

        if (dt == null || dt.Rows.Count == 0) return;

        DataRow row = dt.Rows[0];

        // Helper
        Func<string, string> val = col =>
        {
            if (dt.Columns.Contains(col) && row[col] != DBNull.Value)
            {
                string raw = row[col].ToString();
                decimal d;
                if (decimal.TryParse(raw, out d))
                {
                    return d.ToString("0.##");
                }
                return raw;
            }
            return "-";
        };

        Func<string, string> money = col =>
            dt.Columns.Contains(col) && row[col] != DBNull.Value
                ? Convert.ToDecimal(row[col]).ToString("N2") : "-";

        // Top summary cards
        lbl_days_in_month.Text  = val("NoOfDaysInMonth");
        lbl_working_days.Text   = val("NoOfWorkingDays");
        lbl_paid_holidays.Text  = val("NoOfPaidHolidays");
        lbl_lop_days.Text       = val("LOPLeaveDays");
        lbl_deduction_days.Text = val("TotalDeductionDays");
        lbl_netpay.Text         = money("NetPay");

        // Full detail table
        lbl_days_in_month2.Text = val("NoOfDaysInMonth");
        lbl_working_days2.Text  = val("NoOfWorkingDays");
        lbl_pd2.Text            = val("NoOfPaidHolidays");
        lbl_il2.Text            = val("InformedLeave");
        lbl_ldy2.Text           = val("LeaveDaysInYear");
        lbl_cmld2.Text          = val("CurrentMonthLeaveDays");
        lbl_loplv2.Text         = val("LOPLeaveDays");
        lbl_uninf2.Text         = val("UninformedLeave");
        lbl_hdc2.Text           = val("HRMSHalfDayCount");
        lbl_hdd2.Text           = val("HRMSHalfDayDeduction");
        lbl_fdd2.Text           = val("HRMSFullDayDeduction");
        lbl_tdd2.Text           = val("TotalDeductionDays");
        lbl_ms2.Text            = money("MonthlySalary");
        lbl_pds2.Text           = money("PerDaySalary");
        lbl_lds2.Text           = money("LeaveDaysSalary");
        lbl_ted2.Text           = val("TotalEligibleDays");
        lbl_esa2.Text           = money("EligibleSalaryAmount");
        lbl_np2.Text            = money("NetPay");
        lbl_ctc2.Text           = money("AnnualCTC");
        lbl_ita2.Text           = val("InTimeAvailableInHRMS");
        lbl_llc2.Text           = val("LateLoginCount");
        lbl_onc2.Text           = val("OutTimeNullCount");
        lbl_gen2.Text           = dt.Columns.Contains("GeneratedBy") && row["GeneratedBy"] != DBNull.Value
                                    ? row["GeneratedBy"].ToString() : "-";
    }

    private void LoadAttendance(int month, int year)
    {
        string sql = @"SELECT WorkDate, InTime, OutTime,
                              GrossWorkingHours, LunchDuration, BreakDuration, NetWorkingDuration
                       FROM IT_V_EmployeeDailyWorkSummary
                       WHERE Employeekey   = @Employeekey
                         AND MONTH(WorkDate) = @Month
                         AND YEAR(WorkDate)  = @Year
                       ORDER BY WorkDate";

        SqlCommand cmd = new SqlCommand(sql);
        cmd.Parameters.AddWithValue("@Employeekey", str_userkey);
        cmd.Parameters.AddWithValue("@Month", month);
        cmd.Parameters.AddWithValue("@Year",  year);

        DataTable dt = DA.GetDataTable(cmd);

        if (dt == null || dt.Rows.Count == 0)
        {
            lbl_nodata.Visible     = true;
            pnl_attendance.Visible = false;
            return;
        }

        pnl_attendance.Visible = true;
        lbl_nodata.Visible     = false;

        StringBuilder sb = new StringBuilder();
        foreach (DataRow row in dt.Rows)
        {
            DateTime workDate = Convert.ToDateTime(row["WorkDate"]);
            string dayName    = workDate.ToString("ddd");

            string inTime  = row["InTime"]  != DBNull.Value ? Convert.ToDateTime(row["InTime"]).ToLocalTime().ToString("hh:mm tt")  : "-";
            string outTime = row["OutTime"] != DBNull.Value ? Convert.ToDateTime(row["OutTime"]).ToLocalTime().ToString("hh:mm tt") : "-";

            string gross   = row["GrossWorkingHours"]  != DBNull.Value ? row["GrossWorkingHours"].ToString()  : "-";
            string lunch   = row["LunchDuration"]      != DBNull.Value ? row["LunchDuration"].ToString()      : "-";
            string brk     = row["BreakDuration"]      != DBNull.Value ? row["BreakDuration"].ToString()      : "-";
            string net     = row["NetWorkingDuration"] != DBNull.Value ? row["NetWorkingDuration"].ToString() : "-";

            string inBadge  = inTime  != "-" ? "<span class='badge-in'>"  + inTime  + "</span>" : "<span style='color:#aaa;'>-</span>";
            string outBadge = outTime != "-" ? "<span class='badge-out'>" + outTime + "</span>" : "<span style='color:#aaa;'>-</span>";
            string netBadge = net     != "-" ? "<span class='badge-hrs'>" + net     + "</span>" : "<span style='color:#aaa;'>-</span>";

            sb.Append("<tr>");
            sb.Append("<td>" + workDate.ToString("dd-MMM-yyyy") + "</td>");
            sb.Append("<td>" + dayName + "</td>");
            sb.Append("<td>" + inBadge + "</td>");
            sb.Append("<td>" + outBadge + "</td>");
            sb.Append("<td>" + gross + "</td>");
            sb.Append("<td>" + lunch + "</td>");
            sb.Append("<td>" + brk + "</td>");
            sb.Append("<td>" + netBadge + "</td>");
            sb.Append("</tr>");
        }

        PH_attendance.Controls.Add(new LiteralControl(sb.ToString()));
    }
}