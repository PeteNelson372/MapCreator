using SkiaSharp;

namespace MapCreator
{
    internal class Cmd_AddRiver(MapCreatorMap map, SKPoint riverPoint) : IMapOperation
    {
        private readonly MapCreatorMap Map = map;
        private readonly SKPoint RiverPoint = riverPoint;
        private MapRiver? River = null;

        public void DoOperation()
        {
            if (River != null)
            {
                River.IsSelected = false;

                WaterFeatureMethods.MAP_RIVER_LIST.Add(River);

                MapLayer waterLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.WATERLAYER);

                waterLayer.MapLayerComponents.Add(River);
            }
            else
            {
                WaterFeatureMethods.RIVER_POINT_LIST.Add(new MapRiverPoint(RiverPoint));

                if (WaterFeatureMethods.RIVER_POINT_LIST.Count > 0)
                {
                    WaterFeatureMethods.SetSelectedRiverPoints(WaterFeatureMethods.NEW_RIVER);
                }

                River = WaterFeatureMethods.ConstructNewRiver(Map);
                WaterFeatureMethods.ResetWaterFeaturesOnCanvas(Map);
            }
        }

        public void UndoOperation()
        {
            List<MapRiver> riverList = WaterFeatureMethods.MAP_RIVER_LIST;

            if (River != null)
            {
                riverList.Remove(River);

                MapLayer waterLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.WATERLAYER);

                for (int j = waterLayer.MapLayerComponents.Count - 1; j > 0; j--)
                {
                    if (waterLayer.MapLayerComponents[j] is MapRiver mr && mr.MapRiverGuid.ToString() == River.MapRiverGuid.ToString())
                    {
                        waterLayer.MapLayerComponents.RemoveAt(j);
                    }
                }
            }
        }
    }
}
