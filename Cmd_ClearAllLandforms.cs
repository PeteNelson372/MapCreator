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
    internal class Cmd_ClearAllLandforms : IMapOperation
    {
        public MapCreatorMap? Map { get; set; } = null;
        public List<MapLandformType2> STORED_LANDFORM_LIST { get; set; } = [];
        public SKPath? STORED_LAND_LAYER_ERASER_PATH { get; set; } = null;
        public SKBitmap? STORED_LANDFORMLAYER_BITMAP { get; set; } = null;
        public SKBitmap? STORED_LANDCOASTLINELAYER_BITMAP { get; set; } = null;
        public SKBitmap? STORED_LANDDRAWINGLAYER_BITMAP { get; set; } = null;

        public void DoOperation()
        {
            if (Map != null)
            {
                LandformType2Methods.LANDFORM_LIST.Clear();
                LandformType2Methods.LAND_LAYER_ERASER_PATH.Reset();

                LandformType2Methods.ResetLandCanvases(Map);
            }
        }

        public void UndoOperation()
        {
            if (Map != null && STORED_LANDFORMLAYER_BITMAP != null && STORED_LANDCOASTLINELAYER_BITMAP != null && STORED_LANDDRAWINGLAYER_BITMAP != null)
            {
                LandformType2Methods.LANDFORM_LIST = new(STORED_LANDFORM_LIST);
                LandformType2Methods.LAND_LAYER_ERASER_PATH = new(STORED_LAND_LAYER_ERASER_PATH);

                MapBuilder.ClearLayerCanvas(Map, MapBuilder.LANDFORMLAYER);
                MapBuilder.ClearLayerBitmap(Map, MapBuilder.LANDFORMLAYER);
                MapBuilder.SetLayerBitMap(Map, MapBuilder.LANDFORMLAYER, STORED_LANDFORMLAYER_BITMAP);

                MapBuilder.ClearLayerCanvas(Map, MapBuilder.LANDCOASTLINELAYER);
                MapBuilder.ClearLayerBitmap(Map, MapBuilder.LANDCOASTLINELAYER);
                MapBuilder.SetLayerBitMap(Map, MapBuilder.LANDCOASTLINELAYER, STORED_LANDCOASTLINELAYER_BITMAP);

                MapBuilder.ClearLayerCanvas(Map, MapBuilder.LANDDRAWINGLAYER);
                MapBuilder.ClearLayerBitmap(Map, MapBuilder.LANDDRAWINGLAYER);
                MapBuilder.SetLayerBitMap(Map, MapBuilder.LANDDRAWINGLAYER, STORED_LANDDRAWINGLAYER_BITMAP);
            }
        }
    }
}
