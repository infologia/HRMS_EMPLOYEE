<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Latepermissionrequestview.aspx.cs" Inherits="Employee_Latepermissionrequestview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>
    <script type="text/javascript">
        var deletePermissionKey = "";

        function fn_DeleteProject(employeepermissiondetailskey) {
            deletePermissionKey = employeepermissiondetailskey;
            $('#confirmDeleteModal').modal('show');
        }

        function confirmDeleteProject() {
            if (deletePermissionKey === "") {
                return;
            }
            $.ajax({
                type: "POST",
                url: "Latepermissionrequestview.aspx/DeleteProject",
                data: JSON.stringify({
                    str_employeepermissiondetailskey: deletePermissionKey
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $('#confirmDeleteModal').modal('hide');
                    if (data.d === "1") {
                        showToastr('success', 'Record removed successfully');
                        setTimeout(function () {
                            location.reload();
                        }, 1500);
                    }
                    else {
                        showToastr('error', 'Unable to remove record. Please try again.');
                    }
                },
                error: function () {
                    $('#confirmDeleteModal').modal('hide');
                    showToastr('error', 'Server error. Please try again.');
                }
            });
        }

        var currentFilter = 'all';
        var tableAll, tablePending, tableApproved, tableRejected;

        $(document).ready(function () {
            var dtOptions = {
                "order": [[1, "desc"]], // sort by Request Date descending
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
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="pr-wrap">

        <div class="pr-header">
            <h1>Late Permission Requests</h1>
            <div style="display: flex; gap: 10px; align-items: center; flex-wrap: wrap;">
                <asp:DropDownList ID="ddl_year" runat="server" CssClass="pr-filter-select" AutoPostBack="true" OnSelectedIndexChanged="ddl_filter_SelectedIndexChanged">
                </asp:DropDownList>
                <asp:DropDownList ID="ddl_month" runat="server" CssClass="pr-filter-select" AutoPostBack="true" OnSelectedIndexChanged="ddl_filter_SelectedIndexChanged">
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
                <a href="Latepermissionrequest.aspx" class="btn-create-permission" style="margin-left: 5px;">
                    <i class="icon-plus-circle2"></i> Create Record
                </a>
            </div>
        </div>

        <div class="pr-summary">
            <div class="pr-stat-card active" id="card_all" onclick="filterByCard('all')" style="cursor:pointer;">
                <div class="pr-stat-icon total"><i class="icon-users4"></i></div>
                <div>
                    <span class="pr-stat-label">Total Requests</span>
                    <p class="pr-stat-value"><asp:Label ID="lbl_total" runat="server" Text="0"></asp:Label></p>
                </div>
            </div>
            <div class="pr-stat-card" id="card_pending" onclick="filterByCard('pending')" style="cursor:pointer;">
                <div class="pr-stat-icon pending"><i class="icon-history"></i></div>
                <div>
                    <span class="pr-stat-label">Pending Requests</span>
                    <p class="pr-stat-value"><asp:Label ID="lbl_pending" runat="server" Text="0"></asp:Label></p>
                </div>
            </div>
            <div class="pr-stat-card" id="card_approved" onclick="filterByCard('approved')" style="cursor:pointer;">
                <div class="pr-stat-icon approved"><i class="icon-checkmark-circle"></i></div>
                <div>
                    <span class="pr-stat-label">Approved Requests</span>
                    <p class="pr-stat-value"><asp:Label ID="lbl_approved" runat="server" Text="0"></asp:Label></p>
                </div>
            </div>
            <div class="pr-stat-card" id="card_rejected" onclick="filterByCard('rejected')" style="cursor:pointer;">
                <div class="pr-stat-icon rejected"><i class="icon-cancel-circle2"></i></div>
                <div>
                    <span class="pr-stat-label">Rejected Requests</span>
                    <p class="pr-stat-value"><asp:Label ID="lbl_rejected" runat="server" Text="0"></asp:Label></p>
                </div>
            </div>
        </div>

        <div class="pr-grid-card" id="grid_all">
            <div style="padding: 15px 20px; border-bottom: 1px solid #eee; font-weight: 600; font-size: 15px; color: #333;"><i class="icon-users4" style="margin-right: 8px; color: #888;"></i>All Requests</div>
            <div class="pr-grid-scroll">
                <table class="table datatable-basic table-xxs text-size-small pr-datatable" id="tbl_all">
                    <thead>
                        <tr>
                            <th>User Name</th>
                            <th>Request Date</th>
                            <th>Time Slot</th>
                            <th>Late Hours</th>
                            <th>Status</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder ID="PH_All" runat="server"></asp:PlaceHolder>
                    </tbody>
                </table>
            </div>
        </div>

        <div class="pr-grid-card" id="grid_pending" style="display:none;">
            <div style="padding: 15px 20px; border-bottom: 1px solid #eee; font-weight: 600; font-size: 15px; color: #333;"><i class="icon-history" style="margin-right: 8px; color: #FF9800;"></i>Pending Requests</div>
            <div class="pr-grid-scroll">
                <table class="table datatable-basic table-xxs text-size-small pr-datatable" id="tbl_pending">
                    <thead>
                        <tr>
                            <th>User Name</th>
                            <th>Request Date</th>
                            <th>Time Slot</th>
                            <th>Late Hours</th>
                            <th>Status</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder ID="PH_Pending" runat="server"></asp:PlaceHolder>
                    </tbody>
                </table>
            </div>
        </div>

        <div class="pr-grid-card" id="grid_approved" style="display:none;">
            <div style="padding: 15px 20px; border-bottom: 1px solid #eee; font-weight: 600; font-size: 15px; color: #333;"><i class="icon-checkmark-circle" style="margin-right: 8px; color: #4CAF50;"></i>Approved Requests</div>
            <div class="pr-grid-scroll">
                <table class="table datatable-basic table-xxs text-size-small pr-datatable" id="tbl_approved">
                    <thead>
                        <tr>
                            <th>User Name</th>
                            <th>Request Date</th>
                            <th>Time Slot</th>
                            <th>Late Hours</th>
                            <th>Status</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder ID="PH_Approved" runat="server"></asp:PlaceHolder>
                    </tbody>
                </table>
            </div>
        </div>

        <div class="pr-grid-card" id="grid_rejected" style="display:none;">
            <div style="padding: 15px 20px; border-bottom: 1px solid #eee; font-weight: 600; font-size: 15px; color: #333;"><i class="icon-cancel-circle2" style="margin-right: 8px; color: #F44336;"></i>Rejected Requests</div>
            <div class="pr-grid-scroll">
                <table class="table datatable-basic table-xxs text-size-small pr-datatable" id="tbl_rejected">
                    <thead>
                        <tr>
                            <th>User Name</th>
                            <th>Request Date</th>
                            <th>Time Slot</th>
                            <th>Late Hours</th>
                            <th>Status</th>
                            <th>Actions</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder ID="PH_Rejected" runat="server"></asp:PlaceHolder>
                    </tbody>
                </table>
            </div>
        </div>

    </div>

    <div class="modal fade" id="confirmDeleteModal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
                <div class="modal-header bg-danger text-white">
                    <h5 class="modal-title">Confirm Delete</h5>
                    <button type="button" class="close text-white" data-dismiss="modal">
                        <span>&times;</span>
                    </button>
                </div>
                <div class="modal-body text-center">
                    <p class="mb-0">
                        Are you sure you want to remove this record?
                    </p>
                </div>
                <div class="modal-footer justify-content-center">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">
                        No
                    </button>
                    <button type="button" class="btn btn-danger" onclick="confirmDeleteProject()">
                        Yes, Remove
                    </button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
