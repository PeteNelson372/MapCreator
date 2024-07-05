namespace MapCreator
{
    partial class SymbolCollectionForm
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
            label1 = new Label();
            CollectionPathTextBox = new TextBox();
            OpenCollectionDirectoryButton = new Button();
            label2 = new Label();
            CollectionNameTextBox = new TextBox();
            label3 = new Label();
            LicenseComboBox = new ComboBox();
            SymbolPictureBox = new PictureBox();
            label6 = new Label();
            SymbolNumberLabel = new Label();
            label7 = new Label();
            CollectionSizeLabel = new Label();
            NextSymbolButton = new FontAwesome.Sharp.IconButton();
            PreviousSymbolButton = new FontAwesome.Sharp.IconButton();
            LastSymbolButton = new FontAwesome.Sharp.IconButton();
            FirstSymbolButton = new FontAwesome.Sharp.IconButton();
            StructureRadioButton = new RadioButton();
            VegetationRadioButton = new RadioButton();
            TerrainRadioButton = new RadioButton();
            CheckedTagsListBox = new CheckedListBox();
            TagSearchTextBox = new TextBox();
            AddTagButton = new FontAwesome.Sharp.IconButton();
            NewTagTextBox = new TextBox();
            ApplyButton = new FontAwesome.Sharp.IconButton();
            CloseButton = new Button();
            label8 = new Label();
            SaveButton = new Button();
            label9 = new Label();
            NumberSymbolsTaggedLabel = new Label();
            AutoAdvanceCheck = new CheckBox();
            ResetTagsButton = new FontAwesome.Sharp.IconButton();
            OtherRadioButton = new RadioButton();
            SymbolNameTextBox = new TextBox();
            label10 = new Label();
            ClearAllChecksButton = new Button();
            panel1 = new Panel();
            UseCustomColorsRadio = new RadioButton();
            GrayScaleSymbolRadio = new RadioButton();
            AddToAllButton = new Button();
            ((System.ComponentModel.ISupportInitialize)SymbolPictureBox).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(110, 21);
            label1.Name = "label1";
            label1.Size = new Size(32, 14);
            label1.TabIndex = 0;
            label1.Text = "Path";
            // 
            // CollectionPathTextBox
            // 
            CollectionPathTextBox.BorderStyle = BorderStyle.FixedSingle;
            CollectionPathTextBox.Location = new Point(148, 17);
            CollectionPathTextBox.Name = "CollectionPathTextBox";
            CollectionPathTextBox.Size = new Size(519, 23);
            CollectionPathTextBox.TabIndex = 1;
            // 
            // OpenCollectionDirectoryButton
            // 
            OpenCollectionDirectoryButton.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            OpenCollectionDirectoryButton.Location = new Point(673, 16);
            OpenCollectionDirectoryButton.Name = "OpenCollectionDirectoryButton";
            OpenCollectionDirectoryButton.Size = new Size(75, 25);
            OpenCollectionDirectoryButton.TabIndex = 2;
            OpenCollectionDirectoryButton.Text = "Open";
            OpenCollectionDirectoryButton.UseVisualStyleBackColor = true;
            OpenCollectionDirectoryButton.Click += OpenCollectionDirectoryButton_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(101, 56);
            label2.Name = "label2";
            label2.Size = new Size(38, 14);
            label2.TabIndex = 3;
            label2.Text = "Name";
            // 
            // CollectionNameTextBox
            // 
            CollectionNameTextBox.BorderStyle = BorderStyle.FixedSingle;
            CollectionNameTextBox.Location = new Point(148, 53);
            CollectionNameTextBox.Name = "CollectionNameTextBox";
            CollectionNameTextBox.Size = new Size(249, 23);
            CollectionNameTextBox.TabIndex = 4;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(95, 102);
            label3.Name = "label3";
            label3.Size = new Size(47, 14);
            label3.TabIndex = 5;
            label3.Text = "License";
            // 
            // LicenseComboBox
            // 
            LicenseComboBox.Font = new Font("Cascadia Mono", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LicenseComboBox.FormattingEnabled = true;
            LicenseComboBox.Items.AddRange(new object[] { "CAL-NR:             No Restrictions", "CAL-BY:             Attribution Required", "CAL-BY-NRB:         Attribution required, commercial use allowed, no reselling of base items", "CAL-BY-NCR:         Attribution required, no reselling", "CAL-BY-NC:          Attribution required, non-commercial", "CAL-BY-NC-NS:       Attribution required, non-commercial, modifications cannot be shared", "CAL-BY-AS:          Attribution required, non-commercial, no modifications", "CAL-NA:             No Attribution", "CAL-NA-NRB:         No Attribution, no reselling of base items", "CAL-NA-NCR:         No Attribution, no reselling", "CAL-NA-NC:          No Attribution, non-commercial", "CAL-NA-NC-NS:       No Attribution, non-commercial, modifications cannot be shared", "CAL-NA-NCR-NS:      No Attribution, commercial use allowed (no reselling), modifications cannot be shared", "CAL-NA-AS:          No Attribution, non-commercial, no modifications" });
            LicenseComboBox.Location = new Point(148, 99);
            LicenseComboBox.Name = "LicenseComboBox";
            LicenseComboBox.Size = new Size(519, 23);
            LicenseComboBox.TabIndex = 6;
            // 
            // SymbolPictureBox
            // 
            SymbolPictureBox.BorderStyle = BorderStyle.FixedSingle;
            SymbolPictureBox.Location = new Point(199, 175);
            SymbolPictureBox.Name = "SymbolPictureBox";
            SymbolPictureBox.Size = new Size(96, 96);
            SymbolPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            SymbolPictureBox.TabIndex = 9;
            SymbolPictureBox.TabStop = false;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(145, 158);
            label6.Name = "label6";
            label6.Size = new Size(46, 14);
            label6.TabIndex = 10;
            label6.Text = "Symbol";
            // 
            // SymbolNumberLabel
            // 
            SymbolNumberLabel.Location = new Point(188, 157);
            SymbolNumberLabel.Name = "SymbolNumberLabel";
            SymbolNumberLabel.Size = new Size(35, 16);
            SymbolNumberLabel.TabIndex = 11;
            SymbolNumberLabel.Text = "1";
            SymbolNumberLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(229, 158);
            label7.Name = "label7";
            label7.Size = new Size(18, 16);
            label7.TabIndex = 12;
            label7.Text = "of";
            // 
            // CollectionSizeLabel
            // 
            CollectionSizeLabel.AutoSize = true;
            CollectionSizeLabel.Location = new Point(253, 158);
            CollectionSizeLabel.Name = "CollectionSizeLabel";
            CollectionSizeLabel.Size = new Size(35, 16);
            CollectionSizeLabel.TabIndex = 13;
            CollectionSizeLabel.Text = "9999";
            CollectionSizeLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // NextSymbolButton
            // 
            NextSymbolButton.IconChar = FontAwesome.Sharp.IconChar.Forward;
            NextSymbolButton.IconColor = Color.Black;
            NextSymbolButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            NextSymbolButton.IconSize = 32;
            NextSymbolButton.Location = new Point(301, 177);
            NextSymbolButton.Name = "NextSymbolButton";
            NextSymbolButton.Size = new Size(45, 94);
            NextSymbolButton.TabIndex = 14;
            NextSymbolButton.UseVisualStyleBackColor = true;
            NextSymbolButton.Click += NextSymbolButton_Click;
            // 
            // PreviousSymbolButton
            // 
            PreviousSymbolButton.IconChar = FontAwesome.Sharp.IconChar.Backward;
            PreviousSymbolButton.IconColor = Color.Black;
            PreviousSymbolButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            PreviousSymbolButton.IconSize = 32;
            PreviousSymbolButton.Location = new Point(148, 175);
            PreviousSymbolButton.Name = "PreviousSymbolButton";
            PreviousSymbolButton.Size = new Size(45, 94);
            PreviousSymbolButton.TabIndex = 15;
            PreviousSymbolButton.UseVisualStyleBackColor = true;
            PreviousSymbolButton.Click += PreviousSymbolButton_Click;
            // 
            // LastSymbolButton
            // 
            LastSymbolButton.IconChar = FontAwesome.Sharp.IconChar.FastForward;
            LastSymbolButton.IconColor = Color.Black;
            LastSymbolButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            LastSymbolButton.IconSize = 32;
            LastSymbolButton.Location = new Point(352, 177);
            LastSymbolButton.Name = "LastSymbolButton";
            LastSymbolButton.Size = new Size(45, 94);
            LastSymbolButton.TabIndex = 16;
            LastSymbolButton.UseVisualStyleBackColor = true;
            LastSymbolButton.Click += LastSymbolButton_Click;
            // 
            // FirstSymbolButton
            // 
            FirstSymbolButton.IconChar = FontAwesome.Sharp.IconChar.FastBackward;
            FirstSymbolButton.IconColor = Color.Black;
            FirstSymbolButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            FirstSymbolButton.IconSize = 32;
            FirstSymbolButton.Location = new Point(97, 176);
            FirstSymbolButton.Name = "FirstSymbolButton";
            FirstSymbolButton.Size = new Size(45, 94);
            FirstSymbolButton.TabIndex = 17;
            FirstSymbolButton.UseVisualStyleBackColor = true;
            FirstSymbolButton.Click += FirstSymbolButton_Click;
            // 
            // StructureRadioButton
            // 
            StructureRadioButton.AutoSize = true;
            StructureRadioButton.Location = new Point(101, 277);
            StructureRadioButton.Name = "StructureRadioButton";
            StructureRadioButton.Size = new Size(78, 20);
            StructureRadioButton.TabIndex = 18;
            StructureRadioButton.Text = "Structure";
            StructureRadioButton.UseVisualStyleBackColor = true;
            // 
            // VegetationRadioButton
            // 
            VegetationRadioButton.AutoSize = true;
            VegetationRadioButton.Location = new Point(185, 277);
            VegetationRadioButton.Name = "VegetationRadioButton";
            VegetationRadioButton.Size = new Size(86, 20);
            VegetationRadioButton.TabIndex = 19;
            VegetationRadioButton.Text = "Vegetation";
            VegetationRadioButton.UseVisualStyleBackColor = true;
            // 
            // TerrainRadioButton
            // 
            TerrainRadioButton.AutoSize = true;
            TerrainRadioButton.Location = new Point(277, 277);
            TerrainRadioButton.Name = "TerrainRadioButton";
            TerrainRadioButton.Size = new Size(67, 20);
            TerrainRadioButton.TabIndex = 20;
            TerrainRadioButton.Text = "Terrain";
            TerrainRadioButton.UseVisualStyleBackColor = true;
            // 
            // CheckedTagsListBox
            // 
            CheckedTagsListBox.BorderStyle = BorderStyle.FixedSingle;
            CheckedTagsListBox.CheckOnClick = true;
            CheckedTagsListBox.Location = new Point(438, 206);
            CheckedTagsListBox.Name = "CheckedTagsListBox";
            CheckedTagsListBox.ScrollAlwaysVisible = true;
            CheckedTagsListBox.Size = new Size(148, 92);
            CheckedTagsListBox.Sorted = true;
            CheckedTagsListBox.TabIndex = 21;
            // 
            // TagSearchTextBox
            // 
            TagSearchTextBox.BorderStyle = BorderStyle.FixedSingle;
            TagSearchTextBox.Location = new Point(438, 177);
            TagSearchTextBox.Name = "TagSearchTextBox";
            TagSearchTextBox.Size = new Size(148, 23);
            TagSearchTextBox.TabIndex = 22;
            TagSearchTextBox.KeyUp += TagSearchTextBox_KeyUp;
            // 
            // AddTagButton
            // 
            AddTagButton.IconChar = FontAwesome.Sharp.IconChar.Plus;
            AddTagButton.IconColor = Color.Black;
            AddTagButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            AddTagButton.IconSize = 18;
            AddTagButton.Location = new Point(544, 306);
            AddTagButton.Name = "AddTagButton";
            AddTagButton.Size = new Size(42, 23);
            AddTagButton.TabIndex = 23;
            AddTagButton.UseVisualStyleBackColor = true;
            AddTagButton.Click += AddTagButton_Click;
            // 
            // NewTagTextBox
            // 
            NewTagTextBox.BorderStyle = BorderStyle.FixedSingle;
            NewTagTextBox.Location = new Point(438, 306);
            NewTagTextBox.Name = "NewTagTextBox";
            NewTagTextBox.Size = new Size(100, 23);
            NewTagTextBox.TabIndex = 24;
            // 
            // ApplyButton
            // 
            ApplyButton.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ApplyButton.IconChar = FontAwesome.Sharp.IconChar.Check;
            ApplyButton.IconColor = Color.Black;
            ApplyButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            ApplyButton.Location = new Point(592, 205);
            ApplyButton.Name = "ApplyButton";
            ApplyButton.Size = new Size(75, 94);
            ApplyButton.TabIndex = 25;
            ApplyButton.Text = "Apply";
            ApplyButton.TextImageRelation = TextImageRelation.TextAboveImage;
            ApplyButton.UseVisualStyleBackColor = true;
            ApplyButton.Click += ApplyButton_Click;
            // 
            // CloseButton
            // 
            CloseButton.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            CloseButton.Location = new Point(728, 455);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new Size(60, 60);
            CloseButton.TabIndex = 26;
            CloseButton.Text = "&Close";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += CloseButton_Click;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(438, 158);
            label8.Name = "label8";
            label8.Size = new Size(35, 16);
            label8.TabIndex = 27;
            label8.Text = "Tags";
            // 
            // SaveButton
            // 
            SaveButton.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            SaveButton.Location = new Point(662, 455);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(60, 60);
            SaveButton.TabIndex = 28;
            SaveButton.Text = "&Save";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(301, 158);
            label9.Name = "label9";
            label9.Size = new Size(59, 16);
            label9.TabIndex = 29;
            label9.Text = "Tagged: ";
            // 
            // NumberSymbolsTaggedLabel
            // 
            NumberSymbolsTaggedLabel.Location = new Point(352, 158);
            NumberSymbolsTaggedLabel.Name = "NumberSymbolsTaggedLabel";
            NumberSymbolsTaggedLabel.Size = new Size(44, 15);
            NumberSymbolsTaggedLabel.TabIndex = 30;
            NumberSymbolsTaggedLabel.Text = "0";
            NumberSymbolsTaggedLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // AutoAdvanceCheck
            // 
            AutoAdvanceCheck.AutoSize = true;
            AutoAdvanceCheck.Checked = true;
            AutoAdvanceCheck.CheckState = CheckState.Checked;
            AutoAdvanceCheck.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            AutoAdvanceCheck.Location = new Point(592, 306);
            AutoAdvanceCheck.Name = "AutoAdvanceCheck";
            AutoAdvanceCheck.Size = new Size(113, 17);
            AutoAdvanceCheck.TabIndex = 31;
            AutoAdvanceCheck.Text = "Advance on Apply";
            AutoAdvanceCheck.UseVisualStyleBackColor = true;
            // 
            // ResetTagsButton
            // 
            ResetTagsButton.IconChar = FontAwesome.Sharp.IconChar.RotateBack;
            ResetTagsButton.IconColor = Color.Black;
            ResetTagsButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            ResetTagsButton.IconSize = 12;
            ResetTagsButton.Location = new Point(592, 176);
            ResetTagsButton.Name = "ResetTagsButton";
            ResetTagsButton.Size = new Size(75, 23);
            ResetTagsButton.TabIndex = 33;
            ResetTagsButton.Text = "Reset";
            ResetTagsButton.TextImageRelation = TextImageRelation.ImageBeforeText;
            ResetTagsButton.UseVisualStyleBackColor = true;
            ResetTagsButton.Click += ResetTagsButton_Click;
            // 
            // OtherRadioButton
            // 
            OtherRadioButton.AutoSize = true;
            OtherRadioButton.Location = new Point(350, 277);
            OtherRadioButton.Name = "OtherRadioButton";
            OtherRadioButton.Size = new Size(57, 20);
            OtherRadioButton.TabIndex = 34;
            OtherRadioButton.Text = "Other";
            OtherRadioButton.UseVisualStyleBackColor = true;
            // 
            // SymbolNameTextBox
            // 
            SymbolNameTextBox.Location = new Point(199, 306);
            SymbolNameTextBox.Name = "SymbolNameTextBox";
            SymbolNameTextBox.Size = new Size(198, 23);
            SymbolNameTextBox.TabIndex = 35;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(107, 309);
            label10.Name = "label10";
            label10.Size = new Size(86, 16);
            label10.TabIndex = 36;
            label10.Text = "Symbol Name";
            // 
            // ClearAllChecksButton
            // 
            ClearAllChecksButton.Location = new Point(438, 335);
            ClearAllChecksButton.Name = "ClearAllChecksButton";
            ClearAllChecksButton.Size = new Size(148, 23);
            ClearAllChecksButton.TabIndex = 37;
            ClearAllChecksButton.Text = "Clear All Checked Tags";
            ClearAllChecksButton.UseVisualStyleBackColor = true;
            ClearAllChecksButton.Click += ClearAllChecksButton_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(UseCustomColorsRadio);
            panel1.Controls.Add(GrayScaleSymbolRadio);
            panel1.Location = new Point(108, 328);
            panel1.Name = "panel1";
            panel1.Size = new Size(317, 33);
            panel1.TabIndex = 38;
            // 
            // UseCustomColorsRadio
            // 
            UseCustomColorsRadio.AutoSize = true;
            UseCustomColorsRadio.Location = new Point(143, 8);
            UseCustomColorsRadio.Name = "UseCustomColorsRadio";
            UseCustomColorsRadio.Size = new Size(168, 20);
            UseCustomColorsRadio.TabIndex = 41;
            UseCustomColorsRadio.TabStop = true;
            UseCustomColorsRadio.Text = "Paint with Custom Colors";
            UseCustomColorsRadio.UseVisualStyleBackColor = true;
            // 
            // GrayScaleSymbolRadio
            // 
            GrayScaleSymbolRadio.AutoSize = true;
            GrayScaleSymbolRadio.Location = new Point(6, 8);
            GrayScaleSymbolRadio.Name = "GrayScaleSymbolRadio";
            GrayScaleSymbolRadio.Size = new Size(126, 20);
            GrayScaleSymbolRadio.TabIndex = 40;
            GrayScaleSymbolRadio.TabStop = true;
            GrayScaleSymbolRadio.Text = "Grayscale Symbol";
            GrayScaleSymbolRadio.UseVisualStyleBackColor = true;
            // 
            // AddToAllButton
            // 
            AddToAllButton.Location = new Point(438, 364);
            AddToAllButton.Name = "AddToAllButton";
            AddToAllButton.Size = new Size(148, 23);
            AddToAllButton.TabIndex = 39;
            AddToAllButton.Text = "Add Checked To All";
            AddToAllButton.UseVisualStyleBackColor = true;
            AddToAllButton.Click += AddToAllButton_Click;
            // 
            // SymbolCollectionForm
            // 
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = CloseButton;
            ClientSize = new Size(800, 527);
            ControlBox = false;
            Controls.Add(AddToAllButton);
            Controls.Add(panel1);
            Controls.Add(ClearAllChecksButton);
            Controls.Add(label10);
            Controls.Add(SymbolNameTextBox);
            Controls.Add(OtherRadioButton);
            Controls.Add(ResetTagsButton);
            Controls.Add(AutoAdvanceCheck);
            Controls.Add(NumberSymbolsTaggedLabel);
            Controls.Add(label9);
            Controls.Add(SaveButton);
            Controls.Add(label8);
            Controls.Add(CloseButton);
            Controls.Add(ApplyButton);
            Controls.Add(NewTagTextBox);
            Controls.Add(AddTagButton);
            Controls.Add(TagSearchTextBox);
            Controls.Add(CheckedTagsListBox);
            Controls.Add(TerrainRadioButton);
            Controls.Add(VegetationRadioButton);
            Controls.Add(StructureRadioButton);
            Controls.Add(FirstSymbolButton);
            Controls.Add(LastSymbolButton);
            Controls.Add(PreviousSymbolButton);
            Controls.Add(NextSymbolButton);
            Controls.Add(CollectionSizeLabel);
            Controls.Add(label7);
            Controls.Add(SymbolNumberLabel);
            Controls.Add(label6);
            Controls.Add(SymbolPictureBox);
            Controls.Add(LicenseComboBox);
            Controls.Add(label3);
            Controls.Add(CollectionNameTextBox);
            Controls.Add(label2);
            Controls.Add(OpenCollectionDirectoryButton);
            Controls.Add(CollectionPathTextBox);
            Controls.Add(label1);
            Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "SymbolCollectionForm";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Add/Update Symbol Collection";
            Load += SymbolCollectionForm_Load;
            ((System.ComponentModel.ISupportInitialize)SymbolPictureBox).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox CollectionPathTextBox;
        private Button OpenCollectionDirectoryButton;
        private Label label2;
        private TextBox CollectionNameTextBox;
        private Label label3;
        private ComboBox LicenseComboBox;
        private PictureBox SymbolPictureBox;
        private Label label6;
        private Label SymbolNumberLabel;
        private Label label7;
        private Label CollectionSizeLabel;
        private FontAwesome.Sharp.IconButton NextSymbolButton;
        private FontAwesome.Sharp.IconButton PreviousSymbolButton;
        private FontAwesome.Sharp.IconButton LastSymbolButton;
        private FontAwesome.Sharp.IconButton FirstSymbolButton;
        private RadioButton StructureRadioButton;
        private RadioButton VegetationRadioButton;
        private RadioButton TerrainRadioButton;
        private CheckedListBox CheckedTagsListBox;
        private TextBox TagSearchTextBox;
        private FontAwesome.Sharp.IconButton AddTagButton;
        private TextBox NewTagTextBox;
        private FontAwesome.Sharp.IconButton ApplyButton;
        private Button CloseButton;
        private Label label8;
        private Button SaveButton;
        private Label label9;
        private Label NumberSymbolsTaggedLabel;
        private CheckBox AutoAdvanceCheck;
        private FontAwesome.Sharp.IconButton ResetTagsButton;
        private RadioButton OtherRadioButton;
        private TextBox SymbolNameTextBox;
        private Label label10;
        private Button ClearAllChecksButton;
        private Panel panel1;
        private RadioButton UseCustomColorsRadio;
        private RadioButton GrayScaleSymbolRadio;
        private Button AddToAllButton;
    }
}