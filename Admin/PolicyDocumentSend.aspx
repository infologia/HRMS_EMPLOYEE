<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="PolicyDocumentSend.aspx.cs" Inherits="Admin_PolicyDocumentSend" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/uploaders/fileinput/fileinput.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/uploader_bootstrap.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/styling/uniform.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/styling/switchery.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/styling/switch.min.js"></script>
    <!-- Theme JS files -->
    <script type="text/javascript" src="../Template/assets/js/plugins/ui/moment/moment.min.js"></script>
    <style>
          .status-radio input[type="radio"] {
        margin-right: 6px;  
    }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">

        <div class="col-md-3"></div>
        <div class="col-md-6">

            <!-- Vertical form -->
            <div class="panel panel-flat">
                <div class="panel-heading">
                    <h5 class="panel-title"></h5>
                </div>

                <div class="panel-body">
                    <div action="#">
                        <fieldset>
                            <legend class="text-semibold"><i class="icon-reading position-left"></i>
                                <asp:Label ID="create" runat="server"></asp:Label>
                            </legend>
                        </fieldset>

                        <div class="form-group">
                            <div>
                                <label>Document Name</label>
                                <asp:TextBox ID="txt_letter" runat="server" class="form-control">
                                </asp:TextBox>
                            </div>
                        </div>

                        <div class="form-group">
                            <label>Documents </label>
                            <asp:FileUpload ID="up_document" runat="server" class="file-input" multiple="multiple" data-show-upload="false" data-show-caption="true" data-show-preview="true" />
                            <!-- Old document view -->
                            <asp:Panel ID="pnlOldDoc" runat="server" Visible="false">
                                <br />
                                <asp:HyperLink ID="lnkViewDoc" runat="server"
                                    Text="View "
                                    Target="_blank"
                                    CssClass="primary" />
                            </asp:Panel>

                            <!-- store old file path -->
                            <asp:HiddenField ID="hdnOldDocument" runat="server" />

                        </div>
                        <div class="form-group">
                            <label>Status</label>
                            <asp:RadioButtonList ID="rblStatus" runat="server" RepeatDirection="Horizontal" CssClass="status-radio">
                                <asp:ListItem Text="Active" Value="1" style="margin-right:10px"></asp:ListItem>
                                <asp:ListItem Text="Inactive" Value="2"></asp:ListItem>
                            </asp:RadioButtonList>
                        </div>

                        <div class="form-group">
                            <div class="text-right">
                                <a href="PolicyDocument.aspx" class="btn btn-primary" style="margin-right: 15px">Back</a>
                                <asp:Button ID="btn_send" runat="server" class="btn btn-primary" OnClick="btn_send_Click" Style="margin-right: 15px"></asp:Button>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-3"></div>
    </div>
</asp:Content>

