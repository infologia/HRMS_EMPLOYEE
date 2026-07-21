<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="UpdatePermission.aspx.cs" Inherits="WEB_Employee_UpdatePermission" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .icon-pencil4{
            margin-right: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
        <div class="row">
        <div class="col-md-2"></div>
        <div class="col-md-8">
            <div class="panel panel-flat">
                <div class="panel-heading">
                    <h5 class="panel-title"></h5>
                </div>

                <div class="panel-body">
                    <fieldset>
                        <legend class="text-semibold"><i class="icon-pencil4"></i> Permission Request</legend>
                       <div action="#">
                        <div class="row">
                            <div class="col-md-6">
                                <label class="content-group text-semibold">Request Date </label>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                    <asp:TextBox ID="txt_date" runat="server" CssClass="form-control pickadate"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txt_date" runat="server" ErrorMessage="Please select Date" ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>

                            <div class="col-md-6">
                                <label class="content-group text-semibold">From Time <span style="color: red">*</span></label>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="icon-alarm"></i></span>
                                    <asp:TextBox ID="txt_fromtime" runat="server" CssClass="form-control pickatime-clear"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="txt_fromtime" runat="server" ErrorMessage="Please select From time" ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                         
                        <div class="row">
                            <div class="col-md-6">
                                <label class="content-group text-semibold">To Time <span style="color: red">*</span></label>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="icon-alarm"></i></span>
                                    <asp:TextBox ID="txt_totime" runat="server" class="form-control pickatime-clear" required=""></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="txt_totime" runat="server" ErrorMessage="Please select To Time" ForeColor="Red"></asp:RequiredFieldValidator>

                            </div>

                            <div class="col-md-6">
                                <label class="content-group text-semibold">Reason<span style="color: red">*</span></label>
                                <textarea id="txt_reasons" runat="server" rows="1" class="form-control" required=""></textarea>
                                 <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="txt_reasons" runat="server" ErrorMessage="Enter a reason" ForeColor="Red"></asp:RequiredFieldValidator>
                           
                            </div>
                        </div>
                          
                        <div class="row">
                        <div id="div_Reson" runat="server" visible="false">
                            <div class="col-md-6">
                                <label class="content-group text-semibold">Status <span style="color: red">*</span></label>
                                <asp:DropDownList ID="ddl_category" runat="server" class="form-control">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddl_category" InitialValue="0" runat="server" ErrorMessage="Please select staus" ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-6">
                                <label class="content-group text-semibold">Admin Reason <span style="color: red">*</span></label>
                                <textarea id="txt_reason1" runat="server" rows="1" class="form-control"></textarea>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt_reason1" ErrorMessage="Admin Reason is a required" ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                            </div>
                        </div>

                  </div>  
                       <div class="form-group">
                    <div class="text-right">
                        <a href="PermissionRequestView.aspx" class="btn btn-primary" style="margin-right: 15px">Back</a>
                        <asp:Button ID="btn_request" runat="server" Text="Update" OnClick="btn_request_Click" class="btn btn-primary"></asp:Button>
                    </div>
                </div>
                    </fieldset>
                </div>
            </div>
        </div>
        <div class="col-md-2"></div>
    </div>

    <script>
        $('.pickadate').pickadate({ format: 'dd/mm/yyyy' });
    </script>
</asp:Content>

