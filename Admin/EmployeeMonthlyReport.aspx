<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="EmployeeMonthlyReport.aspx.cs" Inherits="Admin_EmployeeMonthlyReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        /* ── Wrapper ── */
        .ps-wrap { background:#fff; border-radius:8px; box-shadow:0 1px 8px rgba(0,0,0,.1); overflow:hidden; }

        /* ── Header & Filter Bar ── */
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
        
        .filter-panel { 
            background: #f7faff; 
            padding: 12px 20px; 
            border-bottom: 1px solid #e8edf3; 
            display: flex;
            align-items: center;
            gap: 10px;
            flex-wrap: wrap;
        }
        .filter-panel select, .filter-panel input {
            font-size: 12px; height: 30px; padding: 4px 8px; border: 1px solid #cbd5e1; border-radius: 4px;
        }
        .btn-sm-custom {
            font-size:12px; padding: 5px 15px; border-radius: 4px; font-weight:600; cursor:pointer;
        }
        .btn-blue { background: #2d6a9f; color: #fff; border: 1px solid #2d6a9f; }
        .btn-blue:hover { background: #1e4a73; }
        .btn-default-custom { background: #fff; color: #333; border: 1px solid #ccc; }

        /* ── 6 Cards in one row ── */
        .ps-cards {
            display: flex;
            flex-wrap: nowrap;
            gap: 0;
            border-bottom: 1px solid #e8edf3;
        }
        .ps-card {
            flex: 1;
            padding: 14px 16px;
            border-left: 1px solid #e8edf3;
            position: relative;
        }
        .ps-card:first-child { border-left: none; }
        .ps-card .card-icon { font-size: 22px; margin-bottom: 6px; }
        .ps-card .card-label {
            font-size: 10px; color: #999;
            text-transform: uppercase; letter-spacing: .05em; margin-bottom: 4px;
        }
        .ps-card .card-value { font-size: 20px; font-weight: 700; color: #1e3a5f; line-height: 1; }
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

        /* ── Employee Profile Card ── */
        .emp-info { 
            display: flex; align-items: center; padding: 14px 20px; 
            border-bottom: 1px solid #e8edf3; background: #fff;
        }
        .emp-info img { width: 45px; height: 45px; border-radius: 50%; margin-right: 15px; object-fit: cover; border: 1px solid #dee2e6; }
        .emp-details { display: grid; grid-template-columns: repeat(auto-fit, minmax(150px, 1fr)); gap: 15px; width: 100%; }
        .emp-details div { display: flex; flex-direction: column; }
        .emp-details span { font-size: 10px; color: #999; text-transform: uppercase; }
        .emp-details strong { font-size: 12px; color: #1e3a5f; }

        /* ── Grid Container ── */
        .ps-att-wrap { padding: 15px 20px; }

        /* ── Compact Table ── */
        .ps-att-table {
            width: 100%; border-collapse: collapse;
            font-size: 12px; margin-top: 10px;
        }
        .ps-att-table thead tr { background: #f0f5ff; }
        .ps-att-table thead th {
            padding: 8px 12px; color: #1e3a5f; border: 1px solid #e3eaf3;
            font-weight: 600; text-align: left;
        }
        .ps-att-table tbody tr:nth-child(even) { background: #f7faff; }
        .ps-att-table tbody tr:hover { background: #edf3fb; }
        .ps-att-table tbody td {
            padding: 7px 12px; border: 1px solid #e3eaf3;
            color: #333;
        }
        
        .badge-score { font-weight: bold; padding: 2px 6px; border-radius: 3px; font-size: 11px; }
        .badge-excellent { background-color: #d4edda; color: #155724; }
        .badge-good { background-color: #fff3cd; color: #856404; }
        .badge-poor { background-color: #f8d7da; color: #721c24; }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div class="ps-wrap">

    <%-- ── Header ── --%>
    <div class="ps-top-bar">
        <div>
            <p class="ps-title">Employee Monthly Performance Report</p>
            <p class="ps-sub">Performance Analytics & Log Overview</p>
        </div>
    </div>

    <%-- ── Filters ── --%>
    <div class="filter-panel">
        <asp:DropDownList ID="ddlEmployee" runat="server"></asp:DropDownList>
        <asp:DropDownList ID="ddlMonth" runat="server">
            <asp:ListItem Value="0">All Months</asp:ListItem>
            <asp:ListItem Value="1">January</asp:ListItem>
            <asp:ListItem Value="2">February</asp:ListItem>
            <asp:ListItem Value="3">March</asp:ListItem>
            <asp:ListItem Value="4">April</asp:ListItem>
            <asp:ListItem Value="5">May</asp:ListItem>
            <asp:ListItem Value="6">June</asp:ListItem>
            <asp:ListItem Value="7">July</asp:ListItem>
            <asp:ListItem Value="8">August</asp:ListItem>
            <asp:ListItem Value="9">September</asp:ListItem>
            <asp:ListItem Value="10">October</asp:ListItem>
            <asp:ListItem Value="11">November</asp:ListItem>
            <asp:ListItem Value="12">December</asp:ListItem>
        </asp:DropDownList>
        <asp:DropDownList ID="ddlYear" runat="server">
            <asp:ListItem Value="2023">2023</asp:ListItem>
            <asp:ListItem Value="2024">2024</asp:ListItem>
            <asp:ListItem Value="2025">2025</asp:ListItem>
            <asp:ListItem Value="2026">2026</asp:ListItem>
        </asp:DropDownList>
        <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn-sm-custom btn-blue" OnClick="btnSearch_Click" />
        <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn-sm-custom btn-default-custom" />
    </div>

    <%-- ── Employee Info Panel ── --%>
    <div class="emp-info" id="divEmpInfo" runat="server" visible="false">
        <asp:Image ID="imgProfile" runat="server" ImageUrl="~/Images/nopicture.jpg" />
        <div class="emp-details">
            <div><span>Name</span><strong><asp:Label ID="lblName" runat="server"></asp:Label></strong></div>
            <div><span>Emp ID</span><strong><asp:Label ID="lblEmpId" runat="server"></asp:Label></strong></div>
            <div><span>Designation</span><strong><asp:Label ID="lblDept" runat="server"></asp:Label></strong></div>
            <div><span>Department</span><strong><asp:Label ID="lblDesignation" runat="server"></asp:Label></strong></div>
            <div><span>Report Month</span><strong><asp:Label ID="lblReportMonth" runat="server"></asp:Label></strong></div>
        </div>
    </div>

    <%-- ── Summary KPI Cards ── --%>
    <div class="ps-cards" id="divKpiCards" runat="server" visible="false">
        <div class="ps-card">
            <div class="card-icon ic-blue"><i class="icon-calendar"></i></div>
            <div class="card-label">Working Days</div>
            <div class="card-value"><asp:Label ID="lblWorkingDaysKpi" runat="server" Text="22"></asp:Label></div>
            <div class="card-sub">Eligible days</div>
        </div>
        <div class="ps-card">
            <div class="card-icon ic-green"><i class="icon-checkmark-circle"></i></div>
            <div class="card-label">Tasks Completed</div>
            <div class="card-value"><asp:Label ID="lblCompletedTasksKpi" runat="server" Text="-"></asp:Label></div>
            <div class="card-sub">Out of total tasks</div>
        </div>
        <div class="ps-card">
            <div class="card-icon ic-teal"><i class="icon-history"></i></div>
            <div class="card-label">Present Days</div>
            <div class="card-value"><asp:Label ID="lblPresentDaysKpi" runat="server" Text="-"></asp:Label></div>
            <div class="card-sub">Attendance log count</div>
        </div>
        <div class="ps-card">
            <div class="card-icon ic-orange"><i class="icon-warning2"></i></div>
            <div class="card-label">Leaves Taken</div>
            <div class="card-value"><asp:Label ID="lblLeavesKpi" runat="server" Text="-"></asp:Label></div>
            <div class="card-sub">Leaves in month</div>
        </div>
        <div class="ps-card">
            <div class="card-icon ic-teal"><i class="icon-clock"></i></div>
            <div class="card-label">Permission Taken</div>
            <div class="card-value"><asp:Label ID="lblPermissionKpi" runat="server" Text="-"></asp:Label></div>
            <div class="card-sub">Permission hours</div>
        </div>
        <div class="ps-card">
            <div class="card-icon ic-red"><i class="icon-alarm"></i></div>
            <div class="card-label">Late Logins</div>
            <div class="card-value"><asp:Label ID="lblLateKpi" runat="server" Text="0"></asp:Label></div>
            <div class="card-sub">Time exceptions</div>
        </div>
        <div class="ps-card">
            <div class="card-icon ic-purple"><i class="icon-stats-growth"></i></div>
            <div class="card-label">Performance Score</div>
            <div class="card-value"><asp:Label ID="lblPerformanceScoreKpi" runat="server" Text="-%"></asp:Label></div>
            <div class="card-sub">Overall Rating</div>
        </div>
    </div>

    <%-- ── Dynamic Performance Grid ── --%>
    <div class="ps-sect">
        <asp:Label ID="lblGridSectionTitle" runat="server" Text="Monthly Performance Overview"></asp:Label>
    </div>
    <div class="ps-att-wrap" id="divReportContent" runat="server" visible="false">
        <!-- Consolidated Grid (Visible when ALL employees are shown) -->
        <asp:GridView ID="gvPerformanceReport" runat="server" CssClass="ps-att-table" AutoGenerateColumns="false" GridLines="None" OnRowDataBound="gvPerformanceReport_RowDataBound">
            <Columns>
                <asp:BoundField DataField="EmployeeName" HeaderText="Employee Name" />
                <asp:BoundField DataField="WorkingDays" HeaderText="Working Days" />
                <asp:BoundField DataField="PresentDays" HeaderText="Present Days" />
                <asp:BoundField DataField="LeavesTaken" HeaderText="Leaves Taken" />
                <asp:BoundField DataField="PermissionHours" HeaderText="Permission Hours" DataFormatString="{0:0.##} hrs" />
                <asp:BoundField DataField="TotalTasks" HeaderText="Total Tasks" />
                <asp:BoundField DataField="CompletedTasks" HeaderText="Completed Tasks" />
                <asp:BoundField DataField="TaskCompletionRate" HeaderText="Task Rate (%)" DataFormatString="{0:0.00}%" />
                <asp:TemplateField HeaderText="Performance Score">
                    <ItemTemplate>
                        <asp:Label ID="lblScore" runat="server" CssClass="badge-score" Text='<%# Eval("OverallScore", "{0:0.00}") + "%" %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        <!-- Charts Panel (Visible when a SPECIFIC employee is selected) -->
        <asp:Panel ID="pnlCharts" runat="server" Visible="false">
            <div style="display: flex; gap: 20px; flex-wrap: wrap; margin-top: 15px;">
                <div style="flex: 1; min-width: 280px; border: 1px solid #e8edf3; padding: 15px; border-radius: 6px; background: #fff; display: flex; flex-direction: column; align-items: center;">
                    <strong style="font-size: 13px; color: #1e3a5f; margin-bottom: 10px;">Task Status Breakdown</strong>
                    <div style="width: 220px; height: 220px;">
                        <canvas id="taskStatusChart"></canvas>
                    </div>
                </div>
                <div style="flex: 1.5; min-width: 320px; border: 1px solid #e8edf3; padding: 15px; border-radius: 6px; background: #fff; display: flex; flex-direction: column; align-items: center;">
                    <strong style="font-size: 13px; color: #1e3a5f; margin-bottom: 10px;">Tasks by Project</strong>
                    <div style="width: 100%; height: 220px;">
                        <canvas id="projectTasksChart"></canvas>
                    </div>
                </div>
            </div>
            <asp:HiddenField ID="hfTaskStatusValues" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hfProjectLabels" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hfProjectValues" runat="server" ClientIDMode="Static" />
        </asp:Panel>
    </div>

    <!-- Chart.js Libraries & Script -->
    <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
    <script type="text/javascript">
        var statusChart, projectChart;
        function renderCharts() {
            // 1. Task Status Chart (Donut)
            var statusVals = document.getElementById('hfTaskStatusValues').value.split(',');
            if (statusVals.length >= 2 && (parseInt(statusVals[0]) > 0 || parseInt(statusVals[1]) > 0)) {
                var ctx1 = document.getElementById('taskStatusChart').getContext('2d');
                if (statusChart) statusChart.destroy();
                statusChart = new Chart(ctx1, {
                    type: 'doughnut',
                    data: {
                        labels: ['Completed', 'Pending'],
                        datasets: [{
                            data: [parseInt(statusVals[0]), parseInt(statusVals[1])],
                            backgroundColor: ['#1a9e4d', '#e07b00'],
                            borderWidth: 1
                        }]
                    },
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        plugins: { legend: { position: 'bottom', labels: { boxWidth: 12, font: { size: 10 } } } }
                    }
                });
            }

            // 2. Projects Chart (Bar)
            var projLabelsVal = document.getElementById('hfProjectLabels').value;
            var projValsVal = document.getElementById('hfProjectValues').value;
            if (projLabelsVal && projValsVal) {
                var projLabels = projLabelsVal.split(',');
                var projVals = projValsVal.split(',').map(Number);
                var ctx2 = document.getElementById('projectTasksChart').getContext('2d');
                if (projectChart) projectChart.destroy();
                projectChart = new Chart(ctx2, {
                    type: 'bar',
                    data: {
                        labels: projLabels,
                        datasets: [{
                            label: 'Total Tasks',
                            data: projVals,
                            backgroundColor: '#2d6a9f',
                            borderWidth: 1
                        }]
                    },
                    options: {
                        responsive: true,
                        maintainAspectRatio: false,
                        plugins: { legend: { display: false } },
                        scales: {
                            y: { beginAtZero: true, ticks: { font: { size: 9 } } },
                            x: { ticks: { font: { size: 9 } } }
                        }
                    }
                });
            }
        }
        
        // Handle postback or load
        window.onload = function() {
            renderCharts();
        };
        
        // If Sys WebForms PageRequestManager is present (UpdatePanel support)
        if (typeof(Sys) !== 'undefined') {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function() {
                renderCharts();
            });
        }
    </script>

</div>
</asp:Content>
