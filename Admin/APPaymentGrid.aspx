<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="APPaymentGrid.aspx.cs" Inherits="Admin_APPaymentGrid" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        var deletePaymentId = "";

        // Called when Delete button clicked
        function fn_DeleteAPPayment(AP_Id) {
            deletePaymentId = AP_Id;
            $('#confirmDeleteModal').modal('show');
        }

        // Yes button click
        function confirmDeletePayment() {
            $('#confirmDeleteModal').modal('hide');

            $.ajax({
                type: "POST",
                url: "APPaymentGrid.aspx/DeleteAPPayment",
                data: JSON.stringify({ id: deletePaymentId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",

                success: function (response) {
                    if (response.d == "Success") {
                        if (typeof toastr !== 'undefined') toastr.success("Payment has been removed successfully!");
                        else alert("Payment has been removed successfully!");
                        
                        setTimeout(function () {
                            window.location.href = "/Admin/APPaymentGrid.aspx";
                        }, 2000);
                    } else {
                        if (typeof toastr !== 'undefined') toastr.error("Unable to remove: " + response.d);
                        else alert("Unable to remove: " + response.d);
                    }
                },
                error: function () {
                    if (typeof toastr !== 'undefined') toastr.error("An error occurred while removing the Payment.");
                    else alert("An error occurred while removing the Payment.");
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
                    <h5 class="panel-title">Payment Details</h5>
                </div>
                <div class="col-md-6 pull-right">
                    <div style="display: flex; gap: 10px; justify-content: flex-end; align-items: center;">
                        <asp:DropDownList ID="ddlFinancialYear" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFinancialYear_SelectedIndexChanged" style="width: 200px;"></asp:DropDownList>
                        <a href="APPaymentEntry.aspx" class="btn btn-primary"><i class="icon-plus-circle2"></i> Create Payment</a>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="panel-body" style="padding: 0px;">
        </div>
        
        <table class="table datatable-basic">
            <thead>
                <tr>
                    <th>Vendor Name</th>
                    <th>Invoice No</th>
                    <th>Invoice Date</th>
                    <th>Grand Total</th>
                    <th>Payment</th>
                    <th>Balance Amount</th>
                    <th>Status</th>
                    <th>Edit</th>
                    <th>Remove</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_APPayment" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
        <br />
    </div>

    <!-- Delete Confirmation Modal -->
    <div class="modal fade" id="confirmDeleteModal" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">

                <div class="modal-header bg-danger text-white">
                    <button type="button" class="close text-white" data-dismiss="modal">
                        <span>&times;</span>
                    </button>
                    <h5 class="modal-title">Confirm Delete</h5>
                </div>

                <div class="modal-body text-center" style="padding-top:20px;">
                    <p class="mb-0" style="font-size:16px;">
                        Are you sure you want to remove this Payment?
                    </p>
                </div>

                <div class="modal-footer justify-content-center" style="text-align:center;">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">
                        No
                    </button>
                    <button type="button" class="btn btn-danger" onclick="confirmDeletePayment()">
                        Yes, Remove
                    </button>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
