namespace MapCreator
{
    partial class NameGeneratorSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            NameGenSettingsCancelButton = new Button();
            NameGenSettingsApplyButton = new Button();
            NameGeneratorCheckedList = new CheckedListBox();
            label1 = new Label();
            NameBaseCheckedList = new CheckedListBox();
            label2 = new Label();
            SelectAllGeneratorsCheck = new CheckBox();
            SelectAllNameBasesCheck = new CheckBox();
            label3 = new Label();
            SelectAllLanguagesCheck = new CheckBox();
            LanguagesCheckedList = new CheckedListBox();
            GenerateNamesButton = new Button();
            GeneratedNamesList = new ListBox();
            SuspendLayout();
            // 
            // NameGenSettingsCancelButton
            // 
            NameGenSettingsCancelButton.Font = new Font("Tahoma", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            NameGenSettingsCancelButton.Location = new Point(560, 368);
            NameGenSettingsCancelButton.Name = "NameGenSettingsCancelButton";
            NameGenSettingsCancelButton.Size = new Size(89, 44);
            NameGenSettingsCancelButton.TabIndex = 12;
            NameGenSettingsCancelButton.Text = "&Cancel";
            NameGenSettingsCancelButton.UseVisualStyleBackColor = true;
            NameGenSettingsCancelButton.Click += NameGenSettingsCancelButton_Click;
            // 
            // NameGenSettingsApplyButton
            // 
            NameGenSettingsApplyButton.BackColor = SystemColors.ActiveCaption;
            NameGenSettingsApplyButton.Font = new Font("Tahoma", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            NameGenSettingsApplyButton.Location = new Point(456, 368);
            NameGenSettingsApplyButton.Name = "NameGenSettingsApplyButton";
            NameGenSettingsApplyButton.Size = new Size(89, 44);
            NameGenSettingsApplyButton.TabIndex = 11;
            NameGenSettingsApplyButton.Text = "&Apply";
            NameGenSettingsApplyButton.UseVisualStyleBackColor = false;
            NameGenSettingsApplyButton.Click += NameGenSettingsApplyButton_Click;
            // 
            // NameGeneratorCheckedList
            // 
            NameGeneratorCheckedList.FormattingEnabled = true;
            NameGeneratorCheckedList.Location = new Point(12, 69);
            NameGeneratorCheckedList.Name = "NameGeneratorCheckedList";
            NameGeneratorCheckedList.Size = new Size(200, 94);
            NameGeneratorCheckedList.TabIndex = 13;
            NameGeneratorCheckedList.ItemCheck += NameGeneratorCheckedList_ItemCheck;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 21);
            label1.Name = "label1";
            label1.Size = new Size(132, 19);
            label1.TabIndex = 14;
            label1.Text = "Name Generators";
            // 
            // NameBaseCheckedList
            // 
            NameBaseCheckedList.FormattingEnabled = true;
            NameBaseCheckedList.Location = new Point(12, 230);
            NameBaseCheckedList.Name = "NameBaseCheckedList";
            NameBaseCheckedList.Size = new Size(200, 184);
            NameBaseCheckedList.TabIndex = 15;
            NameBaseCheckedList.ItemCheck += NameBaseCheckedList_ItemCheck;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(13, 182);
            label2.Name = "label2";
            label2.Size = new Size(94, 19);
            label2.TabIndex = 16;
            label2.Text = "Name Bases";
            // 
            // SelectAllGeneratorsCheck
            // 
            SelectAllGeneratorsCheck.AutoSize = true;
            SelectAllGeneratorsCheck.Checked = true;
            SelectAllGeneratorsCheck.CheckState = CheckState.Checked;
            SelectAllGeneratorsCheck.Location = new Point(12, 43);
            SelectAllGeneratorsCheck.Name = "SelectAllGeneratorsCheck";
            SelectAllGeneratorsCheck.Size = new Size(79, 20);
            SelectAllGeneratorsCheck.TabIndex = 17;
            SelectAllGeneratorsCheck.Text = "Select All";
            SelectAllGeneratorsCheck.UseVisualStyleBackColor = true;
            SelectAllGeneratorsCheck.CheckedChanged += SelectAllGeneratorsCheck_CheckedChanged;
            // 
            // SelectAllNameBasesCheck
            // 
            SelectAllNameBasesCheck.AutoSize = true;
            SelectAllNameBasesCheck.Checked = true;
            SelectAllNameBasesCheck.CheckState = CheckState.Checked;
            SelectAllNameBasesCheck.Location = new Point(13, 204);
            SelectAllNameBasesCheck.Name = "SelectAllNameBasesCheck";
            SelectAllNameBasesCheck.Size = new Size(79, 20);
            SelectAllNameBasesCheck.TabIndex = 18;
            SelectAllNameBasesCheck.Text = "Select All";
            SelectAllNameBasesCheck.UseVisualStyleBackColor = true;
            SelectAllNameBasesCheck.CheckedChanged += SelectAllNameBasesCheck_CheckedChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(232, 20);
            label3.Name = "label3";
            label3.Size = new Size(84, 19);
            label3.TabIndex = 19;
            label3.Text = "Languages";
            // 
            // SelectAllLanguagesCheck
            // 
            SelectAllLanguagesCheck.AutoSize = true;
            SelectAllLanguagesCheck.Checked = true;
            SelectAllLanguagesCheck.CheckState = CheckState.Checked;
            SelectAllLanguagesCheck.Location = new Point(232, 42);
            SelectAllLanguagesCheck.Name = "SelectAllLanguagesCheck";
            SelectAllLanguagesCheck.Size = new Size(79, 20);
            SelectAllLanguagesCheck.TabIndex = 20;
            SelectAllLanguagesCheck.Text = "Select All";
            SelectAllLanguagesCheck.UseVisualStyleBackColor = true;
            SelectAllLanguagesCheck.CheckedChanged += SelectAllLanguagesCheck_CheckedChanged;
            // 
            // LanguagesCheckedList
            // 
            LanguagesCheckedList.FormattingEnabled = true;
            LanguagesCheckedList.Location = new Point(232, 68);
            LanguagesCheckedList.Name = "LanguagesCheckedList";
            LanguagesCheckedList.Size = new Size(200, 346);
            LanguagesCheckedList.Sorted = true;
            LanguagesCheckedList.TabIndex = 21;
            LanguagesCheckedList.ItemCheck += LanguagesCheckedList_ItemCheck;
            // 
            // GenerateNamesButton
            // 
            GenerateNamesButton.Font = new Font("Tahoma", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            GenerateNamesButton.Location = new Point(456, 68);
            GenerateNamesButton.Name = "GenerateNamesButton";
            GenerateNamesButton.Size = new Size(193, 44);
            GenerateNamesButton.TabIndex = 23;
            GenerateNamesButton.Text = "&Generate Names";
            GenerateNamesButton.UseVisualStyleBackColor = true;
            GenerateNamesButton.Click += GenerateNamesButton_Click;
            // 
            // GeneratedNamesList
            // 
            GeneratedNamesList.FormattingEnabled = true;
            GeneratedNamesList.Location = new Point(456, 118);
            GeneratedNamesList.Name = "GeneratedNamesList";
            GeneratedNamesList.Size = new Size(193, 244);
            GeneratedNamesList.TabIndex = 24;
            GeneratedNamesList.SelectedValueChanged += GeneratedNamesList_SelectedValueChanged;
            // 
            // NameGeneratorSettings
            // 
            AcceptButton = NameGenSettingsApplyButton;
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = NameGenSettingsCancelButton;
            ClientSize = new Size(661, 437);
            Controls.Add(GeneratedNamesList);
            Controls.Add(GenerateNamesButton);
            Controls.Add(LanguagesCheckedList);
            Controls.Add(SelectAllLanguagesCheck);
            Controls.Add(label3);
            Controls.Add(SelectAllNameBasesCheck);
            Controls.Add(SelectAllGeneratorsCheck);
            Controls.Add(label2);
            Controls.Add(NameBaseCheckedList);
            Controls.Add(label1);
            Controls.Add(NameGeneratorCheckedList);
            Controls.Add(NameGenSettingsCancelButton);
            Controls.Add(NameGenSettingsApplyButton);
            Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "NameGeneratorSettings";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Name Generator Settings";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button NameGenSettingsCancelButton;
        private Button NameGenSettingsApplyButton;
        private Label label1;
        private Label label2;
        private CheckBox SelectAllGeneratorsCheck;
        private CheckBox SelectAllNameBasesCheck;
        private Label label3;
        private CheckBox SelectAllLanguagesCheck;
        private Button GenerateNamesButton;
        internal CheckedListBox NameGeneratorCheckedList;
        internal CheckedListBox NameBaseCheckedList;
        internal CheckedListBox LanguagesCheckedList;
        internal ListBox GeneratedNamesList;
    }
}