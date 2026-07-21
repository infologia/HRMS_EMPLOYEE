using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_PettyCash : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    PhTemplate PH;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        this.PH = new PhTemplate();

        string str_userid = this.SC.Userid;

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Petty Cash";

            BindDateDropdown();
            PopulateYearDropdown();

            ddlDate.SelectedValue = "0"; // All
            ddlYear.SelectedValue = DateTime.Now.Year.ToString();

            LoadCashByMonthYear();
        }

        if (SC.UserRole == "0")
        {
            id_pettycash.Visible = true;
        }
    }

    private void LoadCash(int month, int year)
    {
        string str_query = @"
    SELECT 
        a.PC_CashKey,
        a.PC_Description,
        a.PC_Amount,
        a.PC_BalanceAmount,
        a.PC_Status,
a.PC_Date, 
ISNULL(b.Firstname + ' ' + b.Lastname, '') AS Username
    FROM TT_PettyCash a
    LEFT JOIN IT_EmployeeRegister b 
        ON a.CreatedBy = b.EmployeeKey
    WHERE
        (
            @Month = 0 AND YEAR(a.PC_Date) = @Year          -- All months
        )
        OR
        (
            @Month BETWEEN 1 AND 12
            AND MONTH(a.PC_Date) = @Month
            AND YEAR(a.PC_Date) = @Year                    -- Specific month
        )
        OR
        (
            @Month = -1
            AND CAST(a.PC_Date AS DATE) = CAST(GETDATE() AS DATE)  -- Today
        )
    ORDER BY a.PC_Date DESC";

        SqlCommand cmd = new SqlCommand(str_query);
        cmd.Parameters.AddWithValue("@Month", month);
        cmd.Parameters.AddWithValue("@Year", year);

        DataTable dt = DA.GetDataTable(cmd);

        if (dt.Rows.Count > 0)
        {

            if (!dt.Columns.Contains("ActiveText"))
                dt.Columns.Add("ActiveText");
            if (!dt.Columns.Contains("PC_DateText"))
                dt.Columns.Add("PC_DateText", typeof(string));

            foreach (DataRow dr in dt.Rows)
            {

                if (dr["PC_Date"] != DBNull.Value)
                {
                    DateTime pcDate = Convert.ToDateTime(dr["PC_Date"]);
                    dr["PC_DateText"] = pcDate.ToString("dd/MM/yyyy");
                }
                int status = Convert.ToInt32(dr["PC_Status"]);
                dr["ActiveText"] = status == 1
                    ? "<span class='label label-sm label-success'>CR</span>"
                    : "<span class='label label-sm label-danger'>DT</span>";
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            PH.LoadGridItem(ds, PH_PettyCash, "PettyCash.txt", "");
        }
    }

    private void LoadPettyCashTotals(int month, int year)
    {
        string query = @"
    SELECT
        ISNULL(SUM(CASE WHEN PC_Status = 1 THEN PC_Amount ELSE 0 END), 0) AS CRAmount,
        ISNULL(SUM(CASE WHEN PC_Status = 2 THEN PC_Amount ELSE 0 END), 0) AS DTAmount,
        ISNULL(
            (
                SELECT TOP 1 PC_BalanceAmount
                FROM TT_PettyCash
                WHERE
                    (
                        @Month = 0 AND YEAR(PC_Date) = @Year
                    )
                    OR
                    (
                        @Month BETWEEN 1 AND 12
                        AND MONTH(PC_Date) = @Month
                        AND YEAR(PC_Date) = @Year
                    )
                    OR
                    (
                        @Month = -1
                        AND CAST(PC_Date AS DATE) = CAST(GETDATE() AS DATE)
                    )
                ORDER BY PC_Date DESC, PC_CashKey DESC
            ), 0
        ) AS BalanceAmount
    FROM TT_PettyCash
    WHERE
        (
            @Month = 0 AND YEAR(PC_Date) = @Year
        )
        OR
        (
            @Month BETWEEN 1 AND 12
            AND MONTH(PC_Date) = @Month
            AND YEAR(PC_Date) = @Year
        )
        OR
        (
            @Month = -1
            AND CAST(PC_Date AS DATE) = CAST(GETDATE() AS DATE)
        )";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@Month", month);
        cmd.Parameters.AddWithValue("@Year", year);

        DataTable dt = DA.GetDataTable(cmd);

        if (dt.Rows.Count > 0)
        {
            lblCR.Text = Convert.ToDecimal(dt.Rows[0]["CRAmount"]).ToString("0.00");
            lblDT.Text = Convert.ToDecimal(dt.Rows[0]["DTAmount"]).ToString("0.00");
            lblBalance.Text = Convert.ToDecimal(dt.Rows[0]["BalanceAmount"]).ToString("0.00");
        }
    }




    [System.Web.Services.WebMethod]
    public static string DeleteCash(int CashKey)
    {
        try
        {
            DataAccess DA = new DataAccess();

            string query = "DELETE FROM TT_PettyCash WHERE PC_CashKey = @PC_CashKey";

            SqlCommand cmd = new SqlCommand(query);
            cmd.Parameters.AddWithValue("@PC_CashKey", CashKey);

            DA.ExecuteNonQuery(cmd);

            return "SUCCESS";
        }
        catch (Exception)
        {
            return "ERROR";
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

        // Page load default → All
        ddlDate.SelectedValue = "0";
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


    private void LoadCashByMonthYear()
    {
        int month = Convert.ToInt32(ddlDate.SelectedValue);
        int year = Convert.ToInt32(ddlYear.SelectedValue);

        LoadCash(month, year);
        LoadPettyCashTotals(month, year);
    }


    protected void ddlDate_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCashByMonthYear();
    }

    protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadCashByMonthYear();
    }

}