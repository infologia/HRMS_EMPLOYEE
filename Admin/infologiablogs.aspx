<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="infologiablogs.aspx.cs" Inherits="Admin_infologiablogs" validateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>

    <style>
        /* Status column: Pending button / Approved badge share the same fixed size */
        .blog-status-btn {
            display: inline-block;
            width: 90px;
            padding: 5px 0;
            font-size: 12px;
            font-weight: 600;
            text-align: center;
            border-radius: 4px;
            border: none;
            line-height: 1.4;
        }

            .blog-status-btn.is-pending {
                background: #f0ad4e;
                color: #fff;
                cursor: pointer;
            }

                .blog-status-btn.is-pending:hover {
                    background: #ec971f;
                    color: #fff;
                    text-decoration: none;
                }

            .blog-status-btn.is-approved {
                background: #5cb85c;
                color: #fff;
            }

        /* View icon: compact square button */
        .blog-view-btn {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 26px;
            height: 26px;
            border-radius: 4px;
            background: #5bc0de;
            color: #fff;
            font-size: 11px;
            margin-right: 4px;
        }

            .blog-view-btn:hover {
                background: #31b0d5;
                color: #fff;
                text-decoration: none;
            }

        /* Update icon: compact square primary button */
        .blog-update-btn {
            display: inline-flex;
            align-items: center;
            justify-content: center;
            width: 26px;
            height: 26px;
            border-radius: 4px;
            background: #337ab7;
            color: #fff;
            font-size: 11px;
        }

            .blog-update-btn:hover {
                background: #286090;
                color: #fff;
                text-decoration: none;
            }

            .blog-update-btn.is-disabled {
                background: #b8c2cc;
                color: #fff;
                cursor: not-allowed;
                pointer-events: none;
            }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="panel panel-flat">
        <div class="panel-heading">
            <div class="row">
                <div class="col-lg-4">
                    <h5 class="panel-title">Blog Management</h5>
                </div>
                <div class="col-lg-8">
                    <a href="infologiablog.aspx" class="btn btn-primary pull-right"><i class="icon-plus-circle2"></i> Add New Blog</a>
                </div>
            </div>
        </div>
        <div class="panel-body" style="padding: 0px;"></div>
        <table class="table datatable-basic">
            <thead>
                <tr>
                    <th>Title</th>
                    <th>Creater Name</th>
                    <th>Status</th>
                    <th>Is Published</th>
                    <th class="text-center">View</th>
                    <th class="text-center">Update</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_BlogList" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
    </div>

</asp:Content>
