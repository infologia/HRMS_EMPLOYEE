<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="TaskActivity.aspx.cs" Inherits="Admin_TaskActivity" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
      <script type="text/javascript" src="../Template/assets/js/plugins/uploaders/fileinput/fileinput.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/uploader_bootstrap.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/styling/uniform.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/styling/switchery.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/styling/switch.min.js"></script>

    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_advanced.js"></script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    
    <script type="text/javascript">

        function command() {
            var tb = document.getElementById("txt_commens");

            tb.style = display = "none";
        }
    </script>

    <div class="panel">
        <div class="panel-body text-center">
            <div class="form-group">
                <span class="text-semibold">Activity</span><br />
                <br />
                <br />
                <div class="heading-elements">
							<a href="Taskview.aspx" class="btn bg-blue"><i class="icon-undo"></i> Back</a>
					
						</div>
                <ul class="list-inline list-inline-condensed heading-text">


                    <li>
                        <asp:LinkButton ID="allactivity" runat="server" Text="All" class="label bg-blue-300" OnClick="allactivity_Click"></asp:LinkButton></li>

                    <li>
                        <asp:LinkButton ID="cmd" runat="server" Text="Comments" class="label bg-blue-300" OnClick="cmd_Click"></asp:LinkButton></li>

                    <li>
                        <asp:LinkButton ID="work" runat="server" Text="Worklog" class="label bg-blue-300"></asp:LinkButton></li>

                    <li>
                        <asp:LinkButton ID="lb_history" runat="server" Text="history" class="label bg-blue-300" OnClick="lb_history_Click"></asp:LinkButton></li>



                    <li>
                        <asp:LinkButton ID="lb_activity" runat="server" Text="Activity" class="label bg-blue-300" OnClick="lb_activity_Click"></asp:LinkButton></li>
                </ul>
                <br />


            </div>
        </div>
    </div>


    <div id="alltab" runat="server">
        <div class="row">
            <div class="col-lg-3"></div>
            <div class="col-lg-6">

                <div class="panel panel-flat">
                    <div class="panel-heading">
                        <h5 class="panel-title">All  Details</h5>
                        <div class="heading-elements">
                            <ul class="icons-list">
                                <li><a href="#"><i class="icon-file-pdf" title="Export to .pdf"></i></a></li>
                                <li><a href="#"><i class="icon-file-excel" title="Export to .csv"></i></a></li>
                                <li><a href="#"><i class="icon-file-word" title="Export to .doc"></i></a></li>
                            </ul>
                        </div>
                    </div>

                    <div class="panel-body" style="padding: 0px;">
                    </div>

                    <table class="table datatable-button-init-basic">
                        <thead>
                            <tr>
                                <th>Taskname</th>

                                <th>Created</th>
                                <th>Comment</th>
                                <th>Createdon</th>



                            </tr>
                        </thead>
                        <tbody>
                            <asp:PlaceHolder ID="all" runat="server"></asp:PlaceHolder>
                        </tbody>
                    </table>

                </div>
            </div>
        </div>
    </div>
    <!-- /scrollable datatable -->







    <div id="coms" class="row" runat="server" >
        <div class="row">
            <div class="col-lg-2"></div>
            <div class="col-lg-8">
                <div class="panel panel-flat">
                    <div class="panel-heading">
                        <h5 class="panel-title text-semiold"><i class="icon-bubbles4 position-left"></i>Comments</h5>
                        
                    </div>

                    <div class="panel-body">
                        <ul class="media-list content-group-lg stack-media-on-mobile">
                           <asp:PlaceHolder ID="Ph_chat" runat="server"></asp:PlaceHolder>


                        </ul>


                        <div class="text-right">
                            <a class="btn bg-blue" onclick="command();"><i class="icon-plus22"></i>Add comment</a>
                        </div>
                        <br />

                        <div class="row">
                               
                                <div class="col-md-12">


                                    <div id="txt_commens" style="display:none">
                                        <div class="panel panel-flat">
                                              <div class="panel-heading">
                    <h5 class="panel-title"></h5>
                    <div class="heading-elements">
                        <ul class="icons-list">
                            <li><a data-action="collapse"></a></li>
                            <li><a data-action="reload"></a></li>
                            <li><a data-action="close"></a></li>
                        </ul>
                    </div>
                </div>
                                            <div class="panel-body">
                                               
                                                <div action="#">
                                                    <div class="form-group">
                                                        <textarea id="txt_reason" runat="server" rows="5" cols="5" class="form-control" placeholder="Enter your Reason here" ></textarea>
                                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txt_reason"  runat="server" ErrorMessage="Please write comment" Forecolor="red"></asp:RequiredFieldValidator>
                                                    </div>

                                                    <div class="row">
                                                        <div class="col-xs-12">
                                                            <div class="form-group">

                                                                <asp:FileUpload ID="comment" runat="server" class="file-input" data-show-caption="false" data-show-upload="false" data-browse-class="btn btn-primary btn-xs" data-remove-class="btn btn-default btn-xs" />



                                                            </div>
                                                            <div class="col-xs-6 text-right">
                                                                <asp:LinkButton ID="btn_submit" class="btn btn-primary btn-labeled btn-labeled-right" runat="server" OnClick="btn_submit_Click">Send<b><i class="icon-circle-right2"></i></b></asp:LinkButton>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                        </div>
                                    </div>
                                </div>

                            </div>


                    </div>
                </div>

            </div>
        </div>
    </div>







    <div id="Div3" class="row" runat="server" visible="true">
        <asp:PlaceHolder ID="ph_history" runat="server"></asp:PlaceHolder>
    </div>
    <br />
    <asp:PlaceHolder ID="ph_taskview" runat="server"></asp:PlaceHolder>

    </div>
								
							


                
           

       


 
</asp:Content>

