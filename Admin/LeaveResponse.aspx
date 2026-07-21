<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="LeaveResponse.aspx.cs" Inherits="WEB_Admin_LeaveResponse" %>

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
            margin-right: 0;
            font-size: 13px;white-space: nowrap;

        }

        .date-filter {
            display: flex;
            align-items: center;
            gap: 12px;
        }

        .date-label {
            font-size: 13px;
margin: 0;
white-space: nowrap;
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


    <%--<script type="text/javascript">
        function fn_DeleteProject(Employeeleavedetailskey) {


            if (confirm("Are you sure,you want to remove this?")) {
                $.ajax({
                    type: "POST",
                    url: "Leaveresponse.aspx/DeleteProject",
                    data: "{ Productkey: '" + Employeeleavedetailskey + "'}",
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
            <h5 class="panel-title">Employee Leave Details</h5>
            <br />
            <div class="date-filter">
                <!-- Month Dropdown -->
                <label for="ddlDate" class="date-label">Month : </label>
                <asp:DropDownList ID="ddlDate" runat="server" CssClass="form-control Date-dropdown"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlDate_SelectedIndexChanged">
                </asp:DropDownList>

                <!-- Year Dropdown -->
                <label for="ddlYear" class="year-label" >Year : </label>
                <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control year-dropdown"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                </asp:DropDownList>

                <!-- Button -->
                <a href="UninformedLeaves.aspx" class="btn btn-info " style="margin-left: 6px;">Uninformed Leave</a>
            </div>
        </div>
    


    <div class="panel-body" style="padding: 0px;">
    </div>

    <table class="table datatable-basic">
        <thead>
            <tr>
                <th>Employee ID</th>
                <th>User Name</th>
                <th>From Date</th>
                <th>To Date</th>
                <th>Leave Days</th>
                <th>Status</th>
                <th>View</th>

            </tr>
        </thead>
        <tbody>
            <asp:PlaceHolder ID="PH_Leave" runat="server"></asp:PlaceHolder>
        </tbody>
    </table>
    </div>
</asp:Content>

