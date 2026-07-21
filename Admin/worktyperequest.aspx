<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="worktyperequest.aspx.cs" Inherits="Admin_worktyperequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="form-horizontal">
        <div class="panel panel-flat">
            <div class="panel-heading">
                <h5 class="panel-title"></h5>
            </div>

            <div class="panel-body">
                <fieldset>
                    <legend class="text-semibold"><i class="icon-pencil5"></i>Work Type Request</legend>
                    <div action="#">
                        <div class="row">
                        <div class="col-md-4">
                            <label class="content-group text-semibold">Employee Name</label>
                            <asp:DropDownList ID="ddlEmployee" CssClass="form-control" runat="server">
                            </asp:DropDownList>
                            <br />
                        </div>

                        <div class="col-md-4">
                            <label class="content-group text-semibold">Work Type</label>
                            <asp:DropDownList ID="ddlWorkType" CssClass="form-control" runat="server">
                            </asp:DropDownList>
                            <br />
                        </div>
                        <div class="col-md-4">
                            <label class="content-group text-semibold">Reason</label>
                            <textarea id="txt_reason" runat="server" rows="4" cols="4" class="form-control" required="required"></textarea>
                        </div>

                        <div class="col-md-4">
                            <label class="content-group text-semibold">From Date</label>
                            <div class="input-group">
                                <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                <asp:TextBox ID="txt_fromdate" runat="server" CssClass="form-control pickadate" placeholder="DD/MM/YYYY"></asp:TextBox>
                            </div>
                            <br />
                        </div>

                        <div class="col-md-4">
                            <label class="content-group text-semibold">To Date</label>
                            <div class="input-group">
                                <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                <asp:TextBox ID="txt_todate" runat="server" CssClass="form-control pickadate" placeholder="DD/MM/YYYY"></asp:TextBox>
                            </div>
                        </div>
                    <br />
                    </div>

                    <div class="row">
                        <div class="text-right">
                            <a href="worktyperequests.aspx" class="btn btn-primary" style="margin-right: 15px">Back</a>
                            <asp:Button ID="btn_request" runat="server" Text="Request" OnClick="btn_request_Click" class="btn btn-primary" style="margin-right: 15px"></asp:Button>
                        </div>
                    </div>

                </fieldset>
            </div>
        </div>
    </div>

    <script>
        $('.pickadate').pickadate({ format: 'dd/mm/yyyy' });
    </script>
</asp:Content>
