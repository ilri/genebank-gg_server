<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ecoclasscontrol.ascx.cs" Inherits="GrinGlobal.Web.taxon.ecoclasscontrol" %>
<div><br />
    <asp:Label ID="lblChoose" runat="server" Text="Choose Class"></asp:Label>
    <asp:DropDownList ID="ddlClass" runat="server" DataTextField="text" 
        DataValueField="code" AutoPostBack="True" 
         onselectedindexchanged="ddlClass_SelectedIndexChanged">
    </asp:DropDownList> &nbsp;&nbsp;&nbsp;
    <asp:Label ID="lblChooseSub" runat="server" Text="Subclass"></asp:Label>
    <asp:ListBox ID="lstSubclass" runat="server" SelectionMode="Multiple" DataTextField="code" DataValueField="text">
    </asp:ListBox>
    <asp:Button ID="btnClear" runat="server" Text="Clear Criterion" 
        onclick="btnClear_Click" Width="97px" />
</div>