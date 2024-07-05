using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapCreator
{
    public partial class ThemeList : Form
    {
        MapTheme? selectedTheme = null;
        MapTheme[]? mapThemes = null;
        ThemeFilter themeFilter = new();

        public ThemeList()
        {
            InitializeComponent();
        }

        public void SetThemes(MapTheme[] themes)
        {
            mapThemes = themes;

            for (int i = 0; i <  themes.Length; i++)
            {
#pragma warning disable CS8604 // Possible null reference argument.
                ThemeListComboBox.Items.Add(themes[i].ThemeName);

                if (themes[i].IsDefaultTheme)
                {
                    ThemeListComboBox.SelectedIndex = i;
                    ThemeListComboBox.SelectedText = themes[i].ThemeName;
                    selectedTheme = themes[i];
                }
#pragma warning restore CS8604 // Possible null reference argument.
            }
        }

#pragma warning disable CS8603 // Possible null reference return.
        public MapTheme GetSelectedTheme() => selectedTheme;
#pragma warning restore CS8603 // Possible null reference return.

        public ThemeFilter GetThemeFilter()
        {
            themeFilter.ApplyBackgroundSettings = ApplyBackgroundSettingsCheck.Checked;
            themeFilter.ApplyOceanSettings = ApplyOceanSettingsCheck.Checked;
            themeFilter.ApplyLandSettings = ApplyLandformSettingsCheck.Checked;
            themeFilter.ApplyFreshwaterSettings = ApplyWaterSettingsCheck.Checked;
            themeFilter.ApplyPathSetSettings = ApplyPathSettingsCheck.Checked;
            themeFilter.ApplySymbolSettings = ApplySymbolSettingsCheck.Checked;
            themeFilter.ApplyFrameSettings = ApplyFrameSettingsCheck.Checked;
            themeFilter.ApplyLabelSettings = ApplyLabelSettingsCheck.Checked;
            themeFilter.ApplyOverlaySettings = ApplyOverlaySettingsCheck.Checked;

            return themeFilter;
        }

        private void ThemeListComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (ThemeListComboBox.SelectedIndex > -1)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                selectedTheme = mapThemes[ThemeListComboBox.SelectedIndex];
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
        }

        private void CheckAllCheck_CheckStateChanged(object sender, EventArgs e)
        {
            ApplyBackgroundSettingsCheck.Checked = CheckAllCheck.Checked;
            ApplyOceanSettingsCheck.Checked = CheckAllCheck.Checked;
            ApplyLandformSettingsCheck.Checked = CheckAllCheck.Checked;
            ApplyWaterSettingsCheck.Checked = CheckAllCheck.Checked;
            ApplyPathSettingsCheck.Checked = CheckAllCheck.Checked;
            ApplySymbolSettingsCheck.Checked = CheckAllCheck.Checked;
            ApplyFrameSettingsCheck.Checked = CheckAllCheck.Checked;
            ApplyLabelSettingsCheck.Checked = CheckAllCheck.Checked;
            ApplyOverlaySettingsCheck.Checked = CheckAllCheck.Checked;
        }
    }
}
