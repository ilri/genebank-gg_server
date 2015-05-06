<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cwrgeneticcontrol.ascx.cs" Inherits="GrinGlobal.Web.taxon.cwrgeneticcontrol" %>
<asp:Panel ID="pnlControl" runat="server">
        <asp:Label ID="lblCrop" runat="server" Text="" 
            style="font-weight: 900"></asp:Label><br />
<asp:Label ID="lblCompiled" runat="server" Width="990px"></asp:Label>  
<table>
<tr><td>&nbsp;&nbsp;&nbsp;</td><td><asp:Label ID="lblCropTaxon" runat="server" Text="Crop taxon:" style="font-weight: 700"></asp:Label></td></tr>
<tr>
<td>&nbsp;&nbsp;&nbsp;</td><td>
   <asp:Repeater ID="rptTaxon" runat="server">
    <HeaderTemplate>        
        <ul style="list-style-type: decimal">
    </HeaderTemplate> 
    <ItemTemplate>
       <li><b><a href="../taxonomydetail.aspx?id=<%# Eval("taxonomy_species_id") %>" title="Link to GRIN report for this crop"><%# Eval("taxonomy_name")%></a> - <%# Eval("common_crop_name")%></b></li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater></td>
</tr>
<tr><td>&nbsp;&nbsp;&nbsp;</td><td><b>Crop wild relatives:</b></td></tr>
<tr>
<td>&nbsp;&nbsp;&nbsp;</td><td>
   <asp:Repeater ID="rptPrimary" runat="server">
    <HeaderTemplate>        
    <asp:Label ID="Label1" runat="server" Text="Primary" 
            ToolTip="Taxa that cross readily with the crop (or can be predicted to do so based on their taxonomic or phylogenetic relationships), yielding (or being expected to yield) fertile hybrids with good chromosome pairing, making gene transfer through hybridization simple." 
            ForeColor="Red" style="font-weight: 700"></asp:Label>
        <ul style="list-style-type: decimal">
    </HeaderTemplate> 
    <ItemTemplate>
<%--       <li><a href="../taxonomydetail.aspx?id=<%# Eval("taxonomy_species_id") %>" title="Link to GRIN report for this taxon"><%# Eval("taxonomy_name")%></a> - [<a href="taxonomyliterature.aspx?tid=<%# Eval("taxonomy_species_id") %>" title="Link to literature documentation for crop relative gene pool assignment">References]</li>--%>
          <li><a href="../taxonomydetail.aspx?id=<%# Eval("taxonomy_species_id") %>" title="Link to GRIN report for this taxon"><%# Eval("taxonomy_name")%></a> - [<a href="#", onclick="javascript:window.open('taxonomyliterature.aspx?tid=<%# Eval("taxonomy_species_id") %>' ,'','scrollbars=yes,titlebar=no,width=450,height=600');" title="Link to literature documentation for crop relative gene pool assignment">Reference]</a></li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater></td>
</tr>
<tr>
<td>&nbsp;&nbsp;&nbsp;</td><td>
   <asp:Repeater ID="rptSecondary" runat="server">
    <HeaderTemplate>        
    <asp:Label ID="Label2" runat="server" Text="Secondary" 
            ToolTip="Taxa that will successfully cross with the crop (or can be predicted to do so based on their taxonomic or phylogenetic relationships), but yield (or would be expected to yield) partially or mostly sterile hybrids with poor chromosome pairing, making gene transfer through hybridization difficult." 
            ForeColor="Red" style="font-weight: 700"></asp:Label>
        <ul style="list-style-type: decimal">
    </HeaderTemplate> 
    <ItemTemplate>
       <li><a href="../taxonomydetail.aspx?id=<%# Eval("taxonomy_species_id") %>" title="Link to GRIN report for this taxon"><%# Eval("taxonomy_name")%></a> - [<a href="#", onclick="javascript:window.open('taxonomyliterature.aspx?tid=<%# Eval("taxonomy_species_id") %>' ,'','scrollbars=yes,titlebar=no,width=570,height=380');" title="Link to literature documentation for crop relative gene pool assignment">Reference]</a></li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater></td>
</tr>
<tr>
<td>&nbsp;&nbsp;&nbsp;</td><td>
   <asp:Repeater ID="rptTertiary" runat="server">
    <HeaderTemplate>        
    <asp:Label ID="Label3" runat="server" Text="Tertiary" 
            ToolTip="Taxa that can be crossed with the crop (or can be predicted to do so based on their taxonomic or phylogenetic relationships), but hybrids are (or are expected to be)lethal or completely sterile. Special breeding techniques, some yet to be developed, are required for gene transfer."
            ForeColor="Red" style="font-weight: 700"></asp:Label>
        <ul style="list-style-type: decimal">
    </HeaderTemplate> 
    <ItemTemplate>
       <li><a href="../taxonomydetail.aspx?id=<%# Eval("taxonomy_species_id") %>" title="Link to GRIN report for this taxon"><%# Eval("taxonomy_name")%></a> - [<a href="#", onclick="javascript:window.open('taxonomyliterature.aspx?tid=<%# Eval("taxonomy_species_id") %>' ,'','scrollbars=yes,titlebar=no,width=570,height=380');" title="Link to literature documentation for crop relative gene pool assignment">Reference]</a></li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater></td>
</tr>
<tr>
<td>&nbsp;&nbsp;&nbsp;</td><td>
   <asp:Repeater ID="rptGraftstock" runat="server">
    <HeaderTemplate>        
    <asp:Label ID="Label4" runat="server" Text="Graftstock" 
            ToolTip="Taxa used as rootstocks for grafting scions of a crop, or used as genetic resources in the breeding of such rootstocks." 
            ForeColor="Red" style="font-weight: 700"></asp:Label>
        <ul style="list-style-type: decimal">
    </HeaderTemplate> 
    <ItemTemplate>
       <li><a href="../taxonomydetail.aspx?id=<%# Eval("taxonomy_species_id") %>" title="Link to GRIN report for this taxon"><%# Eval("taxonomy_name")%></a> - [<a href="#", onclick="javascript:window.open('taxonomyliterature.aspx?tid=<%# Eval("taxonomy_species_id") %>' ,'','scrollbars=yes,titlebar=no,width=570,height=380');" title="Link to literature documentation for crop relative gene pool assignment">Reference]</a></li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater></td>
</tr>
</table>
</asp:Panel>

