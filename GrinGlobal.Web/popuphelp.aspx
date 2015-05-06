<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="popuphelp.aspx.cs" Inherits="GrinGlobal.Web.PopUpHelp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Help</title>
    <link href="~/styles/default.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    	<p><asp:Label id="lblHeading" runat="server"  Font-Bold="True" Font-Size="Medium"></asp:Label></p>
			<p>
				<asp:Label id="lblTitle" runat="server"  Font-Bold="True" Font-Size="Small"></asp:Label></p>
			<p>
				<asp:Label id="lblHelp" runat="server"  Width="400px" Font-Size="Small"
					Font-Bold="False"></asp:Label></p>
			
			<div style='right:5%; bottom:5%; position: absolute;'>
			<input name="btnClose" id="btnClose" type="button" value=<%= Page.DisplayText("btnClose", "Close Window")%>
					onclick="javascript:window.close();" />
			</div>
    
    </div>
    </form>
</body>
</html>
