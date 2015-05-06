<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="methodaccession.aspx.cs" Inherits="GrinGlobal.Web.methodaccession" MasterPageFile="Site1.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <table><tr><td><h1>Accessions evaluated for: </h1></td><td><b><asp:Label ID="lblDesc" runat="server" Text=""></asp:Label></b></td></tr></table>  
<asp:Button ID="btnDownload" runat="server" onclick="btnDownload_Click" 
    Text="Download Dateset"/>
 <br />
<asp:GridView ID="gvAccession" runat="server" 
        AutoGenerateColumns="False" CssClass="grid" 
        onrowcreated="gvAccession_RowCreated" 
        onrowdatabound="gvAccession_RowDataBound">
<EmptyDataTemplate>
    No data found
</EmptyDataTemplate>
<Columns>
   <asp:TemplateField HeaderText="Accession">
        <ItemTemplate>
            <a href="AccessionDetail.aspx?id=<%# Eval("accession_id") %>"><%#Eval("pi_number") %></a>
        </ItemTemplate>
    </asp:TemplateField>
    <asp:BoundField DataField="traitvalue"/>
    <asp:BoundField DataField="top_name" HeaderText="Plant Name" />
    <asp:TemplateField HeaderText="Observation details">
        <ItemTemplate>
            <%# Eval("mini")%>
        </ItemTemplate>
    </asp:TemplateField>
</Columns>
</asp:GridView>
<asp:GridView ID="gvAccession2" runat="server" Visible="False">
</asp:GridView>
<br />
<hr /> 
</asp:Content>