<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="summary.aspx.cs" Inherits="GrinGlobal.Web.query.summary" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Summary Statistics for GRIN NPGS Collections</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
<center>
<pre>
<label style="background-color: #2F571B; color: #FFFFFF; font-size: 26px; font-weight: bold;">&nbsp; National Plant Germplasm System&nbsp;</label>

Summary of the holdings of the NPGS as of <%= DateTime.Now.ToString("dd-MMM-yyyy") %>

  Number of families represented:        <%=cntFamily%>

    Number of genera represented:        <%=cntGenus%>

   Number of species represented:       <%=cntSpecies%>

Number of accessions represented:      <%=cntAccession%>
	 
</pre></center>
<br />
</asp:Content>