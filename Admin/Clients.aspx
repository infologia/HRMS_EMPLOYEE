<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Clients.aspx.cs" Inherits="Admin_Clients" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>
    <style>
        #tbl_active th, #tbl_inactive th { white-space: nowrap; font-size: 12px; }
        #tbl_active td, #tbl_inactive td { font-size: 12px; }
        .panel-heading { margin-bottom: 0; padding-bottom: 10px; border-bottom: 1px solid #ddd; }
        .panel .panel-heading + div { margin-top: 0; }
    </style>

    <script type="text/javascript">
        var deleteClientKey = null;

        function fn_DeleteProject(leadKey) {
            if (!leadKey) { toastr.error("Invalid Client Key"); return; }
            deleteClientKey = leadKey;
            $('#confirmDeleteModal').modal('show');
        }

        function confirmDeleteClient() {
            if (!deleteClientKey) { toastr.error("Client Key missing. Please try again."); return; }
            $('#confirmDeleteModal').modal('hide');
            $.ajax({
                type: "POST",
                url: "Clients.aspx/DeleteProject",
                data: JSON.stringify({ str_leadkey: deleteClientKey }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response && response.d === "1") {
                        toastr.success("Organization has been removed successfully!");
                        setTimeout(function () { location.reload(); }, 1500);
                    } else {
                        toastr.warning("Sorry, unable to remove this Organization. Please try again.");
                    }
                    deleteClientKey = null;
                },
                error: function (xhr, status, error) {
                    console.error(error);
                    toastr.error("An error occurred while removing the Organization.");
                    deleteClientKey = null;
                }
            });
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <%-- Active Clients Grid --%>
    <div class="panel panel-flat">
        <div class="panel-heading">
            <div class="row">
                <div class="col-lg-4">
                    <h5 class="panel-title">Active Organization</h5>
                </div>
                <div class="col-lg-8">
                    <a href="Clientsdetails.aspx" id="a_createlead" runat="server" visible="false" class="btn btn-primary pull-right"><i class="icon-plus-circle2"></i> Create Organization</a>
                </div>
            </div>
        </div>
      
        <div style="overflow-x: auto;">
        <table class="table datatable-basic" id="tbl_active">
            <thead>
                <tr>
                    <th>Organization Code</th>
                    <th>Organization Name</th>
                    <th>Type</th>
                    <th>Company Name</th>
                    <th>Contact Person</th>
                    <th>Status</th>
                    <th>Created On</th>
                    <th style="min-width: 100px; text-align: center;">Action</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_ActiveClients" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
        </div>
        </div>
    <div class="panel panel-flat" style="margin-top: 20px;">
        <div class="panel-heading">
            <h5 class="panel-title">Inactive Organization</h5>
        </div>
        <div class="panel-body" style="padding: 0px;"></div>
        <div style="overflow-x: auto;">
        <table class="table datatable-basic" id="tbl_inactive">
            <thead>
                <tr>
                    <th>Organization Code</th>
                    <th>Organization Name</th>
                    <th>Type</th>
                    <th>Company Name</th>
                    <th>Contact Person</th>
                    <th>Status</th>
                    <th>Created On</th>
                    <th style="min-width: 100px; text-align: center;">Action</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_InactiveClients" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
        </div>
    </div>


    <!-- Delete Confirmation Modal -->
    <div class="modal fade" id="confirmDeleteModal" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
                <div class="modal-header bg-danger text-white">
                    <h5 class="modal-title">Confirm Delete</h5>
                    <button type="button" class="close text-white" data-dismiss="modal"><span>&times;</span></button>
                </div>
                <div class="modal-body text-center">
                    <p class="mb-0">Are you sure you want to remove this Organization?</p>
                </div>
                <div class="modal-footer justify-content-center">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">No</button>
                    <button type="button" class="btn btn-danger" onclick="confirmDeleteClient()">Yes, Remove</button>
                </div>
            </div>
        </div>
    </div>

    </asp:Content>