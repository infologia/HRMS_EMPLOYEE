<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Meetingdetails.aspx.cs" Inherits="Employee_Meetingdetails" enableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        /* Make all readonly inputs look same */
.view-mode input[readonly],
.view-mode textarea {
    background-color: #f5f5f5;
    cursor: not-allowed;
}

/* Disable date & time picker click */
.view-mode .pickatime-clear,
.view-mode .daterange-single {
    pointer-events: none;
}

/* Multiselect disable (IMPORTANT) */
.view-mode .multiselect {
    pointer-events: none;
    opacity: 0.6;
}

    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form-horizontal">
        <!-- Basic layout-->
        <div class="panel panel-flat">
            <div class="panel-heading">
                <h5 class="panel-title"></h5>
            </div>
            <div class="panel-body">
                <legend class="text-semibold"><i class="icon-reading position-left"></i>Create Meeting</legend>
                <div class="row">
                    <div class="col-md-4">
                        <label>Meeting Title<span style="color: red"> *</span></label>
                        <asp:TextBox ID="txt_MeetingTitle" runat="server" Class="form-control" placeholder="Enter Meeting Title"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator_name" runat="server" ControlToValidate="txt_MeetingTitle" ErrorMessage="Enter meeting title" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-4">
                        <label>Meeting Date<span style="color: red"> *</span></label>
                        <asp:TextBox ID="txt_MeetingDate" runat="server" Class="form-control pickadate" placeholder="DD/MM/YYYY"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_MeetingDate" ErrorMessage="Select Meeting Date" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-4">
                        <label>Meeting Type</label>
                        <asp:DropDownList ID="ddl_meetingtype" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddl_meetingtype_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem Text="Online" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Offline" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Client Call" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Internal" Value="4"></asp:ListItem>
                            <asp:ListItem Text="Leads Call" Value="5"></asp:ListItem>
                            <asp:ListItem Text="Bussiness Meettings" Value="6"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                            <label>Start Time<span style="color: red"> *</span></label>
                            <div class="input-group">
                                <span class="input-group-addon"><i class="icon-alarm"></i></span>
                                <asp:TextBox ID="txt_starttime" runat="server" CssClass="form-control pickatime-clear" required="" Placeholder="HH:mm"></asp:TextBox>
                            </div>
                        
                    </div>
                    <div class="col-md-4">
                            <label>End Time<span style="color: red"> *</span></label>
                            <div class="input-group">
                                <span class="input-group-addon"><i class="icon-alarm"></i></span>
                                <asp:TextBox ID="txt_endtime" runat="server" class="form-control pickatime-clear" required="" Placeholder="HH:mm"></asp:TextBox>
                            </div>
                       
                    </div>

                    <div class="col-md-4">
                        <label>Meeting Link</label>
                        <asp:TextBox ID="txt_MeetingLink" runat="server" Class="form-control" placeholder="Enter meeting link" requried="" ></asp:TextBox>
                    </div>
          </div>
                <br />
                <div class="row">
                    <div class="col-md-4">
                        <label>Meeting Status</label>
                        <asp:DropDownList ID="ddl_status" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Scheduled" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Completed" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Cancelled" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Postponed" Value="4"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <%--<div class="col-md-4">
                        <label>Participations</label>
                        <asp:DropDownList ID="ddl_employee" runat="server" CssClass="multiselect dropdown-toggle btn btn-default" data-toggle="dropdown">
                        </asp:DropDownList>
                    </div>--%>
                    <div class="col-md-4">
                        <label>Project Participants</label>
                        <div class="multi-select-full">
                          <asp:ListBox 
                            ID="ddl_employee"
                            runat="server"
                            CssClass="multiselect form-control"
                            SelectionMode="Multiple"   onchange="onEmployeeChange();">
                         </asp:ListBox>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <label>Meeting Description</label>
                        <textarea id="txt_Description" runat="server" class="form-control" placeholder="Meeting notes"></textarea>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <label>Fathom Details</label>
                        <textarea id="txt_FathomDetails" runat="server" class="form-control" placeholder="Enter Fathom Details"></textarea>
                    </div>
                    <div class="col-md-4" runat="server" id="div_client" visible="false">
                        <label>Select Client</label>
                        <asp:DropDownList ID="ddl_Client" runat="server" CssClass="form-control" onchange="loadProjects();">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-4" runat="server" id="div_projects" visible="false">
                        <label>Select Project</label>
                        <asp:DropDownList ID="ddl_Projects" runat="server" CssClass="form-control" onchange="setProjectKey();">
                        </asp:DropDownList>
                    </div>
                      <div class="col-md-4" runat="server" id="div_leads" visible="false">
      <label>Select Lead</label>
      <asp:DropDownList ID="ddl_Leads" runat="server" CssClass="form-control">
      </asp:DropDownList>
  </div>
                </div>
                <div class="row" style="margin-top: 25px;">
                    <div class="text-right">
                        <a href="Meetings.aspx" class="btn btn-primary">Back</a>
                        <asp:Button ID="btn_request" runat="server" Text="Create" Class="btn btn-primary" OnClick="btn_request_Click" Visible="false" Style="margin-right: 15px"></asp:Button>
                        <asp:Button ID="btn_update" runat="server" Text="Update" Class="btn btn-primary" OnClick="btn_update_Click" Visible="false" Style="margin-right: 15px"></asp:Button>
                    </div>
                </div>
            </div>
        </div>
  </div>


    <asp:HiddenField ID="hfMeetingKey" runat="server" />
    <asp:HiddenField ID="UserKeys" runat="server" />
    <asp:HiddenField ID="hfProjectKey" runat="server" />

<!-- Conflict Modal (using template's danger modal) -->
<div id="modal_theme_danger" class="modal fade" tabindex="-1">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header bg-danger">
        <h5 class="modal-title">Meeting Conflict</h5>
        <button type="button" class="close" data-dismiss="modal">&times;</button>
      </div>
      <div class="modal-body">
        <p><strong>Employee:</strong> <span id="conflictEmployee"></span></p>
        <p><strong>Meeting Title:</strong> <span id="conflictTitle"></span></p>
        <p><strong>Date:</strong> <span id="conflictDate"></span></p>
        <p><strong>Time:</strong> <span id="conflictTime"></span></p>
        <p class="text-danger">This employee already has a meeting at this time so Please coordinate with the concerned employee <span id="EmployeeName"></span></p>
      </div>
      <div class="modal-footer">
<button id="btnContinue" type="button" class="btn btn-success">Remove</button>

      </div>
    </div>
  </div>
</div>

    <div id="ajaxLoader" style="display:none;
    position:fixed;
    top:0; left:0;
    width:100%; height:100%;
    background:rgba(0,0,0,0.4);
    z-index:1055;
    text-align:center;">
    <div style="position:absolute;top:50%;left:50%;transform:translate(-50%,-50%);">
        <i class="fa fa-spinner fa-spin fa-3x text-white"></i>
        <p style="color:#fff;margin-top:10px;">Checking meeting conflict...</p>
    </div>
</div>

    <div id="ajaxremoveLoader" style="display:none;
    position:fixed;
    top:0; left:0;
    width:100%; height:100%;
    background:rgba(0,0,0,0.4);
    z-index:1060;
    text-align:center;">
    <div style="position:absolute;top:50%;left:50%;transform:translate(-50%,-50%);">
        <i class="fa fa-spinner fa-spin fa-3x text-white"></i>
        <p style="color:#fff;margin-top:10px;">Processing...</p>
    </div>
</div>


    <div id="modal_theme_unselect" class="modal fade" tabindex="-1">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header bg-danger">
        <h5 class="modal-title">Meeting Conflict</h5>
        <button type="button" class="close" data-dismiss="modal">&times;</button>
      </div>
      <div class="modal-body">
        <p><strong>Employee:</strong> <span id="conflictEmployeeunselect"></span></p>
        <p><strong>Meeting Title:</strong> <span id="conflictTitleunselect"></span></p>
        <p><strong>Date:</strong> <span id="conflictDateunselect"></span></p>
        <p><strong>Time:</strong> <span id="conflictTimeunselect"></span></p>
        <p class="text-danger">Are you sure you want to remove this employee? <span id="EmployeeNameunselect"></span></p>
      </div>
      <div class="modal-footer">
<button id="btnContinueunselect" type="button" class="btn btn-danger">Remove</button>
          <button id="btnContinueselect" type="button" class="btn btn-success">Cancel</button>

      </div>
    </div>
  </div>
</div>

           <script>
               var conflictEmployeeKey = null; 
               var previousEmployees = [];

               function onEmployeeChange() {

                   var ddlEmployees = $('#<%= ddl_employee.ClientID %>');
    var currentEmployees = ddlEmployees.val() || [];

    if (currentEmployees.length < previousEmployees.length) {
        
        previousEmployees = currentEmployees.slice();
        return;
    }

    
    if (currentEmployees.length === previousEmployees.length) return;

    var lastSelectedEmployee = currentEmployees[currentEmployees.length - 1];

    var meetingDate = $('#<%= txt_MeetingDate.ClientID %>').val();
    var startTime   = $('#<%= txt_starttime.ClientID %>').val();
                   var endTime = $('#<%= txt_endtime.ClientID %>').val();

                   if (!meetingDate || !startTime || !endTime) {
                       previousEmployees = currentEmployees.slice();
                       return;
                   }

                   $.ajax({
                       type: "POST",
                       url: "Meetingdetails.aspx/CheckMeetingConflict",
                       data: JSON.stringify({
                           employeeKey: lastSelectedEmployee,
                           meetingDate: meetingDate,
                           startTime: meetingDate + " " + startTime,
                           endTime: meetingDate + " " + endTime,
                           meetingKey: $('#<%= hfMeetingKey.ClientID %>').val() || 0
                       }),
                       contentType: "application/json; charset=utf-8",
                       dataType: "json",
                       beforeSend: function () {
                           $('#ajaxLoader').show();
                       },
                       success: function (res) {

                           if (!res.d || res.d.conflict !== true) {
                               previousEmployees = currentEmployees.slice();
                               return;
                           }

                           $('#conflictEmployee').text(res.d.employeeName);
                           $('#EmployeeName').text(res.d.employeeName);
                           $('#conflictTitle').text(res.d.title);
                           $('#conflictDate').text(res.d.date);
                           $('#conflictTime').text(res.d.time);

                           conflictEmployeeKey = lastSelectedEmployee;

                           $('#modal_theme_danger').modal({
                               backdrop: 'static',
                               keyboard: false
                           }).modal('show');
                       },
                       complete: function () {
                           $('#ajaxLoader').hide();
                           previousEmployees = ddlEmployees.val() || [];
                       }
                   });
               }
               $('#btnContinue').off('click').on('click', function () {
                   var ddlEmployees = $('#<%= ddl_employee.ClientID %>');
    var selectedEmployees = ddlEmployees.val() || [];

    if (!conflictEmployeeKey) {
        $('#modal_theme_danger').modal('hide');
        return;
    }

    selectedEmployees = selectedEmployees.filter(function (e) {
        return e !== conflictEmployeeKey;
    });

    ddlEmployees.val(selectedEmployees);

    previousEmployees = selectedEmployees.slice();

    if (typeof ddlEmployees.multiselect === 'function') {
        ddlEmployees.multiselect('refresh');
    }
    // Programmatic .val() doesn't fire native 'change', so trigger it manually
    // to keep the "X selected" button text in sync after this conflict-resolution update.
    ddlEmployees.trigger('change');

    conflictEmployeeKey = null;
    $('#modal_theme_danger').modal('hide');
});

           </script>
    <script>
        $(document).ready(function () {
            var ddlEmployees = $('#<%= ddl_employee.ClientID %>');
            previousEmployees = ddlEmployees.val() || [];

            if (typeof ddlEmployees.multiselect === 'function') {
                ddlEmployees.multiselect('refresh');
            }
    });
    </script>
    <script>
        // Force "X selected" text for Project Participants, instead of bootstrap-multiselect's
        // default (names for <=3 selected, count for 4+). Same fix as Project.aspx.
        // Uses window 'load' (not document 'ready') so the plugin's .btn-group/.multiselect-container
        // markup is guaranteed to already exist when we bind to it.
        $(window).on('load', function () {
            function forceCountText(selectId) {
                var $select = $(selectId);
                var $btnGroup = $select.next('.btn-group');

                if ($btnGroup.length === 0) {
                    setTimeout(function () { forceCountText(selectId); }, 200);
                    return;
                }

                function updateText() {
                    var count = $select.find('option:selected').length;
                    $btnGroup.find('.multiselect-selected-text').text(count === 0 ? 'None selected' : count + ' selected');
                }

                // 'change' (not 'click') because these checkboxes are styled by jQuery Uniform -
                // the real click lands on Uniform's overlay, not the native <input>, but 'change'
                // still fires reliably. No setTimeout delay, so it updates in the same tick as
                // the plugin's own button-text update (avoids a name->count flicker).
                $btnGroup.on('change', '.multiselect-container input[type="checkbox"]', function () {
                    updateText();
                });

                $select.on('change', updateText);
                updateText();
            }

            forceCountText('#<%= ddl_employee.ClientID %>');
        });
    </script>
      <script>
          $('.pickadate').pickadate({
              format: 'dd/mm/yyyy',
              selectMonths: true,
              selectYears: true,
              closeOnSelect: true
          });
      </script>

      <script>
          function loadProjects() {
              var clientKey = $('#<%= ddl_Client.ClientID %>').val();
              if (!clientKey) {
                  $('#<%= ddl_Projects.ClientID %>').empty().append('<option value="">-- Select Project --</option>');
                  $('#<%= hfProjectKey.ClientID %>').val('');
                  return;
              }
              $.ajax({
                  type: "POST",
                  url: "Meetingdetails.aspx/GetProjectsByClient",
                  data: JSON.stringify({ clientKey: clientKey }),
                  contentType: "application/json; charset=utf-8",
                  dataType: "json",
                  success: function (res) {
                      var ddl = $('#<%= ddl_Projects.ClientID %>');
                      ddl.empty().append('<option value="">-- Select Project --</option>');
                      $.each(res.d, function (i, item) {
                          ddl.append($('<option></option>').val(item.ProjectKey).text(item.ProjectName));
                      });
                  }
              });
          }

          function setProjectKey() {
              var projectKey = $('#<%= ddl_Projects.ClientID %>').val();
              $('#<%= hfProjectKey.ClientID %>').val(projectKey);
          }
      </script>
   

</asp:Content>

