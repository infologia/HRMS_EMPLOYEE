<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Mailtemplatesend.aspx.cs" Inherits="WEB_Admin_Mailtemplatesend" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    
    <script type="text/javascript">
        // for check all checkbox  
        function CheckAll(Checkbox) {
            var GridVwHeaderCheckbox = document.getElementById("<%=gvEmp.ClientID %>");
            for (i = 1; i < GridVwHeaderCheckbox.rows.length; i++) {
                GridVwHeaderCheckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }
        }
    </script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="row">
         
<%-- <div class="col-md-1"></div>
          <div class="col-md-10">--%>

   

       <div>
            <asp:GridView ID="gvEmp" AutoGenerateColumns="false" CellPadding="5" runat="server"  HeaderStyle-BackColor="Lavender" BorderColor="Red" style="width: 784px;"  HeaderStyle-ForeColor="IndianRed" CssClass="table table-bordered table-hover datatable-highlight"
                    AlternatingRowStyle-BackColor="DarkSeaGreen"
                    SelectedRowStyle-ForeColor="Blue">
                   
                    <AlternatingRowStyle BackColor="LightGoldenrodYellow" /> 
                <Columns >  
                    <asp:TemplateField>  
                        <HeaderTemplate>   
                            <asp:CheckBox ID="chkAllSelect" runat="server"  onclick="CheckAll(this);" />  
                        </HeaderTemplate>  
                        <ItemTemplate>  
                            <asp:CheckBox ID="chkSelect" runat="server" />  
                        </ItemTemplate>  

                    </asp:TemplateField>  
                    <asp:BoundField HeaderText="Employeeid" DataField="Employeeid" />  
                    <asp:BoundField HeaderText="UserName" DataField="UserName" />  
                    <asp:BoundField HeaderText="EmailId" DataField="Email" />  
                    <asp:BoundField HeaderText="Phonenumber" DataField="Phonenumber" /> 
                    
                </Columns>  
                <HeaderStyle  BackColor="#5d5d5d" Font-Bold="true" ForeColor="White"  />  
            </asp:GridView>   
                    <br /> <br /> 
        </div>  

               <asp:Button ID="btn_send" runat="server" Text="Send" class="btn btn-primary" OnClick="btn_send_Click" Style="margin-left: 270px;" ></asp:Button>
      <a href="MailTemplate.aspx" class="btn btn-primary" >Back</a>
<%--              </div>
         <div class="col-md-1"></div>--%>
     </div>
    
                 
   
                          
 
      

<div id="destination"></div>
                    
                 
</asp:Content>

