<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="manageprograms.aspx.cs" Inherits="GrinGlobal.Web.feedback.admin.manageprograms" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <h2>Manage Feedback Programs</h2>
    <p>
        <asp:GridView AutoGenerateColumns="false" ID="gvPrograms" runat="server" CssClass="grid">
            <EmptyDataTemplate>
                No programs exist.
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField HeaderText="Program Name">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlProgramName" runat="server" NavigateUrl='<%# "~/feedback/admin/programeditor.aspx?id=" + Eval("feedback_id") %>' Text='<%# Eval("title")%>'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Start Date" DataField="start_date" DataFormatString="{0:MM/dd/yyyy}" />
                <asp:BoundField HeaderText="End Date" DataField="end_date" DataFormatString="{0:MM/dd/yyyy}" />
            </Columns>
        </asp:GridView>
    </p>
    <p><a href='programeditor.aspx?id=-1'>Add New Program</a></p>
    <p>&nbsp;</p>
    <h1>Orders Pending Inclusion</h1>
    <p>
        <asp:GridView AutoGenerateColumns="false" ID="gvOrders" runat="server" 
            CssClass="grid" DataKeyNames="feedback_id, order_request_id, cooperator_id, feedback_result_group_id"
            onrowdatabound="gvOrders_RowDataBound">
            <EmptyDataTemplate>
                No eligible orders exist.
            </EmptyDataTemplate>
            <Columns>
            <asp:TemplateField HeaderText="Include?">
                <ItemTemplate>
                    <asp:CheckBox ID="chkInclude" runat="server" Text="" />
                </ItemTemplate>
            </asp:TemplateField>
                <asp:BoundField DataField="order_request_id" HeaderText="Order #" />
                <asp:BoundField DataField="requestor_name" HeaderText="Requestor Name" />
                <asp:BoundField DataField="title" HeaderText="Program" />
            </Columns>
        </asp:GridView>
    </p>
    <asp:Button ID="btnIncludeInProgram" runat="server" Text="Include in Program" 
        onclick="btnIncludeInProgram_Click" />
    <p>&nbsp;</p>
    <h1>Submissions Pending Review</h1>
    <p>
        <asp:GridView ID="gvSubmissions" runat="server" AutoGenerateColumns="false" 
            DataKeyNames="feedback_result_group_id" CssClass="grid" onselectedindexchanged="gvSubmissions_SelectedIndexChanged" 
            >
            <EmptyDataTemplate>
                No submissions are pending review.
            </EmptyDataTemplate>
            <Columns>
                <asp:BoundField DataField="full_name" HeaderText="Participant" />
                <asp:BoundField DataField="year" HeaderText="Year" />
                <asp:BoundField DataField="title" HeaderText="Report" />
                <asp:BoundField DataField="submitted_date" HeaderText="Date Submitted" DataFormatString="{0:MM/dd/yyyy}" />
                <asp:ButtonField ButtonType="Button" HeaderText="" CommandName="Select" Text="Review" />
            </Columns>
        </asp:GridView>
    </p>
</asp:Content>
