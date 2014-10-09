<%@ Page Title="" Language="C#" MasterPageFile="~/Municipalities.Master" AutoEventWireup="true" CodeBehind="RegistrationNew.aspx.cs" Inherits="Municipalities.RegistrationNew" %>
<%@ Import Namespace="System.ServiceModel.Security" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    	<!-- Load Javascript -->
	<script type="text/javascript" src="./js/jquery.js"></script>
	<script type="text/javascript" src="./js/jquery.query-2.1.7.js"></script>
	<script type="text/javascript" src="./js/rainbows.js"></script>
	<!-- // Load Javascipt -->

	<!-- Load stylesheets -->
	<link type="text/css" rel="stylesheet" href="css/styleNew.css" media="screen" />
	<!-- // Load stylesheets -->
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    <div id="wrapper">
		<div id="wrappertop"></div>

		<div id="wrappermiddle">

			<h2>Регистрација</h2>

			<div id="username_input">

				<div id="username_inputleft"></div>

				<div id="username_inputmiddle">
				   <asp:TextBox ID="txtUsername" runat="server" BorderStyle="None"></asp:TextBox>
<%--				    <input type="text" name="inpNickname" id="inpNickname" value="E-mail Address" onclick="this.value = ''" runat="server"/>--%>
				    <img id="url_user" src="./images/mailicon.png" alt=""/>
			
				</div>

				<div id="username_inputright"></div>

			</div>

			<div id="password_input">

				<div id="password_inputleft"></div>

				<div id="password_inputmiddle">
                    <asp:TextBox ID="txtPassword" runat="server" BorderStyle="None"  TextMode="password"></asp:TextBox>
<%--				    <input type="password" name="inpPassword" id="inpPassword" value="Password" onclick="this.value = ''" runat="server"/>--%>
				    <img id="url_password" src="./images/passicon.png" alt=""/>
			
				</div>

				<div id="password_inputright"></div>

			</div>

			<div id="submit">
			<asp:ImageButton id="imagebutton1" runat="server"          
           ImageUrl="./images/registration.png" OnClick="imagebutton1_Click"
           />
				    <%--<input type="image" src="./images/submit.png" id="submit1" value="Sign In" runat="server" onclick="importUser()"/>--%>
				
			
			</div>

<div id="links_left">

    <asp:Label ID="Label1" runat="server" Text="Label" Visible="false">корисничкото име е зафатено</asp:Label>

			</div>
		

            <div id="links_right"><a href="AdminAgain.aspx">Кон страната за најава</a></div>
		</div>

		<div id="wrapperbottom">
		    
            <asp:Label ID="Label2" runat="server" Text="Label" Visible="False">Успешна најава</asp:Label>

		</div>
		
		
	</div>
   
    	
</asp:Content>
