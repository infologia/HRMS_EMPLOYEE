<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="AssetInventory.aspx.cs" Inherits="Admin_AssetInventory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .form-control {
            position: relative !important;
            z-index: 10 !important;
            pointer-events: auto !important;
        }
    </style>

    <style>
    @media (max-width: 767px) {
        .form-horizontal .row > [class*="col-"] {
            margin-bottom: 15px;
        }
    }
</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form-horizontal">
        <!-- Basic layout-->
        <div class="panel panel-flat">
           <%-- <div class="panel-heading">
                <h5 class="panel-title"></h5>
            </div>--%>
            <div class="panel-body">
                <legend class="text-semibold"><i class="icon-reading position-left"></i>
                    <asp:Label ID="create" runat="server"></asp:Label></legend>
                <div class="row">
                    <div class="col-md-4">
                        <label>Asset Type <span style="color: red">*</span></label>
                        <asp:DropDownList ID="ddl_assettype" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl_assettype_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator
                            ID="RequiredFieldValidator_ddl_assettype"
                            runat="server"
                            ControlToValidate="ddl_assettype"
                            ErrorMessage="Please Select Asset Type"
                            ForeColor="Red" />
                    </div>

                    <div class="col-md-4">
                        <label>Asset Category <span style="color: red">*</span></label>
                        <asp:DropDownList ID="ddl_assetcategory" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="ddl_assetcategory" runat="server" ErrorMessage="Please Select Asset Category." ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-4">
                        <label>Asset Tag <span style="color: red">*</span></label>
                        <asp:TextBox ID="txt_assettag" runat="server" Class="form-control" placeholder="Enter Asset Tag"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator_name" runat="server" ControlToValidate="txt_assettag" ErrorMessage="AssetTag is a required field." ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>

                </div>
                <div class="row">
                    <%-- <div class="col-md-4">
                        <label>Quantity <span style="color: red">*</span></label>
                        <asp:TextBox ID="txt_quantity" runat="server" CssClass="form-control" placeholder="Enter Quantity"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_quantity" ErrorMessage="Quantity is a required field." ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>--%>
                    <div class="col-md-4">
                        <label>Equipment Name <span style="color: red">*</span></label>
                        <asp:TextBox ID="txt_equipmentname" runat="server" CssClass="form-control" placeholder="Enter Equipment Name"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator_txt_company" runat="server" ControlToValidate="txt_equipmentname" ErrorMessage="Equipment Name is a required field." ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>

                    <div class="col-md-4">
                        <label>Brand <span style="color: red">*</span></label>
                        <asp:TextBox ID="txt_brand" runat="server" CssClass="form-control" placeholder="Enter Brand"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt_brand" ErrorMessage="Brand is a required field." ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-4">
                        <label>Model Serial Number <span style="color: red">*</span></label>
                        <asp:TextBox ID="txt_modelserialno" runat="server" CssClass="form-control" placeholder="Enter Model Serial Number"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_modelserialno" ErrorMessage="Model Serial Number is a required field." ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>


                </div>
                <div class="row">
                    <div class="col-md-4">
                        <label>Placed Location <span style="color: red">*</span></label>
                        <asp:TextBox ID="txt_placedlocation" runat="server" CssClass="form-control" placeholder="Enter Placed Location"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txt_placedlocation" ErrorMessage="Placed Location is a required field." ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-4">
                        <label>Asset Condition <span style="color: red">*</span></label>
                        <asp:TextBox ID="txt_assetcondition" runat="server" CssClass="form-control" placeholder="Enter Asset Condition"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txt_assetcondition" ErrorMessage="Asset Condition is a required field." ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-4">
                        <label>Purchase Cost <span style="color: red">*</span></label>
                        <asp:TextBox ID="txt_purchasedcost" runat="server" CssClass="form-control" placeholder="Enter Purchase Cost"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txt_purchasedcost" ErrorMessage="Purchase Cost is a required field." ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>


                </div>
                <div class="row">
                    <div class="col-md-4">
                        <label>Purchase Date <span style="color: red">*</span></label>
                        <div class="input-group">
                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                            <asp:TextBox ID="txt_purchaseddate" runat="server" CssClass="form-control pickadate" placeholder="Enter Purchase Date"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txt_purchaseddate" ErrorMessage="Purchase Date is a required field." ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-4">
                        <label>Invoice</label>
                        <asp:FileUpload ID="fu_file" runat="server" CssClass="form-control" />
                        <!-- View link -->
                        <asp:HyperLink ID="lnkViewFile" runat="server" Text="View File" Target="_blank" Visible="false" CssClass="btn btn-link p-0 mt-1"> </asp:HyperLink>
                    </div>
                    <div class="col-md-4">
                        <label>Warranty</label>
                        <asp:TextBox ID="txt_amcdetails" runat="server" CssClass="form-control" placeholder="Enter AMC Details"></asp:TextBox>
                        <%--                        <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txt_amcdetails" ErrorMessage="AMC Details is a required field." ForeColor="Red"></asp:RequiredFieldValidator>--%>
                    </div>




                </div>
                <div class="row">
                    <div class="col-md-4" id="div_status" runat="server" visible="false">
                        <label>Status <span style="color: red">*</span></label>
                        <asp:RadioButtonList ID="rd_Status" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="&nbspActive&nbsp&nbsp&nbsp" Selected="True" Value="1"></asp:ListItem>
                            <asp:ListItem Text="&nbspInActive&nbsp" Value="2"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>


                <div class="row" style="margin-top: 25px;">
                    <div class="text-right">
                        <a href="AssetInventoryView.aspx" class="btn btn-primary" style="margin-right:15px">Back</a>
                        <asp:Button ID="btn_Submit" runat="server" CssClass="btn btn-primary" OnClick="btn_Submit_Click" style="margin-right:15px"></asp:Button>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <script>
        $('.pickadate').pickadate({ format: 'dd/mm/yyyy' });
    </script>
</asp:Content>

