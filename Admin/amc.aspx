<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="amc.aspx.cs" Inherits="Admin_amc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
 <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
 <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
 <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>

 <script type="text/javascript">
     var deleteAMCKey = null;

     function fn_DeleteProject(leadKey) {
         if (!leadKey) {
             toastr.error("Invalid AMC Key");
             return;
         }
         deleteAMCKey = leadKey;
         $('#confirmDeleteModal').modal('show');
     }

     function confirmDeleteProject() {
         if (!deleteAMCKey) {
             toastr.error("AMC Key missing. Please try again.");
             return;
         }
         $('#confirmDeleteModal').modal('hide');
         $.ajax({
             type: "POST",
             url: "amc.aspx/DeleteProject",
             data: JSON.stringify({ str_leadkey: deleteAMCKey }),
             contentType: "application/json; charset=utf-8",
             dataType: "json",
             success: function (response) {
                 if (response && response.d === "1") {
                     toastr.success("AMC has been removed successfully!");
                     setTimeout(function () { location.reload(); }, 1500);
                 } else {
                     toastr.warning("Sorry, unable to remove this AMC. Please try again.");
                 }
                 deleteAMCKey = null;
             },
             error: function (xhr, status, error) {
                 console.error(error);
                 toastr.error("An error occurred while removing the AMC.");
                 deleteAMCKey = null;
             }
         });
     }
 </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="panel panel-flat">
    <div class="panel-heading">
        <div class="row">
            <div class="col-md-6 pull-left">
                <h5 class="panel-title">AMS Details - Live</h5>
            </div>
            <div class="col-md-6 pull-right">
                <a href="AmcDetails.aspx" id="a_createlead" runat="server" visible="false" class="btn btn-primary pull-right"><i class="icon-plus-circle2"></i> Create AMC</a>
            </div>
        </div>
    </div>

    <div class="panel-body" style="padding: 0px;">
    </div>
    <table class="table datatable-basic">
        <thead>
            <tr>
                <th>Company Name</th>
                <th>Project Name</th>
                <th>Live Date</th>
                <th>Status</th>
                <th>Created Date</th>
                <th>Action</th>
            </tr>
        </thead>
        <tbody>
            <asp:PlaceHolder ID="PH_Amc_Incompleted" runat="server"></asp:PlaceHolder>
        </tbody>
    </table>
</div>

    <div class="panel panel-flat">
        <div class="panel-heading">
<div class="row">
<div class="col-md-6 pull-left">
<h5 class="panel-title">AMS Details - Closed</h5></div>
</div>
 
</div>
    
    <div class="panel-body" style="padding: 0px;">
    </div>        
    <table class="table datatable-basic">
        <thead>
            <tr>
                <th>Company Name</th>
                <th>Project Name</th>
                <th>Live Date</th>
                <th>Status</th>
                <th>Created Date</th>                   
                <th>Action</th>
            </tr>
        </thead>
        <tbody>
            <asp:PlaceHolder ID="PH_Amc_Completed" runat="server"></asp:PlaceHolder>
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
                    <p class="mb-0">Are you sure you want to remove this AMC?</p>
                </div>
                <div class="modal-footer justify-content-center">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">No</button>
                    <button type="button" class="btn btn-danger" onclick="confirmDeleteProject()">Yes, Remove</button>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
