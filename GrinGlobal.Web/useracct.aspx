<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="useracct.aspx.cs" Inherits="GrinGlobal.Web.UserAcct" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type='text/javascript'>
        $(document).ready(function() {
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:MultiView ID="mv1" runat="server" ActiveViewIndex="0">
    <asp:View ID="vwCreateAcct" runat="server">
        <table width="620" border="0" cellpadding="2" cellspacing="0">
        <tr >
          <td colspan="2" align="left">
              Enter your Email address and choose a password for your account.</td>
        </tr>
        <tr>
          <td width="225"><div align="right">
          <b>Email Address <span style="color: #ff0066">*</span>
              <br />
              (Your User Name):</b></div></td>
           <td width="390"  align="left">
               <asp:TextBox ID="txtUserName" runat="server" Width="150px"></asp:TextBox>
               <asp:RequiredFieldValidator ID="rfvName" runat="server" 
                   ErrorMessage="* - Required" ControlToValidate="txtUserName" 
                   Display="Dynamic"></asp:RequiredFieldValidator>
               &nbsp; 
               <asp:RegularExpressionValidator ID="rev3" runat="server" 
                   ControlToValidate="txtUserName" ErrorMessage="Invalid Email Address" 
                   ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
          <td  align="left"><div align="right">
              <b> Choose a Password: <span style="color: #ff0066">*</span></b></div></td>
           <td width="390"  align="left">
               <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="150px"></asp:TextBox>
               <asp:RequiredFieldValidator ID="rfvPass" runat="server" 
                   ErrorMessage="* - Required" ControlToValidate="txtPassword" 
                   Display="Dynamic" Width="111px"></asp:RequiredFieldValidator>
         <asp:Label runat="server" ID="lblHelpPwdR" Visible="False">&nbsp;<a onclick="javascript:window.open('PopUpHelp.aspx?heading=Password','','scrollbars=no,titlebar=no,width=400,height=220')"><img src="images/help.ico" alt="Help" title="Help" align="top" /></a>
         </asp:Label>
          <asp:RegularExpressionValidator ID="rev1" runat="server" ErrorMessage="Invalid Password"
                   ValidationExpression="[\w| @,#,$,%,&,*]*" ControlToValidate="txtPassword" Enabled="False"></asp:RegularExpressionValidator></td>
        </tr>
            <tr>
                <td align="left">
                    <div align="right">
                        &nbsp;<b> Re-enter Password: <span style="color: #ff0066">*</span></b></div>
                </td>
                <td align="left" width="390">
                    <asp:TextBox ID="txtConPassword" runat="server" TextMode="Password" 
                        Width="150px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvConPass" runat="server" 
                        ControlToValidate="txtConPassword" Display="Dynamic" 
                        ErrorMessage="* - Required" Width="111px"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="rev2" runat="server" 
                        ControlToValidate="txtConPassword" ErrorMessage="Invalid Password" 
                        ValidationExpression="[\w| @,#,$,%,&amp;,*]*" Enabled="False"></asp:RegularExpressionValidator>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    &nbsp; &nbsp; &nbsp;
                    <asp:Button ID="btnCancelAcct" runat="server" CausesValidation="False" 
                        OnClick="btnCancelAcct_Click" Text="Cancel" />
                    &nbsp; &nbsp; &nbsp;&nbsp;
                    <asp:Button ID="btnCreateAcct" runat="server" OnClick="btnCreateAcct_Click" 
                        Text="Create Account" Width="108px" />
                </td>
            </tr>
    </table>
    </asp:View>
    <asp:View ID="vwUpdateAcct" runat="server">
        <table width="600" border="0" cellpadding="2" cellspacing="0">
        <tr>
          <td colspan="2" align="left"  >
              <asp:Label 
                  ID="lblChgPassword" runat="server" 
                  Text="Use the form below to change the Email address or password. If you change your Email address, use the new address next time you login."></asp:Label>
            </td>
         </tr>
            <tr>
                <td colspan="2"> 
                    <hr />
                </td>
            </tr>
            <tr>
                <td width="225">
                    <div align="right">
                        <b>New Email Address <span style="color: #ff0066">*</span>
                        <br />
                        (Your User Name):</b></div>
                </td>
                <td align="left" width="375">
                    <asp:TextBox ID="txtNewUserName" runat="server" Width="150px"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvNewName" runat="server" 
                        ControlToValidate="txtNewUserName" Display="Dynamic" 
                        ErrorMessage="* - Required"></asp:RequiredFieldValidator>
                    &nbsp;
                    <asp:RegularExpressionValidator ID="rev4" runat="server" 
                        ControlToValidate="txtNewUserName" ErrorMessage="Invalid Email Address" 
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                </td>
            </tr>
        <tr>
          <td  align="left"><div align="right">
              <b>&nbsp;New Password: </b></div></td>
           <td width="375"  align="left">
               <asp:TextBox ID="txtNewPassword" runat="server" TextMode="Password" 
                   Width="150px"></asp:TextBox>
               <asp:Label runat="server" ID="lblHelpPwdC" Visible="False">&nbsp;<a onclick="javascript:window.open('PopUpHelp.aspx?heading=Password','','scrollbars=no,titlebar=no,width=400,height=220')"><img src="images/help.ico" alt="Help" title="Help" align="top" /></a>
</asp:Label>
               <asp:RegularExpressionValidator ID="rev5" runat="server" 
                   ControlToValidate="txtNewPassword" ErrorMessage="Invalid Password" 
                   ValidationExpression="[\w| @,#,$,%,&amp;,*]*" Enabled="False"></asp:RegularExpressionValidator>
               </td>
        </tr>
        <tr>
          <td  align="left"><div align="right">
              &nbsp;<b> Re-enter New Password: </b></div></td>
           <td width="375"  align="left">
               <asp:TextBox ID="txtNewConPassword" runat="server" TextMode="Password" 
                   Width="150px"></asp:TextBox>
               <asp:RegularExpressionValidator ID="rev6" runat="server" 
                   ControlToValidate="txtNewConPassword" ErrorMessage="Invalid Password" 
                   ValidationExpression="[\w| @,#,$,%,&amp;,*]*" Enabled="False"></asp:RegularExpressionValidator>
               </td>
        </tr>
            <tr>
                <td align="center" colspan="2">
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="left" colspan="2">
                    Please enter your current password (required to make changes).</td>
            </tr>
            <tr>
                <td>
                    <div align="right">
                        <b>Current Password: <span style="color: #ff0066">*</span>
                        </b></div>
                </td>
                <td align="left" width="375">
                    <asp:TextBox ID="txtCurPassword" runat="server" Width="150px" 
                        TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvCurPwd" runat="server" 
                        ControlToValidate="txtCurPassword" Display="Dynamic" 
                        ErrorMessage="* - Required"></asp:RequiredFieldValidator>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    &nbsp; &nbsp; &nbsp;
                    <asp:Button ID="btnCancelU" runat="server" CausesValidation="False" 
                        OnClick="btnCancelAcct_Click" Text="Cancel" />
                    &nbsp; &nbsp; &nbsp;&nbsp;
                    <asp:Button ID="btnUpdateAcct" runat="server" OnClick="btnUpdateAcct_Click" 
                        Text="Update Account" Width="108px" />
                </td>
            </tr>
    </table>
    </asp:View>
    <asp:View ID="vwForgotPwd" runat="server">
        <table width="600" border="0" cellpadding="2" cellspacing="0">
        <tr >
          <td colspan="2" align="left">
              Enter the Email address associated with your account, then click Continue.<br />
              <br />
              We&#39;ll email you a new temporary password.<br />
              <br />
            </td>
        </tr>
        <tr>
          <td><div align="right">
          <b>Email Address <span style="color: #ff0066">*</span>
              <br />
              (Your User Name):</b></div></td>
           <td width="375"  align="left">
               <asp:TextBox ID="txtForgotPwdName" runat="server" Width="150px"></asp:TextBox>
               <asp:RequiredFieldValidator ID="rfvforgotPwd" runat="server" 
                   ErrorMessage="* - Required Field" ControlToValidate="txtForgotPwdName" 
                   Display="Dynamic"></asp:RequiredFieldValidator>
               &nbsp; 
               <asp:RegularExpressionValidator ID="rev7" runat="server" 
                   ControlToValidate="txtForgotPwdName" ErrorMessage="Invalid Email Address" 
                   ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
            </td>
        </tr>
 
         
            <tr>
                <td align="center" colspan="2">
                    &nbsp;</td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Button ID="Button1" runat="server" CausesValidation="False" 
                        OnClick="btnCancelAcct_Click" Text="Cancel" />
                    &nbsp; &nbsp;
                    <asp:Button ID="btnContinue" runat="server" OnClick="btnContinue_Click" 
                        Text="Continue" />
                </td>
            </tr>
    </table>
    </asp:View>
    <asp:View ID="vwForgotPwdEmail" runat="server">
        <table width="600" border="0" cellpadding="2" cellspacing="0">
        <tr >
          <td align="left">
              If the Email address you entered:
              <asp:Label ID="lblEmail" runat="server"></asp:Label>
              &nbsp;is associated with an account in our records, you will receive an e-mail from 
              us regarding your password.<br />
              <br />
            </td>
        </tr>

    </table>
    </asp:View>
</asp:MultiView> 
<hr />
</asp:Content>
