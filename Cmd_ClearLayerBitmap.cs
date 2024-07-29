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

namespace MapCreator
{
    internal class Cmd_ClearLayerBitmap : IMapOperation
    {
        public MapCreatorMap? Map { get; set; } = null;
        public int LayerIndex { get; set; } = -1;
        SKBitmap? LayerBitmap { get; set; } = null;

        public Cmd_ClearLayerBitmap(MapCreatorMap map, int layerIndex, SKBitmap bitmap)
        {
            Map = map;
            LayerIndex = layerIndex;
            LayerBitmap = bitmap;
        }

        public void DoOperation()
        {
            if (Map != null && LayerIndex >= 0 && LayerIndex < MapBuilder.MAP_LAYER_COUNT)
            {
                MapBuilder.ClearLayerBitmap(Map, LayerIndex);
            }
        }

        public void UndoOperation()
        {
            if (Map != null && LayerIndex >= 0 && LayerIndex < MapBuilder.MAP_LAYER_COUNT && LayerBitmap != null)
            {
                MapBuilder.SetLayerBitMap(Map, LayerIndex, LayerBitmap);
            }
        }
    }
}
