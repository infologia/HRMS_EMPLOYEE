<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Suggestionresponse.aspx.cs" Inherits="WEB_Admin_Suggestionresponse" %>

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
    <div class="panel panel-flat">
        <div class="panel-heading">
            <h5 class="panel-title">Employee Suggestion Details</h5>
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
            </div>

        </div>

        <div class="panel-body" style="padding: 0px;">
        </div>

        <table class="table datatable-basic">
            <thead>
                <tr>
                    <th>Suggestion Id</th>
                    <th>Employee Id</th>
                    <th>User Name</th>
                    <th>Suggestion Category</th>
                    <th>Status</th>
                    <th>Action</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_Suggestion" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
    </div>
</asp:Content>

