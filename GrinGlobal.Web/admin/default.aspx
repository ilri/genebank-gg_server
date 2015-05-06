<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="GrinGlobal.Web.admin._default" validateRequest="false" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
<br />
<asp:Button ID="btnClearCache" runat="server" Text="Clear Cache" 
        onclick="btnClearCache_Click" /> <br /><br />
        
<asp:Button ID="btnEncryptConnection" runat="server" Text="Encrypt Connection String" 
        onclick="btnEncryptString_Click" Width="164px" Enabled="False" /> <br /><br />
        
<asp:TextBox ID="tb1" runat="server" Width="800px"></asp:TextBox>
    <br />
<asp:Button ID="btnEncryptText" runat="server" Text="Encrypt Above Text" onclick="btnEncryptText_Click" /> <br />
<asp:TextBox ID="tb2" runat="server" Width="800px"></asp:TextBox>
<br /><br />  
</asp:Content>
