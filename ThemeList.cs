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
* This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY;
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
namespace MapCreator
{
    public partial class ThemeList : Form
    {
        private MapTheme? selectedTheme = null;
        private MapTheme[]? mapThemes = null;
        private readonly ThemeFilter themeFilter = new();

        public MapTheme? SettingsTheme { get; set; } = null;

        public ThemeList()
        {
            InitializeComponent();
        }

        public void SetThemes(MapTheme[] themes)
        {
            mapThemes = themes;

            for (int i = 0; i < themes.Length; i++)
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
            themeFilter.ApplyOceanColorPaletteSettings = ApplyOceanColorPaletteSettingsCheck.Checked;
            themeFilter.ApplyLandSettings = ApplyLandformSettingsCheck.Checked;
            themeFilter.ApplyLandformColorPaletteSettings = ApplyLandformColorPaletteSettingsCheck.Checked;
            themeFilter.ApplyFreshwaterSettings = ApplyWaterSettingsCheck.Checked;
            themeFilter.ApplyFreshwaterColorPaletteSettings = ApplyFreshwaterColorPaletteSettingsCheck.Checked;
            themeFilter.ApplyPathSetSettings = ApplyPathSettingsCheck.Checked;
            themeFilter.ApplySymbolSettings = ApplySymbolSettingsCheck.Checked;
            themeFilter.ApplyLabelPresetSettings = ApplyLabelPresetSettingsCheck.Checked;

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
            ApplyOceanColorPaletteSettingsCheck.Checked = CheckAllCheck.Checked;
            ApplyLandformSettingsCheck.Checked = CheckAllCheck.Checked;
            ApplyLandformColorPaletteSettingsCheck.Checked = CheckAllCheck.Checked;
            ApplyWaterSettingsCheck.Checked = CheckAllCheck.Checked;
            ApplyFreshwaterColorPaletteSettingsCheck.Checked = CheckAllCheck.Checked;
            ApplyPathSettingsCheck.Checked = CheckAllCheck.Checked;
            ApplySymbolSettingsCheck.Checked = CheckAllCheck.Checked;
            ApplyLabelPresetSettingsCheck.Checked = CheckAllCheck.Checked;
        }

        private void SaveThemeButton_Click(object sender, EventArgs e)
        {
            // TODO: apply changes made in UI to theme

            if (SettingsTheme != null)
            {
                ThemeNameEntry nameEntryDlg = new();
                DialogResult result = nameEntryDlg.ShowDialog();

                if (result == DialogResult.OK)
                {
                    string themeName = nameEntryDlg.ThemeNameTextBox.Text;
                    SettingsTheme.ThemeName = themeName;

                    string assetDirectory = Resources.ASSET_DIRECTORY;
                    string themePath = assetDirectory + Path.DirectorySeparatorChar + "Themes" + Path.DirectorySeparatorChar + themeName + ".mctheme";
                    SettingsTheme.ThemePath = themePath;

                    if (!File.Exists(themePath))
                    {
                        ThemeMethods.SaveTheme(SettingsTheme);
                    }
                    else
                    {
                        try
                        {
                            MapTheme? t = MapFileMethods.ReadThemeFromXml(themePath);

                            if (t != null)
                            {
                                if (!t.IsSystemTheme)
                                {
                                    if (MessageBox.Show("A theme with name " + themeName + " already exists. Replace it?", "Theme Exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly) == DialogResult.Yes)
                                    {
                                        ThemeMethods.SaveTheme(SettingsTheme);
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("The theme named " + themeName + " is a default theme. It cannot be replaced.", "Default Theme", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                                }
                            }
                        }
                        catch(Exception ex)
                        {
                            Program.LOGGER.Error(ex);
                        }
                    }
                }
            }
        }
    }
}
