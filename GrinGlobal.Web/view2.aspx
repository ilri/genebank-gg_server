<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="view2.aspx.cs" Inherits="GrinGlobal.Web.View2" ValidateRequest="False" %>
<%@ Register TagPrefix="gg" TagName="PivotView" Src="~/pivotviewcontrol.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type='text/javascript'>
        function actionItemClicked(kind) {
            var pks = pivotView.rememberPrimaryKeys('checked');
//            pivotView.debug(pks);

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
<h1><%= HeaderText %></h1>
    <asp:Panel ID="pnlSearchResultsHeader" runat="server">
        <div style='width:100%;'>
            <span style=''>
                <a href='javascript:Actions...' onclick='javascript:$("#divActions").toggle("fast");return false;'><img src='images/btn_actions.gif' alt='Actions...' /></a>
            </span>
            <div id="divActions" style="width:200px;height:80px;position:absolute" class='popup'>
                <asp:ImageButton AlternateText='Add Selected Items to Cart' ID="btnAddToOrder" runat="server" ImageUrl="~/images/actions_addtoorder.gif" OnClientClick="javascript:return actionItemClicked('cart');" onclick="btnAddToOrder_Click" /><br />
                <a href='javascript:Hide Selected Rows' onclick='javascript:pivotView.hideRows("checked").refresh().noop(event);$("#divActions").toggle("fast");'><img src='images/actions_removefromresults.gif' alt='Hide Selected Rows' /></a><br />
                <asp:ImageButton AlternateText='Add Selected Items to Cart' ID="btnAddToFavorite" runat="server" ImageUrl="~/images/actions_addtofavs.gif" OnClientClick="javascript:return actionItemClicked('favorite');" onclick="btnAddToFavorite_Click" /><br />
            </div>
        </div>
    </asp:Panel>
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
></gg:PivotView>
<br />
</asp:Content>
