<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Createtasknew.aspx.cs" Inherits="Employee_Createtask" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .status-assigned {
            background-color: #17a2b8 !important;  /* info - cyan */
            color: white !important;
            font-weight: 600;
        }
        .status-ongoing {
            background-color: #2196f3 !important;  /* blue */
            color: white !important;
            font-weight: 600;
        }
        .status-completed {
            background-color: #4caf50 !important;  /* green */
            color: white !important;
            font-weight: 600;
        }
        /* Row edit mode styles */
        .row-view-mode .editable-field { display: none !important; }
        .row-view-mode .display-field  { display: block; }
        .row-edit-mode  .editable-field { display: block !important; }
        .row-edit-mode  .display-field  { display: none !important; }
        /* Status & Actual Hours: always a single live control, never swapped/duplicated regardless of row mode */
        .row-view-mode .always-on-field,
        .row-edit-mode .always-on-field { display: block !important; }
        .row-view-mode .always-on-field { opacity: 0.8; }
        .row-view-mode select.always-on-field { pointer-events: none; }
        /* Compact table rows */
        #taskTableBody td { padding: 2px 6px; vertical-align: middle; }
        #taskTableBody .form-control { height: 26px; padding: 2px 6px; font-size: 12px; line-height: 1.2; }
        #taskTableBody textarea.form-control { height: auto; min-height: 26px; padding: 3px 6px; }
        #taskTableBody select.form-control { padding-top: 2px; padding-bottom: 2px; }
        .table-task-details thead th { padding: 5px 6px !important; font-size: 12px; }
        .table-task-details.table-bordered > tbody > tr > td,
        .table-task-details.table-bordered > thead > tr > th { padding: 3px 6px; }
        /* Compact action buttons */
        #taskTableBody .btn-xs { padding: 1px 5px; font-size: 11px; }
        #taskTableBody .btn-xs i { font-size: 11px; }
        #taskTableBody .btn-sm { padding: 3px 8px; font-size: 12px; }
        #taskTableBody .btn-sm i { font-size: 12px; }
    </style>
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
                    <asp:HiddenField ID="hfTaskKey" runat="server" />
                    <asp:HiddenField ID="hfEmployeeKey" runat="server" />
                    <asp:HiddenField ID="hfHoursValid" runat="server" Value="true" />
                    <asp:HiddenField ID="hfEndDate" runat="server" />
                    <asp:HiddenField ID="hfViewMode" runat="server" Value="0" />
                    <asp:HiddenField ID="hfHasFullAccess" runat="server" Value="0" />
                    <div class="col-md-3">
                        <label>Work Day <span style="color: red">*</span></label>
                        <div class="input-group">
                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                            <asp:TextBox ID="txtStartDate"
                                runat="server"
                                CssClass="form-control pickadate">
                            </asp:TextBox>
                        </div>
                        <span id="lblDuplicateWarning" style="color:red; display:none; font-size:11px; font-weight:bold; margin-top:4px;">Task already exists for this date!</span>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtStartDate" ErrorMessage="Select Start Date" ForeColor="Red" />
                    </div>

                    <div class="col-md-3">
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
                    <div class="col-md-3">
                        <label>Team Members <span style="color: red">*</span></label>
                        <asp:DropDownList ID="ddlEmployee"
                            runat="server"
                            CssClass="form-control">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="ddlEmployee" InitialValue="0" ErrorMessage="Select Team members" ForeColor="Red" />
                    </div>

                    <div class="col-md-3">
                        <label>Work Type</label>
                        <asp:DropDownList ID="ddlRole"
                            runat="server"
                            CssClass="form-control">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="ddlRole" InitialValue="0" ErrorMessage="Select work type" ForeColor="Red" />
                    </div>
                    <!-- Row 2 -->
                    <!-- Row 2 - Task Details Table -->
                    <div class="row" style="margin-top: 15px;">
                        <div class="col-md-12">
                            <div style="margin-bottom: 10px; display: flex; align-items: center; justify-content: space-between;">
                                <label class="text-semibold" style="margin: 0;"> Task Details</label>
                                <asp:Button ID="btnAddRow" runat="server" 
                                    Text="Add Row" 
                                    CssClass="btn btn-primary btn-xs" 
                                    OnClientClick="addTaskRow(); return false;">
                                </asp:Button>
                            </div>

                            <div class="table-responsive">
                                <table class="table table-bordered table-striped table-hover" style="font-size:12px;">
                                    <thead class="bg-primary">
                                        <tr>
                                            <th style="width: 18%; padding: 4px 6px; font-size:11px;">Task Name</th>
                                            <th style="width: 22%; padding: 4px 6px; font-size:11px;">Task Description</th>
                                            <th style="width: 10%; padding: 4px 6px; font-size:11px; text-align:center;">Assigned Hours</th>
                                            <th style="width: 10%; padding: 4px 6px; font-size:11px; text-align:center;">Actual Hours</th>
                                            <th style="width: 14%; padding: 4px 6px; font-size:11px;">Status</th>
                                            <th style="width: 18%; padding: 4px 6px; font-size:11px;">Notes</th>
                                            <th style="width: 10%; padding: 4px 6px; font-size:11px;" class="text-center">Action</th>
                                        </tr>
                                    </thead>
                                    <tbody id="taskTableBody">
                                        <asp:Literal ID="ltTaskDetails" runat="server"></asp:Literal>
                                    </tbody>
                                </table>
                            </div>

                            <select id="statusTemplate" style="display: none">
                                <asp:Literal ID="ltStatusOptions" runat="server"></asp:Literal>
                            </select>
                            <select id="hoursTemplate" style="display: none">
                                <asp:Literal ID="ltHoursOptions" runat="server"></asp:Literal>
                            </select>
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
                                OnClientClick="return validateTaskRows();"
                                OnClick="btnSaveTask_Click" />
                            <asp:Button ID="btnUpdateTask" runat="server"
                                Text="Update Task"
                                CssClass="btn btn-primary"
                                OnClientClick="return validateTaskRows();"
                                OnClick="btnUpdateTask_Click"
                                Visible="false" />
                        </div>
                    </div>
            </fieldset>
        </div>
    </div>

    <script>
        let isDirty = false;

        var $dp = $('#<%= txtStartDate.ClientID %>');
        var existingVal = $dp.val();

        $dp.pickadate({
            format: 'dd/mm/yyyy',
            formatSubmit: 'dd/mm/yyyy',
            hiddenName: false,
            selectMonths: true,
            selectYears: 15,
            onSet: function(context) {
                if (context.select) {
                    setTimeout(checkDuplicateTask, 100);
                }
            },
            onOpen: function() {
                // Ensure picker reads current textbox value correctly
                var v = $dp.val();
                if (v) {
                    v = v.replace(/-/g, '/');
                    var picker = $dp.pickadate('picker');
                    if (picker && !picker.get('select')) {
                        picker.set('select', v, { format: 'dd/mm/yyyy', muted: true });
                    }
                }
            }
        });

        // If server pre-filled the date, normalize format and set into picker
        if (existingVal) {
            existingVal = existingVal.replace(/-/g, '/').replace(/\./g, '/');
            $dp.val(existingVal);
            var picker = $dp.pickadate('picker');
            if (picker) {
                picker.set('select', existingVal, { format: 'dd/mm/yyyy', muted: true });
            }
        }

        // Page load - restore employee selection
        $(document).ready(function () {
            var empKey = $('#<%= hfEmployeeKey.ClientID %>').val();
            if (empKey) {
                $('#<%= ddlEmployee.ClientID %>').val(empKey);
            }

            applyStatusColors();
            
            $(document).on('input keyup change', 'input[name="task_actual_hours"]', function() {
                validateActualHoursField(this);
            });

            // Lock all rows on page load - skip if create mode (no task key)
            var isCreateMode = $('#<%= hfTaskKey.ClientID %>').val() === '';
            if (!isCreateMode) {
                $('#taskTableBody tr').each(function() {
                    lockRow(this);
                });
            }

            // View mode: disable action buttons
            if ($('#<%= hfViewMode.ClientID %>').val() === '1') {
                // Hide the Add Row button
                $('button[onclick="addTaskRow()"]').hide();
                // Replace action buttons with disabled versions (readonly appearance)
                $('#taskTableBody tr').each(function() {
                    var td = $(this).find('td:last-child');
                    td.html(
                        '<button type="button" class="btn btn-default btn-xs" disabled style="margin-right:4px; opacity:0.5; cursor:not-allowed;" title="View only"><i class="glyphicon glyphicon-pencil"></i></button>' +
                        '<button type="button" class="btn btn-default btn-xs" disabled style="opacity:0.5; cursor:not-allowed;" title="View only"><i class="glyphicon glyphicon-trash"></i></button>'
                    );
                });
                // Lock all row inputs
                $('#taskTableBody tr').each(function() {
                    $(this).removeClass('row-edit-mode').addClass('row-view-mode');
                });
            }
        });

        function applyStatusColors() {
            $('select[name="task_status"], select.status-select').each(function() {
                updateStatusColor(this);
            }).off('change.statuscolor').on('change.statuscolor', function() {
                updateStatusColor(this);
                validateActualHoursField(this);
            });
        }

        function validateActualHoursField(element) {
            var row = $(element).closest('tr');
            var status = row.find('.status-select').val();
            var hoursInput = row.find('input[name="task_actual_hours"]');
            var errorLabel = row.find('.actual-hours-error');
            
            if (status === '4' && (!hoursInput.val() || parseFloat(hoursInput.val()) <= 0)) {
                errorLabel.show();
                hoursInput.css('border-color', 'red');
            } else {
                errorLabel.hide();
                hoursInput.css('border-color', '');
            }
        }

        function updateStatusColor(selectElement) {
            var val = $(selectElement).val();
            $(selectElement).removeClass('status-assigned status-ongoing status-completed');
            
            if (val == '1') {
                $(selectElement).addClass('status-assigned');
            } else if (val == '2') {
                $(selectElement).addClass('status-ongoing');
            } else if (val == '4') {
                $(selectElement).addClass('status-completed');
            }
        }

        $('#<%= ddlProject.ClientID %>').change(function () {
            var projectKey = $(this).val();
            if (projectKey) {
                $('#<%= btnBack.ClientID %>').attr('href', 'newtaskgrids.aspx?id=' + encodeURIComponent(projectKey));
            } else {
                $('#<%= btnBack.ClientID %>').attr('href', 'newtaskgrids.aspx');
            }
            checkDuplicateTask();
        });

        $('#<%= ddlEmployee.ClientID %>').change(function () {
            var employeeKey = $(this).val();
            $('#<%= hfEmployeeKey.ClientID %>').val(employeeKey);
            checkDuplicateTask();
        });

        function checkDuplicateTask() {
            var projectKey = $('#<%= ddlProject.ClientID %>').val();
            var employeeKey = $('#<%= hfEmployeeKey.ClientID %>').val() || $('#<%= ddlEmployee.ClientID %>').val();
            var startDate = $('#<%= txtStartDate.ClientID %>').val();
            var taskKey = $('#<%= hfTaskKey.ClientID %>').val() || 0;

            if (projectKey && employeeKey && startDate) {
                $.ajax({
                    type: "POST",
                    url: "createtasknew.aspx/CheckDuplicateTask",
                    data: JSON.stringify({ 
                        projectKey: parseInt(projectKey), 
                        startDate: startDate, 
                        employeeKey: employeeKey, 
                        taskKey: parseInt(taskKey) 
                    }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response.d === "1") {
                            $('#lblDuplicateWarning').show();
                            $('#<%= btnSaveTask.ClientID %>').prop('disabled', true);
                            $('#<%= btnUpdateTask.ClientID %>').prop('disabled', true);
                        } else {
                            $('#lblDuplicateWarning').hide();
                            $('#<%= btnSaveTask.ClientID %>').prop('disabled', false);
                            $('#<%= btnUpdateTask.ClientID %>').prop('disabled', false);
                        }
                    },
                    error: function () {
                        // ignore
                    }
                });
            } else {
                $('#lblDuplicateWarning').hide();
                $('#<%= btnSaveTask.ClientID %>').prop('disabled', false);
                $('#<%= btnUpdateTask.ClientID %>').prop('disabled', false);
            }
        }

        function addTaskRow() {
            // Check if in view mode
            if ($('#<%= hfViewMode.ClientID %>').val() === '1') {
                return false;
            }
            
            var tbody = document.getElementById('taskTableBody');
            var template = document.getElementById('statusTemplate');

            if (!template) {
                alert("Status template missing!");
                return;
            }

            var hasFullAccess = $('#<%= hfHasFullAccess.ClientID %>').val() === '1';
            var deleteBtnStr = hasFullAccess ? 
                '<button type="button" class="btn btn-danger btn-xs" onclick="removeTaskRow(this)" title="Delete Row"><i class="glyphicon glyphicon-trash"></i></button>' : 
                '<button type="button" class="btn btn-danger btn-xs" disabled style="opacity:0.5; cursor:not-allowed;" title="Delete Row"><i class="glyphicon glyphicon-trash"></i></button>';

            var statusOptions = template.innerHTML.trim();
            var hoursTemplate = document.getElementById('hoursTemplate');
            var hoursOptions = hoursTemplate ? hoursTemplate.innerHTML.trim() : '';
            var tr = document.createElement('tr');
            tr.className = 'row-edit-mode';

            tr.innerHTML =
                '<td style="padding:2px 6px;"><input type="text" class="form-control input-sm editable-field" name="task_name" placeholder="Enter Task Name" />'
                + '<span class="display-field" style="font-size:12px;"></span></td>'
                + '<td style="padding:2px 6px;"><textarea class="form-control editable-field" name="task_description" rows="1" style="resize:vertical;font-size:12px;"></textarea>'
                + '<textarea class="form-control display-field" name="task_description_display" rows="1" style="resize:vertical;font-size:12px;" readonly></textarea></td>'
                + '<td style="padding:2px 6px;text-align:center;"><select class="form-control input-sm editable-field" name="task_assigned_hours">' + hoursOptions + '</select>'
                + '<span class="display-field" style="font-size:12px;"></span></td>'
                + '<td style="padding:2px 6px;text-align:center;"><input type="number" class="form-control input-sm always-on-field" name="task_actual_hours" min="0" step="1" oninput="validateActualHoursField(this)" onchange="validateActualHoursField(this)" onkeyup="validateActualHoursField(this)" />'
                + '<div class="actual-hours-error" style="color:red; font-size:10px; display:none; font-weight:bold; margin-top:2px;">Required</div></td>'
                + '<td style="padding:2px 6px;"><select class="form-control input-sm always-on-field status-select" name="task_status">' + statusOptions + '</select></td>'
                + '<td style="padding:2px 6px;"><textarea class="form-control always-on-field" name="task_remarks" rows="1" placeholder="Notes" style="resize:vertical;font-size:12px;"></textarea></td>'
                + '<td class="text-center" style="white-space:nowrap;padding:2px 6px;">'
                + '<button type="button" class="btn btn-primary btn-xs btn-edit-row" onclick="editTaskRow(this)" title="Edit Row" style="margin-right:2px;"><i class="glyphicon glyphicon-pencil"></i></button>'
                + deleteBtnStr
                + '</td>';

            tbody.appendChild(tr);
            applyStatusColors();
            updateEditButton(tr.querySelector('.btn-edit-row'), true);
        }

        function lockRow(row) {
            row.querySelectorAll('.always-on-field').forEach(function(el) {
                if (el.tagName === 'SELECT') {
                    var hidden = el.parentNode.querySelector('input[data-status-backup]');
                    if (!hidden) {
                        hidden = document.createElement('input');
                        hidden.type = 'hidden';
                        hidden.name = 'task_status';
                        hidden.setAttribute('data-status-backup', '1');
                        el.parentNode.appendChild(hidden);
                    }
                    hidden.value = el.value;
                    el.setAttribute('disabled', 'disabled');
                    el.name = '';
                } else {
                    el.setAttribute('readonly', 'readonly');
                }
            });
            row.classList.remove('row-edit-mode');
            row.classList.add('row-view-mode');
        }

        function unlockRow(row) {
            row.querySelectorAll('.always-on-field').forEach(function(el) {
                el.removeAttribute('readonly');
                if (el.tagName === 'SELECT') {
                    el.removeAttribute('disabled');
                    el.name = 'task_status';
                    var hidden = el.parentNode.querySelector('input[data-status-backup]');
                    if (hidden) hidden.remove();
                }
            });
            row.classList.remove('row-view-mode');
            row.classList.add('row-edit-mode');
        }

        function editTaskRow(btn) {
            var row = btn.closest('tr');
            var isEditing = row.classList.contains('row-edit-mode');

            if (isEditing) {
                // copy editable-field values to display elements
                row.querySelectorAll('td').forEach(function(td) {
                    var input = td.querySelector('input.editable-field');
                    var textarea = td.querySelector('textarea.editable-field');
                    var displayTextarea = td.querySelector('textarea.display-field');
                    var displaySpan = td.querySelector('span.display-field');
                    var select = td.querySelector('select.editable-field');
                    if (textarea && displayTextarea) {
                        displayTextarea.value = textarea.value;
                    } else if (input && displaySpan) {
                        displaySpan.textContent = input.value;
                        displaySpan.style.cssText = 'display:inline-block; font-size:12px;';
                    } else if (select && displaySpan) {
                        var selectedText = select.options[select.selectedIndex] ? select.options[select.selectedIndex].text : '';
                        displaySpan.textContent = selectedText;
                        if (select.classList.contains('status-select')) {
                            var color = getStatusColor(select.value);
                            displaySpan.style.cssText = color
                                ? 'display:inline-block; padding:4px 10px; border-radius:4px; font-weight:600; color:white; background-color:' + color
                                : 'display:inline-block; font-size:12px;';
                        } else {
                            displaySpan.style.cssText = 'display:inline-block; font-size:12px;';
                        }
                    }
                });
                lockRow(row);
                updateEditButton(btn, false);
            } else {
                unlockRow(row);
                updateEditButton(btn, true);
                applyStatusColors();
            }
        }

        function updateEditButton(btn, isEditing) {
            if (isEditing) {
                btn.classList.remove('btn-primary');
                btn.classList.add('btn-success');
                btn.title = 'Save Row';
                btn.innerHTML = '<i class="glyphicon glyphicon-ok"></i>';
            } else {
                btn.classList.remove('btn-success');
                btn.classList.add('btn-primary');
                btn.title = 'Edit Row';
                btn.innerHTML = '<i class="glyphicon glyphicon-pencil"></i>';
            }
        }

        function getStatusColorClass(val) {
            var map = {'1':'status-assigned','2':'status-ongoing','4':'status-completed'};
            return map[val] || '';
        }

        function getStatusColor(val) {
            var map = {'1':'#17a2b8','2':'#2196f3','4':'#4caf50'};
            return map[val] || '';
        }

        function removeTaskRow(btn) {
            var tbody = document.getElementById('taskTableBody');
            var rowCount = tbody.getElementsByTagName('tr').length;

            if (rowCount <= 1) {
                alert("At least one row is required!");
                return;
            }

            if (!confirm("Are you sure you want to delete this task detail?")) return;

            isDirty = true;
            var row = btn.closest('tr');
            var taskDetailIdInput = row.querySelector("input[name='task_detail_id']");

            if (taskDetailIdInput && taskDetailIdInput.value !== "") {
                var hidden = document.createElement("input");
                hidden.type = "hidden";
                hidden.name = "deleted_task_detail_id";
                hidden.value = taskDetailIdInput.value;
                document.forms[0].appendChild(hidden);
            }

            row.remove();
        }

        function validateTaskRows() {
            var rows = document.querySelectorAll('#taskTableBody tr');
            var hasError = false;
            var errorMessages = [];
            var hasUnsavedRows = false;

            rows.forEach(function(row, index) {
                if (row.classList.contains('row-edit-mode')) {
                    hasUnsavedRows = true;
                }
            });

            if (hasUnsavedRows) {
                if (typeof toastr !== 'undefined') {
                    toastr.options = { positionClass: 'toast-bottom-right', timeOut: 4000, closeButton: true, escapeHtml: false };
                    toastr.warning('Please click the <i class="glyphicon glyphicon-ok"></i> button on all edited rows before saving.', 'Unsaved Rows');
                } else {
                    alert('Please click the ✓ button on all edited rows before saving.');
                }
                return false;
            }

            rows.forEach(function(row, index) {
                var rowNum = index + 1;
                var taskName = row.querySelector('input[name="task_name"]');
                var taskDesc = row.querySelector('textarea[name="task_description"]');
                var assignedHours = row.querySelector('select[name="task_assigned_hours"]');
                var taskStatus = row.querySelector('select[name="task_status"]');
                var taskStatusHidden = row.querySelector('input[data-status-backup]');
                var taskActualHours = row.querySelector('input[name="task_actual_hours"]');

                var nameVal = taskName ? taskName.value.trim() : '';
                var descVal = taskDesc ? taskDesc.value.trim() : '';
                var hoursVal = assignedHours ? assignedHours.value : '';
                var statusVal = taskStatus ? taskStatus.value : (taskStatusHidden ? taskStatusHidden.value : '');
                var actualHoursVal = taskActualHours ? taskActualHours.value.trim() : '';

                var rowErrors = [];
                if (!nameVal) rowErrors.push('Task Name');
                if (!descVal) rowErrors.push('Task Description');
                if (!hoursVal) rowErrors.push('Assigned Hours');
                if (!statusVal) rowErrors.push('Status');
                
                if (statusVal === '4' && (!actualHoursVal || parseFloat(actualHoursVal) <= 0)) {
                    rowErrors.push('Actual Hours (required for Completed status)');
                }

                if (rowErrors.length > 0) {
                    hasError = true;
                    errorMessages.push('Row ' + rowNum + ': ' + rowErrors.join(', ') + ' required');
                }
            });

            if (hasError) {
                if (typeof toastr !== 'undefined') {
                    toastr.options = {
                        positionClass: 'toast-top-center',
                        timeOut: 4000,
                        closeButton: true
                    };
                    errorMessages.forEach(function(msg) {
                        toastr.error(msg, 'Please fill required fields');
                    });
                } else {
                    alert('Please fill all required fields:\n' + errorMessages.join('\n'));
                }
                return false;
            }
            return true;
        }

        window.onbeforeunload = function () {
            if (isDirty) {
                return "You have unsaved changes. Click Update to save.";
            }
        };
    </script>

</asp:Content>

