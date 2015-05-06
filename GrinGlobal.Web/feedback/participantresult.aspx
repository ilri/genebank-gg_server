<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="participantresult.aspx.cs" Inherits="GrinGlobal.Web.feedback.participantresult" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type='text/javascript'>
    $(document).ready(function() {
        $('._attach').click(function() {
            $('._attachtgt').toggle('fast');
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <h2>Submit Results for <asp:Literal ID="litReport" runat="server"></asp:Literal> (<asp:Literal ID="litProgram" runat="server"></asp:Literal>)</h2>
<asp:DropDownList ID="ddlAccession" runat="server" DataTextField="pi_number" 
        DataValueField="result_and_inventory" AutoPostBack="True"  CausesValidation="false"
        onselectedindexchanged="ddlAccession_SelectedIndexChanged"></asp:DropDownList>
<h3>General Information</h3>
<asp:Label ID="lblReportName" runat="server"></asp:Label>
<table border='0' cellpadding='1' cellspacing='1'>
<asp:Repeater ID="rptFields" runat="server" 
        onitemdatabound="rptFields_ItemDataBound" 
        onitemcommand="rptFields_ItemCommand">
    <ItemTemplate>
        <tr>
        <td>
            <asp:Literal ID="litField" runat="server" Visible="false"></asp:Literal><asp:Label ID="lblTitle" runat="server"></asp:Label><asp:Label ID="lblRequired" runat="server" Text=" *" Visible="false"></asp:Label>
        </td>
        <td>
        <asp:HiddenField ID="hidFieldType" runat='server' />
        <asp:HiddenField ID="hidFieldResultID" runat="server" />
        <asp:HiddenField ID="hidFieldFormID" runat="server" />
        <asp:HiddenField ID="hidLookupID" runat="server" />
        <asp:CheckBox ID="chkField" runat="server" Visible="false" />
        <asp:TextBox ID="txtField" runat="server" MaxLength="500" Width="150"></asp:TextBox>
        <asp:DropDownList ID="ddlField" runat="server"></asp:DropDownList>
        <asp:Button ID="btnField" runat="server" Text="..." />
        <asp:Panel ID="pnlLookup" runat="server" Visible="false" BorderStyle="Groove">
            Look for: <asp:TextBox ID="txtLookup" runat="server" MaxLength="50"></asp:TextBox>&nbsp;<asp:Button ID="btnLookup" runat="server" Text="Find" OnClick="btnLookup_Click" />&nbsp;<asp:Button ID="btnLookupCancel" runat="server" Text="Cancel" OnClick="btnLookupCancel_Click" /><br />
            <asp:DropDownList ID="ddlItems" runat="server" DataTextField="display_member" DataValueField="value_member"></asp:DropDownList>&nbsp;<asp:Button ID="btnLookupSelected" runat="server" Text="Select" OnClick="btnLookupSelected_Click" />
        </asp:Panel>
        <asp:Calendar id="calField" runat="server"></asp:Calendar>
        <asp:RequiredFieldValidator ID="reqField" ControlToValidate="txtField" runat="server" EnableClientScript="true" ErrorMessage="Field is required" Enabled="false" Visible="false"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="revFieldInteger" ControlToValidate="txtField" runat="server" EnableClientScript="true" ErrorMessage="Only numbers may be entered." Enabled="false" ValidationExpression="[0-9]*" Visible="false">
        </asp:RegularExpressionValidator>
        <asp:RegularExpressionValidator ID="revFieldDecimal" ControlToValidate="txtField" runat="server" EnableClientScript="true" ErrorMessage="Only numbers or decimal point may be entered." Enabled="false" ValidationExpression="[0-9.]*" Visible="false">
        </asp:RegularExpressionValidator>
        &nbsp;&nbsp;&nbsp;<asp:Label ID="lblDescription" runat="server"></asp:Label>
        </td>
        </tr>
    </ItemTemplate>
</asp:Repeater>
</table>
<h3>Observations</h3>
<table border='0' cellpadding='1' cellspacing='1'>
<asp:Repeater ID="rptTraits" runat="server" 
        onitemdatabound="rptTraits_ItemDataBound">
    <ItemTemplate>
        <tr>
            <td>
                <asp:Label ID="lblTitle" runat="server"></asp:Label>
            </td>
            <td>
                <asp:HiddenField ID="hidTraitType" runat='server' />
                <asp:HiddenField ID="hidTraitResultID" runat="server" />
                <asp:HiddenField ID="hidFormTraitID" runat="server" />
                <asp:HiddenField ID="hidCropTraitID" runat="server" />
                <asp:TextBox ID="txtTrait" runat="server" MaxLength="500" Width="150"></asp:TextBox>
                <asp:DropDownList ID="ddlTrait" runat="server"></asp:DropDownList>
            </td>
            <td width="50%">
                <asp:Label ID="lblDescription" runat="server"></asp:Label>
            </td>
        </tr>
    </ItemTemplate>
</asp:Repeater>
 </table>
 
 
     <p>
        <h2>Attachments</h2>
        <asp:GridView CssClass='grid' ID="gvAttachments" runat="server" DataKeyNames="feedback_result_attach_id"
            AutoGenerateColumns="False" onrowdeleting="gvAttachments_RowDeleting">
            <EmptyDataTemplate>
                No attachments exist.
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField HeaderText="File">
                    <ItemTemplate>
                        <asp:Image ID="Image1" runat="server" Height="16px" ImageUrl="~/images/attachment.png" Width="16px" />&nbsp;
                        <asp:HyperLink ID="hlAttach" runat="server" NavigateUrl='<%#Eval("virtual_path") %>' Text='<%# Eval("title") %>'></asp:HyperLink><br />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowDeleteButton="True" />
            </Columns>
        </asp:GridView>
        <br />
        <a href='#_nowhere' class='_attach'>Add Attachment</a><br />
        <div class='_attachtgt hide'>
            <asp:FileUpload ID="fuAttachment" runat="server" />
            &nbsp;(10 MB maximum)<br />
            Title: <asp:TextBox ID="txtAttachmentTitle" runat="server" MaxLength="50"></asp:TextBox>
            &nbsp;<asp:Button ID="btnUploadAttachment" runat="server" Text="Upload"  CausesValidation='false'
                onclick="btnUploadAttachment_Click" />
                <a href='#_nowhere' class='_attach'>Cancel</a>
            </div>
        </p>

 <asp:ValidationSummary id="valSummary" runat="server" ShowMessageBox="true" ShowSummary="true" DisplayMode="BulletList" EnableClientScript="true" />
<asp:Button ID="btnSubmit" runat="server" Text="Submit" onclick="btnSubmit_Click" />
<asp:Button ID="btnSaveForLater" runat="server" Text="Save For Later" onclick="btnSaveForLater_Click" CausesValidation='false' />
<p>&nbsp;</p>
<asp:Label id="lblHasBeenSubmitted" runat="server" Text="These results have already been submitted and cannot be changed." Font-Bold="true" Font-Size="Large" Visible="false"></asp:Label>
<p>&nbsp;</p>
</asp:Content>
