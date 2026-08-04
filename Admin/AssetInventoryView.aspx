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
    <div class="panel-body" style="padding: 15px;">
        <div class="row">
            <div class="col-md-12">
                <div class="pull-right" style="display: flex; gap: 10px; align-items: center;">
                    <span class="text-semibold" style="margin-right: 5px;">Filter:</span>
                    <asp:DropDownList ID="ddl_Month" runat="server" CssClass="form-control" AutoPostBack="true" Width="150px" OnSelectedIndexChanged="ddl_Filter_SelectedIndexChanged">
                        <asp:ListItem Value="" Text="-- All Months --"></asp:ListItem>
                        <asp:ListItem Value="1" Text="January"></asp:ListItem>
                        <asp:ListItem Value="2" Text="February"></asp:ListItem>
                        <asp:ListItem Value="3" Text="March"></asp:ListItem>
                        <asp:ListItem Value="4" Text="April"></asp:ListItem>
                        <asp:ListItem Value="5" Text="May"></asp:ListItem>
                        <asp:ListItem Value="6" Text="June"></asp:ListItem>
                        <asp:ListItem Value="7" Text="July"></asp:ListItem>
                        <asp:ListItem Value="8" Text="August"></asp:ListItem>
                        <asp:ListItem Value="9" Text="September"></asp:ListItem>
                        <asp:ListItem Value="10" Text="October"></asp:ListItem>
                        <asp:ListItem Value="11" Text="November"></asp:ListItem>
                        <asp:ListItem Value="12" Text="December"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddl_Year" runat="server" CssClass="form-control" AutoPostBack="true" Width="120px" OnSelectedIndexChanged="ddl_Filter_SelectedIndexChanged">
                        <asp:ListItem Value="" Text="-- All Years --"></asp:ListItem>
                        <asp:ListItem Value="2020" Text="2020"></asp:ListItem>
                        <asp:ListItem Value="2021" Text="2021"></asp:ListItem>
                        <asp:ListItem Value="2022" Text="2022"></asp:ListItem>
                        <asp:ListItem Value="2023" Text="2023"></asp:ListItem>
                        <asp:ListItem Value="2024" Text="2024"></asp:ListItem>
                        <asp:ListItem Value="2025" Text="2025"></asp:ListItem>
                        <asp:ListItem Value="2026" Text="2026"></asp:ListItem>
                        <asp:ListItem Value="2027" Text="2027"></asp:ListItem>
                        <asp:ListItem Value="2028" Text="2028"></asp:ListItem>
                        <asp:ListItem Value="2029" Text="2029"></asp:ListItem>
                        <asp:ListItem Value="2030" Text="2030"></asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
        </div>
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

