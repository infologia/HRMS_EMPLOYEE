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
/* ===== Petty Cash — modern layout (visual only, no functional changes) ===== */

#tablecash th, #tablecash td {
    font-size: 12.5px;
    vertical-align: middle;
}

.pc-panel .panel-heading {
    background: #fff;
    border-bottom: 1px solid #eef0f2;
    padding: 16px 20px;
}

.pc-title {
    font-size: 17px;
    font-weight: 600;
    color: #2d3339;
    margin: 0;
}
.pc-subtitle {
    font-size: 12px;
    color: #8b95a1;
    margin: 2px 0 0;
}

/* Filter bar */
.pc-filters {
    display: flex;
    gap: 8px;
    align-items: center;
    flex-wrap: wrap;
    justify-content: flex-end;
    margin-top: 12px;
}
.pc-header-actions {
    display: flex;
    justify-content: flex-end;
    align-items: flex-start;
}
.pc-filters .form-control {
    height: 34px;
    font-size: 12.5px;
    border-radius: 6px;
    border: 1px solid #dde1e6;
    padding: 0 10px;
    width: auto;
    background-color: #fbfbfc;
}
.pc-filters .form-control:focus {
    border-color: #7367f0;
    box-shadow: 0 0 0 3px rgba(115,103,240,.12);
    background-color: #fff;
}

.btn-apply-filter {
    height: 34px;
    padding: 0 16px;
    font-size: 12.5px;
    font-weight: 500;
    border-radius: 6px;
    border: 1px solid #7367f0;
    background: #7367f0;
    color: #fff;
    display: inline-flex;
    align-items: center;
    gap: 6px;
    white-space: nowrap;
    transition: background .15s ease, box-shadow .15s ease;
}
.btn-apply-filter:hover {
    background: #6355e8;
    color: #fff;
}
.btn-apply-filter:focus {
    box-shadow: 0 0 0 3px rgba(115,103,240,.25);
}

.btn-create-cash {
    height: 34px;
    padding: 0 16px;
    font-size: 12.5px;
    font-weight: 500;
    border-radius: 6px;
    white-space: nowrap;
    display: inline-flex;
    align-items: center;
    gap: 6px;
}

@media (max-width: 768px) {
    .pc-header-actions { justify-content: flex-start; margin-top: 12px; }
}

/* Summary cards */
.pc-summary-row {
    display: flex;
    gap: 14px;
    flex-wrap: wrap;
    padding: 18px 20px 4px;
}
.pc-summary-card {
    flex: 1 1 180px;
    border-radius: 10px;
    padding: 14px 16px;
    display: flex;
    align-items: center;
    gap: 12px;
    border: 1px solid #eef0f2;
}
.pc-summary-card .pc-icon {
    width: 38px;
    height: 38px;
    border-radius: 8px;
    display: flex;
    align-items: center;
    justify-content: center;
    font-size: 17px;
    flex: none;
}
.pc-summary-card .pc-label {
    font-size: 11.5px;
    color: #8b95a1;
    margin: 0 0 2px;
    text-transform: uppercase;
    letter-spacing: .3px;
}
.pc-summary-card .pc-value {
    font-size: 18px;
    font-weight: 600;
    margin: 0;
}

.pc-summary-card.cr { background: #f0faf4; }
.pc-summary-card.cr .pc-icon { background: #d7f3e3; color: #1eae6a; }
.pc-summary-card.cr .pc-value { color: #1eae6a; }

.pc-summary-card.dt { background: #fdf2f2; }
.pc-summary-card.dt .pc-icon { background: #fadcdc; color: #e2504a; }
.pc-summary-card.dt .pc-value { color: #e2504a; }

.pc-summary-card.bal { background: #f0f2fe; }
.pc-summary-card.bal .pc-icon { background: #dfe2fb; color: #5c6ceb; }
.pc-summary-card.bal .pc-value { color: #5c6ceb; }

/* Table polish */
#tablecash {
    border-collapse: separate;
    border-spacing: 0;
}
#tablecash thead th {
    background: #f7f8fa;
    color: #6b7280;
    font-weight: 600;
    text-transform: uppercase;
    font-size: 11px;
    letter-spacing: .3px;
    border-bottom: 1px solid #eef0f2 !important;
}
#tablecash tbody tr:hover {
    background-color: #fafbff;
}
#tablecash .label {
    border-radius: 20px;
    padding: 4px 10px;
    font-weight: 600;
    font-size: 11px;
}
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <div class="panel panel-flat pc-panel">
<div class="panel-heading">
    <div class="row align-items-center">

        <!-- LEFT : Title -->
        <div class="col-lg-6 col-md-6 col-sm-12">
            <h5 class="pc-title">Petty Cash</h5>
            <p class="pc-subtitle">Track cash entries, balances and daily flow</p>
        </div>

        <!-- RIGHT : Create Cash -->
        <div class="col-lg-6 col-md-6 col-sm-12">
            <div class="pc-header-actions">
                <a href="CreatePettyCash.aspx"
                   id="id_pettycash"
                   runat="server"
                   visible="true"
                   class="btn btn-primary btn-create-cash">
                    <i class="icon-plus-circle2"></i> Create Cash
                </a>
            </div>
        </div>

        <!-- FULL WIDTH : Filters, left aligned, same line -->
        <div class="col-lg-12 col-md-12 col-sm-12">
            <div class="pc-filters">

                <asp:DropDownList ID="ddlFinancialYear" runat="server"
                    CssClass="form-control"
                    style="width:150px">
                </asp:DropDownList>

                <asp:DropDownList ID="ddlDate" runat="server"
                    CssClass="form-control"
                    style="width:130px">
                </asp:DropDownList>

                <asp:DropDownList ID="ddlYear" runat="server"
                    CssClass="form-control"
                    style="width:110px">
                </asp:DropDownList>

                <asp:DropDownList ID="ddlType" runat="server"
                    CssClass="form-control"
                    style="width:110px">
                    <asp:ListItem Text="All Type" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Credit (CR)" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Debit (DT)" Value="2"></asp:ListItem>
                </asp:DropDownList>

                <asp:Button ID="btnApplyFilter" runat="server"
                    Text="Apply Filter"
                    CssClass="btn-apply-filter" 
                    OnClick="btnApplyFilter_Click"
                    UseSubmitBehavior="false" />

            </div>
        </div>

    </div>
</div>

    <div class="pc-summary-row">
        <div class="pc-summary-card cr">
            <div class="pc-icon"><i class="icon-arrow-down8"></i></div>
            <div>
                <p class="pc-label">CR Amount</p>
                <p class="pc-value"><asp:Label ID="lblCR" runat="server"></asp:Label></p>
            </div>
        </div>
        <div class="pc-summary-card dt">
            <div class="pc-icon"><i class="icon-arrow-up8"></i></div>
            <div>
                <p class="pc-label">DT Amount</p>
                <p class="pc-value"><asp:Label ID="lblDT" runat="server"></asp:Label></p>
            </div>
        </div>
        <div class="pc-summary-card bal">
            <div class="pc-icon"><i class="icon-wallet"></i></div>
            <div>
                <p class="pc-label">Balance</p>
                <p class="pc-value"><asp:Label ID="lblBalance" runat="server"></asp:Label></p>
            </div>
        </div>
    </div>

     <div class="panel-body" style="padding: 10px 20px 0;">
     </div>
     <div style="padding: 0 20px 20px;">
     <table id="tablecash" class="table datatable-basic">
         <thead>
             <tr>
                 <th>Description</th>
                 <th>Amount</th>
                 <th style="white-space: nowrap;">Balance Amount</th>
                 <th>Type</th>
                 <th style="white-space: nowrap;">Entry Date</th>
                 <th>Created By</th>                   
                 <th>Actions</th>
             </tr>
         </thead>
         <tbody>
             <asp:PlaceHolder ID="PH_PettyCash" runat="server"></asp:PlaceHolder>
         </tbody>
     </table>
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
