<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Documents.aspx.cs" Inherits="WEB_Documents" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="panel panel-flat">
        <div class="panel-heading">
            <h5 class="panel-title">Document</h5>
                 <a href="DocumentSend.aspx" class="btn btn-primary  pull-right"><i class="icon-stack-plus"></i> Create New</a>
        </div>
        <br />
        <div class="panel-body" style="padding: 0px;">
        </div>

        <table class="table datatable-basic">
            <thead>
                <tr>
                    <th>Date</th>
                    <th style="min-width:110px;">Employee Id</th>
                    <th>Name</th>
                    <th style="min-width:150px;">Document Name</th>
                    <th class="text-center">Action</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_DocumentView" runat="server"></asp:PlaceHolder>

            </tbody>
        </table>
    </div>
     
</asp:Content>

