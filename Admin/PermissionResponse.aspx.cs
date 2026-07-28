using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Services;

public partial class WEB_Admin_PermissionResponse : System.Web.UI.Page
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

            Load_WorkedHoursData();    
        }
    }
    private void BindDateDropdown()
    {
        ddlDate.Items.Clear();
        ddlDate.Items.Add(new ListItem("All", "0"));
        ddlDate.Items.Add(new ListItem("Today", "1"));
        int currentYear = DateTime.Now.Year;
        for (int m = 1; m <= 12; m++)
        {
            string monthName = new DateTime(currentYear, m, 1).ToString("MMMM");
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
        //int selectedYear = int.Parse(ddlYear.SelectedValue);
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
        string query = @"SELECT b.Employeeid,b.Firstname + ' ' + b.lastname AS username,CONVERT(VARCHAR(10), a.Requestdate, 103) AS Requestdate,a.Fromtime,a.ToTime,a.responsestatus,a.reason,a.employeekey,a.Permissionhourse,a.Employeepermissiondetailskey,a.responsereason,CONVERT(VARCHAR(10), a.CreatedOn, 103) AS CreatedDate FROM IT_EmployeePermissionDetails a LEFT JOIN IT_EmployeeRegister b ON a.createdby = b.Employeekey WHERE 1 = 1 ";

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
            int year = DateTime.Now.Year;
            query += " AND MONTH(a.CreatedOn) = @MonthValue AND YEAR(a.CreatedOn)=@Year";
            cmd.Parameters.AddWithValue("@MonthValue", month);
            cmd.Parameters.AddWithValue("@Year", year);
        }

        if (ddlEmployee.SelectedValue != "0" && !string.IsNullOrEmpty(ddlEmployee.SelectedValue))
        {
            query += " AND a.createdby = @EmployeeKey";
            cmd.Parameters.AddWithValue("@EmployeeKey", ddlEmployee.SelectedValue);
        }

        query += " ORDER BY a.CreatedOn DESC";
        cmd.CommandText = query;

        DataTable dt = DA.GetDataTable(cmd);
        DataSet ds = new DataSet();
        ds.Merge(dt);

        
        if (!ds.Tables[0].Columns.Contains("ActiveText"))
            ds.Tables[0].Columns.Add("ActiveText");
        
        if (!ds.Tables[0].Columns.Contains("ReasonDisplay"))
            ds.Tables[0].Columns.Add("ReasonDisplay");
        if (!ds.Tables[0].Columns.Contains("ReasonFull"))
            ds.Tables[0].Columns.Add("ReasonFull");

        int total = ds.Tables[0].Rows.Count;
        int pendingCount = 0, approvedCount = 0, rejectedCount = 0;

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            int status = Convert.ToInt32(dr["responsestatus"]);
            string reason = dr["responsereason"].ToString();

            if (status == 1)
            {
                pendingCount++;
                dr["ActiveText"] = "<span class='pr-pill pending' title='" + Server.HtmlEncode(reason) + "'><i class='icon-history'></i>Pending</span>";
            }
            else if (status == 2)
            {
                approvedCount++;
                dr["ActiveText"] = "<span class='pr-pill approved' title='" + Server.HtmlEncode(reason) + "'><i class='icon-checkmark-circle'></i>Approved</span>";
            }
            else if (status == 3)
            {
                rejectedCount++;
                dr["ActiveText"] = "<span class='pr-pill rejected' title='" + Server.HtmlEncode(reason) + "'><i class='icon-cancel-circle2'></i>Rejected</span>";
            }
            
            string fullReason = dr["reason"] == null ? "" : dr["reason"].ToString();
            string encodedFull = Server.HtmlEncode(fullReason);
            string displayReason = fullReason.Length > 40 ? fullReason.Substring(0, 40) + "..." : fullReason;
            dr["ReasonFull"] = encodedFull;
            dr["ReasonDisplay"] = Server.HtmlEncode(displayReason);
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
            this.PH.LoadGridItem(ds, PH_All, "Permissionresponse.txt", "");

            DataView dvPending = new DataView(ds.Tables[0]);
            dvPending.RowFilter = "Responsestatus = '1'";
            DataSet dsPending = new DataSet();
            dsPending.Tables.Add(dvPending.ToTable());
            this.PH.LoadGridItem(dsPending, PH_Pending, "Permissionresponse.txt", "");

            DataView dvApproved = new DataView(ds.Tables[0]);
            dvApproved.RowFilter = "Responsestatus = '2'";
            DataSet dsApproved = new DataSet();
            dsApproved.Tables.Add(dvApproved.ToTable());
            this.PH.LoadGridItem(dsApproved, PH_Approved, "Permissionresponse.txt", "");

            DataView dvRejected = new DataView(ds.Tables[0]);
            dvRejected.RowFilter = "Responsestatus = '3'";
            DataSet dsRejected = new DataSet();
            dsRejected.Tables.Add(dvRejected.ToTable());
            this.PH.LoadGridItem(dsRejected, PH_Rejected, "Permissionresponse.txt", "");
        }
        else
        {
            this.PH.LoadGridItem(ds, PH_All, "Permissionresponse.txt", "");
            this.PH.LoadGridItem(ds, PH_Pending, "Permissionresponse.txt", "");
            this.PH.LoadGridItem(ds, PH_Approved, "Permissionresponse.txt", "");
            this.PH.LoadGridItem(ds, PH_Rejected, "Permissionresponse.txt", "");
        }
    }



}
