<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pingsearchengine.aspx.cs" Inherits="GrinGlobal.Web.pingsearchengine" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
Successfully pinged the search engine.<br />
BindingType = <%= GetBindingType() %><br />
BindingUrl = <%= GetBindingUrl() %><br />
</asp:Content>
