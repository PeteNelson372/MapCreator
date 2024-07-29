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
namespace MapCreator
{
    internal class Cmd_RemoveSymbol(MapCreatorMap map, MapSymbol placedSymbol) : IMapOperation
    {
        private readonly MapCreatorMap Map = map;
        private readonly MapSymbol PlacedSymbol = placedSymbol;

        public void DoOperation()
        {
            for (int i = SymbolMethods.PlacedSymbolList.Count - 1; i >= 0; i--)
            {
                if (SymbolMethods.PlacedSymbolList[i].GetSymbolGuid().ToString() == PlacedSymbol.GetSymbolGuid().ToString())
                {
                    SymbolMethods.PlacedSymbolList.RemoveAt(i);
                }
            }

            MapLayer symbolLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.SYMBOLLAYER);

            for (int i = symbolLayer.MapLayerComponents.Count - 1; i >= 0; i--)
            {
                if (symbolLayer.MapLayerComponents[i] is MapSymbol ms && ms.GetSymbolGuid().ToString() == PlacedSymbol.GetSymbolGuid().ToString())
                {
                    symbolLayer.MapLayerComponents.RemoveAt(i);
                }
            }
        }

        public void UndoOperation()
        {
            PlacedSymbol.SetIsSelected(false);

            SymbolMethods.PlacedSymbolList.Add(PlacedSymbol);

            MapLayer symbolLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.SYMBOLLAYER);

            symbolLayer.MapLayerComponents.Add(PlacedSymbol);
        }
    }
}
