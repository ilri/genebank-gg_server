<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="taxonomysearch.aspx.cs" Inherits="GrinGlobal.Web.taxon.taxonomysearch" MasterPageFile="~/Site1.Master" Title="Advanced Query of Species Data"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Panel ID="pnlSearch" runat="server">
    <br /><center><b>Advanced Query of GRIN TAXONOMY Species Data</b></center>
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
    Continent:<asp:DropDownList ID="ddlContinent" DataTextField="name" DataValueField="id"
        runat="server" onselectedindexchanged="ddlContinent_SelectedIndexChanged" 
        AutoPostBack="True">
    </asp:DropDownList> &nbsp; &nbsp;
    Region:<asp:DropDownList ID="ddlRegion" DataTextField="subcontinent" DataValueField="region_id" runat="server">
        <asp:ListItem Value="0">ALL REGIONS</asp:ListItem>
    </asp:DropDownList><br /><br />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Country(ies):<asp:ListBox ID="lstCountry" DataTextField="countryname" 
        DataValueField="countrycode" runat="server" Rows="5" AutoPostBack="True" 
        onselectedindexchanged="lstCountry_SelectedIndexChanged" 
        SelectionMode="Multiple"></asp:ListBox>&nbsp; &nbsp;
    State/Province:<asp:DropDownList ID="ddlState" DataTextField="statename" DataValueField="gid" runat="server" Visible="False">
    </asp:DropDownList>
        <asp:TextBox ID="txtState" runat="server"></asp:TextBox><asp:Label ID="lblState" runat="server"
            Text="(e.g. Alabama)"></asp:Label><br />
<br />
<table><tr><td>
<b> Non-native distribution: </b>(entry as</td><td>
        <asp:RadioButtonList ID="rblNonNative" runat="server" 
            RepeatDirection="Horizontal" Font-Bold="True">
            <asp:ListItem Selected="True" Value="OrNon">Or </asp:ListItem>
            <asp:ListItem Value="AndNon">And</asp:ListItem>
        </asp:RadioButtonList></td>
        <td> criterion) </td><td><asp:TextBox ID="txtNon" runat="server" Width="200px" Wrap="False" ToolTip="Performs string search on distribution comment field. Please separate multiple terms by commas."></asp:TextBox></td><td>
    (e.g. cultivated, naturalized, Africa, United States, Macaronesia)</td></tr></table><br />
    <asp:CheckBox ID="cbAccepted" runat="server" /><b>Restrict to only accepted names</b><br />
    <asp:CheckBox ID="cbGerm" runat="server" /><b>Restrict to names with germplasm in GRIN</b>
<br /><br />

      <asp:Button ID="btnSearch" runat="server" Text="Search" 
            onclick="btnSearch_Click" /><br /><br />
    </asp:Panel>        
    <asp:Panel ID="pnlResult" runat="server">

<br /><center><b>Species Nomenclature in the database</b></center><br />
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