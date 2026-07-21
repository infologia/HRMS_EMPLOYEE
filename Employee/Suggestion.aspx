<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Suggestion.aspx.cs" Inherits="WEB_Employee_Suggestion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-4"></div>
        <div class="col-md-6 col-md-offset-3">
            <div class="panel panel-flat">
                <div class="panel-heading">
                </div>

                <div class="panel-body">
                    <fieldset>
                        <legend class="text-semibold"><i class="icon-pencil4"></i> Create Suggestion</legend>
                    </fieldset>

                    <div class="form-group">
                        <label class="content-group text-semibold">SuggestionCategory</label>
                        <asp:DropDownList ID="ddl_category" runat="server" class="form-control">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddl_category" InitialValue="0" runat="server" ErrorMessage="Please select Category" ForeColor="Red"></asp:RequiredFieldValidator>

                    </div>
                    <div class="form-group">

                        <label class="content-group text-semibold">Reason</label>
                        <textarea id="txt_reason" runat="server" rows="5" cols="5" class="form-control" placeholder="Enter your Reason here"></textarea>

                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ErrorMessage="Fillout The Field" ControlToValidate="txt_reason" ForeColor="Red"
                            runat="server" />
                        
                    </div>
                    <div class="text-right">
                         <a href="Suggestionresponseview.aspx" class="btn btn-primary me-2"> Back</a>
                        <asp:LinkButton ID="btn_submit" class="btn btn-primary me-2" runat="server" OnClick="btn_submit_Click"> Submit </asp:LinkButton>
                       
                    </div>
                </div>
            </div>

            <div class="col-md-4"></div>
        </div>
    </div>
</asp:Content>

