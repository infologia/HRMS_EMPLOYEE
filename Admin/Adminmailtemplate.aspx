<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Adminmailtemplate.aspx.cs" Inherits="WEB_Admin_Adminmailtemplate" %>

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
                    <div action="#">


                        <div class="form-group">

                            <label>Header </label>
                            <asp:TextBox ID="txt_header" runat="server" class="form-control daterange-single" required=""></asp:TextBox>
                        </div>

                        <div class="form-group">
                            <label>Subject </label>

                            <asp:TextBox ID="txt_subject" runat="server" class="form-control daterange-single" required=""></asp:TextBox>
                        </div>
                        
                        <div class="form-group">

                            <label>Content </label>
                            <asp:TextBox ID="txt_content" runat="server" class="form-control daterange-single" required=""></asp:TextBox>
                        </div>
                        <br />
                         <div class="form-group">
                                                    <label>Upload Image</label>
                                                  
                                                          <asp:FileUpload ID="up_image" runat="server" CssClass="form-control"  />
                       </div>
                               <div class="form-group">
                            <label>Footer</label>

                            <asp:TextBox ID="txt_footer" runat="server" class="form-control daterange-single" required=""></asp:TextBox>
                        </div>


                        <div class="text-right">
                            <asp:Button ID="btn_sendto" runat="server" Text="Send to" class="btn btn-primary" OnClick="btn_sendto_Click" ></asp:Button>
                              <asp:Button ID="btn_preview" runat="server" Text="Preview" class="btn btn-primary" onclick="btn_preview_Click"></asp:Button>
                        </div>
                             <br />
                        <div class="text-right">
                          
                        </div>
                  

                </div>
            </div>
        </div>
        </div>
        <div class="col-md-3"></div>

    </div>


</asp:Content>

