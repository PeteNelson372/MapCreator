using SkiaSharp;

namespace MapCreator
{
    internal class Cmd_ErasePaintedWaterFeature : IMapOperation
    {
        private readonly MapCreatorMap Map;

        private List<Tuple<Guid, SKPath>> StoredPaths = [];

        public Cmd_ErasePaintedWaterFeature(MapCreatorMap map)
        {
            Map = map;
            foreach (MapPaintedWaterFeature wf in WaterFeatureMethods.PAINTED_WATERFEATURE_LIST)
            {
                Tuple<Guid, SKPath> t = new(wf.WaterFeatureGuid, new(wf.WaterFeaturePath));
                StoredPaths.Add(t);
            }
        }

        public void AddCircle(float x, float y, int brushRadius)
        {
            WaterFeatureMethods.WATER_LAYER_ERASER_PATH.AddCircle(x, y, brushRadius);
        }

        public void DoOperation()
        {
            WaterFeatureMethods.EraseWaterPath(Map);
        }

        public void UndoOperation()
        {
            if (StoredPaths.Count > 0)
            {
                foreach (MapPaintedWaterFeature wf in WaterFeatureMethods.PAINTED_WATERFEATURE_LIST)
                {
                    foreach (Tuple<Guid, SKPath> t in StoredPaths)
                    {
                        if (t.Item1.ToString() == wf.WaterFeatureGuid.ToString())
                        {
                            wf.WaterFeaturePath.Dispose();
                            wf.WaterFeaturePath = new(t.Item2);
                        }
                    }
                }
            }
        }
    }
}
