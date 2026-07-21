<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="TimeMonitoring.aspx.cs" Inherits="WEB_TimeMonitoring" %>

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
            width: 130px;
            height: 30px;
            font-size: 13px;
        }

        .daterange-input {
            width: 180px;
            height: 30px;
            font-size: 13px;
        }

        .form-control {
            padding: 5px 12px !important;
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
                width: 100%;
                margin-top: 8px;
            }

                .date-filter > * {
                    width: 100%;
                }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="panel panel-flat">
        <div class="panel-heading d-flex justify-content-between align-items-center">

            <h5 class="panel-title mb-0">Time Monitoring</h5>
            <br />

            <div class="date-filter">
                <label class="date-label">Month :</label>

                <asp:DropDownList ID="ddlDate" runat="server"
                    CssClass="form-control date-dropdown"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlDate_SelectedIndexChanged">
                </asp:DropDownList>

                <label class="date-label">Period :</label>
                <asp:TextBox ID="SE_date" runat="server" CssClass="form-control daterange-basic daterange-input">
                </asp:TextBox>


                <asp:Button ID="btn_sub" runat="server" CssClass="btn-primary" Text="Submit" OnClick="btn_sub_Click" />
            </div>

        </div>

        <div class="panel-body" style="padding: 0px;">
        </div>

        <table class="table datatable-basic">
            <thead>
                <tr>
                    <th>Employee Name</th>
                    <th>Work Date</th>
                    <th>In Time</th>
                    <th>Out Time</th>
                    <th>Gross Working Hours</th>
                    <th>Lunch Duration</th>
                    <th>Break Duration</th>
                    <th>Net Working Duration</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_TimemonitoringView" runat="server"></asp:PlaceHolder>

            </tbody>
        </table>
    </div>

</asp:Content>
