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
    internal class Cmd_AddLandform(MapCreatorMap map) : IMapOperation
    {
        private readonly MapCreatorMap Map = map;
        private List<MapLandformType2> STORED_LANDFORM_LIST { get; set; } = [];

        private MapLandformType2? STORED_LANDFORM { get; set; } = null;

        public void DoOperation()
        {
            if (STORED_LANDFORM != null)
            {
                LandformType2Methods.SELECTED_LANDFORM = STORED_LANDFORM;
            }

            STORED_LANDFORM_LIST = new(LandformType2Methods.LANDFORM_LIST);
            
            LandformType2Methods.AddSelectedLandFormToLandformList();
            LandformType2Methods.MergeLandforms();

            LandformType2Methods.SELECTED_LANDFORM = LandformType2Methods.LANDFORM_LIST.Last();

            STORED_LANDFORM = LandformType2Methods.SELECTED_LANDFORM;

            LandformType2Methods.CreateType2LandformPaths(Map, LandformType2Methods.SELECTED_LANDFORM);

            LandformType2Methods.ResetLandformsOnCanvas(Map);
        }

        public void UndoOperation()
        {
            // have to create a new selected landform to completely clear the one being undone
            LandformType2Methods.GetNewSelectedLandform(Map);

            LandformType2Methods.LANDFORM_LIST.Clear();
            LandformType2Methods.LANDFORM_LIST = new(STORED_LANDFORM_LIST);

            MapBuilder.ClearLayerBitmap(Map, MapBuilder.LANDFORMLAYER);
            MapBuilder.ClearLayerBitmap(Map, MapBuilder.LANDCOASTLINELAYER);
            MapBuilder.ClearLayerBitmap(Map, MapBuilder.LANDDRAWINGLAYER);

            MapBuilder.ClearLayerCanvas(Map, MapBuilder.LANDFORMLAYER);
            MapBuilder.ClearLayerCanvas(Map, MapBuilder.LANDCOASTLINELAYER);
            MapBuilder.ClearLayerCanvas(Map, MapBuilder.LANDDRAWINGLAYER);

            LandformType2Methods.ResetLandformsOnCanvas(Map);
            STORED_LANDFORM_LIST.Clear();
        }
    }
}
