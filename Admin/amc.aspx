<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="amc.aspx.cs" Inherits="Admin_amc" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
     <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
 <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
 <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
 <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>

 <script type="text/javascript">
     function fn_DeleteProject(leadKey) {
         if (!confirm("Are you sure you want to remove this AMC?")) return;

         $.ajax({
             type: "POST",
             url: "amc.aspx/DeleteProject",
             data: JSON.stringify({ str_leadkey: leadKey }),
             contentType: "application/json; charset=utf-8",
             dataType: "json",
             success: function (response) {
                 if (response.d == "1") {
                     alert("amc has been removed successfully.");
                     location.reload(); // Refresh the page to reflect deletion
                 } else {
                     alert("Sorry, unable to remove this amc. Please try again.");
                 }
             },
             error: function (xhr, status, error) {
                 alert("An error occurred while removing the amc. Please try again.");
             }
         });
     }
 </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="panel panel-flat">
    <div class="panel-heading">
        <div class="row">
            <div class="col-md-6 pull-left">
                <h5 class="panel-title">AMS Details - Live</h5>
            </div>
            <div class="col-md-6 pull-right">
                <a href="AmcDetails.aspx" id="a_createlead" runat="server" visible="false" class="btn btn-primary pull-right"><i class="icon-plus-circle2"></i> Create AMC</a>
            </div>
        </div>
    </div>

    <div class="panel-body" style="padding: 0px;">
    </div>
    <table class="table datatable-basic">
        <thead>
            <tr>
                <th>Company Name</th>
                <th>Project Name</th>
                <th>Live Date</th>
                <th>Status</th>
                <th>Created Date</th>
                <th>Action</th>
            </tr>
        </thead>
        <tbody>
            <asp:PlaceHolder ID="PH_Amc_Incompleted" runat="server"></asp:PlaceHolder>
        </tbody>
    </table>
</div>

    <div class="panel panel-flat">
        <div class="panel-heading">
<div class="row">
<div class="col-md-6 pull-left">
<h5 class="panel-title">AMS Details - Closed</h5></div>
</div>
 
</div>
    
    <div class="panel-body" style="padding: 0px;">
    </div>        
    <table class="table datatable-basic">
        <thead>
            <tr>
                <th>Company Name</th>
                <th>Project Name</th>
                <th>Live Date</th>
                <th>Status</th>
                <th>Created Date</th>                   
                <th>Action</th>
            </tr>
        </thead>
        <tbody>
            <asp:PlaceHolder ID="PH_Amc_Completed" runat="server"></asp:PlaceHolder>
        </tbody>
    </table>
</div>

</asp:Content>
