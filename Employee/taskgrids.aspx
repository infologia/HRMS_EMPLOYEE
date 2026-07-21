<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="taskgrids.aspx.cs" Inherits="Employee_taskgrids" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .dashboard-panel {
            transition: all 0.3s ease;
            border-radius: 12px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
            border: none;
            overflow: hidden;
            position: relative;
            background: linear-gradient(135deg, var(--primary-color) 0%, var(--secondary-color) 100%);
            cursor: pointer;
        }
        .dashboard-panel:hover {
            transform: translateY(-4px);
            box-shadow: 0 8px 20px rgba(0,0,0,0.15);
        }
        .dashboard-panel.active {
            transform: scale(1.03);
            box-shadow: 0 8px 24px rgba(0,0,0,0.2);
            border: 2px solid rgba(255,255,255,0.8);
        }
        .dashboard-panel .panel-body {
            padding: 24px 20px;
            position: relative;
        }
        .dashboard-panel h3 {
            font-size: 36px;
            font-weight: 700;
            margin: 0 0 8px 0;
            color: white;
        }
        .dashboard-panel span {
            font-size: 13px;
            font-weight: 600;
            text-transform: uppercase;
            letter-spacing: 0.5px;
            color: rgba(255,255,255,0.95);
        }
        .dashboard-panel .panel-icon {
            position: absolute;
            top: 20px;
            right: 20px;
            font-size: 32px;
            color: rgba(255,255,255,0.3);
        }
        
        .card-yet-to-start {
            --primary-color: #9c27b0;
            --secondary-color: #ba68c8;
        }
        .card-in-progress {
            --primary-color: #2196f3;
            --secondary-color: #64b5f6;
        }
        .card-overdue {
            --primary-color: #f44336;
            --secondary-color: #ef5350;
        }
        .card-completed {
            --primary-color: #4caf50;
            --secondary-color: #66bb6a;
        }
        
        .card-overdue {
            animation: pulse-glow 2s infinite;
        }
        @keyframes pulse-glow {
            0%, 100% { box-shadow: 0 2px 8px rgba(244,67,54,0.3); }
            50% { box-shadow: 0 4px 16px rgba(244,67,54,0.5); }
        }
        
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
        .employee-filter-wrap {
            display: inline-flex;
            align-items: center;
            gap: 8px;
            padding-right: 20px;
            border-right: 2px solid #e5e7eb;
        }
        .employee-dropdown {
            width: 200px;
            height: 30px;
            padding: 2px 6px;
            font-size: 13px;
        }
        .date-label, .year-label {
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
        
        @media (max-width: 767px) {
            .panel-heading { flex-direction: column; align-items: flex-start; }
            .date-filter { flex-wrap: wrap; justify-content: flex-end; }
            .date-filter label { width: 100%; text-align: left; margin: 4px 0; }
            .dashboard-panel h3 { font-size: 28px; }
            .dashboard-panel .panel-body { padding: 18px 16px; }
            .dashboard-panel .panel-icon { font-size: 24px; }
        }
    </style>
    <script type="text/javascript">
        function confirmDelete(taskKey) {
            if (confirm('Are you sure you want to remove this task?')) {
                window.location.href = 'taskgrids.aspx?action=delete&taskkey=' + taskKey;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:HiddenField ID="hfActiveStatus" runat="server" Value="1" />
    
    <!-- Project Header -->
    <div class="panel panel-flat" style="margin-bottom: 20px;">
        <div class="panel-body" style="padding: 20px; position: relative;">
            <h2 style="margin: 0; color: #333; font-weight: 600; padding-right: 200px;">
                <i class="glyphicon glyphicon-folder-open" style="margin-right: 10px; color: #2196f3;"></i>
                <asp:Label ID="lbl_ProjectHeader" runat="server" Text="All Projects"></asp:Label>
            </h2>
            <div style="position: absolute; top: 20px; right: 20px; display: flex; gap: 10px;">
                <asp:HyperLink ID="lnk_BackButton" runat="server" NavigateUrl="assignedproject.aspx" 
                    CssClass="btn btn-default" 
                    style="padding: 10px 16px; border-radius: 6px; text-decoration: none; font-weight: 600;">
                    <i class="glyphicon glyphicon-arrow-left" style="margin-right: 6px;"></i>Back
                </asp:HyperLink>
                <asp:HyperLink ID="lnk_CreateTask" runat="server" NavigateUrl="CreateTask.aspx" 
                    CssClass="btn btn-primary" 
                    style="padding: 10px 20px; border-radius: 6px; text-decoration: none; font-weight: 600;">
                    <i class="glyphicon glyphicon-plus" style="margin-right: 8px;"></i>Create Task
                </asp:HyperLink>
            </div>
        </div>
    </div>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="row">
                <div class="col-lg-3 col-md-6">
                    <asp:LinkButton ID="btnCard1" runat="server" OnClick="CardClick" CommandArgument="1" style="text-decoration: none;">
                        <div class="panel dashboard-panel card-yet-to-start <%= hfActiveStatus.Value == "1" ? "active" : "" %>">
                            <div class="panel-body">
                                <i class="glyphicon glyphicon-time panel-icon"></i>
                                <h3 class="no-margin text-white"><asp:Label ID="lbl_YetToStartCount" runat="server">0</asp:Label></h3>
                                <span class="text-white">Yet to Start</span>
                            </div>
                        </div>
                    </asp:LinkButton>
                </div>

                <div class="col-lg-3 col-md-6">
                    <asp:LinkButton ID="btnCard2" runat="server" OnClick="CardClick" CommandArgument="2" style="text-decoration: none;">
                        <div class="panel dashboard-panel card-in-progress <%= hfActiveStatus.Value == "2" ? "active" : "" %>">
                            <div class="panel-body">
                                <i class="glyphicon glyphicon-refresh panel-icon"></i>
                                <h3 class="no-margin text-white"><asp:Label ID="lbl_InProgressCount" runat="server">0</asp:Label></h3>
                                <span class="text-white">In Progress</span>
                            </div>
                        </div>
                    </asp:LinkButton>
                </div>

                <div class="col-lg-3 col-md-6">
                    <asp:LinkButton ID="btnCard3" runat="server" OnClick="CardClick" CommandArgument="3" style="text-decoration: none;">
                        <div class="panel dashboard-panel card-overdue <%= hfActiveStatus.Value == "3" ? "active" : "" %>">
                            <div class="panel-body">
                                <i class="glyphicon glyphicon-warning-sign panel-icon"></i>
                                <h3 class="no-margin text-white"><asp:Label ID="lbl_OverDueCount" runat="server">0</asp:Label></h3>
                                <span class="text-white">Overdue</span>
                            </div>
                        </div>
                    </asp:LinkButton>
                </div>

                <div class="col-lg-3 col-md-6">
                    <asp:LinkButton ID="btnCard4" runat="server" OnClick="CardClick" CommandArgument="4" style="text-decoration: none;">
                        <div class="panel dashboard-panel card-completed <%= hfActiveStatus.Value == "4" ? "active" : "" %>">
                            <div class="panel-body">
                                <i class="glyphicon glyphicon-ok-circle panel-icon"></i>
                                <h3 class="no-margin text-white"><asp:Label ID="lbl_CompletedCount" runat="server">0</asp:Label></h3>
                                <span class="text-white">Completed</span>
                            </div>
                        </div>
                    </asp:LinkButton>
                </div>
            </div>

            <div class="panel panel-flat">
                <div class="panel-heading">
                    <h5 class="panel-title">
                        <%= GetStatusTitle() %>
                    </h5>
                    <div class="date-filter">
                        <div id="divEmployeeFilter" runat="server" visible="false" class="employee-filter-wrap">
                            <label class="date-label">Employee:</label>
                            <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-control employee-dropdown" AutoPostBack="true" OnSelectedIndexChanged="Filter_Changed"></asp:DropDownList>
                        </div>
                        <label class="date-label">Month :</label>
                        <asp:DropDownList ID="ddlDate" runat="server" CssClass="form-control date-dropdown" AutoPostBack="true" OnSelectedIndexChanged="Filter_Changed"></asp:DropDownList>
                        <label class="year-label">Year :</label>
                        <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control date-dropdown" AutoPostBack="true" OnSelectedIndexChanged="Filter_Changed"></asp:DropDownList>
                    </div>
                </div>

                <div class="table-responsive">
                    <table class="table datatable-basic">
                        <thead>
                            <tr>
                                <th>Task Name</th>
                                <th>Assigned to</th>
                                <th>Start Date</th>
                                <th>End Date</th>
                                <th>Assigned Hours</th>
                                <th>Overtime Hours</th>
                                <th>Status</th>
                                <th>Update</th>
                                <th>Remove</th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:PlaceHolder ID="PH_Tasks" runat="server"></asp:PlaceHolder>
                        </tbody>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
