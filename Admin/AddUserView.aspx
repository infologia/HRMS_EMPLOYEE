<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="AddUserView.aspx.cs" Inherits="Admin_AddUserView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>


    <script type="text/javascript">
        function fn_DeleteProject(AddUserkey) {


            if (confirm("Are you sure,you want to remove this?")) {
                $.ajax({
                    type: "POST",
                    url: "Adduserview.aspx/DeleteProject",
                    data: "{ str_AddUserkey: '" + AddUserkey + "'}",
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
                            alert(" This  has been removed ");
                            location.reload();
                            return;

                            return;
                        }
                        else {
                            alert("Sorry, unable to remove this, please try after sometime.");
                            HideLoadingScreen();
                            return;
                        }
                    },
                    error: function (xhr, status, error) {
                        alert("Sorry, unable to remove this, please try after sometime.");
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel panel-flat">
        <div class="panel-heading">
            <h5 class="panel-title">
                <asp:Label ID="lb_show" runat="server"></asp:Label>
                /
                <asp:Label ID="lb_show2" runat="server"></asp:Label>
            </h5>
            <div class="heading-elements">

                <a href="Projects.aspx" class="btn btn-primary">Back</a> &nbsp 
          
            <asp:LinkButton ID="btn_add" runat="server" CssClass="btn btn-primary pull-right" OnClick="btn_add_Click"><i class="icon-user-plus"></i> Add New</asp:LinkButton>
            </div>
        </div>

        <div class="panel-body" style="padding: 0px;">
        </div>
       
        <table class="table datatable-basic">
            <thead>
                <tr>
                    <th>Date</th>
                    <th>Username </th>
                    <th>Designation</th>
                    <th>Department</th>
                    <th>Division</th>
                    <th>Action</th>

                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_AddUserView" runat="server"></asp:PlaceHolder>

            </tbody>
        </table>

    </div>
</asp:Content>

