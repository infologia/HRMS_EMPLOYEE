<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="ReceivableInvoiceGrid.aspx.cs" Inherits="Admin_ReceivableInvoiceGrid" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
        <div class="panel panel-flat">
    <div class="panel-heading">
        <div class="row">
            <div class="col-lg-4">
                <h5 class="panel-title">Receivable Invoices Details</h5>
            </div>
            <div class="col-lg-8">
                <a href="createinvoice.aspx"  runat="server" class="btn btn-primary  pull-right"><i class="icon-plus-circle2"></i> Create Invoice</a>
            </div>
        </div>
    </div>
    <div class="panel-body" style="padding: 0px;">
    </div>
    <div class="table-responsive">
        <table class="table datatable-basic">
            <thead>
                <tr>
                    
                    <th>Client Name</th>
                    <th>Project Name</th>
                    <th>Invoice Number</th>
                    <th>Invoice Date</th>
                    <th>Due Date</th>
                    <th>Status</th>  
                    <th>Download</th>
                    <th>CreatedOn</th>
                    <th>Update</th>
        
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_RECEIVABLEINVOICE" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
    </div>
</div>
</asp:Content>

