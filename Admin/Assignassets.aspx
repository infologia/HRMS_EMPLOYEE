<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Assignassets.aspx.cs" Inherits="Admin_Assignassets" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel panel-flat">
        <div class="panel-heading">
            <h5 class="panel-title">Assigned Assets</h5>
            <div class="row">
                <a href="AssignassetsCreation.aspx" class="btn btn-primary  pull-right"><i class="icon-plus-circle2"></i> Create</a>

            </div>
        </div>
    <div class="panel-body" style="padding: 0px;">
    </div>
    <table class="table datatable-basic">
        <thead>
            <tr>
                <th style="min-width:140px; white-space:nowrap;">Employee Name</th>
                <th style="min-width:140px; white-space:nowrap;">Equipment Name</th>
                <th>Brand</th>
                <th style="min-width:150px; white-space:nowrap;">Modal Serial Number</th>
                <th style="min-width:130px; white-space:nowrap;">Assigned Date</th>
                <th>Status</th>
                <th class="text-center">Action</th>

            </tr>
        </thead>
        <tbody>
            <asp:PlaceHolder ID="PH_assests" runat="server"></asp:PlaceHolder>
        </tbody>
    </table>
    </div>
</asp:Content>

