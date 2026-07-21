<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Overallreport.aspx.cs" Inherits="Admin_Overallreport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
   <script>
       $('.datatable-fixed-left').DataTable({
    scrollX: true,     
    scrollY: false,   
    });</script>
	<script type="text/javascript" src="../Template/assets/js/plugins/loaders/pace.min.js"></script>
	<script type="text/javascript" src="../Template/assets/js/core/libraries/jquery.min.js"></script>
	<script type="text/javascript" src="../Template/assets/js/core/libraries/bootstrap.min.js"></script>
	<script type="text/javascript" src="../Template/assets/js/plugins/loaders/blockui.min.js"></script>

	<%--<script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
	<script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/fixed_columns.min.js"></script>
	<script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_fixed_columns.js"></script>--%>
    	<script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
	<script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/responsive.min.js"></script>

	<script type="text/javascript" src="../Template/assets/js/pages/datatables_responsive.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   <div class="panel panel-flat">
    <div class="panel-heading">
        <div class="row">
            <div class="col-lg-4">
                <h5 class="panel-title">Overall Assets Report</h5>
            </div>
        </div>
    </div>
    <table class="table datatable-responsive" width="100%">
        <thead>
           <tr>
    <th>Asset Tag</th>
    <th>Category</th>
    <th>Equipment Name</th>
    <th>Brand</th>
    <th>Username</th>
    <th>Assigned Date</th>
    <th>Status</th>
    <th>Condition</th>
    <th>Placed Location</th>
    <th>Quantity</th>
    <th>Model/Serial No</th>
    <th>Purchase Cost</th>
    <th>Purchase Date</th>
</tr>
        </thead>
        <tbody>
            <asp:PlaceHolder ID="PH_OverallAssets" runat="server"></asp:PlaceHolder>
        </tbody>
    </table>
           </div>
                
</asp:Content>

