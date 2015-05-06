<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="site.aspx.cs" Inherits="GrinGlobal.Web.site" MasterPageFile="Site1.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:DetailsView ID="dvSite" runat="server" AutoGenerateRows="false" DefaultMode="ReadOnly" CssClass='detail' GridLines="None">
    <FieldHeaderStyle CssClass="" />
    <HeaderTemplate>
        <h1> &nbsp; <%# Eval("site_long_name")%></h1>
    </HeaderTemplate>
    <EmptyDataTemplate>
       No site address data found
    </EmptyDataTemplate>
    <Fields>
        <asp:TemplateField>
            <ItemTemplate>
            <table id="Table1" runat="server" cellpadding='1' cellspacing='1' border='0'>
            <tr id="tr_organization">
                <td><%# Eval("organization") %></td>
            </tr>
            <tr id="tr_add1">
                 <td><%# Eval("address_line1")%></td>
            </tr>
            <tr id="tr_add2">
                 <td><%# Eval("address_line2")%></td>
            </tr>
            <tr id="tr_add3">
                 <td><%# Eval("address_line3")%></td>
            </tr>
            <tr id="tr_city">
                 <td><%# Eval("city") %></td>
            </tr>
            <tr><td><br /></td></tr>
            <tr id="tr_phone">
                 <td>Primary Phone: <%# Eval("primary_phone") %></td>
            </tr>
            <tr id="tr_phone2">
                 <td>Secondary Phone: <%# Eval("secondary_phone") %></td>
            </tr>
            <tr id="tr_fax">
                 <td>Fax: <%# Eval("fax") %></td> 
            </tr>
            <tr><td><br /></td></tr> 
            <tr id="tr_email">
                 <td>Email: <a href="mailto:<%# Eval("email") %>"><%# Eval("email")%></a></td>
            </tr><tr><td><br /></td></tr> 
            <tr id="tr_site">
                <td><a href="<%# Eval("note") %>" target='_blank'>Repository Home Page</a></td>
            </tr>       
            </table>
        </ItemTemplate>
        </asp:TemplateField>
    </Fields>
</asp:DetailsView>  
<br />
 <asp:Panel ID="pnlCoop" runat="server" Visible="false">
    <h1>&nbsp;&nbsp;Curator</h1>
    <asp:Repeater ID="rptCoop" runat="server">
        <ItemTemplate>
         &nbsp;&nbsp;<%# Eval("name")%> <%# Eval("note")%> <a href="mailto:<%# Eval("email") %>"><%# Eval("email")%></a> <br /> 
        </ItemTemplate>
    </asp:Repeater>
    </asp:Panel>
    &nbsp;
  <br />
 <asp:Panel ID="pnlCrop" runat="server" Visible="false">
 &nbsp;<a href='#' onclick='javascript:$("#divSiteCrop").toggle("fast");return false;' style='font-weight: bold;'><%= Page.DisplayText("htmlSiteCrop", "Crops evaluated:")%> </a>   
 <br />
 <div id='divSiteCrop' class='hide'>
    <asp:Repeater ID="rptCrop" runat="server">
     <HeaderTemplate><ul></HeaderTemplate>
        <ItemTemplate>
         <li><%# Eval("crop_name")%> </li>
        </ItemTemplate>
      <FooterTemplate></ul></FooterTemplate>
</asp:Repeater>
</div> <br />
</asp:Panel>
<asp:Panel ID="pnlTaxon" runat="server" Visible="false">
 &nbsp;&nbsp;<asp:LinkButton ID="lbSpecies" runat="server" onclick="lbSpecies_Click" Font-Bold="True">Species held at site:</asp:LinkButton><br />
 <asp:Label ID="lblTaxon" runat="server" Text=""></asp:Label>
    <asp:Repeater ID="rptTaxon" runat="server" Visible="False">
     <HeaderTemplate>
         <ul></HeaderTemplate>
        <ItemTemplate>
         <li><%# Eval("name")%> (<a href="view2.aspx?dv=web_site_taxon_accessionlist&params=:taxonomyid=<%# Eval("taxonomy_species_id") %>;:siteid=<%# Eval("site_id") %>"><%# Eval("cnt") %> Accessions</a>)</li>
        </ItemTemplate>
      <FooterTemplate></ul></FooterTemplate>
</asp:Repeater>
<br />
</asp:Panel>
<hr /> 
</asp:Content>