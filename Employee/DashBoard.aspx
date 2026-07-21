<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="DashBoard.aspx.cs" Inherits="WEB_Employee_DashBoard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

   <style>
        @media (max-width: 767px) {

            .panel {
                position: relative !important;
            }

            .panel-body {
                position: relative !important;
            }

                .panel-body .heading-elements {
                    position: absolute !important;
                    bottom: 10px !important;
                    right: 10px !important;
                    display: block !important;
                    z-index: 99;
                    margin: 0;
                    padding: 0;
                }

                    .panel-body .heading-elements .icons-list li a {
                        font-size: 22px;
                    }
        }
    </style>

<%--COMMON PANEL STYLES--%>
<style>
.panel {
    position: relative;
    border-radius: 20px;
    overflow: hidden;
    border: none !important;
    background: #ffffff;
    box-shadow: 0 12px 28px rgba(0,0,0,0.08);
    transition: transform 0.3s ease, box-shadow 0.3s ease;
    min-height: 140px;
    color: #333;
}
.panel:hover {
    transform: translateY(-6px);
    box-shadow: 0 20px 40px rgba(0,0,0,0.12);
}
.panel-body {
    padding: 25px !important;
    position: relative;
    z-index: 1;
}
.panel::after {
    content: "";
    position: absolute;
    inset: 0;
    border-radius: 20px;
    box-shadow: inset 0 0 0 1px rgba(0,0,0,0.05);
    pointer-events: none;
}
/* Headings & text */
.panel-body h3 {
    font-size: 32px;
    font-weight: 700;
    margin-bottom: 8px;
}
.panel-body span,
.panel-body .text-muted,
.panel-body b,
.panel-body small {
    font-size: 16px;
    opacity: 0.9;
}
/* Arrow icon */
.heading-elements a {
    font-size: 18px;
    transition: transform 0.2s;
}
.heading-elements a:hover {
    transform: scale(1.2);
}
</style>
<%--DASHBOARD SUMMARY CARDS--%>
<style>
     
.bg-teal-400 {
    background: linear-gradient(135deg, #33b5ad, #4fd1c5);
    color: #fff;
}

.bg-warning-300 {
    background: linear-gradient(135deg, #f4b942, #ffd166);
    color: #fff;
}

.bg-pink-400 {
    background: linear-gradient(135deg, #e85d8e, #f28ab2);
    color: #fff;
}

.bg-slate-400 {
    background: linear-gradient(135deg, #6b7f8e, #8fa3ad);
    color: #fff;
}

.bg-purple-300 {
    background: linear-gradient(135deg, #8e6ccf, #b497e7);
    color: #fff;
}

.bg-blue-400 {
    background: linear-gradient(135deg, #3b82f6, #60a5fa);
    color: #fff;
}

/* Make text white inside dashboard cards */
.bg-teal-400 *,
.bg-warning-300 *,
.bg-pink-400 *,
.bg-slate-400 *,
.bg-purple-300 *,
.bg-blue-400 * {
    color: #fff !important;
}

    </style>
<%--TEAM MEMBERS PANEL--%>
<style>

.panel-flat {
    background: #f9fafb;
}

/* Scroll area */
.vertical-scroll {
    max-height: 400px;
    overflow-y: auto;
}

/* Section headers */
.media-header {
    padding: 10px 15px;
    font-size: 13px;
    font-weight: 600;
    color: #6b7280;
    text-transform: uppercase;
}

/* Team rows */
.media-list .media {
    padding: 12px 15px;
    border-bottom: 1px solid rgba(0,0,0,0.05);
    transition: background 0.2s, transform 0.2s;
}

.media-list .media:hover {
    background: rgba(0,0,0,0.03);
    transform: translateX(4px);
}

/* Profile images */
.media-left img {
    width: 48px;
    height: 48px;
    border-radius: 50%;
    border: 2px solid #fff;
}

/* Status dots */
.status-mark {
    width: 14px;
    height: 14px;
    border-radius: 50%;
    display: inline-block;
    border: 2px solid #fff;
}

.bg-success { background: #22c55e !important; }
.bg-danger  { background: #ef4444 !important; }

    </style>
<%--TASKS PANEL--%>
<style>
       
.panel .panel-heading {
    background: transparent;
    border-bottom: 1px solid rgba(0,0,0,0.08);
    font-weight: 600;
}

.panel .panel-heading i {
    margin-right: 6px;
    color: #555;
}

.panel-heading-flex {
    display: flex;
    align-items: center;
    justify-content: space-between;
}

/* Task list */
.navigation-alt li a {
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: 14px 16px;
    color: #374151;
    text-decoration: none;
    transition: background 0.2s;
}

.navigation-alt li a:hover {
    background: rgba(0,0,0,0.05);
    border-radius: 12px;
}

.navigation-alt li a i {
    margin-right: 10px;
    font-size: 16px;
    color: #6b7280;
}

/* Badge */
.badge {
    background: #6366f1;
    color: #fff;
    font-size: 12px;
    font-weight: 600;
    padding: 4px 8px;
    border-radius: 12px;
}

    </style>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-lg-4">
            <div class="panel bg-teal-400" style="height: 134px">
                <div class="panel-body" style="padding: 23px;">
                    <div class="heading-elements">
                        <ul class="icons-list">
                            <li><a href="WorkedHoursDetails.aspx" class="glyphicon glyphicon-circle-arrow-right"></a></li>
                        </ul>
                    </div>
                    <h3 class="no-margin">
                        <asp:Label ID="lbl_Workingdays" runat="server" CssClass=""> </asp:Label>
                        Days</h3>
                    Total No of Working Days in Month (<asp:Label runat="server" ID="lbl_Workingdaysavg"></asp:Label>)
                                      
                </div>
                <div class="container-fluid">
                    <div id="members-online"></div>
                </div>
            </div>
        </div>
        <div class="col-lg-4">
            <div class="panel bg-warning-300" style="height: 134px">
                <div class="panel-body" style="padding: 23px;" >
                    <div class="heading-elements">
                        <ul class="icons-list">
                            <li><a href="WorkedHoursDetails.aspx" class="glyphicon glyphicon-circle-arrow-right"></a></li>
                        </ul>
                    </div>
                    <h3 class="no-margin" >
                        <asp:Label ID="lbl_workday" runat="server"></asp:Label>
                        Days</h3>
                    Total No of Worked Days in Year (<asp:Label ID="lbl_WorkedDaysyear" runat="server"></asp:Label>)
                                        <div class="text-muted text-size-small">
                                            <asp:Label runat="server" ID="lb_month"></asp:Label>
                                        </div>
                </div>
                <div class="container-fluid">
                    <div id="Div2"></div>
                </div>
            </div>
        </div>
        <div class="col-lg-4">
            <div class="panel bg-pink-400" style="height: 134px">
                <div class="panel-body" style="padding: 23px;">
                    <div class="heading-elements">
                        <ul class="icons-list">
                            <li><a href="LeaveRequestView.aspx" class="glyphicon glyphicon-circle-arrow-right"></a></li>
                        </ul>
                    </div>
                    <h3 class="no-margin">
                        <asp:Label ID="lbl_Totalleave" runat="server"></asp:Label>
                        Days</h3>
                    Approved Leave for this Month
                                        <asp:Label runat="server" ID="lbl_Totalleaveavg"></asp:Label>
                </div>
                <div id="server-load"></div>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-4">
            <div class="panel bg-slate-400" style="height: 134px">
                <div class="panel-body" style="padding: 23px;">
                    <div class="heading-elements">
                        <ul class="icons-list">
                            <li><a href="Overhours.aspx" class="glyphicon glyphicon-circle-arrow-right"></a></li>
                        </ul>
                    </div>
                    <h3 class="no-margin">
                        <asp:Label ID="lbl_whours" runat="server"></asp:Label>
                        Hours</h3>
                    Total No of Worked Hours in Month <asp:Label ID="lb_wmonth" runat="server"></asp:Label>
                                        <div class="text-muted text-size-small">
                                            <asp:Label runat="server" ID="lbl_test"></asp:Label>
                                        </div>
                </div>
                <div class="container-fluid">
                    <div id="Div1"></div>
                </div>
            </div>
        </div>

        <div class="col-lg-4">
            <div class="panel bg-purple-300" style="height: 134px">
                <div class="panel-body" style="padding: 23px;">
                    <div class="heading-elements">
                        <ul class="icons-list">
                            <li><a href="Overhours.aspx" class="glyphicon glyphicon-circle-arrow-right"></a></li>
                        </ul>
                    </div>
                    <h3 class="no-margin">
                        <asp:Label ID="lbl_totalhours" runat="server"></asp:Label>
                        Hours</h3>
                    Total No of Worked Hours in Year <asp:Label ID="lb_month2" runat="server"></asp:Label>
                                        <div class="text-muted text-size-small">
                                            <asp:Label runat="server" ID="Label4"></asp:Label>
                                        </div>
                </div>
                <div class="container-fluid">
                    <div id="Div3"></div>
                </div>
            </div>
        </div>
        <div class="col-lg-4">
            <div class="panel bg-blue-400" style="height: 134px">
                <div class="panel-body" style="font-size:14px;">
                    <div class="heading-elements">
                        <ul class="icons-list">
                            <li><a href="TimeMonitoring.aspx" class="glyphicon glyphicon-circle-arrow-right"></a></li>
                        </ul>
                    </div>
                    <div class="no-margin">
                        <b>Date: </b><b>
                            <asp:Label ID="lb_date" runat="server"></asp:Label></b>
                    </div>
                    <div class="no-margin">
                        <b>In Time: </b>
                        <asp:Label ID="lb_intime" runat="server"></asp:Label>
                    </div>
                    <div class="no-margin">
                        <b>Out Time: </b>
                        <asp:Label ID="lb_outtime" runat="server"></asp:Label>
                    </div>
                    <br />
                </div>
                <div id="today-revenue"></div>
            </div>
        </div>
    </div>


    <div class="row">
        <div class="col-lg-6">
            <div class="panel panel-flat">
                <div class="panel-heading panel-heading-flex">
                    <h5 class="panel-title">Leave Days Details</h5>
                    <div>
                        <a href="LeaveRequestView.aspx"><span class="label label-primary">View</span></a>
                    </div>
                </div>
             
                <div>
                <table class="table datatable-basic">
                    <thead>
                        <tr>
                            <th>From Date</th>
                            <th>Reason</th>
                            <th>Status</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder ID="PH_LeaveRequest" runat="server"></asp:PlaceHolder>
                    </tbody>
                </table>
            </div>
                </div>
        </div>
        <div class="col-lg-6">
            <div class="panel panel-flat">
                <div class="panel-heading panel-heading-flex">
                    <h5 class="panel-title">Permission Request</h5>
                    <div>
                        <a href="PermissionRequestView.aspx"><span class="label label-primary">View</span></a>
                    </div>
                </div>
                <div>
                <table class="table datatable-basic">
                    <thead>
                        <tr>
                            <th>Request Date</th>
                            <th>Reason</th>
                            <th>Status</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder ID="PH_PermissionRequest" runat="server"></asp:PlaceHolder>

                    </tbody>
                </table>
            </div>
                </div>
        </div>
    </div>
</asp:Content>

