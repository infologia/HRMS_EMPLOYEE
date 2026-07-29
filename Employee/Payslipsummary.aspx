<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Payslipsummary.aspx.cs" Inherits="Employee_Payslipsummary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        /* ── Wrapper ── */
        .ps-wrap { background:#fff; border-radius:8px; box-shadow:0 1px 8px rgba(0,0,0,.1); overflow:hidden; }

        /* ── Header ── */
        .ps-top-bar {
            border-top: 4px solid #2d6a9f;
            padding: 14px 20px;
            display: flex;
            align-items: center;
            justify-content: space-between;
            border-bottom: 1px solid #e8edf3;
            background: #fff;
        }
        .ps-top-bar .ps-title  { font-size:16px; font-weight:700; color:#1e3a5f; margin:0; }
        .ps-top-bar .ps-sub    { font-size:12px; color:#8a9bb8; margin:2px 0 0 0; }
        .ps-top-bar .ps-back {
            font-size:12px; color:#2d6a9f; border:1px solid #2d6a9f;
            border-radius:4px; padding:5px 14px; text-decoration:none;
            transition: all .2s;
        }
        .ps-top-bar .ps-back:hover { background:#2d6a9f; color:#fff; text-decoration:none; }

        /* ── 6 Cards in one row ── */
        .ps-cards {
            display: flex;
            flex-wrap: nowrap;
            gap: 0;
            padding: 16px 20px;
            border-bottom: 1px solid #e8edf3;
        }
        .ps-card {
            flex: 1;
            padding: 14px 16px;
            border-left: 1px solid #e8edf3;
            position: relative;
        }
        .ps-card:first-child { border-left: none; }
        .ps-card .card-icon {
            font-size: 22px;
            margin-bottom: 6px;
        }
        .ps-card .card-label {
            font-size: 10px; color: #999;
            text-transform: uppercase; letter-spacing: .05em; margin-bottom: 4px;
        }
        .ps-card .card-value {
            font-size: 20px; font-weight: 700; color: #1e3a5f; line-height: 1;
        }
        .ps-card .card-sub { font-size: 10px; color: #aaa; margin-top: 3px; }

        /* icon colors */
        .ic-blue   { color: #2d6a9f; }
        .ic-green  { color: #1a9e4d; }
        .ic-teal   { color: #0f8f80; }
        .ic-orange { color: #e07b00; }
        .ic-red    { color: #c0392b; }
        .ic-purple { color: #6c3fc5; }

        /* ── Section Title ── */
        .ps-sect {
            font-size: 11px; font-weight: 700; color: #2d6a9f;
            text-transform: uppercase; letter-spacing: .07em;
            padding: 12px 20px 8px 20px;
            border-bottom: 1px solid #e8edf3;
            background: #f7faff;
        }

        /* ── Compact Payroll Grid ── */
        .ps-detail-wrap { padding: 0 20px 16px 20px; }
        .ps-detail-table {
            width: 100%; border-collapse: collapse;
            font-size: 12.5px; margin-top: 12px;
        }
        .ps-detail-table th, .ps-detail-table td {
            padding: 6px 10px;
            border: 1px solid #e3eaf3;
            white-space: nowrap;
        }
        .ps-detail-table th {
            background: #f0f5ff;
            color: #1e3a5f;
            font-weight: 600;
            width: 22%;
        }
        .ps-detail-table td {
            color: #333;
            width: 28%;
        }
        .ps-detail-table tr:hover td { background: #f7faff; }
        .ps-detail-table .highlight-val {
            font-weight: 700; color: #1a9e4d;
        }

        /* ── Attendance Table ── */
        .ps-att-wrap { padding: 0 20px 20px 20px; overflow-x: auto; }
        .ps-att-table {
            width: 100%; border-collapse: collapse;
            font-size: 12.5px; margin-top: 12px;
        }
        .ps-att-table thead tr { background: #1e3a5f; }
        .ps-att-table thead th {
            padding: 9px 12px; color: #fff;
            font-weight: 500; white-space: nowrap; text-align: left;
        }
        .ps-att-table tbody tr:nth-child(even) { background: #f7faff; }
        .ps-att-table tbody tr:hover { background: #edf3fb; }
        .ps-att-table tbody td {
            padding: 7px 12px; border-bottom: 1px solid #e8edf3;
            color: #333; white-space: nowrap;
        }
        .badge-in  { background:#d4edda; color:#155724; border-radius:3px; padding:2px 7px; font-size:11px; }
        .badge-out { background:#f8d7da; color:#721c24; border-radius:3px; padding:2px 7px; font-size:11px; }
        .badge-hrs { background:#cce5ff; color:#004085; border-radius:3px; padding:2px 7px; font-size:11px; }
        .ps-nodata { text-align:center; padding:30px; color:#bbb; font-size:13px; }
    </style>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="ps-wrap">

    <%-- ── Header ── --%>
    <div class="ps-top-bar">
        <div>
            <p class="ps-title">Payslip Summary &nbsp;&mdash;&nbsp; <asp:Label ID="lbl_monthyear" runat="server" Text=""></asp:Label></p>
            <p class="ps-sub">Attendance &amp; Payroll Overview</p>
        </div>
        <a href="payslipdetails.aspx" class="ps-back">&larr; Back to Payslips</a>
    </div>

    <%-- ── 6 Cards in one row ── --%>
    <div class="ps-cards">
        <div class="ps-card">
            <div class="card-icon ic-blue"><i class="icon-calendar"></i></div>
            <div class="card-label">Days in Month</div>
            <div class="card-value"><asp:Label ID="lbl_days_in_month" runat="server" Text="-"></asp:Label></div>
            <div class="card-sub">Calendar days</div>
        </div>
        <div class="ps-card">
            <div class="card-icon ic-green"><i class="icon-briefcase"></i></div>
            <div class="card-label">Working Days</div>
            <div class="card-value"><asp:Label ID="lbl_working_days" runat="server" Text="-"></asp:Label></div>
            <div class="card-sub">Eligible days</div>
        </div>
        <div class="ps-card">
            <div class="card-icon ic-teal"><i class="icon-checkmark-circle"></i></div>
            <div class="card-label">Paid Holidays</div>
            <div class="card-value"><asp:Label ID="lbl_paid_holidays" runat="server" Text="-"></asp:Label></div>
            <div class="card-sub">Public holidays</div>
        </div>
        <div class="ps-card">
            <div class="card-icon ic-orange"><i class="icon-warning2"></i></div>
            <div class="card-label">LOP Days</div>
            <div class="card-value"><asp:Label ID="lbl_lop_days" runat="server" Text="-"></asp:Label></div>
            <div class="card-sub">Loss of pay</div>
        </div>
        <div class="ps-card">
            <div class="card-icon ic-red"><i class="icon-minus-circle2"></i></div>
            <div class="card-label">Deduction Days</div>
            <div class="card-value"><asp:Label ID="lbl_deduction_days" runat="server" Text="-"></asp:Label></div>
            <div class="card-sub">Total deducted</div>
        </div>
        <div class="ps-card">
            <div class="card-icon ic-purple"><i class="icon-coin-dollar"></i></div>
            <div class="card-label">Net Pay</div>
            <div class="card-value">&#8377; <asp:Label ID="lbl_netpay" runat="server" Text="-"></asp:Label></div>
            <div class="card-sub">Bank credit</div>
        </div>
    </div>

    <%-- ── Payroll Details ── --%>
    <div class="ps-sect">Payroll Details</div>
    <div class="ps-detail-wrap">
        <table class="ps-detail-table">
            <tbody>
                <tr>
                    <th>Days in Month</th>
                    <td><asp:Label ID="lbl_days_in_month2" runat="server" Text="-"></asp:Label></td>
                    <th>Working Days</th>
                    <td><asp:Label ID="lbl_working_days2" runat="server" Text="-"></asp:Label></td>
                </tr>
                <tr>
                    <th>Paid Holidays</th>
                    <td><asp:Label ID="lbl_pd2" runat="server" Text="-"></asp:Label></td>
                    <th>Informed Leave</th>
                    <td><asp:Label ID="lbl_il2" runat="server" Text="-"></asp:Label></td>
                </tr>
                <tr>
                    <th>Leave Days in Year</th>
                    <td><asp:Label ID="lbl_ldy2" runat="server" Text="-"></asp:Label></td>
                    <th>Current Month Leave</th>
                    <td><asp:Label ID="lbl_cmld2" runat="server" Text="-"></asp:Label></td>
                </tr>
                <tr>
                    <th>LOP Leave Days</th>
                    <td><asp:Label ID="lbl_loplv2" runat="server" Text="-"></asp:Label></td>
                    <th>Uninformed Leave</th>
                    <td><asp:Label ID="lbl_uninf2" runat="server" Text="-"></asp:Label></td>
                </tr>
                <tr>
                    <th>Half Day Count</th>
                    <td><asp:Label ID="lbl_hdc2" runat="server" Text="-"></asp:Label></td>
                    <th>Half Day Deduction</th>
                    <td><asp:Label ID="lbl_hdd2" runat="server" Text="-"></asp:Label></td>
                </tr>
                <tr>
                    <th>Full Day Deduction</th>
                    <td><asp:Label ID="lbl_fdd2" runat="server" Text="-"></asp:Label></td>
                    <th>Total Deduction Days</th>
                    <td><asp:Label ID="lbl_tdd2" runat="server" Text="-"></asp:Label></td>
                </tr>
                <tr>
                    <th>Monthly Salary</th>
                    <td>&#8377; <asp:Label ID="lbl_ms2" runat="server" Text="-"></asp:Label></td>
                    <th>Per Day Salary</th>
                    <td>&#8377; <asp:Label ID="lbl_pds2" runat="server" Text="-"></asp:Label></td>
                </tr>
                <tr>
                    <th>Leave Days Salary</th>
                    <td>&#8377; <asp:Label ID="lbl_lds2" runat="server" Text="-"></asp:Label></td>
                    <th>Total Eligible Days</th>
                    <td><asp:Label ID="lbl_ted2" runat="server" Text="-"></asp:Label></td>
                </tr>
                <tr>
                    <th>Eligible Salary Amount</th>
                    <td>&#8377; <asp:Label ID="lbl_esa2" runat="server" Text="-"></asp:Label></td>
                    <th>Net Pay</th>
                    <td>&#8377; <asp:Label ID="lbl_np2" runat="server" Text="-"></asp:Label></td>
                </tr>
                <tr>
                    <th>Annual CTC</th>
                    <td>&#8377; <asp:Label ID="lbl_ctc2" runat="server" Text="-"></asp:Label></td>
                    <th>InTime in HRMS</th>
                    <td><asp:Label ID="lbl_ita2" runat="server" Text="-"></asp:Label></td>
                </tr>
                <tr>
                    <th>Late Login Count</th>
                    <td><asp:Label ID="lbl_llc2" runat="server" Text="-"></asp:Label></td>
                    <th>OutTime Null Count</th>
                    <td><asp:Label ID="lbl_onc2" runat="server" Text="-"></asp:Label></td>
                </tr>
                <tr>
                    <th>Generated By</th>
                    <td colspan="3"><asp:Label ID="lbl_gen2" runat="server" Text="-"></asp:Label></td>
                </tr>
            </tbody>
        </table>
    </div>

    <%-- ── Daily Attendance ── --%>
    <div class="ps-sect">Daily Attendance Log</div>
    <div class="ps-att-wrap">
        <asp:Label ID="lbl_nodata" runat="server" Visible="false" CssClass="ps-nodata" Text="No attendance data found for this month."></asp:Label>
        <asp:Panel ID="pnl_attendance" runat="server" Visible="false">
            <table class="ps-att-table datatable-basic">
                <thead>
                    <tr>
                        <th>Date</th>
                        <th>Day</th>
                        <th>In Time</th>
                        <th>Out Time</th>
                        <th>Gross Hours</th>
                        <th>Lunch</th>
                        <th>Break</th>
                        <th>Net Working Hours</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:PlaceHolder ID="PH_attendance" runat="server"></asp:PlaceHolder>
                </tbody>
            </table>
        </asp:Panel>
    </div>

</div>
</asp:Content>
