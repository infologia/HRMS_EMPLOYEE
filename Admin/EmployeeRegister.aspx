<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="EmployeeRegister.aspx.cs" Inherits="WEB_EmployeeRegister" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form-horizontal">
        <div class="panel panel-flat">
            <div class="panel-heading">
                <h5 class="panel-title"></h5>
            </div>

            <div class="panel-body">
                <fieldset>
                    <legend class="text-semibold"><i class="icon-reading position-left"></i> Employee Registration</legend>
                    <div class="row">
                        <div class="col-md-4">
                            <label>Employee Id<span style="color: red"> *</span></label>
                            <asp:TextBox ID="Txt_Employeeid" runat="server" class="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Txt_Employeeid" ErrorMessage="Employee Id is a required field." ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-4">
                            <label>User Name <span style="color: red">*</span> </label>
                            <asp:TextBox ID="txt_username" runat="server" class="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_username" ErrorMessage="User Name is required field." ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>


                        <div class="col-md-4" style="margin-bottom:10px;">
                            <label>First Name <span style="color: red">*</span></label>
                            <asp:TextBox ID="txt_fname" runat="server" class="form-control"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator_name" runat="server" ControlToValidate="txt_fname" ErrorMessage="Enter First Name." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="regName" runat="server" Style="color: red" ControlToValidate="txt_fname" ValidationExpression="^[a-zA-Z'.\s]{1,50}" Text="Enter a valid name." Display="Dynamic" />
                        </div>
                    </div>
                    <div class="row" >
                        <div class="col-md-4" style="margin-bottom:8px;" >
                            <label>Last Name <span style="color: red">*</span></label>
                            <asp:TextBox ID="txt_lname" runat="server" CssClass="form-control" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_lname" ErrorMessage="Enter Last Name." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Style="color: red" ControlToValidate="txt_lname" ValidationExpression="^[a-zA-Z'.\s]{1,50}" Text="Enter a valid name." Display="Dynamic" />
                        </div>

                        <div class="col-md-4" style="margin-bottom:8px;">
                            <label>DOB<span style="color: red"> *</span></label>
                            <div class="input-group">
                                <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                <asp:TextBox ID="txt_dob" runat="server" class="form-control pickadate" placeholder="DD/MM/YYYY"></asp:TextBox>
                            </div>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txt_dob" ErrorMessage="DOB is a required Field" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-4" >
                            <label>Gender<span style="color: red"> *</span></label>
                            <asp:RadioButtonList ID="rd_gander" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="&nbspMale&nbsp&nbsp&nbsp" Value="0"></asp:ListItem>
                                <asp:ListItem Text="&nbspFemale&nbsp" Value="1"></asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="rd_gander" ErrorMessage="Gender is  required ." ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-4" style="margin-bottom:8px;">
                            <label>Email<span style="color: red"> *</span></label>
                            <asp:TextBox ID="txt_email" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txt_email" ErrorMessage="Invalid Email" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" ForeColor="Red" Display="Dynamic"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txt_email" ErrorMessage="Enter the Email." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-4" style="margin-bottom:8px;">
                            <label>Password<span style="color: red"> *</span></label>
                            <asp:TextBox ID="txt_pwd" runat="server" TextMode="password" CssClass="form-control"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="valPassword" runat="server" ControlToValidate="txt_pwd"
                                ErrorMessage="Minimum password length is 6" Style="color: red" ValidationExpression="^([a-zA-Z0-9@#$%^&+=*]{6,30})$" Display="Dynamic" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txt_pwd" ErrorMessage="Enter the Password." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-4 " style="margin-bottom:8px;">
                            <label>Phone Number<span style="color: red"> *</span></label>
                            <asp:TextBox ID="txt_phone" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Style="color: red"
                                ControlToValidate="txt_phone" ErrorMessage="Enter a Valid Phone Number"
                                ValidationExpression="[0-9]{10}" Display="Dynamic"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txt_phone" ErrorMessage="Enter Phone Number." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                    <div class="row" >
                        <div class="col-md-4" style="margin-top:12px;">
                            <label>Address</label>
                            <asp:TextBox ID="txt_address" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                        <div class="col-md-4" style="margin-top:12px;">
                            <label>Zip Code<span style="color: red"> *</span></label>

                            <asp:TextBox ID="txt_zipcode" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="regZip" runat="server" ControlToValidate="txt_zipcode" Display="None" ErrorMessage="Zip accepts only numerics" ValidationExpression="^[0-9\s.\.]+$"
                                Enabled="false"></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txt_zipcode" ErrorMessage="Enter Zip code." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>

                        </div>
                        <div class="col-md-4" style="margin-top:12px;">
                            <label >City <span style="color: red">*</span></label>
                            <asp:TextBox ID="txt_city" runat="server" CssClass="form-control"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" Style="color: red" ControlToValidate="txt_city" ValidationExpression="^[a-zA-Z'.\s]{1,50}" Text="Enter a valid city name" Display="Dynamic" />
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txt_city" ErrorMessage="Enter the City." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>

                        </div>

                    </div>
                   
                    <div class="row">
                   <div class="col-md-4" style="margin-top:15px;">
                    <label >Qualification<span style="color:red">*</span></label>
                        <asp:TextBox ID="txt_qualification" runat="server" CssClass="form-control" ></asp:TextBox>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator12" runat="server" Style="color: red" ControlToValidate="txt_qualification" ValidationExpression="^[a-zA-Z'.\s]{1,50}" Text="Enter only Text" Display="Dynamic" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txt_qualification" ErrorMessage="Enter the Qualification." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                    </div>             
                    <div class="col-md-4" style="margin-top:15px;">
                          <label>State</label>
                          <asp:DropDownList ID="ddl_state" runat="server" CssClass="dropdown-menu" Style="display: block; position: static; width: 100%; margin-top: 0; float: none" ></asp:DropDownList>
                    </div>

                    <div class="col-md-4" style="margin-top:15px;">
                    <label>Profile Upload <span style="color:red">*</span></label>                  
                        <asp:FileUpload ID="up_img" runat="server" CssClass="file-input" />
                        <asp:CustomValidator ID="CustomValidator1" ClientValidationFunction="ValidateFile" runat="server" ControlToValidate="up_img" Display="dynamic" Style="color: red" ErrorMessage="images only ">
                        </asp:CustomValidator>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="up_img" ErrorMessage="Enter the Image." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>

                    </div>
              
                  </div>
                  <div class="row">
                    <div class="col-md-4" style="margin-top:15px;">
                        <label >Department <span style="color:red">*</span></label>                       
                            <asp:DropDownList ID="ddl_depart" runat="server" CssClass="dropdown-menu" Style="display: block; position: static; width: 100%; margin-top: 0; float: none" ></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="ddl_depart" ErrorMessage="Enter the Department." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>

                        </div>             
                    <div class="col-md-4" style="margin-top:15px;">
                        <label >Desingation<span style="color:red">*</span></label>                     
                            <asp:DropDownList ID="ddl_dest" runat="server" CssClass="dropdown-menu" Style="display: block; position: static; width: 100%; margin-top: 0; float: none;"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="ddl_dest" ErrorMessage="Enter the Desingation." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>

                    </div>
                  <div class="col-md-4" style="margin-top:15px;">
                    <label>Division<span style="color:red">*</span></label>
                    <asp:DropDownList ID="ddl_division" runat="server" CssClass="dropdown-menu" Style="display: block; position: static; width: 100%; margin-top: 0; float: none" ></asp:DropDownList>
                       <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="ddl_division" ErrorMessage="Enter the Division." ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                   </div>
               
              </div>

       
                <div class="form-group" style="margin-top:15px;">
                    <div class="text-right">
                        <a href="EmployeeView.aspx" class="btn btn-primary">Back</a>
                        <asp:Button ID="btn_register" runat="server" Text="Register" class="btn btn-primary" OnClick="btn_register_Click" Style="margin-right: 15px"></asp:Button>

                    </div>
                </div>
         </fieldset>
       </div>
      </div>
    </div>
    
    

    <script lang="javascript">
        function ValidateFile(source, args) {
            try {
                var fileAndPath =
                    document.getElementById(source.controltovalidate).value;
                var lastPathDelimiter = fileAndPath.lastIndexOf("\\");
                var fileNameOnly = fileAndPath.substring(lastPathDelimiter + 1);
                var file_extDelimiter = fileNameOnly.lastIndexOf(".");
                var file_ext = fileNameOnly.substring(file_extDelimiter + 1).toLowerCase();
                if (file_ext != "jpg") {
                    args.IsValid = false;
                    if (file_ext != "gif")
                        args.IsValid = false;
                    if (file_ext != "png") {
                        args.IsValid = false;
                        return;
                    }
                }
            } catch (err) {
                txt = "There was an error on this page.\n\n";
                txt += "Error description: " + err.description + "\n\n";
                txt += "Click OK to continue.\n\n";
                txt += document.getElementById(source.controltovalidate).value;
                alert(txt);
            }

            args.IsValid = true;
        }
    </script>

    <script>
        $('.pickadate').pickadate({
            format: 'dd/mm/yyyy',
            selectMonths: true,
            selectYears: true,
            closeOnSelect: true
        });
    </script>

</asp:Content>

