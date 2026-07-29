<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="CreatePettyCash.aspx.cs" Inherits="Admin_CreatePettyCash" %>

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
            margin-bottom: 10px;
        }

        @media (max-width: 768px) {

            .panel-body .col-md-4 {
                margin-bottom: 15px;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-2"></div>
        <div class="col-md-8">
            <div class="panel panel-flat">

                <div class="panel-body">
                    <legend class="text-semibold">
                        <i class="icon-briefcase position-left"></i>Create Petty Cash
                    </legend>
                    <asp:HiddenField ID="hfProjectKey" runat="server" />
                    <div class="row">
                        <div class="col-md-4">
                            <div class="form-group">
                                <label>Amount <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtAmount" runat="server"
                                    CssClass="form-control"
                                    placeholder="Enter amount" />
                                <asp:RequiredFieldValidator
                                    ID="rfvAmount"
                                    runat="server"
                                    ControlToValidate="txtAmount"
                                    ErrorMessage="Amount is required"
                                    ValidationGroup="vg1"
                                    CssClass="text-danger" />
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="form-group">
                                <label>Type <span class="text-danger">*</span></label>
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="-- Select Type --" Value="" />
                                    <asp:ListItem Text="Credit" Value="1" />
                                    <asp:ListItem Text="Debit" Value="2" />
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator
                                    ID="rfvStatus"
                                    runat="server"
                                    ControlToValidate="ddlStatus"
                                    InitialValue=""
                                    ErrorMessage="Type is required"
                                    ValidationGroup="vg1"
                                    CssClass="text-danger" />
                            </div>
                        </div>



                        <div class="col-md-4">
                            <div class="form-group">
                                <label>
                                    Entry Date <span class="text-danger">*</span>
                                </label>

                                <div class="input-group">
                                    <span class="input-group-addon">
                                        <i class="icon-calendar22"></i>
                                    </span>

                                    <asp:TextBox ID="txt_date"
                                        runat="server"
                                        CssClass="form-control pickadate"
                                        placeholder="DD/MM/YYYY"
                                        autocomplete="off">
                                    </asp:TextBox>
                                </div>

                                <!-- REQUIRED VALIDATION -->
                                <asp:RequiredFieldValidator
                                    ID="rfvDate"
                                    runat="server"
                                    ControlToValidate="txt_date"
                                    ErrorMessage="Entry Date is required"
                                    ValidationGroup="vg1"
                                    CssClass="text-danger"
                                    Display="Dynamic" />
                            </div>
                        </div>
                        <div class="col-md-6" id="divAttachment" style="display:none;">
                            <div class="form-group">
                                <label>Attachment <small class="text-muted">(PDF, JPG, PNG — optional)</small></label>
                                <input type="file" name="fuAttachment"
                                    class="form-control" accept=".pdf,.jpg,.jpeg,.png,.gif,.webp" />
                                <asp:HiddenField ID="hfExistingFile" runat="server" />
                                <asp:Literal ID="lblExistingFile" runat="server" Visible="false"></asp:Literal>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Description <span class="text-danger">*</span></label>
                                <asp:TextBox ID="txtDescription" runat="server"
                                    CssClass="form-control"
                                    TextMode="MultiLine"
                                    Rows="1"
                                    placeholder="Enter description" />
                                <asp:RequiredFieldValidator
                                    ID="rfvDescription"
                                    runat="server"
                                    ControlToValidate="txtDescription"
                                    ErrorMessage="Description is required"
                                    ValidationGroup="vg1"
                                    CssClass="text-danger" />
                            </div>
                        </div>

                    </div>
                    <div class="row  pull-right">
                        <div class="col-lg-12 pull-right">
                            <a href="PettyCash.aspx" class="btn btn-primary">Back</a>
                            <asp:Button ID="btnSave" runat="server"
                                Text="Submit"
                                CssClass="btn btn-primary"
                                OnClick="btnSave_Click" ValidationGroup="vg1" />
                            <asp:Button ID="btnUpdate" runat="server"
                                Text="Update"
                                CssClass="btn btn-primary"
                                OnClick="btnUpdate_Click"
                                Visible="false" ValidationGroup="vg1" />
                        </div>

                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-2"></div>
    </div>
    <script>
        $(document).ready(function () {
            var $input = $('#<%= txt_date.ClientID %>');
            var existingVal = $input.val();

            // normalize any '-' or '.' separators (e.g. browser autofill) to '/'
            if (existingVal) {
                existingVal = existingVal.replace(/[-.]/g, '/');
                $input.val(existingVal);
            }

            var picker = $input.pickadate({
                format: 'dd/mm/yyyy',
                selectMonths: true,
                selectYears: true,
                closeOnSelect: true
            }).pickadate('picker');

            // if edit mode — set existing date into picker
            if (existingVal) {
                var parts = existingVal.split('/');
                if (parts.length === 3) {
                    picker.set('select', [parseInt(parts[2]), parseInt(parts[1]) - 1, parseInt(parts[0])]);
                }
            }

            // sync picker value back to textbox on every set so PostBack gets it
            picker.on('set', function (ctx) {
                if (ctx.select !== undefined) {
                    var d = new Date(ctx.select);
                    var formatted = ('0' + d.getDate()).slice(-2) + '/' +
                                    ('0' + (d.getMonth() + 1)).slice(-2) + '/' +
                                    d.getFullYear();
                    $input.val(formatted);
                }
            });
        });

        $('#<%= ddlStatus.ClientID %>').on('change', function () {
            if ($(this).val() === '2') {
                $('#divAttachment').show();
            } else {
                // hide only if no existing file
                if ($('#<%= hfExistingFile.ClientID %>').val() === '') {
                    $('#divAttachment').hide();
                }
            }
        }).trigger('change');
    </script>

</asp:Content>
