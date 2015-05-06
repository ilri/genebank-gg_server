<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="reporteditor.aspx.cs" Inherits="GrinGlobal.Web.feedback.admin.reportnew" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type='text/javascript'>
        $(document).ready(function() {
            $('._attach').click(function() {
                $('._attachtgt').toggle('fast');
            });
            $('._init input').click(function() {
                if ($(this).is(":checked")) {
                    $('._initialemail').show('fast');
                } else {
                    $('._initialemail').hide('fast');
                }
            }).is(":checked") ? '' : $('._initialemail').hide('fast');


            $('._15 input').click(function() {
                if ($(this).is(":checked")) {
                    $('._15tgt').show('fast');
                } else {
                    $('._15tgt').hide('fast');
                }
            }).is(":checked") ? '' : $('._15tgt').hide('fast');

            $('._30 input').click(function() {
                if ($(this).is(":checked")) {
                    $('._30tgt').show('fast');
                } else {
                    $('._30tgt').hide('fast');
                }
            }).is(":checked") ? '' : $('._30tgt').hide('fast');

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <p><h2><asp:Literal ID="litHeading" runat="server">Create New Report</asp:Literal></h2></p>
    <p>
        <asp:Label ID="lblTitle" runat="server" Text="Title:"></asp:Label>
&nbsp;<asp:TextBox ID="txtTitle" runat="server" Width="371px"></asp:TextBox>
    </p>
    <p>
        <asp:Label ID="lblSchedule" runat="server" Text="Schedule:"></asp:Label>
&nbsp;
        <br />
        Report is due every
        <asp:DropDownList ID="ddlDue" runat="server">
            <asp:ListItem></asp:ListItem>
            <asp:ListItem Selected="True">1</asp:ListItem>
            <asp:ListItem>2</asp:ListItem>
            <asp:ListItem>3</asp:ListItem>
            <asp:ListItem>4</asp:ListItem>
            <asp:ListItem>5</asp:ListItem>
            <asp:ListItem>6</asp:ListItem>
            <asp:ListItem>7</asp:ListItem>
            <asp:ListItem>8</asp:ListItem>
            <asp:ListItem>9</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
        </asp:DropDownList>
&nbsp;<asp:DropDownList ID="ddlInterval" runat="server">
        </asp:DropDownList>
&nbsp;from the
        <asp:DropDownList ID="ddlStartDate" runat="server" AutoPostBack="true" 
            onselectedindexchanged="ddlStartDate_SelectedIndexChanged">
        </asp:DropDownList>
        <asp:TextBox ID="txtCustomDate" runat="server" Visible="false"></asp:TextBox>
    </p>
    <p>
        <asp:CheckBox ID="chkIsObservationData" runat="server" 
            Text="Is this observation data? If so, should it be available to public searches?" />
    &nbsp;<asp:DropDownList ID="ddlYesNo" runat="server">
            <asp:ListItem Value="Y" Text="Yes"></asp:ListItem>
            <asp:ListItem Value="N" Text="No"></asp:ListItem>
        </asp:DropDownList>
    </p>
    <p>
        Form:
        <asp:DropDownList ID="ddlForms" runat="server" DataTextField="title" DataValueField="feedback_form_id" AppendDataBoundItems="true">
        </asp:DropDownList>&nbsp;<asp:LinkButton ID="lnkEditForm" runat="server" 
            NavigateUrl='' Text="Edit form" onclick="lnkEditForm_Click"></asp:LinkButton>&nbsp;<asp:LinkButton ID="lnkAddForm" runat="server" 
            NavigateUrl='' Text="Add New Form" onclick="lnkAddNewForm_Click"></asp:LinkButton></p><p>
    </p>
        <p>
            <h1><asp:Label ID="lblNotificationSchedule" runat="server" Text="Notification Schedule"></asp:Label></h1>
            <asp:CheckBox ID="chkSendInitialEmail" runat="server" Text="Send Initial Email" CssClass="_init" />
            <br />
        </p>
    <div class='_initialemail'>
        Subject:<br /><asp:TextBox ID="txtInitialEmailSubject" runat="server" Width="574px"></asp:TextBox><br />
            Body:<br /><asp:TextBox ID="txtInitialEmail" runat="server" Height="151px" TextMode="MultiLine"
                Width="574px"></asp:TextBox>
            <br />
        <p>
            <h2>Attachments</h2>
            <asp:GridView CssClass='grid' ID="gvAttachments" runat="server" DataKeyNames="feedback_report_attach_id"
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
            <a href='#_nowhere' class='_attach'>Add Attachment</a>
            <div class='_attachtgt hide'>
                <asp:FileUpload ID="fuAttachment" runat="server" />
                &nbsp;(10 MB maximum)<br />
                Title: <asp:TextBox ID="txtAttachmentTitle" runat="server" MaxLength="50" ></asp:TextBox>
                &nbsp;<asp:Button ID="btnUploadAttachment" runat="server" Text="Upload" 
                    onclick="btnUploadAttachment_Click" />
                    <a href='#_nowhere' class='_attach'>Cancel</a>
            </div>
        </p>
    </div>
    <p>
        <asp:CheckBox ID="chkSend30DayNotice" runat="server" CssClass="_30"
            Text="Send 30-day notice of report due" /><br />
            <div class='_30tgt'>
        Subject:<br /><asp:TextBox ID="txt30DayEmailSubject" runat="server" Width="574px"></asp:TextBox><br />
        Body:<br /><asp:TextBox ID="txt30DayEmail" runat="server" Height="151px" TextMode="MultiLine"
            Width="574px"></asp:TextBox>
            </div>
    </p>
    <p>
        <asp:CheckBox ID="chkSend15DayNotice" runat="server" CssClass="_15"
            Text="Send 15-day notice of report due" /><br />
            <div class='_15tgt'>
        Subject:<br /><asp:TextBox ID="txt15DayEmailSubject" runat="server" Width="574px"></asp:TextBox><br />
        Body:<br /><asp:TextBox ID="txt15DayEmail" runat="server" Height="151px" TextMode="MultiLine"
            Width="574px"></asp:TextBox>
            </div>
    </p>
    <p>
            <asp:Button ID="btnSave" runat="server" Text="Save" onclick="btnSave_Click" />
    </p>
    <p>
        <a href='programeditor.aspx?id=<%= ViewState["programID"] %>'>&lt; Back to Program</a>
    </p>
</asp:Content>
