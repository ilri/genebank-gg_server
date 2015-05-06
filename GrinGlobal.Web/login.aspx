<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="GrinGlobal.Web.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <table id="tbLogin" runat="server">
<tr id="tr_header" visible="false"><td align="center" width="48%">
    <h4>
                Returning Member</h4>
    </td>
    <td width="100px"></td> 
    <td align="center">
    <h4>
        New to GRIN-Global?</h4>
    </td>
</tr>
<tr>
<td >
    <asp:Login ID="Login1" runat="server" onauthenticate="Login1_Authenticate" 
    DisplayRememberMe="True" UserNameLabelText="GRIN-Global User Name:" 
    DestinationPageUrl="~/search.aspx" onloggedin="Login1_LoggedIn" 
    onloggingin="Login1_LoggingIn">
    </asp:Login></td>
<td id="td_vline"  visible="false" align="center" >
   <img alt="vertical line" longdesc="vertical separation line" 
        src="images/line-vertical.gif" /></td>
<td id="td_new"  visible="false">
    <table>
        <tr><td>Please sign up for an account, so we can serve you better:</td></tr>
        <tr><td><br /><br /><br /></td></tr>
         <tr><td align="center"  ><asp:Button ID="btnRegister" runat="server" Text="Register" onclick="btnRegister_Click" /></td></tr>
        <tr><td></td></tr>
    </table>
</td></tr>
<tr><td colspan="3" ><a href='UserAcct.aspx?action=forgotPassword'>Forgot password?</a></td></tr>
<tr id="tr_footer" visible="false" style="text-align: center"><td></td>
    <td nowrap="nowrap"> 
    <asp:HyperLink ID="hlNot" NavigateUrl="#" runat="server" Font-Size="X-Small" 
            ToolTip="You will still be required to enter necessary information for us to process the order" Visible="False">Don't want to register?</asp:HyperLink></td></tr>
</table>
<br />
</asp:Content>
