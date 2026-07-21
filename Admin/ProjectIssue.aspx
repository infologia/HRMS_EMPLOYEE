<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="ProjectIssue.aspx.cs" Inherits="Admin_ProjectIssue" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/uploaders/fileinput/fileinput.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/uploader_bootstrap.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/styling/uniform.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/styling/switchery.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/styling/switch.min.js"></script>

    <script type="text/javascript">
     
        function command()
        {
            
            }
</script>
  

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-flat">
                <div class="panel-heading">
                    <h6 class="panel-title">Project Issues</h6>
                    <div class="heading-elements">
                        <ul class="icons-list">
                           <li><a href="#" class="btn btn-link"><i class="icon-menu7 position-left"></i> Advanced search</a></li>
                        </ul>
                    </div>
                </div>

                <div class="panel-body">
                    <div class="row">
                        	<div class="col-md-4">
                            <!-- Revisions -->
                            <div class="panel panel-flat">
                                   
                                <div class="panel-heading">
                                    <h6 class="panel-title"><i class="icon-git-commit position-left"></i>Order by Created</h6>
                                    <div class="heading-elements">
                                        <ul class="icons-list">
                                            <li><a data-action="collapse"></a></li>
                                            <li><a data-action="reload"></a></li>
                                            <li><a data-action="close"></a></li>
                                        </ul>
                                    </div>
                             </div>
                                  <div class="panel-body">
                        <asp:PlaceHolder ID="ph_view" runat="server"></asp:PlaceHolder>
                             </div>
                                </div>
                             
                          
                                
                           <div>
                                <a>
                                    <h8 class="text-semibold heading-divided">+Create Issue</h8>
                                </a>
                            </div>
                      
                         </div>
                        <!-- /revisions -->

                        <div class="col-md-8">
                            <div class="content-group">
                                <h8 class="text-semibold">Project name</h8>
                                <h6 class="heading-divided">Issue Name</h6>




                                <div class="list-group no-border">
                                    <div class="panel-footer panel-footer-transparent pb-20">
                                        <div class="heading-elements">
                                            <ul class="list-inline list-inline-condensed heading-text">
                                                <li><a href="#"><span class="label bg-blue-300"><i class="icon-pencil7"></i>Edit</span></a></li>
                                                <li><a href="#"><span class="label bg-blue-300">Comment</span></a></li>
                                                <li><a href="#"><span class="label bg-blue-300">Assign</span></a></li>
                                                <li><a href="#"><span class="label bg-blue-300">ToDo</span></a></li>
                                                <li><a href="#"><span class="label bg-blue-300">InProgress</span></a></li>
                                                <li><a href="#"><span class="label bg-blue-300">Done</span></a></li>
                                                <li><a href="#"><span class="label bg-blue-300">Admin</span></a></li>
                                            </ul>




                                            <ul class="list-inline list-inline-condensed heading-text pull-right">
                                                <li><a href="#" class="text-default" data-toggle="modal" data-target="#invoice"><i class="icon-eye8"></i></a></li>
                                                <li class="dropdown">
                                                    <a href="#" class="text-default dropdown-toggle" data-toggle="dropdown"><i class="icon-menu7"></i><span class="caret"></span></a>
                                                    <ul class="dropdown-menu dropdown-menu-right">
                                                        <li><a href="#"><i class="icon-printer"></i>Print invoice</a></li>
                                                        <li><a href="#"><i class="icon-file-download"></i>Download invoice</a></li>
                                                        <li class="divider"></li>
                                                        <li><a href="#"><i class="icon-file-plus"></i>Edit invoice</a></li>
                                                        <li><a href="#"><i class="icon-cross2"></i>Remove invoice</a></li>
                                                    </ul>
                                                </li>
                                            </ul>
                                        </div>

                                    </div>
                                </div>

                                <div class="row">

                                 <asp:PlaceHolder ID="ph_taskview" runat="server"></asp:PlaceHolder>

                                    </div>
                                         <span class="text-semibold">Activity</span>
                                               
                                                    <ul class="nav nav-lg nav-tabs nav-tabs-bottom nav-tabs-toolbar no-margin">
                                                        <li class="active"><a href="#course-overview" data-toggle="tab">All</a></li>
                                                        <li><a href="#course-attendees" data-toggle="tab">Commands</a></li>
                                                        <li><a href="#course-schedule" data-toggle="tab">Worklog</a></li>
                                                        <li><a href="#course-attendees" data-toggle="tab">History</a></li>
                                                        <li><a href="#course-schedule" data-toggle="tab">Activity</a></li>
                                                    </ul>


                                             
                                        
                                
                               
                                </div>
                                     
                            </div>
                        </div>


<div>
                        
   
                 
     <div class="row">
               <div class="col-md-8"></div>
                        <div class="col-md-4">
                           
                          <input type="button" text="add" id="command"/>
                            <div id="test" runat="server"  style=" visibility: hidden">
                        <div class="panel panel-flat">
								<div class="panel-heading">
									<h6 class="panel-title">Share your thoughts</h6>
									<div class="heading-elements">
										<ul class="icons-list">
					                		<li><a data-action="close"></a></li>
					                	</ul>
				                	</div>
								</div>

								<div class="panel-body">
									<div action="#">
										<div class="form-group">
											<textarea name="enter-message" class="form-control mb-15" rows="3" cols="1" placeholder="What's on your mind?"></textarea>
										</div>

										<div class="row">
				                    		<div class="col-xs-4">
					                        	<ul class="icons-list icons-list-extended mt-10">
					                                <li><a href="#" data-popup="tooltip" title="Add photo" data-container="body"><i class="icon-images2"></i></a></li>
					                            	<li><a href="#" data-popup="tooltip" title="Add video" data-container="body"><i class="icon-film2"></i></a></li>
					                                <li><a href="#" data-popup="tooltip" title="Add event" data-container="body"><i class="icon-calendar2"></i></a></li>
					                            </ul>
				                    		</div>

				                    		<div class="col-xs-6 text-right">
					                            <button type="button" class="btn btn-primary btn-labeled btn-labeled-right">Share <b><i class="icon-circle-right2"></i></b></button>
				                    		</div>
				                    	</div>
			                    	</div>
		                    	</div>
							</div>
                                </div>
                            </div>
                            </div></div>
							<!-- /share your thoughts -->
                    </div>
                </div>


            </div>
        </div>





    







</asp:Content>

