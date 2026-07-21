<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="WEB_Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">


    <style>
        .panel-heading-flex {
    display: flex;
    align-items: center;
    justify-content: space-between;
}
    </style>

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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-lg-4">

            <!-- Members online -->
            <div class="panel bg-teal-400" style="height: 134px">
                <div class="panel-body">
                    <div class="heading-elements">
                        <ul class="icons-list">
                            <li><a href="TimeMonitoring.aspx" class="glyphicon glyphicon-circle-arrow-right"></a></li>
                        </ul>
                    </div>

                    <h3 class="no-margin">Employees : <asp:Label ID="lbl_Totalmembers" runat="server"></asp:Label></h3>
                    <div class="text-muted text-size-small">Online : <asp:Label ID="lb_intime" runat="server"></asp:Label></div>
                    <div class="text-muted text-size-small">
                        Offline : 
                        <asp:Label ID="lb_outtime" runat="server"></asp:Label>
                    </div>

                </div>


            </div>
            <!-- /members online -->

        </div>

        <div class="col-lg-4">

            <!-- Current server load -->
            <div class="panel bg-pink-400" style="height: 134px">
                <div class="panel-body">
                    <div class="heading-elements">
                        <ul class="icons-list">
                            <li><a href="LeaveResponse.aspx" class="glyphicon glyphicon-circle-arrow-right"></a></li>
                        </ul>
                    </div>

                    <h3 class="no-margin">Leave Request
                    </h3>
                    Total No of pending request ( <asp:Label ID="lbl_mon" runat="server"></asp:Label> ) : 
										<div class="text-muted text-size-small">
                                            <a href="#" style="color: white">
                                                <asp:Label ID="Label1" runat="server"></asp:Label></a>
                                            <asp:Label ID="lbl_leave" runat="server" />
                                        </div>

                </div>

            </div>

            <!-- /current server load -->

        </div>

        <div class="col-lg-4">

            <!-- Today's revenue -->
            <div class="panel bg-blue-400" style="height: 134px">
                <div class="panel-body">
                    <div class="heading-elements">
                        <ul class="icons-list">
                            <li><a href="PermissionResponse.aspx" class="glyphicon glyphicon-circle-arrow-right"></a></li>
                        </ul>
                    </div>

                    <h3 class="no-margin">Permission Request
                    </h3>
                    Total No of pending request ( <asp:Label ID="lbl_month" runat="server"></asp:Label> ) : 
										<div class="text-muted text-size-small">
                                            <asp:Label ID="lbl_perm" runat="server"></asp:Label>
                                            <%-- <asp:Label runat="server" ID="lbl_perm"></asp:Label>--%>
                                        </div>
                </div>

            </div>
            <!-- /today's revenue -->

        </div>
    </div>




    <div class="row">
        <div class="col-lg-6">
            <div class="panel panel-flat">
                <div class="panel-heading panel-heading-flex">
                    <h5 class="panel-title">Employee Complaints Details</h5>
                    <div>
                        <a href="ComplaintResponse.aspx"><span class="label label-primary">View</span></a>

                    </div>
                </div>

                <%--<div class="panel-body" style="padding: 0px;">
                </div>--%>

                <div class="table-responsive">
                <table class="table table-bordered table-framed">
                    <thead>
                        <tr>

                            <th>Createdon</th>
                            <th>Reason</th>
                            <th>Status</th>


                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder ID="PH_Leave" runat="server"></asp:PlaceHolder>

                    </tbody>
                </table>
                    </div>
            </div>
            <!-- /scrollable datatable -->

        </div>
        <div class="col-lg-6">

            <div class="panel panel-flat">
                <div class="panel-heading panel-heading-flex">
                    <h5 class="panel-title">Employee Suggestion Details</h5>
                    <div>
                        <a href="Suggestionresponse.aspx"><span class="label label-primary">View</span></a>
                    </div>
                </div>

               <%-- <div class="panel-body" style="padding: 0px;">
                </div>--%>

                <div class="table-responsive">
                <table class="table table-bordered table-framed">
                    <thead>
                        <tr>


                            <th>Createdon</th>
                            <th>Reason</th>

                            <th>Status</th>

                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder ID="PH_Suggestion" runat="server"></asp:PlaceHolder>

                    </tbody>
                </table>
                    </div>
            </div>
        </div>

        <!-- /scrollable datatable -->


    </div>

    <!-- Traffic sources -->
</asp:Content>

