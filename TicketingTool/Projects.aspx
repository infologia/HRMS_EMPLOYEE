<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Projects.aspx.cs" Inherits="TicketingTool_Projects" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_advanced.js"></script>

    <!-- Theme JS files -->
    <script type="text/javascript" src="../Template/assets/js/plugins/notifications/pnotify.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/notifications/noty.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/notifications/jgrowl.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/components_notifications_other.js"></script>
    <!-- /theme JS files -->

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel panel-flat">
        <div class="panel-heading">
            <h5 class="panel-title">Project Details </h5>
            <div class="heading-elements">
                <a href="CreateProject.aspx" class="btn btn-labeled btn-labeled-right bg-blue heading-btn">Create Project<b><i class="icon-files-empty2"></i></b></a>
                <button type="button" class="btn btn-success btn-sm noty-runner" data-layout="topRight" data-type="success">Launch <i class="icon-play3 position-right"></i></button>
            </div>
        </div>
    </div>

    <div class="row">

        <asp:PlaceHolder ID="PH_Panel" runat="server"></asp:PlaceHolder>
    </div>




    <div class="panel panel-flat">
        <div class="panel-body" style="padding: 0px;">
        </div>

        <table class="table table-bordered table-hover datatable-highlight">
            <thead>
                <tr>

                    <th>Project Name</th>
                    <th>Project Type</th>
                    <th>Created By</th>

                    <th>Createdon</th>

                    <th>Status</th>
                    <th>Action</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_Project" runat="server"></asp:PlaceHolder>
            </tbody>


        </table>

    </div>
</asp:Content>

