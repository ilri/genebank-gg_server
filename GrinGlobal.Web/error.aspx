<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="error.aspx.cs" Inherits="GrinGlobal.Web.Error" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
<h2>We're sorry, your last request caused an error on the server.</h2>
<p>
    Error details have been logged on the server.  Please contact your system administrator if this error
    continues to occur.
</p>
<asp:Panel ID="pnlError" runat="server">
    <a href="#" onclick="javascript:$('#divError').toggle('fast');">View Error Detail</a>
    <br />
    <div id="divError" style='display:none'>
        <h3><%= ErrorMessage %></h3>
        <p>
            <%= ErrorString %>
        </p>
    </div>
<a href="javascript:location.reload();">Retry your last request</a>
</asp:Panel>
</asp:Content>
