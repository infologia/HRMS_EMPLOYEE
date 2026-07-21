<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="PettyCash.aspx.cs" Inherits="Admin_PettyCash" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
<script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
<script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
<script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>
<script type="text/javascript">
    var deleteCashKey = null;

    function fn_DeleteProject(cashKey) {
        deleteCashKey = cashKey;                // store key
        $('#confirmDeleteModal').modal('show'); // open modal
    }

    function confirmDeleteProject() {

        if (deleteCashKey == null) return;

        $.ajax({
            type: "POST",
            url: "PettyCash.aspx/DeleteCash",
            data: JSON.stringify({ CashKey: deleteCashKey }),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (response) {

                if (response.d === "SUCCESS") {
                    $('#confirmDeleteModal').modal('hide');
                    showToastr('success', 'Cash deleted successfully!');

                    setTimeout(function () {
                        location.reload();
                    }, 1500);
                }
                else {
                    showToastr('error', 'Delete failed!');
                }
            },
            error: function () {
                showToastr('error', 'Server error occurred!');
            }
        });
    }
</script>

    <style>
.filter-row {
    display: flex;
    align-items: center;
    justify-content: flex-end;
    gap: 12px;
}

.filter-label {
    margin-bottom: 0;
    white-space: nowrap;
}

.filter-dropdown {
    width: 140px;
}

/* Mobile support */
@media (max-width: 768px) {
    .filter-row {
        flex-wrap: wrap;
        justify-content: flex-start;
        margin-top: 10px;
    }
}


    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <div class="panel panel-flat">
<div class="panel-heading">
    <div class="row align-items-center">

        <!-- LEFT : Title -->
        <div class="col-lg-6 col-md-6 col-sm-12">
            <h5 class="panel-title mb-0">Cash Details</h5>
        </div>

        <!-- RIGHT : Month, Year, Create Button -->
        <div class="col-lg-6 col-md-6 col-sm-12">
            <div class="filter-row">

                <label class="filter-label">Select Month :</label>
                <asp:DropDownList ID="ddlDate" runat="server"
                    CssClass="form-control filter-dropdown"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlDate_SelectedIndexChanged">
                </asp:DropDownList>

                <label class="filter-label">Select Year :</label>
                <asp:DropDownList ID="ddlYear" runat="server"
                    CssClass="form-control filter-dropdown"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                </asp:DropDownList>

                <a href="CreatePettyCash.aspx"
                   id="id_pettycash"
                   runat="server"
                   visible="true"
                   class="btn btn-primary">
                    <i class="icon-plus-circle2"></i> Create Cash
                </a>

            </div>
        </div>

    </div>
</div>
     <div class="panel-body" style="padding: 0px;">
     </div>
     <table id="tablecash" class="table datatable-basic">
         <thead>
             <tr>
                 <th>Description</th>
                 <th>Amount</th>
                 <th>Balance Amount</th>
                 <th>Type</th>
                 <th>Entry Date</th>
                 <th>Created By</th>                   
                 <th>Update</th>
                 <th>Delete</th>
             </tr>
         </thead>
         <tbody>
             <asp:PlaceHolder ID="PH_PettyCash" runat="server"></asp:PlaceHolder>
         </tbody>
     </table>
         <div class="row" style="margin-top:15px;">
    <div class="col-lg-12 text-center">
        <strong>
            CR Amount :
            <asp:Label ID="lblCR" runat="server" CssClass="text-success"></asp:Label>
            &nbsp;&nbsp; | &nbsp;&nbsp;

            DT Amount :
            <asp:Label ID="lblDT" runat="server" CssClass="text-danger"></asp:Label>
            &nbsp;&nbsp; | &nbsp;&nbsp;

            Balance :
            <asp:Label ID="lblBalance" runat="server" CssClass="text-primary"></asp:Label>
        </strong>
    </div>
</div>
<br />
 </div>
        <div class="modal fade" id="confirmDeleteModal" tabindex="-1" role="dialog">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">

                <div class="modal-header bg-danger text-white">
                    <h5 class="modal-title">Confirm Delete</h5>
                    <button type="button" class="close text-white" data-dismiss="modal">
                        <span>&times;</span>
                    </button>
                </div>

                <div class="modal-body text-center">
                    <p class="mb-0">
                        Are you sure you want to remove this Details?
                    </p>
                </div>

                <div class="modal-footer justify-content-center">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">
                        No
                    </button>
                    <button type="button" class="btn btn-danger" onclick="confirmDeleteProject()">
                        Yes, Remove
                    </button>
                </div>

            </div>
        </div>
    </div>


    </asp:Content>
