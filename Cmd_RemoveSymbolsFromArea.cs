using SkiaSharp;

namespace MapCreator
{
    internal class Cmd_RemoveSymbolsFromArea(MapCreatorMap map, float eraserRadius, SKPoint eraserPoint) : IMapOperation
    {
        private readonly MapCreatorMap Map = map;
        private readonly float EraserCircleRadius = eraserRadius;
        private readonly SKPoint CenterPoint = eraserPoint;

        private List<MapSymbol> RemovedSymbolList { get; set; } = [];

        public void DoOperation()
        {
            for (int i = SymbolMethods.PlacedSymbolList.Count - 1; i >= 0; i--)
            {
                SKPoint symbolPoint = new(SymbolMethods.PlacedSymbolList[i].X, SymbolMethods.PlacedSymbolList[i].Y);

                if (MapDrawingMethods.PointInCircle(EraserCircleRadius, CenterPoint, symbolPoint))
                {
                    MapSymbol s = SymbolMethods.PlacedSymbolList[i];
                    SymbolMethods.PlacedSymbolList.Remove(s);

                    MapLayer symbolLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.SYMBOLLAYER);
                    symbolLayer.MapLayerComponents.Remove(s);

                    RemovedSymbolList.Add(s);
                }
            }
        }

        public void UndoOperation()
        {
            foreach (MapSymbol symbol in RemovedSymbolList)
            {
                SymbolMethods.PlacedSymbolList.Add(symbol);

                MapLayer symbolLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.SYMBOLLAYER);
                symbolLayer.MapLayerComponents.Add(symbol);
            }

            RemovedSymbolList.Clear();
        }
    }
}
