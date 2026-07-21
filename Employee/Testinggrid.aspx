<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Testinggrid.aspx.cs" Inherits="Employee_Testinggrid" %>

<%-- Add content controls here --%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
        <script type="text/javascript">
            function fn_DeleteProject(TaskTestingKey) {
            if (!confirm("Are you sure you want to remove this Testing?")) return;

            $.ajax({
                type: "POST",
                url: "Testinggrid.aspx/DeleteProject",
                data: JSON.stringify({ str_TaskTestingKey: TaskTestingKey }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == "1") {
                        alert("Testing has been removed successfully.");
                        location.reload(); // Refresh the page to reflect deletion
                    } else {
                        alert("Sorry, unable to remove this Testing. Please try again.");
                    }
                },
                error: function (xhr, status, error) {
                    alert("An error occurred while removing the Testing. Please try again.");
                }
            });
        }
        </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
        <div class="panel panel-flat">
        <div class="panel-heading">
            <div class="row">
                <div class="col-md-6 pull-left">
                    <h5 class="panel-title">Task Details</h5>
                </div>
            </div>
        </div>
      
        <table class="table datatable-basic">
            <thead>
                <tr>

                    <th>Project Name</th>
                    <th>Assigned By</th>
                    <th>Start Date</th>
                    <th>End Date</th>
                    <th>Modified By</th>
                    <th>Status</th>
                    <th>Update</th>
                     <th >Remove</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_Task" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
    </div>

</asp:Content>
