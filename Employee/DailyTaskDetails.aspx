<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="DailyTaskDetails.aspx.cs" Inherits="Employee_DailyTaskDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .project-card {
            margin-bottom: 20px;
            border: 1px solid #ddd;
            border-radius: 5px;
            box-shadow: 0 1px 3px rgba(0,0,0,0.1);
        }
        .project-header {
            background-color: #f5f5f5;
            padding: 10px 15px;
            border-bottom: 1px solid #ddd;
            font-weight: bold;
            font-size: 12px;
        }
        .task-table {
            margin-bottom: 0;
            font-size: 10px;
        }
        .task-table th {
            white-space: nowrap;
        }
        .status-badge {
            padding: 3px 8px;
            border-radius: 3px;
            color: white;
            font-size: 7px;
        }
        .bg-purple { background-color: #9c27b0; }
        .bg-blue { background-color: #2196f3; }
        .bg-green { background-color: #4caf50; }
        .bg-red { background-color: #f44336; }
        .table > tbody > tr > td, .table > tbody > tr > th,
        .table > tfoot > tr > td, .table > tfoot > tr > th,
        .table > thead > tr > td, .table > thead > tr > th {
            padding: 5px 14px !important;
            line-height: 1.5384616 !important;
            vertical-align: top !important;
            border-top: 1px solid #ddd !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel panel-flat">
        <div class="panel-heading">
            <h5 class="panel-title">Daily Task</h5>
            <div class="heading-elements">
                <a href="Overalltaskgrid.aspx" class="btn btn-default"><i class="glyphicon glyphicon-arrow-left"></i> Back to Dashboard</a>
            </div>
        </div>
        <div class="panel-body">
            <div class="row" style="margin-bottom: 20px;">
                <div class="col-md-4">
                    <strong>Date:</strong> <asp:Label ID="lblDate" runat="server" CssClass="text-primary"></asp:Label>
                </div>
                <div class="col-md-4">
                    <strong>Employee:</strong> <asp:Label ID="lblEmployeeName" runat="server" CssClass="text-primary"></asp:Label>
                </div>
                <div class="col-md-4 text-right">
                    <strong>Total Assigned:</strong> <asp:Label ID="lblTotalHours" runat="server" CssClass="text-primary"></asp:Label> hrs
                    <span style="margin-left: 15px;"><strong>Total Actual:</strong> <asp:Label ID="lblTotalActualHours" runat="server" CssClass="text-primary"></asp:Label> hrs</span>
                </div>
            </div>
            <asp:PlaceHolder ID="phProjects" runat="server"></asp:PlaceHolder>
            
            <h5 class="panel-title" style="margin-top: 20px; margin-bottom: 15px;">Meetings</h5>
            <asp:PlaceHolder ID="phMeetings" runat="server"></asp:PlaceHolder>
        </div>
    </div>
</asp:Content>
