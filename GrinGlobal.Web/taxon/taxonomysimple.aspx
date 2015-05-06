<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="taxonomysimple.aspx.cs" Inherits="GrinGlobal.Web.taxon.taxonomysimple" MasterPageFile="~/Site1.Master" Title="Simple Query Species Data"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type='text/javascript'>
        $(document).ready(function() {
            $(".searchBox").keypress(function(event) {
                var keycode = event.charCode || event.keyCode || 0;
                if (event.ctrlKey && keycode == 13) {
                    $("#<%= btnSearch.ClientID %>").click();
                }
            });

            $('._focusme').focus();

            $(".searchBox2").keypress(function(event) {
                var keycode = event.charCode || event.keyCode || 0;
                if (event.ctrlKey && keycode == 13) {
                    $("#<%= btnSearch2.ClientID %>").click();
                }
            });

            $(function() {
            $('input[type=text]').focus(function() {
                if ($(this).val() == 'New Search')
                    $(this).val('');
                });

            });

        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Panel ID="pnlSearch" runat="server">
<br /><center><b>Simple Query of GRIN TAXONOMY Species Data</b></center>
<hr />
Enter search criterion. Wild cards (*) are accepted: <asp:TextBox ID="txtSearch" runat="server" CssClass="_focusme"></asp:TextBox> &nbsp;&nbsp;&nbsp;&nbsp;
      <asp:Button ID="btnSearch" runat="server" Text="Search" 
            onclick="btnSearch_Click" />
<hr /><br />
You can search for any one of these identifiers:
	<ul>
	<li><b>Scientific name</b> (e.g. <i>Triticum aestivum</i> [without author]).</li>
	<li><b>Common name</b> (e.g. wheat [no diacritics].</li>
	<li><b>Genus name</b> (e.g. <i>Triticum</i>).</li>
	<li><b>Family name</b> (e.g. Poaceae).</li>
	<li><b>Species nomen number</b> (e.g. 40544).</li>
	<li><b>Country in species native range</b> (e.g. Zaire).</li>
	</ul><br /></asp:Panel>
<asp:Panel ID="pnlResult" runat="server">

<br /><center><b>Species records in the database</b></center><br />
<asp:Label ID="lblCriteria" runat="server" Text=""></asp:Label>
<asp:Button ID="btnSearch2" runat="server" Text="" class="right"
            onclick="btnSearch2_Click" BackColor="White" Width="1px" 
        BorderStyle="None" />
<div class="right">
    <asp:TextBox ID="txtSearch2" runat="server" Text="New Search" 
            onclick="btnSearch2_Click" ></asp:TextBox></div>
<br />
<%--    <asp:BulletedList ID="blResult" runat="server" DataTextField="name" DataValueField="species_authority" BulletStyle="Numbered">
    <asp:BulletedList ID="blResult" runat="server" BulletStyle="Numbered" 
        DisplayMode="HyperLink">
    </asp:BulletedList>--%>
   <asp:Repeater ID="rptResult" runat="server">
    <HeaderTemplate>
        <ul style="list-style-type: decimal">
    </HeaderTemplate> 
    <ItemTemplate>
       <li><%# Eval("linktext") %>  </li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>

    </asp:Panel><br />
</asp:Content>