<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Clientsdetails.aspx.cs" Inherits="Admin_Clientsdetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .form-control {
            position: relative !important;
            z-index: 10 !important;
            pointer-events: auto !important;
        }

        @media (max-width: 768px) {
            .form-control {
                margin-bottom: 12px;
            }
        }

        .nav-tabs > li > a {
            font-weight: 600;
        }

        .tab-content {
            padding: 20px 0;
        }

        .section-legend {
            font-size: 14px;
            font-weight: 600;
            margin-bottom: 15px;
            color: #555;
            border-bottom: 1px solid #ddd;
            padding-bottom: 8px;
        }

        .file-upload-row {
            display: flex;
            align-items: center;
            margin-bottom: 10px;
            gap: 10px;
        }

            .file-upload-row label {
                min-width: 200px;
                margin: 0;
            }

            .file-upload-row .btn-danger {
                padding: 4px 10px;
                font-size: 12px;
            }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form-horizontal">
        <div class="panel panel-flat">
            <div class="panel-heading">
                <h5 class="panel-title"></h5>
            </div>
            <div class="panel-body">
                <legend class="text-semibold"><i class="icon-reading position-left"></i>Create Organization</legend>

                <!-- ===================== TAB NAVIGATION ===================== -->
                <ul class="nav nav-tabs" id="clientTabs">
                    <li class="active"><a href="#tab-basic" data-toggle="tab">Basic Info</a></li>
                    <li><a href="#tab-bank" data-toggle="tab">Bank Details</a></li>
                    <li><a href="#tab-contact" data-toggle="tab">Contact Details</a></li>
                    <li><a href="#tab-accountmgr" data-toggle="tab">Account Manager</a></li>
                    <li><a href="#tab-documents" data-toggle="tab">Documents</a></li>
                    <li><a href="#tab-contract" data-toggle="tab">Contract Information</a></li>
                </ul>

                <div class="tab-content">

                    <!-- ==================== TAB 1: BASIC INFO ==================== -->
                    <div class="tab-pane active" id="tab-basic">
                        <div class="row">
                            <div class="col-md-4" id="div_ClientCode">
                                <label>Organization Code<span style="color: red"> *</span></label>
                                <asp:TextBox ID="txt_ClientCode" runat="server" Class="form-control" placeholder="MM_YYYY_01"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator_name" runat="server" ControlToValidate="txt_ClientCode" ErrorMessage="Organization Code is a required field." ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-4" id="div_ClientName">
                                <label>Organization Name<span style="color: red"> *</span></label>
                                <asp:TextBox ID="txt_ClientName" runat="server" Class="form-control" placeholder="Enter Organization Name"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_ClientName" ErrorMessage="Organization Name is a required field." ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-4">
                                <label>Invoicing Company Name<span style="color: red"> *</span></label>
                                <asp:TextBox ID="txt_CompanyName" runat="server" Class="form-control" placeholder="Enter Company Name"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_CompanyName" ErrorMessage="Company Name is a required field." ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <label>Type<span style="color: red"> *</span></label>
                                <asp:DropDownList ID="ddl_Type" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator_Type" runat="server" ControlToValidate="ddl_Type" ErrorMessage="Type is a required field." ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-4">
                                <label>Tax Type<span style="color: red"> *</span></label>
                                <asp:DropDownList ID="DD_TaxType" runat="server" CssClass="form-control"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator_TaxType" runat="server" ControlToValidate="DD_TaxType" ErrorMessage="Tax Type is a required field." ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-4">
                                <label>Contact Person</label>
                                <asp:TextBox ID="txt_ContactPerson" runat="server" Class="form-control" placeholder="Primary contact person name"></asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-4">
                                <label>Designation</label>
                                <asp:TextBox ID="txt_Designation" runat="server" Class="form-control" placeholder="Contact person role/designation"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label>Email</label>
                                <asp:TextBox ID="txt_email" runat="server" Class="form-control" placeholder="Enter Email"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txt_email" ErrorMessage="Enter a Valid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red"></asp:RegularExpressionValidator>
                            </div>
                            <div class="col-md-4">
                                <label>Alternate Email</label>
                                <asp:TextBox ID="txt_AlternateEmail" runat="server" Class="form-control" placeholder="Enter Alternate Email"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txt_AlternateEmail" ErrorMessage="Optional secondary email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red"></asp:RegularExpressionValidator>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-4">
                                <label>Mobile</label>
                                <asp:TextBox ID="txt_mobile" runat="server" Class="form-control" placeholder="Enter Mobile"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Style="color: red" ControlToValidate="txt_mobile" ErrorMessage="Enter a Valid Phone number" ValidationExpression="[0-9]{10}"></asp:RegularExpressionValidator>
                            </div>
                            <div class="col-md-4">
                                <label>Alternate Mobile</label>
                                <asp:TextBox ID="txt_AlternateMobile" runat="server" Class="form-control" placeholder="Enter Alternate Mobile"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" Style="color: red" ControlToValidate="txt_AlternateMobile" ErrorMessage="Secondary mobile number" ValidationExpression="[0-9]{10}"></asp:RegularExpressionValidator>
                            </div>
                            <div class="col-md-4">
                                <label>Industry</label>
                                <asp:TextBox ID="txt_Industry" runat="server" Class="form-control" placeholder="Industry category"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-4">
                                <label>Website</label>
                                <asp:TextBox ID="txt_Website" runat="server" Class="form-control" placeholder="Company Website URL"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label>Address Line 1</label>
                                <asp:TextBox ID="txt_AddressLine1" runat="server" Class="form-control" placeholder="Company primary address"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label>Address Line 2</label>
                                <asp:TextBox ID="txt_AddressLine2" runat="server" Class="form-control" placeholder="Company branch address"></asp:TextBox>
                            </div>
                        </div>
                        <br />
                        <div class="row">
                            <div class="col-md-4">
                                <label>Country<span style="color: red"> *</span></label>
                                <asp:DropDownList ID="ddl_Country" runat="server" Class="form-control"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatorCountry" runat="server" ControlToValidate="ddl_Country" ErrorMessage="Country is a required field." ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-4">
                                <label>Status</label>
                                <asp:DropDownList ID="ddl_Clientstatus" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Active" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4">
                                <label>Boarded by <span style="color: red">*</span></label>
                                <asp:DropDownList ID="ddl_OnboardBy" runat="server" Class="form-control"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator_ddl_OnboardBy" ControlToValidate="ddl_OnboardBy" runat="server" ErrorMessage="Please Select Employee." ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="row mt-10">
                            <div class="col-md-4">
                                <label>Source</label>
                                <asp:TextBox ID="txt_Source" runat="server" Class="form-control" placeholder="Client Source"></asp:TextBox>
                            </div>
                            <div class="col-md-8">
                                <label>Description</label>
                                <asp:TextBox ID="txt_Description" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="1" placeholder="Additional notes"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row mt-10">
                            <div class="col-md-4">
                                <label>Sales Person <span style="color: red">*</span></label>
                                <asp:DropDownList ID="ddlsalesperson" runat="server" Class="form-control"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="ddlsalesperson" runat="server" ErrorMessage="Please Select Employee." ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <!-- END TAB 1 -->

                    <!--  TAB 2: BANK DETAILS -->
                    <div class="tab-pane" id="tab-bank">
                        <p class="section-legend">Bank Details</p>
                        <div class="row mt-10">
                            <div class="col-md-4">
                                <label>Bank Name</label>
                                <asp:TextBox ID="txt_BankName" runat="server" Class="form-control" placeholder="Enter Bank Name"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label>Account Holder Name</label>
                                <asp:TextBox ID="txt_AccountHolderName" runat="server" Class="form-control" placeholder="Enter Account Holder Name"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label>Account Number</label>
                                <asp:TextBox ID="txt_AccountNumber" runat="server" Class="form-control" placeholder="Enter Account Number"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row mt-10">
                            <div class="col-md-4">
                                <label>IFSC Code / Swift Code</label>
                                <asp:TextBox ID="txt_IFSCCode" runat="server" Class="form-control" placeholder="Enter IFSC / Swift Code"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label>Branch</label>
                                <asp:TextBox ID="txt_Branch" runat="server" Class="form-control" placeholder="Enter Branch Name"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label>Bank Address</label>
                                <asp:TextBox ID="txt_BankAddress" runat="server" Class="form-control" placeholder="Enter Bank Address"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <!-- END TAB 2 -->

                    <!--TAB 3: CONTACT DETAILS  -->
                    <div class="tab-pane" id="tab-contact">
                        <p class="section-legend">Contact Details</p>
                        <div class="row mt-10">
                            <div class="col-md-4">
                                <label>Contact Name</label>
                                <asp:TextBox ID="txt_ContactName" runat="server" Class="form-control" placeholder="Enter Contact Name"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label>Title / Designation</label>
                                <asp:TextBox ID="txt_ContactTitle" runat="server" Class="form-control" placeholder="Enter Title or Designation"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label>Department</label>
                                <asp:TextBox ID="txt_Department" runat="server" Class="form-control" placeholder="Enter Department"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row mt-10">
                            <div class="col-md-4">
                                <label>Telephone</label>
                                <asp:TextBox ID="txt_Telephone" runat="server" Class="form-control" placeholder="Enter Telephone Number"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label>Mobile</label>
                                <asp:TextBox ID="txt_ContactMobile" runat="server" Class="form-control" placeholder="Enter Mobile Number"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label>Email</label>
                                <asp:TextBox ID="txt_ContactEmail" runat="server" Class="form-control" placeholder="Enter Email"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txt_ContactEmail" ErrorMessage="Enter a Valid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red"></asp:RegularExpressionValidator>
                            </div>
                        </div>
                    </div>
                    <!-- END TAB 3 -->

                    <!-- TAB 4: ACCOUNT MANAGER  -->
                    <div class="tab-pane" id="tab-accountmgr">
                        <p class="section-legend">Account Manager</p>
                        <div class="row">
                            <div class="col-md-4">
                                <label>Account Manager Name</label>
                                <asp:TextBox ID="txt_AccMgrName" runat="server" Class="form-control" placeholder="Enter Account Manager Name"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label>Account Manager Email</label>
                                <asp:TextBox ID="txt_AccMgrEmail" runat="server" Class="form-control" placeholder="Enter Account Manager Email"></asp:TextBox>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txt_AccMgrEmail" ErrorMessage="Enter a Valid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red"></asp:RegularExpressionValidator>
                            </div>
                            <div class="col-md-4">
                                <label>Account Manager Mobile</label>
                                <asp:TextBox ID="txt_AccMgrMobile" runat="server" Class="form-control" placeholder="Enter Account Manager Mobile"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-4">
                                <label>Assigned Date</label>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                    <asp:TextBox ID="txt_AssignedDate" runat="server" Class="form-control pickadate" TextMode="Date"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <label>Last Follow-up Date</label>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                    <asp:TextBox ID="txt_LastFollowUpDate" runat="server" Class="form-control pickadate" TextMode="Date"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- END TAB 4 -->

                    <!--  TAB 5: DOCUMENTS -->
                    <div class="tab-pane" id="tab-documents">
                        <p class="section-legend">Documents</p>
                        <div class="row">
                            <div class="col-md-12">
                                <table class="table table-bordered">
                                    <thead>
                                        <tr>
                                            <th>Document Name</th>
                                            <th>Upload File</th>
                                            <th>Action</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr>
                                            <td>Contract Copy</td>
                                            <td>
                                                <asp:FileUpload ID="fu_ContractCopy" runat="server" CssClass="form-control" />
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="lnk_RemoveContractCopy" runat="server" CssClass="btn btn-danger btn-xs" OnClick="lnk_RemoveContractCopy_Click">Remove</asp:LinkButton>
                                                <asp:HyperLink ID="hl_ContractCopy" runat="server" Target="_blank" Visible="false" CssClass="btn btn-info btn-xs">View</asp:HyperLink>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>NDA</td>
                                            <td>
                                                <asp:FileUpload ID="fu_NDA" runat="server" CssClass="form-control" />
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="lnk_RemoveNDA" runat="server" CssClass="btn btn-danger btn-xs" OnClick="lnk_RemoveNDA_Click">Remove</asp:LinkButton>
                                                <asp:HyperLink ID="hl_NDA" runat="server" Target="_blank" Visible="false" CssClass="btn btn-info btn-xs">View</asp:HyperLink>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>SOW (Statement of Work)</td>
                                            <td>
                                                <asp:FileUpload ID="fu_SOW" runat="server" CssClass="form-control" />
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="lnk_RemoveSOW" runat="server" CssClass="btn btn-danger btn-xs" OnClick="lnk_RemoveSOW_Click">Remove</asp:LinkButton>
                                                <asp:HyperLink ID="hl_SOW" runat="server" Target="_blank" Visible="false" CssClass="btn btn-info btn-xs">View</asp:HyperLink>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Other Supporting Documents</td>
                                            <td>
                                                <asp:FileUpload ID="fu_OtherDocs" runat="server" CssClass="form-control" />
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="lnk_RemoveOtherDocs" runat="server" CssClass="btn btn-danger btn-xs" OnClick="lnk_RemoveOtherDocs_Click">Remove</asp:LinkButton>
                                                <asp:HyperLink ID="hl_OtherDocs" runat="server" Target="_blank" Visible="false" CssClass="btn btn-info btn-xs">View</asp:HyperLink>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                        </div>
                        <%-- Hidden fields to store existing file paths --%>
                        <asp:HiddenField ID="hf_ContractCopyPath" runat="server" />
                        <asp:HiddenField ID="hf_NDAPath" runat="server" />
                        <asp:HiddenField ID="hf_SOWPath" runat="server" />
                        <asp:HiddenField ID="hf_OtherDocsPath" runat="server" />
                    </div>
                    <!-- END TAB 5 -->

                    <!-- ==================== TAB 6: CONTRACT INFORMATION ==================== -->
                    <div class="tab-pane" id="tab-contract">
                        <p class="section-legend">Contract Information</p>
                        <div class="row mt-10">
                            <div class="col-md-4">
                                <label>Contract Number</label>
                                <asp:TextBox ID="txt_ContractNumber" runat="server" Class="form-control" placeholder="Enter Contract Number"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label>Contract Start Date</label>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                    <asp:TextBox ID="txt_ContractStartDate" runat="server" Class="form-control pickadate" TextMode="Date"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <label>Contract End Date</label>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                    <asp:TextBox ID="txt_ContractEndDate" runat="server" Class="form-control pickadate" TextMode="Date"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row mt-10">
                            <div class="col-md-4">
                                <label>Contract Type</label>
                                <asp:DropDownList ID="ddl_ContractType" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="-- Select Contract Type --" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Fixed Price" Value="Fixed Price"></asp:ListItem>
                                    <asp:ListItem Text="Time & Material" Value="Time & Material"></asp:ListItem>
                                    <asp:ListItem Text="Retainer" Value="Retainer"></asp:ListItem>
                                    <asp:ListItem Text="Milestone Based" Value="Milestone Based"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4">
                                <label>Renewal Date</label>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                    <asp:TextBox ID="txt_RenewalDate" runat="server" Class="form-control pickadate" TextMode="Date"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-4">
                                <label>Notice Period (Days)</label>
                                <asp:TextBox ID="txt_NoticePeriod" runat="server" Class="form-control" placeholder="e.g. 30"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row mt-10">
                            <div class="col-md-4">
                                <label>Contract Status</label>
                                <asp:DropDownList ID="ddl_ContractStatus" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="-- Select Status --" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Active" Value="Active"></asp:ListItem>
                                    <asp:ListItem Text="Expired" Value="Expired"></asp:ListItem>
                                    <asp:ListItem Text="Pending Renewal" Value="Pending Renewal"></asp:ListItem>
                                    <asp:ListItem Text="Terminated" Value="Terminated"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-8">
                                <label>SLA Details</label>
                                <asp:TextBox ID="txt_SLADetails" runat="server" Class="form-control" TextMode="MultiLine" Rows="2" placeholder="Enter SLA details"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <!-- END TAB 6 -->

                </div>
                <!-- END TAB CONTENT -->

                <!-- ===================== ACTION BUTTONS ===================== -->
                <div class="row" style="margin-top: 25px;">
                    <div class="col-lg-12 text-right">
                        <a href="Clients.aspx" class="btn btn-primary">Back</a>
                        <asp:Button ID="btn_request" runat="server" Text="Create" Class="btn btn-primary" OnClick="btn_request_Click" Visible="false"></asp:Button>
                        <asp:Button ID="btn_update" runat="server" Text="Update" Class="btn btn-primary" OnClick="btn_update_Click" Visible="false"></asp:Button>
                    </div>
                </div>

            </div>
        </div>
    </div>
    <script>
        $('.pickadate').pickadate({
            format: 'dd/mm/yyyy',
            selectMonths: true,
            selectYears: true,
            closeOnSelect: true
        });


        $('#<%= ddl_Type.ClientID %>').on('change', toggleTypeFields);
        $(document).ready(function () { toggleTypeFields(); });
    </script>
</asp:Content>
