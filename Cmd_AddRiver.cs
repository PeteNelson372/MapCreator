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
    internal class Cmd_AddRiver(MapCreatorMap map, SKPoint riverPoint) : IMapOperation
    {
        private readonly MapCreatorMap Map = map;
        private readonly SKPoint RiverPoint = riverPoint;
        private MapRiver? River = null;

        public void DoOperation()
        {
            if (River != null)
            {
                River.IsSelected = false;

                WaterFeatureMethods.MAP_RIVER_LIST.Add(River);

                MapLayer waterLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.WATERLAYER);

                waterLayer.MapLayerComponents.Add(River);
            }
            else
            {
                WaterFeatureMethods.RIVER_POINT_LIST.Add(new MapRiverPoint(RiverPoint));

                if (WaterFeatureMethods.RIVER_POINT_LIST.Count > 0)
                {
                    WaterFeatureMethods.SetSelectedRiverPoints(WaterFeatureMethods.NEW_RIVER);
                }

                River = WaterFeatureMethods.ConstructNewRiver(Map);
                WaterFeatureMethods.ResetWaterFeaturesOnCanvas(Map);
            }
        }

        public void UndoOperation()
        {
            List<MapRiver> riverList = WaterFeatureMethods.MAP_RIVER_LIST;

            if (River != null)
            {
                riverList.Remove(River);

                MapLayer waterLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.WATERLAYER);

                for (int j = waterLayer.MapLayerComponents.Count - 1; j > 0; j--)
                {
                    if (waterLayer.MapLayerComponents[j] is MapRiver mr && mr.MapRiverGuid.ToString() == River.MapRiverGuid.ToString())
                    {
                        waterLayer.MapLayerComponents.RemoveAt(j);
                    }
                }
            }
        }
    }
}
