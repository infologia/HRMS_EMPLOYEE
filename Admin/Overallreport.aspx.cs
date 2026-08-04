using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Overallreport : System.Web.UI.Page
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
                control1.Text = "Overall Report Details";

            ddl_Month.SelectedValue = DateTime.Now.Month.ToString();
            ddl_Year.SelectedValue = DateTime.Now.Year.ToString();
            LoadOverallAssetsGrid();
        }
    }
    private void LoadOverallAssetsGrid()
    {
        string str_query = @"SELECT * FROM IT_V_OverallAssetsReports ORDER BY AssetKey DESC";
        SqlCommand cmd = new SqlCommand(str_query);
        DataTable dt_assets = DA.GetDataTable(cmd);

        if (dt_assets.Rows.Count > 0)
        {
            DataTable filteredDt = dt_assets.Clone();
            string selectedMonth = ddl_Month.SelectedValue;
            string selectedYear = ddl_Year.SelectedValue;
            string selectedStatus = ddl_Status.SelectedValue;

            foreach (DataRow row in dt_assets.Rows)
            {
                string dateStr = row["AssignedDate"] != DBNull.Value ? row["AssignedDate"].ToString().Trim() : "";
                if (string.IsNullOrEmpty(dateStr))
                {
                    dateStr = row["PurchaseDate"] != DBNull.Value ? row["PurchaseDate"].ToString().Trim() : "";
                }

                bool match = true;
                if (!string.IsNullOrEmpty(dateStr))
                {
                    DateTime parsedDate;
                    string[] formats = { "dd-MM-yyyy", "dd/MM/yyyy", "yyyy-MM-dd", "MM/dd/yyyy", "dd-MM-yyyy HH:mm:ss", "dd/MM/yyyy HH:mm:ss" };
                    
                    // Split by space to get only the date portion
                    string dateOnly = dateStr.Split(' ')[0];
                    
                    if (DateTime.TryParseExact(dateOnly, formats, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out parsedDate))
                    {
                        if (!string.IsNullOrEmpty(selectedMonth) && parsedDate.Month.ToString() != selectedMonth)
                        {
                            match = false;
                        }
                        if (!string.IsNullOrEmpty(selectedYear) && parsedDate.Year.ToString() != selectedYear)
                        {
                            match = false;
                        }
                    }
                    else
                    {
                        // Fallback parsing
                        if (DateTime.TryParse(dateStr, out parsedDate))
                        {
                            if (!string.IsNullOrEmpty(selectedMonth) && parsedDate.Month.ToString() != selectedMonth)
                            {
                                match = false;
                            }
                            if (!string.IsNullOrEmpty(selectedYear) && parsedDate.Year.ToString() != selectedYear)
                            {
                                match = false;
                            }
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(selectedMonth) || !string.IsNullOrEmpty(selectedYear))
                    {
                        match = false;
                    }
                }

                // Status Filter Check
                if (match && !string.IsNullOrEmpty(selectedStatus))
                {
                    int assignedStatus = Convert.ToInt32(row["AssignedStatus"]);
                    if (selectedStatus == "1" && assignedStatus != 1)
                    {
                        match = false;
                    }
                    else if (selectedStatus == "2" && assignedStatus == 1)
                    {
                        match = false;
                    }
                }

                if (match)
                {
                    filteredDt.ImportRow(row);
                }
            }

            dt_assets = filteredDt;
        }

        DataSet ds = new DataSet();
        ds.Merge(dt_assets);

        PH_OverallAssets.Controls.Clear();
        if (dt_assets.Rows.Count > 0)
        {
            if (!ds.Tables[0].Columns.Contains("AssignmentText"))
                ds.Tables[0].Columns.Add("AssignmentText");

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                int assignedStatus = Convert.ToInt32(dr["AssignedStatus"]);

                if (assignedStatus == 1)
                    dr["AssignmentText"] = "<span class='label label-success'>Assigned</span>";
                else
                    dr["AssignmentText"] = "<span class='label label-warning'>Unassigned</span>";
            }

            PH.LoadGridItem(ds, PH_OverallAssets, "OverallAssets.txt", "");
        }
    }

    protected void ddl_Filter_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.LoadOverallAssetsGrid();
    }
}