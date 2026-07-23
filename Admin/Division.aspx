<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Division.aspx.cs" Inherits="WEB_Employee_Division" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
       <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
 <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/extensions/buttons.min.js"></script>
 <script type="text/javascript" src="../Template/assets/js/pages/datatables_extension_buttons_init.js"></script>
 <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
           
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

        // --- Edit and Delete functions ---
        var currentDivId = "";

        function openEditModal(divId, divName) {
            currentDivId = divId;
            $('#txt_EditDivName').val(divName);
            $('#editDivModal').modal('show');
        }

        function SaveEditDivision() {
            var divName = $('#txt_EditDivName').val().trim();
            if (divName === '') {
                showToastr('error', 'Division name is required!');
                $('#txt_EditDivName').focus();
                return false;
            }

            $.ajax({
                type: "POST",
                url: "Division.aspx/UpdateDivision",
                data: JSON.stringify({ divId: currentDivId, divName: divName }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                    if (response.d === "true") {
                        showToastr('success', 'Division updated successfully!');
                        $('#editDivModal').modal('hide');
                        setTimeout(function () { location.reload(); }, 1500);
                    } else {
                        showToastr('error', 'Failed to update division.');
                    }
                },
                error: function () {
                    showToastr('error', 'Server error. Please try again.');
                }
            });
        }

        var deleteDivId = "";
        function fn_DeleteDivision(divId) {
            deleteDivId = divId;
            $('#confirmDeleteModal').modal('show');
        }

        function confirmDeleteDivision() {
            if (deleteDivId === "") return;

            $.ajax({
                type: "POST",
                url: "Division.aspx/DeleteDivision",
                data: JSON.stringify({ divId: deleteDivId }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    $('#confirmDeleteModal').modal('hide');
                    if (data.d === "true") {
                        showToastr('success', 'Division deleted successfully!');
                        setTimeout(function () { location.reload(); }, 1500);
                    } else {
                        showToastr('error', 'Unable to delete division.');
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
             <table class="table datatable-basic">
                <thead>
                    <tr>
                        <th>Division ID</th>
                        <th>Division Name</th>
                        <th class="text-center">Actions</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:PlaceHolder ID="PH_Division" runat="server"></asp:PlaceHolder>
                </tbody>
            </table>
          </div>
    </div>

    <!-- Edit Modal -->
    <div class="modal fade" id="editDivModal" tabindex="-1" role="dialog" data-backdrop="static" data-keyboard="false">
        <div class="modal-dialog modal-sm" style="margin-top: 15vh;" role="document">
            <div class="modal-content">
                <div class="modal-header bg-primary">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h5 class="modal-title">Edit Division</h5>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label class="text-semibold">Division Name <span class="text-danger">*</span></label>
                        <input type="text" id="txt_EditDivName" class="form-control" placeholder="Enter division name" />
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-link" data-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary" onclick="SaveEditDivision();">Update</button>
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
                    <p class="mb-0">Are you sure you want to delete this division?</p>
                </div>
                <div class="modal-footer justify-content-center">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">No</button>
                    <button type="button" class="btn btn-danger" onclick="confirmDeleteDivision()">Yes, Delete</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
