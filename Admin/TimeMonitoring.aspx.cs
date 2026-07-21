using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WEB_TimeMonitoring : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    CommonFunction CF;

    string str_userkey = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        DA = new DataAccess();
        SC = new SessionCustom();
        PH = new PhTemplate();
        CF = new CommonFunction();
        str_userkey = SC.Userid;

        if (!IsPostBack)
        {
            BindDateDropdown();
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Employee Monitoring";

            Load_WorkedHoursData();   
        }
    }

    private void BindDateDropdown()
    {
        ddlDate.Items.Clear();
        ddlDate.Items.Add(new ListItem("All", "0"));
        ddlDate.Items.Add(new ListItem("Today", "-1"));
        
        for (int m = 1; m <= 12; m++)
        {
            string monthName = new DateTime(2000, m, 1).ToString("MMMM");
            ddlDate.Items.Add(new ListItem(monthName, m.ToString()));
        }
        
        ddlDate.SelectedValue = "-1";   
    }

    protected void ddlDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        Load_WorkedHoursData();
    }
    protected void SE_date_TextChanged(object sender, EventArgs e)
    {
        Load_WorkedHoursData();
       
    }
    private void Load_WorkedHoursData()
    {
        string datevalue = SE_date.Text;

        DateTime startDate = DateTime.MinValue;
        DateTime endDate = DateTime.MinValue;

        if (!string.IsNullOrEmpty(datevalue))
        {
            string[] dates = datevalue.Split('-');

            DateTime.TryParseExact(
                dates[0].Trim(),
                "MM/dd/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out startDate
            );

            DateTime.TryParseExact(
                dates[1].Trim(),
                "MM/dd/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out endDate
            );
        }

        SqlCommand cmd = new SqlCommand();

        string query = @"
WITH CTE AS
(
    SELECT 
        Employeekey,
        EmployeeName,
        WorkDate,
        InTime,
        OutTime,
        GrossWorkingHours,
        LunchDuration,
        BreakDuration,
        NetWorkingDuration,
        ROW_NUMBER() OVER (
            PARTITION BY Employeekey, CAST(WorkDate AS DATE)
            ORDER BY InTime DESC
        ) AS RN
    FROM IT_V_EmployeeDailyWorkSummary
    WHERE 1=1
";

        if (ddlDate.SelectedValue == "-1")
        {
            query += " AND CAST(WorkDate AS DATE) = CAST(GETDATE() AS DATE)";
        }
        else if (ddlDate.SelectedValue != "0" && int.Parse(ddlDate.SelectedValue) >= 1 && int.Parse(ddlDate.SelectedValue) <= 12)
        {
            int month = int.Parse(ddlDate.SelectedValue);
            int year = DateTime.Now.Year;
            query += " AND MONTH(WorkDate) = @Month AND YEAR(WorkDate) = @Year";
            cmd.Parameters.AddWithValue("@Month", month);
            cmd.Parameters.AddWithValue("@Year", year);
        }
        else if (startDate != DateTime.MinValue && endDate != DateTime.MinValue)
        {
            query += " AND CAST(WorkDate AS DATE) BETWEEN @StartDate AND @EndDate";
            cmd.Parameters.AddWithValue("@StartDate", startDate.Date);
            cmd.Parameters.AddWithValue("@EndDate", endDate.Date);
        }

        query += @"
)
SELECT 
    Employeekey,
    EmployeeName,
    WorkDate,
    InTime,
    OutTime,
    GrossWorkingHours,
    LunchDuration,
    BreakDuration,
    NetWorkingDuration
FROM CTE
WHERE RN = 1
ORDER BY WorkDate DESC";

        cmd.CommandText = query;

        DataTable dt = DA.GetDataTable(cmd);

        if (dt.Rows.Count > 0)
        {
            DataTable final = new DataTable();
            final.Columns.Add("EmployeeName");
            final.Columns.Add("WorkDate");
            final.Columns.Add("InTime");
            final.Columns.Add("OutTime");
            final.Columns.Add("GrossWorkingHours");
            final.Columns.Add("LunchDuration");
            final.Columns.Add("BreakDuration");
            final.Columns.Add("NetWorkingDuration");

            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");

            foreach (DataRow row in dt.Rows)
            {
                final.Rows.Add(
                    row["EmployeeName"],
                    Convert.ToDateTime(row["WorkDate"]).ToString("dd/MM/yyyy"),

                    row["InTime"] == DBNull.Value
                        ? "-"
                        : TimeZoneInfo.ConvertTimeFromUtc((DateTime)row["InTime"], istZone)
                            .ToString("hh:mm tt"),

                    row["OutTime"] == DBNull.Value
                        ? "-"
                        : TimeZoneInfo.ConvertTimeFromUtc((DateTime)row["OutTime"], istZone)
                            .ToString("hh:mm tt"),

                    row["GrossWorkingHours"],
                    row["LunchDuration"],
                    row["BreakDuration"],
                    row["NetWorkingDuration"]
                );
            }

            DataSet ds = new DataSet();
            ds.Merge(final);
            PH.LoadGridItem(ds, PH_TimemonitoringView, "TimeMonitoringView.txt", "");
        }
        else
        {
            PH_TimemonitoringView.Controls.Clear();
        }
    }


    protected void btn_sub_Click(object sender, EventArgs e)
    {

       
        ddlDate.SelectedValue = "0";
        Load_WorkedHoursData();
        
    }
}
