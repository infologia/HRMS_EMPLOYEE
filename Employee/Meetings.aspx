<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Meetings.aspx.cs" Inherits="Employee_Meetings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>

    <script type="text/javascript">
        function fn_DeleteProject(leadKey) {
            if (!confirm("Are you sure you want to remove this lead?")) return;

            $.ajax({
                type: "POST",
                url: "Meetings.aspx/DeleteProject",
                data: JSON.stringify({ str_leadkey: leadKey }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == "1") {
                        alert("Lead has been removed successfully.");
                        location.reload(); // Refresh the page to reflect deletion
                    } else {
                        alert("Sorry, unable to remove this Meeting. Please try again.");
                    }
                },
                error: function (xhr, status, error) {
                    alert("An error occurred while removing the Meeting. Please try again.");
                }
            });
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="panel panel-flat">
        <div class="panel-heading">
            <div class="row">
                <div class="col-lg-4">
                    <h5 class="panel-title">Meetings List</h5>
                </div>
                <div class="col-lg-8">
                    <a href="Meetingdetails.aspx" id="a_createlead" runat="server" class="btn btn-primary  pull-right"><i class="icon-plus-circle2"></i>&nbsp;Create Meeting</a>
                </div>
            </div>
        </div>
    </div>
    <div class="panel panel-flat">
        <div class="panel-heading">
            <div class="row">
                <div class="col-lg-4">
                    <h5 class="panel-title">Upcoming Meetings </h5>
                </div>
            </div>
        </div>
        <div class="panel-body" style="padding: 0px;">
        </div>
        <table class="table datatable-basic">
            <thead>
                <tr>
                    <th>Title</th>
                    <th>Meeting Date</th>
                    <th>Start Time</th>
                    <th>End Time</th>
                    <th>Status</th>
                    <th>Created</th>
                    <th id="UpdateView" runat="server">Update</th>

                    <th>Delete</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_leave" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
    </div>
    <div class="panel panel-flat">
        <div class="panel-heading">
            <div class="row">
                <div class="col-lg-4">
                    <h5 class="panel-title">Completed Meetings </h5>
                </div>
            </div>
        </div>

        <table class="table datatable-basic">
            <thead>
                <tr>
                    <th>Title</th>
                    <th>Meeting Date</th>
                    <th>Start Time</th>
                    <th>End Time</th>
                    <th>Hours</th>
                    <th>Status</th>
                    <th>Created</th>
                    <th >View</th>

                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_Completed" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
    </div>
</asp:Content>

