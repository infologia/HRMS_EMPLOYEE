<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="ProjectCategoryView.aspx.cs" Inherits="Admin_ProjectCategoryView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

        <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <%--<script type="text/javascript" src="../Template/assets/js/pages/datatables_advanced.js"></script>--%>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <script type="text/javascript">
        function fn_DeleteProject(PjtCategorykey) {


            if (confirm("Are you sure,you want to delete this?")) {
                $.ajax({
                    type: "POST",
                    url: "Projectcategoryview.aspx/DeleteProject",
                    data: "{ str_PjtCategorykey: '" + PjtCategorykey + "'}",
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
                            alert(" This  has been deleted ");
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
            <h5 class="panel-title">Project Category Details</h5>
            <div class="heading-elements">
                <ul class="icons-list">
                  <a href="ProjectCategory.aspx" class="btn btn-primary pull-right"><i class="icon-user-plus"></i> Add New</a>
                </ul>
            </div>
        </div>

        <div class="panel-body" style="padding: 0px;">
        </div>

        <table class="table datatable-basic">
            <thead>
                <tr>

                   
                   
                    <th>Category Name</th>
                    <th>Description</th>
                    <th>Status</th>
                    <th>Date</th>
                    <th>Update Category</th>
                    <th>Delete Category</th>
                    
                
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_ProjectCategoryView" runat="server"></asp:PlaceHolder>

            </tbody>
        </table>
           
    </div>
</asp:Content>

