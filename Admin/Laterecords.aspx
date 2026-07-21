<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Laterecords.aspx.cs" Inherits="Laterecords" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-2"></div>
<div class="col-md-8">
        <div class="panel panel-flat">
            <div class="panel-heading">
                <h5 class="panel-title"></h5>
               
            </div>

            <div class="panel-body">

                <fieldset>
                    <legend class="text-semibold"><i class="icon-reading position-left"></i>Employee Late Record</legend>
                    <div class="row">
                        <div class="col-md-3">
                            <label class=" content-group text-semibold">Request Date </label>
                            <div class="input-group">
                                <span class="input-group-addon"><i class="icon-calendar22"></i></span>
                                <asp:TextBox ID="txt_date" runat="server" class="form-control pickadate no-calendar" placeholder="DD/MM/YYYY" ReadOnly="true"></asp:TextBox>
                            </div><br />
                        </div>
                        <div class="col-md-3">
                                   <label class=" content-group text-semibold">From Time </label>
                                 <div class="input-group">
                                    <span class="input-group-addon"><i class="icon-alarm"></i></span>
                                    <asp:TextBox ID="txt_fromtime" runat="server" CssClass="form-control pickatime-clear no-calendar" ReadOnly="true"  placeholder="HH:mm" ></asp:TextBox>
                                 </div><br />
                         </div>                                                  
                        <div class="col-md-3">                       
                                <label class=" content-group text-semibold">To Time </label>
                                <div class="input-group">
                                    <span class="input-group-addon"><i class="icon-alarm"></i></span>
                                    <asp:TextBox ID="txt_totime" runat="server" class="form-control pickatime-clear no-calendar" ReadOnly="true"  placeholder="HH:mm" ></asp:TextBox>
                                </div><br />
                            </div>                       
                        <div class="col-md-3">
                            <label class="content-group text-semibold">Status</label>
                            <asp:DropDownList ID="ddl_category" runat="server" class="form-control">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddl_category" InitialValue="0" runat="server" ErrorMessage="Please select status" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>

                    </div>


                </fieldset>
                <br />
                <div class="row">
                    <div class="col-md-6">
                        <label class=" content-group text-semibold">Employee Reason</label>
                        <textarea id="txt_reasons" runat="server" rows="4" cols="4" class="form-control" readonly="readonly" placeholder="Enter a reason"></textarea><br />
                    </div>

                    <div class="col-md-6">
                        <label class=" content-group text-semibold">Admin Reason</label>
                        <textarea id="txt_reason1" runat="server" rows="4" cols="4" class="form-control" required="required" placeholder="Enter a reason"></textarea>
                    </div>

                </div>

                <br />
                <div class="form-group">
                    <div class="text-right">
                        <a href="Laterecordview.aspx" class="btn btn-primary" style="margin-right: 15px"> Back</a>
                        <asp:Button ID="btn_update" runat="server" Text="Update" class="btn btn-primary" OnClick="btn_update_Click" Visible="false" style="margin-right: 15px"></asp:Button>

                    </div>

                </div>
            </div>

        </div>

    </div>
                <div class="col-md-2"></div>

        </div>
        <script>
    $('.pickadate').pickadate({
        format: 'dd/mm/yyyy',
        selectMonths: true,
        selectYears: true,
        closeOnSelect: true
    });
        </script>
</asp:Content>

