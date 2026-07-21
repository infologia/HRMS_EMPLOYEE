<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="SubMenus.aspx.cs" Inherits="Admin_SubMenus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>
    
    <script type="text/javascript">
        var deleteMenuKey = "";

        function fn_DeleteSubMenu(menukey) {
            deleteMenuKey = menukey;
            $('#confirmDeleteModal').modal('show');
        }

        function confirmDeleteSubMenu() {
            if (deleteMenuKey === "") {
                return;
            }

            $.ajax({
                type: "POST",
                url: "SubMenus.aspx/DeleteSubMenu",
                data: JSON.stringify({ str_menukey: deleteMenuKey }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function(data) {
                    $('#confirmDeleteModal').modal('hide');
                    if (data.d === "1") {
                        showToastr('success', 'Sub menu deleted successfully');
                        setTimeout(function() { location.reload(); }, 1500);
                    } else {
                        showToastr('error', 'Unable to delete sub menu. Please try again.');
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
                    <h5 class="panel-title">Sub Menus</h5>
                </div><br />
                <div class="col-lg-8">
                    <a href="CreateSubMenu.aspx" class="btn btn-primary pull-right">
                        <i class="icon-plus-circle2"></i> Create New
                    </a>
                </div>
            </div>
        </div>

        <div class="panel-body" style="padding: 0px;"></div>

        <table class="table datatable-basic">
            <thead>
                <tr>
                    <th>Menu Name</th>
                    <th>Page Name</th>
                    <th>Menu List No</th>
                    <th>Module Name</th>
                    <th>Parent Menu Name</th>
                    <th>Folder Name</th>
                    <th>Edit</th>
                    <th>Delete</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_SubMenus" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
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
                    <p class="mb-0">Are you sure you want to delete this sub menu?</p>
                </div>
                <div class="modal-footer justify-content-center">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">No</button>
                    <button type="button" class="btn btn-danger" onclick="confirmDeleteSubMenu()">Yes, Delete</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
