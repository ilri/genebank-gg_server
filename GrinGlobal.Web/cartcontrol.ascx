<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="cartcontrol.ascx.cs" Inherits="GrinGlobal.Web.CartControl" %>
<div class="cart"><table><tr><td>
    <asp:HyperLink runat="server" ID="lnkViewCart" 
    NavigateUrl="/gringlobal/cartview.aspx" ImageUrl="/gringlobal/images/btn_viewcart.gif" Text="View Cart" 
    ToolTip="View Cart"></asp:HyperLink></td> <td>
    <asp:Label ID="lblContents" runat="server"></asp:Label></td></tr></table> 
</div>