<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Clients.aspx.cs" Inherits="Admin_Clients" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>
    <style>
        #tbl_active th, #tbl_inactive th { white-space: nowrap; }
    </style>

    <script type="text/javascript">
        function fn_DeleteProject(leadKey) {
            if (!confirm("Are you sure you want to remove this client?")) return;

            $.ajax({
                type: "POST",
                url: "Clients.aspx/DeleteProject",
                data: JSON.stringify({ str_leadkey: leadKey }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == "1") {
                        alert("Client has been removed successfully.");
                        location.reload();
                    } else {
                        alert("Sorry, unable to remove this client. Please try again.");
                    }
                },
                error: function (xhr, status, error) {
                    alert("An error occurred while removing the client. Please try again.");
                }
            });
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <%-- Active Clients Grid --%>
    <div class="panel panel-flat">
        <div class="panel-heading">
            <div class="row">
                <div class="col-lg-4">
                    <h5 class="panel-title">Active Organization</h5>
                </div>
                <div class="col-lg-8">
                    <a href="Clientsdetails.aspx" id="a_createlead" runat="server" visible="false" class="btn btn-primary pull-right"><i class="icon-plus-circle2"></i> Create Organization</a>
                </div>
            </div>
        </div>
      
        <div style="overflow-x: auto;">
        <table class="table datatable-basic" id="tbl_active">
            <thead>
                <tr>
                    <th>Organization Code</th>
                    <th>Organization Name</th>
                    <th>Type</th>
                    <th>Company Name</th>
                    <th>Contact Person</th>
                    <th>Status</th>
                    <th>Created On</th>
                    <th style="min-width: 100px; text-align: center;">Action</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_ActiveClients" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
        </div>
        </div>
    <div class="panel panel-flat" style="margin-top: 20px;">
        <div class="panel-heading">
            <h5 class="panel-title">Inactive Organization</h5>
        </div>
        <div class="panel-body" style="padding: 0px;"></div>
        <div style="overflow-x: auto;">
        <table class="table datatable-basic" id="tbl_inactive">
            <thead>
                <tr>
                    <th>Organization Code</th>
                    <th>Organization Name</th>
                    <th>Type</th>
                    <th>Company Name</th>
                    <th>Contact Person</th>
                    <th>Status</th>
                    <th>Created On</th>
                    <th style="min-width: 100px; text-align: center;">Action</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_InactiveClients" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
        </div>
    </div>

    </asp:Content>