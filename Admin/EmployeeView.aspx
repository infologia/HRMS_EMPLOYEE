<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="EmployeeView.aspx.cs" Inherits="WEB_EmployeeView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        /* Ultra Cute & Compact Mini Stats */
        .mini-stat-card {
            background: #fff;
            border-radius: 14px;
            padding: 12px 10px;
            display: flex;
            align-items: center;
            gap: 12px;
            box-shadow: 0 4px 15px rgba(0,0,0,0.02);
            transition: all 0.3s cubic-bezier(0.25, 0.8, 0.25, 1);
            border: 1px solid #f4f5f7;
            position: relative;
            overflow: hidden;
        }
        .mini-stat-card:hover {
            transform: translateY(-3px) scale(1.02);
            box-shadow: 0 10px 25px rgba(0,0,0,0.06);
            border-color: #e2e8f0;
        }
        .mini-stat-icon {
            width: 40px;
            height: 40px;
            border-radius: 12px;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 18px;
            color: #fff;
            flex-shrink: 0;
            box-shadow: 0 4px 10px rgba(0,0,0,0.1);
        }
        .mini-stat-icon i {
            transition: transform 0.3s ease;
        }
        .mini-stat-card:hover .mini-stat-icon i {
            transform: scale(1.2) rotate(5deg);
        }
        .mini-stat-info {
            display: flex;
            flex-direction: column;
            justify-content: center;
        }
        .mini-stat-value {
            font-size: 18px;
            font-weight: 800;
            color: #2d3748;
            line-height: 1;
            margin-bottom: 3px;
        }
        .mini-stat-label {
            font-size: 10px;
            color: #a0aec0;
            font-weight: 700;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }
        
        .bg-gradient-blue { background: linear-gradient(135deg, #4299e1, #3182ce); box-shadow: 0 4px 10px rgba(66, 153, 225, 0.3); }
        .bg-gradient-green { background: linear-gradient(135deg, #48bb78, #38a169); box-shadow: 0 4px 10px rgba(72, 187, 120, 0.3); }
        .bg-gradient-red { background: linear-gradient(135deg, #f56565, #e53e3e); box-shadow: 0 4px 10px rgba(245, 101, 101, 0.3); }
        .bg-gradient-purple { background: linear-gradient(135deg, #9f7aea, #805ad5); box-shadow: 0 4px 10px rgba(159, 122, 234, 0.3); }
        .bg-gradient-pink { background: linear-gradient(135deg, #ed64a6, #d53f8c); box-shadow: 0 4px 10px rgba(237, 100, 166, 0.3); }
        .bg-gradient-orange { background: linear-gradient(135deg, #ed8936, #dd6b20); box-shadow: 0 4px 10px rgba(237, 137, 54, 0.3); }

        /* Compact Clean Tabs */
        .premium-tabs {
            border-bottom: 2px solid #f1f3f5;
            display: flex;
            gap: 8px;
            margin-bottom: 12px;
            padding: 0;
            background: transparent;
            list-style: none;
        }
        .premium-tabs > li { margin-bottom: -2px; }
        .premium-tabs > li > a {
            border: none !important;
            padding: 8px 16px;
            color: #718096;
            font-weight: 600;
            font-size: 12px;
            background: transparent;
            display: flex;
            align-items: center;
            gap: 6px;
            text-decoration: none;
            border-bottom: 2px solid transparent !important;
            transition: all 0.2s ease;
        }
        .premium-tabs > li > a > i { font-size: 14px; opacity: 0.7; }
        .premium-tabs > li > a:hover {
            color: #2b6cb0;
            background: #f7fafc;
            border-radius: 6px 6px 0 0;
        }
        .premium-tabs > li.active > a {
            color: #2b6cb0 !important;
            background: transparent !important;
            border-bottom: 2px solid #2b6cb0 !important;
        }
        
        .tab-content {
            background: #fff;
            padding: 0;
            border: none;
        }
        
        /* Premium Table Design */
        .premium-panel {
            border-radius: 12px;
            box-shadow: 0 4px 20px rgba(0,0,0,0.04);
            border: none;
            overflow: hidden;
            background: #fff;
        }
        .premium-header {
            padding: 16px 20px;
            border-bottom: 1px solid #f0f0f0;
            background: #fff;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }
        .premium-header .panel-title {
            font-weight: 700;
            color: #2d3748;
            font-size: 15px;
            margin: 0;
        }
        .btn-create-premium {
            background: linear-gradient(135deg, #4299e1, #3182ce);
            color: #fff !important;
            border: none;
            border-radius: 20px;
            padding: 6px 16px;
            font-size: 12px;
            font-weight: 600;
            box-shadow: 0 4px 10px rgba(66, 153, 225, 0.3);
            transition: all 0.3s ease;
        }
        .btn-create-premium:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 15px rgba(66, 153, 225, 0.4);
            background: linear-gradient(135deg, #3182ce, #2b6cb0);
        }
        
        .table.datatable-basic {
            font-size: 12.5px;
            color: #4a5568;
            border-collapse: separate;
            border-spacing: 0;
        }
        .table.datatable-basic thead th {
            background-color: #f8fafc;
            color: #4a5568;
            font-weight: 600;
            border-bottom: 2px solid #e2e8f0;
            padding: 10px 12px;
            text-transform: uppercase;
            font-size: 11px;
            letter-spacing: 0.5px;
            position: relative;
        }
        
        /* Hide DataTables Sort Icons */
        .table.datatable-basic thead th.sorting, 
        .table.datatable-basic thead th.sorting_asc, 
        .table.datatable-basic thead th.sorting_desc {
            background-image: none !important;
            padding-right: 12px !important; /* Reset padding since there's no icon */
        }
        
        .table.datatable-basic thead th.sorting::before,
        .table.datatable-basic thead th.sorting::after,
        .table.datatable-basic thead th.sorting_asc::before,
        .table.datatable-basic thead th.sorting_asc::after,
        .table.datatable-basic thead th.sorting_desc::before,
        .table.datatable-basic thead th.sorting_desc::after {
            display: none !important;
            content: none !important;
        }
        .table.datatable-basic tbody td {
            padding: 8px 12px;
            border-bottom: 1px solid #edf2f7;
            vertical-align: middle;
        }
        .table.datatable-basic tbody tr {
            transition: background 0.2s;
        }
        .table.datatable-basic tbody tr:hover {
            background-color: #f7fafc;
        }
        .img-circle {
            box-shadow: 0 2px 5px rgba(0,0,0,0.1);
            border: 2px solid #fff;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <!-- Cute Mini Dashboard Widgets -->
    <div class="row" style="margin-top: 20px; margin-bottom: 5px;">
        
        <div class="col-md-2 col-sm-4 col-xs-6" style="margin-bottom:14px;">
            <div class="mini-stat-card">
                <div class="mini-stat-icon bg-gradient-blue"><i class="icon-users"></i></div>
                <div class="mini-stat-info">
                    <div class="mini-stat-value"><asp:Label ID="lblTotalCount" runat="server" Text="0"></asp:Label></div>
                    <div class="mini-stat-label">Total</div>
                </div>
            </div>
        </div>

        <div class="col-md-2 col-sm-4 col-xs-6" style="margin-bottom:14px;">
            <div class="mini-stat-card">
                <div class="mini-stat-icon bg-gradient-green"><i class="icon-user-check"></i></div>
                <div class="mini-stat-info">
                    <div class="mini-stat-value text-success"><asp:Label ID="lblActiveCount" runat="server" Text="0"></asp:Label></div>
                    <div class="mini-stat-label">Active</div>
                </div>
            </div>
        </div>

        <div class="col-md-2 col-sm-4 col-xs-6" style="margin-bottom:14px;">
            <div class="mini-stat-card">
                <div class="mini-stat-icon bg-gradient-red"><i class="icon-user-block"></i></div>
                <div class="mini-stat-info">
                    <div class="mini-stat-value text-danger"><asp:Label ID="lblInactiveCount" runat="server" Text="0"></asp:Label></div>
                    <div class="mini-stat-label">Inactive</div>
                </div>
            </div>
        </div>

        <div class="col-md-2 col-sm-4 col-xs-6" style="margin-bottom:14px;">
            <div class="mini-stat-card">
                <div class="mini-stat-icon bg-gradient-purple"><i class="icon-man"></i></div>
                <div class="mini-stat-info">
                    <div class="mini-stat-value"><asp:Label ID="lblMaleCount" runat="server" Text="0"></asp:Label></div>
                    <div class="mini-stat-label">Male</div>
                </div>
            </div>
        </div>

        <div class="col-md-2 col-sm-4 col-xs-6" style="margin-bottom:14px;">
            <div class="mini-stat-card">
                <div class="mini-stat-icon bg-gradient-pink"><i class="icon-woman"></i></div>
                <div class="mini-stat-info">
                    <div class="mini-stat-value"><asp:Label ID="lblFemaleCount" runat="server" Text="0"></asp:Label></div>
                    <div class="mini-stat-label">Female</div>
                </div>
            </div>
        </div>

        <div class="col-md-2 col-sm-4 col-xs-6" style="margin-bottom:14px;">
            <div class="mini-stat-card">
                <div class="mini-stat-icon bg-gradient-orange"><i class="icon-warning2"></i></div>
                <div class="mini-stat-info">
                    <div class="mini-stat-value text-warning"><asp:Label ID="lblIncompleteCount" runat="server" Text="0"></asp:Label></div>
                    <div class="mini-stat-label">Incomplete</div>
                </div>
            </div>
        </div>
        
    </div>

    <div class="premium-panel">
        <div class="premium-header">
            <h5 class="panel-title"><i class="icon-users position-left text-primary"></i> Employee Directory</h5>
            <div>
                <a href="EmployeeRegisterNew.aspx" class="btn-create-premium"> <i class="icon-user-plus position-left"></i> Create Employee </a>
            </div>
        </div>

        <div class="panel-body" style="padding: 15px 20px;">
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
                                    <th>Email</th>
                                    <th style="min-width: 150px;">Phone Number</th>
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
                                    <th>Email</th>
                                    <th style="min-width: 150px;">Phone Number</th>
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

