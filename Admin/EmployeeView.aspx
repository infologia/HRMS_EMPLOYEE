<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="EmployeeView.aspx.cs" Inherits="WEB_EmployeeView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .hm-stat-card {
            background: #fff;
            border-radius: 10px;
            border: 1px solid #eee;
            padding: 16px 18px;
            display: flex;
            align-items: center;
            gap: 14px;
            box-shadow: 0 1px 3px rgba(0,0,0,0.04);
            transition: all 0.3s ease;
        }
        .hm-stat-card:hover {
            transform: translateY(-5px);
            box-shadow: 0 8px 20px rgba(0,0,0,0.08);
        }
        .hm-stat-card:hover .hm-stat-icon i {
            animation: bounceIcon 0.5s ease;
        }
        .hm-stat-icon {
            width: 42px;
            height: 42px;
            border-radius: 8px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 18px;
            flex-shrink: 0;
        }
        .hm-stat-icon i {
            display: inline-block;
            animation: floatIcon 3s ease-in-out infinite;
        }
        @keyframes floatIcon {
            0% { transform: translateY(0px); }
            50% { transform: translateY(-3px); }
            100% { transform: translateY(0px); }
        }
        @keyframes bounceIcon {
            0%, 100% { transform: translateY(0); }
            50% { transform: translateY(-6px) scale(1.1); }
        }
        .hm-icon-blue   { background:#e8f0ff; color:#3b6fe0; }
        .hm-icon-orange { background:#fff1e6; color:#f2994a; }
        .hm-icon-green  { background:#e8f9f0; color:#27ae60; }
        .hm-icon-purple { background:#f3eaff; color:#9b59b6; }
        .hm-icon-red    { background:#fdeaea; color:#eb5757; }
        .hm-stat-label { font-size: 12px; color: #888; margin-bottom: 2px; }
        .hm-stat-value { font-size: 20px; font-weight: 600; color: #222; }

        /* Compact Clean Tabs */
        .premium-tabs {
            border-bottom: 1px solid #ddd;
            display: flex;
            gap: 4px;
            margin-bottom: 15px;
            padding: 0;
            background: transparent;
            list-style: none;
        }
        .premium-tabs > li {
            margin-bottom: -1px;
        }
        .premium-tabs > li > a {
            border: 1px solid transparent !important;
            border-radius: 4px 4px 0 0 !important;
            padding: 7px 12px;
            color: #666;
            font-weight: 600;
            font-size: 12px;
            background: #f5f5f5;
            display: flex;
            flex-direction: row;
            align-items: center;
            gap: 6px;
            text-decoration: none;
        }
        .premium-tabs > li > a > i {
            font-size: 13px;
            color: #888;
        }
        .premium-tabs > li > a:hover {
            background: #e5e5e5;
            border-color: #ddd #ddd transparent !important;
            color: #333;
        }
        .premium-tabs > li.active > a, 
        .premium-tabs > li.active > a:hover, 
        .premium-tabs > li.active > a:focus {
            background: #fff !important;
            color: #2196F3 !important;
            border-color: #ddd #ddd #fff !important;
        }
        .premium-tabs > li.active > a > i {
            color: #2196F3;
        }
        
        .tab-content {
            background: #fff;
            padding: 15px;
            border-radius: 0 0 4px 4px;
            border: 1px solid #ddd;
            border-top: none;
            margin-bottom: 15px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <!-- Dashboard Widgets -->
    <div class="row" style="margin-top: 20px;">
        <!-- Combined Employee Status Widget -->
        <div class="col-md-6 col-sm-12" style="margin-bottom:14px;">
            <div class="hm-stat-card" style="display: flex; justify-content: space-between; align-items: center;">
                
                <!-- Total Section -->
                <div style="display: flex; align-items: center; gap: 14px; flex: 1;">
                    <div class="hm-stat-icon hm-icon-blue"><i class="icon-users"></i></div>
                    <div>
                        <div class="hm-stat-label">Total Employees</div>
                        <div class="hm-stat-value">
                            <asp:Label ID="lblTotalCount" runat="server" Text="0"></asp:Label>
                        </div>
                    </div>
                </div>

                <!-- Active Section -->
                <div style="flex: 1; text-align: center; border-left: 1px solid #eee;">
                    <div class="hm-stat-label">Active</div>
                    <div class="hm-stat-value text-success">
                        <asp:Label ID="lblActiveCount" runat="server" Text="0"></asp:Label>
                    </div>
                </div>

                <!-- Inactive Section -->
                <div style="flex: 1; text-align: center; border-left: 1px solid #eee;">
                    <div class="hm-stat-label">Inactive</div>
                    <div class="hm-stat-value text-danger">
                        <asp:Label ID="lblInactiveCount" runat="server" Text="0"></asp:Label>
                    </div>
                </div>

            </div>
        </div>

        <!-- Gender Split -->
        <div class="col-md-3 col-sm-6" style="margin-bottom:14px;">
            <div class="hm-stat-card" style="display: flex; justify-content: space-between; align-items: center;">
                <div style="display: flex; align-items: center; gap: 14px; flex: 1;">
                    <div class="hm-stat-icon hm-icon-purple"><i class="icon-user"></i></div>
                    <div>
                        <div class="hm-stat-label">Male</div>
                        <div class="hm-stat-value">
                            <asp:Label ID="lblMaleCount" runat="server" Text="0"></asp:Label>
                        </div>
                    </div>
                </div>
                <div style="flex: 1; text-align: center; border-left: 1px solid #eee;">
                    <div class="hm-stat-label">Female</div>
                    <div class="hm-stat-value">
                        <asp:Label ID="lblFemaleCount" runat="server" Text="0"></asp:Label>
                    </div>
                </div>
            </div>
        </div>

        <!-- Data Health -->
        <div class="col-md-3 col-sm-6" style="margin-bottom:14px;">
            <div class="hm-stat-card">
                <div class="hm-stat-icon hm-icon-red"><i class="icon-warning2"></i></div>
                <div>
                    <div class="hm-stat-label">Incomplete Profiles</div>
                    <div class="hm-stat-value text-danger">
                        <asp:Label ID="lblIncompleteCount" runat="server" Text="0"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="panel panel-flat">
        <div class="panel-heading">
            <div class="row">
                <div class="col-md-6 pull-left">
            <h5 class="panel-title"> Employee View Details</h5>
                    <br />
                </div>
                <div class="col-md-6  pull-right">
            
                <ul class="icons-list">
                  <a href="EmployeeRegisterNew.aspx" class="btn btn-primary pull-right"> <i class="icon-user-plus"></i> Create Employee </i></a>
                </ul>
            </div>
        </div>
        </div>
            



        <div class="panel-body" style="padding: 0 20px 20px 20px;">
            <ul class="nav premium-tabs">
                <li class="active"><a href="#active-emp" data-toggle="tab"><i class="icon-user-check"></i> Active Employees</a></li>
                <li><a href="#inactive-emp" data-toggle="tab"><i class="icon-user-block"></i> Inactive Employees</a></li>
            </ul>

            <div class="tab-content">
                <!-- Active Employees Tab -->
                <div class="tab-pane active" id="active-emp">
                    <div class="table-responsive">
                        <table class="table datatable-basic">
                            <thead>
                                <tr>
                                    <th>Profile</th>
                                    <th>User ID</th>
                                    <th>Name</th>
                                    <th>Designation</th>
                                    <th>Email</th>
                                    <th>Phone Number</th>
                                    <th>Department</th>
                                    <th>Role</th>
                                    <th>Status</th>
                                    <th>Gender</th>
                                    <th class="text-center">Action</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:PlaceHolder ID="PH_ActiveEmployee" runat="server"></asp:PlaceHolder>
                            </tbody>
                        </table>
                    </div>
                </div>

                <!-- Inactive Employees Tab -->
                <div class="tab-pane" id="inactive-emp">
                    <div class="table-responsive">
                        <table class="table datatable-basic">
                            <thead>
                                <tr>
                                    <th>Profile</th>
                                    <th>User ID</th>
                                    <th>Name</th>
                                    <th>Designation</th>
                                    <th>Email</th>
                                    <th>Phone Number</th>
                                    <th>Department</th>
                                    <th>Role</th>
                                    <th>Status</th>
                                    <th>Gender</th>
                                    <th class="text-center">Action</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:PlaceHolder ID="PH_InactiveEmployee" runat="server"></asp:PlaceHolder>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <!-- /scrollable datatable -->
</asp:Content>

