<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="payslipdetails.aspx.cs" Inherits="Employee_payslipdetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .vp-header {
            background: #3B4472;
            border-radius: 6px 6px 0 0;
            padding: 18px 22px;
        }

            .vp-header .vp-eyebrow {
                font-size: 11px;
                letter-spacing: 0.06em;
                text-transform: uppercase;
                color: #8A93C4;
                margin: 0 0 4px 0;
            }

            .vp-header .panel-title {
                margin: 0 !important;
                color: #F6F5F1 !important;
                font-weight: 500;
                font-size: 19px;
            }

        .vp-table th,
        .vp-table td {
            vertical-align: middle !important;
        }

        .vp-table .btn-xs {
            font-size: 11px;
            padding: 3px 10px;
        }
    </style>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="panel panel-flat">
        <div class="panel-heading vp-header">
           <h5><Strong>My Payslips</Strong></h5> 
        </div>

        <div class="panel-body" style="padding: 0;"></div>
        <div style="overflow-x: auto;">
            <table class="table datatable-basic vp-table">
                <thead>
                    <tr>
                        <th>Month</th>
                        <th>Year</th>
                        <th>Net Pay</th>
                        <th>Generated On</th>
                        <th>View</th>
                        <th>Download</th>
                        <th>Summary</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:PlaceHolder ID="PH_payslipdetails" runat="server"></asp:PlaceHolder>
                </tbody>
            </table>
            <asp:Label ID="lbl_nodata" runat="server" Text="No payslips available." CssClass="text-muted" Style="display: block; padding: 16px; text-align: center;" Visible="false"></asp:Label>
        </div>
    </div>
</asp:Content>
