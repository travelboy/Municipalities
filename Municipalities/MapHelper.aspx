<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MapHelper.aspx.cs" Inherits="Municipalities.MapHelper" %>
<%@ Import Namespace="System.ServiceModel.Channels" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
         <asp:Repeater ID="rptMap" runat="server">
                        <ItemTemplate>
 
        
        <div id="try" style="padding-left: 80px; width: 560px; float: left;">
            
                <div id="opis"   style=" border-width: 4px; border-style: inset; border-color: #FF9B6C;   background-color: #637f83 ; width: 560px; float: left;  height: auto">
             
        <h3 style="font-family: cursive;
color: #e9774a;
font-size: larger;"> Опис на општината </h3>
        <p style="color: whitesmoke;
font-family: cursive;
font-size: smaller;">
<%#Eval("FBdescription") %>
</p>
          
      </div>     
                  
            <div id="test" style="height: 50px"></div>
        <div id="extraInfo" style="  width: 560px; height: auto   border-width: 4px; border-style: inset; border-color: #FF9B6C;   background-color: whitesmoke  " >
       <%-- <p style="color: whitesmoke;
font-family: cursive;
font-size: smaller;">адреса <%#Eval("FBstreet") %></p>
        
         <br/>
         <p  style="color: whitesmoke;
font-family: cursive;
font-size: smaller;">телефон <%#Eval("FBphoto") %></p>
         <br/>
        <a style="color: whitesmoke;
font-family: cursive;
font-size: smaller;" href="'<%#Eval("FBwebsite") %>'">линк кон општината</a>--%>
                  <br/>
            
                <p style=" color: #425f9c;
font-family: fantasy;

font-style: italic"><img src="images/facebook.png" style="width: 30px; height: 30px"> <%= calcFB() %></p>
            
                <p style=" color: #88c9f9;
font-family: fantasy;

font-style: italic"><img src="images/twitter.jpg" style="width: 30px; height: 30px"> <%= calcTW() %></p>
             
                <p style=" color: #e12a27;
font-family: fantasy;

font-style: italic"><img src="images/youtubeplay.png" style="width: 30px; height: 30px"><%= calcYT() %></p>
            </div>
        </div>
  


     
                            
                                         
      <div id="generalnoInfo" style=" border-width: 4px; border-style: inset; border-color: #FF9B6C;   background-color: #637f83 ; float:right; width: 560px; height: auto; padding-right: 80px ">

            <h3 style="font-family: cursive;
color: #e9774a;
font-size: larger;"> Генерално инфо </h3>
         <p style=" color: whitesmoke;
font-family: cursive;
font-size: smaller;">
        <%#Eval("FBgeneralInfo") %>
        </p>
          
          

      </div>
     
        
     

   
</ItemTemplate> </asp:Repeater>
    </form>
</body>
</html>
