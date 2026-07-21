<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="CreateSubMenu.aspx.cs" Inherits="Admin_CreateSubMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .icon-pencil4 {
            margin-right: 5px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row">
        <div class="col-md-2"></div>
        <div class="col-md-8">
            <div class="panel panel-flat">
                <div class="panel-heading">
                    <h5 class="panel-title"></h5>
                </div>

                <div class="panel-body">
                    <fieldset>
                        <legend class="text-semibold"><i class="icon-pencil4"></i> Sub Menu</legend>
                        
                        <div class="row">
                            <div class="col-md-4">
                                <label class="content-group text-semibold">Menu Name <span style="color: red">*</span></label>
                                <asp:TextBox ID="txt_menuname" runat="server" CssClass="form-control form-control-sm" placeholder="Enter menu name"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txt_menuname" runat="server" ErrorMessage="Menu name is required" ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>

                            <div class="col-md-4">
                                <label class="content-group text-semibold">Page Name</label>
                                <asp:TextBox ID="txt_pagename" runat="server" CssClass="form-control form-control-sm" placeholder="e.g., Dashboard.aspx"></asp:TextBox>
                            </div>

                            <div class="col-md-4">
                                <label class="content-group text-semibold">Menu List No <span style="color: red">*</span></label>
                                <asp:TextBox ID="txt_menulistno" runat="server" CssClass="form-control form-control-sm" placeholder="Enter menu list number"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txt_menulistno" runat="server" ErrorMessage="Menu list no is required" ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-4">
                                <label class="content-group text-semibold">Module Name <span style="color: red">*</span></label>
                                <asp:DropDownList ID="ddl_module" runat="server" CssClass="form-control form-select-sm" AutoPostBack="true" OnSelectedIndexChanged="ddl_module_SelectedIndexChanged"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="ddl_module" InitialValue="0" runat="server" ErrorMessage="Please select module" ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>

                            <div class="col-md-4">
                                <label class="content-group text-semibold">Parent Menu Name <span style="color: red">*</span></label>
                                <asp:DropDownList ID="ddl_parentmenu" runat="server" CssClass="form-control form-select-sm"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="ddl_parentmenu" InitialValue="0" runat="server" ErrorMessage="Please select parent menu" ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>

                            <div class="col-md-4">
                                <label class="content-group text-semibold">Folder Name</label>
                                <asp:TextBox ID="txt_foldername" runat="server" CssClass="form-control form-control-sm" placeholder="e.g., Admin"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-4">
                                <label class="content-group text-semibold">Menu Icon</label>
                                <asp:TextBox ID="txt_menuicon" runat="server" CssClass="form-control form-control-sm" placeholder="e.g., fas fa-home"></asp:TextBox>
                                <small><a href="Menuicons.aspx" target="_blank">View icon list</a></small>
                            </div>

                           

                              <div class="col-md-4">
      <label class="content-group text-semibold">Menu Description</label>
      <textarea id="txt_menudesc" runat="server" rows="3" class="form-control form-control-sm" placeholder="Enter menu description"></textarea>
  </div>
                                                    <div class="col-md-4">
    <label class="content-group text-semibold">Status <span style="color: red">*</span></label>
    <asp:RadioButtonList ID="rblStatus" runat="server" RepeatDirection="Horizontal">
        <asp:ListItem Text="&nbsp;Active&nbsp;&nbsp;" Value="1" Selected="True"></asp:ListItem>
        <asp:ListItem Text="&nbsp;Inactive" Value="0"></asp:ListItem>
    </asp:RadioButtonList>
</div>
                        </div>
                        

                      
                        <div class="form-group mt-10">
                            <div class="text-right">
                                <a href="SubMenus.aspx" class="btn btn-primary btn-sm">Back</a>
                                <asp:Button ID="btn_submit" runat="server" Text="Submit" OnClick="btn_submit_Click" CssClass="btn btn-primary btn-sm"></asp:Button>
                            </div>
                        </div>
                    </fieldset>
                </div>
            </div>
        </div>
        <div class="col-md-2"></div>
    </div>
</asp:Content>
