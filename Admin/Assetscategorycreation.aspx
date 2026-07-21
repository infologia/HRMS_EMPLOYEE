<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Assetscategorycreation.aspx.cs" Inherits="Admin_Assetscategorycreation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">

        <div class="col-md-4"></div>
        <div class="col-md-6 col-md-offset-3">

            <!-- Vertical form -->
            <div class="panel panel-flat">
                <%--<div class="panel-heading">
                    <h5 class="panel-title"></h5>
                </div>--%>

                <div class="panel-body">
                    <fieldset>
                        <legend class="text-semibold" id="create" runat="server"></legend>
                        <div action="#">
                            <div class="form-group">
                                <label>Asset Type <span style="color: red">*</span></label>
                                <asp:DropDownList ID="ddl_assettype" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator
                                    ID="RequiredFieldValidator_ddl_assettype"
                                    runat="server"
                                    ControlToValidate="ddl_assettype"
                                    ErrorMessage="Please Select Asset Type"
                                    ForeColor="Red" />
                            </div>
                            <div class="form-group">
                                <label>Category <span style="color: red">*</span></label>
                                <asp:TextBox ID="txt_Category" runat="server" CssClass="form-control" placeholder="Enter Category"></asp:TextBox>
                                  <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt_Category" ErrorMessage="Category is a required" ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>

                            
                            <div class="form-group">
                                <label>Status</label><br />
                                <asp:RadioButtonList ID="rd_Status" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="&nbspActive&nbsp&nbsp&nbsp" Selected="True" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="&nbspInActive&nbsp" Value="2"></asp:ListItem>
                                </asp:RadioButtonList>
                            </div>
                            <div class="form-group">
                                <div class="text-right">
                                    <a href="Assetscategory.aspx" class="btn btn-primary" style="margin-right: 15px">Back</a>
                                    <asp:Button ID="submit" runat="server" OnClick="btn_Create_Click" class="btn btn-primary" style="margin-right: 15px"></asp:Button>
                                </div>

                            </div>
                        </div>
                    </fieldset>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

