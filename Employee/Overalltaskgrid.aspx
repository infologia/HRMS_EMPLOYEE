<%@ Page Title="Overall Task Grid" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Overalltaskgrid.aspx.cs" Inherits="Employee_Overalltaskgrid" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function confirmDelete(taskKey) {
            if (confirm('Are you sure you want to remove this task?')) {
                window.location.href = 'Overalltaskgrid.aspx?action=delete&taskkey=' + taskKey;
            }
        }
    </script>
    <style>
        /* Hide DataTable Sorting Icons but keep functionality */
        table.dataTable thead th.sorting:after,
        table.dataTable thead th.sorting_asc:after,
        table.dataTable thead th.sorting_desc:after,
        table.dataTable thead th.sorting:before,
        table.dataTable thead th.sorting_asc:before,
        table.dataTable thead th.sorting_desc:before {
            display: none !important;
        }

        /* ── CSS combined from taskdashboard and newtaskgrids ── */

        /* From taskdashboard.aspx */
        .chart-section {
            display: flex;
            align-items: center;
            gap: 40px;
            padding: 20px 10px 30px;
            border-bottom: 1px solid #f0f0f0;
            margin-bottom: 20px;
        }

        .chart-wrap {
            position: relative;
            width: 400px;
            height: 200px;
            flex-shrink: 0;
            display: flex;
            align-items: flex-end;
            gap: 20px;
            padding: 10px;
            border: 1px solid #e5e7eb;
            border-radius: 8px;
            background: #fafafa;
        }

        .bar-item {
            flex: 1;
            display: flex;
            flex-direction: column;
            align-items: center;
            gap: 8px;
        }

        .bar-column {
            width: 100%;
            background: linear-gradient(180deg, var(--bar-color) 0%, var(--bar-color-dark) 100%);
            border-radius: 6px 6px 0 0;
            transition: all 0.3s ease;
            position: relative;
            min-height: 4px;
        }

        .bar-column:hover {
            opacity: 0.8;
            transform: translateY(-2px);
        }

        .bar-value {
            position: absolute;
            top: -25px;
            left: 50%;
            transform: translateX(-50%);
            font-size: 10px;
            font-weight: 700;
            color: #222;
            white-space: nowrap;
        }

        .bar-label {
            font-size: 10px;
            color: #666;
            text-align: center;
            font-weight: 600;
            white-space: nowrap;
        }

        .chart-legend {
            display: flex;
            flex-direction: column;
            gap: 12px;
        }

        .legend-item {
            display: flex;
            align-items: center;
            gap: 10px;
            font-size: 13px;
        }

        .legend-dot {
            width: 12px;
            height: 12px;
            border-radius: 50%;
            flex-shrink: 0;
        }

        .legend-label { color: #555; white-space: nowrap; }
        .legend-val   { font-weight: 700; color: #222; margin-left: auto; padding-left: 20px; white-space: nowrap; }

        @keyframes dance {
            0%, 100% { transform: rotate(0deg) translateY(0); }
            25% { transform: rotate(-10deg) translateY(-3px); }
            50% { transform: rotate(10deg) translateY(-6px); }
            75% { transform: rotate(-10deg) translateY(-3px); }
        }

        @keyframes subtleMove {
            0%, 100% { transform: translateY(0px) translateX(0px) scale(1); }
            25% { transform: translateY(-5px) translateX(2px) scale(1.01); }
            50% { transform: translateY(-10px) translateX(0px) scale(1.02); }
            75% { transform: translateY(-5px) translateX(-2px) scale(1.01); }
        }

        .character-assistant {
            width: 200px;
            height: auto;
            display: block;
            animation: subtleMove 4s ease-in-out infinite;
            margin-left: 30px;
            filter: drop-shadow(0 4px 8px rgba(0,0,0,0.1));
            transition: transform 0.3s ease;
        }

        .character-assistant:hover {
            transform: scale(1.05);
        }

        .quote-bubble {
            position: relative;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
            padding: 12px 16px;
            border-radius: 12px;
            font-size: 13px;
            font-weight: 500;
            margin-left: 30px;
            max-width: 250px;
            box-shadow: 0 4px 12px rgba(102, 126, 234, 0.3);
            animation: fadeIn 1s ease;
        }

        .quote-bubble::before {
            content: '';
            position: absolute;
            left: -10px;
            top: 50%;
            transform: translateY(-50%);
            width: 0;
            height: 0;
            border-top: 10px solid transparent;
            border-bottom: 10px solid transparent;
            border-right: 10px solid #667eea;
        }

        @keyframes fadeIn {
            from { opacity: 0; transform: translateY(10px); }
            to { opacity: 1; transform: translateY(0); }
        }

        /* From newtaskgrids.aspx */
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

        .dashboard-panel {
            display: flex;
            align-items: center;
            gap: 14px;
            background: #fff;
            border-radius: 12px;
            border: 1.5px solid #e5e7eb;
            padding: 7px 8px;
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

        .card-icon-box {
            width: 36px;
            height: 36px;
            border-radius: 10px;
            display: flex;
            align-items: center;
            justify-content: center;
            flex-shrink: 0;
            font-size: 20px;
        }

        .card-info {
            display: flex;
            flex-direction: column;
            gap: 2px;
        }

        .card-label {
            font-size:9px;
            color: #6b7280;
            font-weight: 500;
            white-space: nowrap;
        }

        .card-count {
            font-size: 12px;
            font-weight: 700;
            color: #111827;
            line-height: 1.1;
        }

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

        .card-overdue.has-value { animation: pulse-glow 2s infinite; }

        @keyframes pulse-glow {
            0%, 100% { box-shadow: 0 1px 4px rgba(198,40,40,0.15); }
            50%       { box-shadow: 0 4px 14px rgba(198,40,40,0.35); }
        }

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

        .panel.panel-flat {
            border-radius: 12px !important;
            border: 1px solid #e5e7eb !important;
            overflow: hidden;
            box-shadow: 0 2px 8px rgba(0,0,0,0.06);
        }

        .table-responsive { border-radius: 0 0 12px 12px; overflow: auto; }

        .table {
            border-collapse: collapse !important;
            width: 100% !important;
            border-radius: 0 0 12px 12px;
            overflow: hidden;
        }

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

        @media (max-width: 1200px) {
            .card-col { flex: 0 0 50%; }
        }
        @media (max-width: 767px) {
            .card-col { flex: 0 0 50%; }
            .dashboard-panel           { padding: 10px 12px; gap: 10px; }
            .card-icon-box             { width: 28px; height: 28px; font-size: 16px; }
            .card-count                { font-size: 20px; }
            
            .chart-section {
                flex-direction: column;
                gap: 20px;
                padding: 15px 5px;
            }
            .chart-wrap {
                width: 100%;
                height: 180px;
                gap: 10px;
            }
            .chart-legend {
                width: 100%;
            }
            .character-assistant {
                width: 120px;
                margin-left: 0;
            }
            .quote-bubble {
                margin-left: 0;
                max-width: 100%;
                font-size: 12px;
            }
            .panel-heading { flex-direction: column; align-items: flex-start; }
            .date-filter {
                flex-direction: column;
                align-items: stretch;
                gap: 8px;
                width: 100%;
            }
            .filter-pair {
                display: flex;
                align-items: center;
                gap: 10px;
                width: 100%;
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
            .employee-dropdown, .date-dropdown { width: 100% !important; height: 36px !important; }
            .table-responsive { overflow-x: auto; -webkit-overflow-scrolling: touch; }
            .table             { min-width: 800px; font-size: 12px; }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="hfActiveStatus" runat="server" Value="0" />
            <asp:HiddenField ID="hf_meetings" runat="server" />
            <asp:HiddenField ID="hf_task"     runat="server" />
            <asp:HiddenField ID="hf_testing"  runat="server" />
            <asp:HiddenField ID="hf_gender"   runat="server" />

            <div class="panel panel-flat" style="margin-bottom: 20px;">
                <div class="panel-heading">
                    <h5 class="panel-title">Overall Task </h5>
                    <div class="date-filter">
                        <div id="divEmployeeFilter" runat="server" visible="false" class="employee-filter-wrap">
                            <label class="date-label">Employee:</label>
                            <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="employee-dropdown" AutoPostBack="true" OnSelectedIndexChanged="Filter_Changed"></asp:DropDownList>
                        </div>
                        <div style="display:flex; align-items:center; gap: 10px;">
                            <label class="date-label">Month :</label>
                            <asp:DropDownList ID="ddlMonth" runat="server" CssClass="date-dropdown" AutoPostBack="true" OnSelectedIndexChanged="Filter_Changed"></asp:DropDownList>
                        </div>
                        <div style="display:flex; align-items:center; gap: 10px;">
                            <label class="year-label">Year :</label>
                            <asp:DropDownList ID="ddlYear" runat="server" CssClass="date-dropdown" AutoPostBack="true" OnSelectedIndexChanged="Filter_Changed"></asp:DropDownList>
                        </div>
                    </div>
                </div>

                <div class="panel-body">
                    <!-- Chart Section -->
                    <div id="noDataState" runat="server" visible="false" style="text-align:center; padding: 40px 20px; background: #fafafa; border-radius: 8px; border: 1px dashed #ccc; margin-bottom: 20px;">
                        <h4 style="color: #888; margin-bottom: 10px;">No task data available</h4>
                        <p style="color: #aaa; margin: 0;">Try selecting a different employee or period.</p>
                    </div>
                    <div class="chart-section" id="chartSectionContainer" runat="server">
                        <div class="chart-wrap" id="barChart"></div>
                        <div class="chart-legend">
                            <div class="legend-item">
                                <div class="legend-dot" style="background:#6366f1;"></div>
                                <span class="legend-label">&#129309; Meetings</span>
                                <span class="legend-val" id="leg_meetings">0 hrs</span>
                            </div>
                            <div class="legend-item">
                                <div class="legend-dot" style="background:#22c55e;"></div>
                                <span class="legend-label">&#128203; Task</span>
                                <span class="legend-val" id="leg_task">0 hrs</span>
                            </div>
                            <div class="legend-item">
                                <div class="legend-dot" style="background:#f59e0b;"></div>
                                <span class="legend-label">&#129514; Testing</span>
                                <span class="legend-val" id="leg_testing">0 hrs</span>
                            </div>
                            <div class="legend-item" style="border-top:1px solid #eee;padding-top:10px;margin-top:4px;">
                                <div class="legend-dot" style="background:transparent;"></div>
                                <span class="legend-label" style="font-weight:700;color:#222;">Total</span>
                                <span class="legend-val" id="leg_total" style="color:#222;">0 hrs</span>
                            </div>
                        </div>
                        <div style="display: flex; flex-direction: column; align-items: center; gap: 15px;">
                            <img id="characterAssistant" class="character-assistant" src="" alt="Assistant" />
                            <div id="quoteBubble" class="quote-bubble"></div>
                        </div>
                    </div>

                    <!-- Widgets Section -->
                    <div class="cards-row" style="margin-top:20px;">
                        <div class="card-col">
                            <asp:LinkButton ID="btnCard0" runat="server" OnClick="CardClick" CommandArgument="0" style="text-decoration: none; display:block;">
                                <div class='dashboard-panel card-all-tasks <%= hfActiveStatus.Value == "0" ? "active" : "" %>'>
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
                                <div class='dashboard-panel card-yet-to-start <%= hfActiveStatus.Value == "1" ? "active" : "" %>'>
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
                                <div class='dashboard-panel card-in-progress <%= hfActiveStatus.Value == "2" ? "active" : "" %>'>
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
                                <div class='dashboard-panel card-overdue <%= hfActiveStatus.Value == "3" ? "active" : "" %> <%= GetOverdueClass() %>'>
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
                                <div class='dashboard-panel card-completed <%= hfActiveStatus.Value == "4" ? "active" : "" %>'>
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
                        <div class="card-col">
                            <div class='dashboard-panel card-meetings'>
                                <div class="card-icon-box icon-meetings" style="background:#e0f2f1; color:#00796b;">
                                    <i class="glyphicon glyphicon-bullhorn"></i>
                                </div>
                                <div class="card-info" style="flex: 1;">
                                    <span class="card-label">Meetings</span>
                                    <div style="display:flex; justify-content:space-between; width:100%; font-size:12px; font-weight:bold; margin-top:5px;">
                                        <span style="color:#1565c0;">Sch: <asp:Label ID="lbl_SchMeeting" runat="server">0</asp:Label></span>
                                        <span style="color:#2e7d32;">Cmp: <asp:Label ID="lbl_CmpMeeting" runat="server">0</asp:Label></span>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- Grid Section -->
                    <div style="margin-top:20px;">
                        <h5 style="font-weight:600; margin-bottom:10px;"><%= GetStatusTitle() %></h5>
                        <div class="table-responsive">
                            <table class="table datatable-basic">
                                <thead>
                                    <tr>
                                        <th style="min-width: 100px;">Work Day</th>
                                        <th>Assigned To</th>
                                        <th>Assigned Hours</th>
                                        <th>Actual Hours</th>
                                        <th>Task count</th>
                                        <th>Meeting count</th>
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
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <script>
        function renderChart() {
            try {
                var elMeetings = document.getElementById('<%= hf_meetings.ClientID %>');
                var elTask     = document.getElementById('<%= hf_task.ClientID %>');
                var elTesting  = document.getElementById('<%= hf_testing.ClientID %>');
                var elGender   = document.getElementById('<%= hf_gender.ClientID %>');

                var meetings = elMeetings && elMeetings.value ? (parseFloat(elMeetings.value) || 0) : 0;
                var task     = elTask && elTask.value ? (parseFloat(elTask.value) || 0) : 0;
                var testing  = elTesting && elTesting.value ? (parseFloat(elTesting.value) || 0) : 0;
                var gender   = elGender && elGender.value ? elGender.value : '0';
                
                var total    = meetings + task + testing;

                // JS display toggle removed; handled by C# server-side now.

                var characterImg = document.getElementById('characterAssistant');
                if (characterImg) {
                    if (gender === '1') {
                        characterImg.src = '../images/WOMEN.png';
                } else {
                    characterImg.src = '../images/MEN.png';
                }
            }

            var maleQuotes = ["Keep pushing forward, champ!", "You're doing great today!", "Stay focused and crush it!", "Every task completed is a win!", "You've got this, keep going!"];
            var femaleQuotes = ["You're amazing, keep shining!", "Great work today, superstar!", "Stay strong and keep going!", "You're making great progress!", "Believe in yourself, you're doing great!"];
            var quotes = (gender === '1') ? femaleQuotes : maleQuotes;
            var randomQuote = quotes[Math.floor(Math.random() * quotes.length)];
            var quoteBubble = document.getElementById('quoteBubble');
            if (quoteBubble) quoteBubble.innerText = randomQuote;

            var legMeetings = document.getElementById('leg_meetings');
            if (legMeetings) legMeetings.innerText = meetings.toFixed(1) + ' hrs';
            var legTask = document.getElementById('leg_task');
            if (legTask) legTask.innerText = task.toFixed(1) + ' hrs';
            var legTesting = document.getElementById('leg_testing');
            if (legTesting) legTesting.innerText = testing.toFixed(1) + ' hrs';
            var legTotal = document.getElementById('leg_total');
            if (legTotal) legTotal.innerText = total.toFixed(1) + ' hrs';

            var data = [
                { label: '&#129309; Meetings', value: meetings, color: '#6366f1', colorDark: '#4f46e5' },
                { label: '&#128203; Task', value: task, color: '#22c55e', colorDark: '#16a34a' },
                { label: '&#129514; Testing', value: testing, color: '#f59e0b', colorDark: '#d97706' }
            ];

            var maxValue = Math.max.apply(null, data.map(function(d){ return d.value; }));
            if (maxValue === 0) maxValue = 1;

            var barChart = document.getElementById('barChart');
            if (barChart) {
                barChart.innerHTML = '';
                data.forEach(function(item) {
                    var barItem = document.createElement('div');
                    barItem.className = 'bar-item';

                    var barColumn = document.createElement('div');
                    barColumn.className = 'bar-column';
                    var height = (item.value / maxValue) * 160;
                    barColumn.style.height = height + 'px';
                    barColumn.style.setProperty('--bar-color', item.color);
                    barColumn.style.setProperty('--bar-color-dark', item.colorDark);

                    var barValue = document.createElement('div');
                    barValue.className = 'bar-value';
                    barValue.innerText = item.value.toFixed(1) + ' hrs';
                    barColumn.appendChild(barValue);

                    var barLabel = document.createElement('div');
                    barLabel.className = 'bar-label';
                    barLabel.innerHTML = item.label;

                    barItem.appendChild(barColumn);
                    barItem.appendChild(barLabel);
                    barChart.appendChild(barItem);
                });
            }
            } catch (e) {
                console.error("Error in renderChart: ", e);
            }
        }

        function initDataTable() {
            if ($.fn.DataTable.isDataTable('.datatable-basic')) {
                $('.datatable-basic').DataTable().destroy();
            }
            $('.datatable-basic').DataTable({
                paging: true,
                searching: true,
                ordering: true
            });
        }

        window.onload = function () {
            renderChart();
            initDataTable();
        };

        $(document).ready(function () {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            prm.add_endRequest(function () {
                renderChart();
                initDataTable();
            });
        });
    </script>
</asp:Content>
