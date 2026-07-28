<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="LeaveRequestView.aspx.cs" Inherits="WEB_Employee_LeaveRequestView" %>

<asp:Content ID="Content3" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        var deleteLeaveKey = "";
        function fn_DeleteProject(employeeleavedetailskey) {
            deleteLeaveKey = employeeleavedetailskey;
            $('#confirmDeleteModal').modal('show');
        }

        function confirmDeleteProject() {
            if (deleteLeaveKey === "") {
                return;
            }
            $.ajax({
                type: "POST",
                url: "LeaveRequestView.aspx/DeleteProject",
                data: JSON.stringify({ str_employeeleavedetailskey: deleteLeaveKey }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data, status) {
                    $('#confirmDeleteModal').modal('hide');
                    var response = ["success", data];
                    var ResponseData = response[1].d;
                    var ResponseStatus = ResponseData.split("&&&")[0];
                    if (ResponseStatus == "1") {
                        showToastr('success', 'Leave Request Deleted Successfully!');
                        setTimeout(function () { window.location.reload(); }, 1500);
                    }
                    else {
                        showToastr('error', 'Sorry, unable to remove this, please try after sometime.');
                    }
                },
                error: function (xhr, status, error) {
                    $('#confirmDeleteModal').modal('hide');
                    showToastr('error', 'Sorry, unable to remove this, please try after sometime.');
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="pr-wrap">

        <div class="pr-header">
            <h1>Leave Requests</h1>
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
                <a href="#" class="btn-create-permission" data-toggle="modal" data-target="#newRequestModal" onclick="clearNewRequestForm()" style="margin-left: 5px;">
                    <i class="icon-plus-circle2"></i> Create Leave
                </a>
            </div>
        </div>

        <div class="pr-summary">
            <div class="pr-stat-card active" id="card_all" onclick="filterByCard('all')" style="cursor:pointer;">
                <div class="pr-stat-icon total"><i class="icon-users4"></i></div>
                <div>
                    <span class="pr-stat-label">Total Leaves</span>
                    <p class="pr-stat-value"><asp:Label ID="badge_overview" runat="server" Text="0"></asp:Label></p>
                </div>
            </div>
            <div class="pr-stat-card" id="card_pending" onclick="filterByCard('pending')" style="cursor:pointer;">
                <div class="pr-stat-icon pending"><i class="icon-history"></i></div>
                <div>
                    <span class="pr-stat-label">Pending Leaves</span>
                    <p class="pr-stat-value"><asp:Label ID="badge_pending" runat="server" Text="0"></asp:Label></p>
                </div>
            </div>
            <div class="pr-stat-card" id="card_approved" onclick="filterByCard('approved')" style="cursor:pointer;">
                <div class="pr-stat-icon approved"><i class="icon-checkmark-circle"></i></div>
                <div>
                    <span class="pr-stat-label">Approved Leaves</span>
                    <p class="pr-stat-value"><asp:Label ID="badge_approved" runat="server" Text="0"></asp:Label></p>
                </div>
            </div>
            <div class="pr-stat-card" id="card_rejected" onclick="filterByCard('rejected')" style="cursor:pointer;">
                <div class="pr-stat-icon rejected"><i class="icon-cancel-circle2"></i></div>
                <div>
                    <span class="pr-stat-label">Rejected Leaves</span>
                    <p class="pr-stat-value"><asp:Label ID="badge_rejected" runat="server" Text="0"></asp:Label></p>
                </div>
            </div>
        </div>

        <div class="pr-grid-card" id="grid_all">
            <div style="padding: 15px 20px; border-bottom: 1px solid #eee; font-weight: 600; font-size: 15px; color: #333;"><i class="icon-users4" style="margin-right: 8px; color: #888;"></i>All Leaves</div>
            <div class="pr-grid-scroll">
                <table class="table datatable-basic table-xxs text-size-small pr-datatable" id="tbl_all_i">
                    <thead>
                        <tr>
                            <th>User Name</th>
                            <th>From Date</th>
                            <th>To Date</th>
                            <th>Status</th>
                            <th>Leave Days</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder ID="PH_Overview" runat="server"></asp:PlaceHolder>
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
                            <th>User Name</th>
                            <th>From Date</th>
                            <th>To Date</th>
                            <th>Status</th>
                            <th>Leave Days</th>
                            <th>Action</th>
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
                            <th>User Name</th>
                            <th>From Date</th>
                            <th>To Date</th>
                            <th>Status</th>
                            <th>Leave Days</th>
                            <th>Action</th>
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
                            <th>User Name</th>
                            <th>From Date</th>
                            <th>To Date</th>
                            <th>Status</th>
                            <th>Leave Days</th>
                            <th>Action</th>
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

    <!-- NEW REQUEST MODAL -->
    <div class="modal fade" id="newRequestModal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title"><i class="icon-pencil5"></i> New Leave Request</h5>
                    <button type="button" class="close text-white" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-6">
                            <label class="content-group text-semibold">From Date <span style="color: red">*</span> </label>
                            <div class="input-group">
                                <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                <asp:TextBox ID="txt_fromdate" runat="server" class="form-control pickadate" placeholder="DD/MM/YYYY" onchange="checkLeaveDate(this.value, 'from')"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txt_fromdate" runat="server" ErrorMessage="Please select fromdate" ForeColor="Red" ValidationGroup="new_req" Display="Dynamic"></asp:RequiredFieldValidator>
                            <span id="lbl_fromdate_warning" style="color: red; display: none;">Leave has already been applied for this date.</span>
                        </div>
                        <div class="col-md-6">
                            <label class="content-group text-semibold">To Date <span style="color: red">*</span> </label>
                            <div class="input-group">
                                <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                <asp:TextBox ID="txt_todate" runat="server" class="form-control pickadate" placeholder="DD/MM/YYYY" onchange="checkLeaveDate(this.value, 'to')"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txt_todate" runat="server" ErrorMessage="Please select todate" ForeColor="Red" ValidationGroup="new_req" Display="Dynamic"></asp:RequiredFieldValidator>
                            <span id="lbl_todate_warning" style="color: red; display: none;">Leave has already been applied for this date.</span>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-6">
                            <label class="content-group text-semibold">Leave Category <span style="color: red">*</span> </label>
                            <asp:DropDownList ID="ddl_leavecategory" runat="server" class="form-control">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="ddl_leavecategory" InitialValue="0" runat="server" ErrorMessage="Please select leave category" ForeColor="Red" ValidationGroup="new_req" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-6">
                            <label class="content-group text-semibold">Leave Type <span style="color: red">*</span> </label>
                            <asp:DropDownList ID="ddl_leavetype" runat="server" class="form-control">
                                <asp:ListItem Value="" Text="Select Leave Type"></asp:ListItem>
                                <asp:ListItem Value="0" Text="Half Day (Forenoon)"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Half Day (Afternoon)"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Full Day"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddl_leavetype" InitialValue="" runat="server" ErrorMessage="Please select leavetype" ForeColor="Red" ValidationGroup="new_req" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-12">
                            <label class="content-group text-semibold">Reason <span style="color: red">*</span> </label>
                            <textarea id="txt_reason" runat="server" rows="2" class="form-control"></textarea>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="txt_reason" runat="server" ErrorMessage="Enter a reason" ForeColor="Red" ValidationGroup="new_req" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </div>
                <div class="modal-footer justify-content-center">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                    <asp:Button ID="btn_request" runat="server" Text="Request" OnClick="btn_request_Click" class="btn btn-primary" ValidationGroup="new_req"></asp:Button>
                </div>
            </div>
        </div>
    </div>

    <!-- UPDATE / VIEW REQUEST MODAL -->
    <asp:HiddenField ID="hdn_update_id" runat="server" />
    <div class="modal fade" id="updateRequestModal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title" id="updateModalTitle"><i class="icon-pencil5"></i> Update Leave Request</h5>
                    <button type="button" class="close text-white" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-6">
                            <label class="content-group text-semibold">From Date <span style="color: red">*</span> </label>
                            <div class="input-group">
                                <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                <asp:TextBox ID="upd_txt_fromdate" runat="server" CssClass="form-control pickadate"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="upd_txt_fromdate" runat="server" ErrorMessage="Please select fromdate" ForeColor="Red" ValidationGroup="upd_req" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-6">
                            <label class="content-group text-semibold">To Date <span style="color: red">*</span> </label>
                            <div class="input-group">
                                <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                <asp:TextBox ID="upd_txt_todate" runat="server" Class="form-control pickadate"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ControlToValidate="upd_txt_todate" runat="server" ErrorMessage="Please select todate" ForeColor="Red" ValidationGroup="upd_req" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-6">
                            <label class="content-group text-semibold">Leave Category <span style="color: red">*</span> </label>
                            <asp:DropDownList ID="upd_ddl_leavecategory" runat="server" class="form-control">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ControlToValidate="upd_ddl_leavecategory" InitialValue="0" runat="server" ErrorMessage="Please select leave category" ForeColor="Red" ValidationGroup="upd_req" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-6">
                            <label class="content-group text-semibold">Leave Type <span style="color: red">*</span> </label>
                            <asp:DropDownList ID="upd_ddl_leavetype" runat="server" class="form-control">
                                <asp:ListItem Value="" Text="Select Leave Type"></asp:ListItem>
                                <asp:ListItem Value="0" Text="Half Day (Forenoon)"></asp:ListItem>
                                <asp:ListItem Value="1" Text="Half Day (Afternoon)"></asp:ListItem>
                                <asp:ListItem Value="2" Text="Full Day"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ControlToValidate="upd_ddl_leavetype" InitialValue="" runat="server" ErrorMessage="Please select leavetype" ForeColor="Red" ValidationGroup="upd_req" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-md-12">
                            <label class="content-group text-semibold">Reason <span style="color: red">*</span> </label>
                            <textarea id="upd_txt_reason" runat="server" rows="2" class="form-control"></textarea>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ControlToValidate="upd_txt_reason" runat="server" ErrorMessage="Enter a reason" ForeColor="Red" ValidationGroup="upd_req" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <br />
                    <div class="row" id="upd_div_Reson" runat="server" style="display:none;">
                        <div class="col-md-6">
                            <label class="content-group text-semibold">Status <span style="color: red">*</span> </label>
                            <asp:DropDownList ID="upd_ddl_category" runat="server" class="form-control">
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-6">
                            <label class="content-group text-semibold">Admin Reason <span style="color: red">*</span> </label>
                            <textarea id="upd_txt_reason1" runat="server" rows="1" class="form-control"></textarea>
                        </div>
                    </div>
                </div>
                <div class="modal-footer justify-content-center">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal" style="margin-right: 15px">Back</button>
                    <asp:Button ID="btn_update" runat="server" Text="Update" OnClick="btn_update_Click" CssClass="btn btn-primary" ValidationGroup="upd_req"></asp:Button>
                </div>
            </div>
        </div>
    </div>

    <script>
        var today = new Date();
        $('.pickadate').pickadate({
            format: 'dd/mm/yyyy',
            min: today,
            selectMonths: true,
            selectYears: true,
            closeOnSelect: true,
            onSet: function(context) {
                if (context.select) {
                    var selectedDateStr = this.get('select', 'dd/mm/yyyy');
                    // We need to figure out which input triggered this to pass 'from' or 'to'.
                    // So we will just rely on the onchange event inline in the input instead.
                }
            }
        });

        function checkLeaveDate(dateStr, type) {
            if (!dateStr) return;
            $.ajax({
                type: "POST",
                url: "LeaveRequestView.aspx/CheckLeaveDateExists",
                data: JSON.stringify({ dateStr: dateStr, type: type }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var btnId = '<%= btn_request.ClientID %>';
                    if (data.d === "true") {
                        if(type === 'from') $('#lbl_fromdate_warning').show();
                        if(type === 'to') $('#lbl_todate_warning').show();
                        $('#' + btnId).prop('disabled', true);
                    } else {
                        if(type === 'from') $('#lbl_fromdate_warning').hide();
                        if(type === 'to') $('#lbl_todate_warning').hide();
                        
                        // Only enable button if BOTH warnings are hidden
                        if ($('#lbl_fromdate_warning').is(':hidden') && $('#lbl_todate_warning').is(':hidden')) {
                            $('#' + btnId).prop('disabled', false);
                        }
                    }
                },
                error: function () {
                }
            });
        }

        function clearNewRequestForm() {
            // Reset text fields
            $('#<%= txt_fromdate.ClientID %>').val('');
            $('#<%= txt_todate.ClientID %>').val('');
            $('#<%= txt_reason.ClientID %>').val('');
            
            // Reset dropdowns
            var ddlCategory = $('#<%= ddl_leavecategory.ClientID %>');
            if(ddlCategory.length > 0) ddlCategory.val('0');
            
            var ddlType = $('#<%= ddl_leavetype.ClientID %>');
            if(ddlType.length > 0) ddlType.val('');
            
            // Hide warnings and enable button
            $('#lbl_fromdate_warning').hide();
            $('#lbl_todate_warning').hide();
            $('#<%= btn_request.ClientID %>').prop('disabled', false);
        }

        var currentFilter = 'all';
        var tableAll, tablePending, tableApproved, tableRejected;

        $(document).ready(function () {
            // Standard options from global config to maintain look & feel
            var dtOptions = {
                "autoWidth": false
            };

            // Initialize DataTables manually (prevents issues with hidden tables)
            tableAll = $('#tbl_all_i').DataTable(dtOptions);
            tablePending = $('#tbl_pending').DataTable(dtOptions);
            tableApproved = $('#tbl_approved').DataTable(dtOptions);
            tableRejected = $('#tbl_rejected').DataTable(dtOptions);
        });

        function filterByCard(filter) {
            currentFilter = filter;
            
            // Update active card styling
            $('.pr-stat-card').removeClass('active');
            $('#card_' + filter).addClass('active');
            
            // Hide all grids
            $('.pr-grid-card').hide();
            
            // Show the selected grid
            $('#grid_' + filter).show();
            
            // Adjust columns for the active table
            var tblId = '#tbl_' + filter;
            if ($.fn.DataTable.isDataTable(tblId)) {
                $(tblId).DataTable().columns.adjust();
            }
        }

        // --- UPDATE / VIEW REQUEST JS ---
        function openUpdateModal(id, mode) {
            // Set hidden ID
            $('#<%= hdn_update_id.ClientID %>').val(id);

            // Fetch data
            $.ajax({
                type: "POST",
                url: "LeaveRequestView.aspx/GetLeaveDetails",
                data: JSON.stringify({ id: id }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    if (data) {
                        $('#<%= upd_txt_fromdate.ClientID %>').val(data.Fromdate);
                        $('#<%= upd_txt_todate.ClientID %>').val(data.Todate);
                        $('#<%= upd_txt_reason.ClientID %>').val(data.Reason);
                        $('#<%= upd_ddl_leavetype.ClientID %>').val(data.LeaveType);
                        $('#<%= upd_ddl_leavecategory.ClientID %>').val(data.LeaveCategoryId);
                        
                        var ddl = $('#<%= upd_ddl_category.ClientID %>');
                        if(ddl.length > 0) ddl.val(data.Responsestatus);
                        
                        $('#<%= upd_txt_reason1.ClientID %>').val(data.Responsereason);

                        var btnUpdate = $('#<%= btn_update.ClientID %>');
                        var divReason = $('#<%= upd_div_Reson.ClientID %>');
                        var modalTitle = $('#updateModalTitle');

                        // Reset readonly states
                        $('#<%= upd_txt_fromdate.ClientID %>, #<%= upd_txt_todate.ClientID %>, #<%= upd_txt_reason.ClientID %>, #<%= upd_txt_reason1.ClientID %>').prop('readonly', false);
                        $('#<%= upd_ddl_leavetype.ClientID %>').prop('disabled', false);
                        $('#<%= upd_ddl_leavecategory.ClientID %>').prop('disabled', false);
                        ddl.prop('disabled', false);

                        if (mode === 'view' || data.Responsestatus === '2') {
                            // View Mode (or Approved)
                            modalTitle.html('<i class="icon-eye"></i> View Leave Request');
                            divReason.show();
                            btnUpdate.hide();

                            $('#<%= upd_txt_fromdate.ClientID %>, #<%= upd_txt_todate.ClientID %>, #<%= upd_txt_reason.ClientID %>, #<%= upd_txt_reason1.ClientID %>').prop('readonly', true);
                            $('#<%= upd_ddl_leavetype.ClientID %>').prop('disabled', true);
                            $('#<%= upd_ddl_leavecategory.ClientID %>').prop('disabled', true);
                            ddl.prop('disabled', true);
                        } else {
                            // Edit Mode (Pending or Rejected)
                            modalTitle.html('<i class="icon-pencil7"></i> Update Leave Request');
                            divReason.hide();
                            btnUpdate.show();
                        }

                        $('#updateRequestModal').modal('show');
                    }
                },
                error: function () {
                    showToastr('error', 'Failed to load details.');
                }
            });
        }
    </script>
</asp:Content>
