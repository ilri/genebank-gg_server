<%@ Page Title="Cart" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="cartview.aspx.cs" Inherits="GrinGlobal.Web.CartView" %>
<%@ Register TagPrefix="gg" TagName="PivotView" Src="~/pivotviewcontrol.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type='text/javascript'>
        $(document).ready(function() {
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <h1><%= Page.DisplayText("htmlCart", "Shopping Cart")%><asp:Label ID="lblCnt" runat="server" Text=""></asp:Label>
    </h1>
<asp:GridView ID="gvCart" runat="server" ShowFooter="True" 
        AutoGenerateColumns="False" DataKeyNames="accession_id" CssClass="grid" 
        onrowdeleting="gvCart_RowDeleting" onrowdatabound="gvCart_RowDataBound" >
<EmptyDataTemplate>
    <%= Page.DisplayText("htmlnoItem", "You have no items in your cart.")%>
</EmptyDataTemplate>
<Columns>
    <asp:TemplateField HeaderText="Quantity" Visible="false">
    <HeaderStyle CssClass="" />
        <ItemTemplate>
            <asp:TextBox ID="txtQuantity" CssClass='handle2' runat="server" Text='<%# Bind("quantity") %>' MaxLength="4" Width="40px"></asp:TextBox>
        </ItemTemplate>
    </asp:TemplateField> 
    <asp:TemplateField HeaderText="Select">
        <ItemTemplate>
            <asp:CheckBox ID="chkSelect" runat="server" />
        </ItemTemplate>
    </asp:TemplateField>

    <asp:TemplateField HeaderText="ID">
        <ItemTemplate>
            <a href="AccessionDetail.aspx?id=<%# Eval("accession_id") %>"><%#Eval("pi_number") %></a>
        </ItemTemplate>
    </asp:TemplateField>
    <asp:BoundField DataField="top_name" HeaderText="Plant Name" />
    <asp:TemplateField HeaderText="Taxonomy">
        <ItemTemplate>
            <nobr><a href='taxonomydetail.aspx?id=<%#Eval("taxonomy_species_id") %>'><%#Eval("taxonomy_name") %></a></nobr>
        </ItemTemplate>
    </asp:TemplateField>
    <asp:BoundField DataField="standard_distribution_quantity" HeaderText="Distribution Amt" />
    <asp:BoundField DataField="standard_distribution_unit" HeaderText="Distribution Unit" />
    <asp:TemplateField HeaderText="Form Distributed">
        <ItemTemplate>
            <asp:DropDownList runat="server" ID="ddlFormDistributed" DataTextField="display_text" DataValueField="value" AutoPostBack="true" OnSelectedIndexChanged="changedDistributionType"></asp:DropDownList>
            <asp:Label runat="server" ID="lblFormDistributed"></asp:Label>
        </ItemTemplate>
    </asp:TemplateField>
    <asp:TemplateField HeaderText="Maintained by">
                 <ItemTemplate>
                     <a href='site.aspx?id=<%# Eval("site_id") %>'>
                     <%#Eval("site") %></a>
                 </ItemTemplate>
             </asp:TemplateField>
    <asp:TemplateField>
        <ItemTemplate>
            <asp:LinkButton ID="btnRemove" runat="server" CssClass="system" Text="Remove" CommandName="Delete" OnClientClick="javascript:return confirm('Are you sure you want to remove this item?');" ></asp:LinkButton>
        </ItemTemplate>
        <FooterTemplate>
           <asp:LinkButton ID="btnRemoveSelected" runat="server" CssClass="system" Text="Remove Selected" OnClick="btnRemoveSelected_Click" OnClientClick="javascript:return confirm('Are you sure you want to remove all selected items?');" ></asp:LinkButton>
            <br /> <br />
            <asp:LinkButton ID="btnRemoveAll" runat="server" CssClass="system" Text="Remove All" OnClick="btnRemoveAll_Click" OnClientClick="javascript:return confirm('Are you sure you want to remove all items?');" ></asp:LinkButton>
        </FooterTemplate>
    </asp:TemplateField>
</Columns>
</asp:GridView>

<div style='left:5%;'>
    <a href='Search.aspx'><img src='images/btn_searchaccessions.gif' alt='Search for more accessions' style='border:none' /></a><br /><br />
        <span style='right:5%;position:absolute;'>
        <asp:HyperLink ID="hlCheckout" runat="server">
        <img src='images/btn_checkout.gif' alt='Checkout' style='border:none' /></asp:HyperLink> 
    </span>
    <a href='javascript: Back to previous page' onclick='javascript:history.back(); return false;'><img src='images/btn_prevpage.gif' alt='Back to previous page' style='border:none' /></a><br /><br /><br />
    <!--<asp:HyperLink ID="lnkPrevious" runat="server" ImageUrl="~/images/btn_prevpage.gif" ToolTip="Back to previous page" AlternateText="Back to previous page" /> <br /><br />-->
</div>
</asp:Content>
