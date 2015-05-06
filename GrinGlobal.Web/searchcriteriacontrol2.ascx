<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="searchcriteriacontrol2.ascx.cs" Inherits="GrinGlobal.Web.searchcriteriacontrol2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<asp:ToolkitScriptManager ID="tsm" runat="server">
</asp:ToolkitScriptManager>
    <script language="javascript" type="text/javascript">
        function checkNumeric(e) {

            if ((e.which < 48 || e.which > 57) & e.which != 8 & e.keyCode != 9) {
                e.preventDefault();
                return false;
            }
        }

        function checkNumericDotMinus(e) {

            if ((e.which < 45 || e.which > 57 || e.keyCode == 47) & e.which != 8 & e.keyCode != 9) {
                e.preventDefault();
                return false;
            }
        }

        function checkNumericDot(e) {

            if ((e.which < 46 || e.which > 57 || e.keyCode == 47) & e.which != 8 & e.keyCode != 9) {
                e.preventDefault();
                return false;
            }
        }

         </script>
         
         
<div>
    <asp:Panel ID="pnlLatLong" runat="server" Visible="false"><br />
    <table cellpadding="1" ><tr><td width="80"></td><td bgcolor="#efefef" colspan="6">
        <asp:RadioButtonList ID="rblDegree" runat="server" 
            RepeatDirection="Horizontal" Width="340px" 
            onselectedindexchanged="rblDegree_SelectedIndexChanged" AutoPostBack="True">
            <asp:ListItem Selected="True" Value="Degree">Degrees, minutes and seconds</asp:ListItem>
            <asp:ListItem Value="Decimal">Decimal degrees</asp:ListItem>
        </asp:RadioButtonList> 
        </td></tr> 
        <tr><td width="80"></td><td width="110" bgcolor="#efefef">&nbsp;&nbsp;Latitude:</td><td width="30">From</td><td width="200">
            <asp:TextBox ID="txtLAFDC" runat="server" Visible="False" onkeypress="checkNumericDotMinus(event);"></asp:TextBox>
            <asp:Panel ID="pnlLatF" runat="server">
            <asp:TextBox ID="txtLAFD1" runat="server" Width="34px" onkeypress="checkNumeric(event);"></asp:TextBox>
            <sup>°</sup>&nbsp;<asp:TextBox ID="txtLAFD2" runat="server" Width="34px" onkeypress="checkNumeric(event);"></asp:TextBox>
            <sup>'</sup>&nbsp;<asp:TextBox ID="txtLAFD3" runat="server" Width="34px" onkeypress="checkNumeric(event);"></asp:TextBox>
            <sup>"</sup>&nbsp;<asp:DropDownList ID="ddlLAF" runat="server">
                <asp:ListItem Selected="True">N</asp:ListItem>
                <asp:ListItem>S</asp:ListItem>
            </asp:DropDownList></asp:Panel>
            </td><td width="14">To</td><td width="200">
            <asp:TextBox 
                ID="txtLATDC" runat="server" Visible="False" onkeypress="checkNumericDotMinus(event);"></asp:TextBox>
            <asp:Panel ID="pnlLatT" runat="server">
            <asp:TextBox ID="txtLATD1" runat="server" Width="34px" onkeypress="checkNumeric(event);"></asp:TextBox>
            <sup>°</sup>&nbsp;<asp:TextBox ID="txtLATD2" runat="server" Width="34px" onkeypress="checkNumeric(event);"></asp:TextBox>
            <sup>'</sup>&nbsp;<asp:TextBox ID="txtLATD3" runat="server" Width="34px" onkeypress="checkNumeric(event);"></asp:TextBox>
            <sup>"</sup>&nbsp;<asp:DropDownList ID="ddlLAT" runat="server">
                <asp:ListItem Selected="True">N</asp:ListItem>
                <asp:ListItem>S</asp:ListItem>
            </asp:DropDownList></asp:Panel>
            </td><td width="20" bgcolor="#efefef"></td></tr> 
        <tr><td width="80"></td><td width="110" bgcolor="#efefef">&nbsp;&nbsp;Longitude:</td><td>From</td><td width="180">
            <asp:TextBox ID="txtLGFDC" runat="server" Visible="False" onkeypress="checkNumericDotMinus(event);"></asp:TextBox>
            <asp:Panel ID="pnlLogF" runat="server">
            <asp:TextBox ID="txtLGFD1" runat="server" Width="34px" onkeypress="checkNumeric(event);"></asp:TextBox>
            <sup>°</sup>&nbsp;<asp:TextBox ID="txtLGFD2" runat="server" Width="34px" onkeypress="checkNumeric(event);"></asp:TextBox>
            <sup>'</sup>&nbsp;<asp:TextBox ID="txtLGFD3" runat="server" Width="34px" onkeypress="checkNumeric(event);"></asp:TextBox>
            <sup>"</sup>&nbsp;<asp:DropDownList ID="ddlLGF" runat="server">
                <asp:ListItem Selected="True">E</asp:ListItem>
                <asp:ListItem>W</asp:ListItem>
            </asp:DropDownList></asp:Panel>
            </td><td>To</td><td width="180">
            <asp:TextBox ID="txtLGTDC" runat="server" Visible="False" onkeypress="checkNumericDotMinus(event);"></asp:TextBox>
            <asp:Panel ID="pnlLogT" runat="server">
            <asp:TextBox ID="txtLGTD1" runat="server" Width="34px" onkeypress="checkNumeric(event);"></asp:TextBox>
            <sup>°</sup>&nbsp;<asp:TextBox ID="txtLGTD2" runat="server" Width="34px" onkeypress="checkNumeric(event);"></asp:TextBox>
            <sup>'</sup>&nbsp;<asp:TextBox ID="txtLGTD3" runat="server" Width="34px" onkeypress="checkNumeric(event);"></asp:TextBox>
            <sup>"</sup>&nbsp;<asp:DropDownList ID="ddlLGT" runat="server">
                <asp:ListItem Selected="True">E</asp:ListItem>
                <asp:ListItem>W</asp:ListItem>
            </asp:DropDownList></asp:Panel>
            </td><td bgcolor="#efefef"></td></tr> 
        <tr><td width="80"></td><td bgcolor="#efefef">&nbsp;&nbsp;Elevation (m):</td><td>From</td><td>
            <asp:TextBox ID="txtEleF" runat="server" Width="80px" onkeypress="checkNumeric(event);"></asp:TextBox>
            </td><td>To</td><td>
            <asp:TextBox ID="txtEleT" runat="server" Width="80px" onkeypress="checkNumeric(event);"></asp:TextBox>
            </td><td bgcolor="#efefef"></td></tr>
        <tr><td width="80"></td><td bgcolor="#efefef">&nbsp;&nbsp;Collecting date:</td><td>From</td><td>
            <asp:TextBox ID="txtDateF" runat="server"></asp:TextBox>
            <asp:CalendarExtender ID="ceFrom" runat="server" TargetControlID="txtDateF">
            </asp:CalendarExtender>
            </td><td>To</td><td>
            <asp:TextBox ID="txtDateT" runat="server"></asp:TextBox>
            <asp:CalendarExtender ID="ceTo" runat="server" TargetControlID="txtDateT">
            </asp:CalendarExtender>
            </td><td bgcolor="#efefef"></td></tr>
        <tr><td width="80"></td><td bgcolor="#efefef">&nbsp;&nbsp;Collecting note:</td><td colspan="4">
             <asp:DropDownList ID="ddlOperator" runat="server" AutoPostBack="false">
                <asp:ListItem Value="contain" Text="Contains"></asp:ListItem>
                <asp:ListItem Value="=" Text="Equal To"></asp:ListItem>
        </asp:DropDownList>
            <asp:TextBox ID="txtNote" runat="server" Width="350px"></asp:TextBox>
        </td><td bgcolor="#efefef"></td></tr>
        <tr ><td></td><td height="25" colspan="6" bgcolor="#efefef" align="center" 
                valign="middle">
            <asp:Button ID="btnClearLL" runat="server" onclick="btnClearLL_Click" 
                Text="Clear Collecting Site Search Criteria" Width="215px" />
            </td></tr>
    </table>
    </asp:Panel>
</div>


