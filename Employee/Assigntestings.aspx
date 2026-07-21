<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Assigntestings.aspx.cs" Inherits="Employee_Assigntestings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>
    <script type="text/javascript">
        var deleteTestingKey = "";
        function fn_DeleteTesting(taskTestingkey) {
            deleteTestingKey = taskTestingkey;
            $('#confirmDeleteModal').modal('show');
        }
        function confirmDeleteTesting() {
            if (deleteTestingKey === "") {
                return;
            }
            $.ajax({
                type: "POST",
                url: "Assigntestings.aspx/DeleteTesting",
                data: JSON.stringify({
                    str_taskTestingkey: deleteTestingKey
                }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $('#confirmDeleteModal').modal('hide');
                    if (data.d === "1") {
                        toastr.success('Record removed successfully');
                        setTimeout(function () {
                            location.reload();
                        }, 1500);
                    }
                    else {
                        toastr.error('Unable to remove record. Please try again.');
                    }
                },
                error: function () {
                    $('#confirmDeleteModal').modal('hide');
                    toastr.error('Server error. Please try again.');
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel panel-flat">
        <div class="panel-heading">
            <h5 class="panel-title">Testing Assignments</h5>
            <div class="heading-elements">
                <a href="Assigntesting.aspx" class="btn btn-primary pull-right">
                    <i class="icon-plus-circle2"></i> Create Testing
                </a>
            </div>
        </div>

        <div class="panel-body" style="padding: 0px;">
        </div>

        <table class="table datatable-basic">
            <thead>
                <tr>
                    <th>Project Name</th>
                    <th>Task Name</th>
                    <th>Assigned By</th>
                    <th>Assigned To</th>
                    <th>Start Date</th>
                    <th>Hours</th>
                    <th>Status</th>
                    <th>Update</th>
                    <th>Remove</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_Testing" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
    </div>

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
                    <p class="mb-0">
                        Are you sure you want to remove this record?
                    </p>
                </div>
                <div class="modal-footer justify-content-center">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">
                        No
                    </button>
                    <button type="button" class="btn btn-danger" onclick="confirmDeleteTesting()">
                        Yes, Remove
                    </button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
