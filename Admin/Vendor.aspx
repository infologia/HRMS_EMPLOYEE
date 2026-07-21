<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Vendor.aspx.cs" Inherits="Admin_Vendor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>

    <script type="text/javascript">
        function fn_DeleteProject(VendorKey) {
            if (!confirm("Are you sure you want to remove this Vendor?")) return;

            $.ajax({
                type: "POST",
                url: "Vendor.aspx/DeleteProject",
                data: JSON.stringify({ str_vendorkey: VendorKey }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d == "1") {
                        alert("Vendor has been removed successfully.");
                        location.reload(); // Refresh the page to reflect deletion
                    } else {
                        alert("Sorry, unable to remove this Vendor. Please try again.");
                    }
                },
                error: function (xhr, status, error) {
                    alert("An error occurred while removing the Vendor. Please try again.");
                }
            });
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="panel panel-flat">
        <div class="panel-heading">
            <div class="row">
                <div class="col-lg-4">
                    <h5 class="panel-title">Vendors details</h5>
                </div>
                <div class="col-lg-8">
                    <a href="Vendorsdetails.aspx"  runat="server" class="btn btn-primary  pull-right"><i class="icon-plus-circle2"></i> Create Vendor</a>
                </div>
            </div>
        </div>
        <div class="panel-body" style="padding: 0px;">
        </div>
        <table class="table datatable-basic">
            <thead>
                <tr>
                    <th>Vendor Code</th>
                    <th>Vendor Name</th>
                    <th>Contact Person</th>
                    <th>Status</th>
                    <th>Created On</th>                   
                    <th>Update</th>
                    <th>Delete</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_Vendor" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
    </div>
</asp:Content>

