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
using FontAwesome.Sharp;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.Xml.Linq;
using Application = System.Windows.Forms.Application;
using Button = System.Windows.Forms.Button;
using Color = System.Drawing.Color;
using Control = System.Windows.Forms.Control;
using Extensions = SkiaSharp.Views.Desktop.Extensions;
using Image = System.Drawing.Image;
using KeyPressEventArgs = System.Windows.Forms.KeyPressEventArgs;
using MessageBox = System.Windows.Forms.MessageBox;
using Pen = System.Drawing.Pen;
using PixelFormat = System.Drawing.Imaging.PixelFormat;
using Point = System.Drawing.Point;
using TextBox = System.Windows.Forms.TextBox;
using ToolTip = System.Windows.Forms.ToolTip;


namespace MapCreator
{
    public partial class MainForm : Form
    {
        private readonly int MAX_VIEWPORT_WIDTH = 1024;
        private readonly int MAX_VIEWPORT_HEIGHT = 768;

        private MapCreatorMap CURRENT_MAP;
        private SKCanvas? MAP_CANVAS = null;

        private DrawingModeEnum CURRENT_DRAWING_MODE = DrawingModeEnum.None;

        private Point IMAGEBOX_CLICK_POINT = new();
        private SKPoint LAYER_CLICK_POINT = new();
        private SKPoint PREVIOUS_LAYER_CLICK_POINT = new();

        private readonly ToolTip TOOLTIP = new();

        private float PlacementRate = 1.0F;
        private float PlacementDensity = 1.0F;

        private SKPoint? PATH_CLICK_POINT;
        private SKPoint? PREVIOUS_PATH_CLICK_POINT;

        private SKPoint RIVER_CLICK_POINT;
        private SKPoint PREVIOUS_RIVER_CLICK_POINT;

        private SymbolTypeEnum SELECTED_SYMBOL_TYPE = SymbolTypeEnum.NotSet;
        private MapSymbol? UISelectedMapSymbol = null;
        private MapLabel? UISelectedLabel = null;
        private PlacedMapBox? UISelectedBox = null;
        private MapGrid? UIMapGrid = null;
        private MapWindrose? UIWindrose = null;
        private MapScale? UIMapScale = null;
        private MapMeasure? UIMapMeasure = null;
        private MapRegion? UIMapRegion = null;

        public SKRect? UISelectedLandformArea = null;

        private Cmd_ColorOcean COLOR_OCEAN_COMMAND;
        private Cmd_EraseOceanColor ERASE_OCEAN_COLOR_COMMAND;

        private Cmd_EraseLandform ERASE_LANDFORM_COMMAND;
        private Cmd_ColorLandform COLOR_LANDFORM_COMMAND;
        private Cmd_EraseLandformColor ERASE_LANDFORM_COLOR_COMMAND;

        private Cmd_ErasePaintedWaterFeature ERASE_WATERFEATURE_COMMAND;
        private Cmd_ColorPaintedWaterFeature COLOR_WATERFEATURE_COMMAND;
        private Cmd_EraseWaterFeatureColor ERASE_WATERFEATURE_COLOR_COMMAND;

        private readonly AppSplashScreen SPLASH_SCREEN;

        private readonly BackgroundWorker CURSOR_POSITION_WORKER = new()
        {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
        };

        private readonly NameGeneratorSettings NAME_GENERATOR_SETTINGS_DIALOG = new();
        private GenerateLandform? GENERATE_LANDFORM_DIALOG;

        private TextBox? LABEL_TEXT_BOX;

        private static readonly List<object> DRAWING_MODE_BUTTONS = [];

        private static bool EDITING_REGION = false;
        private static MapRegionPoint? NEW_REGION_POINT = null;
        private static int PREVIOUS_REGION_POINT_INDEX = -1;
        private static int NEXT_REGION_POINT_INDEX = -1;

        private GeneratedLandformTypeEnum SELECTED_LANDFORM_TYPE = GeneratedLandformTypeEnum.Random;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private static SKSurface MAP_SURFACE;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        //private static GLControl? GL_CONTROL;
        //private static GRContext? GPU_CONTEXT;
        //private static SKSurface? GPU_SURFACE;

        #region MAINFORM CONSTRUCTOR

        public MainForm()
        {
#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8604 // Possible null reference argument.

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

            Program.LOGGER.Info("Initializing MapCreator");

            InitializeComponent();

            SPLASH_SCREEN = new AppSplashScreen();
            SPLASH_SCREEN.Show();
            SPLASH_SCREEN.Refresh();

            CURRENT_MAP = MapBuilder.CreateMap("", "DEFAULT", (ushort)MAX_VIEWPORT_WIDTH, (ushort)MAX_VIEWPORT_HEIGHT);

            ERASE_LANDFORM_COMMAND = new(CURRENT_MAP);

            COLOR_LANDFORM_COMMAND = new(CURRENT_MAP);
            ERASE_LANDFORM_COLOR_COMMAND = new(CURRENT_MAP);

            COLOR_OCEAN_COMMAND = new(CURRENT_MAP);
            ERASE_OCEAN_COLOR_COMMAND = new(CURRENT_MAP);

            ERASE_WATERFEATURE_COMMAND = new(CURRENT_MAP);
            COLOR_WATERFEATURE_COMMAND = new(CURRENT_MAP);
            ERASE_WATERFEATURE_COLOR_COMMAND = new(CURRENT_MAP);

            DRAWING_MODE_BUTTONS.Add(WindroseButton);
            DRAWING_MODE_BUTTONS.Add(OceanPaintButton);
            DRAWING_MODE_BUTTONS.Add(OceanColorEraseButton);
            DRAWING_MODE_BUTTONS.Add(OceanColorSelectButton);
            DRAWING_MODE_BUTTONS.Add(LandformSelectButton);
            DRAWING_MODE_BUTTONS.Add(LandformPaintButton);
            DRAWING_MODE_BUTTONS.Add(LandEraseButton);
            DRAWING_MODE_BUTTONS.Add(SelectLandformAreaButton);
            DRAWING_MODE_BUTTONS.Add(LandColorButton);
            DRAWING_MODE_BUTTONS.Add(LandColorEraseButton);
            DRAWING_MODE_BUTTONS.Add(LandColorSelectButton);
            DRAWING_MODE_BUTTONS.Add(WaterFeatureSelectButton);
            DRAWING_MODE_BUTTONS.Add(WaterFeaturePaintButton);
            DRAWING_MODE_BUTTONS.Add(WaterFeatureLakeButton);
            DRAWING_MODE_BUTTONS.Add(WaterFeatureRiverButton);
            DRAWING_MODE_BUTTONS.Add(WaterFeatureEraseButton);
            DRAWING_MODE_BUTTONS.Add(WaterColorButton);
            DRAWING_MODE_BUTTONS.Add(WaterColorEraseButton);
            DRAWING_MODE_BUTTONS.Add(WaterColorSelectButton);
            DRAWING_MODE_BUTTONS.Add(SelectPathButton);
            DRAWING_MODE_BUTTONS.Add(DrawPathButton);
            DRAWING_MODE_BUTTONS.Add(SelectSymbolButton);
            DRAWING_MODE_BUTTONS.Add(ColorSymbolsButton);
            DRAWING_MODE_BUTTONS.Add(EraseSymbolsButton);
            DRAWING_MODE_BUTTONS.Add(SelectLabelButton);
            DRAWING_MODE_BUTTONS.Add(PlaceLabelButton);
            DRAWING_MODE_BUTTONS.Add(CreateBoxButton);
            DRAWING_MODE_BUTTONS.Add(CircleTextPathButton);
            DRAWING_MODE_BUTTONS.Add(BezierTextPathButton);
            DRAWING_MODE_BUTTONS.Add(MeasureButton);
            DRAWING_MODE_BUTTONS.Add(SelectRegionButton);
            DRAWING_MODE_BUTTONS.Add(PaintRegionButton);

            SKImageInfo imageInfo = new(1, 1);

            MAP_SURFACE = SKSurface.Create(imageInfo);

            if (MAP_SURFACE == null)
            {
                MessageBox.Show("Could not create map graphics surface. Exiting.", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                Application.Exit();
            }

#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8601 // Possible null reference assignment.
        }

        #endregion

        #region Map Initialization and Rendering

        /**************************************************************************************************************************
        * Map Initialization Methods
        ***************************************************************************************************************************/

        private void CreateMap()
        {
            // open a dialog to set map width, height, and other parameters
            MapProperties mapProperties = new(CURRENT_MAP);

            // set property defaults
            mapProperties.ShowDialog(this);

            // set the map parameters based on user-selected inputs

            // save the map with initial parameters
            CURRENT_MAP = MapBuilder.CreateMap(CURRENT_MAP);
            Text = "Map Creator - " + CURRENT_MAP.MapName;

            CURRENT_MAP.IsSaved = false;

            InitializeMap(CURRENT_MAP);

            // remove any existing vignette and create a new one
            for (int i = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.VIGNETTELAYER).MapLayerComponents.Count - 1; i > 0; i--)
            {
                if (MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.VIGNETTELAYER).MapLayerComponents[i] is MapVignette)
                {
                    MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.VIGNETTELAYER).MapLayerComponents.RemoveAt(i);
                    break;
                }
            }

            MapVignette vignette = new(CURRENT_MAP)
            {
                VignetteColor = VignetteColorSelectionLabel.BackColor,
                VignetteStrength = VignetteStrengthScroll.Value
            };

            MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.VIGNETTELAYER).MapLayerComponents.Add(vignette);

            MapImageBox.Refresh();

            UpdateMapNameAndSize();
            UpdateViewportStatus();
        }

        private void InitializeMap(MapCreatorMap map)
        {

            //if (GPU_CONTEXT == null)
            //{
            //    GL_CONTROL = new GLControl(new GraphicsMode(32, 24, 8, 4));
            //    GL_CONTROL.MakeCurrent();
            //    GPU_CONTEXT = GRContext.CreateGl();
            //}

            //GPU_SURFACE = SKSurface.Create(GPU_CONTEXT, false, new SKImageInfo(map.MapWidth, map.MapHeight));
            //MAP_CANVAS = GPU_SURFACE.Canvas;
            //MapImageBox.Image = Extensions.ToBitmap(GPU_SURFACE.Snapshot());

            MAP_SURFACE.Dispose();

            SKImageInfo imageInfo = new(map.MapWidth, map.MapHeight);

            MAP_SURFACE = SKSurface.Create(imageInfo);

            // MAP_CANVAS is the canvas for the entire map bitmap
            // RenderDrawingPanel renders all of the bitmaps for all of the MapComponents in each layer
            // onto the MAP_CANVAS
            //MAP_CANVAS = new SKCanvas(map.MapBackingBitmap);
            MAP_CANVAS = MAP_SURFACE.Canvas;

            MapImageBox.Image = MAP_SURFACE.Snapshot().ToBitmap();

            WaterFeatureMethods.WATER_LAYER_PATH.Reset();
            WaterFeatureMethods.WATER_LAYER_DRAW_PATH.Reset();
            WaterFeatureMethods.WATER_LAYER_ERASER_PATH.Reset();

            // the cursor overlay bitmap is drawn on top of (in front of) the map in the MapImageBox
            // it holds the dashed circle that follows the cross cursor on the ocean tab and the land tab
            SKBitmap cursorOverlayBitmap = MapPaintMethods.SetCursorOverlayBitmap(new SKBitmap(MapImageBox.Width, MapImageBox.Height));

            using (SKCanvas canvas = new(cursorOverlayBitmap))
            {
                canvas.Clear();
            }

            MapPaintMethods.ConstructPaintObjectsAndShaders();

            SymbolMethods.SetCustomColorAtIndex(Extensions.ToSKColor(SymbolColor1Label.BackColor), 0);
            SymbolMethods.SetCustomColorAtIndex(Extensions.ToSKColor(SymbolColor2Label.BackColor), 1);
            SymbolMethods.SetCustomColorAtIndex(Extensions.ToSKColor(SymbolColor3Label.BackColor), 2);
            SymbolMethods.SetCustomColorAtIndex(Extensions.ToSKColor(SymbolColor4Label.BackColor), 3);

            BackgroundToolPanel.Visible = true;
        }

        private void CreateUserDefinedHatchShaders()
        {
            if (HatchPatternSelectionBox.SelectedIndex > -1)
            {
                int hatchOpacity = (int)(255.9F * (HatchOpacityTrack.Value / 100.0F));
                int bitmapSize = HatchScaleTrack.Value;

                Bitmap b = new(LandformType2Methods.HATCH_TEXTURE_LIST[HatchPatternSelectionBox.SelectedIndex].TexturePath);

                HatchPatternPreviewBox.Image = b;

                // create shaders for user-defined hatch

                SKBitmap resizedSKBitmap = new SKBitmap(bitmapSize, bitmapSize);

                Extensions.ToSKBitmap(b).ScalePixels(resizedSKBitmap, SKFilterQuality.High);

                SKBlendMode blendMode = GetSelectedBlendMode();

                LandformType2Methods.ConstructUserDefinedShaders(resizedSKBitmap, hatchOpacity, bitmapSize, blendMode);
            }
        }

        public void RenderDrawingPanel()
        {
            if (CURRENT_MAP != null && MAP_CANVAS != null)
            {
                MapBuilder.ClearLayerCanvas(CURRENT_MAP, MapBuilder.SELECTIONLAYER);

                // render all of the layers onto the MAP_CANVAS,
                // then display the resulting MAP_SURFACE snapshot as the MapImageBox Image
                CURRENT_MAP.Render(MAP_CANVAS);

                using SKImage snap = MAP_SURFACE.Snapshot();
                MapImageBox.Image = snap.ToBitmap();
            }
        }

        #endregion

        #region Map File Open and Save Methods

        private void OpenExistingMap()
        {
            try
            {
                OpenFileDialog ofd = new()
                {
                    Title = "Open or Create Map",
                    DefaultExt = "mcmapx",
                    Filter = "map files (*.mcmapx)|*.mcmapx|All files (*.*)|*.*",
                    CheckFileExists = true,
                    RestoreDirectory = true,
                    ShowHelp = false,           // enabling the help button causes the dialog not to display files
                    Multiselect = false
                };

                if (ofd.ShowDialog(this) == DialogResult.OK)
                {
                    if (ofd.FileName != "")
                    {
                        try
                        {
                            OpenMap(ofd.FileName);
                        }
                        catch
                        {
                            throw;
                        }

                        MapImageBox.Refresh();

                        UpdateMapNameAndSize();
                        UpdateViewportStatus();
                    }
                }
            }
            catch { }
        }

        private void OpenMap(string mapFilePath)
        {
            // open an existing map
            try
            {
                SetStatusText("Loading: " + Path.GetFileName(mapFilePath));

                try
                {
                    CURRENT_MAP = MapFileMethods.OpenMap(mapFilePath);
                }
                catch
                {
                    throw;
                }

                InitializeMap(CURRENT_MAP);

                MapBuilder.CreateMapCanvases(CURRENT_MAP);

                CURRENT_MAP.IsSaved = true;

                MapLayer landformLayer = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.LANDFORMLAYER);
                LandformType2Methods.LANDFORM_LIST.Clear();
                LandformType2Methods.SELECTED_LANDFORM = new();

                for (int i = 0; i < landformLayer.MapLayerComponents.Count; i++)
                {
                    if (landformLayer.MapLayerComponents[i] is MapLandformType2 landform)
                    {
                        landform.ParentMap = CURRENT_MAP;
                        LandformType2Methods.CreateType2LandformPaths(CURRENT_MAP, landform);
                        LandformType2Methods.LANDFORM_LIST.Add(landform);
                    }
                }

                MapLayer waterLayer = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.WATERLAYER);

                WaterFeatureMethods.PAINTED_WATERFEATURE_LIST.Clear();
                WaterFeatureMethods.NEW_WATERFEATURE = new();

                // remove any duplicate water features from the waterlayer

                for (int i = 0; i < waterLayer.MapLayerComponents.Count; i++)
                {
                    for (int j = 0; (j < waterLayer.MapLayerComponents.Count) && (i != j); j++)
                    {
                        if (waterLayer.MapLayerComponents[i] is MapPaintedWaterFeature && waterLayer.MapLayerComponents[j] is MapPaintedWaterFeature)
                        {
                            if (((MapPaintedWaterFeature)waterLayer.MapLayerComponents[i]).WaterFeatureGuid.Equals(((MapPaintedWaterFeature)waterLayer.MapLayerComponents[j]).WaterFeatureGuid))
                            {
                                ((MapPaintedWaterFeature)waterLayer.MapLayerComponents[i]).IsRemoved = true;
                            }
                        }
                    }
                }

                for (int i = waterLayer.MapLayerComponents.Count - 1; i >= 0; i--)
                {
                    if (waterLayer.MapLayerComponents[i] is MapPaintedWaterFeature waterfeature && waterfeature.IsRemoved)
                    {
                        waterLayer.MapLayerComponents.RemoveAt(i);
                    }
                }

                for (int i = 0; i < waterLayer.MapLayerComponents.Count; i++)
                {
                    if (waterLayer.MapLayerComponents[i] is MapPaintedWaterFeature waterFeature)
                    {
                        if (!waterFeature.IsRemoved)
                        {
                            waterFeature.ParentMap = CURRENT_MAP;
                            WaterFeatureMethods.ConstructWaterFeaturePaintObjects(waterFeature);
                            WaterFeatureMethods.AddPaintedWaterFeature(waterFeature);
                        }
                    }
                    else if (waterLayer.MapLayerComponents[i] is MapRiver river)
                    {
                        WaterFeatureMethods.ConstructRiverPaintObjects(river);
                        WaterFeatureMethods.AddRiver(river);
                    }
                }

                MapLayer pathLowerLayer = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.PATHLOWERLAYER);
                MapLayer pathUpperLayer = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.PATHUPPERLAYER);

                List<MapPath> mapPathList = MapPathMethods.GetMapPathList();
                mapPathList.Clear();

                for (int i = 0; i < pathLowerLayer.MapLayerComponents.Count; i++)
                {
                    if (pathLowerLayer.MapLayerComponents[i] is MapPath mapPath)
                    {
                        MapPathMethods.ConstructPathPaint(mapPath);

                        SKPath path = MapPathMethods.GenerateMapPathBoundaryPath(mapPath.PathPoints);
                        mapPath.BoundaryPath?.Dispose();
                        mapPath.BoundaryPath = new(path);
                        path.Dispose();

                        mapPathList.Add(mapPath);
                    }
                }

                for (int i = 0; i < pathUpperLayer.MapLayerComponents.Count; i++)
                {
                    if (pathUpperLayer.MapLayerComponents[i] is MapPath mapPath)
                    {
                        MapPathMethods.ConstructPathPaint(mapPath);

                        SKPath path = MapPathMethods.GenerateMapPathBoundaryPath(mapPath.PathPoints);
                        mapPath.BoundaryPath?.Dispose();
                        mapPath.BoundaryPath = new(path);
                        path.Dispose();

                        mapPathList.Add(mapPath);
                    }
                }

                MapLayer symbolLayer = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.SYMBOLLAYER);

                for (int i = 0; i < symbolLayer.MapLayerComponents.Count; i++)
                {
                    if (symbolLayer.MapLayerComponents[i] is MapSymbol symbol)
                    {
                        SKColor? paintColor = symbol.GetSymbolCustomColorAtIndex(0);

                        // reconstruct paint object for grayscale symbols
                        if (symbol.GetIsGrayScale() && paintColor != null)
                        {
                            SKPaint paint = new()
                            {
                                ColorFilter = SKColorFilter.CreateBlendMode((SKColor)paintColor,
                                    SKBlendMode.Modulate) // combine the selected color with the bitmap colors
                            };

                            symbol.SetSymbolPaint(paint);

                            SKBitmap? placedBitmap = symbol.GetPlacedBitmap();
                            if (placedBitmap != null)
                            {
                                if (symbol.Width != placedBitmap.Width || symbol.Height != placedBitmap.Height)
                                {
                                    // resize the placed bitmap to match the size set in the symbol - this shouldn't be necessary
                                    SKBitmap resizedPlacedBitmap = new SKBitmap(symbol.Width, symbol.Height);

                                    placedBitmap.ScalePixels(resizedPlacedBitmap, SKFilterQuality.High);
                                    symbol.SetPlacedBitmap(resizedPlacedBitmap);

                                }
                            }
                        }

                        SymbolMethods.PlacedSymbolList.Add(symbol);
                    }
                }

                MapLayer boxLayer = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.BOXLAYER);
                for (int i = 0; i < boxLayer.MapLayerComponents.Count; i++)
                {
                    if (boxLayer.MapLayerComponents[i] is PlacedMapBox box)
                    {
                        MapLabelMethods.MAP_BOXES.Add(box);
                    }
                }

                MapLayer labelLayer = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.LABELLAYER);
                for (int i = 0; i < labelLayer.MapLayerComponents.Count; i++)
                {
                    if (labelLayer.MapLayerComponents[i] is MapLabel label)
                    {
                        MapLabelMethods.MAP_LABELS.Add(label);
                    }
                }

                MapLayer defaultGridLayer = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.DEFAULTGRIDLAYER);
                for (int i = 0; i < defaultGridLayer.MapLayerComponents.Count; i++)
                {
                    if (defaultGridLayer.MapLayerComponents[i] is MapGrid grid)
                    {
                        UIMapGrid = grid;
                        break;
                    }
                }

                if (UIMapGrid == null)
                {
                    MapLayer oceanGridLayer = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.ABOVEOCEANGRIDLAYER);
                    for (int i = 0; i < oceanGridLayer.MapLayerComponents.Count; i++)
                    {
                        if (oceanGridLayer.MapLayerComponents[i] is MapGrid grid)
                        {
                            UIMapGrid = grid;
                            break;
                        }
                    }
                }

                if (UIMapGrid == null)
                {
                    MapLayer symbolGridLayer = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.BELOWSYMBOLSGRIDLAYER);
                    for (int i = 0; i < symbolGridLayer.MapLayerComponents.Count; i++)
                    {
                        if (symbolGridLayer.MapLayerComponents[i] is MapGrid grid)
                        {
                            UIMapGrid = grid;
                            break;
                        }
                    }
                }

                if (UIMapGrid != null)
                {
                    UIMapGrid.ParentMap = CURRENT_MAP;
                    UIMapGrid.GridEnabled = true;

                    switch (UIMapGrid.GridType)
                    {
                        case GridTypeEnum.Square:
                            SquareGridRadio.Checked = true; break;
                        case GridTypeEnum.PointedHex:
                            PointedHexRadio.Checked = true; break;
                        case GridTypeEnum.FlatHex:
                            FlatHexRadio.Checked = true; break;
                    }

                    GridSizeTrack.Value = UIMapGrid.GridSize;
                    GridLineWidthTrack.Value = UIMapGrid.GridLineWidth;
                    GridColorSelectLabel.BackColor = UIMapGrid.GridColor;

                    if (UIMapGrid.GridLayerIndex == MapBuilder.ABOVEOCEANGRIDLAYER)
                    {
                        LayerUpDown.SelectedItem = "Above Ocean";
                    }
                    else if (UIMapGrid.GridLayerIndex == MapBuilder.BELOWSYMBOLSGRIDLAYER)
                    {
                        LayerUpDown.SelectedItem = "Below Symbols";
                    }
                    else
                    {
                        LayerUpDown.SelectedItem = "Default";
                    }
                }

                MapLayer windroseLayer = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.WINDROSELAYER);
                for (int i = 0; i < windroseLayer.MapLayerComponents.Count; i++)
                {
                    if (windroseLayer.MapLayerComponents[i] is MapWindrose windrose)
                    {
                        windrose.WindrosePaint = new()
                        {
                            Style = SKPaintStyle.Stroke,
                            StrokeWidth = windrose.LineWidth,
                            Color = windrose.WindroseColor.ToSKColor(),
                            IsAntialias = true,
                        };
                    }
                }


                MapLayer regionLayer = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.REGIONLAYER);
                for (int i = 0; i < regionLayer.MapLayerComponents.Count; i++)
                {
                    if (regionLayer.MapLayerComponents[i] is MapRegion region)
                    {
                        region.Map = CURRENT_MAP;
                        SKPathEffect? regionBorderEffect = ConstructRegionBorderEffect(region);
                        ConstructRegionPaintObjects(region, regionBorderEffect);
                        MapRegionMethods.MAP_REGION_LIST.Add(region);
                    }
                }

                MapLayer vignetteLayer = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.VIGNETTELAYER);
                for (int i = 0; i < vignetteLayer.MapLayerComponents.Count; i++)
                {
                    if (vignetteLayer.MapLayerComponents[i] is MapVignette vignette)
                    {
                        vignette.Map = CURRENT_MAP;

                    }
                }

                Text = "Map Creator - " + CURRENT_MAP.MapName;
                SetStatusText("Loaded: " + CURRENT_MAP.MapName);

                UpdateMapNameAndSize();
                UpdateViewportStatus();
            }
            catch
            {
#pragma warning disable CS8601 // Possible null reference assignment.
#pragma warning disable CS8604 // Possible null reference argument.

                MessageBox.Show("An error has occurred while opening the map. The map file may be corrupt.", "Error Loading Map", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                CURRENT_MAP = MapBuilder.CreateMap("", "DEFAULT", MapBuilder.MAP_DEFAULT_WIDTH, MapBuilder.MAP_DEFAULT_HEIGHT);

                InitializeMap(CURRENT_MAP);

                for (int i = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.VIGNETTELAYER).MapLayerComponents.Count - 1; i > 0; i--)
                {
                    if (MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.VIGNETTELAYER).MapLayerComponents[i] is MapVignette)
                    {
                        MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.VIGNETTELAYER).MapLayerComponents.RemoveAt(i);
                        break;
                    }
                }

                MapVignette vignette = new(CURRENT_MAP)
                {
                    VignetteColor = VignetteColorSelectionLabel.BackColor,
                    VignetteStrength = VignetteStrengthScroll.Value
                };

                MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.VIGNETTELAYER).MapLayerComponents.Add(vignette);

                CURRENT_MAP.IsSaved = false;

                Text = "Map Creator - " + CURRENT_MAP.MapName;

#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8601 // Possible null reference assignment.

                throw;
            }
        }

        private DialogResult SaveMap()
        {
            SaveFileDialog sfd = new()
            {
                DefaultExt = "mcmapx",
                CheckWriteAccess = true,
                ExpandedMode = true,
                AddExtension = true,
                SupportMultiDottedExtensions = false,
                AddToRecent = true,
                Filter = "MapCreator Map|*.mcmapx",
                Title = "Save Map",
            };

            if (!string.IsNullOrEmpty(CURRENT_MAP.MapPath))
            {
                sfd.FileName = CURRENT_MAP.MapPath;
            }
            else if (!string.IsNullOrEmpty(CURRENT_MAP.MapName))
            {
                sfd.FileName = CURRENT_MAP.MapName;
            }

            DialogResult result = sfd.ShowDialog();

            if (result == DialogResult.OK)
            {
                if (!string.IsNullOrEmpty(sfd.FileName))
                {
                    CURRENT_MAP.MapPath = sfd.FileName;
                    try
                    {
                        MapFileMethods.SaveMap(CURRENT_MAP);
                        CURRENT_MAP.IsSaved = true;
                    }
                    catch (Exception ex)
                    {
                        Program.LOGGER.Error(ex);
                        MessageBox.Show("An error has occurred while saving the map. The map file map be corrupted.", "Map Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    }
                }
            }

            return result;
        }

        #endregion

        #region Main Form and Drawing Mode Methods

        /******************************************************************************************************* 
         * MAIN FORM METHODS
         *******************************************************************************************************/

        private SKBlendMode GetSelectedBlendMode()
        {
            // return the blend mode selected in the combo box
            SKBlendMode selectedBlendMode = SKBlendMode.DstOver;

            if (BlendModeSelectionBox.SelectedIndex > -1)
            {
                switch (BlendModeSelectionBox.SelectedIndex)
                {
                    case 0:
                        selectedBlendMode = SKBlendMode.Clear;
                        break;
                    case 1:
                        selectedBlendMode = SKBlendMode.Src;
                        break;
                    case 2:
                        selectedBlendMode = SKBlendMode.Dst;
                        break;
                    case 3:
                        selectedBlendMode = SKBlendMode.SrcOver;
                        break;
                    case 4:
                        selectedBlendMode = SKBlendMode.DstOver;
                        break;
                    case 5:
                        selectedBlendMode = SKBlendMode.SrcIn;
                        break;
                    case 6:
                        selectedBlendMode = SKBlendMode.DstIn;
                        break;
                    case 7:
                        selectedBlendMode = SKBlendMode.SrcOut;
                        break;
                    case 8:
                        selectedBlendMode = SKBlendMode.DstOut;
                        break;
                    case 9:
                        selectedBlendMode = SKBlendMode.SrcATop;
                        break;
                    case 10:
                        selectedBlendMode = SKBlendMode.DstATop;
                        break;
                    case 11:
                        selectedBlendMode = SKBlendMode.Xor;
                        break;
                    case 12:
                        selectedBlendMode = SKBlendMode.Plus;
                        break;
                    case 13:
                        selectedBlendMode = SKBlendMode.Modulate;
                        break;
                    case 14:
                        selectedBlendMode = SKBlendMode.Screen;
                        break;
                    case 15:
                        selectedBlendMode = SKBlendMode.Overlay;
                        break;
                    case 16:
                        selectedBlendMode = SKBlendMode.Darken;
                        break;
                    case 17:
                        selectedBlendMode = SKBlendMode.Lighten;
                        break;
                    case 18:
                        selectedBlendMode = SKBlendMode.ColorDodge;
                        break;
                    case 19:
                        selectedBlendMode = SKBlendMode.ColorBurn;
                        break;
                    case 20:
                        selectedBlendMode = SKBlendMode.HardLight;
                        break;
                    case 21:
                        selectedBlendMode = SKBlendMode.SoftLight;
                        break;
                    case 22:
                        selectedBlendMode = SKBlendMode.Difference;
                        break;
                    case 23:
                        selectedBlendMode = SKBlendMode.Exclusion;
                        break;
                    case 24:
                        selectedBlendMode = SKBlendMode.Multiply;
                        break;
                    case 25:
                        selectedBlendMode = SKBlendMode.Hue;
                        break;
                    case 26:
                        selectedBlendMode = SKBlendMode.Saturation;
                        break;
                    case 27:
                        selectedBlendMode = SKBlendMode.Color;
                        break;
                    case 28:
                        selectedBlendMode = SKBlendMode.Luminosity;
                        break;
                }
            }
            else
            {
                selectedBlendMode = SKBlendMode.DstOver;
            }

            return selectedBlendMode;
        }

        public void SetStatusText(string text)
        {
            // set the test of the first status strip text box
            ApplicationStatusStrip.Items[0].Text = text;
        }

        private void UpdateMapNameAndSize()
        {
            MapNameLabel.Text = CURRENT_MAP.MapName;
            MapSizeLabel.Text = "Map Size: " + CURRENT_MAP.MapWidth.ToString() + " x " + CURRENT_MAP.MapHeight.ToString();
        }

        private void UpdateDrawingPointLabel(SKPoint cursorPoint, SKPoint mapPoint)
        {
            DrawingPointLabel.Text = "Cursor Point: "
                + cursorPoint.X.ToString()
                + " , "
                + cursorPoint.Y.ToString()
                + "   Map Point: "
                + mapPoint.X.ToString()
                + " , "
                + mapPoint.Y.ToString();

            ApplicationStatusStrip.Refresh();
        }

        private void UpdateViewportStatus()
        {
            ZoomLevelLabel.Text = "Zoom: " + MapImageBox.Zoom.ToString() + "%";
            ApplicationStatusStrip.Refresh();
        }

        private void SetDrawingModeLabel()
        {
            string modeText = "Drawing Mode: ";

            switch (CURRENT_DRAWING_MODE)
            {
                case DrawingModeEnum.None:
                    modeText += "None";
                    break;
                case DrawingModeEnum.LandPaint:
                    modeText += "Landform Paint";
                    break;
                case DrawingModeEnum.LandErase:
                    modeText += "Landform Erase";
                    break;
                case DrawingModeEnum.LandColorErase:
                    modeText += "Landform Color Erase";
                    break;
                case DrawingModeEnum.LandColor:
                    modeText += "Landform Color";
                    break;
                case DrawingModeEnum.OceanErase:
                    modeText += "Ocean Erase";
                    break;
                case DrawingModeEnum.OceanPaint:
                    modeText += "Ocean Paint";
                    break;
                case DrawingModeEnum.ColorSelect:
                    modeText += "Color Select";
                    break;
                case DrawingModeEnum.LandformSelect:
                    modeText += "Landform Select";
                    break;
                case DrawingModeEnum.WaterPaint:
                    modeText += "Water Feature Paint";
                    break;
                case DrawingModeEnum.WaterErase:
                    modeText += "Water Feature Erase";
                    break;
                case DrawingModeEnum.WaterColor:
                    modeText += "Water Feature Color";
                    break;
                case DrawingModeEnum.WaterColorErase:
                    modeText += "Water Feature Color Erase";
                    break;
                case DrawingModeEnum.LakePaint:
                    modeText += "Lake Paint";
                    break;
                case DrawingModeEnum.RiverPaint:
                    modeText += "River Paint";
                    break;
                case DrawingModeEnum.WaterFeatureSelect:
                    modeText += "Water Feature Select";
                    break;
                case DrawingModeEnum.PathPaint:
                    modeText += "Draw Path";
                    break;
                case DrawingModeEnum.PathSelect:
                    modeText += "Select Path";
                    break;
                case DrawingModeEnum.PathEdit:
                    modeText += "Edit Path";
                    break;
                case DrawingModeEnum.SymbolErase:
                    modeText += "Erase Symbol";
                    break;
                case DrawingModeEnum.SymbolPlace:
                    modeText += "Place Symbol";
                    break;
                case DrawingModeEnum.SymbolSelect:
                    modeText += "Select Symbol";
                    break;
                case DrawingModeEnum.SymbolColor:
                    modeText += "Symbol Color";
                    break;
                case DrawingModeEnum.DrawBezierLabelPath:
                    modeText += "Draw Bezier Label Path";
                    break;
                case DrawingModeEnum.DrawArcLabelPath:
                    modeText += "Draw Arc Label Path";
                    break;
                case DrawingModeEnum.DrawLabel:
                    modeText += "Place Label";
                    break;
                case DrawingModeEnum.LabelSelect:
                    modeText += "Select Label";
                    break;
                case DrawingModeEnum.DrawBox:
                    modeText += "Draw Box";
                    break;
                case DrawingModeEnum.PlaceWindrose:
                    modeText += "Place Windrose";
                    break;
                case DrawingModeEnum.SelectMapScale:
                    modeText += "Move Map Scale";
                    break;
                case DrawingModeEnum.DrawMapMeasure:
                    modeText += "Draw Map Measure";
                    break;
                case DrawingModeEnum.RegionPaint:
                    modeText += "Draw Region";
                    break;
                case DrawingModeEnum.RegionSelect:
                    modeText += "Select Region";
                    break;
                case DrawingModeEnum.LandformAreaSelect:
                    modeText += "Select Landform Area";
                    break;
                default:
                    modeText += "Undefined";
                    break;
            }

            modeText += ". Selected Brush: ";

            switch (MapPaintMethods.GetSelectedColorBrushType())
            {
                case ColorPaintBrush.SoftBrush:
                    modeText += "Soft Brush";
                    break;
                case ColorPaintBrush.HardBrush:
                    modeText += "Hard Brush";
                    break;
                case ColorPaintBrush.None:
                    break;
                default:
                    modeText += "None";
                    break;
            }

            DrawingModeLabel.Text = modeText;
            ApplicationStatusStrip.Refresh();
        }

        private static void ClearDrawingModeButtons()
        {
            foreach (object o in DRAWING_MODE_BUTTONS)
            {
                if (o is IconToolStripButton toolstripButton)
                {
                    toolstripButton.Checked = false;
                    toolstripButton.BackColor = SystemColors.Control;
                    toolstripButton.IconColor = Color.Black;
                    toolstripButton.ForeColor = Color.Black;

                }
                else if (o is IconButton iconButton)
                {
                    if (iconButton.FlatStyle == FlatStyle.Flat)
                    {
                        iconButton.BackColor = SystemColors.ControlLightLight;
                        iconButton.IconColor = Color.Black;
                        iconButton.ForeColor = Color.Black;
                    }
                    else if (iconButton.FlatStyle == FlatStyle.Standard)
                    {
                        iconButton.BackColor = SystemColors.Control;
                        iconButton.IconColor = Color.Black;
                        iconButton.ForeColor = Color.Black;
                    }
                    else
                    {
                        iconButton.BackColor = SystemColors.Control;
                        iconButton.IconColor = Color.Black;
                        iconButton.ForeColor = Color.Black;
                    }

                    iconButton.Refresh();
                }
            }

            MapLabelMethods.CreatingLabel = false;
        }

        private void SetDrawingMode(DrawingModeEnum newMode, object? modeButton, bool forceModeSet = false)
        {
            ClearDrawingModeButtons();

            DeselectAllLandforms();
            DeselectAllWaterFeatures();
            DeselectAllPaths();

            // TODO: deselect symbols, labels, regions

            if (CURRENT_DRAWING_MODE != newMode || forceModeSet)
            {
                CURRENT_DRAWING_MODE = newMode;

                if (CURRENT_DRAWING_MODE == DrawingModeEnum.ColorSelect)
                {
                    Cursor = MapPaintMethods.EYEDROPPER_CURSOR;
                }

                if (modeButton != null)
                {
                    Color selectColor = ColorTranslator.FromHtml("#D2F1C1");

                    if (modeButton is IconToolStripButton toolstripButton)
                    {
                        toolstripButton.Checked = false;
                        toolstripButton.BackColor = selectColor;
                        toolstripButton.IconColor = Color.Black;
                        toolstripButton.ForeColor = Color.Black;
                    }
                    else if (modeButton is IconButton iconButton)
                    {
                        if (iconButton.FlatStyle == FlatStyle.Flat)
                        {
                            iconButton.BackColor = selectColor;
                            iconButton.IconColor = Color.Black;
                            iconButton.ForeColor = Color.Black;
                        }
                        else if (iconButton.FlatStyle == FlatStyle.Standard)
                        {
                            iconButton.BackColor = selectColor;
                            iconButton.IconColor = Color.Black;
                            iconButton.ForeColor = Color.Black;
                        }
                        else
                        {
                            iconButton.BackColor = selectColor;
                            iconButton.IconColor = Color.Black;
                            iconButton.ForeColor = Color.Black;
                        }
                    }
                }
            }
            else
            {
                CURRENT_DRAWING_MODE = DrawingModeEnum.None;
            }

            SetDrawingModeLabel();

            Refresh();
        }

        public void UpdateProgressBar(int newValue, string? progressState)
        {
            MapOperationProgressBar.Value = (newValue > 0) ? newValue : 0;

            if (progressState != null)
            {
                SetStatusText(progressState);
            }

            ApplicationStatusStrip.Refresh();
        }

        #endregion

        #region Main Form Event Handlers

        /*******************************************************************************************************
        * Main Form Event Handlers
         *******************************************************************************************************/
        private void Form1_Load(object sender, EventArgs e)
        {
            // load the form
            try
            {
                LoadAllAssets();

                NAME_GENERATOR_SETTINGS_DIALOG.NameGenerated += new EventHandler(NameGenerator_NameGenerated);

                NAME_GENERATOR_SETTINGS_DIALOG.NameGeneratorCheckedList.Items.Clear();

                foreach (string nameGeneratorName in MapToolMethods.NameGeneratorNames)
                {
                    NAME_GENERATOR_SETTINGS_DIALOG.NameGeneratorCheckedList.Items.Add(nameGeneratorName);
                    NAME_GENERATOR_SETTINGS_DIALOG.NameGeneratorCheckedList.SetItemChecked(
                        NAME_GENERATOR_SETTINGS_DIALOG.NameGeneratorCheckedList.Items.Count - 1, true);
                }

                foreach (string nameBaseName in MapToolMethods.NameBaseNames)
                {
                    NAME_GENERATOR_SETTINGS_DIALOG.NameBaseCheckedList.Items.Add(nameBaseName);
                    NAME_GENERATOR_SETTINGS_DIALOG.NameBaseCheckedList.SetItemChecked(
                        NAME_GENERATOR_SETTINGS_DIALOG.NameBaseCheckedList.Items.Count - 1, true);
                }

                foreach (string languageName in MapToolMethods.NameLanguages)
                {
                    NAME_GENERATOR_SETTINGS_DIALOG.LanguagesCheckedList.Items.Add(languageName);
                }

                for (int i = 0; i < NAME_GENERATOR_SETTINGS_DIALOG.LanguagesCheckedList.Items.Count; i++)
                {
                    NAME_GENERATOR_SETTINGS_DIALOG.LanguagesCheckedList.SetItemChecked(i, true);
                }

                GENERATE_LANDFORM_DIALOG = new();
                GENERATE_LANDFORM_DIALOG.LandformGenerated += new EventHandler(GenerateLandform_LandformGenerated);

                // construct MapBitmap and other map objects
                InitializeMap(CURRENT_MAP);

                for (int i = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.VIGNETTELAYER).MapLayerComponents.Count - 1; i > 0; i--)
                {
                    if (MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.VIGNETTELAYER).MapLayerComponents[i] is MapVignette)
                    {
                        MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.VIGNETTELAYER).MapLayerComponents.RemoveAt(i);
                    }
                }

                MapVignette vignette = new(CURRENT_MAP)
                {
                    VignetteColor = VignetteColorSelectionLabel.BackColor,
                    VignetteStrength = VignetteStrengthScroll.Value
                };

                MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.VIGNETTELAYER).MapLayerComponents.Add(vignette);

                UpdateMapNameAndSize();
                UpdateViewportStatus();

                MapImageBox.Refresh();

            }
            catch (UnauthorizedAccessException uAEx)
            {
                Program.LOGGER.Error(uAEx.Message);
            }
            catch (PathTooLongException pathEx)
            {
                Program.LOGGER.Error(pathEx.Message);
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            CURSOR_POSITION_WORKER.CancelAsync();

            SymbolMethods.SaveSymbolTags();
            SymbolMethods.SaveCollections();

            NAME_GENERATOR_SETTINGS_DIALOG.Close();

            if (!CURRENT_MAP.IsSaved)
            {
                TopMost = true;
                DialogResult result =
                    MessageBox.Show("The map has not been saved. Do you want to save the map?", "Exit Application", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                if (result == DialogResult.Yes)
                {
                    DialogResult saveResult = SaveMap();

                    if (saveResult == DialogResult.Cancel)
                    {
                        e.Cancel = true;
                    }
                }
                else if (result == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }

                TopMost = false;
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            BackgroundToolPanel.Visible = true;
            OceanColorToolPanel.Visible = false;
            LandColorToolPanel.Visible = false;
            WaterColorToolPanel.Visible = false;
            PathPanel.Visible = false;
            SymbolsToolPanel.Visible = false;
            LabelsToolPanel.Visible = false;
            OverlayToolsPanel.Visible = false;
            RegionsToolPanel.Visible = false;

            CURSOR_POSITION_WORKER.DoWork += CursorPositionWorkerDoWork;
            CURSOR_POSITION_WORKER.RunWorkerCompleted += CursorPositionRunWorkerCompleted;

            CURSOR_POSITION_WORKER.RunWorkerAsync();

            // refresh the form to get everything rendered completely, then render the map
            Refresh();

            SPLASH_SCREEN.Hide();
            SPLASH_SCREEN.Dispose();
        }

        void NameGenerator_NameGenerated(object? sender, EventArgs e)
        {
            if (sender is NameGeneratorSettings ngs)
            {
                string selectedName = ngs.SelectedName;

                if (MapLabelMethods.CreatingLabel && !string.IsNullOrEmpty(selectedName))
                {
                    if (LABEL_TEXT_BOX != null && !LABEL_TEXT_BOX.IsDisposed)
                    {
                        LABEL_TEXT_BOX.Text = selectedName;
                        LABEL_TEXT_BOX.Refresh();
                    }
                }
            }
        }

        void GenerateLandform_LandformGenerated(object? sender, EventArgs e)
        {
            if (sender is GenerateLandform gl)
            {
                SetLandformData(LandformType2Methods.SELECTED_LANDFORM);

                RenderDrawingPanel();
                //MapImageBox.Refresh();
                gl.Hide();
                UISelectedLandformArea = null;
                //MapBuilder.ClearLayerCanvas(CURRENT_MAP, MapBuilder.WORKLAYER);
            }
        }

        private void CursorPositionWorkerDoWork(object? sender, DoWorkEventArgs e)
        {
            Thread.Sleep(33);   // If you need to make a pause between runs
        }

        private void CursorPositionRunWorkerCompleted(object? sender, RunWorkerCompletedEventArgs e)
        {
            if (!MapImageBox.IsDisposed)
            {
                SKPoint cursorPosition = MapImageBox.PointToClient(Cursor.Position).ToSKPoint();
                int brushRadius = SetBrushRadius(CURRENT_DRAWING_MODE);

                SKPoint mapPoint = Extensions.ToSKPoint(MapImageBox.PointToImage(new Point((int)cursorPosition.X - brushRadius, (int)cursorPosition.Y - brushRadius)));
                UpdateDrawingPointLabel(cursorPosition, mapPoint);

                if (!CURSOR_POSITION_WORKER.CancellationPending)
                {
                    // Run again
                    CURSOR_POSITION_WORKER.RunWorkerAsync();
                }
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            MapImageBox.Zoom = 100;
            MapImageBox.ScrollTo(0, 0);
        }

        private void ZoomToFitButton_Click(object sender, EventArgs e)
        {
            MapImageBox.ZoomToFit();
            MapImageBox.ScrollTo(0, 0);
        }

        #endregion

        #region Main Menu Event Handlers
        /*******************************************************************************************************
        * Main Menu Event Handlers 
         *******************************************************************************************************/
        private void ExitToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (!CURRENT_MAP.IsSaved)
            {
                TopMost = true;

                DialogResult result =
                    MessageBox.Show("The map has not been saved. Do you want to save the map?", "Exit Application", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                if (result == DialogResult.Yes)
                {
                    DialogResult saveResult = SaveMap();

                    if (saveResult == DialogResult.OK)
                    {
                        Application.Exit();
                    }
                }
                else if (result == DialogResult.No)
                {
                    CURRENT_MAP.IsSaved = true;
                    Application.Exit();
                }

                TopMost = false;
            }
            else
            {
                Application.Exit();
            }
        }

        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CURRENT_MAP.MapPath))
            {
                CreateMap();
            }
            else
            {
                if (SaveMap() == DialogResult.OK)
                {
                    CreateMap();
                }
            }
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveMap();
        }

        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenExistingMap();
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveMap();
        }

        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UndoManager.Undo();
            MapImageBox.Refresh();
        }

        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UndoManager.Redo();
            MapImageBox.Refresh();
        }

        private void PropertiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // handler for Map->Properties menu item

            MapProperties mapProperties = new(CURRENT_MAP);

            // set property defaults
            mapProperties.ShowDialog(this);
        }

        private void ExportToolStripMenuItem_Click(object sender, EventArgs e)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            MapImageBox.Refresh();


            // export the map as a PNG, JPG, or other graphics format
            SaveFileDialog ofd = new()
            {
                Title = "Export Map",
                DefaultExt = "png",
                RestoreDirectory = true,
                ShowHelp = true,
                Filter = "",
                AddExtension = true,
                CheckPathExists = true,
                ShowHiddenFiles = false,
                ValidateNames = true,
            };

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            string sep = string.Empty;

            foreach (var c in codecs)
            {
                string codecName = c.CodecName.Substring(8).Replace("Codec", "Files").Trim();

                ofd.Filter = string.Format("{0}{1}{2} ({3})|{3}", ofd.Filter, sep, codecName, c.FilenameExtension.ToLower());
                sep = "|";
            }

            ofd.Filter = string.Format("{0}{1}{2} ({3})|{3}", ofd.Filter, sep, "All Files", "*.*");

            DialogResult result = ofd.ShowDialog();

            if (result == DialogResult.OK)
            {
                string filename = ofd.FileName;

                Image exportBitmap = (Image)MapImageBox.Image.Clone();

                try
                {
                    exportBitmap.Save(filename);
                    MessageBox.Show("Map exported to " + ofd.FileName, "Map Exported", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
                catch
                {
                    MessageBox.Show("Failed to export map to " + ofd.FileName, "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
            }

#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        private void PrintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // print the map
        }

        private void PrintPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // preview the map as it will be printed
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // cut selected object (a symbol) from the map
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // copy a selected object (a symbol)
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // paste an object (a symbol) onto the map at the cursor position
        }

        private void ChangeSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // change the size in pixels (width and/or height) of the map
        }

        private void DetailMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // create a detail map from a selected region of the current map, upscaling the detail map to the desired size
            // aspect ratio of the selected region vs. the desired size need to be taken into account
        }

        private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // reload assets (textures, symbols, etc.) from files
            LoadAllAssets();
            MessageBox.Show("All assets reloaded", "Assets Reloaded.", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
        }

        private void SelectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // select assets (is this menu option needed?)
        }

        private void AddSymbolCollectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SymbolCollectionForm symbolCollectionForm = new();
            symbolCollectionForm.ShowDialog(this);
        }

        private void WonderdraftAssetZipFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WonderdraftAssetImportDialog dlg = new WonderdraftAssetImportDialog();
            dlg.ShowDialog(this);
        }

        private void WonderdraftUserFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WonderdraftUserFolderImportDlg dlg = new WonderdraftUserFolderImportDlg();
            dlg.ShowDialog(this);
        }

        private void OptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // show application options dialog
        }

        private void ContentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // show application help file (PDF document?)
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // show the application About dialog
            MapCreatorAboutBox aboutBox = new();
            aboutBox.ShowDialog();
        }

        private void ApplyThemeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // show the theme dialog
            ThemeList themeList = new();

            // get the current state of the UI as a theme
            // and send it to the ThemeList dialog
            MapTheme settingsTheme = SaveCurentSettingsToTheme();

            themeList.SetThemes([.. MapPaintMethods.THEME_LIST]);
            themeList.SettingsTheme = settingsTheme;

            DialogResult result = themeList.ShowDialog();

            if (result == DialogResult.OK)
            {
                // on OK result, apply the selected theme
                MapTheme selectedTheme = themeList.GetSelectedTheme();
                ThemeFilter themeFilter = themeList.GetThemeFilter();

                if (selectedTheme != null)
                {
                    MapPaintMethods.CURRENT_THEME = selectedTheme;
                    ApplyTheme(selectedTheme, themeFilter);
                }
            }
        }

        private void PreferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UserPreferences preferences = new UserPreferences();
            preferences.ShowDialog();
        }

        #endregion

        #region Asset Methods
        /******************************************************************************************************
        * *****************************************************************************************************
        * Asset Methods
        * *****************************************************************************************************
        *******************************************************************************************************/

        private void ResetAssets()
        {
            SymbolTagsListBox.Items.Clear();
            MapPaintMethods.BACKGROUND_TEXTURE_LIST.Clear();
            WaterFeatureMethods.WATER_TEXTURE_LIST.Clear();
            LandformType2Methods.LAND_TEXTURE_LIST.Clear();
            LandformType2Methods.HATCH_TEXTURE_LIST.Clear();
            MapPathMethods.PATH_TEXTURE_LIST.Clear();
            MapPathMethods.PATH_VECTOR_LIST.Clear();
            MapPaintMethods.BRUSH_LIST.Clear();
            MapPaintMethods.APPLICATION_ICON_LIST.Clear();
            MapPaintMethods.THEME_LIST.Clear();
            MapLabelMethods.LABEL_PRESETS.Clear();

            backgroundTxBox.Items.Clear();
            OceanTextureList.Items.Clear();
            LandformTextureBox.Items.Clear();
            HatchPatternSelectionBox.Items.Clear();
            PathTextureBox.Items.Clear();
            SymbolCollectionsListBox.Items.Clear();
            LabelPresetCombo.Items.Clear();

            SymbolMethods.ClearSymbolCollections();
        }

        private void LoadAllAssets()
        {
            string assetDirectory = Resources.ASSET_DIRECTORY;

            ResetAssets();

            MapPaintMethods.EYEDROPPER_CURSOR = new Cursor(Resources.Eye_Dropper.Handle);

            // load symbol tags
            SymbolMethods.LoadSymbolTags();

            foreach (string tag in SymbolMethods.GetSymbolTags())
            {
                SymbolTagsListBox.Items.Add(tag);
            }

            // load name generator files
            MapToolMethods.LoadNameGeneratorFiles();

            // load assets
            int numAssets = 0;

            var files = from file in Directory.EnumerateFiles(assetDirectory, "*.*", SearchOption.AllDirectories).Order()
                        where file.Contains(".png")
                            || file.Contains(".jpg")
                            || file.Contains(".ico")
                            || file.Contains(".mctheme")
                            || file.Contains(".svg")
                            || file.Contains(".mclblprst")
                        select new
                        {
                            File = file
                        };

            foreach (var f in files)
            {
                string assetName = Path.GetFileNameWithoutExtension(f.File);
                string path = Path.GetFullPath(f.File);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                if (Path.GetDirectoryName(f.File).EndsWith("Textures\\Background"))
                {
                    MapTexture t = new(assetName, path);
                    MapPaintMethods.AddBackgroundTexture(t);
                    backgroundTxBox.Items.Add(assetName);
                }
                else if (Path.GetDirectoryName(f.File).EndsWith("Textures\\Water"))
                {
                    MapTexture t = new(assetName, path);
                    WaterFeatureMethods.WATER_TEXTURE_LIST.Add(t);
                    OceanTextureList.Items.Add(assetName);
                }
                else if (Path.GetDirectoryName(f.File).EndsWith("Textures\\Land"))
                {
                    MapTexture t = new(assetName, path);
                    LandformType2Methods.LAND_TEXTURE_LIST.Add(t);

                    LandformTextureBox.Items.Add(assetName);
                }
                else if (Path.GetDirectoryName(f.File).EndsWith("Textures\\Hatch"))
                {
                    MapTexture t = new(assetName, path);
                    LandformType2Methods.HATCH_TEXTURE_LIST.Add(t);

                    HatchPatternSelectionBox.Items.Add(assetName);
                }
                else if (Path.GetDirectoryName(f.File).EndsWith("Textures\\Path"))
                {
                    MapTexture t = new(assetName, path);
                    MapPathMethods.PATH_TEXTURE_LIST.Add(t);
                    PathTextureBox.Items.Add(assetName);
                }
                else if (Path.GetDirectoryName(f.File).EndsWith("Vectors\\Path"))
                {
                    MapVector v = new(assetName, path);

                    XDocument svgXmlRoot = XDocument.Load(v.VectorPath, LoadOptions.None);

                    if (svgXmlRoot != null)
                    {
                        XElement? rootElement = svgXmlRoot.Root;

                        if (rootElement != null)
                        {
                            string? viewBoxAttr = (string?)rootElement.Attribute("viewBox");

                            if (viewBoxAttr != null)
                            {
                                string[] viewBoxElements = viewBoxAttr.Split(' ');

                                v.ViewBoxSizeWidth = float.Parse(viewBoxElements[2]);
                                v.ViewBoxSizeHeight = float.Parse(viewBoxElements[3]);
                            }
                            else
                            {
                                v.ViewBoxSizeWidth = 0.0F;
                                v.ViewBoxSizeHeight = 0.0F;
                            }

                            var nodes = rootElement.Descendants();

                            foreach (var n in nodes)
                            {
                                if (n.Name.LocalName == "path")
                                {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                                    v.VectorSvg += " " + (string)n.Attribute("d");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                                }
                            }

                            if (v.VectorSvg != null && v.VectorSvg.Length > 0)
                            {
                                v.VectorSkPath = SKPath.ParseSvgPathData(v.VectorSvg);
                            }

                            MapPathMethods.PATH_VECTOR_LIST.Add(v);
                        }

                    }
                }
                else if (Path.GetDirectoryName(f.File).EndsWith("\\Brushes"))
                {
                    MapBrush mb = new() { BrushPath = path, BrushName = assetName };

                    MapPaintMethods.BRUSH_LIST.Add(mb);
                }
                else if (Path.GetDirectoryName(f.File).EndsWith("\\Icons"))
                {
                    ApplicationIcon icon = new()
                    {
                        IconName = assetName,
                        IconPath = path,
                        IconCursor = new Cursor(path)
                    };

                    MapPaintMethods.APPLICATION_ICON_LIST.Add(icon);
                }
                else if (Path.GetDirectoryName(f.File).EndsWith("\\Themes"))
                {
                    MapTheme? t = MapFileMethods.ReadThemeFromXml(path);

                    if (t != null)
                    {
                        MapPaintMethods.THEME_LIST.Add(t);

                        if (t.IsDefaultTheme)
                        {
                            ThemeFilter tf = new();
                            ApplyTheme(t, tf);
                            MapPaintMethods.CURRENT_THEME = t;
                        }
                    }
                }
                else if (Path.GetDirectoryName(f.File).EndsWith("\\Stamps"))
                {
                }
                else if (Path.GetDirectoryName(f.File).EndsWith("\\Boxes"))
                {
                    MapBox b = new()
                    {
                        BoxName = assetName,
                        BoxBitmapPath = path
                    };

                    MapLabelMethods.MAP_BOX_TEXTURES.Add(b);
                }
                else if (Path.GetDirectoryName(f.File).EndsWith("\\LabelPresets"))
                {
                    LabelPreset? preset = MapFileMethods.ReadLabelPreset(path);

                    if (preset != null)
                    {
                        MapLabelMethods.LABEL_PRESETS.Add(preset);
                    }
                }

#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }

            foreach (LabelPreset preset in MapLabelMethods.LABEL_PRESETS)
            {
                if (!string.IsNullOrEmpty(preset.LabelPresetTheme)
                    && MapPaintMethods.CURRENT_THEME != null
                    && preset.LabelPresetTheme == MapPaintMethods.CURRENT_THEME.ThemeName)
                {
                    LabelPresetCombo.Items.Add(preset.LabelPresetName);
                }
            }

            if (LabelPresetCombo.Items.Count > 0)
            {
                LabelPresetCombo.SelectedIndex = 0;

                LabelPreset? selectedPreset = MapLabelMethods.LABEL_PRESETS.Find(x => !string.IsNullOrEmpty((string?)LabelPresetCombo.Items[0]) && x.LabelPresetName == (string?)LabelPresetCombo.Items[0]);

                if (selectedPreset != null)
                {
                    SetLabelValuesFromPreset(selectedPreset);
                }
            }

            numAssets += files.Count();

            int numSymbols = SymbolMethods.LoadSymbolCollections();

            numAssets += numSymbols;

            int numFrames = OverlayMethods.LoadFrameAssets();

            numAssets += numFrames;

            SetStatusText("Loaded: " + numAssets + " assets.");

            foreach (MapSymbolCollection collection in SymbolMethods.GetSymbolCollections())
            {
                SymbolCollectionsListBox.Items.Add(collection.GetCollectionName());
            }

            AddMapBoxesToLabelBoxTable(MapLabelMethods.MAP_BOX_TEXTURES);

            AddMapFramesToFrameTable(OverlayMethods.MAP_FRAME_TEXTURES);

            Refresh();
        }

        #endregion

        #region Theme Methods
        /******************************************************************************************************
        * *****************************************************************************************************
        * Theme Methods
        * *****************************************************************************************************
        *******************************************************************************************************/

        private MapTheme SaveCurentSettingsToTheme()
        {
            MapTheme theme = new();

            // label presets for the theme are serialized to a file at the time they are created
            // and loaded when the theme is loaded/selected; they are not stored with the theme

            // background
            if (backgroundTxBox.SelectedIndex > -1)
            {
                theme.BackgroundTexture = MapPaintMethods.GetBackgroundTextureList()[backgroundTxBox.SelectedIndex];
            }

            // vignette color and strength
            theme.VignetteColor = (XmlColor)VignetteColorSelectionLabel.BackColor;
            theme.VignetteStrength = VignetteStrengthScroll.Value;

            // ocean
            if (OceanTextureList.SelectedIndex > -1)
            {
                theme.OceanTexture = WaterFeatureMethods.WATER_TEXTURE_LIST[OceanTextureList.SelectedIndex];
            }

            theme.OceanTextureOpacity = OceanTextureOpacityTrack.Value;
            theme.OceanColor = OceanColorSelectionLabel.BackColor;

            // save ocean custom colors
            theme.OceanColorPalette.Add(OceanCustomColorButton1.BackColor);
            theme.OceanColorPalette.Add(OceanCustomColorButton2.BackColor);
            theme.OceanColorPalette.Add(OceanCustomColorButton3.BackColor);
            theme.OceanColorPalette.Add(OceanCustomColorButton4.BackColor);
            theme.OceanColorPalette.Add(OceanCustomColorButton5.BackColor);
            theme.OceanColorPalette.Add(OceanCustomColorButton6.BackColor);
            theme.OceanColorPalette.Add(OceanCustomColorButton7.BackColor);
            theme.OceanColorPalette.Add(OceanCustomColorButton8.BackColor);

            // landform
            theme.LandformOutlineColor = LandOutlineColorSelectionLabel.BackColor;
            theme.LandformOutlineWidth = LandOutlineWidthScroll.Value;

            if (LandformTextureBox.SelectedIndex > -1)
            {
                theme.LandformTexture = LandformType2Methods.LAND_TEXTURE_LIST[LandformTextureBox.SelectedIndex];
            }

            if (ShorelineStyleBox.SelectedIndex > -1)
            {
                theme.LandShorelineStyle = (string?)ShorelineStyleBox.Items[ShorelineStyleBox.SelectedIndex];
            }

            theme.LandformCoastlineColor = CoastColorSelectionLabel.BackColor;
            theme.LandformCoastlineEffectDistance = FxDistanceTrack.Value;

            if (CoastStyleSelectionBox.SelectedIndex > -1)
            {
                theme.LandformCoastlineStyle = (string?)CoastStyleSelectionBox.Items[CoastStyleSelectionBox.SelectedIndex];
            }

            // save land custom colors
            theme.LandformColorPalette.Add(LandCustomColorButton1.BackColor);
            theme.LandformColorPalette.Add(LandCustomColorButton2.BackColor);
            theme.LandformColorPalette.Add(LandCustomColorButton3.BackColor);
            theme.LandformColorPalette.Add(LandCustomColorButton4.BackColor);
            theme.LandformColorPalette.Add(LandCustomColorButton5.BackColor);
            theme.LandformColorPalette.Add(LandCustomColorButton6.BackColor);

            // freshwater
            theme.FreshwaterColor = WaterColorSelectionLabel.BackColor;
            theme.FreshwaterShorelineColor = ShorelineColorSelectionLabel.BackColor;

            theme.RiverWidth = RiverWidthTrack.Value;
            theme.RiverSourceFadeIn = RiverSourceFadeInCheck.Checked;

            // save freshwater custom colors
            theme.FreshwaterColorPalette.Add(WaterCustomColor1.BackColor);
            theme.FreshwaterColorPalette.Add(WaterCustomColor2.BackColor);
            theme.FreshwaterColorPalette.Add(WaterCustomColor3.BackColor);
            theme.FreshwaterColorPalette.Add(WaterCustomColor4.BackColor);
            theme.FreshwaterColorPalette.Add(WaterCustomColor5.BackColor);
            theme.FreshwaterColorPalette.Add(WaterCustomColor6.BackColor);
            theme.FreshwaterColorPalette.Add(WaterCustomColor7.BackColor);
            theme.FreshwaterColorPalette.Add(WaterCustomColor8.BackColor);

            // path
            theme.PathColor = PathColorSelectionLabel.BackColor;
            theme.PathWidth = PathWidthTrack.Value;

            // symbols
            theme.SymbolCustomColors[0] = (XmlColor)SymbolColor1Label.BackColor;
            theme.SymbolCustomColors[1] = (XmlColor)SymbolColor2Label.BackColor;
            theme.SymbolCustomColors[2] = (XmlColor)SymbolColor3Label.BackColor;
            theme.SymbolCustomColors[3] = (XmlColor)SymbolColor4Label.BackColor;

            return theme;
        }

        private void ApplyTheme(MapTheme theme, ThemeFilter themeFilter)
        {
            if (theme == null || themeFilter == null) return;

            if (themeFilter.ApplyBackgroundSettings)
            {
                if (theme.BackgroundTexture != null)
                {
                    for (int i = 0; i < MapPaintMethods.GetBackgroundTextureList().Count; i++)
                    {
                        if (MapPaintMethods.GetBackgroundTextureList()[i].TextureName == theme.BackgroundTexture.TextureName)
                        {
                            backgroundTxBox.SelectedIndex = i;
                            break;
                        }
                    }

                    backgroundTxBox.Refresh();
                }

                if (theme.VignetteColor != null)
                {
                    VignetteColorSelectionLabel.BackColor = (Color)theme.VignetteColor;
                    VignetteColorSelectionLabel.Refresh();
                }

                if (theme.VignetteStrength != null)
                {
                    VignetteStrengthScroll.Value = (int)theme.VignetteStrength;
                    VignetteStrengthScroll.Refresh();
                }

                for (int i = 0; i < MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.VIGNETTELAYER).MapLayerComponents.Count; i++)
                {
                    if (MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.VIGNETTELAYER).MapLayerComponents[i] is MapVignette v)
                    {
                        v.VignetteColor = VignetteColorSelectionLabel.BackColor;
                        v.VignetteStrength = VignetteStrengthScroll.Value;
                    }
                }
            }

            if (themeFilter.ApplyOceanSettings)
            {
                List<MapTexture> waterTextureList = WaterFeatureMethods.WATER_TEXTURE_LIST;
                for (int i = 0; i < waterTextureList.Count; i++)
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    if (waterTextureList[i].TextureName == theme.OceanTexture.TextureName)
                    {
                        OceanTextureList.SelectedIndex = i;
                        break;
                    }

                    OceanTextureList.Refresh();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }

                if (theme.OceanTextureOpacity != null)
                {
                    OceanTextureOpacityTrack.Value = (int)theme.OceanTextureOpacity;
                    OceanTextureOpacityTrack.Refresh();
                }

                if (theme.OceanColor != null)
                {
                    OceanColorSelectionLabel.BackColor = (Color)theme.OceanColor;
                    OceanColorSelectionLabel.Refresh();
                }
            }

            if (themeFilter.ApplyOceanColorPaletteSettings)
            {
                if (theme.OceanColorPalette.Count > 0)
                {
                    OceanCustomColorButton1.BackColor = theme.OceanColorPalette[0];
                    OceanCustomColorButton1.ForeColor = SystemColors.ControlDark;
                    OceanCustomColorButton1.Text = ColorTranslator.ToHtml(OceanCustomColorButton1.BackColor);

                    OceanCustomColorButton1.Refresh();
                }

                if (theme.OceanColorPalette.Count > 1)
                {
                    OceanCustomColorButton2.BackColor = theme.OceanColorPalette[1];
                    OceanCustomColorButton2.ForeColor = SystemColors.ControlDark;
                    OceanCustomColorButton2.Text = ColorTranslator.ToHtml(OceanCustomColorButton2.BackColor);

                    OceanCustomColorButton2.Refresh();
                }

                if (theme.OceanColorPalette.Count > 2)
                {
                    OceanCustomColorButton3.BackColor = theme.OceanColorPalette[2];
                    OceanCustomColorButton3.ForeColor = SystemColors.ControlDark;
                    OceanCustomColorButton3.Text = ColorTranslator.ToHtml(OceanCustomColorButton3.BackColor);

                    OceanCustomColorButton3.Refresh();
                }

                if (theme.OceanColorPalette.Count > 3)
                {
                    OceanCustomColorButton4.BackColor = theme.OceanColorPalette[3];
                    OceanCustomColorButton4.ForeColor = SystemColors.ControlDark;
                    OceanCustomColorButton4.Text = ColorTranslator.ToHtml(OceanCustomColorButton4.BackColor);

                    OceanCustomColorButton4.Refresh();
                }

                if (theme.OceanColorPalette.Count > 4)
                {
                    OceanCustomColorButton5.BackColor = theme.OceanColorPalette[4];
                    OceanCustomColorButton5.ForeColor = SystemColors.ControlDark;
                    OceanCustomColorButton5.Text = ColorTranslator.ToHtml(OceanCustomColorButton5.BackColor);

                    OceanCustomColorButton5.Refresh();
                }

                if (theme.OceanColorPalette.Count > 5)
                {
                    OceanCustomColorButton6.BackColor = theme.OceanColorPalette[5];
                    OceanCustomColorButton6.ForeColor = SystemColors.ControlDark;
                    OceanCustomColorButton6.Text = ColorTranslator.ToHtml(OceanCustomColorButton6.BackColor);

                    OceanCustomColorButton6.Refresh();
                }

                if (theme.OceanColorPalette.Count > 6)
                {
                    OceanCustomColorButton7.BackColor = theme.OceanColorPalette[6];
                    OceanCustomColorButton7.ForeColor = SystemColors.ControlDark;
                    OceanCustomColorButton7.Text = ColorTranslator.ToHtml(OceanCustomColorButton7.BackColor);

                    OceanCustomColorButton7.Refresh();
                }

                if (theme.OceanColorPalette.Count > 7)
                {
                    OceanCustomColorButton8.BackColor = theme.OceanColorPalette[7];
                    OceanCustomColorButton8.ForeColor = SystemColors.ControlDark;
                    OceanCustomColorButton8.Text = ColorTranslator.ToHtml(OceanCustomColorButton8.BackColor);

                    OceanCustomColorButton8.Refresh();
                }
            }

            if (themeFilter.ApplyLandSettings)
            {
                if (theme.LandformOutlineColor != null)
                {
                    LandOutlineColorSelectionLabel.BackColor = (Color)theme.LandformOutlineColor;
                    LandOutlineColorSelectionLabel.Refresh();
                }

                if (theme.LandformOutlineWidth != null)
                {
                    LandOutlineWidthScroll.Value = (int)theme.LandformOutlineWidth;
                    LandOutlineWidthScroll.Refresh();
                }

                List<MapTexture> landTextureList = LandformType2Methods.LAND_TEXTURE_LIST;

                for (int i = 0; i < landTextureList.Count; i++)
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    if (landTextureList[i].TextureName == theme.LandformTexture.TextureName)
                    {
                        LandformTextureBox.SelectedIndex = i;

                        Bitmap b = new(theme.LandformTexture.TexturePath);
                        Bitmap resizedBitmap = new(b, CURRENT_MAP.MapWidth, CURRENT_MAP.MapHeight);

                        // create and set a shader from the selected texture

                        SKShader lfpShader = SKShader.CreateBitmap(Extensions.ToSKBitmap(resizedBitmap),
                            SKShaderTileMode.Mirror, SKShaderTileMode.Mirror);

                        LandformType2Methods.LAND_FILL_PAINT.Shader = lfpShader;
                        LandformType2Methods.LAND_FILL_PAINT.BlendMode = SKBlendMode.DstATop;

                        break;
                    }

                    LandformTextureBox.Refresh();

#pragma warning restore CS8602 // Dereference of a possibly null reference.
                }

                if (theme.LandShorelineStyle != null)
                {
                    for (int i = 0; i < ShorelineStyleBox.Items.Count; i++)
                    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        if (ShorelineStyleBox.Items[i].ToString() == theme.LandShorelineStyle)
                        {
                            ShorelineStyleBox.SelectedIndex = i;
                            break;
                        }

                        ShorelineStyleBox.Refresh();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    }
                }

                if (theme.LandformCoastlineColor != null)
                {
                    CoastColorSelectionLabel.BackColor = (Color)theme.LandformCoastlineColor;
                    CoastColorSelectionLabel.Refresh();
                }

                if (theme.LandformCoastlineEffectDistance != null)
                {
                    FxDistanceTrack.Value = (int)theme.LandformCoastlineEffectDistance;
                    FxDistanceTrack.Refresh();

                    FxDistanceLabel.Text = theme.LandformCoastlineEffectDistance.ToString();
                    FxDistanceLabel.Refresh();
                }

                if (theme.LandformCoastlineStyle != null)
                {
                    for (int i = 0; i < CoastStyleSelectionBox.Items.Count; i++)
                    {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        if (CoastStyleSelectionBox.Items[i].ToString() == theme.LandformCoastlineStyle)
                        {
                            CoastStyleSelectionBox.SelectedIndex = i;
                            break;
                        }

                        CoastStyleSelectionBox.Refresh();

#pragma warning restore CS8602 // Dereference of a possibly null reference.
                    }
                }
            }

            if (themeFilter.ApplyLandformColorPaletteSettings)
            {
                if (theme.LandformColorPalette.Count > 0)
                {
                    LandCustomColorButton1.BackColor = theme.LandformColorPalette[0];
                    LandCustomColorButton1.ForeColor = SystemColors.ControlDark;
                    LandCustomColorButton1.Text = ColorTranslator.ToHtml(LandCustomColorButton1.BackColor);

                    LandCustomColorButton1.Refresh();
                }

                if (theme.LandformColorPalette.Count > 1)
                {
                    LandCustomColorButton2.BackColor = theme.LandformColorPalette[1];
                    LandCustomColorButton2.ForeColor = SystemColors.ControlDark;
                    LandCustomColorButton2.Text = ColorTranslator.ToHtml(LandCustomColorButton2.BackColor);

                    LandCustomColorButton2.Refresh();
                }

                if (theme.LandformColorPalette.Count > 2)
                {
                    LandCustomColorButton3.BackColor = theme.LandformColorPalette[2];
                    LandCustomColorButton3.ForeColor = SystemColors.ControlDark;
                    LandCustomColorButton3.Text = ColorTranslator.ToHtml(LandCustomColorButton3.BackColor);

                    LandCustomColorButton3.Refresh();
                }

                if (theme.LandformColorPalette.Count > 3)
                {
                    LandCustomColorButton4.BackColor = theme.LandformColorPalette[3];
                    LandCustomColorButton4.ForeColor = SystemColors.ControlDark;
                    LandCustomColorButton4.Text = ColorTranslator.ToHtml(LandCustomColorButton4.BackColor);

                    LandCustomColorButton4.Refresh();
                }

                if (theme.LandformColorPalette.Count > 4)
                {
                    LandCustomColorButton5.BackColor = theme.LandformColorPalette[4];
                    LandCustomColorButton5.ForeColor = SystemColors.ControlDark;
                    LandCustomColorButton5.Text = ColorTranslator.ToHtml(LandCustomColorButton5.BackColor);

                    LandCustomColorButton5.Refresh();
                }

                if (theme.LandformColorPalette.Count > 5)
                {
                    LandCustomColorButton6.BackColor = theme.LandformColorPalette[5];
                    LandCustomColorButton6.ForeColor = SystemColors.ControlDark;
                    LandCustomColorButton6.Text = ColorTranslator.ToHtml(LandCustomColorButton6.BackColor);

                    LandCustomColorButton6.Refresh();
                }
            }

            if (themeFilter.ApplyFreshwaterSettings)
            {
                if (theme.FreshwaterColor != null)
                {
                    WaterColorSelectionLabel.BackColor = (Color)theme.FreshwaterColor;
                    WaterColorSelectionLabel.Refresh();
                }
                else
                {
                    WaterColorSelectionLabel.BackColor = WaterFeatureMethods.DEFAULT_WATER_COLOR;
                    WaterColorSelectionLabel.Refresh();
                }

                if (theme.FreshwaterShorelineColor != null)
                {
                    ShorelineColorSelectionLabel.BackColor = (Color)theme.FreshwaterShorelineColor;
                    ShorelineColorSelectionLabel.Refresh();
                }
                else
                {
                    ShorelineColorSelectionLabel.BackColor = WaterFeatureMethods.DEFAULT_WATER_OUTLINE_COLOR;
                    ShorelineColorSelectionLabel.Refresh();
                }

                if (theme.RiverWidth != null)
                {
                    RiverWidthTrack.Value = (int)theme.RiverWidth;
                    RiverWidthTrack.Refresh();

                    RiverWidthLabel.Text = theme.RiverWidth.ToString();
                    RiverWidthLabel.Refresh();
                }

                if (theme.RiverSourceFadeIn != null)
                {
                    RiverSourceFadeInCheck.Checked = (bool)theme.RiverSourceFadeIn;
                }
            }
            else
            {
                ShorelineColorSelectionLabel.BackColor = WaterFeatureMethods.DEFAULT_WATER_OUTLINE_COLOR;
                ShorelineColorSelectionLabel.Refresh();

                WaterColorSelectionLabel.BackColor = WaterFeatureMethods.DEFAULT_WATER_COLOR;
                WaterColorSelectionLabel.Refresh();
            }

            if (themeFilter.ApplyFreshwaterColorPaletteSettings)
            {
                if (theme.FreshwaterColorPalette.Count > 0)
                {
                    WaterCustomColor1.BackColor = theme.FreshwaterColorPalette[0];
                    WaterCustomColor1.ForeColor = SystemColors.ControlDark;
                    WaterCustomColor1.Text = ColorTranslator.ToHtml(WaterCustomColor1.BackColor);

                    WaterCustomColor1.Refresh();
                }

                if (theme.FreshwaterColorPalette.Count > 1)
                {
                    WaterCustomColor2.BackColor = theme.FreshwaterColorPalette[1];
                    WaterCustomColor2.ForeColor = SystemColors.ControlDark;
                    WaterCustomColor2.Text = ColorTranslator.ToHtml(WaterCustomColor2.BackColor);

                    WaterCustomColor2.Refresh();
                }

                if (theme.FreshwaterColorPalette.Count > 2)
                {
                    WaterCustomColor3.BackColor = theme.FreshwaterColorPalette[2];
                    WaterCustomColor3.ForeColor = SystemColors.ControlDark;
                    WaterCustomColor3.Text = ColorTranslator.ToHtml(WaterCustomColor3.BackColor);

                    WaterCustomColor3.Refresh();
                }

                if (theme.FreshwaterColorPalette.Count > 3)
                {
                    WaterCustomColor4.BackColor = theme.FreshwaterColorPalette[3];
                    WaterCustomColor4.ForeColor = SystemColors.ControlDark;
                    WaterCustomColor4.Text = ColorTranslator.ToHtml(WaterCustomColor4.BackColor);

                    WaterCustomColor4.Refresh();
                }

                if (theme.FreshwaterColorPalette.Count > 4)
                {
                    WaterCustomColor5.BackColor = theme.FreshwaterColorPalette[4];
                    WaterCustomColor5.ForeColor = SystemColors.ControlDark;
                    WaterCustomColor5.Text = ColorTranslator.ToHtml(WaterCustomColor5.BackColor);

                    WaterCustomColor5.Refresh();
                }

                if (theme.FreshwaterColorPalette.Count > 5)
                {
                    WaterCustomColor6.BackColor = theme.FreshwaterColorPalette[5];
                    WaterCustomColor6.ForeColor = SystemColors.ControlDark;
                    WaterCustomColor6.Text = ColorTranslator.ToHtml(WaterCustomColor6.BackColor);

                    WaterCustomColor6.Refresh();
                }

                if (theme.FreshwaterColorPalette.Count > 6)
                {
                    WaterCustomColor7.BackColor = theme.FreshwaterColorPalette[6];
                    WaterCustomColor7.ForeColor = SystemColors.ControlDark;
                    WaterCustomColor7.Text = ColorTranslator.ToHtml(WaterCustomColor7.BackColor);

                    WaterCustomColor7.Refresh();
                }

                if (theme.FreshwaterColorPalette.Count > 7)
                {
                    WaterCustomColor8.BackColor = theme.FreshwaterColorPalette[7];
                    WaterCustomColor8.ForeColor = SystemColors.ControlDark;
                    WaterCustomColor8.Text = ColorTranslator.ToHtml(WaterCustomColor8.BackColor);

                    WaterCustomColor8.Refresh();
                }
            }

            if (themeFilter.ApplyPathSetSettings)
            {
                if (theme.PathColor != null && !((Color)theme.PathColor).IsEmpty)
                {
                    PathColorSelectionLabel.BackColor = (Color)theme.PathColor;
                    PathColorSelectionLabel.Refresh();
                }

                if (theme.PathWidth != null)
                {
                    PathWidthTrack.Value = (int)theme.PathWidth;
                    PathWidthTrack.Refresh();
                }
            }

            if (themeFilter.ApplySymbolSettings)
            {
                if (theme.SymbolCustomColors != null && theme.SymbolCustomColors[0] != Color.Empty)
                {
                    SymbolColor1Label.BackColor = theme.SymbolCustomColors[0];
                    SymbolColor1Label.Refresh();

                    SymbolMethods.SetCustomColorAtIndex(Extensions.ToSKColor(theme.SymbolCustomColors[0]), 0);
                }

                if (theme.SymbolCustomColors != null && theme.SymbolCustomColors[1] != Color.Empty)
                {
                    SymbolColor2Label.BackColor = theme.SymbolCustomColors[1];
                    SymbolColor2Label.Refresh();

                    SymbolMethods.SetCustomColorAtIndex(Extensions.ToSKColor(theme.SymbolCustomColors[1]), 1);
                }

                if (theme.SymbolCustomColors != null && theme.SymbolCustomColors[2] != Color.Empty)
                {
                    SymbolColor3Label.BackColor = theme.SymbolCustomColors[2];
                    SymbolColor3Label.Refresh();

                    SymbolMethods.SetCustomColorAtIndex(Extensions.ToSKColor(theme.SymbolCustomColors[2]), 2);
                }

                if (theme.SymbolCustomColors != null && theme.SymbolCustomColors[3] != Color.Empty)
                {
                    SymbolColor4Label.BackColor = theme.SymbolCustomColors[3];
                    SymbolColor4Label.Refresh();

                    SymbolMethods.SetCustomColorAtIndex(Extensions.ToSKColor(theme.SymbolCustomColors[3]), 3);
                }
            }

            if (themeFilter.ApplyLabelPresetSettings)
            {
                LabelPresetCombo.Items.Clear();

                foreach (LabelPreset preset in MapLabelMethods.LABEL_PRESETS)
                {
                    if (!string.IsNullOrEmpty(preset.LabelPresetTheme)
                        && MapPaintMethods.CURRENT_THEME != null
                        && preset.LabelPresetTheme == MapPaintMethods.CURRENT_THEME.ThemeName)
                    {
                        LabelPresetCombo.Items.Add(preset.LabelPresetName);
                    }
                }

                if (LabelPresetCombo.Items.Count > 0)
                {
                    LabelPresetCombo.SelectedIndex = 0;

                    LabelPreset? selectedPreset = MapLabelMethods.LABEL_PRESETS.Find(x => !string.IsNullOrEmpty((string?)LabelPresetCombo.Items[0]) && x.LabelPresetName == (string?)LabelPresetCombo.Items[0]);

                    if (selectedPreset != null)
                    {
                        SetLabelValuesFromPreset(selectedPreset);
                    }
                }

                LabelPresetCombo.Refresh();
            }
        }

        #endregion

        #region Layer Select Tab
        /******************************************************************************************************
        * *****************************************************************************************************
        * Layer Select Tab Event Handlers
        * *****************************************************************************************************
        *******************************************************************************************************/

        private void LayerSelectTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            // clear the drawing mode (and uncheck all drawing, paint, and erase buttons) when switching tabs
            SetDrawingMode(DrawingModeEnum.None, null);

            BackgroundToolPanel.Visible = true;
            OceanColorToolPanel.Visible = false;
            LandColorToolPanel.Visible = false;
            WaterColorToolPanel.Visible = false;
            PathPanel.Visible = false;
            SymbolsToolPanel.Visible = false;
            LabelsToolPanel.Visible = false;
            OverlayToolsPanel.Visible = false;
            RegionsToolPanel.Visible = false;


            switch (LayerSelectTabControl.SelectedIndex)
            {
                case 0:
                    BackgroundToolPanel.Visible = true;
                    break;
                case 1:
                    OceanColorToolPanel.Visible = true;
                    BackgroundToolPanel.Visible = false;
                    break;
                case 2:
                    LandColorToolPanel.Visible = true;
                    BackgroundToolPanel.Visible = false;
                    break;
                case 3:
                    WaterColorToolPanel.Visible = true;
                    BackgroundToolPanel.Visible = false;
                    break;
                case 4:
                    PathPanel.Visible = true;
                    BackgroundToolPanel.Visible = false;
                    break;
                case 5:
                    SymbolsToolPanel.Visible = true;
                    BackgroundToolPanel.Visible = false;
                    break;
                case 6:
                    LabelsToolPanel.Visible = true;
                    BackgroundToolPanel.Visible = false;
                    break;
                case 7:
                    OverlayToolsPanel.Visible = true;
                    BackgroundToolPanel.Visible = false;
                    break;
                case 8:
                    RegionsToolPanel.Visible = true;
                    BackgroundToolPanel.Visible = false;
                    break;
                case 9:
                    BackgroundToolPanel.Visible = true;
                    break;
            }

            Refresh();
        }

        private void ColorPresetButtonMouseClickHandler(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (ModifierKeys == Keys.Control)
                {
                    ResetColorPresetButton((Button)sender);
                }
                else
                {
                    if (LayerSelectTabControl.SelectedTab != null)
                    {
                        if (LayerSelectTabControl.SelectedTab.Text == "Ocean")
                        {
                            SetOceanPaintColorFromCustomPresetButton((Button)sender);
                        }
                        else if (LayerSelectTabControl.SelectedTab.Text == "Land")
                        {
                            SetLandPaintColorFromCustomPresetButton((Button)sender);
                        }
                        else if (LayerSelectTabControl.SelectedTab.Text == "Water")
                        {
                            SetWaterPaintColorFromCustomPresetButton((Button)sender);
                        }

                        ((Button)sender).Refresh();
                    }
                }
            }
        }

        #endregion

        #region Background Tab

        /******************************************************************************************************
        * *****************************************************************************************************
        * Background Layer Methods
        * *****************************************************************************************************
        *******************************************************************************************************/

        /*******************************************************************************************************
         * Background Layer Control Event Handlers
        *******************************************************************************************************/
        private void BackgroundTxBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int textureIndex = backgroundTxBox.SelectedIndex;

            if (textureIndex >= 0)
            {
                backgroundTxPictureBox.Image = new Bitmap(MapPaintMethods.GetBackgroundTextureList()[textureIndex].TexturePath);
            }
            else
            {
                backgroundTxPictureBox.Image = null;
            }

            backgroundTxPictureBox.Refresh();
        }

        private void FillBackgroundButton_Click(object sender, EventArgs e)
        {
            int textureIndex = backgroundTxBox.SelectedIndex;

            if (textureIndex >= 0)
            {
                MapLayer backgroundLayer = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.BASELAYER);

                if (backgroundLayer.MapLayerComponents.Count() <= 1)
                {
                    Bitmap baseBitmap = new(MapPaintMethods.GetBackgroundTextureList()[textureIndex].TexturePath);

                    Bitmap resizedBitmap = new(baseBitmap, CURRENT_MAP.MapWidth, CURRENT_MAP.MapHeight);

                    Cmd_SetBackgroundBitmap cmd = new(CURRENT_MAP, Extensions.ToSKBitmap(resizedBitmap));
                    UndoManager.AddCommand(cmd);
                    cmd.DoOperation();
                }

                if (backgroundLayer.MapLayerComponents.Count() > 1)
                {
                    MapBitmap backgroundBitmap = (MapBitmap)MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.BASELAYER).MapLayerComponents[1];
                    backgroundBitmap.Show = ShowBackgroundLayerCheck.Checked;
                }

                MapImageBox.Refresh();
            }
        }

        private void ClearBackgroundButton_Click(object sender, EventArgs e)
        {
            CURRENT_MAP.IsSaved = false;

            MapLayer backgroundLayer = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.BASELAYER);

            if (backgroundLayer.MapLayerComponents.Count > 1)
            {
                MapBitmap layerBitmap = (MapBitmap)MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.BASELAYER).MapLayerComponents[1];

                Cmd_ClearBackgroundBitmap cmd = new(CURRENT_MAP, layerBitmap);
                UndoManager.AddCommand(cmd);
                cmd.DoOperation();
            }

            MapImageBox.Refresh();
        }

        private void VignetteColorSelectionLabel_Click(object sender, EventArgs e)
        {
            Color selectedColor = MapPaintMethods.SelectColorFromDialog(this, VignetteColorSelectionLabel.BackColor);

            if (selectedColor != Color.Empty)
            {
                VignetteColorSelectionLabel.BackColor = selectedColor;
                VignetteColorSelectionLabel.Refresh();

                for (int i = 0; i < MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.VIGNETTELAYER).MapLayerComponents.Count; i++)
                {
                    if (MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.VIGNETTELAYER).MapLayerComponents[i] is MapVignette v)
                    {
                        v.VignetteColor = selectedColor;
                    }
                }
            }

            MapImageBox.Refresh();
        }

        private void VignetteStrengthScroll_Scroll(object sender, EventArgs e)
        {
            for (int i = 0; i < MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.VIGNETTELAYER).MapLayerComponents.Count; i++)
            {
                if (MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.VIGNETTELAYER).MapLayerComponents[i] is MapVignette v)
                {
                    v.VignetteStrength = VignetteStrengthScroll.Value;
                }
            }

            MapImageBox.Refresh();
        }

        private void ShowBackgroundLayerCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (ShowBackgroundLayerCheck.Checked)
            {
                MapLayer backgroundLayer = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.BASELAYER);

                if (backgroundLayer.MapLayerComponents.Count() > 1)
                {
                    MapBitmap layerBitmap = (MapBitmap)MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.BASELAYER).MapLayerComponents[1];
                    layerBitmap.Show = true;
                }

                for (int i = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.VIGNETTELAYER).MapLayerComponents.Count - 1; i > 0; i--)
                {
                    if (MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.VIGNETTELAYER).MapLayerComponents[i] is MapVignette)
                    {
                        MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.VIGNETTELAYER).MapLayerComponents.RemoveAt(i);
                    }
                }

                MapLayer vignetteLayer = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.VIGNETTELAYER);
                MapBitmap vignetteBitmap = (MapBitmap)vignetteLayer.MapLayerComponents[0];

                vignetteBitmap.MCanvas?.Clear(SKColors.Transparent);

                MapVignette vignette = new(CURRENT_MAP)
                {
                    VignetteColor = VignetteColorSelectionLabel.BackColor,
                    VignetteStrength = VignetteStrengthScroll.Value
                };

                MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.VIGNETTELAYER).MapLayerComponents.Add(vignette);
            }
            else
            {
                MapLayer baseLayer = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.BASELAYER);
                MapBitmap baseBitmap = (MapBitmap)baseLayer.MapLayerComponents[0];

                baseBitmap.MCanvas?.Clear(SKColors.White);

                if (baseLayer.MapLayerComponents.Count() > 1)
                {
                    MapBitmap backgroundBitmap = (MapBitmap)MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.BASELAYER).MapLayerComponents[1];
                    backgroundBitmap.Show = false;
                }

                MapLayer vignetteLayer = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.VIGNETTELAYER);
                MapBitmap vignetteBitmap = (MapBitmap)vignetteLayer.MapLayerComponents[0];
                vignetteBitmap.Show = false;

                for (int i = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.VIGNETTELAYER).MapLayerComponents.Count - 1; i > 0; i--)
                {
                    if (MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.VIGNETTELAYER).MapLayerComponents[i] is MapVignette)
                    {
                        MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.VIGNETTELAYER).MapLayerComponents.RemoveAt(i);
                    }
                }
            }


            MapImageBox.Refresh();
        }

        #endregion

        #region Ocean Tab

        #region Ocean Tab Methods (Wind Rose Methods)
        /******************************************************************************************************
        * *****************************************************************************************************
        * Ocean Tab Methods
        * *****************************************************************************************************
        *******************************************************************************************************/
        private MapWindrose CreateWindrose()
        {
            MapWindrose windrose = new()
            {
                InnerCircles = WindroseInnerCirclesTrack.Value,
                InnerRadius = (int)WindroseInnerRadiusUpDown.Value,
                FadeOut = WindroseFadeOutCheck.Checked,
                LineWidth = (int)WindroseLineWidthUpDown.Value,
                OuterRadius = (int)WindroseOuterRadiusUpDown.Value,
                WindroseColor = WindroseColorSelectLabel.BackColor,
                DirectionCount = (int)WindroseDirectionsUpDown.Value,
            };

            windrose.WindrosePaint = new()
            {
                Style = SKPaintStyle.Stroke,
                StrokeWidth = windrose.LineWidth,
                Color = windrose.WindroseColor.ToSKColor(),
                IsAntialias = true,
            };

            return windrose;
        }

        private void UpdateUIWindrose()
        {
            if (UIWindrose != null)
            {
                UIWindrose.InnerCircles = WindroseInnerCirclesTrack.Value;
                UIWindrose.InnerRadius = (int)WindroseInnerRadiusUpDown.Value;
                UIWindrose.FadeOut = WindroseFadeOutCheck.Checked;
                UIWindrose.LineWidth = (int)WindroseLineWidthUpDown.Value;
                UIWindrose.OuterRadius = (int)WindroseOuterRadiusUpDown.Value;
                UIWindrose.WindroseColor = WindroseColorSelectLabel.BackColor;
                UIWindrose.DirectionCount = (int)WindroseDirectionsUpDown.Value;

                UIWindrose.WindrosePaint = new()
                {
                    Style = SKPaintStyle.Stroke,
                    StrokeWidth = UIWindrose.LineWidth,
                    Color = UIWindrose.WindroseColor.ToSKColor(),
                    IsAntialias = true,
                };
            }
        }

        #endregion

        #region Ocean Tab Event Handlers
        /*******************************************************************************************************
        * Ocean Tab Event Handlers 
        *******************************************************************************************************/
        private void ShowOceanLayerCheck_CheckedChanged(object sender, EventArgs e)
        {
            // TODO
            for (int i = 0; i < MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.OCEANTEXTURELAYER).MapLayerComponents.Count; i++)
            {
                if (MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.OCEANTEXTURELAYER).MapLayerComponents[i] is MapBitmap m)
                {
                    m.Show = ShowOceanLayerCheck.Checked;
                }
            }

            for (int i = 0; i < MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.OCEANTEXTUREOVERLAYLAYER).MapLayerComponents.Count; i++)
            {
                if (MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.OCEANTEXTUREOVERLAYLAYER).MapLayerComponents[i] is MapBitmap m)
                {
                    m.Show = ShowOceanLayerCheck.Checked;
                }
            }

            MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.OCEANDRAWINGLAYER).ShowLayer = ShowOceanLayerCheck.Checked;

            MapImageBox.Refresh();
        }

        #region Ocean Texture and Color Event Handlers
        private void OceanColorSelectLabel_Click(object sender, EventArgs e)
        {
            Color selectedColor = MapPaintMethods.SelectColorFromDialog(this, OceanColorSelectionLabel.BackColor);

            if (selectedColor != Color.Empty)
            {
                OceanColorSelectionLabel.BackColor = selectedColor;
                OceanColorSelectionLabel.Refresh();
            }
        }

        private void OceanBrushSizeScroll_Scroll(object sender, EventArgs e)
        {
            OceanPaintMethods.OCEAN_BRUSH_SIZE = OceanBrushSizeScroll.Value;
            OceanBrushSizeLabel.Text = OceanBrushSizeScroll.Value.ToString();
            OceanBrushSizeLabel.Refresh();
        }

        private void OceanPaintButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.OceanPaint, sender);
        }

        private void OceanEraseButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.OceanErase, sender);
        }

        private void OceanClearColorButton_Click(object sender, EventArgs e)
        {
            // clear the ocean texture overlay layer
            SetDrawingMode(DrawingModeEnum.None, sender);

            SKBitmap layerBitmap = MapBuilder.GetLayerBitmap(CURRENT_MAP, MapBuilder.OCEANTEXTUREOVERLAYLAYER);

            Cmd_ClearLayerBitmap cmd = new(CURRENT_MAP, MapBuilder.OCEANTEXTUREOVERLAYLAYER, layerBitmap);
            cmd.DoOperation();

            MapImageBox.Refresh();
        }

        private void OceanFillColorButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.None, sender);

            // get the user-selected ocean color
            Color fillColor = OceanColorSelectionLabel.BackColor;
            SKBitmap b = new(CURRENT_MAP.MapWidth, CURRENT_MAP.MapHeight);

            using (SKCanvas canvas = new(b))
            {
                canvas.Clear(Extensions.ToSKColor(fillColor));
            }

            Cmd_SetLayerBitmap cmd = new(CURRENT_MAP, MapBuilder.OCEANTEXTUREOVERLAYLAYER, b);
            UndoManager.AddCommand(cmd);
            cmd.DoOperation();

            MapImageBox.Refresh();
        }

        private void OceanTxList_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (OceanTextureList.SelectedIndex > -1)
            {
                if (CURRENT_MAP != null)
                {
                    Bitmap b = new(WaterFeatureMethods.WATER_TEXTURE_LIST[OceanTextureList.SelectedIndex].TexturePath);
                    OceanTxPictureBox.Image = b;
                }
            }
            else
            {
                OceanTxPictureBox.Image = null;
            }

            OceanTxPictureBox.Refresh();
        }

        private void OceanFillTxButton_Click(object sender, EventArgs e)
        {
            if (OceanTextureList.SelectedIndex > -1)
            {
                if (CURRENT_MAP != null)
                {
                    Bitmap resizedBitmap = new(OceanTxPictureBox.Image, CURRENT_MAP.MapWidth, CURRENT_MAP.MapHeight);

                    Cmd_SetLayerBitmap cmd = new(CURRENT_MAP, MapBuilder.OCEANTEXTURELAYER, Extensions.ToSKBitmap(resizedBitmap));
                    UndoManager.AddCommand(cmd);
                    cmd.DoOperation();

                    MapImageBox.Refresh();
                }
            }
        }

        private void OceanClearTxButton_Click(object sender, EventArgs e)
        {
            SKBitmap layerBitmap = MapBuilder.GetLayerBitmap(CURRENT_MAP, MapBuilder.OCEANTEXTURELAYER);

            Cmd_ClearLayerBitmap cmd = new(CURRENT_MAP, MapBuilder.OCEANTEXTURELAYER, layerBitmap);
            cmd.DoOperation();

            MapImageBox.Refresh();
        }

        private void OceanTxOpacityScroll_Scroll(object sender, EventArgs e)
        {
            OceanTxOpacityLabel.Text = OceanTextureOpacityTrack.Value.ToString();

            if (OceanTextureList.SelectedIndex < 0) return;

            Bitmap? b = new(WaterFeatureMethods.WATER_TEXTURE_LIST[OceanTextureList.SelectedIndex].TexturePath);

            if (b != null)
            {
                Bitmap newB = new(b.Width, b.Height, PixelFormat.Format32bppArgb);

                using Graphics g = Graphics.FromImage(newB);
                //create a color matrix object  
                ColorMatrix matrix = new()
                {
                    //set the opacity  
                    Matrix33 = OceanTextureOpacityTrack.Value / 100F
                };

                //create image attributes  
                ImageAttributes attributes = new ImageAttributes();

                //set the color(opacity) of the image  
                attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                //now draw the image
                g.DrawImage(b, new Rectangle(0, 0, b.Width, b.Height), 0, 0, b.Width, b.Height, GraphicsUnit.Pixel, attributes);

                OceanTxPictureBox.Image = newB;
            }
            else
            {
                OceanTxPictureBox.Image = null;
            }

            OceanTxPictureBox.Refresh();
        }

        private void OceanEraseAllButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.None, sender);

            SKBitmap layerBitmap = MapBuilder.GetLayerBitmap(CURRENT_MAP, MapBuilder.OCEANDRAWINGLAYER);

            Cmd_ClearLayerBitmap cmd = new(CURRENT_MAP, MapBuilder.OCEANDRAWINGLAYER, layerBitmap);
            cmd.DoOperation();

            MapImageBox.Refresh();
        }

        private void OceanSoftBrushButton_Click(object sender, EventArgs e)
        {
            MapPaintMethods.SetSelectedColorBrushType(ColorPaintBrush.SoftBrush);
            OceanSoftBrushButton.FlatAppearance.BorderColor = Color.DarkSeaGreen;
            OceanHardBrushButton.FlatAppearance.BorderColor = Color.LightGray;

            OceanSoftBrushButton.Refresh();
            OceanHardBrushButton.Refresh();

            SetDrawingModeLabel();
        }

        private void OceanHardBrushButton_Click(object sender, EventArgs e)
        {
            MapPaintMethods.SetSelectedColorBrushType(ColorPaintBrush.HardBrush);
            OceanHardBrushButton.FlatAppearance.BorderColor = Color.DarkSeaGreen;
            OceanSoftBrushButton.FlatAppearance.BorderColor = Color.LightGray;

            OceanSoftBrushButton.Refresh();
            OceanHardBrushButton.Refresh();

            SetDrawingModeLabel();
        }

        private void OceanBrushSizeScroll_ValueChanged(object sender, EventArgs e)
        {
            OceanBrushSizeLabel.Text = OceanBrushSizeScroll.Value.ToString();
            OceanPaintMethods.OCEAN_BRUSH_SIZE = OceanBrushSizeScroll.Value;
            OceanBrushSizeLabel.Refresh();
        }

        private void OceanEraserSizeScroll_ValueChanged(object sender, EventArgs e)
        {
            OceanEraserSizeLabel.Text = OceanEraserSizeScroll.Value.ToString();
            OceanPaintMethods.OCEAN_ERASER_SIZE = OceanEraserSizeScroll.Value;
            OceanEraserSizeLabel.Refresh();
        }

        private void OceanColorSelectionButton_Click(object sender, EventArgs e)
        {
            Color selectedColor = MapPaintMethods.SelectColorFromDialog(this, OceanSelectedPaintColorLabel.BackColor);

            if (selectedColor != Color.Empty)
            {
                OceanSelectedPaintColorLabel.BackColor = selectedColor;
                OceanSelectedPaintColorLabel.Refresh();
            }
        }

        private void OceanAddColorPresetButton_Click(object sender, EventArgs e)
        {
            Color selectedColor = MapPaintMethods.SelectColorFromDialog(this, Color.Empty);

            if (selectedColor != Color.Empty)
            {
                Color oceanColor = selectedColor;

                if (OceanCustomColorButton1.Text == "")
                {
                    OceanCustomColorButton1.BackColor = oceanColor;
                    OceanCustomColorButton1.Text = ColorTranslator.ToHtml(oceanColor);
                    OceanCustomColorButton1.Refresh();
                }
                else if (OceanCustomColorButton2.Text == "")
                {
                    OceanCustomColorButton2.BackColor = oceanColor;
                    OceanCustomColorButton2.Text = ColorTranslator.ToHtml(oceanColor);
                    OceanCustomColorButton2.Refresh();
                }
                else if (OceanCustomColorButton3.Text == "")
                {
                    OceanCustomColorButton3.BackColor = oceanColor;
                    OceanCustomColorButton3.Text = ColorTranslator.ToHtml(oceanColor);
                    OceanCustomColorButton3.Refresh();
                }
                else if (OceanCustomColorButton4.Text == "")
                {
                    OceanCustomColorButton4.BackColor = oceanColor;
                    OceanCustomColorButton4.Text = ColorTranslator.ToHtml(oceanColor);
                    OceanCustomColorButton4.Refresh();
                }
                else if (OceanCustomColorButton5.Text == "")
                {
                    OceanCustomColorButton5.BackColor = oceanColor;
                    OceanCustomColorButton5.Text = ColorTranslator.ToHtml(oceanColor);
                    OceanCustomColorButton5.Refresh();
                }
                else if (OceanCustomColorButton6.Text == "")
                {
                    OceanCustomColorButton6.BackColor = oceanColor;
                    OceanCustomColorButton6.Text = ColorTranslator.ToHtml(oceanColor);
                    OceanCustomColorButton6.Refresh();
                }
                else if (OceanCustomColorButton7.Text == "")
                {
                    OceanCustomColorButton7.BackColor = oceanColor;
                    OceanCustomColorButton7.Text = ColorTranslator.ToHtml(oceanColor);
                    OceanCustomColorButton7.Refresh();
                }
                else if (OceanCustomColorButton8.Text == "")
                {
                    OceanCustomColorButton8.BackColor = oceanColor;
                    OceanCustomColorButton8.Text = ColorTranslator.ToHtml(oceanColor);
                    OceanCustomColorButton8.Refresh();
                }
            }
        }

        private void SetOceanColorFromPreset(string htmlColor)
        {
            Color oceanColor = ColorTranslator.FromHtml(htmlColor);

            OceanSelectedPaintColorLabel.BackColor = oceanColor;
            OceanSelectedPaintColorLabel.Refresh();
        }

        private void OceanButton91CBB8_Click(object sender, EventArgs e)
        {
            SetOceanColorFromPreset("#91CBB8");
        }

        private void OceanButton88B5BB_Click(object sender, EventArgs e)
        {
            SetOceanColorFromPreset("#88B5BB");
        }

        private void OceanButton6BA5B9_Click(object sender, EventArgs e)
        {
            SetOceanColorFromPreset("#6BA5B9");
        }

        private void OceanButton42718D_Click(object sender, EventArgs e)
        {
            SetOceanColorFromPreset("#42718D");
        }

        private void SetOceanPaintColorFromCustomPresetButton(Button b)
        {
            if (b.Text != "")
            {
                Color oceanColor = b.BackColor;

                OceanSelectedPaintColorLabel.BackColor = oceanColor;

                OceanSelectedPaintColorLabel.Refresh();
            }
        }

        private static void ResetColorPresetButton(Button b)
        {
            b.Text = string.Empty;
            b.BackColor = Color.White;
            b.Refresh();
        }

        private void OceanCustomColorButton1_Click(object sender, MouseEventArgs e)
        {
            ColorPresetButtonMouseClickHandler(sender, e);
        }

        private void OceanCustomColorButton2_Click(object sender, MouseEventArgs e)
        {
            ColorPresetButtonMouseClickHandler(sender, e);
        }

        private void OceanCustomColorButton3_Click(object sender, MouseEventArgs e)
        {
            ColorPresetButtonMouseClickHandler(sender, e);
        }

        private void OceanCustomColorButton4_Click(object sender, MouseEventArgs e)
        {
            ColorPresetButtonMouseClickHandler(sender, e);
        }

        private void OceanCustomColorButton5_Click(object sender, MouseEventArgs e)
        {
            ColorPresetButtonMouseClickHandler(sender, e);
        }

        private void OceanCustomColorButton6_Click(object sender, MouseEventArgs e)
        {
            ColorPresetButtonMouseClickHandler(sender, e);
        }

        private void OceanCustomColorButton7_Click(object sender, MouseEventArgs e)
        {
            ColorPresetButtonMouseClickHandler(sender, e);
        }

        private void OceanCustomColorButton8_Click(object sender, MouseEventArgs e)
        {
            ColorPresetButtonMouseClickHandler(sender, e);
        }

        private void OceanColorSelect_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.ColorSelect, sender);
        }

        #endregion

        #region Wind Rose Event Handlers
        // windrose
        private void WindroseButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.PlaceWindrose, sender);

            if (CURRENT_DRAWING_MODE == DrawingModeEnum.PlaceWindrose)
            {

                UIWindrose = CreateWindrose();
            }
            else
            {
                MapBuilder.ClearLayerCanvas(CURRENT_MAP, MapBuilder.WINDROSELAYER);
                MapBuilder.ClearLayerBitmap(CURRENT_MAP, MapBuilder.WINDROSELAYER);

                MapImageBox.Refresh();
            }
        }

        private void WindroseColorSelectLabel_Click(object sender, EventArgs e)
        {
            Color selectedColor = MapPaintMethods.SelectColorFromDialog(this, WindroseColorSelectLabel.BackColor);

            if (selectedColor != Color.Empty)
            {
                WindroseColorSelectLabel.BackColor = selectedColor;
                WindroseColorSelectLabel.Refresh();

                UpdateUIWindrose();
            }
        }

        private void WindroseDirectionsUpDown_ValueChanged(object sender, EventArgs e)
        {
            UpdateUIWindrose();
        }

        private void WindroseLineWidthUpDown_ValueChanged(object sender, EventArgs e)
        {
            UpdateUIWindrose();
        }

        private void WindroseInnerRadiusUpDown_ValueChanged(object sender, EventArgs e)
        {
            UpdateUIWindrose();
        }

        private void WindroseOuterRadiusUpDown_ValueChanged(object sender, EventArgs e)
        {
            UpdateUIWindrose();
        }

        private void WindroseInnerCirclesTrack_ValueChanged(object sender, EventArgs e)
        {
            UpdateUIWindrose();
        }

        private void WindroseFadeOutCheck_CheckedChanged(object sender, EventArgs e)
        {
            UpdateUIWindrose();
        }

        private void ClearWindroseButton_Click(object sender, EventArgs e)
        {
            for (int i = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.WINDROSELAYER).MapLayerComponents.Count - 1; i > 0; i--)
            {
                if (MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.WINDROSELAYER).MapLayerComponents[i] is MapWindrose)
                {
                    MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.WINDROSELAYER).MapLayerComponents.RemoveAt(i);
                    break;
                }
            }
        }

        #endregion

        #endregion

        #endregion

        #region Land Tab

        #region Landform Methods

        /******************************************************************************************************
        * *****************************************************************************************************
        * Land Tab Methods
        * *****************************************************************************************************
        *******************************************************************************************************/

        private void SetLandformData(MapLandformType2 mapLandform)
        {
            if (mapLandform == null) { return; }

            mapLandform.LandformOutlineColor = LandOutlineColorSelectionLabel.BackColor;
            mapLandform.CoastlineColor = CoastColorSelectionLabel.BackColor;
            mapLandform.CoastlineEffectDistance = FxDistanceTrack.Value;

#pragma warning disable CS8602 // Dereference of a possibly null reference.

            if (BlendModeSelectionBox.SelectedIndex > -1)
            {
                mapLandform.CoastlineHatchBlendMode = BlendModeSelectionBox.Items[BlendModeSelectionBox.SelectedIndex].ToString();
            }

            if (HatchPatternSelectionBox.SelectedIndex > -1)
            {
                mapLandform.CoastlineHatchPattern = HatchPatternSelectionBox.Items[HatchPatternSelectionBox.SelectedIndex].ToString();
            }

            mapLandform.CoastlineHatchOpacity = HatchOpacityTrack.Value;
            mapLandform.CoastlineHatchScale = HatchScaleTrack.Value;

            if (CoastStyleSelectionBox.SelectedIndex > -1)
            {
#pragma warning disable CS8601 // Possible null reference assignment.
                mapLandform.CoastlineStyleName = CoastStyleSelectionBox.Items[CoastStyleSelectionBox.SelectedIndex].ToString();
#pragma warning restore CS8601 // Possible null reference assignment.
            }

            mapLandform.LandformOutlineColor = LandOutlineColorSelectionLabel.BackColor;
            mapLandform.LandformOutlineWidth = LandOutlineWidthScroll.Value;

            if (LandformTextureBox.SelectedIndex > -1)
            {
                mapLandform.LandformTexture = LandformType2Methods.LAND_TEXTURE_LIST[LandformTextureBox.SelectedIndex];

                Bitmap b = new(mapLandform.LandformTexture.TexturePath);

                Bitmap resizedBitmap = new(b, CURRENT_MAP.MapWidth, CURRENT_MAP.MapHeight);

                // create and set a shader from the selected texture
                SKShader s = SKShader.CreateBitmap(Extensions.ToSKBitmap(resizedBitmap),
                    SKShaderTileMode.Mirror, SKShaderTileMode.Mirror);

                SKPaint p = new()
                {
                    Shader = s,
                    Style = SKPaintStyle.Fill,
                    IsAntialias = true,
                    BlendMode = SKBlendMode.DstATop
                };

                mapLandform.LandformBackgroundPaint = p;
            }

            mapLandform.PaintCoastlineGradient = PaintGradientCheck.Checked;

            if (ShorelineStyleBox.SelectedIndex > -1)
            {
                switch (ShorelineStyleBox.SelectedIndex)
                {
                    case 0:
                        mapLandform.ShorelineStyle = GradientDirectionEnum.None; break;
                    case 1:
                        mapLandform.ShorelineStyle = GradientDirectionEnum.DarkToLight; break;
                    case 2:
                        mapLandform.ShorelineStyle = GradientDirectionEnum.LightToDark; break;
                }
            }

#pragma warning restore CS8602 // Dereference of a possibly null reference.
        }

        private void DeselectAllLandforms()
        {
            if (CURRENT_DRAWING_MODE != DrawingModeEnum.LandformSelect)
            {
                // deselect all landforms
                foreach (MapLandformType2 lf in LandformType2Methods.LANDFORM_LIST)
                {
                    lf.IsSelected = false;
                }

                foreach (MapComponent mc in MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.LANDFORMLAYER).MapLayerComponents)
                {
                    if (mc is MapLandformType2 lf)
                    {
                        lf.IsSelected = false;
                    }
                }

                MapImageBox.Refresh();
            }
        }

        #endregion

        #region Land Tab Event Handlers

        /*******************************************************************************************************
        * Land Tab Event Handlers 
        *******************************************************************************************************/

        private void LandOutlineColorSelectionLabel_Click(object sender, EventArgs e)
        {
            Color selectedColor = MapPaintMethods.SelectColorFromDialog(this, LandOutlineColorSelectionLabel.BackColor);

            if (selectedColor != Color.Empty)
            {
                Color landOutlineColor = selectedColor;
                LandOutlineColorSelectionLabel.BackColor = landOutlineColor;
                LandOutlineColorSelectionLabel.Refresh();
            }
        }

        private void LandOutlineWidthScroll_ValueChanged(object sender, EventArgs e)
        {
            LandOutlineWidthLabel.Text = LandOutlineWidthScroll.Value.ToString();
            LandOutlineWidthLabel.Refresh();
        }

        private void LandformTextureBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LandformTextureBox.SelectedIndex > -1)
            {
                if (CURRENT_MAP != null)
                {
                    Bitmap b = new(LandformType2Methods.LAND_TEXTURE_LIST[LandformTextureBox.SelectedIndex].TexturePath);

                    LandTexturePreviewBox.Image = b;

                    Bitmap resizedBitmap = new(b, CURRENT_MAP.MapWidth, CURRENT_MAP.MapHeight);

                    // create and set a shader from the selected texture
                    SKShader flpShader = SKShader.CreateBitmap(Extensions.ToSKBitmap(resizedBitmap),
                        SKShaderTileMode.Mirror, SKShaderTileMode.Mirror);

                    LandformType2Methods.LAND_FILL_PAINT.Shader = flpShader;
                    LandformType2Methods.LAND_FILL_PAINT.BlendMode = SKBlendMode.DstATop;
                }
            }
            else
            {
                LandTexturePreviewBox.Image = null;
            }

            LandTexturePreviewBox.Refresh();
        }

        private void CoastColorSelectionLabel_Click(object sender, EventArgs e)
        {
            Color selectedColor = MapPaintMethods.SelectColorFromDialog(this, CoastColorSelectionLabel.BackColor);

            if (selectedColor != Color.Empty)
            {
                CoastColorSelectionLabel.BackColor = selectedColor;
                CoastColorSelectionLabel.Refresh();
            }
        }

        private void LandEraserSizeScroll_ValueChanged(object sender, EventArgs e)
        {
            LandformType2Methods.LAND_ERASER_SIZE = LandEraserSizeScroll.Value;
            LandEraserSizeLabel.Text = LandEraserSizeScroll.Value.ToString();
            LandEraserSizeLabel.Refresh();
        }

        private void LandPaintButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.LandPaint, sender);
        }

        private void LandEraseButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.LandErase, sender);
        }

        private void LandBrushSizeScroll_ValueChanged(object sender, EventArgs e)
        {
            LandformType2Methods.LAND_BRUSH_SIZE = LandBrushSizeScroll.Value;

            LandBrushSizeLabel.Text = LandBrushSizeScroll.Value.ToString();
            LandBrushSizeLabel.Refresh();
        }

        private void LandFillButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.None, sender);

            LandformSelectButton.Checked = false;

            if (LandformTextureBox.SelectedIndex > 0)
            {
                if (LandformType2Methods.LANDFORM_LIST.Count > 0)
                {
                    MessageBox.Show("Landforms have already been drawn. Please clear them before filling the map.", "Landforms Already Drawn", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
                else
                {
                    MapLandformType2 landform = new();
                    SetLandformData(landform);

                    LandformType2Methods.FillMapWithLandForm(CURRENT_MAP, landform);

                    CURRENT_MAP.IsSaved = false;

                    MapImageBox.Refresh();
                }
            }
            else
            {
                MessageBox.Show("Please select a landform texture.", "Select Texture", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
            }
        }

        private void LandClearButton_Click(object sender, EventArgs e)
        {
            LandformSelectButton.Checked = false;
            DialogResult confirmResult = MessageBox.Show("This action will clear all landform drawing and any drawn landforms.\nPlease confirm.", "Clear All?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

            if (confirmResult != DialogResult.OK)
            {
                return;
            }

            SetDrawingMode(DrawingModeEnum.None, sender);

            Cmd_ClearAllLandforms cmd = new()
            {
                Map = CURRENT_MAP,
                STORED_LANDFORM_LIST = new List<MapLandformType2>(LandformType2Methods.LANDFORM_LIST),
                STORED_LAND_LAYER_ERASER_PATH = LandformType2Methods.LAND_LAYER_ERASER_PATH,
                STORED_LANDFORMLAYER_BITMAP = MapBuilder.GetLayerBitmap(CURRENT_MAP, MapBuilder.LANDFORMLAYER),
                STORED_LANDCOASTLINELAYER_BITMAP = MapBuilder.GetLayerBitmap(CURRENT_MAP, MapBuilder.LANDCOASTLINELAYER),
                STORED_LANDDRAWINGLAYER_BITMAP = MapBuilder.GetLayerBitmap(CURRENT_MAP, MapBuilder.LANDDRAWINGLAYER)
            };

            UndoManager.AddCommand(cmd);
            cmd.DoOperation();

            MapImageBox.Refresh();
        }

        private void LandColorButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.LandColor, sender);
        }

        private void FractalizeButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("The coastline of the selected landform will be randomized. This operation cannot be undone. Continue?",
                "Randomize Selected Landform?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

            if (result == DialogResult.Yes)
            {
                SetDrawingMode(DrawingModeEnum.None, sender);

                LandformSelectButton.Checked = false;

                UpdateProgressBar(0, "");

                // TODO: Undo/Redo
                LandformRandomizer r = new LandformRandomizer(this, CURRENT_MAP);

                MapLandformType2? randomizedLandform = null;

                foreach (MapLandformType2 landform in LandformType2Methods.LANDFORM_LIST)
                {
                    if (landform.IsSelected)
                    {
                        randomizedLandform = landform;
                    }
                }

                if (randomizedLandform != null)
                {
                    r.RandomizeLandform(ref randomizedLandform);
                }

                CURRENT_MAP.IsSaved = false;
            }
        }

        private void GenerateLandFormButton_Click(object sender, EventArgs e)
        {
            switch (SELECTED_LANDFORM_TYPE)
            {
                case GeneratedLandformTypeEnum.Random:
                    MapLandformType2? newLandform = MapGenerator.GenerateRandomLandform(CURRENT_MAP, UISelectedLandformArea);

                    if (newLandform != null)
                    {
                        SetLandformData(newLandform);
                        newLandform.DrawLandform = true;

                        MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.LANDFORMLAYER).MapLayerComponents.Add(newLandform);
                        LandformType2Methods.LANDFORM_LIST.Add(newLandform);

                        // TODO: merging generated landforms isn't working - why?
                        //LandformType2Methods.MergeLandforms();

                        LandformType2Methods.SELECTED_LANDFORM = newLandform;

                        RenderDrawingPanel();

                        MapImageBox.Refresh();
                    }
                    break;
            }
        }

        private void FxDistanceTrack_ValueChanged(object sender, EventArgs e)
        {
            FxDistanceLabel.Text = FxDistanceTrack.Value.ToString();
            FxDistanceLabel.Refresh();
        }

        private void CoastStyleSelectionBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            // user-defined coast style
            HatchPatternSelectionBox.Enabled = CoastStyleSelectionBox.SelectedIndex == 8;
            HatchOpacityTrack.Enabled = CoastStyleSelectionBox.SelectedIndex == 8;
            HatchScaleTrack.Enabled = CoastStyleSelectionBox.SelectedIndex == 8;
            PaintGradientCheck.Enabled = CoastStyleSelectionBox.SelectedIndex == 8;
        }

        private void HatchPatternSelectionBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (HatchPatternSelectionBox.SelectedIndex > -1)
            {
                if (CURRENT_MAP != null)
                {
                    Bitmap b = new(LandformType2Methods.HATCH_TEXTURE_LIST[HatchPatternSelectionBox.SelectedIndex].TexturePath);

                    HatchPatternPreviewBox.Image = b;

                    // create shaders for user-defined hatch
                    CreateUserDefinedHatchShaders();
                }
            }
            else
            {
                HatchPatternPreviewBox.Image = null;
            }

            HatchPatternPreviewBox.Refresh();
        }

        private void HatchOpacityTrack_Scroll(object sender, EventArgs e)
        {
            HatchOpacityLabel.Text = HatchOpacityTrack.Value.ToString();
            HatchOpacityLabel.Refresh();

            CreateUserDefinedHatchShaders();
        }

        private void HatchScaleTrack_Scroll(object sender, EventArgs e)
        {
            HatchScaleLabel.Text = HatchScaleTrack.Value.ToString();
            HatchScaleLabel.Refresh();

            CreateUserDefinedHatchShaders();
        }

        private void BlendModeSelectionBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            CreateUserDefinedHatchShaders();
        }

        private void LandSoftBrushButton_Click(object sender, EventArgs e)
        {
            MapPaintMethods.SetSelectedColorBrushType(ColorPaintBrush.SoftBrush);
            LandSoftBrushButton.FlatAppearance.BorderColor = Color.DarkSeaGreen;
            LandHardBrushButton.FlatAppearance.BorderColor = Color.LightGray;

            LandSoftBrushButton.Refresh();
            LandHardBrushButton.Refresh();

            SetDrawingModeLabel();
        }

        private void LandHardBrushButton_Click(object sender, EventArgs e)
        {
            MapPaintMethods.SetSelectedColorBrushType(ColorPaintBrush.HardBrush);
            LandHardBrushButton.FlatAppearance.BorderColor = Color.DarkSeaGreen;
            LandSoftBrushButton.FlatAppearance.BorderColor = Color.LightGray;

            LandSoftBrushButton.Refresh();
            LandHardBrushButton.Refresh();

            SetDrawingModeLabel();
        }

        private void LandColorEraserSizeTrack_ValueChanged(object sender, EventArgs e)
        {
            LandColorEraserSizeLabel.Text = LandColorEraserSizeTrack.Value.ToString();
            LandformType2Methods.LAND_COLOR_ERASER_SIZE = LandColorEraserSizeTrack.Value;
            LandColorEraserSizeLabel.Refresh();
        }

        private void LandColorBrushSizeTrack_ValueChanged(object sender, EventArgs e)
        {
            LandColorBrushSizeLabel.Text = LandColorBrushSizeTrack.Value.ToString();
            LandformType2Methods.LAND_COLOR_BRUSH_SIZE = LandColorBrushSizeTrack.Value;
            LandColorBrushSizeLabel.Refresh();
        }

        private void LandColorSelectionButton_Click(object sender, EventArgs e)
        {
            Color selectedColor = MapPaintMethods.SelectColorFromDialog(this, LandSelectedPaintColorLabel.BackColor);

            if (selectedColor != Color.Empty)
            {
                LandSelectedPaintColorLabel.BackColor = selectedColor;
                LandSelectedPaintColorLabel.Refresh();
            }
        }

        private void LandColorEraseButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.LandColorErase, sender);
        }

        private void LandAddColorPresetButton_Click(object sender, EventArgs e)
        {
            Color selectedColor = MapPaintMethods.SelectColorFromDialog(this, Color.Empty);

            if (selectedColor != Color.Empty)
            {
                Color landColor = selectedColor;

                if (LandCustomColorButton1.Text == "")
                {
                    LandCustomColorButton1.BackColor = landColor;
                    LandCustomColorButton1.Text = ColorTranslator.ToHtml(landColor);
                    LandCustomColorButton1.Refresh();
                }
                else if (LandCustomColorButton2.Text == "")
                {
                    LandCustomColorButton2.BackColor = landColor;
                    LandCustomColorButton2.Text = ColorTranslator.ToHtml(landColor);
                    LandCustomColorButton2.Refresh();
                }
                else if (LandCustomColorButton3.Text == "")
                {
                    LandCustomColorButton3.BackColor = landColor;
                    LandCustomColorButton3.Text = ColorTranslator.ToHtml(landColor);
                    LandCustomColorButton3.Refresh();
                }
                else if (LandCustomColorButton4.Text == "")
                {
                    LandCustomColorButton4.BackColor = landColor;
                    LandCustomColorButton4.Text = ColorTranslator.ToHtml(landColor);
                    LandCustomColorButton4.Refresh();
                }
                else if (LandCustomColorButton5.Text == "")
                {
                    LandCustomColorButton5.BackColor = landColor;
                    LandCustomColorButton5.Text = ColorTranslator.ToHtml(landColor);
                    LandCustomColorButton5.Refresh();
                }
                else if (LandCustomColorButton6.Text == "")
                {
                    LandCustomColorButton6.BackColor = landColor;
                    LandCustomColorButton6.Text = ColorTranslator.ToHtml(landColor);
                    LandCustomColorButton6.Refresh();
                }
            }
        }

        private void SetLandColorFromPreset(string htmlColor)
        {
            Color landColor = ColorTranslator.FromHtml(htmlColor);

            LandSelectedPaintColorLabel.BackColor = landColor;

            LandSelectedPaintColorLabel.Refresh();
        }

        private void LandButtonE6D0AB_Click(object sender, EventArgs e)
        {
            SetLandColorFromPreset("#E6D0AB");
        }

        private void LandButtonD8B48F_Click(object sender, EventArgs e)
        {
            SetLandColorFromPreset("#D8B48F");
        }

        private void LandButtonBEBB8E_Click(object sender, EventArgs e)
        {
            SetLandColorFromPreset("#BEBB8E");
        }

        private void LandButtonD7C293_Click(object sender, EventArgs e)
        {
            SetLandColorFromPreset("#D7C293");
        }

        private void LandButtonAD9C7E_Click(object sender, EventArgs e)
        {
            SetLandColorFromPreset("#AD9C7E");
        }

        private void LandButton3D3728_Click(object sender, EventArgs e)
        {
            SetLandColorFromPreset("#3D3728");
        }

        private void SetLandPaintColorFromCustomPresetButton(Button b)
        {
            if (b.Text != "")
            {
                Color landColor = b.BackColor;

                LandSelectedPaintColorLabel.BackColor = landColor;

                LandSelectedPaintColorLabel.Refresh();
            }
        }

        private void LandCustomColorButton1_MouseClick(object sender, MouseEventArgs e)
        {
            ColorPresetButtonMouseClickHandler(sender, e);
        }

        private void LandCustomColorButton2_MouseClick(object sender, MouseEventArgs e)
        {
            ColorPresetButtonMouseClickHandler(sender, e);
        }

        private void LandCustomColorButton3_MouseClick(object sender, MouseEventArgs e)
        {
            ColorPresetButtonMouseClickHandler(sender, e);
        }

        private void LandCustomColorButton4_MouseClick(object sender, MouseEventArgs e)
        {
            ColorPresetButtonMouseClickHandler(sender, e);
        }

        private void LandCustomColorButton5_MouseClick(object sender, MouseEventArgs e)
        {
            ColorPresetButtonMouseClickHandler(sender, e);
        }

        private void LandCustomColorButton6_MouseClick(object sender, MouseEventArgs e)
        {
            ColorPresetButtonMouseClickHandler(sender, e);
        }

        private void LandColorSelectButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.ColorSelect, sender);
        }

        private void LandformSelectButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.LandformSelect, sender);
        }

        private void SelectLandformAreaButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.LandformAreaSelect, sender);
        }

        private void ShowLandLayerCheck_CheckedChanged(object sender, EventArgs e)
        {
            MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.LANDFORMLAYER).ShowLayer = ShowLandLayerCheck.Checked;
            MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.LANDCOASTLINELAYER).ShowLayer = ShowLandLayerCheck.Checked;
            MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.LANDDRAWINGLAYER).ShowLayer = ShowLandLayerCheck.Checked;

            MapImageBox.Refresh();
        }

        private void RandomLandformButton_Click(object sender, EventArgs e)
        {
            UncheckAllLandformTypeMenuItems();
            RandomLandformButton.Checked = true;
            SELECTED_LANDFORM_TYPE = GeneratedLandformTypeEnum.Random;
        }

        private void ContinentLandformButton_Click(object sender, EventArgs e)
        {
            UncheckAllLandformTypeMenuItems();
            ContinentLandformButton.Checked = true;
            SELECTED_LANDFORM_TYPE = GeneratedLandformTypeEnum.Continent;
        }

        private void ArchipelagoLandformButton_Click(object sender, EventArgs e)
        {
            UncheckAllLandformTypeMenuItems();
            ArchipelagoLandformButton.Checked = true;
            SELECTED_LANDFORM_TYPE = GeneratedLandformTypeEnum.Archipelago;
        }

        private void AtollLandformButton_Click(object sender, EventArgs e)
        {
            UncheckAllLandformTypeMenuItems();
            AtollLandformButton.Checked = true;
            SELECTED_LANDFORM_TYPE = GeneratedLandformTypeEnum.Atoll;
        }

        private void WorldLandformButton_Click(object sender, EventArgs e)
        {
            UncheckAllLandformTypeMenuItems();
            WorldLandformButton.Checked = true;
            SELECTED_LANDFORM_TYPE = GeneratedLandformTypeEnum.World;
        }

        private void EquilateralLandformButton_Click(object sender, EventArgs e)
        {
            UncheckAllLandformTypeMenuItems();
            EquilateralLandformButton.Checked = true;
            SELECTED_LANDFORM_TYPE = GeneratedLandformTypeEnum.Equirectangular;
        }

        private void AdvancedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GENERATE_LANDFORM_DIALOG != null)
            {
                MapLandformType2 newLandform = LandformType2Methods.GetNewLandform(CURRENT_MAP);

                SetLandformData(newLandform);
                newLandform.LandformPath.Reset();

                GENERATE_LANDFORM_DIALOG.Initialize(CURRENT_MAP, newLandform, UISelectedLandformArea);

                GENERATE_LANDFORM_DIALOG.Show();
            }
        }

        private void UncheckAllLandformTypeMenuItems()
        {
            RandomLandformButton.Checked = false;
            ContinentLandformButton.Checked = false;
            ArchipelagoLandformButton.Checked = false;
            AtollLandformButton.Checked = false;
            WorldLandformButton.Checked = false;
            EquilateralLandformButton.Checked = false;
        }

        #endregion

        #endregion

        #region Water Tab

        #region Water Tab Methods

        /******************************************************************************************************
        * *****************************************************************************************************
        * Water Tab Methods
        * *****************************************************************************************************
        *******************************************************************************************************/

        internal void SetWaterFeatureData(MapPaintedWaterFeature waterFeature, WaterFeatureTypeEnum waterFeatureType = WaterFeatureTypeEnum.NotSet)
        {
            if (waterFeature == null) { return; }

            waterFeature.WaterFeatureType = waterFeatureType;

            waterFeature.WaterFeatureColor = WaterColorSelectionLabel.BackColor;
            waterFeature.WaterFeatureShorelineColor = ShorelineColorSelectionLabel.BackColor;

            WaterFeatureMethods.ConstructWaterFeaturePaintObjects(waterFeature);
        }

        internal void SetRiverData(MapRiver mapRiver)
        {
            if (mapRiver == null) { return; }

            mapRiver.RiverColor = WaterColorSelectionLabel.BackColor;

            mapRiver.RiverShorelineColor = ShorelineColorSelectionLabel.BackColor;

            mapRiver.RiverWidth = RiverWidthTrack.Value / 2.0F;

            mapRiver.RiverSourceFadeIn = RiverSourceFadeInCheck.Checked;

            WaterFeatureMethods.ConstructRiverPaintObjects(mapRiver);
        }

        private void DrawRiverLines()
        {
            CURRENT_MAP.IsSaved = false;

            // clip path painting to landform
            RIVER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));

            using SKBitmap b = Extensions.ToSKBitmap((Bitmap)MapImageBox.Image);
            using SKCanvas c = new(b);

            SKRectI recti = new(0, 0, CURRENT_MAP.MapWidth, CURRENT_MAP.MapHeight);

            using SKRegion pathDrawingRegion = new();
            pathDrawingRegion.SetRect(recti);

            using SKRegion pathRegion = new(pathDrawingRegion);

            List<MapLandformType2> landFormList = LandformType2Methods.LANDFORM_LIST;

            for (int i = 0; i < landFormList.Count; i++)
            {
                SKPath landformOutlinePath = landFormList[i].LandformPath;

                if (landformOutlinePath != null && landformOutlinePath.PointCount > 0)
                {
                    pathRegion.SetPath(landformOutlinePath);

                    //c.Save();
                    //c.ClipRegion(pathRegion);

                    WaterFeatureMethods.RIVER_POINT_LIST.Add(new MapRiverPoint(RIVER_CLICK_POINT));
                    MapRiver newPath = WaterFeatureMethods.NEW_RIVER;

                    WaterFeatureMethods.SetSelectedRiverPoints(newPath);

                    WaterFeatureMethods.DrawRiver(CURRENT_MAP, newPath, c);

                    c.Restore();
                }
            }

            MapImageBox.Image = Extensions.ToBitmap(b);
        }

        private void DeselectAllWaterFeatures()
        {
            if (CURRENT_DRAWING_MODE != DrawingModeEnum.LandformSelect)
            {
                // deselect all water features
                foreach (MapPaintedWaterFeature wf in WaterFeatureMethods.PAINTED_WATERFEATURE_LIST)
                {
                    wf.IsSelected = false;
                }

                foreach (MapRiver r in WaterFeatureMethods.MAP_RIVER_LIST)
                {
                    r.IsSelected = false;
                }

                foreach (MapComponent mc in MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.WATERLAYER).MapLayerComponents)
                {
                    if (mc is MapPaintedWaterFeature wf)
                    {
                        wf.IsSelected = false;
                    }
                    else if (mc is MapRiver r)
                    {
                        r.IsSelected = false;
                    }
                }

                MapImageBox.Refresh();
            }
        }

        #endregion

        #region Water Tab Event Handlers

        /*******************************************************************************************************
        * Water Tab Event Handlers 
        *******************************************************************************************************/
        private void WaterColorSelectButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.ColorSelect, sender);
        }

        private void WaterFeaturePaintButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.WaterPaint, sender);
        }

        private void WaterBrushSizeTrack_ValueChanged(object sender, EventArgs e)
        {
            WaterFeatureMethods.WATER_BRUSH_SIZE = WaterBrushSizeTrack.Value;
            WaterBrushSizeLabel.Text = WaterBrushSizeTrack.Value.ToString();
            WaterBrushSizeLabel.Refresh();
        }

        private void WaterColorSelectionLabel_Click(object sender, EventArgs e)
        {
            Color selectedColor = MapPaintMethods.SelectColorFromDialog(this, WaterColorSelectionLabel.BackColor);

            if (selectedColor != Color.Empty)
            {
                WaterColorSelectionLabel.BackColor = selectedColor;
                WaterColorSelectionLabel.Refresh();
            }
        }

        private void ShorelineColorSelectionLabel_Click(object sender, EventArgs e)
        {
            Color selectedColor = MapPaintMethods.SelectColorFromDialog(this, ShorelineColorSelectionLabel.BackColor);

            if (selectedColor != Color.Empty)
            {
                ShorelineColorSelectionLabel.BackColor = selectedColor;
                ShorelineColorSelectionLabel.Refresh();
            }
        }

        private void RiverWidthTrack_Scroll(object sender, EventArgs e)
        {
            RiverWidthLabel.Text = RiverWidthTrack.Value.ToString();
            RiverWidthLabel.Refresh();
        }

        private void WaterEraserSizeTrack_ValueChanged(object sender, EventArgs e)
        {
            WaterFeatureMethods.WATER_ERASER_SIZE = WaterEraserSizeTrack.Value;
            WaterEraserSizeLabel.Text = WaterEraserSizeTrack.Value.ToString();
            WaterEraserSizeLabel.Refresh();
        }

        private void WaterFeatureSelectButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.WaterFeatureSelect, sender);
        }

        private void WaterFeatureEraseButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.WaterErase, sender);
        }

        private void WaterFeatureLakeButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.LakePaint, sender);
        }

        private void WaterFeatureRiverButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.RiverPaint, sender);
        }

        private void WaterSoftBrushButton_Click(object sender, EventArgs e)
        {
            MapPaintMethods.SetSelectedColorBrushType(ColorPaintBrush.SoftBrush);
            WaterSoftBrushButton.FlatAppearance.BorderColor = Color.DarkSeaGreen;
            WaterSoftBrushButton.FlatAppearance.BorderColor = Color.LightGray;

            WaterSoftBrushButton.Refresh();
            WaterHardBrushButton.Refresh();

            SetDrawingModeLabel();
        }

        private void WaterHardBrushButton_Click(object sender, EventArgs e)
        {
            // TODO: refactor to handle all brush setting in one method
            MapPaintMethods.SetSelectedColorBrushType(ColorPaintBrush.HardBrush);
            WaterHardBrushButton.FlatAppearance.BorderColor = Color.DarkSeaGreen;
            WaterSoftBrushButton.FlatAppearance.BorderColor = Color.LightGray;

            WaterSoftBrushButton.Refresh();
            WaterHardBrushButton.Refresh();

            SetDrawingModeLabel();
        }

        private void WaterColorButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.WaterColor, sender);
        }

        private void WaterColorBrushSizeTrack_Scroll(object sender, EventArgs e)
        {
            WaterColorBrushSizeLabel.Text = WaterColorBrushSizeTrack.Value.ToString();
            WaterFeatureMethods.WATER_COLOR_BRUSH_SIZE = WaterColorBrushSizeTrack.Value;
            WaterColorBrushSizeLabel.Refresh();
        }

        private void WaterColorEraserSizeTrack_Scroll(object sender, EventArgs e)
        {
            WaterColorEraserSizeLabel.Text = WaterColorEraserSizeTrack.Value.ToString();
            WaterFeatureMethods.WATER_COLOR_ERASER_SIZE = WaterColorEraserSizeTrack.Value;
            WaterColorEraserSizeLabel.Refresh();
        }

        private void WaterColorEraseButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.WaterColorErase, sender);
        }

        private void WaterColorSelectionButton_Click(object sender, EventArgs e)
        {
            Color selectedColor = MapPaintMethods.SelectColorFromDialog(this, WaterSelectedPaintColorLabel.BackColor);

            if (selectedColor != Color.Empty)
            {
                WaterSelectedPaintColorLabel.BackColor = selectedColor;

                WaterSelectedPaintColorLabel.Refresh();
            }
        }

        private void WaterAddColorPresetButton_Click(object sender, EventArgs e)
        {
            Color selectedColor = MapPaintMethods.SelectColorFromDialog(this, Color.Empty);

            if (selectedColor != Color.Empty)
            {
                Color waterColor = selectedColor;

                if (WaterCustomColor1.Text == "")
                {
                    WaterCustomColor1.BackColor = waterColor;
                    WaterCustomColor1.Text = ColorTranslator.ToHtml(waterColor);
                    WaterCustomColor1.Refresh();
                }
                else if (WaterCustomColor2.Text == "")
                {
                    WaterCustomColor2.BackColor = waterColor;
                    WaterCustomColor2.Text = ColorTranslator.ToHtml(waterColor);
                    WaterCustomColor2.Refresh();
                }
                else if (WaterCustomColor3.Text == "")
                {
                    WaterCustomColor3.BackColor = waterColor;
                    WaterCustomColor3.Text = ColorTranslator.ToHtml(waterColor);
                    WaterCustomColor3.Refresh();
                }
                else if (WaterCustomColor4.Text == "")
                {
                    WaterCustomColor4.BackColor = waterColor;
                    WaterCustomColor4.Text = ColorTranslator.ToHtml(waterColor);
                    WaterCustomColor4.Refresh();
                }
                else if (WaterCustomColor5.Text == "")
                {
                    WaterCustomColor5.BackColor = waterColor;
                    WaterCustomColor5.Text = ColorTranslator.ToHtml(waterColor);
                    WaterCustomColor5.Refresh();
                }
                else if (WaterCustomColor6.Text == "")
                {
                    WaterCustomColor6.BackColor = waterColor;
                    WaterCustomColor6.Text = ColorTranslator.ToHtml(waterColor);
                    WaterCustomColor6.Refresh();
                }
                else if (WaterCustomColor7.Text == "")
                {
                    WaterCustomColor7.BackColor = waterColor;
                    WaterCustomColor7.Text = ColorTranslator.ToHtml(waterColor);
                    WaterCustomColor7.Refresh();
                }
                else if (WaterCustomColor8.Text == "")
                {
                    WaterCustomColor8.BackColor = waterColor;
                    WaterCustomColor8.Text = ColorTranslator.ToHtml(waterColor);
                    WaterCustomColor8.Refresh();
                }
            }
        }

        private void SetWaterColorFromPreset(string htmlColor)
        {
            Color waterColor = ColorTranslator.FromHtml(htmlColor);

            WaterSelectedPaintColorLabel.BackColor = waterColor;
            WaterSelectedPaintColorLabel.Refresh();
        }

        private void WaterButton91CBB8_Click(object sender, EventArgs e)
        {
            SetWaterColorFromPreset("#91CBB8");
        }

        private void WaterButton88B5BB_Click(object sender, EventArgs e)
        {
            SetWaterColorFromPreset("#88B5BB");
        }

        private void WaterButton6BA5B9_Click(object sender, EventArgs e)
        {
            SetWaterColorFromPreset("#6BA5B9");
        }

        private void WaterButton42718D_Click(object sender, EventArgs e)
        {
            SetWaterColorFromPreset("#42718D");
        }

        private void SetWaterPaintColorFromCustomPresetButton(Button b)
        {
            if (b.Text != "")
            {
                Color waterColor = b.BackColor;

                WaterSelectedPaintColorLabel.BackColor = waterColor;

                WaterSelectedPaintColorLabel.Refresh();
            }
        }

        private void WaterCustomColor1_MouseClick(object sender, MouseEventArgs e)
        {
            ColorPresetButtonMouseClickHandler(sender, e);
        }

        private void WaterCustomColor2_MouseClick(object sender, MouseEventArgs e)
        {
            ColorPresetButtonMouseClickHandler(sender, e);
        }

        private void WaterCustomColor3_MouseClick(object sender, MouseEventArgs e)
        {
            ColorPresetButtonMouseClickHandler(sender, e);
        }

        private void WaterCustomColor4_MouseClick(object sender, MouseEventArgs e)
        {
            ColorPresetButtonMouseClickHandler(sender, e);
        }

        private void WaterCustomColor5_MouseClick(object sender, MouseEventArgs e)
        {
            ColorPresetButtonMouseClickHandler(sender, e);
        }

        private void WaterCustomColor6_MouseClick(object sender, MouseEventArgs e)
        {
            ColorPresetButtonMouseClickHandler(sender, e);
        }

        private void WaterCustomColor7_MouseClick(object sender, MouseEventArgs e)
        {
            ColorPresetButtonMouseClickHandler(sender, e);
        }

        private void WaterCustomColor8_MouseClick(object sender, MouseEventArgs e)
        {
            ColorPresetButtonMouseClickHandler(sender, e);
        }

        private void ShowWaterLayerCheck_CheckedChanged(object sender, EventArgs e)
        {
            MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.WATERLAYER).ShowLayer = ShowWaterLayerCheck.Checked;
            MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.WATERDRAWINGLAYER).ShowLayer = ShowWaterLayerCheck.Checked;

            MapImageBox.Refresh();
        }

        #endregion

        #endregion

        #region Path Tab

        #region Path Tab Methods

        /******************************************************************************************************
        * *****************************************************************************************************
        * Path Tab Methods
        * *****************************************************************************************************
        *******************************************************************************************************/
        public void SetPathData(MapPath mapPath)
        {
            if (mapPath == null) { return; }

            mapPath.PathColor = PathColorSelectionLabel.BackColor;

            mapPath.PathWidth = PathWidthTrack.Value / 2.0F;

            mapPath.DrawOverSymbols = DrawOverSymbolsCheck.Checked;

            mapPath.PathType = GetPathType();

            MapPathMethods.ConstructPathPaint(mapPath);
        }

        private void DrawMapPathLines()
        {
            if (PREVIOUS_PATH_CLICK_POINT != null && PATH_CLICK_POINT != null)
            {
                // clip path painting to landform
                PATH_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));

                using SKBitmap b = Extensions.ToSKBitmap((Bitmap)MapImageBox.Image);
                using SKCanvas c = new(b);

                SKRectI recti = new(0, 0, CURRENT_MAP.MapWidth, CURRENT_MAP.MapHeight);

                using SKRegion pathDrawingRegion = new();
                pathDrawingRegion.SetRect(recti);

                using SKRegion pathRegion = new(pathDrawingRegion);

                List<MapLandformType2> landFormList = LandformType2Methods.LANDFORM_LIST;

                for (int i = 0; i < landFormList.Count; i++)
                {
                    SKPath landformOutlinePath = landFormList[i].LandformPath;

                    if (landformOutlinePath != null && landformOutlinePath.PointCount > 0)
                    {
                        pathRegion.SetPath(landformOutlinePath);

                        c.Save();
                        c.ClipRegion(pathRegion);

                        MapPathMethods.AddPointToMapPath(new MapPathPoint((SKPoint)PATH_CLICK_POINT));
                        MapPath newPath = MapPathMethods.GetNewPath();

                        MapPathMethods.SetSelectedMapPathPoints(newPath);

                        MapPathMethods.DrawMapPath(CURRENT_MAP, newPath, c);

                        c.Restore();
                    }
                }


                MapImageBox.Image = Extensions.ToBitmap(b);
            }
        }

        private PathTypeEnum GetPathType()
        {
            if (SolidLineRadio.Checked) return PathTypeEnum.SolidLinePath;
            if (DottedLineRadio.Checked) return PathTypeEnum.DottedLinePath;
            if (DashedLineRadio.Checked) return PathTypeEnum.DashedLinePath;
            if (DashDotLineRadio.Checked) return PathTypeEnum.DashDotLinePath;
            if (DashDotDotLineRadio.Checked) return PathTypeEnum.DashDotDotLinePath;
            if (ChevronLineRadio.Checked) return PathTypeEnum.ChevronLinePath;
            if (LineAndDashesRadio.Checked) return PathTypeEnum.LineAndDashesPath;
            if (SmallDashesRadio.Checked) return PathTypeEnum.ShortIrregularDashPath;
            if (ThickLineRadio.Checked) return PathTypeEnum.ThickSolidLinePath;
            if (BlackBorderPathRadio.Checked) return PathTypeEnum.SolidBlackBorderPath;
            if (BorderedGradientRadio.Checked) return PathTypeEnum.BorderedGradientPath;
            if (BorderedLightSolidRadio.Checked) return PathTypeEnum.BorderedLightSolidPath;
            if (DoubleSolidBorderRadio.Checked) return PathTypeEnum.DoubleSolidBorderPath;
            if (BearTracksRadio.Checked) return PathTypeEnum.BearTracksPath;
            if (BirdTracksRadio.Checked) return PathTypeEnum.BirdTracksPath;
            if (FootPrintsRadio.Checked) return PathTypeEnum.FootprintsPath;
            if (RailroadTracksRadio.Checked) return PathTypeEnum.RailroadTracksPath;
            if (TexturePathRadio.Checked) return PathTypeEnum.TexturedPath;
            if (BorderTexturePathRadio.Checked) return PathTypeEnum.BorderAndTexturePath;

            return PathTypeEnum.SolidLinePath;
        }

        private void CreatePathTextureShader()
        {
            if (PathTextureBox.SelectedIndex > -1)
            {
                Bitmap b = new(MapPathMethods.GetPathTextureList()[PathTextureBox.SelectedIndex].TexturePath);

                PathTexturePreviewBox.Image = b;
                Bitmap resizedBitmap = new(b, PathWidthTrack.Value, PathWidthTrack.Value);

                MapPathMethods.UpdateMapPathTextureShader(resizedBitmap);
            }
        }

        private void DeselectAllPaths()
        {
            if (CURRENT_DRAWING_MODE != DrawingModeEnum.PathSelect && CURRENT_DRAWING_MODE != DrawingModeEnum.PathEdit)
            {
                // deselect all paths
                foreach (MapPath mp in MapPathMethods.GetMapPathList())
                {
                    mp.IsSelected = false;
                }

                foreach (MapComponent mc in MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.PATHLOWERLAYER).MapLayerComponents)
                {
                    if (mc is MapPath p)
                    {
                        p.IsSelected = false;
                    }
                }

                foreach (MapComponent mc in MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.PATHUPPERLAYER).MapLayerComponents)
                {
                    if (mc is MapPath p)
                    {
                        p.IsSelected = false;
                    }
                }

                MapImageBox.Refresh();
            }
        }

        #endregion

        #region Path Tab Event Handlers

        /*******************************************************************************************************
        * Path Tab Event Handlers 
        *******************************************************************************************************/
        private void PathColorSelectionLabel_Click(object sender, EventArgs e)
        {
            Color selectedColor = MapPaintMethods.SelectColorFromDialog(this, PathColorSelectionLabel.BackColor);

            if (selectedColor != Color.Empty)
            {
                PathColorSelectionLabel.BackColor = selectedColor;
            }
        }

        private void PathWidthTrack_Scroll(object sender, EventArgs e)
        {
            CreatePathTextureShader();

            PathWidthLabel.Text = (PathWidthTrack.Value / 2.0F).ToString();
            PathWidthLabel.Refresh();
        }

        private void PathTextureBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PathTextureBox.SelectedIndex > -1)
            {
                if (CURRENT_MAP != null)
                {
                    CreatePathTextureShader();
                }
            }
            else
            {
                PathTexturePreviewBox.Image = null;
            }

            PathTexturePreviewBox.Refresh();
        }

        private void SelectPathButton_Click(object sender, EventArgs e)
        {
            SelectPathButton.Checked = !SelectPathButton.Checked;


            if (CURRENT_DRAWING_MODE != DrawingModeEnum.PathEdit && CURRENT_DRAWING_MODE != DrawingModeEnum.PathSelect)
            {

                if (EditPathPointsCheck.Checked)
                {
                    SetDrawingMode(DrawingModeEnum.PathEdit, sender);
                }
                else
                {
                    SetDrawingMode(DrawingModeEnum.PathSelect, sender);
                    foreach (MapPath mp in MapPathMethods.GetMapPathList())
                    {
                        mp.ShowPathPoints = false;
                    }
                }
            }
            else
            {
                SetDrawingMode(DrawingModeEnum.None, sender);

                foreach (MapPath mp in MapPathMethods.GetMapPathList())
                {
                    mp.IsSelected = false;
                    mp.ShowPathPoints = false;
                }
            }

            MapImageBox.Refresh();
        }

        private void DrawPathButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.PathPaint, sender);
        }

        private void EditPathPointsCheck_Click(object sender, EventArgs e)
        {
            if (EditPathPointsCheck.Checked)
            {
                if (CURRENT_DRAWING_MODE == DrawingModeEnum.PathSelect)
                {
                    SetDrawingMode(DrawingModeEnum.PathEdit, sender);

                    foreach (MapPath mp in MapPathMethods.GetMapPathList())
                    {
                        if (mp.IsSelected)
                        {
                            mp.ShowPathPoints = true;
                        }
                    }
                }
            }
            else
            {
                foreach (MapPath mp in MapPathMethods.GetMapPathList())
                {
                    if (mp.IsSelected)
                    {
                        mp.ShowPathPoints = false;
                    }
                }
            }

            SetDrawingModeLabel();

            MapImageBox.Refresh();
        }

        private void SolidLinePictureBox_Click(object sender, EventArgs e)
        {
            SolidLineRadio.Checked = !SolidLineRadio.Checked;
        }

        private void DottedLinePictureBox_Click(object sender, EventArgs e)
        {
            DottedLineRadio.Checked = !DottedLineRadio.Checked;
        }

        private void DashedLinePictureBox_Click(object sender, EventArgs e)
        {
            DashDotLineRadio.Checked = !DashedLineRadio.Checked;
        }

        private void DashDotPictureBox_Click(object sender, EventArgs e)
        {
            DashDotLineRadio.Checked = !DashDotLineRadio.Checked;
        }

        private void DashDotDotPictureBox_Click(object sender, EventArgs e)
        {
            DashDotDotLineRadio.Checked = !DashDotDotLineRadio.Checked;
        }

        private void DoubleSolidBorderPictureBox_Click(object sender, EventArgs e)
        {
            DoubleSolidBorderRadio.Checked = !DoubleSolidBorderRadio.Checked;
        }

        private void ChevronPictureBox_Click(object sender, EventArgs e)
        {
            ChevronLineRadio.Checked = !ChevronLineRadio.Checked;
        }

        private void LineDashPictureBox_Click(object sender, EventArgs e)
        {
            LineAndDashesRadio.Checked = !LineAndDashesRadio.Checked;
        }

        private void SmallDashesPictureBox_Click(object sender, EventArgs e)
        {
            SmallDashesRadio.Checked = !SmallDashesRadio.Checked;
        }

        private void ThickLinePictureBox_Click(object sender, EventArgs e)
        {
            ThickLineRadio.Checked = !ThickLineRadio.Checked;
        }

        private void BlackBorderLinePictureBox_Click(object sender, EventArgs e)
        {
            BlackBorderPathRadio.Checked = !BlackBorderPathRadio.Checked;
        }

        private void BorderedGradientPictureBox_Click(object sender, EventArgs e)
        {
            BorderedGradientRadio.Checked = !BorderedGradientRadio.Checked;
        }

        private void BorderedLightSolidPictureBox_Click(object sender, EventArgs e)
        {
            BorderedLightSolidRadio.Checked = !BorderedLightSolidRadio.Checked;
        }

        private void BearTracksPictureBox_Click(object sender, EventArgs e)
        {
            BearTracksRadio.Checked = !BearTracksRadio.Checked;
        }

        private void BirdTracksPictureBox_Click(object sender, EventArgs e)
        {
            BirdTracksRadio.Checked = !BirdTracksRadio.Checked;
        }

        private void FootPrintsPictureBox_Click(object sender, EventArgs e)
        {
            FootPrintsRadio.Checked = !FootPrintsRadio.Checked;
        }

        private void RailroadTracksPictureBox_Click(object sender, EventArgs e)
        {
            RailroadTracksRadio.Checked = !RailroadTracksRadio.Checked;
        }

        private void ShowPathLayerCheck_CheckedChanged(object sender, EventArgs e)
        {
            MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.PATHLOWERLAYER).ShowLayer = ShowPathLayerCheck.Checked;
            MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.PATHUPPERLAYER).ShowLayer = ShowPathLayerCheck.Checked;

            MapImageBox.Refresh();
        }

        #endregion

        #endregion

        #region Symbol Tab

        #region Symbol Tab Methods

        /******************************************************************************************************
        * *****************************************************************************************************
        * Symbol Methods
        * *****************************************************************************************************
        *******************************************************************************************************/
        private void AddSymbolsToSymbolTable(List<MapSymbol> symbols)
        {
            SymbolTable.Hide();
            SymbolTable.Controls.Clear();
            foreach (MapSymbol symbol in symbols)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                symbol.SetColorMappedBitmap(symbol.GetSymbolBitmap());
#pragma warning restore CS8604 // Possible null reference argument.

                Bitmap colorMappedBitmap = Extensions.ToBitmap(symbol.GetColorMappedBitmap());

                if (symbol.GetUseCustomColors())
                {
                    MapDrawingMethods.MapCustomColorsToColorableBitmap(ref colorMappedBitmap);

                }

                symbol.SetColorMappedBitmap(Extensions.ToSKBitmap(colorMappedBitmap));

                PictureBox pb = new()
                {
                    Tag = symbol,
                    SizeMode = PictureBoxSizeMode.Zoom,
                    Image = colorMappedBitmap,
                };

#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
                pb.MouseHover += SymbolPictureBox_MouseHover;
                pb.MouseClick += SymbolPictureBox_MouseClick;
#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).

                SymbolTable.Controls.Add(pb);
            }
            SymbolTable.Show();

            Refresh();
        }

        private void SymbolPictureBox_MouseHover(object sender, EventArgs e)
        {
            PictureBox pb = (PictureBox)sender;

            if (pb.Tag is MapSymbol s)
            {
                TOOLTIP.Show(s.GetSymbolName(), pb);
            }
        }

        private void SymbolPictureBox_MouseClick(object sender, EventArgs e)
        {
            if (((MouseEventArgs)e).Button == MouseButtons.Left)
            {
                if (ModifierKeys == Keys.Shift)
                {
                    // secondary symbol selection - for additional symbols to be used when painting symbols to the map (forests, etc.)
                    if (CURRENT_DRAWING_MODE == DrawingModeEnum.SymbolPlace)
                    {
                        PictureBox pb = (PictureBox)sender;

                        if (pb.BackColor == Color.AliceBlue)
                        {
                            pb.BackColor = SystemColors.Control;
                            pb.Refresh();

                            if (pb.Tag is MapSymbol s)
                            {
                                SymbolMethods.RemoveSecondarySelectedSymbol(s);
                            }
                        }
                        else
                        {
                            pb.BackColor = Color.AliceBlue;
                            pb.Refresh();

                            if (pb.Tag is MapSymbol s)
                            {
                                SymbolMethods.AddSecondarySelectedSymbol(s);
                            }
                        }
                    }
                }
                else if (ModifierKeys == Keys.None)
                {
                    // primary symbol selection                    

                    PictureBox pb = (PictureBox)sender;

                    if (pb.Tag is MapSymbol s)
                    {
                        foreach (Control control in SymbolTable.Controls)
                        {
                            if (control != pb)
                            {
                                control.BackColor = SystemColors.Control;
                                control.Refresh();
                            }
                        }

                        SymbolMethods.ClearSecondarySelectedSymbols();
                        Color pbBackColor = pb.BackColor;

                        if (pbBackColor == SystemColors.Control)
                        {
                            // clicked symbol is not selected, so select it
                            pb.BackColor = Color.LightSkyBlue;
                            pb.Refresh();

                            SymbolMethods.SetSelectedMapSymbol(s);

                            SetDrawingMode(DrawingModeEnum.SymbolPlace, null, true);
                        }
                        else
                        {
                            // clicked symbol is already selected, so deselect it
                            pb.BackColor = SystemColors.Control;
                            pb.Refresh();

                            SymbolMethods.ClearSelectedMapSymbol();

                            SetDrawingMode(DrawingModeEnum.None, null, true);
                        }
                    }
                }
            }
            else if (((MouseEventArgs)e).Button == MouseButtons.Right)
            {
                PictureBox pb = (PictureBox)sender;
                if (pb.Tag is MapSymbol s)
                {
                    SymbolInfo si = new(s);
                    si.ShowDialog();
                }
            }
        }

        private void PlaceSelectedSymbolAtCursor(SKPoint mouseCursorPoint)
        {
            MapSymbol? selectedSymbol = SymbolMethods.GetSelectedMapSymbol();
            if (selectedSymbol != null)
            {
                SKBitmap? symbolBitmap = selectedSymbol.GetSymbolBitmap();
                if (symbolBitmap != null)
                {
                    float symbolScale = SymbolScaleTrack.Value / 100.0F;
                    float symbolRotation = SymbolRotationTrack.Value;

                    SKBitmap rotatedAndScaledBitmap = RotateAndScaleSymbolBitmap(symbolBitmap, symbolScale, symbolRotation);
                    SKPoint cursorPoint = new(mouseCursorPoint.X - (rotatedAndScaledBitmap.Width / 2), mouseCursorPoint.Y - (rotatedAndScaledBitmap.Height / 2));

                    PlaceSelectedMapSymbolAtPoint(cursorPoint, PREVIOUS_LAYER_CLICK_POINT);
                }
            }
        }

        private List<MapSymbol> GetFilteredMapSymbols()
        {
            List<string> selectedCollections = SymbolCollectionsListBox.CheckedItems.Cast<string>().ToList();
            List<string> selectedTags = SymbolTagsListBox.CheckedItems.Cast<string>().ToList();
            List<MapSymbol> filteredSymbols = SymbolMethods.GetFilteredSymbolList(SELECTED_SYMBOL_TYPE, selectedCollections, selectedTags);

            return filteredSymbols;
        }

        private void PlaceSelectedMapSymbolAtPoint(SKPoint cursorPoint, SKPoint previousPoint)
        {
            MapSymbol? symbolToPlace = SymbolMethods.GetSelectedMapSymbol();

            if (symbolToPlace != null)
            {
                if (SymbolMethods.GetSecondarySelectedSymbols().Count > 0)
                {
                    int selectedIndex = Random.Shared.Next(0, SymbolMethods.GetSecondarySelectedSymbols().Count + 1);

                    if (selectedIndex > 0)
                    {
                        symbolToPlace = SymbolMethods.GetSecondarySelectedSymbolAtIndex(selectedIndex - 1);
                    }
                }

                SKBitmap? symbolBitmap = symbolToPlace.GetColorMappedBitmap();

                if (symbolBitmap != null)
                {
                    float symbolScale = SymbolScaleTrack.Value / 100.0F;
                    float symbolRotation = SymbolRotationTrack.Value;

                    SKBitmap scaledSymbolBitmap = MapDrawingMethods.ScaleBitmap(symbolBitmap, symbolScale);
                    SKBitmap rotatedAndScaledBitmap = MapDrawingMethods.RotateBitmap(scaledSymbolBitmap, symbolRotation, MirrorSymbolCheck.Checked);

                    if (rotatedAndScaledBitmap != null && (symbolToPlace.GetSymbolType() == SymbolTypeEnum.Vegetation || symbolToPlace.GetSymbolType() == SymbolTypeEnum.Terrain))
                    {
                        float bitmapSize = rotatedAndScaledBitmap.Width + rotatedAndScaledBitmap.Height;

                        // increasing this value reduces the rate of symbol placement on the map
                        // so high values of placement rate on the placement rate trackbar or updown increase placement rate on the map
                        float placementRateSize = bitmapSize / PlacementRate;

                        float pointDistanceSquared = SKPoint.DistanceSquared(previousPoint, cursorPoint);

                        if (pointDistanceSquared > placementRateSize)
                        {
                            bool canPlaceSymbol = SymbolMethods.CanPlaceSymbol(SymbolMethods.GetSelectedMapSymbol(), rotatedAndScaledBitmap, cursorPoint, PlacementDensity);

                            if (canPlaceSymbol)
                            {
                                SymbolMethods.PlaceSymbolOnMap(CURRENT_MAP, SymbolMethods.GetSelectedMapSymbol(), rotatedAndScaledBitmap, cursorPoint);
                            }
                        }
                    }
                    else
                    {
                        SymbolMethods.PlaceSymbolOnMap(CURRENT_MAP, SymbolMethods.GetSelectedMapSymbol(), rotatedAndScaledBitmap, cursorPoint);
                    }
                }
            }
        }

        private void PlaceSelectedSymbolInArea(SKPoint mouseCursorPoint)
        {
            MapSymbol? selectedSymbol = SymbolMethods.GetSelectedMapSymbol();
            if (selectedSymbol != null)
            {
                SKBitmap? symbolBitmap = selectedSymbol.GetSymbolBitmap();
                if (symbolBitmap != null)
                {
                    float symbolScale = SymbolScaleTrack.Value / 100.0F;
                    float symbolRotation = SymbolRotationTrack.Value;

                    SKBitmap rotatedAndScaledBitmap = RotateAndScaleSymbolBitmap(symbolBitmap, symbolScale, symbolRotation);

                    SKPoint cursorPoint = new(mouseCursorPoint.X - (rotatedAndScaledBitmap.Width / 2), mouseCursorPoint.Y - (rotatedAndScaledBitmap.Height / 2));

                    int exclusionRadius = (int)Math.Ceiling(PlacementDensity * ((rotatedAndScaledBitmap.Width + rotatedAndScaledBitmap.Height) / 2.0F));

                    List<SKPoint> areaPoints = MapDrawingMethods.GetPointsInCircle(cursorPoint, (int)Math.Ceiling(AreaBrushSizeTrack.Value / 2.0F), exclusionRadius);

                    foreach (SKPoint p in areaPoints)
                    {
                        SKPoint point = p;

                        // 1% randomization of point location
                        point.X = Random.Shared.Next((int)(p.X * 0.99F), (int)(p.X * 1.01F));
                        point.Y = Random.Shared.Next((int)(p.Y * 0.99F), (int)(p.Y * 1.01F));

                        PlaceSelectedMapSymbolAtPoint(point, PREVIOUS_LAYER_CLICK_POINT);
                    }
                }
            }
        }

        private SKBitmap RotateAndScaleSymbolBitmap(SKBitmap symbolBitmap, float symbolScale, float symbolRotation)
        {
            SKBitmap scaledSymbolBitmap = MapDrawingMethods.ScaleBitmap(symbolBitmap, symbolScale);

            SKBitmap rotatedAndScaledBitmap = MapDrawingMethods.RotateBitmap(scaledSymbolBitmap, symbolRotation, MirrorSymbolCheck.Checked);

            return rotatedAndScaledBitmap;
        }

        private void MoveSelectedSymbolInRenderOrder(ComponentMoveDirectionEnum direction)
        {
            if (UISelectedMapSymbol != null)
            {
                // find the selected symbol in the Symbol Layer MapComponents
                MapLayer symbolLayer = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.SYMBOLLAYER);

                List<MapComponent> symbolComponents = symbolLayer.MapLayerComponents;
                MapSymbol? selectedSymbol = null;

                int selectedSymbolIndex = 0;

                for (int i = 0; i < symbolComponents.Count; i++)
                {
                    MapComponent symbolComponent = symbolComponents[i];
                    if (symbolComponent is MapSymbol symbol && symbol.GetSymbolGuid() == UISelectedMapSymbol.GetSymbolGuid())
                    {
                        selectedSymbolIndex = i;
                        selectedSymbol = symbol;
                        break;
                    }
                }

                if (direction == ComponentMoveDirectionEnum.Up)
                {
                    // moving a symbol up in render order means increasing its index
                    if (selectedSymbol != null && selectedSymbolIndex < symbolComponents.Count - 1)
                    {
                        symbolComponents[selectedSymbolIndex] = symbolComponents[selectedSymbolIndex + 1];
                        symbolComponents[selectedSymbolIndex + 1] = selectedSymbol;
                    }
                }
                else if (direction == ComponentMoveDirectionEnum.Down)
                {
                    // moving a symbol down in render order means decreasing its index
                    // the map component at index 0 is the layer bitmap, so the selectedSymbolIndex must be great than 1 to move it down
                    if (selectedSymbol != null && selectedSymbolIndex > 1)
                    {
                        symbolComponents[selectedSymbolIndex] = symbolComponents[selectedSymbolIndex - 1];
                        symbolComponents[selectedSymbolIndex - 1] = selectedSymbol;
                    }
                }
            }
        }

        #endregion

        #region Symbol Tab Event Handlers

        /*******************************************************************************************************
        * Symbol Tab Event Handlers 
        *******************************************************************************************************/
        private void SymbolScaleTrack_Scroll(object sender, EventArgs e)
        {
            SymbolScaleUpDown.Value = SymbolScaleTrack.Value;
            SymbolScaleUpDown.Refresh();
        }

        private void SymbolScaleUpDown_ValueChanged(object sender, EventArgs e)
        {
            SymbolScaleTrack.Value = (int)SymbolScaleUpDown.Value;
            SymbolScaleTrack.Refresh();
        }

        private void SymbolRotationTrack_Scroll(object sender, EventArgs e)
        {
            SymbolRotationUpDown.Value = SymbolRotationTrack.Value;
            SymbolRotationUpDown.Refresh();
        }

        private void SymbolRotationUpDown_ValueChanged(object sender, EventArgs e)
        {
            SymbolRotationTrack.Value = (int)SymbolRotationUpDown.Value;
            SymbolRotationTrack.Refresh();
        }

        private void ResetRotationButton_Click(object sender, EventArgs e)
        {
            SymbolRotationUpDown.Value = 0;
            SymbolRotationUpDown.Refresh();
        }

        private void SelectSymbolButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.SymbolSelect, sender);
        }

        private void StructureSymbolsButton_Click(object sender, EventArgs e)
        {
            SELECTED_SYMBOL_TYPE = SymbolTypeEnum.Structure;
            List<MapSymbol> selectedSymbols = GetFilteredMapSymbols();

            AddSymbolsToSymbolTable(selectedSymbols);
        }

        private void VegetationSymbolsButton_Click(object sender, EventArgs e)
        {
            SELECTED_SYMBOL_TYPE = SymbolTypeEnum.Vegetation;
            List<MapSymbol> selectedSymbols = GetFilteredMapSymbols();

            AddSymbolsToSymbolTable(selectedSymbols);
        }

        private void TerrainSymbolsButton_Click(object sender, EventArgs e)
        {
            SELECTED_SYMBOL_TYPE = SymbolTypeEnum.Terrain;
            List<MapSymbol> selectedSymbols = GetFilteredMapSymbols();

            AddSymbolsToSymbolTable(selectedSymbols);
        }

        private void OtherSymbolsButton_Click(object sender, EventArgs e)
        {
            SELECTED_SYMBOL_TYPE = SymbolTypeEnum.Other;
            List<MapSymbol> selectedSymbols = GetFilteredMapSymbols();

            AddSymbolsToSymbolTable(selectedSymbols);
        }

        private void EraseSymbolsButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.SymbolErase, sender);
        }

        private void PlacementRateTrack_Scroll(object sender, EventArgs e)
        {
            PlacementRateUpDown.Value = PlacementRateTrack.Value / 40.0M;
            PlacementRateUpDown.Refresh();

            PlacementRate = (float)PlacementRateUpDown.Value;
        }

        private void PlacementRateUpDown_ValueChanged(object sender, EventArgs e)
        {
            PlacementRateTrack.Value = (int)(PlacementRateUpDown.Value * 40.0M);
            PlacementRateTrack.Refresh();

            PlacementRate = (float)PlacementRateUpDown.Value;
        }

        private void ResetPlacementRateButton_Click(object sender, EventArgs e)
        {
            PlacementRateUpDown.Value = 1.0M;
            PlacementRateUpDown.Refresh();
        }

        private void PlacementDensityTrack_Scroll(object sender, EventArgs e)
        {
            PlacementDensityUpDown.Value = PlacementDensityTrack.Value / 40.0M;
            PlacementDensityUpDown.Refresh();

            PlacementDensity = (float)PlacementDensityUpDown.Value;
        }

        private void PlacementDensityUpDown_ValueChanged(object sender, EventArgs e)
        {
            PlacementDensityTrack.Value = (int)(PlacementDensityUpDown.Value * 40.0M);
            PlacementDensityTrack.Refresh();

            PlacementDensity = (float)PlacementDensityUpDown.Value;
        }

        private void ResetPlacementDensityButton_Click(object sender, EventArgs e)
        {
            PlacementDensityUpDown.Value = 1.0M;
            PlacementDensityUpDown.Refresh();
        }

        private void AreaBrushSizeTrack_Scroll(object sender, EventArgs e)
        {
            AreaBrushSizeLabel.Text = AreaBrushSizeTrack.Value.ToString();
            AreaBrushSizeLabel.Refresh();
        }

        private void ColorSymbolsButton_Click(object sender, EventArgs e)
        {
            if (UISelectedMapSymbol != null)
            {
                // if a symbol has been selected and is grayscale, then color it with the
                // selected custom color

                SKColor? paintColor = SymbolMethods.GetCustomColorAtIndex(0);

                if (UISelectedMapSymbol.GetIsGrayScale() && paintColor != null)
                {
                    Cmd_PaintSymbol cmd = new(UISelectedMapSymbol, (SKColor)paintColor);
                    UndoManager.AddCommand(cmd);
                    cmd.DoOperation();

                    MapImageBox.Refresh();
                }
            }
            else
            {
                SetDrawingMode(DrawingModeEnum.SymbolColor, sender);
            }
        }

        private void SymbolColor1Label_Click(object sender, EventArgs e)
        {
            Color c = MapPaintMethods.SelectColorFromDialog(this, SymbolColor1Label.BackColor);

            SymbolColor1Label.BackColor = c;
            SymbolColor1Label.Refresh();

            SymbolMethods.SetCustomColorAtIndex(Extensions.ToSKColor(c), 0);
            List<MapSymbol> selectedSymbols = GetFilteredMapSymbols();
            AddSymbolsToSymbolTable(selectedSymbols);
        }

        private void SymbolColor2Label_Click(object sender, EventArgs e)
        {
            Color c = MapPaintMethods.SelectColorFromDialog(this, SymbolColor2Label.BackColor);

            SymbolColor2Label.BackColor = c;
            SymbolColor2Label.Refresh();

            SymbolMethods.SetCustomColorAtIndex(Extensions.ToSKColor(c), 1);
            List<MapSymbol> selectedSymbols = GetFilteredMapSymbols();
            AddSymbolsToSymbolTable(selectedSymbols);
        }

        private void SymbolColor3Label_Click(object sender, EventArgs e)
        {
            Color c = MapPaintMethods.SelectColorFromDialog(this, SymbolColor3Label.BackColor);

            SymbolColor3Label.BackColor = c;
            SymbolColor3Label.Refresh();

            SymbolMethods.SetCustomColorAtIndex(Extensions.ToSKColor(c), 2);
            List<MapSymbol> selectedSymbols = GetFilteredMapSymbols();
            AddSymbolsToSymbolTable(selectedSymbols);
        }

        private void SymbolColor4Label_Click(object sender, EventArgs e)
        {
            Color c = MapPaintMethods.SelectColorFromDialog(this, SymbolColor4Label.BackColor);

            SymbolColor4Label.BackColor = c;
            SymbolColor4Label.Refresh();

            SymbolMethods.SetCustomColorAtIndex(Extensions.ToSKColor(c), 3);
            List<MapSymbol> selectedSymbols = GetFilteredMapSymbols();
            AddSymbolsToSymbolTable(selectedSymbols);
        }

        private void ResetSymbolColorsButton_Click(object sender, EventArgs e)
        {
            // TODO: default symbol colors are set from theme?
            SymbolColor1Label.BackColor = Color.FromArgb(255, 85, 44, 36);
            SymbolColor1Label.Refresh();

            SymbolColor2Label.BackColor = Color.FromArgb(255, 53, 45, 32);
            SymbolColor2Label.Refresh();

            SymbolColor3Label.BackColor = Color.FromArgb(161, 214, 202, 171);
            SymbolColor3Label.Refresh();

            SymbolColor4Label.BackColor = Color.White;
            SymbolColor4Label.Refresh();
        }

        private void SymbolCollectionsListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            List<string> checkedCollections = new List<string>();
            foreach (var item in SymbolCollectionsListBox.CheckedItems)
            {
                checkedCollections.Add(item.ToString());

            }

            if (e.NewValue == CheckState.Checked)
            {
                checkedCollections.Add(SymbolCollectionsListBox.Items[e.Index].ToString());
            }
            else
            {
                checkedCollections.Remove(SymbolCollectionsListBox.Items[e.Index].ToString());
            }

#pragma warning restore CS8604 // Possible null reference argument.
            List<string> selectedTags = SymbolTagsListBox.CheckedItems.Cast<string>().ToList();
            List<MapSymbol> filteredSymbols = SymbolMethods.GetFilteredSymbolList(SELECTED_SYMBOL_TYPE, checkedCollections, selectedTags);
            AddSymbolsToSymbolTable(filteredSymbols);
        }

        private void SymbolTagsListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            List<string> checkedTags = [];
            foreach (var item in SymbolTagsListBox.CheckedItems)
            {
                checkedTags.Add(item.ToString());

            }

            if (e.NewValue == CheckState.Checked)
            {
                checkedTags.Add(SymbolTagsListBox.Items[e.Index].ToString());
            }
            else
            {
                checkedTags.Remove(SymbolTagsListBox.Items[e.Index].ToString());
            }

#pragma warning restore CS8604 // Possible null reference argument.
            List<string> selectedCollections = SymbolCollectionsListBox.CheckedItems.Cast<string>().ToList();
            List<MapSymbol> filteredSymbols = SymbolMethods.GetFilteredSymbolList(SELECTED_SYMBOL_TYPE, selectedCollections, checkedTags);
            AddSymbolsToSymbolTable(filteredSymbols);
        }

        private void SymbolTable_Scroll(object sender, ScrollEventArgs e)
        {
            SymbolTable.Refresh();
        }

        private void ShowSymbolLayerCheck_CheckedChanged(object sender, EventArgs e)
        {
            MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.SYMBOLLAYER).ShowLayer = ShowSymbolLayerCheck.Checked;

            MapImageBox.Refresh();
        }

        #endregion

        #endregion

        #region Label Tab

        #region Label Tab Methods

        /******************************************************************************************************
        * *****************************************************************************************************
        * Label Tab Methods
        * *****************************************************************************************************
        *******************************************************************************************************/

        private void LabelTextBox_KeyPress(object? sender, EventArgs e)
        {
            if (sender != null)
            {
                TextBox tb = (TextBox)sender;

                Font labelFont = MapLabelMethods.SELECTED_FONT;
                Color labelColor = FontColorSelectLabel.BackColor;

                if (((KeyPressEventArgs)e).KeyChar == (char)Keys.Return)
                {
                    ((KeyPressEventArgs)e).Handled = true;
                    MapLabelMethods.CreatingLabel = false;

                    Color outlineColor = OutlineColorSelectLabel.BackColor;
                    int outlineWidth = (int)OutlineWidthUpDown.Value;

                    Color glowColor = GlowColorSelectLabel.BackColor;
                    int glowStrength = (int)GlowStrengthUpDown.Value;

                    if (!string.IsNullOrEmpty(tb.Text))
                    {
                        // create a new MapLabel object and render it
                        MapLabel label = new()
                        {
                            LabelText = tb.Text,
                            LabelFont = labelFont,
                            IsSelected = true,
                            LabelColor = labelColor,
                            LabelOutlineColor = outlineColor,
                            LabelOutlineWidth = outlineWidth,
                            LabelGlowColor = glowColor,
                            LabelGlowStrength = glowStrength,
                        };

                        SKPaint paint = MapLabelMethods.CreateLabelPaint(labelFont, labelColor);
                        SKFont paintFont = paint.ToFont();

                        label.LabelPaint = paint;
                        label.LabelSKFont.Dispose();
                        label.LabelSKFont = paintFont;

                        SKRect bounds = new SKRect();
                        paint.MeasureText(label.LabelText, ref bounds);

                        Point labelPoint = MapImageBox.PointToImage(new Point(tb.Left, tb.Top));

                        label.X = labelPoint.X;
                        label.Y = labelPoint.Y;
                        label.Width = (int)bounds.Width;
                        label.Height = (int)bounds.Height;

                        if (tb.Tag != null && tb.Tag is SKPath path)
                        {
                            label.LabelPath = path;
                        }
                        else if (MapLabelMethods.LABEL_PATH.PointCount > 0)
                        {
                            label.LabelPath = new(MapLabelMethods.LABEL_PATH);
                            MapLabelMethods.LABEL_PATH.Dispose();
                            MapLabelMethods.LABEL_PATH = new();
                            MapBuilder.GetLayerCanvas(CURRENT_MAP, MapBuilder.WORKLAYER)?.Clear();
                        }

                        Cmd_AddLabel cmd = new(CURRENT_MAP, label);
                        UndoManager.AddCommand(cmd);
                        cmd.DoOperation();

                        MapPaintMethods.DeselectAllMapComponents(label);

                        UISelectedLabel = MapLabelMethods.MAP_LABELS.Last();
                    }

                    SetDrawingMode(DrawingModeEnum.LabelSelect, SelectLabelButton, true);

                    // dispose of the text box, as it isn't needed once the label text has been entered
                    MapImageBox.Controls.Remove(tb);
                    tb.Dispose();

                    MapImageBox.Refresh();
                }
                else
                {
                    if (tb.Text.StartsWith("...Label..."))
                    {
                        tb.Text = tb.Text.Substring("...Label...".Length);
                    }

                    SKPaint paint = MapLabelMethods.CreateLabelPaint(labelFont, labelColor);
                    SKRect bounds = new SKRect();

                    paint.MeasureText(tb.Text, ref bounds);
                    int tbWidth = (int)Math.Max(bounds.Width, tb.Width);
                    tb.Width = tbWidth;
                }
            }
        }

        private void UpdateSelectedLabelOnUIChange()
        {
            if (UISelectedLabel != null)
            {
                Color labelColor = FontColorSelectLabel.BackColor;
                Color outlineColor = OutlineColorSelectLabel.BackColor;
                int outlineWidth = (int)OutlineWidthUpDown.Value;
                Color glowColor = GlowColorSelectLabel.BackColor;
                int glowStrength = (int)GlowStrengthUpDown.Value;

                Cmd_ChangeLabelAttributes cmd = new(UISelectedLabel, labelColor, outlineColor, outlineWidth, glowColor, glowStrength, MapLabelMethods.SELECTED_FONT);
                UndoManager.AddCommand(cmd);
                cmd.DoOperation();

                MapImageBox.Refresh();
            }
        }

        private void AddMapBoxesToLabelBoxTable(List<MapBox> mapBoxes)
        {
            LabelBoxStyleTable.Hide();
            LabelBoxStyleTable.Controls.Clear();
            foreach (MapBox box in mapBoxes)
            {
                if (box.BoxBitmap == null && box.BoxBitmapPath != null)
                {
                    box.BoxBitmap ??= new Bitmap(box.BoxBitmapPath);
                }

                if (box.BoxBitmap != null)
                {
                    PictureBox pb = new()
                    {
                        Tag = box,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Image = (Image)box.BoxBitmap.Clone(),
                    };

#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
                    pb.MouseClick += MapBoxPictureBox_MouseClick;
#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).

                    LabelBoxStyleTable.Controls.Add(pb);
                }

            }
            LabelBoxStyleTable.Show();
            LabelBoxStyleTable.Refresh();
        }

        private void MapBoxPictureBox_MouseClick(object sender, EventArgs e)
        {
            if (((MouseEventArgs)e).Button == MouseButtons.Left)
            {
                if (ModifierKeys == Keys.None)
                {
                    PictureBox pb = (PictureBox)sender;

                    if (pb.Tag is MapBox b)
                    {
                        foreach (Control control in LabelBoxStyleTable.Controls)
                        {
                            if (control != pb)
                            {
                                control.BackColor = SystemColors.Control;
                            }
                        }

                        Color pbBackColor = pb.BackColor;

                        if (pbBackColor.ToArgb() == SystemColors.Control.ToArgb())
                        {
                            // clicked symbol is not selected, so select it
                            pb.BackColor = Color.LightSkyBlue;
                            MapLabelMethods.SetSelectedMapBox(b);
                        }
                        else
                        {
                            // clicked symbol is already selected, so deselect it
                            pb.BackColor = SystemColors.Control;
                            MapLabelMethods.ClearSelectedMapBox();
                        }
                    }
                }
            }
        }

        private void SetLabelValuesFromPreset(LabelPreset preset)
        {
            FontColorSelectLabel.BackColor = Color.FromArgb(preset.LabelColor);
            FontColorSelectLabel.Refresh();

            OutlineColorSelectLabel.BackColor = Color.FromArgb(preset.LabelOutlineColor);
            OutlineColorSelectLabel.Refresh();

            OutlineWidthUpDown.Value = preset.LabelOutlineWidth;
            OutlineWidthUpDown.Refresh();

            GlowColorSelectLabel.BackColor = Color.FromArgb(preset.LabelGlowColor);
            GlowColorSelectLabel.Refresh();

            GlowStrengthUpDown.Value = preset.LabelGlowStrength;
            GlowStrengthUpDown.Refresh();

            string fontString = preset.LabelFontString;
            FontConverter cvt = new();
            Font? font = (Font?)cvt.ConvertFromString(fontString);

            if (font != null)
            {
                MapLabelMethods.SELECTED_FONT = font;
                SelectLabelFontButton.Font = new Font(font.FontFamily, 14);
                SelectLabelFontButton.Refresh();
            }
        }

        #endregion

        #region Label Tab Event Handlers

        /*******************************************************************************************************
        * Label Tab Event Handlers 
        *******************************************************************************************************/
        private void LabelPresetCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LabelPresetCombo.SelectedIndex >= 0 && LabelPresetCombo.SelectedIndex < MapLabelMethods.LABEL_PRESETS.Count)
            {
                LabelPreset selectedPreset = MapLabelMethods.LABEL_PRESETS[LabelPresetCombo.SelectedIndex];
                SetLabelValuesFromPreset(selectedPreset);
            }
        }

        private void AddPresetButton_Click(object sender, EventArgs e)
        {
            LabelPresetNameEntry presetDialog = new();
            DialogResult result = presetDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                string presetName = presetDialog.PresetNameTextBox.Text;

                string currentThemeName = string.Empty;

                if (MapPaintMethods.CURRENT_THEME != null && !string.IsNullOrEmpty(MapPaintMethods.CURRENT_THEME.ThemeName))
                {
                    currentThemeName = MapPaintMethods.CURRENT_THEME.ThemeName;
                }
                else
                {
                    currentThemeName = "DEFAULT";
                }

                string presetFileName = Resources.ASSET_DIRECTORY + Path.DirectorySeparatorChar + "LabelPresets" + Path.DirectorySeparatorChar + Guid.NewGuid().ToString() + ".mclblprst";

                bool makeNewPreset = true;

                if (File.Exists(presetFileName))
                {
                    LabelPreset? existingPreset = MapLabelMethods.LABEL_PRESETS.Find(x => x.LabelPresetName == presetName && x.LabelPresetTheme == currentThemeName);
                    if (existingPreset != null && existingPreset.IsDefault)
                    {
                        makeNewPreset = false;
                    }
                    else
                    {
                        DialogResult r = MessageBox.Show("A label preset named " + presetName + " for theme " + currentThemeName + " already exists. Replace it?", "Replace Preset", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                        if (r == DialogResult.No)
                        {
                            makeNewPreset = false;
                        }
                    }
                }

                if (makeNewPreset)
                {
                    LabelPreset preset = new();

                    FontConverter cvt = new();
                    string? fontString = cvt.ConvertToString(MapLabelMethods.SELECTED_FONT);

                    if (!string.IsNullOrEmpty(fontString))
                    {
                        preset.LabelPresetName = presetName;
                        if (MapPaintMethods.CURRENT_THEME != null && !string.IsNullOrEmpty(MapPaintMethods.CURRENT_THEME.ThemeName))
                        {
                            preset.LabelPresetTheme = MapPaintMethods.CURRENT_THEME.ThemeName;
                        }
                        else
                        {
                            preset.LabelPresetTheme = "DEFAULT";
                        }

                        preset.LabelFontString = fontString;
                        preset.LabelColor = FontColorSelectLabel.BackColor.ToArgb();
                        preset.LabelOutlineColor = OutlineColorSelectLabel.BackColor.ToArgb();
                        preset.LabelOutlineWidth = (int)OutlineWidthUpDown.Value;
                        preset.LabelGlowColor = GlowColorSelectLabel.BackColor.ToArgb();
                        preset.LabelGlowStrength = (int)GlowStrengthUpDown.Value;

                        preset.PresetXmlFilePath = presetFileName;

                        MapFileMethods.SerializeLabelPreset(preset);

                        LoadAllAssets();
                    }
                }
            }
        }

        private void RemovePresetButton_Click(object sender, EventArgs e)
        {
            //remove a preset (prevent default presets from being deleted or changed)

            if (LabelPresetCombo.SelectedIndex >= 0)
            {
                string? presetName = (string?)LabelPresetCombo.SelectedItem;

                if (!string.IsNullOrEmpty(presetName))
                {
                    string currentThemeName = string.Empty;

                    if (MapPaintMethods.CURRENT_THEME != null && !string.IsNullOrEmpty(MapPaintMethods.CURRENT_THEME.ThemeName))
                    {
                        currentThemeName = MapPaintMethods.CURRENT_THEME.ThemeName;
                    }
                    else
                    {
                        currentThemeName = "DEFAULT";
                    }

                    LabelPreset? existingPreset = MapLabelMethods.LABEL_PRESETS.Find(x => x.LabelPresetName == presetName && x.LabelPresetTheme == currentThemeName);

                    if (existingPreset != null && !existingPreset.IsDefault)
                    {
                        if (!string.IsNullOrEmpty(existingPreset.PresetXmlFilePath))
                        {
                            if (File.Exists(existingPreset.PresetXmlFilePath))
                            {
                                DialogResult r = MessageBox.Show("The label preset named " + presetName + " for theme " + currentThemeName + " will be deleted. Continue?", "Delete Label Preset",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                                if (r == DialogResult.Yes)
                                {
                                    try
                                    {
                                        File.Delete(existingPreset.PresetXmlFilePath);
                                        LoadAllAssets();
                                        MessageBox.Show("The label preset has been deleted.", "Preset Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                                    }
                                    catch (Exception ex)
                                    {
                                        Program.LOGGER.Error(ex);
                                        MessageBox.Show("The label preset could not be deleted.", "Preset Not Deleted", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("The selected label preset cannot be deleted.", "Preset Not Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    }
                }
            }
        }

        private void SelectLabelFontButton_Click(object sender, EventArgs e)
        {
            FontSelectionDialog fsd = new FontSelectionDialog();
            DialogResult result = fsd.ShowDialog(this);

            if (result == DialogResult.OK && fsd.SelectedFont != null)
            {
                SelectLabelFontButton.Font = new Font(fsd.SelectedFont.FontFamily, 14);
                SelectLabelFontButton.Refresh();

                MapLabelMethods.SELECTED_FONT = fsd.SelectedFont;

                UpdateSelectedLabelOnUIChange();
            }
        }

        private void FontColorSelectLabel_Click(object sender, EventArgs e)
        {
            Color labelColor = MapPaintMethods.SelectColorFromDialog(this, FontColorSelectLabel.BackColor);

            if (labelColor.ToArgb() != Color.Empty.ToArgb())
            {
                FontColorSelectLabel.BackColor = labelColor;

                FontColorSelectLabel.Refresh();

                UpdateSelectedLabelOnUIChange();
            }
        }

        private void OutlineColorSelectLabel_Click(object sender, EventArgs e)
        {
            Color outlineColor = MapPaintMethods.SelectColorFromDialog(this, OutlineColorSelectLabel.BackColor);

            if (outlineColor.ToArgb() != Color.Empty.ToArgb())
            {
                OutlineColorSelectLabel.BackColor = outlineColor;
                OutlineColorSelectLabel.Refresh();

                UpdateSelectedLabelOnUIChange();
            }
        }

        private void OutlineWidthUpDown_ValueChanged(object sender, EventArgs e)
        {
            UpdateSelectedLabelOnUIChange();
        }

        private void GlowColorSelectLabel_Click(object sender, EventArgs e)
        {
            Color glowColor = MapPaintMethods.SelectColorFromDialog(this, GlowColorSelectLabel.BackColor);

            if (glowColor.ToArgb() != Color.Empty.ToArgb())
            {
                GlowColorSelectLabel.BackColor = glowColor;
                GlowColorSelectLabel.Refresh();

                UpdateSelectedLabelOnUIChange();
            }
        }

        private void GlowStrengthUpDown_ValueChanged(object sender, EventArgs e)
        {
            UpdateSelectedLabelOnUIChange();
        }

        private void CircleTextPathButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.DrawArcLabelPath, sender);
        }

        private void BezierTextPathButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.DrawBezierLabelPath, sender);
        }

        private void SelectLabelButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.LabelSelect, sender);
        }

        private void PlaceLabelButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.DrawLabel, sender);
        }

        private void CreateBoxButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.DrawBox, sender);
        }

        private void LabelRotationTrack_Scroll(object sender, EventArgs e)
        {
            // triggers on value change
            if (UISelectedLabel != null)
            {
                LabelRotationUpDown.Value = LabelRotationTrack.Value;
                LabelRotationUpDown.Refresh();

                MapImageBox.Refresh();
            }
        }

        private void LabelRotationUpDown_ValueChanged(object sender, EventArgs e)
        {
            if (UISelectedLabel != null)
            {
                Cmd_ChangeLabelRotation cmd = new(UISelectedLabel, (float)LabelRotationUpDown.Value, this);
                UndoManager.AddCommand(cmd);
                cmd.DoOperation();

                LabelRotationTrack.Value = (int)LabelRotationUpDown.Value;
                LabelRotationTrack.Refresh();

                MapImageBox.Refresh();
            }
        }

        private void SelectBoxTintLabel_Click(object sender, EventArgs e)
        {
            Color boxColor = MapPaintMethods.SelectColorFromDialog(this, SelectBoxTintLabel.BackColor);

            if (boxColor.ToArgb() != Color.Empty.ToArgb())
            {
                SelectBoxTintLabel.BackColor = boxColor;

                SelectBoxTintLabel.Refresh();

                if (UISelectedBox != null)
                {
                    Cmd_ChangeBoxColor cmd = new(UISelectedBox, boxColor);
                    UndoManager.AddCommand(cmd);
                    cmd.DoOperation();

                    MapImageBox.Refresh();
                }
            }
        }

        // name generator
        private void GenerateNameButton_Click(object sender, EventArgs e)
        {
            if (ModifierKeys == Keys.None)
            {
                string generatedName = MapToolMethods.GenerateRandomPlaceName();

                if (MapLabelMethods.CreatingLabel)
                {
                    if (LABEL_TEXT_BOX != null && !LABEL_TEXT_BOX.IsDisposed)
                    {
                        LABEL_TEXT_BOX.Text = generatedName;
                        LABEL_TEXT_BOX.Refresh();
                    }
                }
            }
            else if (ModifierKeys == Keys.Shift)
            {
                NAME_GENERATOR_SETTINGS_DIALOG.Show();
            }
        }

        private void ShowLabelLayerCheck_CheckedChanged(object sender, EventArgs e)
        {
            MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.LABELLAYER).ShowLayer = ShowLabelLayerCheck.Checked;
            MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.BOXLAYER).ShowLayer = ShowLabelLayerCheck.Checked;

            MapImageBox.Refresh();
        }

        #endregion

        #endregion

        #region Overlay Tab

        #region Overlay Tab Methods

        /******************************************************************************************************
        * *****************************************************************************************************
        * Overlay Tab Methods
        * *****************************************************************************************************
        *******************************************************************************************************/

        private void AddMapFramesToFrameTable(List<MapFrame> mapFrames)
        {
            FrameStyleTable.Hide();
            FrameStyleTable.Controls.Clear();
            foreach (MapFrame frame in mapFrames)
            {
                if (frame.FrameBitmapPath != null)
                {
                    if (frame.FrameBitmap == null)
                    {
                        SKImage image = SKImage.FromEncodedData(frame.FrameBitmapPath);
                        frame.FrameBitmap ??= SKBitmap.FromImage(image);
                    }

                    PictureBox pb = new()
                    {
                        Tag = frame,
                        SizeMode = PictureBoxSizeMode.Zoom,
                        Image = frame.FrameBitmap.ToBitmap(),
                    };

#pragma warning disable CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).
                    pb.MouseClick += FramePictureBox_MouseClick;
#pragma warning restore CS8622 // Nullability of reference types in type of parameter doesn't match the target delegate (possibly because of nullability attributes).

                    FrameStyleTable.Controls.Add(pb);
                }
            }
            FrameStyleTable.Show();
        }

        private void CreateGrid()
        {
            UIMapGrid = new MapGrid
            {
                ParentMap = CURRENT_MAP,
                GridEnabled = true,
                GridColor = GridColorSelectLabel.BackColor,
                GridLineWidth = GridLineWidthTrack.Value,
                GridSize = GridSizeTrack.Value,
                Width = CURRENT_MAP.MapWidth,
                Height = CURRENT_MAP.MapHeight,
            };

            if (SquareGridRadio.Checked)
            {
                UIMapGrid.GridType = GridTypeEnum.Square;
            }
            else if (FlatHexRadio.Checked)
            {
                UIMapGrid.GridType = GridTypeEnum.FlatHex;
                UIMapGrid.GridSize /= 2;
            }
            else if (PointedHexRadio.Checked)
            {
                UIMapGrid.GridType = GridTypeEnum.PointedHex;
                UIMapGrid.GridSize /= 2;
            }

            string? selectedLayerItem = (string?)LayerUpDown.SelectedItem;

            if (selectedLayerItem != null)
            {
                switch (selectedLayerItem)
                {
                    case "Default":
                        UIMapGrid.GridLayerIndex = MapBuilder.DEFAULTGRIDLAYER;
                        break;
                    case "Above Ocean":
                        UIMapGrid.GridLayerIndex = MapBuilder.ABOVEOCEANGRIDLAYER;
                        break;
                    case "Below Symbols":
                        UIMapGrid.GridLayerIndex = MapBuilder.BELOWSYMBOLSGRIDLAYER;
                        break;
                    default:
                        UIMapGrid.GridLayerIndex = MapBuilder.DEFAULTGRIDLAYER;
                        break;
                }
            }
            else
            {
                UIMapGrid.GridLayerIndex = MapBuilder.DEFAULTGRIDLAYER;
            }

            UIMapGrid.GridPaint = new()
            {
                Style = SKPaintStyle.Stroke,
                Color = UIMapGrid.GridColor.ToSKColor(),
                StrokeWidth = UIMapGrid.GridLineWidth,
                StrokeJoin = SKStrokeJoin.Bevel
            };
        }

        private void RemoveGrid()
        {
            for (int i = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.DEFAULTGRIDLAYER).MapLayerComponents.Count - 1; i > 0; i--)
            {
                if (MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.DEFAULTGRIDLAYER).MapLayerComponents[i] is MapGrid)
                {
                    MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.DEFAULTGRIDLAYER).MapLayerComponents.RemoveAt(i);
                    break;
                }
            }

            for (int i = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.ABOVEOCEANGRIDLAYER).MapLayerComponents.Count - 1; i > 0; i--)
            {
                if (MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.ABOVEOCEANGRIDLAYER).MapLayerComponents[i] is MapGrid)
                {
                    MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.ABOVEOCEANGRIDLAYER).MapLayerComponents.RemoveAt(i);
                    break;
                }
            }

            for (int i = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.BELOWSYMBOLSGRIDLAYER).MapLayerComponents.Count - 1; i > 0; i--)
            {
                if (MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.BELOWSYMBOLSGRIDLAYER).MapLayerComponents[i] is MapGrid)
                {
                    MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.BELOWSYMBOLSGRIDLAYER).MapLayerComponents.RemoveAt(i);
                    break;
                }
            }
        }

        private MapMeasure? GetCurrentMapMeasure()
        {
            MapMeasure? currentMapMeasure = UIMapMeasure;

            if (currentMapMeasure == null)
            {
                MapLayer measureLayer = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.MEASURELAYER);

                for (int i = measureLayer.MapLayerComponents.Count - 1; i >= 0; i--)
                {
                    if (measureLayer.MapLayerComponents[i] is MapMeasure)
                    {
                        currentMapMeasure = (MapMeasure?)measureLayer.MapLayerComponents[i];
                        break;
                    }
                }
            }

            return currentMapMeasure;
        }

        #endregion

        #region Overlay Tab Event Handlers

        /*******************************************************************************************************
        * Overlay Tab Event Handlers 
        *******************************************************************************************************/

        private void FramePictureBox_MouseClick(object sender, EventArgs e)
        {
            if (((MouseEventArgs)e).Button == MouseButtons.Left)
            {
                if (ModifierKeys == Keys.None)
                {
                    PictureBox pb = (PictureBox)sender;

                    if (pb.Tag is MapFrame frame)
                    {
                        foreach (Control control in FrameStyleTable.Controls)
                        {
                            if (control != pb)
                            {
                                control.BackColor = SystemColors.Control;
                            }
                        }

                        Color pbBackColor = pb.BackColor;

                        if (pbBackColor.ToArgb() == SystemColors.Control.ToArgb())
                        {
                            // clicked symbol is not selected, so select it
                            pb.BackColor = Color.LightSkyBlue;

                            OverlayMethods.SelectedFrame = frame;

                            Cmd_CreateMapFrame cmd = new(CURRENT_MAP, frame, SelectFrameTintLabel.BackColor, (float)FrameScaleUpDown.Value);
                            UndoManager.AddCommand(cmd);
                            cmd.DoOperation();

                            MapImageBox.Refresh();
                        }
                        else
                        {
                            // clicked symbol is already selected, so deselect it
                            pb.BackColor = SystemColors.Control;
                            OverlayMethods.SelectedFrame = null;
                        }
                    }
                }
            }
        }

        private void FrameStyleTable_Scroll(object sender, ScrollEventArgs e)
        {
            FrameStyleTable.Refresh();
        }

        private void FrameScaleUpDown_ValueChanged(object sender, EventArgs e)
        {
            // there can only be one frame on the map, so find it and update the scale
            for (int i = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.OVERLAYLAYER).MapLayerComponents.Count - 1; i > 0; i--)
            {
                if (MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.OVERLAYLAYER).MapLayerComponents[i] is PlacedMapFrame)
                {
                    PlacedMapFrame placedFrame = (PlacedMapFrame)MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.OVERLAYLAYER).MapLayerComponents[i];
                    placedFrame.FrameScale = (float)FrameScaleUpDown.Value;

                    OverlayMethods.CompletePlacedFrame(placedFrame);

                    MapImageBox.Refresh();
                    break;
                }
            }

            FrameScaleTrack.Value = (int)(FrameScaleUpDown.Value * 100);
            FrameScaleTrack.Refresh();
        }

        private void FrameScaleTrack_Scroll(object sender, EventArgs e)
        {
            FrameScaleUpDown.Value = (decimal)(FrameScaleTrack.Value / 100.0);
            FrameScaleUpDown.Refresh();
        }

        private void EnableFrameCheck_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.OVERLAYLAYER).MapLayerComponents.Count - 1; i > 0; i--)
            {
                if (MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.OVERLAYLAYER).MapLayerComponents[i] is PlacedMapFrame)
                {
                    PlacedMapFrame placedFrame = (PlacedMapFrame)MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.OVERLAYLAYER).MapLayerComponents[i];

                    placedFrame.FrameEnabled = EnableFrameCheck.Checked;

                    MapImageBox.Refresh();
                    break;
                }
            }
        }

        private void SelectFrameTintLabel_Click(object sender, EventArgs e)
        {
            Color frameColor = MapPaintMethods.SelectColorFromDialog(this, SelectFrameTintLabel.BackColor);

            if (frameColor.ToArgb() != Color.Empty.ToArgb())
            {
                SelectFrameTintLabel.BackColor = frameColor;
                SelectFrameTintLabel.Refresh();

                for (int i = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.OVERLAYLAYER).MapLayerComponents.Count - 1; i > 0; i--)
                {
                    if (MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.OVERLAYLAYER).MapLayerComponents[i] is PlacedMapFrame)
                    {
                        PlacedMapFrame placedFrame = (PlacedMapFrame)MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.OVERLAYLAYER).MapLayerComponents[i];

                        if (placedFrame != null)
                        {
                            Cmd_ChangeFrameColor cmd = new(placedFrame, SelectFrameTintLabel.BackColor);
                            UndoManager.AddCommand(cmd);
                            cmd.DoOperation();
                        }

                        MapImageBox.Refresh();
                        break;
                    }
                }
            }
        }

        // grid
        private void GridButton_Click(object sender, EventArgs e)
        {
            if (EnableGridCheck.Checked)
            {
                // make sure there is only one grid
                RemoveGrid();

                CreateGrid();

                if (UIMapGrid != null)
                {
                    MapBuilder.GetMapLayerByIndex(CURRENT_MAP, UIMapGrid.GridLayerIndex).MapLayerComponents.Add(UIMapGrid);
                }

                MapImageBox.Refresh();
            }
            else
            {
                // make sure there is only one grid
                RemoveGrid();

                MapImageBox.Refresh();
            }
        }

        private void EnableGridCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (UIMapGrid != null)
            {
                UIMapGrid.GridEnabled = EnableGridCheck.Checked;

                MapImageBox.Refresh();
            }
        }

        private void SquareGridRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (UIMapGrid != null)
            {
                UIMapGrid.GridType = GridTypeEnum.Square;

                MapImageBox.Refresh();
            }
        }

        private void FlatHexRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (UIMapGrid != null)
            {
                UIMapGrid.GridType = GridTypeEnum.FlatHex;

                MapImageBox.Refresh();
            }
        }

        private void PointedHexRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (UIMapGrid != null)
            {
                UIMapGrid.GridType = GridTypeEnum.PointedHex;

                MapImageBox.Refresh();
            }
        }

        private void LayerUpDown_SelectedItemChanged(object sender, EventArgs e)
        {
            if (EnableGridCheck.Checked)
            {
                // make sure there is only one grid
                RemoveGrid();

                CreateGrid();

                if (UIMapGrid != null)
                {
                    MapBuilder.GetMapLayerByIndex(CURRENT_MAP, UIMapGrid.GridLayerIndex).MapLayerComponents.Add(UIMapGrid);
                }

                MapImageBox.Refresh();
            }
            else
            {
                // make sure there is only one grid
                RemoveGrid();

                MapImageBox.Refresh();
            }
        }

        private void GridSizeTrack_Scroll(object sender, EventArgs e)
        {
            GridSizeLabel.Text = GridSizeTrack.Value.ToString();
            GridSizeLabel.Refresh();

            if (UIMapGrid != null)
            {
                UIMapGrid.GridSize = GridSizeTrack.Value;
                MapImageBox.Refresh();
            }
        }

        private void GridLineWidthTrack_Scroll(object sender, EventArgs e)
        {
            GridLineWidthLabel.Text = GridLineWidthTrack.Value.ToString();
            GridLineWidthLabel.Refresh();

            if (UIMapGrid != null)
            {
                UIMapGrid.GridLineWidth = GridLineWidthTrack.Value;

                UIMapGrid.GridPaint = new()
                {
                    Style = SKPaintStyle.Stroke,
                    Color = UIMapGrid.GridColor.ToSKColor(),
                    StrokeWidth = UIMapGrid.GridLineWidth,
                    StrokeJoin = SKStrokeJoin.Bevel
                };

                MapImageBox.Refresh();
            }
        }

        private void GridColorSelectLabel_Click(object sender, EventArgs e)
        {
            Color gridColor = MapPaintMethods.SelectColorFromDialog(this, GridColorSelectLabel.BackColor);

            if (gridColor.ToArgb() != Color.Empty.ToArgb())
            {
                GridColorSelectLabel.BackColor = gridColor;
                GridColorSelectLabel.Refresh();

                if (UIMapGrid != null)
                {
                    UIMapGrid.GridColor = GridColorSelectLabel.BackColor;

                    UIMapGrid.GridPaint = new()
                    {
                        Style = SKPaintStyle.Stroke,
                        Color = UIMapGrid.GridColor.ToSKColor(),
                        StrokeWidth = UIMapGrid.GridLineWidth,
                        StrokeJoin = SKStrokeJoin.Bevel
                    };

                    MapImageBox.Refresh();
                }

            }
        }

        private void ShowGridSizeCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (UIMapGrid != null)
            {
                UIMapGrid.ShowGridSize = ShowGridSizeCheck.Checked;

                MapImageBox.Refresh();
            }
        }

        // map scale object
        private void ScaleButton_Click(object sender, EventArgs e)
        {
#pragma warning disable CS8604 // Possible null reference argument.

            MapScaleCreator scaleCreator = new MapScaleCreator(CURRENT_MAP, MAP_CANVAS);

            // TODO: fix hardcoded location
            scaleCreator.Location = PointToScreen(new Point(1080, 52));
            scaleCreator.Show(this);

#pragma warning restore CS8604 // Possible null reference argument.           
        }

        private void MeasureButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.DrawMapMeasure, sender);
        }

        private void MeasureColorLabel_Click(object sender, EventArgs e)
        {
            Color measureColor = MapPaintMethods.SelectColorFromDialog(this, MeasureColorLabel.BackColor);

            if (measureColor.ToArgb() != Color.Empty.ToArgb())
            {
                MeasureColorLabel.BackColor = measureColor;
                MeasureColorLabel.Refresh();

                MapMeasure? currentMapMeasure = GetCurrentMapMeasure();

                if (currentMapMeasure != null)
                {
                    currentMapMeasure.MeasureLineColor = MeasureColorLabel.BackColor;

                    currentMapMeasure.MeasureLinePaint = new()
                    {
                        Style = SKPaintStyle.Stroke,
                        StrokeWidth = 1,
                        Color = currentMapMeasure.MeasureLineColor.ToSKColor()
                    };

                    currentMapMeasure.MeasureAreaPaint = new()
                    {
                        Style = SKPaintStyle.StrokeAndFill,
                        StrokeWidth = 1,
                        Color = currentMapMeasure.MeasureLineColor.ToSKColor()
                    };

                    MapImageBox.Refresh();
                }

            }
        }

        private void UseScaleUnitsCheck_CheckedChanged(object sender, EventArgs e)
        {
            MapMeasure? currentMapMeasure = GetCurrentMapMeasure();

            if (currentMapMeasure != null)
            {
                currentMapMeasure.UseMapUnits = UseScaleUnitsCheck.Checked;

                MapImageBox.Refresh();
            }
        }

        private void MeasureAreaCheck_CheckedChanged(object sender, EventArgs e)
        {
            MapMeasure? currentMapMeasure = GetCurrentMapMeasure();

            if (currentMapMeasure != null)
            {
                currentMapMeasure.MeasureArea = MeasureAreaCheck.Checked;

                MapImageBox.Refresh();
            }
        }

        private void ClearMeasureObjects_Click(object sender, EventArgs e)
        {
            MapLayer measureLayer = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.MEASURELAYER);

            for (int i = measureLayer.MapLayerComponents.Count - 1; i >= 0; i--)
            {
                if (measureLayer.MapLayerComponents[i] is MapMeasure)
                {
                    measureLayer.MapLayerComponents.RemoveAt(i);
                }
            }

            MapImageBox.Refresh();
        }

        private void ShowOverlayLayerCheck_CheckedChanged(object sender, EventArgs e)
        {
            MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.OVERLAYLAYER).ShowLayer = ShowOverlayLayerCheck.Checked;
            MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.DEFAULTGRIDLAYER).ShowLayer = ShowOverlayLayerCheck.Checked;
            MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.ABOVEOCEANGRIDLAYER).ShowLayer = ShowOverlayLayerCheck.Checked;
            MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.BELOWSYMBOLSGRIDLAYER).ShowLayer = ShowOverlayLayerCheck.Checked;
            MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.MEASURELAYER).ShowLayer = ShowOverlayLayerCheck.Checked;

            MapImageBox.Refresh();
        }

        #endregion

        #endregion

        #region Region Tab

        #region Region Tab Methods

        /******************************************************************************************************
        * *****************************************************************************************************
        * Region Tab Methods
        * *****************************************************************************************************
        *******************************************************************************************************/

        internal void SetRegionData(MapRegion mapRegion)
        {
            if (mapRegion == null) { return; }

            mapRegion.RegionBorderColor = RegionColorSelectLabel.BackColor;
            mapRegion.RegionBorderWidth = RegionBorderWidthTrack.Value;
            mapRegion.RegionInnerOpacity = RegionOpacityTrack.Value;
            mapRegion.RegionBorderSmoothing = RegionBorderSmoothingTrack.Value;

            if (RegionSolidBorderRadio.Checked)
            {
                mapRegion.RegionBorderType = PathTypeEnum.SolidLinePath;
            }

            SKPathEffect? regionBorderEffect = null;
            if (RegionDottedBorderRadio.Checked)
            {
                mapRegion.RegionBorderType = PathTypeEnum.DottedLinePath;
                float[] intervals = [0, mapRegion.RegionBorderWidth * 2];
                regionBorderEffect = SKPathEffect.CreateDash(intervals, 0);
            }

            if (RegionDashBorderRadio.Checked)
            {
                mapRegion.RegionBorderType = PathTypeEnum.DashedLinePath;
                float[] intervals = [mapRegion.RegionBorderWidth, mapRegion.RegionBorderWidth * 2];
                regionBorderEffect = SKPathEffect.CreateDash(intervals, 0);
            }

            if (RegionDashDotBorderRadio.Checked)
            {
                mapRegion.RegionBorderType = PathTypeEnum.DashDotLinePath;

            }

            if (RegionDashDotDotBorderRadio.Checked)
            {
                mapRegion.RegionBorderType = PathTypeEnum.DashDotDotLinePath;
            }

            if (RegionDoubleSolidBorderRadio.Checked)
            {
                mapRegion.RegionBorderType = PathTypeEnum.DoubleSolidBorderPath;
            }

            if (RegionSolidAndDashesBorderRadio.Checked)
            {
                mapRegion.RegionBorderType = PathTypeEnum.LineAndDashesPath;
            }

            if (RegionBorderedGradientRadio.Checked)
            {
                mapRegion.RegionBorderType = PathTypeEnum.BorderedGradientPath;
            }

            if (RegionBorderedLightSolidRadio.Checked)
            {
                mapRegion.RegionBorderType = PathTypeEnum.BorderedLightSolidPath;
            }

            regionBorderEffect = ConstructRegionBorderEffect(mapRegion);
            ConstructRegionPaintObjects(mapRegion, regionBorderEffect);
        }

        private SKPathEffect? ConstructRegionBorderEffect(MapRegion region)
        {
            SKPathEffect? pathEffect = null;
            switch (region.RegionBorderType)
            {
                case PathTypeEnum.DottedLinePath:
                    float[] intervals = [0, region.RegionBorderWidth * 2];
                    pathEffect = SKPathEffect.CreateDash(intervals, 0);
                    break;
                case PathTypeEnum.DashedLinePath:
                    intervals = [region.RegionBorderWidth, region.RegionBorderWidth * 2];
                    pathEffect = SKPathEffect.CreateDash(intervals, 0);
                    break;
                case PathTypeEnum.DashDotLinePath:
                    intervals = [region.RegionBorderWidth, region.RegionBorderWidth * 2, 0, region.RegionBorderWidth * 2];
                    pathEffect = SKPathEffect.CreateDash(intervals, 0);
                    break;
                case PathTypeEnum.DashDotDotLinePath:
                    intervals = [region.RegionBorderWidth, region.RegionBorderWidth * 2, 0, region.RegionBorderWidth * 2, 0, region.RegionBorderWidth * 2];
                    pathEffect = SKPathEffect.CreateDash(intervals, 0);
                    break;
                case PathTypeEnum.LineAndDashesPath:
                    float ldWidth = Math.Max(1, region.RegionBorderWidth / 2.0F);

                    string svgPath = "M 0 0"
                        + " h" + (region.RegionBorderWidth).ToString()
                        + " v" + Math.Max(1, ldWidth / 2.0F).ToString()
                        + " h" + (-region.RegionBorderWidth).ToString()
                        + " M0" + "," + (region.RegionBorderWidth - 1.0F).ToString()
                        + " h" + (ldWidth).ToString()
                        + " v2"
                        + " h" + (-ldWidth).ToString();

                    pathEffect = SKPathEffect.Create1DPath(SKPath.ParseSvgPathData(svgPath),
                        region.RegionBorderWidth, 0, SKPath1DPathEffectStyle.Morph);
                    break;
            }

            return pathEffect;
        }

        private void ConstructRegionPaintObjects(MapRegion region, SKPathEffect? regionBorderEffect)
        {
            region.RegionBorderPaint = new SKPaint()
            {
                StrokeWidth = region.RegionBorderWidth,
                Color = region.RegionBorderColor.ToSKColor(),
                Style = SKPaintStyle.Stroke,
                StrokeCap = SKStrokeCap.Round,
                StrokeJoin = SKStrokeJoin.Round,
                IsAntialias = true,
                PathEffect = SKPathEffect.CreateCorner(region.RegionBorderSmoothing)
            };

            Color innerColor = Color.FromArgb(region.RegionInnerOpacity, region.RegionBorderColor.R, region.RegionBorderColor.G, region.RegionBorderColor.B);

            region.RegionInnerPaint = new SKPaint()
            {
                Color = Extensions.ToSKColor(innerColor),
                Style = SKPaintStyle.Fill,
                PathEffect = SKPathEffect.CreateCorner(region.RegionBorderSmoothing)
            };

            if (regionBorderEffect != null)
            {
                region.RegionBorderPaint.PathEffect = SKPathEffect.CreateCompose(regionBorderEffect, SKPathEffect.CreateCorner(region.RegionBorderSmoothing));
            }
        }

        private void MoveSelectedRegionInRenderOrder(ComponentMoveDirectionEnum direction)
        {
            if (UIMapRegion != null)
            {
                // find the selected region in the Region Layer MapComponents
                MapLayer regionLayer = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.REGIONLAYER);

                List<MapComponent> regionComponents = regionLayer.MapLayerComponents;
                MapRegion? selectedRegion = null;

                int selectedRegionIndex = 0;

                for (int i = 0; i < regionComponents.Count; i++)
                {
                    MapComponent regionComponent = regionComponents[i];
                    if (regionComponent is MapRegion region && region.RegionGuid.ToString() == UIMapRegion.RegionGuid.ToString())
                    {
                        selectedRegionIndex = i;
                        selectedRegion = region;
                        break;
                    }
                }

                if (direction == ComponentMoveDirectionEnum.Up)
                {
                    // moving a region up in render order means increasing its index
                    if (selectedRegion != null && selectedRegionIndex < regionComponents.Count - 1)
                    {
                        regionComponents[selectedRegionIndex] = regionComponents[selectedRegionIndex + 1];
                        regionComponents[selectedRegionIndex + 1] = selectedRegion;
                    }
                }
                else if (direction == ComponentMoveDirectionEnum.Down)
                {
                    // moving a symbol down in render order means decreasing its index
                    // the map component at index 0 is the layer bitmap, so the selectedSymbolIndex must be great than 1 to move it down
                    if (selectedRegion != null && selectedRegionIndex > 1)
                    {
                        regionComponents[selectedRegionIndex] = regionComponents[selectedRegionIndex - 1];
                        regionComponents[selectedRegionIndex - 1] = selectedRegion;
                    }
                }
            }
        }

        #endregion

        #region Region Tab Event Handlers

        /*******************************************************************************************************
        * Region Tab Event Handlers 
        *******************************************************************************************************/

        private void PaintRegionButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.RegionPaint, sender);
        }

        private void SelectRegionButton_Click(object sender, EventArgs e)
        {
            SetDrawingMode(DrawingModeEnum.RegionSelect, sender);

            if (CURRENT_DRAWING_MODE != DrawingModeEnum.RegionSelect)
            {
                // unselect all regions
                foreach (MapRegion r in MapRegionMethods.MAP_REGION_LIST)
                {
                    r.IsSelected = false;
                }

                if (UIMapRegion != null)
                {
                    for (int i = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.REGIONLAYER).MapLayerComponents.Count - 1; i > 0; i--)
                    {
                        if (MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.REGIONLAYER).MapLayerComponents[i] is MapRegion r)
                        {
                            if (r.RegionGuid.ToString() == UIMapRegion.RegionGuid.ToString())
                            {
                                r.IsSelected = false;
                                break;
                            }
                        }
                    }

                    UIMapRegion.IsSelected = false;
                }

                MapBuilder.GetLayerCanvas(CURRENT_MAP, MapBuilder.REGIONLAYER).Clear();
                MapBuilder.GetLayerCanvas(CURRENT_MAP, MapBuilder.REGIONOVERLAYLAYER).Clear();

                MapImageBox.Refresh();
                UIMapRegion = null;
            }
        }

        private void RegionColorSelectLabel_Click(object sender, EventArgs e)
        {
            Color regionColor = MapPaintMethods.SelectColorFromDialog(this, RegionColorSelectLabel.BackColor);

            if (regionColor.ToArgb() != Color.Empty.ToArgb())
            {
                RegionColorSelectLabel.BackColor = regionColor;
                RegionColorSelectLabel.Refresh();

                if (UIMapRegion != null)
                {
                    SetRegionData(UIMapRegion);
                }
            }
        }

        private void RegionBorderWidthTrack_Scroll(object sender, EventArgs e)
        {
            RegionBorderWidthLabel.Text = RegionBorderWidthTrack.Value.ToString();
            RegionBorderWidthLabel.Refresh();

            if (UIMapRegion != null)
            {
                SetRegionData(UIMapRegion);
            }
        }

        private void RegionOpacityTrack_Scroll(object sender, EventArgs e)
        {
            RegionInnerOpacityLabel.Text = RegionOpacityTrack.Value.ToString();
            RegionInnerOpacityLabel.Refresh();

            if (UIMapRegion != null)
            {
                SetRegionData(UIMapRegion);
            }
        }

        private void RegionBorderSmoothingTrack_Scroll(object sender, EventArgs e)
        {
            RegionBorderSmoothingLabel.Text = RegionBorderSmoothingTrack.Value.ToString();
            RegionBorderSmoothingLabel.Refresh();

            if (UIMapRegion != null)
            {
                SetRegionData(UIMapRegion);
            }
        }

        private void SolidRegionBorderPicture_Click(object sender, EventArgs e)
        {
            RegionSolidBorderRadio.Checked = true;

            if (UIMapRegion != null)
            {
                SetRegionData(UIMapRegion);
            }
        }

        private void DottedRegionBorderPicture_Click(object sender, EventArgs e)
        {
            RegionDottedBorderRadio.Checked = true;

            if (UIMapRegion != null)
            {
                SetRegionData(UIMapRegion);
            }
        }

        private void DashedRegionBorderPicture_Click(object sender, EventArgs e)
        {
            RegionDashBorderRadio.Checked = true;

            if (UIMapRegion != null)
            {
                SetRegionData(UIMapRegion);
            }
        }

        private void DashDotRegionBorderPicture_Click(object sender, EventArgs e)
        {
            RegionDashDotBorderRadio.Checked = true;

            if (UIMapRegion != null)
            {
                SetRegionData(UIMapRegion);
            }
        }

        private void DashDotDotRegionBorderPicture_Click(object sender, EventArgs e)
        {
            RegionDashDotDotBorderRadio.Checked = true;

            if (UIMapRegion != null)
            {
                SetRegionData(UIMapRegion);
            }
        }

        private void DoubleSolidRegionBorderPicture_Click(object sender, EventArgs e)
        {
            RegionDoubleSolidBorderRadio.Checked = true;

            if (UIMapRegion != null)
            {
                SetRegionData(UIMapRegion);
            }
        }

        private void SolidAndDashRegionBorderPicture_Click(object sender, EventArgs e)
        {
            RegionSolidAndDashesBorderRadio.Checked = true;

            if (UIMapRegion != null)
            {
                SetRegionData(UIMapRegion);
            }
        }

        private void GradientRegionBorderPicture_Click(object sender, EventArgs e)
        {
            RegionBorderedGradientRadio.Checked = true;

            if (UIMapRegion != null)
            {
                SetRegionData(UIMapRegion);
            }
        }

        private void LightSolidRegionBorderPicture_Click(object sender, EventArgs e)
        {
            RegionBorderedLightSolidRadio.Checked = true;

            if (UIMapRegion != null)
            {
                SetRegionData(UIMapRegion);
            }
        }

        private void RegionSolidBorderRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (UIMapRegion != null)
            {
                SetRegionData(UIMapRegion);
            }
        }

        private void RegionDottedBorderRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (UIMapRegion != null)
            {
                SetRegionData(UIMapRegion);
            }
        }

        private void RegionDashBorderRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (UIMapRegion != null)
            {
                SetRegionData(UIMapRegion);
            }
        }

        private void RegionDashDotBorderRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (UIMapRegion != null)
            {
                SetRegionData(UIMapRegion);
            }
        }

        private void RegionDashDotDotBorderRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (UIMapRegion != null)
            {
                SetRegionData(UIMapRegion);
            }
        }

        private void RegionDoubleSolidBorderRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (UIMapRegion != null)
            {
                SetRegionData(UIMapRegion);
            }
        }

        private void RegionSolidAndDashesBorderRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (UIMapRegion != null)
            {
                SetRegionData(UIMapRegion);
            }
        }

        private void RegionBorderedGradientRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (UIMapRegion != null)
            {
                SetRegionData(UIMapRegion);
            }
        }

        private void RegionBorderedLightSolidRadio_CheckedChanged(object sender, EventArgs e)
        {
            if (UIMapRegion != null)
            {
                SetRegionData(UIMapRegion);
            }
        }

        private void ShowRegionLayerCheck_CheckedChanged(object sender, EventArgs e)
        {
            MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.REGIONLAYER).ShowLayer = ShowRegionLayerCheck.Checked;

            MapImageBox.Refresh();
        }

        #endregion

        #endregion

        #region Drawing Tab

        /******************************************************************************************************
        * *****************************************************************************************************
        * Drawing Tab Methods
        * *****************************************************************************************************
        *******************************************************************************************************/

        /*******************************************************************************************************
        * Drawing Tab Event Handlers 
        *******************************************************************************************************/

        private void ShowDrawingLayerCheck_CheckedChanged(object sender, EventArgs e)
        {
            MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.DRAWINGLAYER).ShowLayer = ShowDrawingLayerCheck.Checked;

            MapImageBox.Refresh();
        }

        #endregion

        #region Image Box Event Handlers

        /*******************************************************************************************************
         * *****************************************************************************************************
         * IMAGE BOX EVENT HANDLERS
         * *****************************************************************************************************
         * *****************************************************************************************************/

        #region IMAGE BOX MOUSE DOWN

        // MOUSE DOWN
        private void MapImageBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                LeftButtonMouseDownHandler(sender, e);
            }

            if (e.Button == MouseButtons.Right)
            {
                RightButtonMouseDownHandler(sender, e);
            }
        }

        #endregion

        #region IMAGE BOX MOUSE UP

        // MOUSE UP
        private void MapImageBox_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                LeftButtonMouseUpHandler(sender, e);
            }

            if (e.Button == MouseButtons.Right)
            {
                RightButtonMouseUpHandler(sender, e);
            }
        }

        #endregion

        #region IMAGE BOX MOUSE MOVE

        // MOUSE MOVE
        private void MapImageBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (CURRENT_DRAWING_MODE == DrawingModeEnum.ColorSelect)
            {
                Cursor = MapPaintMethods.EYEDROPPER_CURSOR;
            }

            int brushRadius = SetBrushRadius(CURRENT_DRAWING_MODE);

            IMAGEBOX_CLICK_POINT.X = e.X - brushRadius;
            IMAGEBOX_CLICK_POINT.Y = e.Y - brushRadius;

            LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));

            if (e.Button == MouseButtons.Left)
            {
                LeftButtonMouseMoveHandler(e, brushRadius);
            }
            else if (e.Button == MouseButtons.Right)
            {
                RightButtonMouseMoveHandler(sender, e);
            }

            if (e.Button == MouseButtons.None)
            {
                NoButtonMouseMoveHandler(sender, e, brushRadius);
            }
        }

        #endregion

        #region IMAGE BOX MOUSE DOUBLE-CLICK

        private void MapImageBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (ModifierKeys == Keys.Shift)
            {
                MapImageBox.ZoomToFit();
                MapImageBox.ScrollTo(0, 0);
            }
        }

        #endregion

        #region IMAGE BOX PAINT

        // PAINT
        private void MapImageBox_Paint(object sender, PaintEventArgs e)
        {
            RenderDrawingPanel();

            MapPaintMethods.PaintMap(ref CURRENT_MAP, CURRENT_DRAWING_MODE);

            int selectedBrushSize = 20;
            bool showCircleAroundCursor = false;

            switch (CURRENT_DRAWING_MODE)
            {
                // if ocean or land paint or erase mode, draw the circle around the cursor
                case DrawingModeEnum.OceanPaint:
                    selectedBrushSize = OceanPaintMethods.OCEAN_BRUSH_SIZE;
                    showCircleAroundCursor = true;
                    break;
                case DrawingModeEnum.OceanErase:
                    selectedBrushSize = OceanPaintMethods.OCEAN_ERASER_SIZE;
                    showCircleAroundCursor = true;
                    break;
                case DrawingModeEnum.LandPaint:
                    selectedBrushSize = LandformType2Methods.LAND_BRUSH_SIZE;
                    showCircleAroundCursor = true;
                    break;
                case DrawingModeEnum.LandErase:
                    selectedBrushSize = LandformType2Methods.LAND_ERASER_SIZE;
                    showCircleAroundCursor = true;
                    break;
                case DrawingModeEnum.LandColor:
                    selectedBrushSize = LandformType2Methods.LAND_COLOR_BRUSH_SIZE;
                    showCircleAroundCursor = true;
                    break;
                case DrawingModeEnum.LandColorErase:
                    selectedBrushSize = LandformType2Methods.LAND_COLOR_ERASER_SIZE;
                    showCircleAroundCursor = true;
                    break;
                case DrawingModeEnum.WaterPaint:
                    selectedBrushSize = WaterFeatureMethods.WATER_BRUSH_SIZE;
                    showCircleAroundCursor = true;
                    break;
                case DrawingModeEnum.WaterErase:
                    selectedBrushSize = WaterFeatureMethods.WATER_ERASER_SIZE;
                    showCircleAroundCursor = true;
                    break;
                case DrawingModeEnum.LakePaint:
                    selectedBrushSize = WaterFeatureMethods.WATER_BRUSH_SIZE;
                    showCircleAroundCursor = true;
                    break;
                case DrawingModeEnum.WaterColor:
                    selectedBrushSize = WaterFeatureMethods.WATER_COLOR_BRUSH_SIZE;
                    showCircleAroundCursor = true;
                    break;
                case DrawingModeEnum.WaterColorErase:
                    selectedBrushSize = WaterFeatureMethods.WATER_COLOR_ERASER_SIZE;
                    showCircleAroundCursor = true;
                    break;
                case DrawingModeEnum.PathPaint:
                    DrawMapPathLines();
                    break;
                case DrawingModeEnum.SymbolErase:
                    selectedBrushSize = AreaBrushSizeTrack.Value;
                    showCircleAroundCursor = true;
                    break;
                case DrawingModeEnum.SymbolPlace:
                    if (AreaBrushCheck.Checked)
                    {
                        selectedBrushSize = AreaBrushSizeTrack.Value;
                        showCircleAroundCursor = true;
                    }
                    else
                    {
                        // TODO: change to use GDI for drawing symbol as it is being dragged

                        // draw the selected symbol to the cursor overlay bitmap to move it around the map
                        SKBitmap? cursorOverlayBitmap = MapPaintMethods.GetCursorOverlayBitmap();

                        if (cursorOverlayBitmap != null)
                        {
                            using SKCanvas canvas = new(cursorOverlayBitmap);
                            canvas.Clear();

                            MapSymbol? selectedSymbol = SymbolMethods.GetSelectedMapSymbol();
                            if (selectedSymbol != null)
                            {
                                SKBitmap? symbolBitmap = selectedSymbol.GetColorMappedBitmap();
                                if (symbolBitmap != null)
                                {
                                    float symbolScale = (float)((SymbolScaleTrack.Value / 100.0F) * MapImageBox.ZoomFactor);
                                    float symbolRotation = SymbolRotationTrack.Value;
                                    SKBitmap scaledSymbolBitmap = MapDrawingMethods.ScaleBitmap(symbolBitmap, symbolScale);

                                    SKBitmap rotatedAndScaledBitmap = MapDrawingMethods.RotateBitmap(scaledSymbolBitmap, symbolRotation, MirrorSymbolCheck.Checked);

                                    if (rotatedAndScaledBitmap != null)
                                    {
                                        SKPoint cursorPoint = new(IMAGEBOX_CLICK_POINT.X - (rotatedAndScaledBitmap.Width / 2), IMAGEBOX_CLICK_POINT.Y - (rotatedAndScaledBitmap.Height / 2));

                                        canvas.DrawBitmap(rotatedAndScaledBitmap, cursorPoint, null);
                                    }

                                    // use the selected symbol as the cursor
                                    e.Graphics.DrawImage(Extensions.ToBitmap(cursorOverlayBitmap), 0, 0);
                                }
                            }
                        }
                    }
                    break;
                case DrawingModeEnum.SymbolColor:
                    selectedBrushSize = AreaBrushSizeTrack.Value;
                    showCircleAroundCursor = true;
                    break;
            }

            if (showCircleAroundCursor)
            {
                using Pen p = new(Color.Black)
                {
                    DashStyle = System.Drawing.Drawing2D.DashStyle.Dot,
                };

                Point point = MapImageBox.PointToClient(Cursor.Position);

                e.Graphics.DrawEllipse(p,
                    (float)(point.X - (selectedBrushSize / 2 * MapImageBox.ZoomFactor)),
                    (float)(point.Y - (selectedBrushSize / 2 * MapImageBox.ZoomFactor)),
                    (float)Math.Ceiling(selectedBrushSize * MapImageBox.ZoomFactor),
                    (float)Math.Ceiling(selectedBrushSize * MapImageBox.ZoomFactor));
            }
        }

        #endregion

        #region IMAGE BOX ZOOM CHANGE

        // ZOOM CHANGE
        private void MapImageBox_ZoomChanged(object sender, EventArgs e)
        {
            UpdateViewportStatus();
        }

        #endregion

        #region IMAGE BOX MOUSE ENTER

        // MOUSE ENTER
        private void MapImageBox_MouseEnter(object sender, EventArgs e)
        {
            if (CURRENT_DRAWING_MODE == DrawingModeEnum.ColorSelect)
            {
                Cursor = MapPaintMethods.EYEDROPPER_CURSOR;
            }
            else
            {
                Cursor = Cursors.Cross;
            }
        }

        #endregion

        #region IMAGE BOX MOUSE LEAVE
        // MOUSE LEAVE
        private void MapImageBox_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
        }

        #endregion

        #region IMAGE BOX MOUSE WHEEL

        // MOUSE WHEEL
        private void MapImageBox_MouseWheel(object sender, MouseEventArgs e)
        {
            int cursorDelta = 5;

            if (ModifierKeys == Keys.Control && CURRENT_DRAWING_MODE == DrawingModeEnum.LandErase)
            {
                int sizeDelta = e.Delta < 0 ? -cursorDelta : cursorDelta;
                int newValue = LandEraserSizeScroll.Value + sizeDelta;
                newValue = Math.Max(LandEraserSizeScroll.Minimum, Math.Min(newValue, LandEraserSizeScroll.Maximum));

                // landform eraser
                LandEraserSizeScroll.Value = newValue;
                LandformType2Methods.LAND_ERASER_SIZE = newValue;
                LandEraserSizeLabel.Text = newValue.ToString();
                LandEraserSizeLabel.Refresh();
                MapImageBox.Refresh();
            }
            else if (ModifierKeys == Keys.Control && CURRENT_DRAWING_MODE == DrawingModeEnum.LandPaint)
            {
                int sizeDelta = e.Delta < 0 ? -cursorDelta : cursorDelta;
                int newValue = LandBrushSizeScroll.Value + sizeDelta;
                newValue = Math.Max(LandBrushSizeScroll.Minimum, Math.Min(newValue, LandBrushSizeScroll.Maximum));

                LandBrushSizeScroll.Value = newValue;
                LandformType2Methods.LAND_BRUSH_SIZE = newValue;
                LandBrushSizeLabel.Text = newValue.ToString();
                LandBrushSizeLabel.Refresh();
                MapImageBox.Refresh();
            }
            else if (ModifierKeys == Keys.Control && CURRENT_DRAWING_MODE == DrawingModeEnum.LandColor)
            {
                int sizeDelta = e.Delta < 0 ? -cursorDelta : cursorDelta;
                int newValue = LandColorBrushSizeTrack.Value + sizeDelta;
                newValue = Math.Max(LandColorBrushSizeTrack.Minimum, Math.Min(newValue, LandColorBrushSizeTrack.Maximum));

                LandColorBrushSizeTrack.Value = newValue;
                LandformType2Methods.LAND_COLOR_BRUSH_SIZE = newValue;
                LandColorBrushSizeLabel.Text = newValue.ToString();
                LandColorBrushSizeLabel.Refresh();
                MapImageBox.Refresh();
            }
            else if (ModifierKeys == Keys.Control && CURRENT_DRAWING_MODE == DrawingModeEnum.LandColorErase)
            {
                int sizeDelta = e.Delta < 0 ? -cursorDelta : cursorDelta;
                int newValue = LandColorEraserSizeTrack.Value + sizeDelta;
                newValue = Math.Max(LandColorEraserSizeTrack.Minimum, Math.Min(newValue, LandColorEraserSizeTrack.Maximum));

                // land color eraser
                LandColorEraserSizeTrack.Value = newValue;
                LandformType2Methods.LAND_COLOR_ERASER_SIZE = newValue;
                LandColorEraserSizeLabel.Text = newValue.ToString();
                LandColorEraserSizeLabel.Refresh();
                MapImageBox.Refresh();
            }
            else if (ModifierKeys == Keys.Control && CURRENT_DRAWING_MODE == DrawingModeEnum.OceanErase)
            {
                int sizeDelta = e.Delta < 0 ? -cursorDelta : cursorDelta;
                int newValue = OceanBrushSizeScroll.Value + sizeDelta;
                newValue = Math.Max(OceanBrushSizeScroll.Minimum, Math.Min(newValue, OceanBrushSizeScroll.Maximum));

                OceanEraserSizeScroll.Value = newValue;
                OceanPaintMethods.OCEAN_ERASER_SIZE = newValue;
                OceanEraserSizeLabel.Text = newValue.ToString();
                OceanEraserSizeLabel.Refresh();
                MapImageBox.Refresh();
            }
            else if (ModifierKeys == Keys.Control && CURRENT_DRAWING_MODE == DrawingModeEnum.OceanPaint)
            {
                int sizeDelta = e.Delta < 0 ? -cursorDelta : cursorDelta;
                int newValue = OceanBrushSizeScroll.Value + sizeDelta;
                newValue = Math.Max(OceanBrushSizeScroll.Minimum, Math.Min(newValue, OceanBrushSizeScroll.Maximum));

                OceanBrushSizeScroll.Value = newValue;
                OceanPaintMethods.OCEAN_BRUSH_SIZE = newValue;
                OceanBrushSizeLabel.Text = newValue.ToString();
                OceanBrushSizeLabel.Refresh();
                MapImageBox.Refresh();
            }
            else if (ModifierKeys == Keys.Control && CURRENT_DRAWING_MODE == DrawingModeEnum.WaterPaint)
            {
                int sizeDelta = e.Delta < 0 ? -cursorDelta : cursorDelta;
                int newValue = WaterBrushSizeTrack.Value + sizeDelta;
                newValue = Math.Max(WaterBrushSizeTrack.Minimum, Math.Min(newValue, WaterBrushSizeTrack.Maximum));

                WaterBrushSizeTrack.Value = newValue;
                WaterFeatureMethods.WATER_BRUSH_SIZE = newValue;
                WaterBrushSizeLabel.Text = newValue.ToString();
                WaterBrushSizeLabel.Refresh();
                MapImageBox.Refresh();
            }
            else if (ModifierKeys == Keys.Control && CURRENT_DRAWING_MODE == DrawingModeEnum.WaterErase)
            {
                int sizeDelta = e.Delta < 0 ? -cursorDelta : cursorDelta;
                int newValue = WaterEraserSizeTrack.Value + sizeDelta;
                newValue = Math.Max(WaterEraserSizeTrack.Minimum, Math.Min(newValue, WaterEraserSizeTrack.Maximum));

                WaterEraserSizeTrack.Value = newValue;
                WaterFeatureMethods.WATER_ERASER_SIZE = newValue;
                WaterEraserSizeLabel.Text = newValue.ToString();
                WaterEraserSizeLabel.Refresh();
                MapImageBox.Refresh();
            }
            else if (ModifierKeys == Keys.Control && CURRENT_DRAWING_MODE == DrawingModeEnum.LakePaint)
            {
                int sizeDelta = e.Delta < 0 ? -cursorDelta : cursorDelta;
                int newValue = WaterBrushSizeTrack.Value + sizeDelta;
                newValue = Math.Max(WaterBrushSizeTrack.Minimum, Math.Min(newValue, WaterBrushSizeTrack.Maximum));

                WaterBrushSizeTrack.Value = newValue;
                WaterFeatureMethods.WATER_BRUSH_SIZE = newValue;
                WaterBrushSizeLabel.Text = newValue.ToString();
                WaterBrushSizeLabel.Refresh();
                MapImageBox.Refresh();
            }
            else if (ModifierKeys == Keys.Control && CURRENT_DRAWING_MODE == DrawingModeEnum.WaterColor)
            {
                int sizeDelta = e.Delta < 0 ? -cursorDelta : cursorDelta;
                int newValue = WaterColorBrushSizeTrack.Value + sizeDelta;
                newValue = Math.Max(WaterColorBrushSizeTrack.Minimum, Math.Min(newValue, WaterColorBrushSizeTrack.Maximum));

                WaterColorBrushSizeTrack.Value = newValue;
                WaterFeatureMethods.WATER_COLOR_BRUSH_SIZE = newValue;
                WaterColorBrushSizeLabel.Text = newValue.ToString();
                WaterColorBrushSizeLabel.Refresh();
                MapImageBox.Refresh();
            }
            else if (ModifierKeys == Keys.Control && CURRENT_DRAWING_MODE == DrawingModeEnum.WaterColorErase)
            {
                int sizeDelta = e.Delta < 0 ? -cursorDelta : cursorDelta;
                int newValue = WaterEraserSizeTrack.Value + sizeDelta;
                newValue = Math.Max(WaterEraserSizeTrack.Minimum, Math.Min(newValue, WaterEraserSizeTrack.Maximum));

                // water color eraser
                WaterEraserSizeTrack.Value = newValue;
                WaterFeatureMethods.WATER_COLOR_ERASER_SIZE = newValue;
                WaterColorEraserSizeLabel.Text = newValue.ToString();
                WaterColorEraserSizeLabel.Refresh();
                MapImageBox.Refresh();
            }
            else if (CURRENT_DRAWING_MODE == DrawingModeEnum.SymbolPlace)
            {
                int sizeDelta = e.Delta < 0 ? -cursorDelta : cursorDelta;
                int newValue = (int)Math.Max(SymbolScaleUpDown.Minimum, SymbolScaleUpDown.Value + sizeDelta);
                newValue = (int)Math.Min(SymbolScaleUpDown.Maximum, newValue);
                SymbolScaleUpDown.Value = newValue;
                MapImageBox.Refresh();
            }
            else if (CURRENT_DRAWING_MODE == DrawingModeEnum.LabelSelect)
            {
                int sizeDelta = e.Delta < 0 ? -cursorDelta : cursorDelta;
                int newValue = (int)Math.Max(LabelRotationUpDown.Minimum, LabelRotationUpDown.Value + sizeDelta);
                newValue = (int)Math.Min(LabelRotationUpDown.Maximum, newValue);
                LabelRotationUpDown.Value = newValue;
                LabelRotationUpDown.Refresh();
                MapImageBox.Refresh();
            }
            else if (ModifierKeys == Keys.Shift)
            {
                // increase/decrease zoom by 10%, limiting to no less than 10% and no greater than 800%
                if (e.Delta < 0)
                {
                    // make sure zoom value is evenly divisible by 10, minimum 10
                    int newZoom = Math.Max(10, MapImageBox.Zoom - 10);
                    MapImageBox.Zoom = MapDrawingMethods.ClosestNumber(newZoom, 10);
                }
                else
                {
                    // make sure zoom value is evenly divisible by 10, maximum 800
                    int newZoom = Math.Min(800, MapImageBox.Zoom + 10);
                    MapImageBox.Zoom = MapDrawingMethods.ClosestNumber(newZoom, 10);
                }

                MapImageBox.Refresh();
            }
        }

        #endregion

        #region IMAGE BOX KEY DOWN

        // KEY DOWN
        private void MapImageBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (CURRENT_DRAWING_MODE == DrawingModeEnum.PathEdit)
                {
                    MapPath? selectedMapPath = MapPathMethods.GetSelectedPath();
                    MapPathPoint? mapPathPoint = MapPathMethods.GetSelectedMapPathPoint();

                    if (selectedMapPath != null && mapPathPoint != null)
                    {
                        Cmd_RemoveMapPathPoint cmd = new(ref selectedMapPath, ref mapPathPoint);
                        UndoManager.AddCommand(cmd);
                        cmd.DoOperation();
                    }

                    CURRENT_MAP.IsSaved = false;

                    return;
                }

                if (CURRENT_DRAWING_MODE == DrawingModeEnum.LabelSelect && UISelectedLabel != null)
                {
                    Cmd_DeleteLabel cmd = new(CURRENT_MAP, UISelectedLabel);
                    UndoManager.AddCommand(cmd);
                    cmd.DoOperation();

                    CURRENT_MAP.IsSaved = false;
                    UISelectedLabel = null;

                    return;
                }

                if (CURRENT_DRAWING_MODE == DrawingModeEnum.LabelSelect && UISelectedBox != null)
                {
                    Cmd_DeleteLabelBox cmd = new(CURRENT_MAP, UISelectedBox);
                    UndoManager.AddCommand(cmd);
                    cmd.DoOperation();

                    CURRENT_MAP.IsSaved = false;
                    UISelectedBox = null;

                    return;
                }

                if (CURRENT_DRAWING_MODE == DrawingModeEnum.RegionSelect && UIMapRegion != null)
                {
                    bool pointSelected = false;

                    foreach (MapRegionPoint mrp in UIMapRegion.MapRegionPoints)
                    {
                        if (mrp.IsSelected)
                        {
                            pointSelected = true;
                            Cmd_DeleteMapRegionPoint cmd = new(CURRENT_MAP, UIMapRegion, mrp);
                            UndoManager.AddCommand(cmd);
                            cmd.DoOperation();

                            break;
                        }
                    }

                    if (!pointSelected)
                    {
                        Cmd_DeleteMapRegion cmd = new(CURRENT_MAP, UIMapRegion);
                        UndoManager.AddCommand(cmd);
                        cmd.DoOperation();

                        UIMapRegion = null;
                    }

                    CURRENT_MAP.IsSaved = false;

                    MapImageBox.Refresh();

                    return;
                }

                List<MapLandformType2> landformList = LandformType2Methods.LANDFORM_LIST;
                for (int i = 0; i < landformList.Count; i++)
                {
                    if (landformList[i].IsSelected)
                    {
                        DialogResult result = MessageBox.Show("Are you sure you want to delete the selected landform?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                        if (result == DialogResult.Yes)
                        {
                            MapLandformType2 selectedLandform = landformList[i];

                            Cmd_DeleteLandform cmd = new(CURRENT_MAP, selectedLandform);
                            UndoManager.AddCommand(cmd);
                            cmd.DoOperation();

                            CURRENT_MAP.IsSaved = false;
                        }

                        break;
                    }
                }

                List<MapPaintedWaterFeature> waterFeatureList = WaterFeatureMethods.PAINTED_WATERFEATURE_LIST;
                for (int i = 0; i < waterFeatureList.Count; i++)
                {
                    if (waterFeatureList[i].IsSelected)
                    {
                        DialogResult result = MessageBox.Show("Are you sure you want to deleted the selected water feature?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                        if (result == DialogResult.Yes)
                        {
                            MapPaintedWaterFeature selectedWaterFeature = waterFeatureList[i];
                            Cmd_DeleteWaterFeature cmd = new(CURRENT_MAP, selectedWaterFeature);
                            UndoManager.AddCommand(cmd);
                            cmd.DoOperation();

                            CURRENT_MAP.IsSaved = false;
                        }

                        break;
                    }
                }

                List<MapRiver> riverList = WaterFeatureMethods.MAP_RIVER_LIST;
                for (int i = 0; i < riverList.Count; i++)
                {
                    if (riverList[i].IsSelected)
                    {
                        DialogResult result = MessageBox.Show("Are you sure you want to deleted the selected river?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                        if (result == DialogResult.Yes)
                        {
                            MapRiver selectedRiver = riverList[i];
                            Cmd_DeleteRiver cmd = new(CURRENT_MAP, selectedRiver);
                            UndoManager.AddCommand(cmd);
                            cmd.DoOperation();

                            CURRENT_MAP.IsSaved = false;
                        }

                        break;
                    }
                }

                List<MapPath> mapPathList = MapPathMethods.GetMapPathList();
                for (int i = 0; i < mapPathList.Count; i++)
                {
                    if (mapPathList[i].IsSelected)
                    {
                        DialogResult result = MessageBox.Show("Are you sure you want to deleted the selected path?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                        if (result == DialogResult.Yes)
                        {
                            MapPath selectedPath = mapPathList[i];

                            Cmd_DeleteMapPath cmd = new(CURRENT_MAP, selectedPath);
                            UndoManager.AddCommand(cmd);
                            cmd.DoOperation();

                            CURRENT_MAP.IsSaved = false;
                        }

                        break;
                    }
                }

                for (int i = 0; i < SymbolMethods.PlacedSymbolList.Count; i++)
                {
                    if (SymbolMethods.PlacedSymbolList[i].GetIsSelected())
                    {
                        MapSymbol selectedSymbol = SymbolMethods.PlacedSymbolList[i];
                        Cmd_RemoveSymbol cmd = new(CURRENT_MAP, selectedSymbol);
                        UndoManager.AddCommand(cmd);
                        cmd.DoOperation();

                        CURRENT_MAP.IsSaved = false;

                        break;
                    }
                }
            }

            if (e.KeyCode == Keys.PageUp)
            {
                if (CURRENT_DRAWING_MODE == DrawingModeEnum.SymbolSelect)
                {
                    MoveSelectedSymbolInRenderOrder(ComponentMoveDirectionEnum.Up);
                }
                else if (CURRENT_DRAWING_MODE == DrawingModeEnum.RegionSelect)
                {
                    MoveSelectedRegionInRenderOrder(ComponentMoveDirectionEnum.Up);
                }
            }

            if (e.KeyCode == Keys.PageDown)
            {
                if (CURRENT_DRAWING_MODE == DrawingModeEnum.SymbolSelect)
                {
                    MoveSelectedSymbolInRenderOrder(ComponentMoveDirectionEnum.Down);
                }
                else if (CURRENT_DRAWING_MODE == DrawingModeEnum.RegionSelect)
                {
                    MoveSelectedRegionInRenderOrder(ComponentMoveDirectionEnum.Down);
                }
            }

            if (e.KeyCode == Keys.Down)
            {
                if (CURRENT_DRAWING_MODE == DrawingModeEnum.SymbolSelect && UISelectedMapSymbol != null)
                {
                    UISelectedMapSymbol.Y++;
                    UISelectedMapSymbol.YLocation++;

                    CURRENT_MAP.IsSaved = false;
                }
            }

            if (e.KeyCode == Keys.Up)
            {
                if (CURRENT_DRAWING_MODE == DrawingModeEnum.SymbolSelect && UISelectedMapSymbol != null)
                {
                    UISelectedMapSymbol.Y--;
                    UISelectedMapSymbol.YLocation--;

                    CURRENT_MAP.IsSaved = false;
                }
            }

            if (e.KeyCode == Keys.Left)
            {
                if (CURRENT_DRAWING_MODE == DrawingModeEnum.SymbolSelect && UISelectedMapSymbol != null)
                {
                    UISelectedMapSymbol.X--;
                    UISelectedMapSymbol.XLocation--;

                    CURRENT_MAP.IsSaved = false;
                }
            }

            if (e.KeyCode == Keys.Right)
            {
                if (CURRENT_DRAWING_MODE == DrawingModeEnum.SymbolSelect && UISelectedMapSymbol != null)
                {
                    UISelectedMapSymbol.X++;
                    UISelectedMapSymbol.XLocation++;

                    CURRENT_MAP.IsSaved = false;
                }
            }

            if (e.KeyCode == Keys.Escape)
            {
                // cancel any in-progress operations
                UISelectedLandformArea = null;
                MapBuilder.ClearLayerCanvas(CURRENT_MAP, MapBuilder.WORKLAYER);
                MapBuilder.ClearLayerCanvas(CURRENT_MAP, MapBuilder.SELECTIONLAYER);
            }

            MapImageBox.Refresh();
        }

        #endregion

        #endregion

        #region IMAGE BOX MOUSE EVENT HANDLER METHODS (called from event handlers)

        /**************************************************************************************************************************
        * ************************************************************************************************************************
        * IMAGE BOX EVENT HANDLER METHODS
        * ************************************************************************************************************************
        * ************************************************************************************************************************/

        #region IMAGE BOX MOUSE DOWN HANDLER METHODS

        /*************************************************************************************************************************
        * MOUSE DOWN HANDLER METHODS
        *************************************************************************************************************************/

        #region LEFT MOUSE BUTTON DOWN

        private void LeftButtonMouseDownHandler(object sender, MouseEventArgs e)
        {
            int brushRadius = SetBrushRadius(CURRENT_DRAWING_MODE);

            IMAGEBOX_CLICK_POINT.X = e.X - brushRadius;
            IMAGEBOX_CLICK_POINT.Y = e.Y - brushRadius;

            LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));

            float zoomedRadius = (float)(brushRadius / MapImageBox.ZoomFactor);
            float X = (float)(LAYER_CLICK_POINT.X + zoomedRadius);
            float Y = (float)(LAYER_CLICK_POINT.Y + zoomedRadius);

            for (int i = 0; i < MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.OVERLAYLAYER).MapLayerComponents.Count; i++)
            {
                if (MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.OVERLAYLAYER).MapLayerComponents[i] is MapScale)
                {
                    UIMapScale = (MapScale?)MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.OVERLAYLAYER).MapLayerComponents[i];
                }
            }

            if (UIMapScale != null)
            {
                Rectangle scaleRect = new(UIMapScale.X, UIMapScale.Y, UIMapScale.Width, UIMapScale.Height);
                if (scaleRect.Contains(new Point((int)X, (int)Y)))
                {
                    UIMapScale.IsSelected = true;
                    SetDrawingMode(DrawingModeEnum.SelectMapScale, null);
                }
            }

            switch (CURRENT_DRAWING_MODE)
            {
                case DrawingModeEnum.OceanPaint:
                    Cursor = Cursors.Cross;

                    COLOR_OCEAN_COMMAND = new(CURRENT_MAP);
                    UndoManager.AddCommand(COLOR_OCEAN_COMMAND);

                    break;
                case DrawingModeEnum.OceanErase:
                    Cursor = Cursors.Cross;

                    ERASE_OCEAN_COLOR_COMMAND = new(CURRENT_MAP);
                    UndoManager.AddCommand(ERASE_OCEAN_COLOR_COMMAND);

                    break;
                case DrawingModeEnum.LandPaint:
                    Cursor = Cursors.Cross;
                    SetLandformData(LandformType2Methods.GetNewSelectedLandform(CURRENT_MAP));
                    LandformType2Methods.SELECTED_LANDFORM.LandformPath.Reset();

                    CURRENT_MAP.RenderOnlyLayers.Add(MapBuilder.LANDFORMLAYER);
                    //CURRENT_MAP.RenderOnlyLayers.Add(MapBuilder.LANDCOASTLINELAYER);

                    // add the landfor to the landform layer components
                    MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.LANDFORMLAYER).MapLayerComponents.Add(LandformType2Methods.SELECTED_LANDFORM);
                    break;
                case DrawingModeEnum.LandErase:
                    Cursor = Cursors.Cross;
                    PREVIOUS_LAYER_CLICK_POINT = LAYER_CLICK_POINT;

                    ERASE_LANDFORM_COMMAND = new(CURRENT_MAP);
                    UndoManager.AddCommand(ERASE_LANDFORM_COMMAND);

                    CURRENT_MAP.RenderOnlyLayers.Add(MapBuilder.LANDFORMLAYER);
                    CURRENT_MAP.RenderOnlyLayers.Add(MapBuilder.LANDCOASTLINELAYER);

                    break;
                case DrawingModeEnum.LandColor:
                    Cursor = Cursors.Cross;

                    COLOR_LANDFORM_COMMAND = new(CURRENT_MAP);
                    UndoManager.AddCommand(COLOR_LANDFORM_COMMAND);

                    break;
                case DrawingModeEnum.LandColorErase:
                    Cursor = Cursors.Cross;

                    ERASE_LANDFORM_COLOR_COMMAND = new(CURRENT_MAP);
                    UndoManager.AddCommand(ERASE_LANDFORM_COLOR_COMMAND);
                    break;
                case DrawingModeEnum.WaterPaint:
                    Cursor = Cursors.Cross;
                    SetWaterFeatureData(WaterFeatureMethods.GetNewWaterFeature(CURRENT_MAP));
                    WaterFeatureMethods.ResetAllWaterPaths();
                    break;
                case DrawingModeEnum.WaterErase:
                    Cursor = Cursors.Cross;
                    PREVIOUS_LAYER_CLICK_POINT = LAYER_CLICK_POINT;

                    ERASE_WATERFEATURE_COMMAND = new(CURRENT_MAP);
                    UndoManager.AddCommand(ERASE_WATERFEATURE_COMMAND);
                    break;
                case DrawingModeEnum.LakePaint:
                    Cursor = Cursors.Cross;
                    SetWaterFeatureData(WaterFeatureMethods.GetNewWaterFeature(CURRENT_MAP), WaterFeatureTypeEnum.Lake);
                    WaterFeatureMethods.ResetAllWaterPaths();
                    break;
                case DrawingModeEnum.WaterColor:
                    Cursor = Cursors.Cross;

                    COLOR_WATERFEATURE_COMMAND = new(CURRENT_MAP);
                    UndoManager.AddCommand(COLOR_WATERFEATURE_COMMAND);
                    break;
                case DrawingModeEnum.WaterColorErase:
                    Cursor = Cursors.Cross;

                    ERASE_WATERFEATURE_COLOR_COMMAND = new(CURRENT_MAP);
                    UndoManager.AddCommand(ERASE_WATERFEATURE_COLOR_COMMAND);
                    break;
                case DrawingModeEnum.RiverPaint:
                    Cursor = Cursors.Cross;

                    IMAGEBOX_CLICK_POINT.X = e.X;
                    IMAGEBOX_CLICK_POINT.Y = e.Y;

                    LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));
                    PREVIOUS_LAYER_CLICK_POINT = LAYER_CLICK_POINT;

                    if (WaterFeatureMethods.RIVER_POINT_LIST.Count == 0)
                    {
                        // initialize river
                        RIVER_CLICK_POINT = LAYER_CLICK_POINT;
                        PREVIOUS_RIVER_CLICK_POINT = LAYER_CLICK_POINT;

                        WaterFeatureMethods.NEW_RIVER = new MapRiver();
                        SetRiverData(WaterFeatureMethods.NEW_RIVER);
                    }
                    else
                    {
                        PREVIOUS_RIVER_CLICK_POINT = RIVER_CLICK_POINT;
                    }

                    break;
                case DrawingModeEnum.PathPaint:
                    Cursor = Cursors.Cross;

                    IMAGEBOX_CLICK_POINT.X = e.X;
                    IMAGEBOX_CLICK_POINT.Y = e.Y;

                    LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));
                    PREVIOUS_LAYER_CLICK_POINT = LAYER_CLICK_POINT;

                    if (MapPathMethods.GetPathPointList().Count == 0)
                    {
                        // initialize path
                        PATH_CLICK_POINT = LAYER_CLICK_POINT;
                        PREVIOUS_PATH_CLICK_POINT = LAYER_CLICK_POINT;

                        MapPathMethods.CreateNewMapPath();
                        SetPathData(MapPathMethods.GetNewPath());
                    }
                    else
                    {
                        PREVIOUS_PATH_CLICK_POINT = PATH_CLICK_POINT;
                    }
                    break;
                case DrawingModeEnum.PathSelect:
                    IMAGEBOX_CLICK_POINT.X = e.X;
                    IMAGEBOX_CLICK_POINT.Y = e.Y;

                    LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));
                    PREVIOUS_LAYER_CLICK_POINT = LAYER_CLICK_POINT;
                    break;
                case DrawingModeEnum.SymbolPlace:
                    LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(e.Location));

                    if (AreaBrushCheck.Checked)
                    {
                        PlaceSelectedSymbolInArea(new SKPoint(X, Y));
                    }
                    else
                    {
                        PlaceSelectedSymbolAtCursor(LAYER_CLICK_POINT);
                    }

                    break;
                case DrawingModeEnum.SymbolErase:
                    int eraserRadius = AreaBrushSizeTrack.Value / 2;

                    SKPoint eraserCursorPoint = new(LAYER_CLICK_POINT.X + eraserRadius, LAYER_CLICK_POINT.Y + eraserRadius);

                    SymbolMethods.RemovePlacedSymbolsFromArea(CURRENT_MAP, eraserCursorPoint, eraserRadius);
                    break;
                case DrawingModeEnum.DrawBezierLabelPath:
                    Cursor = Cursors.Cross;

                    IMAGEBOX_CLICK_POINT.X = e.X;
                    IMAGEBOX_CLICK_POINT.Y = e.Y;

                    LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));
                    PREVIOUS_LAYER_CLICK_POINT = LAYER_CLICK_POINT;

                    PATH_CLICK_POINT = LAYER_CLICK_POINT;
                    PREVIOUS_PATH_CLICK_POINT = LAYER_CLICK_POINT;

                    MapLabelMethods.LABEL_PATH_POINTS.Clear();

                    MapLabelMethods.LABEL_PATH.Dispose();
                    MapLabelMethods.LABEL_PATH = new();

                    MapBuilder.GetLayerCanvas(CURRENT_MAP, MapBuilder.WORKLAYER)?.Clear();

                    break;
                case DrawingModeEnum.DrawArcLabelPath:
                    Cursor = Cursors.Cross;

                    IMAGEBOX_CLICK_POINT.X = e.X;
                    IMAGEBOX_CLICK_POINT.Y = e.Y;

                    LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));
                    PREVIOUS_LAYER_CLICK_POINT = LAYER_CLICK_POINT;

                    PATH_CLICK_POINT = LAYER_CLICK_POINT;
                    PREVIOUS_PATH_CLICK_POINT = LAYER_CLICK_POINT;

                    MapLabelMethods.LABEL_PATH_POINTS.Clear();

                    MapLabelMethods.LABEL_PATH.Dispose();
                    MapLabelMethods.LABEL_PATH = new();

                    MapBuilder.GetLayerCanvas(CURRENT_MAP, MapBuilder.WORKLAYER)?.Clear();
                    break;
                case DrawingModeEnum.DrawBox:
                    // initialize new box
                    Cursor = Cursors.Cross;

                    IMAGEBOX_CLICK_POINT.X = e.X;
                    IMAGEBOX_CLICK_POINT.Y = e.Y;

                    LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));
                    PREVIOUS_LAYER_CLICK_POINT = LAYER_CLICK_POINT;

                    UISelectedBox = new();

                    break;
                case DrawingModeEnum.DrawMapMeasure:
                    LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));

                    if (UIMapMeasure == null)
                    {
                        // make sure there is only one measure object
                        MapLayer measureLayer = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.MEASURELAYER);

                        for (int i = measureLayer.MapLayerComponents.Count - 1; i >= 0; i--)
                        {
                            if (measureLayer.MapLayerComponents[i] is MapMeasure)
                            {
                                measureLayer.MapLayerComponents.RemoveAt(i);
                            }
                        }

                        UIMapMeasure = new(CURRENT_MAP)
                        {
                            UseMapUnits = UseScaleUnitsCheck.Checked,
                            MeasureArea = MeasureAreaCheck.Checked,
                            MeasureLineColor = MeasureColorLabel.BackColor
                        };

                        PREVIOUS_LAYER_CLICK_POINT = LAYER_CLICK_POINT;
                        MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.MEASURELAYER).MapLayerComponents.Add(UIMapMeasure);
                    }
                    else
                    {
                        if (!UIMapMeasure.MeasurePoints.Contains(LAYER_CLICK_POINT))
                        {
                            UIMapMeasure.MeasurePoints.Add(LAYER_CLICK_POINT);
                        }
                    }

                    break;
                case DrawingModeEnum.RegionPaint:
                    Cursor = Cursors.Cross;

                    IMAGEBOX_CLICK_POINT.X = e.X;
                    IMAGEBOX_CLICK_POINT.Y = e.Y;

                    LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));

                    // initialize region
                    if (UIMapRegion == null)
                    {
                        UIMapRegion = new(CURRENT_MAP);
                        SetRegionData(UIMapRegion);
                    }

                    if (ModifierKeys == Keys.Shift)
                    {
                        // find the closest point to the current point
                        // on the contour path of a coastline;
                        // if the nearest point on the coastline
                        // is within 20 pixels of the current point,
                        // then set the region point to be the point
                        // on the coastline
                        // then check the *previous* region point; if the previous
                        // region point is on the coastline of the same landform
                        // then add all of the points on the coastline contour
                        // to the region points

                        int coastlinePointIndex = -1;
                        SKPoint coastlinePoint = SKPoint.Empty;
                        MapLandformType2? landform1 = null;
                        MapLandformType2? landform2 = null;

                        float currentDistance = float.MaxValue;

                        foreach (MapLandformType2 lf in LandformType2Methods.LANDFORM_LIST)
                        {
                            for (int i = 0; i < lf.LandformContourPoints.Count; i++)
                            {
                                SKPoint p = lf.LandformContourPoints[i];
                                float distance = SKPoint.Distance(LAYER_CLICK_POINT, p);

                                if (distance < currentDistance && distance < 5)
                                {
                                    landform1 = lf;
                                    coastlinePointIndex = i;
                                    coastlinePoint = p;
                                    currentDistance = distance;
                                }
                            }

                            if (coastlinePointIndex >= 0) break;
                        }


                        int previousCoastlinePointIndex = -1;
                        currentDistance = float.MaxValue;

                        if (landform1 != null && coastlinePointIndex >= 0)
                        {
                            MapRegionPoint mrp = new MapRegionPoint(landform1.LandformContourPoints[coastlinePointIndex]);
                            UIMapRegion.MapRegionPoints.Add(mrp);

                            if (UIMapRegion.MapRegionPoints.Count > 1 && coastlinePointIndex > 1)
                            {
                                SKPoint previousPoint = UIMapRegion.MapRegionPoints[UIMapRegion.MapRegionPoints.Count - 2].RegionPoint;

                                foreach (MapLandformType2 lf in LandformType2Methods.LANDFORM_LIST)
                                {
                                    for (int i = 0; i < lf.LandformContourPoints.Count; i++)
                                    {
                                        SKPoint p = lf.LandformContourPoints[i];
                                        float distance = SKPoint.Distance(previousPoint, p);

                                        if (distance < currentDistance && !coastlinePoint.Equals(previousPoint))
                                        {
                                            landform2 = lf;
                                            previousCoastlinePointIndex = i;
                                            currentDistance = distance;
                                        }
                                    }

                                    if (previousCoastlinePointIndex >= 0) break;
                                }
                            }
                        }

                        if (landform1 != null && landform2 != null
                            && landform1.LandformGuid.ToString() == landform2.LandformGuid.ToString()
                            && coastlinePointIndex >= 0 && previousCoastlinePointIndex >= 0)
                        {
                            UIMapRegion.MapRegionPoints.Clear();

                            landform1.LandformContourPath.GetTightBounds(out SKRect boundingRect);
                            float xCenter = boundingRect.MidX;

                            if (LAYER_CLICK_POINT.Y < PREVIOUS_LAYER_CLICK_POINT.Y)
                            {
                                // drag mouse up to snap to west coast of landform
                                for (int i = previousCoastlinePointIndex; i < landform1.LandformContourPoints.Count - 1; i++)
                                {
                                    MapRegionPoint mrp = new MapRegionPoint(landform1.LandformContourPoints[i]);
                                    UIMapRegion.MapRegionPoints.Add(mrp);
                                }

                                for (int i = 0; i <= coastlinePointIndex; i++)
                                {
                                    MapRegionPoint mrp = new MapRegionPoint(landform1.LandformContourPoints[i]);
                                    UIMapRegion.MapRegionPoints.Add(mrp);
                                }
                            }
                            else
                            {
                                // drag mouse down to snap region to east coast of landform
                                if (coastlinePointIndex > previousCoastlinePointIndex)
                                {
                                    for (int i = previousCoastlinePointIndex; i <= coastlinePointIndex; i++)
                                    {
                                        MapRegionPoint mrp = new MapRegionPoint(landform1.LandformContourPoints[i]);
                                        UIMapRegion.MapRegionPoints.Add(mrp);
                                    }
                                }
                                else
                                {
                                    for (int i = coastlinePointIndex; i <= previousCoastlinePointIndex; i++)
                                    {
                                        MapRegionPoint mrp = new MapRegionPoint(landform1.LandformContourPoints[i]);
                                        UIMapRegion.MapRegionPoints.Add(mrp);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        MapRegionPoint mrp = new MapRegionPoint(LAYER_CLICK_POINT);
                        UIMapRegion.MapRegionPoints.Add(mrp);
                    }

                    PREVIOUS_LAYER_CLICK_POINT = LAYER_CLICK_POINT;
                    break;
                case DrawingModeEnum.RegionSelect:
                    {
                        if (UIMapRegion != null && NEW_REGION_POINT != null)
                        {
                            Cmd_AddMapRegionPoint cmd = new(CURRENT_MAP, UIMapRegion, NEW_REGION_POINT, NEXT_REGION_POINT_INDEX);
                            UndoManager.AddCommand(cmd);
                            cmd.DoOperation();

                            // reset
                            NEW_REGION_POINT = null;
                            NEXT_REGION_POINT_INDEX = -1;
                            PREVIOUS_REGION_POINT_INDEX = -1;

                            SKCanvas regionOverlayCanvas = MapBuilder.GetLayerCanvas(CURRENT_MAP, MapBuilder.REGIONOVERLAYLAYER);
                            regionOverlayCanvas.Clear();
                        }
                    }
                    break;
                case DrawingModeEnum.LandformAreaSelect:
                    Cursor = Cursors.Cross;

                    IMAGEBOX_CLICK_POINT.X = e.X;
                    IMAGEBOX_CLICK_POINT.Y = e.Y;

                    LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));
                    PREVIOUS_LAYER_CLICK_POINT = LAYER_CLICK_POINT;
                    break;
            }
        }

        #endregion

        #region RIGHT MOUSE BUTTON DOWN

        private void RightButtonMouseDownHandler(object sender, MouseEventArgs e)
        {
            switch (CURRENT_DRAWING_MODE)
            {
                case DrawingModeEnum.LabelSelect:

                    if (UISelectedLabel != null)
                    {
                        MapLabelMethods.CreatingLabel = true;

                        IMAGEBOX_CLICK_POINT.X = e.X;
                        IMAGEBOX_CLICK_POINT.Y = e.Y;

                        LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));

                        Font tbFont = new Font(UISelectedLabel.LabelFont.FontFamily, (float)(MapLabelMethods.SELECTED_FONT.Size * MapImageBox.ZoomFactor), GraphicsUnit.Pixel);

                        Size labelSize = TextRenderer.MeasureText(UISelectedLabel.LabelText, tbFont, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.Default);

                        LABEL_TEXT_BOX = new()
                        {
                            Parent = MapImageBox,
                            Name = Guid.NewGuid().ToString(),
                            Left = IMAGEBOX_CLICK_POINT.X - (labelSize.Width / 2),
                            Top = IMAGEBOX_CLICK_POINT.Y - (labelSize.Height / 2),
                            Width = labelSize.Width,
                            Height = labelSize.Height,
                            Margin = Padding.Empty,
                            AutoSize = false,
                            Font = tbFont,
                            Visible = true,
                            BackColor = Color.AliceBlue,
                            ForeColor = FontColorSelectLabel.BackColor,
                            BorderStyle = BorderStyle.None,
                            Multiline = false,
                            TextAlign = HorizontalAlignment.Center,
                            Text = UISelectedLabel.LabelText,
                        };

                        LABEL_TEXT_BOX.KeyPress += LabelTextBox_KeyPress;

                        MapImageBox.Controls.Add(LABEL_TEXT_BOX);

                        MapImageBox.Refresh();

                        LABEL_TEXT_BOX.BringToFront();
                        LABEL_TEXT_BOX.Select(LABEL_TEXT_BOX.Text.Length, 0);
                        LABEL_TEXT_BOX.Focus();
                        LABEL_TEXT_BOX.ScrollToCaret();

                        LABEL_TEXT_BOX.Tag = UISelectedLabel.LabelPath;

                        // delete the currently selected label

                        for (int i = MapLabelMethods.MAP_LABELS.Count - 1; i >= 0; i--)
                        {
                            if (MapLabelMethods.MAP_LABELS[i].LabelGuid.ToString() == UISelectedLabel.LabelGuid.ToString())
                            {
                                MapLabelMethods.MAP_LABELS.RemoveAt(i);
                            }
                        }

                        MapLayer labelLayer = MapBuilder.GetMapLayerByIndex(CURRENT_MAP, MapBuilder.LABELLAYER);

                        for (int i = labelLayer.MapLayerComponents.Count - 1; i >= 0; i--)
                        {
                            if (labelLayer.MapLayerComponents[i] is MapLabel l && l.LabelGuid.ToString() == UISelectedLabel.LabelGuid.ToString())
                            {
                                labelLayer.MapLayerComponents.RemoveAt(i);
                            }
                        }

                        MapImageBox.Refresh();

                    }

                    break;
                case DrawingModeEnum.DrawMapMeasure:
                    if (UIMapMeasure != null)
                    {
                        LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));
                        UIMapMeasure.MeasurePoints.Add(LAYER_CLICK_POINT);

                        float lineLength = SKPoint.Distance(PREVIOUS_LAYER_CLICK_POINT, LAYER_CLICK_POINT);
                        UIMapMeasure.TotalMeasureLength += lineLength;
                        UIMapMeasure.RenderValue = true;

                        // reset everything
                        MapBuilder.GetLayerCanvas(CURRENT_MAP, MapBuilder.WORKLAYER).Clear();
                        PREVIOUS_LAYER_CLICK_POINT = SKPoint.Empty;
                        LAYER_CLICK_POINT = SKPoint.Empty;

                        UIMapMeasure = null;
                        SetDrawingMode(DrawingModeEnum.None, null);
                    }

                    break;
                case DrawingModeEnum.RegionPaint:
                    if (UIMapRegion != null)
                    {
                        IMAGEBOX_CLICK_POINT.X = e.X;
                        IMAGEBOX_CLICK_POINT.Y = e.Y;

                        LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));

                        MapRegionPoint mrp = new(LAYER_CLICK_POINT);
                        UIMapRegion.MapRegionPoints.Add(mrp);

                        CURRENT_MAP.IsSaved = false;

                        SKCanvas workCanvas = MapBuilder.GetLayerCanvas(CURRENT_MAP, MapBuilder.WORKLAYER);
                        workCanvas.Clear();

                        MapBuilder.ClearLayerCanvas(CURRENT_MAP, MapBuilder.REGIONLAYER);

                        Cmd_AddMapRegion cmd = new(CURRENT_MAP, UIMapRegion);
                        UndoManager.AddCommand(cmd);
                        cmd.DoOperation();

                        UIMapRegion.IsSelected = false;

                        // reset everything
                        MapBuilder.GetLayerCanvas(CURRENT_MAP, MapBuilder.WORKLAYER).Clear();
                        PREVIOUS_LAYER_CLICK_POINT = SKPoint.Empty;
                        LAYER_CLICK_POINT = SKPoint.Empty;

                        UIMapRegion = null;
                        SetDrawingMode(DrawingModeEnum.None, null);
                    }

                    break;
            }
        }

        #endregion

        #endregion

        #region IMAGE BOX MOUSE UP HANDLER METHODS

        /*************************************************************************************************************************
        * MOUSE UP HANDLER METHODS
        *************************************************************************************************************************/

        #region LEFT MOUSE BUTTON UP

        private void LeftButtonMouseUpHandler(object sender, MouseEventArgs e)
        {
            IMAGEBOX_CLICK_POINT = Point.Empty;
            LAYER_CLICK_POINT = SKPoint.Empty;

            switch (CURRENT_DRAWING_MODE)
            {
                case DrawingModeEnum.OceanPaint:
                    COLOR_OCEAN_COMMAND.DoOperation();
                    break;
                case DrawingModeEnum.OceanErase:
                    ERASE_OCEAN_COLOR_COMMAND.DoOperation();
                    break;
                case DrawingModeEnum.LandPaint:
                    {
                        CURRENT_MAP.RenderOnlyLayers.Clear();

                        Cmd_AddLandform cmd = new(CURRENT_MAP);
                        UndoManager.AddCommand(cmd);
                        cmd.DoOperation();

                        MapImageBox.Refresh();

                        LandformSelectButton.Checked = false;

                        CURRENT_MAP.IsSaved = false;

                        LandformType2Methods.SELECTED_LANDFORM.DrawLandform = false;
                    }
                    break;
                case DrawingModeEnum.LandErase:
                    CURRENT_MAP.RenderOnlyLayers.Clear();

                    LandformSelectButton.Checked = false;
                    CURRENT_MAP.IsSaved = false;
                    break;
                case DrawingModeEnum.LandColor:
                    COLOR_LANDFORM_COMMAND.DoOperation();

                    break;
                case DrawingModeEnum.LandColorErase:
                    ERASE_LANDFORM_COLOR_COMMAND.DoOperation();

                    break;
                case DrawingModeEnum.WaterPaint:
                    {
                        Cmd_AddPaintedWaterFeature cmd = new(CURRENT_MAP);
                        UndoManager.AddCommand(cmd);
                        cmd.DoOperation();

                        CURRENT_MAP.IsSaved = false;
                    }
                    break;
                case DrawingModeEnum.WaterErase:
                    WaterFeatureSelectButton.Checked = false;
                    CURRENT_MAP.IsSaved = false;
                    break;
                case DrawingModeEnum.LakePaint:
                    {
                        IMAGEBOX_CLICK_POINT.X = e.X;
                        IMAGEBOX_CLICK_POINT.Y = e.Y;
                        LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));

                        int brushRadius = WaterFeatureMethods.WATER_BRUSH_SIZE / 2;

                        Cmd_AddLake cmd = new(CURRENT_MAP, brushRadius, LAYER_CLICK_POINT);
                        UndoManager.AddCommand(cmd);
                        cmd.DoOperation();

                        CURRENT_MAP.IsSaved = false;
                    }
                    break;
                case DrawingModeEnum.WaterColor:
                    COLOR_WATERFEATURE_COMMAND.DoOperation();

                    break;
                case DrawingModeEnum.WaterColorErase:
                    ERASE_WATERFEATURE_COLOR_COMMAND.DoOperation();

                    break;
                case DrawingModeEnum.RiverPaint:
                    {
                        IMAGEBOX_CLICK_POINT.X = e.X;
                        IMAGEBOX_CLICK_POINT.Y = e.Y;
                        RIVER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));

                        if (RIVER_CLICK_POINT.X >= CURRENT_MAP.MapWidth)
                        {
                            RIVER_CLICK_POINT.X = CURRENT_MAP.MapWidth - 1;
                        }

                        if (RIVER_CLICK_POINT.Y >= CURRENT_MAP.MapHeight)
                        {
                            RIVER_CLICK_POINT.Y = CURRENT_MAP.MapHeight - 1;
                        }

                        if (RIVER_CLICK_POINT != SKPoint.Empty
                            && RIVER_CLICK_POINT.X > 0
                            && RIVER_CLICK_POINT.Y > 0
                            && RIVER_CLICK_POINT.X < CURRENT_MAP.MapWidth
                            && RIVER_CLICK_POINT.Y < CURRENT_MAP.MapHeight)
                        {
                            Cmd_AddRiver cmd = new(CURRENT_MAP, RIVER_CLICK_POINT);
                            UndoManager.AddCommand(cmd);
                            cmd.DoOperation();

                        }

                        CURRENT_MAP.IsSaved = false;

                        RIVER_CLICK_POINT = SKPoint.Empty;
                        PREVIOUS_RIVER_CLICK_POINT = SKPoint.Empty;

                        WaterFeatureMethods.RIVER_POINT_LIST = [];

                    }
                    break;
                case DrawingModeEnum.PathPaint:
                    {
                        IMAGEBOX_CLICK_POINT.X = e.X;
                        IMAGEBOX_CLICK_POINT.Y = e.Y;
                        PATH_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));

                        if (PATH_CLICK_POINT != null && PREVIOUS_PATH_CLICK_POINT != null
                            && ((SKPoint)PATH_CLICK_POINT).X > 0
                            && ((SKPoint)PATH_CLICK_POINT).Y > 0
                            && ((SKPoint)PATH_CLICK_POINT).X < CURRENT_MAP.MapWidth
                            && ((SKPoint)PATH_CLICK_POINT).Y < CURRENT_MAP.MapHeight)
                        {
                            Cmd_AddMapPath cmd = new(CURRENT_MAP, (SKPoint)PATH_CLICK_POINT);
                            UndoManager.AddCommand(cmd);
                            cmd.DoOperation();
                        }

                        CURRENT_MAP.IsSaved = false;

                        PATH_CLICK_POINT = null;
                        PREVIOUS_PATH_CLICK_POINT = null;

                        MapPathMethods.ClearPathPointList();
                    }
                    break;
                case DrawingModeEnum.ColorSelect:
                    // eyedropper color select function
                    Cursor = Cursors.Default;
                    SetDrawingMode(DrawingModeEnum.None, null);

                    IMAGEBOX_CLICK_POINT.X = e.X;
                    IMAGEBOX_CLICK_POINT.Y = e.Y;
                    LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));

                    Color pixelColor = ((Bitmap)MapImageBox.Image).GetPixel((int)LAYER_CLICK_POINT.X, (int)LAYER_CLICK_POINT.Y);

                    IMAGEBOX_CLICK_POINT = Point.Empty;
                    LAYER_CLICK_POINT = SKPoint.Empty;

                    switch (LayerSelectTabControl.SelectedIndex)
                    {
                        case 0:
                            // baselayer
                            break;
                        case 1:
                            // ocean layer
                            OceanSelectedPaintColorLabel.BackColor = pixelColor;
                            OceanSelectedPaintColorLabel.Refresh();
                            break;
                        case 2:
                            // land layer
                            LandSelectedPaintColorLabel.BackColor = pixelColor;
                            LandSelectedPaintColorLabel.Refresh();
                            break;
                        case 3:
                            // water layer
                            WaterSelectedPaintColorLabel.BackColor = pixelColor;
                            WaterSelectedPaintColorLabel.Refresh();
                            break;
                    }
                    break;
                case DrawingModeEnum.LandformSelect:
                    {
                        Point clickPoint = new(e.X, e.Y);
                        Point mapClickPoint = MapImageBox.PointToImage(clickPoint);
                        MapPaintMethods.SelectLandformAtPoint(CURRENT_MAP, mapClickPoint);

                        MapImageBox.Refresh();

                    }
                    break;
                case DrawingModeEnum.WaterFeatureSelect:
                    {
                        Point clickPoint = new(e.X, e.Y);
                        Point mapClickPoint = MapImageBox.PointToImage(clickPoint);
                        MapPaintMethods.SelectWaterFeatureAtPoint(mapClickPoint);

                    }
                    break;
                case DrawingModeEnum.PathSelect:
                    {
                        Point clickPoint = new(e.X, e.Y);
                        Point mapClickPoint = MapImageBox.PointToImage(clickPoint);
                        MapPath? selectedPath = MapPathMethods.SelectMapPathAtPoint(mapClickPoint);

                        if (selectedPath != null)
                        {
                            selectedPath.IsSelected = !selectedPath.IsSelected;
                            selectedPath.ShowPathPoints = false;
                            MapPaintMethods.DeselectAllMapComponents(selectedPath);

                        }
                    }
                    break;
                case DrawingModeEnum.PathEdit:
                    {
                        Point clickPoint = new(e.X, e.Y);
                        Point mapClickPoint = MapImageBox.PointToImage(clickPoint);
                        MapPath? selectedPath = MapPathMethods.SelectMapPathAtPoint(mapClickPoint);

                        if (selectedPath != null)
                        {
                            Cmd_MapPathEdit cmd = new Cmd_MapPathEdit(selectedPath, MapImageBox.PointToImage(e.Location));
                            UndoManager.AddCommand(cmd);
                            cmd.DoOperation();
                        }
                    }
                    break;
                case DrawingModeEnum.SymbolSelect:
                    {
                        Point clickPoint = new(e.X, e.Y);
                        Point mapClickPoint = MapImageBox.PointToImage(clickPoint);

                        MapSymbol? selectedSymbol = SymbolMethods.SelectSymbolAtPoint(mapClickPoint);

                        if (selectedSymbol != null)
                        {
                            bool isSelected = selectedSymbol.GetIsSelected();

                            selectedSymbol.SetIsSelected(!isSelected);
                            MapPaintMethods.DeselectAllMapComponents(selectedSymbol);

                            if (selectedSymbol.GetIsSelected())
                            {
                                UISelectedMapSymbol = selectedSymbol;
                            }
                            else
                            {
                                UISelectedMapSymbol = null;
                            }
                        }
                    }
                    break;
                case DrawingModeEnum.DrawLabel:
                    if (!MapLabelMethods.CreatingLabel)
                    {
                        MapLabelMethods.CreatingLabel = true;

                        IMAGEBOX_CLICK_POINT.X = e.X;
                        IMAGEBOX_CLICK_POINT.Y = e.Y;

                        LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));

                        Font tbFont = new Font(MapLabelMethods.SELECTED_FONT.FontFamily, (float)(MapLabelMethods.SELECTED_FONT.Size * MapImageBox.ZoomFactor), GraphicsUnit.Pixel);

                        Size labelSize = TextRenderer.MeasureText("...Label...", tbFont, new Size(int.MaxValue, int.MaxValue), TextFormatFlags.Default);

                        LABEL_TEXT_BOX = new()
                        {
                            Parent = MapImageBox,
                            Name = Guid.NewGuid().ToString(),
                            Left = IMAGEBOX_CLICK_POINT.X - (labelSize.Width / 2),
                            Top = IMAGEBOX_CLICK_POINT.Y - (labelSize.Height / 2),
                            Width = labelSize.Width,
                            Height = labelSize.Height,
                            Margin = Padding.Empty,
                            AutoSize = false,
                            Font = tbFont,
                            Visible = true,
                            PlaceholderText = "...Label...",
                            BackColor = Color.AliceBlue,
                            ForeColor = FontColorSelectLabel.BackColor,
                            BorderStyle = BorderStyle.None,
                            Multiline = false,
                            TextAlign = HorizontalAlignment.Center,
                            Text = "...Label...",
                        };

                        LABEL_TEXT_BOX.KeyPress += LabelTextBox_KeyPress;

                        MapImageBox.Controls.Add(LABEL_TEXT_BOX);

                        MapImageBox.Refresh();

                        LABEL_TEXT_BOX.BringToFront();
                        LABEL_TEXT_BOX.Select(LABEL_TEXT_BOX.Text.Length, 0);
                        LABEL_TEXT_BOX.Focus();
                        LABEL_TEXT_BOX.ScrollToCaret();

                    }
                    break;
                case DrawingModeEnum.LabelSelect:
                    {
                        Point clickPoint = new(e.X, e.Y);
                        Point mapClickPoint = MapImageBox.PointToImage(clickPoint);

                        MapLabel? selectedLabel = MapLabelMethods.SelectLabelAtPoint(mapClickPoint);

                        if (selectedLabel != null)
                        {
                            bool isSelected = selectedLabel.IsSelected;

                            selectedLabel.IsSelected = !isSelected;
                            MapPaintMethods.DeselectAllMapComponents(selectedLabel);

                            if (selectedLabel.IsSelected)
                            {
                                UISelectedLabel = selectedLabel;
                            }
                            else
                            {
                                UISelectedLabel = null;
                            }
                        }
                        else
                        {
                            PlacedMapBox? selectedMapBox = MapLabelMethods.SelectMapBoxAtPoint(mapClickPoint);

                            if (selectedMapBox != null)
                            {
                                bool isSelected = selectedMapBox.IsSelected;
                                selectedMapBox.IsSelected = !isSelected;

                                MapPaintMethods.DeselectAllMapComponents(selectedMapBox);

                                if (selectedMapBox.IsSelected)
                                {
                                    UISelectedBox = selectedMapBox;
                                }
                                else
                                {
                                    UISelectedBox = null;
                                }
                            }
                        }
                    }

                    break;
                case DrawingModeEnum.DrawBox:
                    // finalize box drawing
                    if (UISelectedBox != null)
                    {
                        Cmd_AddLabelBox cmd = new Cmd_AddLabelBox(CURRENT_MAP, UISelectedBox);
                        UndoManager.AddCommand(cmd);
                        cmd.DoOperation();

                        CURRENT_MAP.IsSaved = false;
                        UISelectedBox = null;
                    }

                    break;
                case DrawingModeEnum.PlaceWindrose:
                    if (UIWindrose != null)
                    {
                        Cmd_AddWindrose cmd = new(CURRENT_MAP, UIWindrose);
                        UndoManager.AddCommand(cmd);
                        cmd.DoOperation();

                        CURRENT_MAP.IsSaved = false;
                        UIWindrose = CreateWindrose();
                    }
                    break;
                case DrawingModeEnum.SelectMapScale:
                    if (UIMapScale != null)
                    {
                        UIMapScale.IsSelected = false;
                        UIMapScale = null;
                        SetDrawingMode(DrawingModeEnum.None, null);

                        CURRENT_MAP.IsSaved = false;
                    }
                    break;
                case DrawingModeEnum.DrawMapMeasure:
                    IMAGEBOX_CLICK_POINT.X = e.X;
                    IMAGEBOX_CLICK_POINT.Y = e.Y;
                    LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));

                    if (UIMapMeasure != null)
                    {
                        if (!UIMapMeasure.MeasurePoints.Contains(PREVIOUS_LAYER_CLICK_POINT))
                        {
                            UIMapMeasure.MeasurePoints.Add(PREVIOUS_LAYER_CLICK_POINT);
                        }

                        UIMapMeasure.MeasurePoints.Add(LAYER_CLICK_POINT);

                        float lineLength = SKPoint.Distance(PREVIOUS_LAYER_CLICK_POINT, LAYER_CLICK_POINT);
                        UIMapMeasure.TotalMeasureLength += lineLength;
                    }

                    PREVIOUS_LAYER_CLICK_POINT = LAYER_CLICK_POINT;
                    break;
                case DrawingModeEnum.RegionSelect:
                    {
                        if (EDITING_REGION)
                        {
                            EDITING_REGION = false;
                        }
                        else
                        {
                            MapBuilder.GetLayerCanvas(CURRENT_MAP, MapBuilder.REGIONLAYER).Clear();
                            MapBuilder.GetLayerCanvas(CURRENT_MAP, MapBuilder.REGIONOVERLAYLAYER).Clear();

                            Point clickPoint = new(e.X, e.Y);
                            Point mapClickPoint = MapImageBox.PointToImage(clickPoint);
                            MapRegion? selectedRegion = MapRegionMethods.SelectRegionAtPoint(mapClickPoint);

                            if (selectedRegion != null)
                            {
                                if (selectedRegion.IsSelected)
                                {
                                    UIMapRegion = selectedRegion;
                                }
                                else
                                {
                                    UIMapRegion = null;
                                }
                            }
                        }
                    }
                    break;
                case DrawingModeEnum.LandformAreaSelect:
                    IMAGEBOX_CLICK_POINT = e.Location;
                    LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));

                    UISelectedLandformArea = new(PREVIOUS_LAYER_CLICK_POINT.X, PREVIOUS_LAYER_CLICK_POINT.Y, LAYER_CLICK_POINT.X, LAYER_CLICK_POINT.Y);

                    MapBuilder.ClearLayerCanvas(CURRENT_MAP, MapBuilder.WORKLAYER);
                    MapBuilder.GetLayerCanvas(CURRENT_MAP, MapBuilder.WORKLAYER)?.DrawRect((SKRect)UISelectedLandformArea, LandformType2Methods.LANDFORM_AREA_SELECT_PAINT);

                    break;
            }

            MapImageBox.Refresh();
        }

        #endregion

        #region RIGHT MOUSE BUTTON UP

        private void RightButtonMouseUpHandler(object sender, MouseEventArgs e)
        {
            Point clickPoint = new(e.X, e.Y);
            Point mapClickPoint = MapImageBox.PointToImage(clickPoint);

            switch (CURRENT_DRAWING_MODE)
            {
                case DrawingModeEnum.LandformSelect:

                    LandformSelectButton.Checked = false;

                    MapLandformType2? selectedLandform = MapPaintMethods.SelectLandformAtPoint(CURRENT_MAP, mapClickPoint);
                    MapImageBox.Refresh();

                    if (selectedLandform != null)
                    {
                        LandformData landformData = new();
                        landformData.SetMapLandform(selectedLandform);
                        landformData.ShowDialog(this);
                    }
                    break;
                case DrawingModeEnum.WaterFeatureSelect:
                    WaterFeature? selectedWaterFeature = MapPaintMethods.SelectWaterFeatureAtPoint(mapClickPoint);

                    if (selectedWaterFeature != null && selectedWaterFeature is MapPaintedWaterFeature)
                    {
                        // TODO: info dialog for water feature
                        //MessageBox.Show("selected water feature");
                    }
                    else if (selectedWaterFeature != null && selectedWaterFeature is MapRiver)
                    {
                        // TODO: info dialog for river
                        //MessageBox.Show("selected river");
                    }
                    break;
                case DrawingModeEnum.PathSelect:
                    MapPath? selectedPath = MapPathMethods.SelectMapPathAtPoint(mapClickPoint);

                    if (selectedPath != null)
                    {
                        // TODO: info dialog for path
                        //MessageBox.Show("selected path");
                    }
                    break;
                case DrawingModeEnum.SymbolSelect:
                    MapSymbol? selectedSymbol = SymbolMethods.SelectSymbolAtPoint(mapClickPoint);
                    if (selectedSymbol != null)
                    {
                        PlacedSymbolInfo psi = new(selectedSymbol, this);
                        psi.Show();
                    }
                    break;
            }
        }

        #endregion

        #endregion

        #region IMAGE BOX MOUSE MOVE HANDLER METHODS

        /*************************************************************************************************************************
        * MOUSE MOVE HANDLER METHODS
        *************************************************************************************************************************/

        #region MOUSE MOVE LEFT BUTTON PRESSED

        // LEFT BUTTON
        private void LeftButtonMouseMoveHandler(MouseEventArgs e, int brushRadius)
        {
            float zoomedRadius = (float)(brushRadius / MapImageBox.ZoomFactor);
            float X = (float)(LAYER_CLICK_POINT.X + zoomedRadius);
            float Y = (float)(LAYER_CLICK_POINT.Y + zoomedRadius);

            if (X >= CURRENT_MAP.MapWidth)
            {
                X = CURRENT_MAP.MapWidth - 1;
            }

            if (Y >= CURRENT_MAP.MapHeight)
            {
                Y = CURRENT_MAP.MapHeight - 1;
            }

            switch (CURRENT_DRAWING_MODE)
            {
                case DrawingModeEnum.OceanPaint:
                    SKShader oceanshader = MapPaintMethods.ConstructColorPaintShader(MapPaintMethods.GetSelectedColorBrushType(),
                        OceanSelectedPaintColorLabel.BackColor, brushRadius, X, Y);

                    COLOR_OCEAN_COMMAND.AddCircle(X, Y, brushRadius, oceanshader);

                    CURRENT_MAP.IsSaved = false;
                    break;
                case DrawingModeEnum.OceanErase:
                    ERASE_OCEAN_COLOR_COMMAND.AddCircle(X, Y, brushRadius);

                    CURRENT_MAP.IsSaved = false;
                    break;
                case DrawingModeEnum.LandPaint:
                    LandformType2Methods.SELECTED_LANDFORM.LandformPath.AddCircle(X, Y, brushRadius);
                    LandformType2Methods.SELECTED_LANDFORM.DrawLandform = true;

                    LandformType2Methods.CreateType2LandformPaths(CURRENT_MAP, LandformType2Methods.SELECTED_LANDFORM);

                    CURRENT_MAP.IsSaved = false;

                    break;
                case DrawingModeEnum.LandErase:

                    ERASE_LANDFORM_COMMAND.AddCircle(X, Y, brushRadius);
                    ERASE_LANDFORM_COMMAND.DoOperation();

                    CURRENT_MAP.IsSaved = false;
                    break;
                case DrawingModeEnum.LandColor:
                    if (UseSelectedBlendModeCheck.Checked)
                    {
                        LandformType2Methods.LAND_COLOR_PAINT.BlendMode = GetSelectedBlendMode();
                    }
                    else
                    {
                        LandformType2Methods.LAND_COLOR_PAINT.BlendMode = SKBlendMode.SrcOver;
                    }

                    SKShader landshader = MapPaintMethods.ConstructColorPaintShader(MapPaintMethods.GetSelectedColorBrushType(),
                        LandSelectedPaintColorLabel.BackColor, brushRadius, X, Y);

                    COLOR_LANDFORM_COMMAND.AddCircle(X, Y, brushRadius, landshader);

                    CURRENT_MAP.IsSaved = false;
                    break;
                case DrawingModeEnum.LandColorErase:
                    ERASE_LANDFORM_COLOR_COMMAND.AddCircle(X, Y, brushRadius);

                    CURRENT_MAP.IsSaved = false;
                    break;
                case DrawingModeEnum.WaterPaint:
                    WaterFeatureMethods.WATER_LAYER_DRAW_PATH.AddCircle(X, Y, brushRadius);

                    CURRENT_MAP.IsSaved = false;
                    break;
                case DrawingModeEnum.WaterErase:
                    ERASE_WATERFEATURE_COMMAND.AddCircle(X, Y, brushRadius);
                    ERASE_WATERFEATURE_COMMAND.DoOperation();

                    CURRENT_MAP.IsSaved = false;
                    break;
                case DrawingModeEnum.WaterColor:
                    SKShader watershader = MapPaintMethods.ConstructColorPaintShader(MapPaintMethods.GetSelectedColorBrushType(),
                        WaterSelectedPaintColorLabel.BackColor, brushRadius, X, Y);

                    COLOR_WATERFEATURE_COMMAND.AddCircle(X, Y, brushRadius, watershader);

                    CURRENT_MAP.IsSaved = false;
                    break;
                case DrawingModeEnum.WaterColorErase:
                    ERASE_WATERFEATURE_COLOR_COMMAND.AddCircle(X, Y, brushRadius);

                    CURRENT_MAP.IsSaved = false;
                    break;
                case DrawingModeEnum.PathPaint:
                    IMAGEBOX_CLICK_POINT = e.Location;
                    LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));

                    DrawMapPathLines();
                    break;
                case DrawingModeEnum.PathEdit:
                    SKPoint movePoint = Extensions.ToSKPoint(MapImageBox.PointToImage(e.Location));
                    MapPathMethods.MoveSelectedMapPathPoint(movePoint);

                    CURRENT_MAP.IsSaved = false;
                    break;
                case DrawingModeEnum.PathSelect:
                    IMAGEBOX_CLICK_POINT = e.Location;
                    LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));

                    SizeF deltaSize = new()
                    {
                        Width = LAYER_CLICK_POINT.X - PREVIOUS_LAYER_CLICK_POINT.X,
                        Height = LAYER_CLICK_POINT.Y - PREVIOUS_LAYER_CLICK_POINT.Y
                    };

                    MapPath? selectedPath = MapPathMethods.GetSelectedPath();

                    if (selectedPath != null)
                    {
                        List<MapPathPoint> pathPoints = selectedPath.PathPoints;

                        foreach (MapPathPoint point in pathPoints)
                        {
                            SKPoint p = point.MapPoint;
                            p.X += deltaSize.Width;
                            p.Y += deltaSize.Height;
                            point.MapPoint = p;
                        }

                        SKPath boundaryPath = MapPathMethods.GenerateMapPathBoundaryPath(pathPoints);
                        selectedPath.BoundaryPath = boundaryPath;
                    }

                    PREVIOUS_LAYER_CLICK_POINT = LAYER_CLICK_POINT;
                    CURRENT_MAP.IsSaved = false;
                    break;
                case DrawingModeEnum.RiverPaint:
                    IMAGEBOX_CLICK_POINT = e.Location;
                    LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));

                    DrawRiverLines();

                    break;
                case DrawingModeEnum.SymbolPlace:
                    IMAGEBOX_CLICK_POINT = e.Location;
                    LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));

                    if (AreaBrushCheck.Checked)
                    {
                        PlaceSelectedSymbolInArea(LAYER_CLICK_POINT);
                    }
                    else
                    {
                        PlaceSelectedSymbolAtCursor(LAYER_CLICK_POINT);
                    }

                    PREVIOUS_LAYER_CLICK_POINT = LAYER_CLICK_POINT;
                    CURRENT_MAP.IsSaved = false;
                    break;
                case DrawingModeEnum.SymbolErase:
                    int eraserRadius = AreaBrushSizeTrack.Value / 2;

                    SKPoint eraserCursorPoint = new(LAYER_CLICK_POINT.X + eraserRadius, LAYER_CLICK_POINT.Y + eraserRadius);

                    SymbolMethods.RemovePlacedSymbolsFromArea(CURRENT_MAP, eraserCursorPoint, eraserRadius);

                    CURRENT_MAP.IsSaved = false;
                    break;
                case DrawingModeEnum.SymbolColor:

                    int colorBrushRadius = AreaBrushSizeTrack.Value / 2;

                    SKPoint colorCursorPoint = new(LAYER_CLICK_POINT.X + colorBrushRadius, LAYER_CLICK_POINT.Y + colorBrushRadius);

                    Color[] symbolColors = { SymbolColor1Label.BackColor, SymbolColor2Label.BackColor, SymbolColor3Label.BackColor, SymbolColor4Label.BackColor };
                    SymbolMethods.ColorSymbolsInArea(colorCursorPoint, colorBrushRadius, symbolColors);

                    CURRENT_MAP.IsSaved = false;
                    break;
                case DrawingModeEnum.SymbolSelect:
                    if (UISelectedMapSymbol != null)
                    {
                        UISelectedMapSymbol.X = (int)LAYER_CLICK_POINT.X - UISelectedMapSymbol.Width / 2;
                        UISelectedMapSymbol.XLocation = (int)LAYER_CLICK_POINT.X - UISelectedMapSymbol.Width / 2;
                        UISelectedMapSymbol.Y = (int)LAYER_CLICK_POINT.Y - UISelectedMapSymbol.Height / 2;
                        UISelectedMapSymbol.YLocation = (int)LAYER_CLICK_POINT.Y - UISelectedMapSymbol.Height / 2;

                        CURRENT_MAP.IsSaved = false;
                    }

                    break;
                case DrawingModeEnum.LabelSelect:
                    if (UISelectedLabel != null)
                    {
                        if (UISelectedLabel.LabelPath != null)
                        {
                            float dx = LAYER_CLICK_POINT.X - (UISelectedLabel.X + (UISelectedLabel.Width / 2));
                            float dy = LAYER_CLICK_POINT.Y - (UISelectedLabel.Y + (UISelectedLabel.Height / 2));
                            UISelectedLabel.LabelPath.Transform(SKMatrix.CreateTranslation(dx, dy));
                        }
                        else
                        {
                            UISelectedLabel.X = (int)LAYER_CLICK_POINT.X - UISelectedLabel.Width / 2;
                            UISelectedLabel.Y = (int)LAYER_CLICK_POINT.Y;
                        }

                        PREVIOUS_LAYER_CLICK_POINT = LAYER_CLICK_POINT;
                        LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));
                    }
                    else if (UISelectedBox != null)
                    {
                        MapBuilder.GetLayerCanvas(CURRENT_MAP, MapBuilder.BOXLAYER)?.Clear();
                        UISelectedBox.X = (int)LAYER_CLICK_POINT.X;
                        UISelectedBox.Y = (int)LAYER_CLICK_POINT.Y;
                    }

                    CURRENT_MAP.IsSaved = false;
                    break;
                case DrawingModeEnum.DrawBezierLabelPath:
                    IMAGEBOX_CLICK_POINT = e.Location;
                    LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));

                    MapLabelMethods.LABEL_PATH_POINTS.Add(LAYER_CLICK_POINT);
                    MapLabelMethods.ConstructPathFromPoints();
                    MapLabelMethods.DrawLabelPath(CURRENT_MAP);
                    break;
                case DrawingModeEnum.DrawArcLabelPath:
                    IMAGEBOX_CLICK_POINT = e.Location;
                    LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));

                    SKRect r = new(PREVIOUS_LAYER_CLICK_POINT.X, PREVIOUS_LAYER_CLICK_POINT.Y, LAYER_CLICK_POINT.X, LAYER_CLICK_POINT.Y);
                    MapLabelMethods.LABEL_PATH.Dispose();
                    MapLabelMethods.LABEL_PATH = new();
                    MapLabelMethods.LABEL_PATH.AddArc(r, 180, 180);

                    MapBuilder.GetLayerCanvas(CURRENT_MAP, MapBuilder.WORKLAYER)?.Clear();

                    MapLabelMethods.DrawLabelPath(CURRENT_MAP);
                    break;
                case DrawingModeEnum.DrawBox:
                    // draw box as mouse is moved
                    IMAGEBOX_CLICK_POINT = e.Location;
                    LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));

                    if (MapLabelMethods.GetSelectedMapBox() != null)
                    {
                        SKRect boxRect = new(PREVIOUS_LAYER_CLICK_POINT.X, PREVIOUS_LAYER_CLICK_POINT.Y, LAYER_CLICK_POINT.X, LAYER_CLICK_POINT.Y);

                        MapBox? mb = MapLabelMethods.GetSelectedMapBox();

                        if (mb != null && boxRect.Width > 0 && boxRect.Height > 0)
                        {
                            Bitmap? b = mb.BoxBitmap;

                            if (b != null)
                            {
                                MapBuilder.GetLayerCanvas(CURRENT_MAP, MapBuilder.BOXLAYER)?.Clear();

                                using Bitmap resizedBitmap = new(b, (int)boxRect.Width, (int)boxRect.Height);

                                UISelectedBox ??= new();

                                UISelectedBox.SetBoxBitmap(new(resizedBitmap));
                                UISelectedBox.X = (int)PREVIOUS_LAYER_CLICK_POINT.X;
                                UISelectedBox.Y = (int)PREVIOUS_LAYER_CLICK_POINT.Y;
                                UISelectedBox.Width = (int)resizedBitmap.Width;
                                UISelectedBox.Height = (int)resizedBitmap.Height;

                                UISelectedBox.BoxTint = SelectBoxTintLabel.BackColor;

                                using SKPaint boxPaint = new()
                                {
                                    Style = SKPaintStyle.Fill,
                                    ColorFilter = SKColorFilter.CreateBlendMode(
                                        Extensions.ToSKColor(UISelectedBox.BoxTint),
                                        SKBlendMode.Modulate) // combine the tint with the bitmap color
                                };

                                MapBuilder.GetLayerCanvas(CURRENT_MAP, MapBuilder.BOXLAYER)?.DrawBitmap(Extensions.ToSKBitmap(resizedBitmap), PREVIOUS_LAYER_CLICK_POINT, boxPaint);
                            }
                        }
                    }
                    break;
                case DrawingModeEnum.SelectMapScale:
                    if (UIMapScale != null)
                    {
                        UIMapScale.X = (int)LAYER_CLICK_POINT.X - UIMapScale.Width / 2;
                        UIMapScale.Y = (int)LAYER_CLICK_POINT.Y - UIMapScale.Height / 2;

                        CURRENT_MAP.IsSaved = false;
                    }
                    break;
                case DrawingModeEnum.DrawMapMeasure:
                    IMAGEBOX_CLICK_POINT = e.Location;
                    LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));

                    if (UIMapMeasure != null)
                    {
                        SKCanvas workCanvas = MapBuilder.GetLayerCanvas(CURRENT_MAP, MapBuilder.WORKLAYER);
                        workCanvas.Clear();

                        if (UIMapMeasure.MeasureArea && UIMapMeasure.MeasurePoints.Count > 1)
                        {
                            SKPath path = new();

                            path.MoveTo(UIMapMeasure.MeasurePoints.First());

                            for (int i = 1; i < UIMapMeasure.MeasurePoints.Count; i++)
                            {
                                path.LineTo(UIMapMeasure.MeasurePoints[i]);
                            }

                            path.LineTo(LAYER_CLICK_POINT);

                            path.Close();

                            workCanvas.DrawPath(path, UIMapMeasure.MeasureAreaPaint);
                        }
                        else
                        {
                            workCanvas.DrawLine(PREVIOUS_LAYER_CLICK_POINT, LAYER_CLICK_POINT, UIMapMeasure.MeasureLinePaint);
                        }

                        // render measure value and units
                        SKPoint measureValuePoint = new(LAYER_CLICK_POINT.X + 30, LAYER_CLICK_POINT.Y + 20);

                        float lineLength = SKPoint.Distance(PREVIOUS_LAYER_CLICK_POINT, LAYER_CLICK_POINT);
                        float totalLength = UIMapMeasure.TotalMeasureLength + lineLength;

                        UIMapMeasure.RenderDistanceLabel(workCanvas, measureValuePoint, totalLength);

                        if (UIMapMeasure.MeasureArea && UIMapMeasure.MeasurePoints.Count > 1)
                        {
                            // temporarity add the point at the mouse position
                            UIMapMeasure.MeasurePoints.Add(LAYER_CLICK_POINT);

                            // calculate the polygon area
                            float area = MapDrawingMethods.CalculatePolygonArea(UIMapMeasure.MeasurePoints);

                            // remove the temporarily added point
                            UIMapMeasure.MeasurePoints.RemoveAt(UIMapMeasure.MeasurePoints.Count - 1);

                            // display the area label
                            SKPoint measureAreaPoint = new(LAYER_CLICK_POINT.X + 30, LAYER_CLICK_POINT.Y + 40);

                            UIMapMeasure.RenderAreaLabel(workCanvas, measureAreaPoint, area);
                        }
                    }
                    break;
                case DrawingModeEnum.RegionPaint:
                    IMAGEBOX_CLICK_POINT = e.Location;
                    SKPoint tempPoint = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));

                    if (UIMapRegion != null)
                    {
                        SKCanvas workCanvas = MapBuilder.GetLayerCanvas(CURRENT_MAP, MapBuilder.WORKLAYER);
                        workCanvas.Clear();

                        if (UIMapRegion.MapRegionPoints.Count > 1)
                        {
                            // temporarily add the layer click point for rendering
                            MapRegionPoint mrp = new MapRegionPoint(tempPoint);
                            UIMapRegion.MapRegionPoints.Add(mrp);

                            // render
                            UIMapRegion.Render(workCanvas);

                            // remove it
                            UIMapRegion.MapRegionPoints.RemoveAt(UIMapRegion.MapRegionPoints.Count - 1);
                        }
                        else
                        {
                            workCanvas.DrawLine(PREVIOUS_LAYER_CLICK_POINT, LAYER_CLICK_POINT, UIMapRegion.RegionBorderPaint);
                        }
                    }
                    break;
                case DrawingModeEnum.RegionSelect:
                    {
                        IMAGEBOX_CLICK_POINT = e.Location;
                        LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));

                        if (UIMapRegion != null && UIMapRegion.IsSelected)
                        {
                            MapRegionPoint? selectedMapRegionPoint = null;
                            foreach (MapRegionPoint p in UIMapRegion.MapRegionPoints)
                            {
                                using SKPath path = new();
                                path.AddCircle(p.RegionPoint.X, p.RegionPoint.Y, 5);

                                if (path.Contains(LAYER_CLICK_POINT.X, LAYER_CLICK_POINT.Y))
                                {
                                    p.IsSelected = true;
                                    selectedMapRegionPoint = p;
                                    EDITING_REGION = true;
                                }
                                else
                                {
                                    p.IsSelected = false;
                                }
                            }

                            if (selectedMapRegionPoint != null)
                            {
                                SKCanvas regionCanvas = MapBuilder.GetLayerCanvas(CURRENT_MAP, MapBuilder.REGIONLAYER);
                                regionCanvas.Clear();

                                SKCanvas regionOverlayCanvas = MapBuilder.GetLayerCanvas(CURRENT_MAP, MapBuilder.REGIONOVERLAYLAYER);
                                regionOverlayCanvas.Clear();

                                selectedMapRegionPoint.RegionPoint = LAYER_CLICK_POINT;
                            }
                        }
                    }
                    break;
                case DrawingModeEnum.LandformAreaSelect:
                    IMAGEBOX_CLICK_POINT = e.Location;
                    LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));

                    SKRect landAreaRect = new(PREVIOUS_LAYER_CLICK_POINT.X, PREVIOUS_LAYER_CLICK_POINT.Y, LAYER_CLICK_POINT.X, LAYER_CLICK_POINT.Y);

                    MapBuilder.ClearLayerCanvas(CURRENT_MAP, MapBuilder.WORKLAYER);
                    MapBuilder.GetLayerCanvas(CURRENT_MAP, MapBuilder.WORKLAYER)?.DrawRect(landAreaRect, LandformType2Methods.LANDFORM_AREA_SELECT_PAINT);
                    break;
            }


        }

        #endregion

        #region MOUSE MOVE RIGHT BUTTON PRESSED

        // RIGHT BUTTON
        private void RightButtonMouseMoveHandler(object sender, MouseEventArgs e)
        {
            // no right button mouse move actions at this point
        }

        #endregion

        #region MOUSE MOVE NO BUTTON PRESSED

        // NO BUTTON
        private void NoButtonMouseMoveHandler(object sender, MouseEventArgs e, float brushRadius)
        {
            if (CURRENT_DRAWING_MODE == DrawingModeEnum.None) return;

            float X = (float)(LAYER_CLICK_POINT.X);
            float Y = (float)(LAYER_CLICK_POINT.Y);

            if (X >= CURRENT_MAP.MapWidth)
            {
                X = CURRENT_MAP.MapWidth - 1;
            }

            if (Y >= CURRENT_MAP.MapHeight)
            {
                Y = CURRENT_MAP.MapHeight - 1;
            }

            switch (CURRENT_DRAWING_MODE)
            {
                case DrawingModeEnum.PathEdit:
                    {
                        MapPath? selectedMapPath = MapPathMethods.GetSelectedPath();

                        if (selectedMapPath != null)
                        {
                            foreach (MapPathPoint mp in selectedMapPath.PathPoints)
                            {
                                mp.IsSelected = false;
                            }

                            PointF mapPoint = MapImageBox.PointToImage(e.Location);
                            MapPathMethods.SelectMapPathPointAtPoint(selectedMapPath, mapPoint);
                        }
                    }
                    break;
                case DrawingModeEnum.SymbolPlace:
                    break;
                case DrawingModeEnum.PlaceWindrose:
                    if (UIWindrose != null)
                    {
                        UIWindrose.X = (int)X;
                        UIWindrose.Y = (int)Y;

                        MapBuilder.GetLayerCanvas(CURRENT_MAP, MapBuilder.WINDROSELAYER).Clear();

                        UIWindrose.Render(MapBuilder.GetLayerCanvas(CURRENT_MAP, MapBuilder.WINDROSELAYER));
                    }
                    break;
                case DrawingModeEnum.RegionPaint:
                    {
                        IMAGEBOX_CLICK_POINT = e.Location;
                        SKPoint tempPoint = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));

                        if (UIMapRegion != null)
                        {
                            SKCanvas workCanvas = MapBuilder.GetLayerCanvas(CURRENT_MAP, MapBuilder.WORKLAYER);
                            workCanvas.Clear();

                            if (UIMapRegion.MapRegionPoints.Count > 1)
                            {
                                // temporarily add the layer click point for rendering
                                MapRegionPoint mrp = new MapRegionPoint(tempPoint);
                                UIMapRegion.MapRegionPoints.Add(mrp);

                                // render
                                UIMapRegion.Render(workCanvas);

                                // remove it
                                UIMapRegion.MapRegionPoints.RemoveAt(UIMapRegion.MapRegionPoints.Count - 1);
                            }
                            else
                            {
                                workCanvas.DrawLine(PREVIOUS_LAYER_CLICK_POINT, LAYER_CLICK_POINT, UIMapRegion.RegionBorderPaint);
                            }
                        }
                    }
                    break;
                case DrawingModeEnum.RegionSelect:
                    {
                        IMAGEBOX_CLICK_POINT = e.Location;
                        LAYER_CLICK_POINT = Extensions.ToSKPoint(MapImageBox.PointToImage(IMAGEBOX_CLICK_POINT));

                        if (UIMapRegion != null && UIMapRegion.IsSelected)
                        {
                            bool pointSelected = false;
                            foreach (MapRegionPoint p in UIMapRegion.MapRegionPoints)
                            {
                                using SKPath path = new();
                                path.AddCircle(p.RegionPoint.X, p.RegionPoint.Y, 5);

                                if (path.Contains(LAYER_CLICK_POINT.X, LAYER_CLICK_POINT.Y))
                                {
                                    pointSelected = true;
                                    p.IsSelected = true;
                                }
                                else
                                {
                                    if (!EDITING_REGION)
                                    {
                                        p.IsSelected = false;
                                    }
                                }
                            }

                            SKCanvas regionOverlayCanvas = MapBuilder.GetLayerCanvas(CURRENT_MAP, MapBuilder.REGIONOVERLAYLAYER);
                            regionOverlayCanvas.Clear();

                            if (!pointSelected)
                            {
                                PREVIOUS_REGION_POINT_INDEX = -1;
                                NEXT_REGION_POINT_INDEX = -1;
                                NEW_REGION_POINT = null;

                                // is the cursor on a line segment between 2 region vertices? if so, draw a circle at the cursor location
                                for (int i = 0; i < UIMapRegion.MapRegionPoints.Count - 1; i++)
                                {
                                    if (MapDrawingMethods.LineContainsPoint(LAYER_CLICK_POINT, UIMapRegion.MapRegionPoints[i].RegionPoint, UIMapRegion.MapRegionPoints[i + 1].RegionPoint))
                                    {
                                        EDITING_REGION = true;

                                        PREVIOUS_REGION_POINT_INDEX = i;
                                        NEXT_REGION_POINT_INDEX = i + 1;

                                        NEW_REGION_POINT = new MapRegionPoint(LAYER_CLICK_POINT);

                                        regionOverlayCanvas.DrawCircle(LAYER_CLICK_POINT, MapRegionMethods.POINT_CIRCLE_RADIUS, MapRegionMethods.REGION_NEW_POINT_FILL_PAINT);
                                        regionOverlayCanvas.DrawCircle(LAYER_CLICK_POINT, MapRegionMethods.POINT_CIRCLE_RADIUS, MapRegionMethods.REGION_POINT_OUTLINE_PAINT);

                                        break;
                                    }
                                }

                                if (MapDrawingMethods.LineContainsPoint(LAYER_CLICK_POINT, UIMapRegion.MapRegionPoints[0].RegionPoint,
                                    UIMapRegion.MapRegionPoints[^1].RegionPoint))
                                {
                                    EDITING_REGION = true;

                                    PREVIOUS_REGION_POINT_INDEX = 0;
                                    NEXT_REGION_POINT_INDEX = UIMapRegion.MapRegionPoints.Count;

                                    NEW_REGION_POINT = new MapRegionPoint(LAYER_CLICK_POINT);

                                    regionOverlayCanvas.DrawCircle(LAYER_CLICK_POINT, MapRegionMethods.POINT_CIRCLE_RADIUS, MapRegionMethods.REGION_NEW_POINT_FILL_PAINT);
                                    regionOverlayCanvas.DrawCircle(LAYER_CLICK_POINT, MapRegionMethods.POINT_CIRCLE_RADIUS, MapRegionMethods.REGION_POINT_OUTLINE_PAINT);
                                }
                            }
                        }
                    }
                    break;
            }
        }

        #endregion

        #endregion

        #endregion

        #region IMAGE BOX MOUSE EVENT HANDLER UTILITY METHODS

        /*************************************************************************************************************************
        * IMAGE BOX MOUSE EVENT HANDLER UTILITY METHODS
        *************************************************************************************************************************/
        private int SetBrushRadius(DrawingModeEnum mode)
        {
            switch (mode)
            {
                case DrawingModeEnum.OceanPaint:
                    return OceanPaintMethods.OCEAN_BRUSH_SIZE / 2;
                case DrawingModeEnum.OceanErase:
                    return OceanPaintMethods.OCEAN_ERASER_SIZE / 2;
                case DrawingModeEnum.LandPaint:
                    return LandformType2Methods.LAND_BRUSH_SIZE / 2;
                case DrawingModeEnum.LandErase:
                    return LandformType2Methods.LAND_ERASER_SIZE / 2;
                case DrawingModeEnum.LandColor:
                    return LandformType2Methods.LAND_COLOR_BRUSH_SIZE / 2;
                case DrawingModeEnum.LandColorErase:
                    return LandformType2Methods.LAND_COLOR_ERASER_SIZE / 2;
                case DrawingModeEnum.WaterPaint:
                    return WaterFeatureMethods.WATER_BRUSH_SIZE / 2;
                case DrawingModeEnum.WaterErase:
                    return WaterFeatureMethods.WATER_ERASER_SIZE / 2;
                case DrawingModeEnum.WaterColor:
                    return WaterFeatureMethods.WATER_COLOR_BRUSH_SIZE / 2;
                case DrawingModeEnum.WaterColorErase:
                    return WaterFeatureMethods.WATER_COLOR_ERASER_SIZE / 2;
                case DrawingModeEnum.LakePaint:
                    return WaterFeatureMethods.WATER_BRUSH_SIZE / 2;
                case DrawingModeEnum.RiverPaint:
                    return 0;
                case DrawingModeEnum.SymbolErase:
                    return AreaBrushSizeTrack.Value / 2;
                case DrawingModeEnum.SymbolColor:
                    return AreaBrushSizeTrack.Value / 2;
                case DrawingModeEnum.SymbolPlace:
                    if (AreaBrushCheck.Checked)
                    {
                        return AreaBrushSizeTrack.Value / 2;
                    }
                    else
                    {
                        return 0;
                    }
                default:
                    break;
            }

            return 0;
        }

        #endregion
    }
}
