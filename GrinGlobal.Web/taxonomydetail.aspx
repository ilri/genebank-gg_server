<%@ Page Title="Taxonomy" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="taxonomydetail.aspx.cs" Inherits="GrinGlobal.Web.TaxonomyDetail" %>
<%@ Import Namespace="GrinGlobal.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
<asp:DetailsView ID="dvTaxonomy" runat="server" AutoGenerateRows="false" DefaultMode="ReadOnly" CssClass='detail' GridLines="None">
    <FieldHeaderStyle CssClass="" />
    <HeaderTemplate>
        <h1 style="font-size: 150%"><a href='taxon/abouttaxonomy.aspx?chapter=scient' target='_blank'>Taxon:</a>
        <%# Eval("taxonomy_name") %></h1>
        
        <%#Eval("synonym_for_taxonomy")%>
    </HeaderTemplate>
    <EmptyDataTemplate>
        No taxonomy data found
    </EmptyDataTemplate>
    <Fields>
        <asp:TemplateField>
            <ItemTemplate>
            <table runat="server" cellpadding='1' cellspacing='1' border='0' class='grid horiz' style='width:635px; border:1px solid black'>
            <tr id="tr_genus">
                <th>Genus:</th>
                <td><i><%# Eval("genus_name") %></i></td>
            </tr>
            <tr id="tr_subgenus">
                <th>&nbsp;&nbsp;Subgenus:</th>
                <td><i><%# Eval("subgenus_name") %></i></td>
            </tr>
            <tr id="tr_section">
                <th>&nbsp;&nbsp;&nbsp;&nbsp;Section:</th>
                <td><i><%# Eval("section_name") %></i></td>
            </tr>
            <tr id="tr_subsection">
                <th>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Subsection:</th>
                <td><i><%# Eval("subsection_name") %></i></td>
            </tr>
            <tr id="tr_family">
                <th>Family:</th>
                <td><i><%# Eval("family_name") %></i><%# Eval("alt_familyname").ToString() == "" ? "" : " (alt.<i>" + Eval("alt_familyname").ToString() + "</i>)"%></td>
            </tr>
            <tr id="tr_subfamily">
                <th>&nbsp;&nbsp;Subfamily:</th>
                <td><i><%# Eval("subfamily") %></i></td>
            </tr>
            <tr id="tr_tribe">
                <th>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Tribe:</th>
                <td><i><%# Eval("tribe") %></i></td>
            </tr>
            <tr id="tr_subtribe">
                <th>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Subtribe:</th>
                <td><i><%# Eval("subtribe")%></i></td>
            </tr>
            <tr>
                <th>Nomen number:</th>
                <td><%# Eval("Nomen_number") %></td>
            </tr>
            <tr id="tr_protologue">
                <th>Place of publication:</th>
                <td><%# Eval("protologue") %></td>
            </tr>
             <tr id="tr_comment">
                <th><%# DisplayCommentHead(Eval("note")) %></th>
                <td><%# DisplayComment(Eval("note")) %>
                </td>
            </tr>
            <tr id="tr_typification">
                <th>Typification:</th>
                <td></td>
            </tr>
            <tr>
                <th>Name Verified on:</th>
                <td> <%# DisplayVerified(Eval("name_verified_date"), Eval("verifier_cooperator_id"), Eval("verifier_name"))%> <b>Last Changed:</b> <%# Toolkit.Coalesce(Eval("modified_date", "{0:dd-MMM-yyyy}"), Eval("created_date", "{0:dd-MMM-yyyy}")) %></td> 

            </tr>
            <tr>
                <th>Species priority site is:</th>
                <td><%# Eval("site_1_long") %>  <%# Eval("site_1_long").ToString() == "" ? "" : "(" + Eval("priority_site_1") + ")"%></td>
            </tr>            
            <tr>
                <th>Accessions:</th>
                <td><a href='view2.aspx?dv=web_taxonomyspecies_view_accessionlist&params=:taxonomyid=<%# Eval("taxonomy_species_id")%>'><%# Eval("access_count") %> in National Plant Germplasm System</a> <%# Eval("acc_mapcnt").ToString() == "0" || Eval("Nolocation").ToString() == "Y" ? "" : " (" + "<a href=\"maps.aspx?taxonomyid=" + Eval("taxonomy_species_id") + "\"</a>" + "GoogleMap)"%></td>
            </tr>
        </table>
            </ItemTemplate>
        </asp:TemplateField>
    </Fields>
</asp:DetailsView>

<asp:Panel ID="pnlConspecific" runat="server" Visible="false">
<asp:Repeater ID="rptConspecific" runat="server">
    <HeaderTemplate>
        <h1><%= Page.DisplayText("htmlOther", "See other conspecific taxa:")%></h1>
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li><a href='taxonomydetail.aspx?id=<%# Eval("taxonomy_species_id")%>'><%# Eval("name") %></a> (<a href='view2.aspx?dv=web_taxonomyspecies_view_accessionlist&params=:taxonomyid=<%# Eval("taxonomy_species_id")%>'><%# Eval("accession_count") %> accessions</a>)</li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>
</asp:Panel>

<asp:Panel ID="pnlCommonNames" runat="server" Visible="false">
<asp:Repeater ID="rptCommonNames" runat="server">
    <HeaderTemplate>
        <h1><a href='taxon/abouttaxonomy.aspx?chapter=common' target='_blank'><%= Page.DisplayText("htmlCommon", "Common names:")%></a></h1>
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li><b><%# Eval("name") %></b>&nbsp;&nbsp;<small><%# DisplayCommonNameCitation(Eval("literature_id"), Eval("source"))%> - <%#Eval("language_description") %></small></li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>
</asp:Panel>

<asp:Panel ID="pnlEconomicImportance" runat="server" Visible="false">
<asp:Repeater ID="rptEconomicImportance" runat="server">
    <HeaderTemplate>
        <h1><a href='taxon/abouttaxonomy.aspx?chapter=econ' target='_blank'><%= Page.DisplayText("htmlEconomic", "Economic Importance:")%></a></h1>
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li><b><%# Eval("economic_usage")%>:</b> <%# Eval("usage_type") %> (<%# Eval("note") %>) </li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>
    </asp:Panel>


<asp:Panel ID="pnlDistributionRange" runat="server" Visible="false">
<h1><a href='taxon/abouttaxonomy.aspx?chapter=distrib' target='_blank'><%= Page.DisplayText("htmlDistribution", "Distributional Range:")%></a></h1>
<asp:Repeater ID="rptDistribution" runat="server">
    <HeaderTemplate>
        <ul>  
    </HeaderTemplate>
    <ItemTemplate>
        
        <h1><%# Eval("geo_status")%>:</h1>
        <asp:Label ID="lblGeoNote" runat="server" Visible="False"></asp:Label>
        <asp:Repeater ID="rptDistributionRange" runat="server">
        <HeaderTemplate>
        </HeaderTemplate>
        <ItemTemplate><li>
            <h3><%# Eval("continent") %></h3>
            <asp:Repeater ID="rptDistSubcontinent" runat="server">
            <HeaderTemplate><ul></HeaderTemplate>
            <ItemTemplate>
                <li>
                <i><%# Eval("subcontinent") %></i><asp:Repeater ID="rptDistCountry" runat="server">
                <HeaderTemplate>:</HeaderTemplate>
                <ItemTemplate>
                    <b><%# Eval("country_name") %></b><asp:Repeater ID="rptDistState" runat="server">
                        <ItemTemplate><%# Eval("state_name").ToString() == "" ? "" : " - "%><%# Eval("state_name") %></ItemTemplate>
                        <SeparatorTemplate>, </SeparatorTemplate></asp:Repeater></ItemTemplate><SeparatorTemplate>;</SeparatorTemplate>
                </asp:Repeater>
                </li>
            </ItemTemplate>
            <FooterTemplate></ul></FooterTemplate>
            </asp:Repeater></li>
        </ItemTemplate>
        <FooterTemplate>
            <br> 
        </FooterTemplate>
        </asp:Repeater>
         
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>
</asp:Panel>

<asp:Panel ID="pnlReferences" runat="server" Visible="false">
<asp:Repeater ID="rptReferences" runat="server">
    <HeaderTemplate>
        <h1><a href='taxon/abouttaxonomy.aspx?chapter=liter' target='_blank'><%= Page.DisplayText("htmlReferences", "References:")%></a></h1>
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li><b><%# Eval("author") %></b> <%# Eval("citation_year")%><%# Eval("citation_year").ToString() == "" ? "" : "."%> <%# Eval("title") %> <%# Eval("abbrev")%> <%# Eval("refernece")%></li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>
</asp:Panel>

<asp:Panel ID="pnlSynonyms" runat="server" Visible="false">
<asp:Repeater ID="rptSynonyms" runat="server">
    <HeaderTemplate>
        <h1><%= Page.DisplayText("htmlSynonyms", "Synonyms:")%></h1>
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li><a href='taxonomydetail.aspx?id=<%# Eval("taxonomy_species_id") %>'><%# Eval("name") %></a> <%# Eval("authority") %></li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>
</asp:Panel>

<asp:Repeater ID="rptCheckOther" runat="server">
    <HeaderTemplate>
        <h1><%= Page.DisplayText("htmlCheck", "Check other web resources for")%> <%# getName()%>:</h1>
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li><%# Eval("otherPre") %><b><%# Eval("otherDBlink") %></b> <%# Eval("otherDB") %> </li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>

<asp:Panel ID="pnlImage" runat="server" Visible="false">
<asp:Repeater ID="rptImages" runat="server">
    <HeaderTemplate>
        <h1><%= Page.DisplayText("htmlImages", "Images:")%></h1>
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li><b><%# Eval("title") %>: </b><%# DisplayNote(Eval("note")) %></li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>
</asp:Panel>
<hr />
</asp:Content>
