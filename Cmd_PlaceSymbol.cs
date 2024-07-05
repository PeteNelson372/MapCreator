
namespace MapCreator
{
    internal class Cmd_PlaceSymbol(MapCreatorMap map, MapSymbol placedSymbol) : IMapOperation
    {
        private readonly MapCreatorMap Map = map;
        private readonly MapSymbol PlacedSymbol = placedSymbol;

        public void DoOperation()
        {
            SymbolMethods.PlacedSymbolList.Add(PlacedSymbol);

            MapLayer symbolLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.SYMBOLLAYER);

            symbolLayer.MapLayerComponents.Add(PlacedSymbol);
        }

        public void UndoOperation()
        {
            for (int i = SymbolMethods.PlacedSymbolList.Count - 1; i >= 0; i--)
            {
                if (SymbolMethods.PlacedSymbolList[i].GetSymbolGuid().ToString() == PlacedSymbol.GetSymbolGuid().ToString())
                {
                    SymbolMethods.PlacedSymbolList.RemoveAt(i);
                }
            }

            MapLayer symbolLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.SYMBOLLAYER);

            for (int i = symbolLayer.MapLayerComponents.Count - 1; i >= 0; i--)
            {
                if (symbolLayer.MapLayerComponents[i] is MapSymbol ms && ms.GetSymbolGuid().ToString() == PlacedSymbol.GetSymbolGuid().ToString())
                {
                    symbolLayer.MapLayerComponents.RemoveAt(i);
                }
            }
        }
    }
}
