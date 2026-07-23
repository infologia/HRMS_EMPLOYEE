<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Projectgrid.aspx.cs" Inherits="Employee_Projectgrid" %>

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
    }
</style>    <style>
        @media (max-width: 767px) {
            .panel-heading {
                flex-direction: column;
                align-items: flex-start;
            }
            .date-filter {
                flex-wrap: wrap;
                justify-content: flex-end;
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

        var deleteProjectKey = null;

        function fn_DeleteProject(projectKey) {
            if (!projectKey) {
                toastr.error("Invalid Project Key");
                return;
            }
            deleteProjectKey = projectKey;
            $('#confirmDeleteModal').modal('show');
        }

        function confirmDeleteProject() {
            if (!deleteProjectKey) {
                toastr.error("Project Key missing. Please try again.");
                return;
            }
            $('#confirmDeleteModal').modal('hide');
            $.ajax({
                type: "POST",
                url: "Projectgrid.aspx/DeleteProject",
                data: JSON.stringify({ str_projectkey: deleteProjectKey }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response && response.d === "1") {
                        toastr.success("Project has been removed successfully!");
                        setTimeout(function () { location.reload(); }, 1500);
                    } else {
                        toastr.warning("Sorry, unable to remove this Project. Please try again.");
                    }
                    deleteProjectKey = null;
                },
                error: function (xhr, status, error) {
                    console.error(error);
                    toastr.error("An error occurred while removing the Project.");
                    deleteProjectKey = null;
                }
            });
        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <%-- Active/In Progress Projects Grid --%>
    <div class="panel panel-flat">
<div class="panel-heading">
    <div class="row">
        <div class="col-lg-4">
            <h5 class="panel-title">Planned & In Progress Projects</h5>
        </div>

        <div class="col-lg-8">
            <div class="date-filter">

                <label for="ddlDate" class="date-label">
                    Select Month:
                </label>

                <asp:DropDownList ID="ddlDate" runat="server"
                    CssClass="form-control Date-dropdown"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlDate_SelectedIndexChanged">
                </asp:DropDownList>

                <label for="ddlYear" class="year-label">
                    Select Year:
                </label>

                <asp:DropDownList ID="ddlYear" runat="server"
                    CssClass="form-control year-dropdown"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
                </asp:DropDownList>

                <a href="Project.aspx"
                    id="Create_Project"
                    runat="server"
                    class="btn btn-primary">
                    <i class="icon-plus-circle2"></i>
                    Create Project
                </a>

            </div>
        </div>
    </div>
</div>    
        <div class="panel-body" style="padding: 0px;"></div>
        <table class="table datatable-basic" id="tblActive">
            <thead>
                <tr>
                    <th style="white-space: nowrap;">Project Code</th>
                    <th>Project Name</th>
                    <th>Client Name</th>
                    <th style="white-space: nowrap;">Start Date</th>
                    <th style="white-space: nowrap;">End Date</th>
                    <th>Status</th>
                    <th style="min-width: 100px; text-align: center;">Action</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_ActiveProjects" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
    </div>

    <%-- Completed Projects Grid --%>
    <div class="panel panel-flat" style="margin-top: 20px;">
        <div class="panel-heading">
            <h5 class="panel-title">Completed Projects</h5>
        </div>
        <div class="panel-body" style="padding: 0px;"></div>
        <table class="table datatable-basic" id="tblCompleted">
            <thead>
                <tr>
                    <th style="white-space: nowrap;">Project Code</th>
                    <th>Project Name</th>
                    <th>Client Name</th>
                    <th style="white-space: nowrap;">Start Date</th>
                    <th style="white-space: nowrap;">End Date</th>
                    <th>Status</th>
                    <th style="min-width: 100px; text-align: center;">Action</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_CompletedProjects" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
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
                    <p class="mb-0">Are you sure you want to remove this project?</p>
                </div>
                <div class="modal-footer justify-content-center">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">No</button>
                    <button type="button" class="btn btn-danger" onclick="confirmDeleteProject()">Yes, Remove</button>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
