<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cropmarkerview.aspx.cs" Inherits="GrinGlobal.Web.cropmarkerview" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
<br />
    <asp:GridView ID="gvData" runat="server"  HeaderStyle-BackColor="Silver"
    AlternatingRowStyle-CssClass="altrow" > 
    </asp:GridView>
<br />
<hr /> 
</asp:Content>