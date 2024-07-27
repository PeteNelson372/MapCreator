namespace MapCreator
{
    internal class Cmd_AddMapRegionPoint(MapCreatorMap map, MapRegion mapRegion, MapRegionPoint newMapRegionPoint, int locationIndex) : IMapOperation
    {
        private readonly MapCreatorMap Map = map;
        private readonly MapRegion SelectedMapRegion = mapRegion;
        private readonly MapRegionPoint NewMapRegionPoint = newMapRegionPoint;
        private readonly int LocationIndex = locationIndex;

        public void DoOperation()
        {
            SelectedMapRegion.MapRegionPoints.Insert(LocationIndex, NewMapRegionPoint);
        }

        public void UndoOperation()
        {
            SelectedMapRegion.MapRegionPoints.Remove(NewMapRegionPoint);
            MapBuilder.GetLayerCanvas(Map, MapBuilder.REGIONLAYER).Clear();
        }
    }
}
