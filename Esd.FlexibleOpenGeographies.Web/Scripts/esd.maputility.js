var MapUtility = {
    //Please keep in alphabetical order to make things easy to find
    addAreaById: function (id) {
        var geoJsonUrl = '/areaGeography?id=' + id;
        var vectorSource = new ol.source.GeoJSON({ url: geoJsonUrl, projection: 'EPSG:3857', extractStyles: false });
        var vector = new ol.layer.Vector({ source: vectorSource, style: MapUtility.selectedAreaStyle });
        MapUtility.map.addLayer(vector);
        MapUtility.onAreaAdded(id);
    },
    areaLayer: function (area, isOverlay) {
        var defaultStyle;
        if (isOverlay === true) {
            defaultStyle = new ol.style.Style({
                fill: new ol.style.Fill({ color: 'rgba(0,0,0,0.1)' }),
                stroke: new ol.style.Stroke({ color: 'red', width: 2 })
            });
        } else {
            var colour;
            $.get('/colour?id=' + area, function (data) {
                colour = data == null || data == '' ? 'green' : '#' + data;
            });
            defaultStyle = new ol.style.Style({
                fill: new ol.style.Fill({ color: colour }),
                stroke: new ol.style.Stroke({ color: 'black', width: 1 })
            });
        }
        var geoJsonUrl = '/areaGeography?id=' + area;
        var vectorSource = new ol.source.GeoJSON({ url: geoJsonUrl, projection: 'EPSG:3857', extractStyles: false });
        var vector = new ol.layer.Vector({ source: vectorSource, style: defaultStyle });
        if (isOverlay === false) {
            vector.setOpacity(.3);
            vector.addEventListener("change", function() {
                if (!ol.extent.isEmpty(vectorSource.getExtent())) {
                    MapUtility.map.getView().fitExtent(vectorSource.getExtent(), MapUtility.map.getSize());
                } else if (MapUtility.warningDiv != null) {
                    MapUtility.warningDiv.html("No map could be generated for this area");
                    MapUtility.warningDiv.removeClass('hide');
                }
            });
        }
        return vector;
    },
    areaTypeBoundedLayer: function (areaType, boundingType, boundingArea) {
        var defaultStyle = new ol.style.Style({
            fill: new ol.style.Fill({ color: '#aaaaaa' }),
            stroke: new ol.style.Stroke({ color: '#000000', width: 1 })
        });
        var geoJsonUrl = '/geoJsonForTypeBounded?boundingType=' + boundingType + '&boundingCode=' + boundingArea + '&typeCode=' + areaType;
        var vectorSource = new ol.source.GeoJSON({ url: geoJsonUrl, projection: 'EPSG:3857', extractStyles: false });
        var vector = new ol.layer.Vector({ source: vectorSource, style: defaultStyle });
        vector.setOpacity(.3);
        return vector;
    },
    areaTypeLayer: function (areaType) {
        var params = {
            LAYERS: 'areas_for_type',
            FORMAT: 'image/png',
            viewparams: 'typecode:' + areaType
        };
        var wmsImageSource = new ol.source.ImageWMS({ url: MapUtility.geoServerRoot + 'wms', params: params });
        return new ol.layer.Image({ opacity: 0.3, source: wmsImageSource });
    },
    canSelect: false,
    displayInfo: function (e) {
        var pixel = MapUtility.map.getEventPixel(e.originalEvent);
        var features = MapUtility.map.forEachFeatureAtPixel(pixel, function (feature) {
            return feature;
        });
        if (features) {
            MapUtility.showInfo(features.get('name'), e);
        } else {
            MapUtility.hideInfo();
        }
    },
    dragBox: null,
    enableDragBox: function () {
        MapUtility.dragBox = new ol.interaction.DragBox({
            condition: ol.events.condition.shiftKeyOnly,
            style: new ol.style.Style({
                stroke: new ol.style.Stroke({ color: 'black' })
            })
        }
        );
        MapUtility.map.addInteraction(MapUtility.dragBox);
    },
    enableDragBoxForAreaType: function (areaType, boundingType, boundingArea) {
        MapUtility.enableDragBox();
        MapUtility.dragBox.on('boxend', function () {
            var extent = MapUtility.dragBox.getGeometry().getExtent();
            var bl = ol.proj.transform(ol.extent.getBottomLeft(extent), 'EPSG:3857', 'EPSG:4326');
            var tr = ol.proj.transform(ol.extent.getTopRight(extent), 'EPSG:3857', 'EPSG:4326');
            $.getJSON('/areasInBoxForTypeAJAX', {
                'typeCode': areaType,
                'minX': bl[0],
                'minY': bl[1],
                'maxX': tr[0],
                'maxY': tr[1],
                'boundingType': boundingType,
                'boundingArea': boundingArea
            }, function (data) {
                if (data != null && data.length > 0) {
                    if (data.length > 100) {
                        alert('Too many areas selected');
                    } else {
                        for (var i = 0; i < data.length; i++) {
                            var pos = $.inArray(data[i], MapUtility.selectedAreas);
                            if (pos === -1) {
                                MapUtility.selectedAreas.push(data[i]);
                                MapUtility.addAreaById(data[i]);
                            }
                        }
                    }
                }
            });
        });
    },
    geoServerRoot: null,
    hideInfo: function () {
        $('#map-info').hide();
    },
    init: function () {
        $('#map-container').empty();
        MapUtility.map = new ol.Map({
            controls: [
                new ol.control.Attribution(),
                new ol.control.ScaleLine(),
                new ol.control.Zoom(),
                new ol.control.ZoomSlider()
            ],
            target: 'map-container',
            layers: [new ol.layer.Tile({ source: new ol.source.MapQuest({ layer: 'osm' }) })],
            view: new ol.View({ center: [0, 0], zoom: 2 })
        });
    },
    map: null,
    onAreaAdded: function(areaId) {
        var event = $.Event('areaadded');
        event.areaId = areaId;
        $(window).trigger(event);
    },
    onAreaRemoved: function(areaId) {
        var event = $.Event('arearemoved');
        event.areaId = areaId;
        $(window).trigger(event);
    },
    rdiLock: false,
    readAndDisplayInfo: function (typeCode, e, boundingType, boundingArea) {
        if (MapUtility.rdiLock) return;
        MapUtility.rdiLock = true;
        var coordinates;
        try {
            coordinates = ol.proj.transform(MapUtility.map.getEventCoordinate(e), 'EPSG:3857', 'EPSG:4326');
        } catch (ex) {
            //sometimes get exception when removing area with click causes map to refresh and getEventCoordinate returns undefined
        }
        var hasArea = false;
        $.ajaxSetup({ async: false });
        if (coordinates != null && coordinates.length > 0) {
            $.getJSON('/areaAtPointForTypeAJAX', {
                'typeCode': typeCode,
                'x': coordinates[0],
                'y': coordinates[1],
                'boundingType': boundingType,
                'boundingArea': boundingArea
            }, function (data) {
                if (data != null) {
                    hasArea = true;
                    MapUtility.showInfo(data.Label + ' (' + data.Code + ')', e);
                }
            });
        }
        $.ajaxSetup({ async: true });
        if (!hasArea) MapUtility.hideInfo();
        setTimeout(function () { MapUtility.rdiLock = false; }, 200);
    },
    selectArea: function (areaType, e, boundingType, boundingArea) {
        var coordinates = ol.proj.transform(e.coordinate, 'EPSG:3857', 'EPSG:4326');
        if (coordinates == null || coordinates.length === 0) return;
        $.ajaxSetup({ async: false });
        $.getJSON('/areaAtPointForTypeAJAX', {
            'typeCode': areaType,
            'x': coordinates[0],
            'y': coordinates[1],
            'boundingType': boundingType,
            'boundingArea': boundingArea
        }, function (data) {
            if (data != null && data.Id != null) {
                MapUtility.selectAreaById(areaType, data.Id, boundingType, boundingArea);
            }
        });
        $.ajaxSetup({ async: true });
    },
    selectAreaById: function (areaType, areaId, boundingType, boundingArea) {
        var pos = $.inArray(areaId, MapUtility.selectedAreas);
        if (pos > -1) {
            MapUtility.selectedAreas.splice(pos, 1);
            //The next three lines redraw the map then zoom back to (close but not exactly) the same place
            //It would be better if we remove the layer without redrawing the map, but I don't know how
            //I attempted layers with non-contiguous indexes where the index was the area ID but it didn't work
            //A question online currently has no replies:
            //https://gis.stackexchange.com/questions/130059/adding-and-removing-openlayers-3-layers-based-on-a-key
            var size = MapUtility.map.getSize();
            var extent = MapUtility.map.getView().calculateExtent(size);
            MapUtility.showMapForType(areaType, boundingType, boundingArea);
            MapUtility.map.getView().fitExtent(extent, size);
            MapUtility.onAreaRemoved(areaId);
        } else {
            $.getJSON('/areaInBounds', {
                'areaId': areaId,
                'typeCode': areaType,
                'boundingType': boundingType,
                'boundingCode': boundingArea
            }, function (data) {
                if (data != null && data != '' && data != 0) {
                    MapUtility.selectedAreas.push(areaId);
                    MapUtility.addAreaById(areaId);
                }
            });
        }
    },
    selectedAreas: [],
    selectedAreaStyle: new ol.style.Style({
        fill: new ol.style.Fill({ color: 'green' }),
        stroke: new ol.style.Stroke({ color: 'black', width: 1 })
    }),
    showInfo: function (name, e) {
        $('#map-info').html(name);
        $('#map-info').css({ 'top': (e.pageY - $(document).scrollTop()) + 'px', 'left': e.pageX + 10 + 'px' }).show();
    },
    showMap: function (area) {
        $.ajaxSetup({ async: false });
        if (MapUtility.warningDiv != null) {
            MapUtility.warningDiv.addClass('hide');
        }
        MapUtility.init();
        if (area != null && area != 0) {
            MapUtility.map.addLayer(MapUtility.areaLayer(area, false));
            $(MapUtility.map.getViewport()).on('mousemove', function(e) {
                MapUtility.displayInfo(e);
            });
        }
        $.ajaxSetup({ async: true });
    },
    showMapForType: function (areaType, boundingType, boundingAreaCode) {
        if (MapUtility.warningDiv != null) {
            MapUtility.warningDiv.addClass('hide');
        }
        MapUtility.init();
        if (areaType != null && areaType != '') {
            var hasBoundings = boundingType != null && boundingType != '' && boundingAreaCode != null && boundingAreaCode != '';
            $.ajaxSetup({ async: false });
            var boundingAreaId;
            if (hasBoundings) {
                $.getJSON('/areaId', { areaType: boundingType, areaCode: boundingAreaCode }, function(data) {
                    boundingAreaId = data;
                });
            }
            var url = hasBoundings
                ? '/boundingBoxForAreaAJAX?typeCode=' + boundingType + '&areaCode=' + boundingAreaCode
                : '/boundingBoxForTypeAJAX?code=' + areaType;
            $.getJSON(url, function(bbdata) {
                if (bbdata != null) {
                    var box = [bbdata.MinimumX, bbdata.MinimumY, bbdata.MaximumX, bbdata.MaximumY];
                    var areaExtent = ol.extent.applyTransform(box, ol.proj.getTransform('EPSG:4326', 'EPSG:3857'));
                    MapUtility.map.getView().fitExtent(areaExtent, MapUtility.map.getSize());
                } else if (MapUtility.warningDiv != null) {
                    MapUtility.warningDiv.html("No map could be generated for this area type");
                    MapUtility.warningDiv.removeClass('hide');
                    return;
                }
            });
            $.getJSON('/typeCodesByTypeAJAX', { typeCode: areaType }, function (data) {
                $.each(data, function(index, layerType) {
                    var layer = MapUtility.areaTypeLayer(layerType);
                    MapUtility.map.addLayer(layer);
                });
            });
            if (hasBoundings) {
                MapUtility.map.addLayer(MapUtility.areaLayer(boundingAreaId, true));
            }
            $(MapUtility.map.getViewport()).on('mousemove', function (e) {
                MapUtility.readAndDisplayInfo(areaType, e, boundingType, boundingAreaCode);
            });
            if (MapUtility.canSelect) {
                $('#selection-info-control').show();
                $(MapUtility.map.on('singleclick', function (e) {
                    MapUtility.selectArea(areaType, e, boundingType, boundingAreaCode);
                }));
                MapUtility.enableDragBoxForAreaType(areaType, boundingType, boundingAreaCode);
            }
            if (MapUtility.selectedAreas.length > 0) {
                for (var i = 0; i < MapUtility.selectedAreas.length; i++) {
                    MapUtility.addAreaById(MapUtility.selectedAreas[i]);
                }
            }
            $.ajaxSetup({ async: true });
        }
    },
    warningDiv: null,
    zoomToBounds: function (bbdata) {
        var box = [bbdata.MinimumX, bbdata.MinimumY, bbdata.MaximumX, bbdata.MaximumY];
        var areaExtent = ol.extent.applyTransform(box, ol.proj.getTransform('EPSG:4326', 'EPSG:3857'));
        MapUtility.map.getView().fitExtent(areaExtent, MapUtility.map.getSize());
    },
}