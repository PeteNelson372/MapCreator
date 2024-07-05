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
