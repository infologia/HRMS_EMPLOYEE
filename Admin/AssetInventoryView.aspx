<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage/AdminMaster.master" AutoEventWireup="true" CodeFile="AssetInventoryView.aspx.cs" Inherits="Web_AssetInventoryView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel panel-flat">
        <div class="panel-heading">
            <div class="row">
                <div class="col-lg-4">
                    <h5 class="panel-title">Asset Inventory Details</h5>
                </div>
                <br />
                <div class="col-lg-8">
                    <a href="AssetInventory.aspx" runat="server" class="btn btn-primary  pull-right"><i class="icon-plus-circle2"></i> Create </a>
                </div>
            </div>
        </div>
        <div class="panel-body" style="padding: 0px;">
        </div>
        <table class="table datatable-basic">
            <thead>
                <tr>
                    <th>Asset Tag</th>
                    <th>Category</th>
                    <th>Brand</th>
                    <th>Placed Location</th>
                    <th style="min-width:120px; white-space:nowrap;">Purchase Date</th>
                    <th>Status</th>
                    <th class="text-center">Action</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_assetinventoryview" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
    </div>
</asp:Content>

