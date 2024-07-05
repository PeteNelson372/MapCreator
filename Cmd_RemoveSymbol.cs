namespace MapCreator
{
    internal class Cmd_RemoveSymbol(MapCreatorMap map, MapSymbol placedSymbol) : IMapOperation
    {
        private readonly MapCreatorMap Map = map;
        private readonly MapSymbol PlacedSymbol = placedSymbol;

        public void DoOperation()
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

        public void UndoOperation()
        {
            PlacedSymbol.SetIsSelected(false);

            SymbolMethods.PlacedSymbolList.Add(PlacedSymbol);

            MapLayer symbolLayer = MapBuilder.GetMapLayerByIndex(Map, MapBuilder.SYMBOLLAYER);

            symbolLayer.MapLayerComponents.Add(PlacedSymbol);
        }
    }
}
