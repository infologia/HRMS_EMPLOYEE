<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="AddUser.aspx.cs" Inherits="Admin_AddUser" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript">
        $(function () {            
            var request2 = document.getElementById('ContentPlaceHolder1_ddl_pjtname').value
            alert("Your book is overdue.");
            $('[id*=txt_adduser]').typeahead({
                hint: true,
                highlight: true,
                minLength: 1
                , source: function (request, response) {
                    $.ajax({
                        alert("Your book is overdue.");
                        url: '<%=ResolveUrl("~/Admin/AddUser.aspx/GetCustomers") %>',
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-3"></div>
        <div class="col-md-6">
            <div class="panel panel-flat">
                <div class="panel-heading">
                </div>
                <div class="panel-body">
                    <div class="form-group">
                        <label>Project:</label>
                        <asp:DropDownList ID="ddl_pjtname" runat="server" AutoPostBack="true" OnSelectedIndexChanged="txt_pjtname_SelectedIndexChanged" required="" class="form-control">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddl_pjtname" InitialValue="0" runat="server" ErrorMessage="Please select Category" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                    <div class="form-group">
                        <label>Category:</label>
                        <asp:TextBox ID="txt_category" runat="server" Rows="5" cols="5" class="form-control" required=""></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Description:</label>
                        <textarea id="txt_description" runat="server" rows="5" cols="5" class="form-control" required=""></textarea>
                    </div>
                    

                    <div class="form-group">
                        <label>Add Employee</label>
                        <asp:DropDownList ID="ddl_employeelist" runat="server" AutoPostBack="true" required="" class="form-control">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="ddl_employeelist" InitialValue="0" runat="server" ErrorMessage="Please select Employee" ForeColor="Red"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <div class="text-center">
                        <a href="Projects.aspx" class="btn btn-primary ">Back</a>
                        <asp:LinkButton ID="btn_submit" CssClass="btn btn-primary" runat="server" OnClick="btn_submit_Click">Submit </asp:LinkButton>
                    </div>
                </div>
            </div>
            <div class="col-md-3"></div>
        </div>
    </div>
    <asp:HiddenField ID="hfCustomerId" runat="server" />
    <asp:HiddenField ID="hfkey" runat="server" />
</asp:Content>

