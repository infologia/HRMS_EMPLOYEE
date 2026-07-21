<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="WorkedHoursDetails.aspx.cs" Inherits="Employee_WorkedHoursDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
    <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>
    <style>
       
        .year-filter {
            display: flex;
            align-items: center;
        }

       .year-label {
           margin-right: 8px;
           font-size: 13px;
       }

      .year-dropdown {
          width: 120px;
          height: 30px;
          padding: 2px 6px;
          font-size: 13px;
      }

    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
     <div class="panel panel-flat">
  <div class="panel-heading">
            <div class="row">
                <div class="col-lg-4">
    <h5 class="panel-title">Worked Days Details</h5>
        </div><br />

    <div class="heading-elements year-filter">
        <label for="ddlYear" class="year-label">Selected Year:</label>
        <asp:DropDownList ID="ddlYear" runat="server" CssClass="form-control year-dropdown"
            AutoPostBack="true" OnSelectedIndexChanged="ddlYear_SelectedIndexChanged">
        </asp:DropDownList>
    </div>
</div>
      </div>


    <div class="panel-body" style="padding: 0px;">
    </div>
    <table class="table datatable-basic">
        <thead>
            <tr>
                <th>Name</th>
                <th>Month</th>
                <th>Year</th>
                <th>Worked Days</th>
                <th>Working Days</th>
                <th>Over Days Worked</th>
            </tr>
        </thead>
        <tbody>
            <asp:PlaceHolder ID="PH_leave" runat="server"></asp:PlaceHolder>

        </tbody>
    </table>
    </div>
</asp:Content>

