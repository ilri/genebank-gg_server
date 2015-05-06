<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="display.aspx.cs" Inherits="GrinGlobal.Web.display" MasterPageFile="Site1.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <table><tr><td><h1><%= HeaderText %></h1></td><td>&nbsp; &nbsp; &nbsp; &nbsp; <asp:Button ID="btnDownload" runat="server" Text="Export Data to Excel" onclick="btnDownload_Click" /></td>
    </tr></table><asp:GridView ID="gv1" runat="server" HeaderStyle-BackColor="Silver"
    AlternatingRowStyle-CssClass="altrow" Font-Names="Arial" ></asp:GridView>
    
<br />
<hr /> 
</asp:Content>
