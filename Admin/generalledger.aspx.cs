using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_generalledger : System.Web.UI.Page
{
    DataAccess DA;

    protected void Page_Load(object sender, EventArgs e)
    {
        Response.ContentEncoding = System.Text.Encoding.UTF8;
        Response.Charset = "utf-8";

        this.DA = new DataAccess();

        if (!IsPostBack)
        {
            BindDropdowns();
            LoadKPICards();
            LoadLedger();
        }
    }

    // ── Dropdowns ──────────────────────────────────────────────────
    private void BindDropdowns()
    {
        // Financial Year
        ddl_FinancialYear.Items.Clear();
        ddl_FinancialYear.Items.Add(new ListItem("All FY", "0"));
        int currentYear  = DateTime.Now.Year;
        int currentMonth = DateTime.Now.Month;
        int fyStartYear  = currentMonth >= 4 ? currentYear : currentYear - 1;
        for (int y = fyStartYear; y >= 2020; y--)
        {
            string fyText  = "FY " + y + "-" + (y + 1).ToString().Substring(2, 2);
            string fyValue = y.ToString();
            ddl_FinancialYear.Items.Add(new ListItem(fyText, fyValue));
        }
        // Default: current FY
        ddl_FinancialYear.SelectedValue = fyStartYear.ToString();

        // Month
        ddl_Month.Items.Clear();
        ddl_Month.Items.Add(new ListItem("All Months", "0"));
        for (int m = 1; m <= 12; m++)
            ddl_Month.Items.Add(new ListItem(new DateTime(2025, m, 1).ToString("MMMM"), m.ToString()));
        ddl_Month.SelectedValue = "0"; // default All

        // Year
        ddl_Year.Items.Clear();
        ddl_Year.Items.Add(new ListItem("All Years", "0"));
        int yr = DateTime.Now.Year;
        for (int y = yr; y >= yr - 5; y--)
            ddl_Year.Items.Add(new ListItem(y.ToString(), y.ToString()));
        ddl_Year.SelectedValue = "0"; // default All
    }

    // ── Filter Helpers ─────────────────────────────────────────────
    private int GetMonth() { return int.Parse(ddl_Month.SelectedValue); }
    private int GetYear()  { return int.Parse(ddl_Year.SelectedValue); }
    private string GetSrc() { return ddl_Source.SelectedValue == "0" ? "" : ddl_Source.SelectedValue; }

    private void GetFYDates(out DateTime? fyStart, out DateTime? fyEnd)
    {
        string fyVal = ddl_FinancialYear.SelectedValue;
        if (fyVal == "0")
        {
            fyStart = null;
            fyEnd   = null;
        }
        else
        {
            int startYear = Convert.ToInt32(fyVal);
            fyStart = new DateTime(startYear, 4, 1);                      // Apr 1
            fyEnd   = new DateTime(startYear + 1, 3, 31, 23, 59, 59);    // Mar 31
        }
    }

    // ── Submit Button ──────────────────────────────────────────────
    protected void btn_Filter_Click(object sender, EventArgs e)
    {
        LoadKPICards();
        LoadLedger();
    }

    // ── Format Helpers ─────────────────────────────────────────────
    private string FormatAmount(object val)
    {
        if (val == null || val == DBNull.Value) return "0";
        decimal d = Convert.ToDecimal(val);
        return string.Format("{0:N0}", d);
    }

    private string SourcePill(string source)
    {
        switch (source)
        {
            case "Payroll":    return "<span class='src-pill pill-payslip'><i class='glyphicon glyphicon-file'></i> Payroll</span>";
            case "PettyCash":  return "<span class='src-pill pill-petty'><i class='glyphicon glyphicon-piggy-bank'></i> Petty Cash</span>";
            case "Payable":    return "<span class='src-pill pill-payable'><i class='glyphicon glyphicon-upload'></i> Invoice (Payable)</span>";
            case "Receivable": return "<span class='src-pill pill-receivable'><i class='glyphicon glyphicon-download-alt'></i> Invoice (Receivable)</span>";
            default:           return "<span class='src-pill pill-petty'>" + source + "</span>";
        }
    }

    private string CategoryBadge(string cat)
    {
        switch (cat)
        {
            case "Expense":   return "<span class='cat-badge cat-expense'>Expense</span>";
            case "Liability": return "<span class='cat-badge cat-liability'>Liability</span>";
            case "Asset":     return "<span class='cat-badge cat-asset'>Asset</span>";
            case "Income":    return "<span class='cat-badge cat-income'>Income</span>";
            default:          return "<span class='cat-badge cat-expense'>" + cat + "</span>";
        }
    }

    // ── KPI Cards ──────────────────────────────────────────────────
    private void LoadKPICards()
    {
        int month = GetMonth();    // 0 = All
        int year  = GetYear();     // 0 = All
        DateTime? fyStart, fyEnd;
        GetFYDates(out fyStart, out fyEnd);

        // 1. Pending Receivables
        SqlCommand cmdRecv = new SqlCommand(@"
            SELECT ISNULL(SUM(AR_BalanceAmount), 0) AS Total, COUNT(*) AS Cnt
            FROM IT_ARPaymentEntry
            WHERE AR_BalanceAmount > 0
              AND (@FYStart IS NULL OR AR_InvoiceDate >= @FYStart)
              AND (@FYEnd   IS NULL OR AR_InvoiceDate <= @FYEnd)
              AND (@Month   = 0     OR MONTH(AR_InvoiceDate) = @Month)
              AND (@Year    = 0     OR YEAR(AR_InvoiceDate)  = @Year)");
        cmdRecv.Parameters.AddWithValue("@FYStart", fyStart.HasValue ? (object)fyStart.Value : DBNull.Value);
        cmdRecv.Parameters.AddWithValue("@FYEnd",   fyEnd.HasValue   ? (object)fyEnd.Value   : DBNull.Value);
        cmdRecv.Parameters.AddWithValue("@Month",   month);
        cmdRecv.Parameters.AddWithValue("@Year",    year);
        DataTable dtRecv = DA.GetDataTable(cmdRecv);
        lbl_TotalReceivables.Text = FormatAmount(dtRecv.Rows[0]["Total"]);
        lbl_ReceivablesCount.Text = dtRecv.Rows[0]["Cnt"].ToString();

        // 2. Total Payables
        SqlCommand cmdPay = new SqlCommand(@"
            SELECT ISNULL(SUM(AP_BalanceAmount), 0) AS Total, COUNT(*) AS Cnt
            FROM IT_APPaymentEntry
            WHERE AP_BalanceAmount > 0
              AND (@FYStart IS NULL OR AP_InvoiceDate >= @FYStart)
              AND (@FYEnd   IS NULL OR AP_InvoiceDate <= @FYEnd)
              AND (@Month   = 0     OR MONTH(AP_InvoiceDate) = @Month)
              AND (@Year    = 0     OR YEAR(AP_InvoiceDate)  = @Year)");
        cmdPay.Parameters.AddWithValue("@FYStart", fyStart.HasValue ? (object)fyStart.Value : DBNull.Value);
        cmdPay.Parameters.AddWithValue("@FYEnd",   fyEnd.HasValue   ? (object)fyEnd.Value   : DBNull.Value);
        cmdPay.Parameters.AddWithValue("@Month",   month);
        cmdPay.Parameters.AddWithValue("@Year",    year);
        DataTable dtPay = DA.GetDataTable(cmdPay);
        lbl_TotalPayables.Text = FormatAmount(dtPay.Rows[0]["Total"]);
        lbl_PayablesCount.Text = dtPay.Rows[0]["Cnt"].ToString();

        // 3. Petty Cash Balance
        SqlCommand cmdCash = new SqlCommand(@"
            SELECT TOP 1 PC_BalanceAmount
            FROM TT_PettyCash
            WHERE (@FYStart IS NULL OR PC_Date >= @FYStart)
              AND (@FYEnd   IS NULL OR PC_Date <= @FYEnd)
              AND (@Month   = 0     OR MONTH(PC_Date) = @Month)
              AND (@Year    = 0     OR YEAR(PC_Date)  = @Year)
            ORDER BY PC_Date DESC, PC_CashKey DESC");
        cmdCash.Parameters.AddWithValue("@FYStart", fyStart.HasValue ? (object)fyStart.Value : DBNull.Value);
        cmdCash.Parameters.AddWithValue("@FYEnd",   fyEnd.HasValue   ? (object)fyEnd.Value   : DBNull.Value);
        cmdCash.Parameters.AddWithValue("@Month",   month);
        cmdCash.Parameters.AddWithValue("@Year",    year);
        DataTable dtCash = DA.GetDataTable(cmdCash);
        lbl_PettyCashBalance.Text = dtCash.Rows.Count > 0
            ? FormatAmount(dtCash.Rows[0]["PC_BalanceAmount"])
            : "0";

        // 4. Total Payroll
        SqlCommand cmdSal = new SqlCommand(@"
            SELECT ISNULL(SUM(NetPay), 0) AS Total
            FROM IT_EmployeePayrollDetails
            WHERE (@FYStart IS NULL OR CAST(
                        CAST(PayrollYear AS VARCHAR(4)) + '-' +
                        RIGHT('0' + CAST(PayrollMonth AS VARCHAR(2)), 2) + '-01'
                        AS DATE) >= @FYStart)
              AND (@FYEnd   IS NULL OR CAST(
                        CAST(PayrollYear AS VARCHAR(4)) + '-' +
                        RIGHT('0' + CAST(PayrollMonth AS VARCHAR(2)), 2) + '-01'
                        AS DATE) <= @FYEnd)
              AND (@Month   = 0     OR PayrollMonth = @Month)
              AND (@Year    = 0     OR PayrollYear  = @Year)");
        cmdSal.Parameters.AddWithValue("@FYStart", fyStart.HasValue ? (object)fyStart.Value : DBNull.Value);
        cmdSal.Parameters.AddWithValue("@FYEnd",   fyEnd.HasValue   ? (object)fyEnd.Value   : DBNull.Value);
        cmdSal.Parameters.AddWithValue("@Month",   month);
        cmdSal.Parameters.AddWithValue("@Year",    year);
        DataTable dtSal = DA.GetDataTable(cmdSal);
        lbl_MonthlyPayroll.Text = FormatAmount(dtSal.Rows[0]["Total"]);

        // Sub-label for Payroll card
        string fyLabel = ddl_FinancialYear.SelectedItem != null && ddl_FinancialYear.SelectedValue != "0"
                         ? ddl_FinancialYear.SelectedItem.Text : "";
        if (!string.IsNullOrEmpty(fyLabel))
            lbl_PayrollMonth.Text = fyLabel;
        else if (month == 0 && year == 0)
            lbl_PayrollMonth.Text = "All Period";
        else if (month == 0)
            lbl_PayrollMonth.Text = year.ToString();
        else if (year == 0)
            lbl_PayrollMonth.Text = new DateTime(2025, month, 1).ToString("MMMM");
        else
            lbl_PayrollMonth.Text = new DateTime(year, month, 1).ToString("MMMM yyyy");
    }

    // ── Ledger Grid ────────────────────────────────────────────────
    private void LoadLedger()
    {
        int month = GetMonth();
        int year  = GetYear();
        string source = GetSrc();
        DateTime? fyStart, fyEnd;
        GetFYDates(out fyStart, out fyEnd);

        SqlCommand cmd = new SqlCommand("GL_Griddetails");
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@FYStart", fyStart.HasValue ? (object)fyStart.Value : DBNull.Value);
        cmd.Parameters.AddWithValue("@FYEnd",   fyEnd.HasValue   ? (object)fyEnd.Value   : DBNull.Value);
        cmd.Parameters.AddWithValue("@Month",   month == 0 ? (object)DBNull.Value : month);
        cmd.Parameters.AddWithValue("@Year",    year  == 0 ? (object)DBNull.Value : year);
        cmd.Parameters.AddWithValue("@Source",  string.IsNullOrWhiteSpace(source) ? (object)DBNull.Value : source);

        DataTable dt = DA.GetDataTable(cmd);
        RenderLedgerRows(dt);
    }

    // ── Render Table Rows ──────────────────────────────────────────
    private void RenderLedgerRows(DataTable dt)
    {
        PH_Ledger.Controls.Clear();

        if (dt == null || dt.Rows.Count == 0)
        {
            PH_Ledger.Controls.Add(new Literal
            {
                Text = "<tr><td colspan='6' class='text-center' style='padding:30px;color:#9ca3af;'>No transactions found.</td></tr>"
            });
            lbl_TxnCount.Text    = "0";
            lbl_TotalDebit.Text  = "0";
            lbl_TotalCredit.Text = "0";
            lbl_BalDebit.Text    = "0";
            lbl_BalCredit.Text   = "0";
            lbl_NetBalance.Text  = "0";
            return;
        }

        decimal totalDebit = 0, totalCredit = 0;

        foreach (DataRow dr in dt.Rows)
        {
            string date    = dr["TransDate"]   != DBNull.Value ? Convert.ToDateTime(dr["TransDate"]).ToString("dd/MM/yyyy") : "-";
            string source  = dr["Source"]      != DBNull.Value ? dr["Source"].ToString() : "";
            string desc    = dr["Description"] != DBNull.Value ? dr["Description"].ToString() : "-";
            string cat     = dr["Category"]    != DBNull.Value ? dr["Category"].ToString() : "";
            decimal debit  = dr["Debit"]  != DBNull.Value ? Convert.ToDecimal(dr["Debit"])  : 0;
            decimal credit = dr["Credit"] != DBNull.Value ? Convert.ToDecimal(dr["Credit"]) : 0;

            totalDebit  += debit;
            totalCredit += credit;

            string debitCell  = debit  > 0 ? "<td class='num amt-debit'>"  + string.Format("{0:N0}", debit)  + "</td>" : "<td class='num amt-dash'>&mdash;</td>";
            string creditCell = credit > 0 ? "<td class='num amt-credit'>" + string.Format("{0:N0}", credit) + "</td>" : "<td class='num amt-dash'>&mdash;</td>";

            string row =
                "<tr>" +
                    "<td class='date-cell'>" + date + "</td>" +
                    "<td>" + SourcePill(source) + "</td>" +
                    "<td>" + System.Web.HttpUtility.HtmlEncode(desc) + "</td>" +
                    "<td>" + CategoryBadge(cat) + "</td>" +
                    debitCell  +
                    creditCell +
                "</tr>";

            PH_Ledger.Controls.Add(new Literal { Text = row });
        }

        decimal net   = totalCredit - totalDebit;
        string netStr = (net < 0 ? "-" : "") + string.Format("{0:N0}", Math.Abs(net));

        lbl_TxnCount.Text    = dt.Rows.Count.ToString();
        lbl_TotalDebit.Text  = string.Format("{0:N0}", totalDebit);
        lbl_TotalCredit.Text = string.Format("{0:N0}", totalCredit);
        lbl_BalDebit.Text    = string.Format("{0:N0}", totalDebit);
        lbl_BalCredit.Text   = string.Format("{0:N0}", totalCredit);
        lbl_NetBalance.Text  = netStr;
    }
}