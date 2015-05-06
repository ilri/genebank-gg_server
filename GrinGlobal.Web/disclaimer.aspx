<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="disclaimer.aspx.cs" Inherits="GrinGlobal.Web.disclaimer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="~/styles/default.css" rel="stylesheet" type="text/css" />
    <title>Disclaimer</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <h2 align="center">
        <asp:Label ID="lblHeading" runat="server" Text="Material Transfer Agreement"></asp:Label></h2>
    <br />
    <hr />
    <asp:Label ID="lblDescription" runat="server" Text=""></asp:Label>
    <br />
        <asp:Panel ID="pnlGGDisclaimer" runat="server" Visible="False">
            <p>
                This software was created by USDA/ARS, with Bioversity International 
                coordinating testing and feedback from the international genebank community.&nbsp; 
                Development was supported financially by USDA/ARS and by a major grant from the 
                Global Crop Diversity Trust.&nbsp; This statement by USDA does not imply approval of 
                these enterprises to the exclusion of others which might also be suitable.</p>
            <p>
            </p>
            <p>
                USDA grants to each Recipient of this software non-exclusive, royalty free, 
                world-wide, permission to use, copy, modify, publish, distribute, perform 
                publicly and display publicly this software.&nbsp; Notice of this permission as well 
                as the other paragraphs in this notice shall be included in all copies or modifications of this software.</p>
            <p>
            </p>
            <p>
                This software application has not been tested or otherwise examined for 
                suitability for implementation on, or compatibility with, any other computer 
                systems.&nbsp; USDA does not warrant, either explicitly or implicitly, that this 
                software program will not cause damage to the user’s computer or computer 
                operating system, nor does USDA warrant, either explicitly or implicitly, the 
                effectiveness of the software application.</p>
            <p>
            </p>
            <p>
                The English text above shall take precedence in the event of any inconsistencies between the English text and any translation of this notice.
            </p>
        </asp:Panel>
        <br />
        <center><input type="button" value="Exit" onclick="self.close()" /></center>
    <hr />
    </div>
    </form>
</body>
</html>
