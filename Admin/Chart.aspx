<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage/AdminMaster.master" AutoEventWireup="true" CodeFile="Chart.aspx.cs" Inherits="Admin_Chart" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
<script type="text/javascript" src="https://www.google.com/jsapi"></script>

    <script type="text/javascript">
        google.load("visualization", "1", { packages: ["corechart"] });
        google.setOnLoadCallback(drawChart);
        function drawChart() {
            var pjkey = document.getElementById('ContentPlaceHolder1_hn_pjkey').value;
       
            var options = {
                title: 'USA City Distribution',
                width: 600,
                height: 400,
                bar: { groupWidth: "50%" },
                legend: { position: "none" },
                isStacked: true
            };
            $.ajax({
                type: "POST",
                url: "Chart.aspx/GetChartData",
                data: "{ str_PjtCategorykey: '" + pjkey + "'}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (r) {
                    var data = google.visualization.arrayToDataTable(r.d);
                    var chart = new google.visualization.ColumnChart($("#chart")[0]);
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
    <div id="chart" style="width: 900px; height: 500px;"></div>
    <asp:HiddenField ID="hn_pjkey" runat="server" />
</asp:Content>

