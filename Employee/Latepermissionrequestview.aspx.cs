using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Employee_Latepermissionrequestview : System.Web.UI.Page
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
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Late Permissions";

            // Populate Year DropDown
            int currentYear = DateTime.Now.Year;
            ddl_year.Items.Add(new ListItem("All Years", "0"));
            for (int y = currentYear; y >= 2020; y--)
            {
                ddl_year.Items.Add(new ListItem(y.ToString(), y.ToString()));
            }

            // Default to current year & current month
            ddl_year.SelectedValue = currentYear.ToString();
            ddl_month.SelectedValue = DateTime.Now.Month.ToString();
        }

        BindLateGrids();
    }

    private void BindLateGrids()
    {
        string str_query = @"
            SELECT 
                b.firstname + ' ' + b.lastname AS name,
                CONVERT(varchar, a.Requestdate, 103) AS request,
                a.Fromtime,
                a.Totime,
                a.responsereason,
                a.LatePermissionDetailskey,
                a.Permissionhourse,
                a.responsestatus
            FROM IT_LatePermissionDetails a
            LEFT OUTER JOIN IT_EmployeeRegister b 
                ON a.createdby = b.Employeekey
            WHERE a.createdby = @createdby";

        if (ddl_year.SelectedValue != "0")
        {
            str_query += " AND YEAR(a.Requestdate) = @year";
        }
        if (ddl_month.SelectedValue != "0")
        {
            str_query += " AND MONTH(a.Requestdate) = @month";
        }

        str_query += " ORDER BY a.createdon DESC";

        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@createdby", SC.Userid);
        if (ddl_year.SelectedValue != "0")
        {
            cmd.Parameters.AddWithValue("@year", Convert.ToInt32(ddl_year.SelectedValue));
        }
        if (ddl_month.SelectedValue != "0")
        {
            cmd.Parameters.AddWithValue("@month", Convert.ToInt32(ddl_month.SelectedValue));
        }

        DataTable dt_dashboard = DA.GetDataTable(cmd);

        // ---- Summary counts ----
        int total = dt_dashboard.Rows.Count;
        int pendingCount = 0, approvedCount = 0, rejectedCount = 0;
        foreach (DataRow row in dt_dashboard.Rows)
        {
            string statusValue = row["responsestatus"] != DBNull.Value ? row["responsestatus"].ToString().Trim() : "";
            int activetype = 0;
            bool isNumeric = int.TryParse(statusValue, out activetype);
            if (!isNumeric)
            {
                string statusLower = statusValue.ToLower();
                if (statusLower == "pending") activetype = 1;
                else if (statusLower == "approved") activetype = 2;
                else if (statusLower == "rejected") activetype = 3;
            }

            if (activetype == 1) pendingCount++;
            else if (activetype == 2) approvedCount++;
            else if (activetype == 3) rejectedCount++;
        }

        lbl_total.Text = total.ToString();
        lbl_pending.Text = pendingCount.ToString();
        lbl_approved.Text = approvedCount.ToString();
        lbl_rejected.Text = rejectedCount.ToString();

        DataSet ds = new DataSet();
        ds.Merge(dt_dashboard);

        if (dt_dashboard.Rows.Count > 0)
        {
            if (!ds.Tables[0].Columns.Contains("ActiveText"))
                ds.Tables[0].Columns.Add("ActiveText");

            if (!ds.Tables[0].Columns.Contains("ViewText"))
                ds.Tables[0].Columns.Add("ViewText");

            if (!ds.Tables[0].Columns.Contains("HideViewBtn"))
                ds.Tables[0].Columns.Add("HideViewBtn");

            if (!ds.Tables[0].Columns.Contains("StatusType"))
                ds.Tables[0].Columns.Add("StatusType", typeof(int));

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string str_reason = dr["responsereason"] != DBNull.Value ? dr["responsereason"].ToString() : "";
                string statusValue = dr["responsestatus"] != DBNull.Value ? dr["responsestatus"].ToString().Trim() : "";

                int activetype = 0; 
                bool isNumeric = int.TryParse(statusValue, out activetype);

                if (!isNumeric)
                {
                    string statusLower = statusValue.ToLower();
                    if (statusLower == "pending") activetype = 1;
                    else if (statusLower == "approved") activetype = 2;
                    else if (statusLower == "rejected") activetype = 3;
                }

                dr["StatusType"] = activetype;

                if (activetype == 1)
                {
                    dr["ActiveText"] = "<span class='pr-pill pending' title='" + Server.HtmlEncode(str_reason) + "'><i class='icon-history'></i>Pending</span>";
                    dr["ViewText"] = "";
                    dr["HideViewBtn"] = "hidden";
                }
                else if (activetype == 2)
                {
                    dr["ActiveText"] = "<span class='pr-pill approved' title='" + Server.HtmlEncode(str_reason) + "'><i class='icon-checkmark-circle'></i>Approved</span>";
                    dr["ViewText"] = "hidden";
                    dr["HideViewBtn"] = "";
                }
                else if (activetype == 3)
                {
                    dr["ActiveText"] = "<span class='pr-pill rejected' title='" + Server.HtmlEncode(str_reason) + "'><i class='icon-cancel-circle2'></i>Rejected</span>";
                    dr["ViewText"] = "hidden";
                    dr["HideViewBtn"] = "";
                }
                else
                {
                    dr["ActiveText"] = "<span class='pr-pill pending' title='" + Server.HtmlEncode(str_reason) + "'><i class='icon-history'></i>Unknown</span>";
                    dr["ViewText"] = "";
                    dr["HideViewBtn"] = "hidden";
                }
            }

            // Bind All
            this.PH.LoadGridItem(ds, PH_All, "Laterecordviewemp_new.txt", "");

            // Bind Pending
            DataView dvPending = new DataView(ds.Tables[0]);
            dvPending.RowFilter = "StatusType = 1";
            DataSet dsPending = new DataSet();
            dsPending.Tables.Add(dvPending.ToTable());
            this.PH.LoadGridItem(dsPending, PH_Pending, "Laterecordviewemp_new.txt", "");

            // Bind Approved
            DataView dvApproved = new DataView(ds.Tables[0]);
            dvApproved.RowFilter = "StatusType = 2";
            DataSet dsApproved = new DataSet();
            dsApproved.Tables.Add(dvApproved.ToTable());
            this.PH.LoadGridItem(dsApproved, PH_Approved, "Laterecordviewemp_new.txt", "");

            // Bind Rejected
            DataView dvRejected = new DataView(ds.Tables[0]);
            dvRejected.RowFilter = "StatusType = 3";
            DataSet dsRejected = new DataSet();
            dsRejected.Tables.Add(dvRejected.ToTable());
            this.PH.LoadGridItem(dsRejected, PH_Rejected, "Laterecordviewemp_new.txt", "");
        }
    }

    protected void ddl_filter_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Handled automatically on Page_Load on postback
    }

    [WebMethod]
    public static string DeleteProject(string str_employeepermissiondetailskey)
    {
        try
        {
            DataAccess DA1 = new DataAccess();

            string str_Sql = "DELETE FROM IT_LatePermissionDetails WHERE LatePermissionDetailskey = @LatePermissionDetailskey";
            SqlCommand cmd = new SqlCommand(str_Sql);
            cmd.Parameters.AddWithValue("@LatePermissionDetailskey", str_employeepermissiondetailskey);

            DA1.ExecuteNonQuery(cmd);

            return "1";
        }
        catch
        {
            return "0";
        }
    }
}
