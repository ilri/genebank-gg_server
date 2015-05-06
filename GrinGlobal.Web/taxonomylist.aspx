<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="taxonomylist.aspx.cs" Inherits="GrinGlobal.Web.TaxonomyList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">

<h1><asp:Label ID="lblTitle" runat="server" Text=""></asp:Label></h1>
<br />
 
<asp:Repeater ID="rptRecordlist" runat="server">
    <HeaderTemplate>
        <h1></h1>
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li><b><%# Eval("name")%></li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>   
    
</asp:Content>