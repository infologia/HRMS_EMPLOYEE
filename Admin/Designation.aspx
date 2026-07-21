<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Designation.aspx.cs" Inherits="WEB_Employee_Designation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
             <script type="text/javascript" src="../Template/assets/js/plugins/tables/datatables/datatables.min.js"></script>
 <script type="text/javascript" src="../Template/assets/js/plugins/forms/selects/select2.min.js"></script>
 <script type="text/javascript" src="../Template/assets/js/pages/datatables_advanced.js"></script>
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
        <a onclick="RemoveRow(this)"
           class="btn btn-danger btn-rounded btn-xs">
           ✕
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
            if (!confirm("Are you sure? do you want to remove this Designation?"))
                return false;
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
   
     <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" OnRowDeleting="GridView1_RowDeleting" OnRowUpdating="GridView1_RowUpdating" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnRowEditing="GridView1_RowEditing" DataKeyNames="Desgid" CssClass="table table-bordered table-hover datatable-highlight" style="width:100%" OnRowDataBound="GridView1_RowDataBound">
    
         <columns>
        
        <asp:BoundField DataField="Desgid" HeaderText="Designation ID"  readonly="true"/>
             <asp:BoundField DataField="Destinationname" HeaderText="Designation Name" ControlStyle-CssClass="form-control" />

        <asp:CommandField HeaderText="Edit" ShowEditButton="true" ControlStyle-CssClass="label label-info"/>
      <asp:CommandField HeaderText="Delete" ShowDeleteButton="true" ControlStyle-CssClass="label label-danger"/> </columns>  
      </asp:GridView>
               </div>
         </div>
</asp:Content>

