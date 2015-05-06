<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="programeditor.aspx.cs" Inherits="GrinGlobal.Web.feedback.admin.newprogram" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type='text/javascript'>
        $(document).ready(function() {
            $('._byinv input').click(function() {
                if ($(this).is(":checked")) {
                    $('._byinvtgt').show('fast');
                } else {
                $('._byinvtgt').hide('fast');
                }
            }).is(":checked") ? '' : $('._byinvtgt').hide('fast');
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
<h2><asp:Literal ID="litHeading" runat="server">Create New Program</asp:Literal></h2>
    <p><asp:Label ID="lblTitle" runat="server" Text="Title/Name:"></asp:Label>&nbsp;<asp:TextBox 
            ID="txtTitle" runat="server"  Width="400"></asp:TextBox>
    </p>
    <p><asp:Label ID="lblStartDate" runat="server" Text="Start Date:"></asp:Label>
    &nbsp;<asp:TextBox ID="txtStartDate" runat="server" ></asp:TextBox>
    <br />
    <asp:Label ID="lblEndDate" runat="server" Text="End Date:"></asp:Label>&nbsp;<asp:TextBox
            ID="txtEndDate" runat="server" ></asp:TextBox>
    </p>
    <h2>Inventory Lots</h2>
    <p>
        <asp:CheckBox ID="chkByInventory" runat="server" 
            Text="Only active for specified inventory lots" CssClass="_byinv"
            /><br />
        <div class='_byinvtgt'>
            <asp:GridView ID="gvInventory" runat="server" AutoGenerateColumns="False" 
                DataKeyNames="feedback_inventory_id"  CssClass='grid'
                onrowdeleting="gvInventory_RowDeleting">
                <EmptyDataTemplate>
                    No inventory items have been specified
                </EmptyDataTemplate>
                <Columns>
                    <asp:BoundField HeaderText="Inventory" DataField="inventory_number" />
                    <asp:CommandField ShowDeleteButton="True" DeleteText="Remove" ButtonType="Button" />
                </Columns>
            </asp:GridView>
            <asp:LinkButton ID="lnkAddInventory" runat="server" Text="Add Inventory" 
                onclick="lnkAddInventory_Click"></asp:LinkButton>
            <asp:Panel ID="pnlChooseInventory" runat="server" Visible="false" CssClass='grid'>
                <asp:Label ID="lblInventory" runat="server" Text="Search for inventory number:"></asp:Label>&nbsp;<asp:TextBox ID="txtInventory" runat="server"></asp:TextBox>
                &nbsp;<asp:Button ID="btnInventorySearch" runat="server" Text="Find" 
                    onclick="btnInventorySearch_Click" />&nbsp;<asp:Button ID="btnInventorySearchCancel" runat="server" Text="Cancel" 
                    onclick="btnInventorySearchCancel_Click" /><br />
                <asp:GridView ID="gvInventorySearch" runat="server" DataKeyNames="inventory_id"  CssClass='grid'
                    AutoGenerateColumns="false" 
                    onselectedindexchanging="gvInventorySearch_SelectedIndexChanging">
                    <EmptyDataTemplate>
                        No inventory items found for your search criteria.  Please alter and try again.
                    </EmptyDataTemplate>
                    <Columns>
                        <asp:BoundField DataField="inventory_number" />
                        <asp:CommandField ShowSelectButton="true" ButtonType="Button" SelectText="Add" />
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </div>
    <p>
        <h1><asp:Label ID="lblReportsForms" runat="server" Text="Reports/Forms"></asp:Label></h1>
&nbsp;&nbsp;
        <br />
        <asp:GridView ID="gvReports" runat="server" AutoGenerateColumns="false" 
        CssClass='grid' onrowcommand="gvReports_RowCommand" 
        onrowdatabound="gvReports_RowDataBound">
            <EmptyDataTemplate>
                No reports exist.
            </EmptyDataTemplate>
            <Columns>
                <asp:TemplateField HeaderText="Report">
                    <ItemTemplate>
                        <asp:HyperLink NavigateUrl='<%# "~/feedback/admin/reporteditor.aspx?id=" + Eval("feedback_report_id") + "&programid=" + Eval("feedback_id") %>' Text='<%# Eval("report_title") %>' runat="server"></asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Schedule">
                    <ItemTemplate>
                        <%# Eval("due_interval") %> <%# Eval("interval_length_title")%> ; <%# Eval("interval_type_title")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False" Visible="true">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnUp" runat="server" CausesValidation="false" AlternateText="Up" ImageUrl="~/images/up.gif"
                            CommandName="UP" CommandArgument='<%# Bind("feedback_report_id") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False" Visible="true">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnDown" runat="server" CausesValidation="false" AlternateText="Down" ImageUrl="~/images/down.gif"
                            CommandName="DOWN" CommandArgument='<%# Bind("feedback_report_id") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <asp:HyperLink ID="hlNewReport" runat="server" Height="19px" Text="Add New" NavigateUrl='' />
        <p>&nbsp;</p>
        <asp:Button ID="btnSave" runat="server" Text="Save" 
        onclick="btnSave_Click" />
        <p>&nbsp;</p>
        <p><a href='manageprograms.aspx'>&lt; Back to Manage Programs</a></p>
        <p>&nbsp;</p>
</asp:Content>
