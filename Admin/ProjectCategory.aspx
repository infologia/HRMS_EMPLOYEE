<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="ProjectCategory.aspx.cs" Inherits="Admin_ProjectCategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="row">
        <div class="col-md-3"></div>
        <div class="col-md-6">
            <div class="panel panel-flat">
                <div class="panel-heading">
                </div>
                <div class="panel-body">
                    <div class="form-group">
                        <label>Category Name:</label>

                        <asp:TextBox ID="txt_category" runat="server" required="required" CssClass="form-control">
                        </asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Description:</label>
                        <textarea id="txt_description" runat="server" rows="5" cols="5" class="form-control" required=""></textarea>
                    </div>
                    <div class="form-group">
                        <label>Status</label>
                        <asp:RadioButtonList ID="Rd_Status" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="Rd_Status_SelectedIndexChanged" AutoPostBack="true" required="required">
                            <asp:ListItem Text="&nbspActive&nbsp&nbsp&nbsp" Selected="True" Value="1"></asp:ListItem>
                            <asp:ListItem Text="&nbspInactive&nbsp" Value="0"></asp:ListItem>
                        </asp:RadioButtonList>
                        <div id="txt_active" runat="server" visible="false"></div>
                    </div>
                </div>
                <div class="form-group">
                    <div class="text-center">
                        <a href="ProjectCategoryView.aspx" class="btn btn-primary margin-left-1">Back</a>
                        <asp:LinkButton ID="btn_send" class="btn btn-primary" runat="server" OnClick="btn_send_Click">Submit</asp:LinkButton>
                    </div>
                </div>
            </div>
            <div class="col-md-3"></div>
        </div>
    </div>
</asp:Content>

