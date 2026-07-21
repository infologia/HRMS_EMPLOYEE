<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Salesdocs.aspx.cs" Inherits="Admin_Salesdocs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="panel panel-flat">
        <div class="panel-heading">
            <div class="row">
                <div class="col-lg-4">
                    <h5 class="panel-title">Sales Document</h5>
                </div>
                <div class="col-lg-8">
                    <a href="CreateSalesdocs.aspx" id="sales_doc" runat="server" class="btn btn-primary  pull-right"><i class="icon-plus-circle2"></i> Create Document</a>
                </div>
            </div>
        </div>
        <div class="panel-body" style="padding: 0px;">
        </div>
        <table class="table datatable-basic">
            <thead>
                <tr>
                    <th>File Title</th>
                    <th>Created By</th>
                    <th>Created On</th>
                    <th>View Document</th>
                    <th>Download</th>
                    <th>Delete</th>
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_SalesDocs" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
    </div>


    <script>
        function deleteDoc(fileName) {
            if (confirm("Are you sure you want to delete this document?")) {
                window.location.href = "Salesdocs.aspx?delFile=" + encodeURIComponent(fileName);
            }
        }
    </script>

</asp:Content>

