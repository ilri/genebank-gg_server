<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="manageforms.aspx.cs" Inherits="GrinGlobal.Web.feedback.admin.manageforms" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <h2>Manage Feedback Forms</h2>
    <p>
        <asp:GridView AutoGenerateColumns="False" ID="gvForms" runat="server"  DataKeyNames="feedback_form_id"
            CssClass="grid" onrowdeleting="gvForms_RowDeleting">
            <EmptyDataTemplate>
                No programs exist.
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField HeaderText="Form Name">
                    <ItemTemplate>
                        <asp:HyperLink ID="hlFormName" runat="server" NavigateUrl='<%# "~/feedback/admin/formeditor.aspx?id=" + Eval("feedback_form_id") %>' Text='<%# Eval("title")%>'></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Created Date" DataField="created_date" DataFormatString="{0:MM/dd/yyyy}" />
                <asp:BoundField HeaderText="Created By" DataField="created_by_name" />
                <asp:CommandField ShowDeleteButton="True" ButtonType="Image" DeleteImageUrl="~/images/remove.gif" />
            </Columns>
        </asp:GridView>
    </p>
    <p><a href='formeditor.aspx?id=-1'>Add New Form</a></p>
    <p>&nbsp;</p>
</asp:Content>
