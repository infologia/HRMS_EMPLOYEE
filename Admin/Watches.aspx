<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Watches.aspx.cs" Inherits="Admin_Watches" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript" src='http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.3.min.js'></script>
    <script type="text/javascript" src='http://cdnjs.cloudflare.com/ajax/libs/twitter-bootstrap/3.0.3/js/bootstrap.min.js'></script>
    <script type="text/javascript" src="http://cdn.rawgit.com/bassjobsen/Bootstrap-3-Typeahead/master/bootstrap3-typeahead.min.js"></script>
    <link rel="Stylesheet" href="https://twitter.github.io/typeahead.js/css/examples.css" />
   

    <script type="text/javascript">
        $(function () {
            
            var request2 = document.getElementById('ContentPlaceHolder1_hfkey').value
     
            $('[id*=txt_adduser]').typeahead({
                hint: true,
                highlight: true,
                minLength: 1
            , source: function (request, response) {
                $.ajax({
                   
                    url: '<%=ResolveUrl("~/Admin/Watches.aspx/GetCustomers") %>',
                    data: "{ 'prefix': '" + request + "','prefix2': '" + request2 + "'}",
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    success: function (data) {
                        items = [];
                        map = {};
                        $.each(data.d, function (i, item) {
                         

                            var name = item.split(',')[0];
                            var id = item.split(',')[1];
                            map[name] = { name: name, id: id };
                            items.push(name);
                        });
                        response(items);
                        $(".dropdown-menu").css("height", "auto");
                    },
                    error: function (response) {

                        alert(response.responseText);
                    },
                    failure: function (response) {
                        alert(response.responseText);
                    }
                });
            },
                updater: function (item) {
                 

                    $('[id*=hfCustomerId]').val(map[item].id);
                    $('[id*=txt_empname]').val(map[item].id);
                    return item;
                }
            });
        });
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

          <div class="row">
        <div class="col-md-3"></div>
        <div class="col-md-6">
            <div class="panel panel-flat">
                <div class="panel-heading">
                </div>

                <div class="panel-body">


                    <div class="form-group">
                        <label>Project:</label>

                        <asp:textbox ID="txt_pjtname" runat="server"  required="" class="form-control">
                        </asp:textbox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txt_pjtname"  runat="server" ErrorMessage="Please select Project" ForeColor="Red"></asp:RequiredFieldValidator>

                    </div>
                    <div class="form-group">

                        <label>Task Name:</label>
                        <asp:TextBox ID="txt_taskname" runat="server" Rows="5" cols="5" class="form-control" required="required"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="txt_taskname"  runat="server" ErrorMessage="Please select Category" ForeColor="Red"></asp:RequiredFieldValidator>

                    
                    </div>
                    <div class="form-group">

                        <label>Description:</label>
                        <textarea id="txt_description" runat="server" rows="5" cols="5" class="form-control" required="required"></textarea>
                   <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="txt_description"  runat="server" ErrorMessage="Please select Description" ForeColor="Red"></asp:RequiredFieldValidator>

                         </div>


                    <div class="form-group">
                        <label>Add User</label>
                        <asp:TextBox ID="txt_adduser" runat="server"  class="form-control"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" style="color:red" ErrorMessage="Enter Username"
        ControlToValidate="txt_adduser"></asp:RequiredFieldValidator>
                           
                    </div>



                </div>

                <div class="form-group">
                    <div class="text-center">
                        <a href="ProjectIssue.aspx" class="btn btn-primary margin-left-1"><i class="icon-undo"></i> Back</a>
                        
                        <asp:LinkButton ID="btn_submit" cssclass="btn btn-primary" runat="server" OnClick="btn_submit_Click">Submit <i class="icon-arrow-right14 position-right" ></i></asp:LinkButton>
                    </div>
                </div>



            </div>

            <div class="col-md-3"></div>
        </div>
    </div>

    <asp:HiddenField ID="hfCustomerId" runat="server" />
    
    <asp:HiddenField ID="hfkey" runat="server" />


</asp:Content>

