<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="descriptordetail.aspx.cs" Inherits="GrinGlobal.Web.descriptordetail" %>
<%@ Register src="imagecontrol.ascx" tagname="ImageControl" tagprefix="gg" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">

    <div id='main-wrapper'>
    <div id='sidebar-wrapper2' class="images">
		  <gg:ImageControl ID="imagePreviewer" runat="server" />
    </div>
    <asp:DetailsView ID="dvDescriptor" runat="server" AutoGenerateRows="false" DefaultMode="ReadOnly" CssClass='detail' GridLines="None">
    <FieldHeaderStyle CssClass="" />
    <HeaderTemplate>
        <table><tr><td><h1><%= Page.DisplayText("htmlDescriptor", "Descriptor")%>: <%# Eval("descriptor_sname")%></h1></td>
        <td>&nbsp; &nbsp;&nbsp; &nbsp;<asp:Button ID="btnDownload" runat="server" Text="Download this trait" onclick="btnDownload_Click"  /></td></tr></table>
    </HeaderTemplate>
    <EmptyDataTemplate>
        No Descriptor detail data found
    </EmptyDataTemplate>
    <Fields>
        <asp:TemplateField>
            <ItemTemplate>
            <table id="Table1" runat="server" cellpadding='1' cellspacing='1' border='0' class='grid horiz' style='width:600px; border:1px solid black'>
            <tr>
                <th><%= Page.DisplayText("htmlDefinitiono", "Definition")%>:</th>
                <td><%# Eval("descriptor_definition") %></td>
            </tr>
            <tr>
                <th><%= Page.DisplayText("htmlCrop", "Crop")%>:</th>
                <td><a href="crop.aspx?id=<%# Eval("crop_id") %>"><%# Eval("crop_name") %></a></td>
            </tr>
            <tr>
                <th><%= Page.DisplayText("htmlCategory", "Category")%>:</th>
                <td><%# Eval("category_name") %></td>
            </tr>
            <tr>
                <th><%= Page.DisplayText("htmlStatus", "Status")%>:</th>
                <td><%# Eval("approve_status") %></td>
            </tr>
            <tr>
                <th><%= Page.DisplayText("htmlDataType", "Data Type")%>:</th>
                <td><%# Eval("data_type") %></td>
            </tr>
            <tr>
                <th><%= Page.DisplayText("htmlMaximum", "Maximum Length")%>:</th>
                <td><%# Eval("maximum_length") %></td>
            </tr>
            <tr id="tr_nformat">
                <th><%= Page.DisplayText("htmlDataFormat", "Data Format")%>:</th>
                <td><%# Eval("data_format") %></td>
            </tr>
            <tr id="tr_nminimum">
                <th><%= Page.DisplayText("htmlMinimumValue", "Minimum value")%>:</th>
                <td><%# Eval("numeric_minimum") %></td>
            </tr>
            <tr id="tr_nmaximum">
                <th><%= Page.DisplayText("htmlMaximumValue", "Data Format")%>:</th>
                <td><%# Eval("numeric_maximum")%></td>
            </tr>
            <tr id="tr_original_type">
                <th><%= Page.DisplayText("htmlOriginalType", "Original data type")%>:</th>
                <td><%# Eval("original_value_type_code")%></td>
            </tr>
            <tr id="tr_original_format">
                <th><%= Page.DisplayText("htmlOriginalFormat", "Original data format")%>:</th>
                <td><%# Eval("original_value_format")%></td>
            </tr>
            <tr id="tr_ontology">
                <th><%= Page.DisplayText("htmlOntology", "Ontology")%>:</th>
                <td><%# Eval("ontology_url")%></td>
            </tr>
            <tr id="tr_note">
                <th><%= Page.DisplayText("htmlComment", "Comment")%>:</th>
                <td><%# Eval("note") %></td>
            </tr>
            <tr>
                <th><%= Page.DisplayText("htmlResponsibleSite", "Responsible site")%>:</th>
                <td><%# Eval("site_long_name")%>&nbsp;(<a href="site.aspx?id=<%# Eval("site_id") %>"><%# Eval("site_short_name")%></a>)</td>
            </tr>
        </table>
    </ItemTemplate>
    </asp:TemplateField>
    </Fields>
</asp:DetailsView>  
<br />

<asp:Panel ID="plMethod" runat="server" Visible="False">
<h1><%= Page.DisplayText("htmlMethod", "Studies or environments for this trait")%></h1>
<asp:Repeater ID="rptMethod" runat="server">
    <HeaderTemplate>
        <ul>
    </HeaderTemplate>
    <ItemTemplate>
        <li><a href="method.aspx?id=<%# Eval("method_id") %>"><%# Eval("name") %></a>&nbsp;  -  &nbsp;(<%# Eval("cnt")%><a href="methodaccession.aspx?id1=<%# Eval("crop_trait_id") %>&id2=<%# Eval("method_id")%> "> Accessions</a>)</li> 
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>
</asp:Panel>
 
<asp:Panel ID="plLink" runat="server" Visible="False">
<h1><%= Page.DisplayText("htmlOther", "Other information about the descriptor")%></h1>
<asp:Repeater ID="rptLink" runat="server">
    <HeaderTemplate>
        <ul>
    </HeaderTemplate> 
    <ItemTemplate>
       <li><a href="<%# Eval("virtual_path") %>" target='_blank'><%# Eval("description")%></a> <b>Comment: </b><%# Eval("note")%></li>
    </ItemTemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:Repeater>
</asp:Panel>
<br />
<h1><asp:Label ID="lblName" runat="server"></asp:Label></h1>
    <asp:GridView ID="gvCodeValue" runat="server" AutoGenerateColumns="true" CssClass="grid" onrowdatabound="gvCodeValue_RowDataBound" onrowcreated="gvCodeValue_RowCreated">
    </asp:GridView>

<asp:HiddenField ID="hf1" runat="server" />
<asp:GridView ID="gv1" runat="server" Visible="False">
</asp:GridView>  
<hr /> 
 </div>
</asp:Content>