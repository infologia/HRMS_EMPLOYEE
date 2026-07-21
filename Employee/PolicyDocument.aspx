<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="PolicyDocument.aspx.cs" Inherits="Employee_PolicyDocument" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel panel-flat">
        <div class="panel-heading">
            <h5 class="panel-title">Documents</h5>
           <%-- <div class="heading-elements">
                <a href="Complaints.aspx" class="btn btn-primary  pull-right"><i class="icon-plus-circle2"></i>Create Complaint</a>
            </div>--%>
        </div>
        <div class="panel-body" style="padding: 0px;">
        </div>
        <table class="table datatable-basic">
            <thead>
                <tr>
                     <th>S.No</th>
                    <th>Document Name </th>
                    <th>View Document </th>
                    <th>Download</th>
                    <th>Status</th>
                    <th>Created On</th>
                   
         
                </tr>
            </thead>
            <tbody>
                <asp:PlaceHolder ID="PH_EmployeeDocumentView" runat="server"></asp:PlaceHolder>
            </tbody>
        </table>
    </div>


</asp:Content>
