using AForge;
using AForge.Imaging.Filters;
using DelaunatorSharp;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Drawing;
using System.Drawing.Imaging;

namespace MapCreator
{
    public partial class GenerateLandform : Form
    {
        public event EventHandler LandformGenerated;

        private MapCreatorMap? Map {  get; set; }
        private MapLandformType2? NewLandform {  get; set; }

        private GeneratedLandformTypeEnum SelectedLandformType = GeneratedLandformTypeEnum.Random;

        private SKRect? SelectedLandformArea = null;

        internal List<GeneratedMapData> LandformDataList { get; set; } = [];

        internal GeneratedMapData LandformData { get; set; } = new();


#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public GenerateLandform()
        {
            InitializeComponent();

            GenLandformSplitContainer.SplitterDistance = 530;
        }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        public void Initialize(MapCreatorMap map, MapLandformType2 newLandform, SKRect? selectedLandformArea)
        {
            Map = map;
            NewLandform = newLandform;
            SelectedLandformArea = null;

            LandformData.MapWidth = map.MapWidth;
            LandformData.MapHeight = map.MapHeight;

            if (selectedLandformArea != null)
            {
                LandformData.LandformLocationLeft = (int)((SKRect)selectedLandformArea).Left;
                LandformData.LandformLocationTop = (int)((SKRect)selectedLandformArea).Top;
                LandformData.LandformAreaWidth = (int)((SKRect)selectedLandformArea).Width;
                LandformData.LandformAreaHeight = (int)((SKRect)selectedLandformArea).Height;

                SelectedLandformArea = selectedLandformArea;
            }
            else
            {
                LandformData.LandformLocationLeft = 0;
                LandformData.LandformLocationTop = 0;
                LandformData.LandformAreaWidth = Map.MapWidth;
                LandformData.LandformAreaHeight = Map.MapHeight;
            }

            LandformData.GridSize = (int)GridSizeUpDown.Value;
            LandformData.SeaLevel = (float)SeaLevelUpDown.Value;
        }

        protected virtual void OnLandformGenerated(EventArgs e)
        {
            LandformGenerated?.Invoke(this, e);
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void GeneratePointsButton_Click(object sender, EventArgs e)
        {
            List<IPoint> landformPoints = MapGenerator.GenerateRandomLandformPoints(0, 0,
                LandformData.MapWidth, LandformData.MapHeight, LandformData.GridSize);

            LandformData.MapPoints = landformPoints;

            Bitmap b = MapGenerator.GetPointsBitmap(LandformData.MapWidth, LandformData.MapHeight, LandformData.MapPoints);

            LandformPictureBox.Image = b;

        }

        private void GenerateCellsButton_Click(object sender, EventArgs e)
        {
            MapGenerator.GenerateVoronoiCells(LandformData);

            Bitmap? b = MapGenerator.GetVoronoiCellBitmap(LandformData);

            if (b != null)
            {
                LandformPictureBox.Image = b;
            }
        }

        private void AssignHeightButton_Click(object sender, EventArgs e)
        {
            float noiseScale = (float)NoiseScaleUpDown.Value;
            float interpolationWeight = (float)LerpWeightUpDown.Value;
            string distanceFunction = DistanceFunctionUpDown.Text;

            LandformData.NoiseScale = noiseScale;
            LandformData.InterpolationWeight = interpolationWeight;
            LandformData.DistanceFunction = distanceFunction;

            List<Tuple<int, float, VoronoiCell>> cellsWithHeight = MapGenerator.AssignHeightToVoronoiCells(LandformData);
            LandformData.CellsWithHeight = cellsWithHeight;

            Bitmap b = MapGenerator.GetVoronoiCellsWithHeightBitmap(LandformData);
            // once heights are assigned, subsequent operations are performed on the bitmap,
            // rather than the voronoi cells
            // (except if further processing is done for automatic coloring - maybe future functionality)

            LandformData.OriginalBitmap = b;
            LandformData.ScaledBitmap = b;

            LandformPictureBox.Image = b;
        }

        private void RemoveDisconnectedCellsButton_Click(object sender, EventArgs e)
        {
            LandformPictureBox.Image = null;
            LandformPictureBox.Refresh();

            if (LandformData.ScaledBitmap != null)
            {
                Bitmap b = LandformData.ScaledBitmap;

                Bitmap filledB = MapGenerator.FillHoles(LandformData.ScaledBitmap);

                //LandformPictureBox.Image = filledB;
                
                Bitmap? landformBitmap = MapGenerator.ExtractLargestBlob(filledB);

                if (landformBitmap != null)
                {
                    //LandformPictureBox.Image = landformBitmap;

                    SKBitmap resizedSKBitmap = new(LandformData.LandformAreaWidth, LandformData.LandformAreaHeight);
                    landformBitmap.ToSKBitmap().ScalePixels(resizedSKBitmap, SKFilterQuality.High);

                    LandformPictureBox.Image = resizedSKBitmap.ToBitmap();

                    LandformData.ScaledBitmap = resizedSKBitmap.ToBitmap();
                    LandformData.RotatedScaledBitmap = resizedSKBitmap.ToBitmap();
                }
            }
        }

        private void CreateBoundaryButton_Click(object sender, EventArgs e)
        {
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
                float variation = VariationTrack.Value;
                float smoothing = SmoothingTrack.Value;
                float segmentLength = RoughnessTrack.Value;

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

                //LandformPictureBox.Image = pathBitmap;

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

                //LandformPictureBox.Image = lf32bpp2;

                //lf32bpp2.Save("C:\\Users\\Pete Nelson\\OneDrive\\Desktop\\pathbitmap.bmp");

                SKPath newContourPath = MapDrawingMethods.GetContourPathFromBitmap(lf32bpp2, out List<SKPoint> newContourPoints);

                LandformData.LandformContourPath = newContourPath;

                DrawLandformBoundary();
            }
        }

        private void DrawLandformBoundary(bool scaleToFit = false)
        {
            if (LandformData.LandformContourPath != null && LandformData.RotatedScaledBitmap != null)
            {
                float variation = VariationTrack.Value;
                float smoothing = SmoothingTrack.Value;
                float segmentLength = RoughnessTrack.Value;

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

                canvas.DrawPath(LandformData.LandformContourPath, contourPaint);

                Bitmap b = skb.ToBitmap();

                if (scaleToFit)
                {
                    float horizontalScalingFactor = (float)LandformPictureBox.Width / LandformData.LandformAreaWidth;
                    float verticalScalingFactor = (float)LandformPictureBox.Height / LandformData.LandformAreaHeight;

                    Bitmap resized = new Bitmap(b, new Size((int)(b.Width * horizontalScalingFactor), (int)(b.Height * verticalScalingFactor)));

                    b.Dispose();
                    b = new(resized);
                }

                LandformPictureBox.Image = b;

                LandformPictureBox.Refresh();
            }
        }

        private void GridSizeUpDown_ValueChanged(object sender, EventArgs e)
        {
            LandformData.GridSize = (int)GridSizeUpDown.Value;
        }

        private void SeaLevelUpDown_ValueChanged(object sender, EventArgs e)
        {
            LandformData.SeaLevel = (float)SeaLevelUpDown.Value;
        }

        private void SmoothingTrack_ValueChanged(object sender, EventArgs e)
        {
            DrawLandformBoundary();
        }

        private void VariationTrack_ValueChanged(object sender, EventArgs e)
        {
            DrawLandformBoundary();
        }

        private void RoughnessTrack_ValueChanged(object sender, EventArgs e)
        {
            DrawLandformBoundary();
        }

        private void ScaleTrack_ValueChanged(object sender, EventArgs e)
        {
            if (LandformData.ScaledBitmap != null && LandformData.OriginalBitmap != null)
            {
                // scale the path
                //      scale the bitmap and draw it to a in-memory bitmap,
                //      get a new contour path
                //      re-draw the new contour path
                float newWidth = LandformData.OriginalBitmap.Width * (ScaleTrack.Value / 100.0F);
                float newHeight = LandformData.OriginalBitmap.Height * (ScaleTrack.Value / 100.0F);

                Bitmap resizedImage = new((int)newWidth, (int)newHeight);
                using Graphics g = Graphics.FromImage(resizedImage);

                float horizontalScalingFactor = newWidth / LandformData.OriginalBitmap.Width;
                float verticalScalingFactor = newHeight / LandformData.OriginalBitmap.Height;

                g.ScaleTransform(horizontalScalingFactor, verticalScalingFactor);
                g.DrawImage(LandformData.OriginalBitmap, 0, 0);

                Bitmap b = MapGenerator.FillHoles(resizedImage);
                Bitmap? landformBitmap = MapGenerator.ExtractLargestBlob(b);

                if (landformBitmap != null)
                {
                    Bitmap newBitmap = new(landformBitmap.Width, landformBitmap.Height);
                    using Graphics g2 = Graphics.FromImage(newBitmap);
                    g2.DrawImageUnscaled(landformBitmap, 0, 0);

                    LandformData.ScaledBitmap = newBitmap;

                    if (LandformData.ScaledBitmap != null && LandformData.OriginalBitmap != null)
                    {
                        float rotation = RotationTrack.Value;

                        using Bitmap rotatedImage = MapDrawingMethods.RotateBitmap(LandformData.ScaledBitmap.ToSKBitmap(), rotation, false).ToBitmap();

                        Bitmap lf32bpp = new(rotatedImage.Width, rotatedImage.Height, PixelFormat.Format32bppArgb);
                        using Graphics g3 = Graphics.FromImage(lf32bpp);
                        g3.Clear(Color.White);
                        g3.DrawImage(rotatedImage, 0, 0);

                        if (lf32bpp != null)
                        {
                            LandformData.RotatedScaledBitmap = lf32bpp;

                            LandformPictureBox.Image = lf32bpp;
                            LandformPictureBox.Refresh();
                        }
                    }

                }
            }
        }

        private void RotationTrack_ValueChanged(object sender, EventArgs e)
        {
            // rotate the path
            //      rotate the bitmap and draw it to a in-memory bitmap,
            //      get a new contour path
            //      re-draw the new contour path

            if (LandformData.ScaledBitmap != null && LandformData.OriginalBitmap != null)
            {
                float rotation = RotationTrack.Value;

                using Bitmap rotatedImage = MapDrawingMethods.RotateBitmap(LandformData.ScaledBitmap.ToSKBitmap(), rotation, false).ToBitmap();

                Bitmap lf32bpp = new(rotatedImage.Width, rotatedImage.Height, PixelFormat.Format32bppArgb);
                using Graphics g = Graphics.FromImage(lf32bpp);
                g.Clear(Color.White);
                g.DrawImage(rotatedImage, 0, 0);

                if (lf32bpp != null)
                {
                    LandformData.RotatedScaledBitmap = lf32bpp;

                    LandformPictureBox.Image = lf32bpp;
                    LandformPictureBox.Refresh();
                }
            }
        }

        private void ShowHideAdvancedButton_Click(object sender, EventArgs e)
        {
            if (GenLandformSplitContainer.SplitterDistance < GenLandformSplitContainer.Size.Height / 2)
            {
                GenLandformSplitContainer.SplitterDistance = GenLandformSplitContainer.Size.Height - GenLandformSplitContainer.Panel2MinSize;
            }
            else
            {
                GenLandformSplitContainer.SplitterDistance = GenLandformSplitContainer.Panel1MinSize;
            }
        }

        private void GenerateLandformButton_Click(object sender, EventArgs e)
        {
            switch (SelectedLandformType)
            {
                case GeneratedLandformTypeEnum.Random:
                    GenerateRandomLandform();
                    break;
                case GeneratedLandformTypeEnum.Continent:
                    // TODO
                    break;

                case GeneratedLandformTypeEnum.Atoll:
                    // TODO
                    break;

                case GeneratedLandformTypeEnum.Archipelago:
                    // TODO
                    break;

                case GeneratedLandformTypeEnum.World:
                    // TODO
                    break;

                case GeneratedLandformTypeEnum.Equirectangular:
                    // TODO
                    break;

                default:
                    GenerateRandomLandform();
                    break;
            }
        }

        private void GenerateRandomLandform()
        {
            const int MAX_TRIES = 20;

            if (Map != null)
            {
                int top = (int)((SelectedLandformArea == null) ? 0 : ((SKRect)SelectedLandformArea).Top);
                int left = (int)((SelectedLandformArea == null) ? 0 : ((SKRect)SelectedLandformArea).Left);
                int width = (int)((SelectedLandformArea == null) ? Map.MapWidth : ((SKRect)SelectedLandformArea).Width);
                int height = (int)((SelectedLandformArea == null) ? Map.MapHeight : ((SKRect)SelectedLandformArea).Height);

                LandformData = new()
                {
                    LandformLocationTop = top,
                    LandformLocationLeft = left,
                    LandformAreaWidth = width,
                    LandformAreaHeight = height,
                    MapWidth = Map.MapWidth,
                    MapHeight = Map.MapHeight,
                    GridSize = 20
                };

                int tryCount = 0;

                GenerationStatusLabel.Text = "Generating landform...";
                GenerationStatusLabel.Refresh();

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
                        MapWidth = Map.MapWidth,
                        MapHeight = Map.MapHeight,
                        GridSize = 20
                    };

                    RandomizeLandformData();

                    LandformData.Variation = VariationTrack.Value;
                    LandformData.Smoothing = SmoothingTrack.Value;
                    LandformData.SegmentLength = RoughnessTrack.Value;

                    MapGenerator.GenerateLandform(LandformData);

                    tryCount++;

                    if (tryCount > MAX_TRIES) break;
                }

                if (tryCount < MAX_TRIES)
                {
                    GenerationStatusLabel.Text += "Complete.";
                    GenerationStatusLabel.Refresh();
                    DrawLandformBoundary(true);

                    LandformDataList.Add(LandformData);
                }
                else
                {
                    GenerationStatusLabel.Text = "Landform generation failed. Please try again.";
                    GenerationStatusLabel.Refresh();
                }
            }
        }

        private void RandomizeLandformData()
        {
            LandformData.SeaLevel = 0.5F;
            float noiseScale = (float)Random.Shared.NextDouble();
            LandformData.NoiseScale = noiseScale;

            float interpolationWeight = (float)Random.Shared.NextDouble();
            LandformData.InterpolationWeight = interpolationWeight;

            string? distanceFunction = (string?)DistanceFunctionUpDown.Items[0];

            if (!string.IsNullOrEmpty(distanceFunction))
            {
                LandformData.DistanceFunction = distanceFunction;
            }
        }

        private void RandomLandformRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (RandomLandformRadio.Checked)
            {
                SelectedLandformType = GeneratedLandformTypeEnum.Random;
            }
        }

        private void ContinentRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (ContinentRadio.Checked)
            {
                SelectedLandformType = GeneratedLandformTypeEnum.Continent;
            }
        }

        private void ArchipelagoRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (ArchipelagoRadio.Checked)
            {
                SelectedLandformType = GeneratedLandformTypeEnum.Archipelago;
            }
        }

        private void AtollRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (AtollRadio.Checked)
            {
                SelectedLandformType = GeneratedLandformTypeEnum.Atoll;
            }
        }

        private void WorldRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (WorldRadio.Checked)
            {
                SelectedLandformType = GeneratedLandformTypeEnum.World;
            }
        }

        private void EquirectangularRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (EquirectangularRadio.Checked)
            {
                SelectedLandformType = GeneratedLandformTypeEnum.Equirectangular;
            }
        }

        private void PlaceLandformButton_Click(object sender, EventArgs e)
        {
            if (LandformData.LandformContourPath != null && LandformData.LandformContourPath.PointCount > 0
                && NewLandform != null && Map != null)
            {
                NewLandform.GenMapData = LandformData;
                NewLandform.X = LandformData.LandformLocationLeft;
                NewLandform.Y = LandformData.LandformLocationTop;
                NewLandform.Width = LandformData.LandformAreaWidth;
                NewLandform.Height = LandformData.LandformAreaHeight;

                SKMatrix translateMatrix = SKMatrix.CreateTranslation(LandformData.LandformLocationLeft, LandformData.LandformLocationTop);

                LandformData.LandformContourPath.Transform(translateMatrix);

                NewLandform.LandformPath = LandformData.LandformContourPath;

                LandformType2Methods.CreateType2LandformPaths(Map, NewLandform);

                MapBuilder.GetMapLayerByIndex(Map, MapBuilder.LANDFORMLAYER).MapLayerComponents.Add(NewLandform);
                LandformType2Methods.LANDFORM_LIST.Add(NewLandform);

                // TODO: merging generated landforms isn't working - why?
                LandformType2Methods.MergeLandforms();

                LandformType2Methods.SELECTED_LANDFORM = NewLandform;

                // TODO: undo/redo

                OnLandformGenerated(EventArgs.Empty);

                

            }
        }
    }
}
