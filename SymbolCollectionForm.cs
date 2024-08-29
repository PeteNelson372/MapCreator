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
using SkiaSharp.Views.Desktop;
using System.Text.Json.Nodes;

namespace MapCreator
{
    public partial class SymbolCollectionForm : Form
    {
        private int SYMBOL_NUMBER = 0;
        private bool collectionSaved = false;
        private MapSymbolCollection? collection = null;
        private MapSymbol? selectedSymbol = null;
        private string SEARCH_STRING = string.Empty;

        public SymbolCollectionForm()
        {
            InitializeComponent();
        }

        /******************************************************************************************************
        * *****************************************************************************************************
        * Form Event Handlers
        * *****************************************************************************************************
        *******************************************************************************************************/

        private void SymbolCollectionForm_Load(object sender, EventArgs e)
        {
            foreach (string tag in SymbolMethods.GetSymbolTags())
            {
                // add the tags in the symbol to the tag list box
                CheckedTagsListBox.Items.Add(tag);
            }
        }

        private void OpenCollectionDirectoryButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new()
            {
                InitialDirectory = SymbolMethods.GetDefaultSymbolDirectory()
            };

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                CollectionPathTextBox.Text = fbd.SelectedPath;

                string collectionFilePath = fbd.SelectedPath + Path.DirectorySeparatorChar + SymbolMethods.GetCollectionFileName();

                if (File.Exists(collectionFilePath))
                {
                    try
                    {
                        collection = MapFileMethods.ReadCollectionFromXml(collectionFilePath);

                        if (collection != null)
                        {
                            // load symbol file into object
                            foreach (MapSymbol symbol in collection.GetCollectionMapSymbols())
                            {
                                if (!string.IsNullOrEmpty(symbol.GetSymbolFilePath()))
                                {
                                    if (symbol.GetSymbolFormat() == SymbolFormatEnum.PNG
                                        || symbol.GetSymbolFormat() == SymbolFormatEnum.JPG
                                        || symbol.GetSymbolFormat() == SymbolFormatEnum.BMP)
                                    {
                                        symbol.SetSymbolBitmapFromPath(symbol.GetSymbolFilePath());
                                        SymbolMethods.AnalyzeSymbolBitmapColors(symbol);
                                    }
                                }

                                if (string.IsNullOrEmpty(symbol.GetCollectionPath()))
                                {
                                    symbol.SetCollectionPath(collectionFilePath);
                                }
                            }

                            CollectionNameTextBox.Text = collection.GetCollectionName();
                            CollectionSizeLabel.Text = collection.GetCollectionMapSymbols().Count.ToString();
                            NumberSymbolsTaggedLabel.Text = collection.GetNumberOfTaggedSymbols().ToString();

                            selectedSymbol = collection.GetCollectionMapSymbols().First();

                            if (selectedSymbol != null)
                            {
                                SYMBOL_NUMBER = 0;
                                SetUIFromSymbol(SYMBOL_NUMBER);
                            }
                        }

                    }
                    catch { }
                }
                else
                {
                    DialogResult result = MessageBox.Show("No collection file found.\nCreate a new collection from assets in the selected directory?", "No Collection File Found", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                    if (result == DialogResult.Yes)
                    {
                        collection = new();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                        collection.SetCollectionPath(collectionFilePath);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                        string[] pathParts = fbd.SelectedPath.Split(Path.DirectorySeparatorChar);
                        collection.SetCollectionName(pathParts[^1]);

                        CollectionNameTextBox.Text = collection.GetCollectionName();

                        List<MapSymbol> symbolList = MapFileMethods.ReadMapSymbolAssets(fbd.SelectedPath);

                        string wonderdraftSymbolsFilePath = fbd.SelectedPath + Path.DirectorySeparatorChar + SymbolMethods.GetWonderdraftSymbolsFileName();
                        List<WonderdraftSymbol> wdSymbols = ReadWonderdraftSymbolsFile(wonderdraftSymbolsFilePath);

                        foreach (var mapSymbol in symbolList)
                        {
                            mapSymbol.SetCollectionName(collection.GetCollectionName());
                            string symbolName = SymbolMethods.GetSymbolName(mapSymbol);

                            List<string> potentialTags = SymbolMethods.AutoTagSymbol(mapSymbol);

                            foreach (string tag in potentialTags)
                            {
                                // add the potential tags to the symbol
                                mapSymbol.AddSymbolTag(tag);
                            }

                            foreach (WonderdraftSymbol s in wdSymbols)
                            {
                                if (!string.IsNullOrEmpty(s.name) && s.name.Trim() == symbolName)
                                {
                                    if (s.draw_mode == "custom_colors")
                                    {
                                        mapSymbol.SetUseCustomColors(true);
                                    }
                                }
                            }

                            if (string.IsNullOrEmpty(mapSymbol.GetCollectionPath()))
                            {
                                mapSymbol.SetCollectionPath(collectionFilePath);
                            }

                            collection.AddCollectionMapSymbol(mapSymbol);
                        }

                        CollectionSizeLabel.Text = collection.GetCollectionMapSymbols().Count.ToString();
                        SymbolNumberLabel.Text = (SYMBOL_NUMBER + 1).ToString();

                        selectedSymbol = collection.GetCollectionMapSymbols().First();

                        if (selectedSymbol != null)
                        {
                            SYMBOL_NUMBER = 0;
                            SetUIFromSymbol(SYMBOL_NUMBER);
                        }
                    }
                }
            }
        }

        private void NextSymbolButton_Click(object sender, EventArgs e)
        {
            AdvanceToNextSymbol();
        }

        private void FirstSymbolButton_Click(object sender, EventArgs e)
        {
            SYMBOL_NUMBER = 0;
            SetUIFromSymbol(0);
        }

        private void PreviousSymbolButton_Click(object sender, EventArgs e)
        {
            SYMBOL_NUMBER = (SYMBOL_NUMBER > 0) ? SYMBOL_NUMBER - 1 : 0;
            SetUIFromSymbol(SYMBOL_NUMBER);
        }

        private void LastSymbolButton_Click(object sender, EventArgs e)
        {
            if (collection != null)
            {
                SYMBOL_NUMBER = collection.GetCollectionMapSymbols().Count - 1;
                SetUIFromSymbol(SYMBOL_NUMBER);
            }
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
                }

                NewTagTextBox.Clear();
            }
        }

        private void ApplyButton_Click(object sender, EventArgs e)
        {
            if (collection != null)
            {
                if (selectedSymbol != null)
                {
                    if (StructureRadioButton.Checked)
                    {
                        selectedSymbol.SetSymbolType(SymbolTypeEnum.Structure);
                    }
                    else if (VegetationRadioButton.Checked)
                    {
                        selectedSymbol.SetSymbolType(SymbolTypeEnum.Vegetation);
                    }
                    else if (TerrainRadioButton.Checked)
                    {
                        selectedSymbol.SetSymbolType(SymbolTypeEnum.Terrain);
                    }
                    else if (OtherRadioButton.Checked)
                    {
                        selectedSymbol.SetSymbolType(SymbolTypeEnum.Other);
                    }
                    else
                    {
                        selectedSymbol.SetSymbolType(SymbolTypeEnum.NotSet);
                    }

                    if (!string.IsNullOrEmpty(SymbolNameTextBox.Text))
                    {
                        selectedSymbol.SetSymbolName(SymbolNameTextBox.Text);
                    }

                    if (GrayScaleSymbolRadio.Checked)
                    {
                        selectedSymbol.SetIsGrayScale(true);
                    }

                    if (UseCustomColorsRadio.Checked)
                    {
                        selectedSymbol.SetUseCustomColors(true);
                    }

                    selectedSymbol.ClearSymbolTags();

                    List<string> selectedTags = GetSelectedTags();

                    foreach (string tag in selectedTags)
                    {
                        if (!string.IsNullOrEmpty(tag))
                        {
                            if (!selectedSymbol.HasTag(tag))
                            {
                                selectedSymbol.AddSymbolTag(tag);
                            }
                        }
                    }

                    collectionSaved = false;
                }

                NumberSymbolsTaggedLabel.Text = collection.GetNumberOfTaggedSymbols().ToString();
                NumberSymbolsTaggedLabel.Refresh();

                if (AutoAdvanceCheck.Checked)
                {
                    AdvanceToNextSymbol();
                }
            }
        }

        private void TagSearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            // filter the tag list based on what user enters in search box
            SEARCH_STRING = TagSearchTextBox.Text;
            FilterTags(SEARCH_STRING);
        }

        private void ClearAllChecksButton_Click(object sender, EventArgs e)
        {
            CheckedTagsListBox.ClearSelected();

            for (int index = 0; index < CheckedTagsListBox.Items.Count; index++)
            {
                CheckedTagsListBox.SetItemCheckState(index, CheckState.Unchecked);
            }
        }

        private void ResetTagsButton_Click(object sender, EventArgs e)
        {
            TagSearchTextBox.Text = string.Empty;
            AddTagsToListBox(SymbolMethods.GetSymbolTags());
        }

        private void AddToAllButton_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to apply the selected tags to all symbols in the collection?", "Apply tags to all symbols?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

            if (result == DialogResult.Yes)
            {
                List<string> selectedTags = GetSelectedTags();

                foreach (string tag in selectedTags)
                {
                    if (!string.IsNullOrEmpty(tag) && collection != null)
                    {
                        foreach (MapSymbol symbol in collection.CollectionMapSymbols)
                        {
                            if (!symbol.HasTag(tag))
                            {
                                symbol.AddSymbolTag(tag);
                            }
                        }
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (collection != null && collection.GetNumberOfTaggedSymbols() < collection.CollectionMapSymbols.Count)
            {
                DialogResult result = MessageBox.Show("Not all symbols have been tagged. Save collection anyway?", "Untagged Symbols", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                if (result == DialogResult.Yes)
                {
                    SaveCollection();
                }
            }
            else
            {
                SaveCollection();
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            if (collection != null)
            {
                bool allSymbolTypesAssigned = true;

                foreach (MapSymbol symbol in collection.GetCollectionMapSymbols())
                {
                    if (symbol.GetSymbolType() == SymbolTypeEnum.NotSet)
                    {
                        allSymbolTypesAssigned = false;
                        break;
                    }
                }

                if (!allSymbolTypesAssigned)
                {
                    DialogResult result = MessageBox.Show("Some symbols do not have a symbol type assigned. Close anyway?", "Symbol type not assigned", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                    if (result == DialogResult.Yes)
                    {
                        if (!collectionSaved)
                        {
                            result = MessageBox.Show("The collection.xml file has not been saved. Do you want to save it?", "Collection not saved", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                            if (result == DialogResult.Yes)
                            {
                                SaveCollection();
                                Close();
                            }
                            else if (result == DialogResult.No)
                            {
                                Close();
                            }
                        }
                    }
                }
                else
                {
                    if (!collectionSaved)
                    {
                        DialogResult result = MessageBox.Show("The collection.xml file has not been saved. Do you want to save it?", "Collection not saved", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);

                        if (result == DialogResult.Yes)
                        {
                            SaveCollection();
                            Close();
                        }
                        else if (result == DialogResult.No)
                        {
                            Close();
                        }
                    }
                }
            }
        }

        /******************************************************************************************************
        * *****************************************************************************************************
        * Form Methods
        * *****************************************************************************************************
        *******************************************************************************************************/

        private void AddTagsToListBox(List<string> tags)
        {
            foreach (string tag in tags)
            {
                // add the tags in the symbol to the tag list box
                AddTagToTagList(tag);
            }
        }

        private void FilterTags(string searchString)
        {
            List<string> tags = SymbolMethods.GetSymbolTags().ToList();

            if (searchString.Length > 2)
            {
                for (int i = tags.Count - 1; i >= 0; i--)
                {
                    if (!tags[i].Contains(searchString))
                    {
                        tags.RemoveRange(i, 1);
                    }
                }
            }

            CheckedTagsListBox.Items.Clear();
            AddTagsToListBox(tags);
        }

        private List<string> GetSelectedTags()
        {
            List<string> checkedTags = CheckedTagsListBox.CheckedItems.Cast<string>().ToList();
            return checkedTags;
        }

        private void AdvanceToNextSymbol()
        {
            if (collection != null && selectedSymbol != null)
            {
                if (selectedSymbol.GetSymbolType() != SymbolTypeEnum.NotSet)
                {
                    SYMBOL_NUMBER++;
                    SYMBOL_NUMBER = Math.Min(SYMBOL_NUMBER, collection.GetCollectionMapSymbols().Count - 1);
                    SetUIFromSymbol(SYMBOL_NUMBER);
                }
                else
                {
                    MessageBox.Show("Please select a symbol type (Structure, Vegetation, Terrain, Other) before advancing to the next symbol.", "Symbol type not set", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }
            }
        }

        private void SetUIFromSymbol(int symbolNumber)
        {
            if (collection != null)
            {
                CollectionSizeLabel.Text = collection.GetCollectionMapSymbols().Count.ToString();
                SymbolNumberLabel.Text = (symbolNumber + 1).ToString();

                symbolNumber = Math.Max(symbolNumber, 0);
                selectedSymbol = collection.GetCollectionMapSymbols()[symbolNumber];

                SetSymbolTypeRadio(selectedSymbol);
                SetSelectedTags(selectedSymbol);
                SymbolNameTextBox.Text = SymbolMethods.GetSymbolName(selectedSymbol);
                GrayScaleSymbolRadio.Checked = selectedSymbol.GetIsGrayScale();
                UseCustomColorsRadio.Checked = selectedSymbol.GetUseCustomColors();

                // clear checked items from tag list box
                CheckedTagsListBox.ClearSelected();

                for (int index = 0; index < CheckedTagsListBox.Items.Count; index++)
                {
                    CheckedTagsListBox.SetItemCheckState(index, CheckState.Unchecked);
                }

                AddTagsToListBox(selectedSymbol.GetSymbolTags());

                foreach (string tag in selectedSymbol.GetSymbolTags())
                {
                    // check the symbol tags
                    CheckTagInTagList(tag, CheckState.Checked);
                }

                List<string> potentialTags = SymbolMethods.AutoTagSymbol(selectedSymbol);
                AddTagsToListBox(potentialTags);

                foreach (string tag in potentialTags)
                {
                    // set the potential tags to indeterminate state
                    CheckTagInTagList(tag, CheckState.Indeterminate);
                }

                SymbolPictureBox.Image = Extensions.ToBitmap(selectedSymbol.GetSymbolBitmap());
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

        private void SetSymbolTypeRadio(MapSymbol symbol)
        {
            switch (symbol.GetSymbolType())
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
                case SymbolTypeEnum.Other:
                    OtherRadioButton.Checked = true;
                    break;
                default:
                    TerrainRadioButton.Checked = false;
                    StructureRadioButton.Checked = false;
                    VegetationRadioButton.Checked = false;
                    OtherRadioButton.Checked = false;
                    break;
            }
        }

        private void SaveCollection()
        {
            if (collection != null && collection.GetCollectionPath().Length > 0)
            {
                MapFileMethods.SerializeSymbolCollection(collection);
                MessageBox.Show("Collection has been saved.", "Collection Saved", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                collectionSaved = true;
            }
        }

        private void SetSelectedTags(MapSymbol symbol)
        {
            for (int i = 0; i < CheckedTagsListBox.Items.Count; i++)
            {
                if (symbol.HasTag((string)CheckedTagsListBox.Items[i]))
                {
                    CheckedTagsListBox.SetItemChecked(i, true);
                }
            }
        }

        /******************************************************************************************************
        * *****************************************************************************************************
        * Static Methods
        * *****************************************************************************************************
        *******************************************************************************************************/

        private static List<WonderdraftSymbol> ReadWonderdraftSymbolsFile(string wonderdraftSymbolsFilePath)
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.

            List<WonderdraftSymbol> wonderdraftSymbols = [];

            try
            {
                // the .wonderdraft_symbols file isn't really needed - the drawing mode
                // is already determined from the bitmap colors
                if (File.Exists(wonderdraftSymbolsFilePath))
                {
                    string jsonString = File.ReadAllText(wonderdraftSymbolsFilePath);

                    var jsonObject = JsonNode.Parse(jsonString).AsObject();

                    if (jsonObject != null)
                    {
                        foreach (KeyValuePair<string, JsonNode?> kvPair in jsonObject)
                        {
                            WonderdraftSymbol ws = new();

                            ws.name = kvPair.Key;

                            foreach (var kvp in kvPair.Value.AsObject().Where(kvp => kvp.Key == "draw_mode"))
                            {
                                ws.draw_mode = (string?)kvp.Value;
                            }

                            wonderdraftSymbols.Add(ws);
                        }
                    }
                }
            } catch { }

#pragma warning restore CS8602 // Dereference of a possibly null reference.
            return wonderdraftSymbols;
        }
    }
}
