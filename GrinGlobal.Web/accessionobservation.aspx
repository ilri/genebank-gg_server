<%@ Page Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="accessionobservation.aspx.cs" Inherits="GrinGlobal.Web.AccessionObservation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <h1><%= Page.DisplayText("htmlObs", "Observations for accession")%> 
    <asp:Label ID="lblPINumber" runat="server" Text=""></asp:Label></h1>
   <asp:Label ID="lblCE" runat="server" 
            Text="Characterization and Evaluation Data:"></asp:Label>
    <table id="tblCropTrait" runat="server" cellspacing="0" rules="all" border="1" style="border-collapse:collapse;">
    <tr>
    <th><%= Page.DisplayText("htmlDescriptor", "Descriptor")%></th><th><%= Page.DisplayText("htmlValue", "Value")%></th><th><%= Page.DisplayText("htmlStudy", "Study/Environment")%></th><th><%= Page.DisplayText("htmlInvID", "Inventory ID")%></th>
    </tr>
    </table>
    <asp:Button ID="Button1" runat="server" onclick="btnExportPheno_Click" 
    Text="Export Phenotype Data to Excel" Width="200px" />

    <br /><br />
    <div runat="server" id="divGeno" visible="false">
        <asp:Label ID="lblMolecular" runat="server" Text="Molecular Data:"></asp:Label> 
        <asp:GridView ID="gvGeno" runat="server" AutoGenerateColumns="False">
         <Columns>
            <asp:BoundField DataField="Poly_Type" HeaderText="Poly Type" />
            <asp:BoundField DataField="Marker" HeaderText="Marker" />
            <asp:BoundField DataField="Value" HeaderText="Value" />
            <asp:BoundField DataField="Evaluation" HeaderText="Evaluation" />
            <asp:BoundField DataField="Study_Type" HeaderText="Study Type" />
            <asp:BoundField DataField="Inventory_ID" HeaderText="Inventory ID" />
        </Columns>
        </asp:GridView>
        <asp:Button ID="btnExportGeno" runat="server" onclick="btnExportGeno_Click" 
            Text="Export Genotype Data to Excel" Width="193px" />
    </div> 
    <br />
    <asp:Panel runat="server" Visible="False">
    <asp:GridView ID="gv" runat="server" AutoGenerateColumns="False">
    <Columns>
        <asp:BoundField DataField="category_code" HeaderText="Category" />
        <asp:BoundField DataField="trait_name" HeaderText="Descriptor" />
        <asp:BoundField DataField="trait_value" HeaderText="Value" />
        <asp:BoundField DataField="method_name" HeaderText="Study/Environment" />
        <asp:BoundField DataField="inventory_id" HeaderText="Inventory ID" />
    </Columns>
    </asp:GridView></asp:Panel>
</asp:Content>
