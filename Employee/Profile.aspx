<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Profile.aspx.cs" Inherits="WEB_Employee_Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
<!-- Core JS files -->
<script type="text/javascript" src="../Template/assets/js/plugins/loaders/pace.min.js"></script>
<script type="text/javascript" src="../Template/assets/js/core/libraries/jquery.min.js"></script>
<script type="text/javascript" src="../Template/assets/js/core/libraries/bootstrap.min.js"></script>
<script type="text/javascript" src="../Template/assets/js/plugins/loaders/blockui.min.js"></script>
<!-- /core JS files -->

<!-- Theme JS files -->
<script type="text/javascript" src="../Template/assets/js/plugins/notifications/jgrowl.min.js"></script>
<script type="text/javascript" src="../Template/assets/js/plugins/ui/moment/moment.min.js"></script>
<script type="text/javascript" src="../Template/assets/js/plugins/pickers/daterangepicker.js"></script>
<script type="text/javascript" src="../Template/assets/js/plugins/pickers/anytime.min.js"></script>
<script type="text/javascript" src="../Template/assets/js/plugins/pickers/pickadate/picker.js"></script>
<script type="text/javascript" src="../Template/assets/js/plugins/pickers/pickadate/picker.date.js"></script>
<script type="text/javascript" src="../Template/assets/js/plugins/pickers/pickadate/picker.time.js"></script>
<script type="text/javascript" src="../Template/assets/js/plugins/pickers/pickadate/legacy.js"></script>

<script type="text/javascript" src="../Template/assets/js/core/app.js"></script>
<script type="text/javascript" src="../Template/assets/js/pages/picker_date.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <asp:Panel ID="pn_Control" runat="server">
            <div class="col-lg-9">
                <div class="panel panel-flat">
                    <div class="panel-heading">
                        <h6 class="panel-title"><b>Profile information</b></h6>
                    </div>
                    <div class="panel-body">
                        <div id="div_error" runat="server" class="" visible="false">
                            <asp:Label ID="lbl_error" runat="server"></asp:Label>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-6">
                                    <label>Employee Id <span style="color: red">*</span></label>
                                    <asp:TextBox ID="txt_empid" runat="server" class="form-control" required="" ReadOnly="true"></asp:TextBox>

                                </div>
                                <div class="col-md-6">
                                    <label>Username<span style="color: red">*</span></label>
                                    <asp:TextBox ID="txt_username" runat="server" class="form-control" required="" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-6">
                                    <label>First Name<span style="color: red">*</span></label>
                                    <asp:TextBox ID="txt_fname" runat="server" class="form-control" required="" ReadOnly="true"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="regName" runat="server" Style="color: red" ControlToValidate="txt_fname" ValidationExpression="^[a-zA-Z'.\s]{1,50}" Text="Enter a valid name" />
                                </div>
                                <div class="col-md-6">
                                    <label>Last Name<span style="color: red">*</span></label>
                                    <asp:TextBox ID="txt_lname" runat="server" class="form-control" required="" ReadOnly="true"> </asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Style="color: red" ControlToValidate="txt_lname" ValidationExpression="^[a-zA-Z'.\s]{1,50}" Text="Enter a valid name" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-6">
                                    <label>Gender</label><br />
                                    <asp:RadioButtonList ID="rd_gander" runat="server" RepeatDirection="Horizontal" required="required" ReadOnly="true">
                                        <asp:ListItem Text="&nbspMale&nbsp&nbsp&nbsp" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="&nbspFemale&nbsp" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div class="col-md-6">
                                    <label>DOB<span style="color: red">*</span></label>
                                    <div class="input-group">
                                        <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                    <asp:TextBox class="form-control pickadate" runat="server" ID="txt_dob" ReadOnly="true"></asp:TextBox>
                                        </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-6">
                                    <label>Address </label>
                                    <asp:TextBox ID="txt_address" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>

                                </div>
                                <div class="col-md-6">
                                    <label>Email</label>
                                    <asp:TextBox ID="txt_email" runat="server" class="form-control" ReadOnly="true"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txt_email" ErrorMessage="Invalid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-6">
                                    <label>Password </label>
                                    <asp:TextBox ID="txt_pwd" runat="server" TextMode="Password" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="valPassword" runat="server" ControlToValidate="txt_pwd" ErrorMessage="Minimum password length is 6" Style="color: red" ValidationExpression="^([a-zA-Z0-9@#$%^&+=*]{6,30})$" />
                                </div>

                                <div class="col-md-6">
                                    <label>Phonenumber</label>
                                    <asp:TextBox ID="txt_phone" runat="server" required="" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Style="color: red" ControlToValidate="txt_phone" ErrorMessage="Enter a Valid Phonenumber" ValidationExpression="[0-9]{10}"></asp:RegularExpressionValidator>

                                </div>
                            </div>
                        </div>


                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-6">
                                    <label>City</label>
                                    <asp:TextBox ID="txt_city" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="Rev" runat="server" Style="color: red" ControlToValidate="txt_city" ValidationExpression="^[a-zA-Z'.\s]{1,50}" Text="Enter a valid name" />
                                </div>
                                <div class="col-md-6">
                                    <label>State</label>
                                    <asp:DropDownList ID="ddl_state" runat="server" CssClass="form-control" required="" ReadOnly="true"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-6">
                                    <label>Zip Code</label>
                                    <asp:TextBox ID="txt_zipcode" runat="server" class="form-control" MaxLength="8" ReadOnly="true"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="regZip" runat="server" ControlToValidate="txt_zipcode" Display="None" ErrorMessage="Zip accepts only numerics" ValidationExpression="^[0-9\s.\.]+$" Enabled="false"></asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-6">
                                    <label>Qualification<span style="color: red">*</span></label>
                                    <asp:TextBox ID="txt_qualification" runat="server" class="form-control" MaxLength="12" required="" ReadOnly="true"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server" Style="color: red" ControlToValidate="txt_qualification" ValidationExpression="^[a-zA-Z'.\s]{1,50}" Text="Enter only Text" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-6">
                                    <label>Department<span style="color: red">*</span></label>
                                    <asp:DropDownList ID="ddl_dest" runat="server" required="" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-md-6">
                                    <label>Desingation<span style="color: red">*</span></label>
                                    <asp:DropDownList ID="ddl_depart" runat="server" CssClass="form-control" required=""></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-6">
                                    <label>Division<span style="color: red">*</span></label>
                                    <asp:DropDownList ID="ddl_division" runat="server" CssClass="form-control" required=""></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
      <div class="text-center ">
 
          <asp:Button ID="btn_Resgister" runat="server" CssClass="btn btn-primary" Visible="false" Text="Register" OnClick="btn_Resgister_Click" />
          <asp:Button ID="Btn_Update" runat="server" CssClass="btn btn-primary" Visible="false" Style="margin-right: 40px;" Text="Update" OnClick="Btn_Update_Click" />
      </div>
  </div>

                    </div>
                </div>
            </div>
            <div class="col-lg-3">
                <div class="thumbnail">
                    <div class="thumb thumb-rounded thumb-slide">
                        <asp:Image ID="Img_Profile" runat="server" />
                    </div>
                    <div class="caption text-center">
                        <h6 class="text-semibold no-margin">
                            <asp:Label ID="label_UserName" runat="server"></asp:Label>
                            <small class="display-block">
                                <asp:Label ID="label_UserDesignationShow" runat="server"></asp:Label>
                            </small></h6>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>

    <script>
    $('.pickadate').pickadate({ format: 'dd/mm/yyyy' });
    </script>

</asp:Content>

