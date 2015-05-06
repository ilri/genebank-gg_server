<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="imagetester.aspx.cs" Inherits="GrinGlobal.Web.ImageTester" %>
<%@ Register Src="~/imagecontrol.ascx" TagPrefix="gg" TagName="Image" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <gg:Image runat="server" id="imagePreviewer"></gg:Image>
</asp:Content>
