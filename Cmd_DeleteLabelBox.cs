namespace MapCreator
{
    internal class Cmd_DeleteLabelBox(MapCreatorMap map, PlacedMapBox selectedBox) : IMapOperation
    {
        private readonly MapCreatorMap Map = map;
        private PlacedMapBox SelectedBox = selectedBox;

        public void DoOperation()
        {
            // remove the selected box
            MapLayer boxLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.BOXLAYER);

            for (int i = boxLayer.MapLayerComponents.Count - 1; i > 0; i--)
            {
                if (boxLayer.MapLayerComponents[i] is PlacedMapBox box && box.BoxGuid.ToString() == SelectedBox.BoxGuid.ToString())
                {
                    boxLayer.MapLayerComponents.RemoveAt(i);
                }
            }

            for (int i = MapLabelMethods.MAP_BOXES.Count - 1; i >= 0; i--)
            {
                if (MapLabelMethods.MAP_BOXES[i].BoxGuid.ToString() == SelectedBox.BoxGuid.ToString())
                {
                    MapLabelMethods.MAP_BOXES.RemoveAt(i);
                }
            }

            MapBuilder.GetLayerCanvas(Map, MapBuilder.BOXLAYER)?.Clear();
        }

        public void UndoOperation()
        {
            SelectedBox.IsSelected = false;

            MapLabelMethods.MAP_BOXES.Add(SelectedBox);

            MapLayer boxLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.BOXLAYER);

            boxLayer.MapLayerComponents.Add(SelectedBox);
        }
    }
}