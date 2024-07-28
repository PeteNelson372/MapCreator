
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace MapCreator
{
    internal class SymbolMethods
    {
        private static List<MapSymbolCollection> MapSymbolCollections = [];
        
        // the symbols read from symbol collections
        private static List<MapSymbol> MapSymbolList { get; set; } = [];

        // the symbol selected by the user from the SymbolTable control on the UI
        private static MapSymbol? SelectedMapSymbol = null;

        // additional symbols selected by the user from the SymbolTable control on the US
        private static readonly List<MapSymbol> SecondarySelectedSymbols = [];

        // the symbols placed on the map
        public static List<MapSymbol> PlacedSymbolList { get; set; } = [];

        // the tags that can be selected in the UI to filter the tags in the tag list box on the UI
        private static readonly List<string> OriginalSymbolTags = [];
        private static readonly List<string> SymbolTags = [];

        // the symbols associated with each tag
        private static readonly List<Tuple<string, List<MapSymbol>>> TagSymbolAssociationList = [];

        private static readonly string DefaultSymbolDirectory = Resources.ASSET_DIRECTORY + Path.DirectorySeparatorChar + "Symbols";
        
        private static readonly string SymbolTagsFilePath = DefaultSymbolDirectory + Path.DirectorySeparatorChar + "SymbolTags.txt";

        private static readonly string CollectionFileName = "collection.xml";

        private static readonly string WonderdraftSymbolsFileName = ".wonderdraft_symbols";

        // the SKPaint object used to draw the box around the selected symbol
        private static readonly SKPaint MAPSYMBOL_SELECT_PAINT = new();

        // the set of user-selected colors used to color grayscale and colorable symbols
        private static readonly SKColor[] CustomColors = new SKColor[4];


        public static void ConstructMapSymbolPaintObjects()
        {
            MAPSYMBOL_SELECT_PAINT.Style = SKPaintStyle.Stroke;
            MAPSYMBOL_SELECT_PAINT.IsAntialias = true;
            MAPSYMBOL_SELECT_PAINT.Color = SKColors.LawnGreen;
            MAPSYMBOL_SELECT_PAINT.StrokeWidth = 1;
            MAPSYMBOL_SELECT_PAINT.PathEffect = SKPathEffect.CreateDash([3F, 3F], 6F);
        }

        public static string GetDefaultSymbolDirectory()
        {
            return DefaultSymbolDirectory;
        }

        public static string GetCollectionFileName()
        {
            return CollectionFileName;
        }

        public static string GetWonderdraftSymbolsFileName()
        {
            return WonderdraftSymbolsFileName;
        }

        public static void ClearSymbolCollections()
        {
            MapSymbolCollections.Clear();
        }

        public static List<MapSymbolCollection> GetSymbolCollections()
        {
            return MapSymbolCollections;
        }

        public static string GetSymbolTagsFilePath()
        {
            return SymbolTagsFilePath;
        }

        public static void LoadSymbolTags()
        {
            SymbolTags.Clear();
            OriginalSymbolTags.Clear();

            IEnumerable<string> tags = File.ReadLines(SymbolTagsFilePath);
            foreach (string tag in tags)
            {
                if (!string.IsNullOrEmpty(tag))
                {
                    AddSymbolTag(tag);
                }
            }

            SymbolTags.Sort();

            foreach (string tag in SymbolTags)
            {
                OriginalSymbolTags.Add(tag);
            }
        }

        public static void SaveSymbolTags()
        {
            if (SymbolTags.Count >= OriginalSymbolTags.Count)
            {
                File.WriteAllLines(SymbolTagsFilePath, SymbolTags);
            }
        }

        public static List<string> GetSymbolTags()
        {
            return SymbolTags;
        }

        public static void AddTagSymbolAssocation(string tag, MapSymbol mapSymbol)
        {
            Tuple<string, List<MapSymbol>>? tagSymbols = TagSymbolAssociationList.Find(x => x.Item1.Equals(tag));

            if (tagSymbols != null)
            {
                if (!tagSymbols.Item2.Contains(mapSymbol))
                {
                    tagSymbols.Item2.Add(mapSymbol);
                }
            }
            else
            {
                Tuple<string, List<MapSymbol>> newTagAssociation = new(tag, []);
                newTagAssociation.Item2.Add(mapSymbol);

                TagSymbolAssociationList.Add(newTagAssociation);
            }
        }

        public static void AddMapSymbolCollection(MapSymbolCollection collection)
        {
            MapSymbolCollections.Add(collection);
        }

        public static void AddMapSymbol(MapSymbol mapSymbol)
        {
            MapSymbolList.Add(mapSymbol);
        }

        public static void AddSymbolTag(string tag)
        {
            tag = tag.Trim([' ', ',']).ToLower();
            if (SymbolTags.Contains(tag)) return;
            SymbolTags.Add(tag);
        }

        internal static int LoadSymbolCollections()
        {
            int numSymbols = 0;
            try
            {
                var files = from file in Directory.EnumerateFiles(DefaultSymbolDirectory, "*.*", SearchOption.AllDirectories).Order()
                            where file.EndsWith(CollectionFileName)
                            select new
                            {
                                File = file
                            };

                foreach (var f in files)
                {
                    MapSymbolCollection? collection = MapFileMethods.ReadCollectionFromXml(f.File);

                    if (collection != null)
                    {
                        MapSymbolCollections.Add(collection);

                        // load symbol file into object
                        foreach (MapSymbol symbol in collection.GetCollectionMapSymbols())
                        {
                            numSymbols++;

                            if (!string.IsNullOrEmpty(symbol.GetSymbolFilePath()))
                            {
                                if (symbol.GetSymbolFormat() == SymbolFormatEnum.PNG
                                    || symbol.GetSymbolFormat() == SymbolFormatEnum.JPG
                                    || symbol.GetSymbolFormat() == SymbolFormatEnum.BMP)
                                {
                                    symbol.SetSymbolBitmapFromPath(symbol.GetSymbolFilePath());
                                }

                                MapSymbolList.Add(symbol);

                                foreach (string tag in  symbol.GetSymbolTags())
                                {
                                    AddTagSymbolAssocation(tag, symbol);
                                }
                            }

                            if (string.IsNullOrEmpty(symbol.GetCollectionPath()))
                            {
                                symbol.SetCollectionPath(collection.GetCollectionPath());
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading symbols from collection: " + ex.Message);
            }

            return numSymbols;
        }

        internal static void SaveCollections()
        {
            foreach (MapSymbolCollection collection in MapSymbolCollections)
            {
                if (collection != null && collection.IsModified)
                {
                    // make a backup of the current collection.xml file
                    File.Copy(collection.GetCollectionPath(), collection.GetCollectionPath() + ".backup", true);

                    MapFileMethods.SerializeSymbolCollection(collection);
                }
            }
        }

        internal static string GetSymbolName(MapSymbol symbol)
        {
            string symbolName = symbol.GetSymbolName();

            if (string.IsNullOrEmpty(symbolName))
            {
                symbolName = Path.GetFileNameWithoutExtension(symbol.GetSymbolFilePath());
                symbol.SetSymbolName(symbolName);
            }

            return symbolName;
        }

        internal static MapSymbol? GetSelectedMapSymbol()
        {
            return SelectedMapSymbol;
        }

        internal static void SetSelectedMapSymbol(MapSymbol mapSymbol)
        {
            SelectedMapSymbol = mapSymbol;
        }

        internal static void ClearSelectedMapSymbol()
        {
            SelectedMapSymbol = null;
        }

        internal static void PlaceSymbolOnMap(MapCreatorMap map, MapSymbol? mapSymbol, SKBitmap? bitmap, SKPoint cursorPoint)
        {
            if (mapSymbol != null && bitmap != null)
            {
                map.IsSaved = false;

                MapSymbol placedSymbol = new(mapSymbol)
                {
                    X = (int)cursorPoint.X,
                    XLocation = (int)cursorPoint.X,
                    Y = (int)cursorPoint.Y,
                    YLocation = (int)cursorPoint.Y,
                    Width = bitmap.Width,
                    SymbolWidth = bitmap.Width,
                    Height = bitmap.Height,
                    SymbolHeight = bitmap.Height,
                };

                for (int i = 0; i < CustomColors.Length; i++)
                {
                    placedSymbol.SetSymbolCustomColorAtIndex(GetCustomColorAtIndex(i), i);
                }

                placedSymbol.SetPlacedBitmap(bitmap);

                Cmd_PlaceSymbol cmd = new(map, placedSymbol);
                UndoManager.AddCommand(cmd);
                cmd.DoOperation();
            }
        }

        internal static void DrawAllSymbols(MapCreatorMap map)
        {
            foreach (MapSymbol symbol in PlacedSymbolList)
            {
                SKCanvas? symbolDrawingCanvas = MapBuilder.GetLayerCanvas(map, MapBuilder.SYMBOLLAYER);

                if (symbolDrawingCanvas != null)
                {
                    symbol.Render(symbolDrawingCanvas);
                }
            }
        }

        internal static MapSymbol? SelectSymbolAtPoint(Point mapClickPoint)
        {
            foreach (MapSymbol symbol in PlacedSymbolList)
            {
                Rectangle symbolRect = new((int)symbol.X, (int)symbol.Y, (int)symbol.Width, (int)symbol.Height);
                if (symbolRect.Contains(mapClickPoint))
                {
                    return symbol;
                }
            }

            return null;
        }

        internal static SKPaint GetSymbolSelectPaint()
        {
            return MAPSYMBOL_SELECT_PAINT;
        }

        internal static List<MapSymbol> GetMapSymbolsWithType(SymbolTypeEnum symbolType)
        {
            List<MapSymbol> typeSymbols = MapSymbolList.FindAll(x => x.GetSymbolType() == symbolType);
            return typeSymbols;
        }

        internal static bool CanPlaceSymbol(MapSymbol? mapSymbol, SKBitmap rotatedAndScaledBitmap, SKPoint cursorPoint, float placementDensity)
        {
            // if the symbol is within the excluded radius from any other symbols of the same kind, then the symbol cannot be placed at the cursor point
            if (mapSymbol == null) return false;

            bool canPlace = true;
            List<MapSymbol> placedSymbols = PlacedSymbolList.FindAll(x => x.GetSymbolFilePath() == mapSymbol.GetSymbolFilePath());
            float exclusionRadius = ((rotatedAndScaledBitmap.Width + rotatedAndScaledBitmap.Height) / 2.0F) / placementDensity;

            foreach (MapSymbol symbol in placedSymbols)
            {
                SKPoint symbolPoint = new((int)symbol.X, (int)symbol.Y);
                bool placeAllowed = !MapDrawingMethods.PointInCircle(exclusionRadius, symbolPoint, cursorPoint);

                if (!placeAllowed)
                {
                    canPlace = false;
                    break;
                }
            }

            return canPlace;
        }

        internal static void AddSecondarySelectedSymbol(MapSymbol symbol)
        {
            if (symbol == null) return;
            SecondarySelectedSymbols.Add(symbol);
        }

        internal static void RemoveSecondarySelectedSymbol(MapSymbol symbol)
        {
            if (symbol == null) return;
            SecondarySelectedSymbols.Remove(symbol);
        }

        internal static void ClearSecondarySelectedSymbols()
        {
            SecondarySelectedSymbols.Clear();
        }

        internal static List<MapSymbol> GetSecondarySelectedSymbols()
        {
            return SecondarySelectedSymbols;
        }

        internal static MapSymbol GetSecondarySelectedSymbolAtIndex(int index)
        {
            return SecondarySelectedSymbols[index];
        }

        internal static void RemovePlacedSymbolsFromArea(MapCreatorMap map, SKPoint centerPoint, float eraserCircleRadius)
        {
            Cmd_RemoveSymbolsFromArea cmd = new(map, eraserCircleRadius, centerPoint);
            UndoManager.AddCommand(cmd);
            cmd.DoOperation();
        }

        public static void SetCustomColorAtIndex(SKColor color, int index)
        {
            if (index < 0 || index > CustomColors.Length - 1) return;
            CustomColors[index] = color;
        }

        public static SKColor GetCustomColorAtIndex(int index)
        {
            return CustomColors[index];
        }

        internal static void ColorSymbolsInArea(SKPoint colorCursorPoint, int colorBrushRadius, Color[] symbolColors)
        {
            foreach (MapSymbol symbol in PlacedSymbolList)
            {
                SKPoint symbolPoint = new(symbol.X, symbol.Y);

                if (MapDrawingMethods.PointInCircle(colorBrushRadius, colorCursorPoint, symbolPoint))
                {
                    if (symbol.GetIsGrayScale())
                    {
                        SKPaint paint = new()
                        {
                            ColorFilter = SKColorFilter.CreateBlendMode(
                                Extensions.ToSKColor(symbolColors[0]),
                                SKBlendMode.Modulate) // combine the selected color with the bitmap colors
                        };

                        symbol.SetSymbolPaint(paint);
                    }

                    symbol.SetSymbolCustomColorAtIndex(Extensions.ToSKColor(symbolColors[0]), 0);
                    symbol.SetSymbolCustomColorAtIndex(Extensions.ToSKColor(symbolColors[1]), 1);
                    symbol.SetSymbolCustomColorAtIndex(Extensions.ToSKColor(symbolColors[2]), 2);
                    symbol.SetSymbolCustomColorAtIndex(Extensions.ToSKColor(symbolColors[3]), 3);
                }
            }
        }

        internal static void AnalyzeSymbolBitmapColors(MapSymbol symbol)
        {
            // gather the colors from each pixel of the bitmap
            // if all colors are shades of gray, transparent, black, or white, then the bitmap is grayscale
            // if all of the colors are either red, green, blue, transparent, or black, the image should use custom colors when it is painted
            // the red, green, and blue values may not be exactly pure colors
            SKBitmap? bitmap = symbol.GetSymbolBitmap();

            if (bitmap == null) return;
            
            if (!symbol.GetIsGrayScale())
            {
                // check for grayscale image
                bool IsGrayScale = MapDrawingMethods.IsGrayScaleImage(Extensions.ToBitmap(bitmap));

                symbol.SetIsGrayScale(IsGrayScale);
            }

            // if the symbol is paintable (probably determined from .wonderdraft_symbols file or user input), then no need to analyze the bitmap colors
            if (!symbol.GetUseCustomColors() && !symbol.GetIsGrayScale())
            {
                // otherwise, check to see if the bitmap uses any non-paintable colors
                // (colors that are not near a pure shade of red, green, blue, or black)
                bool IsPaintable = MapDrawingMethods.IsPaintableImage(Extensions.ToBitmap(bitmap));
                symbol.SetUseCustomColors(IsPaintable);
            }
        }

        internal static List<MapSymbol> GetFilteredSymbolList(SymbolTypeEnum selectedSymbolType, List<string> selectedCollections, List<string> selectedTags)
        {
            List<MapSymbol> filteredSymbols = GetMapSymbolsWithType(selectedSymbolType);

            if (selectedCollections.Count > 0)
            {
                for (int i = filteredSymbols.Count - 1; i >= 0; i--)
                {
                    
                    if (!selectedCollections.Contains(filteredSymbols[i].GetCollectionName()))
                    {
                        filteredSymbols.RemoveAt(i);
                    }
                }
            }

            if (selectedTags.Count > 0)
            {
                foreach (string tag in selectedTags)
                {
                    for (int i = filteredSymbols.Count - 1; i >= 0; i--)
                    {
                        if (!filteredSymbols[i].GetSymbolTags().Contains(tag))
                        {
                            filteredSymbols.RemoveAt(i);
                        }
                    }
                }
            }

            return filteredSymbols;
        }

        internal static List<string> AutoTagSymbol(MapSymbol symbol)
        {
            const int minTagLength = 2;

            List<string> potentialTags = [];
            string[] symbolNameParts = symbol.GetSymbolName().Split([' ', '_']);
            string[] collectionNameParts = symbol.GetCollectionName().Split([' ', '_']);

            foreach (string symbolNamePart in symbolNameParts)
            {
                string potentialTag = new string(symbolNamePart.Where(char.IsLetter).ToArray()).ToLower();

                if (!string.IsNullOrEmpty(potentialTag) && potentialTag.Length > minTagLength && !potentialTags.Contains(potentialTag))
                {
                    potentialTags.Add(potentialTag);
                }
            }

            foreach (string collectionNamePart in collectionNameParts)
            {
                string potentialTag = new string(collectionNamePart.Where(char.IsLetter).ToArray()).ToLower();

                if (!string.IsNullOrEmpty(potentialTag) && potentialTag.Length > minTagLength && !potentialTags.Contains(potentialTag))
                {
                    potentialTags.Add(potentialTag);
                }
            }

            for (int i = potentialTags.Count - 1; i >= 0; i--)
            {
                string potentialTag = potentialTags[i];
                bool tagMatched = false;

                foreach (string tag in SymbolTags)
                {
                    if (tag.Contains(potentialTag) || potentialTag.Contains(tag))
                    {
                        tagMatched = true;
                    }
                }

                if (!tagMatched)
                {
                    potentialTags.RemoveAt(i);
                }
            }

            return potentialTags;

        }
    }
}
