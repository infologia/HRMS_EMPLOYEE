<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="gst.aspx.cs" Inherits="Admin_gst" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel panel-flat">
        <div class="panel-heading">
            <div class="row">
                <div class="col-lg-4">
                    <h5 class="panel-title">Payable GST Details</h5>
                </div>
                <div class="col-lg-8">
                    <div style="display: flex; gap: 10px; justify-content: flex-end; align-items: center;">
                        <asp:DropDownList ID="ddlFinancialYear" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlFinancialYear_SelectedIndexChanged" style="width: 200px;"></asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>

        <table class="table datatable-basic" data-order='[[ 1, "desc" ]]'>
            <thead>
                <tr>
                    <th>Invoice No</th>
                    <th>Invoice Date</th>
                    <th>GST Amount</th>
                    <th>GST Paid Date</th>
                    <th>Invoice Amount</th>
                    <th>Status</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_Project" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
        <div class="row" style="margin-top:15px;">
            <div class="col-lg-12 text-center">
                <strong>
                    Total GST:
                    <asp:Label ID="lblTotalGST" runat="server" CssClass="text-primary"></asp:Label>
                    &nbsp;&nbsp;|&nbsp;&nbsp;
                    Total Amount:
                    <asp:Label ID="lblTotalAmount" runat="server" CssClass="text-primary"></asp:Label>
                </strong>
            </div>
        </div>
        <br />
    </div>

    <!-- CONFIRM MODAL -->
    <div id="payConfirmModal" class="modal fade">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">

                <div class="modal-header">
                    <h5 class="modal-title">Confirm Payment</h5>
                </div>

                <div class="modal-body">
                    <p>Are you sure you want to mark as paid?</p>

                    <div class="form-group">
                        <label for="txtDescription">GST Description</label>
                        <asp:TextBox ID="txtDescription"
                            runat="server"
                            CssClass="form-control"
                            TextMode="MultiLine"
                            Rows="3" />
                    </div>
                </div>

                <div class="modal-footer">
                    <asp:HiddenField ID="hdnInvoiceKey" runat="server" />

                    <button type="button" class="btn btn-primary"
                        onclick="confirmPayInvoice()">
                        OK
                    </button>
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        Cancel
                    </button>
                </div>

            </div>
        </div>
    </div>


    <script type="text/javascript">

        function fn_PayInvoice(invoiceKey) {
            // Set hidden field value
            document.getElementById('<%= hdnInvoiceKey.ClientID %>').value = invoiceKey;

            // Show the modal
            $('#payConfirmModal').modal('show');
        }

        function confirmPayInvoice() {
            // Trigger server-side postback
            __doPostBack('PayInvoice', '');
        }

    </script>

</asp:Content>

