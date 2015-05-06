<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="query.aspx.cs" Inherits="GrinGlobal.Web.query.query" MasterPageFile="~/Site1.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Query and Report</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
        <div>
        <asp:Label ID="lblError" runat="server" BorderStyle="Solid" BorderWidth="1px" 
            Font-Bold="True" ForeColor="Red" Text="-" Visible="false" 
            Width="700px"></asp:Label>
    </div>
    <asp:Panel ID="pnlChoose" runat="server" Visible="False">
    <div>
       <h3><asp:Label ID="lblChoose" runat="server" Text="Choose Report:"></asp:Label></h3>
    </div>
    <asp:DropDownList ID="ddlDataView" runat="server" DataTextField="title" 
        DataValueField="dataview_name" AutoPostBack="true"
        onselectedindexchanged="ddlDataView_SelectedIndexChanged">
        </asp:DropDownList>
        <br /><br /></asp:Panel>
    <asp:Panel ID="PnlDesc" runat="server" Visible="False">
        <h3>Report Description:</h3><%--        <asp:TextBox ID="txtDesc" runat="server" ReadOnly="True" TextMode="MultiLine" 
            Width="600px" Height="80"></asp:TextBox>
--%>            <asp:Label ID="lblDesc" runat="server" Text="" Width="600" CssClass="box"></asp:Label></asp:Panel><asp:Panel ID="pnlParam" runat="server" Visible="False">
        <h3>Enter Parameter Value(s):</h3><asp:GridView ID="gvParamValues" runat="server" AutoGenerateColumns="False" CssClass="grid">
        <Columns>
            <%-- <asp:BoundField DataField="param_name" HeaderText="Name" /> --%>
            <asp:TemplateField HeaderText="Name">
                <ItemTemplate>
                    <asp:Label ID="lblParamName" runat="server" Text='<%# String.Format("{0}", Eval("param_name")).Substring(1, Eval("param_name").ToString().Length - 1) %>'></asp:Label></ItemTemplate></asp:TemplateField><asp:TemplateField HeaderText="Value">
                <ItemTemplate>
                    <asp:HiddenField ID="hidParamName" runat="server" Value='<%# Bind("param_name") %>' />
                    <asp:TextBox ID="txtParamValue" runat="server" Text='<%# Bind("param_value") %>'></asp:TextBox></ItemTemplate></asp:TemplateField></Columns></asp:GridView><br />
        &nbsp;Limit:
        <asp:TextBox ID="txtLimit" runat="server">1000</asp:TextBox><%--        &nbsp;&nbsp;&nbsp;Offset:--%>        
        <asp:TextBox ID="txtOffset" runat="server" Visible="False">0</asp:TextBox><br /><br />
           <asp:Button Text="Generate Report" ID="btnGetData" runat="server" 
            onclick="btnGetData_Click" Visible="True" />
        <br /></asp:Panel>
            <br />
        <asp:Panel ID="pnlData" runat="server" Visible="False">
        <asp:Label id="lblRowCount" runat="server"></asp:Label>&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btnExport" runat="server" onclick="btnExport_Click" 
        Text="Export Data to Excel" />
        <br /><br /> 
               
    <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="true" CssClass="grid" onrowdatabound="gvData_RowDataBound" HeaderStyle-HorizontalAlign="Left"> 
    <AlternatingRowStyle CssClass="altrow" /></asp:GridView></asp:Panel>
<br />
</asp:Content>