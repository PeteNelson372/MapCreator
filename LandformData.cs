namespace MapCreator
{
    public partial class LandformData : Form
    {
        private MapLandformType2? landform;

        public LandformData()
        {
            InitializeComponent();
        }

        public void SetMapLandform(MapLandformType2 mapLandform)
        {
            landform = mapLandform;

            LandformGuidLabel.Text = landform.LandformGuid.ToString();
            LandformNameText.Text = landform.LandformName;
            LandformTextureText.Text = landform.LandformTexture?.TextureName;
#pragma warning disable CS8629 // Nullable value type may be null.
            LandformOutlineColorText.Text = ColorTranslator.ToHtml((Color)(landform?.LandformOutlineColor));
#pragma warning restore CS8629 // Nullable value type may be null.
            LandformOutlineWidthText.Text = landform.LandformOutlineWidth.ToString();
            LandformShorelineStyleText.Text = landform.ShorelineStyle.ToString();
#pragma warning disable CS8629 // Nullable value type may be null.
            LandformCoastlineColorText.Text = ColorTranslator.ToHtml((Color)(landform?.CoastlineColor));
#pragma warning restore CS8629 // Nullable value type may be null.
            CoastlineColorOpacityText.Text = landform?.CoastlineColorOpacity.ToString();
            CoastlineEffectDistanceText.Text = landform?.CoastlineEffectDistance.ToString();
            CoastlineStyleNameText.Text = landform?.CoastlineStyleName;
            CoastlineHatchPatternText.Text = landform?.CoastlineHatchPattern;
            CoastlineHatchOpacityText.Text = landform?.CoastlineHatchOpacity.ToString();
            CoastlineHatchScaleText.Text = landform?.CoastlineHatchScale.ToString();
            CoastlineHatchBlendModeText.Text = landform?.CoastlineHatchBlendMode;

#pragma warning disable CS8629 // Nullable value type may be null.
            PaintCoastlineGradientCheck.Checked = (bool)(landform?.PaintCoastlineGradient);
#pragma warning restore CS8629 // Nullable value type may be null.

            //ColorPathCountLabel.Text = landform?.LandformColorPaths.Count.ToString();
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
