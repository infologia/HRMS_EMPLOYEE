using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_CreatePettyCash : System.Web.UI.Page
{
    DataAccess DA;
    SessionCustom SC;
    string str_userid;
    protected void Page_Load(object sender, EventArgs e)
    {
        this.DA = new DataAccess();
        this.SC = new SessionCustom();
        str_userid = this.SC.Userid;

        if (!IsPostBack)
        {
            Label control1 = this.Master.FindControl("lbl_bread") as Label;
            if (control1 != null)
                control1.Text = "Petty Cash";

            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
            {
                int CashKey = Convert.ToInt32(Request.QueryString["id"]);
                hfProjectKey.Value = CashKey.ToString();

                PopulateCashData(CashKey);

                btnSave.Visible = false;
                btnUpdate.Visible = true;
            }
            else
            {
                btnSave.Visible = true;
                btnUpdate.Visible = false;
            }
        }
    }
    private void PopulateCashData(int projectKey)
    {



        string query = @"
        SELECT 
            PC_Description,
            PC_Amount,
            PC_Status,PC_Date
        FROM TT_PettyCash
        WHERE PC_CashKey = @PC_CashKey";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@PC_CashKey", projectKey);

        DataTable dt = DA.GetDataTable(cmd);

        if (dt.Rows.Count > 0)
        {
            txtDescription.Text = dt.Rows[0]["PC_Description"].ToString();
            txtAmount.Text = dt.Rows[0]["PC_Amount"].ToString();
            ddlStatus.SelectedValue = dt.Rows[0]["PC_Status"].ToString();
            ddlStatus.SelectedValue = dt.Rows[0]["PC_Status"].ToString();
            if (dt.Rows[0]["PC_Date"] != DBNull.Value)
            {
                DateTime pcDate = Convert.ToDateTime(dt.Rows[0]["PC_Date"]);
                txt_date.Text = pcDate.ToString("dd/MM/yyyy");
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        DateTime date;

        if (!DateTime.TryParseExact(
                txt_date.Text,
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out date))
        {
            // invalid date handle
            return;
        }



        decimal enteredAmount = Convert.ToDecimal(txtAmount.Text);
        int status = Convert.ToInt32(ddlStatus.SelectedValue); // 1=CR, 2=DT
        decimal lastBalance = 0;

        // 🔹 Get Last Balance
        string balQuery = @"
        SELECT TOP 1 PC_BalanceAmount 
        FROM TT_PettyCash 
        ORDER BY CreatedOn DESC";

        DataTable dtBal = DA.GetDataTable(new SqlCommand(balQuery));

        if (dtBal.Rows.Count > 0)
            lastBalance = Convert.ToDecimal(dtBal.Rows[0]["PC_BalanceAmount"]);

        // 🔹 Calculate New Balance
        decimal newBalance = 0;

        if (status == 1) // Credit
            newBalance = lastBalance + enteredAmount;
        else if (status == 2) // Debit
            newBalance = lastBalance - enteredAmount;

        // 🔹 Insert Query
        string insertQuery = @"
        INSERT INTO TT_PettyCash
        (
            PC_Description,
            PC_Amount,
            PC_BalanceAmount,
            PC_Status,
            CreatedOn,
            CreatedBy,
            PC_Date
        )
        VALUES
        (
            @Description,
            @Amount,
            @BalanceAmount,
            @Status,
            GETDATE(),
            @CreatedBy,@PC_Date
        )";

        SqlCommand cmd = new SqlCommand(insertQuery);
        cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
        cmd.Parameters.AddWithValue("@Amount", enteredAmount);
        cmd.Parameters.AddWithValue("@BalanceAmount", newBalance);
        cmd.Parameters.AddWithValue("@Status", status);
        cmd.Parameters.AddWithValue("@CreatedBy", str_userid);
        cmd.Parameters.AddWithValue("@PC_Date", date);
        DA.ExecuteNonQuery(cmd);

        ScriptManager.RegisterStartupScript(
            this,
            this.GetType(),
            "toastr_redirect",
            "showToastr('success','Cash entry saved successfully!');" +
            "setTimeout(function(){ window.location.href = '/Admin/PettyCash.aspx'; }, 2000);",
            true
        );
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        DateTime date;

        if (!DateTime.TryParseExact(
                txt_date.Text,
                "dd/MM/yyyy",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out date))
        {
            // invalid date handle
            return;
        }
        int cashKey = Convert.ToInt32(hfProjectKey.Value);
        decimal enteredAmount = Convert.ToDecimal(txtAmount.Text);
        int status = Convert.ToInt32(ddlStatus.SelectedValue); // 1=CR, 2=DT
        decimal previousBalance = 0;

        // 🔹 1. Get Previous Balance (last record before this one)
        string prevBalQuery = @"
        SELECT TOP 1 PC_BalanceAmount
        FROM TT_PettyCash
        WHERE PC_CashKey < @PC_CashKey
        ORDER BY PC_CashKey DESC";

        SqlCommand prevCmd = new SqlCommand(prevBalQuery);
        prevCmd.Parameters.AddWithValue("@PC_CashKey", cashKey);

        DataTable dtPrev = DA.GetDataTable(prevCmd);

        if (dtPrev.Rows.Count > 0)
            previousBalance = Convert.ToDecimal(dtPrev.Rows[0]["PC_BalanceAmount"]);

        // 🔹 2. Calculate new balance for current row
        decimal currentBalance = 0;

        if (status == 1) // Credit
            currentBalance = previousBalance + enteredAmount;
        else // Debit
            currentBalance = previousBalance - enteredAmount;

        // 🔹 3. Update current record
        string updateQuery = @"
        UPDATE TT_PettyCash
        SET 
            PC_Description = @Description,
            PC_Amount = @Amount,
            PC_BalanceAmount = @Balance,
            PC_Status = @Status,
            ModifiedOn = GETDATE(),
            ModifiedBy = @ModifiedBy,PC_Date=@PC_Date
        WHERE PC_CashKey = @PC_CashKey";

        SqlCommand cmd = new SqlCommand(updateQuery);
        cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
        cmd.Parameters.AddWithValue("@Amount", enteredAmount);
        cmd.Parameters.AddWithValue("@Balance", currentBalance);
        cmd.Parameters.AddWithValue("@Status", status);
        cmd.Parameters.AddWithValue("@ModifiedBy", SC.Userid);
        cmd.Parameters.AddWithValue("@PC_CashKey", cashKey);
        cmd.Parameters.AddWithValue("@PC_Date", date);

        DA.ExecuteNonQuery(cmd);

        // 🔹 4. Recalculate all NEXT balances
        RecalculateNextBalances(cashKey, currentBalance);

        ScriptManager.RegisterStartupScript(
            this,
            this.GetType(),
            "toastr_redirect",
            "showToastr('success','Cash Updated Successfully!');" +
            "setTimeout(function(){ window.location.href = '/Admin/PettyCash.aspx'; }, 2000);",
            true
        );
    }

    private void RecalculateNextBalances(int fromCashKey, decimal startingBalance)
    {
        string query = @"
        SELECT PC_CashKey, PC_Amount, PC_Status
        FROM TT_PettyCash
        WHERE PC_CashKey > @PC_CashKey
        ORDER BY PC_CashKey ASC";

        SqlCommand cmd = new SqlCommand(query);
        cmd.Parameters.AddWithValue("@PC_CashKey", fromCashKey);

        DataTable dt = DA.GetDataTable(cmd);

        decimal runningBalance = startingBalance;

        foreach (DataRow dr in dt.Rows)
        {
            int status = Convert.ToInt32(dr["PC_Status"]);
            decimal amount = Convert.ToDecimal(dr["PC_Amount"]);

            if (status == 1) // Credit
                runningBalance += amount;
            else // Debit
                runningBalance -= amount;

            SqlCommand upd = new SqlCommand(@"
            UPDATE TT_PettyCash
            SET PC_BalanceAmount = @Balance
            WHERE PC_CashKey = @PC_CashKey");

            upd.Parameters.AddWithValue("@Balance", runningBalance);
            upd.Parameters.AddWithValue("@PC_CashKey", dr["PC_CashKey"]);

            DA.ExecuteNonQuery(upd);
        }
    }

}