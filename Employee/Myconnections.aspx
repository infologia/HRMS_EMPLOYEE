<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Myconnections.aspx.cs" Inherits="Employee_Myconnections" %>

<%-- Add content controls here --%>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .invoice-table { width:100%; border-collapse:collapse; margin-top:8px; }
        .invoice-table thead tr { background-color:#f0f4f8; }
        .invoice-table > thead > tr > th { padding:10px 12px; font-size:13px; font-weight:600; border:1px solid #dde3ea; color:#444; }
        .invoice-table > tbody > tr > td { padding:7px 10px; border:1px solid #dde3ea; vertical-align:middle; }
        .invoice-table .form-control { margin-bottom:0; height:34px; }
        .invoice-table > tbody > tr:hover { background-color:#fafbfc; }
        .btn-add-row { background:#3a7bd5; color:#fff; border:none; border-radius:4px; padding:7px 16px; font-size:13px; font-weight:500; cursor:pointer; display:inline-flex; align-items:center; gap:5px; }
        .btn-add-row:hover { background:#2a5fb5; }
        .btn-remove-inv { background:#e53935; color:#fff; border:none; border-radius:4px; padding:5px 12px; font-size:12px; cursor:pointer; }
        .btn-remove-inv:hover { background:#b71c1c; }
        .col-date { width:220px; }
        .col-action { width:80px; text-align:center; }
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
                <legend class="text-semibold"><i class="icon-reading position-left"></i>Create Connection</legend>
                <div class="row">
                    <div class="col-md-4">
                        <label>Name<span style="color: red"> *</span></label>
                        <asp:TextBox ID="txt_name" runat="server" Class="form-control" placeholder="Enter Name"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator_name" runat="server" ControlToValidate="txt_name" ErrorMessage="Name is a required field." ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-4" style="position:relative;">
                        <label>Company<span style="color: red"> *</span></label>
                        <asp:TextBox ID="txt_company" runat="server" CssClass="form-control" placeholder="Enter Company Name" onkeyup="searchCompanies(this.value)"></asp:TextBox>
                        <div id="companyList" style="position:absolute; z-index:1000; background:#fff; border:1px solid #ddd; max-height:200px; overflow-y:auto; width:100%; display:none;"></div>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator_txt_company" runat="server" ControlToValidate="txt_company" ErrorMessage="Company is a required field." ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                    <div class="col-md-4">
                        <label>Position</label>
                        <asp:TextBox ID="txt_position" runat="server" CssClass="form-control" placeholder="Enter Position"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <label>Email</label>
                        <asp:TextBox ID="txt_email" runat="server" CssClass="form-control" placeholder="Enter Email"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txt_email" ErrorMessage="Enter a Valid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red"></asp:RegularExpressionValidator>
                    </div>
                    <div class="col-md-4">
                        <label>Mobile</label>
                        <asp:TextBox ID="txt_mobile" runat="server" CssClass="form-control" placeholder="Enter Mobile"></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Style="color: red"
                            ControlToValidate="txt_mobile" ErrorMessage="Enter a Valid Phone number"
                            ValidationExpression="[0-9]{10}"></asp:RegularExpressionValidator>
                    </div>
                    <div class="col-md-4">
                        <label>Source</label>
                        <asp:TextBox ID="txt_source" runat="server" CssClass="form-control" placeholder="Lead Source"></asp:TextBox>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-4">
                        <label>Lead Type <span style="color: red">*</span></label>
                        <asp:DropDownList ID="ddl_leadtype" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator_ddl_leadtype" ControlToValidate="ddl_leadtype" runat="server" ErrorMessage="Please Select Lead type." ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>

                    <div class="col-md-4">
                        <label>Description</label>
                        <asp:TextBox ID="txt_description" TextMode="MultiLine" Rows="3" runat="server" CssClass="form-control" placeholder="Enter Description"></asp:TextBox>
                    </div>
                </div>
                <br />
                <legend class="text-semibold"><i class="icon-file-text2 position-left"></i>Follow Up Details</legend>

                <table class="invoice-table" id="followupTable">
                    <thead>
                        <tr>
                            <th>Description</th>
                            <th class="col-date">Date</th>
                            <th class="col-action">Action</th>
                        </tr>
                    </thead>
                    <tbody id="followupBody" runat="server">
                        <tr>
                            <td><textarea class="form-control" name="fuDesc[]" placeholder="Enter Description" rows="2"></textarea></td>
                            <td>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                    <input type="text" class="form-control pickadate" name="fuDate[]" placeholder="DD/MM/YYYY" readonly="readonly" />
                                </div>
                            </td>
                            <td style="text-align:center;"><button type="button" class="btn-remove-inv removeFuRow">Remove</button></td>
                        </tr>
                    </tbody>
                </table>

                <div style="margin-top:10px;">
                    <button type="button" class="btn-add-row" onclick="addFollowupRow()">
                        <i class="icon-plus2"></i> Add Row
                    </button>
                </div>

                <div class="row" style="padding-top: 25px">
                    <div class="col-lg-12 text-right">
                        <a href="Myconnectionsdetails.aspx" class="btn btn-primary me-2">Back</a>
                        <asp:Button ID="btn_request" runat="server" Text="Create"
                            CssClass="btn btn-primary" OnClick="btn_request_Click" Visible="false" />
                        <asp:Button ID="btn_update" runat="server" Text="Update"
                            CssClass="btn btn-primary" OnClick="btn_update_Click" Visible="false" />
                    </div>
                </div>

            </div>
        </div>
    </div>


    <script>
        function addFollowupRow() {
            var row = '<tr>' +
                '<td><textarea class="form-control" name="fuDesc[]" placeholder="Enter Description" rows="2"></textarea></td>' +
                '<td><div class="input-group"><span class="input-group-addon"><i class="icon-calendar22"></i></span><input type="text" class="form-control pickadate" name="fuDate[]" placeholder="DD/MM/YYYY" readonly="readonly" /></div></td>' +
                '<td style="text-align:center;"><button type="button" class="btn-remove-inv removeFuRow">Remove</button></td>' +
                '</tr>';
            $('#<%= followupBody.ClientID %>').append(row);
            setTimeout(function () {
                $('#<%= followupBody.ClientID %> tr:last .pickadate').pickadate('stop').pickadate({
                    format: 'dd/mm/yyyy',
                    selectMonths: true,
                    selectYears: true,
                    closeOnSelect: true
                });
            }, 50);
        }

        $(document).on('click', '.removeFuRow', function () {
            if ($('#<%= followupBody.ClientID %> tr').length > 1)
                $(this).closest('tr').remove();
            else
                alert('At least one row is required.');
        });

        $(document).ready(function () {
            setTimeout(function () {
                $('.pickadate').pickadate({
                    format: 'dd/mm/yyyy',
                    selectMonths: true,
                    selectYears: true,
                    closeOnSelect: true
                });
            }, 50);
        });

        function searchCompanies(query) {
            var companyList = document.getElementById('companyList');
            
            if (query.trim() === '') {
                companyList.style.display = 'none';
                companyList.innerHTML = '';
                return;
            }

            var xhr = new XMLHttpRequest();
            xhr.open('POST', 'Myconnections.aspx/GetCompanyNames', true);
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
    </script>

</asp:Content>
