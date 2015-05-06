<%@ Page Title="Descriptors" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="descriptors.aspx.cs" Inherits="GrinGlobal.Web.Descriptors" MaintainScrollPositionOnPostback="True" ValidateRequest="False" %>
<%@ Register TagPrefix="gg" TagName="PivotView" Src="~/pivotviewcontrol.ascx" %>
<%@ Register Src="~/searchcriteriacontrol.ascx" TagPrefix="gg" TagName="SearchCriteria" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    <script type='text/javascript'>
        $(document).ready(function() {
            $('#choose-crop').click(function() {
                $('#panel-crop').toggle('fast');
            });
            $('#choose-descriptor').click(function() {
                $('#panel-descriptor').toggle('fast');
            });
            $('#select-descriptor-value').click(function() {
                $('#panel-select').toggle('fast');
            });
            $('#add-passport').click(function() {
                $('#divPassport').toggle('fast');
            });

            $('#show-results').click(function() {
                $('#panel-results').toggle('fast');
            });

            $('#<%= DefaultDisplayPanel() %>').show();
        });
        
        function actionItemClicked(kind) {
            var pks = pivotView.rememberPrimaryKeys('checked');

            if (pks.length == 0) {
                alert("You must select at least one accession from the list.");
                $("#divActions").hide("fast");
                return false;
            }
            return true;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
        <h3 ><%= Page.DisplayText("htmlChooseCrop", "Choose Crop")%>:   
           &nbsp;&nbsp; <asp:Label ID="lblCrop" runat="server" Text="" ToolTip="Go to crop page"></asp:Label></h3>
                <div id='panel-crop2' class=''>
                    <asp:DropDownList ID="ddlCrops" runat="server" DataTextField="name" 
                    DataValueField="crop_id" CssClass="ddl_crops" 
                    onselectedindexchanged="ddlCrops_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>
                       <asp:Button ID="btnResetAll" runat="server" 
                Text="New Search" onclick="btnResetAll_Click" 
        Width="79px" />
        </div>
    <br />
    <asp:Panel ID="pnlTraits" runat="server" Visible="false">
        <h3 ><a href='#' id='choose-descriptor'><%= Page.DisplayText("htmlChooseDesc", "Choose descriptor(s)")%>:</a>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnResetDescriptorChoice" runat="server" 
                Text="Clear Descriptor Choices" onclick="ddlCrops_SelectedIndexChanged" 
        Width="155px" /></h3>
        <div id='panel-descriptor' class='hide'>
            <asp:DataList ID="dlListTraits" runat="server"  RepeatDirection="Vertical" 
                RepeatColumns="6" DataKeyField="category_code" 
                onitemdatabound="dlListTraits_ItemDataBound"
                onitemcommand="dlListTraits_ItemCommand" >
                <HeaderTemplate>
                    
                <table>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr><td>
                    <fieldset>
                        <h4><%# Eval("display_text") %> <br /><asp:Button ID="btnCheckAllDesc" runat="server" Text=""/>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Button ID="btnClearAllDesc" runat="server" Text=""/></h4>
                        <asp:CheckBoxList ID="chkListTraits" runat="server" RepeatColumns="5" RepeatDirection="Vertical" DataTextField="display_text" DataValueField="crop_trait_id"></asp:CheckBoxList>
                    </fieldset>
                </td></tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:DataList>
            <asp:Button ID="btnSelectTraits" runat="server" Text="Go" 
                onclick="btnSelectTraits_Click" />
            <br /><br />
        </div>
    </asp:Panel>
    <asp:Panel ID="pnlSelectedTraits" runat="server" Visible="false">
        <h3><a href='#noop' id='select-descriptor-value'><%= Page.DisplayText("htmlChooseDescVal", "Select descriptor values")%>:</a>&nbsp;&nbsp;&nbsp;<asp:Button ID="btnResetDescriptorValue" runat="server" 
                Text="Clear Descriptor Values" onclick="btnSelectTraits_Click" 
                Width="151px" /></h3>
        <div id='panel-select' class='hide'>
           &nbsp;Results Match: <asp:RadioButtonList ID="rblMatch" RepeatDirection="Horizontal"  RepeatLayout="Flow" runat="server">
                <asp:ListItem Value="all" Text="All Conditions" Selected="True"></asp:ListItem>
                <asp:ListItem Value="any" Text="Any Condition"></asp:ListItem>
            </asp:RadioButtonList>
            &nbsp;&nbsp;
           <asp:CheckBox ID="cbValue" runat="server" Text="Results have observation data for all selected descriptors" /> <br /><br />
        <asp:DataList ID="dlSelectedTraits" runat="server"  RepeatDirection="Vertical" RepeatColumns="3" DataKeyField="crop_trait_id">
            <HeaderTemplate>
            </HeaderTemplate>
            <ItemTemplate>
                <fieldset>
                    <h4><%# Eval("crop_trait_name") %> (<%#Eval("accession_count") %>)</h4>
                    <asp:DropDownList ID="ddlOperator" runat="server" AutoPostBack="false">
                        <asp:ListItem Value="" Text="(Any)"></asp:ListItem>
                        <asp:ListItem Value="GT" Text="Greater Than or Equal To"></asp:ListItem>
                        <asp:ListItem Value="EQ" Text="Equal To"></asp:ListItem>
                        <asp:ListItem Value="NEQ" Text="Not Equal To"></asp:ListItem>
                        <asp:ListItem Value="LT" Text="Less Than or Equal To"></asp:ListItem>
                    </asp:DropDownList><br /><br />
                    <asp:ListBox ID="lstValues" runat="server" SelectionMode="Multiple" DataTextField="crop_trait_code_text" DataValueField="field_name_value">
                    </asp:ListBox>
                </fieldset>
            </ItemTemplate>
            <FooterTemplate>
            </FooterTemplate>
        </asp:DataList>
        
        <asp:Panel ID="pnlPassport" runat="server" Visible="false">
        <h3 ><a href='#' id='add-passport'>Add passport data criteria:</a></h3> 
        <div id='divPassport' style='display' class='box'>
             <br />Accessions: <asp:CheckBox ID="ck1" runat="server" Checked="false" Text="Exclude unavailable" />
            <asp:CheckBox ID="ck2" runat="server" Checked="false" Text="With images" />
            <asp:CheckBox ID="ck3" runat="server" Checked="false" Text="With NCBI link" />
            <asp:CheckBox ID="ck4" runat="server" Checked="false" Text="With genomic data" />

            <asp:PlaceHolder ID="ph1" runat="server">
                <gg:SearchCriteria runat="server" id="searchItem1" Sequence="1"></gg:SearchCriteria>
            </asp:PlaceHolder>
            <asp:Panel ID="pnl2" runat="server" Visible = "false">
                <gg:SearchCriteria runat="server" id="searchItem2"></gg:SearchCriteria>             
            </asp:Panel>
            <asp:Panel ID="pnl3" runat="server" Visible = "false">
                <gg:SearchCriteria runat="server" id="searchItem3"></gg:SearchCriteria>             
            </asp:Panel>
            <asp:Panel ID="pnl4" runat="server" Visible = "false">
                <gg:SearchCriteria runat="server" id="searchItem4"></gg:SearchCriteria>             
            </asp:Panel>
            <asp:Panel ID="pnl5" runat="server" Visible = "false">
                <gg:SearchCriteria runat="server" id="searchItem5"></gg:SearchCriteria>             
            </asp:Panel>
            <br />
            <asp:Button ID="btnMore" runat="server" onclick="btnMore_Click" Text="Add More Criteria" /> &nbsp; &nbsp; &nbsp; <asp:Button ID="btnClear" runat="server" onclick="btnClear_Click" Text="Clear All Criteria" /> <br /><br />
        </div></asp:Panel><br />
        <asp:Button ID="btnSearch" runat="server" Text="Search" 
            onclick="btnSearch_Click" /><br /><br />
                </div>
        <asp:Panel ID="pnlResult" runat="server" Visible="false">
        <h3><a href='#noop' id='show-results'><%= Page.DisplayText("htmlResults", "Results")%>:</a></h3>
            <div id='panel-results' class='hide'>
                <asp:Panel ID="pnlSearchResultsHeader" runat="server">
                    <div style='width:100%;'>
                    <%--<span style=''>--%>
                            <table><tr>
                                <td><a href='javascript:Actions...' onclick='javascript:$("#divActions").toggle("fast");return false;'><img src='images/btn_actions.gif' alt='Actions...' /></a></td>
                                <td valign="top"><asp:Button ID="btnExport" runat="server" Text="Export with Options" OnClientClick="window.open('descriptorexport.aspx', 'ExportPage', 'scrollbars=yes,titlebar=no,resizable=yes,width=450,height=580'); return false" /></td>
                                <td valign="top"><asp:Button ID="btnExportFB" runat="server" onclick="btnExportFB_Click" Text="Export Fieldbooks"  Visible="False" /></td>
                            </tr></table>
                    <%--</span>--%>
                        <div id="divActions" style="width:200px;height:80px;position:absolute" class='popup'>
                        <asp:ImageButton AlternateText='Add Selected Items to Cart' ID="btnAddToOrder" runat="server" ImageUrl="~/images/actions_addtoorder.gif" OnClientClick="javascript:return actionItemClicked('cart');" onclick="btnAddToOrder_Click" /><br />
                        <a href='javascript:Hide Selected Rows' onclick='javascript:pivotView.hideRows("checked").refresh().noop(event);$("#divActions").toggle("fast");'><img src='images/actions_removefromresults.gif' alt='Hide Selected Rows' /></a><br />
                        <asp:ImageButton AlternateText='Add Selected Items to Cart' ID="btnAddToFavorite" runat="server" ImageUrl="~/images/actions_addtofavs.gif" OnClientClick="javascript:return actionItemClicked('favorite');" onclick="btnAddToFavorite_Click" /><br />
                        </div>
                    </div>
                </asp:Panel>
                <div id='pivotViewWrapper'>
                <gg:PivotView  
                    ID="ggPivotView"
                    runat="server"
                    PageIndex="0"
                    PageSize="25"
                    PrimaryKeyName="accession_id"
                    AlternateKeyName="pi_number"
                    AllowPivoting="false"
                    AllowGrouping="true"
                    AllowFilteringAutoComplete="false"
                    OnLanguageChanged="PivotView_LanguageChanged"
                ></gg:PivotView>  <br /></div>
           </div>
        </asp:Panel>
        </asp:Panel>
        <br />
                <asp:GridView ID="gvResult" runat="server" Visible="False">
        </asp:GridView>

</asp:Content>
