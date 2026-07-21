<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="EmployeeSalaryResgistraion.aspx.cs" Inherits="WEB_Admin_EmployeeSalaryResgistraion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
<script>
    $(document).ready(function () {

        function calculateTotals() {

            var basicsalary = parseFloat($("#ContentPlaceHolder1_txt_basicsalary").val()) || 0;
            var hra = parseFloat($("#ContentPlaceHolder1_txt_hra").val()) || 0;
            var mediall = parseFloat($("#ContentPlaceHolder1_txt_mediall").val()) || 0;
            var conveyance = parseFloat($("#ContentPlaceHolder1_txt_Conveyance").val()) || 0;
            var allowance = parseFloat($("#ContentPlaceHolder1_txt_allowance").val()) || 0;

            var totalEarnings = basicsalary + hra + mediall + conveyance + allowance;
            $("#ContentPlaceHolder1_txt_earnings").val(totalEarnings.toFixed(2));

            var pfamount = parseFloat($("#ContentPlaceHolder1_txt_pfamount").val()) || 0;
            var esiamount = parseFloat($("#ContentPlaceHolder1_txt_esiamount").val()) || 0;

            var totalDeduction = pfamount + esiamount;
            $("#ContentPlaceHolder1_txt_deduction").val(totalDeduction.toFixed(2));

            var netPay = totalEarnings - totalDeduction;
            $("#ContentPlaceHolder1_txt_netpay").val(netPay.toFixed(2));
        }

        // Use input instead of keyup (better)
        $("#ContentPlaceHolder1_txt_basicsalary, \
           #ContentPlaceHolder1_txt_hra, \
           #ContentPlaceHolder1_txt_mediall, \
           #ContentPlaceHolder1_txt_Conveyance, \
           #ContentPlaceHolder1_txt_allowance, \
           #ContentPlaceHolder1_txt_pfamount, \
           #ContentPlaceHolder1_txt_esiamount")
            .on("input", calculateTotals);

    });
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="panel panel-white">
        <div class="panel-heading">
            <h6 class="panel-title">Employee salary Deatils</h6>

        </div>

        <div class="row">
            <div class="col-md-12 ">

                <div class="panel-body">
                    <div class="row">
                        <div class="form-group col-md-4">
                            <label for="username" class="control-label">Select EmployeeID:</label>
                            <span class="required" style="color: red">*</span>
                            <asp:DropDownList ID="ddl_Empid" AutoPostBack="true" OnSelectedIndexChanged="ddl_Empid_SelectedIndexChanged" runat="server" class="form-control"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfv_Empid" runat="server" ControlToValidate="ddl_Empid" InitialValue="0" ErrorMessage="Employee ID is required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>

                        <div class="form-group col-md-4">
                            <label for="username" class="control-label">Employee name:</label>
                            <span class="required" style="color: red">*</span>
                            <asp:TextBox ID="txt_Empname" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfv_Empname" runat="server" ControlToValidate="txt_Empname" ErrorMessage="Employee name is required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>

                        <div class="form-group col-md-4">
                            <label for="first_name" class="control-label">Employee Department:</label>
                            <span class="required" style="color: red">*</span>
                            <asp:DropDownList ID="ddl_Empdep" runat="server" class="form-control" disabled="disabled"></asp:DropDownList>
                        </div>
                    </div>

                    <div class="row">
                        <div class="form-group col-md-4">
                            <label for="last_name" class="control-label">Employee Division:</label>
                            <span class="required" style="color: red">*</span>
                            <asp:DropDownList ID="ddl_Empdiv" runat="server" class="form-control" disabled="disabled"></asp:DropDownList>
                        </div>

                        <div class="form-group col-md-4">
                            <label for="Gender" class="control-label">Employee Designation:</label>
                            <span class="required" style="color: red">*</span>
                            <asp:DropDownList ID="ddl_Empdeg" runat="server" class="form-control" disabled="disabled"></asp:DropDownList>
                        </div>

                        <div class="form-group col-md-4">
                            <label for="Email" class="control-label">PF Number:</label>
                            <asp:TextBox ID="txt_pfnumber" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>

                    <div class="row">
                        <div class="form-group col-md-4">
                            <label for="password" class="control-label">ESI Number:</label>
                            <asp:TextBox ID="txt_Esinumber" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>

                        <div class="form-group col-md-4">
                            <label for="phone" class="control-label">PAN Number:</label>
                            <span class="required" style="color: red">*</span>
                            <asp:TextBox ID="txt_Pannumber" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfv_Pannumber" runat="server" ControlToValidate="txt_Pannumber" ErrorMessage="PAN Number is required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>

                        <div class="form-group col-md-4">
                            <label>Employee Date Of joining: </label>
                            <span class="required" style="color: red">*</span>
                            <div class="input-group">
                                <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                <asp:TextBox ID="txt_doj" runat="server" class="form-control pickadate" placeholder="DD/MM/YYYY"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="rfv_doj" runat="server" ControlToValidate="txt_doj" ErrorMessage="Date of Joining is required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>

                    <div class="row">
                        <div class="form-group col-md-4">
                            <label for="State" class="control-label">Monthly Salary:</label>
                            <span class="required" style="color: red">*</span>
                            <asp:TextBox ID="txt_Monthlysalary" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfv_Monthlysalary" runat="server" ControlToValidate="txt_Monthlysalary" ErrorMessage="Monthly Salary is required" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>

                        <div class="form-group col-md-4">
                            <label class="control-label">Basic Salary:</label>
                            <div class="row">
                                <div class="form-group col-sm-3">
                                    <asp:TextBox ID="txt_basicint" runat="server" CssClass="form-control" placeholder="%" ></asp:TextBox>
                                </div>
                                <div class="form-group col-sm-9">
                                    <asp:TextBox ID="txt_basicsalary" runat="server" CssClass="form-control" placeholder="Amount"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="form-group col-md-4">
                            <label for="Image" class="control-label">HRA:</label>
                            <div class="row">
                                <div class="form-group col-sm-3">
                                    <asp:TextBox ID="txt_hraint" runat="server" CssClass="form-control" placeholder="%" ></asp:TextBox>
                                </div>
                                <div class="form-group col-sm-9">
                                    <asp:TextBox ID="txt_hra" runat="server" CssClass="form-control" placeholder="Amount"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="form-group col-md-4">
                            <label for="Zip Code" class="control-label">Medical Allowance:</label>
                            <div class="row">
                                <div class="col-sm-3">
                                    <asp:TextBox ID="txt_mdint" runat="server" CssClass="form-control" placeholder="%" ></asp:TextBox>
                                </div>
                                <div class="col-sm-9">
                                    <asp:TextBox ID="txt_mediall" runat="server" CssClass="form-control" placeholder="Amount"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group col-md-4">
                            <label class="control-label">Conveyance:</label>
                            <div class="row">
                                <div class="form-group col-sm-3">
                                    <asp:TextBox ID="txt_conveyint" runat="server" CssClass="form-control" placeholder="%" ></asp:TextBox>
                                </div>
                                <div class="form-group col-sm-9">
                                    <asp:TextBox ID="txt_Conveyance" runat="server" CssClass="form-control" placeholder="Amount"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="form-group col-md-4">
                            <label for="Division" class="control-label">Special Allowance:</label>
                            <div class="row">
                                <div class="form-group col-sm-3">
                                    <asp:TextBox ID="txt_splint" runat="server" CssClass="form-control" placeholder="%"></asp:TextBox>
                                </div>
                                <div class="form-group col-sm-9">
                                    <asp:TextBox ID="txt_allowance" runat="server" CssClass="form-control" placeholder="Amount"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                    </div>

                    <div class="row">
                        <div class="form-group col-md-4">
                            <label for="Department" class="control-label">PF Amount:</label>
                            <div class="row">
                                <div class="form-group col-sm-3">
                                    <asp:TextBox ID="txtpfint" runat="server" CssClass="form-control" placeholder="%"></asp:TextBox>
                                </div>
                                <div class="form-group col-sm-9">
                                    <asp:TextBox ID="txt_pfamount" runat="server" CssClass="form-control" placeholder="Amount"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="form-group col-md-4">
                            <label for="Qualification" class="control-label">ESI Amount:</label>
                            <div class="row">
                                <div class="form-group col-sm-3">
                                    <asp:TextBox ID="txt_esiint" runat="server" CssClass="form-control" placeholder="%" ></asp:TextBox>
                                </div>
                                <div class="form-group col-sm-9">
                                    <asp:TextBox ID="txt_esiamount" runat="server" CssClass="form-control" placeholder="Amount"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="form-group col-md-4">
                            <label class="control-label">Total Earnings:</label>
                            <asp:TextBox ID="txt_earnings" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>

                    </div>

                    <div class="row">

                        <div class="form-group col-md-4">
                            <label class="control-label">Total Deduction:</label>
                            <asp:TextBox ID="txt_deduction" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>

                        <div class="form-group col-md-4">
                            <label class="control-label">Net Pay:</label>
                            <asp:TextBox ID="txt_netpay" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>

                        <div class="form-group col-md-1">
                            <label>&nbsp;</label>
                            <button type="button" id="btn_calculate_client" class="btn btn-primary form-control" onclick="document.getElementById('<%=btn_calculate.ClientID%>').click();"><i class="icon-calculator"></i></button>
                            <asp:Button ID="btn_calculate" runat="server" Text="" CssClass="btn btn-primary form-control" OnClick="btn_calculate_Click" CausesValidation="false" style="display:none;" />
                        </div>
                    </div>
                    <div class="text-right">
                        <a href="SalaryDetails.aspx" class="btn btn-primary stepy-finish position-right"><i class="icon-arrow-left13"></i> Back</a>
                        <asp:Button ID="btn_register" runat="server" Text="Register" CssClass="btn btn-primary stepy-finish position-right" OnClick="btn_submit_Click" Visible="false" style="margin-right: 15px" />
                        <asp:Button ID="btn_update" runat="server" Text="Update" CssClass="btn btn-primary stepy-finish position-right" OnClick="btn_update_Click" Visible="false" style="margin-right: 15px" />
                        <div id="txt" runat="server"></div>
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
    </script>

</asp:Content>

