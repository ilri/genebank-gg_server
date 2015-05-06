<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="client.aspx.cs" Inherits="GrinGlobal.Web.Client" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">

    <h2>Documentation</h2>
    <asp:GridView ID="gvDocuments" runat="server" AutoGenerateColumns="false" 
        onrowdeleting="gvDocuments_RowDeleting" DataKeyNames="Name" CssClass="grid" >
        <AlternatingRowStyle CssClass="altrow" />
        <EmptyDataTemplate>
            There are currently no documentation files available on this server.
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="File">
                <ItemTemplate>
                    <a href='uploads/documents/<%# Eval("Name") %>'><%# Eval("Name") %></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Create Date">
                <ItemTemplate>
                    <%# Eval("CreationTime", "{0:yyyy-MM-dd hh:mm:ss tt}") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="File Size">
                <ItemTemplate>
                    <div style='width:100%;text-align:right'>
                    <%# Eval("Size") %>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:Button ID="Button1" runat="server" CausesValidation="false" 
                        CommandName="Delete" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <p>&nbsp;</p>
    <asp:Panel ID="pnlUploadDocument" runat="server">
    <fieldset style='width:600px'>
    <legend>Upload Documentation File</legend>
        <asp:FileUpload ID="filUploadDocument" runat="server" />
        <br />
        <asp:Button id="btnUploadDocument" runat="server" Text="Upload" 
            onclick="btnUploadDocument_Click"/>
    </fieldset>
    </asp:Panel>


    <h2>Installers</h2>

    <asp:GridView ID="gvExecutables" runat="server" AutoGenerateColumns="false" 
        onrowdeleting="gvExecutables_RowDeleting" DataKeyNames="Name" CssClass="grid" >
        <AlternatingRowStyle CssClass="altrow" />
        <EmptyDataTemplate>
            There are currently no installer files available on this server.
        </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField HeaderText="File">
                <ItemTemplate>
                    <a href='uploads/installers/<%# Eval("Name") %>'><%# Eval("Name") %></a>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Create Date">
                <ItemTemplate>
                    <%# Eval("CreationTime", "{0:yyyy-MM-dd HH:mm:ss tt zzz}") %>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="File Size (MB)">
                <ItemTemplate>
                    <div style='width:100%;text-align:right'>
                    <%# Eval("Size", "{0:###,###,##0.0#}") %>
                    </div>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ShowHeader="False">
                <ItemTemplate>
                    <asp:Button ID="Button1" runat="server" CausesValidation="false" 
                        CommandName="Delete" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete this item?');" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

<p>&nbsp;</p>
    <asp:Panel ID="pnlUploadExecutable" runat="server">
    <fieldset style='width:600px'>
    <legend>Upload Executable File</legend>
        <asp:FileUpload ID="filUploadExecutable" runat="server" />
        <br />
        <asp:Button id="btnUploadExecutable" runat="server" Text="Upload" 
            onclick="btnUploadExecutable_Click" />
    </fieldset>
    </asp:Panel>

    


    
</asp:Content>
