<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="ProjectFlowchart.aspx.cs" Inherits="Admin_ProjectFlowchart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>
<script type="text/javascript">
    google.load("visualization", "1", { packages: ["corechart"] });
    google.setOnLoadCallback(drawChart);
    function drawChart() {
        var options = {
            title: 'Time Monitoring',
            width: 600,
            height: 400,
            legend: { position: 'top', maxLines: 3 },
            bar: { groupWidth: '75%' },
            isStacked: true
        };
        $.ajax({
            type: "POST",
            url: "ProjectFlowchart.aspx/GetChartData",
            data: '{}',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (r) {
                var data = google.visualization.arrayToDataTable(r.d);
                var chart = new google.visualization.BarChart($("#chart")[0]);
                chart.draw(data, options);
            },
            failure: function (r) {
                alert(r.d);
            },
            error: function (r) {
                alert(r.d);
            }
        });
    }
</script>

    <script type="text/javascript">
        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(drawChart1);
        function drawChart1() {
            var options = {
                title: 'Employeedetails',
                width: 600,
                height: 400,
                legend: { position: 'top', maxLines: 3 },
                bar: { groupWidth: '75%' },
                isStacked: true
            };
            $.ajax({
                type: "POST",
                url: "ProjectFlowchart.aspx/GetChartData1",
                data: '{}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (r) {
                    var data = google.visualization.arrayToDataTable(r.d);
                    var chart = new google.visualization.LineChart($("#linechart")[0]);
                    chart.draw(data, options);
                },
                failure: function (r) {
                    alert(r.d);
                },
                error: function (r) {
                    alert(r.d);
                }
            });
        }
</script>

    <script type="text/javascript">
        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(drawChart2);
        function drawChart2() {
            var options = {
                title: 'Employee Salarydetails',
                width: 600,
                height: 400,
                legend: { position: 'top', maxLines: 3 },
                bar: { groupWidth: '75%' },
                isStacked: true
            };
            $.ajax({
                type: "POST",
                url: "ProjectFlowchart.aspx/GetChartData2",
                data: '{}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (r) {
                    var data = google.visualization.arrayToDataTable(r.d);
                    var chart = new google.visualization.ScatterChart($("#piecart")[0]);
                    chart.draw(data, options);
                },
                failure: function (r) {
                    alert(r.d);
                },
                error: function (r) {
                    alert(r.d);
                }
            });
        }
</script>

       <script type="text/javascript">
           google.load("visualization", "1", { packages: ["corechart"] });
           google.setOnLoadCallback(drawChart3);
           function drawChart3() {
               var options = {
                   title: 'Employee Task',
                   width: 600,
                   height: 400,
                   legend: { position: 'top', maxLines: 3 },
                   bar: { groupWidth: '75%' },
                   isStacked: true
               };
               $.ajax({
                   type: "POST",
                   url: "ProjectFlowchart.aspx/GetChartData3",
                   data: '{}',
                   contentType: "application/json; charset=utf-8",
                   dataType: "json",
                   success: function (r) {
                       var data = google.visualization.arrayToDataTable(r.d);
                       var chart = new google.visualization.PieChart($("#task")[0]);
                       chart.draw(data, options);
                   },
                   failure: function (r) {
                       alert(r.d);
                   },
                   error: function (r) {
                       alert(r.d);
                   }
               });
           }
</script>


    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row">
    <div class="col-md-6">
    <div id="chart"   onchange="drawChart();">   
     </div> 
    </div>
            <div class="col-md-6">
     <div id="linechart"   onchange="drawChart1();">   
     </div>  
  </div>
</div>


    <div class="row">
    <div class="col-md-6">
    <div id="piecart"   onchange="drawChart2();">   
     </div> 
    </div>
            <div class="col-md-6">
     <div id="task" >   
     </div>  </div>
  
</div>
</asp:Content>

