<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="AllConnections.aspx.cs" Inherits="Employee_AllConnections" %>

<%-- Add content controls here --%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

        <div class="panel panel-flat">
    <div class="panel-heading">
        <div class="row">
            <div class="col-lg-4">
                <h5 class="panel-title">All Connections Details</h5>
            </div>
            <div class="col-lg-8">
                <a href="Myconnections.aspx" runat="server" class="btn btn-primary  pull-right"><i class="icon-plus-circle2"></i> Create Connection</a>
            </div>
        </div>
    </div>
    <div class="panel-body" style="padding: 0px;">
    </div>
    <table class="table datatable-basic">
        <thead>
            <tr>
                <th>Name</th>
                <th>Company</th>
                <th>Position</th>
                <th>LeadType</th>
                <th>Created</th>
                <th>CreatedBy</th>
                <th>Update</th>
            </tr>
        </thead>
        <tbody>
            <asp:PlaceHolder ID="PH_connection" runat="server"></asp:PlaceHolder>
        </tbody>
    </table>
</div>

</asp:Content>
