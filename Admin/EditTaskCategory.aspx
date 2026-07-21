<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="EditTaskCategory.aspx.cs" Inherits="Admin_EditTaskCategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <div class="row">
        <div class="col-md-3"></div>
        <div class="col-md-6">
            <div class="panel panel-flat">
                <div class="panel-heading">
                </div>

                <div class="panel-body">


                    <div class="form-group">
                        <label>Category Name:</label>
                        
                              <asp:textbox ID="txt_category" runat="server"  class="form-control">
                                </asp:textbox>
                    </div>
                    <div class="form-group">

                        <label>Description:</label>
                        <textarea id="txt_description" runat="server" rows="5" cols="5" class="form-control" required="" ></textarea>
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
                             <a href="TaskCategoryView.aspx" class="btn btn-primary" > Back</a>
                        <asp:LinkButton ID="btn_update" class="btn btn-primary" runat="server" OnClick="btn_update_Click">Update</asp:LinkButton>
                           </div> </div>

                    

            </div>

            <div class="col-md-3"></div>
        </div>
    </div>
</asp:Content>

