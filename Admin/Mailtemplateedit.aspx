<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Mailtemplateedit.aspx.cs" Inherits="WEB_Admin_Mailtemplateedit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
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
                                                  
                                                          <asp:FileUpload ID="up_image" runat="server" CssClass="form-control" />
                       </div>
                                                   <asp:Image ID="Img_1"  runat="server" class="img-responsive img-thumbnail pull-right" Style="height: 100px" />
                                       </div>     
                        <br /> <br />  <br /> 
                            
                                                         
                               <div class="form-group">
                              <label>Footer</label>

                            <asp:TextBox ID="txt_footer" runat="server" class="form-control daterange-single" required=""></asp:TextBox>
                        </div>


                       
                           <div class="text-center">
                             <a href="MailTemplate.aspx" class="btn btn-primary" >Back</a>
                              <asp:Button ID="btn_edit" runat="server" Text="Update" class="btn btn-primary" OnClick="btn_edit_Click" ></asp:Button>
                        </div>   

                 
                        </div>
                             <br />
                      
                  

                </div>
            
        </div>
        
        <div class="col-md-3"></div>

    </div>

</asp:Content>

