<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Assignassets.aspx.cs" Inherits="Admin_Assignassets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel panel-flat">
        <div class="panel-heading">
            <h5 class="panel-title">Assigned Assets</h5>
            <div class="row">
                <a href="AssignassetsCreation.aspx" class="btn btn-primary  pull-right"><i class="icon-plus-circle2"></i> Assign Assets</a>

            </div>
        </div>
    <div class="panel-body" style="padding: 0px;">
    </div>
    <table class="table datatable-basic">
        <thead>
            <tr>
                <th>Employee Name</th>
                <th>Equipment Name</th>
                <th>Brand</th>
                <th>Modal Serial Number</th>
                <th>Assigned Date</th>
                <th>Status</th>
                <th>Update</th>

            </tr>
        </thead>
        <tbody>
            <asp:PlaceHolder ID="PH_assests" runat="server"></asp:PlaceHolder>
        </tbody>
    </table>
    </div>
</asp:Content>

