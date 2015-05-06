<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="webquery.aspx.cs" Inherits="GrinGlobal.Web.Admin.Sql" MasterPageFile="~/Site1.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>SQL</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <div>
        <asp:Label ID="lblError" runat="server" BorderStyle="Solid" BorderWidth="1px" 
            Font-Bold="True" ForeColor="Red" Text="-" Visible="false" 
            Width="700px"></asp:Label>
    </div>
    <h4>SQL:</h4>Enter or load from the existing file a select statement.  Any column that is not a simple column must be aliased.
    <asp:TextBox ID="txtSql" runat="server" Columns="100" Rows="8" 
        TextMode="MultiLine"></asp:TextBox>
    <br /><br />
    <div style="border: 1px solid #b2d79d; width: 620px">
    <table><tr><td>
    <fieldset style='width:450px'>
    <legend>Load SQL From File</legend>
    <asp:FileUpload ID="upload1" runat="server" Width="430px" /> &nbsp;<br />
    <asp:Button ID="btnLoad" runat="server" Text="Open File" 
            OnClick="btnLoad_Click"  /> 
   &nbsp;Note: click <strong>Browse...</strong> button first, then click <strong>Open File</strong> button.
    </fieldset></td><td>&nbsp;&nbsp;&nbsp;
         <asp:Button ID="btnSave" runat="server" Text="Save SQL to File" 
     onclick="btnSave_Click" /> </td></tr></table> 
    </div>
    <br />
    <b>Result Display Settings: </b>&nbsp;&nbsp; Limit to:     <asp:TextBox ID="txtLimit" runat="server">1000</asp:TextBox>
&nbsp;rows&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Rows per page:
    <asp:DropDownList ID="ddlPageSize" runat="server" 
        onselectedindexchanged="ddlPageSize_SelectedIndexChanged" AutoPostBack="True">
        <asp:ListItem>10</asp:ListItem>
        <asp:ListItem>20</asp:ListItem>
        <asp:ListItem>30</asp:ListItem>
        <asp:ListItem>50</asp:ListItem>
        <asp:ListItem Selected="True">100</asp:ListItem>
        <asp:ListItem>250</asp:ListItem>
        <asp:ListItem>500</asp:ListItem>
        <asp:ListItem Value="1000">1,000</asp:ListItem>
    </asp:DropDownList>
    <br />
    <br />
    <asp:Button ID="btnRun" runat="server" onclick="btnRun_Click" 
        Text="Execute SQL" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="btnExportCurrent" runat="server" onclick="btnDownload_Click" Text="Download Current Page" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
    <asp:Button ID="btnExportAll" runat="server" onclick="btnDownloadAll_Click" Text="Download All Rows" />
    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblRetrieved" runat="server" Text="" Visible="false"></asp:Label>
    <br /><br />
<%--    <asp:Panel ID="plResults" runat="server"  ScrollBars="Auto" Width="980px">
--%>    <asp:Panel ID="Panel1" runat="server">
    <asp:GridView ID="GridView1" runat="server" AllowSorting="true" 
        AllowPaging="true" PageSize="100" 
        onpageindexchanging="GridView1_PageIndexChanging" 
        onsorting="GridView1_Sorting" >
    </asp:GridView>
    </asp:Panel>   
    <br />
</asp:Content>
