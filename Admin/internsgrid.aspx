<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="internsgrid.aspx.cs" Inherits="Admin_internsgrid" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel panel-flat">
        <div class="panel-heading">
            <div class="row">
                <div class="col-lg-6">
                    <h5 class="panel-title">Interns</h5>
                </div>
            </div>
        </div>
        <table class="table datatable-basic" id="tblInterns">
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Qualification</th>
                    <th>Passed Out</th>
                    <th>College</th>
                    <th>Location</th>
                    <th>Phone</th>
                    <th>Applied On</th>
                    <th style="text-align:center;">Resume</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_Interns" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
    </div>
</asp:Content>
