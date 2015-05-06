<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="famgensearch.aspx.cs" Inherits="GrinGlobal.Web.taxon.famgensearch" MasterPageFile="~/Site1.Master" Title="Families and Genera"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Panel ID="pnlSearch" runat="server">
    <br /><center><font size="3"><b>Families and Genera in</b></font><b> GRIN TAXONOMY </b></center>
<hr />
        Any or all fields can be searched. Wild cards (*) are accepted.
<br /><br />
    
        <asp:CheckBox ID="cbpathog" runat="server" Font-Bold="True" 
            Text="Include miscellaneous pathogens" /><br />
        <asp:CheckBox ID="cbferns" runat="server" Font-Bold="True" 
            Text="Include pteridophytes" Checked="True" /><br />
        <asp:CheckBox ID="cbgymno" runat="server" Font-Bold="True" 
            Text="Include gymnosperms" Checked="True" /><br />
        <asp:CheckBox ID="cbangio" runat="server" Font-Bold="True" 
            Text="Include angiosperms" Checked="True" /><br /><br /><br />
         
         <b>Family name:</b> 
        <asp:TextBox ID="txtFamily" runat="server"></asp:TextBox>
        ( Family list: <asp:DropDownList ID="ddlFamily" DataTextField="name" DataValueField="id" runat="server"></asp:DropDownList> )<br />
        &nbsp;&nbsp;&nbsp;<asp:CheckBox ID="cbinfrafam" runat="server" 
            Text="Exclude infrafamilial names" Checked="True" /><br /><br />
            
        <b>Genus name:</b> 
        <asp:TextBox ID="txtGenus" runat="server"></asp:TextBox> (e.g. <i>Zizania</i> )<br />
        &nbsp;&nbsp;&nbsp;<asp:CheckBox ID="cbinfragen" runat="server" 
            Text="Exclude infrageneric names" Checked="True" /><br /><br /><br />
            
         <asp:CheckBox ID="cbAccept" runat="server" Font-Bold="True" 
            Text="Restrict to just accepted names" /><br /><br /><br />
       
        <asp:Button ID="btnSearch" runat="server" Text="Search" 
            onclick="btnSearch_Click" /><br /><br />
   
    </asp:Panel>        
    <asp:Panel ID="pnlResult" runat="server">

<br /><center><b>
    <asp:Label ID="lblFamGen" runat="server" Text=""></asp:Label></b></center><br />
    <center><asp:Label ID="lblCriteria" runat="server" Text=""></asp:Label></center> 
<br />
   <asp:Repeater ID="rptResult" runat="server">
    <HeaderTemplate>
        <ul style="list-style-type: decimal">
    </HeaderTemplate> 
    <ItemTemplate>
       <li><%# Eval("linktext") %>  </li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>

    </asp:Panel><br />
</asp:Content>