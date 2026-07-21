<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="CreateProject.aspx.cs" Inherits="TicketingTool_CreateProject" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">

        <div class="col-md-3"></div>
        <div class="col-md-6">

            <!-- Vertical form -->
            <div class="panel panel-flat">
                <div class="panel-heading">
                    <h5 class="panel-title"></h5>
                    <div class="heading-elements">
                    </div>
                </div>

                <div class="panel-body">
                    <div>


                        <div class="form-group">
                            <div>
                                <label>Project Name </label>
                                <asp:TextBox ID="txt_prjname" runat="server" Class="form-control" required="required"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <div>
                                <label>Project Type</label>
                                <asp:DropDownList ID="ddl_prjtype" runat="server" class="form-control" required="required">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddl_prjtype" InitialValue="0" runat="server" ErrorMessage="Please select Category" ForeColor="Red"></asp:RequiredFieldValidator>

                            </div>
                        </div>


                        <div class="form-group">
                            <label>Project Description</label>
                            <textarea id="txt_des" runat="server" rows="4" cols="4" class="form-control" required="required"></textarea>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txt_des" runat="server" ErrorMessage="Please select Category" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>


                        <div class="form-group" id="div_status" runat="server" visible="false">


                            <label>Status</label>
                            <div class="row">
                                <div class="col-lg-6">
                                    <asp:RadioButtonList ID="Rd_Status" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="Rd_Status_SelectedIndexChanged" AutoPostBack="true" required="required">
                                        <asp:ListItem Text="&nbspActive&nbsp&nbsp&nbsp" Selected="True" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="&nbspInactive&nbsp" Value="2"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div class="col-lg-3"></div>
                                <div class="col-lg-3">
                                    <asp:LinkButton ID="lk_remove" runat="server" OnClick="lk_remove_Click">Remove User</asp:LinkButton>
                                </div>
                            </div>
                        </div>


                        <div class="form-group">
                            <div class="text-center">

                                <a href="Projects.aspx" class="btn btn-primary">Back</a>
                                <asp:Button ID="btn_send" runat="server" Text="Create" class="btn btn-primary" OnClick="btn_send_Click" Visible="false"></asp:Button>
                                <asp:Button ID="Btn_update" runat="server" Text="Update" CssClass="btn btn-primary" OnClick="Btn_update_Click" Visible="false" />
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-3"></div>
    </div>





</asp:Content>

