<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="orderhistory.aspx.cs" Inherits="GrinGlobal.Web.OrderHistory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type='text/javascript'>
        $(document).ready(function() {
        });
    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <asp:Label ID="lblSummary" runat="server" Text="You have no order history."></asp:Label>
    <br />
    <hr />
    
    <asp:GridView ID="gvOrders" runat="server" AutoGenerateColumns="False" 
        CellPadding="5" GridLines="Horizontal" DataKeyNames="orderID" 
        OnSelectedIndexChanged="gvOrders_SelectedIndexChanged" BorderStyle="None" 
        BorderWidth="0px"  ShowHeader="False" 
        >
    <AlternatingRowStyle BackColor="WhiteSmoke" />
    <EmptyDataTemplate>
    No order request found.
   </EmptyDataTemplate>
         <Columns>
            <asp:TemplateField>
               <ItemTemplate>
                      <table style="border-style: none">
                      <tr><td> order placed on:</td><td></td></tr>
                      <tr>
                        <td style="text-align:left;font-weight:bold;font-size:1.2em">
                            <asp:Label runat="server" Text='<%# Eval("statusDate", "{0:MMMM d, yyyy}") %>' ID="Label1"/>
                         </td><td></td>
                      </tr>
                      <tr>
                          <td>
                           <asp:LinkButton runat="server" Text='view detail' CommandName="Select" ID="LinkButton1"/>
                          </td><td><%#Eval("special_instruction") %></td> 
                      </tr>
                      <tr></tr>
                      <tr>
                        <td >
                            Order Request Number: <asp:Label runat="server" Text= '<%# Eval("orderID") %>'  ID="Label12" Font-Bold="True" />
                         </td><td></td>
                      </tr>
                      <tr>
                        <td >
<%--                            Order Request Status: <asp:Label runat="server" Text= '<%# GetOrderStatus(Eval("orderID"), Eval("status_code")) %>'  ID="Label4"/>
--%>                         </td><td></td>
                      </tr>
                  </table>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns> 
    </asp:GridView>
    <br />
    <hr />
</asp:Content>