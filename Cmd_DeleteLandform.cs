namespace MapCreator
{
    internal class Cmd_DeleteLandform(MapCreatorMap map, MapLandformType2 landform) : IMapOperation
    {
        private readonly MapCreatorMap Map = map;
        private readonly MapLandformType2 Landform = landform;

        public void DoOperation()
        {
            List<MapLandformType2> landformList = LandformType2Methods.LANDFORM_LIST;

            landformList.Remove(Landform);

            MapLayer landLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.LANDFORMLAYER);

            for (int j = landLayer.MapLayerComponents.Count - 1; j > 0; j--)
            {
                if (landLayer.MapLayerComponents[j] is MapLandformType2 lf && lf.LandformGuid.ToString() == Landform.LandformGuid.ToString())
                {
                    landLayer.MapLayerComponents.RemoveAt(j);
                }
            }
        }

        public void UndoOperation()
        {
            Landform.IsSelected = false;

            LandformType2Methods.LANDFORM_LIST.Add(Landform);

            MapLayer landLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.LANDFORMLAYER);

            landLayer.MapLayerComponents.Add(Landform);
        }
    }
}
