using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Holidays : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_userid = "";
    string str_id = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.str_userid = SC.Userid.ToString();

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Holidays";

            this.LoadYearFilter();
        }

        // Key logic moved to client-side modal popup (no page reload)

        this.LoadStats();
        this.Loadgrid();
    }

    private void LoadYearFilter()
    {
        ddl_year.Items.Clear();
        ddl_year.Items.Add(new ListItem("Year: All", ""));
        int currentYear = DateTime.Now.Year;
        for (int y = currentYear - 2; y <= currentYear + 2; y++)
        {
            ddl_year.Items.Add(new ListItem(y.ToString(), y.ToString()));
        }
        ddl_year.SelectedValue = currentYear.ToString();
    }

    private void Assignvalues()
    {
        try 
        {
            string str_query = "SELECT Holidays,description,Day,NoOfLeave,Holidayskey,HolidayType,Status from IT_Holidays Where Holidayskey='" + this.str_id + "' ";
            SqlCommand cmd = new SqlCommand(str_query);
            DataTable dt_assign = DA.GetDataTable(cmd);

            if (dt_assign.Rows.Count > 0)
            {
                txt_desc.Text = dt_assign.Rows[0]["description"].ToString();
                txt_day.Text = dt_assign.Rows[0]["Day"].ToString();
                txt_nofday.Text = dt_assign.Rows[0]["NoOfLeave"].ToString();
                txt_date.Text = dt_assign.Rows[0]["Holidays"] != DBNull.Value
                    ? Convert.ToDateTime(dt_assign.Rows[0]["Holidays"]).ToString("dd/MM/yyyy")
                    : "";

                string holidayType = dt_assign.Rows[0]["HolidayType"] != DBNull.Value ? dt_assign.Rows[0]["HolidayType"].ToString() : "Public";
                string status = dt_assign.Rows[0]["Status"] != DBNull.Value ? dt_assign.Rows[0]["Status"].ToString() : "Active";

                if (ddl_holidayType.Items.FindByValue(holidayType) != null)
                    ddl_holidayType.SelectedValue = holidayType;
                if (ddl_holidayStatus.Items.FindByValue(status) != null)
                    ddl_holidayStatus.SelectedValue = status;
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "assignErr", "alert('Backend Error: " + ex.Message.Replace("'", "") + "');", true);
        }
    }

    private void LoadStats()
    {
        string str_query = @"SELECT
                                (SELECT COUNT(*) FROM IT_Holidays) AS TotalCount,
                                (SELECT COUNT(*) FROM IT_Holidays WHERE Holidays BETWEEN GETDATE() AND DATEADD(DAY,30,GETDATE())) AS UpcomingCount,
                                (SELECT COUNT(*) FROM IT_Holidays WHERE HolidayType='Public') AS PublicCount,
                                (SELECT COUNT(*) FROM IT_Holidays WHERE HolidayType='Restricted') AS OptionalCount";

        SqlCommand cmd = new SqlCommand(str_query);
        DataTable dt = DA.GetDataTable(cmd);

        if (dt.Rows.Count > 0)
        {
            lit_total.Text = dt.Rows[0]["TotalCount"].ToString();
            lit_upcoming.Text = dt.Rows[0]["UpcomingCount"].ToString();
            lit_public.Text = dt.Rows[0]["PublicCount"].ToString();
            lit_optional.Text = dt.Rows[0]["OptionalCount"].ToString();
        }
    }

    private void Loadgrid()
    {
        string str_query = @"SELECT CONVERT(varchar, Holidays, 103) AS Holidays,description,Day,NoOfLeave,Holidayskey,
                              ISNULL(HolidayType,'Public') AS HolidayType, ISNULL(Status,'Active') AS Status
                              FROM IT_Holidays WHERE 1=1 ";

        if (!string.IsNullOrEmpty(ddl_year.SelectedValue))
            str_query += " AND YEAR(Holidays) = @Year ";
        if (!string.IsNullOrEmpty(ddl_type.SelectedValue))
            str_query += " AND HolidayType = @Type ";
        if (!string.IsNullOrEmpty(ddl_status.SelectedValue))
            str_query += " AND Status = @Status ";

        str_query += " ORDER BY Holidays ASC";

        SqlCommand cmd = new SqlCommand(str_query);
        if (!string.IsNullOrEmpty(ddl_year.SelectedValue))
            cmd.Parameters.AddWithValue("@Year", ddl_year.SelectedValue);
        if (!string.IsNullOrEmpty(ddl_type.SelectedValue))
            cmd.Parameters.AddWithValue("@Type", ddl_type.SelectedValue);
        if (!string.IsNullOrEmpty(ddl_status.SelectedValue))
            cmd.Parameters.AddWithValue("@Status", ddl_status.SelectedValue);

        DataTable dt_grid = DA.GetDataTable(cmd);
        rpt_Holidays.DataSource = dt_grid;
        rpt_Holidays.DataBind();
    }

    protected void ddl_Filter_Changed(object sender, EventArgs e)
    {
        this.LoadStats();
        this.Loadgrid();
    }

    protected void rpt_Holidays_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        // reserved for future repeater commands
    }

    private object ParseDateOrDBNull(string dateText)
    {
        if (string.IsNullOrWhiteSpace(dateText))
            return DBNull.Value;

        DateTime parsedDate;
        if (DateTime.TryParseExact(
            dateText,
            new string[] { "dd/MM/yyyy", "d MMMM, yyyy", "dd MMMM, yyyy", "yyyy-MM-dd" },
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out parsedDate))
        {
            return parsedDate;
        }

        return DBNull.Value;
    }

    protected void btn_submit_Click(object sender, EventArgs e)
    {
        DateTime holidayDate;
        if (!DateTime.TryParseExact(
            txt_date.Text.Trim(),
            new string[] { "dd/MM/yyyy", "d MMMM, yyyy", "dd MMMM, yyyy", "yyyy-MM-dd" },
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out holidayDate))
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr.error('Invalid date format.');", true);
            return;
        }

        string editKey = hf_holidayKey.Value;

        if (string.IsNullOrEmpty(editKey))
        {
            string str_check = "SELECT * FROM IT_Holidays WHERE Holidays = @Holidays";
            SqlCommand cmd1 = new SqlCommand(str_check);
            cmd1.Parameters.Add("@Holidays", SqlDbType.Date).Value = holidayDate;
            DataTable dt_check = DA.GetDataTable(cmd1);

            if (dt_check.Rows.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr.error('Already created');", true);
                return;
            }

            string str_sqlinsert = @"insert into IT_Holidays(Holidays,description,Day,NoOfLeave,createdby,HolidayType,Status)
                                      values(@Holidays,@description,@Day,@NoOfLeave,@createdby,@HolidayType,@Status)";
            SqlCommand cmd = new SqlCommand(str_sqlinsert);
            cmd.Parameters.AddWithValue("@Holidays", ParseDateOrDBNull(txt_date.Text));
            cmd.Parameters.AddWithValue("@description", txt_desc.Text);
            cmd.Parameters.AddWithValue("@Day", txt_day.Text);
            cmd.Parameters.AddWithValue("@NoOfLeave", txt_nofday.Text);
            cmd.Parameters.AddWithValue("@createdby", str_userid);
            cmd.Parameters.AddWithValue("@HolidayType", ddl_holidayType.SelectedValue);
            cmd.Parameters.AddWithValue("@Status", ddl_holidayStatus.SelectedValue);
            DA.ExecuteNonQuery(cmd);

            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr.success('Holiday Submitted Successfully'); setTimeout(function(){ window.location.href='holidays.aspx'; }, 2000);", true);
        }
        else
        {
            string str_sqlupdate = @"update IT_Holidays set modifiedby=@modifiedby,modifiedon=getdate(),Holidays=@Holidays,
                                      Description=@Description,Day=@Day,NoOfLeave=@NoOfLeave,HolidayType=@HolidayType,Status=@Status
                                      where Holidayskey=@Holidayskey";
            SqlCommand cmd = new SqlCommand(str_sqlupdate);
            cmd.Parameters.AddWithValue("@Holidayskey", editKey);
            cmd.Parameters.AddWithValue("@Holidays", ParseDateOrDBNull(txt_date.Text));
            cmd.Parameters.AddWithValue("@description", txt_desc.Text);
            cmd.Parameters.AddWithValue("@Day", txt_day.Text);
            cmd.Parameters.AddWithValue("@NoOfLeave", txt_nofday.Text);
            cmd.Parameters.AddWithValue("@modifiedby", str_userid);
            cmd.Parameters.AddWithValue("@HolidayType", ddl_holidayType.SelectedValue);
            cmd.Parameters.AddWithValue("@Status", ddl_holidayStatus.SelectedValue);
            DA.ExecuteNonQuery(cmd);

            ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr.success('Holiday Updated Successfully'); setTimeout(function(){ window.location.href='holidays.aspx'; }, 2000);", true);
        }
    }

    [WebMethod] //Delete
    public static string DeleteProject(string Holidayskey)
    {
        string str_Response = "0";
        try
        {
            DataAccess DA1 = new DataAccess();
            string str_Sql = "delete from IT_Holidays where Holidayskey=@Holidayskey";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@Holidayskey", Holidayskey);
            DA1.ExecuteNonQuery(cmd);
            return str_Response = "1";
        }
        catch (Exception ex)
        {
            return str_Response;
        }
    }
}
