<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Suggestionstatus.aspx.cs" Inherits="WEB_Employee_Suggestionstatus" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-4"></div>
         <div class="col-md-6 col-md-offset-3">
            <div class="panel panel-flat">
                <div class="panel-heading">
                    <h5 class="panel-title"></h5>
                </div>
                <div class="panel-body">
                    <div action="#">
                          <fieldset>
                            <legend class="text-semibold"><i class="icon-pencil4"></i>&nbsp;   Create  Suggestion </legend>


                        </fieldset>
                        <div class="form-group">
                            <label class="content-group text-semibold">Suggestion Category</label>
                            <asp:DropDownList ID="ddl_category" runat="server" class="form-control">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="ddl_category" InitialValue="0" runat="server" ErrorMessage="Please select Category" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <label class="content-group text-semibold">Employee Reason</label>
                            <textarea id="txt_reason" runat="server" rows="4" cols="4" class="form-control" required=""></textarea>
                        </div>
                        <div id="sugg" runat="server" visible="false">
                            <label class="content-group text-semibold">Status</label>
                            <asp:DropDownList ID="ddl_status" runat="server" class="form-control" disabled="disabled">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddl_status" InitialValue="0" runat="server" ErrorMessage="Please select Status" ForeColor="Red"></asp:RequiredFieldValidator>
                            <br />
                            <div class="form-group">
                                <label class="content-group text-semibold">Admin Response</label>
                                <textarea id="txt_response" runat="server" rows="4" cols="4" class="form-control" readonly="readonly" required=""></textarea>
                            </div>
                        </div>
                        <div class="text-right">
                            <a id="btn_request" runat="server" href="Suggestionresponseview.aspx" class="btn btn-primary me-2">Back</a>
                            <asp:Button ID="btn_update" runat="server" Text="Update" OnClick="btn_update_Click" class="btn btn-primary me-2"></asp:Button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

