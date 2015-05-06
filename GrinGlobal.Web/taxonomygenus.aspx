<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="taxonomygenus.aspx.cs" Inherits="GrinGlobal.Web.TaxonomyGenus" %>
<%@ Import Namespace="GrinGlobal.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
<asp:DetailsView ID="dvGenus" runat="server" AutoGenerateRows="false" DefaultMode="ReadOnly" CssClass='detail' GridLines="None">
    <FieldHeaderStyle CssClass="" />
    <HeaderTemplate>
        <h1 style="font-size: 150%"><a href='taxon/abouttaxonomy.aspx?chapter=scient' target='_blank'>Genus:</a>
         <%# Eval("genus_name") %></h1>
        <%#Eval("synonym_for_genus")%>
        <asp:Panel ID="pnlSubGenus" runat="server" Visible="false">
        <h2>&nbsp;&nbsp; subg. <i><%# Eval("subgenus_name") %></i></h2></asp:Panel>
        <asp:Panel ID="pnlSect" runat="server" Visible="false">
        <h2>&nbsp;&nbsp; &nbsp;&nbsp; sect. <i><%# Eval("section_name") %></i></h2></asp:Panel>
        <asp:Panel ID="pnlSubSect" runat="server" Visible="false">
        <h2>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; subsect. <i><%# Eval("subsection_name") %></i></h2></asp:Panel>
         <asp:Panel ID="pnlSeri" runat="server" Visible="false">
        <h2>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; seri. <i><%# Eval("series_name") %></i></h2></asp:Panel>
        <asp:Panel ID="pnlSubSeri" runat="server" Visible="false">
        <h2>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;subseri. <i><%# Eval("subseries_name") %></i></h2></asp:Panel>
    </HeaderTemplate>
    <EmptyDataTemplate>
        No genus data found
    </EmptyDataTemplate>
    <Fields>
        <asp:TemplateField>
            <ItemTemplate>
            <table runat="server" cellpadding='1' cellspacing='1' border='0' class='grid horiz' style='width:505px; border:1px solid black'>
            <tr>
                <th>Family:</th>
                <td><i><%# Eval("family_name") %></i></td>
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
                <td><i><%# Eval("subtribe") %> </i></td>
            </tr>
            <tr id="tr_altfamily">
                <th>Altfamily:</th>
                <td><i><%# Eval("altfamily") %></i></td>
            </tr>
            <tr id="tr_common_name">
                <th>Common names:</th>
                <td><%# Eval("common_name") %></td>
            </tr>
            <tr>
                <th>Genus number:</th>
                <td><%# Eval("genus_number") %></td>
            </tr>
            <tr>
                <th>Last updated:</th>
                <td><%# Toolkit.Coalesce(Eval("modified_date", "{0:dd-MMM-yyyy}"), Eval("created_date", "{0:dd-MMM-yyyy}")) %></td>
            </tr>
            <tr  id="tr_count">
                <th>Accession Count:</th>
                <td><a href='view2.aspx?dv=web_taxonomygenus_view_accessionlist&params=:taxonomygenusid=<%# Eval("genus_number")%>&hdv=web_taxonomygenus_header'><%# Eval("accession_count") %></a></td>
            </tr>
            <tr  id="tr_comments">
                <th>Comments:</th>
                <td><%# Eval("note") %> </td>
            </tr>
        </table>
            </ItemTemplate>
        </asp:TemplateField>
    </Fields>
</asp:DetailsView>

<br/>
<b>
    <asp:HyperLink ID="hlRecordlist" runat="server"> <b>List of Species Records in GRIN</b></asp:HyperLink>
</b>
<br />
<br />


<asp:Repeater ID="rptReferences" runat="server">
    <HeaderTemplate>
        <h1><%= Page.DisplayText("htmlReferences", "References for ")%>&nbsp;<%# getNameTitle()%>:</h1>
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li><b><%# Eval("author") %></b> <%# Eval("citation_year")%><%# Eval("citation_year").ToString() == "" ? "" : "."%> <%# Eval("title") %>. <%# Eval("abbrev")%> <%# Eval("refernece")%></li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>

<asp:Panel ID="pnlReference" runat="server" Visible="false">
    <h1>References for genus:</h1>
    <ul>
    <li>There are no references for this <%# getNameTitle()%> in GRIN-Global.</li></ul>
 </asp:Panel>
 
<asp:Panel ID="pnlMore" runat="server" Visible="false">
<dl><dd><b><%= Page.DisplayText("htmlMore", "More")%></b></dd></dl>
<ul>
<li><nobr><asp:HyperLink ID="hlKBD" runat="server" Target="_blank"> 
		<b>KBD:</b></asp:HyperLink> Kew Bibliographic Databases of Royal
			Botanic Gardens, Kew </nobr>
	<br /><font size=-2>Note: Log on to KBD for better access.
		</font></li> 
<li><a href="http://scholar.google.com/">
        <img src="http://scholar.google.com/scholar/scholar_sm.gif"
                alt="Google Scholar" width="105" height="40"
                border="0"/></a>&nbsp; <font size=-2>
                        <asp:TextBox ID="txtGoogle" runat="server" ></asp:TextBox>
                        &nbsp;<asp:Button ID="btnGoogle" runat="server" Text="Search" />
		</font>
         </li>
</ul>
</asp:Panel>

<asp:Panel ID="pnlSynonyms" runat="server" Visible="false">
<asp:Repeater ID="rptSynonyms" runat="server">
    <HeaderTemplate>
        <h1>Synonyms (=), probable synonyms (=~), and possible synonysm (~) for genus:</h1>
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li><%# Eval("qualifying_code")%>&nbsp;<a href='taxonomygenus.aspx?id=<%# Eval("taxonomy_genus_id") %>'><%# Eval("genus_name") %></a> <%# Eval("authority") %></li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>
</asp:Panel> 

<!--
<asp:Panel ID="pnlCheckOther" runat="server" Visible="false">
<asp:Repeater ID="rptCheckOther" runat="server">
    <HeaderTemplate>
        <h1><%= Page.DisplayText("htmlCheckOther", "Check other nomenclature databases for ")%>&nbsp;<i><%# getName() %></i>:</h1>
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li><b><%# Eval("otherDBlink") %></b>: &nbsp <%# Eval("otherDB") %> </li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>
</asp:Panel>
-->

<asp:Panel ID="pnlSubdivisons" runat="server" Visible="false">
<h1>Subdivisions of <%# getNameTitle()%> <i><%# getName() %></i>:</h1>
    <asp:GridView ID="gvSubdivisions" runat="server" GridLines="None" AutoGenerateColumns="False">
    <Columns>
    <asp:TemplateField HeaderText="Subgenus"><ItemTemplate><%# Eval("Subgenus")%></ItemTemplate></asp:TemplateField> 
    <asp:TemplateField HeaderText="Section"><ItemTemplate><%# Eval("Section")%></ItemTemplate></asp:TemplateField> 
    <asp:TemplateField HeaderText="Subsection"><ItemTemplate><%# Eval("Subsection")%></ItemTemplate></asp:TemplateField> 
    <asp:TemplateField HeaderText="Series"><ItemTemplate><%# Eval("Series")%></ItemTemplate></asp:TemplateField> 
    <asp:TemplateField HeaderText="Subseries"><ItemTemplate><%# Eval("Subseries")%></ItemTemplate></asp:TemplateField> 
    </Columns>
     </asp:GridView></asp:Panel>

<asp:Panel ID="pnlImages" runat="server" Visible="false">
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