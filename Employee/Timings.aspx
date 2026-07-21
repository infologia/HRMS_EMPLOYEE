<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Timings.aspx.cs" Inherits="Employee_Timings" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .vertical-scroll {
            max-height: 350px;
            overflow-y: auto;
        }
        .confirm-modal{position:fixed!important;top:0!important;left:0!important;right:0!important;bottom:0!important;width:100%!important;height:100%!important;background:rgba(0,0,0,0.5)!important;display:flex!important;align-items:center!important;justify-content:center!important;z-index:99999!important;visibility:hidden;opacity:0;transition:opacity 0.3s;}
        .confirm-modal.show{visibility:visible!important;opacity:1!important;}
        .confirm-modal-content{background:#fff!important;border-radius:8px!important;padding:30px!important;max-width:400px!important;width:90%!important;box-shadow:0 4px 20px rgba(0,0,0,0.3)!important;text-align:center!important;margin:auto!important;position:relative!important;}
        .confirm-modal-content h3{margin:0 0 15px;font-size:20px;color:#333;}
        .confirm-modal-content p{margin:0 0 25px;color:#666;font-size:16px;}
        .confirm-modal-actions{display:flex;gap:15px;justify-content:center;}
        .confirm-modal-btn{padding:10px 30px;border:none;border-radius:5px;font-size:14px;cursor:pointer;transition:all 0.3s;}
        .confirm-modal-btn.yes{background:#28a745;color:#fff;}
        .confirm-modal-btn.yes:hover{background:#218838;}
        .confirm-modal-btn.no{background:#dc3545;color:#fff;}
        .confirm-modal-btn.no:hover{background:#c82333;}
        
        .eom-card{background:#EFEFFC;border-radius:10px;box-shadow:0 4px 12px rgba(0,0,0,0.1);padding:15px;color:#333;position:relative;overflow:hidden;border:1px solid #d0d0e8!important;}
        .eom-card::before{content:'';position:absolute;top:-50%;right:-50%;width:200%;height:200%;background:radial-gradient(circle,rgba(106,27,154,0.05) 0%,transparent 70%);animation:shimmer 8s infinite;}
        @keyframes shimmer{0%,100%{transform:rotate(0deg);}50%{transform:rotate(180deg);}}
        .eom-content{position:relative;z-index:2;text-align:center;}
        .eom-badge{background:linear-gradient(135deg,#6a1b9a,#4a148c);color:#ffd700;display:inline-block;padding:4px 12px;border-radius:15px;font-weight:bold;font-size:10px;text-transform:uppercase;margin-bottom:8px;box-shadow:0 2px 6px rgba(106,27,154,0.3);}
        .eom-profile{width:50px;height:50px;border-radius:50%;border:3px solid #6a1b9a;box-shadow:0 4px 10px rgba(106,27,154,0.3);margin:0 auto 8px;display:block;object-fit:cover;}
        .eom-title{font-size:14px;font-weight:bold;margin:0 0 5px;color:#6a1b9a;}
        .eom-name{font-size:18px;font-weight:800;margin:0 0 8px;letter-spacing:0.5px;text-transform:uppercase;color:#4a148c;}
        .eom-footer{font-size:11px;opacity:0.85;font-style:italic;margin-top:8px;color:#6a1b9a;}
        
        .today-card{border-radius:12px;box-shadow:0 4px 15px rgba(0,0,0,0.1);border:1px solid #e3f2fd!important;overflow:hidden;}
        .today-card .panel-heading{background:linear-gradient(135deg,#2196F3,#1976D2);color:#fff;padding:12px 15px;border:none;}
        .today-card .panel-body{padding:20px;background:#fff;}
        .today-card .small{margin-bottom:10px;}
        .today-card .small:last-child{margin-bottom:0;}
        .today-card .small span:first-child{min-width:140px;display:inline-block;}
        
        .attendance-card{border-radius:12px;box-shadow:0 4px 15px rgba(0,0,0,0.1);border:1px solid #e8f5e9!important;overflow:hidden;}
        .attendance-card .panel-heading{background:linear-gradient(135deg,#4CAF50,#388E3C);color:#fff;padding:12px 15px;border:none;}
        .attendance-card .panel-body{padding:20px;background:#fff;}
        .attendance-card p{margin-bottom:10px;}
        .attendance-card p:last-child{margin-bottom:0;}
        
        .quotes-card{border-radius:12px;box-shadow:0 4px 15px rgba(0,0,0,0.1);border:1px solid #fff3e0!important;background:linear-gradient(135deg,#fff9e6,#fffef7);padding:20px;}
        .quotes-card h6{color:#e65100;font-size:14px;font-weight:700;margin-bottom:15px;text-transform:uppercase;letter-spacing:0.5px;}
        .quotes-card blockquote{border-left:4px solid #ff9800;padding-left:15px;margin:0;font-style:italic;color:#5d4037;font-size:15px;line-height:1.6;}
        .quotes-card footer{color:#8d6e63;font-size:13px;margin-top:10px;font-weight:600;}
        
        .timing-card{background:white;border-radius:16px;box-shadow:0 10px 30px rgba(168,197,255,0.2);padding:20px;border:none!important;position:relative;overflow:hidden;}
        .timing-card::before{content:'';position:absolute;top:-50%;right:-30%;width:300px;height:300px;background:radial-gradient(circle,rgba(255,255,255,0.15),transparent);}
        .timing-card::after{content:'';position:absolute;bottom:-50%;left:-30%;width:300px;height:300px;background:radial-gradient(circle,rgba(255,255,255,0.12),transparent);}
        .timing-section{position:relative;z-index:1;}
        .timing-btn-group{margin-bottom:12px;}
        .timing-btn{border:2px solid rgba(255,255,255,0.2)!important;background:#5a5a5a!important;color:#fff!important;font-weight:600!important;padding:10px 16px!important;border-radius:8px!important;transition:all 0.3s ease!important;text-transform:uppercase;letter-spacing:0.5px;font-size:12px!important;}
        .timing-btn:hover:not(:disabled){background:#4a4a4a!important;border-color:rgba(255,255,255,0.4)!important;transform:translateY(-2px);box-shadow:0 5px 15px rgba(0,0,0,0.3);}
        .timing-btn:disabled{opacity:0.55;cursor:not-allowed;background:#888!important;border-color:rgba(255,255,255,0.2)!important;}
        .timing-btn.btn-success{background:#5cb85c!important;border-color:#4caf50!important;}
        .timing-btn.btn-success:hover:not(:disabled){background:#4caf50!important;border-color:#388e3c!important;}
        .timing-btn.btn-danger{background:#d9534f!important;border-color:#f44336!important;}
        .timing-btn.btn-danger:hover:not(:disabled){background:#f44336!important;border-color:#d32f2f!important;}
        .timing-details{background:#ffffff;border-radius:12px;padding:0;box-shadow:0 4px 15px rgba(0,0,0,0.1);overflow:hidden;border:1px solid #f0f0f0;max-height:260px;}
        .timing-details table{margin:0;border-collapse:collapse;width:100%;}
        .timing-details tbody tr{border-bottom:none;transition:all 0.3s ease;}
        .timing-details tbody tr:hover{background-color:#f9f9f9;}
        .timing-details tbody tr:last-child{border-bottom:none;}
        .timing-details td{padding:7px 10px!important;border:none!important;color:#333;}
        .timing-details td:first-child{min-width:75px;padding-left:10px!important;}
        .timing-details td:last-child{padding-right:12px!important;}
        .timing-details td strong{font-weight:600;display:inline-flex;align-items:center;gap:4px;font-size:12px;color:#222;}
        .timing-details td:nth-child(2){text-align:right;color:#333;font-weight:500;font-size:12px;}

        /* Color-coded rows */
        .timing-details tbody tr:nth-child(1) td:first-child strong::before{content:'🔵';opacity:0.7;font-size:11px;}
        .timing-details tbody tr:nth-child(1){background-color:#f0f8f5;}
        .timing-details tbody tr:nth-child(2) td:first-child strong::before{content:'🔴';opacity:0.7;font-size:11px;}
        .timing-details tbody tr:nth-child(2){background-color:#fef5f5;}
        .timing-details tbody tr:nth-child(3) td:first-child strong::before{content:'⏸️';opacity:0.7;font-size:11px;}
        .timing-details tbody tr:nth-child(3){background-color:#fffbf5;}
        .timing-details tbody tr:nth-child(4) td:first-child strong::before{content:'🍽️';opacity:0.7;font-size:11px;}
        .timing-details tbody tr:nth-child(4){background-color:#f5f9ff;}
        .timing-details tbody tr:nth-child(5) td:first-child strong::before{content:'🍽️';opacity:0.7;font-size:11px;}
        .timing-details tbody tr:nth-child(5){background-color:#faf5ff;}

        .timing-details tbody tr:nth-child(1) td strong{color:#1b5e20;}
        .timing-details tbody tr:nth-child(2) td strong{color:#b71c1c;}
        .timing-details tbody tr:nth-child(3) td strong{color:#bf360c;}
        .timing-details tbody tr:nth-child(4) td strong{color:#0d47a1;}
        .timing-details tbody tr:nth-child(5) td strong{color:#4a148c;}

        /* Time and Date Display */
        .timing-value{font-size:13px;font-weight:700;color:#222;margin-bottom:1px;display:block;}
        .timing-date{font-size:10px;color:#555;font-weight:500;display:block;}
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="div_error" runat="server" class="alert alert-success" visible="false">
        <asp:Label ID="lbl_error" runat="server"></asp:Label>
    </div>
    <div class="row">
        <div class="col-md-8">
            <%--Employee of the month--%>
            <div class="row">
                <div class="panel eom-card">
                    <div class="eom-content">
                        <div class="eom-badge">🏆 Employee of the Month</div>
                        <asp:Image ID="Img_Profile" runat="server" CssClass="eom-profile" />
                        <div class="eom-title">Congratulations!</div>
                        <div class="eom-name"><asp:Label ID="lbl_employeeofthemonth" runat="server"></asp:Label></div>
                        <div class="eom-footer">✨ Infologia Technologies ✨</div>
                    </div>
                </div>
            </div>
            <%--in/out time--%>
             <div class="row">
                 <div class="panel timing-card">
                     <div class="timing-section">
                         <!-- LEFT : BUTTONS -->
                         <div class="col-lg-6 col-md-6" style="padding-top:25px">
                             <div class="timing-btn-group">
                                 <div class="row">
                                     <div class="col-xs-6">
                                         <asp:Button ID="btn_intime" runat="server"
                                             CssClass="btn btn-success btn-block timing-btn"
                                             Text="In Time" Enabled="false"
                                             OnClick="btn_intime_Click" />
                                     </div>
                                     <div class="col-xs-6">
                                         <asp:Button ID="btn_outtime" runat="server"
                                             CssClass="btn btn-danger btn-block timing-btn"
                                             Text="Out Time" Enabled="false"
                                             OnClick="btn_outtime_Click" />
                                     </div>
                                 </div>
                             </div>

                             <div class="timing-btn-group">
                                 <div class="row">
                                     <div class="col-xs-6">
                                         <asp:Button ID="btn_breakin" runat="server"
                                             CssClass="btn btn-success btn-block timing-btn"
                                             Text="Break In" Enabled="false"
                                             OnClick="btn_breakin_Click" />
                                     </div>
                                     <div class="col-xs-6">
                                         <asp:Button ID="btn_breakout" runat="server"
                                             CssClass="btn btn-danger btn-block timing-btn"
                                             Text="Break Out" Enabled="false"
                                             OnClick="btn_breakout_Click" />
                                     </div>
                                 </div>
                             </div>

                             <div class="timing-btn-group">
                                 <div class="row">
                                     <div class="col-xs-6">
                                         <asp:Button ID="btn_lunchin" runat="server"
                                             CssClass="btn btn-success btn-block timing-btn"
                                             Text="Lunch In" Enabled="false"
                                             OnClick="btn_lunchin_Click" />
                                     </div>
                                     <div class="col-xs-6">
                                         <asp:Button ID="btn_lunchout" runat="server"
                                             CssClass="btn btn-danger btn-block timing-btn"
                                             Text="Lunch Out" Enabled="false"
                                             OnClick="btn_lunchout_Click" />
                                     </div>
                                 </div>
                             </div>
                         </div>

                         <!-- RIGHT : DETAILS -->
                         <div class="col-lg-6 col-md-6">
                             <div class="timing-details">
                                 <table >
                                     <tr>
                                         <td><strong class="text-success">In Time</strong></td>
                                         <td>
                                             <div><asp:Label ID="lbl_InTime" runat="server" style="font-size:13px;font-weight:700;color:#222;" /></div>
                                             <div class="timing-date"><asp:Label ID="lbl_InTimeDate" runat="server" /></div>
                                         </td>
                                     </tr>
                                     <tr>
                                         <td><strong class="text-danger">Out Time</strong></td>
                                         <td>
                                             <div><asp:Label ID="lbl_OutTime" runat="server" style="font-size:13px;font-weight:700;color:#222;" /></div>
                                             <div class="timing-date"><asp:Label ID="lbl_OutTimeDate" runat="server" /></div>
                                         </td>
                                     </tr>
                                     <tr>
                                         <td><strong class="text-warning">Last Break</strong></td>
                                         <td>
                                             <div><asp:Label ID="lbl_LastBreak" runat="server" style="font-size:13px;font-weight:700;color:#222;" /></div>
                                             <div class="timing-date"><asp:Label ID="lbl_LastBreakDate" runat="server" /></div>
                                         </td>
                                     </tr>
                                     <tr>
                                         <td><strong class="text-info">Lunch In</strong></td>
                                         <td>
                                             <div><asp:Label ID="lbl_LunchIn" runat="server" style="font-size:13px;font-weight:700;color:#222;" /></div>
                                             <div class="timing-date"><asp:Label ID="lbl_LunchInDate" runat="server" /></div>
                                         </td>
                                     </tr>
                                     <tr>
                                         <td><strong class="text-primary">Lunch Out</strong></td>
                                         <td>
                                             <div><asp:Label ID="lbl_LunchOut" runat="server" style="font-size:13px;font-weight:700;color:#222;" /></div>
                                             <div class="timing-date"><asp:Label ID="lbl_LunchOutDate" runat="server" /></div>
                                         </td>
                                     </tr>
                                 </table>
                             </div>
                         </div>
                     </div>
                 </div>
             </div>

        </div>
        <div class="col-md-4">
            <!-- Team -->
            <div class="panel panel-flat"
                style="border-radius: 8px; box-shadow: 0 2px 6px rgba(0,0,0,0.15); border: 1px solid #ddd;">

                <!-- FIXED HEADER -->
                <div class="panel-heading"
                    style="background-color: #f5f5f5; border-bottom: 1px solid #ddd; border-top-left-radius: 8px; border-top-right-radius: 8px; padding: 10px 15px; position: sticky; top: 0; z-index: 10;">
                    <h5 class="panel-title"
                        style="margin: 0; font-weight: 600; color: #333;">Team
                    </h5>
                </div>

                <!-- SCROLLABLE BODY -->
                <div style="max-height: 430px; overflow-y: scroll;">
                    <ul class="media-list media-list-linked"
                        style="padding: 15px; margin: 0;">
                        <asp:PlaceHolder ID="PH_Userlist" runat="server"></asp:PlaceHolder>
                    </ul>
                </div>

            </div>
        </div>

    </div>
    <div class="row">
        <div class="col-md-8">
            <div class="row d-flex">

                <!-- TODAY CARD -->
                <div class="col-md-6 d-flex">

                    <div class="panel panel-info today-card d-flex flex-column h-100 w-100">

                        <!-- Header -->
                        <div class="panel-heading text-center">
                            <strong>Today</strong>
                            <asp:Literal ID="litTodayStatus" runat="server"></asp:Literal>
                        </div>

                        <!-- Body -->
                        <div class="panel-body flex-grow-1">

                            <!-- Percentage Circle -->
                            <div class="svg-center position-relative mb-4" id="progress_percentage_two"></div>

                            <p class="small d-flex justify-content-between">
                                <span class="fw-bold text-primary"><strong>In Time:</strong></span>
                                <span>
                                    <asp:Literal ID="Ltr_Intime" runat="server"></asp:Literal></span>
                            </p>

                            <p class="small d-flex justify-content-between">
                                <span class="fw-bold text-warning"><strong>Break Duration:</strong></span>
                                <span>
                                    <asp:Literal ID="Ltr_Breakduration" runat="server"></asp:Literal></span>
                            </p>

                            <p class="small d-flex justify-content-between">
                                <span class="fw-bold text-info"><strong>Lunch Duration:</strong></span>
                                <span>
                                    <asp:Literal ID="Ltr_LunchDuration" runat="server"></asp:Literal></span>
                            </p>

                            <p class="small d-flex justify-content-between">
                                <span class="fw-bold text-danger"><strong>Out Time:</strong></span>
                                <span>
                                    <asp:Literal ID="Ltr_Outtime" runat="server"></asp:Literal></span>
                            </p>

                            <p class="small d-flex justify-content-between">
                                <span class="fw-bold text-success"><strong>Worked Hours:</strong></span>
                                <span>
                                    <asp:Literal ID="Ltr_WorkedHours" runat="server"></asp:Literal></span>
                            </p>

                           
                        </div>
                    </div>

                </div>


                <!-- ATTENDANCE CARD -->
                <div class="col-md-6 d-flex">

                    <div class="panel panel-primary attendance-card d-flex flex-column h-100 w-100">

                        <!-- Header -->
                        <div class="panel-heading text-center">
                            <strong>My Attendance</strong>
                        </div>

                        <!-- Body -->
                        <div class="panel-body flex-grow-1 vertical-scroll">

                            <p class="d-flex justify-content-between">
                                <span><strong>Paid Leave Taken:</strong></span>
                                <span>
                                    <asp:Literal ID="litLeaveTaken" runat="server"></asp:Literal>/12
                                </span>
                            </p>

                            <p class="d-flex justify-content-between text-danger">
                                <span><strong>LOP:</strong></span>
                                <span>
                                    <asp:Literal ID="litLOPDays" runat="server"></asp:Literal>
                                </span>
                            </p>

                            <p class="d-flex justify-content-between text-success">
                                <span><strong>Balance Leave:</strong></span>
                                <span>
                                    <asp:Literal ID="litBalanceLeave" runat="server"></asp:Literal>
                                </span>
                            </p>
                             <p class="small d-flex justify-content-between">
     <span class="fw-bold text-primary"><strong>Permission Taken:</strong></span>
     <span>
         <asp:Literal ID="litPermissionUsed" runat="server"></asp:Literal></span>
 </p>

 
 <p class="d-flex justify-content-between text-success">
     <span><strong>Balance Permission:</strong></span>
     <span>
         <asp:Literal ID="litPermissionBalance" runat="server"></asp:Literal>
     </span>
 </p>

                        </div>
                    </div>

                </div>

            </div>
            <%--Quotes--%>
            <div class="row">
                <div class="col-md-12">
                    <div class="panel quotes-card">
                        <h6 class="no-margin text-semibold">💭 Quotes</h6>
                        <blockquote class="no-margin">
                            <asp:Label ID="lbl_quotes" runat="server"></asp:Label>
                            <footer>
                                <asp:Label ID="lbl_author" runat="server"></asp:Label>
                            </footer>
                        </blockquote>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-md-4">
            <!-- Simple stats with thumbnail -->
            <div class="panel"
                style="border-radius: 8px; box-shadow: 0 2px 6px rgba(0,0,0,0.15); border: 1px solid #ddd; transition: box-shadow 0.3s;">
                <!-- Card header with icon -->
                <div style="padding: 10px 15px; background-color: #f5f5f5; border-bottom: 1px solid #ddd; border-top-left-radius: 8px; border-top-right-radius: 8px; display: flex; align-items: center;">
                    <h4 class="text-semibold no-margin" style="font-size: 16px; color: black;">Task Details</h4>
                </div>

                <!-- Card body with vertical scroll -->
                <div class="panel-body" style="max-height: 300px; overflow-y: auto; padding: 15px;"
                    onscroll="this.parentElement.style.boxShadow='0 4px 12px rgba(0,0,0,0.25)'; setTimeout(() => {this.parentElement.style.boxShadow='0 2px 6px rgba(0,0,0,0.15)';}, 300);">
                    <asp:PlaceHolder ID="PH_TaskSummary" runat="server"></asp:PlaceHolder>
                </div>
            </div>
            <!-- /simple stats with thumbnail -->
        </div>
    </div>

    <div id="confirmModal" class="confirm-modal">
        <div class="confirm-modal-content">
            <h3>Confirm Action</h3>
            <p>Are you sure?</p>
            <div class="confirm-modal-actions">
                <button type="button" class="confirm-modal-btn yes" onclick="confirmAction(true)">Yes</button>
                <button type="button" class="confirm-modal-btn no" onclick="confirmAction(false)">No</button>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        var pendingButton = null;
        
        function showConfirmModal(btn) {
            pendingButton = btn;
            var btnText = btn.value || btn.innerText;
            var modalText = document.querySelector('#confirmModal p');
            modalText.textContent = 'Are you sure you want to ' + btnText + '?';
            document.getElementById('confirmModal').classList.add('show');
            return false;
        }
        
        function confirmAction(confirmed) {
            document.getElementById('confirmModal').classList.remove('show');
            if (confirmed && pendingButton) {
                pendingButton.onclick = null;
                pendingButton.click();
            }
            pendingButton = null;
        }
        
        window.onload = function() {
            var buttons = ['<%= btn_intime.ClientID %>', '<%= btn_outtime.ClientID %>', '<%= btn_breakin.ClientID %>', '<%= btn_breakout.ClientID %>', '<%= btn_lunchin.ClientID %>', '<%= btn_lunchout.ClientID %>'];
            buttons.forEach(function(id) {
                var btn = document.getElementById(id);
                if (btn) {
                    btn.onclick = function() { return showConfirmModal(this); };
                }
            });
        };
    </script>
</asp:Content>

<%--	<div class="col-lg-4">
	<asp:Chart ID="Chart1" runat="server" Width="400px" Height="160px">
		<Series>
			<asp:Series Name="Series1" XValueMember="0" YValueMembers="1">
			</asp:Series>
		</Series>
		<ChartAreas>
			<asp:ChartArea Name="ChartArea1">
			</asp:ChartArea>
		</ChartAreas>
	</asp:Chart>
</div>--%>