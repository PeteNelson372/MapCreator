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
                SKPath lakePath = MapDrawingMethods.GenerateRandomLakePath(LakePoint, BrushRadius);

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
