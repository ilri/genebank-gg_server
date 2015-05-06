<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="useraddress.aspx.cs" Inherits="GrinGlobal.Web.UserAddress" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type='text/javascript'>
        $(document).ready(function() {
        });
    </script>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:MultiView ID="mvAddress" runat="server">
    <asp:View ID="vwAddEdit" runat="server">
    <table>
    <tr bgcolor="#2f571b" style="color: #FFFFFF"><td colspan="2"><asp:Label 
            ID="lblAddEdit" runat="server" Font-Bold="True" Text="Add new address"></asp:Label>
        </td>
    </tr>
    <tr><td></td><td></td></tr>
    <tr><td></td><td></td></tr>
    <tr><td><span style="color: #ff0066">*</span>Address Name:&nbsp;</td>
    <td><asp:TextBox ID="txtAddrName" runat="server" TabIndex="22" Width="250px" 
                    MaxLength="50"></asp:TextBox>
               <asp:RequiredFieldValidator ID="rfvAddrName" runat="server" 
                ControlToValidate="txtAddrName" ErrorMessage="* - Required Field"></asp:RequiredFieldValidator></td>
     </tr>
   <tr><td><span style="color: #ff0066">*</span>Address Line 1:&nbsp;</td>
    <td><asp:TextBox ID="txtAddr1" runat="server" TabIndex="22" Width="250px" 
                    MaxLength="100"></asp:TextBox>
               <asp:RequiredFieldValidator ID="rfvAddr1" runat="server" 
                ControlToValidate="txtAddr1" ErrorMessage="* - Required Field"></asp:RequiredFieldValidator></td>
     </tr>    
   <tr><td>&nbsp;&nbsp;Address Line 2:&nbsp;</td>
    <td><asp:TextBox ID="txtAddr2" runat="server" TabIndex="22" Width="250px" 
                    MaxLength="100"></asp:TextBox>
               </td>
     </tr>  
        <tr><td>&nbsp;&nbsp;Address Line 3:&nbsp;</td>
    <td><asp:TextBox ID="txtAddr3" runat="server" TabIndex="22" Width="250px" 
                    MaxLength="100"></asp:TextBox>
               </td>
     </tr>
         <tr><td><span style="color: #ff0066">*</span>Country:</td><td>
        <asp:DropDownList ID="ddlCountry" runat="server" AutoPostBack="True" 
            onselectedindexchanged="ddlCountry_SelectedIndexChanged">
        </asp:DropDownList>
        </td></tr>

        <tr><td><span style="color: #ff0066">*</span>City:&nbsp;</td>
    <td><asp:TextBox ID="txtAddrCity" runat="server" TabIndex="22" 
                    MaxLength="50"></asp:TextBox>
               <asp:RequiredFieldValidator ID="rfvAddrCity" runat="server" 
                ControlToValidate="txtAddrCity" ErrorMessage="* - Required Field"></asp:RequiredFieldValidator></td>
     </tr>
    <tr><td><span style="color: #ff0066">*</span>State/Province</td><td>
        <asp:DropDownList ID="ddlState" runat="server">
        </asp:DropDownList>
        <asp:RequiredFieldValidator ID="rfvState" runat="server" 
                ControlToValidate="ddlState" ErrorMessage="* - Required Field" 
                InitialValue="0"></asp:RequiredFieldValidator>
        </td></tr>
       <tr><td><span style="color: #ff0066">*</span>Zip/Postal Code:&nbsp;</td>
    <td><asp:TextBox ID="txtAddrZip" runat="server" TabIndex="22" 
                    MaxLength="50"></asp:TextBox>
               <asp:RequiredFieldValidator ID="rfvAddrZip" runat="server" 
                ControlToValidate="txtAddrZip" ErrorMessage="* - Required Field"></asp:RequiredFieldValidator>
    <asp:RegularExpressionValidator ID="revAddrZip2" runat="server" 
                ControlToValidate="txtAddrZip" ErrorMessage="Invalid Zip/Postal Code" 
                ValidationExpression="[\w-]*" Enabled="False"></asp:RegularExpressionValidator></td>
     </tr>
        
    <tr><td></td><td></td></tr>
    <tr><td></td><td></td></tr>
    <tr><td>
        &nbsp;</td><td>
            <asp:Button ID="btnSave" runat="server" Text="Save" 
                onclick="btnSave_Click" />
            &nbsp;&nbsp; &nbsp;
            <asp:Button ID="btnCancelEdit" runat="server" Text="Cancel" 
                CausesValidation="False" onclick="btnCancel_Click" />
        </td></tr>
    </table>
    <br />
    <hr />
    </asp:View>
    <asp:View ID="vwDisplay" runat="server">
        Click the Edit button next to the information below to make the change.
        <br />
        <br />
        Click here to add a new shipping address 
        <asp:Button ID="btnEnterNew" runat="server" Text="Add a new address" 
            onclick="btnEnterNew_Click" Width="124px" />
        <br />
    <br />
    <hr />
    <asp:GridView ID="gvAddress" runat="server" AutoGenerateColumns="False"  
        CellPadding="5" GridLines="Horizontal" DataKeyNames="web_user_shipping_address_id" 
        BorderStyle="None" BorderWidth="0px"  ShowHeader="False" 
            Caption="My Address Book" onrowcommand="gvAddress_RowCommand" 
            onrowediting="gvAddress_RowEditing" onrowdeleting="gvAddress_RowDeleting">
    <AlternatingRowStyle BackColor="WhiteSmoke" />
    <EmptyDataTemplate>
    You have an empty shipping address book.
    </EmptyDataTemplate>
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <table style="border-style: none">
                    <tr>
                        <td width="120"><asp:CheckBox ID="cbDefault" runat="server" Checked=<%# MarkDefault(Eval("is_default")) %> Text='<%# MarkDefaultText(Eval("is_default")) %>' Enabled="False" /></td>
                        <td width="350">
                            <b><%# Eval("address_name") %></b>
                         </td>
                         <td>
                         <asp:Button ID="btnEdit" runat="server" CommandName="Edit" CommandArgument='<%# Eval("web_user_shipping_address_id") %>' ToolTip="Edit address" Text="Edit" />  
                         <asp:Button ID="btnDefault" runat="server" CommandName="MarkDefault" CommandArgument='<%# Eval("web_user_shipping_address_id") %>' ToolTip="Set as default shipping address" Text="Set Default" />
                         <asp:Button ID="btnDelete" runat="server" CommandName="Delete" CommandArgument='<%# Eval("web_user_shipping_address_id") %>' ToolTip="Delete address" OnClientClick="javascript:return confirm('Are you sure you want to delete this address?');" Text="Delete" />
                         </td>
                    </tr>                      
                    <tr>
                        <td></td>
                        <td><%# Eval("address_line1")%> 
                        </td><td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><%# Eval("address_line2")%> 
                        </td><td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><%# Eval("address_line3")%> 
                        </td><td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><%# Eval("city")%>, <%# Eval("state_name")%>  <%# Eval("postal_index")%>  
                        </td><td></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><%# Eval("country_name")%> 
                        </td><td></td>
                    </tr>
                    </table>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns> 
    </asp:GridView>
    <hr />
    </asp:View>
    </asp:MultiView>
</asp:Content>