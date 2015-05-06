<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="formeditor.aspx.cs" Inherits="GrinGlobal.Web.feedback.admin.formbuilder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type='text/javascript'>
        $(document).ready(function() {
            $("._anf").click(function() {
                $(this).toggle('fast');
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    
    Form name: <asp:TextBox ID="txtFormName" runat="server"></asp:TextBox>&nbsp;<asp:Button 
        ID="btnSaveForm" runat="server" Text="Save" onclick="btnSaveForm_Click" />
        <asp:Panel ID="pnlMustSaveFirst" runat="server" Visible="false">
            You must give the form a name before you can add fields or traits.
        </asp:Panel>
        <asp:Panel ID="pnlFieldsAndTraits" runat="server" Visible="false">
    <fieldset>
        <legend>Fields</legend>
        <asp:GridView ID="gvFields" runat="server" AutoGenerateColumns="False"
            CssClass="grid" onrowediting="gvFields_RowEditing" 
            DataKeyNames="feedback_form_field_id" onrowcommand="gvFields_RowCommand" 
            onrowdeleted="gvFields_RowDeleted" onrowdatabound="gvFields_RowDataBound" 
            onrowcancelingedit="gvFields_RowCancelingEdit" 
            onrowcreated="gvFields_RowCreated" onrowupdated="gvFields_RowUpdated" 
            onrowdeleting="gvFields_RowDeleting" onrowupdating="gvFields_RowUpdating">
            <Columns>
                <asp:TemplateField HeaderText="Name">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtFieldName" runat="server" Text='<%# Bind("title") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# Bind("title") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Display As">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlFieldDisplayAs" runat="server">
                            <asp:ListItem Value="TEXT_CONTROL" Text="Textbox (free-form)"></asp:ListItem>
                            <asp:ListItem Value="LONGTEXT_CONTROL" Text="Large Textbox (free-form)"></asp:ListItem>
                            <asp:ListItem Value="BOOLEAN_CONTROL" Text="Checkbox"></asp:ListItem>
                            <asp:ListItem Value="INTEGER_CONTROL" Text="Textbox (integers only)"></asp:ListItem>
                            <asp:ListItem Value="DECIMAL_CONTROL" Text="Textbox (decimals only)"></asp:ListItem>
                            <asp:ListItem Value="DATE_CONTROL" Text="Date / Time"></asp:ListItem>
                            <asp:ListItem Value="SMALL_SINGLE_SELECT_CONTROL" Text="Drop Down"></asp:ListItem>
                            <asp:ListItem Value="LARGE_SINGLE_SELECT_CONTROL" Text="Lookup"></asp:ListItem>
                            <asp:ListItem Value="LITERAL_CONTROL" Text="Literal Text"></asp:ListItem>
                            <asp:ListItem Value="H1_CONTROL" Text="Header 1"></asp:ListItem>
                            <asp:ListItem Value="H2_CONTROL" Text="Header 2"></asp:ListItem>
                            <asp:ListItem Value="HR_CONTROL" Text="Horizontal Rule"></asp:ListItem>
                            <asp:ListItem Value="BR_CONTROL" Text="Line Break"></asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblFieldGuiHint" runat="server" Text='<%# Eval("gui_hint_text") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Drop Down Source">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlFieldGroups" runat="server" AppendDataBoundItems="true">
                            <asp:ListItem Value="" Text="- Select -"></asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblFieldDropDownSource" runat="server" Text='<%# Eval("group_name") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Lookup Source">
                    <EditItemTemplate>
                        <asp:DropDownList ID="ddlFieldLookups" runat="server" AppendDataBoundItems="true">
                            <asp:ListItem Value="" Text="- Select -"></asp:ListItem>
                        </asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblFieldLookupSource" runat="server" Text='<%# Eval("foreign_key_dataview_name") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Read Only?">
                    <EditItemTemplate>
                        <asp:CheckBox ID="chkReadOnly" runat="server" Checked='<%# Eval("is_readonly") == "Y" %>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblReadOnly" runat="server" Text='<%# Eval("is_readonly") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Required?">
                    <EditItemTemplate>
                        <asp:CheckBox ID="chkRequired" runat="server" Checked='<%# Eval("is_required") == "Y" %>' />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblRequired" runat="server" Text='<%# Eval("is_required") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Default Value">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtDefaultValue" runat="server" Text='<%# Bind("default_value") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblDefaultValue" runat="server" Text='<%# Bind("default_value") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Categories">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtCategories" runat="server" Text='<%# Bind("category_tag") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblCategories" runat="server" Text='<%# Bind("category_tag") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False" Visible="true">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnUp" runat="server" CausesValidation="false" AlternateText="Up" ImageUrl="~/images/up.gif"
                            CommandName="UP" CommandArgument='<%# Bind("feedback_form_field_id") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False" Visible="true">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnDown" runat="server" CausesValidation="false" AlternateText="Down" ImageUrl="~/images/down.gif"
                            CommandName="DOWN" CommandArgument='<%# Bind("feedback_form_field_id") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowEditButton="True" ButtonType="Image" EditImageUrl="~/images/prefs.gif" UpdateImageUrl="~/images/save.gif" CancelImageUrl="~/images/undo.gif" />
                <asp:CommandField ShowDeleteButton="True" ButtonType="Image" DeleteImageUrl="~/images/remove.gif" />
            </Columns>
        </asp:GridView>
        <fieldset>
            <legend>Add New Field</legend>
            <asp:Label ID="lblFieldName" runat="server">Field Name:</asp:Label>
            <asp:TextBox ID="txtFieldName" runat="server" MaxLength="100" ValidationGroup="fields"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator id="reqFieldName" runat="server" ControlToValidate="txtFieldName" ErrorMessage="Field Name is required" ValidationGroup="fields"></asp:RequiredFieldValidator>
            <br />
            <asp:Label ID="lblDisplayAs" runat="server">Display As:</asp:Label>
            <asp:DropDownList ID="ddlFieldDisplayAs" runat="server" ValidationGroup="fields">
                <asp:ListItem Value="TEXT_CONTROL" Text="Textbox (free-form)"></asp:ListItem>
                <asp:ListItem Value="LONGTEXT_CONTROL" Text="Large Textbox (free-form)"></asp:ListItem>
                <asp:ListItem Value="BOOLEAN_CONTROL" Text="Checkbox"></asp:ListItem>
                <asp:ListItem Value="INTEGER_CONTROL" Text="Textbox (integers only)"></asp:ListItem>
                <asp:ListItem Value="DECIMAL_CONTROL" Text="Textbox (decimals only)"></asp:ListItem>
                <asp:ListItem Value="DATE_CONTROL" Text="Date / Time"></asp:ListItem>
                <asp:ListItem Value="SMALL_SINGLE_SELECT_CONTROL" Text="Drop Down"></asp:ListItem>
                <asp:ListItem Value="LARGE_SINGLE_SELECT_CONTROL" Text="Lookup"></asp:ListItem>
                <asp:ListItem Value="LITERAL_CONTROL" Text="Literal Text"></asp:ListItem>
                <asp:ListItem Value="H1_CONTROL" Text="Header 1"></asp:ListItem>
                <asp:ListItem Value="H2_CONTROL" Text="Header 2"></asp:ListItem>
                <asp:ListItem Value="HR_CONTROL" Text="Horizontal Rule"></asp:ListItem>
                <asp:ListItem Value="BR_CONTROL" Text="Line Break"></asp:ListItem>
            </asp:DropDownList>
            <br />
            <asp:Label ID="lblFieldGroups" runat="server">Drop Down Source:</asp:Label>
            <asp:DropDownList ID="ddlFieldGroups" runat="server" AppendDataBoundItems="true" ValidationGroup="fields">
                <asp:ListItem Value="" Text="- Select -"></asp:ListItem>
            </asp:DropDownList>
            <br />
            <asp:Label ID="lblFieldLookups" runat="server">Lookup Source:</asp:Label>
            <asp:DropDownList ID="ddlFieldLookups" runat="server" AppendDataBoundItems="true" ValidationGroup="fields">
                <asp:ListItem Value="" Text="- Select -"></asp:ListItem>
            </asp:DropDownList><br />
            <asp:CheckBox ID="chkIsReadOnly" runat="server" Text="Read Only" ValidationGroup="fields" />
            <asp:CheckBox ID="chkIsRequired" runat="server" Text="Required" ValidationGroup="fields" /><br />
            Default Value: <asp:TextBox ID="txtDefaultValue" runat="server" ValidationGroup="fields"></asp:TextBox>
            <br />
            Categories (optional): <asp:TextBox ID="txtFieldTags" runat="server" ValidationGroup="fields"></asp:TextBox><br />
            <asp:Button ID="btnAddField" runat="server" Text="Add" 
                onclick="btnAddField_Click" ValidationGroup="fields" />
        </fieldset>
    </fieldset>



    <fieldset>
        <legend>Traits</legend>
        <asp:GridView ID="gvTraits" runat="server" AutoGenerateColumns="False" 
            CssClass="grid" DataKeyNames="feedback_form_trait_id" 
            onrowcancelingedit="gvTraits_RowCancelingEdit" 
            onrowdeleting="gvTraits_RowDeleting" onrowcommand="gvTraits_RowCommand" 
            onrowdatabound="gvTraits_RowDataBound" onrowediting="gvTraits_RowEditing" 
            onrowupdating="gvTraits_RowUpdating">
            <Columns>
                <asp:TemplateField HeaderText="Trait">
                    <EditItemTemplate>
                        Crop: <asp:Label ID="lblCrop" runat="server" Text='<%# Bind("crop_name") %>'></asp:Label><br />
                        Trait: <asp:DropDownList ID="ddlCropTrait" runat="server" DataTextField="display_text" DataValueField="crop_trait_id" ></asp:DropDownList>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblTraitTitle" runat="server" Text='<%# Bind("trait_title") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Type">
                    <EditItemTemplate>
                        <asp:Label ID="lblDataTypeCode" runat="server" Text='<%# Bind("data_type_code") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblDataTypeCode" runat="server" Text='<%# Bind("data_type_code") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Is Coded?">
                    <EditItemTemplate>
                        <asp:Label ID="lblIsCoded" runat="server" Text='<%# Bind("is_coded") %>'></asp:Label>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblIsCoded" runat="server" Text='<%# Bind("is_coded") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Custom Title">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtCustomTitle" runat="server" Text='<%# Bind("custom_title") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblCustomTitle" runat="server" Text='<%# Bind("custom_title") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Custom Description">
                    <EditItemTemplate>
                        <asp:TextBox ID="txtCustomDescription" runat="server" 
                            Text='<%# Bind("custom_description") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblCustomDescription" runat="server" Text='<%# Bind("custom_description") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False" Visible="true">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnUp" runat="server" CausesValidation="false" AlternateText="Up" ImageUrl="~/images/up.gif"
                            CommandName="UP" CommandArgument='<%# Bind("feedback_form_trait_id") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField ShowHeader="False" Visible="true">
                    <ItemTemplate>
                        <asp:ImageButton ID="btnDown" runat="server" CausesValidation="false" AlternateText="Down" ImageUrl="~/images/down.gif"
                            CommandName="DOWN" CommandArgument='<%# Bind("feedback_form_trait_id") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:CommandField ShowEditButton="True" ButtonType="Image" EditImageUrl="~/images/prefs.gif" UpdateImageUrl="~/images/save.gif" CancelImageUrl="~/images/undo.gif" />
                <asp:CommandField ShowDeleteButton="True" ButtonType="Image" DeleteImageUrl="~/images/remove.gif" />
            </Columns>
        </asp:GridView>
        <fieldset>
            <legend>Add New Trait</legend>
            <asp:Label ID="lblCrop" runat="server">Crop Name:</asp:Label>
            <asp:DropDownList ID="ddlCrop" runat="server" DataTextField="name" 
                DataValueField="crop_id" AutoPostBack="True"  CausesValidation="false"
                onselectedindexchanged="ddlCrop_SelectedIndexChanged"></asp:DropDownList>
            <asp:RequiredFieldValidator ID="reqCrop" runat="server" ValidationGroup="traits" ControlToValidate="ddlCrop" ErrorMessage="Crop is required"></asp:RequiredFieldValidator>
            <asp:Panel ID="pnlCropTrait" runat="server">
                <asp:Label ID="lblCropTrait" runat="server">Trait:</asp:Label>
                <asp:DropDownList ID="ddlCropTrait" runat="server" DataTextField="display_text" DataValueField="crop_trait_id" ValidationGroup="traits" ></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="reqCropTrait" runat="server" ValidationGroup="traits" ControlToValidate="ddlCropTrait" ErrorMessage="Trait is required"></asp:RequiredFieldValidator><br />
                Custom Title: <asp:TextBox ID="txtTitle" runat="server" ValidationGroup="traits"></asp:TextBox><br />
                Custom Description: <asp:TextBox ID="txtDescription" runat="server" ValidationGroup="traits"></asp:TextBox><br />
                <asp:Button ID="btnAddTrait" runat="server" Text="Add" 
                    onclick="btnAddTrait_Click" ValidationGroup="traits" />
            </asp:Panel>
        </fieldset>
    </fieldset>
</asp:Panel>
    <p><a href='reporteditor.aspx?id=<%= ViewState["reportID"] %>&programid=<%= ViewState["programID"] %>'>&lt; Back to Report</a></p>
    <p><a href='programeditor.aspx?id=<%= ViewState["programID"] %>'>&lt; Back to Program</a></p>
        <p>&nbsp;</p>
        
</asp:Content>
