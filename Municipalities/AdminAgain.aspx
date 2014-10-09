<%@ Page Title="" Language="C#" MasterPageFile="~/Municipalities.Master" AutoEventWireup="true" CodeBehind="AdminAgain.aspx.cs" Inherits="Municipalities.AdminAgain" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    
	<script type="text/javascript" src="./js/jquery.js"></script>
	<script type="text/javascript" src="./js/jquery.query-2.1.7.js"></script>
	<script type="text/javascript" src="./js/rainbows.js"></script>



	<link type="text/css" rel="stylesheet" href="css/styleNew.css" media="screen" />
	
 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   
    <div id="wrapper">
		<div id="wrappertop"></div>

		<div id="wrappermiddle">

			<h2>Најава</h2>

			<div id="username_input">

				<div id="username_inputleft"></div>

				<div id="username_inputmiddle">
				   <asp:TextBox ID="txtUsername" runat="server" BorderStyle="None"></asp:TextBox>
				    <img id="url_user" src="./images/mailicon.png" alt=""/>
			
				</div>

				<div id="username_inputright"></div>

			</div>

			<div id="password_input">

				<div id="password_inputleft"></div>

				<div id="password_inputmiddle">
                    <asp:TextBox ID="txtPassword" runat="server" BorderStyle="None"  TextMode="password"></asp:TextBox>
				    <img id="url_password" src="./images/passicon.png" alt=""/>
			
				</div>

				<div id="password_inputright"></div>

			</div>

			<div id="submit">
			<asp:ImageButton id="imagebutton1" runat="server"          
           ImageUrl="./images/submit.png" OnClick="imagebutton1_Click"
           />
				
			
			</div>

<div id="links_left">

    <asp:Label ID="Label1" runat="server" Text="Label" Visible="false">корисникот веќе постои</asp:Label>

			</div>
		

			
			<div id="links_right"><a href="RegistrationNew.aspx">Регистрирајте се</a></div>

		</div>

		<div id="wrapperbottom"></div>
		
		
	</div>
   
    	
</asp:Content>
