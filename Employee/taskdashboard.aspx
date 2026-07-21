<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="taskdashboard.aspx.cs" Inherits="Employee_taskdashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<style>
    @keyframes fadeSlideUp {
        from { opacity: 0; transform: translateY(30px); }
        to   { opacity: 1; transform: translateY(0); }
    }

    @keyframes fillBar {
        from { width: 0%; }
        to   { width: var(--bar-width); }
    }

    .dash-row-group {
        display: flex;
        gap: 14px;
        margin-bottom: 18px;
    }

    .dash-card {
        flex: 1;
        min-height: 200px;
        padding: 14px 16px;
        border-radius: 14px;
        box-sizing: border-box;
        background: #fff;
        box-shadow: 0 4px 14px rgba(0,0,0,0.08);
        transition: transform 0.25s ease, box-shadow 0.25s ease;
        animation: fadeSlideUp 0.5s ease both;
        position: relative;
        overflow: hidden;
    }

    .dash-card:hover {
        transform: translateY(-6px);
        box-shadow: 0 10px 28px rgba(0,0,0,0.15);
    }

    .green-card   { border-top: 4px solid #22c55e; }
    .red-card     { border-top: 4px solid #ef4444; }
    .leave-card   { border-top: 4px solid #f59e0b; background: #fff7ed; }
    .halfday-card { border-top: 4px solid #22c55e; }

    .green-card   .dash-date { color: #16a34a; }
    .red-card     .dash-date { color: #dc2626; }
    .leave-card   .dash-date { color: #d97706; }
    .halfday-card .dash-date { color: #16a34a; }

    .dash-date {
        font-weight: 700;
        font-size: 13px;
        margin-bottom: 12px;
        letter-spacing: 0.4px;
    }

    .dash-row {
        display: flex;
        justify-content: space-between;
        font-size: 12px;
        color: #555;
        margin-bottom: 5px;
    }

    .dash-row span.val {
        font-weight: 600;
        color: #222;
    }

    .dash-total {
        margin-top: 12px;
        font-weight: 700;
        font-size: 13px;
        display: flex;
        justify-content: space-between;
        border-top: 1px solid #eee;
        padding-top: 8px;
    }

    .progress-wrap {
        margin-top: 10px;
        background: #f0f0f0;
        border-radius: 6px;
        height: 6px;
        overflow: hidden;
    }

    .progress-bar {
        height: 6px;
        border-radius: 6px;
        animation: fillBar 0.8s ease forwards;
        width: var(--bar-width);
    }

    .green-card   .progress-bar { background: #22c55e; }
    .red-card     .progress-bar { background: #ef4444; }
    .leave-card   .progress-bar { background: #f59e0b; }
    .halfday-card .progress-bar { background: #22c55e; }

    .leave-center {
        height: 140px;
        display: flex;
        justify-content: center;
        align-items: center;
        font-weight: 700;
        font-size: 14px;
        color: #d97706;
        text-align: center;
    }

    .panel-heading {
        display: flex;
        justify-content: space-between;
        align-items: center;
        flex-wrap: wrap;
        gap: 15px;
    }

    .date-filter {
        display: flex;
        align-items: center;
        gap: 12px;
        flex-wrap: wrap;
    }

    .employee-filter-wrap {
        display: inline-flex;
        align-items: center;
        gap: 8px;
        padding-right: 20px;
        border-right: 2px solid #e5e7eb;
    }

    .date-label { font-size: 13px; font-weight: 600; color: #374151; }
    .year-label { font-size: 13px; font-weight: 600; color: #374151; }

    .date-dropdown {
        width: 120px;
        height: 32px;
        padding: 4px 8px;
        font-size: 13px;
        border: 1px solid #d1d5db;
        border-radius: 6px;
        background: #fff;
    }

    .employee-dropdown {
        width: 200px;
        height: 32px;
        padding: 4px 8px;
        font-size: 13px;
        border: 1px solid #d1d5db;
        border-radius: 6px;
        background: #fff;
    }

    .no-data {
        text-align: center;
        padding: 30px;
        color: #888;
        font-size: 14px;
    }

    /* Bar chart section */
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
        font-size: 11px;
        font-weight: 700;
        color: #222;
        white-space: nowrap;
    }

    .bar-label {
        font-size: 11px;
        color: #666;
        text-align: center;
        font-weight: 600;
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

    .legend-label { color: #555; }
    .legend-val   { font-weight: 700; color: #222; margin-left: auto; padding-left: 20px; }

    @keyframes dance {
        0%, 100% { transform: rotate(0deg) translateY(0); }
        25% { transform: rotate(-10deg) translateY(-3px); }
        50% { transform: rotate(10deg) translateY(-6px); }
        75% { transform: rotate(-10deg) translateY(-3px); }
    }

    @keyframes float {
        0%, 100% { transform: translateY(0px); }
        50% { transform: translateY(-10px); }
    }

    @keyframes breathe {
        0%, 100% { transform: translateY(0px) scale(1); }
        50% { transform: translateY(-8px) scale(1.02); }
    }

    @keyframes subtleMove {
        0%, 100% { transform: translateY(0px) translateX(0px) scale(1); }
        25% { transform: translateY(-5px) translateX(2px) scale(1.01); }
        50% { transform: translateY(-10px) translateX(0px) scale(1.02); }
        75% { transform: translateY(-5px) translateX(-2px) scale(1.01); }
    }

    .dancing-man {
        font-size: 32px;
        display: inline-block;
        animation: dance 0.6s ease-in-out infinite;
        margin-left: 15px;
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

/*    .meeting-label {
        cursor: help;
        border-bottom: 1px dotted #666;
    }*/

    /* Mobile Responsive */
    @media (max-width: 768px) {
        .dash-row-group {
            flex-direction: column;
            gap: 12px;
        }

        .dash-card {
            min-height: auto;
        }

        .panel-heading {
            flex-direction: column;
            align-items: flex-start;
        }

        .date-filter {
            width: 100%;
            flex-direction: column;
            align-items: stretch;
            gap: 10px;
        }

        .employee-filter-wrap {
            border-right: none;
            border-bottom: 2px solid #e5e7eb;
            padding-right: 0;
            padding-bottom: 10px;
        }

        .date-dropdown, .employee-dropdown {
            width: 100%;
        }

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

        .bar-column {
            min-height: 3px;
        }

        .bar-value {
            font-size: 10px;
            top: -22px;
        }

        .bar-label {
            font-size: 10px;
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
    }
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:HiddenField ID="hf_meetings" runat="server" />
    <asp:HiddenField ID="hf_task"     runat="server" />
    <asp:HiddenField ID="hf_testing"  runat="server" />
    <asp:HiddenField ID="hf_outside"  runat="server" />
    <asp:HiddenField ID="hf_gender"   runat="server" />

    <div class="panel panel-flat">
        <div class="panel-heading">
            <h5 class="panel-title">Task Dashboard</h5>
            <div class="date-filter">
                <div id="divEmployeeFilter" runat="server" visible="false" class="employee-filter-wrap">
                    <label class="date-label">Employee:</label>
                    <asp:DropDownList ID="ddlEmployee" runat="server"
                        CssClass="employee-dropdown"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlEmployee_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>

                <label class="date-label">Month:</label>
                <asp:DropDownList ID="ddlMonth" runat="server"
                    CssClass="date-dropdown"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlMonth_SelectedIndexChanged">
                </asp:DropDownList>

                <label class="year-label">Year:</label>
                <asp:DropDownList ID="ddlYear" runat="server"
                    CssClass="date-dropdown"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
        </div>

        <div class="panel-body">

            <!-- Bar Chart -->
            <div class="chart-section">
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
                    <div class="legend-item">
                        <div class="legend-dot" style="background:#ef4444;"></div>
                        <span class="legend-label">&#128188; Business Meetings</span>
                        <span class="legend-val" id="leg_outside">0 hrs</span>
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
            <asp:PlaceHolder ID="PH_Dashboard" runat="server"></asp:PlaceHolder>
        </div>
    </div>

    <script>
        window.onload = function () {
            var meetings = parseFloat(document.getElementById('<%= hf_meetings.ClientID %>').value) || 0;
            var task     = parseFloat(document.getElementById('<%= hf_task.ClientID %>').value)     || 0;
            var testing  = parseFloat(document.getElementById('<%= hf_testing.ClientID %>').value)  || 0;
            var outside  = parseFloat(document.getElementById('<%= hf_outside.ClientID %>').value)  || 0;
            var gender   = document.getElementById('<%= hf_gender.ClientID %>').value;
            var total    = meetings + task + testing + outside;

            // Set character image based on gender
            var characterImg = document.getElementById('characterAssistant');
            if (gender === '1') {
                characterImg.src = '../images/WOMEN.png';
            } else {
                characterImg.src = '../images/MEN.png';
            }

            // Motivational quotes based on gender
            var maleQuotes = [
                "Keep pushing forward, champ!",
                "You're doing great today!",
                "Stay focused and crush it!",
                "Every task completed is a win!",
                "You've got this, keep going!"
            ];

            var femaleQuotes = [
                "You're amazing, keep shining!",
                "Great work today, superstar!",
                "Stay strong and keep going!",
                "You're making great progress!",
                "Believe in yourself, you're doing great!"
            ];

            var quotes = (gender === '1') ? femaleQuotes : maleQuotes;
            var randomQuote = quotes[Math.floor(Math.random() * quotes.length)];
            document.getElementById('quoteBubble').innerText = randomQuote;

            document.getElementById('leg_meetings').innerText = meetings.toFixed(1) + ' hrs';
            document.getElementById('leg_task').innerText      = task.toFixed(1)     + ' hrs';
            document.getElementById('leg_testing').innerText   = testing.toFixed(1)  + ' hrs';
            document.getElementById('leg_outside').innerText   = outside.toFixed(1)  + ' hrs';
            document.getElementById('leg_total').innerText     = total.toFixed(1)    + ' hrs';

            var data = [
                { label: '&#129309; Meetings', value: meetings, color: '#6366f1', colorDark: '#4f46e5' },
                { label: '&#128203; Task', value: task, color: '#22c55e', colorDark: '#16a34a' },
                { label: '&#129514; Testing', value: testing, color: '#f59e0b', colorDark: '#d97706' },
                { label: '&#128188; Business Meetings', value: outside, color: '#ef4444', colorDark: '#dc2626' }
            ];

            var maxValue = Math.max.apply(null, data.map(function(d){ return d.value; }));
            if (maxValue === 0) maxValue = 1;

            var barChart = document.getElementById('barChart');
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
        };
    </script>

</asp:Content>
