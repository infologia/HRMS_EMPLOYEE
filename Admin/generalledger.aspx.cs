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
            BindMonthYear();
            LoadKPICards();
            LoadLedger();
        }
    }

    // ── Dropdowns ──────────────────────────────────────────────────
    private void BindMonthYear()
    {
        ddl_Month.Items.Clear();
        ddl_Month.Items.Add(new ListItem("All Months", "0"));
        for (int m = 1; m <= 12; m++)
            ddl_Month.Items.Add(new ListItem(new DateTime(2025, m, 1).ToString("MMMM"), m.ToString()));
        ddl_Month.SelectedValue = DateTime.Now.Month.ToString();

        ddl_Year.Items.Clear();
        int yr = DateTime.Now.Year;
        for (int y = yr - 3; y <= yr + 1; y++)
            ddl_Year.Items.Add(new ListItem(y.ToString(), y.ToString()));
        ddl_Year.SelectedValue = yr.ToString();
    }
    private int GetMonth() { return int.Parse(ddl_Month.SelectedValue); }
    private int GetYear() { return int.Parse(ddl_Year.SelectedValue); }
    private string GetSrc() { return ddl_Source.SelectedValue == "0" ? "" : ddl_Source.SelectedValue; }

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
            case "Payroll": return "<span class='src-pill pill-payslip'><i class='glyphicon glyphicon-file'></i> Payroll</span>";
            case "PettyCash": return "<span class='src-pill pill-petty'><i class='glyphicon glyphicon-piggy-bank'></i> Petty Cash</span>";
            case "Payable": return "<span class='src-pill pill-payable'><i class='glyphicon glyphicon-upload'></i> Invoice (Payable)</span>";
            case "Receivable": return "<span class='src-pill pill-receivable'><i class='glyphicon glyphicon-download-alt'></i> Invoice (Receivable)</span>";
            default: return "<span class='src-pill pill-petty'>" + source + "</span>";
        }
    }
    private string CategoryBadge(string cat)
    {
        switch (cat)
        {
            case "Expense": return "<span class='cat-badge cat-expense'>Expense</span>";
            case "Liability": return "<span class='cat-badge cat-liability'>Liability</span>";
            case "Asset": return "<span class='cat-badge cat-asset'>Asset</span>";
            case "Income": return "<span class='cat-badge cat-income'>Income</span>";
            default: return "<span class='cat-badge cat-expense'>" + cat + "</span>";
        }
    }
    private void LoadKPICards()
    {

        int month = GetMonth();      // 0 = All Months
        int year = GetYear();

        // 1. Total Receivables — invoices with an outstanding balance
        SqlCommand cmdRecv = new SqlCommand(@"
           SELECT ISNULL(SUM(AR_BalanceAmount), 0) AS Total, COUNT(*) AS Cnt
FROM IT_ARPaymentEntry
WHERE AR_BalanceAmount > 0 and 
  (@Month IS NULL OR MONTH(AR_InvoiceDate) = @Month)
        AND (@Year IS NULL OR YEAR(AR_InvoiceDate) = @Year)");
        cmdRecv.Parameters.AddWithValue("@Month", month);
        cmdRecv.Parameters.AddWithValue("@Year", year);
        DataTable dtRecv = DA.GetDataTable(cmdRecv);
        lbl_TotalReceivables.Text = FormatAmount(dtRecv.Rows[0]["Total"]);
        lbl_ReceivablesCount.Text = dtRecv.Rows[0]["Cnt"].ToString();
        
        // 2. Total Payables — vendor invoices with an outstanding balance
        SqlCommand cmdPay = new SqlCommand(@"
            SELECT ISNULL(SUM(AP_BalanceAmount), 0) AS Total, COUNT(*) AS Cnt
            FROM IT_APPaymentEntry
            WHERE AP_BalanceAmount > 0  and 
  (@Month IS NULL OR MONTH(AP_InvoiceDate) = @Month)
        AND (@Year IS NULL OR YEAR(AP_InvoiceDate) = @Year)");
        cmdPay.Parameters.AddWithValue("@Month", month);
        cmdPay.Parameters.AddWithValue("@Year", year);
        DataTable dtPay = DA.GetDataTable(cmdPay);
        lbl_TotalPayables.Text = FormatAmount(dtPay.Rows[0]["Total"]);
        lbl_PayablesCount.Text = dtPay.Rows[0]["Cnt"].ToString();

        // 3. Petty Cash Balance — PC_BalanceAmount is a running balance,
        //    so the most recent entry holds the current balance.
        SqlCommand cmdCash = new SqlCommand(@"
            SELECT TOP 1 PC_BalanceAmount
            FROM TT_PettyCash where (@Month IS NULL OR MONTH(PC_Date) = @Month)
        AND (@Year IS NULL OR YEAR(PC_Date) = @Year)
            ORDER BY PC_Date DESC, PC_CashKey DESC");
        cmdCash.Parameters.AddWithValue("@Month", month);
        cmdCash.Parameters.AddWithValue("@Year", year);
        DataTable dtCash = DA.GetDataTable(cmdCash);
        lbl_PettyCashBalance.Text = dtCash.Rows.Count > 0
            ? FormatAmount(dtCash.Rows[0]["PC_BalanceAmount"])
            : "0";

        // 4. Monthly Payroll — current calendar month
        SqlCommand cmdSal = new SqlCommand(@"
            SELECT ISNULL(SUM(NetPay), 0) AS Total
            FROM IT_EmployeePayrollDetails
            WHERE PayrollMonth = @Month AND PayrollYear = @Year");
        cmdSal.Parameters.AddWithValue("@Month", month);
        cmdSal.Parameters.AddWithValue("@Year", year);
        DataTable dtSal = DA.GetDataTable(cmdSal);
        lbl_MonthlyPayroll.Text = FormatAmount(dtSal.Rows[0]["Total"]);
        if (month==0) {
            lbl_PayrollMonth.Text = "All Month";
        }
        else
        {
            lbl_PayrollMonth.Text = new DateTime(year, month, 1).ToString("MMMM yyyy");
        }
        
    }

    private void LoadLedger()
    {
        int month = GetMonth();      // 0 = All Months
        int year = GetYear();        // 0 = All Years
        string source = GetSrc();    // "" = All Sources

        SqlCommand cmd = new SqlCommand("GL_Griddetails");
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@Month", month == 0 ? (object)DBNull.Value : month);
        cmd.Parameters.AddWithValue("@Year", year == 0 ? (object)DBNull.Value : year);
        cmd.Parameters.AddWithValue("@Source",
            string.IsNullOrWhiteSpace(source) ? (object)DBNull.Value : source);

        DataTable dt = DA.GetDataTable(cmd);
        RenderLedgerRows(dt);
    }

    protected void Filter_Changed(object sender, EventArgs e)
    {
        LoadKPICards();
        LoadLedger();
    }

    private void RenderLedgerRows(DataTable dt)
    {
        PH_Ledger.Controls.Clear();

        if (dt == null || dt.Rows.Count == 0)
        {
            PH_Ledger.Controls.Add(new Literal
            {
                Text = "<tr><td colspan='6' class='text-center' style='padding:30px;color:#9ca3af;'>No transactions found.</td></tr>"
            });
            lbl_TxnCount.Text = "0";
            lbl_TotalDebit.Text = "0";
            lbl_TotalCredit.Text = "0";
            lbl_BalDebit.Text = "0";
            lbl_BalCredit.Text = "0";
            lbl_NetBalance.Text = "0";
            return;
        }

        decimal totalDebit = 0, totalCredit = 0;

        foreach (DataRow dr in dt.Rows)
        {
            string date = dr["TransDate"] != DBNull.Value ? Convert.ToDateTime(dr["TransDate"]).ToString("dd/MM/yyyy") : "-";
            string source = dr["Source"] != DBNull.Value ? dr["Source"].ToString() : "";
            string desc = dr["Description"] != DBNull.Value ? dr["Description"].ToString() : "-";
            string cat = dr["Category"] != DBNull.Value ? dr["Category"].ToString() : "";
            decimal debit = dr["Debit"] != DBNull.Value ? Convert.ToDecimal(dr["Debit"]) : 0;
            decimal credit = dr["Credit"] != DBNull.Value ? Convert.ToDecimal(dr["Credit"]) : 0;

            totalDebit += debit;
            totalCredit += credit;

            string debitCell = debit > 0 ? "<td class='num amt-debit'>" + string.Format("{0:N0}", debit) + "</td>" : "<td class='num amt-dash'>&mdash;</td>";
            string creditCell = credit > 0 ? "<td class='num amt-credit'>" + string.Format("{0:N0}", credit) + "</td>" : "<td class='num amt-dash'>&mdash;</td>";

            string row =
                "<tr>" +
                    "<td class='date-cell'>" + date + "</td>" +
                    "<td>" + SourcePill(source) + "</td>" +
                    "<td>" + System.Web.HttpUtility.HtmlEncode(desc) + "</td>" +
                    "<td>" + CategoryBadge(cat) + "</td>" +
                    debitCell +
                    creditCell +
                "</tr>";

            PH_Ledger.Controls.Add(new Literal { Text = row });
        }

        // Net = Debit (Assets/Expenses) - Credit (Liabilities/Income)
        decimal net = totalCredit - totalDebit;
        string netStr = (net < 0 ? "-" : "") + string.Format("{0:N0}", Math.Abs(net));

        lbl_TxnCount.Text = dt.Rows.Count.ToString();
        lbl_TotalDebit.Text = string.Format("{0:N0}", totalDebit);
        lbl_TotalCredit.Text = string.Format("{0:N0}", totalCredit);
        lbl_BalDebit.Text = string.Format("{0:N0}", totalDebit);
        lbl_BalCredit.Text = string.Format("{0:N0}", totalCredit);
        lbl_NetBalance.Text = netStr;
    }
}