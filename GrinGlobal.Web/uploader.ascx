<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uploader.ascx.cs" Inherits="GrinGlobal.Web.Uploader" %>
<fieldset style='width:556px'>
    <legend>Upload&nbsp; File</legend>
<asp:FileUpload ID="uploadAttachments" runat="server" Width="522px" />&nbsp;<br />
<asp:Button
    ID="btnUpload" runat="server" Text="Upload" Font-Bold="True" OnClick="btnUpload_Click" />
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
Note: To save and upload a document, the <strong>upload</strong> button
must be pressed.
<br />
</fieldset>
<p style="color:Red"><asp:Label ID="lblError" runat="server" Visible="False" Width="550px"></asp:Label></p>
<asp:GridView id="gvAttachments" runat="server" OnRowDeleting="gvAttachments_RowDeleting" OnRowCommand="gvAttachments_RowCommand" 
DataKeyNames="FilePath" AutoGenerateColumns="False" class='grid'>
 <EmptyDataTemplate>
    There are currently no uploaded files for this order request.
</EmptyDataTemplate>

    <Columns>
        <asp:TemplateField HeaderText="File Name">
            <ItemTemplate>
                <a href = "#" onclick="window.open('<%# getAttachmentsUrl( (string) Eval("filepath"))%> ')">
                    <%# getAttachmentName((string)Eval("filepath"))%>
                </a>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Upload Timestamp">
            <ItemTemplate>
                <%# Eval("uploadDate", "{0:yyyy-MM-dd hh:mm:ss tt}")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:linkButton ID="btnDelete" Text="Delete" runat="server" OnClientClick="return confirm('Are you sure you want to delete this attachment?');"
                    CommandName="Delete" CommandArgument='<%# Eval("ID") %>' ></asp:linkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <HeaderStyle Font-Bold="True" />
</asp:GridView>
