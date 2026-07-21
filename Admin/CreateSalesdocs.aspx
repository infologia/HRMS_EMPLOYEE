<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="CreateSalesdocs.aspx.cs" Inherits="Admin_CreateSalesdocs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

  



    <div class="row">
        <div class="col-md-3"></div>

        <div class="col-md-6">
            <div class="panel panel-flat" style="padding: 30px; border: 1px solid #ddd; border-radius: 6px;">
                <div class="panel-body">

                    <div class="form-group">
                        <label>File Title <span style="color: red">*</span></label>
                        <asp:TextBox ID="txt_filetitle" runat="server" class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_filetitle" ErrorMessage="Please Enter File Title" ForeColor="Red" />
                    </div>

                    <div class="form-group">
                        <label>File Name <span style="color: red">*</span></label>
                        <asp:FileUpload ID="fu_file" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="fu_file" ErrorMessage="Please Select File" ForeColor="Red" />
                    </div>

                    <div class="form-group">
                        <label>Description <span style="color: red">*</span></label>
                        <textarea id="txt_des" rows="5" cols="5" runat="server" class="form-control"></textarea>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txt_des" ErrorMessage="Please Enter Description" ForeColor="Red" />
                    </div>

                    <div class="form-group text-right">
                        <asp:Button
                            ID="Button1"
                            runat="server"
                            Text="Back"
                            CssClass="btn btn-primary"
                            CausesValidation="false"
                            PostBackUrl="~/Admin/Salesdocs.aspx"
                            Style="margin-right: 10px;" />

                        <asp:Button
                            ID="Button2"
                            runat="server"
                            Text="Create"
                            CssClass="btn btn-primary"
                            OnClick="btn_Create_Click" />
                    </div>

                </div>
            </div>
        </div>
    </div>



</asp:Content>
