<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="WorkingDayViewDetails.aspx.cs" Inherits="Admin_WorkingDayViewDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>
    <style>
        .year-filter {
            display: flex;
            align-items: center;   
        }

        .year-label {
            margin-right: 8px;
            font-size: 13px;
        }

        .year-dropdown {
            width: 120px;
            height: 30px;
            padding: 2px 6px;
            font-size: 13px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel panel-flat">
        <div class="panel-heading">
            <div class="row">
                <div class="col-md-6 pull-left">
            <h5 class="panel-title">Employee View Details</h5>
            <br />
            <div class="year-filter">
                <label for="ddlYear" class="year-label"> Year:</label>
                <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control year-dropdown"
                    AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                </asp:DropDownList>
                </div><br />
                    </div>
                    <div class="col-md-6  pull-right">
                    <a href="WorkingDayDetails.aspx" class="btn btn-primary  pull-right"><i class="icon-stack-plus"></i> Create New</a>
                </div>
            </div>
                
        </div>

        <div class="panel-body" style="padding: 0px;">
        </div>

        <table class="table datatable-basic">
            <thead>
                <tr>
                    <th>Year</th>
                    <th>Month</th>
                    <th>Number of days in month</th>
                    <th>Number of working days in month</th>
                    <th>Number of leave days in month</th>
                    <th>Created on</th>
                    <th>Update</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_EmployeeView" runat="server"></asp:PlaceHolder>

            </tbody>
        </table>
    </div>

</asp:Content>

