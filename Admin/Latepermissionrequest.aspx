<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Latepermissionrequest.aspx.cs" Inherits="Employee_Latepermissionrequest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" src="../Template/assets/js/plugins/pickers/anytime.min.js"></script>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <div class="row">

        <div class="col-md-4"></div>
        <div class="col-md-4">

            <!-- Vertical form -->
            <div class="panel panel-flat">
                <div class="panel-heading">
                    <h5 class="panel-title"></h5>
                    <div class="heading-elements">
                    </div>
                </div>

                <div class="panel-body">
                    <fieldset>
                        <legend class="text-semibold"><i class="icon-pencil4"></i> Late Permission Request</legend>
                        <div class="form-group">
                            <label class="content-group text-semibold">Request Date </label>
                            <div class="input-group">
                                <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                <asp:TextBox ID="txt_date" runat="server" Enabled="false" CssClass="form-control" required=""></asp:TextBox>
                            </div>
                        </div>
                        <br />
                    </fieldset>
                    <div class="row">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="content-group text-semibold">From Time </label>

                                <div class="input-group">
                                    <span class="input-group-addon"><i class="icon-alarm"></i></span>
                                    <asp:TextBox ID="txt_fromtime" runat="server" CssClass="form-control pickatime-clear" required=""></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-6">
                            <div class="form-group">
                                <label class="content-group text-semibold">To Time </label>

                                <div class="input-group">
                                    <span class="input-group-addon"><i class="icon-alarm"></i></span>
                                    <asp:TextBox ID="txt_totime" runat="server" class="form-control pickatime-clear" required=""></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="form-group">
                        <label class="content-group text-semibold">Reason </label>
                        <textarea id="txt_reasons" runat="server" rows="4" cols="4" class="form-control" required=""></textarea>
                    </div>
                    <div class="form-group">
                        <div class="text-center">
                            <a href="Latepermissionrequestview.aspx" class="btn btn-primary" style="margin-right: 15px">Back</a>
                            <asp:Button ID="btn_request" runat="server" Text="Request" OnClick="btn_perm_Click" class="btn btn-primary"></asp:Button>
                        </div>

                    </div>
                </div>

            </div>

        </div>

    </div>
</asp:Content>

