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
    internal class Cmd_EraseLandform : IMapOperation
    {
        private readonly MapCreatorMap Map;

        private List<Tuple<Guid, SKPath>> StoredPaths = [];

        public Cmd_EraseLandform(MapCreatorMap map)
        {
            Map = map;
            foreach (MapLandformType2 lft2 in LandformType2Methods.LANDFORM_LIST)
            {
                Tuple<Guid, SKPath> t = new(lft2.LandformGuid, new(lft2.LandformPath));
                StoredPaths.Add(t);
            }
        }

        public void AddCircle(float x, float y, int brushRadius)
        {
            LandformType2Methods.LAND_LAYER_ERASER_PATH.AddCircle(x, y, brushRadius);
        }

        public void DoOperation()
        {
            LandformType2Methods.EraseLandForm(Map);

            foreach (MapLandformType2 lf in LandformType2Methods.LANDFORM_LIST)
            {
                lf.LandformContourPath.Dispose();
                LandformType2Methods.CreateType2LandformPaths(Map, lf);
            }

            LandformType2Methods.LAND_LAYER_ERASER_PATH.Reset();
        }

        public void UndoOperation()
        {
            if (StoredPaths.Count > 0)
            {
                foreach (MapLandformType2 lf in LandformType2Methods.LANDFORM_LIST)
                {
                    foreach(Tuple<Guid, SKPath> t in StoredPaths)
                    {
                        if (t.Item1.ToString() == lf.LandformGuid.ToString())
                        {
                            lf.LandformPath.Dispose();
                            lf.LandformPath = new(t.Item2);
                            LandformType2Methods.CreateType2LandformPaths(Map, lf);
                        }
                    }
                }
            }

        }
    }
}
