<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="SalaryDetails.aspx.cs" Inherits="WEB_Admin_SalaryDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <%--<script type="text/javascript" src="../Template/assets/js/pages/datatables_advanced.js"></script>--%>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel panel-flat">
        <div class="panel-heading">
            <div class="row">
<div class="col-md-6 pull-left">
            <h5 class="panel-title">Employee Salary Details</h5>
    <br />
    </div>
<div class="col-md-6  pull-right">
                <ul class="icons-list">
                    <a href="EmployeeSalaryResgistraion.aspx" class="btn btn-primary  pull-right"><i class=" icon-pencil5"></i> Register Salary</a>
                    <%-- <li><a href="#"><i class="icon-file-pdf" title="Export to .pdf"></i></a></li>
                    <li><a href="#"><i class="icon-file-excel" title="Export to .csv"></i></a></li>
                    <li><a href="#"><i class="icon-file-word" title="Export to .doc"></i></a></li>--%>
                </ul>
    </div>
                </div>
            </div>
       

        <div class="panel-body" style="padding: 0px;">
        </div>

        <table class="table datatable-basic">
            <thead>
                <tr>
                    <th>Employee ID</th>
                    <th>User Name</th>
                    <th>Monthly Salary</th>
                    <th>Net Pay</th>
                    <th>Update</th>
                    <th>Created On</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_Salery" runat="server"></asp:PlaceHolder>

            </tbody>
        </table>
    </div>
</asp:Content>

