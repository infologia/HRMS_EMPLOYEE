<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="EmployeePayrollView.aspx.cs" Inherits="Employee_EmployeePayrollView" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
     <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.22/pdfmake.min.js"></script>
 <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/html2canvas/0.4.1/html2canvas.min.js"></script>
 <script type="text/javascript">
     function Export() {
         html2canvas(document.getElementById('print'), {
             onrendered: function (canvas) {
                 var data = canvas.toDataURL();
                 var docDefinition = {
                     content: [{
                         image: data,
                         width: 500
                     }]
                 };
                 pdfMake.createPdf(docDefinition).download("Table.pdf");
             }
         });
     }
     </script>

    <style>
        .payslip-card {
            max-width: 700px;
            margin: 30px auto;
            padding: 30px;
            border-radius: 15px;
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
            background-color: #fff;
        }

        .payslip-header {
            text-align: center;
            margin-bottom: 20px;
        }

        .payslip-header h3 {
            color: #007bff;
            font-weight: 600;
            letter-spacing: 0.5px;
        }

        .payslip-table td {
            padding: 8px 5px;
            font-size: 15px;
        }

        .btn-download {
            background-color: #007bff;
            color: white;
            border: none;
            padding: 8px 20px;
            border-radius: 6px;
        }

        .btn-download:hover {
            background-color: #0056b3;
        }

        .btn-back {
            background-color: #6c757d;
            color: white;
            border: none;
            padding: 8px 20px;
            border-radius: 6px;
            margin-left: 10px;
        }

        .btn-back:hover {
            background-color: #5a6268;
        }
    </style>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="d-flex justify-content-end mt-3 me-3">
        <button type="button" class="btn-download" onclick="Export()">Download</button>
        <a href="EmployeePayroll.aspx" class="btn-back">Back</a>
    </div>

   <div id="print" class="payslip-card">
        <!--<div class="payslip-header">
            <h3>Employee Payslip</h3>
            <hr />
        </div>-->

        <div class="payslip-body">
            <asp:PlaceHolder ID="payroll" runat="server"></asp:PlaceHolder>
        </div>
    </div>
</asp:Content>
