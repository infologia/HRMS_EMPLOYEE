<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="InternOnboardingView.aspx.cs" Inherits="WEB_InternOnboardingView" %>

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
                <div class="col-md-6 pull-left">
                    <h5 class="panel-title">Interns Details View</h5>
                    <br />
                </div>
                <div class="col-md-6 pull-right">
                    <ul class="icons-list">
                      <a href="InternOnboarding.aspx" class="btn btn-primary pull-right"><i class="icon-user-plus"></i> Create Intern</a>
                    </ul>
                </div>
            </div>
        </div>

        <div class="panel-body" style="padding: 0px;">
        </div>

        <table class="table datatable-basic">
            <thead>
                <tr>
                    <th>Profile</th>
                    <th>Intern Code</th>
                    <th>Full Name</th>
                    <th>Email</th>
                    <th>Phone Number</th>
                    <th>Department</th>
                    <th>Action</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_InternView" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
    </div>
</asp:Content>
