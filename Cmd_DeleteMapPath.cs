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
    internal class Cmd_DeleteMapPath(MapCreatorMap map, MapPath mapPath) : IMapOperation
    {
        private readonly MapCreatorMap Map = map;
        private readonly MapPath SelectedMapPath = mapPath;
        private int pathLayerIndex = -1;

        public void DoOperation()
        {
            List<MapPath> mapPathList = MapPathMethods.GetMapPathList();
            mapPathList.Remove(SelectedMapPath);

            // remove the selected path from the lower pathLayer
            MapLayer pathLayerLower = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.PATHLOWERLAYER);

            for (int j = pathLayerLower.MapLayerComponents.Count - 1; j > 0; j--)
            {
                if (pathLayerLower.MapLayerComponents[j] is MapPath p && p.MapPathGuid.ToString() == SelectedMapPath.MapPathGuid.ToString())
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
                if (pathLayerUpper.MapLayerComponents[j] is MapPath p && p.MapPathGuid.ToString() == SelectedMapPath.MapPathGuid.ToString())
                {
                    pathLayerUpper.MapLayerComponents.RemoveAt(j);
                    pathLayerIndex = MapBuilder.PATHUPPERLAYER;
                    break;
                }
            }
        }

        public void UndoOperation()
        {
            if (pathLayerIndex == MapBuilder.PATHLOWERLAYER || pathLayerIndex == MapBuilder.PATHUPPERLAYER)
            {
                SelectedMapPath.IsSelected = false;

                MapPathMethods.GetMapPathList().Add(SelectedMapPath);

                MapLayer pathLayer = MapBuilder.GetMapLayerByIndex(Map, pathLayerIndex);

                pathLayer.MapLayerComponents.Add(SelectedMapPath);
            }
        }
    }
}
