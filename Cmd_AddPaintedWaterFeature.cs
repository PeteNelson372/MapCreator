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
    internal class Cmd_AddPaintedWaterFeature(MapCreatorMap map) : IMapOperation
    {
        private readonly MapCreatorMap Map = map;
        private List<MapPaintedWaterFeature> STORED_WATERFEATURE_LIST { get; set; } = [];

        private MapPaintedWaterFeature? STORED_WATERFEATURE { get; set; } = null;

        public void DoOperation()
        {
            if (STORED_WATERFEATURE != null)
            {
                WaterFeatureMethods.NEW_WATERFEATURE = STORED_WATERFEATURE;
            }

            STORED_WATERFEATURE_LIST = new(WaterFeatureMethods.PAINTED_WATERFEATURE_LIST);

            WaterFeatureMethods.AddNewPaintedWaterFeatureToWaterFeatureList();

            // merge water features
            WaterFeatureMethods.MergeWaterFeatures();

            WaterFeatureMethods.NEW_WATERFEATURE = WaterFeatureMethods.PAINTED_WATERFEATURE_LIST.Last();

            STORED_WATERFEATURE = WaterFeatureMethods.NEW_WATERFEATURE;

            WaterFeatureMethods.ResetWaterFeaturesOnCanvas(Map);
        }

        public void UndoOperation()
        {
            //WaterFeatureMethods.GetNewWaterFeature(Map);

            WaterFeatureMethods.NEW_WATERFEATURE = null;

            WaterFeatureMethods.PAINTED_WATERFEATURE_LIST.Clear();
            WaterFeatureMethods.PAINTED_WATERFEATURE_LIST = new(STORED_WATERFEATURE_LIST);

            MapBuilder.ClearLayerBitmap(Map, MapBuilder.WATERLAYER);
            MapBuilder.ClearLayerCanvas(Map, MapBuilder.WATERLAYER);

            WaterFeatureMethods.ResetWaterFeaturesOnCanvas(Map);
            STORED_WATERFEATURE_LIST.Clear();
        }
    }
}
