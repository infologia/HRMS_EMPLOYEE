<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Assetscategory.aspx.cs" Inherits="Admin_Assetscategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <div class="panel panel-flat">
    <div class="panel-heading">
        <h5 class="panel-title">Assets Category</h5>
            <a href="Assetscategorycreation.aspx" class="btn btn-primary  pull-right" ><i class="icon-plus-circle2"></i> Create Category</a>
    </div>
       <br />
    <div class="panel-body" style="padding: 0px;">
    </div>
    <table class="table datatable-basic">
        <thead>
            <tr>
                <th>Category</th>
                <th>Asset Type</th>
                <th>Created Date</th>
                <th>Modified Date</th>
                <th>Status</th>
                <th>Update</th>
            </tr>
        </thead>
        <tbody>
            <asp:PlaceHolder ID="PH_category" runat="server"></asp:PlaceHolder>
        </tbody>
    </table>
</div>
</asp:Content>

