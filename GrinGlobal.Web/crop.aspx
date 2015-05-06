<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="crop.aspx.cs" Inherits="GrinGlobal.Web.crop" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:DetailsView ID="dvCrop" runat="server" AutoGenerateRows="false" DefaultMode="ReadOnly" CssClass='detail' GridLines="None">
    <FieldHeaderStyle CssClass="" />
    <HeaderTemplate>
        <h1> <%# Eval("name")%></h1>
    </HeaderTemplate>
    <EmptyDataTemplate>
        No crop data found
    </EmptyDataTemplate>
    <Fields>
        <asp:TemplateField>
            <ItemTemplate>
            <table id="Table1" runat="server" cellpadding='1' cellspacing='1' border='0'>
            <tr id="tr_note">
                 <td><%# Eval("note") %> </td>
            </tr>
        </table>
        </ItemTemplate>
        </asp:TemplateField>
    </Fields>
</asp:DetailsView>  
<br />
&nbsp;&nbsp; <asp:Label ID="lblLinks" runat="server"></asp:Label>
<br />
<asp:Panel ID="plLink" runat="server" Visible="False">
<asp:Repeater ID="rptLink" runat="server">
    <HeaderTemplate>
        <br />
    </HeaderTemplate> 
    
    <ItemTemplate>
       &nbsp;&nbsp;&nbsp;<a href="<%# Eval("virtual_path") %>" target='_blank'><%# Eval("description")%></a> <br />
    </ItemTemplate>
    <FooterTemplate>
   
    </FooterTemplate>
</asp:Repeater>
</asp:Panel>

<asp:Panel ID="plImage" runat="server" Visible="False">
&nbsp;&nbsp;<asp:Image ID="image1" runat="server" Width="500" /><br />
&nbsp;&nbsp;<asp:Label ID="lblimg1" runat="server" Text=""></asp:Label> 
</asp:Panel>
<asp:Panel ID="plImage2" runat="server" Visible="False">
&nbsp;&nbsp;<asp:Image ID="image2" runat="server" Width="500" /> 
<asp:Label ID="lblimg2" runat="server" Text="" ></asp:Label> 
</asp:Panel>

<br />
<hr /> 
</asp:Content>