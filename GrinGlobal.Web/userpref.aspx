<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="userpref.aspx.cs" Inherits="GrinGlobal.Web.UserPref" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type='text/javascript'>
        $(document).ready(function() {
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    
    <table>
    <tr><td><b>Preferences:</b></td></tr>
    <tr><td></td></tr>
    <tr>
        <td>
        Select Language: 
            <asp:DropDownList ID="ddlLang" runat="server" 
                onselectedindexchanged="ddlLang_SelectedIndexChanged" AutoPostBack="True">
        </asp:DropDownList>  
        </td>    
    </tr>
    <tr><td></td></tr>
    <tr><td>
        &nbsp;</td></tr>
    <tr>
        <td>
            <asp:CheckBox ID="cbEmail" runat="server" 
                Text="Email me a copy of my orders when placed" 
                oncheckedchanged="cbEmail_CheckedChanged" AutoPostBack="True" />
        </td>
    </tr>
   <tr>
        <td>
            <asp:CheckBox ID="cbShipping" runat="server" 
                Text="Email me when my orders are shipped" 
                oncheckedchanged="cbShipping_CheckedChanged" AutoPostBack="True" 
                Visible="False" />
        </td>
    </tr>    
    <tr>
        <td>
            <asp:CheckBox ID="cbEmailNews" runat="server" 
                Text="Email me news &amp; information about new features of GRIN-Global" 
                oncheckedchanged="cbEmailNews_CheckedChanged" AutoPostBack="True" />
        </td>
    </tr>
    </table>

<br />
<asp:Button ID="btnOK" runat="server" Text="Save" onclick="btnOK_Click" /><br /><br />
</asp:Content>