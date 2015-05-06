<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="descriptoraccession.aspx.cs" Inherits="GrinGlobal.Web.descriptoraccession" MasterPageFile="Site1.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <h1><asp:Label ID="lblDesc" runat="server" Text=""></asp:Label> </h1>
<asp:GridView ID="gvAccession" runat="server" 
        AutoGenerateColumns="False" CssClass="grid" 
        onrowdatabound="gvAccession_RowDataBound">
<EmptyDataTemplate>
    No data found
</EmptyDataTemplate>
<Columns>
   <asp:BoundField DataField="row_count" HeaderText=" " />
   <asp:TemplateField HeaderText="Accession">
        <ItemTemplate>
            <a href="AccessionDetail.aspx?id=<%# Eval("accession_id") %>"><%#Eval("pi_number") %></a>
        </ItemTemplate>
    </asp:TemplateField>
    <asp:BoundField DataField="top_name" HeaderText="Plant Name" />
    <asp:BoundField DataField="species" HeaderText="Species" />
    <asp:BoundField DataField="traitvalue" HeaderText="Value"/>
</Columns>
</asp:GridView>
<br />
<hr /> 
</asp:Content>