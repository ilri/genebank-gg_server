<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="method.aspx.cs" Inherits="GrinGlobal.Web.method" MasterPageFile="Site1.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
<asp:DetailsView ID="dvMethod" runat="server" AutoGenerateRows="false" 
        DefaultMode="ReadOnly" CssClass='detail' GridLines="None">
    <FieldHeaderStyle CssClass="" />
    <HeaderTemplate>
        <table><tr><td><h1> <%# Eval("name")%></h1></td>
        <td>&nbsp; &nbsp;&nbsp; &nbsp;<asp:Button ID="btnView" runat="server" Text="View" onclick="btnView_Click"  /> </td>&nbsp; &nbsp;
        <td><asp:Button ID="btnDownload" runat="server" Text="Download" onclick="btnDownload_Click" /></td>
        <td>(includes all traits listed below)</td></tr></table>
    </HeaderTemplate>
    <EmptyDataTemplate>
    </EmptyDataTemplate>
    <Fields>
        <asp:TemplateField>
            <ItemTemplate>
            <table runat="server" cellpadding='1' cellspacing='1' border='0' class='grid horiz' style='width:600px; border:1px solid black'>
                <tr id="tr_location">
                    <th><%= Page.DisplayText("htmlEvalLoc", "Evaluation location:")%></th>
                    <td><%# Eval("state_name") %>, <%# Eval("country_name")%></td>
                </tr>
                <tr id="tr_method">
                    <th><%= Page.DisplayText("htmlMethod", "Methods:")%></th>
                    <td><%# Eval("methods")%></td>
                </tr>
            </table>
             </ItemTemplate>
        </asp:TemplateField>
    </Fields>
</asp:DetailsView>

<asp:Panel ID="plCitations" runat="server" Visible="False">
<h1><%= Page.DisplayText("htmlCitations", "Citations")%></h1>
<asp:Repeater ID="rptCitations" runat="server">
    <HeaderTemplate>
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li><b><%# Eval("author") %></b> <%# Eval("citation_year")%><%# Eval("citation_year").ToString() == "" ? "" : "."%> <%# Eval("title") %> <%# Eval("abbrev")%> <%# Eval("refernece")%></li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>
</asp:Panel>
<asp:Panel ID="plResearcher" runat="server" Visible="False">
<h1>Researchers:</h1> 
<asp:Repeater ID="rptCoop" runat="server">
    <HeaderTemplate>
        <ul> 
    </HeaderTemplate>
    <ItemTemplate>
        <li><a href="cooperator.aspx?id=<%# Eval("cooperator_id") %>"><%# Eval("last_name") %><%# Eval("last_name").ToString() == "" ? "" : ","%> <%# Eval("first_name")%><%# Eval("first_name").ToString() == "" ? "" : ","%> <%# Eval("organization") %></a></li> 
    </ItemTemplate>
    <FooterTemplate>
       </ul>
    </FooterTemplate>
</asp:Repeater>
</asp:Panel> 
<asp:Panel ID="plTrait" runat="server" Visible="False">
<h1>Traits evaluated:</h1> 
<asp:Repeater ID="rptAccession" runat="server">
    <HeaderTemplate>
        <ul>
    </HeaderTemplate> 
    <ItemTemplate>
       <li><a href="descriptordetail.aspx?id=<%# Eval("crop_trait_id") %>"><%# Eval("coded_name")%></a>&nbsp;  -  &nbsp;(<%# Eval("cnt") %><a href="methodaccession.aspx?id1=<%# Eval("crop_trait_id") %>&id2=<%# Eval("method_id")%> "> Accessions</a>)</li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
    
</asp:Repeater>
</asp:Panel> 
<asp:GridView runat="server" ID="gv1" Visible="False">
    </asp:GridView>
<hr /> 
</asp:Content>