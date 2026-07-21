<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="complaintresponseview.aspx.cs" Inherits="WEB_Employee_complaintresponseview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <%--    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>--%>
    <script type="text/javascript">
        function fn_DeleteProject(Complaintkey) {
            if (confirm("Are you sure,you want to remove this?")) {
                $.ajax({
                    type: "POST",
                    url: "complaintresponseview.aspx/DeleteProject",
                    data: "{ str_Complaintkey: '" + Complaintkey + "'}",
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
            <%--<h5 class="panel-title">Employee Complaint Details</h5>
            <div class="heading-elements">
                <a href="Complaints.aspx" class="btn btn-primary  pull-right"><i class="icon-plus-circle2"></i> Create Complaint</a>
            </div>--%>

            <div class="row">
                <div class="col-lg-4">
                    <h5 class="panel-title">Employee Complaint Details</h5>
                </div><br />
                <div class="col-lg-8">
                <a href="Complaints.aspx" class="btn btn-primary  pull-right"><i class="icon-plus-circle2"></i> Create Complaint</a>
                </div>
            </div>

        </div>
        <div class="panel-body" style="padding: 0px;">
        </div>
        <table class="table datatable-basic">
            <thead>
                <tr>
                    <th>Complaint Id</th>
                    <th>Employee Id</th>
                    <th>Employee Name</th>
                    <th>Complaint Category</th>
                    <th>Status</th>
                    <th>Update</th>
                    <th>Remove</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_Complaint" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
    </div>



</asp:Content>

