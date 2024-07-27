using SkiaSharp;

namespace MapCreator
{
    internal class Cmd_ClearLayerCanvas : IMapOperation
    {
        public MapCreatorMap? Map { get; set; } = null;
        public int LayerIndex { get; set; } = -1;
        SKCanvas? LayerCanvas { get; set; } = null;

        public Cmd_ClearLayerCanvas(MapCreatorMap map, int layerIndex)
        {
            Map = map;
            LayerIndex = layerIndex;
        }

        public void DoOperation()
        {
            if (Map != null && LayerIndex >= 0 && LayerIndex < MapBuilder.MAP_LAYER_COUNT)
            {
                MapBuilder.ClearLayerCanvas(Map, LayerIndex);
            }
        }

        public void UndoOperation()
        {
        }
    }
}
