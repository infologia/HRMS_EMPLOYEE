<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Latepermissionrequest.aspx.cs" Inherits="Employee_Latepermissionrequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/pickers/anytime.min.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
 <div class="row">
     <div class="col-md-2"></div>
     <div class="col-md-8">
         <div class="panel panel-flat">
             <div class="panel-heading">
                 <h5 class="panel-title"></h5>

             </div>


             <div class="panel-body">
                 <fieldset>
                     <legend class="text-semibold"><i class="icon-pencil4"></i> Late Permission Request</legend>
                     <div class="row">
                         <div class="col-md-6">

                             <label class="content-group text-semibold">Request Date<span style="color: red">*</span> </label>
                             <div class="input-group">
                                 <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                 <asp:TextBox ID="txt_date" runat="server" class="form-control pickadate" Enabled="true" required=""></asp:TextBox>
                             </div>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txt_date" runat="server" ErrorMessage="Please select Date" ForeColor="Red"></asp:RequiredFieldValidator>
                         </div>
                         <div class="col-md-6">

                             <label class="content-group text-semibold">From Time <span style="color: red">*</span></label>

                             <div class="input-group">
                                 <span class="input-group-addon"><i class="icon-alarm"></i></span>
                                 <asp:TextBox ID="txt_fromtime" runat="server" CssClass="form-control" required=""></asp:TextBox>
                             </div>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txt_fromtime" runat="server" ErrorMessage="Please select From time" ForeColor="Red"></asp:RequiredFieldValidator>

                         </div>
                     </div>
                     <div class="row">
                         <div class="col-md-6">
                             <label class="content-group text-semibold">To Time <span style="color: red">*</span></label>
                             <div class="input-group">
                                 <span class="input-group-addon"><i class="icon-alarm"></i></span>
                                 <asp:TextBox ID="txt_totime" runat="server" class="form-control" required=""></asp:TextBox>
                             </div>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txt_totime" runat="server" ErrorMessage="Please select To time" ForeColor="Red"></asp:RequiredFieldValidator>
                         </div>

                         <div class="col-md-6">
                             <label class="content-group text-semibold">Reason<span style="color: red">*</span> </label>
                             <textarea id="txt_reasons" runat="server" rows="1" cols="4" class="form-control" required=""></textarea>
                             <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="txt_reasons" runat="server" ErrorMessage="Enter a reason" ForeColor="Red"></asp:RequiredFieldValidator>

                         </div>
                     </div>
              
                 <div class="form-group">
                     <div class="text-right">
                         <a href="Latepermissionrequestview.aspx" class="btn btn-primary" style="margin-right: 15px;">Back</a>
                         <asp:Button ID="btn_request" runat="server" Text="Request" OnClick="btn_perm_Click" class="btn btn-primary" Style="margin-right: 15px;"></asp:Button>
                     </div>
                 </div>
                </fieldset>
             </div>

         </div>
     </div>
     <div class="col-md-2"></div>
 </div>

 <script>
     $(document).ready(function () {
         var today = new Date();

         $('.pickadate').pickadate({
             format: 'dd/mm/yyyy',
             min: today,
             max: today,
             selectMonths: false,
             selectYears: false
         });
     });
 </script>
     <script>
         $('.pickadate').pickadate({
             format: 'dd/mm/yyyy',        // UI format
             formatSubmit: 'dd/mm/yyyy',  // value format
             hiddenName: false
         });
     </script>


</asp:Content>

