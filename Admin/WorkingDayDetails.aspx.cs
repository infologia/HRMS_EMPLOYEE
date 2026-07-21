using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class WEB_WorkingDayDetails : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    CommonFunction CF;
    
    string str_userid = "";
    string str_id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();
        this.CF = new CommonFunction();

        this.str_userid = SC.Userid.ToString();

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Employee Pay";
        }
        if (!IsPostBack)
        {
            BindYearDropdown();
            CalculateDays();

        }
        if (Request.QueryString["id"] != null && Request.QueryString["id"] != "")
        {
            this.str_id = Request.QueryString["id"].ToString();
        }
        if (!IsPostBack)
        {
            CF.LoadMonthToDropdown(ddl_month);
        }
        if (this.str_id != null && this.str_id != "")
        {
            if (!IsPostBack)
            {
                btn_submit.Visible = false;
                this.Loadlanguage();
            }
        }
        else
        {
            btn_submit.Visible = true;
            btn_update.Visible = false;
        }
    }
    public string GetDaysNameOptions()
    {
        string str_query = "select DN_ID, DN_Name from IT_DaysName order by DN_ID";
        SqlCommand cmd = new SqlCommand(str_query);
        DataTable dt = DA.GetDataTable(cmd);
        string options = "<option value=''>Select Day</option>";
        foreach (DataRow row in dt.Rows)
        {
            options += "<option value='" + row["DN_Name"].ToString() + "'>" + row["DN_Name"].ToString() + "</option>";
        }
        return options;
    }
    private void BindYearDropdown()
    {
        ddl_year.Items.Clear();

        int startYear = 2012;
        int endYear = 2030;

        for (int y = startYear; y <= endYear; y++)
        {
            ddl_year.Items.Add(new ListItem(y.ToString(), y.ToString()));
        }

        ddl_year.SelectedValue = DateTime.Now.Year.ToString();
    }

    private void Loadlanguage()
    {
        string str_query = "select year,monthvalue,numberofdaysinmonth,numberofworkdaysinmonth from IT_EmployeeWorkingDayDetails  where Employeeworkingdaydetailskey=@Employeeworkingdaydetailskey";
        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@Employeeworkingdaydetailskey", str_id);
        DataTable dt_dashboard = DA.GetDataTable(cmd);
        if (dt_dashboard.Rows.Count > 0)
        {
            ddl_year.SelectedValue = dt_dashboard.Rows[0]["year"].ToString();
            ddl_month.SelectedValue = dt_dashboard.Rows[0]["monthvalue"].ToString();
            txt_days.Text = dt_dashboard.Rows[0]["numberofdaysinmonth"].ToString();
            txt_work.Text = dt_dashboard.Rows[0]["numberofworkdaysinmonth"].ToString();

        }
        LoadHolidayDetails();
    }

    private void LoadHolidayDetails()
    {
        string str_query = "SELECT ewd.date, dn.DN_Name as day, ewd.description FROM IT_EmployeeWorkingDaysubtable ewd LEFT JOIN IT_DaysName dn ON ewd.day = dn.DN_ID WHERE ewd.Employeeworkingdaydetailskey=@Employeeworkingdaydetailskey ORDER BY ewd.date";
        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@Employeeworkingdaydetailskey", str_id);
        DataTable dt = DA.GetDataTable(cmd);

        if (dt.Rows.Count > 0)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.Append("<script>setTimeout(function() {");
            
            foreach (DataRow row in dt.Rows)
            {
                string date = Convert.ToDateTime(row["date"]).ToString("dd/MM/yyyy");
                string day = row["day"] != DBNull.Value ? row["day"].ToString() : "";
                string desc = row["description"].ToString().Replace("'", "\\'").Replace("\"", "&quot;");

                sb.Append("var tbody = $('#tableBody');");
                sb.Append("var template = document.getElementById('daysTemplate');");
                sb.Append("var daysOptions = template.innerHTML;");
                sb.Append("var rowHtml = '<tr>' +");
                sb.AppendFormat("'<td><input type=\"text\" class=\"form-control form-control-sm\" name=\"holiday_date\" value=\"{0}\" /></td>' +", date);
                sb.Append("'<td><select class=\"form-control form-control-sm\" name=\"holiday_day\">' + daysOptions + '</select></td>' +");
                sb.AppendFormat("'<td><input type=\"text\" class=\"form-control form-control-sm\" name=\"holiday_desc\" value=\"{0}\" /></td>' +", desc);
                sb.Append("'<td class=\"text-center\"><button type=\"button\" class=\"btn btn-danger btn-xs\" onclick=\"removeRow(this)\">X</button></td>' +");
                sb.Append("'</tr>';");
                sb.Append("var newRow = $(rowHtml);");
                sb.Append("tbody.append(newRow);");
                sb.AppendFormat("newRow.find('select[name=\"holiday_day\"]').val('{0}');", day);
                sb.Append("newRow.find('input[name=\"holiday_date\"]').pickadate({ format: 'dd/mm/yyyy' });");
            }
            
            sb.Append("}, 100);</script>");
            ClientScript.RegisterStartupScript(this.GetType(), "LoadHolidays", sb.ToString());
        }
    }

    private void CalculateDays()
    {
        if (string.IsNullOrEmpty(ddl_year.SelectedValue) ||
            string.IsNullOrEmpty(ddl_month.SelectedValue))
            return;
        int year = Convert.ToInt32(ddl_year.SelectedValue);
        int month = Convert.ToInt32(ddl_month.SelectedValue);

        txt_days.Text = DateTime.DaysInMonth(year, month).ToString();
    }
    protected void ddl_month_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddl_year.SelectedValue == "" || ddl_month.SelectedValue == "")
            return;

        int year = Convert.ToInt32(ddl_year.SelectedValue);
        int month = Convert.ToInt32(ddl_month.SelectedValue);

        int days = DateTime.DaysInMonth(year, month);
        txt_days.Text = days.ToString();
    }
    protected void ddl_year_SelectedIndexChanged(object sender, EventArgs e)
    {
        CalculateDays();
    }
    protected void btn_submit_Click(object sender, EventArgs e)
    {
        String str_check = "select * from IT_EmployeeWorkingDayDetails  where Year=@year and monthvalue=@monthvalue";
        SqlCommand cmd1 = new SqlCommand(str_check);
        cmd1.Parameters.AddWithValue("@Year",ddl_year.SelectedValue);
        cmd1.Parameters.AddWithValue("@monthvalue", ddl_month.SelectedValue);
        DataTable dt_check = DA.GetDataTable(cmd1);
        if (dt_check.Rows.Count>0)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Infologia", "<script>alert('Already created for this month and year')</script>");
            return;
        
        }
        int selectedmonth = Convert.ToInt16(ddl_month.SelectedValue);
        int selectedyear = Convert.ToInt16(ddl_year.SelectedValue);
        DateTime dt_currentmonth = DateTime.Now;
        int month = dt_currentmonth.Month;
        int year = dt_currentmonth.Year;
        if (selectedyear > year && selectedmonth > month)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "Infologia", "<script>alert('Please select valid month and year')</script>");
            return;
        }

        int day = int.Parse(txt_days.Text);
        int workingday = int.Parse(txt_work.Text);
        int leavedays = day - workingday;

        String str_sql = "insert into IT_EmployeeWorkingDayDetails(Year,monthvalue,Numberofdaysinmonth,Numberofworkdaysinmonth,Numberofleavedaysinmonth,createdby)values(@Year,@monthvalue,@Numberofdaysinmonth,@Numberofworkdaysinmonth,@Numberofleavedaysinmonth,@createdby)";
        SqlCommand cmd = new SqlCommand(str_sql);
        cmd.Parameters.AddWithValue("@Year",ddl_year.SelectedValue);
        cmd.Parameters.AddWithValue("@monthvalue", ddl_month.SelectedValue);
        cmd.Parameters.AddWithValue("@Numberofdaysinmonth", day);
        cmd.Parameters.AddWithValue("@Numberofworkdaysinmonth", workingday);
        cmd.Parameters.AddWithValue("@Numberofleavedaysinmonth", leavedays);
        cmd.Parameters.AddWithValue("@createdby", str_userid);
        
        DA.ExecuteNonQuery(cmd);

        string str_getId = "SELECT TOP 1 Employeeworkingdaydetailskey FROM IT_EmployeeWorkingDayDetails WHERE Year=@Year AND monthvalue=@monthvalue ORDER BY Employeeworkingdaydetailskey DESC";
        SqlCommand cmd_getId = new SqlCommand(str_getId);
        cmd_getId.Parameters.AddWithValue("@Year", ddl_year.SelectedValue);
        cmd_getId.Parameters.AddWithValue("@monthvalue", ddl_month.SelectedValue);
        DataTable dt_id = DA.GetDataTable(cmd_getId);
        
        if (dt_id.Rows.Count > 0)
        {
            string newId = dt_id.Rows[0]["Employeeworkingdaydetailskey"].ToString();
            SaveHolidayDetails(newId);
        }

        ClientScript.RegisterStartupScript(this.GetType(),"Infologia","<script>alert('Submited Successfully')</script>");
        return;

    }
    private void SaveHolidayDetails(string employeeworkingdaydetailskey)
    {
        string[] dates = Request.Form.GetValues("holiday_date");
        string[] days = Request.Form.GetValues("holiday_day");
        string[] descriptions = Request.Form.GetValues("holiday_desc");

        if (dates != null && dates.Length > 0)
        {
            for (int i = 0; i < dates.Length; i++)
            {
                if (!string.IsNullOrEmpty(dates[i]))
                {
                    // Get day ID from day name
                    string str_getDayId = "SELECT DN_ID FROM IT_DaysName WHERE DN_Name=@DN_Name";
                    SqlCommand cmd_getDayId = new SqlCommand(str_getDayId);
                    cmd_getDayId.Parameters.AddWithValue("@DN_Name", days[i]);
                    DataTable dt_dayId = DA.GetDataTable(cmd_getDayId);
                    
                    int dayId = 0;
                    if (dt_dayId.Rows.Count > 0)
                    {
                        dayId = Convert.ToInt32(dt_dayId.Rows[0]["DN_ID"]);
                    }

                    string str_insert = "INSERT INTO IT_EmployeeWorkingDaysubtable(Employeeworkingdaydetailskey, date, day, description, CreatedBy) VALUES(@Employeeworkingdaydetailskey, @date, @day, @description, @CreatedBy)";
                    SqlCommand cmd = new SqlCommand(str_insert);
                    cmd.Parameters.AddWithValue("@Employeeworkingdaydetailskey", employeeworkingdaydetailskey);
                    cmd.Parameters.AddWithValue("@date", DateTime.ParseExact(dates[i], "dd/MM/yyyy", null));
                    cmd.Parameters.AddWithValue("@day", dayId);
                    cmd.Parameters.AddWithValue("@description", descriptions[i] ?? "");
                    cmd.Parameters.AddWithValue("@CreatedBy", str_userid);
                    DA.ExecuteNonQuery(cmd);
                }
            }
        }
    }
    protected void btn_update_Click(object sender, EventArgs e)
    {

        int day1 = int.Parse(txt_days.Text);
        int workingday1 = int.Parse(txt_work.Text);
        int leavedays1 = day1 - workingday1;
        this.str_userid = SC.Userid;
        //string str_modify = DateTime.UtcNow.ToString();
        string str_sql = "update IT_EmployeeWorkingDayDetails set modifiedby=@modifiedby,modifiedon=@modifiedon,year=@year,monthvalue=@monthvalue,numberofdaysinmonth=@numberofdaysinmonth,numberofworkdaysinmonth=@numberofworkdaysinmonth,numberofleavedaysinmonth=@numberofleavedaysinmonth where EmployeeWorkingDayDetailskey=@EmployeeWorkingDayDetailskey";
        SqlCommand cmd = new SqlCommand(str_sql);
        cmd.Parameters.AddWithValue("@EmployeeWorkingDayDetailskey", this.str_id);
        cmd.Parameters.AddWithValue("@year", ddl_year.SelectedValue);
        cmd.Parameters.AddWithValue("@monthvalue", ddl_month.SelectedValue);
        cmd.Parameters.AddWithValue("@numberofdaysinmonth", day1);
        cmd.Parameters.AddWithValue("@numberofworkdaysinmonth", workingday1);
        cmd.Parameters.AddWithValue("@Numberofleavedaysinmonth", leavedays1);
        cmd.Parameters.AddWithValue("@modifiedby", this.str_userid);
        cmd.Parameters.Add("@modifiedon", SqlDbType.DateTime).Value = DateTime.UtcNow;
        DA.ExecuteNonQuery(cmd);

        string str_delete = "DELETE FROM IT_EmployeeWorkingDaysubtable WHERE Employeeworkingdaydetailskey=@Employeeworkingdaydetailskey";
        SqlCommand cmd_delete = new SqlCommand(str_delete);
        cmd_delete.Parameters.AddWithValue("@Employeeworkingdaydetailskey", this.str_id);
        DA.ExecuteNonQuery(cmd_delete);

        SaveHolidayDetails(this.str_id);
        ClientScript.RegisterStartupScript(this.GetType(), "Infologia", "<script>alert('Updated Successfully')</script>");
        
    }
}