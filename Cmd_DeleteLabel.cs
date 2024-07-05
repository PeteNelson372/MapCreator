namespace MapCreator
{
    internal class Cmd_DeleteLabel(MapCreatorMap map, MapLabel label) : IMapOperation
    {
        private readonly MapCreatorMap Map = map;
        private readonly MapLabel Label = label;

        public void DoOperation()
        {
            for (int i = MapLabelMethods.MAP_LABELS.Count - 1; i >= 0; i--)
            {
                if (MapLabelMethods.MAP_LABELS[i].LabelGuid.ToString() == Label.LabelGuid.ToString())
                {
                    MapLabelMethods.MAP_LABELS.RemoveAt(i);
                }
            }

            MapLayer labelLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.LABELLAYER);

            for (int i = labelLayer.MapLayerComponents.Count - 1; i >= 0; i--)
            {
                if (labelLayer.MapLayerComponents[i] is MapLabel l && l.LabelGuid.ToString() == Label.LabelGuid.ToString())
                {
                    labelLayer.MapLayerComponents.RemoveAt(i);
                }
            }
        }

        public void UndoOperation()
        {
            Label.IsSelected = false;

            MapLayer labelLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.LABELLAYER);
            labelLayer.MapLayerComponents.Add(Label);
            MapLabelMethods.MAP_LABELS.Add(Label);
        }
    }
}
