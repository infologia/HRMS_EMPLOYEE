<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="generalledger.aspx.cs" Inherits="Admin_generalledger" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        /* ── KPI Cards ── */
        .gl-kpi-row {
            display: grid;
            grid-template-columns: repeat(4, minmax(0, 1fr));
            gap: 12px;
            margin-bottom: 20px;
        }
        .gl-kpi {
            background: #fff;
            border: 0.5px solid #e5e7eb;
            border-radius: 12px;
            padding: 16px 18px;
            border-top: 3px solid var(--ka);
        }
        .gl-kpi-top {
            display: flex;
            align-items: center;
            gap: 10px;
            margin-bottom: 8px;
        }
        .gl-kpi-icon {
            width: 36px;
            height: 36px;
            border-radius: 8px;
            display: flex;
            align-items: center;
            justify-content: center;
            background: var(--ki);
            flex-shrink: 0;
        }
        .gl-kpi-icon i {
            font-size: 18px;
            color: var(--kc);
        }
        .gl-kpi-label {
            font-size: 11px;
            color: #9ca3af;
            text-transform: uppercase;
            letter-spacing: 0.4px;
            font-weight: 600;
        }
        .gl-kpi-value {
            font-size: 26px;
            font-weight: 600;
            color: #111827;
            line-height: 1.1;
        }
        .gl-kpi-sub {
            font-size: 11px;
            color: #9ca3af;
            margin-top: 4px;
        }
        .gl-kpi-recv  { --ka: #2a78d6; --ki: #e6f1fb; --kc: #2a78d6; }
        .gl-kpi-pay   { --ka: #e34948; --ki: #fdeceb; --kc: #e34948; }
        .gl-kpi-cash  { --ka: #1baf7a; --ki: #e1f5ee; --kc: #1baf7a; }
        .gl-kpi-sal   { --ka: #eda100; --ki: #faeeda; --kc: #eda100; }

        /* ── Section heading ── */
        .gl-section-head {
            display: flex;
            align-items: center;
            justify-content: space-between;
            margin-bottom: 12px;
            flex-wrap: wrap;
            gap: 10px;
        }
        .gl-section-title {
            font-size: 15px;
            font-weight: 600;
            color: #111827;
            display: flex;
            align-items: center;
            gap: 8px;
        }
        .gl-section-title i {
            font-size: 17px;
            color: #6b7280;
        }
        .gl-filters {
            display: flex;
            gap: 8px;
            align-items: center;
            flex-wrap: wrap;
        }
        .gl-filters .form-control {
            height: 32px;
            font-size: 12px;
            border-radius: 6px;
            border: 0.5px solid #d1d5db;
            padding: 0 10px;
        }

        /* ── Ledger table card ── */
        .gl-card {
            background: #fff;
            border: 0.5px solid #e5e7eb;
            border-radius: 12px;
            overflow: hidden;
            margin-bottom: 14px;
        }
        .gl-table {
            width: 100%;
            border-collapse: collapse;
            table-layout: fixed;
        }
        .gl-table thead th {
            background: #f9fafb;
            color: #9ca3af;
            font-size: 11px !important;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 0.4px;
            padding: 10px 14px !important;
            border-bottom: 0.5px solid #e5e7eb !important;
            border-top: none !important;
            white-space: nowrap;
            text-align: left;
        }
        .gl-table thead th.num { text-align: right; }
        .gl-table tbody tr {
            border-bottom: 0.5px solid #f3f4f6;
            transition: background 0.1s;
        }
        .gl-table tbody tr:last-child { border-bottom: none; }
        .gl-table tbody tr:hover td { background: #f9fafb; }
        .gl-table tbody td {
            padding: 9px 14px !important;
            font-size: 12.5px !important;
            color: #374151;
            vertical-align: middle;
            border: none !important;
        }
        .gl-table tbody td.num {
            text-align: right;
            font-variant-numeric: tabular-nums;
        }
        .gl-table tfoot td {
            padding: 10px 14px !important;
            background: #f9fafb;
            font-size: 12.5px !important;
            font-weight: 600;
            color: #374151;
            border-top: 0.5px solid #e5e7eb !important;
            border-bottom: none !important;
        }

        /* ── Source pills ── */
        .src-pill {
            display: inline-flex;
            align-items: center;
            gap: 4px;
            padding: 2px 8px;
            border-radius: 20px;
            font-size: 11px;
            font-weight: 600;
        }
        .src-pill i { font-size: 11px; }
        .pill-payslip  { background: #faeeda; color: #854f0b; }
        .pill-petty    { background: #e1f5ee; color: #085041; }
        .pill-payable  { background: #fdeceb; color: #993c1d; }
        .pill-receivable { background: #e6f1fb; color: #185fa5; }

        /* ── Category badges ── */
        .cat-badge {
            display: inline-block;
            padding: 2px 8px;
            border-radius: 4px;
            font-size: 11px;
            font-weight: 600;
        }
        .cat-expense   { background: #faeeda; color: #854f0b; }
        .cat-liability { background: #fdeceb; color: #993c1d; }
        .cat-asset     { background: #e1f5ee; color: #085041; }
        .cat-income    { background: #e6f1fb; color: #185fa5; }

        /* ── Debit / Credit ── */
        .amt-debit  { color: #d03b3b; font-weight: 600; }
        .amt-credit { color: #0ca30c; font-weight: 600; }
        .amt-dash   { color: #d1d5db; }

        /* ── Balance bar ── */
       .gl-balance-bar {
    background: #fff;
    border: 0.5px solid #e5e7eb;
    border-radius: 12px;
    padding: 14px 18px;
    display: flex;
    align-items: center;
    gap: 24px;
    flex-wrap: wrap;
}
       .gl-bal-item{
    display:flex;
    align-items:center;
    gap:8px;
}

.gl-bal-item:last-child{
    margin-right:auto;
}
.gl-table tfoot td.num{
    text-align:right !important;
    padding-right:14px !important;
}

.gl-table thead th.num,
.gl-table tbody td.num{
    text-align:right !important;
}
.gl-balance-bar > div:last-child{
    margin-left:auto;
}
        .gl-bal-dot {
            width: 10px;
            height: 10px;
            border-radius: 2px;
        }
        .gl-bal-label { font-size: 12px; color: #6b7280; }
        .gl-bal-val   { font-size: 14px; font-weight: 600; color: #111827; }
        .gl-bal-net   { font-size: 14px; font-weight: 600; color: #d03b3b; }

        /* ── Date col muted ── */
        .date-cell { color: #6b7280; font-size: 12px; }

        @media (max-width: 992px) {
            .gl-kpi-row { grid-template-columns: repeat(2, 1fr); }
        }
        @media (max-width: 576px) {
            .gl-kpi-row { grid-template-columns: 1fr; }
            .gl-filters { flex-direction: column; align-items: stretch; }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <!-- KPI Cards -->
    <div class="gl-kpi-row">
        <div class="gl-kpi gl-kpi-recv">
            <div class="gl-kpi-top">
                <div class="gl-kpi-icon"><i class="glyphicon glyphicon-download-alt"></i></div>
                <div class="gl-kpi-label">Pending Receivables</div>
            </div>
            <div class="gl-kpi-value">
                &#8377;<asp:Label ID="lbl_TotalReceivables" runat="server" Text="0"></asp:Label>
            </div>
            <div class="gl-kpi-sub">
                <asp:Label ID="lbl_ReceivablesCount" runat="server" Text="0"></asp:Label> pending invoices
            </div>
        </div>

        <div class="gl-kpi gl-kpi-pay">
            <div class="gl-kpi-top">
                <div class="gl-kpi-icon"><i class="glyphicon glyphicon-upload"></i></div>
                <div class="gl-kpi-label">Outstanding Payables</div>
            </div>
            <div class="gl-kpi-value">
                &#8377;<asp:Label ID="lbl_TotalPayables" runat="server" Text="0"></asp:Label>
            </div>
            <div class="gl-kpi-sub">
                <asp:Label ID="lbl_PayablesCount" runat="server" Text="0"></asp:Label> vendor invoices
            </div>
        </div>

        <div class="gl-kpi gl-kpi-cash">
            <div class="gl-kpi-top">
                <div class="gl-kpi-icon"><i class="glyphicon glyphicon-piggy-bank"></i></div>
                <div class="gl-kpi-label">Petty Cash Balance</div>
            </div>
            <div class="gl-kpi-value">
                &#8377;<asp:Label ID="lbl_PettyCashBalance" runat="server" Text="0"></asp:Label>
            </div>
            <div class="gl-kpi-sub">Opening balance &#8722; expenses </div>
        </div>

        <div class="gl-kpi gl-kpi-sal">
            <div class="gl-kpi-top">
                <div class="gl-kpi-icon"><i class="glyphicon glyphicon-user"></i></div>
                <div class="gl-kpi-label">Monthly Payroll</div>
            </div>
            <div class="gl-kpi-value">
                &#8377;<asp:Label ID="lbl_MonthlyPayroll" runat="server" Text="0"></asp:Label>
            </div>
            <div class="gl-kpi-sub">
                <asp:Label ID="lbl_PayrollMonth" runat="server" Text="Current month"></asp:Label> payslips
            </div>
        </div>
    </div>

    <!-- Section heading + filters -->
    <div class="gl-section-head">
        <div class="gl-section-title">
            <i class="glyphicon glyphicon-list-alt"></i>
            General Ledger
        </div>
        <div class="gl-filters">

            <asp:DropDownList ID="ddl_FinancialYear" runat="server" CssClass="form-control"
                style="width:150px">
            </asp:DropDownList>

            <asp:DropDownList ID="ddl_Month" runat="server" CssClass="form-control"
                style="width:140px">
            </asp:DropDownList>

            <asp:DropDownList ID="ddl_Year" runat="server" CssClass="form-control"
                style="width:110px">
            </asp:DropDownList>

            <asp:DropDownList ID="ddl_Source" runat="server" CssClass="form-control"
                style="width:180px">
                <asp:ListItem Value="0">All Sources</asp:ListItem>
                <asp:ListItem Value="Payroll">Payroll</asp:ListItem>
                <asp:ListItem Value="PettyCash">Petty Cash</asp:ListItem>
                <asp:ListItem Value="Payable">Invoice (Payable)</asp:ListItem>
                <asp:ListItem Value="Receivable">Invoice (Receivable)</asp:ListItem>
            </asp:DropDownList>

            <asp:Button ID="btn_Filter" runat="server" Text="Apply Filter"
                CssClass="btn btn-primary btn-sm"
                OnClick="btn_Filter_Click"
                style="height:32px; padding:0 14px; font-size:12px; border-radius:6px;" />

        </div>
    </div>

    <!-- Ledger table -->
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="gl-card">
                <table class="gl-table datatable-basic">
                    <colgroup>
                        <col style="width:100px">
                        <col style="width:160px">
                        <col>
                        <col style="width:105px">
                        <col style="width:120px">
                        <col style="width:120px">
                    </colgroup>
                    <thead>
                        <tr>
                            <th>Date</th>
                            <th>Source</th>
                            <th>Description</th>
                            <th>Category</th>
                            <th class="num">Debit (+)</th>
                            <th class="num">Credit (&minus;)</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder ID="PH_Ledger" runat="server"></asp:PlaceHolder>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td colspan="4" style="color:#6b7280;font-size:12px;">
                                <asp:Label ID="lbl_TxnCount" runat="server" Text="0"></asp:Label> transactions
                            </td>
                            <td class="num amt-debit">
                                &#8377;<asp:Label ID="lbl_TotalDebit" runat="server" Text="0"></asp:Label>
                            </td>
                            <td class="num amt-credit">
                                &#8377;<asp:Label ID="lbl_TotalCredit" runat="server" Text="0"></asp:Label>
                            </td>
                        </tr>
                    </tfoot>
                </table>
            </div>

            <!-- Balance summary bar -->
            <div class="gl-balance-bar">
                <div class="gl-bal-item">
                    <div class="gl-bal-dot" style="background:#e34948;"></div>
                    <span class="gl-bal-label">Total Debit</span>
                    <span class="gl-bal-val">&#8377;<asp:Label ID="lbl_BalDebit" runat="server" Text="0"></asp:Label></span>
                </div>
                <div class="gl-bal-item">
                    <div class="gl-bal-dot" style="background:#1baf7a;"></div>
                    <span class="gl-bal-label">Total Credit</span>
                    <span class="gl-bal-val">&#8377;<asp:Label ID="lbl_BalCredit" runat="server" Text="0"></asp:Label></span>
                </div>
                <div class="gl-bal-item">
                    <div class="gl-bal-dot" style="background:#eda100;"></div>
                    <span class="gl-bal-label">Net Balance</span>
                    <span class="gl-bal-net">&#8377;<asp:Label ID="lbl_NetBalance" runat="server" Text="0"></asp:Label></span>
                </div>
                <div style="margin-left:auto; display:flex; gap:8px;">
                    
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
