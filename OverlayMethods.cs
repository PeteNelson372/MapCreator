
using SkiaSharp;
using SkiaSharp.Views.Desktop;

namespace MapCreator
{
    internal class OverlayMethods
    {
        public static PlacedMapFrame? MAP_FRAME { get; set; } = null;

        public static List<MapFrame> MAP_FRAME_TEXTURES { get; set; } = [];

        public static MapFrame? SelectedFrame { get; set; } = null;

        internal static void CreateFrame(MapCreatorMap map, MapFrame? frame, Color frameTint, int frameTintOpacity, float frameScale)
        {
            if (frame == null) return;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            PlacedMapFrame mapFrame = new()
            {
                X = 0,
                Y = 0,
                Width = map.MapWidth,
                Height = map.MapHeight,
                MapWidth = map.MapWidth,
                MapHeight = map.MapHeight,
                FrameBitmap = frame.FrameBitmap.Copy(),
                FrameScale = frameScale,
                FrameCenterLeft = frame.FrameCenterLeft,
                FrameCenterTop = frame.FrameCenterTop,
                FrameCenterRight = frame.FrameCenterRight,
                FrameCenterBottom = frame.FrameCenterBottom,
                FrameTint = frameTint,
            };

            using SKPaint framePaint = new()
            {
                Style = SKPaintStyle.Fill,
                ColorFilter = SKColorFilter.CreateBlendMode(
                Extensions.ToSKColor(mapFrame.FrameTint),
                SKBlendMode.Modulate) // combine the tint with the bitmap color
            };

            mapFrame.FramePaint = framePaint.Clone();

            CompletePlacedFrame(mapFrame);

            // there can only be one frame on the map, so remove any existing frame
            for (int i = MapBuilder.GetMapLayerByIndex(map, MapBuilder.OVERLAYLAYER).MapLayerComponents.Count - 1; i > 0; i--)
            {
                if (MapBuilder.GetMapLayerByIndex(map, MapBuilder.OVERLAYLAYER).MapLayerComponents[i] is PlacedMapFrame)
                {
                    MapBuilder.GetMapLayerByIndex(map, MapBuilder.OVERLAYLAYER).MapLayerComponents.RemoveAt(i);
                    break;
                }
            }

            MapBuilder.GetMapLayerByIndex(map, MapBuilder.OVERLAYLAYER).MapLayerComponents.Add(mapFrame);

#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        public static void SliceNinePatchBitmap(PlacedMapFrame mapFrame, SKBitmap resizedBitmap, SKRectI center)
        {
            SKBitmap[] slicedBitmaps = MapDrawingMethods.SliceNinePatchBitmap(resizedBitmap, center);

            mapFrame.Patch_A = slicedBitmaps[0].Copy();
            mapFrame.Patch_B = slicedBitmaps[1].Copy();
            mapFrame.Patch_C = slicedBitmaps[2].Copy();
            mapFrame.Patch_D = slicedBitmaps[3].Copy();
            mapFrame.Patch_E = slicedBitmaps[4].Copy();
            mapFrame.Patch_F = slicedBitmaps[5].Copy();
            mapFrame.Patch_G = slicedBitmaps[6].Copy();
            mapFrame.Patch_H = slicedBitmaps[7].Copy();
            mapFrame.Patch_I = slicedBitmaps[8].Copy();

            slicedBitmaps[0].Dispose();
            slicedBitmaps[1].Dispose();
            slicedBitmaps[2].Dispose();
            slicedBitmaps[3].Dispose();
            slicedBitmaps[4].Dispose();
            slicedBitmaps[5].Dispose();
            slicedBitmaps[6].Dispose();
            slicedBitmaps[7].Dispose();
            slicedBitmaps[8].Dispose();
        }

        internal static void RemoveFrame(MapCreatorMap map)
        {
            for (int i = MapBuilder.GetMapLayerByIndex(map, MapBuilder.OVERLAYLAYER).MapLayerComponents.Count - 1; i > 0; i--)
            {
                if (MapBuilder.GetMapLayerByIndex(map, MapBuilder.OVERLAYLAYER).MapLayerComponents[i] is PlacedMapFrame)
                {
                    MapBuilder.GetMapLayerByIndex(map, MapBuilder.OVERLAYLAYER).MapLayerComponents.RemoveAt(i);
                    break;
                }
            }
        }

        internal static int LoadFrameAssets()
        {
            MAP_FRAME_TEXTURES.Clear();

            string frameAssetDirectory = Resources.ASSET_DIRECTORY + Path.DirectorySeparatorChar + "Frames" + Path.DirectorySeparatorChar;

            int numFrames = 0;
            var files = from file in Directory.EnumerateFiles(frameAssetDirectory, "*.*", SearchOption.AllDirectories).Order()
                        where file.Contains(".xml")
                        select new
                        {
                            File = file
                        };

            foreach (var f in files)
            {
                MapFrame? mapFrame = MapFileMethods.ReadFrameAssetFromXml(f.File);

                if (mapFrame != null)
                {
                    numFrames++;
                    MAP_FRAME_TEXTURES.Add(mapFrame);
                }

            }

            return numFrames;
        }

        internal static void CompletePlacedFrame(PlacedMapFrame mapFrame)
        {
            if (mapFrame.FrameBitmap != null)
            {
                float mapWidthScale = (float)mapFrame.MapWidth / mapFrame.FrameBitmap.Width;
                float mapHeightScale = (float)mapFrame.MapHeight / mapFrame.FrameBitmap.Height;

                SKRectI center = new((int)mapFrame.FrameCenterLeft,
                    (int)mapFrame.FrameCenterTop,
                    (int)(mapFrame.FrameBitmap.Width - mapFrame.FrameCenterRight),
                    (int)(mapFrame.FrameBitmap.Height - mapFrame.FrameCenterBottom));

                if (center.IsEmpty || center.Left < 0 || center.Right <= 0 || center.Top < 0 || center.Bottom <= 0)
                {
                    return;
                }
                else if (center.Width <= 0 || center.Height <= 0)
                {
                    // swap 
                    if (center.Right < center.Left)
                    {
                        (center.Left, center.Right) = (center.Right, center.Left);
                    }

                    if (center.Bottom < center.Top)
                    {
                        (center.Top, center.Bottom) = (center.Bottom, center.Top);
                    }
                }

                SliceNinePatchBitmap(mapFrame, mapFrame.FrameBitmap, center);

                if (mapFrame.FrameBitmap != null
                    && mapFrame.Patch_A != null
                    && mapFrame.Patch_B != null
                    && mapFrame.Patch_C != null
                    && mapFrame.Patch_D != null
                    && mapFrame.Patch_E != null
                    && mapFrame.Patch_F != null
                    && mapFrame.Patch_G != null
                    && mapFrame.Patch_H != null
                    && mapFrame.Patch_I != null)
                {
                    // have to account for the total width and height of the map
                    while ((mapFrame.Patch_A.Width * mapFrame.FrameScale * mapWidthScale)
                        + (mapFrame.Patch_B.Width * mapFrame.FrameScale * mapWidthScale)
                        + (mapFrame.Patch_C.Width * mapFrame.FrameScale * mapWidthScale) > mapFrame.MapWidth)
                    {
                        mapFrame.FrameScale = mapFrame.FrameScale - 0.1F;
                    }

                    while ((mapFrame.Patch_A.Height * mapFrame.FrameScale * mapHeightScale)
                        + (mapFrame.Patch_D.Height * mapFrame.FrameScale * mapHeightScale)
                        + (mapFrame.Patch_G.Height * mapFrame.FrameScale * mapHeightScale) > mapFrame.MapHeight)
                    {
                        mapFrame.FrameScale = mapFrame.FrameScale - 0.1F;
                    }

                    // scale the patches
                    using SKBitmap scaledA = new((int)Math.Round(mapFrame.Patch_A.Width * mapFrame.FrameScale * mapWidthScale), (int)Math.Round(mapFrame.Patch_A.Height * mapFrame.FrameScale * mapHeightScale));
                    using SKBitmap scaledB = new((int)Math.Round(mapFrame.Patch_B.Width * mapFrame.FrameScale * mapWidthScale), (int)Math.Round(mapFrame.Patch_B.Height * mapFrame.FrameScale * mapHeightScale));
                    using SKBitmap scaledC = new((int)Math.Round(mapFrame.Patch_C.Width * mapFrame.FrameScale * mapWidthScale), (int)Math.Round(mapFrame.Patch_C.Height * mapFrame.FrameScale * mapHeightScale));
                    using SKBitmap scaledD = new((int)Math.Round(mapFrame.Patch_D.Width * mapFrame.FrameScale * mapWidthScale), (int)Math.Round(mapFrame.Patch_D.Height * mapFrame.FrameScale * mapHeightScale));
                    using SKBitmap scaledE = new((int)Math.Round(mapFrame.Patch_E.Width * mapFrame.FrameScale * mapWidthScale), (int)Math.Round(mapFrame.Patch_E.Height * mapFrame.FrameScale * mapHeightScale));
                    using SKBitmap scaledF = new((int)Math.Round(mapFrame.Patch_F.Width * mapFrame.FrameScale * mapWidthScale), (int)Math.Round(mapFrame.Patch_F.Height * mapFrame.FrameScale * mapHeightScale));
                    using SKBitmap scaledG = new((int)Math.Round(mapFrame.Patch_G.Width * mapFrame.FrameScale * mapWidthScale), (int)Math.Round(mapFrame.Patch_G.Height * mapFrame.FrameScale * mapHeightScale));
                    using SKBitmap scaledH = new((int)Math.Round(mapFrame.Patch_H.Width * mapFrame.FrameScale * mapWidthScale), (int)Math.Round(mapFrame.Patch_H.Height * mapFrame.FrameScale * mapHeightScale));
                    using SKBitmap scaledI = new((int)Math.Round(mapFrame.Patch_I.Width * mapFrame.FrameScale * mapWidthScale), (int)Math.Round(mapFrame.Patch_I.Height * mapFrame.FrameScale * mapHeightScale));

                    mapFrame.Patch_A.ScalePixels(scaledA, SKFilterQuality.High);
                    mapFrame.Patch_B.ScalePixels(scaledB, SKFilterQuality.High);
                    mapFrame.Patch_C.ScalePixels(scaledC, SKFilterQuality.High);
                    mapFrame.Patch_D.ScalePixels(scaledD, SKFilterQuality.High);
                    mapFrame.Patch_E.ScalePixels(scaledE, SKFilterQuality.High);
                    mapFrame.Patch_F.ScalePixels(scaledF, SKFilterQuality.High);
                    mapFrame.Patch_G.ScalePixels(scaledG, SKFilterQuality.High);
                    mapFrame.Patch_H.ScalePixels(scaledH, SKFilterQuality.High);
                    mapFrame.Patch_I.ScalePixels(scaledI, SKFilterQuality.High);

                    mapFrame.Patch_A.Dispose();
                    mapFrame.Patch_B.Dispose();
                    mapFrame.Patch_C.Dispose();
                    mapFrame.Patch_D.Dispose();
                    mapFrame.Patch_E.Dispose();
                    mapFrame.Patch_F.Dispose();
                    mapFrame.Patch_G.Dispose();
                    mapFrame.Patch_H.Dispose();
                    mapFrame.Patch_I.Dispose();

                    mapFrame.Patch_A = scaledA.Copy();
                    mapFrame.Patch_B = scaledB.Copy();
                    mapFrame.Patch_C = scaledC.Copy();
                    mapFrame.Patch_D = scaledD.Copy();
                    mapFrame.Patch_E = scaledE.Copy();
                    mapFrame.Patch_F = scaledF.Copy();
                    mapFrame.Patch_G = scaledG.Copy();
                    mapFrame.Patch_H = scaledH.Copy();
                    mapFrame.Patch_I = scaledI.Copy();

                }

                Color frameColor = Color.FromArgb(mapFrame.FrameTintOpacity, mapFrame.FrameTint);

                SKPaint framePaint = new()
                {
                    Style = SKPaintStyle.Fill,
                    ColorFilter = SKColorFilter.CreateBlendMode(
                        Extensions.ToSKColor(frameColor),
                        SKBlendMode.Modulate), // combine the tint with the bitmap color
                };

                mapFrame.FramePaint = framePaint;
            }
        }
    }
}
