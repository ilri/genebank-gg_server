<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="taxonomyliterature.aspx.cs" Inherits="GrinGlobal.Web.taxon.taxonomyliterature" Title="Literature citations for crop relative gene pool assignment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<link href="~/styles/default.css" rel="stylesheet" type="text/css" />
<title>Literature citations for crop relative gene pool assignment</title></head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Panel ID="pnlReference" runat="server">
        <asp:Repeater ID="rptReference" runat="server">   
        <HeaderTemplate>
            <center><h1><%= Page.DisplayText("htmlTaxonLiterature", "Literature References for GRIN Taxonomy Crop Relative Gene Pool Assignment")%></h1></center> </br/>
            <asp:Label ID="Label1" runat="server" Text="Taxon" Font-Bold="True"></asp:Label><br /><br />
        </HeaderTemplate>
        <ItemTemplate>
                <li>
                <%# Eval("author")%>&nbsp; <%# Eval("citation_year")%>.&nbsp;<%# Eval("title")%> &nbsp; <%# Eval("abbrev")%> &nbsp; <%# Eval("reference")%> &nbsp: [<%# Eval("cit_note")%>]
                </li>
         </ItemTemplate>
        </asp:Repeater>
        </asp:Panel>
    </div>
    </form>
</body>
</html>
