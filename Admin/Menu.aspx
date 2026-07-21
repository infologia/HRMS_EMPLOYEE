<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Menu.aspx.cs" Inherits="Admin_Menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <script type="text/javascript">
        function fn_DeleteProject(MenuKey) {
         

            if (confirm("Are you sure,you want to delete this?")) {
                $.ajax({
                    type: "POST",
                    url: "Menu.aspx/DeleteProject",
                    data: "{ str_PjtCategorykey: '" + MenuKey + "'}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: "true",
                    cache: "false",

                    success: function (data, status) {
                        // On success  
                        var response = ["success", data];
                        var ResponseData = response[1].d;
                        var ResponseStatus = ResponseData.split("&&&")[0];
                        if (ResponseStatus == "1") {
                            alert(" This Menu has been deleted ");
                            location.reload();
                            return;

                            return;
                        }
                        else {
                            alert("Sorry, unable to delete this, please try after sometime.");
                            HideLoadingScreen();
                            return;
                        }
                    },
                    error: function (xhr, status, error) {
                        alert("Sorry, unable to delete this, please try after sometime.");
                        HideLoadingScreen();
                        return;
                    }
                });
            }
            else {
                HideLoadingScreen();
                return;
            }
        }
        </script>
      <div class="panel panel-flat">
          <div class="panel-heading">
<div class="row">
<div class="col-md-6 pull-left">
<h5 class="panel-title">Sub Menulist</h5></div><div class="col-md-6  pull-right">
<a href="MenuUpdate.aspx" class="btn btn-primary pull-right"><i class="icon-user-plus"></i> Add New</a>
</div>
</div>
 
</div>
       
        <div class="panel-body" style="padding: 0px;">
        </div>
        <table class="table datatable-basic">
            <thead>
                <tr>                   
                    <th>Menu Name</th>
                    <th>Page Name</th>
                    <th>ListOn</th>
                    <th>CreatedOn</th>
                    <th>Update</th>
                    <th>Delete</th>                                   
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_Menu" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
           
    </div>
</asp:Content>


