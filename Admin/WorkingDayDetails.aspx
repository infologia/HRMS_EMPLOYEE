<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="WorkingDayDetails.aspx.cs" Inherits="WEB_WorkingDayDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-md-6">
            <!-- Basic legend -->
            <div class="form-horizontal" action="#">
                <div class="panel panel-flat">
                    <div class="panel-heading">
                        <h5 class="panel-title"><i class="icon-clipboard6"></i>Employee working day details</h5>
                    </div>
                    <div class="panel-body">
                        <fieldset>
                            <legend class="text-semibold"></legend>
                            <div class="row">
                                <div class="col-md-5">
                                    <div class="form-group">
                                        <label>Select Year:</label>
                                        <asp:DropDownList ID="ddl_year" runat="server" cssclass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl_year_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-2"></div>
                                <div class="col-md-5">
                                    <div class="form-group">
                                        <label>Select Month:</label>

                                        <asp:DropDownList ID="ddl_month" runat="server" cssclass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddl_month_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                            <div class="form-group">
                                <label>Number of days in month:</label>
                                <asp:TextBox ID="txt_days" runat="server" CssClass="form-control" readonly="true"></asp:TextBox>
                            </div>

                            <div class="form-group">
                                <label>Number of working days in month:</label>

                                <asp:TextBox ID="txt_work" runat="server" CssClass="form-control" required="required"></asp:TextBox>
                            </div>

                            <div class="form-group">
                                <div class="text-right">

                                    <a href="WorkingDayViewDetails.aspx" class="btn btn-primary" style="margin-right: 15px">Back</a>
                                    <asp:Button ID="btn_update" runat="server" Text="Update" CssClass="btn btn-primary" OnClick="btn_update_Click" style="margin-right: 15px"></asp:Button>
                                    <asp:Button ID="btn_submit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btn_submit_Click" style="margin-right: 15px"></asp:Button>
                                </div>
                            </div>
                        </fieldset>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-6">
            <div class="form-horizontal">
                <div class="panel panel-flat">
                    <div class="panel-heading">
                        <h5 class="panel-title">Holiday Details</h5>
                    </div>
                    <div class="panel-body">
                        <fieldset>
                            <legend class="text-semibold"></legend>

                            <div class="text-right" style="margin-bottom: 10px;">
                                <button type="button" class="btn btn-info btn-sm" onclick="addRow()">
                                  Add Row
                                </button>
                            </div>

                            <div class="table-responsive">
                                <table class="table table-bordered table-sm table-condensed" style="font-size:12px;">
                                    <thead class="table-light">
                                        <tr>
                                            <th style="width:130px; padding:4px 6px;">Date</th>
                                            <th style="width:110px; padding:4px 6px;">Day</th>
                                            <th style="padding:4px 6px;">Description</th>
                                            <th style="width:50px; padding:4px 6px; text-align:center;">Del</th>
                                        </tr>
                                    </thead>
                                    <tbody id="tableBody">
                                    </tbody>
                                </table>
                            </div>

                            <select id="daysTemplate" style="display: none">
                                <%= GetDaysNameOptions() %>
                            </select>
                        </fieldset>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <style>
        #tableBody td { padding: 3px 4px; vertical-align: middle; }
        #tableBody .form-control-sm { height: 26px; padding: 2px 5px; font-size: 12px; }
        #tableBody .btn-xs { padding: 1px 6px; font-size: 11px; line-height: 1.5; }
    </style>
    <script>
        function addRow() {
            var tbody = $('#tableBody');
            var template = document.getElementById('daysTemplate');
            var daysOptions = template.innerHTML;

            var rowHtml = '<tr>' +
                '<td><input type="text" class="form-control form-control-sm pickadate-new" name="holiday_date" placeholder="DD/MM/YYYY" /></td>' +
                '<td><select class="form-control form-control-sm" name="holiday_day">' + daysOptions + '</select></td>' +
                '<td><input type="text" class="form-control form-control-sm" name="holiday_desc" placeholder="Description" /></td>' +
                '<td class="text-center"><button type="button" class="btn btn-danger btn-xs" onclick="removeRow(this)">&#10005;</button></td>' +
                '</tr>';

            var newRow = $(rowHtml);
            tbody.append(newRow);
            newRow.find('.pickadate-new').pickadate({ format: 'dd/mm/yyyy' });
        }

        function removeRow(btn) {
            $(btn).closest('tr').remove();
        }

        $(document).ready(function () {
            $('.pickadate').pickadate({ format: 'dd/mm/yyyy' });
        });
    </script>
</asp:Content>

