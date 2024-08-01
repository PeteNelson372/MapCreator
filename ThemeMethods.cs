namespace MapCreator
{
    internal class ThemeMethods
    {
        public static void SaveTheme(MapTheme theme)
        {
            if (theme != null)
            {
                // serialze theme as XML to a file

                // validate the data entered in the dialog
                //  - verify theme name is entered
                //  - what other data is required?
                // select a path

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
    }
}
