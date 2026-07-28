<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="ReceivableInvoiceGrid.aspx.cs" Inherits="Admin_ReceivableInvoiceGrid" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script>
        var deleteInvId = "";
        function fn_DeleteInvoice(invId) {
            deleteInvId = invId;
            $('#confirmDeleteModal').modal('show');
        }

        function confirmDeleteInvoice() {
            if (deleteInvId === "") return;

            $.ajax({
                type: "POST",
                url: "ReceivableInvoiceGrid.aspx/DeleteInvoiceWebMethod",
                data: JSON.stringify({ invoiceKey: deleteInvId.toString() }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $('#confirmDeleteModal').modal('hide');
                    if (data.d === "true") {
                        showToastr('success', 'Invoice deleted successfully!');
                        setTimeout(function () { location.reload(); }, 1500);
                    } else {
                        showToastr('error', 'Unable to delete invoice.');
                    }
                },
                error: function () {
                    $('#confirmDeleteModal').modal('hide');
                    showToastr('error', 'Server error. Please try again.');
                }
            });
        }
    </script>
        <div class="panel panel-flat">
    <div class="panel-heading">
        <div class="row">
            <div class="col-lg-4">
                <h5 class="panel-title">Receivable Invoices Details</h5>
            </div>
            <div class="col-lg-8">
                <div style="display: flex; gap: 10px; justify-content: flex-end; align-items: center;">
                    <asp:DropDownList ID="ddlFinancialYear" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFinancialYear_SelectedIndexChanged" style="width: 200px;"></asp:DropDownList>
                    <a href="createinvoice.aspx"  runat="server" class="btn btn-primary"><i class="icon-plus-circle2"></i> Create Invoice</a>
                </div>
            </div>
        </div>
    </div>
    <div class="panel-body" style="padding: 0px;">
    </div>
    <div class="table-responsive">
        <table class="table datatable-basic" data-order='[[ 3, "desc" ]]'>
            <thead>
                <tr>
                    
                    <th style="white-space: nowrap;">Invoice Number</th>
                    <th style="white-space: nowrap;">Client Name</th>
                    <th style="white-space: nowrap;">Project Name</th>
                    <th style="white-space: nowrap;">Invoice Date</th>
                    <th style="white-space: nowrap;">Due Date</th>
                    <th>Status</th>  
                    <th class="text-center">Download</th>
                    <th>CreatedOn</th>
                    <th class="text-center">Actions</th>
        
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_RECEIVABLEINVOICE" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
    </div>
</div>

    <!-- Delete Confirmation Modal -->
    <div class="modal fade" id="confirmDeleteModal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm" style="margin-top: 15vh;" role="document">
            <div class="modal-content">
                <div class="modal-header bg-danger text-white">
                    <button type="button" class="close text-white" data-dismiss="modal">
                        <span>&times;</span>
                    </button>
                    <h5 class="modal-title">Confirm Delete</h5>
                </div>
                <div class="modal-body text-center">
                    <p class="mb-0">Are you sure you want to delete this invoice?</p>
                </div>
                <div class="modal-footer justify-content-center">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">No</button>
                    <button type="button" class="btn btn-danger" onclick="confirmDeleteInvoice()">Yes, Delete</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

