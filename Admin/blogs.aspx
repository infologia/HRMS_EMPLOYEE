<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="blogs.aspx.cs" Inherits="Admin_blogs" validateRequest="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="panel panel-flat">
        <div class="panel-heading">
            <div class="row">
                <div class="col-lg-4">
                    <h5 class="panel-title">Blog Management</h5>
                </div>
                <div class="col-lg-8">
                    <a href="blog.aspx" class="btn btn-primary pull-right"><i class="icon-plus-circle2"></i> Add New Blog</a>
                </div>
            </div>
        </div>
        <div class="panel-body" style="padding: 0px;"></div>
        <table class="table datatable-basic">
            <thead>
                <tr>
                    <th>Blog Title</th>
                    <th>Creator Name</th>
                    <th>Description</th>
                    <th>Title</th>
                    <th class="text-center">Action</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_BlogList" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
    </div>

</asp:Content>
