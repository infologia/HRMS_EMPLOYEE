<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Meetings.aspx.cs" Inherits="Employee_Meetings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>
    <script>
        var deleteMeetingKey = null;

        function fn_DeleteProject(leadKey) {
            if (!leadKey) {
                toastr.error("Invalid Meeting Key");
                return;
            }
            deleteMeetingKey = leadKey;
            $('#confirmDeleteModal').modal('show');
        }

        function confirmDeleteProject() {
            if (!deleteMeetingKey) {
                toastr.error("Meeting Key missing. Please try again.");
                return;
            }
            $('#confirmDeleteModal').modal('hide');
            $.ajax({
                type: "POST",
                url: "Meetings.aspx/DeleteProject",
                data: JSON.stringify({ str_leadkey: deleteMeetingKey }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response && response.d === "1") {
                        toastr.success("Meeting has been removed successfully!");
                        setTimeout(function () { location.reload(); }, 1500);
                    } else {
                        toastr.warning("Sorry, unable to remove this Meeting. Please try again.");
                    }
                    deleteMeetingKey = null;
                },
                error: function (xhr, status, error) {
                    console.error(error);
                    toastr.error("An error occurred while removing the Meeting.");
                    deleteMeetingKey = null;
                }
            });
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="panel panel-flat">
        <div class="panel-heading">
            <div class="row">
                <div class="col-lg-4">
                    <h5 class="panel-title">Meetings List</h5>
                </div>
                <div class="col-lg-8">
                    <a href="Meetingdetails.aspx" id="a_createlead" runat="server" class="btn btn-primary  pull-right"><i class="icon-plus-circle2"></i>&nbsp;Create Meeting</a>
                </div>
            </div>
        </div>
    </div>
    <div class="panel panel-flat">
        <div class="panel-heading">
            <div class="row">
                <div class="col-lg-4">
                    <h5 class="panel-title">Upcoming Meetings </h5>
                </div>
            </div>
        </div>
        <div class="panel-body" style="padding: 0px;">
        </div>
        <table class="table datatable-basic">
            <thead>
                <tr>
                    <th>Title</th>
                    <th style="white-space: nowrap; min-width: 110px;">Meeting Date</th>
                    <th style="white-space: nowrap; min-width: 110px;">Start Time</th>
                    <th style="white-space: nowrap; min-width: 110px;">End Time</th>
                    <th>Status</th>
                    <th>Created</th>
                    <th id="UpdateView" runat="server" class="text-center">Action</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_leave" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
    </div>
    <div class="panel panel-flat">
        <div class="panel-heading">
            <div class="row">
                <div class="col-lg-4">
                    <h5 class="panel-title">Completed Meetings </h5>
                </div>
            </div>
        </div>

        <table class="table datatable-basic">
            <thead>
                <tr>
                    <th>Title</th>
                    <th style="white-space: nowrap; min-width: 110px;">Meeting Date</th>
                    <th style="white-space: nowrap; min-width: 110px;">Start Time</th>
                    <th style="white-space: nowrap; min-width: 110px;">End Time</th>
                    <th>Hours</th>
                    <th>Status</th>
                    <th>Created</th>
                    <th class="text-center">Action</th>

                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_Completed" runat="server"></asp:PlaceHolder>
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
                    <p class="mb-0">Are you sure you want to remove this Meeting?</p>
                </div>
                <div class="modal-footer justify-content-center">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">No</button>
                    <button type="button" class="btn btn-danger" onclick="confirmDeleteProject()">Yes, Remove</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

