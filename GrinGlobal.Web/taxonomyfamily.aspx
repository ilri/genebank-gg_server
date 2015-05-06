<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="taxonomyfamily.aspx.cs" Inherits="GrinGlobal.Web.TaxonomyFamily" %>
<%@ Import Namespace="GrinGlobal.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
<asp:DetailsView ID="dvFamily" runat="server" AutoGenerateRows="false" DefaultMode="ReadOnly" CssClass='detail' GridLines="None">
    <FieldHeaderStyle CssClass="" />
    <HeaderTemplate>
        <h1 style="font-size: 150%"><a href='taxon/abouttaxonomy.aspx?chapter=scient' target='_blank'>Family:</a>
        <%# Eval("family_name") %></h1>
        <asp:Panel ID="pnlSubFamily1" runat="server" Visible="false">
        <h1>&nbsp;&nbsp; subfam. <i><%# Eval("subfamily") %></i></h1></asp:Panel>
        <asp:Panel ID="pnlTribe1" runat="server" Visible="false">
        <h1>&nbsp;&nbsp; &nbsp;&nbsp;tribe <i><%# Eval("tribe") %></i></h1></asp:Panel>
         <asp:Panel ID="pnlSubTribe1" runat="server" Visible="false">
        <h1>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;subtribe <i><%# Eval("subtribe") %></i></h1></asp:Panel>
        
        <%#Eval("synonym_for_family")%>
        
        <asp:Panel ID="pnlSubFamily" runat="server" Visible="false">
        <h2>&nbsp;&nbsp; subfam. <i><%# Eval("subfamily") %></i></h2></asp:Panel>
        <asp:Panel ID="pnlTribe" runat="server" Visible="false">
        <h2>&nbsp;&nbsp; &nbsp;&nbsp;tribe <i><%# Eval("tribe") %></i></h2></asp:Panel>
         <asp:Panel ID="pnlSubTribe" runat="server" Visible="false">
        <h2>&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;subtribe <i><%# Eval("subtribe") %></i></h2></asp:Panel>

    </HeaderTemplate>
    <EmptyDataTemplate>
        No family data found
    </EmptyDataTemplate>
    <Fields>
        <asp:TemplateField>
            <ItemTemplate>
            <table id="Table1" runat="server" cellpadding='1' cellspacing='1' border='0' class='grid horiz2' style='width:600px; border:1px solid black'>
            <tr>
                <th>Family number:</th>
                <td><%# Eval("family_number") %> &nbsp;&nbsp;
                <b>Last Updated:</b> <%# Toolkit.Coalesce(Eval("modified_date", "{0:dd-MMM-yyyy}"), Eval("created_date", "{0:dd-MMM-yyyy}")) %></td>
            </tr>
            <tr id="tr_alternatename">
                <th>Alternate name:</th>
                <td><%# Eval("altfamily") %></td>
            </tr>
            <tr>
                <th>Number of accepted genera:</th>
                <td><%# Eval("genera_count")%></td>
            </tr>
            <tr id="tr_typegenus">
                <th>Type genus:</th>
                <td><%# Eval("genus_type")%></td>
            </tr>
            <tr id="tr_comments">
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
    <asp:HyperLink ID="hlRecordlist" runat="server"> <b>Complete list of genera</b></asp:HyperLink>
</b>

<br/>

<!--
<asp:Panel ID="pnlCheckOther" runat="server" Visible="false">
<asp:Repeater ID="rptCheckOther" runat="server">
    <HeaderTemplate>
        <h1><%= Page.DisplayText("htmlCheck", "Check other databases for ")%> <i><%# getName() %></i>:</h1>
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li><%# Eval("otherDBlink") %>: &nbsp <%# Eval("otherDB") %> </li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>
</asp:Panel>
-->

<asp:Repeater ID="rptReferences" runat="server">
    <HeaderTemplate>
        <h1><%= Page.DisplayText("htmlReferences", "References for ")%>&nbsp; <%# getNameTitle()%>:</h1>
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li><b><%# Eval("author") %></b> <%# Eval("citation_year")%><%# Eval("citation_year").ToString() == "" ? "" : "."%> <%# Eval("title") %>. <%# Eval("abbrev")%> <%# Eval("refernece")%></li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>

<asp:Panel ID="pnlReference" runat="server" Visible="true">
    <h1>References for <%# getNameTitle()%>:</h1>
    <ul>
    <li>There are no references for this <%# getNameTitle() %> in GRIN-Global.</li></ul>
 </asp:Panel>
 
<asp:Panel ID="pnlMore" runat="server" Visible="false">
<dl><dd><b><%= Page.DisplayText("htmlCheckOther", "Check other bibliographic databases: ")%></b></dd></dl>
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
        <h1>Synonyms for family:</h1>
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li><a href='taxonomyfamily.aspx?id=<%# Eval("taxonomy_family_id") %>'><%# Eval("family_name") %></a> <%# Eval("author_name") %></li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>
</asp:Panel> 

<asp:Panel ID="pnlSubdivisons" runat="server" Visible="false">
<h1>Subfamilies and tribes for  <i><%#  getName() %></i>:</h1>
    <asp:GridView ID="gvSubdivisions" runat="server" GridLines="None" AutoGenerateColumns="False">
    <Columns>
    <asp:TemplateField HeaderText="Subfamily"><ItemTemplate><%# Eval("Subfamily")%></ItemTemplate></asp:TemplateField> 
    <asp:TemplateField HeaderText="Tribe"><ItemTemplate><%# Eval("Tribe")%></ItemTemplate></asp:TemplateField> 
    <asp:TemplateField HeaderText="Subtribe"><ItemTemplate><%# Eval("SubTribe")%></ItemTemplate></asp:TemplateField> 
    </Columns>
     </asp:GridView>
</asp:Panel>

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