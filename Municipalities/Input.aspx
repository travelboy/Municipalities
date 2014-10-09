<%@ Page Title="" Language="C#" MasterPageFile="~/Municipalities.Master" AutoEventWireup="true" CodeBehind="Input.aspx.cs" Inherits="Municipalities.Input" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width:700px ; float:right">
        <tr>
            <td><p style="color: skyblue;
font-family: monospace;"> Внесете информации за нова општина. </p></td>
        </tr>

  <tr>
       <td> <p>Име на општина:  </p> </td>  

       
          
            <td>   <div><asp:TextBox ID="txtNameMunicipalities" runat="server"></asp:TextBox>
          <asp:Label ID="lblExist" runat="server" Text="Постои општина со тоа име!" Visible="False"></asp:Label>
    </div> </td>    
   
   
    </tr>
    <tr><td>
          <p>Линк кон Facebook страна:   </p>  
        </td>
        <td>
               <asp:TextBox ID="txtFBLink" runat="server"></asp:TextBox>
        </td>
    </tr>
   
         <tr><td>
        <p>Линк кон Twitter страна:  </p>  
        </td>
        <td>
              
        <asp:TextBox ID="txtTwitterLink" runat="server"></asp:TextBox>
        </td>
    </tr>
   
    
         <tr><td>
        <p>Линк кон YouTube канал(Username):   </p>  
        </td>
        <td>
              
        <asp:TextBox ID="txtYTLink" runat="server"></asp:TextBox>
        </td>
    </tr> 
      <tr><td>
       <asp:Button ID="btnInput" runat="server" Text="Внеси" OnClick="btnInput_Click" />  
        </td>
        <td>
              
       
        </td>
    </tr> 
 
  </table>
    
            <div id="crawler" style="float: left;
width: 600px;
padding-left: 90px;">
            
    <p style="color: skyblue;
font-family: monospace;"> Тука можете да ја обновите базата на податоци со најновите информации од социјалните медиуми. </p>
      <p style="color: red;
font-size: x-small;"> Поради можната преоптовареност на серверите, ве молиме вршете парцијално обновување на податоците!  </p>
    </br>
        <asp:CheckBox ID="cbFB" runat="server" Text="обнови ги Facebook податоците" />
    </br>
         <asp:CheckBox ID="cbTW" runat="server" Text="обнови ги Twitter податоците" />
    </br>
         <asp:CheckBox ID="cbYT" runat="server" Text="обнови ги YouTube податоците "/>
   </br>
    <asp:Button ID="Button1" runat="server" Text="Обнови" OnClick="Button1_Click" />
    </div>
    </br>
    
           <div id="Div1" style="width: 600px;
padding-left: 90px;">
            
    <p style="color: skyblue;
font-family: monospace;"> Избришете општина од базата на податоци. </p>
    <asp:DropDownList ID="DropDownList1" runat="server" >
            </asp:DropDownList>
               </br>
    <asp:Button ID="Button2" runat="server" Text="Избриши" OnClick="Button2_Click" />
    </div>
</asp:Content>
