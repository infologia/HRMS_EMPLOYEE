<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="generatelead.aspx.cs" Inherits="Admin_generatelead" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .form-control { position: relative !important; z-index: 10 !important; pointer-events: auto !important; }
        @media (max-width: 768px) { .form-control { margin-bottom: 12px; } }
        .nav-tabs > li > a { font-weight: 600; }
        .tab-content { padding: 20px 0; }
        .cp-table { width: 100%; border-collapse: collapse; margin-top: 8px; }
        .cp-table thead tr { background-color: #f0f4f8; }
        .cp-table > thead > tr > th { padding: 8px 10px; font-size: 12px; font-weight: 600; border: 1px solid #dde3ea; text-align: left; color: #444; }
        .cp-table > tbody > tr > td { padding: 5px 8px; border: 1px solid #dde3ea; vertical-align: middle; }
        .cp-table .form-control { margin-bottom: 0; height: 30px; font-size: 12px; padding: 4px 8px; }
        .cp-table > tbody > tr:hover { background-color: #fafbfc; }
        .btn-add-row { background: #3a7bd5; color: #fff; border: none; border-radius: 4px; padding: 6px 14px; font-size: 12px; font-weight: 500; cursor: pointer; display: inline-flex; align-items: center; gap: 5px; }
        .btn-add-row:hover { background: #2a5fb5; }
        .btn-remove-cp { background: #e53935; color: #fff; border: none; border-radius: 4px; padding: 3px 10px; font-size: 12px; cursor: pointer; }
        .btn-remove-cp:hover { background: #b71c1c; }
        .section-legend { font-size: 14px; font-weight: 600; margin-bottom: 15px; color: #555; border-bottom: 1px solid #ddd; padding-bottom: 8px; }
    </style>
    <script type="text/javascript">
        // Collect all contact rows into JSON and store in hidden field before form submits
        function serializeContactsToHiddenField() {
            var rows = [];
            $("#cpTableBody tr").each(function () {
                var $tr = $(this);
                rows.push({
                    FirstName:        $tr.find('[data-field="FirstName"]').val()        || '',
                    LastName:         $tr.find('[data-field="LastName"]').val()         || '',
                    Designation:      $tr.find('[data-field="Designation"]').val()      || '',
                    Department:       $tr.find('[data-field="Department"]').val()       || '',
                    Email:            $tr.find('[data-field="Email"]').val()            || '',
                    MobileNumber:     $tr.find('[data-field="MobileNumber"]').val()     || '',
                    LinkedInProfile:  $tr.find('[data-field="LinkedInProfile"]').val()  || '',
                    NextFollowUpDate: $tr.find('[data-field="NextFollowUpDate"]').val() || ''
                });
            });
            var hf = document.getElementById('<%= hfContactPersons.ClientID %>');
            if (hf) hf.value = JSON.stringify(rows);
        }

        function addContactPersonRow() {
            $("#cpTableBody").append(
                '<tr>' +
                '<td><input type="text" class="form-control" data-field="FirstName"        placeholder="First Name" /></td>' +
                '<td><input type="text" class="form-control" data-field="LastName"         placeholder="Last Name" /></td>' +
                '<td><input type="text" class="form-control" data-field="Designation"      placeholder="Job Title" /></td>' +
                '<td><input type="text" class="form-control" data-field="Department"       placeholder="Department" /></td>' +
                '<td><input type="email" class="form-control" data-field="Email"           placeholder="Email" /></td>' +
                '<td><input type="text" class="form-control" data-field="MobileNumber"     placeholder="Mobile" /></td>' +
                '<td><input type="text" class="form-control" data-field="LinkedInProfile"  placeholder="LinkedIn URL" /></td>' +
                '<td><input type="date" class="form-control" data-field="NextFollowUpDate" /></td>' +
                '<td style="text-align:center;"><button type="button" class="btn-remove-cp removeCpRow">Remove</button></td>' +
                '</tr>'
            );
        }

        $(document).on('click', '.removeCpRow', function () {
            if ($("#cpTableBody tr").length > 1) {
                $(this).closest('tr').remove();
            } else {
                alert('At least one contact person row is required.');
            }
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form-horizontal">
        <div class="panel panel-flat">
            <div class="panel-heading">
                <h5 class="panel-title"></h5>
            </div>
            <div class="panel-body">
                <legend class="text-semibold"><i class="icon-reading position-left"></i>Create / Update Lead</legend>
                <asp:HiddenField ID="hfContactPersons" runat="server" />
                <ul class="nav nav-tabs" id="leadTabs">
                    <li class="active"><a href="#tab-basic" data-toggle="tab">Basic Info</a></li>
                    <li><a href="#tab-company" data-toggle="tab">Company Details</a></li>
                    <li><a href="#tab-source" data-toggle="tab">Source & Status</a></li>
                    <li><a href="#tab-person" data-toggle="tab">Contacts</a></li>
                </ul>

                <div class="tab-content">
                    <!-- TAB 1: BASIC INFO -->
                    <div class="tab-pane active" id="tab-basic">
                        <div class="row">
                            <div class="col-md-4">
                                <label>Company Name<span style="color: red"> *</span></label>
                                <asp:TextBox ID="txtCompanyName" runat="server" CssClass="form-control" placeholder="Enter Company Name"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvCompanyName" runat="server" ControlToValidate="txtCompanyName" ErrorMessage="Company Name is required." ForeColor="Red" Display="Dynamic" />
                            </div>
                            <div class="col-md-4">
                                <label>Legal Company Name</label>
                                <asp:TextBox ID="txtLegalCompanyName" runat="server" CssClass="form-control" placeholder="Registered legal name"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label>Company Code</label>
                                <asp:TextBox ID="txtCompanyCode" runat="server" CssClass="form-control" placeholder="Internal unique code"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row mt-10">
                            <div class="col-md-4">
                                <label>Industry</label>
                                <asp:TextBox ID="txtIndustry" runat="server" CssClass="form-control" placeholder="e.g. Logistics"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label>Business Type</label>
                                <asp:TextBox ID="txtBusinessType" runat="server" CssClass="form-control" placeholder="e.g. Freight Forwarder"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label>Website</label>
                                <asp:TextBox ID="txtWebsite" runat="server" CssClass="form-control" placeholder="Company Website"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row mt-10">
                            <div class="col-md-4">
                                <label>Status</label><br />
                                <input type="radio" name="rblIsActive" id="rblActive" value="1" checked="checked" onclick="document.getElementById('hfIsActive').value='1';" /> <label for="rblActive">Active</label>
                                &nbsp;&nbsp;
                                <input type="radio" name="rblIsActive" id="rblInactive" value="0" onclick="document.getElementById('hfIsActive').value='0';" /> <label for="rblInactive">Inactive</label>
                                <input type="hidden" id="hfIsActive" name="hfIsActive" value="1" />
                            </div>
                        </div>
                    </div>
                    <!-- END TAB 1 -->

                    <!-- TAB 2: COMPANY DETAILS -->
                    <div class="tab-pane" id="tab-company">
                        <p class="section-legend">Company Metrics & Location</p>
                        <div class="row">
                            <div class="col-md-4">
                                <label>Company Size</label>
                                <asp:TextBox ID="txtCompanySize" runat="server" CssClass="form-control" placeholder="e.g. 11-50"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label>Employee Count</label>
                                <asp:TextBox ID="txtEmployeeCount" runat="server" CssClass="form-control" TextMode="Number" placeholder="Exact employee count"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label>Annual Revenue</label>
                                <asp:TextBox ID="txtAnnualRevenue" runat="server" CssClass="form-control" TextMode="Number" placeholder="Optional"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row mt-10">
                            <div class="col-md-4">
                                <label>Country</label>
                                <asp:TextBox ID="txtCountry" runat="server" CssClass="form-control" placeholder="Country"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label>State</label>
                                <asp:TextBox ID="txtState" runat="server" CssClass="form-control" placeholder="State"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label>City</label>
                                <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" placeholder="City"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row mt-10">
                            <div class="col-md-8">
                                <label>Address</label>
                                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="1" placeholder="Company address"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <label>Postal Code</label>
                                <asp:TextBox ID="txtPostalCode" runat="server" CssClass="form-control" placeholder="ZIP/Post Code"></asp:TextBox>
                            </div>
                            <div class="col-md-2">
                                <label>Time Zone</label>
                                <asp:TextBox ID="txtTimeZone" runat="server" CssClass="form-control" placeholder="Time zone"></asp:TextBox>
                            </div>
                        </div>
                        <p class="section-legend" style="margin-top: 20px;">Communication Channels</p>
                        <div class="row">
                            <div class="col-md-4">
                                <label>Company Phone</label>
                                <asp:TextBox ID="txtCompanyPhone" runat="server" CssClass="form-control" placeholder="Main office phone"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label>Company Email</label>
                                <asp:TextBox ID="txtCompanyEmail" runat="server" CssClass="form-control" placeholder="General email"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <label>LinkedIn URL</label>
                                <asp:TextBox ID="txtLinkedInURL" runat="server" CssClass="form-control" placeholder="LinkedIn company page"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row mt-10">
                            <div class="col-md-12">
                                <label>Contact Page URL</label>
                                <asp:TextBox ID="txtContactPageURL" runat="server" CssClass="form-control" placeholder="Contact page URL"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <!-- END TAB 2 -->

                    <!-- TAB 3: SOURCE & STATUS -->
                    <div class="tab-pane" id="tab-source">
                        <p class="section-legend">Lead Generation Info</p>
                        <div class="row">
                            <div class="col-md-6">
                                <label>Source</label>
                                <asp:TextBox ID="txtSource" runat="server" CssClass="form-control" placeholder="LinkedIn, Apollo, Website..."></asp:TextBox>
                            </div>
                            <div class="col-md-6">
                                <label>Source URL</label>
                                <asp:TextBox ID="txtSourceURL" runat="server" CssClass="form-control" placeholder="Original source URL"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row mt-10">
                            <div class="col-md-4">
                                <label>Lead Status</label>
                                <asp:DropDownList ID="ddlLeadStatus" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="New" Value="New"></asp:ListItem>
                                    <asp:ListItem Text="Contacted" Value="Contacted"></asp:ListItem>
                                    <asp:ListItem Text="Qualified" Value="Qualified"></asp:ListItem>
                                    <asp:ListItem Text="Lost" Value="Lost"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4">
                                <label>Priority</label>
                                <asp:DropDownList ID="ddlPriority" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="High" Value="High"></asp:ListItem>
                                    <asp:ListItem Text="Medium" Value="Medium"></asp:ListItem>
                                    <asp:ListItem Text="Low" Value="Low"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-4">
                                <label>Notes</label>
                                <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="1" placeholder="Internal notes"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                    <!-- END TAB 3 -->

                    <!-- TAB 4: CONTACTS -->
                    <div class="tab-pane" id="tab-person">
                        <table class="cp-table" id="contactPersonTable">
                            <thead>
                                <tr>
                                    <th>First Name</th>
                                    <th>Last Name</th>
                                    <th>Designation</th>
                                    <th>Department</th>
                                    <th>Email</th>
                                    <th>Mobile</th>
                                    <th>LinkedIn Profile</th>
                                    <th>Next Follow Up</th>
                                    <th style="width: 60px; text-align: center;">Action</th>
                                </tr>
                            </thead>
                            <tbody id="cpTableBody">
                                <asp:Repeater ID="rptContacts" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><input type="text"  class="form-control" data-field="FirstName"        placeholder="First Name" value='<%# AttrEncode(Eval("FirstName")) %>' /></td>
                                            <td><input type="text"  class="form-control" data-field="LastName"         placeholder="Last Name" value='<%# AttrEncode(Eval("LastName")) %>' /></td>
                                            <td><input type="text"  class="form-control" data-field="Designation"      placeholder="Job Title" value='<%# AttrEncode(Eval("Designation")) %>' /></td>
                                            <td><input type="text"  class="form-control" data-field="Department"       placeholder="Department" value='<%# AttrEncode(Eval("Department")) %>' /></td>
                                            <td><input type="email" class="form-control" data-field="Email"            placeholder="Email" value='<%# AttrEncode(Eval("Email")) %>' /></td>
                                            <td><input type="text"  class="form-control" data-field="MobileNumber"     placeholder="Mobile" value='<%# AttrEncode(Eval("MobileNumber")) %>' /></td>
                                            <td><input type="text"  class="form-control" data-field="LinkedInProfile"  placeholder="LinkedIn URL" value='<%# AttrEncode(Eval("LinkedInProfile")) %>' /></td>
                                            <td><input type="date"  class="form-control" data-field="NextFollowUpDate" value='<%# FormatContactDate(Eval("NextFollowUpDate")) %>' /></td>
                                            <td style="text-align: center;"><button type="button" class="btn-remove-cp removeCpRow">Remove</button></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <asp:PlaceHolder ID="phBlankRow" runat="server">
                                <tr>
                                    <td><input type="text"  class="form-control" data-field="FirstName"        placeholder="First Name" /></td>
                                    <td><input type="text"  class="form-control" data-field="LastName"         placeholder="Last Name" /></td>
                                    <td><input type="text"  class="form-control" data-field="Designation"      placeholder="Job Title" /></td>
                                    <td><input type="text"  class="form-control" data-field="Department"       placeholder="Department" /></td>
                                    <td><input type="email" class="form-control" data-field="Email"            placeholder="Email" /></td>
                                    <td><input type="text"  class="form-control" data-field="MobileNumber"     placeholder="Mobile" /></td>
                                    <td><input type="text"  class="form-control" data-field="LinkedInProfile"  placeholder="LinkedIn URL" /></td>
                                    <td><input type="date"  class="form-control" data-field="NextFollowUpDate" /></td>
                                    <td style="text-align: center;"><button type="button" class="btn-remove-cp removeCpRow">Remove</button></td>
                                </tr>
                                </asp:PlaceHolder>
                            </tbody>
                        </table>
                        <div style="margin-top: 8px;">
                            <button type="button" class="btn-add-row" onclick="addContactPersonRow()">
                                <i class="icon-plus2"></i> Add Row
                            </button>
                        </div>
                    </div>
                    <!-- END TAB 4 -->
                </div>

                <div class="row mt-10">
                    <div class="col-md-12">
                        <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>
                    </div>
                </div>

                <div class="row" style="margin-top: 25px;">
                    <div class="col-lg-12 text-right">
                        <asp:Button ID="btnCancel" runat="server" Text="Back" CssClass="btn btn-primary" OnClick="btnCancel_Click" CausesValidation="false" />
                        <asp:Button ID="btnSave" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSave_Click" OnClientClick="serializeContactsToHiddenField();" />
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
