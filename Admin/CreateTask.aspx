<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="CreateTask.aspx.cs" Inherits="TicketingTool_CreateTask" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/pickers/anytime.min.js"></script>
    <!-- Theme JS files -->
    <script type="text/javascript" src="../Template/assets/js/plugins/notifications/jgrowl.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/ui/moment/moment.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/pickers/daterangepicker.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/pickers/pickadate/picker.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/pickers/pickadate/picker.date.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/pickers/pickadate/picker.time.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/pickers/pickadate/legacy.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/picker_date.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-3"></div>
        <div class="col-md-6">
            <div action="#">
                <div class="panel panel-flat">
                    <div class="panel-heading">
                        <div class="row">
                        </div>
                    </div>
                    <div class="panel-body">
                        <fieldset class="step" id="Fieldset1">

                            <h6 class="form-wizard-title text-semibold">
                                <span class="form-wizard-count" style="margin-top: -7px;"><i class="icon-pencil5"></i></span>Create Task</h6>
                            <div class="row">
                                <div class="col-md-9 col-md-offset-1">

                                    <div class="text-center">
                                    </div>
                                    <div class="form-group">
                                        <label>Project Name:</label>
                                        <asp:DropDownList ID="ddl_pjname" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl_pjname_SelectedIndexChanged" required=""></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="ddl_pjname" runat="server" InitialValue="0" ErrorMessage="Please select Project" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>

                                    <div class="form-group">
                                        <label>Issuetype :</label>
                                        <asp:DropDownList ID="ddl_isstype" runat="server" class="form-control " required=""></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddl_isstype" runat="server" InitialValue="0" ErrorMessage="Please select Issuetype" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>

                                    <div class="form-group">
                                        <label>Task Name:</label>
                                        <asp:TextBox ID="txt_tasname" runat="server" class="form-control " required=""></asp:TextBox>
                                    </div>

                                    <div class="form-group">
                                        <label>Due Date:</label>
                                        <div class="input-group">
                                            <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                            <asp:TextBox ID="txt_cldate" runat="server" TextMode="Date" class="form-control daterange-single" placeholder="MM/DD/YYYY"></asp:TextBox>
                                        </div>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="txt_cldate" runat="server" ErrorMessage="Please Enter duedate" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>

                                    <div class="form-group">
                                        <label>Description:</label>
                                        <textarea id="txt_des" rows="5" cols="5" runat="server" class="form-control " required=""></textarea>
                                    </div>
                                    <div class="form-group">
                                        <label>Priority :</label>
                                        <asp:DropDownList ID="ddl_prty" runat="server" class="form-control " required=""></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="ddl_prty" runat="server" InitialValue="0" ErrorMessage="Please select Priority" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group" id="div_assign" runat="server" visible="false">
                                        <label>Assignee :</label>
                                        <asp:DropDownList ID="ddl_assign" runat="server" class="form-control" required=""></asp:DropDownList>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="ddl_assign" runat="server" InitialValue="0" ErrorMessage="Please select Assignee" ForeColor="Red"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="form-group">
                                        <label>Attachments:</label>
                                        <asp:FileUpload ID="up_file" runat="server" class="file-input" multiple="multiple" data-show-upload="false" data-show-caption="true" data-show-preview="true" />
                                    </div>
                                    <div class="form-group" id="div_status" runat="server" visible="false">
                                        <label>Status</label>
                                        <asp:DropDownList ID="ddl_status" runat="server" class="form-control" required=""></asp:DropDownList>
                                    </div>                                    
                                    <div class="form-group">
                                        <label>Work time:</label>
                                        <asp:TextBox ID="txt_time" runat="server" class="form-control " TextMode="Time" required=""></asp:TextBox>
                                    </div>
                                    <div class="form-group">
                                        <div class="text-center">
                                            <a href="../TicketingTool/Viewtask.aspx" class="btn btn-primary" style="margin-right: 15px">Back</a>
                                            <asp:Button ID="btn_Create" runat="server" OnClick="btn_create_Click" class="btn btn-primary"></asp:Button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

