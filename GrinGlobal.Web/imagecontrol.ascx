<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="imagecontrol.ascx.cs" Inherits="GrinGlobal.Web.ImageControl" %>
<!--
<script type='text/javascript'>
    $(document).ready(function() {
        $('a.thumbnail').onclick(function() {
            $('#previewImage').attr('src', $('img', this).attr('src'));
        });
    });
</script>

<div style='border:1px solid black;margin:10px 10px 10px 10px;padding:10px 10px 10px 10px'>
    <img id='#previewImage' />
    <asp:DataList runat="server" ID="dlThumbnails" RepeatColumns="6" RepeatDirection="Horizontal" RepeatLayout="Flow">
        <ItemTemplate>
            <a class='thumbnail'><img src=<%# Eval("thumbnail_virtual_path") %> alt='<%# Eval("title") %>' height='30' width='30' /></a>
        </ItemTemplate>
    </asp:DataList>
</div>
-->
<script type='text/javascript'>
    $(document).ready(function() {
        $('a.thumbnail').click(function(e) {
            e.preventDefault();
            var smallSrc = $('img.smallImage', this).attr('src');
            var smallAlt = $('img.smallImage', this).attr('alt');
            var smallTit = $('img.smallImage', this).attr('title');
//            $('img.previewSmallImage').attr('src', smallSrc).attr('alt', smallAlt);
            
            var fullSrc = $('img.fullImage', this).attr('src');
            var fullAlt = $('img.fullImage', this).attr('alt');
            var fullTit = $('img.fullImage', this).attr('title');
            
            $('img.previewSmallImage').attr('src', fullSrc).attr('alt', smallAlt).attr('title', smallTit);
            $('img.previewFullImage').attr('src', fullSrc).attr('alt', fullAlt).attr('title', fullTit);
            
        });

        $('a.thumbnail').dblclick(function(e) {
            e.preventDefault();
//            var url = $('img.fullImage', this).attr('src');
//            window.open("imagedisplay.aspx?imgPath=" + url, "", "scrollbars=no, width=720, height=510, resizable=yes, menubar=yes");
            var title = $('img.fullImage', this).attr('title');
            window.open("imagedisplay.aspx?lnk=" + title, "", "scrollbars=no, width=720, height=600, resizable=yes, menubar=yes");

        });

        $('a.preview').click(function(e) {
            e.preventDefault();
            var title = $('img.previewFullImage').attr('title');
            window.open("imagedisplay.aspx?lnk=" + title, "", "scrollbars=no, width=720, height=600, resizable=yes, menubar=yes");
        });
        $('a.thumbnail').eq(0).click();
    });
</script>

<div style='width: 200px;'>
    <a class='preview'>
        <img class='previewSmallImage' height='140' src="" alt=""/>
        <img class='previewFullImage' style="display:none" src="" alt="" />
    </a>
    <div style='height: 10px;'></div>
    <asp:Panel ID="plThumbnails" runat="server" HorizontalAlign="Left">
    <asp:DataList runat="server" ID="dlTN" RepeatDirection="Horizontal" RepeatLayout="Flow" CellPadding="5" CellSpacing="5">
        <ItemTemplate>
            <a class="thumbnail">
                <img src='<%# Resolve(Eval("thumbnail_virtual_path")) %>' alt='<%# Eval("description") %>' title='<%# Eval("title") %>' height='33' class='smallImage' />
                <img src='<%# Resolve(Eval("virtual_path")) %>' alt='<%# Eval("description") %>' title='<%# Eval("title") %>' height='1' width='1' style='display:none' class='fullImage' />
            </a>
        </ItemTemplate>
    </asp:DataList>
    </asp:Panel>
</div>
