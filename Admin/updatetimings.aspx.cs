using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class Admin_updatetimings : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;

    // ==================================================================================
    // NOTE ON IT_Lunch : this table has no Employeekey/date column of its own and no
    // direct link to IT_InOutTime. Confirmed with the client: "Createdby" on IT_Lunch
    // actually holds the employee's Employeekey, so a Lunch row for a given employee/day
    // is found via Createdby = @Employeekey AND CAST(Createdon AS DATE) = @SelectedDate
    // (same pattern as the IT_InOutTime lookup above).
    // ==================================================================================

    private Guid? GetCurrentAdminKey()
    {
        Guid g;
        if (SC != null && Guid.TryParse(SC.Userid, out g))
            return g;
        return null;
    }

    // ==================================================================================
    // NOTE ON TIME ZONE : InTime / OutTime / LunchIn / LunchOut are stored in the
    // database as UTC. The admin picks/sees times as local (India) time on this page,
    // so every value coming FROM the database must be converted UTC -> IST before it's
    // shown, and every value going back INTO the database must be converted IST -> UTC
    // first. India does not observe daylight saving, so a fixed +5:30 offset is safe.
    // ==================================================================================
    private static readonly TimeSpan IstOffset = TimeSpan.FromMinutes(5 * 60 + 30);

    private static DateTime UtcToIst(DateTime utcValue)
    {
        return utcValue.Add(IstOffset);
    }

    private static DateTime IstToUtc(DateTime istValue)
    {
        return istValue.Subtract(IstOffset);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        if (!IsPostBack)
        {
            BindEmployeeDropdown();
            ClearTimingFields();
        }
    }

    private void BindEmployeeDropdown()
    {
        const string sql = @"SELECT Employeekey, Employeeid, Firstname, Lastname
                              FROM IT_EmployeeRegister
                              WHERE Employeestatus = 1
                              ORDER BY Firstname, Lastname";

        SqlCommand cmd = new SqlCommand(sql);
        DataTable dt = DA.GetDataTable(cmd);

        if (!dt.Columns.Contains("DisplayName"))
        {
            dt.Columns.Add("DisplayName", typeof(string));
            foreach (DataRow row in dt.Rows)
            {
                row["DisplayName"] = string.Format("{0} {1}",
                    row["Firstname"], row["Lastname"]);
            }
        }

        ddlEmployee.DataSource = dt;
        ddlEmployee.DataTextField = "DisplayName";
        ddlEmployee.DataValueField = "Employeekey";
        ddlEmployee.DataBind();
        ddlEmployee.Items.Insert(0, new ListItem("-- Select Employee --", ""));
    }

    protected void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadTimings();
    }

    protected void txtSelectedDate_TextChanged(object sender, EventArgs e)
    {
        LoadTimings();
    }


    private void LoadTimings()
    {
        ClearTimingFields();

        if (string.IsNullOrEmpty(ddlEmployee.SelectedValue) || string.IsNullOrWhiteSpace(txtSelectedDate.Text))
        {
            litMessage.Text = "";
            return;
        }

        Guid employeeKey;
        if (!Guid.TryParse(ddlEmployee.SelectedValue, out employeeKey)) return;

        DateTime selectedDate;
        // pickadate can display the date as "13 July, 2026" (its default display format)
        // as well as dd/mm/yyyy, so we try several known formats before giving up.
        string[] formats = { "d MMMM, yyyy", "d MMM, yyyy", "dd/MM/yyyy", "d/M/yyyy", "dd-MM-yyyy", "yyyy-MM-dd", "MM/dd/yyyy" };
        if (!DateTime.TryParseExact(txtSelectedDate.Text.Trim(), formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out selectedDate))
        {
            if (!DateTime.TryParse(txtSelectedDate.Text.Trim(), out selectedDate))
            {
                litMessage.Text = "<div class='alert alert-danger'><i class='icon-cross2 position-left'></i>Invalid filter date format. Please pick the date from the calendar.</div>";
                return;
            }
        }

        const string inOutSql = @"SELECT TOP 1 InOutTimekey, InTime, OutTime
                                   FROM IT_InOutTime
                                   WHERE Employeekey = @Employeekey
                                     AND CAST(Createdon AS DATE) = @SelectedDate
                                   ORDER BY Createdon DESC";

        Guid? inOutTimeKey = null;

        SqlCommand cmdInOut = new SqlCommand(inOutSql);
        cmdInOut.Parameters.AddWithValue("@Employeekey", employeeKey);
        cmdInOut.Parameters.AddWithValue("@SelectedDate", selectedDate.Date);
        DataTable dtInOut = DA.GetDataTable(cmdInOut);

        if (dtInOut.Rows.Count > 0)
        {
            DataRow rdr = dtInOut.Rows[0];
            inOutTimeKey = (Guid)rdr["InOutTimekey"];
            hfInOutTimekey.Value = inOutTimeKey.ToString();

            if (rdr["InTime"] != DBNull.Value)
            {
                DateTime inTime = UtcToIst((DateTime)rdr["InTime"]);
                txtInDate.Text = inTime.ToString("dd/MM/yyyy");
                txtInTime.Text = inTime.ToString("hh:mm tt");
                hfOrigInTime.Value = inTime.ToString("o");
            }

            if (rdr["OutTime"] != DBNull.Value)
            {
                DateTime outTime = UtcToIst((DateTime)rdr["OutTime"]);
                txtOutDate.Text = outTime.ToString("dd/MM/yyyy");
                txtOutTime.Text = outTime.ToString("hh:mm tt");
                hfOrigOutTime.Value = outTime.ToString("o");
            }
        }

        bool lunchLoadFailed = false;

        try
        {
            const string lunchSql = @"SELECT TOP 1 LunchTimekey, LunchIn, LunchOut
                                       FROM IT_Lunch
                                       WHERE Createdby = @Employeekey
                                         AND CAST(Createdon AS DATE) = @SelectedDate
                                       ORDER BY Createdon DESC";

            SqlCommand cmdLunch = new SqlCommand(lunchSql);
            cmdLunch.Parameters.AddWithValue("@Employeekey", employeeKey);
            cmdLunch.Parameters.AddWithValue("@SelectedDate", selectedDate.Date);
            DataTable dtLunch = DA.GetDataTable(cmdLunch);

            if (dtLunch.Rows.Count > 0)
            {
                DataRow rdr = dtLunch.Rows[0];
                hfLunchTimekey.Value = rdr["LunchTimekey"].ToString();

                if (rdr["LunchIn"] != DBNull.Value)
                {
                    DateTime lunchIn = UtcToIst((DateTime)rdr["LunchIn"]);
                    txtLunchInDate.Text = lunchIn.ToString("dd/MM/yyyy");
                    txtLunchInTime.Text = lunchIn.ToString("hh:mm tt");
                    hfOrigLunchIn.Value = lunchIn.ToString("o");
                }

                if (rdr["LunchOut"] != DBNull.Value)
                {
                    DateTime lunchOut = UtcToIst((DateTime)rdr["LunchOut"]);
                    txtLunchOutDate.Text = lunchOut.ToString("dd/MM/yyyy");
                    txtLunchOutTime.Text = lunchOut.ToString("hh:mm tt");
                    hfOrigLunchOut.Value = lunchOut.ToString("o");
                }
            }
        }
        catch (SqlException)
        {
            lunchLoadFailed = true;
        }

        if (lunchLoadFailed)
        {
            litMessage.Text = "<div class='alert alert-warning'><i class='icon-warning2 position-left'></i>In/Out time loaded, but Lunch details could not be loaded.</div>";
        }
        else
        {
            litMessage.Text = inOutTimeKey.HasValue
                ? "<div class='alert alert-success'><i class='icon-checkmark3 position-left'></i>Existing timings loaded for the selected date.</div>"
                : "<div class='alert alert-info'><i class='icon-info22 position-left'></i>No record found for this date yet - fill in the punches and click Update to add them.</div>";
        }
    }

    private void ClearTimingFields()
    {
        txtInDate.Text = txtInTime.Text = "";
        txtOutDate.Text = txtOutTime.Text = "";
        txtLunchInDate.Text = txtLunchInTime.Text = "";
        txtLunchOutDate.Text = txtLunchOutTime.Text = "";

        hfInOutTimekey.Value = "";
        hfLunchTimekey.Value = "";
        hfOrigInTime.Value = "";
        hfOrigOutTime.Value = "";
        hfOrigLunchIn.Value = "";
        hfOrigLunchOut.Value = "";
    }

    private DateTime? CombineDateTime(TextBox dateBox, TextBox timeBox)
    {
        string dateStr = string.IsNullOrWhiteSpace(dateBox.Text) ? txtSelectedDate.Text : dateBox.Text;
        
        // ASP.NET ignores client-side changes to ReadOnly="true" TextBoxes during PostBack.
        // We must fetch the actual submitted value from Request.Form using the UniqueID.
        string timeStr = Request.Form[timeBox.UniqueID];
        if (string.IsNullOrEmpty(timeStr)) 
            timeStr = timeBox.Text;

        if (string.IsNullOrWhiteSpace(dateStr) || string.IsNullOrWhiteSpace(timeStr))
            return null;

        DateTime d;
        string[] dateFormats = { "d MMMM, yyyy", "d MMM, yyyy", "dd/MM/yyyy", "d/M/yyyy", "dd-MM-yyyy", "yyyy-MM-dd", "MM/dd/yyyy" };
        if (!DateTime.TryParseExact(dateStr.Trim(), dateFormats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out d))
        {
            if (!DateTime.TryParse(dateStr.Trim(), out d))
                return null;
        }

        // Time field now displays as 12-hour with AM/PM (e.g. "09:46 AM"). Try that
        // format explicitly first (culture-independent), then fall back to 24hr HH:mm
        // and finally a general TryParse, in case of any other typed format.
        TimeSpan t;
        DateTime timeOnly;
        string[] timeFormats = { "hh:mm tt", "h:mm tt", "HH:mm", "H:mm" };
        if (DateTime.TryParseExact(timeStr.Trim(), timeFormats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out timeOnly))
        {
            t = timeOnly.TimeOfDay;
        }
        else if (!TimeSpan.TryParse(timeStr.Trim(), out t))
        {
            DateTime tempDt;
            if (DateTime.TryParse(timeStr.Trim(), out tempDt))
                t = tempDt.TimeOfDay;
            else
                return null;
        }
        return d.Date + t;
    }

    private bool ValuesDiffer(string originalIso, DateTime? newValue)
    {
        DateTime? original = string.IsNullOrEmpty(originalIso) ? (DateTime?)null : DateTime.Parse(originalIso);

        if (!original.HasValue && !newValue.HasValue) return false;
        if (original.HasValue != newValue.HasValue) return true;
        return original.Value != newValue.Value;
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(ddlEmployee.SelectedValue) || string.IsNullOrWhiteSpace(txtSelectedDate.Text))
        {
            litMessage.Text = "<div class='alert alert-danger'><i class='icon-cross2 position-left'></i>Please select an employee and a date first.</div>";
            return;
        }

        Guid employeeKey = Guid.Parse(ddlEmployee.SelectedValue);
        Guid? adminKey = GetCurrentAdminKey();

        DateTime? newInTime = CombineDateTime(txtInDate, txtInTime);
        DateTime? newOutTime = CombineDateTime(txtOutDate, txtOutTime);
        DateTime? newLunchIn = CombineDateTime(txtLunchInDate, txtLunchInTime);
        DateTime? newLunchOut = CombineDateTime(txtLunchOutDate, txtLunchOutTime);
        // NOTE: newInTime/newOutTime/newLunchIn/newLunchOut are IST here (same as what's
        // shown on screen and what hfOrig* holds), so ValuesDiffer compares like-for-like.
        // They get converted to UTC individually, right before being written to the
        // database, further down.

        if (string.IsNullOrEmpty(hfInOutTimekey.Value))
        {
            litMessage.Text = "<div class='alert alert-danger'><i class='icon-cross2 position-left'></i>No existing record found for this employee on this date - there is nothing to update.</div>";
            return;
        }

        Guid inOutTimeKey = Guid.Parse(hfInOutTimekey.Value);

        bool inChanged = ValuesDiffer(hfOrigInTime.Value, newInTime);
        bool outChanged = ValuesDiffer(hfOrigOutTime.Value, newOutTime);

        if (inChanged || outChanged)
        {
            System.Text.StringBuilder setClause = new System.Text.StringBuilder();
            if (inChanged) setClause.Append("InTime = @InTime, ");
            if (outChanged) setClause.Append("OutTime = @OutTime, ");
            setClause.Append("Modifiedon = GETDATE(), Modifiedby = @Modifiedby");

            string updateSql = "UPDATE IT_InOutTime SET " + setClause + " WHERE InOutTimekey = @InOutTimekey";
            SqlCommand cmd = new SqlCommand(updateSql);
            if (inChanged) cmd.Parameters.AddWithValue("@InTime", newInTime.HasValue ? (object)IstToUtc(newInTime.Value) : DBNull.Value);
            if (outChanged) cmd.Parameters.AddWithValue("@OutTime", newOutTime.HasValue ? (object)IstToUtc(newOutTime.Value) : DBNull.Value);
            cmd.Parameters.AddWithValue("@Modifiedby", (object)adminKey ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@InOutTimekey", inOutTimeKey);
            DA.ExecuteNonQuery(cmd);
        }

        bool lunchUpdateFailed = false;

        if (!string.IsNullOrEmpty(hfLunchTimekey.Value))
        {
            Guid lunchKey = Guid.Parse(hfLunchTimekey.Value);
            bool lunchInChanged = ValuesDiffer(hfOrigLunchIn.Value, newLunchIn);
            bool lunchOutChanged = ValuesDiffer(hfOrigLunchOut.Value, newLunchOut);

            if (lunchInChanged || lunchOutChanged)
            {
                try
                {
                    System.Text.StringBuilder setClause = new System.Text.StringBuilder();
                    if (lunchInChanged) setClause.Append("LunchIn = @LunchIn, ");
                    if (lunchOutChanged) setClause.Append("LunchOut = @LunchOut, ");
                    setClause.Append("Modifiedon = GETDATE()");

                    string updateLunchSql = "UPDATE IT_Lunch SET " + setClause + " WHERE LunchTimekey = @LunchTimekey";
                    SqlCommand cmd = new SqlCommand(updateLunchSql);
                    if (lunchInChanged) cmd.Parameters.AddWithValue("@LunchIn", newLunchIn.HasValue ? (object)IstToUtc(newLunchIn.Value) : DBNull.Value);
                    if (lunchOutChanged) cmd.Parameters.AddWithValue("@LunchOut", newLunchOut.HasValue ? (object)IstToUtc(newLunchOut.Value) : DBNull.Value);
                    cmd.Parameters.AddWithValue("@LunchTimekey", lunchKey);
                    DA.ExecuteNonQuery(cmd);
                }
                catch (SqlException)
                {
                    lunchUpdateFailed = true;
                }
            }
        }
        else if (newLunchIn.HasValue || newLunchOut.HasValue)
        {
            litMessage.Text = "<div class='alert alert-warning'><i class='icon-warning2 position-left'></i>No existing lunch record found for this date, so the lunch time(s) were not saved (In/Out Time was updated).</div>";
            return;
        }

        string finalMsg = lunchUpdateFailed
            ? "<div class='alert alert-warning'><i class='icon-warning2 position-left'></i>In/Out Time updated, but Lunch could not be saved. Please try again or check the Lunch record.</div>"
            : "<div class='alert alert-success'><i class='icon-checkmark3 position-left'></i>Timings updated successfully.</div>";

        // reload so the fields + hidden "original value" trackers reflect what's now saved
        LoadTimings();

        // Overwrite litMessage with our final action result instead of LoadTimings() default message
        litMessage.Text = finalMsg;
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearTimingFields();
        litMessage.Text = "";
    }
}
