<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="participantaccession.aspx.cs" Inherits="GrinGlobal.Web.feedback.participantaccession" %>
<%@ Import Namespace="GrinGlobal.Core" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <p><h1><asp:Label ID="Label1" runat="server" Text="Feedback Summary for Accession:"></asp:Label>&nbsp;<asp:Label 
            ID="lblAccessionNumber" runat="server" Text="{Accession Number}"></asp:Label></h1>
        </p>
<asp:Repeater runat="server" ID="rptBox">
    <ItemTemplate>
        <!-- notice the date formatting in the Eval statements below -->
        <table cellpadding='1' cellspacing='1' border='0' class='grid horiz' style='width:505px; border:1px solid black'>
            <tr>
                <th>Taxon:</th>
                <td><a href="../taxonomydetail.aspx?id=<%# Eval("taxonomy_species_id") %>"><%# Eval("taxonomy_name") %></a></td>
            </tr>
            <tr>
                <th>Top Name:</th>
                <td><%# Eval("top_name") %></td>
            </tr>
            <tr>
                <th>Accession Detail Information:</th>
                <td><a href="../accessiondetail.aspx?id=<%# Eval("accession_id") %>">Click here for details</a></td>
            </tr>
            
        </table>
    </ItemTemplate>
</asp:Repeater>
    <p>
       <h1>Included in Orders:</h1>
         <asp:GridView ID="gvOrders" runat="server" CssClass="grid" AutoGenerateColumns="false" AllowSorting="true">
            <EmptyDataTemplate>
                No orders exist.
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField HeaderText="Order #">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlOrder" NavigateUrl='<%# "~/feedback/participantorder.aspx?id=" + Eval("order_request_id") %>' runat="server" Text='<%# Eval("order_request_id") %>'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Order Date">
                    <ItemTemplate>
                        <%# Eval("ordered_date") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Shipped Date">
                    <ItemTemplate>
                        <%# Eval("completed_date") %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    <p><h1><asp:Label ID="Label2" runat="server" Text="Reports:"></asp:Label></h1>
         <asp:GridView ID="gvReports" runat="server" CssClass="grid" AutoGenerateColumns="false" AllowSorting="true">
            <EmptyDataTemplate>
                No reports exist.
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField HeaderText="Year">
                    <ItemTemplate>
                        <%# Eval("report_year") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Report">
                    <ItemTemplate>
                    <a href="participantresult.aspx?groupid=<%# Eval("feedback_result_group_id") %>"><%# Eval("report_name") %></a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </p>
    <p>
        <h1><asp:Label ID="Label5" runat="server" Text="Attachments:"></asp:Label></h1>
        <asp:Repeater ID="rptAttachments" runat="server">
            <ItemTemplate>
                <asp:Image ID="Image1" runat="server" Height="16px" ImageUrl="~/images/attachment.png" Width="16px" />&nbsp;
                <asp:HyperLink ID="hlAttachment" runat="server" NavigateUrl='<%# Eval("virtual_path") %>' Text='<%# Eval("file_name") %>'></asp:HyperLink>&nbsp;
                (<%# Eval("year") %> - <%# Eval("report_title") %>)<br />
            </ItemTemplate>
        </asp:Repeater>
    </p>
</asp:Content>
