<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="employeeofthemonth.aspx.cs" Inherits="Admin_employeeofthemonth" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript">
        function updateRecord(key, year, month, empKey) {
            document.getElementById('<%= hfRecordKey.ClientID %>').value = key;
            document.getElementById('<%= ddlYear.ClientID %>').value = year;
            document.getElementById('<%= ddlMonth.ClientID %>').value = month;
            document.getElementById('<%= ddlEmployee.ClientID %>').value = empKey;
            document.getElementById('<%= btnSave.ClientID %>').style.display = 'none';
            document.getElementById('<%= btnUpdate.ClientID %>').style.display = 'inline-block';
            window.scrollTo(0, 0);
        }

        function deleteRecord(key) {
            if (confirm('Are you sure you want to delete this record?')) {
                window.location.href = 'employeeofthemonth.aspx?action=delete&key=' + key;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="panel panel-flat">
        <div class="panel-heading">
            <h5 class="panel-title">Employee of the Month</h5>
        </div>

        <div class="panel-body">
            <div class="row">
                <asp:HiddenField ID="hfRecordKey" runat="server" />
                <div class="col-md-4">
                    <label>Year <span style="color: red">*</span></label>
                    <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvYear" runat="server" ControlToValidate="ddlYear" 
                        InitialValue="0" ErrorMessage="Select Year" ForeColor="Red" Display="Dynamic" />
                </div>
                <div class="col-md-4">
                    <label>Month <span style="color: red">*</span></label>
                    <asp:DropDownList ID="ddlMonth" runat="server" CssClass="form-control"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvMonth" runat="server" ControlToValidate="ddlMonth" 
                        InitialValue="0" ErrorMessage="Select Month" ForeColor="Red" Display="Dynamic" />
                </div>
                <div class="col-md-4">
                    <label>Employee <span style="color: red">*</span></label>
                    <asp:DropDownList ID="ddlEmployee" runat="server" CssClass="form-control"></asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvEmployee" runat="server" ControlToValidate="ddlEmployee" 
                        InitialValue="0" ErrorMessage="Select Employee" ForeColor="Red" Display="Dynamic" />
                </div>
            </div>
            <br />
            <div class="row">
                <div class="col-md-12">
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Visible="false"></asp:Label>
                    <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                </div>
            </div>
            <div class="row" style="margin-top: 20px;">
                <div class="col-md-12 text-right">
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
                    <asp:Button ID="btnUpdate" runat="server" Text="Update" CssClass="btn btn-primary" OnClick="btnUpdate_Click" Style="display:none;" />
                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-default" OnClick="btnClear_Click" CausesValidation="false" />
                </div>
            </div>
        </div>
    </div>

    <div class="panel panel-flat">
        <div class="panel-heading">
            <h5 class="panel-title">Employee of the Month List</h5>
        </div>
        <div class="panel-body">
            <div class="table-responsive">
                <table class="table datatable-basic">
                    <thead>
                        <tr>
                            <th>Employee Name</th>
                            <th>Year</th>
                            <th>Month</th>
                            <th>Action</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:PlaceHolder ID="PH_EmployeeList" runat="server"></asp:PlaceHolder>
                    </tbody>
                </table>
            </div>
        </div>
    </div>
</asp:Content>

