<%@ Page Title="Browse By Taxonomy" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="browse.aspx.cs" Inherits="GrinGlobal.Web.Browse" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type='text/javascript'>
    function toggleItem(tgt) {
        var el = document.getElementById(tgt);
        if (!el) { return; }
        
        if (el.style.display == 'block'){
            el.style.display = 'none';
            document.getElementById(tgt + 'Img').src = 'images/open.gif';
        } else {
            el.style.display= 'block';
            document.getElementById(tgt + 'Img').src = 'images/close.gif';
        }
        return false;
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <h2>Browse By Taxonomy</h2>
    <p>
        Group By: <asp:RadioButtonList id="rblViewBy" runat="server" AutoPostBack="true" 
            RepeatDirection="Horizontal"  RepeatLayout="Flow"
            onselectedindexchanged="rblViewBy_SelectedIndexChanged">
            <asp:ListItem Value="family" Text="Family" Selected="True"></asp:ListItem>
            <asp:ListItem Value="genus" Text="Genus"></asp:ListItem>
            <asp:ListItem Value="species" Text="Species"></asp:ListItem>
        </asp:RadioButtonList>
        </p>
    <p>
        Order By: <asp:RadioButtonList ID="rblOrderBy" runat="server" AutoPostBack="true" RepeatDirection="Horizontal" RepeatLayout="Flow">
            <asp:ListItem Value="name" Text="Name" Selected="True"></asp:ListItem>
            <asp:ListItem Value="accessioncount" Text="Accession Count"></asp:ListItem>
        </asp:RadioButtonList>
        </p>
    <%= OutputTreeViewHtml() %>
</asp:Content>
