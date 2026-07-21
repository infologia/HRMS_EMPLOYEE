<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Complaintview.aspx.cs" Inherits="WEB_Admin_Complaintview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
      <div class="row">

    <div class="col-md-2"></div>
<div class="col-md-8">
            <div class="panel panel-flat">
                <div class="panel-heading">
                    <h5 class="panel-title"></h5>
                    <%--<div class="heading-elements">
                    </div>--%>
                </div>

                <div class="panel-body">
                    <div action="#">
                          <fieldset>
                            <legend class="text-semibold"><i class="icon-reading position-left"></i> Complaint Response</legend>
                       <div class="row">
                           <div class="col-md-6">
    <label  class=" content-group text-semibold">ComplaintsCategory:</label>
    <asp:DropDownList ID="ddl_category" runat="server" class="form-control" disabled="disabled">
    </asp:DropDownList><br />
</div>
                            <div class="col-md-6">
  <label  class=" content-group text-semibold">Status</label>
  <asp:DropDownList ID="ddl_status" runat="server" class="form-control">
  </asp:DropDownList>
  <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddl_status" InitialValue="0" runat="server" ErrorMessage="Please select Status" ForeColor="Red"></asp:RequiredFieldValidator>
</div>
                       </div>
                        <br />

                     <div class="row">
                        <div class="col-md-6">
                            <label  class=" content-group text-semibold">Employee Reason</label>
                            <textarea id="txt_reason" runat="server" rows="4" cols="4" class="form-control" readonly="readonly"></textarea><br />
                        </div>

                        <div class="col-md-6">
                            <label  class=" content-group text-semibold">Admin Response</label>
                            <textarea id="txt_response" runat="server" rows="4" cols="4" class="form-control" required="required"></textarea>
                        </div>
                     </div>
<br />
                        <div class="form-group">
                            <div class="text-right">
                                     <a href="ComplaintResponse.aspx" class="btn btn-primary" style="margin-right: 15px">Back</a>
                                <asp:Button ID="btn_request" runat="server" Text="Update" OnClick="btn_request_Click" class="btn btn-primary" Visible="false" style="margin-right: 15px"></asp:Button>
                           
                            </div>

                        </div></fieldset>
                    </div>
                </div>
            </div>
        </div>
       <div class="col-md-2"></div>

    </div>
</asp:Content>

