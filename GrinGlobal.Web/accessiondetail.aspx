<%@ Page Title="Accession Detail" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="accessiondetail.aspx.cs" Inherits="GrinGlobal.Web.AccessionDetail" %>
<%@ Import Namespace="GrinGlobal.Core" %>
<%@ Register src="imagecontrol.ascx" tagname="ImageControl" tagprefix="gg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type='text/javascript'>
    $(document).ready(function() {
        $('a.webImage').click(function(e) {
            e.preventDefault();
            //var url = $('img.fullImage', this).attr('src');
            //window.open("ImageDisplay.aspx?imgPath=" + url, "", "scrollbars=no, width=720, height=510, resizable=yes, menubar=yes");
            var id = $('img.Image', this).attr('id');
            window.open("ImageDisplay.aspx?id=" + id, "", "scrollbars=no, width=720, height=600, resizable=yes, menubar=yes");
        });
    });
</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">

    <div id='main-wrapper'>
    <div id='sidebar-wrapper' class="images">
	    <asp:DetailsView ID="dtlAvailability" runat="server" AutoGenerateRows="false" CssClass="availability" GridLines="None">
	        <Fields>
	            <asp:TemplateField HeaderText="Status:" HeaderStyle-BorderStyle="None" >
	                <ItemTemplate>
	                    <%#Eval("availability_status") %>
	                </ItemTemplate>
	            </asp:TemplateField>
	            <asp:TemplateField HeaderText="Amt Distributed:" HeaderStyle-BorderStyle="None" >
	                <ItemTemplate>
	                   <%-- <%# Toolkit.ToInt32(Eval("qty"), 0) == 0 ? "Not Available" : Eval("qty").ToString() %>--%>
	                   <%# Eval("distribution_default_quantity").ToString().Trim() == "" ? "" : (Eval("qty").ToString().Substring(0, 1) == "0" && Eval("qty").ToString().Length == 1) ? "" : Eval("distribution_default_quantity").ToString() + " " + Eval("distribution_unit").ToString()%> 
	                </ItemTemplate>
	            </asp:TemplateField>
	            <asp:TemplateField HeaderText="Type Distributed:" HeaderStyle-BorderStyle="None" >
	                <ItemTemplate>
	                    <%#Eval("distribution_type")%>
	                </ItemTemplate>
	            </asp:TemplateField>
	
		        <asp:TemplateField>
	                <ItemTemplate>
                        <tr>
                            <td colspan="2">
                                <span id="divActions">
                                   <p>
                                   <asp:Panel ID="pnlNotFavorite" runat="server" >	
                                        <asp:ImageButton ID="btnAddToMyFavorites" runat="server" ImageUrl="images/btn_addtomyfavs.gif" onclick="btnAddToMyFavorites_Single" />
                                   </asp:Panel>
                                   <asp:Panel ID="pnlFavorite" runat="server" >
                                    <asp:ImageButton ID="btnRemoveFavorites" runat="server" ImageUrl="images/btn_removefromfavs.gif" onclick="btnRemoverFromMyFavorites_Single" />
                                     </asp:Panel>
                                    </p>
                                    <p>
                                        <asp:Panel ID="pnlAvailable" runat="server" Visible='<%# IsAvailable(Eval("availability_status")) %>' >		                                        
                                            <asp:ImageButton ID="btnAddToOrder" runat="server" ImageUrl="images/btn_addtoorder.gif" onclick="btnAddToOrder_Single" />
                                        </asp:Panel>        
                                        <asp:Panel ID="pnlUnavailable" runat="server" Visible='<%# !IsAvailable(Eval("availability_status")) %>' >
                                            (Not available to order)</asp:Panel>
                                   </p>                     
                                </span>
                            </td>
                        </tr>
                    </ItemTemplate>
	            </asp:TemplateField>
	        </Fields>
	      </asp:DetailsView>
		      <br />
		  <gg:ImageControl ID="imagePreviewer" runat="server" />
        <p>&nbsp;</p>
      </div><!-- end sidebar-wrapper -->

<asp:Repeater runat="server" ID="rptHeader">
    <ItemTemplate>
        <h1><%# Eval("pi_number") %></h1>
        <h2><b><a href="taxonomydetail.aspx?id=<%# Eval("taxonomy_species_id") %>"><%# Eval("taxonomy_name_built") %></a></b></h2>
        <b><%# Eval("cultivar").ToString() == "" ? "" : "'" + Eval("cultivar") + "'" %></b> 
        <br /><br />
    </ItemTemplate>
</asp:Repeater>

<asp:Repeater runat="server" ID="rptBox">
    <ItemTemplate>
        <!-- notice the date formatting in the Eval statements below -->
        <table cellpadding='1' cellspacing='1' border='0' class='grid horiz' style='width:535px; border:1px solid black'>
            <tr>
                <th><%# UpcaseFirstLetter(Eval("source_type_code"))%> from:</th>
                <td><%# Eval("developed_in") %></td>
            </tr>
            <tr>
                <th>Maintained by:</th>
                <td><%# Eval("site_code") %></td>
            </tr>
            <tr>
                <th>NPGS received:</th>
                <td><%# Eval("initial_received_date", "{0:dd-MMM-yyyy}") %></td>
            </tr>
            <tr id="Tr1" runat="server" visible='<%# HasPIValue%>'>
                <th>PI assigned:</th>
                <td><%# PIAssigned %></td>
            </tr>
            <tr id="Tr2" runat="server" visible='<%# HasPIValue%>'>
                <th>Inventory volume:</th>
                <td><%# IVVolume %></td>
            </tr>
            <tr>
                <th>Backup location:</th>
                <td><%# Eval("backup_location") %></td>
            </tr>
            <tr>
                <th>Life form:</th>
                <td><%# Eval("life_form_code") %></td>
            </tr>
            <tr>
                <th>Pedigree:</th>
                <td><%# Eval("pedigree") %></td>
            </tr>            
            <tr>
                <th>Improvement status:</th>
                <td><%# Eval("improvement_status_code")%></td>
            </tr>
            <tr>
                <th>Reproductive uniformity:</th>
                <td><%# Eval("reproductive_uniformity_code")%></td>
            </tr>
            <tr>
                <th>Form received:</th>
                <td><%# Eval("initial_material_type_desc")%></td>
            </tr>
        </table>
    </ItemTemplate>
</asp:Repeater>
<asp:Label ID="lblViewPDF" runat="server" Visible="False"><br />View original Plant Inventory data (<a href='http://sun.ars-grin.gov:8080/npgs_public/prodweb.pdf0?*' target="_blank">PDF</a> format)</asp:Label> 
<asp:Label ID="lblViewIMPDF" runat="server" Visible="False"><br />View Plant Immigrant Series data (<a href='http://sun.ars-grin.gov:8080/npgs_public/prodweb.pdfi0?*' target="_blank">PDF</a> format)</asp:Label> 

<asp:Panel ID="plAccessionNames" runat="server" Visible="False">
<br />
<h1><%= Page.DisplayText("htmlAccName", "Accession names and identifiers")%></h1>
<asp:Repeater ID="rptAccessionNames" runat="server">
    <ItemTemplate>
        <table cellpadding='1' cellspacing='0' border='0' width='40%' class='grid accessNameTable'>
            <tr>
                <th colspan="3" align="center"><%# Eval("top_name") %></th>
            </tr>
            <tr>
                <td class='tdspacer'></td>
                <td class='tdlabel'>Type:</td>
                <td class='tddata'><%# Eval("category") %></td>
            </tr>
             <tr runat="server" visible='<%# HasValue(Eval("group_name")) %>'>
                <td class='tdspacer'></td>
                <td class='tdlabel'>Group:</td>
                <td class='tddata'><%# Eval("group_name") %></td>
            </tr>
            <tr runat="server" visible='<%# HasValue(Eval("note")) %>'>
                <td class='tdspacer'></td>
                <td class='tdlabel'>Comment:</td>
                <td class='tddata'> <%# Eval("note") %></td>
            </tr>
            <tr runat="server" visible='<%# HasValue(RemoveLeadingComma(Eval("full_name"))) %>'>
                <td class='tdspacer'></td>
                <td class='tdlabel'>Cooperator:</td>
                <td class='tddata'><a href="cooperator.aspx?id=<%# Eval("cooperator_id") %>"><%# RemoveLeadingComma(Eval("full_name"))%></a></td>
            </tr>
          </table>
    </ItemTemplate>
</asp:Repeater>
</asp:Panel>

<asp:Panel ID="plAccessionIpr" runat="server" Visible="False">
<br />
<h1><%= Page.DisplayText("htmlIPR", "Intellectual Property Rights")%></h1>
<asp:Repeater ID="rptAccessionIpr" runat="server">
    <ItemTemplate>
        <table>
            <tr>
                <td><b><%# Eval("title") %></b></td>
            </tr>  
          <tr runat="server" visible='<%# !IsMTA(Eval("type_code")) && !IsPatent(Eval("type_code"))&& !IsPVP(Eval("type_code")) %>'>
                <td>Identifier: <%# Eval("ipr_number") %> Crop: <%# Eval("ipr_crop_name") %> . Date Issued: <%# Eval("issued_date", "{0:dd MMM yyyy}") %>. </td>
            </tr>
          <tr runat="server" visible='<%# IsPVP(Eval("type_code")) %>'>
                <td>Identifier: <%# Eval("ipr_number") %> Crop: <%# Eval("ipr_crop_name") %> . Date Issued: <%# Eval("issued_date", "{0:dd MMM yyyy}") %>. <a href="http://www.ars-grin.gov/cgi-bin/npgs/html/showpvp.pl?pvpno=<%# Eval("ipr_number").ToString().Replace("PVP ", "") %>" target='_blank'>Current Status</a> </td>
            </tr>

          <tr runat="server" visible='<%# IsPatent(Eval("type_code")) %>'>
                <td><a href="http://patft.uspto.gov/netacgi/nph-Parser?patentnumber=<%# Eval("ipr_number")%>" target='_blank'><%# Eval("ipr_number")%></a> <%# Eval("issued_date").ToString() == "" ? "" : "Date Issued: " + Eval("issued_date", "{0:dd MMM yyyy}") + "." %> <%# Eval("expired_date").ToString() == "" ? "" : " Date expired: " + Eval("expired_date", "{0:dd MMM yyyy}") + "."%> <%# Eval("comment1").ToString() == "" ? "" : "Comment: " + Eval("comment1") %></td>
            </tr>
            <!--
            <tr runat="server" visible='<%# !IsMTA(Eval("type_code")) %>'>
                <td>Reference: <%# Eval("author_name") %> <%# Eval("citation_title") %> <%# Eval("reference") %></td>
            </tr>
            -->
           <tr runat="server" visible='<%# IsMTA(Eval("type_code")) %>'>
                <td>Date Issued: <%# Eval("issued_date", "{0:dd MMM yyyy}") %> </td>
            </tr> 
          <tr runat="server" visible='<%# IsMTA(Eval("type_code")) %>'>
                <td>
                      <a href="javascript: Disclaimer" onclick="javascript:window.open('disclaimer.aspx?id=<%# Eval("accession_ipr_id") %>','','scrollbars=yes,titlebar=no,resizable=yes,width=570,height=380')"> View MTA disclaimer</a>
                </td>
            </tr>    
        </table>
        <asp:Repeater ID="rptIprCitation" runat="server">
            <HeaderTemplate>
            </HeaderTemplate>
            <ItemTemplate>
                <li>&nbsp;Reference: <%# Eval("author") %>. <%# Eval("citation_year" )%>. <%# Eval("title") %>. <%# Eval("abbrev") %> <%# Eval("refernece")%> <%# Eval("cit_note").ToString() == "" ? "" : "Comment: "%><%# Eval("cit_note")%><br /><br /></li>
            </ItemTemplate>
            <FooterTemplate>
            </FooterTemplate>
       </asp:Repeater>
    </ItemTemplate>
</asp:Repeater>
</asp:Panel>

<asp:Panel ID="plAvailabilityNote" runat="server" Visible="False" Width="980px">
<br />
<h1><%= Page.DisplayText("htmlAvailabilityNote", "Additional Availability Information")%></h1>
<asp:Repeater ID="rptAvailNote" runat="server">
    <ItemTemplate>
        <%# Eval("note")%>
    </ItemTemplate>
</asp:Repeater>
<asp:Label ID="lblAvailNote" runat="server" Text=""></asp:Label>

</asp:Panel>

<asp:Panel ID="plWebAvailabilityNote" runat="server" Visible="False" Width="980px">
<br /><h1><%= Page.DisplayText("htmlWebAvailNote", "Web Availability Note")%></h1>
<asp:Repeater ID="rptWebAvailNote" runat="server">
    <ItemTemplate>
        <%# Eval("availability_note")%>
    </ItemTemplate>
</asp:Repeater>
</asp:Panel>

<asp:Panel ID="plNarrative" runat="server" Visible="False" Width="980px">
<br />
<h1><%= Page.DisplayText("htmlNarrative", "Narrative")%></h1>
<asp:Repeater ID="rptNarrative" runat="server">
    <ItemTemplate>
        <%# Eval("narrative_body") %>
    </ItemTemplate>
</asp:Repeater>

</asp:Panel>
 
 
<asp:Panel ID="plAnnotations" runat="server" Visible="False"> 
<br />
<h1><%= Page.DisplayText("htmlAnnotations", "Annotations")%></h1>
<asp:Repeater ID="rptAnnotations" runat="server">
    <ItemTemplate>
        <table class='tbBorder' rules="all">
            <tr>
                <th>Action</th><th>Date</th><th>By</th><th>Old Name</th><th>New Name</th>
            </tr>
            <tr>
                <td><%# Eval("action_name") %></td><td><%# Eval("action_date", "{0:dd MMM yyyy}") %> </td><td><%# RemoveLeadingComma(Eval("name"))%></td><td><%# Eval("old_name")%></td><td><%# Eval("new_name")%></td>
            </tr>
         </table>
    </ItemTemplate>
</asp:Repeater>
</asp:Panel> 

<asp:Panel ID="plSource" runat="server" Visible="False">
<br />
<h1><%= Page.DisplayText("htmlSource", "Source History")%></h1>
<asp:Repeater ID="rptSourceHistory" runat="server">
    <HeaderTemplate>
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li>
        Accession was <%# LowCaseString(Eval("type_code"))%>. <%# DisplayDate(Eval("source_date"), Eval("source_date_code"), true)%> <%# Eval("state_full_name") %> <%# Eval("country_name") %>
        <br />
        <%# Eval("verbatim").ToString() == "" ? "" : "Locality: "%><%# Eval("Nolocation").ToString() == "Y" && Eval("verbatim").ToString() != "" ? "Not Publicly Available." : Eval("verbatim")%>
        <%# Eval("habitat").ToString() == "" ? "" : "Habitat: " %><%# Eval("Nolocation").ToString() == "Y" && Eval("habitat").ToString() != "" ? "Not Publicly Available." : Eval("habitat")%>
        <%# Eval("Nolocation").ToString() == "Y" ? "" : Eval("locality")%>
        <%# Eval("elevation_meters").ToString() == "" ? "" : "Elevation: " %><%# Eval("Nolocation").ToString() == "Y" && Eval("elevation_meters").ToString().ToString() != "" ? "Not Publicly Available." : Eval("elevation_meters")%><%# Eval("elevation_meters").ToString() == "" || Eval("Nolocation").ToString() == "Y" ? "" : " meters."%>
        <%# Eval("quantity_collected").ToString() == "" ? "" : "Quantity: " %><%# Eval("quantity_collected") %> <%# Eval("unit_quantity_collected_code") %><%# Eval("quantity_collected").ToString() == "" ? "" : "." %>
        <%# SourceDescriptor(Eval("accession_source_id")) %><%# (Eval("verbatim").ToString() == "") &&  (Eval("habitat").ToString() == "") && (Eval("locality") == "") ? "" : "<br />" %>
        
        <%# DisplaySourceType(Eval("accession_source_id"), Eval("type_code"))%>
        
        <asp:Repeater ID="rptSourceDetail" runat="server" Visible="False">
            <HeaderTemplate>
                <br />
                <ol> 
            </HeaderTemplate>
            <ItemTemplate>
                <li><a href="cooperator.aspx?id=<%# Eval("cooperator_id") %>"><%# Eval("last_name") %><%# Eval("last_name").ToString() == "" ? "" : ","%> <%# Eval("first_name")%><%# Eval("first_name").ToString() == "" ? "" : ","%> <%# Eval("organization") %></a></li> 
            </ItemTemplate>
            <FooterTemplate>
               </ol>
               <br />
            </FooterTemplate>
        </asp:Repeater>
        <%# Eval("note").ToString().Trim() == "" ? "" : "Comment: "%><%# Eval("note") %><%# Eval("note").ToString().Trim() == "" ? "" : "<br /><br />"%>
       </li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>
</asp:Panel>

<asp:Panel ID="plCitations" runat="server" Visible="False">

<h1><%= Page.DisplayText("htmlCitations", "Citations")%></h1>
<asp:Repeater ID="rptCitations" runat="server">
    <HeaderTemplate>
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li>Reference: <%# Eval("author_name") %>. <%# Eval("citation_year" )%>. <%# Eval("ctitle") %>. <%# Eval("abbreviation") %> <%# Eval("reference") %>. <%# Eval("url").ToString() == "" ? "" : "<a href= "%><%# Eval("url")%> <%# Eval("url").ToString() == "" ? "" : "target='_blank'>"%> <%# Eval("title") %></a> <%# Eval("doi_reference").ToString() == "" ? "" : " doi:"%><%# Eval("doi_reference")%><%# Eval("note").ToString() == "" ? "" : "Comment: "%><%# Eval("note") %><br /><br /></li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>
</asp:Panel>

<asp:Panel ID="plPedigree" runat="server" Visible="False">

<h1><%= Page.DisplayText("htmlPedigree", "Pedigree")%></h1>
<asp:Repeater ID="rptPedigree" runat="server">
    <ItemTemplate>
        <%# Eval("description") %>  
            <asp:Repeater ID="rptPedigreeCitation" runat="server">
                <HeaderTemplate>
                </HeaderTemplate>
                <ItemTemplate>
                    <li><b><br />Reference:</b> <%# Eval("author") %>. <%# Eval("citation_year" )%>. <%# Eval("title") %>. <%# Eval("abbrev") %> <%# Eval("refernece")%> <%# Eval("cit_note").ToString() == "" ? "" : "Comment: "%><%# Eval("cit_note")%></li>
                </ItemTemplate>
                <FooterTemplate>
                </FooterTemplate>
        </asp:Repeater>
    </ItemTemplate>
</asp:Repeater>
<br /><br />
</asp:Panel>

<asp:Panel ID="plPathogen" runat="server" Visible="False">
<h1><%= Page.DisplayText("htmlPathogen", "Pathogen Test Information")%></h1>
   <table id="tblPathogen" runat="server" class='tbBorder' cellpadding="2" rules="all">
    <tr>
    <th><%= Page.DisplayText("htmlTest", "Test")%></th><th><%= Page.DisplayText("htmlMaterial", "Material")%></th><th><%= Page.DisplayText("htmlTested", "Tested")%></th><th><%= Page.DisplayText("htmlResult", "Result")%></th><th><%= Page.DisplayText("htmlNeeded", "Needed")%></th><th><%= Page.DisplayText("htmlStarted", "Started")%></th><th><%= Page.DisplayText("htmlCompleted", "Completed")%></th><th><%= Page.DisplayText("htmlComments", "Comments")%></th>
    </tr>
    </table><br />
</asp:Panel>


<asp:Panel ID="plQuarantine" runat="server" Visible="False">

<h1><%= Page.DisplayText("htmlQuarantine", "Quarantine information")%></h1>
<asp:Repeater ID="rptQuarantine" runat="server">
    <ItemTemplate>
        <table>
          <tr>
            <td>Quarantine type: <%# Eval("quarantine_type")%>. Status: <%# Eval("quarantine_status")%>.</td>
          </tr>
          <tr>
            <td>Location: <a href="cooperator.aspx?id=<%# Eval("cooperator_id") %>">USDA, ARS</a>. Site: Plant Germplasm Quarantine Program.</td>
          </tr>
          <tr>
            <td>Date entered: <%# Eval("entered_date", "{0:dd MMM yyyy}")%>. Date released: <%# Eval("released_date", "{0:dd MMM yyyy}")%></td>
          </tr>
          <tr>
            <td>Comment: <%# Eval("note")%></td>
          </tr>
        </table>
    </ItemTemplate>
</asp:Repeater>
<br />
</asp:Panel>

<div id="divObserve" runat="server" visible="false">

<h1><%= Page.DisplayText("htmlObservations", "Observations")%></h1> 

 <b>Click link below to see detailed observation data: </b><br />
 <asp:HyperLink ID="hp" runat="server">Detailed Accession Observation Page</asp:HyperLink> <br /><br />

<asp:Panel ID="plObservation" runat="server" Visible="False" ScrollBars="Auto" 
        Width="980px">
        <b><%= Page.DisplayText("htmlCE", "Characterization and Evaluation Data: ")%></b><br />
        <table id="tblCropTrait" runat="server" class='tbBorder' cellpadding="2" rules="all">
        </table>
 </asp:Panel>    
 <br />
 <asp:Panel ID="plGeno" runat="server" Visible="False" ScrollBars="Auto" Width="980px">
        <b><%= Page.DisplayText("htmlMolecular", "Molecular Data: ")%> </b>
        <table id="tblGeno" runat="server" class='tbBorder' rules="all">
        </table>
 </asp:Panel> 
 </div>

<asp:Panel ID="plActionNote" runat="server" Visible="False" Width="980px">

<h1><%= Page.DisplayText("htmlActionNote", "Actions")%></h1>
<asp:Repeater ID="rptActionNote" runat="server">
    <HeaderTemplate>
        <ul>
    </HeaderTemplate> 
    <ItemTemplate>
        <li><%# Eval("action_name_code")%>: <%# Eval("started_date", "{0:dd-MMM-yyyy}")%>. <%# Eval("note")%><br /><br /></li> 
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>
</asp:Panel>
        
<asp:Panel ID="plVouchers" runat="server" Visible="False">

<h1><%= Page.DisplayText("htmlVouchers", "Vouchers for accession")%></h1>
<asp:Repeater ID="rptVouchers" runat="server">
    <ItemTemplate>
    <li> <%# Eval("v_type")%><%# Eval("cooperator_id").ToString() == "" ? "" : ". Taken by: " %> <a href="cooperator.aspx?id=<%# Eval("cooperator_id") %>"> <%# Eval("cooperator_id").ToString() == "" ? "" : Eval("taken_by") %></a><%# Eval("vouchered_date").ToString() == "" ? " " : ". On: "%>  <%# Eval("vouchered_date", "{0:MM'/'dd'/'yyyy}")%>. Located at: <%# Eval("location") %>
    <%# Eval("caption").ToString() == "" ? " " : ". Identifier: "%> <%# Eval("caption") %><%# Eval("inv_sample").ToString() == "" ? "" : ". Inventory sample: "%><%# Eval("inv_sample") %><%# Eval("note").ToString() == ")" ? "" : ". Comment: "%><%# Eval("note")%>.
    <br /><br />
    </li>
    </ItemTemplate>
</asp:Repeater>
</asp:Panel>

<asp:Panel ID="plOther" runat="server" Visible="False">

<h1><%= Page.DisplayText("htmlOtherInfor", "Other information about accession")%></h1>
<asp:Repeater ID="rptOther" runat="server">
    <HeaderTemplate>
        <ul>
    </HeaderTemplate> 
    <ItemTemplate>
       <li><a href="<%# GetOtherLink(Eval("virtual_path")) %>" target='_blank'><%# Eval("title") %></a>  <%# Eval("cooperator_id").ToString() == "0" ? "" : "Provided by: " + "<a href=\"cooperator.aspx?id=" + Eval("cooperator_id") + "\"</a>" + Eval("last_name")  + (Eval("last_name").ToString() == "" ? "" : ", ") + Eval("first_name") + (Eval("first_name").ToString() == "" ? "" : ", ") + Eval("organization") + ". On: " + Eval("created_date", "{0:MM'/'dd'/'yyyy}")%></li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>
</asp:Panel>
</div><!-- end main-wrapper -->
<br />
</asp:Content>
