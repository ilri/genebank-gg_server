<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="maps.aspx.cs" Inherits="GrinGlobal.Web.Maps" MasterPageFile="~/Site1.Master" %>

<%@ Register src="mapscontrol.ascx" tagname="MapsControl" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <h1>
        <asp:Label ID="lblHeading" runat="server" Text="Accessions for"></asp:Label>
        <asp:Label ID="lblTaxonomy" runat="server" Text="Taxonomy"></asp:Label>
    </h1>
    <asp:Label ID="lblPINumber" runat="server" Text="PI"></asp:Label>
    
    <hr />
    <uc1:MapsControl ID="mc1" runat="server" />
    <hr />

<%= Page.DisplayText("htmlKeySymbols", "Key to symbols")%> <sup>1</sup>
    <table runat="server" id="category" 
        style="border-style: double; border-width: thin; width: 37%; height: 98px;" 
        >
        <tr>
            <td>
                <img src="images/arrow.png" style="height: 19px; width: 24px" />
            </td>
            <td >
                <asp:Label ID="lblIDinside" runat="server" Text="PI ID"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <img src="images/mm_20_yellow.png" style="margin-left: 0px" />
            </td>
            <td>
                1 accession
            </td>
        </tr>
        <tr>
            <td>
                <img src="images/mm_20_orange.png" />
            </td>
            <td>
                2-5 accessions
            </td>
        </tr>
        <tr>
            <td>
                <img src="images/mm_20_red.png" />
            </td>
            <td>
                6-10 accessions
            </td>
        </tr>
        <tr>
            <td>
                <img src="images/mm_20_purple.png" /></td>
            <td>
                11-100 accessions
            </td>
        </tr>
            <tr>
            <td>
                <img src="images/mm_20_blue.png" />
            </td> 
            <td>
               > 100 accessions
            </td>
        </tr>
    </table>
<hr />
<p><sup>1</sup><font size="-1"></font>&nbsp For small species each accession is mapped and can be clicked to show the collection site. For mid- or large-sized species, the points are aggregated and they can be clicked to show the total number of accessions collectd from the site. </p>

    
</asp:Content>