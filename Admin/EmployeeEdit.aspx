<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="EmployeeEdit.aspx.cs" Inherits="WEB_EmployeeEdit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
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
                                    <asp:TextBox ID="txt_empid" runat="server" class="form-control" ReadOnly="true" required=""></asp:TextBox>

                                </div>
                                <div class="col-md-6" id="div_username" runat="server">
                                    <label>User Name<span style="color: red">*</span></label>
                                    <asp:TextBox ID="txt_username" runat="server" class="form-control" required="" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-6">
                                    <label>First Name<span style="color: red">*</span></label>
                                    <asp:TextBox ID="txt_fname" runat="server" class="form-control" required=""></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="regName" runat="server" Style="color: red" ControlToValidate="txt_fname" ValidationExpression="^[a-zA-Z'.\s]{1,50}" Text="Enter a valid name" />
                                </div>
                                <div class="col-md-6">
                                    <label>Last Name<span style="color: red">*</span></label>
                                    <asp:TextBox ID="txt_lname" runat="server" class="form-control" required=""> </asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Style="color: red" ControlToValidate="txt_lname" ValidationExpression="^[a-zA-Z'.\s]{1,50}" Text="Enter a valid name" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-6">
                                    <label>Gender</label><br />
                                    <asp:RadioButtonList ID="rd_gander" runat="server" RepeatDirection="Horizontal" required="required">
                                        <asp:ListItem Text="&nbspMale&nbsp&nbsp&nbsp" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="&nbspFemale&nbsp" Value="1"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </div>
                                <div class="col-md-6">
                                    <label>DOB<span style="color: red">*</span></label>
                                    <asp:TextBox CssClass="form-control pickadate" runat="server" ID="txt_dob" type="text"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-6">
                                    <label>Address </label>
                                    <asp:TextBox ID="txt_address" runat="server" CssClass="form-control"></asp:TextBox>

                                </div>
                                <div class="col-md-6">
                                    <label>Email</label>
                                    <asp:TextBox ID="txt_email" runat="server" class="form-control"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator6" runat="server" ControlToValidate="txt_email" ErrorMessage="Invalid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red"></asp:RegularExpressionValidator>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-6" id="div_Password" runat="server">
                                    <label>Password </label>
                                    <asp:TextBox ID="txt_pwd" runat="server" TextMode="Password" CssClass="form-control" ReadOnly="true"></asp:TextBox>

                                </div>

                                <div class="col-md-6">
                                    <label>Phonenumber</label>
                                    <asp:TextBox ID="txt_phone" runat="server" required="" CssClass="form-control"></asp:TextBox>


                                </div>
                            </div>
                        </div>


                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-6">
                                    <label>City</label>
                                    <asp:TextBox ID="txt_city" runat="server" CssClass="form-control"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="Rev" runat="server" Style="color: red" ControlToValidate="txt_city" ValidationExpression="^[a-zA-Z'.\s]{1,50}" Text="Enter a valid name" />
                                </div>
                                <div class="col-md-6">
                                    <label>State</label>
                                    <asp:DropDownList ID="ddl_state" runat="server" CssClass="form-control" required=""></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-6">
                                    <label>Zip Code</label>
                                    <asp:TextBox ID="txt_zipcode" runat="server" class="form-control" MaxLength="8"></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="regZip" runat="server" ControlToValidate="txt_zipcode" Display="None" ErrorMessage="Zip accepts only numerics" ValidationExpression="^[0-9\s.\.]+$" Enabled="false"></asp:RegularExpressionValidator>
                                </div>
                                <div class="col-md-6">
                                    <label>Qualification<span style="color: red">*</span></label>
                                    <asp:TextBox ID="txt_qualification" runat="server" class="form-control" MaxLength="12" required=""></asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server" Style="color: red" ControlToValidate="txt_qualification" ValidationExpression="^[a-zA-Z'.\s]{1,50}" Text="Enter only Text" />
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-6">
                                    <label>Role<span style="color: red">*</span></label>
                                    <asp:DropDownList ID="ddl_dest" runat="server" required="" CssClass="form-control"></asp:DropDownList>
                                </div>
                                <div class="col-md-6">
                                    <label>Department<span style="color: red">*</span></label>
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
                                <br />
                                <div class="col-md-6">
                                    <label>Status</label><br />
                                    <asp:RadioButtonList ID="Rd_Status" runat="server" RepeatDirection="Horizontal" OnSelectedIndexChanged="Rd_Status_SelectedIndexChanged" AutoPostBack="true" required="required">
                                        <asp:ListItem Text="&nbspActive&nbsp&nbsp&nbsp" Selected="True" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="&nbspInactive&nbsp" Value="0"></asp:ListItem>
                                    </asp:RadioButtonList>

                                    <div id="txt_active" runat="server" visible="false">



                                        <textarea runat="server" id="txt_areamessage" style="width: 250px; height: 120px" placeholder=" reason" required="required"></textarea><br />
                                    </div>

                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="text-right">
                                <a href="EmployeeView.aspx" class="btn btn-primary">Back</a>
                                <asp:Button ID="btn_update" runat="server" CssClass="btn btn-primary" Text="Update" OnClick="btn_update_Click" />


                            </div>
                        </div>

                    </div>
                </div>
            </div>

            <div class="col-lg-3">
                <div class="thumbnail">
                    <div class="thumb thumb-rounded thumb-slide">
                        <asp:Image ID="Img_Profile" runat="server" ImageUrl="~/images/nopicture.jpg" />
                        <div class="caption">
                            <span>
                                <asp:FileUpload ID="Fi_Updatepicture" runat="server" CssClass="btn bg-success-400 btn-icon btn-xs" data-popup="lightbox" />
                            </span>
                        </div>
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
           $('.pickadate').pickadate({
               format: 'dd/mm/yyyy',
               selectMonths: true,
               selectYears: true,
               closeOnSelect: true
           });
       </script>

</asp:Content>

