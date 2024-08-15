using AForge;
using AForge.Imaging.Filters;
using DelaunatorSharp;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Drawing.Imaging;

namespace MapCreator
{
    public partial class GenerateLandform : Form
    {
        public event EventHandler LandformGenerated;

        private MapCreatorMap? Map {  get; set; }
        private MapLandformType2? NewLandform {  get; set; }

        private GeneratedLandformTypeEnum SelectedLandformType = GeneratedLandformTypeEnum.Random;

        internal GeneratedMapData LandformData { get; set; } = new();

        public GenerateLandform()
        {
            InitializeComponent();

            GenLandformSplitContainer.SplitterDistance = 530;
        }

        public void Initialize(MapCreatorMap map, MapLandformType2 newLandform)
        {
            Map = map;
            NewLandform = newLandform;

            LandformData.MapWidth = Map.MapWidth;
            LandformData.MapHeight = Map.MapHeight;
            LandformData.GridSize = (int)GridSizeUpDown.Value;
            LandformData.SeaLevel = (float)SeaLevelUpDown.Value;
        }

        protected virtual void OnLandformGenerated(EventArgs e)
        {
            EventHandler eventHandler = LandformGenerated;
            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void GeneratePointsButton_Click(object sender, EventArgs e)
        {
            List<IPoint> landformPoints = MapGenerator.GenerateRandomLandformPoints(LandformData.MapWidth, LandformData.MapHeight, LandformData.GridSize);

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

                Bitmap? landformBitmap = MapGenerator.ExtractLargestBlob(filledB);

                if (landformBitmap != null)
                {
                    Bitmap newBitmap = new(LandformData.MapWidth, LandformData.MapHeight);
                    Graphics g = Graphics.FromImage(newBitmap);
                    g.DrawImageUnscaled(landformBitmap, 0, 0);

                    LandformPictureBox.Image = newBitmap;

                    LandformData.ScaledBitmap = newBitmap;
                    LandformData.RotatedScaledBitmap = newBitmap;

                    g.Dispose();
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
                    // convert the bitmap to an 8bpp grayscale image for processing
                    Bitmap newB = Grayscale.CommonAlgorithms.BT709.Apply(LandformData.RotatedScaledBitmap);
                    LandformData.RotatedScaledBitmap = newB;
                }

                SobelEdgeDetector filter = new SobelEdgeDetector();
                // apply the filter
                Bitmap b = filter.Apply(LandformData.RotatedScaledBitmap);

                Invert invert = new();
                Bitmap invertedBitmap = invert.Apply(b);

                // create filter
                PointedColorFloodFill f2 = new PointedColorFloodFill();
                // configure the filter
                f2.Tolerance = Color.FromArgb(32, 32, 32);
                f2.FillColor = Color.Black;
                f2.StartingPoint = new IntPoint(LandformData.RotatedScaledBitmap.Width / 2, LandformData.RotatedScaledBitmap.Height / 2);
                // apply the filter
                f2.ApplyInPlace(invertedBitmap);

                Bitmap lf32bpp = new(invertedBitmap.Width, invertedBitmap.Height, PixelFormat.Format32bppArgb);
                using Graphics g = Graphics.FromImage(lf32bpp);
                g.Clear(Color.White);
                g.DrawImage(invertedBitmap, 0, 0);

                SKPath contourPath = MapDrawingMethods.GetContourPathFromBitmap(lf32bpp, out List<SKPoint> contourPoints);
                LandformData.LandformContourPath = contourPath;

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


                using SKBitmap skb = new(LandformData.MapWidth, LandformData.MapHeight);
                using SKCanvas canvas = new(skb);

                canvas.DrawPath(LandformData.LandformContourPath, contourPaint);

                Bitmap b = skb.ToBitmap();

                if (scaleToFit)
                {
                    float horizontalScalingFactor = (float)LandformPictureBox.Width / LandformData.MapWidth;
                    float verticalScalingFactor = (float)LandformPictureBox.Height / LandformData.MapHeight;

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

                    break;

                case GeneratedLandformTypeEnum.Atoll:

                    break;

                case GeneratedLandformTypeEnum.Archipelago:

                    break;

                case GeneratedLandformTypeEnum.World:

                    break;

                case GeneratedLandformTypeEnum.Equirectangular:

                    break;

                default:

                    break;
            }
        }

        private void GenerateRandomLandform()
        {
            int tryCount = 0;

            while (LandformData.LandformContourPath == null
                || LandformData.LandformContourPath.PointCount < 100
                || LandformData.LandformContourPath.GetOvalBounds().Width < 100
                || LandformData.RotatedScaledBitmap == null
                || LandformData.RotatedScaledBitmap.Width < 100
                || LandformData.RotatedScaledBitmap.Height < 100)
            {
                LandformData = new()
                {
                    MapWidth = Map.MapWidth,
                    MapHeight = Map.MapHeight,
                    GridSize = 25
                };

                RandomizeLandformData();
                MapGenerator.GenerateLandform(LandformData);

                tryCount++;

                if (tryCount > 20) break;
            }

            DrawLandformBoundary(true);
        }

        private void RandomizeLandformData()
        {
            LandformData.SeaLevel = 0.5F;
            float noiseScale = (float)Random.Shared.NextDouble();
            LandformData.NoiseScale = noiseScale;

            float interpolationWeight = (float)Random.Shared.NextDouble();
            LandformData.InterpolationWeight = interpolationWeight;

            int distanceFunctionIdx = Random.Shared.Next(0, DistanceFunctionUpDown.Items.Count);

            //string? distanceFunction = (string?)DistanceFunctionUpDown.Items[distanceFunctionIdx];

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
                NewLandform.LandformPath = LandformData.LandformContourPath;

                LandformType2Methods.CreateType2LandformPaths(Map, NewLandform);

                MapBuilder.GetMapLayerByIndex(Map, MapBuilder.LANDFORMLAYER).MapLayerComponents.Add(NewLandform);
                LandformType2Methods.LANDFORM_LIST.Add(NewLandform);

                // TODO: merging generated landforms isn't working
                LandformType2Methods.MergeLandforms();

                LandformType2Methods.SELECTED_LANDFORM = NewLandform;

                // TODO: undo/redo

                OnLandformGenerated(EventArgs.Empty);
            }
        }
    }
}
