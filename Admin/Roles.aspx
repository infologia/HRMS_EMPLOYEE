<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Roles.aspx.cs" Inherits="Admin_Roles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/bootstrap_multiselect.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/form_multiselect.js"></script>
        <style>

        /* Fix multiselect checkbox display */

        .multiselect-container > li > a > label {

            padding: 3px 20px 3px 40px !important;

            margin: 0 !important;

        }

        .multiselect-container > li > a > label > input[type="checkbox"] {

            position: absolute;

            left: 10px;

            top: 50%;

            transform: translateY(-50%);

            margin: 0 !important;

            width: 17px;

            height: 17px;

        }

        .multiselect-container {

            max-height: 250px;

            overflow-y: auto;

        }

        .multiselect-container > li > a {

            padding: 0 !important;

        }

        .multiselect-container > li > a:hover {

            background-color: #f5f5f5;

        }

        .multi-select-full .btn-group {

            width: 100%;

        }

        .multi-select-full .multiselect {

            width: 100%;

            text-align: left;

        }
</style>

 
    <script type="text/javascript">
        var deleteRoleKey = "";
        var currentRoleId = "";

        function openModal(roleId, roleName, moduleIds, description) {
            var listBox = document.getElementById('<%= lstModules.ClientID %>');
            
            // Clear all selections first
            for (var i = 0; i < listBox.options.length; i++) {
                listBox.options[i].selected = false;
            }
            
            if (roleId) {
                currentRoleId = roleId;
                $('#modalTitle').text('Edit Role');
                $('#txt_RoleName').val(roleName);
                $('#txt_Description').val(description);
                
                // Select the modules
                if (moduleIds && moduleIds.trim() !== '') {
                    var selectedModules = moduleIds.split(',');
                    
                    for (var i = 0; i < listBox.options.length; i++) {
                        var optionValue = listBox.options[i].value.trim();
                        for (var j = 0; j < selectedModules.length; j++) {
                            if (optionValue === selectedModules[j].trim()) {
                                listBox.options[i].selected = true;
                                break;
                            }
                        }
                    }
                }
                $('#btnSave').text('Update');
            } else {
                currentRoleId = "";
                $('#modalTitle').text('Create Role');
                $('#txt_RoleName').val('');
                $('#txt_Description').val('');
                $('#btnSave').text('Save');
            }
            
            $('#roleModal').modal('show');
            
            // Rebuild multiselect after modal is shown
            setTimeout(function() {
                $('#<%= lstModules.ClientID %>').multiselect('rebuild');
            }, 200);
        }

        function SaveRole() {
            var roleName = $('#txt_RoleName').val().trim();
            var description = $('#txt_Description').val().trim();
            
            var listBox = document.getElementById('<%= lstModules.ClientID %>');
            var selectedModules = [];
            for (var i = 0; i < listBox.options.length; i++) {
                if (listBox.options[i].selected) {
                    selectedModules.push(listBox.options[i].value);
                }
            }
            var moduleIds = selectedModules.join(',');

            if (roleName === '') {
                showToastr('error', 'Role name is required!');
                $('#txt_RoleName').focus();
                return false;
            }

            var method = currentRoleId ? "UpdateRole" : "SaveRole";
            var data = currentRoleId 
                ? JSON.stringify({ roleId: currentRoleId, roleName: roleName, moduleIds: moduleIds, description: description })
                : JSON.stringify({ roleName: roleName, moduleIds: moduleIds, description: description });

            $.ajax({
                type: "POST",
                url: "Roles.aspx/" + method,
                data: data,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(response) {
                    if (response.d === "true") {
                        var msg = currentRoleId ? 'Role updated successfully!' : 'Role saved successfully!';
                        showToastr('success', msg);
                        $('#roleModal').modal('hide');
                        setTimeout(function() { location.reload(); }, 1500);
                    } else {
                        showToastr('error', 'Failed to save role');
                    }
                },
                error: function() {
                    showToastr('error', 'Server error. Please try again.');
                }
            });
        }

        function fn_DeleteRole(roleid) {
            deleteRoleKey = roleid;
            $('#confirmDeleteModal').modal('show');
        }

        function confirmDeleteRole() {
            if (deleteRoleKey === "") {
                return;
            }

            $.ajax({
                type: "POST",
                url: "Roles.aspx/DeleteRole",
                data: JSON.stringify({ str_roleid: deleteRoleKey }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(data) {
                    $('#confirmDeleteModal').modal('hide');
                    if (data.d === "1") {
                        showToastr('success', 'Role deleted successfully');
                        setTimeout(function() { location.reload(); }, 1500);
                    } else {
                        showToastr('error', 'Unable to delete role. Please try again.');
                    }
                },
                error: function() {
                    $('#confirmDeleteModal').modal('hide');
                    showToastr('error', 'Server error. Please try again.');
                }
            });
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="panel panel-flat">
        <div class="panel-heading">
            <div class="row">
                <div class="col-lg-4">
                    <h5 class="panel-title">Roles</h5>
                </div><br />
                <div class="col-lg-8">
                    <button type="button" onclick="openModal();" class="btn btn-primary pull-right">
                        <i class="icon-plus-circle2"></i> Create New
                    </button>
                </div>
            </div>
        </div>

        <div class="panel-body" style="padding: 0px;"></div>

        <table class="table datatable-basic">
            <thead>
                <tr>
                    <th>Role Name</th>
                    <th>Modules</th>
                    <th>Description</th>
                    <th>Edit</th>
                    <th>Delete</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_Roles" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
    </div>

    <!-- Create/Edit Modal -->
    <div class="modal fade" id="roleModal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header bg-primary">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h5 class="modal-title" id="modalTitle">Create Role</h5>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="text-semibold">Role Name <span class="text-danger">*</span></label>
                                <input type="text" id="txt_RoleName" class="form-control" placeholder="Enter role name" />
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="text-semibold">Module Name</label>
                                <div class="multi-select-full">
                                    <asp:ListBox ID="lstModules" runat="server" CssClass="multiselect form-control" SelectionMode="Multiple"></asp:ListBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="text-semibold">Description</label>
                        <textarea id="txt_Description" class="form-control" rows="3" placeholder="Enter description"></textarea>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-link" data-dismiss="modal">Close</button>
                    <button type="button" id="btnSave" class="btn btn-primary" onclick="SaveRole();">Save</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Delete Confirmation Modal -->
    <div class="modal fade" id="confirmDeleteModal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm" style="margin-top: 15vh;" role="document">
            <div class="modal-content">
                <div class="modal-header bg-danger text-white">
                    <h5 class="modal-title">Confirm Delete</h5>
                    <button type="button" class="close text-white" data-dismiss="modal">
                        <span>&times;</span>
                    </button>
                </div>
                <div class="modal-body text-center">
                    <p class="mb-0">Are you sure you want to delete this role?</p>
                </div>
                <div class="modal-footer justify-content-center">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">No</button>
                    <button type="button" class="btn btn-danger" onclick="confirmDeleteRole()">Yes, Delete</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
