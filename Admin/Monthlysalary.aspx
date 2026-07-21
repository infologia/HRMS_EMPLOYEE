<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Monthlysalary.aspx.cs" Inherits="Admin_Monthlysalary" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
      <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <%--<script type="text/javascript" src="../Template/assets/js/pages/datatables_advanced.js"></script>--%>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <div id="div_error" runat="server" class="alert alert-danger" visible="false">
                            <button type="button" class="close" data-dismiss="alert"><span>&times;</span><span class="sr-only">Close</span></button>							
                            <asp:Label ID="lbl_error" runat="server"></asp:Label>
                        </div>
     <div class="panel panel-flat">
          
                
                     <div class="panel-heading">
                         <h5 class="panel-title">Monthly Salary Details</h5>
              <div class="heading-elements">
                <ul class="icons-list">
                                   <a href="payroll.aspx" class="btn btn-primary  pull-right"> Back</a>
              
               
                </ul></div>
                         </div>
                       <div class="panel-body" style="padding: 0px;">
        </div>
                    <table class="table datatable-basic">
                        <thead>
                            <tr>
                                <th>Employee Id</th>
                                <th>Workingdays</th>
                                <th>Leavedays</th>
                                <th>SalaryMonth</th>                            
                                <th>LOP</th>
                               <th>Totalsalary</th>

                            </tr>
                        </thead>
                        <tbody>

                            <asp:PlaceHolder ID="Ph_salary" runat="server"></asp:PlaceHolder>


                        </tbody>
                    </table>

                </div>
          
        
</asp:Content>

