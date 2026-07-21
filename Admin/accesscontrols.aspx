<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="accesscontrols.aspx.cs" Inherits="Admin_accesscontrols" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel panel-flat">
        <div class="panel-heading">
            <h5 class="panel-title">Access Controls</h5>
            <div class="heading-elements">
                <button type="button" id="createBtn" class="btn btn-primary" data-toggle="modal" data-target="#createModal">
                   <i class="icon-user-plus"></i> Create
                </button>
            </div>
        </div>

        <div class="panel-body" style="padding: 0px;">
        </div>

        <table class="table datatable-basic">
            <thead>
                <tr>
                    <th>ID</th>
                    <th>Status</th>
                    <th>Created On</th>
                    <th>Action</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_AccessControl" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
    </div>

    <!-- Create Modal -->
    <div class="modal fade" id="createModal" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title"><i class="icon-pencil"></i> Create Access Control</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label class="text-semibold">IP Type</label>
                        <div class="radio">
                            <label>
                                <input type="radio" name="createIpType" value="0" checked>
                                Office IP
                            </label>
                        </div>
                        <div class="radio">
                            <label>
                                <input type="radio" name="createIpType" value="1">
                                Own IP
                            </label>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="text-semibold">Office IP Address</label>
                        <asp:TextBox ID="txt_createOfficeIP" runat="server" CssClass="form-control" placeholder="Enter Office IP"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    <asp:Button ID="btn_create" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btn_create_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- Edit Modal -->
    <div class="modal fade" id="editModal" tabindex="-1" role="dialog">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title"><i class="icon-pencil"></i> Edit Access Control</h4>
                </div>
                <div class="modal-body">
                    <asp:HiddenField ID="hf_editId" runat="server" />
                    <asp:HiddenField ID="hf_editStatus" runat="server" />
                    <div class="form-group">
                        <label class="text-semibold">IP Type</label>
                        <div class="radio">
                            <label>
                                <input type="radio" name="editIpType" id="editOfficeIp" value="0">
                                Office IP
                            </label>
                        </div>
                        <div class="radio">
                            <label>
                                <input type="radio" name="editIpType" id="editOwnIp" value="1">
                                Own IP
                            </label>
                        </div>
                    </div>
                    <div class="form-group">
                        <label class="text-semibold">Office IP Address</label>
                        <asp:TextBox ID="txt_editOfficeIP" runat="server" CssClass="form-control" placeholder="Enter Office IP"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                    <asp:Button ID="btn_update" runat="server" Text="Update" CssClass="btn btn-success" OnClick="btn_update_Click" />
                </div>
            </div>
        </div>
    </div>

    <script>
        function editAccessControl(id, status) {
            console.log('Edit clicked - ID:', id, 'Status:', status);
            
            document.getElementById('<%= hf_editId.ClientID %>').value = id;
            document.getElementById('<%= hf_editStatus.ClientID %>').value = status;
            
            if (status == '0') {
                document.getElementById('editOfficeIp').checked = true;
                document.getElementById('editOwnIp').checked = false;
            } else {
                document.getElementById('editOfficeIp').checked = false;
                document.getElementById('editOwnIp').checked = true;
            }
            
            // Load Office IP from database
            $.ajax({
                type: "POST",
                url: "accesscontrols.aspx/GetOfficeIP",
                data: JSON.stringify({ id: id }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    document.getElementById('<%= txt_editOfficeIP.ClientID %>').value = response.d;
                },
                error: function () {
                    document.getElementById('<%= txt_editOfficeIP.ClientID %>').value = '';
                }
            });
            
            $('#editModal').modal('show');
            return false;
        }
    </script>
</asp:Content>

