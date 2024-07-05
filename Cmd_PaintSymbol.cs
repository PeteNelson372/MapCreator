using SkiaSharp;

namespace MapCreator
{
    internal class Cmd_PaintSymbol(MapSymbol symbol, SKColor color) : IMapOperation
    {
        private MapSymbol Symbol = symbol;
        private SKColor SymbolColor = color;

        public void DoOperation()
        {
            Symbol.SetSymbolCustomColorAtIndex(SymbolColor, 0);
            SKPaint paint = new()
            {
                ColorFilter = SKColorFilter.CreateBlendMode(SymbolColor,
                    SKBlendMode.Modulate) // combine the selected color with the bitmap colors
            };

            Symbol.SetSymbolPaint(paint);
        }

        public void UndoOperation()
        {
            Symbol.SetSymbolPaint(null);
        }
    }
}
