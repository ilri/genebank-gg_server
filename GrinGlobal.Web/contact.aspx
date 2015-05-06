<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="contact.aspx.cs" Inherits="GrinGlobal.Web.Contact" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
<link href="~/styles/default.css" rel="stylesheet" type="text/css" />
<title>Contact Us</title></head>
<body>
    <form id="form1" runat="server">
    <div>
       <asp:panel ID="pnlSendEmail" runat="server">
          <table width="550" border="0" cellspacing="0" cellpadding="0">
                <tr>
                  <td height="30" valign="middle" 
                        style="font-weight: bold; color: #FFFFFF; background-color: #2f571b">&nbsp;&nbsp; 
                      <%= Page.DisplayText("htmlContactUs", "Contact Us")%></td>
                </tr>
          </table>
          <table width="550" border="0" cellpadding="5" cellspacing="0" bgcolor="#FFFFFF" id="Contact">
		        <tr valign="middle" bgcolor="#ffffff">
					<td  colspan="2" height="30"><font color="#ff0000">*</font> <b><%= Page.DisplayText("htmlRequired", "Required Fields")%></b></td>
				</tr>
				<tr  bgcolor="#f4f4f4" valign="middle">
					<td  width="25%"><b><font color="#ff0000">*</font></b><%= Page.DisplayText("htmlYourName", "Your Name")%>
					:</td>
					<td>
						<asp:TextBox id="txtName" runat="server" 
                            ToolTip="Your Name" Width="180px"></asp:TextBox>
						<b>
				  <asp:RequiredFieldValidator id="rfvName" runat="server" ControlToValidate="txtName"
								Display="Dynamic" ErrorMessage="Please enter your name"></asp:RequiredFieldValidator></b></td>
				</tr>
				<tr valign="middle" bgcolor="#f4f4f4">
					<td bgcolor="#FFFFFF"><b><font color="#ff0000">* </font>
							</b><%= Page.DisplayText("htmlEmail", "Email Address")%>:</td>
				  <td bgcolor="#FFFFFF">
						<asp:TextBox id="txtEmail" runat="server" 
                            ToolTip="Email Address" Width="180px"></asp:TextBox>
						<b>
							<asp:RequiredFieldValidator id="rfvEmail" runat="server" ControlToValidate="txtEmail"
								Display="Dynamic" ErrorMessage="Please enter your email address"></asp:RequiredFieldValidator>
				  <asp:RegularExpressionValidator id="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail"
								Display="Dynamic" ErrorMessage="Invalid email address" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
								EnableClientScript="true"></asp:RegularExpressionValidator></b></td>
				</tr>
				<tr bgcolor="#f7f7f7">
					<td valign="top"><span>&nbsp;&nbsp; <%= Page.DisplayText("htmlSubject", "Subject")%>:</span></td>
					<td>
						<asp:DropDownList id="ddlSubject" runat="server" 
                            Width="186px">
							<asp:ListItem value="0">Help/Technical Support</asp:ListItem>
							<asp:ListItem Value="1">Accessibility Problem</asp:ListItem>
                            <asp:ListItem Value="2">Other Request</asp:ListItem>
						</asp:DropDownList></td>
						</tr>
				<tr>
					<td valign="top" bgcolor="#FFFFFF" style="height: 120px"><b><font color="#ff0000">*</font></b><%= Page.DisplayText("htmlYourMessage", "Your Message")%>: </td>
					<td bgcolor="#FFFFFF" valign="top" style="height: 120px">
						<asp:TextBox id="txtMessage" runat="server" TextMode="MultiLine" 
                            ToolTip="Your Message" Width="380px" Height="121px"></asp:TextBox>
                        <b> <br />
                        <asp:RequiredFieldValidator id="rfvMessage" runat="server" ControlToValidate="txtMessage"
								Display="Dynamic" ErrorMessage="Please enter a text message" EnableClientScript="true"></asp:RequiredFieldValidator></b></td>
				</tr>
				<tr bgcolor="#f7f7f7">
					<td bgcolor="#ffffff" colspan="2"><div align="left">
							<asp:Button  Height="28" id="btnSendFeedback" runat="server" Text="Send Request"
								ToolTip="Send Feedback!" Width="120" OnClick="btnSendFeedback_Click"></asp:Button></div>						
				    </td>
				</tr>
			</table>
		</asp:panel>
		<table>
			<tr bgcolor="#f7f7f7">
			  <td bgcolor="#ffffff">
                  <asp:Panel ID="pnlMailSent" width="550px" 
                      runat="server" Visible="False">
			      <span >Your e-mail message has been sent to us.</span> 
                  <br />
                  <br />
                  <input type="button" id="btnClose" title="Close Window" onclick="javascript:window.close();" value="Close Window" /></asp:Panel>
	          </td>
			</tr>
        </table>
    </div>
    </form>
</body>
</html>
