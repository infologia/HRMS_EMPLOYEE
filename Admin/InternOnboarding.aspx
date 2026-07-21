<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="InternOnboarding.aspx.cs" Inherits="WEB_InternOnboarding" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .wizard-card {
            background: #fff;
            border-radius: 8px;
            box-shadow: 0 4px 15px rgba(0,0,0,0.05);
            padding: 25px;
            margin: 10px auto;
            max-width: 800px;
            border: 1px solid #eaeaea;
        }

        .wizard-header h3 {
            text-align: center;
            margin-bottom: 20px;
            font-weight: 600;
            font-size: 20px;
            color: #222;
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }

        .wizard-steps {
            display: flex;
            justify-content: space-between;
            border-bottom: 2px solid #e0e0e0;
            margin-bottom: 20px;
        }

        .wizard-step {
            flex: 1;
            text-align: center;
            padding-bottom: 8px;
            color: #888;
            font-weight: 600;
            font-size: 13px;
            cursor: default;
            transition: 0.3s;
        }

            .wizard-step.active {
                color: #1a237e;
                border-bottom: 3px solid #1a237e;
                margin-bottom: -2px;
            }

        .step-content {
            display: none;
        }

            .step-content.active {
                display: block;
                animation: fadeIn 0.3s ease-in-out;
            }

        @keyframes fadeIn {
            from {
                opacity: 0;
                transform: translateY(10px);
            }

            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        .form-group {
            margin-bottom: 12px;
        }

        .form-control {
            border-radius: 4px;
            border: 1px solid #ccc;
            padding: 6px 12px;
            font-size: 12px;
            height: auto;
            box-shadow: inset 0 1px 2px rgba(0,0,0,0.02);
            background-color: #fdfdfd;
            transition: 0.2s;
        }

            .form-control:focus {
                border-color: #1a237e;
                box-shadow: 0 0 0 0.2rem rgba(26, 35, 126, 0.15);
                background-color: #fff;
                outline: none;
            }

            .form-control[readonly] {
                background-color: #f0f2f5;
                cursor: not-allowed;
            }

            .form-control.pickadate[readonly] {
                background-color: #fdfdfd;
                cursor: pointer;
            }

        .wizard-footer {
            margin-top: 20px;
            text-align: right;
            border-top: 1px solid #eee;
            padding-top: 15px;
        }

        .btn-next, .btn-submit {
            background: #1a237e;
            color: #fff;
            border-radius: 4px;
            padding: 6px 20px;
            font-weight: 600;
            border: none;
            font-size: 13px;
            transition: 0.3s;
        }

            .btn-next:hover, .btn-submit:hover {
                background: #121858;
                color: #fff;
                box-shadow: 0 4px 10px rgba(26,35,126,0.3);
            }

        .btn-back {
            background: #828a92;
            color: #fff;
            border-radius: 4px;
            padding: 6px 20px;
            font-weight: 600;
            border: none;
            font-size: 13px;
            margin-right: 10px;
            transition: 0.3s;
        }

            .btn-back:hover {
                background: #6c757d;
                color: #fff;
                box-shadow: 0 4px 10px rgba(108,117,125,0.3);
            }

        .checkbox-custom {
            display: flex;
            align-items: center;
            font-size: 12px;
            color: #555;
            margin-bottom: 5px;
            cursor: pointer;
        }

            .checkbox-custom input {
                margin-right: 8px;
                transform: scale(1.1);
                cursor: pointer;
            }

        .radio-list td {
            padding-right: 12px;
            font-size: 13px;
            color: #444;
        }

        .agreement-well {
            background: #f8f9fa;
            border: 1px solid #e9ecef;
            border-radius: 4px;
            padding: 12px;
            margin-bottom: 15px;
            font-size: 12px;
        }

            .agreement-well ul {
                padding-left: 15px;
                color: #555;
                line-height: 1.5;
                margin-top: 5px;
            }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="wizard-card">
        <div class="wizard-header">
            <h3>Internship Onboarding Form </h3>
        </div>

        <div class="wizard-steps">
            <div class="wizard-step active" id="header-step-1">1. Personal Information</div>
            <div class="wizard-step" id="header-step-2">2. Internship Details</div>
            <div class="wizard-step" id="header-step-3">3. Document Uploads</div>
            <div class="wizard-step" id="header-step-4">4. Agreement & Submission</div>
        </div>

        <!-- STEP 1: Personal Information -->
        <div class="step-content active" id="step-1">
            <div class="row">
                <div class="col-md-4 form-group">
                    <label class="control-label">Intern Code</label>
                    <asp:TextBox ID="txt_intern_code" runat="server" class="form-control form-control-sm" ReadOnly="true"></asp:TextBox>
                </div>
                <div class="col-md-4 form-group">
                    <label class="control-label">Full Name <span style="color: red">*</span></label>
                    <asp:TextBox ID="txt_name" runat="server" class="form-control form-control-sm" placeholder="e.g., Jane Doe"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_name" runat="server" ControlToValidate="txt_name" ErrorMessage="Enter Name." ForeColor="Red" Display="Dynamic" ValidationGroup="step1"></asp:RequiredFieldValidator>
                </div>
                <div class="col-md-4 form-group">
                    <label class="control-label">Date of Birth <span style="color: red">*</span></label>
                    <div class="input-group">
                        <span class="input-group-addon" style="border-radius: 4px 0 0 4px;"><i class="icon-calendar22"></i></span>
                        <asp:TextBox ID="txt_dob" runat="server" class="form-control form-control-sm pickadate" placeholder="DD/MM/YYYY" Style="border-radius: 0 4px 4px 0;"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txt_dob" ErrorMessage="DOB is required" ForeColor="Red" Display="Dynamic" ValidationGroup="step1"></asp:RequiredFieldValidator>
                </div>
            </div>



            <div class="row">
                <div class="col-md-4 form-group">
                    <label class="control-label">Gender <span style="color: red">*</span></label>
                    <div style="padding-top: 5px;">
                        <asp:RadioButtonList ID="rd_gander" runat="server" RepeatDirection="Horizontal" CssClass="radio-list">
                            <asp:ListItem Text="&nbsp;Male" Value="0"></asp:ListItem>
                            <asp:ListItem Text="&nbsp;Female" Value="1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="rd_gander" ErrorMessage="Gender is required." ForeColor="Red" Display="Dynamic" ValidationGroup="step1"></asp:RequiredFieldValidator>
                </div>
                <div class="col-md-4 form-group">
                    <label class="control-label">Email Address <span style="color: red">*</span></label>
                    <asp:TextBox ID="txt_email" runat="server" CssClass="form-control form-control-sm" placeholder="jane.doe@example.com"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txt_email" ErrorMessage="Invalid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red" Display="Dynamic" ValidationGroup="step1"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txt_email" ErrorMessage="Enter the Email." ForeColor="Red" Display="Dynamic" ValidationGroup="step1"></asp:RequiredFieldValidator>
                </div>
                <div class="col-md-4 form-group">
                    <label class="control-label">Phone Number <span style="color: red">*</span></label>
                    <asp:TextBox ID="txt_phone" runat="server" class="form-control form-control-sm" placeholder="+91"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txt_phone" ErrorMessage="Enter a Valid Phone Number" ValidationExpression="[0-9]{10,15}" ForeColor="Red" Display="Dynamic" ValidationGroup="step1"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txt_phone" ErrorMessage="Enter Phone Number." ForeColor="Red" Display="Dynamic" ValidationGroup="step1"></asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="row">
                <div class="col-md-4 form-group">
                    <label class="control-label">Emergency Contact Name <span style="color: red">*</span></label>
                    <asp:TextBox ID="txt_emergency_name" runat="server" class="form-control form-control-sm"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txt_emergency_name" ErrorMessage="Enter Emergency Contact Name." ForeColor="Red" Display="Dynamic" ValidationGroup="step1"></asp:RequiredFieldValidator>
                </div>
                <div class="col-md-4 form-group">
                    <label class="control-label">Emergency Contact Number <span style="color: red">*</span></label>
                    <asp:TextBox ID="txt_emergency_number" runat="server" class="form-control form-control-sm"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator_EmergencyNum" runat="server" ControlToValidate="txt_emergency_number" ErrorMessage="Enter a Valid Phone Number" ValidationExpression="[0-9]{10,15}" ForeColor="Red" Display="Dynamic" ValidationGroup="step1"></asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_EmergencyNum" runat="server" ControlToValidate="txt_emergency_number" ErrorMessage="Enter Emergency Contact Number." ForeColor="Red" Display="Dynamic" ValidationGroup="step1"></asp:RequiredFieldValidator>
                </div>
                <div class="col-md-4 form-group">
                    <label class="control-label">Blood Group <span style="color: red">*</span></label>
                    <asp:TextBox ID="txt_blood_group" runat="server" class="form-control form-control-sm"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_blood_group" ErrorMessage="Required." ForeColor="Red" Display="Dynamic" ValidationGroup="step1"></asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="row">
                <div class="col-md-4 form-group">
                    <label class="control-label">University / College <span style="color: red">*</span></label>
                    <asp:TextBox ID="txt_university" runat="server" class="form-control form-control-sm"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txt_university" ErrorMessage="Required." ForeColor="Red" Display="Dynamic" ValidationGroup="step1"></asp:RequiredFieldValidator>
                </div>
                <div class="col-md-4 form-group">
                    <label class="control-label">Course / Degree <span style="color: red">*</span></label>
                    <asp:TextBox ID="txt_course" runat="server" class="form-control form-control-sm"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txt_course" ErrorMessage="Required." ForeColor="Red" Display="Dynamic" ValidationGroup="step1"></asp:RequiredFieldValidator>
                </div>
                <div class="col-md-4 form-group">
                    <label class="control-label">Year of Study / Graduation <span style="color: red">*</span></label>
                    <asp:TextBox ID="txt_year_of_study" runat="server" class="form-control form-control-sm"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txt_year_of_study" ErrorMessage="Required." ForeColor="Red" Display="Dynamic" ValidationGroup="step1"></asp:RequiredFieldValidator>
                </div>
                <div class="row">
                    <div class="col-md-4 form-group">
                        <label class="control-label">Profile Image <span style="color: red">*</span></label>
                        <asp:FileUpload ID="up_img" runat="server" class="form-control form-control-sm" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator_img" runat="server" ControlToValidate="up_img" ErrorMessage="Upload Profile Image." ForeColor="Red" Display="Dynamic" ValidationGroup="step1"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-6 form-group">
                    <label class="control-label">Permanent Address <span style="color: red">*</span></label>
                    <asp:TextBox ID="txt_permanent_address" runat="server" TextMode="MultiLine" Rows="3" class="form-control form-control-sm"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_permanent_address" ErrorMessage="Enter Permanent Address." ForeColor="Red" Display="Dynamic" ValidationGroup="step1"></asp:RequiredFieldValidator>
                </div>
                <div class="col-md-6 form-group">
                    <div style="display: flex; justify-content: space-between; align-items: flex-end;">
                        <label class="control-label">Present Address <span style="color: red">*</span></label>
                        <label class="checkbox-custom">
                            <input type="checkbox" id="chk_same_address" onclick="CopyAddress()" />
                            Same as Permanent Address
                       
                        </label>
                    </div>
                    <asp:TextBox ID="txt_present_address" runat="server" TextMode="MultiLine" Rows="3" class="form-control form-control-sm"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_present_address" ErrorMessage="Enter Present Address." ForeColor="Red" Display="Dynamic" ValidationGroup="step1"></asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="wizard-footer">
                <a href="InternOnboardingView.aspx" class="btn btn-back">Back</a>
                <button type="button" class="btn btn-next" onclick="if(Page_ClientValidate('step1')) nextStep(2);">Next</button>
            </div>
        </div>

        <!-- STEP 2: Internship Details -->
        <div class="step-content" id="step-2">
            <div class="row">
                <div class="col-md-4 form-group">
                    <label class="control-label">Internship Duration <span style="color: red">*</span></label>
                    <asp:TextBox ID="txt_internship_duration" runat="server" class="form-control form-control-sm"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txt_internship_duration" ErrorMessage="Enter Duration." ForeColor="Red" Display="Dynamic" ValidationGroup="step2"></asp:RequiredFieldValidator>
                </div>
                <div class="col-md-4 form-group">
                    <label class="control-label">Department <span style="color: red">*</span></label>
                    <asp:TextBox ID="txt_department" runat="server" class="form-control form-control-sm"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="txt_department" ErrorMessage="Enter Department." ForeColor="Red" Display="Dynamic" ValidationGroup="step2"></asp:RequiredFieldValidator>
                </div>
                <div class="col-md-4 form-group">
                    <label class="control-label">Start Date <span style="color: red">*</span></label>
                    <div class="input-group">
                        <span class="input-group-addon" style="border-radius: 4px 0 0 4px;"><i class="icon-calendar22"></i></span>
                        <asp:TextBox ID="txt_start_date" runat="server" class="form-control form-control-sm pickadate" placeholder="DD/MM/YYYY" Style="border-radius: 0 4px 4px 0;"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="txt_start_date" ErrorMessage="Enter Start Date." ForeColor="Red" Display="Dynamic" ValidationGroup="step2"></asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="row">
                <div class="col-md-4 form-group">
                    <label class="control-label">End Date <span style="color: red">*</span></label>
                    <div class="input-group">
                        <span class="input-group-addon" style="border-radius: 4px 0 0 4px;"><i class="icon-calendar22"></i></span>
                        <asp:TextBox ID="txt_end_date" runat="server" class="form-control form-control-sm pickadate" placeholder="DD/MM/YYYY" Style="border-radius: 0 4px 4px 0;"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="txt_end_date" ErrorMessage="Enter End Date." ForeColor="Red" Display="Dynamic" ValidationGroup="step2"></asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="wizard-footer">
                <button type="button" class="btn btn-back" onclick="prevStep(1);">Back</button>
                <button type="button" class="btn btn-next" onclick="if(Page_ClientValidate('step2')) nextStep(3);">Next</button>
            </div>
        </div>

        <!-- STEP 3: Document Uploads -->
        <div class="step-content" id="step-3">
            <div class="row">
                <div class="col-md-4 form-group">
                    <label class="control-label">Updated Resume <span style="color: red">*</span></label>
                    <asp:FileUpload ID="up_resume" runat="server" class="form-control form-control-sm" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_Resume" runat="server" ControlToValidate="up_resume" ErrorMessage="Upload Resume." ForeColor="Red" Display="Dynamic" ValidationGroup="step3"></asp:RequiredFieldValidator>
                </div>
                <div class="col-md-4 form-group">
                    <label class="control-label">Aadhar Card <span style="color: red">*</span></label>
                    <asp:FileUpload ID="up_aadhar" runat="server" class="form-control form-control-sm" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_Aadhar" runat="server" ControlToValidate="up_aadhar" ErrorMessage="Upload Aadhar." ForeColor="Red" Display="Dynamic" ValidationGroup="step3"></asp:RequiredFieldValidator>
                </div>
                <div class="col-md-4 form-group">
                    <label class="control-label">PAN Card <span style="color: red">*</span></label>
                    <asp:FileUpload ID="up_pan" runat="server" class="form-control form-control-sm" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_PAN" runat="server" ControlToValidate="up_pan" ErrorMessage="Upload PAN." ForeColor="Red" Display="Dynamic" ValidationGroup="step3"></asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="row">
                <div class="col-md-4 form-group">
                    <label class="control-label">Passport <span style="color: red">*</span></label>
                    <asp:FileUpload ID="up_passport" runat="server" class="form-control form-control-sm" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_Passport" runat="server" ControlToValidate="up_passport" ErrorMessage="Upload Passport." ForeColor="Red" Display="Dynamic" ValidationGroup="step3"></asp:RequiredFieldValidator>
                </div>
                <div class="col-md-4 form-group">
                    <label class="control-label">10th Marksheet <span style="color: red">*</span></label>
                    <asp:FileUpload ID="up_10th" runat="server" class="form-control form-control-sm" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_10th" runat="server" ControlToValidate="up_10th" ErrorMessage="Upload 10th Marksheet." ForeColor="Red" Display="Dynamic" ValidationGroup="step3"></asp:RequiredFieldValidator>
                </div>
                <div class="col-md-4 form-group">
                    <label class="control-label">12th Marksheet <span style="color: red">*</span></label>
                    <asp:FileUpload ID="up_12th" runat="server" class="form-control form-control-sm" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_12th" runat="server" ControlToValidate="up_12th" ErrorMessage="Upload 12th Marksheet." ForeColor="Red" Display="Dynamic" ValidationGroup="step3"></asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="row">
                <div class="col-md-4 form-group">
                    <label class="control-label">Degree Certificate <span style="color: red">*</span></label>
                    <asp:FileUpload ID="up_degree" runat="server" class="form-control form-control-sm" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_Degree" runat="server" ControlToValidate="up_degree" ErrorMessage="Upload Degree." ForeColor="Red" Display="Dynamic" ValidationGroup="step3"></asp:RequiredFieldValidator>
                </div>
                <div class="col-md-4 form-group">
                    <label class="control-label">Bonafide Certificate <span style="color: red">*</span></label>
                    <asp:FileUpload ID="up_bonafide" runat="server" class="form-control form-control-sm" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_Bonafide" runat="server" ControlToValidate="up_bonafide" ErrorMessage="Upload Bonafide." ForeColor="Red" Display="Dynamic" ValidationGroup="step3"></asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="wizard-footer">
                <button type="button" class="btn btn-back" onclick="prevStep(2);">Back</button>
                <button type="button" class="btn btn-next" onclick="if(Page_ClientValidate('step3')) nextStep(4);">Next</button>
            </div>
        </div>

        <!-- STEP 4: Agreement & Submission -->
        <div class="step-content" id="step-4">
            <div class="agreement-well">
                <p><strong>Terms and Conditions:</strong></p>
                <ul>
                    <li>I will comply with all company policies and procedures.</li>
                    <li>I will maintain confidentiality of all company information and materials.</li>
                    <li>I will be punctual and adhere to the assigned working hours.</li>
                    <li>I understand that this internship is [paid/unpaid] and is not a guarantee of full-time employment at the end of the internship.</li>
                </ul>
            </div>

            <div class="form-group" style="margin-top: 15px;">
                <label style="font-weight: bold; font-size: 15px; color: #333; cursor: pointer; display: flex; align-items: center;">
                    <asp:CheckBox ID="chk_agreement" runat="server" Style="margin-right: 10px; transform: scale(1.2);" />
                    <span>I, <span id="span_agreement_name" style="font-weight: 800; color: #1a237e;">[Your Name]</span> hereby agree to the above terms and conditions as part of my internship.</span>
                </label>
                <asp:CustomValidator ID="CustomValidatorAgreement" runat="server" ErrorMessage="You must agree to the terms to submit." ClientValidationFunction="ValidateAgreement" ForeColor="Red" Display="Dynamic" ValidationGroup="step4"></asp:CustomValidator>
            </div>

            <div class="row" style="margin-top: 30px;">
                <div class="col-md-4 form-group">
                    <label class="control-label">Digital Signature (Type Full Name) <span style="color: red">*</span></label>
                    <asp:TextBox ID="txt_digital_signature" runat="server" class="form-control form-control-sm" placeholder="e.g., John Doe"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_Signature" runat="server" ControlToValidate="txt_digital_signature" ErrorMessage="Signature is required." ForeColor="Red" Display="Dynamic" ValidationGroup="step4"></asp:RequiredFieldValidator>
                </div>
                <div class="col-md-4 form-group">
                    <label class="control-label">Agreement Date <span style="color: red">*</span></label>
                    <div class="input-group">
                        <span class="input-group-addon" style="border-radius: 4px 0 0 4px;"><i class="icon-calendar22"></i></span>
                        <asp:TextBox ID="txt_agreement_date" runat="server" class="form-control form-control-sm pickadate" placeholder="DD/MM/YYYY" Style="border-radius: 0 4px 4px 0;"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_AgreementDate" runat="server" ControlToValidate="txt_agreement_date" ErrorMessage="Date is required." ForeColor="Red" Display="Dynamic" ValidationGroup="step4"></asp:RequiredFieldValidator>
                </div>
            </div>

            <div class="wizard-footer">
                <button type="button" class="btn btn-back" onclick="prevStep(3);">Back</button>
                <asp:Button ID="btn_register" runat="server" Text="Submit Application" class="btn btn-submit" OnClientClick="if(!Page_ClientValidate('step4')) return false;" OnClick="btn_register_Click"></asp:Button>
            </div>
        </div>

    </div>

    <script lang="javascript">
        // Tab Navigation Logic
        function nextStep(step) {
            document.querySelectorAll('.step-content').forEach(el => el.classList.remove('active'));
            document.getElementById('step-' + step).classList.add('active');

            document.querySelectorAll('.wizard-step').forEach(el => el.classList.remove('active'));
            for (let i = 1; i <= step; i++) {
                document.getElementById('header-step-' + i).classList.add('active');
            }
        }

        function prevStep(step) {
            document.querySelectorAll('.step-content').forEach(el => el.classList.remove('active'));
            document.getElementById('step-' + step).classList.add('active');

            document.querySelectorAll('.wizard-step').forEach(el => el.classList.remove('active'));
            for (let i = 1; i <= step; i++) {
                document.getElementById('header-step-' + i).classList.add('active');
            }
        }

        // Agreement Validation
        function ValidateAgreement(source, args) {
            var chk = document.getElementById('<%= chk_agreement.ClientID %>');
            args.IsValid = chk.checked;
        }

        // Address Copy Logic
        function CopyAddress() {
            var chk = document.getElementById('chk_same_address');
            var permanent = document.getElementById('<%= txt_permanent_address.ClientID %>');
            var present = document.getElementById('<%= txt_present_address.ClientID %>');

            if (chk.checked) {
                present.value = permanent.value;
                present.readOnly = true;
                present.style.backgroundColor = "#f0f2f5";
            } else {
                present.value = '';
                present.readOnly = false;
                present.style.backgroundColor = "";
            }

            // Force ASP.NET Validator to re-check the present address field so the error hides
            if (typeof ValidatorValidate === "function") {
                var presentValidator = document.getElementById('<%= RequiredFieldValidator2.ClientID %>');
                if (presentValidator) {
                    ValidatorValidate(presentValidator);
                }
            }
        }

        // Auto-update present address if checking while editing
        document.getElementById('<%= txt_permanent_address.ClientID %>').addEventListener('input', function () {
            var chk = document.getElementById('chk_same_address');
            if (chk.checked) {
                document.getElementById('<%= txt_present_address.ClientID %>').value = this.value;

                if (typeof ValidatorValidate === "function") {
                    var presentValidator = document.getElementById('<%= RequiredFieldValidator2.ClientID %>');
                    if (presentValidator) {
                        ValidatorValidate(presentValidator);
                    }
                }
            }
        });

        // Sync name to agreement
        document.getElementById('<%= txt_name.ClientID %>').addEventListener('input', function (e) {
            var lbl = document.getElementById('span_agreement_name');
            lbl.innerText = e.target.value ? e.target.value : "[Your Name]";
        });
    </script>

    <script>
        // Init Datepicker
        $('.pickadate').pickadate({
            format: 'dd/mm/yyyy',
            selectMonths: true,
            selectYears: true,
            closeOnSelect: true
        });
    </script>

</asp:Content>
