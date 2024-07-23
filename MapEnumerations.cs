namespace MapCreator
{
    public enum DrawingModeEnum
    {
        None,
        OceanPaint,
        OceanErase,
        LandPaint,
        LandErase,
        LandColor,
        LandColorErase,
        WaterPaint,
        WaterErase,
        WaterColor,
        WaterColorErase,
        LakePaint,
        RiverPaint,
        ColorSelect,
        LandformSelect,
        WaterFeatureSelect,
        PathSelect,
        PathPaint,
        PathEdit,
        SymbolSelect,
        SymbolPlace,
        SymbolErase,
        SymbolMove,
        SymbolColor,
        LabelSelect,
        DrawLabel,
        DrawArcLabelPath,
        DrawBezierLabelPath,
        DrawBox,
        PlaceWindrose,
        SelectMapScale,
        DrawMapMeasure,
        RegionSelect,
        RegionPaint
    }

    public enum GradientDirectionEnum
    {
        None,
        DarkToLight,
        LightToDark
    }

    public enum ColorPaintBrush
    {
        None,
        SoftBrush,
        HardBrush
    }

    public enum  WaterFeatureTypeEnum
    {
        NotSet,
        Lake,
        River,
        Other
        // could define other types, like swamp, canal, inland sea, etc.
    }

    public enum PathTypeEnum
    {
        SolidLinePath,
        DottedLinePath,
        DashedLinePath,
        DashDotLinePath,
        DashDotDotLinePath,
        DoubleSolidBorderPath,
        ChevronLinePath,
        LineAndDashesPath,
        ShortIrregularDashPath,
        ThickSolidLinePath,
        SolidBlackBorderPath,
        BorderedGradientPath,
        BorderedLightSolidPath,
        BearTracksPath,
        BirdTracksPath,
        FootprintsPath,
        RailroadTracksPath,
        TexturedPath,
        BorderAndTexturePath
    }

    public enum ParallelEnum
    {
        Above,
        Below
    }

    public enum SymbolFormatEnum
    {
        NotSet,
        PNG,
        JPG,
        BMP,
        Vector
    }

    public enum SymbolTypeEnum
    {
        NotSet,
        Structure,
        Vegetation,
        Terrain,
        Other
    }

    public enum SymbolRenderMoveDirectionEnum
    {
        Up, Down
    }

    public enum LabelTextAlignEnum
    {
        AlignLeft,
        AlignCenter,
        AlignRight
    }

    public enum GridTypeEnum
    {
        NotSet,
        Square,
        FlatHex,
        PointedHex
    }

    public enum ScaleNumbersDisplayEnum
    {
        None,
        Ends,
        EveryOther,
        All
    }
}
