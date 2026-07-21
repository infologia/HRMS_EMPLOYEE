using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_Payslip : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    string str_id = "";
    string str_pay = "";
    string str_key = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();
        str_key = this.SC.Userid;

        if (Request.QueryString["key"] != null && Request.QueryString["key"] != "")
            this.str_id = Request.QueryString["key"].ToString();
        else { Response.Redirect("~/Admin/payroll.aspx"); return; }

        if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
            this.str_pay = Request.QueryString["id"].ToString();
        else { Response.Redirect("~/Admin/payroll.aspx"); return; }

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Payslip Generation";

            this.loadpayroll();
            this.loadgrid();
        }
    }

    private void loadgrid()
    {
        string str_payroll = @"SELECT p.Workingdays, p.SalaryMonth, p.SalaryYear, p.Totalsalary, p.Netpay, p.LOP,
            e.Firstname, e.Employeeid, p.Leavedays, p.Payrollkey, p.perdaysalary
            FROM IT_PayrollOne p
            LEFT OUTER JOIN IT_EmployeeRegister e ON p.Employeekey = e.Employeekey
            WHERE p.SalaryMonth = @Month AND p.SalaryYear = @Year
            ORDER BY e.Employeeid";

        SqlCommand sc1 = new SqlCommand(str_payroll);
        sc1.Parameters.AddWithValue("@Month", str_id);
        sc1.Parameters.AddWithValue("@Year", str_pay);

        DataTable dt_payrolls = DA.GetDataTable(sc1);
        DataSet ds = new DataSet();
        ds.Merge(dt_payrolls);

        if (dt_payrolls.Rows.Count > 0)
            PH.LoadGridItem(ds, payroll, "payrollview.txt", "");
    }

    public void loadpayroll()
    {
        int selectedMonth = Convert.ToInt32(str_id);
        int selectedYear = Convert.ToInt32(str_pay);

        // ===== Step 1: Working days for the month =====
        string str_workdays = @"SELECT Numberofworkdaysinmonth
            FROM IT_EmployeeWorkingDayDetails
            WHERE monthvalue = @Month AND Year = @Year";

        SqlCommand cmdWork = new SqlCommand(str_workdays);
        cmdWork.Parameters.AddWithValue("@Month", str_id);
        cmdWork.Parameters.AddWithValue("@Year", str_pay);

        DataTable dtWork = DA.GetDataTable(cmdWork);
        if (dtWork.Rows.Count == 0) return;

        int workdays = Convert.ToInt32(dtWork.Rows[0][0]);

        // ===== Step 2: Active employees =====
        string str_emp = @"SELECT e.Employeekey, e.Employeeid, s.Totalearnings
            FROM IT_EmployeeRegister e
            INNER JOIN IT_EmployeeSalaryDetails s ON e.Employeekey = s.Employeekey
            WHERE e.Employeestatus = 1";

        DataTable dtEmp = DA.GetDataTable(new SqlCommand(str_emp));

        int successCount = 0, skipCount = 0;

        foreach (DataRow empRow in dtEmp.Rows)
        {
            string empKey = empRow["Employeekey"].ToString();
            string empId = empRow["Employeeid"].ToString();
            decimal grossSalary = Convert.ToDecimal(empRow["Totalearnings"]);

            if (grossSalary <= 0) { skipCount++; continue; }

            // ===== Step 3: Duplicate check =====
            string chk = @"SELECT COUNT(*) FROM IT_PayrollOne
                WHERE Employeekey = @emp AND SalaryMonth = @m AND SalaryYear = @y";

            SqlCommand chkCmd = new SqlCommand(chk);
            chkCmd.Parameters.AddWithValue("@emp", empKey);
            chkCmd.Parameters.AddWithValue("@m", str_id);
            chkCmd.Parameters.AddWithValue("@y", str_pay);

            int already = Convert.ToInt32(DA.GetDataTable(chkCmd).Rows[0][0]);
            if (already > 0) { skipCount++; continue; }

            // ===== Step 4: Approved leaves =====
            // FIX: b.Leavedays = total days for that leave request (e.g. 2.0 for 2-day leave)
            // Leavedates has one row per date
            // So per date value = b.Leavedays / COUNT of dates in that request
            // Using window function to divide correctly per date
            decimal totalApprovedLeaveDays = 0m;
            HashSet<DateTime> leaveDates = new HashSet<DateTime>();

            string leaveQuery = @"
                SELECT c.Leavedays,
                       b.Leavedays / COUNT(c.Leavedateskey) OVER (
                           PARTITION BY b.Employeeleavedetailskey
                       ) AS LeaveValue
                FROM IT_EmployeeLeaveDetails b
                INNER JOIN Leavedates c ON b.Employeeleavedetailskey = c.Leavekey
                WHERE b.Employeekey    = @emp
                AND MONTH(c.Leavedays) = @m
                AND YEAR(c.Leavedays)  = @y
                AND b.Responsestatus   = 2";

            SqlCommand cmdLeave = new SqlCommand(leaveQuery);
            cmdLeave.Parameters.AddWithValue("@emp", empKey);
            cmdLeave.Parameters.AddWithValue("@m", str_id);
            cmdLeave.Parameters.AddWithValue("@y", str_pay);

            DataTable dtLeave = DA.GetDataTable(cmdLeave);

            foreach (DataRow row in dtLeave.Rows)
            {
                DateTime d = Convert.ToDateTime(row["Leavedays"]).Date;
                decimal val = Convert.ToDecimal(row["LeaveValue"]);

                totalApprovedLeaveDays += val; // per date correct value add
                leaveDates.Add(d);             // unique dates
            }

            // ===== Step 5: Attendance =====
            decimal totalAttendanceLeaveDays = 0m;
            HashSet<DateTime> processedDates = new HashSet<DateTime>();

            string str_att = @"SELECT WorkDate, OutTime, NetWorkingDuration
                FROM IT_V_EmployeeDailyWorkSummary
                WHERE Employeekey = @emp
                AND MONTH(WorkDate) = @m AND YEAR(WorkDate) = @y
                AND NetWorkingDuration IS NOT NULL AND OutTime IS NOT NULL";

            SqlCommand cmdAtt = new SqlCommand(str_att);
            cmdAtt.Parameters.AddWithValue("@emp", empKey);
            cmdAtt.Parameters.AddWithValue("@m", str_id);
            cmdAtt.Parameters.AddWithValue("@y", str_pay);

            DataTable dtAtt = DA.GetDataTable(cmdAtt);

            foreach (DataRow att in dtAtt.Rows)
            {
                DateTime workDate = Convert.ToDateTime(att["WorkDate"]).Date;

                if (processedDates.Contains(workDate)) continue;
                processedDates.Add(workDate);

                // Leave date → skip attendance check
                // totalApprovedLeaveDays already has correct value
                if (leaveDates.Contains(workDate)) continue;

                DateTime outTime = Convert.ToDateTime(att["OutTime"]);

                // ===== Net working hours =====
                decimal netHours = 0m;
                string netStr = att["NetWorkingDuration"].ToString().ToLower();

                try
                {
                    int hours = 0, minutes = 0;

                    if (netStr.Contains("hour"))
                        int.TryParse(
                            netStr.Split(new string[] { "hour" }, StringSplitOptions.None)[0].Trim(),
                            out hours);

                    if (netStr.Contains("minute"))
                    {
                        string[] parts = netStr.Split(' ');
                        int.TryParse(parts[parts.Length - 2], out minutes);
                    }

                    netHours = hours + (minutes / 60m);
                }
                catch { netHours = 0m; }

                // ===== Meeting hours that START after outTime only =====
                decimal meetingHours = 0m;

                string str_meet = @"SELECT DISTINCT m.StartTime, m.EndTime
                    FROM IT_Meetings m
                    WHERE (m.CreatedBy = @emp OR EXISTS (
                        SELECT 1 FROM IT_MeetingParticipants mp
                        WHERE mp.Meetingkey = m.Meetingkey AND mp.Employeekey = @emp
                    ))
                    AND CAST(m.StartTime AS DATE) = @d
                    AND m.Status IN (1, 2)";

                SqlCommand cmdMeet = new SqlCommand(str_meet);
                cmdMeet.Parameters.AddWithValue("@emp", empKey);
                cmdMeet.Parameters.AddWithValue("@d", workDate);

                DataTable dtMeet = DA.GetDataTable(cmdMeet);

                foreach (DataRow meet in dtMeet.Rows)
                {
                    DateTime start = Convert.ToDateTime(meet["StartTime"]);
                    DateTime end = Convert.ToDateTime(meet["EndTime"]);

                    // Only meetings that START after outTime
                    if (start <= outTime) continue;

                    decimal hrs = (decimal)(end - start).TotalMinutes / 60m;
                    if (hrs > 0) meetingHours += hrs;
                }

                decimal finalHours = netHours + meetingHours;

                // ===== Attendance-based leave deduction =====
                if (finalHours < 5m)
                    totalAttendanceLeaveDays += 1m;      // < 5 hrs  → full day
                else if (finalHours < 8m)
                    totalAttendanceLeaveDays += 0.5m;    // 5-7 hrs  → half day
                // >= 8 hrs → full day present, no deduction
            }

            // ===== Step 6: Absent days =====
            // actual present = attended days excluding leave dates
            int actualPresentDays = processedDates
                .Where(d => !leaveDates.Contains(d))
                .Count();

            // absent = workdays - present days - leave date count
            decimal absentDays = workdays - actualPresentDays - leaveDates.Count;
            if (absentDays < 0m) absentDays = 0m;

            // ===== Step 7: Total leave days =====
            decimal totalLeaveDays = totalApprovedLeaveDays
                                   + totalAttendanceLeaveDays
                                   + absentDays;

            // ===== Step 8: LOP Calculation =====
            string str_prev = @"SELECT ISNULL(SUM(Leavedays - ROUND(LOP / perdaysalary, 2)), 0)
                FROM IT_PayrollOne
                WHERE Employeekey = @emp
                AND SalaryYear    = @y
                AND SalaryMonth   < @m
                AND perdaysalary  > 0";

            SqlCommand cmdPrev = new SqlCommand(str_prev);
            cmdPrev.Parameters.AddWithValue("@emp", empKey);
            cmdPrev.Parameters.AddWithValue("@y", str_pay);
            cmdPrev.Parameters.AddWithValue("@m", str_id);

            decimal prevFreeLeaves = Convert.ToDecimal(DA.GetDataTable(cmdPrev).Rows[0][0]);
            if (prevFreeLeaves < 0m) prevFreeLeaves = 0m;

            decimal balance = Math.Max(0m, 12m - prevFreeLeaves);

            decimal lopDays = 0m;
            if (totalLeaveDays > balance)
                lopDays = totalLeaveDays - balance;

            // ===== Step 9: Salary =====
            int daysInMonth = DateTime.DaysInMonth(selectedYear, selectedMonth);
            decimal perDaySalary = grossSalary / daysInMonth;
            decimal lopAmount = Math.Round(perDaySalary * lopDays, 0);
            decimal netSalary = Math.Max(0m, grossSalary - lopAmount);

            // ===== Step 10: Insert =====
            string insert = @"INSERT INTO IT_PayrollOne
                (Employeekey, Employeeid, Workingdays, Leavedays,
                SalaryMonth, SalaryYear, Totalsalary, Netpay, LOP,
                Createdby, Payslipissued, perdaysalary)
                VALUES
                (@e, @id, @wd, @ld,
                @m, @y, @ts, @np, @lop,
                @cb, GETDATE(), @pd)";

            SqlCommand cmd = new SqlCommand(insert);
            cmd.Parameters.AddWithValue("@e", empKey);
            cmd.Parameters.AddWithValue("@id", empId);
            cmd.Parameters.AddWithValue("@wd", workdays);
            cmd.Parameters.AddWithValue("@ld", totalLeaveDays);
            cmd.Parameters.AddWithValue("@m", str_id);
            cmd.Parameters.AddWithValue("@y", str_pay);
            cmd.Parameters.AddWithValue("@ts", grossSalary);
            cmd.Parameters.AddWithValue("@np", netSalary);
            cmd.Parameters.AddWithValue("@lop", lopAmount);
            cmd.Parameters.AddWithValue("@cb", str_key);
            cmd.Parameters.AddWithValue("@pd", perDaySalary);

            DA.ExecuteNonQuery(cmd);
            successCount++;
        }

        ClientScript.RegisterStartupScript(
            this.GetType(),
            "msg",
            string.Format("alert('Generated: {0}, Skipped: {1}');", successCount, skipCount),
            true);
    }
}
