<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeBehind="literature.aspx.cs" Inherits="GrinGlobal.Web.Literature" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<link href="~/styles/default.css" rel="stylesheet" type="text/css" />
<title>Literature</title></head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Panel ID="pnlLiterature" runat="server">
        <asp:Repeater ID="rptLiterature" runat="server">   
        <HeaderTemplate>
            <h1><%= Page.DisplayText("htmlLiterature", "Literature")%></h1>
        </HeaderTemplate>
        <ItemTemplate>
             <b><%= Page.DisplayText("htmlLitYear", "Year of publication")%>:</b> &nbsp; <%# Eval("created_date", "{0:yyyy}") %><br />
             <b><%= Page.DisplayText("htmlLitEditor", "Author or Editor")%>:</b> &nbsp; <%# Eval("editor_author_name") %> <br />
             <b><%= Page.DisplayText("htmlLitAbbr", "GRIN abbreviation")%>:</b> &nbsp; <%# Eval("abbreviation") %> <br />
        </ItemTemplate>
        </asp:Repeater>
        </asp:Panel>

        <asp:Panel ID="pnlCitation" runat="server">
        <h1><%= Page.DisplayText("htmlCitationAccession", "List of Accessions included in the following publication:")%></h1>
        <asp:Label ID="lblCitation" runat="server" Text=""></asp:Label><br /><br />
        <asp:Repeater ID="rptCitation" runat="server">
            <ItemTemplate>
               &nbsp;<%# Eval("cnt")%>. &nbsp; <%# Eval("pi_number")%> &nbsp; <%# Eval("name")%> &nbsp; <%# Eval("top_name")%> <br />
            </ItemTemplate>
        </asp:Repeater>
        </asp:Panel>
        <br /><br /><br />
        <hr />
        <%--<asp:HyperLink runat='server' ID="btnPrevious" ImageUrl="~/images/btn_prevpage.gif"></asp:HyperLink><br /><br />--%>
    </div>
    </form>
</body>
</html>
