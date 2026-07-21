using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_UninformedLeaves : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_responsestatus = "";
    string str_userkey = "";
    string str_requestleave = "";
    string str_requestleave1 = "";
    private string key = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        if (!IsPostBack)
        {
            BindEmployeeDropdown();
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Self Services";
        }
    }


    private void BindEmployeeDropdown()
    {
        try
        {
            using (SqlConnection con = new SqlConnection(DA.ConnectionString))
            {
                // Select only real employees with a proper Employeeid
                string query = @"
                SELECT Employeekey, Firstname + ' ' + Lastname AS Username
                FROM IT_EmployeeRegister
                WHERE Employeestatus = 1 and Division in (1,2,3)
                ORDER BY Username";

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                ddlEmployee.DataSource = dt;
                ddlEmployee.DataTextField = "Username";   // Must match alias in SQL
                ddlEmployee.DataValueField = "Employeekey"; // Must match column name
                ddlEmployee.DataBind();

                ddlEmployee.Items.Insert(0, new ListItem("Select Employee", "0"));
            }
        }
        catch (Exception ex)
        {
            Response.Write(ex.Message);
        }
    }


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string empKey = ddlEmployee.SelectedValue;
        string empName = ddlEmployee.SelectedItem.Text;
    }


    static int CountDays(DayOfWeek day, DateTime start, DateTime end)
    {
        TimeSpan ts = end - start;                       // Total duration
        int count = (int)Math.Floor(ts.TotalDays / 7);   // Number of whole weeks
        int remainder = (int)(ts.TotalDays % 7);         // Number of remaining days
        int sinceLastDay = (int)(end.DayOfWeek - day);   // Number of days since last [day]
        if (sinceLastDay < 0) sinceLastDay += 7;         // Adjust for negative days since last [day]


        if (remainder >= sinceLastDay) count++;

        return count;
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
        // Parse and validate input dates with exact format dd/MM/yyyy
        DateTime dt_date;
        DateTime dtToInclusive;


        Guid myGuid;
        if (!Guid.TryParse(SC.Userid, out myGuid))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "infologia", "<script>alert('Invalid User GUID');</script>");
            return;
        }

        if (!DateTime.TryParseExact(txt_fromdate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt_date))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "infologia", "<script>alert('Invalid From date');</script>");
            return;
        }

        if (!DateTime.TryParseExact(txt_todate.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dtToInclusive))
        {
            ClientScript.RegisterStartupScript(this.GetType(), "infologia", "<script>alert('Invalid To date');</script>");
            return;
        }

        dt_date = dt_date.Date;
        dtToInclusive = dtToInclusive.Date;
        DateTime dtExclusive = dtToInclusive.AddDays(1);

        // Calculate total days and Sundays
        TimeSpan ts = dtExclusive - dt_date;
        int totalDaysBetween = ts.Days;

        int sundays = 0;
        for (DateTime d = dt_date; d < dtExclusive; d = d.AddDays(1))
        {
            if (d.DayOfWeek == DayOfWeek.Sunday)
                sundays++;
        }

        // Fetch holidays
        DataTable dtHolidays = new DataTable();
        List<DateTime> holidayDates = new List<DateTime>();
        int holidayCount = 0;

        using (SqlConnection conn = new SqlConnection(DA.ConnectionString))
        {
            conn.Open();

            using (SqlCommand cmdH = new SqlCommand(
                "SELECT Holidays FROM IT_Holidays WHERE Holidays >= @fromDate AND Holidays < @toDate", conn))
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

            // Count second Saturdays
            int secondSaturdayCount = 0;
            for (DateTime d = dt_date; d < dtExclusive; d = d.AddDays(1))
            {
                if (d.DayOfWeek == DayOfWeek.Saturday && d.Day >= 8 && d.Day <= 14)
                    secondSaturdayCount++;
            }

            int NoOfDays = totalDaysBetween - holidayCount - sundays - secondSaturdayCount;
            if (NoOfDays < 0) NoOfDays = 0;

            this.str_responsestatus = "1";
            this.str_userkey = SC.Userid.ToString();

            DateTime dt_current = DateTime.Now.Date;
            DateTime dt_fromdate = dt_date;
            DateTime dt_todate = dtToInclusive;

            // Validate previous dates only
            if (dt_fromdate >= DateTime.Today || dt_todate >= DateTime.Today)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "infologia",
                "<script>alert('Only previous dates are allowed');</script>");
                return;
            }

            if (dt_fromdate > dt_todate)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "infologia", "<script>alert('From date cannot be greater than To date');</script>");
                return;
            }

            // Check duplicates
            using (SqlCommand cmdChkFrom = new SqlCommand(
                "SELECT 1 FROM IT_EmployeeLeaveDetails WHERE Fromdate = @fromDate AND Employeekey = @empKey", conn))
            {
                cmdChkFrom.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = dt_fromdate;
                cmdChkFrom.Parameters.Add("@empKey", SqlDbType.VarChar).Value = SC.Userid.ToString();
                object o = cmdChkFrom.ExecuteScalar();
                if (o != null)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "infologia", "<script>alert('Already Taken (From Date).');</script>");
                    return;
                }
            }

            using (SqlCommand cmdChkTo = new SqlCommand(
                "SELECT 1 FROM IT_EmployeeLeaveDetails WHERE Todate = @toDate AND Employeekey = @empKey", conn))
            {
                cmdChkTo.Parameters.Add("@toDate", SqlDbType.DateTime).Value = dt_todate;
                cmdChkTo.Parameters.Add("@empKey", SqlDbType.VarChar).Value = SC.Userid.ToString();
                object o = cmdChkTo.ExecuteScalar();
                if (o != null)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "infologia", "<script>alert('Already Taken (To Date).');</script>");
                    return;
                }
            }

            // Transaction to insert leave
            using (SqlTransaction transaction = conn.BeginTransaction("Leavedays"))
            using (SqlCommand cmd = conn.CreateCommand())
            {
                cmd.Transaction = transaction;
                try
                {
                    cmd.CommandText = @"INSERT INTO IT_EmployeeLeaveDetails (Employeekey, Createdby, Fromdate, Todate, Reason, Responsestatus, leavedays) OUTPUT INSERTED.Employeeleavedetailskey VALUES (@Employeekey, @Createdby, @Fromdate, @Todate, @Reason, @Responsestatus, @leavedays);";

                    string empGuid = ddlEmployee.SelectedValue;
                    Guid empGuidValue;
                    if (!Guid.TryParse(empGuid, out empGuidValue))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "infologia", "<script>alert('Invalid Employee GUID');</script>");
                        return;
                    }

                    Guid createdByGuid;
                    if (!Guid.TryParse(str_userkey, out createdByGuid))
                    {
                        ClientScript.RegisterStartupScript(this.GetType(), "infologia", "<script>alert('Invalid CreatedBy GUID');</script>");
                        return;
                    }

                    cmd.Parameters.Clear();
                    cmd.Parameters.Add("@Employeekey", SqlDbType.UniqueIdentifier).Value = empGuidValue;
                    cmd.Parameters.Add("@Createdby", SqlDbType.UniqueIdentifier).Value = createdByGuid;
                    cmd.Parameters.Add("@Fromdate", SqlDbType.DateTime).Value = dt_fromdate;
                    cmd.Parameters.Add("@Todate", SqlDbType.DateTime).Value = dt_todate;
                    cmd.Parameters.Add("@Reason", SqlDbType.NVarChar).Value = txt_reason.InnerText.Trim();
                    cmd.Parameters.Add("@Responsestatus", SqlDbType.NVarChar).Value = str_responsestatus;
                    cmd.Parameters.Add("@leavedays", SqlDbType.Decimal).Value = NoOfDays;

                    object newIdObj = cmd.ExecuteScalar();
                    if (newIdObj == null || newIdObj == DBNull.Value)
                        throw new Exception("Failed to insert leave record (GUID not returned).");

                    Guid leaveKey = (Guid)newIdObj;

                    List<DateTime> dates = GetDatesBetween(dt_date, dtExclusive);

                    foreach (DateTime dtdate in dates)
                    {
                        DateTime dateOnly = dtdate.Date;
                        if (holidayDates.Contains(dateOnly)) continue;
                        if (dateOnly.DayOfWeek == DayOfWeek.Sunday) continue;
                        if (dateOnly.DayOfWeek == DayOfWeek.Saturday && dateOnly.Day >= 8 && dateOnly.Day <= 14) continue;

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

                    ClientScript.RegisterStartupScript(
                        this.GetType(),
                        "infologia",
                        "<script>alert('Error: " + ex.Message.Replace("'", "") + "');</script>"
                    );

                    return;
                }
            } // transaction disposed
        } // connection disposed

        Response.Redirect("~/Admin/LeaveResponse.aspx");


}



}