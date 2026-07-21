<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="tds.aspx.cs" Inherits="Admin_tds" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="panel panel-flat">
    <div class="panel-heading">
        <div class="row">
            <div class="col-lg-4">
                <h5 class="panel-title">Receivable TDS Details</h5>
            </div>
        </div>
    </div>

    <div class="horizontal-scroll">
        <table class="table datatable-basic">
           <thead>
    <tr>
        <th>Client Name</th>
        <th>Invoice Number</th>
        <th>Invoice Amount</th>
        <th>TDS Amount</th>
        <th>Total Amount</th>
        <th>Status</th>
        <th>Invoice Date</th>
    </tr>
</thead>

            <tbody>
                <asp:PlaceHolder ID="PH_TDSInvoices" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
    </div>
</div>

</asp:Content>

