<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="participantorder.aspx.cs" Inherits="GrinGlobal.Web.feedback.participantorder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <p>
    <asp:Repeater runat="server" ID="rptHeader">
    <ItemTemplate>
        <h1>Feedback Summary for Order #: <%# Eval("order_request_id") %></h1>
        <b>Order Date: <%# Eval("ordered_date", "{0:MM/dd/yyyy}") %></b> 
        <br /><br />
    </ItemTemplate>
</asp:Repeater>

   <h1>Accessions in Order</h1>
         <asp:GridView ID="gvAccessions" runat="server" CssClass="grid" AutoGenerateColumns="false" AllowSorting="true">
            <EmptyDataTemplate>
                No accessions exist.
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField HeaderText="Accession #">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlAccession" NavigateUrl='<%# "~/feedback/participantaccession.aspx?id=" + Eval("accession_id") %>' runat="server" Text='<%# Eval("pi_number") %>'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Accession Name">
                    <ItemTemplate>
                        <%# Eval("top_name") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Taxon">
                    <ItemTemplate>
                        <%# Eval("taxonomy_name") %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    
    <p>
        
   <h1>Report Schedule</h1>
         <asp:GridView ID="gvReports" runat="server" CssClass="grid" AutoGenerateColumns="false" AllowSorting="true">
            <EmptyDataTemplate>
                No reports exist.
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField HeaderText="Program">
                    <ItemTemplate>
                        <%#Eval("program_title")%>&nbsp;&nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>                
                <asp:TemplateField HeaderText="Report">
                    <ItemTemplate>
                    <asp:HyperLink ID="hlTitle" NavigateUrl='<%# "~/feedback/participantresult.aspx?groupid=" + Eval("feedback_result_group_id") %>' runat="server" Text='<%# Eval("title") %>'></asp:HyperLink>&nbsp;&nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Due Date">
                    <ItemTemplate>
                        <%#Eval("due_date", "{0:MM/dd/yyyy}")%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
</asp:Content>
