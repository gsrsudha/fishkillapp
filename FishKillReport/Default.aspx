<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="FishKillReport.Default" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home Page | FishKillReport</title>
    <meta charset="utf-8" />
    <link rel="stylesheet" href="http://traveler.modot.org/arcgis_js_api/library/3.23/3.23/dijit/themes/claro/claro.css" />
    <link rel="stylesheet" href="http://traveler.modot.org/arcgis_js_api/library/3.23/3.23/esri/css/esri.css" />
    <link rel="stylesheet" href="Style/style.css" />
    <script type="text/javascript" src="http://traveler.modot.org/arcgis_js_api/library/3.23/3.23/dojo/dojo.js"></script>
    <script type="text/javascript" src="Scripts/BaseMap.js"></script>
    <script>
        var m_map;
        var m_fullext;
        var m_baseLayers = [];
        var m_graphicsLayers = [];
        var m_point;
        var m_pic;
        function GetMap() {
            return m_map;
        }
        function ZoomToFullExtent() {
            m_map.setExtent(m_fullext, true);
        }
        function setMapBackgroundColor(clr) {
            require(["dojo/dom-style"], function (domStyle) {
                domStyle.set("mapDiv", "background-color", clr);
            });
        }
        function setBaseOpacity(op) {
            m_baseLayers[0].setOpacity(op);
            m_baseLayers[1].setOpacity(op);
        }
        function configMap() {
            require(["esri/map", "esri/geometry/Extent", "esri/layers/ArcGISDynamicMapServiceLayer", "esri/layers/GraphicsLayer", "esri/symbols/SimpleMarkerSymbol", "esri/Color", "dojo/domReady!"], function (Map, Extent, arcGISDynamicMapServiceLayer, GraphicsLayer, SimpleMarkerSymbol, Color) {

                m_fullext = new Extent({
                    xmin: -95.838, ymin: 35.962, xmax: -89.049, ymax: 40.702,
                    spatialReference: { wkid: 4326 }
                });

                m_map = new Map("mapDiv", {
                    extent: m_fullext
                });

                // Construct Basemap
                ModotBaseMapping.DeclareTiledLayer("gov.mo.modot.ModotBaseMapTiledLayer", ModotBaseMapping.BaseLevelsOfDetail(), "https://mapping.modot.org/mapcache/Base-Map_Combined_Cache/Layers/_alllayers/");
                var cachedLyr = new gov.mo.modot.ModotBaseMapTiledLayer();
                cachedLyr.setMaxScale(12001);
                m_map.addLayer(cachedLyr);

                // Construct Dynamic Basemap
                var baseMapDynamic = new arcGISDynamicMapServiceLayer("https://mapping.modot.org/arcgis/rest/services/Base-Map/Combined_Dynamic/MapServer", { id: "baseMapDynamic" });
                baseMapDynamic.setMinScale(12000);
                m_map.addLayer(baseMapDynamic);

                m_baseLayers = [cachedLyr, baseMapDynamic];

                var gl = new GraphicsLayer();
                m_map.addLayer(gl);
                m_graphicsLayers = [gl];

                m_point = new SimpleMarkerSymbol().setSize(10).setColor(new Color([255, 0, 0]));

                if (typeof onBasicMapConfigure !== 'undefined') {
                    onBasicMapConfigure(m_map);
                }

            });
        }
        require(["dojo/parser", "dojo/ready"], function (parser, ready) {
            ready(function () {
                parser.parse();
                configMap();
            });
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
        <div>
            <asp:FileUpload runat="server" ID="fileCtrl" ToolTip="Upload image" />
            <asp:Button runat="server" ID="btnSubmit" Text="Submit" OnClick="btnSubmit_Click"/>
        </div>
        

        <div id="mapDiv" style="position: absolute; width: 98%; height: 98%; overflow: hidden; background-color: #899FAA;">
            <div class="zoomfull" onclick="Map.ZoomFull()">
            </div>
            <div data-dojo-type="dijit/Dialog" data-dojo-id="myFormDialog" id="myFormDialog" title="Fish Kill Info" style="background-color: steelblue;"
                execute="Map.Send();">

                <div class="dijitDialogPaneContentArea" style="background: white; border: solid 1px blue">
                    <table>
                        <tr>
                            <td>
                                <label for="xcoord">X-coord: </label>
                            </td>
                            <td>
                                <input data-dojo-type="dijit/form/TextBox" type="text" name="xcoord" id="xcoord" data-dojo-props="'readonly': 'readonly'" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label for="ycoord">Y-coord: </label>
                            </td>
                            <td>
                                <input data-dojo-type="dijit/form/TextBox" type="text" name="ycoord" id="ycoord" data-dojo-props="'readonly': 'readonly'" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label for="damage">Damage Type: </label>
                            </td>
                            <td>
                                <input data-dojo-type="dijit/form/TextBox" data-dojo-id="damage" type="text" name="damage" id="damage" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label for="number">Number killed: </label>
                            </td>
                            <td>
                                <input data-dojo-type="dijit/form/NumberSpinner" value="0" data-dojo-id="number" name="number" id="number" /></td>
                        </tr>
                        <tr>
                            <td>
                                <label for="pic">Add picture: </label>
                            </td>
                            <td>
                                <input name="pic" type="file" id="pic" accept="image/*" /></td>
                        </tr>
                    </table>
                </div>

                <div class="dijitDialogPaneActionBar">
                    <button data-dojo-type="dijit/form/Button" style="background: white;" type="submit" onclick="return myFormDialog.isValid();">
                        OK
                    </button>
                    <button data-dojo-type="dijit/form/Button" style="background: white;" type="button" onclick="myFormDialog.hide()">
                        Cancel
                    </button>
                </div>
            </div>
        </div>
    </form>    
    <script>
        function onBasicMapConfigure(map) {
            try {
                require(["esri/layers/ArcGISDynamicMapServiceLayer", "dojox/widget/Standby", "dijit/registry", "dijit/form/Form", "dijit/form/Button", "dijit/form/ValidationTextBox", "dijit/form/DateTextBox"], function (arcGISDynamicMapServiceLayer, _standby, reg) {

                    var map = GetMap();

                    document.getElementById("pic").onchange = function (e) { // on file select                        
                        var fr = new FileReader; // reads files
                        fr.onloadend = function () {             // when done reading
                            m_pic = fr.result;
                        };
                        fr.readAsDataURL(e.target.files[0]); // start reading
                    };

                    map.on("click", function (evt) {
                        Map.onClick(evt);
                    });
                    /*maintAgreeLayer = new arcGISDynamicMapServiceLayer("@BIMaps.Util.Constants.MaintAgreementServiceUrl", { id: "MaintAgreeLayer" });
                    maintAgreeLayer.setVisibleLayers([]);
                    maintAgreeLayer.on("update-end", function () {
                    idBusy.hide();
                    });
                    map.addLayer(maintAgreeLayer);
                    */
                });
            } catch (e) {
                console.log("An error occurred in the onBasicMapConfigure function.  " + e);
            }
        }
    </script>

</body>
</html>
