<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="EmployeeRegisterNew.aspx.cs" Inherits="WEB_EmployeeRegisterNew" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .permission-table {
            font-size: 12px;
        }

        .permission-table th {
            background-color: #f8f9fa;
            font-weight: 600;
        }

        .permission-table td {
            vertical-align: middle;
        }

        .menu-check {
            margin-right: 8px;
        }

        /* Compact Clean Tabs */
        .premium-tabs {
            border-bottom: 1px solid #ddd;
            display: flex;
            gap: 4px;
            margin-bottom: 15px;
            padding: 0;
            background: transparent;
            list-style: none;
        }
        .premium-tabs > li {
            margin-bottom: -1px;
        }
        .premium-tabs > li > a {
            border: 1px solid transparent !important;
            border-radius: 4px 4px 0 0 !important;
            padding: 7px 12px;
            color: #666;
            font-weight: 600;
            font-size: 12px;
            background: #f5f5f5;
            display: flex;
            flex-direction: row;
            align-items: center;
            gap: 6px;
            text-decoration: none;
        }
        .premium-tabs > li > a > i {
            font-size: 13px;
            color: #888;
        }
        .premium-tabs > li > a:hover {
            background: #e5e5e5;
            border-color: #ddd #ddd transparent !important;
            color: #333;
        }
        .premium-tabs > li.active > a, 
        .premium-tabs > li.active > a:hover, 
        .premium-tabs > li.active > a:focus {
            background: #fff !important;
            color: #2196F3 !important;
            border-color: #ddd #ddd #fff !important;
        }
        .premium-tabs > li.active > a > i {
            color: #2196F3;
        }
        
        .tab-content {
            background: #fff;
            padding: 15px;
            border-radius: 0 0 4px 4px;
            border: 1px solid #ddd;
            border-top: none;
            margin-bottom: 15px;
        }
        
        fieldset legend {
            color: #2196F3;
            font-weight: 600;
            font-size: 13px;
            border-bottom: 1px solid #eee;
            padding-bottom: 5px;
            margin-bottom: 15px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form-horizontal">
        <div class="panel panel-flat" style="border-radius: 12px; box-shadow: 0 5px 20px rgba(0,0,0,0.05); border: none;">
            <div class="panel-heading" style="background: #fff; border-radius: 12px 12px 0 0; border-bottom: 1px solid #eee;">
                <h5 class="panel-title" style="font-weight: 700; color: #333;"><i class="icon-user-plus position-left text-primary"></i> Employee Registration</h5>
            </div>

            
            <div class="panel-body">
                <ul class="nav premium-tabs">
                    <li class="active"><a href="#tab-basic" data-toggle="tab"><i class="icon-user"></i> Basic Details</a></li>
                    <li><a href="#tab-work" data-toggle="tab"><i class="icon-briefcase"></i> Work Details</a></li>
                    <li><a href="#tab-statutory" data-toggle="tab"><i class="icon-vcard"></i> Statutory & Bank</a></li>
                    <li><a href="#tab-documents" data-toggle="tab"><i class="icon-folder-open"></i> Documents</a></li>
                    <li><a href="#tab-permissions" data-toggle="tab"><i class="icon-lock"></i> Menu Permissions</a></li>
                </ul>

                <div class="tab-content">
                    <!-- Tab 1: Basic Details -->
                    <div class="tab-pane active" id="tab-basic">
                        <fieldset>
                            <legend class="text-semibold"><i class="icon-reading position-left"></i>Employee Registration</legend>
                            <div class="row">
                        <div class="col-md-3">
                            <label>Employee Id<span style="color: red"> *</span></label>
                            <asp:TextBox ID="Txt_Employeeid" runat="server" class="form-control" placeholder="Employee ID"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Txt_Employeeid" ErrorMessage="Employee Id is required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-md-3">
                            <label>User Name <span style="color: red">*</span> </label>
                            <asp:TextBox ID="txt_username" runat="server" class="form-control" autocomplete="off" placeholder="User Name"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_username" ErrorMessage="User Name is required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-md-3">
                            <label>Role <span style="color: red">*</span></label>
                            <asp:DropDownList ID="ddl_dest" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl_dest_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="ddl_dest" InitialValue="0" ErrorMessage="Select Role" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-md-3">
                            <label>First Name <span style="color: red">*</span></label>
                            <asp:TextBox ID="txt_fname" runat="server" class="form-control" placeholder="First Name"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator_name" runat="server" ControlToValidate="txt_fname" ErrorMessage="Enter First Name" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="regName" runat="server" ControlToValidate="txt_fname" ValidationExpression="^[a-zA-Z'. ]{1,50}$" Text="Enter a valid name" ForeColor="Red" Display="Dynamic" />
                        </div>
                    </div>

                    <div class="row" style="margin-top: 10px;">
                        <div class="col-md-3">
                            <label>Last Name <span style="color: red">*</span></label>
                            <asp:TextBox ID="txt_lname" runat="server" CssClass="form-control" placeholder="Last Name"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_lname" ErrorMessage="Enter Last Name" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txt_lname" ValidationExpression="^[a-zA-Z'. ]{1,50}$" Text="Enter a valid name" ForeColor="Red" Display="Dynamic" />
                        </div>

                        <div class="col-md-3">
                            <label>DOB<span style="color: red"> *</span></label>
                            <div class="input-group">
                                <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                <asp:TextBox ID="txt_dob" runat="server" class="form-control pickadate" placeholder="DD/MM/YYYY"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txt_dob" ErrorMessage="DOB is required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-md-3">
                            <label>Gender<span style="color: red"> *</span></label>
                            <asp:RadioButtonList ID="rd_gander" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="&nbspMale&nbsp&nbsp&nbsp" Value="0"></asp:ListItem>
                                <asp:ListItem Text="&nbspFemale&nbsp" Value="1"></asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="rd_gander" ErrorMessage="Gender is required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-md-3">
                            <label>Email<span style="color: red"> *</span></label>
                            <asp:TextBox ID="txt_email" runat="server" CssClass="form-control" placeholder="Email Address"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txt_email" ErrorMessage="Invalid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\\w+)*\.\w+([-.]\\w+)*" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txt_email" ErrorMessage="Enter Email" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="row" style="margin-top: 10px;">
                        <div class="col-md-3">
                            <label>Password<span style="color: red"> *</span></label>
                            <div class="input-group">
                                <asp:TextBox ID="txt_pwd" runat="server" TextMode="password" CssClass="form-control" autocomplete="new-password" placeholder="Password"></asp:TextBox>
                                <span class="input-group-addon" onclick="togglePassword()" style="cursor: pointer;">
                                    <i id="eyeIcon" class="icon-eye"></i>
                                </span>
                            </div>
                            <asp:RegularExpressionValidator ID="valPassword" runat="server" ControlToValidate="txt_pwd" ErrorMessage="Minimum password length is 6" ValidationExpression="^([a-zA-Z0-9@#$%^&+=*]{6,30})$" ForeColor="Red" Display="Dynamic" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txt_pwd" ErrorMessage="Enter Password" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-md-3">
                            <label>Phone Number<span style="color: red"> *</span></label>
                            <asp:TextBox ID="txt_phone" runat="server" CssClass="form-control" placeholder="Phone Number"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txt_phone" ErrorMessage="Enter a Valid Phone Number" ValidationExpression="[0-9]{10}" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txt_phone" ErrorMessage="Enter Phone Number" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-md-3">
                            <label>Address</label>
                            <asp:TextBox ID="txt_address" runat="server" CssClass="form-control" placeholder="Address"></asp:TextBox>
                        </div>

                        <div class="col-md-3">
                            <label>City <span style="color: red">*</span></label>
                            <asp:TextBox ID="txt_city" runat="server" CssClass="form-control" placeholder="City"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txt_city" ValidationExpression="^[a-zA-Z'. ]{1,50}$" Text="Enter a valid city name" ForeColor="Red" Display="Dynamic" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txt_city" ErrorMessage="Enter City" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="row" style="margin-top: 10px;">
                        <div class="col-md-3">
                            <label>State</label>
                            <asp:DropDownList ID="ddl_state" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>

                        <div class="col-md-3">
                            <label>Zip Code<span style="color: red"> *</span></label>
                            <asp:TextBox ID="txt_zipcode" runat="server" CssClass="form-control" placeholder="Zip Code"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txt_zipcode" ErrorMessage="Enter Zip code" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-md-3">
                            <label>Qualification<span style="color: red">*</span></label>
                            <asp:TextBox ID="txt_qualification" runat="server" CssClass="form-control" placeholder="Qualification"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server" ControlToValidate="txt_qualification" ValidationExpression="^[a-zA-Z'. ]{1,50}$" Text="Enter only Text" ForeColor="Red" Display="Dynamic" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txt_qualification" ErrorMessage="Enter Qualification" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>

                        <div class="col-md-3">
                            <label>Profile Upload</label>
                            <asp:FileUpload ID="up_img" runat="server" CssClass="file-input" />
                            <asp:CustomValidator ID="CustomValidator1" ClientValidationFunction="ValidateFile" runat="server" ControlToValidate="up_img" Display="dynamic" ErrorMessage="images only" ForeColor="Red"></asp:CustomValidator>
                        </div>
                    </div>
                </fieldset>
                        </fieldset>
                        <fieldset style="margin-top: 20px;">
                            <legend class="text-semibold"><i class="icon-phone2 position-left"></i>Emergency Contact Details</legend>
                            <div class="row">
                        <div class="col-md-3">
                            <label>Contact Name <span style="color: red">*</span></label>
                            <asp:TextBox ID="txt_emergency_name" runat="server" CssClass="form-control" placeholder="Name"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator_emg_name" runat="server" ControlToValidate="txt_emergency_name" ErrorMessage="Enter Name" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-3">
                            <label>Relationship <span style="color: red">*</span></label>
                            <asp:TextBox ID="txt_emergency_relation" runat="server" CssClass="form-control" placeholder="Relationship"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator_emg_rel" runat="server" ControlToValidate="txt_emergency_relation" ErrorMessage="Enter Relationship" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-3">
                            <label>Phone Number <span style="color: red">*</span></label>
                            <asp:TextBox ID="txt_emergency_phone" runat="server" CssClass="form-control" placeholder="Phone Number" MaxLength="15"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator_emg_phone" runat="server" ControlToValidate="txt_emergency_phone" ErrorMessage="Enter Phone Number" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                        </fieldset>
                    </div>
                    
                    <!-- Tab 2: Work Details -->
                    <div class="tab-pane" id="tab-work">
                        <fieldset>
                            <legend class="text-semibold"><i class="icon-briefcase position-left"></i>Work Details</legend>
                            <div class="row">
                                <div class="col-md-3">
                                    <label>Date of Joining <span style="color: red">*</span></label>
                                    <asp:TextBox ID="txt_doj" runat="server" CssClass="form-control" type="date"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_doj" runat="server" ControlToValidate="txt_doj" ErrorMessage="Select Date of Joining" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-3">
                                    <label>Employee Type <span style="color: red">*</span></label>
                                    <asp:DropDownList ID="ddl_emptype" runat="server" CssClass="form-control"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_emptype" runat="server" ControlToValidate="ddl_emptype" InitialValue="0" ErrorMessage="Select Employee Type" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-3">
                                    <label>Reporting Manager</label>
                                    <asp:DropDownList ID="ddl_manager" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-md-3">
                                    <label>Work Type <span style="color: red">*</span></label>
                                    <asp:DropDownList ID="ddl_worktype" runat="server" CssClass="form-control"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_worktype" runat="server" ControlToValidate="ddl_worktype" InitialValue="0" ErrorMessage="Select Work Type" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                            <div class="row" style="margin-top: 10px;">
                                <div class="col-md-3">
                                    <label>Division<span style="color: red">*</span></label>
                                    <asp:DropDownList ID="ddl_division" runat="server" CssClass="form-control"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="ddl_division" InitialValue="0" ErrorMessage="Select Division" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-3">
                                    <label>Designation  <span style="color: red">*</span></label>
                                    <asp:DropDownList ID="ddl_depart" runat="server" CssClass="form-control"></asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddl_depart" InitialValue="0" ErrorMessage="Select Department" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-3">
                                    <label>Status <span style="color: red">*</span></label>
                                    <asp:DropDownList ID="ddl_status" runat="server" CssClass="form-control">
                                        <asp:ListItem Text="Select Status" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="ddl_status" InitialValue="2" ErrorMessage="Select Status" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </fieldset>
                    </div>

                    <!-- Tab 3: Statutory & Bank -->
                    <div class="tab-pane" id="tab-statutory">
                        <fieldset>
                            <legend class="text-semibold"><i class="icon-vcard position-left"></i>Identity & Statutory</legend>
                            <div class="row">
                        <div class="col-md-3">
                            <label>Aadhaar Number <span style="color: red">*</span></label>
                            <asp:TextBox ID="txt_aadhaar" runat="server" CssClass="form-control" placeholder="12-Digit Number" MaxLength="12"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator_aadhaar" runat="server" ControlToValidate="txt_aadhaar" ErrorMessage="Enter Aadhaar Number" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-3">
                            <label>PAN Number <span style="color: red">*</span></label>
                            <asp:TextBox ID="txt_pan" runat="server" CssClass="form-control" placeholder="ABCDE1234F" MaxLength="10" Style="text-transform:uppercase;"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator_pan" runat="server" ControlToValidate="txt_pan" ErrorMessage="Enter PAN Number" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-3">
                            <label>UAN / PF Number</label>
                            <asp:TextBox ID="txt_uan" runat="server" CssClass="form-control" placeholder="UAN/PF Number"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label>Blood Group</label>
                            <asp:DropDownList ID="ddl_bloodgroup" runat="server" CssClass="form-control"></asp:DropDownList>
                        </div>
                    </div>
                        </fieldset>
                        <fieldset style="margin-top: 20px;">
                            <legend class="text-semibold"><i class="icon-coins position-left"></i>Bank Details</legend>
                            <div class="row">
                        <div class="col-md-3">
                            <label>Bank Name</label>
                            <asp:TextBox ID="txt_bankname" runat="server" CssClass="form-control" placeholder="Bank Name"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label>Account Number</label>
                            <asp:TextBox ID="txt_accountno" runat="server" CssClass="form-control" placeholder="Account Number"></asp:TextBox>
                        </div>
                        <div class="col-md-3">
                            <label>IFSC Code</label>
                            <asp:TextBox ID="txt_ifsc" runat="server" CssClass="form-control" placeholder="IFSC Code" Style="text-transform:uppercase;"></asp:TextBox>
                        </div>
                    </div>
                        </fieldset>
                    </div>

                    <!-- Tab 4: Documents -->
                    <div class="tab-pane" id="tab-documents">
                        <fieldset>
                            <legend class="text-semibold" style="display: flex; justify-content: space-between; align-items: center;">
                                <span><i class="icon-file-empty position-left"></i>Employee Documents</span>
                                <button type="button" class="btn btn-success btn-xs" onclick="addDocRow()"><i class="icon-plus2 text-size-small"></i> Add New</button>
                            </legend>
                            <div class="row">
                                <div class="col-md-9">
                                    <div class="table-responsive">
                                        <table class="table table-bordered table-condensed text-size-small" id="docTable">
                                            <thead style="background-color: #fcfcfc;">
                                                <tr>
                                                    <th style="padding: 8px;">Document Name</th>
                                                    <th style="padding: 8px;">File (Max 5MB)</th>
                                                    <th class="text-center" style="padding: 8px; width: 80px;">Preview</th>
                                                    <th class="text-center" style="padding: 8px; width: 80px;">Action</th>
                                                </tr>
                                            </thead>
                                            <tbody id="docTableBody">
                                                <asp:Literal ID="litExistingDocs" runat="server"></asp:Literal>
                                                <tr>
                                                    <td style="padding: 5px;"><input type="text" name="DocName_1" class="form-control input-sm" placeholder="e.g. Aadhaar Card, PAN Card, Resume" style="height: 32px; padding: 5px 10px;" /></td>
                                                    <td style="padding: 5px;"><input type="file" name="DocFile_1" class="form-control input-sm" accept=".pdf,.jpg,.jpeg,.png" style="height: 32px; padding: 3px 10px;" onchange="previewLocalFile(this, 1)" /></td>
                                                    <td class="text-center" style="padding: 5px; vertical-align: middle;" id="preview_td_1"><span class="text-muted" style="font-size: 11px;">N/A</span></td>
                                                    <td class="text-center" style="padding: 5px;">
                                                        <button type="button" class="btn btn-danger btn-xs" onclick="removeDocRow(this)" title="Remove"><i class="icon-trash"></i></button>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                    <input type="hidden" name="DocRowCount" id="DocRowCount" value="1" />
                                </div>
                            </div>
                        </fieldset>
                    </div>

                    <!-- Tab 5: Menu Permissions -->
                    <div class="tab-pane" id="tab-permissions">
                        <fieldset>
                            <legend class="text-semibold"><i class="icon-lock position-left"></i>Menu Permissions</legend>
                            <div class="alert alert-info" style="font-size: 12px;">
                        <i class="icon-info22"></i> Select the role above to load menu permissions automatically. You can customize permissions below.
                    </div>

                    <div id="menuAccordion">
                        <asp:PlaceHolder ID="phMenus" runat="server"></asp:PlaceHolder>
                    </div>
                        </fieldset>
                    </div>
                </div>

                <asp:HiddenField ID="hfSelectedMenus" runat="server" />

                <div class="form-group" style="margin-top: 20px;">
                    <div class="text-right">
                        <a href="EmployeeView.aspx" id="btnBackToGrid" class="btn btn-default" style="margin-right: 5px;">Back</a>
                        <button type="button" id="btnPrev" class="btn btn-warning" onclick="prevTab()" style="display: none; margin-right: 5px;">Previous</button>
                        <button type="button" id="btnNext" class="btn btn-info" onclick="nextTab()" style="margin-right: 5px;">Next</button>
                        <asp:Button ID="btn_register" runat="server" Text="Register" class="btn btn-primary" OnClientClick="return validateAllAndGoToError();" OnClick="btn_register_Click" Style="display: none; margin-right: 15px;"></asp:Button>
                    </div>
                </div>
            </div>

        </div>
    </div>

    <script lang="javascript">
        function toggleModule(moduleCheckbox, event) {
            event.stopPropagation();
            var moduleId = moduleCheckbox.value;
            var isChecked = moduleCheckbox.checked;
            var table = document.querySelector("table[data-module='" + moduleId + "']");
            
            if (table) {
                var allCheckboxes = table.querySelectorAll("input[type='checkbox']");
                allCheckboxes.forEach(function(checkbox) {
                    checkbox.checked = isChecked;
                });
            }
        }

        function togglePassword() {
            var passwordField = document.getElementById("<%=txt_pwd.ClientID%>");
            var eyeIcon = document.getElementById("eyeIcon");
            
            if (passwordField.type === "password") {
                passwordField.type = "text";
                eyeIcon.className = "icon-eye-blocked";
            } else {
                passwordField.type = "password";
                eyeIcon.className = "icon-eye";
            }
        }

        function ValidateFile(source, args) {
            try {
                var fileAndPath = document.getElementById(source.controltovalidate).value;
                var lastPathDelimiter = fileAndPath.lastIndexOf("\\");
                var fileNameOnly = fileAndPath.substring(lastPathDelimiter + 1);
                var file_extDelimiter = fileNameOnly.lastIndexOf(".");
                var file_ext = fileNameOnly.substring(file_extDelimiter + 1).toLowerCase();

                if (file_ext == "jpg" || file_ext == "gif" || file_ext == "png" || file_ext == "jpeg") {
                    args.IsValid = true;
                } else {
                    args.IsValid = false;
                }
            } catch (err) {
                alert("Error validating file: " + err.description);
                args.IsValid = false;
            }
        }

        $('.pickadate').pickadate({
            format: 'dd/mm/yyyy',
            selectMonths: true,
            selectYears: true,
            closeOnSelect: true
        });

        // Collect selected menus before submit
        function collectSelectedMenus() {
            var rows = [];
            var addedMenus = {};

            document.querySelectorAll(".permission-table tbody tr").forEach(function (tr) {
                var menuCheckbox = tr.querySelector(".menu-check");
                if (!menuCheckbox) return;

                var menuId   = menuCheckbox.value;
                var parentId = menuCheckbox.getAttribute("data-parent") || "0";
                var table    = tr.closest("table");
                var moduleId = table ? (table.getAttribute("data-module") || "0") : "0";

                var view   = tr.querySelector("input[name='view_"   + menuId + "']");
                var create = tr.querySelector("input[name='create_" + menuId + "']");
                var edit   = tr.querySelector("input[name='edit_"   + menuId + "']");
                var del    = tr.querySelector("input[name='delete_" + menuId + "']");
                var order  = tr.querySelector("input[name='order_"  + menuId + "']");

                var viewVal   = view   && view.checked   ? 1 : 0;
                var createVal = create && create.checked ? 1 : 0;
                var editVal   = edit   && edit.checked   ? 1 : 0;
                var deleteVal = del    && del.checked    ? 1 : 0;
                var orderVal  = order  ? (order.value || "0") : "0";

                // Check if this is a parent menu (has children) or single menu
                var hasChildren = document.querySelector(".child-" + menuId);

                if (menuCheckbox.checked || viewVal || createVal || editVal || deleteVal) {
                    if (hasChildren && !view) {
                        // Parent menu with children - save with ViewPermission=1 + its own order
                        rows.push(moduleId + "|0|" + menuId + "|1|0|0|0|" + orderVal);
                    } else {
                        // Single menu or submenu - save with actual permissions + order
                        rows.push(moduleId + "|" + parentId + "|" + menuId + "|" + viewVal + "|" + createVal + "|" + editVal + "|" + deleteVal + "|" + orderVal);
                    }
                    addedMenus[menuId] = true;

                    // Add parent menu if this is a submenu and parent not added
                    if (parentId !== "0" && !addedMenus[parentId]) {
                        var parentOrder = "0";
                        var parentOrderInput = document.querySelector("input[name='order_" + parentId + "']");
                        if (parentOrderInput) parentOrder = parentOrderInput.value || "0";
                        rows.push(moduleId + "|0|" + parentId + "|1|0|0|0|" + parentOrder);
                        addedMenus[parentId] = true;
                    }
                }
            });

            document.getElementById("<%=hfSelectedMenus.ClientID%>").value = rows.join(",");
        }

        // Menu permission toggle functions
        function menuClicked(menuCheckbox) {
            var row = menuCheckbox.closest("tr");
            var view = row.querySelector("input[name='view_" + menuCheckbox.value + "']");

            if (menuCheckbox.checked) {
                toggleParent(menuCheckbox);
                if (view) view.checked = true;
            } else {
                var permissions = row.querySelectorAll("input[type='checkbox']:not(.menu-check)");
                permissions.forEach(function (p) { p.checked = false; });
                toggleChildren(menuCheckbox);
            }
        }

        function toggleChildren(parentCheckbox) {
            var parentId = parentCheckbox.value;
            var children = document.querySelectorAll(".child-" + parentId);

            children.forEach(function (child) {
                child.checked = parentCheckbox.checked;
                var row = child.closest("tr");
                var view = row.querySelector("input[name='view_" + child.value + "']");
                var create = row.querySelector("input[name='create_" + child.value + "']");
                var edit = row.querySelector("input[name='edit_" + child.value + "']");
                var del = row.querySelector("input[name='delete_" + child.value + "']");

                if (parentCheckbox.checked) {
                    if (view) view.checked = true;
                    if (create) create.checked = false;
                    if (edit) edit.checked = false;
                    if (del) del.checked = false;
                } else {
                    if (view) view.checked = false;
                    if (create) create.checked = false;
                    if (edit) edit.checked = false;
                    if (del) del.checked = false;
                }

                toggleChildren(child);
            });
        }

        function toggleParent(childCheckbox) {
            var parentId = childCheckbox.getAttribute("data-parent");
            if (!parentId || parentId == "0") return;

            var parentCheckbox = document.querySelector(".menu-check[value='" + parentId + "']");
            if (parentCheckbox) {
                parentCheckbox.checked = true;
                toggleParent(parentCheckbox);
            }
        }

        function permissionClicked(permissionCheckbox) {
            var row = permissionCheckbox.closest("tr");
            var menuCheckbox = row.querySelector(".menu-check");

            if (menuCheckbox) {
                menuCheckbox.checked = true;
                toggleParent(menuCheckbox);
            }

            var permissionName = permissionCheckbox.name;
            if (permissionName.startsWith("create_") || permissionName.startsWith("edit_") || permissionName.startsWith("delete_")) {
                var viewCheckbox = row.querySelector("input[name^='view_']");
                if (viewCheckbox) viewCheckbox.checked = true;
            }
        }
        function addDocRow() {
            var rowCount = parseInt(document.getElementById("DocRowCount").value) + 1;
            document.getElementById("DocRowCount").value = rowCount;
            
            var tbody = document.getElementById("docTableBody");
            var tr = document.createElement("tr");
            tr.innerHTML = `
                <td style="padding: 5px;"><input type="text" name="DocName_${rowCount}" class="form-control input-sm" placeholder="e.g. Aadhaar Card, PAN Card" style="height: 32px; padding: 5px 10px;" /></td>
                <td style="padding: 5px;"><input type="file" name="DocFile_${rowCount}" class="form-control input-sm" accept=".pdf,.jpg,.jpeg,.png" style="height: 32px; padding: 3px 10px;" onchange="previewLocalFile(this, ${rowCount})" /></td>
                <td class="text-center" style="padding: 5px; vertical-align: middle;" id="preview_td_${rowCount}"><span class="text-muted" style="font-size: 11px;">N/A</span></td>
                <td class="text-center" style="padding: 5px;">
                    <button type="button" class="btn btn-danger btn-xs" onclick="removeDocRow(this)" title="Remove"><i class="icon-trash"></i></button>
                </td>
            `;
            tbody.appendChild(tr);
        }

        function previewLocalFile(input, rowId) {
            var td = document.getElementById('preview_td_' + rowId);
            if (input.files && input.files[0]) {
                var file = input.files[0];
                var fileUrl = URL.createObjectURL(file);
                td.innerHTML = '<a href="' + fileUrl + '" target="_blank" class="btn btn-info btn-xs" title="Preview"><i class="icon-eye"></i></a>';
            } else {
                td.innerHTML = '<span class="text-muted" style="font-size: 11px;">N/A</span>';
            }
        }

        function removeDocRow(btn) {
            var tr = btn.closest("tr");
            tr.remove();
        }

        function deleteExistingDoc(docId) {
            if(confirm('Are you sure you want to remove this document?')) {
                $.ajax({
                    type: "POST",
                    url: "EmployeeRegisterNew.aspx/DeleteDocument",
                    data: JSON.stringify({ id: docId }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        if (response.d == "Success") {
                            toastr.success("Document removed successfully");
                            document.getElementById("row_doc_" + docId).remove();
                        } else {
                            toastr.error("Error: " + response.d);
                        }
                    },
                    error: function () {
                        toastr.error("An error occurred");
                    }
                });
            }
        }

        // --- WIZARD LOGIC ---
        var tabs = ["tab-basic", "tab-work", "tab-statutory", "tab-documents", "tab-permissions"];
        var currentTabIndex = 0;

        function updateButtons() {
            document.getElementById("btnPrev").style.display = currentTabIndex === 0 ? "none" : "inline-block";
            document.getElementById("btnNext").style.display = currentTabIndex === tabs.length - 1 ? "none" : "inline-block";
            document.getElementById("<%=btn_register.ClientID%>").style.display = currentTabIndex === tabs.length - 1 ? "inline-block" : "none";
            
            var btnBack = document.getElementById("btnBackToGrid");
            if(btnBack) {
                btnBack.style.display = currentTabIndex === 0 ? "inline-block" : "none";
            }
        }

        function nextTab() {
            if (currentTabIndex < tabs.length - 1) {
                currentTabIndex++;
                $('.premium-tabs a[href="#' + tabs[currentTabIndex] + '"]').tab('show');
                updateButtons();
            }
        }

        function prevTab() {
            if (currentTabIndex > 0) {
                currentTabIndex--;
                $('.premium-tabs a[href="#' + tabs[currentTabIndex] + '"]').tab('show');
                updateButtons();
            }
        }

        $('.premium-tabs a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
            var target = $(e.target).attr("href").substring(1);
            currentTabIndex = tabs.indexOf(target);
            updateButtons();
        });

        function validateAllAndGoToError() {
            if (typeof(Page_ClientValidate) === "function") {
                var isValid = Page_ClientValidate();
                if (!isValid) {
                    // Find the first invalid validator and jump to its tab
                    for (var i = 0; i < Page_Validators.length; i++) {
                        if (!Page_Validators[i].isvalid) {
                            var parentTab = $(Page_Validators[i]).closest('.tab-pane');
                            if (parentTab.length > 0) {
                                var tabId = parentTab.attr('id');
                                $('.premium-tabs a[href="#' + tabId + '"]').tab('show');
                                toastr.error("Please check the fields with errors.");
                                return false; // stop submission
                            }
                        }
                    }
                    return false;
                }
            }
            
            collectSelectedMenus();
            return true; // allow submission
        }
        
        // Initialize buttons on load
        $(document).ready(function() {
            updateButtons();
        });

    </script>
</asp:Content>
