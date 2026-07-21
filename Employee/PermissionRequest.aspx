<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="PermissionRequest.aspx.cs" Inherits="WEB_Employee_PermissionRequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
                  <legend class="text-semibold"><i class="icon-pencil4"></i> Permission Request</legend>
                  <div class="row">
                      <div class="col-md-6">
                          <label class="content-group text-semibold">Request Date <span style="color: red">*</span> </label>
                          <div class="input-group">
                              <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                              <asp:TextBox ID="txt_date" runat="server" class="form-control pickadate" onchange="checkLeave(this.value)"></asp:TextBox>
                          </div>
                          <div>
                              <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txt_date" ErrorMessage="Request Date is a required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator><span id="lbl_leave_warning" style="color: red; display: none; ">You are on leave on this date.</span>
                          </div>
                      </div>
                      <div class="col-md-6">
                          <label class="content-group text-semibold">From Time <span style="color: red">*</span> </label>
                          <div class="input-group">
                              <span class="input-group-addon"><i class="icon-alarm"></i></span>
                              <asp:TextBox ID="txt_fromtime" runat="server" TextMode="Time" CssClass="form-control pickatime-clear"></asp:TextBox>
                          </div>
                           <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_fromtime" ErrorMessage="From Time is a required" ForeColor="Red"></asp:RequiredFieldValidator>
                      </div>
                     </div>
                     
                      <div class="row">
                      <div class="col-md-6">
                          <label class="content-group text-semibold">To Time <span style="color: red">*</span> </label>
                          <div class="input-group">
                              <span class="input-group-addon"><i class="icon-alarm"></i></span>
                              <asp:TextBox ID="txt_totime" runat="server" TextMode="Time" class="form-control pickatime-clear"></asp:TextBox>
                          </div>
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_totime" ErrorMessage="To Time is a required" ForeColor="Red"></asp:RequiredFieldValidator>
                      </div>
           
                  <div class="col-md-6">

                      <label class="content-group text-semibold">Reason <span style="color: red">*</span> </label>
                      <textarea id="txt_reasons" runat="server" rows="1" cols="4" class="form-control"></textarea>
                       <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_reasons" ErrorMessage="Reason is required" ForeColor="Red"></asp:RequiredFieldValidator>
                  </div>
              </div>
          


              <div class="text-right">
                  <a href="PermissionRequestView.aspx" class="btn btn-primary" style="margin-right: 15px">Back</a>
                  <asp:Button ID="btn_request" runat="server" Text="Request" OnClick="btn_perm_Click" class="btn btn-primary"></asp:Button>
              </div>

           </fieldset>
         </div>   
      </div>
        <div class="col-md-2"></div>
   </div>
    
</div>


    <script>
        var today = new Date();

        $('.pickadate').pickadate({
            format: 'dd/mm/yyyy',
            min: today,
            selectMonths: true,
            selectYears: true,
            closeOnSelect: true,
            onSet: function(context) {
                if (context.select) {
                    var selectedDateStr = this.get('select', 'dd/mm/yyyy');
                    checkLeave(selectedDateStr);
                }
            }
        });

        function checkLeave(dateStr) {
            if (!dateStr) return;
            $.ajax({
                type: "POST",
                url: "PermissionRequest.aspx/CheckLeaveForDate",
                data: JSON.stringify({ dateStr: dateStr }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    if (data.d === "true") {
                        $('#lbl_leave_warning').show();
                        $('#<%= btn_request.ClientID %>').prop('disabled', true);
                    } else {
                        $('#lbl_leave_warning').hide();
                        $('#<%= btn_request.ClientID %>').prop('disabled', false);
                    }
                },
                error: function () {
                    // Ignore error silently
                }
            });
        }
    </script>



</asp:Content>

