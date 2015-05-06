<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="trustedsite.aspx.cs" Inherits="GrinGlobal.Web.TrustedSite" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphBody" runat="server">
<p>
Internet Explorer (IE) makes certain precautions when downloading files.  One of these is prompting you via the 'Information Bar' at the top of your window when a potentially harmful file is being downloaded.  If the CSV file extension is associated with a spreadsheet program, IE flags this as potentially dangerous since it will auto-launch that program:
</p>
<p>
TODO: Information Bar image here
</p>
<p>
To prevent this from happening, the GRIN-Global website must be added as a 'trusted site' within Internet Explorer.  <a href='http://surfthenetsafely.com/ieseczone7.htm' target='_blank'>This great article</a> shows a step-by-step method for doing this.  However, you can also <a href='TrustedSiteGenerator.aspx'>download and run this file</a> to do the same thing.  You must have administrative rights to do this.
</p>
<p>
This is a one-time setup process.
</p>
<p>
<b>NOTE: When this file is run, it will alter your Windows Registry and could cause damage.  Use at your own risk.</b>
<h2>You will need to restart Internet Explorer for these settings to take effect.</h2>
</p>
</asp:Content>
