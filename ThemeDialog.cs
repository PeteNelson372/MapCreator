using System.Xml.Serialization;

namespace MapCreator
{
    public partial class ThemeDialog : Form
    {
        private MapTheme theme = new();

        private List<MapTexture> BackgroundTextureList = [];
        private List<MapTexture> WaterTextureList = [];
        private List<MapTexture> LandTextureList = [];

        public ThemeDialog()
        {
            InitializeComponent();
        }

        public void SetTheme(ref MapTheme theme)
        {
            this.theme = theme;
            ApplyThemeValuesToDialog();
        }

        public void InitializeValues(List<MapTexture> backgroundTextureList, List<MapTexture> waterTextureList, List<MapTexture> landTextureList)
        {
            BackgroundTextureList = backgroundTextureList;
            WaterTextureList = waterTextureList;
            LandTextureList = landTextureList;

            foreach (MapTexture texture in BackgroundTextureList)
            {
                texture.TextureBitmap = new Bitmap(texture.TexturePath);
                BackgroundTextureComboBox.Items.Add(texture.TextureName);
            }

            foreach (MapTexture texture in WaterTextureList)
            {
                texture.TextureBitmap = new Bitmap(texture.TexturePath);
                OceanTextureComboBox.Items.Add(texture.TextureName);
            }

            foreach (MapTexture texture in LandTextureList)
            {
                texture.TextureBitmap = new Bitmap(texture.TexturePath);
                LandformTextureComboBox.Items.Add(texture.TextureName);
            }
        }

        private void ApplyThemeValuesToDialog()
        {
            ThemeName.Text = theme.ThemeName;

            if (theme.ThemePath == null || theme.ThemePath.Length == 0)
            {
                theme.ThemePath = Resources.ASSET_DIRECTORY + Path.DirectorySeparatorChar + "Themes";
            }

            ThemePath.Text = theme.ThemePath;
            DefaultThemeCheck.Checked = theme.IsDefaultTheme;

            if (theme.BackgroundTexture != null)
            {
                BackgroundTextureComboBox.SelectedItem = theme.BackgroundTexture.TextureName;
                BackgroundPreviewPictureBox.Image = theme.BackgroundTexture.TextureBitmap;
            }

            if (theme.BackGroundTextureOpacity != null)
            {
                BackgroundTextureOpacityTrack.Value = (int)theme.BackGroundTextureOpacity;
            }

            if (theme.OceanTexture != null)
            {
                OceanTextureComboBox.SelectedItem = theme.OceanTexture.TextureName;
                OceanPreviewPictureBox.Image = theme.OceanTexture.TextureBitmap;
            }

            if (theme.OceanTextureOpacity != null)
            {
                OceanTextureOpacityTrack.Value = (int)theme.OceanTextureOpacity;
            }

            if (theme.OceanColor != null)
            {
                OceanColorLabel.BackColor = (Color)theme.OceanColor;
            }

            if (theme.OceanColorOpacity != null)
            {
                OceanColorOpacityTrack.Value = (int)theme.OceanColorOpacity;
            }

            if (theme.LandformTexture != null)
            {
                LandformTextureComboBox.SelectedItem = theme.LandformTexture.TextureName;
            }

            if (theme.LandformOutlineColor != null)
            {
                LandOutlineColorLabel.BackColor = (Color)theme.LandformOutlineColor;
            }

            if (theme.LandShorelineStyle != null)
            {
                LandShorelineStyleComboxBox.SelectedItem = theme.LandShorelineStyle;
            }

            if (theme.LandformCoastlineColor != null)
            {
                CoastColorLabel.BackColor = (Color)theme.LandformCoastlineColor;
            }

            if (theme.LandformCoastlineColorOpacity != null)
            {
                CoastColorOpacityTrack.Value = (int)theme.LandformCoastlineColorOpacity;
            }

            if (theme.LandformCoastlineEffectDistance != null)
            {
                CoastEffectDistanceTrack.Value = (int)theme.LandformCoastlineEffectDistance;
            }

            if (theme.LandformCoastlineStyle != null)
            {
                LandCoastlineStyleComboBox.SelectedItem = theme.LandformCoastlineStyle;
            }

            if (theme.FreshwaterColor != null)
            {
                FreshwaterColorLabel.BackColor = (Color)theme.FreshwaterColor;
            }

            if (theme.FreshwaterColorOpacity != null)
            {
                FreshwaterColorOpacityTrack.Value = (int)theme.FreshwaterColorOpacity;
            }

            if (theme.FreshwaterShorelineColor != null)
            {
                FreshwaterShorelineColorLabel.BackColor = (Color) theme.FreshwaterShorelineColor;
            }

            if (theme.FreshwaterShorelineColorOpacity != null)
            {
                ShorelineColorOpacityTrack.Value = (int)theme.FreshwaterShorelineColorOpacity;
            }




            // TODO: set values on all other ThemeDialog tabs as they are defined on the MainForm tabs
        }

        private void SaveTheme()
        {
            if (ThemeName.Text == null || ThemeName.Text.Length == 0)
            {
                MessageBox.Show(this, "Please enter a name for this theme before saving.", "Enter Theme Name", MessageBoxButtons.OK);
                return;
            }

            if (theme != null)
            {
                // serialze theme as XML to a file

                // validate the data entered in the dialog
                //  - verify theme name is entered
                //  - what other data is required?
                // select a path

                theme.ThemeName = ThemeName.Text;

                string assetDirectory = Resources.ASSET_DIRECTORY;
                string themeDirectory = assetDirectory + Path.DirectorySeparatorChar + "Themes";

                SaveFileDialog sfd = new()
                {
                    Title = "Save Theme",
                    AddExtension = true,
                    CreatePrompt = true,
                    DefaultExt = "mctheme",
                    Filter = "Map Creator Theme files (*.mctheme)|*.mctheme|All files (*.*)|*.*",
                    FilterIndex = 0,

                    FileName = theme.ThemeName,

                    CheckPathExists = true,

                    InitialDirectory = themeDirectory
                };

                DialogResult result = sfd.ShowDialog();

                if (result == DialogResult.OK)
                {
                    // write the theme as XML
                    theme.ThemePath = sfd.FileName;
                    theme.ThemeName = Path.GetFileNameWithoutExtension(sfd.FileName);
                    MapFileMethods.SerializeTheme(theme);
                }
            }
        }

        private void SaveThemeButton_Click(object sender, EventArgs e)
        {
            SaveTheme();
        }

        private void BackgroundTextureComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (BackgroundTextureComboBox.SelectedIndex > -1)
            {
                BackgroundPreviewPictureBox.Image = BackgroundTextureList[BackgroundTextureComboBox.SelectedIndex].TextureBitmap;
            }
        }

        private void DefaultThemeCheck_CheckedChanged(object sender, EventArgs e)
        {
            theme.IsDefaultTheme = DefaultThemeCheck.Checked;
        }
    }
}
