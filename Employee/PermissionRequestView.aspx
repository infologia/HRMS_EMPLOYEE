<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="PermissionRequestView.aspx.cs" Inherits="WEB_Employee_PermissionRequestView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        /* Ensure date/time pickers appear above the bootstrap modal */
        .picker, .picker--opened {
            z-index: 1060 !important;
        }
    </style>
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
                url: "PermissionRequestView.aspx/DeleteProject",
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
            // Standard options from global config to maintain look & feel, 
            // plus order by column 5 (Applied On) descending
            var dtOptions = {
                "order": [[5, "desc"]],
                "autoWidth": false
            };

            // Initialize DataTables manually (prevents issues with hidden tables)
            tableAll = $('#tbl_all').DataTable(dtOptions);
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
            var activeTable;
            var tblId = '#tbl_' + filter;
            if ($.fn.DataTable.isDataTable(tblId)) {
                $(tblId).DataTable().columns.adjust();
            }
        }

        function checkLeave(dateStr) {
            if (!dateStr) return;
            $.ajax({
                type: "POST",
                url: "PermissionRequestView.aspx/CheckLeaveForDate",
                data: JSON.stringify({ dateStr: dateStr }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    var btnId = '<%= btn_request.ClientID %>';
                    if (data.d === "true") {
                        $('#lbl_leave_warning').show();
                        $('#' + btnId).prop('disabled', true);
                    } else {
                        $('#lbl_leave_warning').hide();
                        $('#' + btnId).prop('disabled', false);
                    }
                },
                error: function () {
                    // Ignore error silently
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="pr-wrap">

        <div class="pr-header">
            <h1>Permission Requests</h1>
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
                <a href="#" class="btn-create-permission" data-toggle="modal" data-target="#newRequestModal" style="margin-left: 5px;">
                    <i class="icon-plus-circle2"></i> New Request
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
                            <th>Request Date</th>
                            <th>Time Slot</th>
                            <th>Hours</th>
                            <th>Reason</th>
                            <th>Status</th>
                            <th>Applied On</th>
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
                            <th>Request Date</th>
                            <th>Time Slot</th>
                            <th>Hours</th>
                            <th>Reason</th>
                            <th>Status</th>
                            <th>Applied On</th>
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
                            <th>Request Date</th>
                            <th>Time Slot</th>
                            <th>Hours</th>
                            <th>Reason</th>
                            <th>Status</th>
                            <th>Applied On</th>
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
                            <th>Request Date</th>
                            <th>Time Slot</th>
                            <th>Hours</th>
                            <th>Reason</th>
                            <th>Status</th>
                            <th>Applied On</th>
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

    <!-- NEW REQUEST MODAL -->
    <div class="modal fade" id="newRequestModal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">

                <div class="modal-header bg-primary text-white">
                    <h5 class="modal-title"><i class="icon-pencil4"></i> New Permission Request</h5>
                    <button type="button" class="close text-white" data-dismiss="modal">
                        <span>&times;</span>
                    </button>
                </div>

                <div class="modal-body">
                   <div class="row">
                       <div class="col-md-6">
                           <label class="content-group text-semibold">Request Date <span style="color: red">*</span> </label>
                           <div class="input-group">
                               <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                               <asp:TextBox ID="txt_date" runat="server" class="form-control pickadate" onchange="checkLeave(this.value)"></asp:TextBox>
                           </div>
                           <div>
                               <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txt_date" ErrorMessage="Request Date is required" ForeColor="Red" Display="Dynamic" ValidationGroup="new_req"></asp:RequiredFieldValidator><span id="lbl_leave_warning" style="color: red; display: none; ">You are on leave on this date.</span>
                           </div>
                       </div>
                       <div class="col-md-6">
                           <label class="content-group text-semibold">From Time <span style="color: red">*</span> </label>
                           <div class="input-group">
                               <span class="input-group-addon"><i class="icon-alarm"></i></span>
                               <asp:TextBox ID="txt_fromtime" runat="server" TextMode="Time" CssClass="form-control pickatime-clear"></asp:TextBox>
                           </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_fromtime" ErrorMessage="From Time is required" ForeColor="Red" Display="Dynamic" ValidationGroup="new_req"></asp:RequiredFieldValidator>
                       </div>
                   </div>
                   <br/>
                   <div class="row">
                       <div class="col-md-6">
                           <label class="content-group text-semibold">To Time <span style="color: red">*</span> </label>
                           <div class="input-group">
                               <span class="input-group-addon"><i class="icon-alarm"></i></span>
                               <asp:TextBox ID="txt_totime" runat="server" TextMode="Time" class="form-control pickatime-clear"></asp:TextBox>
                           </div>
                           <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_totime" ErrorMessage="To Time is required" ForeColor="Red" Display="Dynamic" ValidationGroup="new_req"></asp:RequiredFieldValidator>
                       </div>
            
                       <div class="col-md-6">
                           <label class="content-group text-semibold">Reason <span style="color: red">*</span> </label>
                           <asp:TextBox ID="txt_reasons" runat="server" TextMode="MultiLine" Rows="2" Columns="4" CssClass="form-control"></asp:TextBox>
                           <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_reasons" ErrorMessage="Reason is required" ForeColor="Red" Display="Dynamic" ValidationGroup="new_req"></asp:RequiredFieldValidator>
                       </div>
                   </div>
                </div>

                <div class="modal-footer justify-content-center">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancel</button>
                    <asp:Button ID="btn_request" runat="server" Text="Request" OnClick="btn_perm_Click" class="btn btn-primary" ValidationGroup="new_req" UseSubmitBehavior="false"></asp:Button>
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
                    <h5 class="modal-title" id="updateModalTitle"><i class="icon-pencil4"></i> Update Permission Request</h5>
                    <button type="button" class="close text-white" data-dismiss="modal">
                        <span>&times;</span>
                    </button>
                </div>

                <div class="modal-body">
                   <div class="row">
                       <div class="col-md-6">
                           <label class="content-group text-semibold">Request Date </label>
                           <div class="input-group">
                               <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                               <asp:TextBox ID="upd_txt_date" runat="server" CssClass="form-control pickadate"></asp:TextBox>
                           </div>
                           <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="upd_txt_date" runat="server" ErrorMessage="Please select Date" ForeColor="Red" Display="Dynamic" ValidationGroup="upd_req"></asp:RequiredFieldValidator>
                       </div>

                       <div class="col-md-6">
                           <label class="content-group text-semibold">From Time <span style="color: red">*</span></label>
                           <div class="input-group">
                               <span class="input-group-addon"><i class="icon-alarm"></i></span>
                               <asp:TextBox ID="upd_txt_fromtime" runat="server" CssClass="form-control pickatime-clear"></asp:TextBox>
                           </div>
                           <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ControlToValidate="upd_txt_fromtime" runat="server" ErrorMessage="Please select From time" ForeColor="Red" Display="Dynamic" ValidationGroup="upd_req"></asp:RequiredFieldValidator>
                       </div>
                   </div>
                   <br/>
                   <div class="row">
                       <div class="col-md-6">
                           <label class="content-group text-semibold">To Time <span style="color: red">*</span></label>
                           <div class="input-group">
                               <span class="input-group-addon"><i class="icon-alarm"></i></span>
                               <asp:TextBox ID="upd_txt_totime" runat="server" class="form-control pickatime-clear" required=""></asp:TextBox>
                           </div>
                           <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ControlToValidate="upd_txt_totime" runat="server" ErrorMessage="Please select To Time" ForeColor="Red" Display="Dynamic" ValidationGroup="upd_req"></asp:RequiredFieldValidator>
                       </div>

                       <div class="col-md-6">
                           <label class="content-group text-semibold">Reason<span style="color: red">*</span></label>
                           <asp:TextBox ID="upd_txt_reasons" runat="server" TextMode="MultiLine" Rows="1" CssClass="form-control"></asp:TextBox>
                           <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ControlToValidate="upd_txt_reasons" runat="server" ErrorMessage="Enter a reason" ForeColor="Red" Display="Dynamic" ValidationGroup="upd_req"></asp:RequiredFieldValidator>
                       </div>
                   </div>
                   <br/>
                   <div class="row" id="upd_div_Reson" runat="server" style="display:none;">
                       <div class="col-md-6">
                           <label class="content-group text-semibold">Status <span style="color: red">*</span></label>
                           <asp:DropDownList ID="upd_ddl_category" runat="server" class="form-control">
                           </asp:DropDownList>
                       </div>
                       <div class="col-md-6">
                           <label class="content-group text-semibold">Admin Reason <span style="color: red">*</span></label>
                           <textarea id="upd_txt_reason1" runat="server" rows="1" class="form-control"></textarea>
                       </div>
                   </div>
                </div>

                <div class="form-group">
                    <div class="text-right" style="padding: 15px;">
                        <button type="button" class="btn btn-secondary" data-dismiss="modal" style="margin-right: 15px">Back</button>
                        <asp:Button ID="btn_update" runat="server" Text="Update" OnClick="btn_update_Click" class="btn btn-primary" ValidationGroup="upd_req" UseSubmitBehavior="false"></asp:Button>
                    </div>
                </div>

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

    <!-- Move script to bottom exactly like the original page -->
    <script type="text/javascript">
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
                    checkLeave(selectedDateStr);
                }
            }
        });

        function checkLeave(dateStr) {
            if (!dateStr) return;
            $.ajax({
                type: "POST",
                url: "PermissionRequestView.aspx/CheckLeaveForDate",
                data: JSON.stringify({ dateStr: dateStr }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d === "true") {
                        $('#lbl_leave_warning').show();
                        $('#<%= btn_request.ClientID %>').prop('disabled', true);
                    } else {
                        $('#lbl_leave_warning').hide();
                        $('#<%= btn_request.ClientID %>').prop('disabled', false);
                    }
                },
                error: function () {
                    // Ignore error silently
                }
            });
        }

        // --- UPDATE / VIEW REQUEST JS ---
        function openUpdateModal(id, mode) {
            // Set hidden ID
            $('#<%= hdn_update_id.ClientID %>').val(id);

            // Fetch data
            $.ajax({
                type: "POST",
                url: "PermissionRequestView.aspx/GetPermissionDetails",
                data: JSON.stringify({ id: id }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    var data = response.d;
                    if (data) {
                        $('#<%= upd_txt_date.ClientID %>').val(data.Requestdate);
                        $('#<%= upd_txt_fromtime.ClientID %>').val(data.Fromtime);
                        $('#<%= upd_txt_totime.ClientID %>').val(data.Totime);
                        $('#<%= upd_txt_reasons.ClientID %>').val(data.Reason);
                        
                        var ddl = $('#<%= upd_ddl_category.ClientID %>');
                        if(ddl.length > 0) ddl.val(data.Responsestatus);
                        
                        $('#<%= upd_txt_reason1.ClientID %>').val(data.responsereason);

                        var btnUpdate = $('#<%= btn_update.ClientID %>');
                        var divReason = $('#<%= upd_div_Reson.ClientID %>');
                        var modalTitle = $('#updateModalTitle');

                        // Reset readonly states
                        $('#<%= upd_txt_date.ClientID %>, #<%= upd_txt_fromtime.ClientID %>, #<%= upd_txt_totime.ClientID %>, #<%= upd_txt_reasons.ClientID %>, #<%= upd_txt_reason1.ClientID %>').prop('readonly', false);
                        ddl.prop('disabled', false);

                        if (mode === 'view' || data.Responsestatus === '2') {
                            // View Mode (or already approved)
                            modalTitle.html('<i class="icon-eye"></i> View Permission Request');
                            divReason.show();
                            btnUpdate.hide();

                            $('#<%= upd_txt_date.ClientID %>, #<%= upd_txt_fromtime.ClientID %>, #<%= upd_txt_totime.ClientID %>, #<%= upd_txt_reasons.ClientID %>, #<%= upd_txt_reason1.ClientID %>').prop('readonly', true);
                            ddl.prop('disabled', true);
                        } else {
                            // Edit Mode (Pending or Rejected)
                            modalTitle.html('<i class="icon-pencil7"></i> Update Permission Request');
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
