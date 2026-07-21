<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="ChangePassword.aspx.cs" Inherits="WEB_ChangePassword" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="row">
        <div class="col-lg-4"></div>

        <div class="col-lg-4">
            <div class="page-container">
                <div class="page-content">
                    <div class="content-wrapper">
                        <div class="panel panel-body login-form">

                            <div class="text-center">
                                <div class="icon-object border-slate-300 text-slate-300"><i class="icon-user"></i></div>
                                <h5 class="content-group">Change Password <small class="display-block">Your credentials</small></h5>
                            </div>

                            <div class="form-group has-feedback has-feedback-left">
                                <asp:TextBox ID="txt_password" runat="server" TextMode="Password" placeholder="New Password" CssClass="form-control" required></asp:TextBox>
                                <div class="form-control-feedback">
                                    <i class="icon-user text-muted"></i>
                                </div>
                            </div>
                            <div class="form-group has-feedback has-feedback-left">
                                <asp:TextBox ID="txt_cnfrmPassword" runat="server" TextMode="Password" placeholder="Confirm New Password" CssClass="form-control" required></asp:TextBox>
                                <asp:CompareValidator ID="CompareValidator1" runat="server"
                                    ControlToValidate="txt_cnfrmPassword"
                                    CssClass="ValidationError"
                                    ControlToCompare="txt_password"
                                    ErrorMessage="Password must be the same"
                                    ForeColor="Red"/>
                                <div class="form-control-feedback">
                                    <i class="icon-user text-muted"></i>
                                </div>

                                
                            </div>
                          
                            <div class="form-group">
                                <asp:Button ID="btn_Submit" runat="server" CssClass="btn btn-primary btn-block" Text="Submit" OnClick="btn_Submit_Click" />
                            </div>
 <div id="div_error" runat="server" class="alert alert-success" visible="false">
                                 <button type="button" class="close" data-dismiss="alert"><span>&times;</span><span class="sr-only">Close</span></button>
							
                                <asp:Label ID="lbl_error" runat="server"></asp:Label>
                            </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

