<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Createtask.aspx.cs" Inherits="Employee_Createtask" enableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel panel-flat">
        <div class="panel-heading">
            <h5 class="panel-title">Task Management</h5>
        </div>

        <div class="panel-body">
            <fieldset>
                <legend class="text-semibold">
                    <i class="icon-task position-left"></i>Create Task
                </legend>

                <!-- Row 1 -->
                <div class="row">
                    <div class="col-md-4">
                        <label>Task Name <span style="color: red">*</span></label>
                        <asp:TextBox ID="txtTaskName" runat="server" CssClass="form-control" placeholder="Enter Task Name"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvTaskName" runat="server" ControlToValidate="txtTaskName" ErrorMessage="Enter Task Name" ForeColor="Red" />
                    </div>
                    <asp:HiddenField ID="hfTaskKey" runat="server" />
                    <asp:HiddenField ID="hfEmployeeKey" runat="server" />
                    <asp:HiddenField ID="hfHoursValid" runat="server" Value="true" />
                    <asp:HiddenField ID="hfEndDate" runat="server" />
                    <div class="col-md-4">
                        <label>Project Name <span style="color: red">*</span></label>
                        <asp:DropDownList ID="ddlProject"
                            runat="server"
                            CssClass="form-control">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator
                            ID="rfvProject"
                            runat="server"
                            ControlToValidate="ddlProject"
                            InitialValue="0"
                            ErrorMessage="Select Project"
                            ForeColor="Red" />
                    </div>
                    <div class="col-md-4">
                        <label>Team Members <span style="color: red">*</span></label>
                        <asp:DropDownList ID="ddlEmployee"
                            runat="server"
                            CssClass="form-control">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlEmployee" InitialValue="0" ErrorMessage="Select Team members" ForeColor="Red" />
                    </div>
                </div>

                <!-- Row 2 -->
                <div class="row">
                    <div class="col-md-4">
                        <label>Work Type</label>
                        <asp:DropDownList ID="ddlRole"
                            runat="server"
                            CssClass="form-control">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlRole" InitialValue="0" ErrorMessage="Select work type" ForeColor="Red" />
                    </div>
                    <div class="col-md-4">
                        <label>Start Date <span style="color: red">*</span></label>
                        <div class="input-group">
                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                            <asp:TextBox ID="txtStartDate"
                                runat="server"
                                CssClass="form-control pickadate"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtStartDate" ErrorMessage="Select Start Date" ForeColor="Red" />
                    </div>
                    <div class="col-md-4">
                        <label>End Date <span style="color: red">*</span></label>
                        <div class="input-group">
                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                            <asp:TextBox ID="txtEndDate"
                                runat="server"
                                CssClass="form-control pickadate"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEndDate" ErrorMessage="Select End Date" ForeColor="Red" />
                    </div>
                </div>

                <!-- Row 3 -->
                <div class="row">
                    <div class="col-md-4">
                        <label>Assigned Hours <span style="color: red">*</span></label>
                        <asp:TextBox ID="txtHours"
                            runat="server"
                            CssClass="form-control"
                            placeholder="Enter Hours"
                            type="number"
                            min="0"
                            step="1"></asp:TextBox>
                        <asp:Label ID="lblTotalHours"
                            runat="server"
                            ForeColor="Red"
                            Font-Bold="true"
                            Font-Size="Smaller"
                            Style="display:none;">
                        </asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtHours" ErrorMessage="Enter assigned Hours" ForeColor="Red" />
                    </div>
                    <div class="col-md-4">
                        <label>Actual Hours <span id="spanActualRequired" style="color: red; display:none;">*</span></label>
                        <asp:TextBox ID="txtActualHours"
                            runat="server"
                            CssClass="form-control"
                            placeholder="Enter Actual Hours"
                            type="number"
                            min="0"
                            step="1"></asp:TextBox>
                        <asp:Label ID="lblActualHoursError" runat="server" ForeColor="Red" Visible="false" Style="font-size: 12px; display: block; margin-top: 5px;"></asp:Label>
                        <asp:RequiredFieldValidator 
                            ID="rfvActualHours" 
                            runat="server" 
                            ControlToValidate="txtActualHours" 
                            ErrorMessage="Actual Hours is required" 
                            ForeColor="Red"
                            Display="Dynamic"
                            Enabled="false" />
                    </div>
                    <div class="col-md-4">
                        <label>Status</label>
                        <asp:DropDownList ID="ddlTaskStatus"
                            runat="server"
                            CssClass="form-control">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlTaskStatus" InitialValue="0" ErrorMessage="Select task status" ForeColor="Red" />
                    </div>
                </div>
                
                <!-- Row 4 -->
                <div class="row">
                    <div class="col-md-12">
                        <label>Task Description</label>
                        <asp:TextBox ID="txtTaskDescription"
                            runat="server"
                            CssClass="form-control"
                            TextMode="MultiLine"
                            Rows="4"
                            placeholder="Enter Task Description"></asp:TextBox>
                    </div>
                </div>
                <br />

                <!-- Error / Success messages -->
                <div class="row">
                    <div class="col-md-12">
                        <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                        <asp:Label ID="lblSuccess" runat="server" ForeColor="Green"></asp:Label>
                    </div>
                </div>
                
                <!-- Buttons -->
                <div class="row" style="margin-top: 20px;">
                    <div class="col-md-12 text-right">
                        <a id="btnBack" runat="server" class="btn btn-default" style="margin-right: 10px;">
                            <i class="glyphicon glyphicon-arrow-left" style="margin-right: 6px;"></i>Back
                        </a>
                        <asp:Button ID="btnSaveTask" runat="server"
                            Text="Save Task"
                            CssClass="btn btn-success"
                            OnClick="btnSaveTask_Click" />
                        <asp:Button ID="btnUpdateTask" runat="server"
                            Text="Update Task"
                            CssClass="btn btn-primary"
                            OnClick="btnUpdateTask_Click"
                            Visible="false" />
                    </div>
                </div>
            </fieldset>
        </div>
    </div>

    <script>
        var endDatePicker;
        
        $('.pickadate').pickadate({
            format: 'dd/mm/yyyy',
            formatSubmit: 'dd/mm/yyyy',
            hiddenName: false
        });
        
        endDatePicker = $('#<%= txtEndDate.ClientID %>').pickadate('picker');

        $('#<%= txtStartDate.ClientID %>').change(function () { 
            var startDate = $(this).val();
            if (startDate) {
                // Set End Date to Start Date if empty
                if (!$('#<%= txtEndDate.ClientID %>').val()) {
                    $('#<%= txtEndDate.ClientID %>').val(startDate);
                }
                
                // Store in hidden field for postback
                $('#<%= hfEndDate.ClientID %>').val(startDate);
                
                // Set minimum date for End Date picker
                if (endDatePicker) {
                    var parts = startDate.split('/');
                    var minDate = new Date(parts[2], parts[1] - 1, parts[0]);
                    endDatePicker.set('min', minDate);
                    
                    // Only set End Date if it's before Start Date
                    var currentEndDate = $('#<%= txtEndDate.ClientID %>').val();
                    if (currentEndDate) {
                        var endParts = currentEndDate.split('/');
                        var endDateObj = new Date(endParts[2], endParts[1] - 1, endParts[0]);
                        if (endDateObj < minDate) {
                            endDatePicker.set('select', minDate);
                        }
                    } else {
                        endDatePicker.set('select', minDate);
                    }
                }
            }
        });
        
        // 8 hours per day restriction removed
        //$('#<%= txtHours.ClientID %>').on('input', function () { checkAssignedHours(); });

        // Page load - restore employee selection
        $(document).ready(function() {
            var empKey = $('#<%= hfEmployeeKey.ClientID %>').val();
            if (empKey) {
                $('#<%= ddlEmployee.ClientID %>').val(empKey);
            }
        });

        $('#<%= ddlEmployee.ClientID %>').change(function () {
            var employeeKey = $(this).val();
            $('#<%= hfEmployeeKey.ClientID %>').val(employeeKey);
            // 8 hours per day restriction removed
            //checkAssignedHours();
        });

        // 8 hours per day restriction removed - function commented out
        //function checkAssignedHours() {
        //    var employeeKey = $('#<%= ddlEmployee.ClientID %>').val();
        //    var startDate = $('#<%= txtStartDate.ClientID %>').val();
        //    var hours = $('#<%= txtHours.ClientID %>').val();
        //    var taskKey = $('#<%= hfTaskKey.ClientID %>').val() || '0';
        //    var $lbl = $('#<%= lblTotalHours.ClientID %>');

        //    if (!employeeKey || !startDate || !hours) { 
        //        $lbl.hide().text('');
        //        $('#<%= hfHoursValid.ClientID %>').val('true');
        //        return; 
        //    }

        //    $.ajax({
        //        type: 'POST',
        //        url: 'Createtask.aspx/CheckHours',
        //        contentType: 'application/json',
        //        data: JSON.stringify({ employeeKey: employeeKey, startDate: startDate, hours: parseInt(hours), taskKey: parseInt(taskKey) }),
        //        success: function (res) {
        //            if (res.d) {
        //                $lbl.text(res.d).show();
        //                $('#<%= hfHoursValid.ClientID %>').val('false');
        //            } else {
        //                $lbl.hide().text('');
        //                $('#<%= hfHoursValid.ClientID %>').val('true');
        //            }
        //        }
        //    });
        //}

        $('#<%= ddlProject.ClientID %>').change(function () {
            var projectKey = $(this).val();
            
            // Update back button with current project key
            if (projectKey) {
                $('#<%= btnBack.ClientID %>').attr('href', 'taskgrids.aspx?id=' + encodeURIComponent(projectKey));
            } else {
                $('#<%= btnBack.ClientID %>').attr('href', 'taskgrids.aspx');
            }
        });

        // Status dropdown change event - Make Actual Hours mandatory for Completed status
        $('#<%= ddlTaskStatus.ClientID %>').change(function () {
            var statusValue = $(this).val();
            var $actualHours = $('#<%= txtActualHours.ClientID %>');
            var $validator = $('#<%= rfvActualHours.ClientID %>');
            var $requiredStar = $('#spanActualRequired');
            
            // Status = 4 (Completed)
            if (statusValue == '4') {
                $actualHours.prop('required', true);
                $requiredStar.show();
                // Enable validator
                if ($validator.length > 0 && typeof ValidatorEnable === 'function') {
                    ValidatorEnable($validator[0], true);
                }
            } else {
                $actualHours.prop('required', false);
                $requiredStar.hide();
                // Disable validator
                if ($validator.length > 0 && typeof ValidatorEnable === 'function') {
                    ValidatorEnable($validator[0], false);
                }
            }
        });

        // Page load - check initial status
        $(document).ready(function() {
            $('#<%= ddlTaskStatus.ClientID %>').trigger('change');
        });
    </script>

</asp:Content>

