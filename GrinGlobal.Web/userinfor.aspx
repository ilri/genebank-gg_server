<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="userinfor.aspx.cs" Inherits="GrinGlobal.Web.UserInfor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type='text/javascript'>
        $(document).ready(function() {
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:MultiView ID="mv2" runat="server" ActiveViewIndex="0">
    <asp:View ID="vwEnterInfor" runat="server">
    <table id="tUserAcct" runat="server" width="525" cellpadding="2" cellspacing="0">
        <tr bgcolor="#2f571b" style="color: #FFFFFF">
            <td width="155" align="left"><b>Log-in Information</b></td>
            <td align="right" >
                <a href="UserAcct.aspx?action=editAcct" style="color: #FFFFFF"><b>Edit</b></a>
            </td>
        </tr>
            <tr>
                <td align="right" width="155">
                    <b>Login User Name:&nbsp;</b></td>
                    <td align="left" style="width: 370px">
                        <asp:Label ID="lblUserName" runat="server"></asp:Label>
                    </td>
            </tr>
         <tr>
            <td width="155" align="right"><b>Password:&nbsp;</b></td><td align="left" style="width: 370px">
             <asp:Label ID="lblPassword" runat="server"></asp:Label>
             </td>
        </tr></table>
    <table>
   <tr><td colspan="5" align="center"><hr /></td></tr>
        <tr>
            <td align="center" colspan="5">
                <b>Please enter your information</b></td>
        </tr>
        <tr><td>&nbsp; Title:</td><td style="margin-left: 40px">
            <asp:DropDownList ID="ddlTitle" runat="server">
            </asp:DropDownList>
            </td><td></td><td >
           </td><td>&nbsp;</td></tr>
        <tr>
        <td><span style="color: #ff0066">*</span>First Name:</td><td>
            <asp:TextBox ID="txtFirstname" runat="server" MaxLength="100" Width="200px" 
                TabIndex="2"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" 
                ControlToValidate="txtFirstname" ErrorMessage="* - Required"></asp:RequiredFieldValidator>
        </td><td>
            &nbsp;</td><td>
        </td>
        <td>
            &nbsp;<asp:Label ID="lblUseAddrBook" runat="server" 
                Text="Select from your address book:" Visible="False"></asp:Label>
            </td>
    </tr>
    <tr>
        <td><span style="color: #ff0066">*</span>Last Name:</td><td>
            <asp:TextBox ID="txtLastname" runat="server" MaxLength="100" Width="200px" 
                TabIndex="3"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvLastName" runat="server" 
                ControlToValidate="txtLastname" ErrorMessage="* - Required"></asp:RequiredFieldValidator>
        </td><td>
            &nbsp;</td><td >
            <b>Shipping Address:</b></td>
        <td>
            &nbsp;<asp:DropDownList ID="ddlShipping" runat="server" 
                onselectedindexchanged="ddlShipping_SelectedIndexChanged" TabIndex="20" 
                Visible="False" AutoPostBack="True">
            </asp:DropDownList>
        </td>
    </tr>
    <tr>
        <td><b>Organization:</b></td><td>
            &nbsp;</td><td >
            &nbsp;</td><td >
        </td>
        <td>
            <asp:CheckBox ID="cbSameAddress" runat="server" AutoPostBack="True" 
                oncheckedchanged="cbSameAddress_CheckedChanged" TabIndex="21" 
                Text="Shipping Address is same" />
        </td></tr><tr>
        <td>
            <span style="color: #ff0066">*</span>Organization:</td><td>
            <asp:TextBox ID="txtOrganization" runat="server" Width="250px" TabIndex="4" 
                    MaxLength="100"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvOrgName" runat="server" 
                    ControlToValidate="txtOrganization" ErrorMessage="* - Required"></asp:RequiredFieldValidator>
            </td><td >
            &nbsp;</td><td >
                <span style="color: #ff0066">*</span>Address Name:&nbsp;</td><td>
                <asp:TextBox ID="txtAddressName" runat="server" TabIndex="22" Width="250px" 
                    MaxLength="50"></asp:TextBox>
               <asp:RequiredFieldValidator ID="rfvAddName" runat="server" 
                ControlToValidate="txtAddressName" ErrorMessage="* - Required"></asp:RequiredFieldValidator>
        </td>
    </tr>
    <tr>
        <td><span style="color: #ff0066">*</span>Address Line 1:</td><td>
            <asp:TextBox ID="txtAddr1" runat="server" Width="250px" TabIndex="5" 
                MaxLength="100"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvAddr1" runat="server" 
                ControlToValidate="txtAddr1" ErrorMessage="* - Required"></asp:RequiredFieldValidator>
        </td><td >&nbsp;</td><td><span style="color: #ff0066">*</span>Shipping Address 
        Line 1:</td><td>
            <asp:TextBox ID="txtShipAddr1" runat="server" Width="250px" TabIndex="23" 
                MaxLength="100" AutoPostBack="True" 
                ontextchanged="txtShipAddr1_TextChanged"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvShipAddr1" runat="server" 
                ControlToValidate="txtShipAddr1" ErrorMessage="* - Required"></asp:RequiredFieldValidator>

                </td></tr><tr>
        <td>&nbsp;Address Line 2:</td><td style="margin-left: 40px">
            <asp:TextBox ID="txtAddr2" runat="server" Width="250px" TabIndex="6" 
                MaxLength="100"></asp:TextBox></td><td >&nbsp;</td><td >
            &nbsp;Shipping Address Line 2:</td><td>
            <asp:TextBox ID="txtShipAddr2" runat="server" Width="250px" TabIndex="24" 
                    MaxLength="100" AutoPostBack="True" 
                    ontextchanged="txtShipAddr2_TextChanged"></asp:TextBox></td></tr><tr>
        <td>&nbsp;Address Line 3:</td><td>
            <asp:TextBox ID="txtAddr3" runat="server" Width="250px" TabIndex="7" 
                MaxLength="100"></asp:TextBox></td><td >&nbsp;</td><td >
            &nbsp;Shipping Address Line 3:</td><td>
            <asp:TextBox ID="txtShipAddr3" runat="server" Width="250px" TabIndex="25" 
                    MaxLength="100" AutoPostBack="True" 
                    ontextchanged="txtShipAddr3_TextChanged"></asp:TextBox></td></tr><tr>
        <td><span style="color: #ff0066">*</span>Country</td><td>
            <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="True" 
                onselectedindexchanged="ddlCountry_SelectedIndexChanged" TabIndex="11">
            </asp:DropDownList>
            </td><td >&nbsp;</td><td><span style="color: #ff0066">*</span>Shipping Country</td><td>
            <asp:DropDownList ID="ddlShipCountry" runat="server" AutoPostBack="True" 
                onselectedindexchanged="ddlShipCountry_SelectedIndexChanged" TabIndex="29">
            </asp:DropDownList>
            </td></tr><tr>
        <td><span style="color: #ff0066">*</span>City:</td><td>
            <asp:TextBox ID="txtCity" runat="server" MaxLength="100" TabIndex="8"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvCity" runat="server" 
                ControlToValidate="txtCity" ErrorMessage="* - Required"></asp:RequiredFieldValidator>
            </td><td >&nbsp;</td><td><span style="color: #ff0066">*</span>Shipping City:</td><td>
                <asp:TextBox ID="txtShipCity" runat="server" AutoPostBack="True" 
                    MaxLength="100" ontextchanged="txtShipCity_TextChanged" TabIndex="26"></asp:TextBox>
            <asp:RequiredFieldValidator ID="rfvShipCity" runat="server" 
                ControlToValidate="txtShipCity" ErrorMessage="* - Required"></asp:RequiredFieldValidator>
            </td></tr><tr>
        <td><span style="color: #ff0066">*</span>State/Province:</td><td>
            <asp:DropDownList ID="ddlState" runat="server" CausesValidation="True" 
                TabIndex="9">
            </asp:DropDownList>
            <asp:RequiredFieldValidator ID="rfvState" runat="server" 
                ControlToValidate="ddlState" ErrorMessage="* - Required" InitialValue="0"></asp:RequiredFieldValidator>
            </td><td >&nbsp;</td><td><span style="color: #ff0066">*</span>Shpping 
            State/Province:</td><td>
                <asp:DropDownList ID="ddlShipState" runat="server" AutoPostBack="True" 
                    CausesValidation="True" 
                    onselectedindexchanged="ddlShipState_SelectedIndexChanged" TabIndex="27">
                </asp:DropDownList>
                           <asp:RequiredFieldValidator ID="rfvShipState" runat="server" 
                ControlToValidate="ddlShipState" ErrorMessage="* - Required" InitialValue="0"></asp:RequiredFieldValidator>

            </td></tr>   
 <tr><td><span style="color: #ff0066">*</span>Zip/Postal Code:</td><td>
     <asp:TextBox ID="txtZip" runat="server" MaxLength="100" TabIndex="10" 
         Width="80px"></asp:TextBox>
     <asp:RequiredFieldValidator ID="rfvZip" runat="server" 
         ControlToValidate="txtZip" Display="Dynamic" ErrorMessage="* - Required"></asp:RequiredFieldValidator>
     <asp:RegularExpressionValidator ID="rev2" runat="server" 
         ControlToValidate="txtZip" Display="Dynamic" Enabled="False" 
         ErrorMessage="Invalid Zip/Postal Code" ValidationExpression="[\w\d-]*"></asp:RegularExpressionValidator>
     </td><td>&nbsp;</td><td><span style="color: #ff0066">*</span>Shipping Zip/Postal 
     Code:</td><td>
         <asp:TextBox ID="txtShipZip" runat="server" AutoPostBack="True" MaxLength="100" 
             ontextchanged="txtShipZip_TextChanged" TabIndex="28" Width="80px"></asp:TextBox>
         <asp:RequiredFieldValidator ID="rfvShipZip" runat="server" 
             ControlToValidate="txtShipZip" Display="Dynamic" ErrorMessage="* - Required"></asp:RequiredFieldValidator>
         <asp:RegularExpressionValidator ID="rev1" runat="server" 
             ControlToValidate="txtShipZip" Display="Dynamic" Enabled="False" 
             ErrorMessage="Invalid Zip/Postal Code" ValidationExpression="[\w\d-]*"></asp:RegularExpressionValidator>
     </td></tr>               
        <tr><td></td><td></td><td></td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr><tr>
        <td colspan="2"><b>Contact information:</b></td><td>
        </td><td colspan="2"><b>Expedite the shipping of this order using my account:</b></td></tr>
    <tr> 
            <td>
                <span style="color: #ff0066">*</span>Phone:</td>
            <td>
                <asp:TextBox ID="txtPhone" runat="server" MaxLength="30" TabIndex="12"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPhone" runat="server" 
                    ControlToValidate="txtPhone" Display="Dynamic" ErrorMessage="* - Required"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="rev3" runat="server" 
                    ControlToValidate="txtPhone" ErrorMessage="Invalid Phone Number" 
                    ValidationExpression="[\d|()-. ]*" Display="Dynamic"></asp:RegularExpressionValidator>
            </td>
            <td>
                </td>
            <td>
                Carrier</td>
            <td>
                <asp:DropDownList ID="ddlCarrier" runat="server" TabIndex="30">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem>FedEx</asp:ListItem>
                    <asp:ListItem>DHL</asp:ListItem>
                    <asp:ListItem>UPS</asp:ListItem>
                </asp:DropDownList>
                &nbsp;</td>
     </tr>
            <tr>
                <td>
                    &nbsp; Alt Phone:</td>
                <td>
                    <asp:TextBox ID="txtAltPhone" runat="server" MaxLength="30" TabIndex="12"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
                        ControlToValidate="txtAltPhone" ErrorMessage="Invalid Phone Number" 
                        ValidationExpression="[\d|()-. ]*"></asp:RegularExpressionValidator>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    Account Number</td>
                <td>
                    <asp:TextBox ID="txtCarrierAcct" runat="server" MaxLength="50" TabIndex="31" 
                        Width="250px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp; FAX:</td>
                <td>
                    <asp:TextBox ID="txtFax" runat="server" MaxLength="30" 
                        TabIndex="13"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="rev5" runat="server" 
                   ControlToValidate="txtFax" ErrorMessage="Invalid Fax  Number" 
                   ValidationExpression="[\d|()-. ]*"></asp:RegularExpressionValidator>

                </td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    <span style="color: #ff0066">*</span>E-Mail:</td>
                <td>
                    <asp:TextBox ID="txtEmail" runat="server" TabIndex="14" Width="220px" 
                        Enabled="False" MaxLength="100"></asp:TextBox>
                    <asp:RegularExpressionValidator ID="revEmail" runat="server" 
                        ControlToValidate="txtEmail" ErrorMessage="Invalid Email Address" 
                        ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    Note:</td>
                <td colspan="4">
                    <asp:TextBox ID="txtNote" runat="server" Height="40px" TabIndex="15" 
                        TextMode="MultiLine" Width="787px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <b>Preferences:</b></td>
                <td colspan="4">
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:CheckBox ID="cbEmailOrder" runat="server" TabIndex="32" 
                        Text="Email me a copy of my orders when placed" />
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:CheckBox ID="cbEmailShipping" runat="server" TabIndex="33" 
                        Text="Email me when my orders are shipped" Visible="False" />
                </td>
            </tr>
        <tr>
            <td colspan="5">
                <asp:CheckBox ID="cbEmailInfor" runat="server" TabIndex="34" 
                    Text="Email me news &amp; information about new features of GRIN-Global" />
            </td>
        </tr>
        </table><br />
                <asp:Button ID="btnCoopCancel" runat="server" 
            OnClick="btnCoopCancel_Click" Text="Cancel" CausesValidation="False" /> &nbsp;&nbsp;
    <asp:Button 
            ID="btnCooperator" runat="server" 
        Text="Submit" Width="70px" onclick="btnCooperator_Click" TabIndex="35" /> <br /><br />
    </asp:View>
    
    <asp:View ID="vwDisplayInfor" runat="server">
        <asp:HyperLink ID="hlOrderHistory" runat="server" NavigateUrl="~/OrderHistory.aspx">My Order History</asp:HyperLink> &nbsp;&nbsp;
        <asp:HyperLink ID="hlUserPref" runat="server" NavigateUrl="~/UserPref.aspx">My Preference</asp:HyperLink> &nbsp;&nbsp;
        <asp:HyperLink ID="hlUserAddressBook" runat="server" NavigateUrl="~/UserAddress.aspx">My Address Book</asp:HyperLink> &nbsp;&nbsp;
        <asp:HyperLink ID="hlUserFav" runat="server" NavigateUrl="~/userfavorite.aspx">My Favorites</asp:HyperLink>
    <br /><br />
    <table id="tUserAcct2" runat="server" cellpadding="2" cellspacing="0">
        <tr bgcolor="#2f571b" style="color: #FFFFFF">
            <td width="155" align="left" ><b>Log-in Information</b></td>
            <td align="right" >
                <a href="UserAcct.aspx?action=editAcct" style="color: #FFFFFF"><b>Edit</b></a>
            </td>
        </tr>
            <tr>
                <td align="right" width="155">
                    <b>Login User Name:&nbsp;</b></td>
                    <td align="left" style="width: 370px">
                        <asp:Label ID="lblUserName2" runat="server"></asp:Label>
                    </td>
            </tr>
         <tr>
            <td width="155" align="right"><b>Password:&nbsp;</b></td><td align="left" style="width: 370px">
             <asp:Label ID="lblPassword2" runat="server"></asp:Label>
             </td>
        </tr></table>
      <hr />                   

        <table id="tblUser" cellpadding="2" cellspacing="0">
            <tr>
                <td align="left" colspan="5">
                     <asp:Label ID="lblVerify" runat="server" 
                        Text="Please review your information below. Make any changes by clicking &quot;Edit&quot; link if necessary."></asp:Label>
                </td>
            </tr>
            <tr bgcolor="#2f571b" style="color: #FFFFFF">
                <td align="left" colspan="4">
                    <b>Requestor Information</b></td>
                <td align="right">
                    <a href="UserInfor.aspx?action=editInfor" style="color: #FFFFFF"><b>Edit</b></a>
                </td>
            </tr>
            <tr>
                <td width="125px">
                    Title:</td>
                <td>
                    <asp:Label ID="lblTitle" runat="server"></asp:Label>
                </td>
                <td width="75px" >
                    &nbsp;</td>
                <td >
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    First Name:</td>
                <td>
                    <asp:Label ID="lblFirstName" runat="server"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    Last Name:</td>
                <td>
                    <asp:Label ID="lblLastName" runat="server"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr><td></td><td></td><td></td><td></td><td></td></tr>
            <tr>
                <td>
                    <b>Organization:</b></td>
                <td>
                    &nbsp;</td>
                <td>
                    &nbsp;</td>
                <td>
                </td>
                <td>
                    &nbsp;</td>
            </tr>
            <tr>
                <td>
                    Name:</td>
                <td>
                    <asp:Label ID="lblOrganization" runat="server"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    <b>Shipping Address:</b></td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    Address Line 1:</td>
                <td>
                    <asp:Label ID="lblAddr1" runat="server"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    Shipping Address Line 1:</td>
                <td>
                    <asp:Label ID="lblShipAddr1" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Address Line 2:</td>
                <td>
                    <asp:Label ID="lblAddr2" runat="server"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    Shipping Address Line 2:</td>
                <td>
                    <asp:Label ID="lblShipAddr2" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Address Line 3:</td>
                <td>
                    <asp:Label ID="lblAddr3" runat="server"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    Shipping Address Line 3:</td>
                <td>
                    <asp:Label ID="lblShipAddr3" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    City:</td>
                <td>
                    <asp:Label ID="lblCity" runat="server"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    Shipping City:</td>
                <td>
                    <asp:Label ID="lblShipCity" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    State/Province:</td>
                <td>
                    <asp:Label ID="lblState" runat="server"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    Shpping State/Province:</td>
                <td>
                    <asp:Label ID="lblShipState" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Zip/Postal Code:</td>
                <td>
                    <asp:Label ID="lblZip" runat="server"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    Shipping Zip/Postal Code:</td>
                <td>
                    <asp:Label ID="lblShipZip" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Country</td>
                <td>
                    <asp:Label ID="lblCountry" runat="server"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                    Shipping Country</td>
                <td>
                    <asp:Label ID="lblShipCountry" runat="server"></asp:Label>
                </td>
            </tr>
           <tr><td></td><td></td><td></td><td></td><td></td></tr>
           <tr><td></td><td></td><td></td><td></td><td></td></tr>
           <tr> 
           <td colspan="2">
                    <b>Contact information:</b></td>
                    <td></td><td colspan="2"><b>Carrier account for expediting the shipping of 
               this order:</b></td>
            </tr>
            <tr>
                <td>
                    Phone:</td>
                <td>
                    <asp:Label ID="lblPhone" runat="server"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>Carrier
                </td>
                <td><asp:Label ID="lblCarrier" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    Alt Phone:</td>
                <td>
                    <asp:Label ID="lblAltPhone" runat="server"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>Account Number
                </td>
                <td><asp:Label ID="lblCarrierAcct" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    FAX:</td>
                <td>
                    <asp:Label ID="lblFax" runat="server"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    E-Mail:</td>
                <td>
                    <asp:Label ID="lblEmail" runat="server"></asp:Label>
                </td>
                <td>
                    &nbsp;</td>
                <td>
                </td>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                    Note:</td>
                <td colspan="4">
                    <asp:Label ID="lblNote" runat="server"></asp:Label>
                </td>
            </tr>
            <tr><td></td><td></td><td></td><td></td><td></td></tr> 
            <tr><td></td><td></td><td></td><td></td><td></td></tr> 
            <tr>
                <td>
                    <b>Preferences:</b></td>
                <td colspan="4">
                    &nbsp;</td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:CheckBox ID="cbEmail1" runat="server" Enabled="False" 
                        Text="Email me a copy of my orders when placed" />
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:CheckBox ID="cbShipping1" runat="server" Enabled="False" 
                        Text="Email me when my orders are shipped" Visible="False" />
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <asp:CheckBox ID="cbEmail2" runat="server" Enabled="False" 
                        Text="Email me news &amp; information about new features of GRIN-Global" />
                </td>
            </tr>
        </table>
        <br />
        <asp:Button ID="btnOKCancel" runat="server" OnClick="btnCoopCancel_Click" 
            Text="Cancel" Visible="False" />
        &nbsp;
        <asp:Button ID="btnOK" runat="server" onclick="btnOK_Click" 
            Text="Continue Check Out" Width="130px" /> <hr />
        <br /> <br />
    </asp:View>
</asp:MultiView> 
</asp:Content>