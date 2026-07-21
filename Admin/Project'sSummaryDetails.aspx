<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Project'sSummaryDetails.aspx.cs" Inherits="TicketingTool_Project_sSummaryDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-12">
            <div class="panel panel-flat">
                <div class="panel-heading">
                    <h5 class="panel-title"></h5>
                    <div class="heading-elements">
                    </div>
                </div>
                <div class="panel-body">
                    <fieldset>
                        <legend class="text-semibold"><i class="icon-pencil5"></i>Project's Summary</legend>
                        <div action="#">
                            <div class="form-group">
                                <label>Project Name :</label>
                                <asp:Label ID="lbl_pjname" runat="server"></asp:Label>
                            </div>
                            <div class="form-group">
                                <label>Created Date :</label>
                                <asp:Label ID="lbl_crdate" runat="server"> </asp:Label>
                            </div>
                            <div class="form-group">
                                <label>Closed Date :</label>
                                <asp:Label ID="lbl_cldate" runat="server"> </asp:Label>
                            </div>
                            <div class="form-group ">
                                <label>Name :</label>
                                <asp:Label ID="lbl_name" runat="server"> </asp:Label>
                            </div>
                            <div class="form-group ">
                                <label>Description</label>

                                <asp:TextBox ID="txt_des" runat="server" TextMode="MultiLine" required="" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                            </div>
                           <%-- <div class="form-group">
                                <div class="text-right">
                                    <a href="Project'sSummary.aspx" class="btn bg-primary-400 margin-left-1" style="margin-right: 15px">Back</a>
                                </div>
                            </div>--%>
                        </div>
                    </fieldset>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

