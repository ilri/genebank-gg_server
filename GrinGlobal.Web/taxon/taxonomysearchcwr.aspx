<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="taxonomysearchcwr.aspx.cs" Inherits="GrinGlobal.Web.taxon.taxonomysearchcwr" MasterPageFile="~/Site1.Master" Title="Crop Wild Relative" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Panel ID="pnlSearch" runat="server">
    <br /><center><b>Query Crop Relatives in GRIN</b></center>
<hr />
    Any or all fields can be searched. Wild cards (*) are accepted. Multiple values 
    could be selected from list boxes by using shift or control key.
        <br />
        <br />
        <b>Crop:</b>
        <asp:ListBox ID="lstCrop" runat="server" SelectionMode="Multiple" DataTextField="title" DataValueField="value" Rows="6"></asp:ListBox>
        <br />
        <br />
        <b>Genus&nbsp; name: </b>
        <asp:TextBox ID="txtSearch" runat="server" Width="245px"></asp:TextBox>
        (e.g. Oryza [without author])<br />
        Note: Only returns CWR in that genus. Select by crop to return all CWR of its 
            crops. 
        <br />
        <br />
        <b>Genetic relative status:</b>
        &nbsp;&nbsp;&nbsp;<asp:CheckBox ID="cbPrimary" runat="server" Checked="True" 
            ToolTip="Taxa that cross readily with the crop (or can be predicted to do so based on their taxonomic or phylogenetic relationships), yielding (or being expected to yield) fertile hybrids with good chromosome pairing, making gene transfer through hybridization simple." 
            Text="primary" ForeColor="Red" />
        &nbsp;&nbsp;&nbsp;<asp:CheckBox ID="cbSecondary" runat="server" Checked="True" 
            Text="secondary" 
            ToolTip="Taxa that will successfully cross with the crop (or can be predicted to do so based on their taxonomic or phylogenetic relationships), but yield (or would be expected to yield) partially or mostly sterile hybrids with poor chromosome pairing, making gene transfer through hybridization difficult." 
            ForeColor="Red" />
        &nbsp;&nbsp;&nbsp;<asp:CheckBox ID="cbTertiary" runat="server" Checked="True" 
            Text="tertiary" 
            ToolTip="Taxa that can be crossed with the crop (or can be predicted to do so based on their taxonomic or phylogenetic relationships), but hybrids are (or are expected to be) lethal or completely sterile. Special breeding techniques, some yet to be developed, are required for gene transfer." 
            ForeColor="Red" />
        &nbsp;&nbsp;&nbsp;<asp:CheckBox ID="cbGratfstock" runat="server" Checked="True" 
            Text="graftstock" 
            ToolTip="Taxa used as rootstocks for grafting scions of a crop, or used as genetic resources in the breeding of such rootstocks." 
            ForeColor="Red" />
        <br />
        <br />
        <b>Family(ies):</b> 
    <asp:ListBox ID="lstFamily" runat="server" SelectionMode="Multiple" 
        DataTextField="name" DataValueField="id" Rows="6"></asp:ListBox>
<br /><br />  
<b>Native distribution:</b><br />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Continent:<asp:DropDownList ID="ddlContinent" DataTextField="name" DataValueField="id"
        runat="server" onselectedindexchanged="ddlContinent_SelectedIndexChanged" 
        AutoPostBack="True">
    </asp:DropDownList> &nbsp; &nbsp;
    Region:<asp:DropDownList ID="ddlRegion" DataTextField="subcontinent" DataValueField="region_id" runat="server">
        <asp:ListItem Value="0">ALL REGIONS</asp:ListItem>
    </asp:DropDownList><br /><br />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Country(ies):<asp:ListBox ID="lstCountry" DataTextField="countryname" 
        DataValueField="countrycode" runat="server" Rows="5" AutoPostBack="True" 
        onselectedindexchanged="lstCountry_SelectedIndexChanged" 
        SelectionMode="Multiple"></asp:ListBox>&nbsp; &nbsp;
    State/Province:<asp:DropDownList ID="ddlState" DataTextField="statename" DataValueField="gid" runat="server" Visible="False">
    </asp:DropDownList>
        <asp:TextBox ID="txtState" runat="server"></asp:TextBox><asp:Label ID="lblState" runat="server"
            Text="(e.g. Alabama)"></asp:Label><br />
<br />
        <b><asp:CheckBox ID="cbNonnative" runat="server" Text="Include non-native distribution" /></b><br /> 
    &nbsp;<b>Restrict to crops maintained at these NPGS repositories </b><asp:ListBox ID="lstRepository" runat="server" SelectionMode="Multiple" DataTextField="title" DataValueField="value"></asp:ListBox><br />
    <asp:CheckBox ID="cbGerm" runat="server" /><b>Restrict to names with germplasm in GRIN</b><br />
    <asp:CheckBox ID="cbNonGerm" runat="server" /><b>Restrict to names without germplasm in GRIN</b> 
<br /><br />
      <asp:Button ID="btnSearch" runat="server" Text="Search" 
            onclick="btnSearch_Click" /><br /><br />
    </asp:Panel>        
    <asp:Panel ID="pnlResult" runat="server">
<br /><center><b>Crop Relatives in GRIN Taxonomy</b></center>
        <br />
        <center><asp:Label ID="lblCriteria" runat="server" Text="" Width="990px"></asp:Label></center> 
        <br />
        Follow links for a) <b>GRIN taxon reports</b> or  b) <b>to view literature supporting this gene pool classification</b> (Place cursor over highlighted items for explanation.) <br /><br />
    <asp:PlaceHolder ID="phData" runat="server"></asp:PlaceHolder>
    </asp:Panel><br />
</asp:Content>