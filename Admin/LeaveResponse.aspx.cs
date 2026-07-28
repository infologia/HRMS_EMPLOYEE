using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.Services;

public partial class WEB_Admin_LeaveResponse : System.Web.UI.Page
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
            BindDateDropdown();
            PopulateYearDropdown();
            BindEmployeeDropdown();

            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Employee Monitoring";

            HtmlAnchor control = this.Master.FindControl("li_EmloyeeMonitoring") as HtmlAnchor;
            if (control != null)
                control.Attributes.Add("class", "active");

            Load_WorkedHoursData();
        }
    }

    private void BindDateDropdown()
    {
        ddlDate.Items.Clear();
        ddlDate.Items.Add(new ListItem("All", "0"));
        ddlDate.Items.Add(new ListItem("Today", "1"));

        for (int m = 1; m <= 12; m++)
        {
            string monthName = new DateTime(2025, m, 1).ToString("MMMM");
            ddlDate.Items.Add(new ListItem(monthName, (m + 1).ToString()));
        }

        
        int currentMonthValue = DateTime.Now.Month + 1;
        ddlDate.SelectedValue = currentMonthValue.ToString();
    }
    private void PopulateYearDropdown()
    {
        ddlYear.Items.Clear();
        int currentYear = DateTime.Now.Year;

        for (int year = currentYear - 5; year <= currentYear + 5; year++)
        {
            ddlYear.Items.Add(new ListItem(year.ToString(), year.ToString()));
        }

        ListItem defaultYearItem = ddlYear.Items.FindByValue(currentYear.ToString());
        if (defaultYearItem != null)
            defaultYearItem.Selected = true;
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        Load_WorkedHoursData();

    }


    protected void ddlDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        Load_WorkedHoursData();
    }

    protected void ddlEmployee_SelectedIndexChanged(object sender, EventArgs e)
    {
        Load_WorkedHoursData();
    }

    private void BindEmployeeDropdown()
    {
        string query = @"SELECT Employeekey, Firstname + ' ' + Lastname AS Username
            FROM IT_EmployeeRegister
            WHERE Employeestatus = 1
            ORDER BY Username";

        SqlCommand cmd = new SqlCommand(query);
        DataTable dt = DA.GetDataTable(cmd);

        ddlEmployee.DataSource = dt;
        ddlEmployee.DataTextField = "Username";
        ddlEmployee.DataValueField = "Employeekey";
        ddlEmployee.DataBind();
        ddlEmployee.Items.Insert(0, new ListItem("All Employees", "0"));
    }

    private void Load_WorkedHoursData()
    {
        string query = @"SELECT b.Employeeid,b.Firstname + ' ' + b.lastname AS username,CONVERT(VARCHAR(10), a.fromdate, 103) AS fromdate,CONVERT(VARCHAR(10), a.Todate, 103) AS Todate,a.responsestatus, a.reason,a.employeekey, a.Leavedays, a.Employeeleavedetailskey, a.responsereason,CONVERT(VARCHAR(10), a.CreatedOn, 103) AS CreatedDate FROM IT_EmployeeLeaveDetails a LEFT JOIN IT_EmployeeRegister b ON a.Employeekey = b.Employeekey  WHERE 1 = 1 ";
        
        SqlCommand cmd = new SqlCommand();

        int selectedYear = int.Parse(ddlYear.SelectedValue);
        query += " AND YEAR(a.CreatedOn) = @YearValue";
        cmd.Parameters.AddWithValue("@YearValue", selectedYear);


        string selected = ddlDate.SelectedValue;

        
        if (selected == "1")
        {
            query += " AND CAST(a.CreatedOn AS DATE) = CAST(GETDATE() AS DATE)";
        }
        
        else if (int.Parse(selected) >= 2)
        {
            int month = int.Parse(selected) - 1;
            query += " AND MONTH(a.CreatedOn) = @MonthValue";
            cmd.Parameters.AddWithValue("@MonthValue", month);
        }

        if (ddlEmployee.SelectedValue != "0" && !string.IsNullOrEmpty(ddlEmployee.SelectedValue))
        {
            query += " AND a.Employeekey = @EmployeeKey";
            cmd.Parameters.AddWithValue("@EmployeeKey", ddlEmployee.SelectedValue);
        }

        query += " ORDER BY a.CreatedOn DESC";

        cmd.CommandText = query;

        DataTable dt = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt);

     
        if (!ds.Tables[0].Columns.Contains("ActiveText"))
            ds.Tables[0].Columns.Add("ActiveText");

        int total = ds.Tables[0].Rows.Count;
        int pendingCount = 0, approvedCount = 0, rejectedCount = 0;

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            string str_reason = dr["responsereason"].ToString();
            int activetype = Convert.ToInt16(dr["responsestatus"]);

            if (activetype == 1)
            {
                pendingCount++;
                dr["ActiveText"] = "<span class='pr-pill pending' title='" + Server.HtmlEncode(str_reason) + "'><i class='icon-history'></i>Pending</span>";
            }
            else if (activetype == 2)
            {
                approvedCount++;
                dr["ActiveText"] = "<span class='pr-pill approved' title='" + Server.HtmlEncode(str_reason) + "'><i class='icon-checkmark-circle'></i>Approved</span>";
            }
            else if (activetype == 3)
            {
                rejectedCount++;
                dr["ActiveText"] = "<span class='pr-pill rejected' title='" + Server.HtmlEncode(str_reason) + "'><i class='icon-cancel-circle2'></i>Rejected</span>";
            }
        }

        try {
            Label lbl_total = (Label)this.Master.FindControl("ContentPlaceHolder1").FindControl("lbl_total");
            if (lbl_total != null) lbl_total.Text = total.ToString();

            Label lbl_pending = (Label)this.Master.FindControl("ContentPlaceHolder1").FindControl("lbl_pending");
            if (lbl_pending != null) lbl_pending.Text = pendingCount.ToString();

            Label lbl_approved = (Label)this.Master.FindControl("ContentPlaceHolder1").FindControl("lbl_approved");
            if (lbl_approved != null) lbl_approved.Text = approvedCount.ToString();

            Label lbl_rejected = (Label)this.Master.FindControl("ContentPlaceHolder1").FindControl("lbl_rejected");
            if (lbl_rejected != null) lbl_rejected.Text = rejectedCount.ToString();
        } catch { }

        if (ds.Tables[0].Rows.Count > 0)
        {
            PH.LoadGridItem(ds, PH_Leave, "Leaveresponse.txt", "");

            DataView dvPending = new DataView(ds.Tables[0]);
            dvPending.RowFilter = "Responsestatus = '1'";
            DataSet dsPending = new DataSet();
            dsPending.Tables.Add(dvPending.ToTable());
            PH.LoadGridItem(dsPending, PH_Pending, "Leaveresponse.txt", "");

            DataView dvApproved = new DataView(ds.Tables[0]);
            dvApproved.RowFilter = "Responsestatus = '2'";
            DataSet dsApproved = new DataSet();
            dsApproved.Tables.Add(dvApproved.ToTable());
            PH.LoadGridItem(dsApproved, PH_Approved, "Leaveresponse.txt", "");

            DataView dvRejected = new DataView(ds.Tables[0]);
            dvRejected.RowFilter = "Responsestatus = '3'";
            DataSet dsRejected = new DataSet();
            dsRejected.Tables.Add(dvRejected.ToTable());
            PH.LoadGridItem(dsRejected, PH_Rejected, "Leaveresponse.txt", "");
        }
        else
        {
            PH.LoadGridItem(ds, PH_Leave, "Leaveresponse.txt", "");
            PH.LoadGridItem(ds, PH_Pending, "Leaveresponse.txt", "");
            PH.LoadGridItem(ds, PH_Approved, "Leaveresponse.txt", "");
            PH.LoadGridItem(ds, PH_Rejected, "Leaveresponse.txt", "");
        }
    }
}
