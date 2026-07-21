<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Holidays.aspx.cs" Inherits="Admin_Holidays" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <style>
        .hm-stat-card {
            background: #fff;
            border-radius: 10px;
            border: 1px solid #eee;
            padding: 16px 18px;
            display: flex;
            align-items: center;
            gap: 14px;
            box-shadow: 0 1px 3px rgba(0,0,0,0.04);
            transition: all 0.3s ease;
        }
        .hm-stat-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 8px 20px rgba(0,0,0,0.08);
        }
        .hm-stat-card:hover .hm-stat-icon i {
            animation: bounceIcon 0.5s ease;
        }
        .hm-stat-icon {
            width: 42px;
            height: 42px;
            border-radius: 8px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 18px;
            flex-shrink: 0;
        }
        .hm-stat-icon i {
            display: inline-block;
            animation: floatIcon 3s ease-in-out infinite;
        }
        @keyframes floatIcon {
            0% { transform: translateY(0px); }
            50% { transform: translateY(-3px); }
            100% { transform: translateY(0px); }
        }
        @keyframes bounceIcon {
            0%, 100% { transform: translateY(0); }
            50% { transform: translateY(-6px) scale(1.1); }
        }
        .hm-icon-blue   { background:#e8f0ff; color:#3b6fe0; }
        .hm-icon-orange { background:#fff1e6; color:#f2994a; }
        .hm-icon-green  { background:#e8f9f0; color:#27ae60; }
        .hm-icon-purple { background:#f3eaff; color:#9b59b6; }
        .hm-stat-label { font-size: 12px; color: #888; margin-bottom: 2px; }
        .hm-stat-value { font-size: 20px; font-weight: 600; color: #222; }
        .hm-stat-sub { font-size: 11px; margin-left: 6px; }
        .hm-sub-up { color: #27ae60; }
        .hm-sub-down { color: #eb5757; }

        .hm-toolbar {
            background: #fff;
            border: 1px solid #eee;
            border-radius: 10px;
            padding: 12px 16px;
            display: flex;
            flex-wrap: nowrap;
            gap: 10px;
            align-items: center;
            justify-content: space-between;
            margin-top: 16px;
            overflow-x: auto;
        }
        .hm-toolbar .form-control { height: 36px; font-size: 13px; min-width: 200px; }
        .hm-toolbar-filters { display: flex; gap: 10px; flex-wrap: nowrap; }

        .badge-type-public     { background:#e8f0ff; color:#3b6fe0; padding:4px 10px; border-radius: 20px; font-size:12px; }
        .badge-type-restricted { background:#f3eaff; color:#9b59b6; padding:4px 10px; border-radius: 20px; font-size:12px; }
        .badge-status-active   { background:#e8f9f0; color:#27ae60; padding:4px 10px; border-radius: 20px; font-size:12px; }
        .badge-status-inactive { background:#fdeaea; color:#eb5757; padding:4px 10px; border-radius: 20px; font-size:12px; }

        .hm-table-card { background:#fff; border:1px solid #eee; border-radius:10px; margin-top:16px; overflow:hidden; }
        .hm-table-card table td { vertical-align: middle; font-size: 13px; }
        .hm-action-icon { cursor:pointer; margin: 0 6px; font-size: 15px; transition: all 0.2s; }
        .hm-action-edit { color: #3b6fe0; }
        .hm-action-edit:hover { color: #2b5bc7; transform: scale(1.15); }
        .hm-action-delete { color: #eb5757; }
        .hm-action-delete:hover { color: #c9302c; transform: scale(1.15); }

        /* Modal Enhancements */
        .modal-content {
            border: none;
            border-radius: 8px;
            box-shadow: 0 5px 15px rgba(0,0,0,0.15);
        }
        .modal-header {
            background: linear-gradient(135deg, #3b6fe0, #5c8aff);
            color: #fff;
            padding: 12px 16px;
            border-bottom: none;
            border-radius: 8px 8px 0 0;
            display: flex;
            align-items: center;
            justify-content: space-between;
        }
        .modal-title {
            font-size: 15px;
            font-weight: 600;
            margin: 0;
            display: flex;
            align-items: center;
            gap: 8px;
        }
        .modal-header .close {
            color: #fff;
            opacity: 0.8;
            text-shadow: none;
            font-size: 20px;
            margin-top: -2px;
        }
        .modal-header .close:hover {
            opacity: 1;
        }
        .modal-body {
            padding: 16px;
            background: #fdfdfd;
        }
        .modal-body .form-group {
            margin-bottom: 12px;
        }
        .modal-body label {
            font-weight: 600;
            color: #444;
            font-size: 12px;
            margin-bottom: 4px;
            display: block;
        }
        .modal-body .form-control {
            border-radius: 4px;
            border: 1px solid #ddd;
            padding: 6px 10px;
            height: 32px;
            font-size: 12px;
            box-shadow: inset 0 1px 2px rgba(0,0,0,0.02);
            transition: all 0.2s;
        }
        .modal-body .form-control:focus {
            border-color: #3b6fe0;
            box-shadow: 0 0 0 3px rgba(59, 111, 224, 0.1);
        }
        .modal-body .input-group-addon {
            border-radius: 4px 0 0 4px;
            border-color: #ddd;
            background-color: #f8f9fa;
            padding: 6px 10px;
            font-size: 12px;
        }
        .modal-footer {
            padding: 12px 16px;
            background: #fff;
            border-top: 1px solid #eee;
            display: flex;
            justify-content: flex-end;
            gap: 8px;
        }
        .modal-footer .btn {
            border-radius: 4px;
            padding: 6px 14px;
            font-weight: 600;
            text-transform: uppercase;
            font-size: 11px;
            letter-spacing: 0.5px;
            transition: all 0.2s;
        }
        .modal-footer .btn-default {
            background: #f1f3f5;
            border: 1px solid #e2e6ea;
            color: #444;
        }
        .modal-footer .btn-default:hover {
            background: #e2e6ea;
        }
        .modal-footer .btn-primary {
            background: #3b6fe0;
            border-color: #3b6fe0;
            box-shadow: 0 4px 10px rgba(59, 111, 224, 0.2);
        }
        .modal-footer .btn-primary:hover {
            background: #2b5bc7;
            transform: translateY(-1px);
            box-shadow: 0 6px 15px rgba(59, 111, 224, 0.3);
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script type="text/javascript">
        function fn_DeleteProject(Holidayskey) {
            if (confirm("Are you sure,you want to remove this?")) {
                $.ajax({
                    type: "POST",
                    url: "Holidays.aspx/DeleteProject",
                    data: "{ Holidayskey: '" + Holidayskey + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: "true",
                    cache: "false",
                    success: function (data, status) {
                        var response = ["success", data];
                        var ResponseData = response[1].d;
                        var ResponseStatus = ResponseData.split("&&&")[0];
                        if (ResponseStatus == "1") {
                            toastr.success("This holiday has been removed.");
                            setTimeout(function(){ location.reload(); }, 2000);
                            return;
                        }
                        else {
                            toastr.error("Sorry, unable to remove this. Please try again.");
                            if (typeof HideLoadingScreen === "function") HideLoadingScreen();
                            return;
                        }
                    },
                    error: function (xhr, status, error) {
                        toastr.error("Sorry, unable to remove this. Please try again.");
                        if (typeof HideLoadingScreen === "function") HideLoadingScreen();
                        return;
                    }
                });
            }
            else {
                HideLoadingScreen();
                return;
            }
        }



        $(document).ready(function () {
            var table = $('.datatable-basic').DataTable({
                "paging": true,
                "info": false
            });

        });

        function fn_OpenAddModal() {
            document.getElementById('<%= hf_holidayKey.ClientID %>').value = '';
            document.getElementById('<%= txt_date.ClientID %>').value = '';
            document.getElementById('<%= txt_day.ClientID %>').value = '';
            document.getElementById('<%= txt_desc.ClientID %>').value = '';
            document.getElementById('<%= txt_nofday.ClientID %>').value = '';
            document.getElementById('<%= ddl_holidayType.ClientID %>').value = 'Public';
            document.getElementById('<%= ddl_holidayStatus.ClientID %>').value = 'Active';
            $('.modal-title').html('<i class="icon-calendar3"></i> Add Holiday Details');

            $('#myModal_AddHoliday').modal('show');
        }

        function fn_EditHoliday(key, date, day, desc, type, status, noofleave) {
            document.getElementById('<%= hf_holidayKey.ClientID %>').value = key;
            document.getElementById('<%= txt_day.ClientID %>').value = day;
            document.getElementById('<%= txt_desc.ClientID %>').value = desc;
            document.getElementById('<%= txt_nofday.ClientID %>').value = noofleave;
            document.getElementById('<%= ddl_holidayType.ClientID %>').value = type;
            document.getElementById('<%= ddl_holidayStatus.ClientID %>').value = status;
            $('.modal-title').html('<i class="icon-pencil7"></i> Edit Holiday Details');

            // Set pickadate value correctly
            var $txt = $('#<%= txt_date.ClientID %>');
            var picker = $txt.pickadate('picker');
            if (picker && date) {
                var parts = date.split('/');
                if (parts.length === 3) {
                    // dd/MM/yyyy → [year, month-1, day]
                    picker.set('select', [parseInt(parts[2]), parseInt(parts[1]) - 1, parseInt(parts[0])]);
                } else {
                    parts = date.split('-');
                    if (parts.length === 3) {
                        // yyyy-MM-dd → [year, month-1, day]
                        picker.set('select', [parseInt(parts[0]), parseInt(parts[1]) - 1, parseInt(parts[2])]);
                    } else {
                        $txt.val(date);
                    }
                }
            } else {
                $txt.val(date);
            }

            $('#myModal_AddHoliday').modal('show');
        }
    </script>

    <!-- Stat Cards -->
    <div class="row">
        <div class="col-md-3 col-sm-6" style="margin-bottom:14px;">
            <div class="hm-stat-card">
                <div class="hm-stat-icon hm-icon-blue"><i class="icon-calendar22"></i></div>
                <div>
                    <div class="hm-stat-label">Total Holidays</div>
                    <div class="hm-stat-value">
                        <asp:Literal ID="lit_total" runat="server" Text="0"></asp:Literal>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-3 col-sm-6" style="margin-bottom:14px;">
            <div class="hm-stat-card">
                <div class="hm-stat-icon hm-icon-orange"><i class="icon-calendar3"></i></div>
                <div>
                    <div class="hm-stat-label">Upcoming Holidays</div>
                    <div class="hm-stat-value">
                        <asp:Literal ID="lit_upcoming" runat="server" Text="0"></asp:Literal>
                        <span class="hm-stat-sub hm-sub-up">in next 30 days</span>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-3 col-sm-6" style="margin-bottom:14px;">
            <div class="hm-stat-card">
                <div class="hm-stat-icon hm-icon-green"><i class="icon-price-tags"></i></div>
                <div>
                    <div class="hm-stat-label">Public Holidays</div>
                    <div class="hm-stat-value">
                        <asp:Literal ID="lit_public" runat="server" Text="0"></asp:Literal>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-3 col-sm-6" style="margin-bottom:14px;">
            <div class="hm-stat-card">
                <div class="hm-stat-icon hm-icon-purple"><i class="icon-price-tag"></i></div>
                <div>
                    <div class="hm-stat-label">Optional Holidays</div>
                    <div class="hm-stat-value">
                        <asp:Literal ID="lit_optional" runat="server" Text="0"></asp:Literal>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Toolbar -->
    <div class="hm-toolbar">
        <div class="hm-toolbar-filters">
            <asp:DropDownList ID="ddl_year" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl_Filter_Changed"></asp:DropDownList>
            <asp:DropDownList ID="ddl_type" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl_Filter_Changed">
                <asp:ListItem Text="Type: All" Value="" />
                <asp:ListItem Text="Public" Value="Public" />
                <asp:ListItem Text="Restricted" Value="Restricted" />
            </asp:DropDownList>
            <asp:DropDownList ID="ddl_status" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl_Filter_Changed">
                <asp:ListItem Text="Status: All" Value="" />
                <asp:ListItem Text="Active" Value="Active" />
                <asp:ListItem Text="Inactive" Value="Inactive" />
            </asp:DropDownList>
        </div>
        <div>
            <button type="button" class="btn btn-primary" onclick="fn_OpenAddModal()">
                <i class="icon-plus3 position-left"></i> Add Holiday
            </button>
        </div>
    </div>

    <!-- Table -->
    <div class="hm-table-card">
        <table class="table datatable-basic" style="margin-bottom:0;">
            <thead>
                <tr>
                    <th>Holiday Name</th>
                    <th>Date</th>
                    <th>Day of the Week</th>
                    <th>Type</th>
                    <th>Status</th>
                    <th class="text-center">Actions</th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rpt_Holidays" runat="server" OnItemCommand="rpt_Holidays_ItemCommand">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <i class="icon-calendar22 position-left" style="color: #3b6fe0; font-size: 14px;"></i>
                                <%# Eval("description") %>
                            </td>
                            <td><%# Eval("Holidays") %></td>
                            <td><%# Eval("Day") %></td>
                            <td>
                                <span class='<%# Eval("HolidayType").ToString() == "Restricted" ? "badge-type-restricted" : "badge-type-public" %>'>
                                    <%# Eval("HolidayType") %>
                                </span>
                            </td>
                            <td>
                                <span class='<%# Eval("Status").ToString() == "Inactive" ? "badge-status-inactive" : "badge-status-active" %>'>
                                    <%# Eval("Status") %>
                                </span>
                            </td>
                            <td class="text-center">
                                <i class="icon-pencil7 hm-action-icon hm-action-edit" onclick="fn_EditHoliday('<%# Eval("Holidayskey") %>', '<%# Eval("Holidays") %>', '<%# Eval("Day") %>', '<%# Eval("description") %>', '<%# Eval("HolidayType") %>', '<%# Eval("Status") %>', '<%# Eval("NoOfLeave") %>')"></i>
                                <i class="icon-trash hm-action-icon hm-action-delete" onclick="fn_DeleteProject('<%# Eval("Holidayskey") %>')"></i>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </tbody>
        </table>
    </div>

    <!-- Add/Edit Modal -->
    <div class="modal fade" id="myModal_AddHoliday" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title"><i class="icon-calendar3"></i> Holiday Details</h5>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="hf_holidayKey" runat="server" />
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Date :<span style="color:red"> *</span></label>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                    <asp:TextBox ID="txt_date" runat="server" CssClass="form-control pickadate" required="required" placeholder="DD/MM/YYYY"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Day :<span style="color:red">*</span></label>
                                <asp:TextBox ID="txt_day" runat="server" CssClass="form-control" required="required"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Description : <span style="color:red">*</span></label>
                                <asp:TextBox ID="txt_desc" runat="server" CssClass="form-control" required="required"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>No of day :<span style="color:red">*</span></label>
                                <asp:TextBox ID="txt_nofday" runat="server" CssClass="form-control" required="required"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Type :</label>
                                <asp:DropDownList ID="ddl_holidayType" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Public" Value="Public" />
                                    <asp:ListItem Text="Restricted" Value="Restricted" />
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Status :</label>
                                <asp:DropDownList ID="ddl_holidayStatus" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Active" Value="Active" />
                                    <asp:ListItem Text="Inactive" Value="Inactive" />
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                    <asp:Button ID="btn_submit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btn_submit_Click"></asp:Button>
                </div>
            </div>
        </div>
    </div>

    <script>
        $(document).ready(function () {
            try {
                if ($('.pickadate').length > 0) {
                    $('.pickadate').pickadate({
                        format: 'yyyy-mm-dd',
                        selectMonths: true,
                        selectYears: true,
                        closeOnSelect: true
                    });
                }
            } catch(e) { console.error("Pickadate err", e); }

            try {
                if (window.location.search.indexOf('key=') > -1 || window.location.search.indexOf('add=1') > -1) {
                    setTimeout(function() {
                        $('#myModal_AddHoliday').modal('show');
                    }, 400);
                }
            } catch(e) { console.error("Modal err", e); }
        });
    </script>
</asp:Content>
