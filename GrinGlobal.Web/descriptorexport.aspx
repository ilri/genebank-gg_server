<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="descriptorexport.aspx.cs" Inherits="GrinGlobal.Web.descriptorexport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="~/styles/default.css" rel="stylesheet" type="text/css" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h3>Essential fields (included automatically)</h3> 
    <ul>
        <li>Accession prefix</li>
        <li>Accession number</li>
        <li>Actual evaluation/characterization value </li>
        <li>Descriptor name</li>
        <li>Evaluation method name</li>
    </ul>
        <h3>Optional fields</h3> 
        <asp:CheckBoxList ID="cblOptions" runat="server">
            <asp:ListItem Value="Accession surfix">Accession suffix</asp:ListItem>
            <asp:ListItem Value="Plant name (cultivar or other identifier)">Plant name (cultivar or other identifier)</asp:ListItem>
            <asp:ListItem Value="Species name">Species name</asp:ListItem>
            <asp:ListItem Value="Country where collected/developed">Country where collected/developed</asp:ListItem>
            <asp:ListItem Value="Original value when observation value is standardized">Original value when ob value is standardized</asp:ListItem>
            <asp:ListItem Value="Frequency within the accession this observation value occurs">Frequency within the accession this ob value occurs</asp:ListItem>
            <asp:ListItem Value="Minimum value for this accession">Minimum value for this accession</asp:ListItem>
            <asp:ListItem Value="Maximum value for accession">Maximum value for accession</asp:ListItem>
            <asp:ListItem Value="Average value for accession">Average value for accession</asp:ListItem>
            <asp:ListItem Value="Standard deviation for accession">Standard deviation for accession</asp:ListItem>
            <asp:ListItem Value="Sample size for above statistics">Sample size for above statistics</asp:ListItem>
            <asp:ListItem Value="Inventory prefix">Inventory prefix</asp:ListItem>
            <asp:ListItem Value="Inventory number">Inventory number</asp:ListItem>
            <asp:ListItem Value="Inventory surfix">Inventory surfix</asp:ListItem>
            <asp:ListItem Value="Comment about the accession">Comment about the accession</asp:ListItem>
        </asp:CheckBoxList>
    <br />
      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnExportSelect" runat="server" 
            Text="Export Selected Traits" onclick="btnExportS_Click" />&nbsp;&nbsp;&nbsp;&nbsp; 
       <asp:Button ID="btnExportAll" runat="server" Text="Export All Traits" 
        onclick="btnExportA_Click" /> 
        <br /><br />
        <asp:GridView ID="gvResult" runat="server" Visible="False">
        </asp:GridView>
        <asp:Label ID="Label1" runat="server" Text=""></asp:Label>
    </div>
    </form>
</body>
</html>
