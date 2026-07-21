<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="OfficeLeavedaysUpdate.aspx.cs" Inherits="Admin_OfficeLeavedaysUpdate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/pickers/anytime.min.js"></script>
            <!-- Theme JS files -->
    <script type="text/javascript" src="../Template/assets/js/plugins/notifications/jgrowl.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/ui/moment/moment.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/pickers/daterangepicker.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/pickers/pickadate/picker.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/pickers/pickadate/picker.date.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/pickers/pickadate/picker.time.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/pickers/pickadate/legacy.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/picker_date.js"></script>
    <!-- /theme JS files --> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h6 class="content-group text-semibold">Office Leave days					
        <small class="display-block">Create Office Leave days</small>
    </h6>
    <div class="row">
        <div class="col-md-4">
        </div>
        <div class="col-md-4">
            <div class="panel panel-flat">
                <div class="panel-heading">
                    <h6 class="panel-title">Create Leave</h6>
                    <div class="heading-elements">
                   
                    </div>
                </div>
                <div class="panel-body">
                    <div class="form-group">
                        <label>Leave Reason</label>
                        <asp:TextBox ID="txt_LeaveReason" runat="server" CssClass="form-control" required=""></asp:TextBox>
                    </div>

                     <div class="form-group">
                        <label>Leave date</label>
                        
                         <div class="input-group">
           <span class="input-group-addon"><i class="icon-calendar22"></i></span>
           <asp:TextBox id="txt_leavedate" runat="server" class="form-control daterange-single"  placeholder="MM/DD/YYYY"></asp:TextBox>
          </div>
                    </div>

                    <div class="form-group">
                        <label>Leave Description</label>
                        <textarea rows="5" cols="5" runat="server" id="txt_Leavedesc" class="form-control" placeholder="Enter leave description" required=""></textarea>
                    </div>
                    <div class="text-center">
                         <a href="OfficeLeavedaysGrid.aspx" class="btn btn-primary"> Back</a>
                        <asp:Button ID="btn_submit" runat="server" Text="submit" CssClass="btn btn-primary" OnClick="btn_submit_Click" />

                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-4">
        </div>
    </div>
</asp:Content>


