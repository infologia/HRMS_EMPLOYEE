<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="assignedproject.aspx.cs" Inherits="Employee_Taskdashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<style>
.project-card {
    border-left: 4px solid #2196f3;
    border-radius: 10px;
    transition: all 0.3s ease;
    animation: fadeInUp 0.5s ease forwards;
    opacity: 0;
}
.project-card:hover {
    transform: translateY(-5px);
    box-shadow: 0 8px 16px rgba(0,0,0,0.15);
}
.border-completed { border-left-color: #4caf50; }
.border-overdue { border-left-color: #ff4444; }
.project-title {
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    max-width: 250px;
    
}
.overdue-dot {
    width: 8px;
    height: 8px;
    background: #ff4444;
    border-radius: 50%;
    animation: pulse 1.5s infinite;
}
.overdue-text {
    color: #ff4444;
}
.ongoing-dot {
    width: 8px;
    height: 8px;
    background: #2196f3;
    border-radius: 50%;
}
.ongoing-text {
    color: #2196f3;
}
.completed-dot {
    width: 8px;
    height: 8px;
    background: #4caf50;
    border-radius: 50%;
}
.completed-text {
    color: #4caf50;
}
.info-value {
    color: #333 !important;
}
.status-box.progress { background: #e3f2fd; }
.status-box.pending { background: #fff3e0; }
.status-box.completed { background: #e8f5e9; }
.status-box.progress .status-count { color: #1976d2; }
.status-box.pending .status-count { color: #f57c00; }
.status-box.completed .status-count { color: #388e3c; }
@keyframes pulse {
    0% { transform: scale(1); opacity: 1; }
    50% { transform: scale(1.2); opacity: 0.7; }
    100% { transform: scale(1); opacity: 1; }
}
@keyframes fadeInUp {
    from {
        opacity: 0;
        transform: translateY(20px);
    }
    to {
        opacity: 1;
        transform: translateY(0);
    }
}
.col-lg-4:nth-child(1) .project-card { animation-delay: 0.1s; }
.col-lg-4:nth-child(2) .project-card { animation-delay: 0.2s; }
.col-lg-4:nth-child(3) .project-card { animation-delay: 0.3s; }
.col-lg-4:nth-child(4) .project-card { animation-delay: 0.4s; }
.col-lg-4:nth-child(5) .project-card { animation-delay: 0.5s; }
.col-lg-4:nth-child(6) .project-card { animation-delay: 0.6s; }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="page-header">
        <h3>Ongoing</h3>
    </div>
    <asp:Repeater ID="rptOngoing" runat="server" OnItemDataBound="rptOngoing_ItemDataBound">
        <HeaderTemplate><div class="row"></HeaderTemplate>
        <ItemTemplate>
            <div class="col-lg-4 col-md-6">
                <div id="divProjectCard" runat="server" class="project-card panel panel-default">
                    <div class="panel-heading">
                        <div class="pull-left">
                            <h3 class="project-title h5 panel-title"><asp:Label ID="lbl_ProjectName" runat="server"></asp:Label></h3>
                            <asp:Panel ID="pnl_OverdueIndicator" runat="server" Visible="false">
                                <div class="overdue-dot pull-left" style="margin-right:8px;margin-top:6px;"></div>
                                <span class="overdue-text small text-uppercase"><strong>OVERDUE</strong></span>
                            </asp:Panel>
                            <asp:Panel ID="pnl_OngoingIndicator" runat="server" Visible="false">
                                <div class="ongoing-dot pull-left" style="margin-right:8px;margin-top:6px;"></div>
                                <span class="ongoing-text small text-uppercase"><strong>ONGOING</strong></span>
                            </asp:Panel>
                        </div>
                        <asp:HyperLink ID="lnk_Project" runat="server" CssClass="btn btn-default btn-sm pull-right">
                            <i class="glyphicon glyphicon-arrow-right"></i>
                        </asp:HyperLink>
                        <div class="clearfix"></div>
                    </div>
                    <div class="panel-body">
                         <div style="display: flex; gap: 16px; font-size: 12px; color: #888;">
     <span><i class="ti ti-calendar"></i>
         <asp:Label ID="lbl_StartDate" runat="server" /></span>
     <span>→</span>
     <span id="endDateWrap" runat="server">
         <i class="ti ti-calendar-due"></i>
         <asp:Label ID="lbl_EndDate" runat="server" />
     </span>
 </div>
                                               <div class="row" style="margin-top: 12px;">
                            <div class="col-xs-4">
                                <span class="info-label small text-uppercase text-muted"><strong>Estimated Hours</strong></span><br>
                                <span class="info-value text-primary"><strong>
                                    <asp:Label ID="lbl_EstimatedHours" runat="server"></asp:Label></strong></span>
                            </div>
                            <div class="col-xs-4">
                                <span class="info-label small text-uppercase text-muted"><strong>Used Hours</strong></span><br>
                                <span class="info-value text-primary"><strong>
                                    <asp:Label ID="lbl_UsedHours" runat="server"></asp:Label></strong></span>
                            </div>
                            <div class="col-xs-4">
                                <span class="info-label small text-uppercase text-muted"><strong>My Contribution</strong></span><br>
                                <span class="info-value text-primary"><strong>
                                    <asp:Label ID="lbl_myhours" runat="server"></asp:Label></strong></span>
                            </div>
                        </div>

                        <hr>
                        <div class="clearfix" style="margin-bottom:12px;">
                            <span class="info-label small text-uppercase text-muted pull-left"><strong>Assigned Tasks</strong></span>
                            <span class="info-value text-primary pull-right"><strong><asp:Label ID="lbl_AssignedTasks" runat="server"></asp:Label></strong></span>
                        </div>
                        <div class="small text-uppercase text-muted" style="margin-bottom:12px;"><strong>Task Status</strong></div>
                        <div class="row">
                            <div class="col-xs-4">
                                <div class="status-box progress text-center" style="padding:12px 8px;border-radius:8px;min-height:70px;">
                                    <span class="status-count h4"><strong><asp:Label ID="lbl_InProgress" runat="server"></asp:Label></strong></span>
                                    <div class="status-label small"><strong>Working</strong></div>
                                </div>
                            </div>
                            <div class="col-xs-4">
                                <div class="status-box pending text-center" style="padding:12px 8px;border-radius:8px;min-height:70px;">
                                    <span class="status-count h4"><strong><asp:Label ID="lbl_Pending" runat="server"></asp:Label></strong></span>
                                    <div class="status-label small"><strong>Pending</strong></div>
                                </div>
                            </div>
                            <div class="col-xs-4">
                                <div class="status-box completed text-center" style="padding:12px 8px;border-radius:8px;min-height:70px;">
                                    <span class="status-count h4"><strong><asp:Label ID="lbl_Completed" runat="server"></asp:Label></strong></span>
                                    <div class="status-label small"><strong>Completed</strong></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%# (Container.ItemIndex % 3 == 2) ? "</div><div class='row'>" : "" %>
        </ItemTemplate>
        <FooterTemplate></div></FooterTemplate>
    </asp:Repeater>

    <div class="page-header">
        <h3>Completed</h3>
    </div>
    <asp:Repeater ID="rptCompleted" runat="server" OnItemDataBound="rptCompleted_ItemDataBound">
        <HeaderTemplate><div class="row"></HeaderTemplate>
        <ItemTemplate>
            <div class="col-lg-4 col-md-6">
                <div class="project-card border-completed panel panel-default">
                    <div class="panel-heading">
                        <div class="pull-left">
                            <h3 class="project-title h5 panel-title"><asp:Label ID="lbl_ProjectName" runat="server"></asp:Label></h3>
                            <div>
                                <div class="completed-dot pull-left" style="margin-right:8px;margin-top:6px;"></div>
                                <span class="completed-text small text-uppercase"><strong>COMPLETED</strong></span>
                            </div>
                        </div>
                        <asp:HyperLink ID="lnk_Project" runat="server" CssClass="btn btn-default btn-sm pull-right">
                            <i class="glyphicon glyphicon-arrow-right"></i>
                        </asp:HyperLink>
                        <div class="clearfix"></div>
                    </div>
                    <div class="panel-body">
                         <div style="display: flex; gap: 16px; font-size: 12px; color: #888;">
     <span><i class="ti ti-calendar"></i>
         <asp:Label ID="lbl_StartDate" runat="server" /></span>
     <span>→</span>
     <span id="endDateWrap" runat="server">
         <i class="ti ti-calendar-due"></i>
         <asp:Label ID="lbl_EndDate" runat="server" />
     </span>
 </div>
                                               <div class="row" style="margin-top: 12px;">
                            <div class="col-xs-4">
                                <span class="info-label small text-uppercase text-muted"><strong>Estimated Hours</strong></span><br>
                                <span class="info-value text-primary"><strong>
                                    <asp:Label ID="lbl_EstimatedHours" runat="server"></asp:Label></strong></span>
                            </div>
                            <div class="col-xs-4">
                                <span class="info-label small text-uppercase text-muted"><strong>Used Hours</strong></span><br>
                                <span class="info-value text-primary"><strong>
                                    <asp:Label ID="lbl_UsedHours" runat="server"></asp:Label></strong></span>
                            </div>
                            <div class="col-xs-4">
                                <span class="info-label small text-uppercase text-muted"><strong>My Contribution</strong></span><br>
                                <span class="info-value text-primary"><strong>
                                    <asp:Label ID="lbl_myhours" runat="server"></asp:Label></strong></span>
                            </div>
                        </div>

                        <hr>
                        <div class="clearfix" style="margin-bottom:12px;">
                            <span class="info-label small text-uppercase text-muted pull-left"><strong>Assigned Tasks</strong></span>
                            <span class="info-value text-primary pull-right"><strong><asp:Label ID="lbl_AssignedTasks" runat="server"></asp:Label></strong></span>
                        </div>
                        <div class="small text-uppercase text-muted" style="margin-bottom:12px;"><strong>Task Status</strong></div>
                        <div class="row">
                            <div class="col-xs-4">
                                <div class="status-box progress text-center" style="padding:12px 8px;border-radius:8px;min-height:70px;">
                                    <span class="status-count h4"><strong><asp:Label ID="lbl_InProgress" runat="server"></asp:Label></strong></span>
                                    <div class="status-label small"><strong>Working</strong></div>
                                </div>
                            </div>
                            <div class="col-xs-4">
                                <div class="status-box pending text-center" style="padding:12px 8px;border-radius:8px;min-height:70px;">
                                    <span class="status-count h4"><strong><asp:Label ID="lbl_Pending" runat="server"></asp:Label></strong></span>
                                    <div class="status-label small"><strong>Pending</strong></div>
                                </div>
                            </div>
                            <div class="col-xs-4">
                                <div class="status-box completed text-center" style="padding:12px 8px;border-radius:8px;min-height:70px;">
                                    <span class="status-count h4"><strong><asp:Label ID="lbl_Completed" runat="server"></asp:Label></strong></span>
                                    <div class="status-label small"><strong>Completed</strong></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <%# (Container.ItemIndex % 3 == 2) ? "</div><div class='row'>" : "" %>
        </ItemTemplate>
        <FooterTemplate></div></FooterTemplate>
    </asp:Repeater>
</asp:Content>

