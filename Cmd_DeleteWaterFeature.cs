namespace MapCreator
{
    internal class Cmd_DeleteWaterFeature(MapCreatorMap map, MapPaintedWaterFeature waterFeature) : IMapOperation
    {
        private readonly MapCreatorMap Map = map;
        private readonly MapPaintedWaterFeature WaterFeature = waterFeature;

        public void DoOperation()
        {
            List<MapPaintedWaterFeature> waterFeatureList = WaterFeatureMethods.PAINTED_WATERFEATURE_LIST;
            waterFeatureList.Remove(WaterFeature);

            MapLayer waterLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.WATERLAYER);

            for (int j = waterLayer.MapLayerComponents.Count - 1; j > 0; j--)
            {
                if (waterLayer.MapLayerComponents[j] is MapPaintedWaterFeature wf && wf.WaterFeatureGuid.ToString() == WaterFeature.WaterFeatureGuid.ToString())
                {
                    waterLayer.MapLayerComponents.RemoveAt(j);
                }
            }
        }

        public void UndoOperation()
        {
            WaterFeature.IsSelected = false;

            WaterFeatureMethods.PAINTED_WATERFEATURE_LIST.Add(WaterFeature);

            MapLayer waterLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.WATERLAYER);

            waterLayer.MapLayerComponents.Add(WaterFeature);
        }
    }
}
