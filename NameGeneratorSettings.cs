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
using System.Windows.Forms;

namespace MapCreator
{
    public partial class NameGeneratorSettings : Form
    {
        public event EventHandler NameGenerated;

        public string SelectedName { get; set; } = string.Empty;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public NameGeneratorSettings()

        {
            InitializeComponent();
        }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        protected virtual void OnNameGenerated(EventArgs e)
        {
            EventHandler eventHandler = NameGenerated;
            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
        }

        private void NameGenSettingsCancelButton_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void NameGenSettingsApplyButton_Click(object sender, EventArgs e)
        {
            // apply name generator settings
            OnNameGenerated(EventArgs.Empty);
        }

        private void GenerateNamesButton_Click(object sender, EventArgs e)
        {
            GeneratedNamesList.Items.Clear();

            for (int i = 0; i < 10; i++)
            {
                string name = MapToolMethods.GenerateRandomPlaceName();
                GeneratedNamesList.Items.Add(name);
            }

            GeneratedNamesList.Refresh();
        }

        private void SelectAllGeneratorsCheck_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < NameGeneratorCheckedList.Items.Count; i++)
            {
                NameGeneratorCheckedList.SetItemChecked(i, SelectAllGeneratorsCheck.Checked);
            }
        }

        private void SelectAllNameBasesCheck_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < NameBaseCheckedList.Items.Count; i++)
            {
                NameBaseCheckedList.SetItemChecked(i, SelectAllNameBasesCheck.Checked);
            }
        }

        private void SelectAllLanguagesCheck_CheckedChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < LanguagesCheckedList.Items.Count; i++)
            {
                LanguagesCheckedList.SetItemChecked(i, SelectAllLanguagesCheck.Checked);
            }
        }

        private void LanguagesCheckedList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            foreach (NameBase nameBase in MapToolMethods.NameBases)
            {
                if (nameBase.NameBaseLanguage == LanguagesCheckedList.Items[e.Index].ToString())
                {
                    nameBase.IsLanguageSelected = e.NewValue == CheckState.Checked;
                }
            }
        }

        private void NameBaseCheckedList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            foreach (NameBase nameBase in MapToolMethods.NameBases)
            {
                if (nameBase.NameBaseName == NameBaseCheckedList.Items[e.Index].ToString())
                {
                    nameBase.IsNameBaseSelected = e.NewValue == CheckState.Checked;
                }
            }
        }

        private void NameGeneratorCheckedList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            foreach (NameGenerator nameGen in MapToolMethods.NameGenerators)
            {
                if (nameGen.NameGeneratorName == NameGeneratorCheckedList.Items[e.Index].ToString())
                {
                    nameGen.IsSelected = e.NewValue == CheckState.Checked;
                }
            }
        }

        private void GeneratedNamesList_SelectedValueChanged(object sender, EventArgs e)
        {
            if (GeneratedNamesList.SelectedItem != null && !string.IsNullOrEmpty((string?)GeneratedNamesList.SelectedItem))
            {
                SelectedName = (string)GeneratedNamesList.SelectedItem;
            }
        }
    }
}
