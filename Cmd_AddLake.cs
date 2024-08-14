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
    internal class Cmd_AddLake(MapCreatorMap map, int brushRadius, SKPoint lakePoint) : IMapOperation
    {
        private readonly MapCreatorMap Map = map;
        private readonly int BrushRadius = brushRadius;
        private readonly SKPoint LakePoint = lakePoint;
        private MapPaintedWaterFeature? Lake;

        private List<MapPaintedWaterFeature> STORED_WATERFEATURE_LIST { get; set; } = [];

        public void DoOperation()
        {
            if (Lake != null)
            {
                Lake.IsSelected = false;

                WaterFeatureMethods.PAINTED_WATERFEATURE_LIST.Add(Lake);

                MapLayer waterLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.WATERLAYER);

                waterLayer.MapLayerComponents.Add(Lake);
            }
            else
            {
                WaterFeatureMethods.AddNewPaintedWaterFeatureToWaterFeatureList();
                SKPath lakePath = MapDrawingMethods.GenerateRandomLakePath2(LakePoint, BrushRadius);

                WaterFeatureMethods.WATER_LAYER_DRAW_PATH.AddPath(lakePath);

                Lake = WaterFeatureMethods.MergeWaterFeatures();

                Lake ??= WaterFeatureMethods.NEW_WATERFEATURE;

                if (lakePath.PointCount > 0)
                {
                    Lake.SetWaterFeaturePath(lakePath);
                }

                WaterFeatureMethods.PaintLake(Map);

                WaterFeatureMethods.ResetWaterFeaturesOnCanvas(Map);
            }

        }

        public void UndoOperation()
        {
            List<MapPaintedWaterFeature> waterFeatureList = WaterFeatureMethods.PAINTED_WATERFEATURE_LIST;

            if (Lake != null)
            {
                waterFeatureList.Remove(Lake);

                MapLayer waterLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.WATERLAYER);

                for (int j = waterLayer.MapLayerComponents.Count - 1; j > 0; j--)
                {
                    if (waterLayer.MapLayerComponents[j] is MapPaintedWaterFeature wf && wf.WaterFeatureGuid.ToString() == Lake.WaterFeatureGuid.ToString())
                    {
                        waterLayer.MapLayerComponents.RemoveAt(j);
                    }
                }

                MapBuilder.ClearLayerBitmap(Map, MapBuilder.WATERLAYER);
                MapBuilder.ClearLayerCanvas(Map, MapBuilder.WATERLAYER);

                WaterFeatureMethods.ResetWaterFeaturesOnCanvas(Map);
            }
        }
    }
}
