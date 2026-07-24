<%@ Page Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="AmcDetails.aspx.cs" Inherits="Admin_AmcDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        /* Label spacing */
        .panel-body label {
            display: block;
            margin-bottom: 6px;
            font-size: 13px;
        }

        .panel-body textarea.form-control {
            margin-bottom: 16px;
            resize: vertical;
        }
.panel-body .row {
    margin-bottom: 25px;   /* adjust as needed */
}

        @media (max-width: 768px) {

            .panel-body .col-md-4 {
                margin-bottom: 25px;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
   <div class="panel panel-flat">
            <div class="panel-heading">
                <h5 class="panel-title">AMC Details</h5>
            </div>

            <div class="panel-body">
                <legend class="text-semibold"><i class="icon-reading position-left"></i>Create AMC</legend>

                <!-- Row 1 -->
                <div class="row">
                    <!-- Client -->
                    <div class="col-md-4">
                        <label>Client <span style="color: red">*</span></label>
                        <asp:DropDownList ID="DD_Client" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DD_Client_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvClient" runat="server" ControlToValidate="DD_Client"
                            InitialValue="" ErrorMessage="Please select client" ForeColor="Red"
                            Display="Dynamic" ValidationGroup="AMC" />
                    </div>

                    <!-- Project -->
                    <div class="col-md-4">
                        <label>Project <span style="color: red">*</span></label>
                        <asp:DropDownList ID="DD_Project" runat="server" CssClass="form-control" AppendDataBoundItems="true">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvProject" runat="server" ControlToValidate="DD_Project"
                            InitialValue="" ErrorMessage="Please select project" ValidationGroup="AMC"
                            Display="Dynamic" ForeColor="Red" />
                    </div>

                    <!-- Live Date -->
                    <div class="col-md-4">
                        <label>Live Date</label>
                        <div class="input-group">
                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                            <asp:TextBox ID="txt_livedate" runat="server" CssClass="form-control pickadate" placeholder="DD/MM/YYYY"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <!-- Row 2 -->
                <div class="row">
                    <!-- AMC Start Date -->
                    <div class="col-md-4">
                        <label>AMC Start Date <span style="color: red">*</span></label>
                        <div class="input-group">
                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                            <asp:TextBox ID="txt_StartdDate" runat="server" CssClass="form-control pickadate" placeholder="DD/MM/YYYY"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvStartDate" runat="server" ControlToValidate="txt_StartdDate"
                                ErrorMessage="Please select AMC start date" ValidationGroup="AMC" Display="Dynamic" ForeColor="Red" />
                        </div>
                    </div>

                    <!-- AMC Next Date -->
                    <div class="col-md-4">
                        <label>AMC Next Date</label>
                        <div class="input-group">
                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                            <asp:TextBox ID="txt_EndDate" runat="server" CssClass="form-control pickadate" placeholder="DD/MM/YYYY"></asp:TextBox>
                        </div>
                    </div>

                    <!-- Project Cost -->
                    <div class="col-md-4">
                        <label>Project Cost</label>
                        <asp:TextBox ID="txt_PP_Cost" runat="server" CssClass="form-control" placeholder="Enter Project Cost"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="rev_PP_Cost" runat="server" ControlToValidate="txt_PP_Cost"
                            ErrorMessage="Enter a valid amount (numbers only, up to 2 decimals)"
                            ValidationExpression="^\d+(\.\d{1,2})?$" ForeColor="Red" Display="Dynamic" ValidationGroup="AMC" />
                    </div>
                </div>

                <!-- Row 3 -->
                <div class="row">
                    <!-- AMC Amount -->
                    <div class="col-md-4">
                        <label>AMC Amount</label>
                        <asp:TextBox ID="txt_AmcAmount" runat="server" CssClass="form-control" placeholder="Enter AMC Amount"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="rev_AmcAmount" runat="server" ControlToValidate="txt_AmcAmount"
                            ErrorMessage="Enter a valid AMC amount (numbers only)"
                            ValidationExpression="^\d+(\.\d{1,2})?$" ForeColor="Red" Display="Dynamic" ValidationGroup="AMC" />
                    </div>

                    <!-- INR Amount -->
                    <div class="col-md-4">
                        <label>INR Amount</label>
                        <asp:TextBox ID="txt_INRAmount" runat="server" CssClass="form-control" placeholder="Enter INR Amount"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="rev_INRAmount" runat="server" ControlToValidate="txt_INRAmount"
                            ErrorMessage="Enter a valid INR amount (numbers only)"
                            ValidationExpression="^\d+(\.\d{1,2})?$" ForeColor="Red" Display="Dynamic" ValidationGroup="AMC" />
                    </div>

                    <!-- Status -->
                    <div class="col-md-4">
                        <label>Status</label>
                        <asp:DropDownList ID="DD_Status" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Live" Value="0" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Closed" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>

                <!-- Row 4 -->
                <div class="row">
                    <div class="col-md-12">
                        <label>Description</label>
                        <asp:TextBox ID="txt_Description" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" placeholder="Enter Description"></asp:TextBox>
                    </div>
                </div>

                <br />

                <!-- Document Details Section -->
                <legend class="text-semibold" style="margin-bottom: 8px;">
                    <i class="icon-file-text2 position-left"></i>AMC Details
                </legend>

                <div style="margin-bottom: 6px;">
                    <button type="button" class="btn-add-row" onclick="addDocumentRow()">
                        <i class="icon-plus2"></i> Add Row
                    </button>
                </div>

                <table class="invoice-table table-condensed" id="docDetailsTable">
                    <thead>
                        <tr>
                            <th style="font-size:12px">AMC Amount</th>
                            <th style="font-size:12px">Description</th>
                            <th style="width:130px;font-size:12px">Status</th>
                            <th style="width:230px;font-size:12px">AMC Date</th>
                            <th style="width:230px;font-size:12px">Next Date</th>
                            <th style="width:10px; text-align:center;font-size:12px">Action</th>
                        </tr>
                    </thead>
                    <tbody id="tBodyDocs" runat="server">
                        <tr>
                            <td><input type="text" class="form-control" name="rowAmcAmount[]" placeholder="Enter AMC Amount" /></td>
                            <td><textarea class="form-control" name="rowDescription[]" rows="1" placeholder="Enter Description"></textarea></td>
                            <td>
                                <select class="form-control" name="rowStatus[]">
                                    <option value="">Select</option>
                                    <option value="Not Received">Not Received</option>
                                    <option value="Received">Received</option>
                                </select>
                            </td>
                            <td>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                    <input type="text" class="form-control pickadate" placeholder="DD/MM/YYYY" name="rowAmcDate[]" readonly />
                                </div>
                            </td>
                            <td>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                    <input type="text" class="form-control pickadate" placeholder="DD/MM/YYYY" name="rowNextDate[]" readonly />
                                </div>
                            </td>
                            <td style="text-align:center;"><button type="button" class="btn btn-xs btn-danger removeDocRow"><i class="icon-trash"></i></button></td>
                        </tr>
                    </tbody>
                </table>


                <br />
                <div class="row pull-right">
                    <div class="col-lg-12 pull-right">
                        <a href="amc.aspx" class="btn btn-primary">Back</a>
                        <asp:Button ID="btn_request" runat="server" Text="Create" CssClass="btn btn-primary"
                            OnClick="btn_request_Click" ValidationGroup="AMC" CausesValidation="true" />
                        <asp:Button ID="btn_update" runat="server" Text="Update" CssClass="btn btn-primary"
                            OnClick="btn_update_Click" Visible="false" ValidationGroup="AMC" CausesValidation="true" />
                    </div>
                </div>
                <asp:HiddenField ID="hfProjectKey" runat="server" />
            </div>
        </div>
    <script type="text/javascript">
    function addDocumentRow() {
        $("#docDetailsTable tbody").append(`
            <tr>
                <td><input type="text" class="form-control" name="rowAmcAmount[]" placeholder="Enter AMC Amount" /></td>
                <td><textarea class="form-control" name="rowDescription[]" rows="1" placeholder="Enter Description"></textarea></td>
                <td>
                    <select class="form-control" name="rowStatus[]">
                        <option value="">-- Select --</option>
                        <option value="Live">Live</option>
                        <option value="Closed">Closed</option>
                    </select>
                </td>
                <td>
                    <div class="input-group">
                        <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                        <input type="text" class="form-control pickadate-doc" placeholder="DD/MM/YYYY" name="rowAmcDate[]" readonly />
                    </div>
                </td>
                <td>
                    <div class="input-group">
                        <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                        <input type="text" class="form-control pickadate-doc" placeholder="DD/MM/YYYY" name="rowNextDate[]" readonly />
                    </div>
                </td>
                <td style="text-align:center;"><button type="button" class="btn btn-xs btn-danger removeDocRow"><i class="icon-trash"></i></button></td>
            </tr>
        `);
        setTimeout(function () {
            $('#docDetailsTable tbody tr:last .pickadate-doc').each(function () {
                var p = $(this).pickadate('picker');
                if (p) p.stop();
            });
            $('#docDetailsTable tbody tr:last .pickadate-doc').pickadate({
                format: 'dd/mm/yyyy',
                selectMonths: true,
                selectYears: true,
                closeOnSelect: true
            });
        }, 50);
    }

    $(document).on("click", ".removeDocRow", function () {
        if ($("#docDetailsTable tbody tr").length > 1) {
            $(this).closest("tr").remove();
        } else {
            alert('At least one document row is required.');
        }
    });

    $(document).ready(function () {
        $('.pickadate-doc').pickadate({
            format: 'dd/mm/yyyy',
            selectMonths: true,
            selectYears: true,
            closeOnSelect: true
        });
    });
    </script>

    <style>
        .invoice-table { width: 100%; border-collapse: collapse; margin-top: 6px; }
        .invoice-table thead tr { background-color: #f0f4f8; }
        .invoice-table > thead > tr > th { padding: 5px 8px; font-size: 12px; font-weight: 600; border: 1px solid #dde3ea; text-align: left; color: #444; line-height: 1.2; }
        .invoice-table > tbody > tr > td { padding: 3px 6px; border: 1px solid #dde3ea; vertical-align: middle; }
        .invoice-table .form-control { margin-bottom: 0; height: 26px; padding: 2px 6px; font-size: 12px; line-height: 1.2; }
        .invoice-table td textarea.form-control { margin-bottom: 0 !important; height: 26px; min-height: 26px; resize: vertical; padding: 2px 6px; font-size: 12px; }
        .invoice-table .input-group { height: 26px; }
        .invoice-table .input-group-addon { padding: 2px 6px; font-size: 12px; height: 26px; line-height: 1.6; }
        .btn-add-row { background: #3a7bd5; color: #fff; border: none; border-radius: 4px; padding: 5px 14px; font-size: 12px; cursor: pointer; }
        .btn-add-row:hover { background: #2a5fb5; }
    </style>
</asp:Content>
