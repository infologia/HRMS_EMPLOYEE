<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="PayableinvoiceGrid.aspx.cs" Inherits="Admin_PayableinvoiceGrid" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
      <script type="text/javascript">

      var deleteInvoiceKey = "";

      // Called when Delete button clicked
      function fn_DeleteProject(InvoiceKey) {
          deleteInvoiceKey = InvoiceKey;
          $('#confirmDeleteModal').modal('show');
      }

      // Yes button click
      function confirmDeleteProject() {

          $('#confirmDeleteModal').modal('hide');

          $.ajax({
              type: "POST",
              url: "PayableinvoiceGrid.aspx/DeleteProject",
              data: JSON.stringify({ str_InvoiceKey: deleteInvoiceKey }),
              contentType: "application/json; charset=utf-8",
              dataType: "json",

              success: function (response) {
                  if (response.d == "1") {
                      toastr.success("Payable Invoice has been removed successfully!");
                      setTimeout(function () {
                          window.location.href = "/Admin/PayableinvoiceGrid.aspx";
                      }, 2000);
                  } else {
                      toastr.warning("Sorry, unable to remove this Payable Invoice. Please try again.");
                  }
              },

              error: function () {
                  toastr.error("An error occurred while removing the Payable Invoice. Please try again.");
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
<h5 class="panel-title">Payable Invoices Details</h5></div><div class="col-md-6  pull-right">
<div style="display: flex; gap: 10px; justify-content: flex-end; align-items: center;">
<asp:DropDownList ID="ddlFinancialYear" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFinancialYear_SelectedIndexChanged" style="width: 200px;"></asp:DropDownList>
<asp:DropDownList ID="ddlVendor" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlVendor_SelectedIndexChanged" style="width: 200px;"></asp:DropDownList>
<a href="Payableinvoice.aspx"  runat="server" class="btn btn-primary"><i class="icon-plus-circle2"></i> Create Invoice</a>
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
                <th>Invoice Number</th>
                <th>Invoice Date</th>
                <th>Due Date</th>
                <th>Status</th>  
                <th>CreatedOn</th>
                <th>Update</th>
                  <th>Remove</th>
    
            </tr>
        </thead>
        <tbody>
            <asp:PlaceHolder ID="PH_PAYABLEINVOICE" runat="server"></asp:PlaceHolder>
        </tbody>
    </table>
    <div class="row" style="margin-top:15px;">
        <div class="col-lg-12 text-center">
            <strong>
                Total Amount to Pay:
                <asp:Label ID="lblTotalAmount" runat="server" CssClass="text-primary"></asp:Label>
            </strong>
        </div>
    </div>
    <br />
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
                <p class="mb-0">
                    Are you sure you want to remove this Invoice?
                </p>
            </div>

            <div class="modal-footer justify-content-center">
                <button type="button" class="btn btn-secondary" data-dismiss="modal">
                    No
                </button>
                <button type="button" class="btn btn-danger" onclick="confirmDeleteProject()">
                    Yes, Remove
                </button>
            </div>

        </div>
    </div>
</div>
</asp:Content>

