<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Myleads.aspx.cs" Inherits="Employee_Myleads" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>
   
    <script type="text/javascript">
        function fn_DeleteProject(leadKey) {           
            if (!confirm("Are you sure you want to remove this lead?")) return;

            $.ajax({
                type: "POST",
                url: "Myleads.aspx/DeleteProject",
                data: JSON.stringify({ str_leadkey: leadKey }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == "1") {
                        alert("Lead has been removed successfully.");
                        location.reload(); // Refresh the page to reflect deletion
                    } else {
                        alert("Sorry, unable to remove this lead. Please try again.");
                    }
                },
                error: function (xhr, status, error) {
                    alert("An error occurred while removing the lead. Please try again.");
                }
            });
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel panel-flat">
       <div class="panel-heading">
          <div class="row">
            <div class="col-md-3">
                <label>Status </label>
                <asp:DropDownList ID="ddl_status" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
            <div class="col-md-3">
                <label>Lead Type </label>
                <asp:DropDownList ID="ddl_leadtype" runat="server" CssClass="form-control">
                </asp:DropDownList>
             </div>
            <div class="col-md-2">
                <label>Year </label>
                <asp:DropDownList ID="ddl_year" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
            <div class="col-md-2">
                <label>Month</label>
                <asp:DropDownList ID="ddl_Month" runat="server" CssClass="form-control">
                </asp:DropDownList>
            </div>
            <div class="col-md-2" style="margin-top: 28px;">
                <asp:Button ID="btn_request" runat="server" Text="Search" CssClass="btn btn-primary  pull-right" OnClick="btn_request_Click"></asp:Button>
            </div>
        </div>
    </div>
</div>
    <div class="panel panel-flat">
        <div class="panel-heading">
            <div class="row">
                <div class="col-lg-4">
                    <h5 class="panel-title">Sales leads details</h5>
                </div>
                <div class="col-lg-8">
                    <a href="Myleadsdetails.aspx" id="a_createlead"  runat="server" visible="false" class="btn btn-primary  pull-right"><i class="icon-plus-circle2"></i> Create Lead</a>
                </div>
            </div>
        </div>
        <div class="panel-body" style="padding: 0px;">
        </div>
        <table class="table datatable-basic">
            <thead>
                <tr>
                    <th>Company</th>
                    <th>Lead Type</th>
                    <th>Created</th>
                    <th>Modified</th>
                    <th>Update</th>
                    <th>Delete</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_leave" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
    </div>
</asp:Content>

