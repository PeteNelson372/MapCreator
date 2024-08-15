using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using DelaunatorSharp;
using SkiaSharp;
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
            List<IPoint> landformPoints = GenerateRandomLandformPoints(LandformData.MapWidth, LandformData.MapHeight, LandformData.GridSize);

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
                        Bitmap newBitmap = new(LandformData.MapWidth, LandformData.MapHeight);
                        Graphics g = Graphics.FromImage(newBitmap);
                        g.DrawImageUnscaled(landformBitmap, 0, 0);

                        LandformData.ScaledBitmap = newBitmap;
                        LandformData.RotatedScaledBitmap = newBitmap;

                        g.Dispose();
                    }


                    if (LandformData.RotatedScaledBitmap != null)
                    {
                        // convert the bitmap to an 8bpp grayscale image for processing
                        if (LandformData.RotatedScaledBitmap.PixelFormat != PixelFormat.Format8bppIndexed)
                        {
                            // convert the bitmap to an 8bpp grayscale image for processing
                            Bitmap newB = Grayscale.CommonAlgorithms.BT709.Apply(LandformData.RotatedScaledBitmap);
                            LandformData.RotatedScaledBitmap = newB;
                        }

                        SobelEdgeDetector filter = new();
                        // apply the filter
                        Bitmap b = filter.Apply(LandformData.RotatedScaledBitmap);

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

                        Bitmap lf32bpp = new(invertedBitmap.Width, invertedBitmap.Height, PixelFormat.Format32bppArgb);
                        using Graphics g = Graphics.FromImage(lf32bpp);
                        g.Clear(Color.White);
                        g.DrawImage(invertedBitmap, 0, 0);

                        SKPath contourPath = MapDrawingMethods.GetContourPathFromBitmap(lf32bpp, out List<SKPoint> contourPoints);
                        LandformData.LandformContourPath = contourPath;
                    }
                }
            }
        }


        public static List<IPoint> GenerateRandomLandformPoints(int width, int height, int gridSize)
        {
            List<IPoint> mapOutlinePoints = SeedMapOutlinePoints(width, height, gridSize);
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

            using Bitmap filledBitmap = fillHolesFilter.Apply(invertedBitmap);

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
            Blob[] blobs = bc.GetObjectsInformation();

            // extract the biggest blob
            if (blobs.Length > 0)
            {
                bc.ExtractBlobsImage(invertedBitmap, blobs[0], true);

                Blob biggestBlob = blobs[0];
                Bitmap managedImage = biggestBlob.Image.ToManagedImage();

                // re-invert the colors
                Bitmap invertedBlobBitmap = invert.Apply(managedImage);

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

                return b;
            }

            return null;

        }

        public static Bitmap GetVoronoiCellsWithHeightBitmap(GeneratedMapData mapData)
        {
            Bitmap b = new(mapData.MapWidth, mapData.MapHeight);
            using Graphics g = Graphics.FromImage(b);
            g.Clear(Color.White);

            Brush blackBrush = new SolidBrush(Color.Black);
            Brush whiteBrush = new SolidBrush(Color.White);

            foreach (Tuple<int, float, VoronoiCell> t in mapData.CellsWithHeight)
            {
                List<PointF> cellPoints = [];

                foreach (Point p in t.Item3.Points)
                {
                    System.Drawing.Point dp = new((int)p.X, (int)p.Y);
                    cellPoints.Add(dp);
                }

                if (cellPoints.Count > 2)
                {
                    if (t.Item2 > mapData.SeaLevel)
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

                        if (centroid.X >= 0 && centroid.X < mapData.MapWidth && centroid.Y >= 0 && centroid.Y < mapData.MapHeight)
                        {
                            double nx = 2 * centroid.X / mapData.MapWidth - 1;
                            double ny = 2 * centroid.Y / mapData.MapHeight - 1;

                            double noiseHeight = noiseMap[(int)centroid.X, (int)centroid.Y] / 256.0F;
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

        private static List<IPoint> SeedMapOutlinePoints(int width, int height, int gridSize)
        {
            List<IPoint> mapPoints = [];

            for (int x = 0; x < width; x += gridSize)
            {
                for (int y = 0; y < height; y += gridSize)
                {
                    Point p = new((float)(x + gridSize * (Random.Shared.NextDouble() - Random.Shared.NextDouble())), (float)(y + gridSize * (Random.Shared.NextDouble() - Random.Shared.NextDouble())));
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

            b.Save("C:\\Users\\Pete Nelson\\OneDrive\\Desktop\\mapPoints.bmp");
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

                b.Save("C:\\Users\\Pete Nelson\\OneDrive\\Desktop\\voronoiCells.bmp");
            }

        }

        private static void SaveDebugVoronoiCellWithHeightBitmap(GeneratedMapData mapData, List<Tuple<int, float, VoronoiCell>> cellsWithHeight)
        {
            using Bitmap b = new(mapData.MapWidth, mapData.MapHeight);
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

            b.Save("C:\\Users\\Pete Nelson\\OneDrive\\Desktop\\voronoiCellsWithHeight.bmp");
        }

        private static int NextHalfedge(int e)
        {
            return (e % 3 == 2) ? e - 2 : e + 1;
        }

        private static int PreviousHalfedge(int e)
        {
            return (e % 3 == 0) ? e + 2 : e - 1;
        }
    }
}
