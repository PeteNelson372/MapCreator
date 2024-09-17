/**************************************************************************************************************************
* Copyright 2024, Peter R. Nelson
*
* This file is part of the MapCreator application. The MapCreator application is intended
* for creating fantasy maps for gaming and world building.
*
* MapCreator is free software: you can redistribute it and/or modify it under the terms
* of the GNU General Public License as published by the Free Software Foundation,
* either version 3 of the License, or (at your option) any later version.
*
* MapCreator is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
* without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
* See the GNU General Public License for more details.
*
* You should have received a copy of the GNU General Public License along with this program.
* The text of the GNU General Public License (GPL) is found in the LICENSE file.
* If the LICENSE file is not present or the text of the GNU GPL is not present in the LICENSE file,
* see https://www.gnu.org/licenses/.
*
* For questions about the MapCreator application or about licensing, please email
* contact@brookmonte.com
*
***************************************************************************************************************************/
using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using DelaunatorSharp;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Drawing.Imaging;
using Point = DelaunatorSharp.Point;

namespace MapCreator
{
    internal class MapGenerator
    {
        // 1. generate random points in the map grid

        // 2. construct Voronoi cells

        // 2.a get centroids or circumcenters of cells

        // 3. island shape - assign height map
        // (this is probably as far as I need to go;
        // from this point just turn the shape into a Skia path by finding the contour of the shape)

        // 4. create biomes (could be done for map coloring; or maybe use heightmap along coastline to color beaches, marshes, etc.?)

        public MapGenerator() { }


        public static void GenerateLandform(GeneratedMapData LandformData)
        {
            List<IPoint> landformPoints = GenerateRandomLandformPoints(0, 0,
                LandformData.MapWidth, LandformData.MapHeight, LandformData.GridSize);

            LandformData.MapPoints = landformPoints;

            GenerateVoronoiCells(LandformData);

            if (LandformData.DistanceFunction != null)
            {
                List<Tuple<int, float, VoronoiCell>> cellsWithHeight = AssignHeightToVoronoiCells(LandformData);

                LandformData.CellsWithHeight = cellsWithHeight;

                Bitmap cellBitmap = GetVoronoiCellsWithHeightBitmap(LandformData);

                // once heights are assigned, subsequent operations are performed on the bitmap,
                // rather than the voronoi cells
                // (except if further processing is done for automatic coloring - maybe future functionality)

                LandformData.OriginalBitmap = cellBitmap;
                LandformData.ScaledBitmap = cellBitmap;

                if (LandformData.ScaledBitmap != null)
                {
                    Bitmap filledB = FillHoles(LandformData.ScaledBitmap);

                    Bitmap? landformBitmap = ExtractLargestBlob(filledB);

                    if (landformBitmap != null)
                    {
                        SKBitmap resizedSKBitmap = new(LandformData.LandformAreaWidth, LandformData.LandformAreaHeight);
                        landformBitmap.ToSKBitmap().ScalePixels(resizedSKBitmap, SKFilterQuality.High);
                        LandformData.ScaledBitmap = resizedSKBitmap.ToBitmap();
                        LandformData.RotatedScaledBitmap = resizedSKBitmap.ToBitmap();
                    }

                    if (LandformData.RotatedScaledBitmap != null)
                    {
                        // convert the bitmap to an 8bpp grayscale image for processing
                        if (LandformData.RotatedScaledBitmap.PixelFormat != PixelFormat.Format8bppIndexed)
                        {
                            Bitmap lf32bpp = new(LandformData.RotatedScaledBitmap.Width, LandformData.RotatedScaledBitmap.Height, PixelFormat.Format32bppArgb);
                            using Graphics g = Graphics.FromImage(lf32bpp);
                            g.Clear(Color.White);
                            g.DrawImage(LandformData.RotatedScaledBitmap, 0, 0);

                            // convert the bitmap to an 8bpp grayscale image for processing
                            Bitmap newB = Grayscale.CommonAlgorithms.BT709.Apply(lf32bpp);
                            LandformData.RotatedScaledBitmap = newB;
                        }

                        SobelEdgeDetector filter = new();
                        // apply the filter
                        Bitmap b = filter.Apply(LandformData.RotatedScaledBitmap);

                        //LandformPictureBox.Image = b;

                        Invert invert = new();
                        Bitmap invertedBitmap = invert.Apply(b);

                        // create filter
                        PointedColorFloodFill f2 = new()
                        {
                            // configure the filter
                            Tolerance = Color.FromArgb(32, 32, 32),
                            FillColor = Color.Black,
                            StartingPoint = new IntPoint(LandformData.RotatedScaledBitmap.Width / 2, LandformData.RotatedScaledBitmap.Height / 2)
                        };

                        // apply the filter
                        f2.ApplyInPlace(invertedBitmap);

                        SKPath contourPath = MapDrawingMethods.GetContourPathFromBitmap(invertedBitmap, out List<SKPoint> contourPoints);

                        if (contourPath.PointCount == 0)
                        {
                            MessageBox.Show("Could not get landform path", "Operation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                            return;
                        }

                        // to smooth the contour path, a smoothed bitmap has to be painted using the path
                        // (using a path effect, as in the DrawLandformBoundary method), then a new
                        // contour path generated from the smoothed bitmap using the SobelEdgeDetector filter,
                        // the PointedColorFloodFill, and the GetContourPathFromBitmap() method;
                        // the bitmap should be scaled and rotated before generating the new contour path
                        float variation = LandformData.Variation;
                        float smoothing = LandformData.Smoothing;
                        float segmentLength = LandformData.SegmentLength;

                        using SKPathEffect discrete = SKPathEffect.CreateDiscrete(segmentLength, variation);
                        using SKPathEffect pe = SKPathEffect.CreateCompose(SKPathEffect.CreateCorner(smoothing), discrete);

                        using SKPaint contourPaint = new()
                        {
                            Style = SKPaintStyle.Stroke,
                            StrokeWidth = 2,
                            Color = SKColors.Black,
                            PathEffect = pe,
                        };

                        using SKBitmap skb = new(LandformData.LandformAreaWidth, LandformData.LandformAreaHeight);
                        using SKCanvas canvas = new(skb);

                        canvas.DrawPath(contourPath, contourPaint);

                        Bitmap pathBitmap = skb.ToBitmap();

                        // convert the bitmap to an 8bpp grayscale image for processing
                        if (pathBitmap.PixelFormat != PixelFormat.Format8bppIndexed)
                        {
                            Bitmap lf32bpp = new(pathBitmap.Width, pathBitmap.Height, PixelFormat.Format32bppArgb);
                            using Graphics g = Graphics.FromImage(lf32bpp);
                            g.Clear(Color.White);
                            g.DrawImage(pathBitmap, 0, 0);

                            // convert the bitmap to an 8bpp grayscale image for processing
                            Bitmap newB = Grayscale.CommonAlgorithms.BT709.Apply(lf32bpp);
                            pathBitmap = newB;
                        }

                        PointedColorFloodFill f3 = new()
                        {
                            // configure the filter
                            Tolerance = Color.FromArgb(32, 32, 32),
                            FillColor = Color.Black,
                            StartingPoint = new IntPoint(pathBitmap.Width / 2, pathBitmap.Height / 2)
                        };

                        // apply the filter
                        f3.ApplyInPlace(pathBitmap);

                        Bitmap lf32bpp2 = new(pathBitmap.Width, pathBitmap.Height, PixelFormat.Format32bppArgb);
                        using Graphics g2 = Graphics.FromImage(lf32bpp2);
                        g2.Clear(Color.White);
                        g2.DrawImage(pathBitmap, 0, 0);

                        MapDrawingMethods.FlattenBitmapColors(ref lf32bpp2);

                        SKPath newContourPath = MapDrawingMethods.GetContourPathFromBitmap(lf32bpp2, out List<SKPoint> newContourPoints);

                        LandformData.LandformContourPath = newContourPath;
                    }
                }
            }
        }


        public static List<IPoint> GenerateRandomLandformPoints(int x, int y, int width, int height, int gridSize)
        {
            List<IPoint> mapOutlinePoints = SeedMapOutlinePoints(x, y, width, height, gridSize);
            return mapOutlinePoints;
        }

        internal static void GenerateVoronoiCells(GeneratedMapData landformData)
        {
            Delaunator delaunator = new([.. landformData.MapPoints]);
            landformData.MapDelaunator = delaunator;
        }

        internal static Bitmap FillHoles(Bitmap landformBitmap)
        {
            // convert the bitmap to an 8bpp grayscale image for processing
            if (landformBitmap.PixelFormat != PixelFormat.Format8bppIndexed)
            {
                // convert the bitmap to an 8bpp grayscale image for processing
                Bitmap newB = Grayscale.CommonAlgorithms.BT709.Apply(landformBitmap);
                landformBitmap = newB;
            }

            // invert the bitmap colors white -> black; black -> white
            // FillHoles filter looks for black holes in white background
            // landformBitmap has white holes in black background, so
            // invert is required
            Invert invert = new();
            using Bitmap invertedBitmap = invert.Apply(landformBitmap);

            // fill holes in the landform (black areas surrounded by white)
            FillHoles fillHolesFilter = new()
            {
                MaxHoleWidth = 200,     // 200 pixels is maximum size of hole filled
                MaxHoleHeight = 200,
                CoupledSizeFiltering = false
            };

            Bitmap filledBitmap = fillHolesFilter.Apply(invertedBitmap);

            // re-invert the colors to restore to original
            Bitmap filledInvertedBitmap = invert.Apply(filledBitmap);

            return filledInvertedBitmap;
        }

        internal static Bitmap? ExtractLargestBlob(Bitmap b)
        {
            if (b.PixelFormat != PixelFormat.Format8bppIndexed)
            {
                // convert the bitmap to an 8bpp grayscale image for processing
                b = Grayscale.CommonAlgorithms.BT709.Apply(b);
            }

            // invert the bitmap colors white -> black; black -> white
            Invert invert = new();
            using Bitmap invertedBitmap = invert.Apply(b);

            // extract the largest blob; this will be the landform to be created
            // create an instance of blob counter algorithm
            BlobCounterBase bc = new BlobCounter
            {
                // set filtering options
                FilterBlobs = true,
                MinWidth = 5,
                MinHeight = 5,

                // set ordering options
                ObjectsOrder = ObjectsOrder.Size
            };

            // process binary image
            bc.ProcessImage(invertedBitmap);

            //bc.ProcessImage(b);

            Blob[] blobs = bc.GetObjectsInformation();

            // extract the biggest blob
            if (blobs.Length > 0)
            {
                //bc.ExtractBlobsImage(b, blobs[0], true);
                bc.ExtractBlobsImage(invertedBitmap, blobs[0], true);

                Blob biggestBlob = blobs[0];
                Bitmap managedImage = biggestBlob.Image.ToManagedImage();

                // re-invert the colors
                Bitmap invertedBlobBitmap = invert.Apply(managedImage);

                //return managedImage;
                return invertedBlobBitmap;
            }

            return null;
        }

        public static Bitmap GetPointsBitmap(int width, int height, List<IPoint> mapOutlinePoints)
        {
            Bitmap b = new(width, height);
            using Graphics g = Graphics.FromImage(b);
            g.Clear(Color.White);

            Brush brush = new SolidBrush(Color.Black);

            foreach (Point p in mapOutlinePoints.Select(v => (Point)v))
            {
                g.FillEllipse(brush, new Rectangle((int)p.X, (int)p.Y, 2, 2));
            }

            return b;
        }

        public static Bitmap? GetVoronoiCellBitmap(GeneratedMapData mapData)
        {
            Bitmap b = new(mapData.MapWidth, mapData.MapHeight);
            using Graphics g = Graphics.FromImage(b);
            g.Clear(Color.White);
            Pen pen = new(Color.Black, 2);

            if (mapData.MapDelaunator != null)
            {
                IEnumerable<IVoronoiCell> cells = mapData.MapDelaunator.GetVoronoiCellsBasedOnCentroids();

                foreach (VoronoiCell cell in cells.Select(v => (VoronoiCell)v))
                {
                    List<PointF> cellPoints = [];

                    foreach (Point p in cell.Points.Select(v => (Point)v))
                    {
                        System.Drawing.Point dp = new((int)p.X, (int)p.Y);
                        cellPoints.Add(dp);
                    }

                    if (cellPoints.Count > 2)
                    {
                        g.DrawPolygon(pen, cellPoints.ToArray());
                    }
                }

                return b;
            }

            return null;

        }

        public static Bitmap GetVoronoiCellsWithHeightBitmap(GeneratedMapData mapData)
        {
            Bitmap b = new(mapData.MapWidth, mapData.MapHeight);
            using Graphics g = Graphics.FromImage(b);
            g.Clear(Color.White);

            using Brush blackBrush = new SolidBrush(Color.Black);
            using Brush whiteBrush = new SolidBrush(Color.White);

            foreach (Tuple<int, float, VoronoiCell> t in mapData.CellsWithHeight)
            {
                List<PointF> cellPoints = [];

                foreach (Point p in t.Item3.Points.Select(v => (Point)v))
                {
                    System.Drawing.Point dp = new((int)p.X, (int)p.Y);
                    cellPoints.Add(dp);
                }

                if (cellPoints.Count > 2)
                {
                    if (t.Item2 < mapData.SeaLevel)
                    {
                        g.FillPolygon(whiteBrush, cellPoints.ToArray());
                    }
                    else
                    {
                        g.FillPolygon(blackBrush, cellPoints.ToArray());
                    }
                }
            }

            return b;
        }

        public static List<Tuple<int, float, VoronoiCell>> AssignHeightToVoronoiCells(GeneratedMapData mapData)
        {
            List<Tuple<int, float, VoronoiCell>> cellsWithHeight = [];

            float[,] noiseMap = SimplexNoise.Noise.Calc2D(mapData.MapWidth, mapData.MapHeight, mapData.NoiseScale);

            if (mapData.MapDelaunator != null)
            {
                IEnumerable<IVoronoiCell> cells = mapData.MapDelaunator.GetVoronoiCellsBasedOnCentroids();

                List<IVoronoiCell> orderedCells = [.. cells.OrderBy(x => x.Index)];

                foreach (VoronoiCell cell in orderedCells.Select(v => (VoronoiCell)v))
                {
                    // figure out height of cell
                    if (cell.Points.Length > 2)
                    {
                        Point centroid = GetCellCentroid(cell);

                        if (centroid.X >= 0
                            && centroid.X < mapData.MapWidth
                            && centroid.Y >= 0
                            && centroid.Y < mapData.MapHeight)
                        {
                            SKPoint mapCenter = new(mapData.MapWidth / 2.0F, mapData.MapHeight / 2.0F);
                            SKPoint centroidPoint = new((float)centroid.X, (float)centroid.Y);

                            float pointDistance = SKPoint.Distance(mapCenter, centroidPoint);

                            double nx = 2 * (pointDistance / mapData.MapWidth) - 1;
                            double ny = 2 * (pointDistance / mapData.MapHeight) - 1;

                            double noiseHeight = noiseMap[(int)centroid.X, (int)centroid.Y] / 256.0;
                            float distance = 0.0F;

                            switch (mapData.DistanceFunction)
                            {
                                case "Distance Squared":
                                    distance = (float)(nx * nx + ny * ny);
                                    break;
                                case "Euclidean Squared":
                                    distance = (float)Math.Min(1, (nx * nx + ny * ny) / Math.Sqrt(2.0F));
                                    break;
                                case "Square Bump":
                                    distance = (float)(1.0 - (1.0 - nx * nx) * (1.0 - ny * ny));
                                    break;
                                case "Hyperboloid":
                                    double k = 0.2;
                                    distance = (float)((Math.Sqrt(nx * nx + ny * ny + k * k) - k) / (Math.Sqrt(1.0 + k * k) - k));
                                    break;
                                case "Trigonometric Product":                                    
                                    distance = (float)(Math.Sin(nx * Math.PI) * Math.Sin(ny * Math.PI));
                                    break;
                                case "Squircle":
                                    distance = (float)(Math.Sqrt(Math.Pow(nx, 4) + Math.Pow(ny, 4)));
                                    break;
                            }

                            noiseHeight = double.Lerp(noiseHeight, distance, mapData.InterpolationWeight);

                            Tuple<int, float, VoronoiCell> t = new(cell.Index, (float)noiseHeight, cell);
                            cellsWithHeight.Add(t);
                        }
                    }
                }
            }

            return cellsWithHeight;
        }

        private static Point GetCellCentroid(VoronoiCell cell)
        {
            double xCentroid = 0;
            double yCentroid = 0;

            foreach (Point p in cell.Points)
            {
                xCentroid += p.X;
                yCentroid += p.Y;
            }

            xCentroid = xCentroid / cell.Points.Length;
            yCentroid = yCentroid / cell.Points.Length;

            return new Point(xCentroid, yCentroid);
        }

        private static double smin(double a, double b, double k)
        {
            //quadratic polynomial smooth minimum

            k *= 4.0;
            double h = Math.Max(k - Math.Abs(a - b), 0.0) / k;
            return Math.Min(a, b) - h * h * k * (1.0 / 4.0);
        }

        private static List<IPoint> SeedMapOutlinePoints(int x, int y, int width, int height, int gridSize)
        {
            List<IPoint> mapPoints = [];

            for (int pointx = x; pointx < x + width; pointx += gridSize)
            {
                for (int pointy = y; pointy < y + height; pointy += gridSize)
                {
                    Point p = new((float)(pointx + gridSize * (Random.Shared.NextDouble() - Random.Shared.NextDouble())), (float)(pointy + gridSize * (Random.Shared.NextDouble() - Random.Shared.NextDouble())));
                    mapPoints.Add(p);
                }
            }

            return mapPoints;
        }

        private static void SaveDebugPointsBitmap(int width, int height, List<IPoint> mapOutlinePoints)
        {
            using Bitmap b = new(width, height);
            using Graphics g = Graphics.FromImage(b);
            g.Clear(Color.White);

            Brush brush = new SolidBrush(Color.Black);

            foreach (Point p in mapOutlinePoints.Select(v => (Point)v))
            {
                g.FillEllipse(brush, new Rectangle((int)p.X, (int)p.Y, 2, 2));
            }

            //b.Save("C:\\Users\\Pete Nelson\\OneDrive\\Desktop\\mapPoints.bmp");
        }

        private static void SaveDebugTriangeEdgeBitmap(int width, int height, GeneratedMapData mapData)
        {
            using Bitmap b = new(width, height);
            using Graphics g = Graphics.FromImage(b);
            g.Clear(Color.White);

            Pen pen = new(Color.Black, 2);

            if (mapData.MapDelaunator != null)
            {

                for (int e = 0; e < mapData.MapDelaunator.Triangles.Length; e++)
                {
                    if (e > mapData.MapDelaunator.Halfedges[e])
                    {
                        Point p = (Point)mapData.MapPoints[mapData.MapDelaunator.Triangles[e]];
                        Point q = (Point)mapData.MapPoints[mapData.MapDelaunator.Triangles[NextHalfedge(e)]];

                        System.Drawing.Point dp = new System.Drawing.Point((int)p.X, (int)p.Y);
                        System.Drawing.Point dq = new System.Drawing.Point((int)q.X, (int)q.Y);

                        g.DrawLine(pen, dp, dq);
                    }
                }

                b.Save("C:\\Users\\Pete Nelson\\OneDrive\\Desktop\\cellBoundaries.bmp");
            }
        }

        private static void SaveDebugVoronoiCellBitmap(int width, int height, GeneratedMapData mapData)
        {
            using Bitmap b = new(width, height);
            using Graphics g = Graphics.FromImage(b);
            g.Clear(Color.White);
            Pen pen = new(Color.Black, 2);

            if (mapData.MapDelaunator != null)
            {
                IEnumerable<IVoronoiCell> cells = mapData.MapDelaunator.GetVoronoiCellsBasedOnCentroids();

                foreach (VoronoiCell cell in cells.Select(v => (VoronoiCell)v))
                {
                    List<PointF> cellPoints = [];

                    foreach (Point p in cell.Points)
                    {
                        System.Drawing.Point dp = new((int)p.X, (int)p.Y);
                        cellPoints.Add(dp);
                    }

                    if (cellPoints.Count > 2)
                    {
                        g.DrawPolygon(pen, cellPoints.ToArray());
                    }
                }

                //b.Save("C:\\Users\\Pete Nelson\\OneDrive\\Desktop\\voronoiCells.bmp");
            }

        }

        private static void SaveDebugVoronoiCellWithHeightBitmap(GeneratedMapData mapData, List<Tuple<int, float, VoronoiCell>> cellsWithHeight)
        {
            using Bitmap b = new(mapData.LandformAreaWidth, mapData.LandformAreaHeight);
            using Graphics g = Graphics.FromImage(b);
            g.Clear(Color.White);

            Brush blackBrush = new SolidBrush(Color.Black);
            Brush whiteBrush = new SolidBrush(Color.AntiqueWhite);

            foreach (Tuple<int, float, VoronoiCell> t in cellsWithHeight)
            {
                List<PointF> cellPoints = [];

                foreach (Point p in t.Item3.Points)
                {
                    System.Drawing.Point dp = new((int)p.X, (int)p.Y);
                    cellPoints.Add(dp);
                }

                if (cellPoints.Count > 2)
                {
                    if (t.Item2 < mapData.SeaLevel)
                    {
                        g.FillPolygon(whiteBrush, cellPoints.ToArray());
                    }
                    else
                    {
                        g.FillPolygon(blackBrush, cellPoints.ToArray());
                    }
                }
            }

            //b.Save("C:\\Users\\Pete Nelson\\OneDrive\\Desktop\\voronoiCellsWithHeight.bmp");
        }

        private static int NextHalfedge(int e)
        {
            return (e % 3 == 2) ? e - 2 : e + 1;
        }

        private static int PreviousHalfedge(int e)
        {
            return (e % 3 == 0) ? e + 2 : e - 1;
        }


        //***********************************************************************************************

        public static MapLandformType2? GenerateRandomLandform(MapCreatorMap map, SKRect? selectedLandformArea)
        {
            const int MAX_TRIES = 20;

            if (map != null)
            {
                int top = (int)((selectedLandformArea == null) ? 0 : ((SKRect)selectedLandformArea).Top);
                int left = (int)((selectedLandformArea == null) ? 0 : ((SKRect)selectedLandformArea).Left);
                int width = (int)((selectedLandformArea == null) ? map.MapWidth : ((SKRect)selectedLandformArea).Width);
                int height = (int)((selectedLandformArea == null) ? map.MapHeight : ((SKRect)selectedLandformArea).Height);

                GeneratedMapData LandformData = new()
                {
                    LandformLocationTop = top,
                    LandformLocationLeft = left,
                    LandformAreaWidth = width,
                    LandformAreaHeight = height,
                    MapWidth = map.MapWidth,
                    MapHeight = map.MapHeight,
                    GridSize = 20
                };

                int tryCount = 0;
                MapLandformType2 NewLandform = new();

                while (LandformData.LandformContourPath == null
                    || LandformData.LandformContourPath.PointCount < 100
                    || LandformData.RotatedScaledBitmap == null)
                {
                    LandformData = new()
                    {
                        LandformLocationTop = top,
                        LandformLocationLeft = left,
                        LandformAreaWidth = width,
                        LandformAreaHeight = height,
                        MapWidth = map.MapWidth,
                        MapHeight = map.MapHeight,
                        GridSize = 20,
                        SeaLevel = 0.5F
                    };

                    float noiseScale = (float)Random.Shared.NextDouble();
                    LandformData.NoiseScale = noiseScale;

                    float interpolationWeight = (float)Random.Shared.NextDouble();
                    LandformData.InterpolationWeight = interpolationWeight;

                    LandformData.DistanceFunction = "Distance Squared";

                    LandformData.Variation = 50;
                    LandformData.Smoothing = 5;
                    LandformData.SegmentLength = 20;

                    GenerateLandform(LandformData);

                    tryCount++;

                    if (tryCount > MAX_TRIES) break;
                }

                if (tryCount < MAX_TRIES)
                {
                    if (LandformData.LandformContourPath != null && LandformData.LandformContourPath.PointCount > 0 && map != null)
                    {
                        NewLandform.GenMapData = LandformData;
                        NewLandform.X = LandformData.LandformLocationLeft;
                        NewLandform.Y = LandformData.LandformLocationTop;
                        NewLandform.Width = LandformData.LandformAreaWidth;
                        NewLandform.Height = LandformData.LandformAreaHeight;

                        SKMatrix translateMatrix = SKMatrix.CreateTranslation(LandformData.LandformLocationLeft, LandformData.LandformLocationTop);

                        LandformData.LandformContourPath.Transform(translateMatrix);

                        NewLandform.LandformPath = LandformData.LandformContourPath;

                        LandformType2Methods.CreateType2LandformPaths(map, NewLandform);

                        return NewLandform;
                    }
                }
            }

            return null;
        }
    }
}
