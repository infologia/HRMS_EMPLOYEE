<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="payslips.aspx.cs" Inherits="Admin_payslips" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        :root {
            --ledger-ink: #3B4472;
            --ledger-ink-light: #4C5691;
            --ledger-border: #E4E2DB;
            --ledger-card-bg: #F7F8FB;
            --ledger-text-secondary: #6B7280;
            --ledger-teal: #0F7A6C;
            --ledger-teal-bg: #E3F3F0;
            --ledger-amber: #97620A;
            --ledger-amber-bg: #FBF0DE;
            --ledger-coral: #A32D2D;
            --ledger-coral-bg: #FBE7DE;
            --ledger-mono: -apple-system, "Segoe UI", Roboto, Helvetica, Arial, sans-serif;
        }

        /* ===== HEADER / FILTER BAR ===== */
        .ledger-header {
            background: var(--ledger-ink) !important;
            border-radius: 6px 6px 0 0;
            padding: 18px 22px !important;
            display: flex;
            align-items: center;
            justify-content: space-between;
            flex-wrap: wrap;
            gap: 14px;
        }

            .ledger-header .ledger-eyebrow {
                font-size: 11px;
                letter-spacing: 0.06em;
                text-transform: uppercase;
                color: #8A93C4;
                margin: 0 0 4px 0;
            }

            .ledger-header .panel-title {
                margin: 0 !important;
                color: #F6F5F1 !important;
                font-weight: 500;
                font-size: 19px;
            }

                .ledger-header .panel-title #lbl_monthyear {
                    color: #F6F5F1 !important;
                }

        .ledger-controls {
            display: flex;
            align-items: center;
            gap: 8px;
            flex-wrap: wrap;
            margin-left: auto;
        }

            .ledger-controls label {
                margin: 0;
                font-size: 12px;
                font-weight: 600;
                color: #C7CCE6;
            }

            .ledger-controls select.form-control {
                height: 34px !important;
                font-size: 12px !important;
                border-radius: 6px !important;
                border: 1px solid rgba(255,255,255,0.25) !important;
                background: #fff !important;
                color: #1B2340 !important;
            }

            .ledger-controls .btn-primary,
            .ledger-controls .btn-success,
            .ledger-controls .btn-info {
                border-radius: 6px !important;
                height: 34px !important;
                font-size: 12px !important;
                width: 120px !important;
                padding: 0 8px !important;
                display: inline-flex;
                align-items: center;
                justify-content: center;
                box-sizing: border-box;
            }

            .ledger-controls .btn-primary {
                background: #3f51b5 !important;
                border-color: #3f51b5 !important;
            }

        /* Compact, read-only header used when arriving from "View" on the Payroll History page */
        .ledger-header-compact {
            padding: 10px 22px !important;
        }

            .ledger-header-compact .panel-title {
                font-size: 16px;
            }

        .ledger-columns-btn {
            border: 1px solid rgba(255,255,255,0.25) !important;
            border-radius: 6px !important;
            padding: 0 10px !important;
            background: rgba(255,255,255,0.06) !important;
            color: #F6F5F1 !important;
            font-size: 12px;
            white-space: nowrap;
            cursor: pointer;
            text-decoration: none;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            gap: 6px;
            height: 34px;
            width: 150px;
            box-sizing: border-box;
        }

            .ledger-columns-btn:hover {
                background: rgba(255,255,255,0.14) !important;
                text-decoration: none;
                color: #fff !important;
            }

        /* ===== KPI SUMMARY STRIP ===== */
        .ledger-kpi-strip {
            display: grid;
            grid-template-columns: repeat(4, 1fr);
            gap: 10px;
            padding: 12px 22px;
            background: #fff;
        }

        @media (max-width: 900px) {
            .ledger-kpi-strip {
                grid-template-columns: repeat(2, 1fr);
            }
        }

        @media (max-width: 500px) {
            .ledger-kpi-strip {
                grid-template-columns: 1fr;
            }
        }

        .ledger-kpi-card {
            background: #fff;
            border: 1px solid #E5E7EB;
            border-radius: 8px;
            padding: 10px 12px;
            display: flex;
            align-items: center;
            gap: 10px;
            box-shadow: 0 1px 2px rgba(16,24,40,0.04);
            min-width: 0;
        }

        .kpi-icon {
            width: 32px;
            height: 32px;
            border-radius: 8px;
            display: flex;
            align-items: center;
            justify-content: center;
            flex-shrink: 0;
        }

            .kpi-icon svg {
                width: 16px;
                height: 16px;
            }

        .kpi-icon-blue {
            background: #DBEAFE;
            color: #2563EB;
        }

        .kpi-icon-violet {
            background: #EDE9FE;
            color: #7C3AED;
        }

        .kpi-icon-amber {
            background: #FEF3C7;
            color: #D97706;
        }

        .kpi-icon-red {
            background: #FEE2E2;
            color: #DC2626;
        }

        .kpi-icon-rose {
            background: #FCE7F3;
            color: #DB2777;
        }

        .kpi-icon-cyan {
            background: #CFFAFE;
            color: #0891B2;
        }

        .kpi-icon-green {
            background: #D1FAE5;
            color: #059669;
        }

        .kpi-icon-slate {
            background: #F1F2F6;
            color: #4B5563;
        }

        .ledger-kpi-card .kpi-text {
            min-width: 0;
        }

        .ledger-kpi-card .kpi-label {
            font-size: 11px;
            color: var(--ledger-text-secondary);
            margin-bottom: 1px;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }

        .ledger-kpi-card .kpi-value {
            font-size: 16px;
            font-weight: 700;
            font-family: var(--ledger-mono);
            color: #1B2340;
            white-space: nowrap;
        }

        /* ===== TABLE ===== */
        .payroll-table {
            font-size: 12px !important;
            border-collapse: collapse !important;
        }

            .payroll-table thead th {
                white-space: nowrap;
                vertical-align: middle !important;
                text-align: center;
                padding: 6px 8px !important;
                font-size: 11px !important;
                font-weight: 600 !important;
                background-color: var(--ledger-ink) !important;
                color: #F6F5F1 !important;
                border: 1px solid #4C5691 !important;
                background-image: none !important;
                position: sticky;
                top: 0;
                z-index: 2;
            }

                .payroll-table thead th:nth-child(1),
                .payroll-table thead th:nth-child(2) {
                    text-align: left;
                }
                /* Hide DataTables sorting icons without hiding the header */
                .payroll-table thead th.sorting:after,
                .payroll-table thead th.sorting_asc:after,
                .payroll-table thead th.sorting_desc:after,
                .payroll-table thead th.sorting:before,
                .payroll-table thead th.sorting_asc:before,
                .payroll-table thead th.sorting_desc:before {
                    display: none !important;
                    content: none !important;
                }

            .payroll-table tbody td {
                white-space: nowrap;
                padding: 4px 8px !important;
                font-size: 12px !important;
                line-height: 1.3 !important;
                vertical-align: middle !important;
                border: 1px solid var(--ledger-border) !important;
            }

                .payroll-table tbody td:nth-child(1) {
                    text-align: left;
                    font-weight: 600;
                    color: #1B2340;
                }

                .payroll-table tbody td:nth-child(2) {
                    text-align: left;
                    color: var(--ledger-text-secondary);
                }

                .payroll-table tbody td:nth-child(n+3) {
                    text-align: right;
                    font-family: var(--ledger-mono);
                    font-variant-numeric: tabular-nums;
                    color: #374151;
                    letter-spacing: -0.1px;
                }

                .payroll-table tbody td:nth-child(6),
                .payroll-table tbody td:nth-child(9),
                .payroll-table tbody td:nth-child(10) {
                    text-align: center;
                }

                .payroll-table tbody td:nth-child(14),
                .payroll-table tbody td:nth-child(20),
                .payroll-table tbody td:nth-child(23) {
                    font-weight: 600;
                    color: #1B2340;
                }

            .payroll-table tbody tr:nth-child(even) {
                background-color: #FAFAF9;
            }

            .payroll-table tbody tr:hover {
                background-color: #EEF1FB !important;
            }

        /* Leave / LOP badge pills, applied via JS to specific columns */
        .ledger-badge {
            display: inline-block;
            padding: 2px 10px;
            border-radius: 999px;
            font-size: 11px;
            font-weight: 600;
            font-family: var(--ledger-mono);
        }

        .ledger-badge-success {
            background: var(--ledger-teal-bg);
            color: var(--ledger-teal);
        }

        .ledger-badge-warning {
            background: var(--ledger-amber-bg);
            color: var(--ledger-amber);
        }

        .ledger-badge-danger {
            background: var(--ledger-coral-bg);
            color: var(--ledger-coral);
        }

        .ledger-dim {
            color: #B4B2A9;
        }

        .custom-btn {
            border: 1px solid #ccc !important;
            border-radius: 7px !important;
            padding: 7px 12px !important;
            background: #fff !important;
            white-space: nowrap;
            cursor: pointer;
            text-decoration: none;
            display: inline-block;
            color: #333 !important;
            font-size: 13px;
        }

            .custom-btn:hover {
                background: #f0f0f0 !important;
                text-decoration: none;
                color: #333 !important;
            }

        /* ===== COLUMN MODAL ===== */
        .column-modal {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(27,35,64,0.55);
            display: none;
            align-items: center;
            justify-content: center;
            z-index: 99999;
            overflow-y: auto;
            padding: 40px 20px;
        }

        .column-box {
            background: #fff;
            padding: 25px;
            border-radius: 10px;
            width: 850px;
            max-width: 90%;
            margin: 40px auto;
            max-height: 80vh;
            overflow-y: auto;
        }

        .column-modal-title {
            color: var(--ledger-ink);
            font-weight: 600;
        }

        #columnList {
            display: grid;
            grid-template-columns: repeat(4, 1fr);
            gap: 8px;
            margin-top: 15px;
        }

        .column-item {
            padding: 10px;
            text-align: center;
            border-radius: 6px;
            border: 1px solid #ddd;
            cursor: pointer;
            font-size: 12px;
            background: #fff;
            transition: all 0.15s ease;
        }

            .column-item.active {
                background: var(--ledger-ink);
                color: #fff;
                border-color: var(--ledger-ink);
            }

        .column-actions {
            display: flex;
            justify-content: center;
            gap: 10px;
            margin-top: 20px;
        }

            .column-actions .btn-primary {
                background: #3f51b5 !important;
                border-color: #3f51b5 !important;
                border-radius: 6px !important;
            }

        .ledger-scroll-hint {
            display: none;
        }

        /* ===== DATATABLES FILTER / LENGTH TOOLBAR (compact, evenly spaced) ===== */
        .payroll-table_wrapper .row:first-child,
        .dataTables_wrapper .row:first-child {
            display: flex !important;
            align-items: center;
            justify-content: flex-start !important;
            flex-wrap: wrap;
            gap: 20px;
            margin: 0 !important;
            padding: 12px 16px;
            border-bottom: 1px solid var(--ledger-border);
            background: #fff;
        }

            .payroll-table_wrapper .row:first-child > div,
            .dataTables_wrapper .row:first-child > div {
                flex: 0 0 auto !important;
                width: auto !important;
                max-width: none !important;
                padding: 0 !important;
                text-align: left !important;
            }

        .dataTables_wrapper .dataTables_filter,
        .dataTables_wrapper .dataTables_length {
            margin: 0 !important;
        }

            .dataTables_wrapper .dataTables_filter label,
            .dataTables_wrapper .dataTables_length label {
                display: inline-flex;
                align-items: center;
                gap: 8px;
                margin: 0 !important;
                font-size: 13px;
                font-weight: 500;
                color: var(--ledger-ink);
                white-space: nowrap;
            }

            .dataTables_wrapper .dataTables_filter input,
            .dataTables_wrapper .dataTables_length select {
                border: 1px solid var(--ledger-border) !important;
                border-radius: 6px !important;
                height: 34px !important;
                font-size: 13px !important;
                box-sizing: border-box;
                margin: 0 !important;
            }

            .dataTables_wrapper .dataTables_filter input {
                width: 220px;
                padding: 0 12px !important;
            }

                .dataTables_wrapper .dataTables_filter input:focus {
                    outline: none;
                    border-color: var(--ledger-ink) !important;
                    box-shadow: 0 0 0 2px rgba(59, 68, 114, 0.12);
                }

            .dataTables_wrapper .dataTables_length select {
                width: 68px;
                padding: 0 8px !important;
            }

        /* Info + pagination row: keep tidy too */
        .payroll-table_wrapper .row:last-child,
        .dataTables_wrapper .row:last-child {
            padding: 10px 16px;
            margin: 0 !important;
            font-size: 12px;
            color: var(--ledger-text-secondary);
        }

        @media (max-width: 768px) {
            /* --- Header + filter bar stacks vertically --- */
            .ledger-header {
                flex-direction: column;
                align-items: stretch !important;
                padding: 14px 16px !important;
                gap: 12px;
            }

                .ledger-header .panel-title {
                    font-size: 16px;
                }

            .ledger-controls {
                flex-direction: column;
                align-items: stretch;
                width: 100%;
                gap: 8px;
            }

                .ledger-controls label {
                    margin-top: 2px;
                }

                .ledger-controls select.form-control,
                .ledger-controls .btn-primary,
                .ledger-controls .btn-success,
                .ledger-controls .btn-info {
                    width: 100% !important;
                }

                .ledger-columns-btn {
                    width: 100%;
                    justify-content: center;
                }

            /* --- Filter/length toolbar: stack vertically, full width --- */
            .payroll-table_wrapper .row:first-child,
            .dataTables_wrapper .row:first-child {
                flex-direction: column;
                align-items: stretch;
                gap: 10px;
                padding: 12px;
            }

                .payroll-table_wrapper .row:first-child > div,
                .dataTables_wrapper .row:first-child > div {
                    width: 100% !important;
                }

            .dataTables_wrapper .dataTables_filter input {
                width: 100%;
            }

            .dataTables_wrapper .dataTables_length select {
                width: 100px;
            }

            /* --- KPI cards: tighter padding, smaller value text --- */
            .ledger-kpi-strip {
                padding-left: 12px !important;
                padding-right: 12px !important;
            }

            .ledger-kpi-card {
                padding: 8px 10px;
                gap: 8px;
            }

            .kpi-icon {
                width: 26px;
                height: 26px;
            }

                .kpi-icon svg {
                    width: 13px;
                    height: 13px;
                }

            .ledger-kpi-card .kpi-label {
                font-size: 10px;
            }

            .ledger-kpi-card .kpi-value {
                font-size: 13px;
            }

            /* --- Table: compact + sticky first column so employee name
                   stays visible while swiping horizontally --- */
            .ledger-scroll-hint {
                display: block;
                font-size: 11px;
                color: var(--ledger-text-secondary);
                padding: 8px 16px 0;
            }

            .payroll-table {
                font-size: 11px !important;
            }

                .payroll-table thead th,
                .payroll-table tbody td {
                    padding: 5px 7px !important;
                }

                .payroll-table thead th:nth-child(1),
                .payroll-table tbody td:nth-child(1) {
                    position: sticky;
                    left: 0;
                    z-index: 3;
                }

                .payroll-table thead th:nth-child(1) {
                    z-index: 4;
                }

                .payroll-table tbody td:nth-child(1) {
                    background-color: #fff !important;
                    box-shadow: 2px 0 4px rgba(0,0,0,0.08);
                }

                .payroll-table tbody tr:nth-child(even) td:nth-child(1) {
                    background-color: #FAFAF9 !important;
                }

            /* --- Column customise modal: fewer columns per row --- */
            .column-box {
                padding: 16px;
                width: 100%;
                margin: 16px auto;
                max-height: 85vh;
            }

            #columnList {
                grid-template-columns: repeat(2, 1fr);
            }
        }

        @media (max-width: 480px) {
            .ledger-eyebrow {
                font-size: 10px;
            }

            .ledger-header .panel-title {
                font-size: 15px;
            }

            #columnList {
                grid-template-columns: 1fr;
            }

            .column-actions {
                flex-direction: column;
            }

                .column-actions .btn {
                    width: 100%;
                }
        }
    </style>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel panel-flat">
        <div class="panel-heading ledger-header" id="div_header" runat="server">
            <div>
                <p class="ledger-eyebrow">Admin / Payroll</p>
                <h5 class="panel-title">Payroll Grid &mdash;
                    <asp:Label ID="lbl_monthyear" runat="server" Text=""></asp:Label>
                </h5>
            </div>
            <div class="ledger-controls" id="div_controls" runat="server">
                <label>Month :</label>
                <asp:DropDownList ID="ddl_month" runat="server" CssClass="form-control" Style="width: 130px; display: inline-block;"></asp:DropDownList>

                <label>Year :</label>
                <asp:DropDownList ID="ddl_year" runat="server" CssClass="form-control" Style="width: 90px; display: inline-block;"></asp:DropDownList>

                <asp:Button ID="btn_load" runat="server" Text="Load" CssClass="btn btn-primary btn-sm" OnClick="btn_load_Click" />

                <asp:Button ID="btn_saveall" runat="server" Text="Save All" CssClass="btn btn-success btn-sm"
                    OnClick="btn_saveall_Click" Visible="false"
                    OnClientClick="return confirm('Save payroll for the selected month and year?');" />

                <asp:Button ID="btn_export" runat="server" Text="Export to Excel" CssClass="btn btn-info btn-sm"
                    OnClick="btn_export_Click" Visible="false" />

                <a href="javascript:void(0)" class="ledger-columns-btn" onclick="openColumnModal()">Customize Columns &#9660;
                </a>
            </div>
        </div>

        <!-- KPI SUMMARY : Row 1 - Headcount & Leave -->
        <div class="ledger-kpi-strip" style="padding-bottom: 0;">
            <div class="ledger-kpi-card">
                <div class="kpi-icon kpi-icon-blue">
                    <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                        <path d="M17 21v-2a4 4 0 0 0-4-4H5a4 4 0 0 0-4 4v2"></path><circle cx="9" cy="7" r="4"></circle><path d="M23 21v-2a4 4 0 0 0-3-3.87"></path><path d="M16 3.13a4 4 0 0 1 0 7.75"></path></svg>
                </div>
                <div class="kpi-text">
                    <div class="kpi-label">Employees</div>
                    <div class="kpi-value">
                        <asp:Label ID="lbl_kpi_employees" runat="server" Text="0"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="ledger-kpi-card">
                <div class="kpi-icon kpi-icon-violet">
                    <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                        <rect x="3" y="4" width="18" height="18" rx="2" ry="2"></rect><line x1="16" y1="2" x2="16" y2="6"></line><line x1="8" y1="2" x2="8" y2="6"></line><line x1="3" y1="10" x2="21" y2="10"></line></svg>
                </div>
                <div class="kpi-text">
                    <div class="kpi-label">Total deduction days</div>
                    <div class="kpi-value">
                        <asp:Label ID="lbl_kpi_deddayscount" runat="server" Text="0"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="ledger-kpi-card">
                <div class="kpi-icon kpi-icon-red">
                    <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                        <circle cx="12" cy="12" r="10"></circle><line x1="15" y1="9" x2="9" y2="15"></line><line x1="9" y1="9" x2="15" y2="15"></line></svg>
                </div>
                <div class="kpi-text">
                    <div class="kpi-label">Total LOP days</div>
                    <div class="kpi-value">
                        <asp:Label ID="lbl_kpi_lopdays" runat="server" Text="0"></asp:Label>
                    </div>
                </div>
            </div>
            <div class="ledger-kpi-card">
                <div class="kpi-icon kpi-icon-amber">
                    <svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                        <path d="M10.29 3.86L1.82 18a2 2 0 0 0 1.71 3h16.94a2 2 0 0 0 1.71-3L13.71 3.86a2 2 0 0 0-3.42 0z"></path><line x1="12" y1="9" x2="12" y2="13"></line><line x1="12" y1="17" x2="12.01" y2="17"></line></svg>
                </div>
                <div class="kpi-text">
                    <div class="kpi-label">Uninformed leave</div>
                    <div class="kpi-value">
                        <asp:Label ID="lbl_kpi_uninformed" runat="server" Text="0"></asp:Label>
                    </div>
                </div>
            </div>
        </div>

        <!-- Grid -->
        <div class="panel-body" style="padding: 0;"></div>
        <div class="ledger-scroll-hint">&#8592; Swipe left/right to see all columns &#8594;</div>
        <div style="overflow-x: auto;">
            <table class="table datatable-basic table-bordered payroll-table">
                <thead>
                    <tr>
                        <th>Employee Name</th>
                        <th>Employee ID</th>
                        <th>Days In Month</th>
                        <th>Working Days</th>
                        <th>Paid Holidays</th>
                        <th>Informed Leave</th>
                        <th>Leave Days In Year</th>
                        <th>Month Leave Days</th>
                        <th>LOP Leave Days</th>
                        <th>Uninformed Leave</th>
                        <th>Half Day Count</th>
                        <th>Half Day Deduction</th>
                        <th>Full Day Deduction</th>
                        <th>Late Login Count</th>
                        <th>OutTime Missing Count</th>
                        <th>Total Deduction</th>
                        <th>Monthly Salary</th>
                        <th>Per Day Salary</th>
                        <th>Leave Days Salary</th>
                        <th>Eligible Days</th>
                        <th>Eligible Amount</th>
                        <th>Net Pay</th>
                        <th>Annual CTC</th>
                        <th>InTime Days</th>
                        <th>Final Net Pay</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:PlaceHolder ID="PH_payslip" runat="server"></asp:PlaceHolder>
                </tbody>
            </table>
        </div>

        <div class="ledger-total-netpay" style="text-align: right; padding: 12px 16px; font-size: 14px;">
            <span style="color: #666; margin-right: 8px;">Total Net Pay:</span>
            <span style="font-weight: 700; font-size: 16px; color: #1a7f37;">
                &#8377; <asp:Label ID="lbl_total_netpay" runat="server" Text="0.00"></asp:Label>
            </span>
        </div>
    </div>

    <!-- COLUMN MODAL -->
    <div id="columnModal" class="column-modal" style="display: none;">
        <div class="column-box">
            <h5 class="column-modal-title" style="text-align: center; margin-top: 0;">Customise your table columns</h5>
            <p class="column-modal-subtitle" style="text-align: center; font-size: 13px; color: #666;">Highlight Column Headers in Blue to View.</p>
            <div id="columnList"></div>
            <div class="column-actions">
                <button type="button" onclick="saveColumnSettings()" class="btn btn-primary">Save</button>
                <button type="button" onclick="closeColumnModal()" class="btn btn-secondary">Close</button>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        var storageKey = "payslipColumns";

        // ===== Column index map (0-based) matching the 23 headers above =====
        var COL_INFORMED_LEAVE = 5;
        var COL_LOP_DAYS = 8;
        var COL_UNINFORMED_LEAVE = 9;
        var COL_FINAL_NET_PAY = 22;

        function openColumnModal() {
            var table = $('.payroll-table').DataTable();
            if (!table) return;

            $("#columnList").html("");
            table.columns().every(function (index) {
                var header = $(this.header()).text().trim();
                if (header !== "") {
                    var visible = this.visible();
                    $("#columnList").append(
                        '<div class="column-item ' + (visible ? 'active' : '') + '" data-column="' + index + '">' +
                        header + '</div>'
                    );
                }
            });
            $("#columnModal").css("display", "flex");
        }

        function closeColumnModal() {
            $("#columnModal").hide();
        }

        function saveColumnSettings() {
            var table = $('.payroll-table').DataTable();
            var columns = [];
            table.columns().every(function () {
                columns.push(this.visible());
            });
            localStorage.setItem(storageKey, JSON.stringify(columns));
            $("#columnModal").fadeOut();
        }

        $(document).on("click", ".column-item", function () {
            var table = $('.payroll-table').DataTable();
            var col = table.column($(this).data("column"));
            col.visible(!col.visible());
            $(this).toggleClass("active");
        });

        // ===== Visual enhancement layer: badges + KPI strip (display only, no data changes) =====
        function ledgerParseNumber(text) {
            var n = parseFloat((text || "0").toString().replace(/,/g, ""));
            return isNaN(n) ? 0 : n;
        }

        function ledgerWrapBadge($td, cssClass) {
            if ($td.length === 0 || $td.hasClass("ledger-wrapped")) return;
            var raw = $td.text().trim();
            var val = ledgerParseNumber(raw);
            $td.addClass("ledger-wrapped");
            if (val > 0) {
                $td.html('<span class="ledger-badge ' + cssClass + '">' + raw + '</span>');
            } else {
                $td.html('<span class="ledger-dim">&mdash;</span>');
            }
        }

        function ledgerApplyBadges() {
            $(".payroll-table tbody tr").each(function () {
                var $tds = $(this).find("td");
                ledgerWrapBadge($tds.eq(COL_INFORMED_LEAVE), "ledger-badge-success");
                ledgerWrapBadge($tds.eq(COL_LOP_DAYS), "ledger-badge-danger");
                ledgerWrapBadge($tds.eq(COL_UNINFORMED_LEAVE), "ledger-badge-warning");
            });
        }

        function ledgerEnhance() {
            ledgerApplyBadges();
        }

        $(document).ready(function () {
            setTimeout(function () {
                var table = $('.payroll-table').DataTable();
                if (table) {
                    var savedColumns = localStorage.getItem(storageKey);
                    if (savedColumns) {
                        try {
                            JSON.parse(savedColumns).forEach(function (visible, index) {
                                table.column(index).visible(visible);
                            });
                        } catch (e) { }
                    }

                    ledgerEnhance();
                    table.on('draw.dt', function () {
                        ledgerApplyBadges();
                    });
                }
            }, 500);
        });
    </script>
</asp:Content>
