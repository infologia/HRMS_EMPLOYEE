using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_viewpayroll : System.Web.UI.Page
{
    DataAccess DA;

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();

        // ---- Download request (comes from the "Download" link generated in the grid row) ----
        // Handled BEFORE anything else so the response can be replaced with the excel stream.
        if (Request.QueryString["download"] == "1"
            && Request.QueryString["month"] != null
            && Request.QueryString["year"] != null)
        {
            int dl_month, dl_year;
            if (int.TryParse(Request.QueryString["month"], out dl_month)
                && int.TryParse(Request.QueryString["year"], out dl_year))
            {
                this.ExportSavedPayroll(dl_month, dl_year);
            }
            return; // ExportSavedPayroll ends the response
        }

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Payroll History";

            this.LoadGrid();
        }
    }

    /// <summary>
    /// Loads every payroll batch that has already been generated & saved
    /// (grouped by Month + Year + who generated it) into PH_viewpayroll
    /// using the viewpayroll.txt row template.
    /// </summary>
    private void LoadGrid()
    {
        string query = @"
            SELECT 
                p.PayrollMonth,
                p.PayrollYear,
                p.Createdby,
                MIN(p.Createdon) AS GeneratedOn,
                r.Username
            FROM IT_EmployeePayrollDetails p
            LEFT JOIN IT_EmployeeRegister r ON r.Employeekey = p.Createdby
            GROUP BY p.PayrollMonth, p.PayrollYear, p.Createdby, r.Username
            ORDER BY p.PayrollYear DESC, p.PayrollMonth DESC";

        SqlCommand cmd = new SqlCommand(query);
        DataTable dt_source = DA.GetDataTable(cmd);

        // Build the display DataTable expected by the viewpayroll.txt template
        // ( %%Month%%  %%Year%%  %%GeneratedBy%%  %%GeneratedOn%%  %%ViewLink%%  %%DownloadLink%% )
        DataTable dt_grid = new DataTable();
        dt_grid.Columns.Add("Month");
        dt_grid.Columns.Add("Year");
        dt_grid.Columns.Add("GeneratedBy");
        dt_grid.Columns.Add("GeneratedOn");
        dt_grid.Columns.Add("ViewLink");
        dt_grid.Columns.Add("DownloadLink");

        if (dt_source != null)
        {
            foreach (DataRow row in dt_source.Rows)
            {
                int month = Convert.ToInt32(row["PayrollMonth"]);
                int year = Convert.ToInt32(row["PayrollYear"]);

                string monthName = new DateTime(year, month, 1).ToString("MMMM");

                string generatedBy = (row["Username"] != DBNull.Value && row["Username"].ToString() != "")
                    ? row["Username"].ToString()
                    : "-";

                string generatedOn = "-";

                if (row["GeneratedOn"] != DBNull.Value)
                {
                    DateTimeOffset dto = (DateTimeOffset)row["GeneratedOn"];
                    generatedOn = dto.ToString("dd-MMM-yyyy hh:mm tt");
                }

                string viewLink = "<a href='payslips.aspx?key=" + month + "&id=" + year + "&view=1"
                    + "' class='btn btn-primary btn-xs'>View</a>";

                string downloadLink = "<a href='viewpayroll.aspx?download=1&month=" + month + "&year=" + year
                    + "' class='btn btn-info btn-xs'>Download</a>";

                DataRow dr = dt_grid.NewRow();
                dr["Month"] = monthName;
                dr["Year"] = year;
                dr["GeneratedBy"] = generatedBy;
                dr["GeneratedOn"] = generatedOn;
                dr["ViewLink"] = viewLink;
                dr["DownloadLink"] = downloadLink;
                dt_grid.Rows.Add(dr);
            }
        }

        PH_viewpayroll.Controls.Clear();

        if (dt_grid.Rows.Count > 0)
        {
            PhTemplate PH = new PhTemplate();
            DataSet ds = new DataSet();
            ds.Merge(dt_grid);
            PH.LoadGridItem(ds, PH_viewpayroll, "viewpayroll.txt", "");
            lbl_nodata.Visible = false;
        }
        else
        {
            lbl_nodata.Visible = true;
        }
    }

    /// <summary>
    /// Exports the payroll data that is ALREADY STORED in IT_EmployeePayrollDetails
    /// for the selected month/year to Excel (does not recompute anything live).
    /// </summary>
    private void ExportSavedPayroll(int month, int year)
    {
        string query = @"
            SELECT * FROM IT_EmployeePayrollDetails
            WHERE PayrollMonth = @Month AND PayrollYear = @Year
            ORDER BY EmployeeName";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@Month", month);
        cmd.Parameters.AddWithValue("@Year", year);

        DataTable dt = DA.GetDataTable(cmd);

        if (dt == null || dt.Rows.Count == 0)
        {
            Response.Write("No stored payroll data found for the selected month/year.");
            Response.End();
            return;
        }

        // Columns as stored in IT_EmployeePayrollDetails -> friendly header text
        var exportColumns = new KeyValuePair<string, string>[]
        {
            new KeyValuePair<string, string>("EmployeeName", "Employee Name"),
            new KeyValuePair<string, string>("Employeeid", "Employee ID"),
            new KeyValuePair<string, string>("NoOfDaysInMonth", "Days In Month"),
            new KeyValuePair<string, string>("NoOfWorkingDays", "Working Days"),
            new KeyValuePair<string, string>("NoOfPaidHolidays", "Paid Holidays"),
            new KeyValuePair<string, string>("InformedLeave", "Informed Leave"),
            new KeyValuePair<string, string>("LeaveDaysInYear", "Leave Days In Year"),
            new KeyValuePair<string, string>("CurrentMonthLeaveDays", "Month Leave Days"),
            new KeyValuePair<string, string>("LOPLeaveDays", "LOP Leave Days"),
            new KeyValuePair<string, string>("UninformedLeave", "Uninformed Leave"),
            new KeyValuePair<string, string>("HRMSHalfDayCount", "Half Day Count"),
            new KeyValuePair<string, string>("HRMSHalfDayDeduction", "Half Day Deduction"),
            new KeyValuePair<string, string>("HRMSFullDayDeduction", "Full Day Deduction"),
            new KeyValuePair<string, string>("LateLoginCount", "Late Login Count"),
            new KeyValuePair<string, string>("OutTimeNullCount", "OutTime Missing Count"),
            new KeyValuePair<string, string>("TotalDeductionDays", "Total Deduction"),
            new KeyValuePair<string, string>("MonthlySalary", "Monthly Salary"),
            new KeyValuePair<string, string>("PerDaySalary", "Per Day Salary"),
            new KeyValuePair<string, string>("LeaveDaysSalary", "Leave Days Salary"),
            new KeyValuePair<string, string>("TotalEligibleDays", "Eligible Days"),
            new KeyValuePair<string, string>("EligibleSalaryAmount", "Eligible Amount"),
            new KeyValuePair<string, string>("NetPay", "Net Pay"),
            new KeyValuePair<string, string>("AnnualCTC", "Annual CTC"),
            new KeyValuePair<string, string>("InTimeAvailableInHRMS", "InTime Days"),
            new KeyValuePair<string, string>("FinalNetPay", "Final Net Pay")
        };

        StringBuilder sb = new StringBuilder();
        sb.Append("<html><head><meta charset=\"utf-8\" /></head><body>");
        sb.Append("<table border='1'>");

        sb.Append("<tr>");
        foreach (var col in exportColumns)
            sb.Append("<th>" + HttpUtility.HtmlEncode(col.Value) + "</th>");
        sb.Append("</tr>");

        foreach (DataRow row in dt.Rows)
        {
            sb.Append("<tr>");
            foreach (var col in exportColumns)
            {
                string val = dt.Columns.Contains(col.Key) && row[col.Key] != DBNull.Value
                    ? row[col.Key].ToString()
                    : "";
                sb.Append("<td>" + HttpUtility.HtmlEncode(val) + "</td>");
            }
            sb.Append("</tr>");
        }

        sb.Append("</table></body></html>");

        DateTime labelDate = new DateTime(year, month, 1);
        string fileName = "Payroll_" + labelDate.ToString("MMM_yyyy") + ".xls";

        Response.Clear();
        Response.Buffer = true;
        Response.ContentType = "application/vnd.ms-excel";
        Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
        Response.Charset = "";
        Response.Write(sb.ToString());
        Response.End();
    }
}
