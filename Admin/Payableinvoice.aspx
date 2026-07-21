<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Payableinvoice.aspx.cs" Inherits="Admin_Payableinvoice1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
                <style>
/* Label spacing */
.panel-body label {
    display: block;
    margin-bottom: 6px;
    font-size: 13px;
}



/* Textarea spacing (IMPORTANT) */
.panel-body textarea.form-control {
    margin-bottom: 16px;
    resize: vertical;
}

/* Row spacing */
.panel-body .row {
    margin-bottom: 10px;
}

/* Button area spacing */


/* -------- MOBILE VIEW -------- */
@media (max-width: 768px) {

    /* Each field space */
    .panel-body .col-md-4,
    .panel-body .col-md-6 {
        margin-bottom: 15px;
    }

  .validation-msg span {
    display: block;        /* force same position */
    margin-top: 4px;       /* controlled spacing */
}.validation-msg span {
    display: block;   /* force same position */
   margin-top: 4px;
}

  
}</style>
    <script type="text/javascript">
        function calculateTotal() {

            var invoiceAmt = parseFloat(document.getElementById('<%= InvoiceAmount.ClientID %>').value) || 0;
            var tdsAmt = parseFloat(document.getElementById('<%= TDSAmount.ClientID %>').value) || 0;
            var gstAmt = parseFloat(document.getElementById('<%= GSTAmount.ClientID %>').value) || 0;

            var total = invoiceAmt;
            var totalAmount = invoiceAmt;

            // Subtract TDS if entered
            if (tdsAmt > 0) {
                total = total - tdsAmt;
            }

            // Add GST if entered
            if (gstAmt > 0) {
                total = total + gstAmt;
                totalAmount = totalAmount + gstAmt;
            }

            document.getElementById('<%= TotalPayableAmount.ClientID %>').value = total.toFixed(2);
            document.getElementById('<%= TotalAmount.ClientID %>').value = totalAmount.toFixed(2);

            $("#<%= hdnSubTotal.ClientID %>").val(invoiceAmt.toFixed(2));
            $("#<%= hdnTDS.ClientID %>").val(tdsAmt.toFixed(2));
            $("#<%= hdnGST.ClientID %>").val(gstAmt.toFixed(2));
            $("#<%= hdnTotal.ClientID %>").val(total.toFixed(2));
            $("#<%= hdnTotalAmount.ClientID %>").val(totalAmount.toFixed(2));

        }
    </script>
  
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <asp:HiddenField ID="hdnSubTotal" runat="server" />
    <asp:HiddenField ID="hdnTDS" runat="server" />
    <asp:HiddenField ID="hdnGST" runat="server" />
    <asp:HiddenField ID="hdnTotal" runat="server" />
    <asp:HiddenField ID="hdnTotalAmount" runat="server" />
    <asp:HiddenField ID="hfInvoiceKey" runat="server" />

    <div class="panel panel-flat">
        <div class="panel-heading">
            <%--            <h5 class="panel-title">Payable Invoice</h5>--%>
        </div>

        <div class="panel-body">
            <legend class="text-semibold">
                <i class="icon-briefcase position-left"></i>Create Invoice
            </legend>

            <!-- Row 1 -->
            <div class="row">
                <div class="col-md-4">
                    <label>Vendor Name <span style="color: red">*</span></label>
                    <asp:DropDownList ID="ddlVendor" runat="server" CssClass="form-control" AutoPostBack="true"
                        OnSelectedIndexChanged="Rd_Status_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator
                        ID="RequiredFieldValidator_ddl_Vendor"
                        runat="server"
                        ControlToValidate="ddlVendor"
                        ErrorMessage="Please Select Vendor Name"
                        ForeColor="Red" />
                </div>

                <div class="col-md-4">
                    <label>Invoice Number <span style="color: red">*</span></label>
                    <asp:TextBox ID="InvoiceNumber" runat="server"
                        CssClass="form-control"
                        placeholder="Enter Invoice Number"></asp:TextBox>
                    <asp:RequiredFieldValidator
                        ID="RequiredFieldValidator1"
                        runat="server"
                        ControlToValidate="InvoiceNumber"
                        ErrorMessage="Enter Invoice Number"
                        ForeColor="Red" />
                </div>

                <div class="col-md-4">
                    <label>Invoice Received Date <span style="color: red">*</span></label>
                    <div class="input-group">
                        <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                        <asp:TextBox ID="IT_InvoiceDate" runat="server"
                            CssClass="form-control pickadate"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator
                        ID="RequiredFieldValidator2"
                        runat="server"
                        ControlToValidate="IT_InvoiceDate"
                        ErrorMessage="Enter Invoice Date"
                        ForeColor="Red" />
                </div>
            </div>


            <!-- Row 2 -->
            <div class="row">
                <div class="col-md-4">
                    <label>Due Date <span style="color: red">*</span></label>
                    <div class="input-group">
                        <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                        <asp:TextBox ID="IT_ReceivedDate" runat="server"
                            CssClass="form-control pickadate"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator
                        ID="RequiredFieldValidator7"
                        runat="server"
                        ControlToValidate="IT_ReceivedDate"
                        ErrorMessage="Enter Due Date"
                        ForeColor="Red" />
                </div>

                <div class="col-md-4">
                    <label>Currency <span style="color: red">*</span></label>
                    <asp:DropDownList ID="DD_Currency" runat="server" CssClass="form-control"></asp:DropDownList>
                    <asp:RequiredFieldValidator
                        ID="RequiredFieldValidator8"
                        runat="server"
                        ControlToValidate="DD_Currency"
                        ErrorMessage="Enter Currency"
                        ForeColor="Red" />
                </div>

                <div class="col-md-4">
                    <label>Invoice Amount <span style="color: red">*</span></label>
                    <asp:TextBox ID="InvoiceAmount" runat="server"
                        CssClass="form-control"
                        placeholder="Enter Invoice Amount"
                        onkeyup="calculateTotal();"  onkeypress="return allowDecimal(event);"></asp:TextBox>
                      <asp:RequiredFieldValidator
                        ID="RequiredFieldValidator3"
                        runat="server"
                        ControlToValidate="InvoiceAmount"
                        ErrorMessage="Enter Invoice Amount"
                        ForeColor="Red" />
                     <div class="validation-msg"><asp:RegularExpressionValidator ID="revAmount" runat="server" ControlToValidate="InvoiceAmount" ErrorMessage="Enter a valid amount" ValidationExpression="^\d+(\.\d{1,2})?$" ForeColor="Red" Display="Dynamic"> </asp:RegularExpressionValidator>
                </div> 
                </div>
            </div>
            <!-- Row 3 -->
            <div class="row">
                <div class="col-md-4">
                    <label>TDS Amount</label>
                    <asp:TextBox ID="TDSAmount" runat="server"
                        CssClass="form-control"
                        placeholder="Enter TDS Amount"
                        onkeyup="calculateTotal();"></asp:TextBox>
                     <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="TDSAmount" ErrorMessage="Enter a valid amount " ValidationExpression="^\d+(\.\d{1,2})?$" ForeColor="Red" Display="Dynamic"> </asp:RegularExpressionValidator>
                </div>
                  <div class="col-md-4">
      <label>TDS %</label>
      <asp:TextBox ID="TDSPercentage" runat="server"
          CssClass="form-control"
          placeholder="Enter TDS %"></asp:TextBox>
       <asp:RegularExpressionValidator ID="revTDSPercent" runat="server" ControlToValidate="TDSPercentage" ErrorMessage="Enter a valid %" ValidationExpression="^\d+(\.\d{1,2})?$" ForeColor="Red" Display="Dynamic"> </asp:RegularExpressionValidator>
  </div>
                <div class="col-md-4">
                    <label>GST Amount <span style="color: red">*</span></label>
                    <asp:TextBox ID="GSTAmount" runat="server"
                        CssClass="form-control"
                        placeholder="Enter GST Amount"
                        onkeyup="calculateTotal();"></asp:TextBox>
              <span class="validation-msg">
    <asp:RequiredFieldValidator
        ID="RequiredFieldValidator4"
        runat="server"
        ControlToValidate="GSTAmount"
        ErrorMessage="Enter GST Amount"
        ForeColor="Red"
        Display="Dynamic" />

    <asp:RegularExpressionValidator
        ID="RegularExpressionValidator2"
        runat="server"
        ControlToValidate="GSTAmount"
        ErrorMessage="Enter a valid amount"
        ValidationExpression="^\d+(\.\d{1,2})?$"
        ForeColor="Red"
        Display="Dynamic" />
</span> </div>
            </div>
            <br />
            <!-- Row 4 -->
            <div class="row">
                                      <div class="col-md-4">
          <label>GST %</label>
          <asp:TextBox ID="GSTPercentage" runat="server"
              CssClass="form-control"
              placeholder="Enter GST %"></asp:TextBox>
          <asp:RegularExpressionValidator ID="revGSTPercent" runat="server" ControlToValidate="GSTPercentage" ErrorMessage="Enter a valid %" ValidationExpression="^\d+(\.\d{1,2})?$" ForeColor="Red" Display="Dynamic"> </asp:RegularExpressionValidator>
      </div>
                <div class="col-md-4">
                    <label>Total Payable Amount <span style="color: red">*</span></label>
                    <asp:TextBox ID="TotalPayableAmount" runat="server"
                        CssClass="form-control"
                        ReadOnly="true"
                        placeholder="Total Payable Amount"></asp:TextBox>
                    <%--  <asp:RequiredFieldValidator
                        ID="RequiredFieldValidator6"
                        runat="server"
                        ControlToValidate="TotalPayableAmount"
                        ErrorMessage="Enter Total Payable Amount"
                        ForeColor="Red" />--%>
                </div>

                <div class="col-md-4">
                    <label>Total Amount </label>
                    <asp:TextBox ID="TotalAmount" runat="server"
                        CssClass="form-control" placeholder="Enter Total Amount"></asp:TextBox>
                </div>
            </div>
            <br />
            <div class="row">
                                <div class="col-md-4">
                    <label>Paid Date</label>
                    <div class="input-group">
                        <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                        <asp:TextBox ID="PaymentDate" runat="server"
                            CssClass="form-control pickadate"></asp:TextBox>
                    </div>
                </div>
                <div class="col-md-4">
                    <label>Payment Mode</label>
                    <asp:TextBox ID="PaymentMode" runat="server"
                        CssClass="form-control"
                        placeholder="Enter Payment Mode"></asp:TextBox>
                </div>

                <div class="col-md-4">
                    <label>Status <span style="color: red">*</span></label>
                    <asp:DropDownList ID="DD_Status" runat="server" CssClass="form-control">
                        <asp:ListItem Value="">-- Select Status --</asp:ListItem>
                        <asp:ListItem Value="0">Pending</asp:ListItem>
                        <asp:ListItem Value="1">Completed</asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator
                        ID="RequiredFieldValidator5"
                        runat="server"
                        ControlToValidate="DD_Status"
                        ErrorMessage="Please Select Status"
                        ForeColor="Red" />
                </div>
            </div>
            <div class="row">
                                <div class="col-md-4">
                    <label>Description</label>
                    <asp:TextBox ID="Description" runat="server"
                        CssClass="form-control"
                        TextMode="MultiLine"
                        Rows="4"
                        placeholder="Enter Description"></asp:TextBox>
                </div>

            </div>
            <!-- Buttons -->
            <div class="row">
                <div class="col-lg-12 text-right">
                    <a href="PayableinvoiceGrid.aspx" class="btn btn-primary">Back</a>

                    <asp:Button ID="btnSave" runat="server"
                        Text="Save"
                        CssClass="btn btn-primary"
                        OnClick="btn_send_Click" />

                    <asp:Button ID="btnUpdate" runat="server"
                        Text="Update"
                        CssClass="btn btn-primary"
                        OnClick="Btn_update_Click"
                        Visible="false" />
                </div>
            </div>
            <script type="text/javascript">
                window.onload = function () {
                    calculateTotal();
                };
            </script>

        </div>
    </div>
    <script>
        var today = new Date();

        $('.pickadate').pickadate({
            format: 'yyyy/mm/dd',
          
            selectMonths: true,
            selectYears: true,
            closeOnSelect: true
        });
    </script>
</asp:Content>

