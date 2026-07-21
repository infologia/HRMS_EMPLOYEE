<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="MyConnectionsdetails.aspx.cs" Inherits="Employee_MyConnectionsdetails" %>

<%-- Add content controls here --%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
        <script type="text/javascript">
            function fn_DeleteProject(ConnectionKey) {           
            if (!confirm("Are you sure you want to remove this Connection?")) return;

            $.ajax({
                type: "POST",
                url: "MyConnectionsdetails.aspx/DeleteProject",
                data: JSON.stringify({ str_ConnectionKey: ConnectionKey }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == "1") {
                        alert("Connection has been removed successfully.");
                        location.reload(); // Refresh the page to reflect deletion
                    } else {
                        alert("Sorry, unable to remove this Connection. Please try again.");
                    }
                },
                error: function (xhr, status, error) {
                    alert("An error occurred while removing the Connection. Please try again.");
                }
            });
        }
        </script>
    </asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">


        <div class="panel panel-flat">
        <div class="panel-heading">
            <div class="row">
                <div class="col-lg-4">
                    <h5 class="panel-title">My Connections Details</h5>
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
                    <th>Delete</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_connection" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
    </div>



    </asp:Content>




