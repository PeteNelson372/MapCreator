namespace MapCreator
{
    public partial class SymbolInfo : Form
    {
        private MapSymbol symbol;
        private readonly MapSymbolCollection? collection;

        public SymbolInfo(MapSymbol symbol)
        {
            InitializeComponent();
            this.symbol = symbol;

            foreach (MapSymbolCollection symbolCollection in SymbolMethods.GetSymbolCollections())
            {
                if (symbolCollection.GetCollectionPath() == symbol.GetCollectionPath())
                {
                    collection = symbolCollection;
                    break;
                }
            }

            SymbolNameLabel.Text = this.symbol.GetSymbolName();
            CollectionNameLabel.Text = this.symbol.GetCollectionName();
            SymbolPathLabel.Text = this.symbol.GetSymbolFilePath();
            SymbolGuidLabel.Text = this.symbol.GetSymbolGuid().ToString();
            SymbolFormatLabel.Text = this.symbol.GetSymbolFormat().ToString();

            switch (this.symbol.GetSymbolType())
            {
                case SymbolTypeEnum.Terrain:
                    TerrainRadioButton.Checked = true;
                    break;
                case SymbolTypeEnum.Structure:
                    StructureRadioButton.Checked = true;
                    break;
                case SymbolTypeEnum.Vegetation:
                    VegetationRadioButton.Checked = true;
                    break;
            }

            if (this.symbol.GetIsGrayScale())
            {
                GrayScaleSymbolRadio.Checked = true;
            }

            if (this.symbol.GetUseCustomColors())
            {
                UseCustomColorsRadio.Checked = true;
            }

            foreach (string tag in SymbolMethods.GetSymbolTags())
            {
                // add the tags in the symbol to the tag list box
                CheckedTagsListBox.Items.Add(tag);
            }

            AddTagsToListBox(this.symbol.GetSymbolTags());

            foreach (string tag in this.symbol.GetSymbolTags())
            {
                // check the symbol tags
                CheckTagInTagList(tag, CheckState.Checked);
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AddTagButton_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(NewTagTextBox.Text) && !CheckedTagsListBox.Items.Contains(NewTagTextBox.Text))
            {
                string newTag = new string(NewTagTextBox.Text.Where(char.IsLetter).ToArray()).ToLower();

                if (!string.IsNullOrEmpty(newTag))
                {
                    AddTagToTagList(newTag);
                    SymbolMethods.AddSymbolTag(newTag);

                    if (collection != null)
                    {
                        collection.IsModified = true;
                    }
                }

                NewTagTextBox.Clear();
            }
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            symbol.ClearSymbolTags();

            List<string> selectedTags = GetSelectedTags();

            foreach (string tag in selectedTags)
            {
                if (!string.IsNullOrEmpty(tag))
                {
                    if (!symbol.HasTag(tag))
                    {
                        symbol.AddSymbolTag(tag);
                        
                        if (collection != null)
                        {
                            collection.IsModified = true;
                        }
                    }
                }
            }
        }

        private List<string> GetSelectedTags()
        {
            List<string> checkedTags = CheckedTagsListBox.CheckedItems.Cast<string>().ToList();
            return checkedTags;
        }

        private void AddTagsToListBox(List<string> tags)
        {
            foreach (string tag in tags)
            {
                // add the tags in the symbol to the tag list box
                AddTagToTagList(tag);
            }
        }

        private void AddTagToTagList(string tag)
        {
            tag = tag.Trim([' ', ',']).ToLower();

            if (!CheckedTagsListBox.Items.Contains(tag))
            {
                CheckedTagsListBox.Items.Add(tag);
            }
        }

        private void CheckTagInTagList(string tag, CheckState checkState)
        {
            int tagIndex = CheckedTagsListBox.Items.IndexOf(tag);
            if (tagIndex > -1)
            {
                CheckedTagsListBox.SetItemCheckState(tagIndex, checkState);
            }
        }
    }
}
