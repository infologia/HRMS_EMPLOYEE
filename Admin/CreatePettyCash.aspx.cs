using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
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

        if (Page.Form != null) 
        {
            Page.Form.Enctype = "multipart/form-data";
            Page.Form.Method = "post";
        }

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
        else
        {
            // PostBack — restore button visibility based on hfProjectKey
            if (!string.IsNullOrEmpty(hfProjectKey.Value))
            {
                btnSave.Visible = false;
                btnUpdate.Visible = true;
            }
        }
    }
    private void PopulateCashData(int projectKey)
    {



        string query = @"
        SELECT 
            PC_Description,
            PC_Amount,
            PC_Status,
            CONVERT(varchar(10), PC_Date, 103) AS PC_Date,
            PC_FilePath
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
            if (dt.Rows[0]["PC_Date"] != DBNull.Value)
            {
                DateTime pcDate = Convert.ToDateTime(dt.Rows[0]["PC_Date"]);
                txt_date.Text = pcDate.ToString("dd/MM/yyyy");
            }
            if (dt.Columns.Contains("PC_FilePath") && dt.Rows[0]["PC_FilePath"] != DBNull.Value && !string.IsNullOrEmpty(dt.Rows[0]["PC_FilePath"].ToString()))
            {
                string filePath = dt.Rows[0]["PC_FilePath"].ToString();
                hfExistingFile.Value = filePath;
                string resolvedUrl = ResolveUrl(filePath);
                lblExistingFile.Text = string.Format("<a href='{0}' target='_blank' class='text-primary' style='font-size:12px; display:inline-block; margin-top:5px;'><i class='icon-eye'></i> View existing attachment</a>", resolvedUrl);
                lblExistingFile.Visible = true;
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        DateTime date;
        if (!TryParseEntryDate(txt_date.Text, out date))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "dateerror",
                "showToastr('error','Invalid Entry Date. Please use DD/MM/YYYY format.');", true);
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
        {
            if (enteredAmount > lastBalance)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "balanceerror",
                    "showToastr('error','Insufficient petty cash balance. Please add cash before recording this expense.');", true);
                return;
            }
            newBalance = lastBalance - enteredAmount;
        }

        string filePath = SaveUploadedFile();

        string insertQuery = @"
        INSERT INTO TT_PettyCash
        (
            PC_Description,
            PC_Amount,
            PC_BalanceAmount,
            PC_Status,
            CreatedOn,
            CreatedBy,
            PC_Date,
            PC_FilePath
        )
        VALUES
        (
            @Description,
            @Amount,
            @BalanceAmount,
            @Status,
            GETDATE(),
            @CreatedBy,
            @PC_Date,
            @FilePath
        )";

        SqlCommand cmd = new SqlCommand(insertQuery);
        cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
        cmd.Parameters.AddWithValue("@Amount", enteredAmount);
        cmd.Parameters.AddWithValue("@BalanceAmount", newBalance);
        cmd.Parameters.AddWithValue("@Status", status);
        cmd.Parameters.AddWithValue("@CreatedBy", str_userid);
        cmd.Parameters.AddWithValue("@PC_Date", date);
        cmd.Parameters.AddWithValue("@FilePath", string.IsNullOrEmpty(filePath) ? (object)DBNull.Value : filePath);
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
        if (!TryParseEntryDate(txt_date.Text, out date))
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "dateerror",
                "showToastr('error','Invalid Entry Date. Please use DD/MM/YYYY format.');", true);
            return;
        }
        int cashKey = Convert.ToInt32(hfProjectKey.Value);
        decimal enteredAmount = Convert.ToDecimal(txtAmount.Text);
        int status = Convert.ToInt32(ddlStatus.SelectedValue); // 1=CR, 2=DT
        decimal previousBalance = 0;

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
        {
            if (enteredAmount > previousBalance)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "balanceerror",
                    "showToastr('error','Insufficient petty cash balance. Please add cash before recording this expense.');", true);
                return;
            }
            currentBalance = previousBalance - enteredAmount;
        }

        string newFilePath = SaveUploadedFile();
        if (string.IsNullOrEmpty(newFilePath))
            newFilePath = hfExistingFile.Value; // keep existing if no new file uploaded

        // 🔹 3. Update current record
        string updateQuery = @"
        UPDATE TT_PettyCash
        SET 
            PC_Description = @Description,
            PC_Amount = @Amount,
            PC_BalanceAmount = @Balance,
            PC_Status = @Status,
            ModifiedOn = GETDATE(),
            ModifiedBy = @ModifiedBy,
            PC_Date = @PC_Date,
            PC_FilePath = @FilePath
        WHERE PC_CashKey = @PC_CashKey";

        SqlCommand cmd = new SqlCommand(updateQuery);
        cmd.Parameters.AddWithValue("@Description", txtDescription.Text.Trim());
        cmd.Parameters.AddWithValue("@Amount", enteredAmount);
        cmd.Parameters.AddWithValue("@Balance", currentBalance);
        cmd.Parameters.AddWithValue("@Status", status);
        cmd.Parameters.AddWithValue("@ModifiedBy", SC.Userid);
        cmd.Parameters.AddWithValue("@PC_CashKey", cashKey);
        cmd.Parameters.AddWithValue("@PC_Date", date);
        cmd.Parameters.AddWithValue("@FilePath", string.IsNullOrEmpty(newFilePath) ? (object)DBNull.Value : newFilePath);

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

    private bool TryParseEntryDate(string input, out DateTime date)
    {
        date = DateTime.MinValue;
        if (string.IsNullOrWhiteSpace(input)) return false;

        string normalized = input.Trim().Replace("-", "/").Replace(".", "/");

        string[] formats = { "dd/MM/yyyy", "d/M/yyyy", "dd/M/yyyy", "d/MM/yyyy" };

        return DateTime.TryParseExact(normalized, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
    }

    private string SaveUploadedFile()
    {
        HttpPostedFile file = Request.Files["fuAttachment"];
        if (file == null || file.ContentLength == 0) return "";

        string saveDir = Server.MapPath("~/Uploads/PettyCash/");
        if (!Directory.Exists(saveDir)) Directory.CreateDirectory(saveDir);

        string uniqueName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);
        file.SaveAs(Path.Combine(saveDir, uniqueName));
        return "~/Uploads/PettyCash/" + uniqueName;
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