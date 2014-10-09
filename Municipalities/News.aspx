
<%@ Page Title="" Language="C#" MasterPageFile="~/Municipalities.Master" AutoEventWireup="true" CodeBehind="News.aspx.cs" Inherits="Municipalities.News" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    	 

		  	    
                  
                  <!--- NEWS PART ---->
		  	   
		  	       <asp:Repeater ID="rptNews" runat="server"><ItemTemplate>
		  	   	<div class="blog_grid">
		  	   	   <h2 class="post_title"><%#Eval("FBName") %></h2>
		  	   	   <img src='<%# Eval("FBpicture") %>'  alt=""height="250px" width="250px"/>
		  	   	    <p><%#Eval("FBmessage") %></p>
                        <a href='<%#Eval("FBlink") %> '>Линк кон веста </a> 
                           <p>Објавено на <%#Eval("FBday") %>/<%#Eval("FBmonth") %>/<%#Eval("FByear") %></p>
		  	       
		  	     </div>
        </ItemTemplate>   </asp:Repeater>
		  	   
		  
		 
</asp:Content>
