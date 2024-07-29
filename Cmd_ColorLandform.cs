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
    internal class Cmd_ColorLandform : IMapOperation
    {
        private readonly MapCreatorMap Map;

        private readonly Guid LandColorPathGuid = Guid.NewGuid();

        private readonly List<Tuple<SKPoint, int, SKShader>> StoredPoints = [];

        private SKPath ColorPath = new();

        public Cmd_ColorLandform(MapCreatorMap map)
        {
            Map = map;
        }

        public void AddCircle(float x, float y, int brushRadius, SKShader shader)
        {
            StoredPoints.Add(new Tuple<SKPoint, int, SKShader>(new SKPoint(x, y), brushRadius, shader));
            ColorPath.AddCircle(x, y, brushRadius);

            LandformType2Methods.LAND_COLOR_PAINT.Shader = shader;

            SKCanvas? landDrawingCanvas = MapBuilder.GetLayerCanvas(Map, MapBuilder.LANDDRAWINGLAYER);
            SKRectI recti = new(0, 0, Map.MapWidth, Map.MapHeight);

            if (landDrawingCanvas != null)
            {
                using SKRegion landDrawingRegion = new();
                landDrawingRegion.SetRect(recti);

                using SKRegion landPathRegion = new(landDrawingRegion);

                if (ColorPath.PointCount > 0)
                {
                    // clip painting to the outer path of the landform
                    // LandformPath is the outer path of the landform

                    for (int i = 0; i < LandformType2Methods.LANDFORM_LIST.Count; i++)
                    {
                        SKPath landformOutlinePath = LandformType2Methods.LANDFORM_LIST[i].LandformPath;

                        landPathRegion.SetPath(landformOutlinePath);

                        landDrawingCanvas.Save();
                        landDrawingCanvas.ClipRegion(landPathRegion);
                        landDrawingCanvas.DrawPath(ColorPath, LandformType2Methods.LAND_COLOR_PAINT);
                        landDrawingCanvas.Restore();
                    }
                }
            }
        }

        public void DoOperation()
        {
            MapBuilder.GetLayerCanvas(Map, MapBuilder.LANDDRAWINGLAYER)?.Clear();
            LandformType2Methods.LAND_LAYER_COLOR_PATHS.Add(new Tuple<Guid, List<Tuple<SKPoint, int, SKShader>>>(LandColorPathGuid, StoredPoints));
            LandformType2Methods.ColorLandformPaths(Map);

            ColorPath.Reset();
        }

        public void UndoOperation()
        {
            for (int i = LandformType2Methods.LAND_LAYER_COLOR_PATHS.Count - 1; i >= 0; i--)
            {
                if (LandformType2Methods.LAND_LAYER_COLOR_PATHS[i].Item1.ToString() == LandColorPathGuid.ToString())
                {
                    LandformType2Methods.LAND_LAYER_COLOR_PATHS.RemoveAt(i);
                }
            }

            MapBuilder.GetLayerCanvas(Map, MapBuilder.LANDDRAWINGLAYER)?.Clear();
            LandformType2Methods.ColorLandformPaths(Map);
        }
    }
}
