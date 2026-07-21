<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="EmployeePayroll.aspx.cs" Inherits="Employee_EmployeePayroll" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel panel-flat">
        <div class="panel-heading">
            <h5 class="panel-title">Monthly Salary Details</h5>
        </div>
        <div class="panel-body" style="padding: 0px;">
        </div>
        <table class="table datatable-basic">
            <thead>
                <tr>
                    <th>Employee Id</th>
                    <th>Month</th>
                    <th>Netpay</th>
                    <th>LOP</th>
                    <th>Workingdays</th>
                    <th>Leavedays</th>
                    <th>Action</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="payroll" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
    </div>
</asp:Content>

