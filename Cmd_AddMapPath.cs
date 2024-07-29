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
    internal class Cmd_AddMapPath(MapCreatorMap map, SKPoint mapPathPoint) : IMapOperation
    {
        private readonly MapCreatorMap Map = map;
        private readonly SKPoint MapPathPoint = mapPathPoint;

        private MapPath? newMapPath = null;
        private int pathLayerIndex = -1;

        public void DoOperation()
        {
            if (newMapPath != null && (pathLayerIndex == MapBuilder.PATHLOWERLAYER || pathLayerIndex == MapBuilder.PATHUPPERLAYER))
            {
                newMapPath.IsSelected = false;

                MapPathMethods.GetMapPathList().Add(newMapPath);

                MapLayer pathLayer = MapBuilder.GetMapLayerByIndex(Map, pathLayerIndex);

                pathLayer.MapLayerComponents.Add(newMapPath);
            }
            else
            {
                MapPathMethods.GetPathPointList().Add(new MapPathPoint(MapPathPoint));
                if (MapPathMethods.GetPathPointList().Count > 0)
                {
                    MapPathMethods.SetSelectedMapPathPoints(MapPathMethods.GetNewPath());
                }

                MapPathMethods.ConstructNewMapPath(Map);

                newMapPath = MapPathMethods.GetNewPath();
            }
        }

        public void UndoOperation()
        {
            if (newMapPath != null)
            {
                List<MapPath> mapPathList = MapPathMethods.GetMapPathList();
                mapPathList.Remove(newMapPath);

                // remove the selected path from the lower pathLayer
                MapLayer pathLayerLower = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.PATHLOWERLAYER);

                for (int j = pathLayerLower.MapLayerComponents.Count - 1; j > 0; j--)
                {
                    if (pathLayerLower.MapLayerComponents[j] is MapPath p && p.MapPathGuid.ToString() == newMapPath.MapPathGuid.ToString())
                    {
                        pathLayerLower.MapLayerComponents.RemoveAt(j);
                        pathLayerIndex = MapBuilder.PATHLOWERLAYER;
                        break;
                    }
                }

                // remove the selected path from the upper pathLayer
                MapLayer pathLayerUpper = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.PATHUPPERLAYER);

                for (int j = pathLayerUpper.MapLayerComponents.Count - 1; j > 0; j--)
                {
                    if (pathLayerUpper.MapLayerComponents[j] is MapPath p && p.MapPathGuid.ToString() == newMapPath.MapPathGuid.ToString())
                    {
                        pathLayerUpper.MapLayerComponents.RemoveAt(j);
                        pathLayerIndex = MapBuilder.PATHUPPERLAYER;
                        break;
                    }
                }
            }

        }
    }
}
