<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="LeaveResponse.aspx.cs" Inherits="WEB_Admin_LeaveResponse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>
    <script type="text/javascript">
        var currentFilter = 'all';
        var tableAll, tablePending, tableApproved, tableRejected;

        $(document).ready(function () {
            var dtOptions = {
                "order": [[2, "desc"]], // From Date
                "autoWidth": false
            };

            tableAll = $('#tbl_all').DataTable(dtOptions);
            tablePending = $('#tbl_pending').DataTable(dtOptions);
            tableApproved = $('#tbl_approved').DataTable(dtOptions);
            tableRejected = $('#tbl_rejected').DataTable(dtOptions);
        });

        function filterByCard(filter) {
            currentFilter = filter;
            
            $('.pr-stat-card').removeClass('active');
            $('#card_' + filter).addClass('active');
            
            $('.pr-grid-card').hide();
            
            $('#grid_' + filter).show();
            
            var tblId = '#tbl_' + filter;
            if ($.fn.DataTable.isDataTable(tblId)) {
                $(tblId).DataTable().columns.adjust();
            }
        }
    </script>
    <style>
        .date-filter {
            display: flex;
            align-items: center;
            gap: 12px;
        }
        .date-label, .year-label {
            font-size: 13px;
            margin: 0;
            white-space: nowrap;
        }
        .date-dropdown, .year-dropdown {
            width: 120px;
            height: 30px;
            padding: 2px 6px;
            font-size: 13px;
            display: inline-block;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="pr-wrap">
        <div class="pr-header">
            <h1>Employee Leave Details</h1>
            <div class="date-filter">
                <label for="ddlEmployee" class="date-label">Employee :</label>
                <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-control date-dropdown" style="width: 150px;"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlEmployee_SelectedIndexChanged">
                </asp:DropDownList>

                <label for="ddlDate" class="date-label">Month :</label>
                <asp:DropDownList ID="ddlDate" runat="server" CssClass="form-control date-dropdown"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlDate_SelectedIndexChanged">
                </asp:DropDownList>

                <label for="ddlYear" class="year-label">Year :</label>
                <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control year-dropdown"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                </asp:DropDownList>

                <a href="UninformedLeaves.aspx" class="btn btn-info" style="margin-left: 6px;">Uninformed Leave</a>
            </div>
        </div>

        <div class="pr-summary">
            <div class="pr-stat-card active" id="card_all" onclick="filterByCard('all')" style="cursor:pointer;">
                <div class="pr-stat-icon total"><i class="icon-users4"></i></div>
                <div>
                    <span class="pr-stat-label">Total Leaves</span>
                    <p class="pr-stat-value"><asp:Label ID="lbl_total" runat="server" Text="0"></asp:Label></p>
                </div>
            </div>
            <div class="pr-stat-card" id="card_pending" onclick="filterByCard('pending')" style="cursor:pointer;">
                <div class="pr-stat-icon pending"><i class="icon-history"></i></div>
                <div>
                    <span class="pr-stat-label">Pending Leaves</span>
                    <p class="pr-stat-value"><asp:Label ID="lbl_pending" runat="server" Text="0"></asp:Label></p>
                </div>
            </div>
            <div class="pr-stat-card" id="card_approved" onclick="filterByCard('approved')" style="cursor:pointer;">
                <div class="pr-stat-icon approved"><i class="icon-checkmark-circle"></i></div>
                <div>
                    <span class="pr-stat-label">Approved Leaves</span>
                    <p class="pr-stat-value"><asp:Label ID="lbl_approved" runat="server" Text="0"></asp:Label></p>
                </div>
            </div>
            <div class="pr-stat-card" id="card_rejected" onclick="filterByCard('rejected')" style="cursor:pointer;">
                <div class="pr-stat-icon rejected"><i class="icon-cancel-circle2"></i></div>
                <div>
                    <span class="pr-stat-label">Rejected Leaves</span>
                    <p class="pr-stat-value"><asp:Label ID="lbl_rejected" runat="server" Text="0"></asp:Label></p>
                </div>
            </div>
        </div>

        <div class="pr-grid-card" id="grid_all">
            <div style="padding: 15px 20px; border-bottom: 1px solid #eee; font-weight: 600; font-size: 15px; color: #333;"><i class="icon-users4" style="margin-right: 8px; color: #888;"></i>All Leaves</div>
            <div class="pr-grid-scroll">
                <table class="table datatable-basic table-xxs text-size-small pr-datatable" id="tbl_all">
                    <thead>
                        <tr>
                            <th>Employee ID</th>
                            <th>User Name</th>
                            <th>From Date</th>
                            <th>To Date</th>
                            <th>Leave Days</th>
                            <th>Status</th>
                            <th class="text-center">View</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder ID="PH_Leave" runat="server"></asp:PlaceHolder>
                    </tbody>
                </table>
            </div>
        </div>

        <div class="pr-grid-card" id="grid_pending" style="display:none;">
            <div style="padding: 15px 20px; border-bottom: 1px solid #eee; font-weight: 600; font-size: 15px; color: #333;"><i class="icon-history" style="margin-right: 8px; color: #FF9800;"></i>Pending Leaves</div>
            <div class="pr-grid-scroll">
                <table class="table datatable-basic table-xxs text-size-small pr-datatable" id="tbl_pending">
                    <thead>
                        <tr>
                            <th>Employee ID</th>
                            <th>User Name</th>
                            <th>From Date</th>
                            <th>To Date</th>
                            <th>Leave Days</th>
                            <th>Status</th>
                            <th class="text-center">View</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder ID="PH_Pending" runat="server"></asp:PlaceHolder>
                    </tbody>
                </table>
            </div>
        </div>

        <div class="pr-grid-card" id="grid_approved" style="display:none;">
            <div style="padding: 15px 20px; border-bottom: 1px solid #eee; font-weight: 600; font-size: 15px; color: #333;"><i class="icon-checkmark-circle" style="margin-right: 8px; color: #4CAF50;"></i>Approved Leaves</div>
            <div class="pr-grid-scroll">
                <table class="table datatable-basic table-xxs text-size-small pr-datatable" id="tbl_approved">
                    <thead>
                        <tr>
                            <th>Employee ID</th>
                            <th>User Name</th>
                            <th>From Date</th>
                            <th>To Date</th>
                            <th>Leave Days</th>
                            <th>Status</th>
                            <th class="text-center">View</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder ID="PH_Approved" runat="server"></asp:PlaceHolder>
                    </tbody>
                </table>
            </div>
        </div>

        <div class="pr-grid-card" id="grid_rejected" style="display:none;">
            <div style="padding: 15px 20px; border-bottom: 1px solid #eee; font-weight: 600; font-size: 15px; color: #333;"><i class="icon-cancel-circle2" style="margin-right: 8px; color: #F44336;"></i>Rejected Leaves</div>
            <div class="pr-grid-scroll">
                <table class="table datatable-basic table-xxs text-size-small pr-datatable" id="tbl_rejected">
                    <thead>
                        <tr>
                            <th>Employee ID</th>
                            <th>User Name</th>
                            <th>From Date</th>
                            <th>To Date</th>
                            <th>Leave Days</th>
                            <th>Status</th>
                            <th class="text-center">View</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder ID="PH_Rejected" runat="server"></asp:PlaceHolder>
                    </tbody>
                </table>
            </div>
        </div>

    </div>
</asp:Content>

