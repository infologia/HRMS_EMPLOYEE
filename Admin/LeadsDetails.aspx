<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="LeadsDetails.aspx.cs" Inherits="Admin_LeadsDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .form-control {
            position: relative !important;
            z-index: 10 !important;
            pointer-events: auto !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form-horizontal">
        <!-- Basic layout-->
        <div class="panel panel-flat">
            <div class="panel-heading">
                <h5 class="panel-title"></h5>
            </div>
            <div class="panel-body">
                <legend class="text-semibold"><i class="icon-reading position-left"></i>Create Lead</legend>
                <div class="row">
                    <div class="col-md-4">
                        <label>Name<span style="color: red"> *</span></label>
                        <asp:TextBox ID="txt_name" runat="server" Class="form-control" placeholder="Enter Name"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator_name" runat="server" ControlToValidate="txt_name" ErrorMessage="Name is a required field." ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-4">
                        <label>Company<span style="color: red"> *</span></label>
                        <asp:TextBox ID="txt_company" runat="server" CssClass="form-control" placeholder="Enter Company Name"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator_txt_company" runat="server" ControlToValidate="txt_company" ErrorMessage="Company is a required field." ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-4">
                        <label>Position</label>
                        <asp:TextBox ID="txt_position" runat="server" CssClass="form-control" placeholder="Enter Position"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <label>Email</label>
                        <asp:TextBox ID="txt_email" runat="server" CssClass="form-control" placeholder="Enter Email"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txt_email" ErrorMessage="Enter a Valid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red"></asp:RegularExpressionValidator>
                    </div>
                    <div class="col-md-4">
                        <label>Mobile</label>
                        <asp:TextBox ID="txt_mobile" runat="server" CssClass="form-control" placeholder="Enter Mobile"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Style="color: red"
                            ControlToValidate="txt_mobile" ErrorMessage="Enter a Valid Phone number"
                            ValidationExpression="[0-9]{10}"></asp:RegularExpressionValidator>
                    </div>
                    <div class="col-md-4">
                        <label>Source</label>
                        <asp:TextBox ID="txt_source" runat="server" CssClass="form-control" placeholder="Lead Source"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <label>Lead Type <span style="color: red">*</span></label>
                        <asp:DropDownList ID="ddl_leadtype" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator_ddl_leadtype" ControlToValidate="ddl_leadtype" runat="server" ErrorMessage="Please Select Lead type." ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-4">
                        <label>Status <span style="color: red">*</span></label>
                        <asp:DropDownList ID="ddl_status" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator_ddl_status" ControlToValidate="ddl_status" runat="server" ErrorMessage="Please Select Status." ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-4">
                        <label>Description</label>
                        <asp:TextBox ID="txt_description" TextMode="MultiLine" Rows="3" runat="server" CssClass="form-control" placeholder="Enter Description"></asp:TextBox>
                    </div>
                </div>
                <div class="row" style="margin-top: 25px;">
                    <div class="col-lg-12 text-right-md">
                        <a href="SalesLeads.aspx" class="btn btn-primary">Back</a>
                        <asp:Button ID="btn_request" runat="server" Text="Create" CssClass="btn btn-primary" OnClick="btn_request_Click" Visible="false"></asp:Button>
                        <asp:Button ID="btn_update" runat="server" Text="Update" CssClass="btn btn-primary" OnClick="btn_update_Click" Visible="false"></asp:Button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

