<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Assigntesting.aspx.cs" Inherits="Employee_Assigntesting" enableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel panel-flat">
        <div class="panel-heading">
            <h5 class="panel-title">Testing Management</h5>
        </div>

        <div class="panel-body">
            <fieldset>
                <legend class="text-semibold">
                    <i class="icon-task position-left"></i>Assign Testing
                </legend>

                <!-- Row 1 -->
                <div class="row">
                    <asp:HiddenField ID="hfTestingKey" runat="server" />
                    <asp:HiddenField ID="hfEmployeeKey" runat="server" />
                    <asp:HiddenField ID="hfHoursValid" runat="server" Value="true" />
                    <asp:HiddenField ID="hfEndDate" runat="server" />
                    <div class="col-md-4">
                        <label>Project Name <span style="color: red">*</span></label>
                        <asp:DropDownList ID="ddlProject" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="rfvProject" runat="server" ControlToValidate="ddlProject" InitialValue="" ErrorMessage="Select Project" ForeColor="Red" />
                    </div>
                    <div class="col-md-4">
                        <label>Task Name </label>
                        <asp:DropDownList ID="ddlTaskName" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                        <asp:HiddenField ID="hfTaskKey" runat="server" />
                        <asp:HiddenField ID="hfTaskName" runat="server" />
                    </div>
                    <div class="col-md-4">
                        <label>Team Members </label>
                        <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                </div>

                <!-- Row 2 -->
                <div class="row">
                    <div class="col-md-4">
                        <label>Work Type</label>
                        <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control" Enabled="false">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-4">
                        <label>Start Date <span style="color: red">*</span></label>
                        <div class="input-group">
                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control pickadate"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtStartDate" ErrorMessage="Select Start Date" ForeColor="Red" />
                    </div>
                    <div class="col-md-4">
                        <label>End Date <span style="color: red">*</span></label>
                        <div class="input-group">
                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control pickadate" Enabled="false"></asp:TextBox>
                        </div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtEndDate" ErrorMessage="Select End Date" ForeColor="Red" />
                    </div>
                </div>

                <!-- Row 3 -->
                <div class="row">
                    <div class="col-md-4">
                        <label>Assigned Hours <span style="color: red">*</span></label>
                        <asp:TextBox ID="txtHours" runat="server" CssClass="form-control" placeholder="Enter Hours" type="number" min="0" step="1"></asp:TextBox>
                        <%-- ✅ FIX #4: inline style நீக்கி CSS class use பண்றோம் --%>
                        <asp:Label ID="lblTotalHours" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Smaller" CssClass="hours-warning"></asp:Label>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtHours" ErrorMessage="Enter assigned Hours" ForeColor="Red" />
                    </div>
                    <div class="col-md-4">
                        <label>Actual Hours</label>
                        <asp:TextBox ID="txtActualHours" runat="server" CssClass="form-control" placeholder="Enter Actual Hours" type="number" min="0" step="1"></asp:TextBox>
                    </div>
                    <div class="col-md-4">
                        <label>Status <span style="color: red">*</span></label>
                        <asp:DropDownList ID="ddlTaskStatus" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ddlTaskStatus" InitialValue="" ErrorMessage="Select task status" ForeColor="Red" />
                    </div>

                </div>
                <div class="row">
                                        <div class="col-md-6">
                        <label>Task Description</label>
                        <asp:TextBox ID="txtTaskDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" ReadOnly="true"></asp:TextBox>
                    </div>

                                                            <div class="col-md-6">
                        <label>Test Description <span style="color: red">*</span></label>
                        <asp:TextBox ID="txtTestDescription" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" placeholder="Enter Test Description"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtTestDescription" ErrorMessage="Enter Test Description" ForeColor="Red" />
                    </div>

                </div>
                <br />

                <!-- Buttons -->
                <div class="row">
                    <div class="form-group text-right">
                        <a id="btnBack" runat="server" class="btn btn-primary" href="Assigntestings.aspx">Back</a>
                        <asp:Button ID="btnSaveTesting" runat="server" Text="Save Testing" CssClass="btn btn-primary" OnClick="btnSaveTesting_Click" CausesValidation="true" />
                    </div>
                </div>
            </fieldset>
        </div>
    </div>

    <style>
        .hours-warning { display: none; }
    </style>

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
                $('#<%= txtEndDate.ClientID %>').prop('disabled', false);
                $('#<%= txtEndDate.ClientID %>').val(startDate);
                
                // Store in hidden field for postback
                $('#<%= hfEndDate.ClientID %>').val(startDate);
                
                if (endDatePicker) {
                    var parts = startDate.split('/');
                    var minDate = new Date(parts[2], parts[1] - 1, parts[0]);
                    endDatePicker.set('min', minDate);
                    endDatePicker.set('select', minDate);
                }
                
                setTimeout(function() {
                    $('#<%= txtEndDate.ClientID %>').prop('disabled', true);
                }, 100);
            }
            checkAssignedHours(); 
        });
        
        $('#<%= txtHours.ClientID %>').on('input', function () { checkAssignedHours(); });

        $('#<%= ddlEmployee.ClientID %>').change(function () {
            var employeeKey = $(this).val();
            $('#<%= hfEmployeeKey.ClientID %>').val(employeeKey);
            checkAssignedHours();
        });

        function checkAssignedHours() {
            var employeeKey = $('#<%= ddlEmployee.ClientID %>').val();
            var startDate = $('#<%= txtStartDate.ClientID %>').val();
            var hours = $('#<%= txtHours.ClientID %>').val();
            var testingKey = $('#<%= hfTestingKey.ClientID %>').val() || '0';
            var $lbl = $('#<%= lblTotalHours.ClientID %>');

            if (!employeeKey || !startDate || !hours) {
                $lbl.hide().text('');
                $('#<%= hfHoursValid.ClientID %>').val('true');
                return;
            }

            $.ajax({
                type: 'POST',
                url: 'Assigntesting.aspx/CheckHours',
                contentType: 'application/json',
                data: JSON.stringify({ employeeKey: employeeKey, startDate: startDate, hours: parseInt(hours), testingKey: parseInt(testingKey) }),
                success: function (res) {
                    if (res.d) {
                        $lbl.text(res.d).show();
                        $('#<%= hfHoursValid.ClientID %>').val('false');
                    } else {
                        $lbl.hide().text('');
                        $('#<%= hfHoursValid.ClientID %>').val('true');
                    }
                }
            });
        }

        $('#<%= ddlTaskName.ClientID %>').change(function () {
            var taskKey = $(this).val();
            var taskText = $(this).find('option:selected').text();
            // ✅ Store task key and task name in hidden fields for postback
            $('#<%= hfTaskKey.ClientID %>').val(taskKey);
            $('#<%= hfTaskName.ClientID %>').val(taskText);
            var $desc = $('#<%= txtTaskDescription.ClientID %>');

            $desc.val('');

            if (!taskKey) return;

            $.ajax({
                type: 'POST',
                url: 'Assigntesting.aspx/GetTaskDescription',
                contentType: 'application/json',
                data: JSON.stringify({ taskKey: parseInt(taskKey) }),
                success: function (res) {
                    if (res.d) {
                        $desc.val(res.d);
                    }
                }
            });
        });

        $('#<%= ddlProject.ClientID %>').change(function () {
            var projectKey = $(this).val();
            var $task = $('#<%= ddlTaskName.ClientID %>');
            var $emp = $('#<%= ddlEmployee.ClientID %>');

            $task.empty().append('<option value="">-- Select Task --</option>');
            $emp.empty().append('<option value="">-- Select Employee --</option>');

            // ✅ FIX #3: Project change-ல hfEmployeeKey & hfHoursValid reset பண்றோம்
            $('#<%= hfEmployeeKey.ClientID %>').val('');
            $('#<%= hfHoursValid.ClientID %>').val('true');
            $('#<%= lblTotalHours.ClientID %>').hide().text('');

            if (!projectKey) return;

            // Load Tasks
            $.ajax({
                type: 'POST',
                url: 'Assigntesting.aspx/GetTasks',
                contentType: 'application/json',
                data: JSON.stringify({ projectKey: parseInt(projectKey) }),
                success: function (res) {
                    $.each(res.d, function (i, task) {
                        $task.append('<option value="' + task.Value + '">' + task.Text + '</option>');
                    });
                    // reset hfTaskKey and hfTaskName when project changes
                    $('#<%= hfTaskKey.ClientID %>').val('');
                    $('#<%= hfTaskName.ClientID %>').val('');
                }
            });

            // Load Employees (now filtered by project)
            $.ajax({
                type: 'POST',
                url: 'Assigntesting.aspx/GetEmployees',
                contentType: 'application/json',
                data: JSON.stringify({ projectKey: parseInt(projectKey) }),
                success: function (res) {
                    $.each(res.d, function (i, emp) {
                        $emp.append('<option value="' + emp.Value + '">' + emp.Text + '</option>');
                    });
                }
            });
        });

        function loadTasksAndEmployees(projectKey, taskKey, employeeKey) {
            $.ajax({
                type: 'POST',
                url: 'Assigntesting.aspx/GetTasks',
                contentType: 'application/json',
                data: JSON.stringify({ projectKey: projectKey }),
                success: function (res) {
                    var $task = $('#<%= ddlTaskName.ClientID %>');
                $task.empty().append('<option value="">-- Select Task --</option>');
                $.each(res.d, function(i, task) {
                    $task.append('<option value="' + task.Value + '">' + task.Text + '</option>');
                });
                $task.val(taskKey);
                $task.trigger('change');
            }
        });

        $.ajax({
            type: 'POST',
            url: 'Assigntesting.aspx/GetEmployees',
            contentType: 'application/json',
            data: JSON.stringify({ projectKey: projectKey }),
            success: function(res) {
                var $emp = $('#<%= ddlEmployee.ClientID %>');
                $emp.empty().append('<option value="">-- Select Employee --</option>');
                $.each(res.d, function (i, emp) {
                    $emp.append('<option value="' + emp.Value + '">' + emp.Text + '</option>');
                });
                $emp.val(employeeKey);
            }
        });
        }
    </script>

</asp:Content>

     

