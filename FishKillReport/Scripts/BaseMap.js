
var ModotBaseMapping = (function () {

    var declare, SpatialRef, Extent, TileInfo, string;

    require([
        "dojo/_base/declare", "esri/SpatialReference", "esri/geometry/Extent",
        "esri/layers/TileInfo", "dojo/string"
    ],
    function (
        _declare, _SpatialRef, _Extent, _TileInfo, _string
    ) {
        declare = _declare;
        SpatialRef = _SpatialRef;
        Extent = _Extent;
        TileInfo = _TileInfo;
        string = _string;
    });

    var baseLods = [
        { "level": 0, "resolution": 0.0571070641399267, "scale": 24000000 },
        { "level": 1, "resolution": 0.0285535320699634, "scale": 12000000 },
        { "level": 2, "resolution": 0.0142767660349817, "scale": 6000000 },
        { "level": 3, "resolution": 0.0107075745262363, "scale": 4500000 },
        { "level": 4, "resolution": 0.00713838301749084, "scale": 3000000 },
        { "level": 5, "resolution": 0.00535378726311813, "scale": 2250000 },
        { "level": 6, "resolution": 0.00356919150874542, "scale": 1500000 },
        { "level": 7, "resolution": 0.00178459575437271, "scale": 750000 },
        { "level": 8, "resolution": 0.000951784402332112, "scale": 400000 },
        { "level": 9, "resolution": 0.000475892201166056, "scale": 200000 },
        { "level": 10, "resolution": 0.000237946100583028, "scale": 100000 },
        { "level": 11, "resolution": 0.000118973050291514, "scale": 50000 },
        { "level": 12, "resolution": 5.9486525145757E-05, "scale": 25000 },
        { "level": 13, "resolution": 2.85535320699634E-05, "scale": 12000 },
        { "level": 14, "resolution": 1.42767660350E-05, "scale": 6000 },
        { "level": 15, "resolution": 7.13838301750E-06, "scale": 3000 },
        { "level": 16, "resolution": 3.56919150875E-06, "scale": 1500 },
        { "level": 17, "resolution": 1.78459575138E-06, "scale": 750 }
    ];

    function iDeclareTiledLayer(dtypeName, dlods, dbaseUrl) {
        declare(dtypeName, esri.layers.TiledMapServiceLayer, {
            constructor: function () {
                this.DataTypeName = dtypeName;
                this.spatialReference = new SpatialRef({ wkid: 4326 });
                this.initialExtent = (this.fullExtent =
                    new Extent(-95.9158003589449, 35.155382740494, -88.9813869334421, 41.3614162758715, this.spatialReference)
                );
                this.tileInfo = new TileInfo({
                    rows: 512,
                    cols: 512,
                    dpi: 96,
                    format: "PNG32",
                    compressionQuality: 0,
                    origin: {
                        x: -95.7743221222839,
                        y: 40.6137030694336
                    },
                    spatialReference: {
                        wkid: 4326
                    },
                    lods: dlods
                });
                this.loaded = true;
                this.onLoad(this);
            },
            getTileUrl: function (level, row, col) {
                var thisTile = dbaseUrl +
                            "L" + string.pad(level, 2, '0') + "/" +
                            "R" + string.pad(row.toString(16), 8, '0') + "/" +
                            "C" + string.pad(col.toString(16), 8, '0') + "." +
                            "png";
                return thisTile;
            }
        });
    }

    return {
        DeclareTiledLayer: function (typeName, lods, baseUrl) {
            iDeclareTiledLayer(typeName, lods, baseUrl);
        },
        BaseLevelsOfDetail: function () {
            return baseLods;
        }
    }

}());


var Map;
(function (Map) {
    Map.onClick = function (evt) {
        require(["esri/graphic", "esri/geometry/Point"], function (Graphic, Point) {
            GetMap().graphics.clear();
            var geometry = new Point(evt.mapPoint.x, evt.mapPoint.y, GetMap().spatialReference);
            var graphic = new Graphic(geometry, m_point);
            GetMap().graphics.add(graphic);
            document.getElementById("xcoord").value = evt.mapPoint.x;
            document.getElementById("ycoord").value = evt.mapPoint.y;
            myFormDialog.show();
        });
    };

    Map.Send = function () {
        var x = document.getElementById("xcoord").value;
        var y = document.getElementById("ycoord").value;
        var damge = document.getElementById("damage").value;
        var number = document.getElementById("number").value;
        var pic = m_pic;
        var blob = new Blob([pic], { type: 'image/jpg' });

        //PageMethods.PostRequest(blob);
    
        
        //alert(x + "/n" + y + "/n" + damage + "/n" + number + + "/n" + pic);
        //xhttp = new XMLHttpRequest();
        //var xhr = new XMLHttpRequest();
        //xhr.withCredentials = true;

        //xhr.addEventListener("readystatechange", function () {
        //    if (this.readyState === 4) {
        //        console.log(this.responseText);
        //    }
        //});

        //xhr.open("Post", "http://fishkillapp.azurewebsites.net/api/MakeFishKillPrediction", true);
        //xhr.setRequestHeader("Content-type", "application/");
        
        //xhr.send(blob);
    };

    Map.Upload = function (evt) {
        
        var reader = new FileReader();
        var selectedFile = evt.target.files[0];

        reader.onload = function (event) {
            var imageSrc = event.target.result;
            m_pic = imageSrc;
        };
        reader.readAsDataURL(selectedFile);
    };

})(Map || (Map = {}));