<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="DocumentSend.aspx.cs" Inherits="WEB_Admin_DocumentSend" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/uploaders/fileinput/fileinput.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/uploader_bootstrap.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/styling/uniform.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/styling/switchery.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/styling/switch.min.js"></script>
    <!-- Theme JS files -->
    <script type="text/javascript" src="../Template/assets/js/plugins/ui/moment/moment.min.js"></script>
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
                            <legend class="text-semibold"><i class="icon-reading position-left"></i> Create Document </legend>
                         </fieldset>

                        <div class="form-group">
                            <div>
                                <label>Employee Id </label>
                                <asp:DropDownList ID="ddl_id" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_id_SelectedIndexChanged" class="form-control">
                                </asp:DropDownList>

                            </div>
                        </div>
                        <div class="form-group">
                            <div>
                                <label>User Name </label>

                                <asp:TextBox ID="txt_user" runat="server" class="form-control">
                                </asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div>
                                <label>Document Name</label>
                                <asp:textbox ID="txt_letter" runat="server" class="form-control">
                                </asp:textbox>
                            </div>
                        </div>

                        <div class="form-group">
                            <label>Documents </label>
                            <asp:FileUpload ID="up_document" runat="server" class="file-input" multiple="multiple" data-show-upload="false" data-show-caption="true" data-show-preview="true" />
                           

                        </div>


                        <div class="form-group">
                            <div class="text-right">
                                 <a href="Documents.aspx" class="btn btn-primary" style="margin-right:15px">Back</a>
                                <asp:Button ID="btn_send" runat="server" Text="Send" class="btn btn-primary" OnClick="btn_send_Click" style="margin-right:15px"></asp:Button>
                               
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-3"></div>
    </div>
</asp:Content>

