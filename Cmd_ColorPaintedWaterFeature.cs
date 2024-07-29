/**************************************************************************************************************************
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

namespace MapCreator
{
    internal class Cmd_ColorPaintedWaterFeature : IMapOperation
    {
        private readonly MapCreatorMap Map;

        private readonly Guid WaterColorPathGuid = Guid.NewGuid();

        private readonly List<Tuple<SKPoint, int, SKShader>> StoredPoints = [];

        private SKPath ColorPath = new();

        public Cmd_ColorPaintedWaterFeature(MapCreatorMap map)
        {
            Map = map;
        }

        public void AddCircle(float x, float y, int brushRadius, SKShader shader)
        {
            StoredPoints.Add(new Tuple<SKPoint, int, SKShader>(new SKPoint(x, y), brushRadius, shader));
            ColorPath.AddCircle(x, y, brushRadius);

            WaterFeatureMethods.WATER_COLOR_PAINT.Shader = shader;

            SKCanvas? waterDrawingCanvas = MapBuilder.GetLayerCanvas(Map, MapBuilder.WATERDRAWINGLAYER);
            SKRectI wrecti = new(0, 0, Map.MapWidth, Map.MapHeight);

            if (waterDrawingCanvas != null)
            {
                using SKRegion waterDrawingRegion = new();
                waterDrawingRegion.SetRect(wrecti);

                using SKRegion waterPathRegion = new(waterDrawingRegion);

                if (ColorPath.PointCount > 0)
                {
                    // if the outer path of the water feature intersects the painted path,
                    // clip painting to the outer path of the water feature
                    for (int i = 0; i < WaterFeatureMethods.PAINTED_WATERFEATURE_LIST.Count; i++)
                    {
                        SKPath waterOutlinePath = WaterFeatureMethods.PAINTED_WATERFEATURE_LIST[i].GetWaterFeaturePath();

                        waterPathRegion.SetPath(waterOutlinePath);

                        waterDrawingCanvas.Save();
                        waterDrawingCanvas.ClipRegion(waterPathRegion);
                        waterDrawingCanvas.DrawPath(ColorPath, WaterFeatureMethods.WATER_COLOR_PAINT);
                        waterDrawingCanvas.Restore();

                        WaterFeatureMethods.PAINTED_WATERFEATURE_LIST[i].AddWaterFeatureColorPath(new(ColorPath));
                    }

                    for (int i = 0; i < WaterFeatureMethods.MAP_RIVER_LIST.Count; i++)
                    {
                        SKPath? riverOutlinePath = WaterFeatureMethods.MAP_RIVER_LIST[i].RiverBoundaryPath;

                        if (riverOutlinePath != null && riverOutlinePath.PointCount > 0)
                        {
                            waterPathRegion.SetPath(riverOutlinePath);

                            waterDrawingCanvas.Save();
                            waterDrawingCanvas.ClipRegion(waterPathRegion);
                            waterDrawingCanvas.DrawPath(ColorPath, WaterFeatureMethods.WATER_COLOR_PAINT);
                            waterDrawingCanvas.Restore();

                            WaterFeatureMethods.MAP_RIVER_LIST[i].RiverColorPaths.Add(new(ColorPath));

                        }
                    }
                }
            }


        }

        public void DoOperation()
        {
            MapBuilder.GetLayerCanvas(Map, MapBuilder.WATERDRAWINGLAYER).Clear();
            WaterFeatureMethods.WATER_LAYER_COLOR_PATHS.Add(new Tuple<Guid, List<Tuple<SKPoint, int, SKShader>>>(WaterColorPathGuid, StoredPoints));
            WaterFeatureMethods.ColorWaterFeaturePaths(Map);

            ColorPath.Reset();
        }

        public void UndoOperation()
        {
            for (int i = WaterFeatureMethods.WATER_LAYER_COLOR_PATHS.Count - 1; i >= 0; i--)
            {
                if (WaterFeatureMethods.WATER_LAYER_COLOR_PATHS[i].Item1.ToString() == WaterColorPathGuid.ToString())
                {
                    WaterFeatureMethods.WATER_LAYER_COLOR_PATHS.RemoveAt(i);
                }
            }

            MapBuilder.GetLayerCanvas(Map, MapBuilder.WATERDRAWINGLAYER)?.Clear();
            WaterFeatureMethods.ColorWaterFeaturePaths(Map);
        }
    }
}
