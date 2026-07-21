<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="CreateProject.aspx.cs" Inherits="TicketingTool_CreateProject" %>

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
                            </div>
                        </div>
                        
                       
                        <div class="form-group">
                          <label>Project Description</label>  
					<textarea id="txt_des" runat="server" rows="4" cols="4"  class="form-control" required="required" ></textarea>

                        </div>


                        <div class="form-group">
                            <div class="text-center">

                                <a href="Projects.aspx" class="btn bg-teal-400 margin-left-1"><i class="icon-undo" ></i> Back</a>
                                <asp:Button ID="btn_send" runat="server" Text="Create" class="btn btn bg-teal-400" OnClick="btn_send_Click" Visible="false"></asp:Button>
                                <asp:Button ID="Btn_update" runat="server" Text="Update" CssClass="btn btn bg-teal-400" OnClick="Btn_update_Click" Visible="false" />
                             </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-3"></div>
    </div> 





</asp:Content>

