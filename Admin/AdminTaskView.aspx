<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="AdminTaskView.aspx.cs" Inherits="Admin_AdminTaskView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel panel-flat">
        <div class="panel-heading">
            <h5 class="panel-title">Document</h5>
            <div class="heading-elements">
                <ul class="icons-list">
                </ul>
            </div>
        </div>
        <div class="panel-body" style="padding: 0px;">
        </div>
        <table class="table datatable-button-init-basic">
            <thead>
                <tr>

                    <th>Project</th>
                    <th>Task</th>
                    <th>Duedate</th>
                    <th>status</th>
                    <th>AllocatedHours</th>
                    <th>SpendedTime</th>
                    <th>Starttime</th>
                    <th>Endtime</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_admintaskview" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
    </div>
</asp:Content>

