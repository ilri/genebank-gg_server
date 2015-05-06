<%@ Page Title="Accessions" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="search.aspx.cs" ValidateRequest="false" Inherits="GrinGlobal.Web.Search" %>
<%@ Register TagPrefix="gg" TagName="PivotView" Src="~/pivotviewcontrol.ascx" %>
<%@ Register Src="~/searchcriteriacontrol.ascx" TagPrefix="gg" TagName="SearchCriteria" %>
<%@ Register Src="~/searchcriteriacontrol2.ascx" TagPrefix="gg" TagName="SearchCriteria2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type='text/javascript'>
        $(document).ready(function() {
            syncSearchBox(false);
            $("#<%= ml.ClientID %>").click(function(event) {
                syncSearchBox(true);
            });
            $(".searchBox2").keypress(function(event) {
                var keycode = event.charCode || event.keyCode || 0;
                if (event.ctrlKey && keycode == 13) {
                    $("#<%= btnSearch.ClientID %>").click();
                }
            });

            if ( '<%= divAdvToggleOn %>' == 'True' )
                $("#divAdvancedSearch").css("display", "");
                
            if ('<%= divLatLongToggleOn %>' == 'True')
                $("#divLatLongSearch").css("display", "");

            $('._focusme').focus();
        });
        
        function actionItemClicked(kind) {

            if (kind == 'remove') {
                alert("Sorry, not implemented yet");
                return false;
            }

            var pks = pivotView.rememberPrimaryKeys('checked');
//            pivotView.debug(pks);


            if (pks.length == 0) {
                alert("You must select at least one accession from the list.");
                $("#divActions").hide("fast");
                return false;
            }

            return true;
        }

        function syncSearchBox(copyText) {
            if ($("#<%= ml.ClientID %>").is(":checked")) {
                $(".searchBox1").hide();
                if (copyText) {
                    $(".searchBox2").val($(".searchBox1").val().replace(/  /, '\r\n'));
                }
                $(".searchBox2").show();
            } else {
                $(".searchBox2").hide();
                if (copyText) {
                    $(".searchBox1").val($(".searchBox2").val());
                }
                $(".searchBox1").show();
            }
        }
        
        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">

    <asp:Panel ID="pnlSearchDown" runat="server">
        <h3>Search Engine is down</h3>
        <p>
            The search engine appears to be offline.  Please contact an administrator.
            <asp:Panel ID="pnlError" runat="server">
                <a href='#' onclick='javascript:$("#divError").toggle("fast");return false;'>Error Detail</a>
                <div style='display:none;' id='divError'>
                <%= SearchError %>
                </div>
            </asp:Panel>
        </p>
    </asp:Panel>
    <asp:Panel ID="pnlSearch" runat="server" DefaultButton="btnSearch">
        <p class="system">
                <span style='vertical-align:top'><%= Page.DisplayText("htmlSearchFor", "Search For:") %>&nbsp;</span><asp:TextBox ID="q" runat="server" Width="300" MaxLength="1000000" CssClass="searchBox1 _focusme"></asp:TextBox>
                <asp:TextBox ID="q2" runat="server" Width="300" Rows="5" Columns="80" TextMode="MultiLine" CssClass="searchBox2 _focusme"></asp:TextBox>
<%--                <a onclick="javascript:window.open('PopUpHelp.aspx?id=1','','scrollbars=yes,titlebar=no,width=450,height=250')"><img src="images/help.png" alt="Help" title="Help" /></a>
--%> 
<%--                <a onclick="javascript:window.open('PopUpHelp.aspx?tag=Content/search/Search.htm','','scrollbars=yes,titlebar=no,width=750,height=650')"><img src="images/help.ico" alt="Help" title="Help" align="top" /></a>
--%>                <a onclick="javascript:window.open('popuphelp.aspx?id=6','','scrollbars=yes,titlebar=no,width=750,height=650')"><img src="images/help.ico" alt="Help" title="Help" align="top" /></a>

                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <span style='vertical-align:top'><%--<asp:Button ID="btnSearch" runat="server" OnClick='btnSearch_Click' Text="Search" Font-Bold="True"/>--%>
                <asp:button id="btnSearch" style="background-image:url('images/search.ico'); cursor:hand; background-repeat:no-repeat; background-position:2px 2px;" Runat="server" Font-Bold="True" Width="100px" OnClick="btnSearch_Click" Text="Search" BorderColor="#2F571B" BorderStyle="Outset" BorderWidth="2" />
               </span><br />
                <a href='#' onclick='javascript:$("#divSearchOptions").toggle("fast");return false;' style='position:relative;left:170px;' class='jsanchor'><%= Page.DisplayText("htmlSearchOption", "Search Options")%> </a>  
                <a href='#' onclick='javascript:$("#divAdvancedSearch").toggle("fast");return false;' style='position:relative;left:171px;' class='jsanchor'>| <%= Page.DisplayText("htmlAdvancedSearch", "Advanced Search") %></a>
        </p>
        <div id='divSearchOptions' style='display:none;' class='box'>
            <p>
                <%= Page.DisplayText("htmlReturnUpTo", "Return up to ")%><asp:DropDownList ID="lim" runat="server">
                    <asp:ListItem Value="10">10</asp:ListItem>
                    <asp:ListItem Value="25">25</asp:ListItem>
                    <asp:ListItem Value="50">50</asp:ListItem>
                    <asp:ListItem Value="100">100</asp:ListItem>
                    <asp:ListItem Value="250">250</asp:ListItem>
                    <asp:ListItem Value="500">500</asp:ListItem>
                    <asp:ListItem Value="1000">1,000</asp:ListItem>
                    <asp:ListItem Value="2500">2,500</asp:ListItem>
                    <asp:ListItem Value="5000">5,000</asp:ListItem>
                    <asp:ListItem Value="10000">10,000</asp:ListItem>
                    <asp:ListItem Value="15000">15,000</asp:ListItem>
                    <asp:ListItem Value="20000">20,000</asp:ListItem>
<%--                     
                    <asp:ListItem Value="30000">30,000</asp:ListItem>
                    <asp:ListItem Value="40000">40,000</asp:ListItem>
                    <asp:ListItem Value="50000">50,000</asp:ListItem> 
--%>               
                </asp:DropDownList> <%= Page.DisplayText("htmlAcs", "accessions")%> 
            </p>
            <p>
                <asp:CheckBox ID="ic" runat="server" Text="Ignore Case" Visible="False" />
                <asp:CheckBox ID="ma" runat="server" Text="Match All Terms" />
                
                <asp:CheckBox ID="ml" runat="server" Checked="false" Text="Allow Multiple Lines" />
                
                <asp:CheckBox ID="ed" runat="server" Checked="false" Text="Exclude unavailable accessions" Visible="False" />
                <asp:CheckBox ID="exp" runat="server" Checked="false" Text="Export simple excel only" Visible="False" />
            </p>
            <p>
                <%= Page.DisplayText("htmlRetrieve", "Retrieve:")%>
                <asp:DropDownList ID="view" runat="server" DataTextField="title" DataValueField="dataview_name" >
                </asp:DropDownList>
            </p>
        </div>
        <div id='divAdvancedSearch' style='display:none;'>
         <br />Accessions: <asp:CheckBox ID="ck1" runat="server" Checked="false" Text="Exclude unavailable" />
                <asp:CheckBox ID="ck2" runat="server" Checked="false" Text="With images" />
                <asp:CheckBox ID="ck3" runat="server" Checked="false" Text="With NCBI link" />
                <asp:CheckBox ID="ck4" runat="server" Checked="false" Text="With genomic data" /><br /><br />
       <a href='#' onclick='javascript:$("#divLatLongSearch").toggle("fast");return false;' class='jsanchor'> <u><%= Page.DisplayText("htmlLatLongSearch", "Accession Collecting Site Search Criteria") %></u></a>
        <br /><div id='divLatLongSearch' style='display:none;'> 
        <gg:SearchCriteria2 runat="server" id="searchItemLL"></gg:SearchCriteria2>
        </div>
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
    </div>
    </asp:Panel>
    <asp:Panel ID="pnlSearchResults" runat="server" Visible="false">
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
        <asp:Label ID="lblTimer" runat="server" ></asp:Label><br />
    </asp:Panel>
</asp:Content>
