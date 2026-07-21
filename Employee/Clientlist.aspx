<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Clientlist.aspx.cs" Inherits="Employee_Clientlist" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <%-- Active Clients Grid --%>
    <div class="panel panel-flat">
        <div class="panel-heading">
            <div class="row">
                <div class="col-lg-4">
                    <h5 class="panel-title">Active Clients</h5>
                </div>
                <div class="col-lg-8">
                    <a href="Clientsdetails.aspx" id="a_createlead" runat="server" visible="false" class="btn btn-primary pull-right"><i class="icon-plus-circle2"></i> Create Client</a>
                </div>
            </div>
        </div>
        <div class="panel-body" style="padding:0px;"></div>
        <table class="table datatable-basic" id="tbl_active">
            <thead>
                <tr>
                    <th>Client Code</th>
                    <th>Client Name</th>
                    <th>Company Name</th>
                    <th>Contact Person</th>
                    <th>Status</th>
                    <th>Created</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_Clientlist" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
    </div>

    <%-- Inactive Clients Grid --%>
    <div class="panel panel-flat" style="margin-top:20px;">
        <div class="panel-heading">
            <h5 class="panel-title">Inactive Clients</h5>
        </div>
        <div class="panel-body" style="padding:0px;"></div>
        <table class="table datatable-basic" id="tbl_inactive">
            <thead>
                <tr>
                    <th>Client Code</th>
                    <th>Client Name</th>
                    <th>Company Name</th>
                    <th>Contact Person</th>
                    <th>Status</th>
                    <th>Created</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_InactiveClients" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
    </div>

</asp:Content>
