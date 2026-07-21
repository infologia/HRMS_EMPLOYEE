<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Vendorsdetails.aspx.cs" Inherits="Admin_Vendorsdetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .form-control {
            position: relative !important;
            z-index: 10 !important;
            pointer-events: auto !important;
        }
        @media (max-width: 768px) {
    .form-control {
        margin-bottom: 12px;
    }
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
                <legend class="text-semibold"><i class="icon-reading position-left"></i>Create Vendor</legend>
                <div class="row">
                    <div class="col-md-4">
                        <label>Vendor Code<span style="color: red"> *</span></label>
                        <asp:TextBox ID="txt_VendorCode" runat="server" Class="form-control" placeholder="Enter Vendor Code"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator_name" runat="server" ControlToValidate="txt_VendorCode" ErrorMessage="Vendor Code is a required field." ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-4">
                        <label>Vendor Name<span style="color: red"> *</span></label>
                        <asp:TextBox ID="txt_VendorName" runat="server" Class="form-control" placeholder="Enter Vendor Name"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_VendorName" ErrorMessage="Vendor Name is a required field." ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-4">
                        <label>Contact Person</label>
                        <asp:TextBox ID="txt_ContactPerson" runat="server" Class="form-control" placeholder="Primary contact person name"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <label>Email</label>
                        <asp:TextBox ID="txt_email" runat="server" Class="form-control" placeholder="Enter Email"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txt_email" ErrorMessage="Enter a Valid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red"></asp:RegularExpressionValidator>
                    </div>
                    <div class="col-md-4">
                        <label>Mobile</label>
                        <asp:TextBox ID="txt_mobile" runat="server" Class="form-control" placeholder="Enter Mobile"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Style="color: red"
                            ControlToValidate="txt_mobile" ErrorMessage="Enter a Valid Phone number"
                            ValidationExpression="[0-9]{10}"></asp:RegularExpressionValidator>
                    </div>
                    <div class="col-md-4">
                        <label>GST Number </label>
                        <asp:TextBox ID="txt_gst" runat="server" CssClass="form-control" placeholder="Enter GST Number"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txt_gst"
                            ErrorMessage="Enter a valid GST number" ValidationExpression="^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$" ForeColor="Red"> </asp:RegularExpressionValidator>
                    </div>
                </div>

                <div class="row">
                    <div class="col-md-4">
                        <label>PAN Number</label>
                        <asp:TextBox ID="txt_pan" runat="server" Class="form-control" placeholder="Enter PAN Number"></asp:TextBox>
                    </div>
                    <div class="col-md-4">
                        <label>Address</label>
                        <asp:TextBox ID="txt_Address" runat="server" Class="form-control" placeholder="Enter primary address"></asp:TextBox>
                    </div>
                    <div class="col-md-4">
                        <label>Country</label>
                        <asp:DropDownList ID="ddl_Country" runat="server" Class="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-4">
                        <label>Bank Name</label>
                        <asp:TextBox ID="txt_bank" runat="server" Class="form-control" placeholder="Enter Bank name"></asp:TextBox>
                    </div>
                    <div class="col-md-4">
                        <label>Account Number</label>
                        <asp:TextBox ID="txt_Accountno" runat="server" Class="form-control" placeholder="Enter Account number"></asp:TextBox>
                    </div>
                    <div class="col-md-4">
                        <label>IFSC Code</label>
                        <asp:TextBox ID="txt_ifsc" runat="server" Class="form-control" placeholder="Enter IFSC Code"></asp:TextBox>
                    </div>

                </div>
                <br />

                <div class="row">
                    <div class="col-md-4">
                        <label>Payment Terms</label>
                        <asp:TextBox ID="txt_payment" runat="server" Class="form-control" placeholder="Enter payment Terms"></asp:TextBox>
                    </div>
                    <div class="col-md-4">
                        <label>Status</label>
                        <asp:DropDownList ID="ddl_Clientstatus" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    
                </div>
                <br />
                <div class="row">
                    <div class="col-md-8">
                        <label>Remarks </label>
                        <textarea id="txt_remarks" runat="server" class="form-control" placeholder="Additional notes"></textarea>
                    </div>

                </div>
                <div class="row" style="margin-top: 25px;">
                    <div class="col-lg-12 text-right">
                        <a href="Vendor.aspx" class="btn btn-primary">Back</a>
                        <asp:Button ID="btn_request" runat="server" Text="Create" Class="btn btn-primary" OnClick="btn_request_Click" Visible="false"></asp:Button>
                        <asp:Button ID="btn_update" runat="server" Text="Update" Class="btn btn-primary" OnClick="btn_update_Click" Visible="false"></asp:Button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

