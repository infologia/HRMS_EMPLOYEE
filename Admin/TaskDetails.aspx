<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="TaskDetails.aspx.cs" Inherits="Admin_TaskDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-9">
            <div class="panel panel-flat">

                <div class="panel-heading">
							<h4><i class="icon-file-text"></i> <span class="text-semibold" id="headtitle" runat="server"></span></h4>
						</div>
<%-- <div class="heading-elements">
                    <a href="Taskview.aspx" class="btn btn-primary margin-left-1" style="margin-right: 60px;"><i class="icon-undo"></i>Back</a>
                </div>--%>


                <div class="panel-body">
                    <h6 class="text-semibold">Task Overview</h6>
                    <p class="content-group" id="taskdesc" runat="server"></p>

                    <h6 class="text-semibold">Project Overview</h6>
                    <p class="content-group-lg" id="pjdesc" runat="server"></p>
                    <br />
                </div>
               
                <div class="mail-attachments-container">
                    <h6 class="mail-attachments-heading">Attachments</h6>

                    <ul class="mail-attachments">


                        <asp:PlaceHolder ID="PH_attach" runat="server"></asp:PlaceHolder>
                    </ul>
                </div>
            </div>
            <br />

            <!-- /attachments -->

        </div>



        <div class="col-md-3">
            <div class="panel panel-flat">
                <div class="panel-heading">
                    <h5 class="panel-title">Simple user list</h5>
                    <div class="heading-elements">
                        <ul class="icons-list">
                            <li><a data-action="collapse"></a></li>
                            <li><a data-action="reload"></a></li>
                            <li><a data-action="close"></a></li>
                        </ul>
                    </div>
                </div>
                <div class="panel-body">

                    <ul class="media-list">

                        <li class="media-header">Assignee</li>

                        <asp:PlaceHolder ID="PH_assignee" runat="server"></asp:PlaceHolder>

                    </ul>
                </div>
            </div>
        </div>


    </div>
</asp:Content>

