<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cropmarker.aspx.cs" Inherits="GrinGlobal.Web.cropmarker" MasterPageFile="~/Site1.Master"%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
    <br /><asp:HyperLink ID="hlView" runat="server" Visible="False">View</asp:HyperLink><asp:Label ID="lblS" runat="server" Text=" | " Visible="False"></asp:Label><asp:LinkButton ID="lbDownload" runat="server" OnClick="btnDownload_Click" Visible="False">Download</asp:LinkButton><asp:Label ID="lblView" runat="server" Text=" this marker" Visible="False"></asp:Label><asp:Label ID="lblMarker" runat="server" Text="" Visible="False"></asp:Label>
    <br /><br />
    <h1><asp:Label ID="lblMarker1" runat="server" Text=""></asp:Label></h1>
    <table id="tblMarker1" runat="server" cellspacing="0" rules="all" border="1" style="border-collapse:collapse;" width="980">
    </table>
    <br />
    <h1><asp:Label ID="lblMarker2" runat="server" Text=""></asp:Label></h1>
    <table id="tblMarker2" runat="server" cellspacing="0" rules="all" border="1" style="border-collapse:collapse;" width="980">
    </table>
    <br />
    <asp:Repeater ID="rptAssay" runat="server">
        <ItemTemplate>
            <h1>Assay Details for the Evaluation <%# Eval("Method") %></h1>
            <table id="tblAssay" runat="server" cellspacing="0" rules="all" border="1" style="border-collapse:collapse;" width="980">
            <tr visible='<%# Eval("Evaluation_Methods").ToString() != ""%>' >
                <th><b>Evaluation Method</b></th>
                <td><%# Eval("Evaluation_Methods")%></td>
            </tr>  
            <tr visible='<%# Eval("Assay_Method").ToString() != ""%>'>
                <th><b>Assay Method</b></th>
                <td><%# Eval("Assay_Method")%></td>
            </tr>  
            <tr visible='<%# Eval("Scoring_Method").ToString() != ""%>'>
                <th><b>Scoring Method</b></th>
                <td><%# Eval("Scoring_Method")%></td>
            </tr>  
            <tr visible='<%# Eval("Control_Values").ToString() != ""%>' >
                <th><b>Control Value</b></th>
                <td><%# Eval("Control_Values")%></td>
            </tr>  
            <tr visible='<%# Eval("Observation_Alleles_Count").ToString() != ""%>'>
                <th><b>Number of Observed Alleles</b></th>
                <td><%# Eval("Observation_Alleles_Count")%></td>
            </tr>  
            <tr visible='<%# Eval("Max_Gob_Alleles").ToString() != ""%>'>
                <th><b>Max Gob Alleles</b></th>
                <td><%# Eval("Max_Gob_Alleles")%></td>
            </tr>  
            <tr visible='<%# Eval("Size_of_Alleles").ToString() != ""%>'>
                <th><b>Size of Alleles</b></th>
                <td><%# Eval("Size_of_Alleles")%></td>
            </tr>  
            <tr visible='<%# Eval("Unusual_Alleles").ToString() != ""%>'>
                <th><b>Unusual Alleles</b></th>
                <td><%# Eval("Unusual_Alleles")%></td>
            </tr>  

           </table>
            <br />
        </ItemTemplate>
    </asp:Repeater>
    <br />
    <asp:GridView ID="gvMarker" runat="server" Visible="False">
    </asp:GridView>
<hr /> 
</asp:Content>