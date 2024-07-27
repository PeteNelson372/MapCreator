namespace MapCreator
{
    internal class Cmd_DeleteMapRegionPoint(MapCreatorMap map, MapRegion mapRegion, MapRegionPoint regionPoint) : IMapOperation
    {
        private readonly MapCreatorMap Map = map;
        private readonly MapRegion SelectedMapRegion = mapRegion;
        private readonly MapRegionPoint DeletedMapRegionPoint = regionPoint;
        private int PointIndex = -1;

        public void DoOperation()
        {
            if (SelectedMapRegion.MapRegionPoints.Count > 2)
            {
                for (int i = 0; i < SelectedMapRegion.MapRegionPoints.Count - 1; i++)
                {
                    if (SelectedMapRegion.MapRegionPoints[i].PointGuid.ToString() == DeletedMapRegionPoint.PointGuid.ToString())
                    {
                        PointIndex = i;
                        break;
                    }
                }

                SelectedMapRegion.MapRegionPoints.Remove(DeletedMapRegionPoint);
                MapBuilder.GetLayerCanvas(Map, MapBuilder.REGIONLAYER).Clear();
            }
        }

        public void UndoOperation()
        {
            SelectedMapRegion.MapRegionPoints.Insert(PointIndex, DeletedMapRegionPoint);
            MapBuilder.GetLayerCanvas(Map, MapBuilder.REGIONLAYER).Clear();
        }
    }
}
