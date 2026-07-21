<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="ParentMenu.aspx.cs" Inherits="Admin_ParentMenu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

      <script type="text/javascript">
          function checkempid() {
              var Menuno = document.getElementById("ContentPlaceHolder1_txt_menuno").value;

              if (Menuno != "") {
                  $(function () {
                      $.ajax({
                          type: 'POST',
                          url: 'ParentMenu.aspx/Checkemployeeid',
                          data: "{'str_menuno':'" + Menuno + "'}",
                          contentType: "application/json; charset=utf-8",
                          dataType: "json",
                          success: function (r) {
                              var obj = r.d;
                              if (obj != "") {
                                  alert(obj);
                                  $('#ContentPlaceHolder1_txt_menuno').val("");
                              }
                          }
                      });
                  });
              }
          }

    </script>
    <style>
    .rbSpace label {
           margin-right: 25px; 
    }
    .rbSpace input[type=radio] {
           margin-right: 6px;
    }
    .rbSpace td {
          vertical-align: middle;
    }
</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h6 class="content-group text-semibold">Parent Menu					
        <small class="display-block">Sub menu under parent menu</small>
    </h6>
    <div class="row">
        <div class="col-md-4">
        </div>
        <div class="col-md-4">
            <div class="panel panel-flat">
                <div class="panel-heading">
                     <fieldset>
                            <legend class="text-semibold"><i class="icon-reading position-left"></i>Create Parent menu</legend>
                         </fieldset>
             
                    <div class="heading-elements">
                        
                    </div>
                </div>
                    <div class="panel-body">
                   <div class="form-group">
                        <label>Designation  <span style="color: red">*</span></label>
                    <asp:DropDownList ID="ddl_desg" runat="server" class="form-control "></asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddl_desg" InitialValue="0" runat="server" ErrorMessage="Please Select Designation" ForeColor="Red"></asp:RequiredFieldValidator>

                         </div>
            
                    <div class="form-group">
                        <label>Parent Menuname  <span style="color: red">*</span></label>
                        <asp:TextBox ID="txt_menuname" runat="server" CssClass="form-control" ></asp:TextBox>
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txt_menuname"  runat="server" ErrorMessage="Enter Parent Menuname" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>

                    <div class="form-group">
                        <label>Status  <span style="color: red">*</span></label>
                       <asp:RadioButtonList ID="rblStatus" RepeatDirection="Horizontal" runat="server" CssClass="rbSpace ">
                           <asp:ListItem Text="Active" Value="1" Selected="True"></asp:ListItem>
                           <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                       </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="rblStatus" InitialValue="" runat="server" ErrorMessage="Please Select Status" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>

                    <div class="form-group">
                        <label>Menu Description <span style="color: red">*</span></label>
                        <textarea rows="5" cols="5" runat="server" id="txt_menudesc" class="form-control" placeholder="Enter Menu Description" required=""></textarea>
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="txt_menudesc"  runat="server" ErrorMessage="Enter Description" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>

                     <div class="form-group">
                        <label>Menu List No  <span style="color: red">*</span></label>
                        <asp:TextBox ID="txt_menuno" runat="server" CssClass="form-control" onchange="checkempid();" reqired=""></asp:TextBox>
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="txt_menuno"  runat="server" ErrorMessage="Enter Menu List No" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                      <div class="form-group">
                        <label>Menu Icon</label>
                        <asp:TextBox ID="txt_icons" runat="server" CssClass="form-control" required=""></asp:TextBox>
                          <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="txt_icons"  runat="server" ErrorMessage="Enter Menu Icon" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                      <div class="form-group">
                          <a href="Menuicons.aspx" style="margin-left: 200px">Copy Icons</a>
                          </div>
                    <div class="text-right">
                         <a href="ParentMenuGrid.aspx" class="btn btn-primary "> Back</a>
                        <asp:Button ID="btn_submit" runat="server" CssClass="btn btn-primary" OnClick="btn_submit_Click" />

                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-4">
        </div>
    </div>
</asp:Content>

