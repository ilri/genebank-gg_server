<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="participantdefault.aspx.cs" Inherits="GrinGlobal.Web.feedback.participantdefault" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <p>
        <h1><asp:Label ID="lblWelcome" runat="server" Text="Welcome {NAME}!"></asp:Label></h1>
    </p>
    <p>
        &nbsp;
    </p>
    <p>
    <h1>Report Overview</h1>
    <asp:GridView ID="gvParticipantReports2" runat="server" CssClass="grid" AutoGenerateColumns="false" AllowSorting="true">
            <EmptyDataTemplate>
                No reports are due.
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField HeaderText="Report">
                    <ItemTemplate>
                        <a href="participantresult.aspx?groupid=<%# Eval("feedback_result_group_id") %>"><%# Eval("title") %> (<%# Eval("program_name") %>)</a>&nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Order # (Year)">
                    <ItemTemplate>
                        <a href="participantorder.aspx?id=<%# Eval("order_request_id") %>"><%# Eval("order_request_id") %> (<%# Eval("report_year") %>)</a>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Due Date">
                    <ItemTemplate>
                        <%# Eval("due_date", "{0:MM/dd/yyyy}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                               <asp:TemplateField HeaderText="Status">
                    <ItemTemplate>
                        <%# Eval("status")%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </p>
    <p>
        &nbsp;</p>
    <p>
        <h1>Accession Overview</h1>
        <asp:GridView ID="gvAccessions" runat="server" CssClass="grid" AutoGenerateColumns="false" AllowSorting="true">
            <EmptyDataTemplate>
                No accessions exist.
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField HeaderText="Order #">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlOrder" NavigateUrl='<%# "~/feedback/participantorder.aspx?id=" + Eval("order_request_id") %>' runat="server" Text='<%#Eval("order_request_id") %>'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Accession #">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlAccession" NavigateUrl='<%# "~/feedback/participantaccession.aspx?id=" + Eval("accession_id") %>' runat="server" Text='<%#Eval("pi_number") %>'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Top Name">
                    <ItemTemplate>
                        <%#Eval("top_name") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Taxon">
                    <ItemTemplate>
                        <%#Eval("taxonomy_name") %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </p>
</asp:Content>
