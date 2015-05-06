<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="mapscontrol.ascx.cs" Inherits="GrinGlobal.Web.MapsControl" %>
<%@ Import Namespace="GrinGlobal.Core" %>
<%= "<script src='http://maps.google.com/maps?file=api&v=2&sensor=false&key="%><%= Toolkit.GetSetting("GoogleMapsAPIKey", "") %><%= "' type='text/javascript'></script>" %>
<asp:ScriptManager ID="ScriptManager1" runat="server">
</asp:ScriptManager>
<asp:HiddenField ID="hfMaps" runat="server" />

<%--<div id="map_canvas" style="width: 650px; height: 450px"></div>
--%>
<div id="map_canvas" style="width: 850px; height: 650px"></div>

<script type='text/javascript'>

    $(document).ready(function() {
        var map = new GMap2(document.getElementById("map_canvas"));
        map.addControl(new GLargeMapControl3D());
        map.addControl(new GMenuMapTypeControl());
        map.addControl(new GOverviewMapControl());
        map.enableDoubleClickZoom();

        var mapsData = document.getElementById("ctl00_cphBody_mc1_hfMaps").value;

        map.setMapType(G_SATELLITE_MAP);

        var json = eval("(" + mapsData + ")");

        var arrowicon = new GIcon(G_DEFAULT_ICON);
        arrowicon.image = "images/arrow.png";
        arrowicon.shadow = "image/iconShadow.png";
        arrowicon.iconSize = new GSize(36, 30);
        arrowicon.shadowSize = new GSize(36, 30);

        var yellowicon = defineIcon("images/mm_20_yellow.png");
        var orangeicon = defineIcon("images/mm_20_orange.png");
        var redicon = defineIcon("images/mm_20_red.png");
        var purpleicon = defineIcon("images/mm_20_purple.png");
        var blueicon = defineIcon("images/mm_20_blue.png");

        var lt, lng, cnt;
        var popup;
        var setcenter = 0;

        for (var i = 0; i < json.table[0].row.length; i++) {

            lt = json.table[0].row[i].col[1].data;
            lng = json.table[0].row[i].col[2].data;

            cnt = json.table[0].row[i].col[4].data;

            point = new GLatLng(lt, lng);

            if (cnt == 0) {
                map.setCenter(point, 4);
                setcenter = 1;
                popup = createPopup(json.table[0].row[i].col[0].data, lt, lng, json.table[0].row[i].col[3].data, false, json.table[0].row[i].col[5].data);
                map.addOverlay(createMarker(point, arrowicon, popup));
            }
            else if (cnt == 1) {
                if (setcenter == 0) {
                    map.setCenter(point, 4);
                    setcenter = 1;
                }
                popup = createPopup(json.table[0].row[i].col[0].data, lt, lng, json.table[0].row[i].col[3].data, false, json.table[0].row[i].col[5].data);
                map.addOverlay(createMarker(point, yellowicon, popup));
            }
            else if ((cnt >= 2) && (cnt <= 5)) {
                popup = createPopup(cnt, lt, lng, json.table[0].row[i].col[3].data, true, 0);
                map.addOverlay(createMarker(point, orangeicon, popup));
            }
            else if ((cnt >= 6) && (cnt <= 10)) {
                popup = createPopup(cnt, lt, lng, json.table[0].row[i].col[3].data, true, 0);
                map.addOverlay(createMarker(point, redicon, popup));
            }
            else if ((cnt >= 11) && (cnt <= 100)) {
                popup = createPopup(cnt, lt, lng, json.table[0].row[i].col[3].data, true, 0);
                map.addOverlay(createMarker(point, purpleicon, popup));
            }
            else {
                popup = createPopup(cnt, lt, lng, json.table[0].row[i].col[3].data, true, 0);
                map.addOverlay(createMarker(point, blueicon, popup));
            }
        }

        function defineIcon(imagePath) {
            var icon = new GIcon(G_DEFAULT_ICON);
            icon.image = imagePath;
            icon.shadow = "image/iconShadow.png";
            icon.iconSize = new GSize(12, 20);
            icon.shadowSize = new GSize(22, 20);
            return icon;
        }

        function createPopup(pi, lt, lng, localality, total, id) {
            var msg;

            if (total) {
                msg = '<b>' + pi + " accessions </b>" + ' (' + lt + (lt > 0 ? 'N' : 'S') + ', ' + lng + (lng > 0 ? 'E' : 'W') + ')' + '<br />' + localality;
            }
            else
                msg = '<a href="accessiondetail.aspx?id=' + id + '"><b>' + pi + '</b></a>' + ' (' + lt + (lt > 0 ? 'N' : 'S') + ', ' + lng + (lng > 0 ? 'E' : 'W') + ')' + '<br />' + localality;
            return msg;
        }

        function createMarker(point, icon, desc) {
            var marker = new GMarker(point, icon);
            GEvent.addListener(marker, "click", function() {
                map.openInfoWindowHtml(point, desc);
            });
            return marker;
        }
    });
    document.body.onunload = GUnload;
</script>
