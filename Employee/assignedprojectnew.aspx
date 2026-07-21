<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="assignedprojectnew.aspx.cs" Inherits="Employee_Taskdashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<style>
/* Container */
body { background: #f8f9fa; }

/* Project Card */
.project-card {
    border: none;
    border-left: 5px solid #2196f3;
    border-radius: 12px;
    background: #fff;
    transition: all 0.3s cubic-bezier(0.4, 0, 0.2, 1);
    animation: fadeInUp 0.6s ease forwards;
    opacity: 0;
    margin-bottom: 24px;
    box-shadow: 0 4px 12px rgba(0,0,0,0.08), 0 1px 3px rgba(0,0,0,0.05);
    overflow: hidden;
    position: relative;
}
.project-card::before {
    content: '';
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    height: 2px;
    background: linear-gradient(90deg, transparent, rgba(33,150,243,0.3), transparent);
    opacity: 0;
    transition: opacity 0.3s;
}
.project-card:hover::before { opacity: 1; }
.project-card:hover {
    transform: translateY(-6px) scale(1.01);
    box-shadow: 0 12px 28px rgba(0,0,0,0.12), 0 4px 8px rgba(0,0,0,0.08);
}
.border-completed { border-left-color: #4caf50; }
.border-overdue { border-left-color: #f44336; }

/* Header */
.pc-heading {
    padding: 18px 20px 14px;
    border-bottom: 1px solid #f0f2f5;
    display: flex;
    align-items: flex-start;
    justify-content: space-between;
    gap: 12px;
    background: linear-gradient(to bottom, #ffffff, #fafbfc);
}
.project-title {
    font-size: 16px;
    font-weight: 700;
    color: #1a237e;
    margin: 0 0 8px 0;
    line-height: 1.3;
    display: -webkit-box;
    -webkit-line-clamp: 2;
    -webkit-box-orient: vertical;
    overflow: hidden;
    text-overflow: ellipsis;
}
.status-indicator {
    display: flex;
    width: 100%;
    align-items: center;
    gap: 8px;
    flex-wrap: nowrap;
}
.workflow-badge {
    margin-left: auto;
    background: linear-gradient(135deg, #f0f9ff, #e0f2fe);
    color: #0284c7;
    padding: 4px 10px;
    border-radius: 20px;
    font-size: 10px;
    font-weight: 700;
    text-transform: uppercase;
    text-decoration: none !important;
    border: 1px solid #bae6fd;
    display: inline-flex;
    align-items: center;
    gap: 4px;
    transition: all 0.2s ease;
    white-space: nowrap;
    flex-shrink: 0;
}
.workflow-badge:hover {
    background: linear-gradient(135deg, #e0f2fe, #bae6fd);
    transform: translateY(-1px);
    box-shadow: 0 2px 4px rgba(2, 132, 199, 0.15);
    color: #0369a1;
}
.overdue-dot, .ongoing-dot, .completed-dot {
    width: 10px;
    height: 10px;
    border-radius: 50%;
    flex-shrink: 0;
    box-shadow: 0 0 0 3px rgba(244,67,54,0.15);
}
.overdue-dot { background: linear-gradient(135deg, #f44336, #e91e63); animation: pulse 2s infinite; }
.ongoing-dot { background: linear-gradient(135deg, #2196f3, #03a9f4); box-shadow: 0 0 0 3px rgba(33,150,243,0.15); }
.completed-dot { background: linear-gradient(135deg, #4caf50, #66bb6a); box-shadow: 0 0 0 3px rgba(76,175,80,0.15); }
.overdue-text { color: #f44336; font-size: 11px; font-weight: 700; text-transform: uppercase; letter-spacing: 0.5px; }
.ongoing-text { color: #2196f3; font-size: 11px; font-weight: 700; text-transform: uppercase; letter-spacing: 0.5px; }
.completed-text { color: #4caf50; font-size: 11px; font-weight: 700; text-transform: uppercase; letter-spacing: 0.5px; }
.pc-dates {
    font-size: 11px;
    color: #90a4ae;
    display: inline-flex;
    gap: 6px;
    align-items: center;
    font-weight: 500;
}
.pc-dates i { font-size: 12px; }

/* Warning Badge */
.hours-warning {
    display: inline-flex;
    align-items: center;
    gap: 5px;
    padding: 4px 10px;
    background: linear-gradient(135deg, #fff3e0, #ffe0b2);
    border: 1px solid #ffb74d;
    border-radius: 6px;
    font-size: 10px;
    font-weight: 700;
    color: #e65100;
    animation: warningPulse 2s infinite;
}
.hours-warning i {
    font-size: 12px;
}
@keyframes warningPulse {
    0%, 100% { box-shadow: 0 0 0 0 rgba(255, 183, 77, 0.4); }
    50% { box-shadow: 0 0 0 4px rgba(255, 183, 77, 0); }
}

/* Button */
.btn-view-project {
    padding: 8px 14px !important;
    border-radius: 8px !important;
    border: 1px solid #e0e0e0 !important;
    background: #fff !important;
    transition: all 0.3s !important;
    font-size: 13px !important;
    color: #546e7a !important;
}
.btn-view-project:hover {
    background: #2196f3 !important;
    color: #fff !important;
    border-color: #2196f3 !important;
    transform: translateX(3px);
}

/* Stats Strip */
.pc-stats-strip {
    display: flex;
    border-bottom: 1px solid #f0f2f5;
    background: #fafbfc;
}
.pc-stat {
    flex: 1;
    padding: 14px 10px;
    text-align: center;
    border-right: 1px solid #eceff1;
    transition: background 0.3s;
    position: relative;
}
.pc-stat:hover { background: #f5f7fa; }
.pc-stat:last-child { border-right: none; }
.pc-stat::after {
    content: '';
    position: absolute;
    bottom: 0;
    left: 50%;
    transform: translateX(-50%);
    width: 0;
    height: 2px;
    background: currentColor;
    transition: width 0.3s;
}
.pc-stat:hover::after { width: 60%; }
.pc-stat-label {
    font-size: 10px;
    text-transform: uppercase;
    color: #78909c;
    font-weight: 700;
    letter-spacing: 0.5px;
    margin-bottom: 6px;
    display: block;
}
.pc-stat-val {
    line-height: 1;
    display: flex;
    align-items: baseline;
    justify-content: center;
    gap: 2px;
}
.pc-stat-val small {
    font-size: 11px;
    font-weight: 600;
    opacity: 0.7;
}
.val-blue { color: #1976d2; }
.val-teal { color: #00897b; }
.val-purple { color: #7b1fa2; }
.val-orange { color: #f57c00; }

/* Task sections */
.pc-body {
    padding: 18px 20px 20px;
}
.pc-section-label {
    font-size: 11px;
    font-weight: 700;
    text-transform: uppercase;
    letter-spacing: 0.8px;
    color: #607d8b;
    margin-bottom: 12px;
    display: flex;
    align-items: center;
    justify-content: space-between;
    gap: 8px;
    padding-bottom: 8px;
    border-bottom: 2px solid #eceff1;
}
.pc-section-label i { font-size: 14px; color: #2196f3; }
.assigned-badge {
    background: linear-gradient(135deg, #81d4fa, #4fc3f7);
    color: #01579b;
    font-size: 10px;
    font-weight: 700;
    padding: 4px 10px;
    border-radius: 12px;
    box-shadow: 0 2px 6px rgba(129,212,250,0.3);
}
.task-grid {
    display: grid;
    grid-template-columns: repeat(5, 1fr);
    gap: 10px;
    margin-bottom: 12px;
}
.task-box {
    border-radius: 10px;
    padding: 14px 8px 12px;
    text-align: center;
    transition: all 0.3s;
    cursor: pointer;
    border: 2px solid transparent;
    position: relative;
}
.task-box:hover {
    transform: translateY(-3px);
    box-shadow: 0 6px 16px rgba(0,0,0,0.1);
}
.task-box .t-count {
    line-height: 1;
    display: block;
    margin-bottom: 6px;
}
.task-box .t-label {
    font-size: 9px;
    font-weight: 700;
    text-transform: uppercase;
    letter-spacing: 0.5px;
}
.task-box .t-subcount {
    position: absolute;
    top: 8px;
    right: 8px;
    background: rgba(0,0,0,0.6);
    color: #fff;
    font-size: 9px;
    font-weight: 700;
    padding: 3px 6px;
    border-radius: 8px;
    min-width: 20px;
}
.tb-overall { background: linear-gradient(135deg, #eceff1, #f5f5f5); border-color: #cfd8dc; }
.tb-overall:hover { background: linear-gradient(135deg, #cfd8dc, #eceff1); }
.tb-overall .t-count { color: #37474f; }
.tb-overall .t-label { color: #546e7a; }
.tb-inprogress { background: linear-gradient(135deg, #e3f2fd, #bbdefb); border-color: #90caf9; }
.tb-inprogress:hover { background: linear-gradient(135deg, #bbdefb, #e3f2fd); }
.tb-inprogress .t-count { color: #0d47a1; }
.tb-inprogress .t-label { color: #1565c0; }
.tb-completed { background: linear-gradient(135deg, #e8f5e9, #c8e6c9); border-color: #a5d6a7; }
.tb-completed:hover { background: linear-gradient(135deg, #c8e6c9, #e8f5e9); }
.tb-completed .t-count { color: #1b5e20; }
.tb-completed .t-label { color: #2e7d32; }
.tb-overdue { background: linear-gradient(135deg, #ffebee, #ffcdd2); border-color: #ef9a9a; }
.tb-overdue:hover { background: linear-gradient(135deg, #ffcdd2, #ffebee); }
.tb-overdue .t-count { color: #b71c1c; }
.tb-overdue .t-label { color: #c62828; }
.tb-assigned { background: linear-gradient(135deg, #e1f5fe, #b3e5fc); border-color: #81d4fa; }
.tb-assigned:hover { background: linear-gradient(135deg, #b3e5fc, #e1f5fe); }
.tb-assigned .t-count { color: #01579b; }
.tb-assigned .t-label { color: #0277bd; }

.pc-section-divider {
    border: none;
    border-top: 2px dashed #e0e0e0;
    margin: 16px 0;
}

/* Section headers */
.page-section-header {
    display: flex;
    align-items: center;
    gap: 16px;
    margin: 32px 0 20px;
    padding-left: 4px;
}
.page-section-header h3 {
    font-size: 18px;
    font-weight: 800;
    color: #263238;
    margin: 0;
    white-space: nowrap;
    text-transform: uppercase;
    letter-spacing: 1px;
    position: relative;
    padding-bottom: 4px;
}
.page-section-header h3::after {
    content: '';
    position: absolute;
    bottom: 0;
    left: 0;
    width: 40px;
    height: 3px;
    background: linear-gradient(90deg, #2196f3, #03a9f4);
    border-radius: 2px;
}
.page-section-header .line {
    flex: 1;
    height: 2px;
    background: linear-gradient(90deg, #e0e0e0, transparent);
}

/* Animations */
@keyframes pulse {
    0%, 100% { transform: scale(1); opacity: 1; }
    50% { transform: scale(1.4); opacity: 0.5; }
}
@keyframes fadeInUp {
    from { opacity: 0; transform: translateY(30px); }
    to { opacity: 1; transform: translateY(0); }
}
.col-lg-6:nth-child(1) .project-card { animation-delay: 0.1s; }
.col-lg-6:nth-child(2) .project-card { animation-delay: 0.2s; }
.col-lg-6:nth-child(3) .project-card { animation-delay: 0.3s; }
.col-lg-6:nth-child(4) .project-card { animation-delay: 0.4s; }
.col-lg-6:nth-child(5) .project-card { animation-delay: 0.5s; }
.col-lg-6:nth-child(6) .project-card { animation-delay: 0.6s; }

/* Responsive */
@media (max-width: 768px) {
    .task-grid { grid-template-columns: repeat(3, 1fr); gap: 8px; }
    .pc-stats-strip { flex-wrap: wrap; }
    .pc-stat { flex: 1 1 50%; border-right: none; border-bottom: 1px solid #eceff1; }
    .pc-stat:nth-child(2n) { border-right: 1px solid #eceff1; }
    .project-title { font-size: 15px; }
    .status-indicator { font-size: 10px; }
    .pc-dates { font-size: 10px; }
}
@media (max-width: 480px) {
    .task-grid { grid-template-columns: 1fr; }
    .pc-heading { padding: 14px 16px; }
    .pc-body { padding: 14px 16px; }
}
</style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <!-- ONGOING -->
    <div class="page-section-header">
        <h3>Ongoing</h3>
        <div class="line"></div>
    </div>

    <asp:Repeater ID="rptOngoing" runat="server" OnItemDataBound="rptOngoing_ItemDataBound">
        <HeaderTemplate><div class="row"></HeaderTemplate>
        <ItemTemplate>
            <div class="col-lg-6 col-md-12">
                <div id="divProjectCard" runat="server" class="project-card">

                    <!-- Header -->
                    <div class="pc-heading">
                        <div style="min-width:0;flex:1;">
                            <div class="project-title"><asp:Label ID="lbl_ProjectName" runat="server"></asp:Label></div>
                            <div style="display:flex;align-items:center;gap:12px;flex-wrap:wrap;margin-bottom:6px;">
                                <div style="font-size:11px;color:#78909c;font-weight:600;">
                                    <i class="ti ti-folder"></i> <asp:Label ID="lbl_ProjectType" runat="server"></asp:Label>
                                </div>
                                <asp:Panel ID="pnl_HoursWarning" runat="server" Visible="false">
                                    <div class="hours-warning">
                                        <i class="ti ti-alert-triangle"></i>
                                        <span>Hours Exceeded</span>
                                    </div>
                                </asp:Panel>
                            </div>
                            <asp:Panel ID="pnl_OverdueIndicator" runat="server" Visible="false" style="width:100%;">
                                <div class="status-indicator">
                                    <div class="overdue-dot"></div>
                                    <span class="overdue-text">Overdue</span>
                                    <span class="pc-dates" style="margin-top:0;">
                                        <i class="ti ti-calendar"></i>&nbsp;<asp:Label ID="lbl_StartDate" runat="server" />
                                        &nbsp;-&nbsp;<asp:Label ID="lbl_EndDate" runat="server" />
                                    </span>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnl_OngoingIndicator" runat="server" Visible="false">
                                <div class="status-indicator">
                                    <div class="ongoing-dot"></div>
                                    <span class="ongoing-text">Ongoing</span>
                                    <span class="pc-dates" style="margin-top:0;">
                                        <i class="ti ti-calendar"></i>&nbsp;<asp:Label ID="lbl_StartDate2" runat="server" />
                                        &nbsp;-&nbsp;<asp:Label ID="lbl_EndDate2" runat="server" />
                                    </span>
                                </div>
                            </asp:Panel>
                        </div>
                        <div style="display:flex; flex-direction:column; gap:8px; align-items:flex-end; flex-shrink:0;">
                            <asp:HyperLink ID="lnk_Project" runat="server" CssClass="btn-view-project">
                                <i class="glyphicon glyphicon-eye-open"></i> View
                            </asp:HyperLink>
                            <asp:HyperLink ID="lnk_Workflow" runat="server" Visible="false" Target="_blank" CssClass="workflow-badge" style="margin-left:0;">
                                <i class="ti ti-file-text"></i> View Workflow
                            </asp:HyperLink>
                        </div>
                    </div>

                    <!-- Stats Strip -->
                    <div class="pc-stats-strip">
                        <div class="pc-stat">
                            <div class="pc-stat-label">Est. Hours</div>
                            <div class="pc-stat-val val-blue"><asp:Label ID="lbl_EstimatedHours" runat="server"></asp:Label><small style="font-size:10px;font-weight:500;">h</small></div>
                        </div>
                        <div class="pc-stat">
                            <div class="pc-stat-label">Overall Used</div>
                            <div class="pc-stat-val val-teal"><asp:Label ID="lbl_UsedHours" runat="server"></asp:Label><small style="font-size:10px;font-weight:500;">h</small></div>
                        </div>
                        <div class="pc-stat">
                            <div class="pc-stat-label">My Used</div>
                            <div class="pc-stat-val val-purple"><asp:Label ID="lbl_myhours" runat="server"></asp:Label><small style="font-size:10px;font-weight:500;">h</small></div>
                        </div>
                    </div>

                    <!-- Body -->
                    <div class="pc-body">
                        <div class="pc-section-label">
                            <div style="display:flex;align-items:center;gap:8px;">
                                <i class="ti ti-list-details"></i> All Task Details
                            </div>
                            <span class="assigned-badge">Assigned Employees: <asp:Label ID="lbl_AllAssignedEmpCount" runat="server">0</asp:Label></span>
                        </div>
                        <div class="task-grid">
                            <div class="task-box tb-overall">
                                <span class="t-count"><asp:Label ID="lbl_AllOverall" runat="server">–</asp:Label></span>
                                <div class="t-label">Overall</div>
                            </div>
                            <div class="task-box tb-assigned">
                                <span class="t-count"><asp:Label ID="lbl_AllAssignedCount" runat="server">0</asp:Label></span>
                                <div class="t-label">Assigned</div>
                            </div>
                            <div class="task-box tb-inprogress">
                                <span class="t-count"><asp:Label ID="lbl_InProgress" runat="server"></asp:Label></span>
                                <div class="t-label">Ongoing</div>
                            </div>
                            <div class="task-box tb-overdue">
                                <span class="t-count"><asp:Label ID="lbl_Pending" runat="server"></asp:Label></span>
                                <div class="t-label">Overdue</div>
                            </div>
                            <div class="task-box tb-completed">
                                <span class="t-count"><asp:Label ID="lbl_Completed" runat="server"></asp:Label></span>
                                <div class="t-label">Completed</div>
                            </div>
                        </div>

                        <hr class="pc-section-divider" />

                        <div class="pc-section-label">
                            <div style="display:flex;align-items:center;gap:8px;">
                                <i class="ti ti-user"></i> My Task Details
                            </div>
                        </div>
                        <div class="task-grid">
                            <div class="task-box tb-overall">
                                <span class="t-count"><asp:Label ID="lbl_MyOverall" runat="server">–</asp:Label></span>
                                <div class="t-label">Overall</div>
                            </div>
                            <div class="task-box tb-assigned">
                                <span class="t-count"><asp:Label ID="lbl_MyAssignedCount" runat="server">0</asp:Label></span>
                                <div class="t-label">Assigned</div>
                            </div>
                            <div class="task-box tb-inprogress">
                                <span class="t-count"><asp:Label ID="lbl_MyOngoing" runat="server">–</asp:Label></span>
                                <div class="t-label">Ongoing</div>
                            </div>
                            <div class="task-box tb-overdue">
                                <span class="t-count"><asp:Label ID="lbl_MyOverdue" runat="server">–</asp:Label></span>
                                <div class="t-label">Overdue</div>
                            </div>
                            <div class="task-box tb-completed">
                                <span class="t-count"><asp:Label ID="lbl_MyCompleted" runat="server">–</asp:Label></span>
                                <div class="t-label">Completed</div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
            <%# (Container.ItemIndex % 2 == 1) ? "</div><div class='row'>" : "" %>
        </ItemTemplate>
        <FooterTemplate></div></FooterTemplate>
    </asp:Repeater>

    <!-- COMPLETED -->
    <div class="page-section-header">
        <h3>Completed</h3>
        <div class="line"></div>
    </div>

    <asp:Repeater ID="rptCompleted" runat="server" OnItemDataBound="rptCompleted_ItemDataBound">
        <HeaderTemplate><div class="row"></HeaderTemplate>
        <ItemTemplate>
            <div class="col-lg-6 col-md-12">
                <div class="project-card border-completed">

                    <!-- Header -->
                    <div class="pc-heading">
                        <div style="min-width:0;flex:1;">
                            <div class="project-title"><asp:Label ID="lbl_ProjectName" runat="server"></asp:Label></div>
                            <div style="display:flex;align-items:center;gap:12px;flex-wrap:wrap;margin-bottom:6px;">
                                <div style="font-size:11px;color:#78909c;font-weight:600;">
                                    <i class="ti ti-folder"></i> <asp:Label ID="lbl_ProjectType" runat="server"></asp:Label>
                                </div>
                                <asp:Panel ID="pnl_HoursWarning" runat="server" Visible="false">
                                    <div class="hours-warning">
                                        <i class="ti ti-alert-triangle"></i>
                                        <span>Hours Exceeded</span>
                                    </div>
                                </asp:Panel>
                            </div>
                            <div class="status-indicator">
                                <div class="completed-dot"></div>
                                <span class="completed-text">Completed</span>
                                <span class="pc-dates" style="margin-top:0;">
                                    <i class="ti ti-calendar"></i>&nbsp;<asp:Label ID="lbl_StartDate" runat="server" />
                                    &nbsp;-&nbsp;<asp:Label ID="lbl_EndDate" runat="server" />
                                </span>
                            </div>
                        </div>
                        <div style="display:flex; flex-direction:column; gap:8px; align-items:flex-end; flex-shrink:0;">
                            <asp:HyperLink ID="lnk_Project" runat="server" CssClass="btn-view-project">
                                <i class="glyphicon glyphicon-eye-open"></i> View
                            </asp:HyperLink>
                            <asp:HyperLink ID="lnk_Workflow" runat="server" Visible="false" Target="_blank" CssClass="workflow-badge" style="margin-left:0;">
                                <i class="ti ti-file-text"></i> View Workflow
                            </asp:HyperLink>
                        </div>
                    </div>

                    <!-- Stats Strip -->
                    <div class="pc-stats-strip">
                        <div class="pc-stat">
                            <div class="pc-stat-label">Est. Hours</div>
                            <div class="pc-stat-val val-blue"><asp:Label ID="lbl_EstimatedHours" runat="server"></asp:Label><small style="font-size:10px;font-weight:500;">h</small></div>
                        </div>
                        <div class="pc-stat">
                            <div class="pc-stat-label">Overall Used</div>
                            <div class="pc-stat-val val-teal"><asp:Label ID="lbl_UsedHours" runat="server"></asp:Label><small style="font-size:10px;font-weight:500;">h</small></div>
                        </div>
                        <div class="pc-stat">
                            <div class="pc-stat-label">My Used</div>
                            <div class="pc-stat-val val-purple"><asp:Label ID="lbl_myhours" runat="server"></asp:Label><small style="font-size:10px;font-weight:500;">h</small></div>
                        </div>
                    </div>

                    <!-- Body -->
                    <div class="pc-body">
                        <div class="pc-section-label">
                            <div style="display:flex;align-items:center;gap:8px;">
                                <i class="ti ti-list-details"></i> All Task Details
                            </div>
                            <span class="assigned-badge">Assigned Employees: <asp:Label ID="lbl_AllAssignedEmpCount" runat="server">0</asp:Label></span>
                        </div>
                        <div class="task-grid">
                            <div class="task-box tb-overall">
                                <span class="t-count"><asp:Label ID="lbl_AllOverall" runat="server">–</asp:Label></span>
                                <div class="t-label">Overall</div>
                            </div>
                            <div class="task-box tb-assigned">
                                <span class="t-count"><asp:Label ID="lbl_AllAssignedCount" runat="server">0</asp:Label></span>
                                <div class="t-label">Assigned</div>
                            </div>
                            <div class="task-box tb-inprogress">
                                <span class="t-count"><asp:Label ID="lbl_InProgress" runat="server"></asp:Label></span>
                                <div class="t-label">Ongoing</div>
                            </div>
                            <div class="task-box tb-overdue">
                                <span class="t-count"><asp:Label ID="lbl_Pending" runat="server"></asp:Label></span>
                                <div class="t-label">Overdue</div>
                            </div>
                            <div class="task-box tb-completed">
                                <span class="t-count"><asp:Label ID="lbl_Completed" runat="server"></asp:Label></span>
                                <div class="t-label">Completed</div>
                            </div>
                        </div>

                        <hr class="pc-section-divider" />

                        <div class="pc-section-label">
                            <div style="display:flex;align-items:center;gap:8px;">
                                <i class="ti ti-user"></i> My Task Details
                            </div>
                        </div>
                        <div class="task-grid">
                            <div class="task-box tb-overall">
                                <span class="t-count"><asp:Label ID="lbl_MyOverall" runat="server">–</asp:Label></span>
                                <div class="t-label">Overall</div>
                            </div>
                            <div class="task-box tb-assigned">
                                <span class="t-count"><asp:Label ID="lbl_MyAssignedCount" runat="server">0</asp:Label></span>
                                <div class="t-label">Assigned</div>
                            </div>
                            <div class="task-box tb-inprogress">
                                <span class="t-count"><asp:Label ID="lbl_MyOngoing" runat="server">–</asp:Label></span>
                                <div class="t-label">Ongoing</div>
                            </div>
                            <div class="task-box tb-overdue">
                                <span class="t-count"><asp:Label ID="lbl_MyOverdue" runat="server">–</asp:Label></span>
                                <div class="t-label">Overdue</div>
                            </div>
                            <div class="task-box tb-completed">
                                <span class="t-count"><asp:Label ID="lbl_MyCompleted" runat="server">–</asp:Label></span>
                                <div class="t-label">Completed</div>
                            </div>
                        </div>
                    </div>

                </div>
            </div>
            <%# (Container.ItemIndex % 2 == 1) ? "</div><div class='row'>" : "" %>
        </ItemTemplate>
        <FooterTemplate></div></FooterTemplate>
    </asp:Repeater>

</asp:Content>
