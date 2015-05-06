<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="pivotajax.aspx.cs" Inherits="GrinGlobal.Web.PivotAjax" %>
<%@ Register TagPrefix="gg" TagName="PivotView" Src="~/pivotviewcontrol.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">

    <asp:Panel ID="pnlSearchDown" runat="server">
        <h3>Search Engine is down</h3>
        <p>
            The search engine appears to be offline.  Please contact an administrator.
        </p>
    </asp:Panel>
    
    <asp:Panel ID="pnlSearch" runat="server" >
        <p class="system">
                Search For: <input type='text' id='txtSearch' name='q' maxlength='1000000' width='300' value='<%= Server.HtmlEncode(Request.QueryString["q"]) %>' />
                <input type='button' name='btnSearch' onclick='javascript:doSearch();' value='Search' /><br />
                <a href='#' onclick='javascript:$("#divAdvancedSearch").toggle("fast");return false;' style='position:relative;left:275px;' class='jsanchor'>Advanced Search</a>
            
        </p>
        <div id='divAdvancedSearch' style='display:none;' class='box'>
            <p>
                Return up to <select name='lim'>
                    <option value='10'>10</option>
                    <option value='25'>25</option>
                    <option value='50'>50</option>
                    <option value='100'>100</option>
                    <option value='250'>250</option>
                    <option value='500'>500</option>
                    <option value='1000' selected='selected'>1,000</option>
                    <option value='2500'>2,500</option>
                    <option value='5000'>5,000</option>
                </select> accessions            
            </p>
            <label><input type='checkbox' id="chkIgnoreCase" name='ic' value='on' <%= (Request.QueryString["ic"] != "on" || Request.QueryString["ic"] == String.Empty ? "checked='checked'" : "") %> />Ignore Case</label>
            <label><input type='checkbox' id="chkMatchAll" name='ma' value='on' <%= (Request.QueryString["ma"] == "on" || Request.QueryString["ma"] == String.Empty ? "checked='checked'" : "") %> />Match All</label>
            <div style='display:none'>
                Retrieve: 
                <select name='rs' id='selDataView'>
                    <option value='web_search_overview'>Overview</option>
                    <option value='web_search_obs'>Observations</option>
                    <option value='web_search_vouchers'>Vouchers</option>
                </select>
            </div>
        </div>
        <div style='width:100%;'>
            <span style=''>
                <a href='javascript:Actions...' onclick='javascript:$("#divActions").toggle("fast");return false;'><img src='images/btn_actions.gif' alt='Actions...' /></a>
            </span>
            <span style='right:5%;position:absolute;'>
                <a href='javascript:Save Results (todo)' onclick='javascript:alert("todo: implement save results"); return false;'><img src='images/btn_saveresults.gif' alt='Save Results' /></a>&nbsp;&nbsp;
            </span>
            <span id="divActions" style="display:none;width:200px;height:60px;position:absolute">
                <a href='javascript:Add Selected Items to Cart' title='Add Selected Items to Cart' onclick='javascript:actionItemClicked("add"); return false;'><img src='images/actions_addtoorder.gif' alt='Add Selected Items to Cart' /></a><br />
            </span>
        </div>
        <gg:PivotView  
            ID="ggPivotView"
            runat="server"
            PageIndex="0"
            PageSize="25"
            AllowPivoting="false"
            AllowGrouping="true"
        ></gg:PivotView>
        <asp:Label ID="lblTimer" runat="server" ></asp:Label><br />
        <asp:Label ID="lblTotalCount" runat="server" ></asp:Label>
        <script type='text/javascript'>
            function doSearch() {
                pivotView.loadData(document.forms[0]);
            }
        </script>
    </asp:Panel>
</asp:Content>
