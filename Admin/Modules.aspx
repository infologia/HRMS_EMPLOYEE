<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Modules.aspx.cs" Inherits="Admin_Modules" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>
    
    <script type="text/javascript">
        var deleteModuleKey = "";
        var currentModuleId = "";

        function openModal(moduleId, moduleName, description, isActive) {
            if (moduleId) {
                // Edit mode
                currentModuleId = moduleId;
                $('#modalTitle').text('Edit Module');
                $('#txt_ModuleName').val(moduleName);
                $('#txt_Description').val(description);
                $('#chk_Active').prop('checked', isActive === 'True' || isActive === true);
                $('#btnSave').text('Update');
            } else {
                // Create mode
                currentModuleId = "";
                $('#modalTitle').text('Create Module');
                $('#txt_ModuleName').val('');
                $('#txt_Description').val('');
                $('#chk_Active').prop('checked', true);
                $('#btnSave').text('Save');
            }
            $('#moduleModal').modal('show');
        }

        function SaveModule() {
            var moduleName = $('#txt_ModuleName').val().trim();
            var description = $('#txt_Description').val().trim();
            var isActive = $('#chk_Active').is(':checked');

            if (moduleName === '') {
                showToastr('error', 'Module name is required!');
                $('#txt_ModuleName').focus();
                return false;
            }

            var method = currentModuleId ? "UpdateModule" : "SaveModule";
            var data = currentModuleId 
                ? JSON.stringify({ moduleId: currentModuleId, moduleName: moduleName, description: description, isActive: isActive })
                : JSON.stringify({ moduleName: moduleName, description: description, isActive: isActive });

            $.ajax({
                type: "POST",
                url: "Modules.aspx/" + method,
                data: data,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(response) {
                    if (response.d === "true") {
                        var msg = currentModuleId ? 'Module updated successfully!' : 'Module saved successfully!';
                        showToastr('success', msg);
                        $('#moduleModal').modal('hide');
                        setTimeout(function() { location.reload(); }, 1500);
                    } else {
                        showToastr('error', 'Failed to save module');
                    }
                },
                error: function() {
                    showToastr('error', 'Server error. Please try again.');
                }
            });
        }

        function fn_DeleteModule(moduleid) {
            deleteModuleKey = moduleid;
            $('#confirmDeleteModal').modal('show');
        }

        function confirmDeleteModule() {
            if (deleteModuleKey === "") {
                return;
            }

            $.ajax({
                type: "POST",
                url: "Modules.aspx/DeleteModule",
                data: JSON.stringify({ str_moduleid: deleteModuleKey }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(data) {
                    $('#confirmDeleteModal').modal('hide');
                    if (data.d === "1") {
                        showToastr('success', 'Module deleted successfully');
                        setTimeout(function() { location.reload(); }, 1500);
                    } else {
                        showToastr('error', 'Unable to delete module. Please try again.');
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
                    <h5 class="panel-title">Modules</h5>
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
                    <th>Module Name</th>
                    <th>Description</th>
                    <th>Active</th>
                    <th>Edit</th>
                    <th>Delete</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_Modules" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
    </div>

    <!-- Create/Edit Modal -->
    <div class="modal fade" id="moduleModal" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h5 class="modal-title" id="modalTitle">Create Module</h5>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label>Module Name <span class="text-danger">*</span></label>
                        <input type="text" id="txt_ModuleName" class="form-control" placeholder="Enter module name" />
                    </div>
                    <div class="form-group">
                        <label>Description</label>
                        <textarea id="txt_Description" class="form-control" rows="3" placeholder="Enter description"></textarea>
                    </div>
                    <div class="form-group">
                        <label class="checkbox-inline">
                            <input type="checkbox" id="chk_Active" checked /> Active
                        </label>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
                    <button type="button" id="btnSave" class="btn btn-primary" onclick="SaveModule();">Save</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Delete Confirmation Modal -->
    <div class="modal fade" id="confirmDeleteModal" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
                <div class="modal-header bg-danger text-white">
                    <h5 class="modal-title">Confirm Delete</h5>
                    <button type="button" class="close text-white" data-dismiss="modal">
                        <span>&times;</span>
                    </button>
                </div>
                <div class="modal-body text-center">
                    <p class="mb-0">Are you sure you want to delete this module?</p>
                </div>
                <div class="modal-footer justify-content-center">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">No</button>
                    <button type="button" class="btn btn-danger" onclick="confirmDeleteModule()">Yes, Delete</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

