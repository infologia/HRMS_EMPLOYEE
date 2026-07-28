<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="ARPaymentEntry.aspx.cs" Inherits="Admin_ARPaymentEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .panel-body label {
            display: block;
            margin-bottom: 6px;
            font-size: 13px;
        }
        .readonly-field {
            background-color: #f2f2f2 !important;
            pointer-events: none;
        }
        .hidden-field {
            display: none;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="panel panel-flat">
        <div class="panel-heading">
        </div>

        <div class="panel-body">
            <legend class="text-semibold">
                <i class="icon-briefcase position-left"></i><span id="headcreate" runat="server">Create AR Payment</span>
            </legend>

            <!-- Row 1 -->
            <div class="row">
                <div class="col-md-4">
                    <label>Client Name <span style="color:red">*</span></label>
                    <asp:DropDownList ID="ddlClientName" runat="server"
                        CssClass="form-control form-select-sm"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlClientName_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server"
                        ControlToValidate="ddlClientName"
                        InitialValue=""
                        ErrorMessage="Please select a Client Name"
                        ForeColor="Red" Display="Dynamic" />
                </div>

                <div class="col-md-4">
                    <label>Invoice No <span style="color:red">*</span></label>
                    <asp:DropDownList ID="ddlInvoiceNo" runat="server"
                        CssClass="form-control form-control-sm"
                        AutoPostBack="true"
                        OnSelectedIndexChanged="ddlInvoiceNo_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator runat="server"
                        ControlToValidate="ddlInvoiceNo"
                        InitialValue=""
                        ErrorMessage="Please select an Invoice No"
                        ForeColor="Red" Display="Dynamic" />
                </div>

                <div class="col-md-4">
                    <label>Invoice Date <span style="color:red">*</span></label>
                    <div class="input-group">
                        <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                        <asp:TextBox ID="txtInvoiceDate" runat="server"
                            CssClass="form-control form-control-sm pickadate"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator runat="server"
                        ControlToValidate="txtInvoiceDate"
                        ErrorMessage="Please select an Invoice Date"
                        ForeColor="Red" Display="Dynamic" />
                </div>
            </div>
            <br />

            <!-- Row 2 -->
            <div class="row">
                <div class="col-md-2">
                    <label>SubTotal</label>
                    <asp:TextBox ID="txtSubTotal" runat="server"
                        CssClass="form-control form-control-sm readonly-field"
                        ReadOnly="true"></asp:TextBox>
                </div>

                <div id="divGSTPercent" class="col-md-2">
                    <label>GST %</label>
                    <asp:TextBox ID="txtGSTPercent" runat="server"
                        CssClass="form-control form-control-sm"
                        placeholder="GST %"></asp:TextBox>
                </div>

                <div id="divGSTAmount" class="col-md-2">
                    <label>GST Amount</label>
                    <asp:TextBox ID="txtGST" runat="server"
                        CssClass="form-control form-control-sm readonly-field"
                        ReadOnly="true"></asp:TextBox>
                </div>

                <div id="divTDSPercent" class="col-md-2">
                    <label>TDS %</label>
                    <asp:TextBox ID="txtPercent" runat="server"
                        CssClass="form-control form-control-sm"
                        placeholder="TDS %"></asp:TextBox>
                </div>

                <div id="divTDSAmount" class="col-md-2">
                    <label>TDS Amount</label>
                    <asp:TextBox ID="txtAmount" runat="server"
                        CssClass="form-control form-control-sm readonly-field"
                        ReadOnly="true"></asp:TextBox>
                </div>
                
                <div id="divConversionAmount" class="col-md-2 hidden-field">
                    <label>Conversion Amount</label>
                    <asp:TextBox ID="txtConversionAmount" runat="server"
                        CssClass="form-control form-control-sm"
                        placeholder="0.00"></asp:TextBox>
                </div>

                <div class="col-md-2">
                    <label>GrandTotal</label>
                    <asp:TextBox ID="txtGrandTotal" runat="server"
                        CssClass="form-control form-control-sm readonly-field"
                        ReadOnly="true"></asp:TextBox>
                </div>
            </div>            <br />

            <!-- Item Details Panel -->
            <div class="panel panel-default border-grey">
                <div class="panel-heading" style="padding-bottom:10px;">
                    <h6 class="panel-title text-semibold text-primary">Item Details</h6>
                    <div class="heading-elements">
                        <button type="button" class="btn btn-primary btn-xs" onclick="addPaymentRow()">
                            <i class="icon-plus-circle2"></i> Add rows
                        </button>
                    </div>
                </div>
                <div>
                    <table class="table table-bordered table-striped table-xxs" style="font-size:12px;">
                        <thead class="bg-primary">
                            <tr>
                                <th style="padding: 5px;">Payment Transaction No</th>
                                <th style="padding: 5px;">Payment Date</th>
                                <th style="padding: 5px;">Payment Amount</th>
                                <th class="text-center" style="padding: 5px;">Remove</th>
                            </tr>
                        </thead>
                        <tbody id="tblPaymentBody">
                            <tr id="emptyRow">
                                <td colspan="4" class="text-center text-muted py-3">
                                    No rows added. Click <strong>Add rows</strong> to begin.
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>

            <br />
            <!-- Row 3: Summary -->
            <div class="row">
                <div class="col-md-4">
                    <label>Net Due</label>
                    <asp:TextBox ID="txtNetDue" runat="server"
                        CssClass="form-control form-control-sm readonly-field"
                        ReadOnly="true"></asp:TextBox>
                </div>

                <div class="col-md-4">
                    <label>Payment</label>
                    <asp:TextBox ID="txtPayment" runat="server"
                        CssClass="form-control form-control-sm readonly-field"
                        ReadOnly="true"></asp:TextBox>
                </div>

                <div class="col-md-4">
                    <label>Balance Amount</label>
                    <asp:TextBox ID="txtBalanceAmount" runat="server"
                        CssClass="form-control form-control-sm readonly-field"
                        ReadOnly="true"></asp:TextBox>
                </div>
            </div>

            <!-- Hidden fields -->
            <asp:HiddenField ID="hfPaymentRows" runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hfARId"        runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hfAmount"      runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hfNetDue"      runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hfPayment"     runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hfBalanceAmt"  runat="server" ClientIDMode="Static" />
            <asp:HiddenField ID="hfIsForeignClient" runat="server" ClientIDMode="Static" Value="0" />

            <br />
            <!-- Buttons -->
            <div class="row">
                <div class="col-lg-12 text-right">
                    <a href="ARPaymentGrid.aspx" class="btn btn-primary">Back</a>
                    <asp:Button ID="btnSave" runat="server"
                        CssClass="btn btn-primary"
                        Text="Save"
                        OnClick="btnSave_Click"
                        OnClientClick="return prepareAndValidate();" />
                </div>
            </div>

        </div>
    </div>

    <script>
        function showToastr(type, message) {
            if (typeof showToastrMsg === 'function') {
                showToastrMsg(type, message);
            } else if (typeof toastr !== 'undefined') {
                if(type === 'success') toastr.success(message);
                else if(type === 'error') toastr.error(message);
                else toastr.info(message);
            } else {
                alert(message);
            }
        }

        let rowCount = 0;

        function addPaymentRow() {
            const tbody = document.getElementById('tblPaymentBody');
            const empty = document.getElementById('emptyRow');
            if (empty) empty.remove();

            rowCount++;
            const tr = document.createElement('tr');
            tr.id = 'payrow_' + rowCount;
            tr.innerHTML = `
                <td style="padding: 3px;"><input type="text"   class="form-control input-sm" style="height:28px; font-size:11px;" name="txnNo_${rowCount}"  placeholder="Transaction No" /></td>
                <td style="padding: 3px;">
                    <div class="input-group" style="margin-bottom:0;">
                        <span class="input-group-addon" style="padding: 4px 8px;"><i class="icon-calendar22" style="font-size:12px;"></i></span>
                        <input type="text" class="form-control pickadate-row input-sm" style="height:28px; font-size:11px;" name="payDate_${rowCount}" placeholder="yyyy/mm/dd" />
                    </div>
                </td>
                <td style="padding: 3px;"><input type="number" class="form-control input-sm" style="height:28px; font-size:11px;" name="payAmt_${rowCount}"  placeholder="0.00" step="0.01" min="0" oninput="recalcTotals()" /></td>
                <td class="text-center" style="padding: 3px;">
                    <button type="button" class="btn btn-danger btn-xs" style="padding: 3px 6px;" onclick="removePayRow(${rowCount})">
                        <i class="icon-trash"></i>
                    </button>
                </td>`;
            tbody.appendChild(tr);
            
            // Re-initialize pickadate on the new row
            $('.pickadate-row').pickadate({
                format: 'yyyy/mm/dd',
                selectMonths: true,
                selectYears: true,
                closeOnSelect: true
            });

            recalcTotals();
        }

        function removePayRow(id) {
            const row = document.getElementById('payrow_' + id);
            if (row) row.remove();
            const tbody = document.getElementById('tblPaymentBody');
            if (tbody.rows.length === 0) {
                const tr = document.createElement('tr');
                tr.id = 'emptyRow';
                tr.innerHTML = '<td colspan="4" class="text-center text-muted py-3">No rows added. Click <strong>Add rows</strong> to begin.</td>';
                tbody.appendChild(tr);
            }
            recalcTotals();
        }

        function recalcTotals() {
            let total = 0;
            document.querySelectorAll('[name^="payAmt_"]').forEach(i => total += parseFloat(i.value) || 0);

            const netDue = parseFloat(document.getElementById('hfNetDue').value) || 0;
            const payment = total.toFixed(2);
            const balance = (netDue - total).toFixed(2);
            
            document.getElementById('<%= txtPayment.ClientID %>').value       = payment;
            document.getElementById('<%= txtBalanceAmount.ClientID %>').value = balance;
            
            document.getElementById('hfPayment').value    = payment;
            document.getElementById('hfBalanceAmt').value = balance;
        }

        function calculateAmount() {
            const subTotal = parseFloat(document.getElementById('<%= txtSubTotal.ClientID %>').value) || 0;
            const isForeign = document.getElementById('hfIsForeignClient').value === "1";
            
            if (isForeign) {
                const convAmt = parseFloat(document.getElementById('<%= txtConversionAmount.ClientID %>').value) || 0;
                const grand = subTotal * convAmt;
                document.getElementById('<%= txtGrandTotal.ClientID %>').value = grand.toFixed(2);
                
                // For foreign, TDS is 0, Net Due is Grand Total
                document.getElementById('<%= txtAmount.ClientID %>').value = "0.00";
                document.getElementById('<%= txtNetDue.ClientID %>').value = grand.toFixed(2);
                
                document.getElementById('hfAmount').value = "0.00";
                document.getElementById('hfNetDue').value = grand.toFixed(2);
            } else {
                const percent  = parseFloat(document.getElementById('<%= txtPercent.ClientID %>').value)   || 0;
                const grand    = parseFloat(document.getElementById('<%= txtGrandTotal.ClientID %>').value) || 0;
                
                const amount = ((subTotal * percent) / 100);
                document.getElementById('<%= txtAmount.ClientID %>').value = amount.toFixed(2);
                
                const netDue = grand - amount;
                document.getElementById('<%= txtNetDue.ClientID %>').value = netDue.toFixed(2);
                
                document.getElementById('hfAmount').value = amount.toFixed(2);
                document.getElementById('hfNetDue').value = netDue.toFixed(2);
            }
            
            recalcTotals();
        }

        function toggleForeignClientView() {
            const isForeign = document.getElementById('hfIsForeignClient').value === "1";
            const divGSTPercent = document.getElementById('divGSTPercent');
            const divGSTAmount = document.getElementById('divGSTAmount');
            const divTDSPercent = document.getElementById('divTDSPercent');
            const divTDSAmount = document.getElementById('divTDSAmount');
            const divConversion = document.getElementById('divConversionAmount');
            
            if (isForeign) {
                if (divGSTPercent) divGSTPercent.classList.add('hidden-field');
                if (divGSTAmount) divGSTAmount.classList.add('hidden-field');
                if (divTDSPercent) divTDSPercent.classList.add('hidden-field');
                if (divTDSAmount) divTDSAmount.classList.add('hidden-field');
                if (divConversion) divConversion.classList.remove('hidden-field');
            } else {
                if (divGSTPercent) divGSTPercent.classList.remove('hidden-field');
                if (divGSTAmount) divGSTAmount.classList.remove('hidden-field');
                if (divTDSPercent) divTDSPercent.classList.remove('hidden-field');
                if (divTDSAmount) divTDSAmount.classList.remove('hidden-field');
                if (divConversion) divConversion.classList.add('hidden-field');
            }
        }

        document.addEventListener('DOMContentLoaded', function () {
            document.getElementById('<%= txtPercent.ClientID %>').addEventListener('input', calculateAmount);
            document.getElementById('<%= txtConversionAmount.ClientID %>').addEventListener('input', calculateAmount);
            toggleForeignClientView();
        });

        function loadExistingPaymentRows() {
            const hfValue = document.getElementById('hfPaymentRows').value;
            if (!hfValue) return;

            const rows = JSON.parse(hfValue);
            const tbody = document.getElementById('tblPaymentBody');
            const empty = document.getElementById('emptyRow');
            if (empty) empty.remove();

            rows.forEach((row, index) => {
                rowCount++;
                const tr = document.createElement('tr');
                tr.id = 'payrow_' + rowCount;
                tr.innerHTML = `
                    <td style="padding: 3px;"><input type="text"   class="form-control input-sm" style="height:28px; font-size:11px;" name="txnNo_${rowCount}"  placeholder="Transaction No" value="${row.txnNo || ''}" /></td>
                    <td style="padding: 3px;">
                        <div class="input-group" style="margin-bottom:0;">
                            <span class="input-group-addon" style="padding: 4px 8px;"><i class="icon-calendar22" style="font-size:12px;"></i></span>
                            <input type="text" class="form-control pickadate-row input-sm" style="height:28px; font-size:11px;" name="payDate_${rowCount}" placeholder="yyyy/mm/dd" value="${row.payDate || ''}" />
                        </div>
                    </td>
                    <td style="padding: 3px;"><input type="number" class="form-control input-sm" style="height:28px; font-size:11px;" name="payAmt_${rowCount}"  placeholder="0.00" step="0.01" min="0" value="${row.payAmt || ''}" oninput="recalcTotals()" /></td>
                    <td class="text-center" style="padding: 3px;">
                        <button type="button" class="btn btn-danger btn-xs" style="padding: 3px 6px;" onclick="removePayRow(${rowCount})">
                            <i class="icon-trash"></i>
                        </button>
                    </td>`;
                tbody.appendChild(tr);
            });
            
            $('.pickadate-row').pickadate({
                format: 'yyyy/mm/dd',
                selectMonths: true,
                selectYears: true,
                closeOnSelect: true
            });
            recalcTotals();
        }

        function serializePaymentRows() {
            const rows = [];
            document.querySelectorAll('[id^="payrow_"]').forEach(tr => {
                rows.push({
                    txnNo:   tr.querySelector('input[type="text"]').value.trim(),
                    payDate: tr.querySelector('input.pickadate-row').value,
                    payAmt:  tr.querySelector('input[type="number"]').value
                });
            });
            document.getElementById('hfPaymentRows').value = JSON.stringify(rows);
        }

        function prepareAndValidate() {
            if (typeof(Page_ClientValidate) == 'function') {
                if (!Page_ClientValidate()) {
                    return false;
                }
            }
            serializePaymentRows();
            return true;
        }

        $(document).ready(function() {
            $('.pickadate').pickadate({
                format: 'yyyy/mm/dd',
                selectMonths: true,
                selectYears: true,
                closeOnSelect: true
            });
        });
    </script>
</asp:Content>
