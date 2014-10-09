<%@ Page Title="" Language="C#" MasterPageFile="~/Municipalities.Master" AutoEventWireup="true" CodeBehind="Analysis.aspx.cs" Inherits="Municipalities.Analysis" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

   
<script type="text/javascript" src="js\canvasjs.min.js"></script>
     <script src="C:\Users\bojan\Documents\Visual Studio 2012\Projects\Municipalities\Municipalities\Scripts\jquery-2.1.1.js"></script>
   

</asp:Content>



<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
  

    
    <div id="holder" style="padding-left: 80px">
    

            <asp:DropDownList ID="DropDownList1" runat="server" >
            </asp:DropDownList>
            <asp:TextBox ID="txtYear" runat="server" placeholder="внеси година"></asp:TextBox >
            <asp:DropDownList ID="DropDownList2" runat="server">
            </asp:DropDownList>
            <asp:TextBox ID="TextBox1" runat="server" placeholder="внеси година"></asp:TextBox>
            <input id="btnFBData" type="button" value="Прикажи графици" onclick="GetFB()"  />
            <br />
        <br />
            <p style="font-size: x-small;
font: status-bar;
color: red;">Почетно, прикажани се активностите на општината Кисела Вода во текот на 2013-та година. Слободно изберете друга општина и соодветна година (2013,2014).</p>
            <p style="font: small-caption;
font-style: oblique;
color: tomato;">Сакате да направите споредба? Дополнително, изберете уште една општина и година </p>
    <br />
         
    
      
  <p style="font-size: x-small;
font: status-bar;
color: lightskyblue;"><i id='parbody'></i> </p>
        <p style="font-size: x-small;
font: status-bar;
color: lightskyblue;"><i id='pb1'></i> </p>      
           <div id="chartContainerFB" style="height: 450px; width: 800px;">
    </div>
    <br/>
        <div id="chartContainerTw" style="height: 450px; width: 800px;">
    </div>
     <br/>
        <div id="chartContainerYT" style="height: 450px; width: 800px;">
    </div>
    <br/>
          <div id="chartContainerPie" style="height: 450px; width: 800px;">
    </div>
      <br/>
        <div id="chartDoubleFB" style="height: 450px; width: 800px;">
    </div>
     <br/>
        <div id="chartDoubleTW" style="height: 450px; width: 800px;">
    </div>
    <br/>
          <div id="chartDoubleYT" style="height: 450px; width: 800px;">
    </div>

        </div>
 <script type="text/javascript">
     

     $(window).load(function () {

         var x = '<%  = getStatsFB("2", "2013") %>';

         handleData(x, 'f', true);

         var y = '<%  = getStatsTwitter("2", "2013") %>';
         handleData(y, 't', true);


         var z = '<%  = getStatsYT("2", "2013") %>';
         handleData(z, 'y', true);

         var pie = '<%  = getYearlyInfo("2", "2013") %>';
         handleData(pie, 'pie', true);
    
       });
    
     var finaldata = "";

     function GetFB() {
        

         var munList = document.getElementById('<%=DropDownList1.ClientID%>');

         var munListS = document.getElementById('<%=DropDownList2.ClientID%>');
         var year1 = document.getElementById('<%=txtYear.ClientID%>');
         document.getElementById('parbody').innerHTML = "Ги гледате активностите на општината " + munList.options[munList.selectedIndex].text + " во текот на "+year1.value+" година.";
         if (munListS.options[munListS.selectedIndex].value != 0)
             document.getElementById('pb1').innerHTML = " Дополнително, последните три графици ќе ја покажат споредбата со податоците на општината " + munListS.options[munListS.selectedIndex].text + " од " + document.getElementById('<%=TextBox1.ClientID%>').value + " година";

         // create the json string
         var jsonData = JSON.stringify({
             'munID': munList.options[munList.selectedIndex].value,
             'y': document.getElementById('<%=txtYear.ClientID%>').value
             
         });
      
        

       
         var frequest = $.ajax({
             type: "POST",
             url: "Analysis.aspx/getStatsFB",
             data: jsonData,
             dataType: "json",
             contentType: "application/json; charset=utf-8",
             success: function(data, status, xhr) {
                 
            
                 //povikaj ja funkcijata za prikaz na single opstini
                 var tag = 'f';
                 handleData(data, tag,false);
                 //i dokolku e napravena selekcija na vtora opstina , zemi gi i nejzinite podatoci i povikaj nova funkcija
                 if (munListS.options[munListS.selectedIndex].value != 0)

                 {
                     var mmm= document.getElementById('<%=DropDownList2.ClientID%>');
                     // create the json string
                     var jsonSecond = JSON.stringify({
                         'munID': munListS.options[mmm.selectedIndex].value,
                         'y': document.getElementById('<%=TextBox1.ClientID%>').value

                     });
                   
                     var fb =$.ajax({
                         type: "POST",
                         url: "Analysis.aspx/getStatsFB",
                         data: jsonSecond,
                         dataType: "json",
                         contentType: "application/json; charset=utf-8",
                         success: function (data1, status, xhr) {
                             var d1 = data1;
                             var d = data;
                             var dtag = 'f';
                             //tuka sredi gi dvojnite grafici
                             handleDoubleData(data,data1, dtag);
                         },
                         error: OnError1
                     });
               
                 }


             },
             error: OnError
         });

         var trequest = $.ajax({
             type: "POST",
             url: "Analysis.aspx/getStatsTwitter",
             data: jsonData,
             dataType: "json",
             contentType: "application/json; charset=utf-8",
             success: function(data, status, xhr) {

                 var tag = 't';
                 handleData(data, tag,false);

                 if (munListS.options[munListS.selectedIndex].value != 0) {
                     var mmm = document.getElementById('<%=DropDownList2.ClientID%>');
                     // create the json string
                     var jsonSecond = JSON.stringify({
                         'munID': munListS.options[mmm.selectedIndex].value,
                         'y': document.getElementById('<%=TextBox1.ClientID%>').value

                     });

                     var tw = $.ajax({
                         type: "POST",
                         url: "Analysis.aspx/getStatsTwitter",
                         data: jsonSecond,
                         dataType: "json",
                         contentType: "application/json; charset=utf-8",
                         success: function (data1, status, xhr) {
                             var d1 = data1;
                             var d = data;
                             var dtag = 't';
                             //tuka sredi gi dvojnite grafici
                             handleDoubleData(data, data1, dtag);
                         },
                         error: OnError1
                     });

                 }



             },
             error: OnError
         });

         var ytrequest = $.ajax({
             type: "POST",
             url: "Analysis.aspx/getStatsYT",
             data: jsonData,
             dataType: "json",
             contentType: "application/json; charset=utf-8",
             success: function (data, status, xhr) {

                 var tag = 'y';
                 handleData(data, tag,false);

                 if (munListS.options[munListS.selectedIndex].value != 0) {
                     var mmm = document.getElementById('<%=DropDownList2.ClientID%>');
                     // create the json string
                     var jsonSecond = JSON.stringify({
                         'munID': munListS.options[mmm.selectedIndex].value,
                         'y': document.getElementById('<%=TextBox1.ClientID%>').value

                     });

                     var fb = $.ajax({
                         type: "POST",
                         url: "Analysis.aspx/getStatsYT",
                         data: jsonSecond,
                         dataType: "json",
                         contentType: "application/json; charset=utf-8",
                         success: function (data1, status, xhr) {
                             var d1 = data1;
                             var d = data;
                             var dtag = 'yt';

                             handleDoubleData(data, data1, dtag);
                         },
                         error: OnError1
                     });

                 }
             },
             error: OnError
         });

         var Pierequest = $.ajax({
             type: "POST",
             url: "Analysis.aspx/getYearlyInfo",
             data: jsonData,
             dataType: "json",
             contentType: "application/json; charset=utf-8",
             success: function (data, status, xhr) {

                 var tag = 'pie';
                 handleData(data, tag,false);
             },
             error: OnError
         });


     }
     function OnError(data) {

     }

     function OnError1(data) {
         
     }

     function handleData(responseData, t, first) {
         var tag = t;
         var json1;
         if (first == true)
              json1 = JSON.parse(responseData);
         else
                json1= JSON.parse(responseData.d);


         //tip na grafik koj sakas da se prikaze
         if (tag == 'f') {
             var plot = "column";
             
            
             //do something if no json data is transmited== mun has no FB activity!!
             var options = {
                 axisX: {
                     title: "месеци"
                 },
                 axisY: {
                     suffix: " постови"
                 },

                 title: {
                   
                     text: "годишни Facebook активности"
         },
                 data: [
                     {
                         type: plot,
                         dataPoints: json1,
                            
                     }
                 ]
             }
             var chart = new CanvasJS.Chart("chartContainerFB", options);

             chart.render();
         } //end plot fb single mun
         if (tag == 't') {
             var plot1 = "column";

            // var json11 = JSON.parse(responseData.d);

             var options1 = {
                 axisX: {
                     title: "месеци"
                 },
                 axisY: {
                     suffix: " твитови"
                 },

                 title: {
                     text: "годишни Twitter активности "
                 },
                 data: [
                     {
                         type: plot1,
                         dataPoints: json1
                     }
                 ]
             }
             var chart1 = new CanvasJS.Chart("chartContainerTw", options1);

             chart1.render();
         } //end plot twitter single mun

         if (tag == 'y') {
             var Yplot1 = "column";

            // var Yjson11 = JSON.parse(responseData.d);

             var Yoptions1 = {
                 axisX: {
                     title: "месеци"
                 },
                 axisY: {
                     suffix: " видеа"
                 },

                 title: {
                     text: "годишни YouTube активности"
                 },
                 data: [
                     {
                         type: Yplot1,
                         dataPoints: json1
                     }
                 ]
             }
             var Ychart1 = new CanvasJS.Chart("chartContainerYT", Yoptions1);

             Ychart1.render();
         } //end plot twitter single mun

         if (tag == 'pie') {
             var plotPie = "pie";

            // var json1Pie = JSON.parse(responseData.d);
             //var x = 0;
             var optionsPie = {
                 axisX: {
                     title: "месеци"
                 },
                 axisY: {
                     suffix: " постови"
                 },

                 title: {
                     text: "севкупни годишни активности на општината"
                 },
                 data: [
                     {
                         type: plotPie,
                         dataPoints: json1
                     }
                 ]
             }
             var chartPie = new CanvasJS.Chart("chartContainerPie", optionsPie);

             chartPie.render();
         } //end plot pie single mun


     }

     function handleDoubleData(response1, response2, t)
     {
         var json1 = JSON.parse(response1.d);
         var json2 = JSON.parse(response2.d);
         var tag = t;
         //tip na grafik koj sakas da se prikaze
         if (tag == 'f') {
             
             //do something if no json data is transmited== mun has no FB activity!!
             var options = {
                 axisX: {
                     title: "месеци"
                 },
                 axisY: {
                     suffix: " постови"
                 },

                 title: {
                     text: "годишни Facebook активности"
                 },
                 data: [
                     {
                         type: "splineArea",
                         color: "rgba(54,158,173,.7)",
                         legendText: document.getElementById('<%=DropDownList1.ClientID%>').options[document.getElementById('<%=DropDownList1.ClientID%>').selectedIndex].text,
                         axisYType: "secondary",
                         showInLegend: true,
                         dataPoints: json1
                     },
                     {
                         type: "splineArea",
                         color: "rgba(194,70,66,.7)",
                         legendText: document.getElementById('<%=DropDownList2.ClientID%>').options[document.getElementById('<%=DropDownList2.ClientID%>').selectedIndex].text,
                         axisYType: "secondary",
                         showInLegend: true,
                         dataPoints: json2
                     }

                 ]
             }
             var chart = new CanvasJS.Chart("chartDoubleFB", options);

             chart.render();
         } //end plot fb single mun
         if (tag == 't') {
           

             var options1 = {
                 axisX: {
                     title: "месеци"
                 },
                 axisY: {
                     suffix: " твитови"
                 },

                 title: {
                     text: "годишни Tweeter активности"
                 },
                 data: [
                     {
                         type: "splineArea",
                         color: "rgba(54,158,173,.7)",
                         legendText: document.getElementById('<%=DropDownList1.ClientID%>').options[document.getElementById('<%=DropDownList1.ClientID%>').selectedIndex].text,
                         axisYType: "secondary",
                         showInLegend: true,
                         dataPoints: json1
                     },
                     {
                         type: "splineArea",
                         color: "rgba(194,70,66,.7)",
                         legendText: document.getElementById('<%=DropDownList2.ClientID%>').options[document.getElementById('<%=DropDownList2.ClientID%>').selectedIndex].text,
                         axisYType: "secondary",
                         showInLegend: true,
                         dataPoints: json2
                     }

                 ]
             }
             var chart1 = new CanvasJS.Chart("chartDoubleTW", options1);

             chart1.render();
         } //end plot twitter single mun

         if (tag == 'yt') {
             

             var Yoptions1 = {
                 axisX: {
                     title: "месеци"
                 },
                 axisY: {
                     suffix: " видеа"
                 },

                 title: {
                     text: "годишни YouTube активности"
                 },
                 data: [
                     {
                         type: "splineArea",
                         color: "rgba(54,158,173,.7)",
                         legendText: document.getElementById('<%=DropDownList1.ClientID%>').options[document.getElementById('<%=DropDownList1.ClientID%>').selectedIndex].text,
                         axisYType: "secondary",
                         showInLegend: true,
                         dataPoints: json1
                     },
                     {
                         type: "splineArea",
                         color: "rgba(194,70,66,.7)",
                         legendText: document.getElementById('<%=DropDownList2.ClientID%>').options[document.getElementById('<%=DropDownList2.ClientID%>').selectedIndex].text,
                         axisYType: "secondary",
                         showInLegend: true,
                         dataPoints: json2
                     }

                 ]
             }
             var Ychart1 = new CanvasJS.Chart("chartDoubleYT", Yoptions1);

             Ychart1.render();
         } //end plot yt single mun
     }


 </script>

</asp:Content>
