<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="employeeholidays.aspx.cs" Inherits="Employee_employeeholidays" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="hol-wrap">

        <%-- Top bar --%>
        <div class="hol-topbar">
            <div>
                <div class="hol-page-title">Holidays - <asp:Literal ID="lit_Year" runat="server"></asp:Literal></div>
                <div class="hol-page-sub">View all scheduled holidays for the year</div>
            </div>
            <div class="hol-year-pill">
                <i class="icon-calendar22"></i>
                Year <asp:Literal ID="lit_Year2" runat="server"></asp:Literal>
            </div>
        </div>

        <%-- Summary cards --%>
        <div class="hol-summary">
            <div class="hol-s-card" onclick="filterByCard('all')" id="card_all">
                <div class="hol-s-num"><asp:Literal ID="lit_Total" runat="server">0</asp:Literal></div>
                <div class="hol-s-lbl">Total holidays</div>
                <div class="hol-s-sub"><i class="icon-calendar22"></i> Full year</div>
            </div>
            <div class="hol-s-card active" onclick="filterByCard('upcoming')" id="card_upcoming">
                <div class="hol-s-num blue"><asp:Literal ID="lit_Upcoming" runat="server">0</asp:Literal></div>
                <div class="hol-s-lbl">Upcoming</div>
                <div class="hol-s-sub blue"><i class="icon-alarm"></i> <asp:Literal ID="lit_NextHoliday" runat="server"></asp:Literal></div>
            </div>
            <div class="hol-s-card" onclick="filterByCard('passed')" id="card_passed">
                <div class="hol-s-num green"><asp:Literal ID="lit_Passed" runat="server">0</asp:Literal></div>
                <div class="hol-s-lbl">Completed</div>
                <div class="hol-s-sub green"><i class="icon-checkmark2"></i> Already passed</div>
            </div>
        </div>

        <%-- Table card --%>
        <div class="hol-table-card">

            <div class="hol-month-header">
                <i class="icon-calendar22" style="font-size:13px;color:#185FA5;"></i>
                <span class="hol-month-label" id="header_title">Upcoming holidays</span>
                <span class="hol-month-count" id="header_count"><asp:Literal ID="lit_HeaderCount" runat="server">0</asp:Literal></span>
            </div>

            <%-- Hidden count holders for JS --%>
            <span id="cnt_all" style="display:none;"><asp:Literal ID="lit_TotalJS" runat="server">0</asp:Literal></span>
            <span id="cnt_upcoming" style="display:none;"><asp:Literal ID="lit_UpcomingJS" runat="server">0</asp:Literal></span>
            <span id="cnt_passed" style="display:none;"><asp:Literal ID="lit_PassedJS" runat="server">0</asp:Literal></span>

            <div id="grid_all" style="display:none;">
                <table class="table datatable-basic" id="tbl_all">
                    <thead><tr><th>S No</th><th>Holiday</th><th>Name</th><th>Day</th><th>No Of Leave</th><th>Status</th></tr></thead>
                    <tbody>
                        <asp:PlaceHolder ID="PH_All" runat="server"></asp:PlaceHolder>
                    </tbody>
                </table>
            </div>

            <div id="grid_upcoming" style="display:block;">
                <table class=" table datatable-basic" id="tbl_upcoming">
                    <thead><tr><th>S No</th><th>Holiday</th><th>Name</th><th>Day</th><th>No Of Leave</th><th>Status</th></tr></thead>
                    <tbody>
                        <asp:PlaceHolder ID="PH_Upcoming" runat="server"></asp:PlaceHolder>
                    </tbody>
                </table>
            </div>

            <div id="grid_passed" style="display:none;">
                <table class=" table datatable-basic" id="tbl_passed">
                    <thead><tr><th>S No</th><th>Holiday</th><th>Name</th><th>Day</th><th>No Of Leave</th><th>Status</th></tr></thead>
                    <tbody>
                        <asp:PlaceHolder ID="PH_Passed" runat="server"></asp:PlaceHolder>
                    </tbody>
                </table>
            </div>

            <%-- Empty state --%>
            <div class="hol-empty" id="hol_empty" style="display:none;">
                <i class="icon-calendar52"></i>
                <p>No holidays found</p>
            </div>

            <div class="hol-legend">
                <div class="hol-leg-item">
                    <div class="hol-leg-dot" style="background:#EBF4FD;border:1px solid #185FA5;"></div> Upcoming holiday
                </div>
                <div class="hol-leg-item">
                    <div class="hol-leg-dot" style="background:#f7f8fa;border:1px solid #e0e0e0;"></div> Past holiday
                </div>
                <div class="hol-leg-item">
                    <i class="icon-alarm" style="font-size:11px;color:#185FA5;"></i> Days until holiday
                </div>
            </div>
        </div>

    </div>

    <script type="text/javascript">
        var currentFilter = 'upcoming';
        var tableAll, tableUpcoming, tablePassed;

        $(document).ready(function() {
            var dtOptions = {
                "pageLength": 10,
                "order": [[0, 'asc']],
                "dom": 'Bfrtip',
                "buttons": ['copy', 'excel', 'pdf']
            };

            // Initialize DataTables manually
            tableAll = $('#tbl_all').DataTable(dtOptions);
            tableUpcoming = $('#tbl_upcoming').DataTable(dtOptions);
            tablePassed = $('#tbl_passed').DataTable(dtOptions);

            // Apply default filter
            applyFilter();
        });

        function filterByCard(filter) {
            currentFilter = filter;
            $('.hol-s-card').removeClass('active');
            $('#card_' + filter).addClass('active');
            applyFilter();
        }

        function applyFilter() {
            $('#header_title').text(
                currentFilter === 'all' ? 'All holidays' :
                currentFilter === 'upcoming' ? 'Upcoming holidays' : 'Completed holidays'
            );

            // Hide all grids
            $('#grid_all').hide();
            $('#grid_upcoming').hide();
            $('#grid_passed').hide();

            var activeTable;

            if (currentFilter === 'all') {
                $('#grid_all').show();
                activeTable = tableAll;
            } else if (currentFilter === 'upcoming') {
                $('#grid_upcoming').show();
                activeTable = tableUpcoming;
            } else if (currentFilter === 'passed') {
                $('#grid_passed').show();
                activeTable = tablePassed;
            }

            // Recalculate column widths after showing a previously hidden table
            if (activeTable) {
                activeTable.columns.adjust();
            }

            // Update header count from server-side values
            var countMap = {
                'all': $('#cnt_all').text().trim(),
                'upcoming': $('#cnt_upcoming').text().trim(),
                'passed': $('#cnt_passed').text().trim()
            };
            $('#header_count').text(countMap[currentFilter] || 0);
        }
    </script>

</asp:Content>
