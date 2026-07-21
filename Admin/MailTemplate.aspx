<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="MailTemplate.aspx.cs" Inherits="WEB_MailTemplate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
                 <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <%--<script type="text/javascript" src="../Template/assets/js/pages/datatables_advanced.js"></script>--%>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
<script type="text/javascript">
    function fn_DeleteProject(Mailtemplatekey) {


        if (confirm("Are you sure,you want to remove this?")) {
            $.ajax({
                type: "POST",
                url: "Mailtemplate.aspx/DeleteProject",
                data: "{ Productkey: '" + Mailtemplatekey + "'}",
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
      <div class="panel panel-flat">
        <div class="panel-heading">
            <h5 class="panel-title">Employee Mail Details</h5>
            <div class="heading-elements">
               
                <a href="Adminmailtemplate.aspx" class="btn btn-primary  pull-right"><i class="icon-plus-circle2"></i> Create New</a>
               
            </div>
        </div>

        <div class="panel-body" style="padding: 0px;">
        </div>

        <table class="table datatable-basic">
            
            <thead>
                <tr>

                    <th>Header</th>
                    <th>Subject</th>
                    <th>Content</th>
                    <th>Image</th>
                    <th>Footer</th>
                   
                    <th>Update</th>
                     <th>Edit</th>
                        <th>Delete</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_mail" runat="server"></asp:PlaceHolder>

            </tbody>
        </table>
    </div>
</asp:Content>

