<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Viewtask.aspx.cs" Inherits="TicketingTool_Viewtask" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <!-- Page header -->
        <div class="page-header">
            <div class="page-header-content">
                <div class="page-title">
                    <h4><i class="icon-arrow-left52 position-left"></i><span class="text-semibold">Task Manager</span> - Grid</h4>

                    <%--	<ul class="breadcrumb position-right">
								<li><a href="index.html">Home</a></li>
								<li><a href="task_manager_grid.html">Task manager</a></li>
								<li class="active">Grid</li>
							</ul>--%>
                </div>
                <%--<div class="heading-elements">
                    <a href="CreateTask.aspx" class="btn bg-blue btn-labeled heading-btn"><b><i class="icon-task"></i></b>Create task</a>

                </div>--%>
            </div>
        </div>
        <!-- /page header -->
        <!-- Content area -->
        <div class="content">
            <!-- Detached content -->
            <div class="container-detached">
                <div class="content-detached">
                    <div class="row">
                        <asp:PlaceHolder ID="PH_task" runat="server"></asp:PlaceHolder>
                    </div>
                </div>
            </div>
        </div>

        
        
    </div>
</asp:Content>

