<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="UpdateLeave.aspx.cs" Inherits="WEB_Employee_UpdateLeave" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
    <div class="col-md-2"></div>
    <div class="col-md-8">
        <div class="panel panel-flat">
            <div class="panel-heading">
                <h5 class="panel-title"></h5>

            </div>

            <div class="panel-body">
                <fieldset>
                    <legend class="text-semibold"><i class="icon-pencil5"></i> Leave Request</legend>
                    <div action="#">
                        <div class="row">
                            <div class="col-md-6">
                                <label class="content-group text-semibold">From Date <span style="color: red">*</span> </label>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                    <asp:TextBox ID="txt_fromdate" runat="server" CssClass="form-control pickadate"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="txt_fromdate" runat="server" ErrorMessage="Please select fromdate" ForeColor="Red"></asp:RequiredFieldValidator>

                            </div>
                            <div class="col-md-6">
                                <label class="content-group text-semibold">To Date <span style="color: red">*</span> </label>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                    <asp:TextBox ID="txt_todate" runat="server" Class="form-control  pickadate"></asp:TextBox>
                                </div>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txt_todate" runat="server" ErrorMessage="Please select todate" ForeColor="Red"></asp:RequiredFieldValidator>

                            </div>
                        </div>
                        <div class="row">
                            <div class="col-md-6">
                                <label class="content-group text-semibold">Leave Type <span style="color: red">*</span> </label>
                                <asp:DropDownList ID="ddl_leavetype" runat="server" class="form-control">
                                    <asp:ListItem Value="" Text="Select Leave Type"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="Half Day (Forenoon)"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="Half Day (Afternoon)"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Full Day"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="ddl_leavetype" InitialValue="" runat="server" ErrorMessage="Please select leavetype" ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                            <div class="col-md-6">
                                <label class="content-group text-semibold">Reason <span style="color: red">*</span> </label>
                                <textarea id="txt_reason" runat="server" rows="1" class="form-control"></textarea>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="txt_reason" runat="server" ErrorMessage="Enter a reason" ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <div class="row">
                            <div id="div_Reson" runat="server" visible="false">
                                <div class="col-md-6">
                                    <label class="content-group text-semibold">Status <span style="color: red">*</span> </label>
                                    <asp:DropDownList ID="ddl_category" runat="server" class="form-control">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddl_category" InitialValue="0" runat="server" ErrorMessage="Please select Status" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                                <div class="col-md-6">
                                    <label class="content-group text-semibold">Admin Reason <span style="color: red">*</span> </label>
                                    <textarea id="txt_reason1" runat="server" rows="1" class="form-control"></textarea>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="txt_reason1" runat="server" ErrorMessage="Enter a reason" ForeColor="Red"></asp:RequiredFieldValidator>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="form-group">
                        <div class="text-right">
                            <a href="LeaveRequestView.aspx" class="btn btn-primary " style="margin-right: 15px">Back</a>
                            <asp:Button ID="btn_request" runat="server" Text="Update" OnClick="btn_request_Click" CssClass="btn btn-primary"></asp:Button>
                        </div>
                    </div>
                    </fieldset>
            </div>
            
        </div>
         
    </div>  
    <div class="col-md-2"></div>
 </div>
    <script>
        $('.pickadate').pickadate({
            format: 'dd/mm/yyyy',        // UI format
            formatSubmit: 'dd/mm/yyyy',  // value format
            hiddenName: false
        });
    </script>

</asp:Content>

