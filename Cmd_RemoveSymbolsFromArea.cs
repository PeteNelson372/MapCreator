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
    internal class Cmd_RemoveSymbolsFromArea(MapCreatorMap map, float eraserRadius, SKPoint eraserPoint) : IMapOperation
    {
        private readonly MapCreatorMap Map = map;
        private readonly float EraserCircleRadius = eraserRadius;
        private readonly SKPoint CenterPoint = eraserPoint;

        private List<MapSymbol> RemovedSymbolList { get; set; } = [];

        public void DoOperation()
        {
            for (int i = SymbolMethods.PlacedSymbolList.Count - 1; i >= 0; i--)
            {
                SKPoint symbolPoint = new(SymbolMethods.PlacedSymbolList[i].X, SymbolMethods.PlacedSymbolList[i].Y);

                if (MapDrawingMethods.PointInCircle(EraserCircleRadius, CenterPoint, symbolPoint))
                {
                    MapSymbol s = SymbolMethods.PlacedSymbolList[i];
                    SymbolMethods.PlacedSymbolList.Remove(s);

                    MapLayer symbolLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.SYMBOLLAYER);
                    symbolLayer.MapLayerComponents.Remove(s);

                    RemovedSymbolList.Add(s);
                }
            }
        }

        public void UndoOperation()
        {
            foreach (MapSymbol symbol in RemovedSymbolList)
            {
                SymbolMethods.PlacedSymbolList.Add(symbol);

                MapLayer symbolLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.SYMBOLLAYER);
                symbolLayer.MapLayerComponents.Add(symbol);
            }

            RemovedSymbolList.Clear();
        }
    }
}
