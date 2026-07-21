
<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="UpdateWorkingDayDetails.aspx.cs" Inherits="Admin_UpdateWorkingDayDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row">
        <div class="col-md-4"></div>
    <div class="col-md-4">

							<!-- Basic legend -->
							<div class="form-horizontal" action="#">
								<div class="panel panel-flat">
									<div class="panel-heading">
										<h5 class="panel-title"><i class="icon-clipboard6"></i> Employee working day details</h5>
										<div class="heading-elements">
											
					                	</div>
									</div>

									<div class="panel-body">
										<fieldset>
											<legend class="text-semibold"></legend>
                                            <div class="row">
                                                <div class="col-md-5">
											<div class="form-group">
												<label >Select Year:</label>
												
													<asp:DropDownList ID="ddl_year" runat="server" cssclass="form-control" ></asp:DropDownList>
												</div>
											</div>
                                                <div class="col-md-2"></div>
                                                <div class="col-md-5">
											<div class="form-group">
												<label >Select Month:</label>
												<asp:DropDownList ID="ddl_month" runat="server" cssclass="form-control" ></asp:DropDownList>
												</div>
											</div>
                                                </div>
											<div class="form-group">
												<label >Number of days in month:</label>
												
													<asp:TextBox ID="txt_days" runat="server" CssClass="form-control" readonly="true"></asp:TextBox>
												</div>
											

											<div class="form-group">
												<label>Number of working days in month:</label>
												
													<asp:TextBox ID="txt_work" runat="server" CssClass="form-control"  required="required" ></asp:TextBox>
												</div>
                                               
                                                <%--	<div class="form-group">
												<label >Number of leave days in month:</label>
												
													<asp:TextBox ID="txt_leave" runat="server" CssClass="form-control" onclick="sub_days();" required="required"></asp:TextBox>
												</div>--%>
											
                                            <div class="form-group">
                                                <div class="text-center">
                                                    <a href="WorkingDayViewDetails.aspx" class="btn bg-slate-700 margin-left-1" style="margin-right:15px"><i class="icon-undo" ></i> Back</a>
                                                    <asp:Button ID="btn_update" runat="server" Text="Update" CssClass="btn bg-slate-700" OnClick="btn_update_Click"></asp:Button>
                                                </div>
                                            </div>
										</fieldset>
                                        </div>
                                    </div>
                                </div>
        </div>
        <div class="col-md-"></div>
        </div>
</asp:Content>

