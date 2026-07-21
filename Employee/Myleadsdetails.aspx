<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Myleadsdetails.aspx.cs" Inherits="Employee_Myleadsdetails" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .form-control {
            position: relative !important;
            z-index: 10 !important;
            pointer-events: auto !important;
        }
    </style>
     
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
<div class="form-horizontal">
    <div class="panel panel-flat">

        <!-- Header -->
        <div class="panel-heading">
            <h5 class="panel-title text-semibold">
                <i class="icon-reading position-left"></i>Create Lead
            </h5>
        </div>

        <div class="panel-body">

            <!-- Row 1 -->
            <div class="row mb-3">
                <div class="col-md-4" style="position:relative;">
                    <label>Company <span class="text-danger">*</span></label>
                    <asp:TextBox ID="txt_company" runat="server" CssClass="form-control" placeholder="Enter Company Name" onkeyup="searchCompanies(this.value)"></asp:TextBox>
                    <div id="companyList" style="position:absolute; z-index:1000; background:#fff; border:1px solid #ddd; max-height:200px; overflow-y:auto; width:100%; display:none;"></div>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_txt_company" runat="server"
                        ControlToValidate="txt_company" ErrorMessage="Company is a required field." ForeColor="Red" />
                </div>

                <div class="col-md-4">
                    <label>Email</label>
                    <asp:TextBox ID="txt_email" runat="server" CssClass="form-control" placeholder="Enter Email"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                        ControlToValidate="txt_email" ErrorMessage="Enter a Valid Email"
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red" />
                </div>

                <div class="col-md-4">
                    <label>Mobile</label>
                    <asp:TextBox ID="txt_mobile" runat="server" CssClass="form-control" placeholder="Enter Mobile"></asp:TextBox>
                </div>
            </div>

            <!-- Row 2 -->
            <div class="row mb-3">
                <div class="col-md-4">
                    <label>Source</label>
                    <asp:TextBox ID="txt_source" runat="server" CssClass="form-control" placeholder="Lead Source"></asp:TextBox>
                </div>

                <div class="col-md-4">
                    <label>Lead Type <span class="text-danger">*</span></label>
                    <asp:DropDownList ID="ddl_leadtype" runat="server" CssClass="form-control"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator_ddl_leadtype" runat="server"
                        ControlToValidate="ddl_leadtype" ErrorMessage="Please Select Lead type." ForeColor="Red" />
                </div>
                <div class="col-md-4">
                    <label>Description</label>
                    <asp:TextBox ID="txt_description" TextMode="MultiLine" Rows="1"
                        runat="server" CssClass="form-control" placeholder="Enter Description"></asp:TextBox>
                </div>
            </div>
            <br />
            <!-- Contacts Table -->
            <div class="row mb-3">
                <div class="col-md-12">
                    <button type="button" class="btn btn-success btn-sm mb-2" onclick="addRow()">
                        + Add Row
                    </button>

                    <div class="table-responsive">
                        <table class="table table-bordered">
                            <thead>
                                <tr>
                                    <th>Name</th>
                                    <th>Position</th>
                                    <th>Contact No</th>
                                    <th>Email</th>
                                    <th>Description</th>
                                    <th>Status</th>
                                    <th>Action</th>
                                </tr>
                            </thead>
                            <tbody id="tableBody">
                                <asp:Literal ID="ltContacts" runat="server"></asp:Literal>
                            </tbody>
                        </table>
                    </div>

                    <select id="statusTemplate" style="display:none">
                        <%= GetSalesStatusOptions("") %>
                    </select>
                </div>
            </div>
           
            <!-- Footer Buttons -->
       <div class="row" style="padding-top:25px;">
    <div class="col-lg-12 text-right">
        <a href="Myleads.aspx" class="btn btn-primary me-2">Back</a>
        <asp:Button ID="btn_request" runat="server" Text="Create"
            CssClass="btn btn-primary" OnClick="btn_request_Click"
            OnClientClick="isDirty = false;" Visible="false" />
        <asp:Button ID="btn_update" runat="server" Text="Update"
            CssClass="btn btn-primary" OnClick="btn_update_Click"
            OnClientClick="isDirty = false;" Visible="false" />
    </div>
</div>


        </div>
    </div>
</div>


<script type="text/javascript">

    let isDirty = false;

    function searchCompanies(query) {
        var companyList = document.getElementById('companyList');
        
        if (query.trim() === '') {
            companyList.style.display = 'none';
            companyList.innerHTML = '';
            return;
        }

        var xhr = new XMLHttpRequest();
        xhr.open('POST', 'Myleadsdetails.aspx/GetCompanyNames', true);
        xhr.setRequestHeader('Content-Type', 'application/json');
        
        xhr.onload = function() {
            if (xhr.status === 200) {
                var response = JSON.parse(xhr.responseText);
                var companies = JSON.parse(response.d);
                
                if (companies.length > 0) {
                    var html = '';
                    for (var i = 0; i < companies.length; i++) {
                        html += '<label style="display:block; padding:8px 10px; cursor:pointer; margin:0; border-bottom:1px solid #eee; font-weight:normal;" onmouseover="this.style.background=\'#f5f5f5\'" onmouseout="this.style.background=\'#fff\'" onclick="selectCompany(\'' + companies[i].replace(/'/g, "\\'") + '\')">• ' + companies[i] + '</label>';
                    }
                    companyList.innerHTML = html;
                    companyList.style.display = 'block';
                } else {
                    companyList.style.display = 'none';
                }
            }
        };
        
        xhr.send(JSON.stringify({ query: query }));
    }

    function selectCompany(name) {
        document.getElementById('<%= txt_company.ClientID %>').value = name;
        document.getElementById('companyList').style.display = 'none';
    }
    function addRow() {

        var tbody = document.getElementById('tableBody');

        var template = document.getElementById('statusTemplate');

        if (!template) {
            alert("Status template missing!");
            return;
        }

        var salesStatusOptions = template.innerHTML;

        var tr = document.createElement('tr');

        tr.innerHTML =
            '<td><input type="text" class="form-control" name="contact_name" /></td>'
            + '<td><input type="text" class="form-control" name="contact_position" /></td>'
            + '<td><input type="text" class="form-control" name="contact_no" /></td>'
            + '<td><input type="email" class="form-control" name="contact_email" /></td>'
            + '<td><input type="text" class="form-control" name="contact_desc" /></td>'
            + '<td><select class="form-control" name="contact_status">' + salesStatusOptions + '</select></td>'
            + '<td class="text-center"><button type="button" class="btn btn-danger" onclick="removeRow(this)">Remove</button></td>';

        tbody.appendChild(tr);
    }

    function removeRow(btn) {

        if (!confirm("Are you sure you want to delete this contact?")) return;

        isDirty = true;

        var row = btn.closest('tr');
        var contactIdInput = row.querySelector("input[name='contact_id']");

        if (contactIdInput && contactIdInput.value !== "") {
            var hidden = document.createElement("input");
            hidden.type = "hidden";
            hidden.name = "deleted_contact_id";
            hidden.value = contactIdInput.value;
            document.forms[0].appendChild(hidden);
        }

        row.remove();
    }

    window.onbeforeunload = function () {
        if (isDirty) {
            return "You have unsaved changes. Click Update to save.";
        }
    };


</script>





</asp:Content>


