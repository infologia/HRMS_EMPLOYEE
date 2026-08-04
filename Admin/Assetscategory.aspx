<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Assetscategory.aspx.cs" Inherits="Admin_Assetscategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <div class="panel panel-flat">
    <div class="panel-heading">
        <div class="row" style="display: flex; align-items: center; justify-content: space-between; flex-wrap: wrap;">
            <div class="col-md-4">
                <h5 class="panel-title">Assets Category</h5>
            </div>
            <div class="col-md-8 text-right" style="display: flex; gap: 10px; align-items: center; justify-content: flex-end; flex-wrap: wrap;">
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
                <a href="Assetscategorycreation.aspx" class="btn btn-primary"><i class="icon-plus-circle2"></i> Create</a>
            </div>
        </div>
    </div>
    <table class="table datatable-basic">
        <thead>
            <tr>
                <th>Category</th>
                <th>Asset Type</th>
                <th>Created Date</th>
                <th>Modified Date</th>
                <th>Status</th>
                <th class="text-center">Action</th>
            </tr>
        </thead>
        <tbody>
            <asp:PlaceHolder ID="PH_category" runat="server"></asp:PlaceHolder>
        </tbody>
    </table>
</div>
</asp:Content>

