<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Salesdocs.aspx.cs" Inherits="Employee_Salesdocs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="panel panel-flat">
        <div class="panel-heading">
            <div class="row">
                <div class="col-lg-4">
                    <h5 class="panel-title">Sales Documents</h5>
                </div>
                <div class="col-lg-8">
                </div>
            </div>
        </div>
        <div class="panel-body" style="padding: 0px;">
        </div>
        <table class="table datatable-basic">
            <thead>
                <tr>
                    <th>File Title</th>
                    <th>Created By</th>
                    <th>Created On</th>
                    <th>Description</th>
                    <th>View Document</th>
                    <th>Download</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_SalesDocs" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
    </div>
</asp:Content>

