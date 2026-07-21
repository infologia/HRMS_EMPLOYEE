<%@ Page Language="C#" AutoEventWireup="true"
    MasterPageFile="~/Masterpage/AdminMaster.master"
    CodeFile="createinvoice.aspx.cs"
    Inherits="Admin_createinvoice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <style>
        .panel-body label {
            display: block;
            margin-bottom: 6px;
            font-size: 13px;
        }

       

        .panel-body .row {
            margin-bottom: 10px;
        }

        @media (max-width: 768px) {

            .panel-body .col-md-4, .col-md-2, .col-md-3 {
                margin-bottom: 15px;
            }
        }
    </style>
    <style>
     

        .tax-wrapper {
            max-width: 1100px; /* 🔑 IMPORTANT */
        }

        .tax-row {
            display: flex;
            gap: 15px;
            align-items: flex-end;
            flex-wrap: nowrap;
        }



        .balance-item {
            white-space: nowrap;
        }

        .balance-text {
            color: red;
            font-weight: bold;
            margin-left: 5px;
        }
    </style>
    <style>
                .invoice-fieldset {
                    border: 1px solid #ddd;
                    padding: 10px 15px;
                    margin-bottom: 15px;
                    border-radius: 4px;
                    background-color: #fcfcfc;
                }
                .invoice-legend {
                    width: auto;
                    padding: 0 10px;
                    font-size: 14px;
                    font-weight: bold;
                    border-bottom: none;
                    margin-bottom: 0;
                }
                .compact-label {
                    font-size: 11px;
                }
                .amount-col {
                    margin-bottom: 10px;
                }
            </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Hidden fields for GST logic -->
    <asp:HiddenField ID="hfCountry" runat="server" />
    <asp:HiddenField ID="hfGstStateCode" runat="server" />

    <div class="panel panel-flat">
        <div class="panel-heading">
            <%--            <h5 class="panel-title">Receivable Invoice</h5>--%>
        </div>

        <div class="panel-body">

            <legend id="lgTitle" runat="server" class="text-semibold"><i class="icon-reading position-left"></i></legend>


            <fieldset class="invoice-fieldset">
                <legend class="invoice-legend">Invoice Details</legend>
                
                <div class="form-group form-group-xs" style="margin-bottom: 0;">
                        <div class="row">
                            <div class="col-sm-3">
                                <label class="compact-label">Clients <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlClient" runat="server" CssClass="form-control input-sm" AutoPostBack="true" OnSelectedIndexChanged="Rd_Status_SelectedIndexChanged"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlClient" ErrorMessage="Select Client" CssClass="text-danger" Display="Dynamic" />
                            </div>
                            <div class="col-sm-3">
                                <label class="compact-label">Projects <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddProjectName" runat="server" CssClass="form-control input-sm" AutoPostBack="true" OnSelectedIndexChanged="Rd_Project_SelectedIndexChanged"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddProjectName" ErrorMessage="Select Project" CssClass="text-danger" Display="Dynamic" />
                            </div>
                            <div class="col-sm-3">
                                <label class="compact-label">Invoice No <span class="text-danger">*</span></label>
                                <asp:TextBox ID="InvoiceNumber" runat="server" CssClass="form-control input-sm" placeholder="#000-YY-MM" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="InvoiceNumber" ErrorMessage="Enter No" CssClass="text-danger" Display="Dynamic" />
                                <asp:RegularExpressionValidator ID="RegexInvoiceNumber" runat="server" ControlToValidate="InvoiceNumber" ValidationExpression="^#[0-9]{3}-[0-9]{2}-(0[1-9]|1[0-2])$" ErrorMessage="Invalid format" CssClass="text-danger" Display="Dynamic" />
                            </div>
                            <div class="col-sm-3">
                                <label class="compact-label">Invoice Date <span class="text-danger">*</span></label>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                    <asp:TextBox ID="IT_InvoiceDate" runat="server" CssClass="form-control input-sm pickadate-start" placeholder="DD/MM/YYYY" />
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="IT_InvoiceDate" ErrorMessage="Enter Date" CssClass="text-danger" Display="Dynamic" />
                            </div>
                        </div>
                        
                        <div class="row" style="margin-top: 10px;">
                            <div class="col-sm-3">
                                <label class="compact-label">Due Date <span class="text-danger">*</span></label>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                    <asp:TextBox ID="IT_ReceivedDate" runat="server" CssClass="form-control input-sm pickadate" placeholder="DD/MM/YYYY" />
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="IT_ReceivedDate" ErrorMessage="Enter Due Date" CssClass="text-danger" Display="Dynamic" />
                            </div>
                            <div class="col-sm-3">
                                <label class="compact-label">Currency <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="DD_Currency" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DD_Currency" ErrorMessage="Select Currency" CssClass="text-danger" Display="Dynamic" />
                            </div>
                            <div class="col-sm-3">
                                <label class="compact-label">Status</label>
                                <asp:DropDownList ID="DD_Status" runat="server" CssClass="form-control input-sm"></asp:DropDownList>
                            </div>
                            <div class="col-sm-3">
                                <label class="compact-label">Notes</label>
                                <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control input-sm" placeholder="Notes..." />
                            </div>
                        </div>
                    </div>
            </fieldset>

            <fieldset class="invoice-fieldset">
                <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 10px;">
                    <legend class="invoice-legend" style="margin-bottom: 0;">Invoice Items</legend>
                    <button type="button" class="btn btn-success btn-xs" onclick="addRow()" style="padding: 2px 10px; height: 26px;">+ Add Row</button>
                </div>
                <div class="table-responsive">
                    <table class="table table-bordered table-condensed text-size-small" id="tblRows">
                        <thead class="bg-light">
                            <tr>
                                <th>Description</th>
                                <th>Amount</th>
                                <th style="width: 80px; text-align: center;">Action</th>
                            </tr>
                        </thead>
                        <tbody id="tblBody" runat="server">
                            <tr>
                                <td style="padding: 5px;"><textarea name="txtName" class="form-control input-sm txtName" rows="2" style="resize: vertical;"></textarea></td>
                                <td style="padding: 5px;"><input type="number" step="any" name="txtAmount" class="form-control input-sm txtAmount" /></td>
                                <td style="padding: 5px; text-align: center;"><button type="button" class="btn btn-danger btn-xs removeRow">Remove</button></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </fieldset>

            <fieldset class="invoice-fieldset">
                <legend class="invoice-legend">Amount Details</legend>
                <div class="form-group form-group-xs" style="margin-bottom: 0;">
                    <div class="row">
                        <div class="col-sm-2 tax-item amount-col" id="GSTPercentViews" runat="server">
                            <label class="compact-label">GST(%)</label>
                            <asp:TextBox ID="GstPercentage" runat="server" CssClass="form-control input-sm gst-calc" placeholder="%" />
                        </div>
                        <div class="col-sm-2 amount-col" id="SGSTViews" runat="server">
                            <label class="compact-label">SGST</label>
                            <asp:TextBox ID="SGSTAmount" runat="server" CssClass="form-control input-sm" ReadOnly="true" />
                        </div>
                        <div class="col-sm-2 amount-col" id="CGSTViews" runat="server">
                            <label class="compact-label">CGST</label>
                            <asp:TextBox ID="CGSTAmount" runat="server" CssClass="form-control input-sm" ReadOnly="true" />
                        </div>
                        <div class="col-sm-2 amount-col" id="IGSTViews" runat="server">
                            <label class="compact-label">IGST</label>
                            <asp:TextBox ID="IGSTAmount" runat="server" CssClass="form-control input-sm" ReadOnly="true" />
                        </div>
                        <div class="col-sm-2 tax-item amount-col" id="TDSPercentViews" runat="server">
                            <label class="compact-label">TDS(%)</label>
                            <asp:TextBox ID="TdsPercentage" runat="server" CssClass="form-control input-sm tds-calc" placeholder="%" />
                        </div>
                        <div class="col-sm-2 tax-item amount-col" id="TDSViews" runat="server">
                            <label class="compact-label">TDS Amount</label>
                            <asp:TextBox ID="TdsAmount" runat="server" CssClass="form-control input-sm" />
                        </div>
                        <div class="col-sm-2 amount-col">
                            <label class="compact-label">Sub Total</label>
                            <asp:TextBox ID="SubTotal" runat="server" CssClass="form-control input-sm" placeholder="Sub Total" ReadOnly="true" />
                        </div>
                        <div class="col-sm-2 amount-col">
                            <label class="compact-label">Total Amount <span class="text-danger">*</span></label>
                            <asp:TextBox ID="TotalAmount" runat="server" CssClass="form-control input-sm" placeholder="Total" ReadOnly="true" />
                        </div>
                        <div class="col-sm-2 amount-col" id="ReceivedViews" runat="server">
                            <label class="compact-label">Received Amount</label>
                            <asp:TextBox ID="ReceivedAmount" runat="server" CssClass="form-control input-sm" placeholder="Received" />
                        </div>
                        <div class="col-sm-3 amount-col" id="ReceiveddateViews" runat="server">
                            <label class="compact-label">Received Date</label>
                            <div class="input-group">
                                <span class="input-group-addon" style="padding: 4px 8px;"><i class="icon-calendar22" style="font-size: 12px;"></i></span>
                                <asp:TextBox ID="Receiveddate" runat="server" CssClass="form-control input-sm pickadate" />
                            </div>
                        </div>
                    </div>

                    <div class="row" id="BalanceViews" runat="server" style="margin-top: 15px;">
                        <div class="col-sm-12 text-center">
                            <label class="compact-label" style="margin-right: 25px;">
                                Total Invoices : 
                                <span id="lblInvoiceCount" runat="server" class="balance-text" style="color: #333;">0</span>
                            </label>
                            <label class="compact-label" style="margin-right: 25px;">
                                Total Amount : 
                                <span id="lblTotalAmount" runat="server" class="balance-text" style="color: #333;">0.00</span>
                            </label>
                            <label class="compact-label">
                                Balance Amount : 
                                <span id="Balance" runat="server" class="balance-text" style="color: #333;">0.00</span>
                            </label>
                        </div>
                    </div>
                </div>
            </fieldset>

            <br />

            <div class="row pull-right">
                <div class="col-lg-12 text-right-md">
                    <a href="ReceivableInvoiceGrid.aspx" class="btn btn-primary">Back</a>
                    <asp:Button ID="btnSave" runat="server"
                        Text="Save"
                        CssClass="btn btn-primary"
                        OnClick="btn_send_Click"
                        UseSubmitBehavior="false" />
                    <asp:Button ID="btnUpdate" runat="server"
                        Text="Update"
                        CssClass="btn btn-primary"
                        OnClick="Btn_update_Click"
                        UseSubmitBehavior="false"
                        Visible="false" />
                </div>
            </div>
        </div>
                        </div>

    <div class="panel panel-flat" id="divGrid" runat="server" visible="false">
        <div class="panel-heading">
            <div class="row">
                <div class="col-lg-4">
                    <h5 class="panel-title">Invoices Details</h5>
                </div>
            </div>
        </div>
        <div class="panel-body" style="padding: 0px;">
        </div>
        <table class="table datatable-basic">
            <thead>
                <tr>

                    <th>Project Name</th>
                    <th>Invoice Number</th>
                    <th>Total Amount</th>
                    <th>Invoice Date</th>
                    <th>Received Date</th>
                    <th>Created On</th>

                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_invoice" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
    </div>

    <asp:HiddenField ID="hdnSubTotal" runat="server" />
    <asp:HiddenField ID="hdnSGST" runat="server" />
    <asp:HiddenField ID="hdnCGST" runat="server" />
    <asp:HiddenField ID="hdnIGST" runat="server" />
    <asp:HiddenField ID="hdnTotal" runat="server" />
    <asp:HiddenField ID="hfInvoiceKey" runat="server" />
    <asp:HiddenField ID="hfDescription" runat="server" />

    <!-- ================= JAVASCRIPT ================= -->
    <script>

        function addRow() {

            $("#tblRows tbody").append(`
        <tr>
            <td style="padding: 5px;"><textarea name="txtName" class="form-control input-sm txtName" rows="2" style="resize: vertical;"></textarea></td>
            <td style="padding: 5px;"><input type="number" step="any" name="txtAmount" class="form-control input-sm txtAmount" /></td>
            <td style="padding: 5px; text-align: center;"><button type="button" class="btn btn-danger btn-xs removeRow">Remove</button></td>
        </tr>
    `);
        }

        $(document).on("keyup change", ".txtAmount", function () {
            calculateSubTotal();
        });

        $(document).on("click", ".removeRow", function () {
            if ($("#tblRows tbody tr").length > 1) {
                $(this).closest("tr").remove();
                calculateSubTotal();
            }
        });
        function calculateSubTotal() {
            var sub = 0;
            $(".txtAmount").each(function () {
                var v = parseFloat($(this).val());
                if (!isNaN(v)) sub += v;
            });

            var des = "";

            $(".txtName").each(function () {
                var txt = $(this).val().trim();
                if (txt !== "") {
                    des += txt + ", ";   // description concatenate
                } else {

                    if (txt === "") {
                        message = "Please enter Description in row.";
                        toastr.options = {
                            closeButton: true,
                            progressBar: true,
                            positionClass: "toast-bottom-right",
                            timeOut: 3000
                        };
                        toastr.warning(message);
                        return false;

                    }
                }
            });

            // last comma remove
            if (des.endsWith(", ")) {
                des = des.slice(0, -2);
            }


            $("#<%= SubTotal.ClientID %>").val(sub.toFixed(2));




            calculateGST();
        }


        function calculateGST() {
            try {
                var subTotalRaw = $("#<%= SubTotal.ClientID %>").val() || "";
                subTotalRaw = subTotalRaw.replace(/,/g, '');
                var subTotal = parseFloat(subTotalRaw) || 0;
                
                var taxType = ($("#<%= hfGstStateCode.ClientID %>").val() || "").trim();

                var $gstInput = $("#<%= GstPercentage.ClientID %>");
                var gstPercentStr = $gstInput.val() ? $gstInput.val().trim() : '';
                var gstPercent = parseFloat(gstPercentStr);

                var sgst = 0, cgst = 0, igst = 0, total = 0;

                if (gstPercentStr !== '' && !isNaN(gstPercent)) {
                    if (taxType === "1") { // Same State
                        sgst = (subTotal * (gstPercent / 2)) / 100;
                        cgst = (subTotal * (gstPercent / 2)) / 100;
                    } else if (taxType === "2") { // Other State
                        igst = (subTotal * gstPercent) / 100;
                    }
                }
                
                total = subTotal + sgst + cgst + igst;

                $("#<%= SGSTAmount.ClientID %>").val(sgst > 0 ? sgst.toFixed(2) : '0.00');
                $("#<%= CGSTAmount.ClientID %>").val(cgst > 0 ? cgst.toFixed(2) : '0.00');
                $("#<%= IGSTAmount.ClientID %>").val(igst > 0 ? igst.toFixed(2) : '0.00');
                $("#<%= TotalAmount.ClientID %>").val(total > 0 ? total.toFixed(2) : subTotal.toFixed(2));

                $("#<%= hdnSubTotal.ClientID %>").val(subTotal.toFixed(2));
                $("#<%= hdnSGST.ClientID %>").val(sgst.toFixed(2));
                $("#<%= hdnCGST.ClientID %>").val(cgst.toFixed(2));
                $("#<%= hdnIGST.ClientID %>").val(igst.toFixed(2));
                $("#<%= hdnTotal.ClientID %>").val(total.toFixed(2));

                calculateTDS();
            } catch (e) {
                console.error("Error in calculateGST:", e);
            }
        }

        function calculateTDS() {
            try {
                var subTotalRaw = $("#<%= SubTotal.ClientID %>").val();
                var subTotal = parseFloat(subTotalRaw) || 0;
                var $tdsInput = $("#<%= TdsPercentage.ClientID %>");
                var tdsPercentStr = $tdsInput.val() ? $tdsInput.val().trim() : '';
                var tdsPercent = parseFloat(tdsPercentStr);

                if (tdsPercentStr === '' || isNaN(tdsPercent)) {
                    $("#<%= TdsAmount.ClientID %>").val('');
                } else if (subTotal >= 0) {
                    var calculatedTds = (subTotal * tdsPercent) / 100;
                    $("#<%= TdsAmount.ClientID %>").val(calculatedTds.toFixed(2));
                }
            } catch (e) {
                console.error("Error calculating TDS: ", e);
            }
        }

    </script>
    <script>
        $(document).on("keyup change input", ".tds-calc", function (e) {
            calculateTDS();
        });

        $(document).on("keyup change input", ".gst-calc", function (e) {
            calculateGST();
        });

        $(document).ready(function () {
            calculateSubTotal();

            // Prevent Enter key from submitting the form accidentally
            $(window).on("keydown", function (e) {
                if (e.keyCode === 13) {
                    var target = e.target.nodeName.toLowerCase();
                    if (target !== 'textarea' && target !== 'button' && target !== 'a') {
                        e.preventDefault();
                        return false;
                    }
                }
            });
        });
    </script>

    <script>
        var startPicker, endPicker;

        // Start Date Picker
        startPicker = $('.pickadate-start').pickadate({
            format: 'dd/mm/yyyy',
            selectMonths: true,
            selectYears: true,
            closeOnSelect: true,
            onSet: function (context) {
                if (context.select) {
                    var startDate = new Date(context.select);

                    // End date must be FUTURE of start date
                    startDate.setDate(startDate.getDate() + 1);

                    if (endPicker) {
                        endPicker.set('min', startDate);
                        endPicker.clear(); // clear old end date
                    }
                }
            }
        }).pickadate('picker');
        endPicker = $('.pickadate').pickadate({
            format: 'dd/mm/yyyy',
            selectMonths: true,
            selectYears: true,
            closeOnSelect: true
        }).pickadate('picker');
    </script>

</asp:Content>
