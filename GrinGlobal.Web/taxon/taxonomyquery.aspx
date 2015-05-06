<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="taxonomyquery.aspx.cs" Inherits="GrinGlobal.Web.taxon.taxonomyquery" MasterPageFile="~/Site1.Master" Title="Query Taxonomy for Plants" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
<br />
<center>
<table  width="75%">
	<tr>
	<td colspan="2"><center><br />
	<hr /><font size="3"><b>Query all GRIN 
		T<font size="2">AXONOMY FOR</font> 
		P<font size="2">LANTS</font>:</b></font><hr /></center></td>
	</tr>	
	<tr>
	<td>&nbsp;</td>
	<td >
	<a href="taxonomysearch.aspx" title="Click here for GRIN Taxonomy for Plants general query form"><font size="2"><b>Advanced queries - species data, multiple criteria</b></font></a></td>
	</tr>
	<tr>
	<td>&nbsp;</td>
	<td >
	<a href="taxonomysimple.aspx" title="Click here for GRIN Taxonomy for Plants simple query form"><font size="2"><b>Simple queries - species data, single criterion</b></font></a></td>
	</tr>
	<tr>
	<td>&nbsp;</td>
	<td>
	<a href="famgensearch.aspx" title="Click here for GRIN Taxonomy for Plants family/genus query form"><font size="2"><b>Queries of family and generic data</b></font></a></td>
	</tr>
	<tr>
	<td colspan="2"><center><br />
	<hr /><font size="3"><b>Query specialized parts of GRIN 
		T<font size="2">AXONOMY FOR</font> 
		P<font size="2">LANTS</font>:</b></font><hr /></center></td>
	</tr>
	<tr>
	<td>&nbsp;</td>
	<td>
	<a href="taxonomysearcheco.aspx" title="Click here for GRIN Taxonomy for Plants economic plants query form"><font size="2"><b>Economic Plants</b></font></a></td>
	</tr>
	<tr>
	<td>&nbsp;</td>
	<td>
	<a href="taxonomysearchcwr.aspx" title="Click here for GRIN crop wild relative query form"><font size="2"><b>Crop Wild Relatives</b></font></a></td>
	</tr>
<%--	<tr>
	<td>&nbsp;</td>
	<td>
	<a href="" title="Click here for GRIN Taxonomy for Plants noxious weed query form"><font size="2"><b>Noxious Weeds - Federal and State (U.S.A.)</b></font></a></td>
	</tr>
	<tr>
	<td>&nbsp;</td>
	<td>
	<a href="" title="Click here for GRIN Taxonomy for Plants rare plant query form"><font size="2"><b>Rare Plants</b></font></a></td>
	</tr>
	<tr>
	<td>&nbsp;</td>
	<td>
	<a href="" title="Click here for GRIN Taxonomy for Plants seed associations' web page"><font size="2"><b>From Seed Associations' Web Page</b></font></a></td>
	</tr>
	<tr>
	<td>&nbsp;</td>
	<td>
	<a href="" title="Click here for GRIN Taxonomy for Plants NRCS/PEAS database query form"><font size="2"><b>NRCS/PEAS Database Nomenclature</b></font></a></td>
	</tr>
	<tr>
	<td>&nbsp;</td>
	<td>
	<a href="" title="Click here for GRIN Taxonomy for Plants rhizobial nodulation query form"><b>Rhizobial Nodulation Data in GRIN</b></a><br /></td>
	</tr>--%>
	<tr><td colspan="2"><br /><hr /></td></tr>
</table> 
</center><br /><br />

</asp:Content>
