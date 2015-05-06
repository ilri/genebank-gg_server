<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="view.aspx.cs" Inherits="GrinGlobal.Web.View" %>
<%@ Register TagPrefix="gg" TagName="PivotView" Src="~/pivotviewcontrol.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
<h1><%= HeaderText %></h1>
<gg:PivotView  
    ID="ggPivotView"
    runat="server"
    PageIndex="0"
    PageSize="25"
    AllowPivoting="false"
    AllowGrouping="false"
    AllowFilteringAutoComplete="false"
    OnLanguageChanged="PivotView_LanguageChanged"
></gg:PivotView>
<br />
</asp:Content>
