<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Designation.aspx.cs" Inherits="WEB_Employee_Designation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
             <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
 <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
 <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>
    <style type="text/css">
        .Remove
        {
            margin-top: -33px;
        }
        .div_Dynamicrow {
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
               placeholder="Designation"
               id="txt_Seq${value}"
               name="txt_Seq${value}"
               class="form-control"   />
    </div>

    <!-- Close button -->
        <div class="col-xs-1 col-sm-1 col-md-1 text-right">
            <a onclick="RemoveRow(this)" class="btn btn-danger btn-rounded btn-xs">
               <i class="icon-trash"></i>
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
            var rowDiv = $(btn).closest("[id^='div_Dynamicrow']");
            rowDiv.remove();
            div_ids = div_ids.replace(rowDiv.attr("id") + ",", "");
        }

        function SaveConfigItems() {
          
            var isValid = true;

            $("#DyanmicCreation input[type='text']").each(function () {
                if ($(this).val().trim() === "") {
                    $(this).focus();
                    showToastr('error', 'Designation field cannot be empty!');
                    isValid = false;
                    return false; 
                }
            });

            if (!isValid) return false;
            var DivIds=$('#ContentPlaceHolder1_hdn_DepIds').val();
            
            var AllDivids=DivIds+div_ids;
            var Contain = "";
            var div_split = new Array();
            AllDivids=AllDivids.substring(0,AllDivids.length-1);
            div_split = AllDivids.split(',');
            for (var i = 0; i < div_split.length; i++) {
               
                if (div_split[i] != '') {
                    $("#" + div_split[i]).find('*').each(function () {
                        var CtrlID = $(this).attr("id");
                        if (typeof CtrlID != "undefined") {
                            TempVal = "";
                            if (CtrlID.indexOf("txt_Seq") > -1) {
                                Contain += $(this).val()+"###";
                                
                            }
                        }
                    });
                }
            }
            $.ajax({
                
                type: "POST",
                url: "Designation.aspx/SaveConfigItems",
                data: "{str_ControlValue:'" + Contain + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccessCall,
                error: OnErrorCall
            });
            function OnSuccessCall(response) {
                if (response.d === "true") {

                    showToastr('success', 'Designation saved successfully!');

                    setTimeout(function () {
                        window.location.href = '/Admin/Designation.aspx';
                    }, 2000);

                } else {
                    showToastr('error', 'Failed to save data');
                }
            }
            function OnErrorCall(response) {
                alert(response.status + " " + response.statusText);
                window.location.reload(true);
            }
        }

        // --- Edit and Delete functions ---
        var currentDesgId = "";

        function openEditModal(desgId, desgName) {
            currentDesgId = desgId;
            $('#txt_EditDesgName').val(desgName);
            $('#editDesgModal').modal('show');
        }

        function SaveEditDesignation() {
            var desgName = $('#txt_EditDesgName').val().trim();
            if (desgName === '') {
                showToastr('error', 'Designation name is required!');
                $('#txt_EditDesgName').focus();
                return false;
            }

            $.ajax({
                type: "POST",
                url: "Designation.aspx/UpdateDesignation",
                data: JSON.stringify({ desgId: currentDesgId, desgName: desgName }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d === "true") {
                        showToastr('success', 'Designation updated successfully!');
                        $('#editDesgModal').modal('hide');
                        setTimeout(function () { location.reload(); }, 1500);
                    } else {
                        showToastr('error', 'Failed to update designation.');
                    }
                },
                error: function () {
                    showToastr('error', 'Server error. Please try again.');
                }
            });
        }

        var deleteDesgId = "";
        function fn_DeleteDesignation(desgId) {
            deleteDesgId = desgId;
            $('#confirmDeleteModal').modal('show');
        }

        function confirmDeleteDesignation() {
            if (deleteDesgId === "") return;

            $.ajax({
                type: "POST",
                url: "Designation.aspx/DeleteDesignation",
                data: JSON.stringify({ desgId: deleteDesgId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $('#confirmDeleteModal').modal('hide');
                    if (data.d === "true") {
                        showToastr('success', 'Designation deleted successfully!');
                        setTimeout(function () { location.reload(); }, 1500);
                    } else {
                        showToastr('error', 'Unable to delete designation.');
                    }
                },
                error: function () {
                    $('#confirmDeleteModal').modal('hide');
                    showToastr('error', 'Server error. Please try again.');
                }
            });
        }
    </script>
  

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

     <asp:HiddenField ID="hdn_DepIds" runat="server" />

    	<div class="panel panel-flat">

            <div class="panel-heading">
<div class="row">
<div class="col-md-6 pull-left">
<h5 class="panel-title">Designation</h5></div><div class="col-md-6  pull-right">
<a   onclick="AddTextBox();"  runat="server" class="btn btn-primary pull-right"><i class="icon-plus-circle2"></i>  Add New</a>
</div>
</div>
</div>
     <div id="DyanmicCreation">
                                        <asp:PlaceHolder ID="pl_ConfigItems" runat="server"></asp:PlaceHolder>
                                    </div>
                            
                                <div class="modal-footer">
                                    <button type="button" id="btn_ConfigSave" onclick="SaveConfigItems();" class="btn btn-primary"> Save</button>
                                </div>
                            </div>
                 
    <div class="panel panel-flat">
						<div class="panel-heading">
							<h5 class="panel-title"> Employee Designation</h5>
							<div class="heading-elements">
							
		                	</div>
						</div>

					
						
           <div class="table-responsive">
             <table class="table datatable-basic">
                <thead>
                    <tr>
                        <th>Designation ID</th>
                        <th>Designation Name</th>
                        <th class="text-center">Actions</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:PlaceHolder ID="PH_Designation" runat="server"></asp:PlaceHolder>
                </tbody>
            </table>
          </div>
    </div>

    <!-- Edit Modal -->
    <div class="modal fade" id="editDesgModal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm" style="margin-top: 15vh;" role="document">
            <div class="modal-content">
                <div class="modal-header bg-primary">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h5 class="modal-title">Edit Designation</h5>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label class="text-semibold">Designation Name <span class="text-danger">*</span></label>
                        <input type="text" id="txt_EditDesgName" class="form-control" placeholder="Enter designation name" />
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-link" data-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary" onclick="SaveEditDesignation();">Update</button>
                </div>
            </div>
        </div>
    </div>

    <!-- Delete Confirmation Modal -->
    <div class="modal fade" id="confirmDeleteModal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm" style="margin-top: 15vh;" role="document">
            <div class="modal-content">
                <div class="modal-header bg-danger text-white">
                    <h5 class="modal-title">Confirm Delete</h5>
                    <button type="button" class="close text-white" data-dismiss="modal">
                        <span>&times;</span>
                    </button>
                </div>
                <div class="modal-body text-center">
                    <p class="mb-0">Are you sure you want to delete this designation?</p>
                </div>
                <div class="modal-footer justify-content-center">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">No</button>
                    <button type="button" class="btn btn-danger" onclick="confirmDeleteDesignation()">Yes, Delete</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
