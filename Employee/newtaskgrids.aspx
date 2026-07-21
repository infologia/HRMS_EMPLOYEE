<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="newtaskgrids.aspx.cs" Inherits="Employee_taskgrids" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function confirmDelete(taskKey) {
            if (confirm('Are you sure you want to remove this task?')) {
                window.location.href = 'newtaskgrids.aspx?action=delete&taskkey=' + taskKey;
            }
        }

    </script>
    <style>
        /* Hide DataTables sorting icons while keeping functionality */
        table.dataTable thead .sorting:after,
        table.dataTable thead .sorting_asc:after,
        table.dataTable thead .sorting_desc:after,
        table.dataTable thead .sorting:before,
        table.dataTable thead .sorting_asc:before,
        table.dataTable thead .sorting_desc:before {
            display: none !important;
            content: "" !important;
        }

        /* btn-xs styles handled by .action-cell .btn */
        .cards-row {
            display: flex;
            flex-wrap: wrap;
            margin-left: -10px;
            margin-right: -10px;
        }

        .card-col {
            padding-left: 10px;
            padding-right: 10px;
            margin-bottom: 10px;
            flex: 1 1 0%;
            min-width: 120px;
        }

        .card-col a {
            display: block;
            text-decoration: none !important;
            height: 100%;
        }

        /* ── Dashboard Cards — horizontal icon-left style ── */
        .dashboard-panel {
            display: flex;
            align-items: center;
            gap: 14px;
            background: #fff;
            border-radius: 12px;
            border: 1.5px solid #e5e7eb;
            padding: 14px 16px;
            cursor: pointer;
            transition: all 0.2s ease;
            height: 100%;
            box-shadow: 0 1px 4px rgba(0,0,0,0.05);
        }

        .dashboard-panel:hover {
            transform: translateY(-2px);
            box-shadow: 0 4px 12px rgba(0,0,0,0.10);
        }

        .dashboard-panel.active {
            border-color: var(--accent-color);
            box-shadow: 0 0 0 2px var(--accent-color);
        }

        /* ── Icon Box ── */
        .card-icon-box {
            width: 46px;
            height: 46px;
            border-radius: 10px;
            display: flex;
            align-items: center;
            justify-content: center;
            flex-shrink: 0;
            font-size: 20px;
        }

        /* ── Card Text ── */
        .card-info {
            display: flex;
            flex-direction: column;
            gap: 2px;
        }

        .card-label {
            font-size: 12px;
            color: #6b7280;
            font-weight: 500;
            white-space: nowrap;
        }

        .card-count {
            font-size: 24px;
            font-weight: 700;
            color: #111827;
            line-height: 1.1;
        }

        /* ── Per-card accent + icon colors ── */
        .card-all-tasks    { --accent-color: #455a64; }
        .card-yet-to-start { --accent-color: #7e57c2; }
        .card-in-progress  { --accent-color: #1565c0; }
        .card-overdue      { --accent-color: #c62828; }
        .card-completed    { --accent-color: #2e7d32; }

        .icon-all-tasks    { background: #eceff1; color: #455a64; }
        .icon-yet-to-start { background: #ede7f6; color: #7e57c2; }
        .icon-in-progress  { background: #e3f2fd; color: #1565c0; }
        .icon-overdue      { background: #ffebee; color: #c62828; }
        .icon-completed    { background: #e8f5e9; color: #2e7d32; }

        .card-overdue { animation: pulse-glow 2s infinite; }

        @keyframes pulse-glow {
            0%, 100% { box-shadow: 0 1px 4px rgba(198,40,40,0.15); }
            50%       { box-shadow: 0 4px 14px rgba(198,40,40,0.35); }
        }

        /* ── Panel Heading & Filters ── */
        .panel-heading {
            display: flex;
            justify-content: space-between;
            align-items: center;
            flex-wrap: wrap;
            gap: 10px;
            padding: 12px 16px;
            background: #fff;
            border-bottom: 1px solid #e5e7eb;
        }

        .panel-heading h5 {
            margin: 0;
            font-size: 15px;
            font-weight: 600;
        }

        .date-filter {
            display: flex;
            align-items: center;
            gap: 10px;
            flex-wrap: wrap;
        }

        .employee-filter-wrap {
            display: inline-flex;
            align-items: center;
            gap: 8px;
            padding-right: 20px;
            border-right: 2px solid #e5e7eb;
        }

        .employee-dropdown {
            width: 200px;
            height: 32px;
            padding: 2px 6px;
            font-size: 13px;
        }

        .date-label, .year-label {
            font-size: 13px;
            margin: 0;
            white-space: nowrap;
            font-weight: 500;
        }

        .date-dropdown {
            width: 120px;
            height: 32px;
            padding: 2px 6px;
            font-size: 13px;
        }

        /* ── Table card with curved edges ── */
        .panel.panel-flat {
            border-radius: 12px !important;
            border: 1px solid #e5e7eb !important;
            overflow: hidden;
            box-shadow: 0 2px 8px rgba(0,0,0,0.06);
        }

        /* ── Table ── */
        .table-responsive { border-radius: 0 0 12px 12px; overflow: hidden; }

        .table {
            border-collapse: collapse !important;
            width: 100% !important;
            border-radius: 0 0 12px 12px;
            overflow: hidden;
        }

        /* First and last header cell rounded corners */
        .table thead th:first-child { border-radius: 0; }
        .table thead th:last-child  { border-radius: 0; }
        /* Last row bottom corners */
        .table tbody tr:last-child td:first-child { border-radius: 0 0 0 12px; }
        .table tbody tr:last-child td:last-child  { border-radius: 0 0 12px 0; }

        .table thead th {
            border: none !important;
            border-bottom: 1px solid #e5e7eb !important;
            background: #f8f9fa;
            font-size: 10.5px !important;
            font-weight: 700;
            color: #6b7280;
            padding: 7px 8px !important;
            white-space: nowrap;
            text-transform: uppercase;
            letter-spacing: 0.4px;
        }
        .table tbody td {
            border: none !important;
            border-bottom: 1px solid #f0f0f0 !important;
            font-size: 12.5px !important;
            padding: 7px 8px !important;
            vertical-align: middle;
        }
        .table tbody tr:hover td { background: #fafbfc; }
        .table .text-center { text-align: center; }

        /* ── Compact Actions cell ── */
        .action-cell {
            text-align: center;
            white-space: nowrap;
        }
        .action-cell .btn {
            width: 28px !important;
            height: 28px !important;
            padding: 0 !important;
            border-radius: 6px !important;
            font-size: 12px !important;
            line-height: 28px !important;
            display: inline-flex !important;
            align-items: center;
            justify-content: center;
            margin: 0 2px !important;
        }
        .action-cell .btn i { font-size: 11px !important; }

        /* ── Assignee cell (avatar + name) ── */
        .assignee-cell {
            display: inline-flex;
            align-items: center;
            gap: 8px;
            white-space: nowrap;
        }
        .assignee-avatar {
            width: 30px;
            height: 30px;
            border-radius: 50%;
            background: #e8f2fd;
            color: #1565c0;
            font-size: 10px;
            font-weight: 700;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            flex-shrink: 0;
            overflow: hidden;
            border: 2px solid #fff;
            box-shadow: 0 1px 4px rgba(0,0,0,0.12);
        }
        .assignee-avatar img {
            width: 100%;
            height: 100%;
            object-fit: cover;
            border-radius: 50%;
            display: block;
        }

        /* ── Count badges (status pills) ── */
        .count-badge {
            display: inline-block;
            min-width: 22px;
            padding: 2px 9px;
            border-radius: 20px;
            font-size: 11px;
            font-weight: 600;
        }
        .badge-purple { background: #f1edfb; color: #6a3fb5; }
        .badge-blue   { background: #e8f2fd; color: #1565c0; }
        .badge-green  { background: #ecf6ed; color: #2e7d32; }
        .badge-red    { background: #fdeceb; color: #c62828; }

        /* ── Project Header ── */
        .project-header-wrap {
            display: flex;
            align-items: center;
            justify-content: space-between;
            flex-wrap: wrap;
            gap: 10px;
        }

        .project-header-wrap h2 {
            margin: 0;
            color: #333;
            font-weight: 600;
            flex: 1;
        }

        .project-header-btns {
            display: flex;
            gap: 10px;
            flex-wrap: wrap;
        }

        @media (max-width: 1200px) {
            .card-col { flex: 0 0 50%; }
        }
     
        @media (max-width: 767px) {

            /* Cards — 2 per row on mobile */
            .card-col { flex: 0 0 50%; }

            .dashboard-panel           { padding: 10px 12px; gap: 10px; }
            .card-icon-box             { width: 38px; height: 38px; font-size: 16px; }
            .card-count                { font-size: 20px; }

            /* Project header stacks */
            .project-header-wrap { flex-direction: column; align-items: flex-start; }
            .project-header-wrap h2 { font-size: 16px !important; padding-right: 0 !important; }
            .project-header-btns { width: 100%; }
            .project-header-btns .btn { flex: 1; text-align: center; font-size: 13px; padding: 8px 10px; }

            /* Panel heading stacks */
            .panel-heading { flex-direction: column; align-items: flex-start; }

            .date-filter {
                flex-direction: column;
                align-items: stretch;
                gap: 8px;
                width: 100%;
            }

            /* Each filter pair: label left, dropdown right */
            .filter-pair {
                display: flex;
                align-items: center;
                gap: 10px;
                width: 100%;
            }

            .filter-pair label {
                min-width: 54px;
                font-size: 13px;
                font-weight: 500;
                margin: 0;
            }

            .filter-pair .form-control {
                flex: 1;
                height: 36px !important;
                font-size: 13px !important;
            }

            .employee-filter-wrap {
                border-right: none !important;
                padding-right: 0 !important;
                border-bottom: 2px solid #e5e7eb;
                padding-bottom: 10px;
                width: 100%;
                flex-direction: column;
                align-items: stretch;
            }

            .employee-dropdown,
            .date-dropdown { width: 100% !important; height: 36px !important; }

            /* Scrollable table */
            .table-responsive { overflow-x: auto; -webkit-overflow-scrolling: touch; }
            .table             { min-width: 800px; font-size: 12px; }
            .table thead th    { padding: 8px 5px !important; font-size: 11px; white-space: nowrap; }
            .table tbody td    { padding: 8px 5px !important; font-size: 11px; }
            .table .btn        { padding: 4px 7px !important; font-size: 10px !important; }
        }

        @media (max-width: 480px) {
            /* Cards — 1 per row on very small screens */
            .card-col { flex: 0 0 100%; }

            .card-count { font-size: 18px; }
            .card-label { font-size: 11px; }

            .project-header-wrap h2 { font-size: 14px !important; }

            .table            { font-size: 10px; min-width: 700px; }
            .table thead th   { font-size: 10px; padding: 6px 3px !important; }
            .table tbody td   { font-size: 10px; padding: 6px 3px !important; }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="hfActiveStatus" runat="server" Value="1" />
    
    <!-- Project Header -->
    <div class="panel panel-flat" style="margin-bottom: 20px;">
        <div class="panel-body" style="padding: 16px 20px;">
            <div class="project-header-wrap">
                <h2>
                    <i class="glyphicon glyphicon-folder-open" style="margin-right: 10px; color: #2196f3;"></i>
                    <asp:Label ID="lbl_ProjectHeader" runat="server" Text="All Projects"></asp:Label>
                </h2>
                <div class="project-header-btns">
                    <asp:HyperLink ID="lnk_BackButton" runat="server" NavigateUrl="assignedprojectnew.aspx"
                        CssClass="btn btn-default"
                        style="border-radius: 6px; font-weight: 600;">
                        <i class="glyphicon glyphicon-arrow-left" style="margin-right: 6px;"></i>Back
                    </asp:HyperLink>
                    <asp:HyperLink ID="lnk_CreateTask" runat="server" NavigateUrl="createtasknew.aspx"
                        CssClass="btn btn-primary"
                        style="border-radius: 6px; font-weight: 600;">
                        <i class="glyphicon glyphicon-plus" style="margin-right: 8px;"></i>Create Task
                    </asp:HyperLink>
                </div>
            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="cards-row">
                <div class="card-col">
                    <asp:LinkButton ID="btnCard0" runat="server" OnClick="CardClick" CommandArgument="0" style="text-decoration: none; display:block;">
                        <div class="dashboard-panel card-all-tasks <%= hfActiveStatus.Value == "0" ? "active" : "" %>">
                            <div class="card-icon-box icon-all-tasks">
                                <i class="glyphicon glyphicon-th-list"></i>
                            </div>
                            <div class="card-info">
                                <span class="card-label">Total Tasks</span>
                                <span class="card-count"><asp:Label ID="lbl_AllTaskCount" runat="server">0</asp:Label></span>
                            </div>
                        </div>
                    </asp:LinkButton>
                </div>

                <div class="card-col">
                    <asp:LinkButton ID="btnCard1" runat="server" OnClick="CardClick" CommandArgument="1" style="text-decoration: none; display:block;">
                        <div class="dashboard-panel card-yet-to-start <%= hfActiveStatus.Value == "1" ? "active" : "" %>">
                            <div class="card-icon-box icon-yet-to-start">
                                <i class="glyphicon glyphicon-time"></i>
                            </div>
                            <div class="card-info">
                                <span class="card-label">Yet to Start</span>
                                <span class="card-count"><asp:Label ID="lbl_YetToStartCount" runat="server">0</asp:Label></span>
                            </div>
                        </div>
                    </asp:LinkButton>
                </div>

                <div class="card-col">
                    <asp:LinkButton ID="btnCard2" runat="server" OnClick="CardClick" CommandArgument="2" style="text-decoration: none; display:block;">
                        <div class="dashboard-panel card-in-progress <%= hfActiveStatus.Value == "2" ? "active" : "" %>">
                            <div class="card-icon-box icon-in-progress">
                                <i class="glyphicon glyphicon-refresh"></i>
                            </div>
                            <div class="card-info">
                                <span class="card-label">In Progress</span>
                                <span class="card-count"><asp:Label ID="lbl_InProgressCount" runat="server">0</asp:Label></span>
                            </div>
                        </div>
                    </asp:LinkButton>
                </div>

                <div class="card-col">
                    <asp:LinkButton ID="btnCard3" runat="server" OnClick="CardClick" CommandArgument="3" style="text-decoration: none; display:block;">
                        <div class="dashboard-panel card-overdue <%= hfActiveStatus.Value == "3" ? "active" : "" %>">
                            <div class="card-icon-box icon-overdue">
                                <i class="glyphicon glyphicon-warning-sign"></i>
                            </div>
                            <div class="card-info">
                                <span class="card-label">Overdue</span>
                                <span class="card-count"><asp:Label ID="lbl_OverDueCount" runat="server">0</asp:Label></span>
                            </div>
                        </div>
                    </asp:LinkButton>
                </div>

                <div class="card-col">
                    <asp:LinkButton ID="btnCard4" runat="server" OnClick="CardClick" CommandArgument="4" style="text-decoration: none; display:block;">
                        <div class="dashboard-panel card-completed <%= hfActiveStatus.Value == "4" ? "active" : "" %>">
                            <div class="card-icon-box icon-completed">
                                <i class="glyphicon glyphicon-ok-circle"></i>
                            </div>
                            <div class="card-info">
                                <span class="card-label">Completed</span>
                                <span class="card-count"><asp:Label ID="lbl_CompletedCount" runat="server">0</asp:Label></span>
                            </div>
                        </div>
                    </asp:LinkButton>
                </div>

            </div>

            <div class="panel panel-flat">
                <div class="panel-heading">
                    <h5 class="panel-title">
                        <%= GetStatusTitle() %>
                    </h5>
                    <div class="date-filter">
                        <div id="divEmployeeFilter" runat="server" visible="false" class="employee-filter-wrap">
                            <div class="filter-pair">
                                <label class="date-label">Employee:</label>
                                <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-control employee-dropdown" AutoPostBack="true" OnSelectedIndexChanged="Filter_Changed"></asp:DropDownList>
                            </div>
                        </div>
                        <div class="filter-pair">
                            <label class="date-label">Month :</label>
                            <asp:DropDownList ID="ddlDate" runat="server" CssClass="form-control date-dropdown" AutoPostBack="true" OnSelectedIndexChanged="Filter_Changed"></asp:DropDownList>
                        </div>
                        <div class="filter-pair">
                            <label class="year-label">Year :</label>
                            <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control date-dropdown" AutoPostBack="true" OnSelectedIndexChanged="Filter_Changed"></asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="table-responsive" style="max-height: 600px; overflow-y: auto;">
                    <table class="table datatable-basic">
                        <thead>
                            <tr>
                                <th style="min-width: 100px;">Work Day</th>
                                <th>Assigned To</th>
                                <th>Assigned Hours</th>
                                <th>Actual Hours</th>
                                <th>Task count</th>
                                <th>Yet to start</th>
                                <th>In Progress</th>
                                <th>Completed</th>
                                <th>Overdue</th>
                                <th style="min-width: 90px;">Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:PlaceHolder ID="PH_Tasks" runat="server"></asp:PlaceHolder>
                        </tbody>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>


  <script>
      function initDataTable() {
          console.log("initDataTable called, rows:", $('.datatable-basic tbody tr').length);
          if ($.fn.DataTable.isDataTable('.datatable-basic')) {
              $('.datatable-basic').DataTable().destroy();
          }
          $('.datatable-basic').DataTable({
              paging: true,
              searching: true,
              ordering: true
          });
      }

      $(document).ready(function () {
          initDataTable();

          var prm = Sys.WebForms.PageRequestManager.getInstance();
          prm.add_endRequest(function () {
              initDataTable();
          });
      });
  </script>
</asp:Content>

