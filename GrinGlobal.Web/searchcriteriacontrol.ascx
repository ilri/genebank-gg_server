<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="searchcriteriacontrol.ascx.cs" Inherits="GrinGlobal.Web.searchcriteriacontrol" %>
<div><br />
    <asp:Label ID="lblChoose" runat="server" Text="Choose Query"></asp:Label>
    <asp:DropDownList ID="ddlItem" runat="server" DataTextField="group_name" 
        DataValueField="name" AutoPostBack="True" 
         onselectedindexchanged="ddlItem_SelectedIndexChanged">
    </asp:DropDownList>
     <asp:DropDownList ID="ddlOperator" runat="server" AutoPostBack="false">
        <asp:ListItem Value="" Text="- Select -"></asp:ListItem>
        <asp:ListItem Value="=" Text="Equal To"></asp:ListItem>
        <asp:ListItem Value="like" Text="Like"></asp:ListItem>
        <asp:ListItem Value="in" Text="In"></asp:ListItem>
        <asp:ListItem Value="contain" Text="Contains"></asp:ListItem>
    </asp:DropDownList>
    <asp:ListBox ID="lstValues" runat="server" SelectionMode="Multiple" DataTextField="title" DataValueField="value" Visible="false">
    </asp:ListBox>
    <asp:TextBox ID="txtValue" runat="server" Visible="False"></asp:TextBox>
    &nbsp; &nbsp;
    <asp:Button ID="btnClear1" runat="server" Text="Clear Criterion" 
        onclick="btnClear_Click" Width="97px" />
    <asp:Panel ID="pnlPIRange" runat="server" Visible="False"><br />
    <table cellpadding="0"><tr><td width="80"></td><td bgcolor="#efefef">
     <asp:Label ID="Label1" runat="server" Text="Individual accession identifier or range of identifiers:"></asp:Label><br />
     <asp:DropDownList ID="ddlIdentifier" runat="server"
        Font-Bold="True"
        DataTextField="value" 
        DataValueField="value">
        </asp:DropDownList>
        &nbsp;<asp:TextBox ID="txtIDFrom" runat="server" Width="75"></asp:TextBox> &nbsp;&nbsp;to &nbsp;
        <asp:TextBox ID="txtIDTo" runat="server" Width="75"></asp:TextBox></td></tr></table>
    </asp:Panel>
    <asp:Panel ID="pnlLocation" runat="server" Visible="False"><br />
    <table cellpadding="0"><tr><td width="80"></td><td bgcolor="#efefef">
     <asp:Label ID="Label2" runat="server" Text="Accession source country"></asp:Label></td>
     <td bgcolor="#efefef"><asp:Label ID="Label3" runat="server" Text="State"/></td> </tr>
     <tr><td width="80"></td><td>
     <asp:DropDownList ID="ddlCountry" runat="server"
        DataTextField="countryname" 
        DataValueField="countrycode"
        onselectedindexchanged="ddlCountry_SelectedIndexChanged" AutoPostBack="True">
        </asp:DropDownList>  
       </td><td>
        <asp:DropDownList ID="ddlState" runat="server"
        DataTextField="value" 
        DataValueField="value">
        </asp:DropDownList>
       </td></tr></table>
    </asp:Panel>
</div>

