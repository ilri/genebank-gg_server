<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="cooperator.aspx.cs" Inherits="GrinGlobal.Web.cooperator" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type='text/javascript'>
        $(document).ready(function() {
         });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:DetailsView ID="dvCooperator" runat="server" AutoGenerateRows="false" DefaultMode="ReadOnly" CssClass='detail' GridLines="None">
    <FieldHeaderStyle CssClass="" />
    <HeaderTemplate>
        <h1> &nbsp; <%= Page.DisplayText("htmlCoop", "Cooperator")%>:</h1>
    </HeaderTemplate>
    <EmptyDataTemplate>
         <%= Page.DisplayText("htmlCoopNoDisplay", "Due to federal privacy laws we cannot display information about this cooperator")%>
    </EmptyDataTemplate>
    <Fields>
        <asp:TemplateField>
            <ItemTemplate>
            <table id="Table1" runat="server" cellpadding='1' cellspacing='1' border='0'>
            <tr id="tr_name">
                 <td><%# Eval("title") %> <%# Eval("title").ToString() == "" ? "" : " "%> <%# Eval("first_name") %> <%# Eval("last_name") %></td>
            </tr>
            <tr id="tr_organization">
                <td><%# Eval("organization") %></td>
            </tr>
            <tr id="tr_add1">
                 <td><%# Eval("address_line1")%></td>
            </tr>
            <tr id="tr_add2">
                 <td><%# Eval("address_line2")%></td>
            </tr>
            <tr id="tr_add3">
                 <td><%# Eval("address_line3")%></td>
            </tr>
            <tr id="tr_city">
                 <td><%# Eval("city") %>,&nbsp;<%# Eval("state") %></td>
            </tr>
            <tr id="tr_zip">
                 <td><%# Eval("country") %> <%# Eval("postal_index")%> </td>
            </tr>
            <tr><td><br /></td></tr>
            <tr id="tr_phone">
                 <td>Primary Phone: <%# Eval("primary_phone") %></td>
            </tr>
            <tr id="tr_phone2">
                 <td>Secondary Phone: <%# Eval("secondary_phone") %></td>
            </tr>
            <tr id="tr_fax">
                 <td>Fax: <%# Eval("fax") %></td> 
            </tr>
            <tr id="tr_email">
                 <td>Email: <%# Eval("email") %></td>
            </tr><tr><td><br /></td></tr>
        </table>
        </ItemTemplate>
        </asp:TemplateField>
    </Fields>
</asp:DetailsView>
&nbsp;<asp:HyperLink ID="hlRecent" runat="server" Visible="False" Font-Bold="True">Link to Most Recent Address<br /><br /></asp:HyperLink>
  <br />
 <asp:Panel ID="pnlMethods" runat="server">
<a href='#' onclick='javascript:$("#divCoopMethod").toggle("fast");return false;' style='font-weight: bold;'><%= Page.DisplayText("htmlCoopMethods", "Participated in Evaluations:")%> </a>   
 <br /><br />
 <div id='divCoopMethod'>
    <asp:Repeater ID="rptMethods" runat="server">
        <ItemTemplate>
         &nbsp; <%# Eval("Row_Counter")%>.&nbsp;&nbsp;<%# Eval("method")%> <br /> 
        </ItemTemplate>
</asp:Repeater>
</div> <br />
</asp:Panel>

 <asp:Panel ID="pnlAccessions" runat="server">
<a href='#' onclick='javascript:$("#divCoopAsSource").toggle("fast");return false;' style='font-weight: bold;'><%= Page.DisplayText("htmlCoopAsSource", "Source of accessions ")%> </a> 
     <asp:Label ID="lblTotal" runat="server" Text="" Font-Bold="True"></asp:Label>  
<br /><br />
 <div id='divCoopAsSource'>
    <asp:Repeater ID="rptAccessions" runat="server">
        <ItemTemplate>
         &nbsp; <%# Eval("Row_Counter")%>.&nbsp;&nbsp;<%# Eval("pi_number")%> &nbsp;<%# Eval("taxonomy_name")%>  <br /> 
        </ItemTemplate>
</asp:Repeater>
</div> 
</asp:Panel>
<br />
<hr /> 
</asp:Content>