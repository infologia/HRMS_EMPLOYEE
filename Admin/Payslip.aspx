<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Payslip.aspx.cs" Inherits="Admin_Payslip" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <%--<script type="text/javascript" src="../Template/assets/js/pages/datatables_advanced.js"></script>--%>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-primary">
                <div class="panel-body">
                     <div class="heading-elements">
                <ul class="icons-list">
                                   <a href="payroll.aspx" class="btn btn-primary  pull-right"> Back</a>
                   <%-- <li><a href="#"><i class="icon-file-pdf" title="Export to .pdf"></i></a></li>
                    <li><a href="#"><i class="icon-file-excel" title="Export to .csv"></i></a></li>
                    <li><a href="#"><i class="icon-file-word" title="Export to .doc"></i></a></li>--%>
                </ul>
                         </div><br /><br />
                    <table class="table datatable-basic">
                        <thead>
                            <tr>
                                <th>Employee Id</th>
                                <th>Workingdays</th>
                                <th>Leavedays</th>
                                <th>SalaryMonth</th>                              
                                <th>LOP</th>
                                 <th>Total Salary</th>
                              

                            </tr>
                        </thead>
                        <tbody>

                            <asp:PlaceHolder ID="payroll" runat="server"></asp:PlaceHolder>


                        </tbody>
                    </table>

                </div>
            </div>
        </div>
    </div>

</asp:Content>

