<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Division.aspx.cs" Inherits="WEB_Employee_Division" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
       <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
 <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
 <script type="text/javascript" src="../Template/assets/js/pages/datatables_advanced.js"></script>
           
    <style type="text/css">
        .Remove
        {
            margin-top: -33px;
        }.div_Dynamicrow {
    margin-bottom: 15px;   /* textbox keezha space */
}

/* Optional – textbox height & clean look */
.div_Dynamicrow .form-control {
    margin-bottom: 0;      /* double gap avoid */
}
    </style>
        <script type="text/javascript">
            var ConfigId;
            var textvalue = <%=int_Seq%>;
         var div_ids = "";
         var formid = "";
            function Dynamicrow(value) {

                return `
    <div class="row div_Dynamicrow align-items-center">
    <div class="col-xs-2 col-sm-2 col-md-2"></div>
        <div class="col-xs-6 col-sm-6 col-md-6">
        <input type="text"
               placeholder="Division"
               id="txt_Seq${value}"
               name="txt_Seq${value}"
               class="form-control" />
    </div>

    <!-- Close button -->
    <div class="col-xs-1 col-sm-1 col-md-1 col-lg-1 text-right">
        <a onclick="RemoveRow(this)"
           class="btn btn-danger btn-rounded btn-xs">
            X
        </a>
    </div>

</div>`;
            }


         function AddTextBox(count) {
             textvalue++;
             var div = document.createElement('DIV');
             div.id = "div_Dynamicrow" + textvalue;
             $("#DyanmicCreation").append("<div id=" + "div_Dynamicrow" + textvalue + ">" + Dynamicrow(textvalue) + "</div>");
             div_ids += div.id + ",";
           
         }

            function RemoveRow(btn) {
                if (!confirm("Are you sure? do you want to remove this Division?"))
                    return false;
                var rowDiv = $(btn).closest("[id^='div_Dynamicrow']");
                rowDiv.remove();
                div_ids = div_ids.replace(rowDiv.attr("id") + ",", "");
            }

            function confirmDeleteProject() {

                if (rowToDelete != null) {

                    document.getElementById("DyanmicCreation").removeChild(rowToDelete);

                    div_ids = div_ids.replace(rowToDelete.id + ",", "");

                    rowToDelete = null;
                }
                $('#confirmDeleteModal').modal('hide');
            }

            function SaveConfigItems() {

                var DivIds = $('#ContentPlaceHolder1_hdn_DivIds').val();
                var AllDivids = DivIds + div_ids;

                if (AllDivids.endsWith(",")) {
                    AllDivids = AllDivids.substring(0, AllDivids.length - 1);
                }

                var Contain = "";
                var div_split = AllDivids.split(',');
                var hasError = false;

                for (var i = 0; i < div_split.length; i++) {

                    if (div_split[i] !== '') {

                        $("#" + div_split[i]).find('*').each(function () {

                            var CtrlID = $(this).attr("id");

                            if (typeof CtrlID !== "undefined" && CtrlID.indexOf("txt_Seq") > -1) {

                                var val = $(this).val().trim();

                                if (val === "") {
                                    hasError = true;
                                    $(this).addClass("input-error"); // highlight
                                } else {
                                    $(this).removeClass("input-error");
                                    Contain += val + "###";
                                }
                            }
                        });
                    }
                }

                if (hasError) {
                    showToastr('error', 'Division field cannot be empty!');
                    return false;  
                }

                if (Contain === "") {
                    showToastr('error', 'Please add at least one Division');
                    return false;
                }

                $.ajax({
                    type: "POST",
                    url: "Division.aspx/SaveConfigItems",
                    data: JSON.stringify({ str_ControlValue: Contain }),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {

                        if (response.d === "true") {

                            showToastr('success', 'Division saved successfully!');

                            setTimeout(function () {
                                window.location.href = '/Admin/Division.aspx';
                            }, 2000);

                        } else {
                            showToastr('error', 'Failed to save data');
                        }
                    },
                    error: function () {
                        showToastr('error', 'Server error occurred');
                    }
                });
            }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

 
       <asp:HiddenField ID="hdn_DivIds" runat="server" />

    	<div class="panel panel-flat">
            <div class="panel-heading">
<div class="row">
<div class="col-md-6 pull-left">
<h5 class="panel-title">Division</h5></div><div class="col-md-6  pull-right">
 <a class="btn btn-primary pull-right" onclick="AddTextBox();" ><i class="icon-plus-circle2"></i>   Add New</a>
<div class="clearfix"></div>
</div>
</div>
 
</div>
						
   
                   

                                    <div id="DyanmicCreation">
                                        <asp:PlaceHolder ID="pl_ConfigItems" runat="server"></asp:PlaceHolder>
                                    </div>
                            




                                <div class="modal-footer">
                                    <button type="button" id="btn_ConfigSave" onclick="SaveConfigItems();" class="btn btn-primary">Save</button>
                                </div>
                            </div>
                 
   <div class="panel panel-flat">
						<div class="panel-heading">
							<h5 class="panel-title"> Employee Division</h5>
							<div class="heading-elements">
							
		                	</div>
						</div>


						
          <div class="table-responsive">
   
     <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" OnRowDeleting="GridView1_RowDeleting" OnRowUpdating="GridView1_RowUpdating" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowEditing="GridView1_RowEditing" DataKeyNames="Divid" CssClass="table table-bordered table-hover datatable-highlight" style="width:100%" OnRowDataBound="GridView1_RowDataBound">
    
         <columns>
        
        <asp:BoundField DataField="Divid" HeaderText="Division ID"  readonly="true"/>
             <asp:BoundField DataField="Divisionname" HeaderText="Division Name" ControlStyle-CssClass="form-control"/>
 

        <asp:CommandField HeaderText="Edit" ShowEditButton="true" ControlStyle-CssClass="label label-info"/>
      <asp:CommandField HeaderText="Delete" ShowDeleteButton="true" ControlStyle-CssClass="label label-danger"/> </columns>  
      </asp:GridView>
         </div></div>
</asp:Content>

