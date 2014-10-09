<%@ Page Title="" Language="C#" MasterPageFile="~/Municipalities.Master" AutoEventWireup="true" CodeBehind="Activities.aspx.cs"  Inherits="Municipalities.Activities" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
 
    

            <div class="container">
            <div class="row">
                  
            <img src="images/FacebookBanner.png" style="width: 780px; padding-left: 30px;"  />
            <!--- NEWS PART ---->
                  
            <div id="grid-gallery" class="grid-gallery" style="width: 997px;">
            <section class="grid-wrap">
            <ul class="grid">
            <asp:Repeater ID="rptFB" runat="server"><ItemTemplate>
						<li class="grid-sizer"></li><!-- for Masonry column width -->
						<li>
							<figure>
								<a href='News.aspx?id=<%#Eval("FBNewsId") %>'><img src='<%# Eval("FBpicture") %>' alt="no picture display"  /></a>
								<figcaption>
								    
                                    <h3><%#Eval("FBName") %></h3>
                                     <h6>Објавено на <%#Eval("FBday") %>/<%#Eval("FBmonth") %>/<%#Eval("FByear") %></h6> 

								</figcaption>
							</figure>
						</li>
                               </ItemTemplate>   </asp:Repeater>
            </ul>
            </section>
            <section class="slideshow">
            <asp:Repeater ID="rptFB1" runat="server"><ItemTemplate>
					<ul>
						<li>
							<a href='News.aspx?id=<%#Eval("FBNewsId") %>'>
							   
                                <img src='<%# Eval("FBpicture") %>' alt="no picture display"  />
							</a>
								<figcaption>
								    
                                    <h3><%#Eval("FBName") %></h3>
                                     <h6>Објавено на <%#Eval("FBday") %>/<%#Eval("FBmonth") %>/<%#Eval("FByear") %></h6> 

								</figcaption>
						</li>
                        </ul>
             </ItemTemplate>   </asp:Repeater>
            </section>
            </div>
                  
                  
                  
                  
                  
                  
                  
                  
	
                                     
            <!---------pagging----------->
            <div style="clear: both;"></div>
       
            <div style="width:820px; margin-bottom:3%; height:60px; margin-top:3%;">    </div>
                       
            <div style="margin:0 auto; width:560px; margin-bottom:20px;">
          
                  <div style="width:100px; height:50px;float:left">
                      <asp:LinkButton ID="lnkFirst" runat="server" CssClass="" OnClick="firstClicked"><img src="images/first.png" />

                      </asp:LinkButton>
                  </div>

            <div style="width:100px; height:50px;float:left">
                <asp:LinkButton ID="lnkPrevious" runat="server" CssClass="" OnClick="lnkLeftClicked"><img src="images/back.png" />

                </asp:LinkButton>
            </div>
        <!--za prikaz na stranite!-->
            <div style="width:175px; height:50px;float:left;  font-family:Arial; font-size:20px;">
                <asp:Label ID="lblpage" runat="server" Text=""></asp:Label>
            </div>

            <div style="width:144px; height:50px;float:right; align:right;">
                 <asp:LinkButton ID="lnkLast" runat="server" CssClass="" OnClick="lastClicked"><img src="images/last.png" style="padding-left:5px;" />

                 </asp:LinkButton>
            </div>
            <div style=" height:80px;float:right">
                <asp:LinkButton ID="lnkNext" runat="server" CssClass="" OnClick="lnkRightClicked"><img src="images/next.png" />

                </asp:LinkButton>
            </div>

            </div>
              
            <div style="width:200px; margin:20px auto; font-family:Arial; padding-top:20px;"></div>  
            <!---------end pagging----------->
                     


            </div>
                  
            <!--- SIDEBAR ---->

            <div class="col-md-5 blog_sidebar">
            <ul class="sidebar">
           
            </ul>
            <h4 class="archive">Посетете ги !</h4>
            <a href='TwitterActivities.aspx' ><img src='images/twitterbanner.jpg' class="img-responsive" alt="" height="300px" width="560px"/></a>
            <br/>
                     
            <a href='YTActivities.aspx' ><img src='images/ytbanner.png' class="img-responsive" alt="" height="250px" width="550px"/></a>
            </br>
                

            </div>
            </div>
    
    

            <script src="js/imagesloaded.pkgd.min.js">
            </script>
		<script src="js/masonry.pkgd.min.js"></script>
		<script src="js/classie.js"></script>
		<script src="js/cbpGridGallery.js"></script>
		<script>
		    new CBPGridGallery(document.getElementById('grid-gallery'));
		</script>
    

</asp:Content>
