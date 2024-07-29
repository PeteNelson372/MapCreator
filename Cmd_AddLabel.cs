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
    internal class Cmd_AddLabel(MapCreatorMap map, MapLabel label) : IMapOperation
    {
        private readonly MapCreatorMap Map = map;
        private readonly MapLabel Label = label;

        public void DoOperation()
        {
            MapLayer labelLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.LABELLAYER);
            labelLayer.MapLayerComponents.Add(Label);
            MapLabelMethods.MAP_LABELS.Add(Label);
        }

        public void UndoOperation()
        {
            for (int i = MapLabelMethods.MAP_LABELS.Count - 1; i >= 0; i--)
            {
                if (MapLabelMethods.MAP_LABELS[i].LabelGuid.ToString() == Label.LabelGuid.ToString())
                {
                    MapLabelMethods.MAP_LABELS.RemoveAt(i);
                }
            }

            MapLayer labelLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.LABELLAYER);

            for (int i = labelLayer.MapLayerComponents.Count - 1; i >= 0; i--)
            {
                if (labelLayer.MapLayerComponents[i] is MapLabel l && l.LabelGuid.ToString() == Label.LabelGuid.ToString())
                {
                    labelLayer.MapLayerComponents.RemoveAt(i);
                }
            }
        }
    }
}
