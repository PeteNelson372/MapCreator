﻿/**************************************************************************************************************************
* Copyright 2024, Peter R. Nelson
*
* This file is part of the MapCreator application. The MapCreator application is intended
* for creating fantasy maps for gaming and world building.
*
* MapCreator is free software: you can redistribute it and/or modify it under the terms
* of the GNU General Public License as published by the Free Software Foundation,
* either version 3 of the License, or (at your option) any later version.
*
* This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
* without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
* See the GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License along with this program.
* The text of the GNU General Public License (GPL) is found in the LICENSE file.
* If the LICENSE file is not present or the text of the GNU GPL is not present in the LICENSE file,
* see https://www.gnu.org/licenses/.
*
* For questions about the MapCreator application or about licensing, please email
* contact@brookmonte.com
*
***************************************************************************************************************************/
using SkiaSharp;
using System.Xml;
using System.Xml.Serialization;

namespace MapCreator
{
    [XmlRoot("map", Namespace = "MapCreator", IsNullable = false)]
    [XmlInclude(typeof(MapComponent))]
    [XmlInclude(typeof(MapBitmap))]
    [XmlInclude(typeof(MapLayer))]
    public class MapCreatorMap
    {
        [XmlIgnore]
        private ushort mapWidth = 0;

        [XmlIgnore]
        private ushort mapHeight = 0;

        [XmlIgnore]
        private float mapAreaWidth = 0;

        [XmlIgnore]
        private float mapAreaHeight = 0;

        [XmlIgnore]
        private bool isSaved = false;

        [XmlAttribute]
        public string MapName { get; set; } = "";

        [XmlAttribute]
        public string MapPath { get; set; } = "";

        // MapHeight and MapWidth are the size of the map in pixels (e.g. 1200 x 800)
        [XmlAttribute]
        public ushort MapWidth { get => mapWidth; set => mapWidth = value; }

        [XmlAttribute]
        public ushort MapHeight { get => mapHeight; set => mapHeight = value; }

        // MapAreaWidth and MapAreaHeight are the size of the map in MapUnits (e.g. 1000 miles x 500 miles)
        [XmlAttribute]
        public float MapAreaWidth { get => mapAreaWidth; set => mapAreaWidth = value; }

        [XmlAttribute]
        public float MapAreaHeight { get => mapAreaHeight; set => mapAreaHeight = value; }

        [XmlAttribute]
        public string MapAreaUnits { get; set; } = string.Empty;

        [XmlArray("MapLayers")]
        public List<MapLayer> MapLayers = new(MapBuilder.MAP_LAYER_COUNT);

        [XmlIgnore]
        public bool IsSaved { get => isSaved; set => isSaved = value; }

        // MapPixelWidth and MapPixelHeight are the size of one pixel in MapAreaUnits
        [XmlIgnore]
        public float MapPixelWidth { get; set; } = 0F;

        [XmlIgnore]
        public float MapPixelHeight { get; set; } = 0F;

        [XmlIgnore]
        public List<int> RenderOnlyLayers { get; set; } = [];

        public void Render(SKCanvas canvas)
        {
            foreach (MapLayer mapLayer in MapLayers)
            {
                if (RenderOnlyLayers.Count == 0 || RenderOnlyLayers.Contains(mapLayer.MapLayerOrder))
                {
                    mapLayer.Render(canvas);
                }
            }
        }
    }
}
