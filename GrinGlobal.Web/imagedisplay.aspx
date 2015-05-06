<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="imagedisplay.aspx.cs" Inherits="GrinGlobal.Web.ImagaDisplay" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Display Image</title>
    <link runat='server' id='lnkTheme' href="styles/default.css" rel='stylesheet' />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:Label ID="lblInformation" runat="server" Text="The Image requested can't be found." Visible="false" Font-Bold="True"></asp:Label>
    <img id="image1" runat="server"  alt="Image" class="displayImage" />
    </div>
    
    </form>
</body>
</html>
