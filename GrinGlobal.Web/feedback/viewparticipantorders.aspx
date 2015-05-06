<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="viewparticipantorders.aspx.cs" Inherits="GrinGlobal.Web.feedback.viewparticpantorders" %>
<%@ Register TagPrefix="gg" TagName="PivotView" Src="~/pivotviewcontrol.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
<h1>My Orders</h1>
<gg:PivotView  
    ID="ggPivotView"
    runat="server"
    PageIndex="0"
    PageSize="25"
    AllowPivoting="false"
    AllowGrouping="true"
    PrimaryKeyName="order_request_id"
    AlternateKeyName="order_request_id"
    AllowFilteringAutoComplete="false"
    OnLanguageChanged="PivotView_LanguageChanged"
></gg:PivotView>
<br />
</asp:Content>
