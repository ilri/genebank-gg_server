<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.Master" CodeBehind="weborderdetail.aspx.cs" Inherits="GrinGlobal.Web.query.weborderdetail" %>

<%@ Register src="../uploader.ascx" tagname="Uploader" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <script type='text/javascript'>
        $(document).ready(function() {
        });
    </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
 <br />
 
 <table>
     <tr style="background-color:#2f571b; color: #FFFFFF">
         <td  width="250"><b >Order Detail Number:</b> 
             <asp:Label ID="lblOrderIDs" runat="server" ></asp:Label>
        </td>
         <td  
             align="right" width="250"><b>Order Status:</b>
             <asp:Label ID="lblOrderStatus" runat="server" ></asp:Label>
        </td>
     </tr>
     <tr>
         <td >
        </td>
         <td >
        </td>
     </tr>
     <tr>
         <td >
             <b>Requestor:</b></td>
         <td >
             <b>Ship To:</b></td>
     </tr>
     <tr>
         <td >
        </td>
         <td >
        </td>
     </tr>
     <tr>
         <td >
             <asp:Label ID="lblName" runat="server"></asp:Label>
        </td>
         <td >
             <asp:Label ID="lblAdd1" runat="server"></asp:Label>
        </td>
     </tr>
     <tr>
         <td >
             <asp:Label ID="lblOrganization" runat="server"></asp:Label>
        </td>
         <td >
             <asp:Label ID="lblAdd2" runat="server"></asp:Label>
        </td>
     </tr>
     <tr>
         <td >
             <asp:Label ID="lblPhone" runat="server"></asp:Label>
         </td>
         <td >
             <asp:Label ID="lblAdd3" runat="server"></asp:Label>
         </td>
     </tr>
     <tr>
         <td >
             <asp:Label ID="lblFax" runat="server"></asp:Label>
         </td>
         <td >
             <asp:Label ID="lblAdd4" runat="server"></asp:Label>
         </td>
     </tr>
 </table>
 
 <br />
     <b>&nbsp;Ordered Items <asp:Label ID="lblCnt" runat="server" Text=""></asp:Label>:</b><br />
     <asp:GridView ID="gvOrderItems" runat="server" AutoGenerateColumns="False" CssClass="grid" >
    <AlternatingRowStyle BackColor="WhiteSmoke" />           
         <EmptyDataTemplate>
             You have no order items.
         </EmptyDataTemplate>
         <Columns>
             <asp:TemplateField HeaderText="ID">
                 <ItemTemplate>
                     <a href='AccessionDetail.aspx?id=<%# Eval("accession_id") %>'>
                     <%#Eval("pi_number") %></a>
                 </ItemTemplate>
             </asp:TemplateField>
             <asp:BoundField DataField="top_name" HeaderText="Plant Name" />
             <asp:TemplateField HeaderText="Taxonomy">
                 <ItemTemplate>
                     <nobr>
                     <a href='taxonomydetail.aspx?id=<%#Eval("taxonomy_species_id") %>'>
                     <%#Eval("taxonomy_name") %></a></nobr>
                 </ItemTemplate>
             </asp:TemplateField>
             <asp:TemplateField HeaderText="Form Distributed">
                <ItemTemplate>
                <%# GetDisplayText(Eval("distribution_form_code")) %>
                </ItemTemplate>
             </asp:TemplateField>
             <asp:BoundField DataField="status_code" 
                 HeaderText="Item Status" />
             <asp:TemplateField HeaderText="Maintained by">
                 <ItemTemplate>
                     <a href='../site.aspx?id=<%# Eval("site_id") %>'>
                     <%#Eval("site") %></a>
                 </ItemTemplate>
             </asp:TemplateField>
             <asp:BoundField DataField="type_code" HeaderText="Restrictions" />
             <asp:BoundField DataField="note" HeaderText="Comments" ItemStyle-Width="200" />
         </Columns>
     </asp:GridView>
     <br />
     <b>Intended use for this germplasm:</b>
    <br />
 &nbsp;&nbsp;&nbsp;<asp:Label ID="lblIntended" 
         runat="server"></asp:Label>
     <br />
     <b>
     <br />
     Special instructions for the order:</b><br />
     &nbsp;&nbsp;&nbsp;<asp:Label ID="lblSpecial" runat="server"></asp:Label>
    <br />
    <br />
   <asp:Label ID="lblMultiShip" runat="server" 
   Text="You may have ordered accessions from more than one NPGS site and your order maybe split between sites and may be handled in different ways. <br /> You may receive your material in several shipments. &nbsp;" Font-Bold="True" CssClass="blueInfor"></asp:Label>
    <br />
    <br />
     <b>&nbsp;Order Request Actions:</b><br />
    <asp:GridView ID="gvOrderActions" runat="server" AutoGenerateColumns="False" 
         CssClass="grid">
         <EmptyDataTemplate>
             No order action found.
         </EmptyDataTemplate>
         <Columns>
            <asp:TemplateField HeaderText="Action Step">
                <ItemTemplate>
                    <asp:Label ID="lblStep" ToolTip='<%# Eval("action_title") %>' runat="server" Text='<%# Eval("action_code") %>'></asp:Label>
                 </ItemTemplate>
            </asp:TemplateField>
             <asp:TemplateField HeaderText="Action Date">
                 <ItemTemplate>
                    <%# Eval("acted_date", "{0:MMMM d, yyyy}") %>
                 </ItemTemplate>
             </asp:TemplateField>
             <asp:TemplateField HeaderText="Action Note">
                 <ItemTemplate>
                     <asp:Label runat="server" Text='<%# Eval("note") %>' Width="500" />
                 </ItemTemplate>
             </asp:TemplateField>
         </Columns>
     </asp:GridView>
     <br />
    <uc1:Uploader ID="Uploader1" runat="server" />
     <br />
     <br />
     <hr />
 </asp:Content>