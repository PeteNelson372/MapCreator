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
    internal class OceanPaintMethods
    {
        public static List<Tuple<Guid, List<Tuple<SKPoint, int, SKShader>>>> OCEAN_COLOR_PATHS { get; set; } = [];

        public static SKPath OCEAN_LAYER_ERASER_PATH { get; set; } = new();

        public static int OCEAN_BRUSH_SIZE { get; set; } = 20;
        public static int OCEAN_ERASER_SIZE { get; set; } = 20;

        public static SKPaint OCEAN_ERASER_PAINT { get; set; } = new();
        public static SKPaint OCEAN_PAINT { get; set; } = new();

        internal static void ColorOceanPaths(MapCreatorMap map)
        {
            MapBuilder.GetLayerCanvas(map, MapBuilder.OCEANDRAWINGLAYER)?.Clear();

            SKPath ColorPath = new();
            foreach(Tuple<Guid, List<Tuple<SKPoint, int, SKShader>>> colorPoints in OCEAN_COLOR_PATHS)
            {
                ColorPath.Reset();

                foreach (Tuple<SKPoint, int, SKShader> sp in colorPoints.Item2)
                {
                    ColorPath.AddCircle(sp.Item1.X, sp.Item1.Y, sp.Item2);

                    OCEAN_PAINT.Shader = sp.Item3;

                    MapBuilder.GetLayerCanvas(map, MapBuilder.OCEANDRAWINGLAYER)?.DrawPath(ColorPath, OCEAN_PAINT);
                }
            }
        }

        internal static void EraseOceanPath(MapCreatorMap map)
        {
            MapBuilder.GetLayerCanvas(map, MapBuilder.OCEANDRAWINGLAYER)?.DrawPath(OCEAN_LAYER_ERASER_PATH, OCEAN_ERASER_PAINT);
        }

        internal static void ConstructOceanPaintObjects()
        {
            OCEAN_ERASER_PAINT.Color = SKColor.Empty;
            OCEAN_ERASER_PAINT.Style = SKPaintStyle.Fill;
            OCEAN_ERASER_PAINT.BlendMode = SKBlendMode.Src;

            OCEAN_PAINT.Style = SKPaintStyle.Fill;
            OCEAN_PAINT.IsAntialias = true;
            OCEAN_PAINT.BlendMode = SKBlendMode.SrcOver;
        }
    }
}
