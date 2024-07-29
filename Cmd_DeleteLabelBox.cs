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
    internal class Cmd_DeleteLabelBox(MapCreatorMap map, PlacedMapBox selectedBox) : IMapOperation
    {
        private readonly MapCreatorMap Map = map;
        private PlacedMapBox SelectedBox = selectedBox;

        public void DoOperation()
        {
            // remove the selected box
            MapLayer boxLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.BOXLAYER);

            for (int i = boxLayer.MapLayerComponents.Count - 1; i > 0; i--)
            {
                if (boxLayer.MapLayerComponents[i] is PlacedMapBox box && box.BoxGuid.ToString() == SelectedBox.BoxGuid.ToString())
                {
                    boxLayer.MapLayerComponents.RemoveAt(i);
                }
            }

            for (int i = MapLabelMethods.MAP_BOXES.Count - 1; i >= 0; i--)
            {
                if (MapLabelMethods.MAP_BOXES[i].BoxGuid.ToString() == SelectedBox.BoxGuid.ToString())
                {
                    MapLabelMethods.MAP_BOXES.RemoveAt(i);
                }
            }

            MapBuilder.GetLayerCanvas(Map, MapBuilder.BOXLAYER)?.Clear();
        }

        public void UndoOperation()
        {
            SelectedBox.IsSelected = false;

            MapLabelMethods.MAP_BOXES.Add(SelectedBox);

            MapLayer boxLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.BOXLAYER);

            boxLayer.MapLayerComponents.Add(SelectedBox);
        }
    }
}