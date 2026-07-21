<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="AssignassetsCreation.aspx.cs" Inherits="Admin_AssignassetsCreation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form-horizontal">
        <div class="panel panel-flat">
            <div class="panel-heading">
                <h5 class="panel-title"></h5>
            </div>

            <div class="panel-body">
                <legend class="text-semibold" id="create" runat="server"></legend>

                <!-- Row 1 -->
                <div class="row">
                    <div class="col-md-4">
                        <label>Employee Name <span style="color: red">*</span></label>
                        <asp:DropDownList ID="ddl_employee" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator
                            ID="RequiredFieldValidator_ddl_employee"
                            runat="server"
                            ControlToValidate="ddl_employee"
                            ErrorMessage="Please Select Employee Name"
                            ForeColor="Red" />
                    </div>
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
                        <asp:DropDownList ID="ddl_assecategory" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl_assecategory_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator
                            ID="RequiredFieldValidator1"
                            runat="server"
                            ControlToValidate="ddl_assecategory"
                            ErrorMessage="Please Select Asset Tag"
                            ForeColor="Red" />
                    </div>

                </div>
                <!-- Row 2 -->
                <div class="row">
                    <div class="col-md-4">
                        <label>Equipment Name <span style="color: red">*</span></label>
                        <asp:DropDownList ID="ddl_equipment" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl_equipment_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator
                            ID="RequiredFieldValidator4"
                            runat="server"
                            ControlToValidate="ddl_equipment"
                            ErrorMessage="Please Select Equipment Name"
                            ForeColor="Red" />
                    </div>
                    <div class="col-md-4">
                        <label>Brand <span style="color: red">*</span></label>
                        <asp:DropDownList ID="ddl_brand" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl_brand_SelectedIndexChanged">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator
                            ID="RequiredFieldValidator2"
                            runat="server"
                            ControlToValidate="ddl_brand"
                            ErrorMessage="Please Select Brand Name"
                            ForeColor="Red" />
                    </div>
                    <div class="col-md-4">
                        <label>Model Serial Number <span style="color: red">*</span></label>
                        <asp:DropDownList ID="ddl_modal" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator
                            ID="RequiredFieldValidator5"
                            runat="server"
                            ControlToValidate="ddl_modal"
                            ErrorMessage="Please Select Model Serial Number"
                            ForeColor="Red" />

                    </div>

                </div>
                <div class="row">
                    <div class="col-md-4">
                        <label>Assigned Date <span style="color: red">*</span></label>
                         <div class="input-group">
                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                            <asp:TextBox ID="txt_assigneddate" runat="server" 
                                 CssClass="form-control pickadate-start"
                                 placeholder="Enter Assigned Date"></asp:TextBox>
                          </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt_assigneddate" ErrorMessage="Assigned Date is a required" ForeColor="Red"></asp:RequiredFieldValidator>
                       
                    </div>
                    <div class="col-md-4">
                        <label>Returned Date</label>
                        <div class="input-group">
                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                        <asp:TextBox ID="txt_returneddate" runat="server" 
                            CssClass="form-control pickadate-start"
                            placeholder="Enter Returned Date"></asp:TextBox>
                         </div>
                    </div>

                    <div class="col-md-4">
                        <label>Description</label>
                        <asp:TextBox ID="txt_description" runat="server"
                            TextMode="MultiLine" Rows="1"
                            CssClass="form-control"
                            placeholder="Enter Description"></asp:TextBox>

                    </div>

                </div>
                <div class="row">
                    <div class="col-md-4">
                        <label>Status</label><br />
                        <asp:RadioButtonList ID="rd_Status" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="&nbspActive&nbsp&nbsp&nbsp" Selected="True" Value="1"></asp:ListItem>
                            <asp:ListItem Text="&nbspInActive&nbsp" Value="2"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>

                <!-- Buttons -->
                <div class="row" style="margin-top: 25px;">
                    <div class="col-md-12 text-right-md">
                        <a href="Assignassets.aspx" class="btn btn-primary">Back</a>
                        <asp:Button ID="submit" runat="server"
                            CssClass="btn btn-primary"
                            OnClick="btn_Create_Click" />
                    </div>
                </div>

            </div>
        </div>
    </div>
    <script>
        var today = new Date();

        $('.pickadate-start').pickadate({
            format: 'dd/mm/yyyy',
            selectMonths: true,
            selectYears: true,
            closeOnSelect: true
        });
</script>
</asp:Content>

