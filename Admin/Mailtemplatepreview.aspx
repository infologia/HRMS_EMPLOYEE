<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Mailtemplatepreview.aspx.cs" Inherits="WEB_Admin_Mailtemplatepreview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <asp:PlaceHolder ID="PH_Preview" runat="server"></asp:PlaceHolder><br />

     <div class="form-group">
                            <asp:Button ID="btn_Submit" runat="server" CssClass="btn btn-primary btn-block" Text="Sendto" OnClick="btn_Submit_Click" style="    width: 100px;"/>

                            
                        </div>
</asp:Content>

