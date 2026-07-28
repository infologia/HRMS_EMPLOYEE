using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_employeeholidays : System.Web.UI.Page
{
    DataAccess DA;
    PhTemplate PH;

    protected void Page_Load(object sender, EventArgs e)
    {
        DA = new DataAccess();
        PH = new PhTemplate();

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Holidays";

            lit_Year.Text = DateTime.Now.Year.ToString();
            lit_Year2.Text = DateTime.Now.Year.ToString();

            LoadHolidays();
        }
    }

    private void LoadHolidays()
    {
        string str_query = @"SELECT 
                                ROW_NUMBER() OVER (ORDER BY Holidays ASC) AS Sno,
                                CONVERT(VARCHAR, Holidays, 103) AS HolidayDate,
                                Holidays AS HolidayRaw,
                                description,
                                Day,
                                NoOfLeave,
                                CONVERT(VARCHAR, createdon, 103) AS createdon
                             FROM IT_Holidays 
                             WHERE YEAR(Holidays) = @Year
                             ORDER BY Holidays ASC";

        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@Year", DateTime.Now.Year);
        DataTable dt = DA.GetDataTable(cmd);

        DateTime today = DateTime.Today;
        int totalCount = 0;
        int upcomingCount = 0;
        int passedCount = 0;
        DateTime? nextHolidayDate = null;

        System.Text.StringBuilder sbAll = new System.Text.StringBuilder();
        System.Text.StringBuilder sbUpcoming = new System.Text.StringBuilder();
        System.Text.StringBuilder sbPassed = new System.Text.StringBuilder();

        if (dt != null && dt.Rows.Count > 0)
        {
            totalCount = dt.Rows.Count;

            foreach (DataRow dr in dt.Rows)
            {
                string sno         = dr["Sno"].ToString();
                string dateStr     = dr["HolidayDate"].ToString();
                string description = dr["description"].ToString();
                string day         = dr["Day"].ToString();
                string noOfLeave   = dr["NoOfLeave"].ToString();

                DateTime holidayDate;
                bool parsed = DateTime.TryParse(dr["HolidayRaw"].ToString(), out holidayDate);
                bool isUpcoming = parsed && holidayDate.Date >= today;

                if (isUpcoming)
                {
                    upcomingCount++;
                    if (nextHolidayDate == null)
                        nextHolidayDate = holidayDate;
                }
                else
                {
                    passedCount++;
                }

                string[] dotStyles = new string[]
                {
                    "background:#E6F1FB;color:#185FA5;",
                    "background:#FAEEDA;color:#BA7517;",
                    "background:#EAF3DE;color:#3B6D11;",
                    "background:#EEEDFE;color:#534AB7;",
                    "background:#E1F5EE;color:#085041;",
                    "background:#FCEBEB;color:#A32D2D;"
                };
                string[] badgeClasses = new string[] { "blue", "amber", "green", "purple", "teal", "red" };

                int colorIdx = (int.Parse(sno) - 1) % dotStyles.Length;
                string dotStyle   = dotStyles[colorIdx];
                string badgeClass = badgeClasses[colorIdx];

                string statusCell;
                string dateClass;
                string rowClass = isUpcoming ? " class=\"hol-upcoming\"" : "";
                if (isUpcoming)
                {
                    dateClass = "hol-date-main upcoming";
                    int daysAway = parsed ? (holidayDate.Date - today).Days : 0;
                    string daysText = daysAway == 0 ? "Today!" : daysAway + " day" + (daysAway == 1 ? "" : "s") + " away";
                    statusCell = string.Format("<span class=\"hol-days-away\"><i class=\"icon-alarm\" style=\"font-size:10px;\"></i> {0}</span>", daysText);
                }
                else
                {
                    dateClass = "hol-date-main";
                    statusCell = "<span class=\"hol-past-tag\">Passed</span>";
                }

                string weekday = parsed ? holidayDate.ToString("dddd") : "";

                string rowHtml = string.Format(
                    "<tr{0}>" +
                        "<td><span class=\"hol-sno\">{1}</span></td>" +
                        "<td><div class=\"{2}\">{3}</div><div class=\"hol-date-weekday\">{4}</div></td>" +
                        "<td><div class=\"hol-name-wrap\">" +
                            "<div class=\"hol-cal-dot\" style=\"{5}\"><i class=\"icon-calendar22\"></i></div>" +
                            "<div><div class=\"hol-name-text\">{6}</div><div class=\"hol-name-desc\">{7}</div></div>" +
                        "</div></td>" +
                        "<td><span class=\"hol-badge {8}\">{9}</span></td>" +
                        "<td style=\"text-align:center;\"><span style=\"display:inline-flex;align-items:center;justify-content:center;width:24px;height:24px;border-radius:6px;background:#E6F1FB;color:#0C447C;font-size:11px;font-weight:600;\">{10}</span></td>" +
                        "<td>{11}</td>" +
                    "</tr>",
                    rowClass, sno, dateClass, dateStr, weekday, dotStyle, description, day, badgeClass, day, noOfLeave, statusCell
                );

                sbAll.Append(rowHtml);
                if (isUpcoming) {
                    sbUpcoming.Append(rowHtml);
                } else {
                    sbPassed.Append(rowHtml);
                }
            }
        }

        if (sbAll.Length > 0)
            PH_All.Controls.Add(new LiteralControl(sbAll.ToString()));
        else
            PH_All.Controls.Add(new LiteralControl("<tr><td colspan='6' class='hol-empty' style='padding:24px;text-align:center;color:#aaa;'>No holidays</td></tr>"));

        if (sbUpcoming.Length > 0)
            PH_Upcoming.Controls.Add(new LiteralControl(sbUpcoming.ToString()));
        else
            PH_Upcoming.Controls.Add(new LiteralControl("<tr><td colspan='6' class='hol-empty' style='padding:24px;text-align:center;color:#aaa;'>No holidays</td></tr>"));

        if (sbPassed.Length > 0)
            PH_Passed.Controls.Add(new LiteralControl(sbPassed.ToString()));
        else
            PH_Passed.Controls.Add(new LiteralControl("<tr><td colspan='6' class='hol-empty' style='padding:24px;text-align:center;color:#aaa;'>No holidays</td></tr>"));

        lit_Total.Text      = totalCount.ToString();
        lit_Upcoming.Text   = upcomingCount.ToString();
        lit_Passed.Text     = passedCount.ToString();
        lit_TotalJS.Text    = totalCount.ToString();
        lit_UpcomingJS.Text = upcomingCount.ToString();
        lit_PassedJS.Text   = passedCount.ToString();
        lit_HeaderCount.Text = upcomingCount.ToString(); // Default: upcoming count on page load

        if (nextHolidayDate.HasValue)
        {
            int daysToNext = (nextHolidayDate.Value.Date - today).Days;
            string nextLabel = daysToNext == 0 ? "Today!" : "Next in " + daysToNext + " day" + (daysToNext == 1 ? "" : "s");
            lit_NextHoliday.Text = nextLabel;
        }
        else
        {
            lit_NextHoliday.Text = "No upcoming holidays";
        }
    }
}
