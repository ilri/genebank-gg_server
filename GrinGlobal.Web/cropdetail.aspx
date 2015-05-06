<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cropdetail.aspx.cs" Inherits="GrinGlobal.Web.cropdescriptor" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <h1><asp:Label ID="lblDesc" runat="server" Text=""></asp:Label> </h1>
    <asp:Panel ID="pnlDescriptor" runat="server">
        <asp:Repeater ID="rptDesc" runat="server">
        <HeaderTemplate>
        </HeaderTemplate>
        <ItemTemplate>
                <b>Category:  <%# Eval("category_code") %> </b>
                <asp:Repeater ID="rptDescDetail" runat="server">
                <HeaderTemplate>
                    <br />
                    <ol> 
                </HeaderTemplate>
                <ItemTemplate>
                    <li><a href="descriptordetail.aspx?id=<%# Eval("crop_trait_id") %>"><%# Eval("Title") %></a> (<%# Eval("coded_name")%>)<br />
                    <%# Eval("descriptor_definition")%>
                    <br /><br />
                    </li> 
                    </ItemTemplate>
                <FooterTemplate>
                   </ol>
                </FooterTemplate>
                </asp:Repeater>
        </ItemTemplate>
        <FooterTemplate>
        </FooterTemplate>
        </asp:Repeater>
    </asp:Panel>
    <asp:Panel ID="pnlMarker" runat="server">
        <asp:HyperLink ID="hlView" runat="server" Visible="False">View</asp:HyperLink><asp:Label ID="lblS" runat="server" Text=" | " Visible="False"></asp:Label><asp:LinkButton ID="lbDownload" runat="server" OnClick="btnDownload_Click" Visible="False">Download</asp:LinkButton><asp:Label ID="lblView" runat="server" Text=" all markers for " Visible="False"></asp:Label>
        <br /><br />
        <asp:GridView ID="gvMarker" runat="server" AutoGenerateColumns="False" CssClass="grid">
        <AlternatingRowStyle CssClass="altrow" />
         <Columns>
             <asp:TemplateField HeaderText="Marker">
                <ItemTemplate>
                <nobr><a href='cropmarker.aspx?id=<%#Eval("genetic_marker_id") %>'><%#Eval("name")%></a></nobr>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="poly_type" HeaderText="Polymorphic type" />
             <asp:TemplateField HeaderText="Site">
                <ItemTemplate>
                <nobr><a href='site.aspx?id=<%#Eval("site_id") %>'><%#Eval("site_short_name")%></a></nobr>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        </asp:GridView>
        <asp:GridView ID="gvAll" runat="server" Visible="False">
        </asp:GridView>
        <asp:HiddenField ID="hf" runat="server" />
    </asp:Panel>
    <asp:Panel ID="pnlSpecies" runat="server">
    <asp:Repeater ID="rptSpecies" runat="server">
        <HeaderTemplate><ul></HeaderTemplate>
        <ItemTemplate>
         <li><a href="taxonomydetail.aspx?id=<%# Eval("taxonomy_species_id") %>"><%# Eval("name") %></a></li><br />
        </ItemTemplate>
        <FooterTemplate></ul></FooterTemplate>
        </asp:Repeater> 
    </asp:Panel>
    <asp:Panel ID="pnlCitation" runat="server">
        <asp:Repeater ID="rptCitation" runat="server">
            <HeaderTemplate>
                <br />
                <b># of Accessions</b>
                <ul style="list-style-type: none; margin-left: 0px; padding-left: 10px;"> 
            </HeaderTemplate>
            <ItemTemplate>
                <li><%# Eval("cnt")%>&nbsp;&nbsp;&nbsp;&nbsp;<%# Eval("author_name")%><%# Eval("author_name").ToString() == "" ? "" : ","%>&nbsp;&nbsp;<a href="cropcitationaccession.aspx?cid=<%# Eval("min_cid")%>"><%# Eval("citation_title")%></a>&nbsp;&nbsp;<%# Eval("citation_year")%><br /><br />
                </li> 
            </ItemTemplate>
            <FooterTemplate>
               </ul>
            </FooterTemplate>
        </asp:Repeater>
    </asp:Panel>
  <br />
  <hr />
</asp:Content>