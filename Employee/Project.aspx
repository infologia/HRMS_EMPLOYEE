<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Project.aspx.cs" Inherits="Employee_Project" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
       <style>
        /* Label spacing */
        .panel-body label {
            display: block;
            margin-bottom: 6px;
            font-size: 13px;
        }

        .panel-body textarea.form-control {
            margin-bottom: 16px;
            resize: vertical;
        }

        .panel-body .row {
            margin-bottom: 10px;
        }

        @media (max-width: 768px) {

            .panel-body .col-md-4 {
                margin-bottom: 15px;
            }
        }

        /* Document Details Table — exact same as createinvoice.aspx */
        .invoice-table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 8px;
        }

        .invoice-table thead tr {
            background-color: #f0f4f8;
        }

        .invoice-table > thead > tr > th {
            padding: 10px 12px;
            font-size: 13px;
            font-weight: 600;
            border: 1px solid #dde3ea;
            text-align: left;
            color: #444;
        }

        .invoice-table > tbody > tr > td {
            padding: 7px 10px;
            border: 1px solid #dde3ea;
            vertical-align: middle;
        }

        .invoice-table .form-control {
            margin-bottom: 0;
            height: 34px;
        }

        .invoice-table > tbody > tr:hover {
            background-color: #fafbfc;
        }

        .btn-add-row {
            background: #3a7bd5;
            color: #fff;
            border: none;
            border-radius: 4px;
            padding: 7px 16px;
            font-size: 13px;
            font-weight: 500;
            cursor: pointer;
            transition: background 0.2s;
            display: inline-flex;
            align-items: center;
            gap: 5px;
        }

        .btn-add-row:hover {
            background: #2a5fb5;
        }

        .btn-remove-inv {
            background: #e53935;
            color: #fff;
            border: none;
            border-radius: 4px;
            padding: 5px 12px;
            font-size: 12px;
            cursor: pointer;
            transition: background 0.2s;
        }

        .btn-remove-inv:hover {
            background: #b71c1c;
        }
    </style>
    <script type="text/javascript">
    function validateEmployees(sender, args) {
        var listBox = document.getElementById('<%= lstEmployees.ClientID %>');
        var selectedCount = 0;

        for (var i = 0; i < listBox.options.length; i++) {
            if (listBox.options[i].selected) {
                selectedCount++;
            }
        }

        args.IsValid = selectedCount > 0;
    }

    function validateTeamLead(sender, args) {
        var listBox = document.getElementById('<%= lstTeamLead.ClientID %>');
        var selectedCount = 0;

        for (var i = 0; i < listBox.options.length; i++) {
            if (listBox.options[i].selected) {
                selectedCount++;
            }
        }

        args.IsValid = selectedCount > 0;
    }

    // Document Details - Add Row (exact same as createinvoice addRow)
    function addDocumentRow() {
        $("#docDetailsTable tbody").append(`
            <tr>
                <td><input type="text" class="form-control" placeholder="Enter Document Name" name="docName[]" /></td>
                <td>
                    <input type="hidden" name="existingDocFile[]" value="" />
                    <input type="file" class="form-control" name="docFile[]" accept=".pdf, .jpg, .jpeg, .png, .gif, .webp" />
                    <small class="text-muted">Only PDF & Images</small>
                </td>
                <td>
                    <div class="input-group">
                        <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                        <input type="text" class="form-control pickadate" placeholder="DD/MM/YYYY" name="docValidFrom[]" />
                    </div>
                </td>
                <td>
                    <div class="input-group">
                        <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                        <input type="text" class="form-control pickadate" placeholder="DD/MM/YYYY" name="docValidTo[]" />
                    </div>
                </td>
                <td style="text-align:center;"><button type="button" class="btn-remove-inv removeDocRow">Remove</button></td>
            </tr>
        `);

        var today = new Date();
        $('#docDetailsTable tbody tr:last .pickadate').pickadate({
            format: 'dd/mm/yyyy',
            min: today,
            selectMonths: true,
            selectYears: true,
            closeOnSelect: true
        });
    }

    $(document).on("click", ".removeDocRow", function () {
        if ($("#docDetailsTable tbody tr").length > 1) {
            $(this).closest("tr").remove();
        } else {
            alert('At least one document row is required.');
        }
    });

    function openPreview(url, type) {
        $('#previewModal').modal('show');
        
        // Reset and hide all viewers
        $('#previewIframe').hide().attr('src', '');
        $('#previewImage').hide().attr('src', '');
        $('#previewMessage').hide();

        if (type === 'image') {
            $('#previewImage').attr('src', url).show();
        } else if (type === 'pdf') {
            $('#previewIframe').attr('src', url).show();
        } else {
            $('#previewMessage').show();
            // Trigger download for unsupported preview types
            var a = document.createElement('a');
            a.href = url;
            a.download = '';
            a.target = '_blank';
            document.body.appendChild(a);
            a.click();
            document.body.removeChild(a);
        }
    }

    $(document).ready(function() {
        $('#previewModal').on('hidden.bs.modal', function () {
            $('#previewIframe').attr('src', '');
            $('#previewImage').attr('src', '');
        });
    });

    // Handle File Selection to generate local View Attachment link in Create/Edit mode
    $(document).on('change', 'input[name="docFile[]"]', function () {
        var file = this.files[0];
        var parentTd = $(this).closest('td');
        
        // Remove any existing preview links (local or server) so they don't stack up
        parentTd.find('.preview-link').remove();

        if (file) {
            var isImage = file.type.startsWith('image/');
            var isPdf = file.type === 'application/pdf';
            var typeStr = isImage ? 'image' : (isPdf ? 'pdf' : 'other');

            var objectUrl = URL.createObjectURL(file);
            parentTd.append('<br/><a href="javascript:void(0);" onclick="openPreview(\'' + objectUrl + '\', \'' + typeStr + '\')" class="preview-link" style="font-size:12px; color:#3a7bd5;"><i class="icon-eye"></i> View Attachment</a>');
        }
    });
    </script>
  
    <!-- Preview Modal -->
    <div id="previewModal" class="modal fade" tabindex="-1">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h5 class="modal-title">Document Preview</h5>
                </div>
                <div class="modal-body" style="text-align: center;">
                    <iframe id="previewIframe" src="" style="width: 100%; height: 75vh; border: none; display: none;"></iframe>
                    <img id="previewImage" src="" style="max-width: 100%; max-height: 75vh; display: none;" />
                    <div id="previewMessage" style="padding: 50px; display: none;">
                        <i class="icon-file-empty" style="font-size: 48px; color: #ccc;"></i>
                        <h4 style="margin-top: 20px; color: #666;">Preview not available for this file type</h4>
                        <p class="text-muted">The file has been automatically downloaded to your device instead.</p>
                    </div>
                </div>
            </div>
        </div>
    </div>
 

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel panel-flat">
        <div class="panel-heading">
            <h5 class="panel-title">Project Management</h5>
        </div>

        <div class="panel-body">
            <legend class="text-semibold">
                <i class="icon-briefcase position-left"></i>Create Project
            </legend>
             <asp:HiddenField ID="hfProjectKey" runat="server" />

            <!-- Row 1: Project Code | Project Name | Project Type | Client -->
            <div class="row">
                <div class="col-md-3">
                    <label>Project Code <span style="color: red">*</span></label>
                    <asp:TextBox ID="txtProjectCode" runat="server"
                        CssClass="form-control" placeholder="Enter Project Code"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"
    ControlToValidate="txtProjectCode"
    ErrorMessage="Project Code is required"
    ForeColor="Red"  ValidationGroup="vg1" />
                </div>
                <div class="col-md-3">
                    <label>Project Name <span style="color: red">*</span></label>
                    <asp:TextBox ID="txtProjectName" runat="server"
                        CssClass="form-control" placeholder="Enter Project Name"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvProjectName" runat="server"
                        ControlToValidate="txtProjectName"
                        ErrorMessage="Project Name is required"
                        ForeColor="Red"  ValidationGroup="vg1" />
                </div>
                <div class="col-md-3">
                    <label>Project Type <span style="color: red">*</span></label>
                    <asp:DropDownList ID="ddlProjectType" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvProjectType" runat="server"
                        ControlToValidate="ddlProjectType"
                        ErrorMessage="Please select Project Type"
                        ForeColor="Red" ValidationGroup="vg1" InitialValue="0" />
                </div>
                <div class="col-md-3">
                    <label>Client <span style="color: red">*</span></label>
                    <asp:DropDownList ID="ddlClient" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvClient" runat="server"
                        ControlToValidate="ddlClient"
                        ErrorMessage="Please select Client"
                        ForeColor="Red"  ValidationGroup="vg1"  InitialValue="0"/>
                </div>
            </div>

            <!-- Row 2 & 3: All fields in one flex container, 4 per row -->
            <div style="display:flex; flex-wrap:wrap; margin:0 -15px;">
                <div style="flex:0 0 25%; padding:0 15px; margin-bottom:10px;">
                    <label>Start Date <span style="color: red">*</span></label>
                    <div class="input-group">
                        <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="form-control pickadate"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"
                        ControlToValidate="txtStartDate" ErrorMessage="Enter Startdate"
                        ForeColor="Red" ValidationGroup="vg1" />
                </div>
                <div style="flex:0 0 25%; padding:0 15px; margin-bottom:10px;">
                    <label>End Date <span style="color: red">*</span></label>
                    <div class="input-group">
                        <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="form-control pickadate"></asp:TextBox>
                    </div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server"
                        ControlToValidate="txtEndDate" ErrorMessage="Enter Enddate"
                        ForeColor="Red" ValidationGroup="vg1" />
                </div>
                <asp:Panel ID="pnlBudget" runat="server" style="flex:0 0 25%; padding:0 15px; margin-bottom:10px;">
                    <label>Budget <span style="color: red">*</span></label>
                    <asp:TextBox ID="txtBudget" runat="server" TextMode="Number"
                        CssClass="form-control" placeholder="Enter Budget"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server"
                        ControlToValidate="txtBudget" ErrorMessage="Enter Budget"
                        ForeColor="Red" ValidationGroup="vg1" />
                </asp:Panel>
                <div style="flex:0 0 25%; padding:0 15px; margin-bottom:10px;">
                    <label>Estimated Hours</label>
                    <asp:TextBox ID="txtEstimatedHours" runat="server" TextMode="Number"
                        CssClass="form-control" placeholder="Enter Estimated Hours"></asp:TextBox>
                </div>
                <div style="flex:0 0 25%; padding:0 15px; margin-bottom:10px;">
                    <label>Project Manager <span style="color: red">*</span></label>
                    <asp:DropDownList ID="ddlProjectManager" runat="server" CssClass="form-control"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvProjectManager" runat="server"
                        ControlToValidate="ddlProjectManager" ErrorMessage="Please select Project Manager"
                        ForeColor="Red" ValidationGroup="vg1" InitialValue="0" />
                </div>
                <div style="flex:0 0 25%; padding:0 15px; margin-bottom:10px;">
                    <label>Team Lead <span style="color: red">*</span></label>
                    <div class="multi-select-full">
                        <asp:ListBox ID="lstTeamLead" runat="server" CssClass="multiselect form-control" SelectionMode="Multiple"></asp:ListBox>
                    </div>
                    <asp:CustomValidator ID="cvTeamLead" runat="server" ControlToValidate="lstTeamLead"
                        ErrorMessage="Select Team Lead" ForeColor="Red"
                        ClientValidationFunction="validateTeamLead" ValidationGroup="vg1">
                    </asp:CustomValidator>
                </div>
                <div style="flex:0 0 25%; padding:0 15px; margin-bottom:10px;">
                    <label>Project Participants <span style="color: red">*</span></label>
                    <div class="multi-select-full">
                        <asp:ListBox ID="lstEmployees" runat="server" CssClass="multiselect form-control" SelectionMode="Multiple"></asp:ListBox>
                    </div>
                    <asp:CustomValidator ID="cvEmployees" runat="server" ControlToValidate="lstEmployees"
                        ErrorMessage="Select project participants" ForeColor="Red"
                        ClientValidationFunction="validateEmployees" ValidationGroup="vg1">
                    </asp:CustomValidator>
                </div>
                <div style="flex:0 0 25%; padding:0 15px; margin-bottom:10px;">
                    <label>Status <span style="color: red">*</span></label>
                    <asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control">
                        <asp:ListItem Text="Select Status" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Planned" Value="Planned"></asp:ListItem>
                        <asp:ListItem Text="In Progress" Value="In Progress"></asp:ListItem>
                        <asp:ListItem Text="Completed" Value="Completed"></asp:ListItem>
                        <asp:ListItem Text="On Hold" Value="On Hold"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server"
                        ControlToValidate="ddlStatus" InitialValue="0"
                        ErrorMessage="Select Status" ForeColor="Red" ValidationGroup="vg1" />
                </div>
            </div>

            <!-- Row 4: Description -->
            <div class="row">
                <div class="col-md-12">
                    <label>Description</label>
                    <asp:TextBox ID="txtDescription" runat="server"
                        CssClass="form-control" TextMode="MultiLine" Rows="3"
                        placeholder="Enter Project Description"></asp:TextBox>
                </div>
            </div>

            <br />

            <!-- Document Details Section -->
            <legend class="text-semibold" style="margin-bottom: 8px;">
                <i class="icon-file-text2 position-left"></i>Document Details
            </legend>
              <div style="margin-top: 10px;">
      <button type="button" class="btn-add-row" onclick="addDocumentRow()">
          <i class="icon-plus2"></i> Add Row
      </button>
  </div>
            <table class="invoice-table" id="docDetailsTable">
                <thead>
                    <tr>
                        <th style="width:200px;">Document Name</th>
                        <th style="width:290px;">Upload File</th>
                        <th style="width:300px;">Validity From</th>
                        <th style="width:300px;">Validity To</th>
                        <th style="width:80px; text-align:center;">Action</th>
                    </tr>
                </thead>
                <tbody id="tBodyDocs" runat="server">
                    <tr>
                        <td><input type="text" class="form-control" placeholder="Enter Document Name" name="docName[]" /></td>
                        <td>
                            <input type="hidden" name="existingDocFile[]" value="" />
                            <input type="file" class="form-control" name="docFile[]" accept=".pdf, .jpg, .jpeg, .png, .gif, .webp" />
                            <small class="text-muted">Only PDF & Images</small>
                        </td>
                        <td>
                            <div class="input-group">
                                <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                <input type="text" class="form-control pickadate" placeholder="DD/MM/YYYY" name="docValidFrom[]" />
                            </div>
                        </td>
                        <td>
                            <div class="input-group">
                                <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                <input type="text" class="form-control pickadate" placeholder="DD/MM/YYYY" name="docValidTo[]" />
                            </div>
                        </td>
                        <td style="text-align:center;"><button type="button" class="btn-remove-inv removeDocRow">Remove</button></td>
                    </tr>
                </tbody>
            </table>

          

            <br />

           
            <div class="row  pull-right">
                <div class="col-lg-12 pull-right">
                    <a href="Projectgrid.aspx" class="btn btn-primary">Back</a>
                    <asp:Button ID="btnSave" runat="server"
                        Text="Save"
                        CssClass="btn btn-primary"
                        OnClick="btnSave_Click"  ValidationGroup="vg1" />
                    <asp:Button ID="btnUpdate" runat="server"
                        Text="Update"
                        CssClass="btn btn-primary"
                        OnClick="btnUpdate_Click"
                        Visible="false"  ValidationGroup="vg1" />
                </div>
            
</div>
        </div>
    </div>
     <script>
         var today = new Date();

         $('.pickadate').pickadate({
             format: 'dd/mm/yyyy',
             min: today,
             selectMonths: true,
             selectYears: true,
             closeOnSelect: true
         });
     </script>
</asp:Content>
