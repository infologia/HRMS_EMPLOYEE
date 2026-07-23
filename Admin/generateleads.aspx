<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="generateleads.aspx.cs" Inherits="Admin_generateleads" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>
    <style>
        .panel-heading .row {
            width: 100%;
            display: flex;
            align-items: center;
        }

        .panel-title {
            margin: 0;
        }

        .date-filter {
            display: flex;
            align-items: center;
            justify-content: flex-end;
            gap: 8px;
            width: 100%;
        }

        .date-label,
        .year-label {
            margin: 0;
            font-size: 13px;
            white-space: nowrap;
        }

        .Date-dropdown,
        .year-dropdown {
            width: 120px !important;
            display: inline-block;
        }

        @media (max-width: 767px) {
            .panel-heading .row {
                flex-direction: column;
                align-items: flex-start;
            }

            .date-filter {
                flex-wrap: wrap;
                justify-content: flex-start;
                margin-top: 10px;
            }
            .date-filter label {
                width: 100%;
                text-align: left;
                margin: 4px 0;
            }
            .date-filter select,
            .date-filter a {
                margin-bottom: 6px;
            }
        }
    </style>
    <script type="text/javascript">
        var deleteLeadKey = null;

        function fn_DeleteLead(leadKey) {
            if (!leadKey) {
                toastr.error("Invalid Lead Key");
                return;
            }
            deleteLeadKey = leadKey;
            $('#confirmDeleteModal').modal('show');
        }

        function confirmDeleteLead() {
            if (!deleteLeadKey) {
                toastr.error("Lead Key missing. Please try again.");
                return;
            }
            $('#confirmDeleteModal').modal('hide');
            // Assuming there's a WebMethod to delete
            $.ajax({
                type: "POST",
                url: "generateleads.aspx/DeleteLead",
                data: JSON.stringify({ str_leadkey: deleteLeadKey }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response && response.d === "1") {
                        toastr.success("Lead has been removed successfully!");
                        setTimeout(function () { location.reload(); }, 1500);
                    } else {
                        toastr.warning("Sorry, unable to remove this Lead. Please try again.");
                    }
                    deleteLeadKey = null;
                },
                error: function (xhr, status, error) {
                    console.error(error);
                    toastr.error("An error occurred while removing the Lead.");
                    deleteLeadKey = null;
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
                    <h5 class="panel-title">Generated Leads Details</h5>
                </div>
                <div class="col-lg-8">
                    <div class="date-filter">
                        <a href="generatelead.aspx" id="a_createlead" runat="server" class="btn btn-primary">
                            <i class="icon-plus-circle2"></i> Create Lead
                        </a>
                    </div>
                </div>
            </div>
        </div>
        
        <div class="panel-body" style="padding: 0px;">
        </div>
        
        <div class="table-responsive">
            <table class="table datatable-basic" id="tblLeads">
                <thead>
                    <tr>
                        <th style="white-space: nowrap;">Company Name</th>
                        <th style="white-space: nowrap;">Industry</th>
                        <th style="white-space: nowrap;">Phone</th>
                        <th style="white-space: nowrap;">Email</th>
                        <th style="white-space: nowrap;">Status</th>
                        <th style="white-space: nowrap;">Priority</th>
                        <th style="min-width: 100px; text-align: center;">Actions</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:PlaceHolder ID="PH_Leads" runat="server"></asp:PlaceHolder>
                </tbody>
            </table>
        </div>
    </div>

    <!-- Delete Confirmation Modal -->
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
                    <p class="mb-0">Are you sure you want to remove this lead?</p>
                </div>
                <div class="modal-footer justify-content-center">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">No</button>
                    <button type="button" class="btn btn-danger" onclick="confirmDeleteLead()">Yes, Remove</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
