<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="PermissionResponse.aspx.cs" Inherits="WEB_Admin_PermissionResponse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>

    <style>

   .panel-heading {
    display: flex;
    justify-content: space-between; 
   align-items: center;
   }

        .year-label {
            margin-right: 8px;
            margin-left: 15px;
            font-size: 13px;
        }

        .date-filter {
            display: flex;
            align-items: center;
        }

        .date-label {
            margin-right: 8px;
            font-size: 13px;
        }

        .date-dropdown {
            width: 120px;
            height: 30px;
            padding: 2px 6px;
            font-size: 13px;
        }
    </style>

    <style>

    @media (max-width: 767px) {

        .panel-heading {
    flex-direction: column;
    align-items: flex-start;

}

    .date-filter {
        flex-wrap: wrap;          
        justify-content: flex-end;
    }

    .date-filter label {
        width: 100%;
        text-align: left;
        margin: 4px 0;
    }

    .date-filter select,
    .date-filter a {
        margin-bottom: 6px;
    }

}

   </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <%-- <script type="text/javascript">
        function fn_DeleteProject(Employeepermissiondetailskey) {


            if (confirm("Are you sure,you want to remove this?")) {
                $.ajax({
                    type: "POST",
                    url: "Permissionresponse.aspx/DeleteProject",
                    data: "{ Productkey: '" + Employeepermissiondetailskey + "'}",
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
        </script>--%>

    <div class="panel panel-flat">
        <div class="panel-heading">
            <h5 class="panel-title">Employee Permission Details</h5>
            <br />
            <div class="date-filter">
                <label for="ddlDate" class="date-label">Select Mode:</label>
                <asp:DropDownList ID="ddlDate" runat="server" CssClass="form-control Date-dropdown"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlDate_SelectedIndexChanged">
                </asp:DropDownList>

                <label for="ddlYear" class="year-label">Select Year: </label>
                <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control year-dropdown"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                </asp:DropDownList>
            </div>
        </div>

        <div class="panel-body" style="padding: 0px;">
        </div>

        <table class="table datatable-basic">
            <thead>
                <tr>
                    <th>Employeeid</th>
                    <th>Username</th>
                    <th>RequestDate</th>
                    <th>Fromtime</th>
                    <th>Totime</th>
                    <th>PermissionHours</th>
                    <th>Status</th>
                    <th>Action</th>

                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_Permission" runat="server"></asp:PlaceHolder>

            </tbody>
        </table>
    </div>
</asp:Content>

