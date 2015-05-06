<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="pivotviewcontrol.ascx.cs" Inherits="GrinGlobal.Web.PivotControl" %>

<script type='text/javascript'>
    function initPivotView(json) {
        <asp:Literal id="litPivoter" runat="server" />
    }
    
    $(document).ready(function() {
        <asp:Literal id="litData" runat="server" />
    });
</script>
<div id='<%= this.ClientID %>'></div>
<input type='hidden' id='pivotHiddenPKList' name='pivotHiddenPKList' value='' />
<div class='pivotViewwait' ><h2>Loading viewer, please be patient...</h2></div>
