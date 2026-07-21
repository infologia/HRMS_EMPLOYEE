<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Menuroleupdate.aspx.cs" Inherits="Admin_Menuroleupdate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h6 class="content-group text-semibold">Menu Role					
        <small class="display-block">Created menu role here</small>
    </h6>
    <div class="row">
        <div class="col-md-4">
        </div>
        <div class="col-md-4">
            <div class="panel panel-flat">
                <div class="panel-heading">
                        <fieldset>
                            <legend class="text-semibold"><i class="icon-reading position-left"></i>Create menu role</legend>
                         </fieldset>

                    <div class="heading-elements">
                      
                    </div>
                </div>
                <div class="panel-body">
                    <div class="form-group">
                        <label>Menu role name</label>
                        <asp:TextBox ID="txt_menuname" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>

                    <div class="text-center">
                         <a href="Menurole.aspx" class="btn btn-primary"> Back</a>
                        <asp:Button ID="btn_submit" runat="server" Text="submit" CssClass="btn btn-primary" OnClick="btn_submit_Click" />

                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-4">
        </div>
    </div>
</asp:Content>


