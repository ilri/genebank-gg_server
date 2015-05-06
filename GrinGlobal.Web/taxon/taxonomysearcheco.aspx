<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="taxonomysearcheco.aspx.cs" Inherits="GrinGlobal.Web.taxon.taxonomysearcheco" MasterPageFile="~/Site1.Master" Title="World Economic Plants"%>
<%@ Register Src="ecoclasscontrol.ascx" TagPrefix="gg" TagName="SearchCriteria" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Panel ID="pnlSearch" runat="server">
    <br /><center><b>World Economic Plants in GRIN</b></center> 
 <center>(based on <a title="Link to 'WORLD ECONOMIC PLANTS: A Standard Reference' webpage" href="http://www.ars-grin.gov/~sbmljw/cgi-bin/wep.pl" target="_blank"><i>WORLD ECONOMIC PLANTS: A Standard Reference</i></a>)</center>
<hr />
    Any or all fields can be searched. Wild cards (*) are accepted. Multiple values 
    could be selected from list boxes by using shift or control key.
<br /><br />
<b>Genus or species name: </b>
<asp:TextBox ID="txtSearch" runat="server"></asp:TextBox> (e.g. <i>Arachis</i> or <i>
    Zea Mays</i> [without author])
<br /><br />
<b>Family(ies):</b> 
    <asp:ListBox ID="lstFamily" runat="server" SelectionMode="Multiple" 
        DataTextField="name" DataValueField="id" Rows="6"></asp:ListBox>
<br /><br />  
<b>Common name:</b> 
    <asp:TextBox ID="txtCommon" runat="server"></asp:TextBox> (e.g. maize [no diacritics])
<br /><br />
<b>Native distribution:</b>
<%--    Continent:<asp:DropDownList ID="ddlContinent" DataTextField="name" DataValueField="id"
        runat="server" onselectedindexchanged="ddlContinent_SelectedIndexChanged" 
        AutoPostBack="True">
    </asp:DropDownList> &nbsp; &nbsp;
    Region:<asp:DropDownList ID="ddlRegion" DataTextField="subcontinent" DataValueField="region_id" runat="server">
        <asp:ListItem Value="0">ALL REGIONS</asp:ListItem>
    </asp:DropDownList> <br />--%><br />
   &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Country(ies):<asp:ListBox ID="lstCountry" DataTextField="countryname" 
        DataValueField="countrycode" runat="server" Rows="5" AutoPostBack="True" 
        onselectedindexchanged="lstCountry_SelectedIndexChanged" 
        SelectionMode="Multiple"></asp:ListBox>&nbsp; &nbsp;
    State/Province:<asp:DropDownList ID="ddlState" DataTextField="statename" DataValueField="gid" runat="server" Visible="False">
    </asp:DropDownList>
        <asp:TextBox ID="txtState" runat="server"></asp:TextBox><asp:Label ID="lblState" runat="server"
            Text="(e.g. Alabama)"></asp:Label><br />
<br />
    <asp:CheckBox ID="cbAccepted" runat="server" /><b>Include synonyms</b><br />
    <asp:CheckBox ID="cbGerm" runat="server" /><b>Restrict to names with germplasm in GRIN</b>
<br /><br />

<b> Select economic impact  <a title="Link to explanation of GRIN economic plant data"  href="http://www.ars-grin.gov/~sbmljw/cgi-bin/wep.pl?chapter=econ1" target="_blank">classes and subclasses</a>.</b> 
<br />
Selections will be linked by default to the more restrictive "<b>AND</b>" (<i>both</i> 1st [Sub]Class <b>AND</b> 2nd [Sub]Class). <br />
Use the less restrictive "<b>OR</b> " (<i>either</i> 1st [Sub]Class <b>OR</b> 2nd [Sub]Class) by checking the appropriate box(es) immediately below.<br />
 <asp:CheckBox ID="cbClass" runat="server" /><b>"OR" Classes</b>  &nbsp;&nbsp;&nbsp;&nbsp;<asp:CheckBox ID="cbSubclass" runat="server" /><b>"OR" Subclass</b> 

    <asp:PlaceHolder ID="ph1" runat="server">
        <gg:SearchCriteria runat="server" id="searchItem1" Sequence="1"></gg:SearchCriteria>
    </asp:PlaceHolder>
    <asp:Panel ID="pnl2" runat="server" Visible = "false">
        <gg:SearchCriteria runat="server" id="searchItem2"></gg:SearchCriteria>             
    </asp:Panel>
    <asp:Panel ID="pnl3" runat="server" Visible = "false">
        <gg:SearchCriteria runat="server" id="searchItem3"></gg:SearchCriteria>             
    </asp:Panel>
    <asp:Panel ID="pnl4" runat="server" Visible = "false">
        <gg:SearchCriteria runat="server" id="searchItem4"></gg:SearchCriteria>             
    </asp:Panel>
    <asp:Panel ID="pnl5" runat="server" Visible = "false">
        <gg:SearchCriteria runat="server" id="searchItem5"></gg:SearchCriteria>             
    </asp:Panel>
    <br />
    <asp:Button ID="btnMore" runat="server" onclick="btnMore_Click" 
            Text="Add More Classes" UseSubmitBehavior="False" /> &nbsp; &nbsp; &nbsp; 
        <asp:Button ID="btnClear" runat="server" onclick="btnClear_Click" 
            Text="Clear All Class" UseSubmitBehavior="False" /> <br /><br />

      <asp:Button ID="btnSearch" runat="server" Text="Search" 
            onclick="btnSearch_Click" /><br /><br />
    </asp:Panel>        
    <asp:Panel ID="pnlResult" runat="server">

<br /><center><b>World Economic Plants in the database</b></center><br />
<center><asp:Label ID="lblCriteria" runat="server" Text="" Width="990px"></asp:Label></center> 
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