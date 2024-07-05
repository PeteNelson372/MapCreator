namespace MapCreator
{
    partial class SymbolInfo
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
            CloseButton = new Button();
            NewTagTextBox = new TextBox();
            AddTagButton = new FontAwesome.Sharp.IconButton();
            CheckedTagsListBox = new CheckedListBox();
            SymbolNameLabel = new Label();
            label1 = new Label();
            label2 = new Label();
            CollectionNameLabel = new Label();
            label4 = new Label();
            SymbolPathLabel = new Label();
            label6 = new Label();
            SymbolFormatLabel = new Label();
            label8 = new Label();
            label10 = new Label();
            SymbolGuidLabel = new Label();
            OtherRadioButton = new RadioButton();
            TerrainRadioButton = new RadioButton();
            VegetationRadioButton = new RadioButton();
            StructureRadioButton = new RadioButton();
            panel1 = new Panel();
            UseCustomColorsRadio = new RadioButton();
            GrayScaleSymbolRadio = new RadioButton();
            ApplyButton = new FontAwesome.Sharp.IconButton();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // CloseButton
            // 
            CloseButton.Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            CloseButton.Location = new Point(477, 402);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new Size(95, 48);
            CloseButton.TabIndex = 0;
            CloseButton.Text = "&Close";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += CloseButton_Click;
            // 
            // NewTagTextBox
            // 
            NewTagTextBox.BorderStyle = BorderStyle.FixedSingle;
            NewTagTextBox.Location = new Point(109, 368);
            NewTagTextBox.Name = "NewTagTextBox";
            NewTagTextBox.Size = new Size(100, 23);
            NewTagTextBox.TabIndex = 27;
            // 
            // AddTagButton
            // 
            AddTagButton.IconChar = FontAwesome.Sharp.IconChar.Plus;
            AddTagButton.IconColor = Color.Black;
            AddTagButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            AddTagButton.IconSize = 18;
            AddTagButton.Location = new Point(215, 368);
            AddTagButton.Name = "AddTagButton";
            AddTagButton.Size = new Size(42, 23);
            AddTagButton.TabIndex = 26;
            AddTagButton.UseVisualStyleBackColor = true;
            AddTagButton.Click += AddTagButton_Click;
            // 
            // CheckedTagsListBox
            // 
            CheckedTagsListBox.BorderStyle = BorderStyle.FixedSingle;
            CheckedTagsListBox.CheckOnClick = true;
            CheckedTagsListBox.Location = new Point(109, 268);
            CheckedTagsListBox.Name = "CheckedTagsListBox";
            CheckedTagsListBox.ScrollAlwaysVisible = true;
            CheckedTagsListBox.Size = new Size(148, 92);
            CheckedTagsListBox.Sorted = true;
            CheckedTagsListBox.TabIndex = 25;
            // 
            // SymbolNameLabel
            // 
            SymbolNameLabel.BorderStyle = BorderStyle.FixedSingle;
            SymbolNameLabel.Location = new Point(114, 9);
            SymbolNameLabel.Name = "SymbolNameLabel";
            SymbolNameLabel.Size = new Size(189, 23);
            SymbolNameLabel.TabIndex = 30;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(26, 10);
            label1.Name = "label1";
            label1.Size = new Size(82, 15);
            label1.TabIndex = 31;
            label1.Text = "Symbol Name";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 48);
            label2.Name = "label2";
            label2.Size = new Size(96, 15);
            label2.TabIndex = 33;
            label2.Text = "Collection Name";
            // 
            // CollectionNameLabel
            // 
            CollectionNameLabel.BorderStyle = BorderStyle.FixedSingle;
            CollectionNameLabel.Location = new Point(114, 47);
            CollectionNameLabel.Name = "CollectionNameLabel";
            CollectionNameLabel.Size = new Size(189, 23);
            CollectionNameLabel.TabIndex = 32;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(34, 85);
            label4.Name = "label4";
            label4.Size = new Size(74, 15);
            label4.TabIndex = 35;
            label4.Text = "Symbol Path";
            // 
            // SymbolPathLabel
            // 
            SymbolPathLabel.BorderStyle = BorderStyle.FixedSingle;
            SymbolPathLabel.Location = new Point(114, 84);
            SymbolPathLabel.Name = "SymbolPathLabel";
            SymbolPathLabel.Size = new Size(458, 23);
            SymbolPathLabel.TabIndex = 34;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(20, 158);
            label6.Name = "label6";
            label6.Size = new Size(88, 15);
            label6.TabIndex = 37;
            label6.Text = "Symbol Format";
            // 
            // SymbolFormatLabel
            // 
            SymbolFormatLabel.BorderStyle = BorderStyle.FixedSingle;
            SymbolFormatLabel.Location = new Point(114, 157);
            SymbolFormatLabel.Name = "SymbolFormatLabel";
            SymbolFormatLabel.Size = new Size(60, 23);
            SymbolFormatLabel.TabIndex = 36;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(35, 194);
            label8.Name = "label8";
            label8.Size = new Size(74, 15);
            label8.TabIndex = 39;
            label8.Text = "Symbol Type";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(34, 122);
            label10.Name = "label10";
            label10.Size = new Size(75, 15);
            label10.TabIndex = 41;
            label10.Text = "Symbol Guid";
            // 
            // SymbolGuidLabel
            // 
            SymbolGuidLabel.BorderStyle = BorderStyle.FixedSingle;
            SymbolGuidLabel.Location = new Point(114, 121);
            SymbolGuidLabel.Name = "SymbolGuidLabel";
            SymbolGuidLabel.Size = new Size(189, 23);
            SymbolGuidLabel.TabIndex = 40;
            // 
            // OtherRadioButton
            // 
            OtherRadioButton.AutoSize = true;
            OtherRadioButton.Location = new Point(364, 194);
            OtherRadioButton.Name = "OtherRadioButton";
            OtherRadioButton.Size = new Size(55, 19);
            OtherRadioButton.TabIndex = 45;
            OtherRadioButton.Text = "Other";
            OtherRadioButton.UseVisualStyleBackColor = true;
            // 
            // TerrainRadioButton
            // 
            TerrainRadioButton.AutoSize = true;
            TerrainRadioButton.Location = new Point(291, 194);
            TerrainRadioButton.Name = "TerrainRadioButton";
            TerrainRadioButton.Size = new Size(60, 19);
            TerrainRadioButton.TabIndex = 44;
            TerrainRadioButton.Text = "Terrain";
            TerrainRadioButton.UseVisualStyleBackColor = true;
            // 
            // VegetationRadioButton
            // 
            VegetationRadioButton.AutoSize = true;
            VegetationRadioButton.Location = new Point(199, 194);
            VegetationRadioButton.Name = "VegetationRadioButton";
            VegetationRadioButton.Size = new Size(81, 19);
            VegetationRadioButton.TabIndex = 43;
            VegetationRadioButton.Text = "Vegetation";
            VegetationRadioButton.UseVisualStyleBackColor = true;
            // 
            // StructureRadioButton
            // 
            StructureRadioButton.AutoSize = true;
            StructureRadioButton.Location = new Point(115, 194);
            StructureRadioButton.Name = "StructureRadioButton";
            StructureRadioButton.Size = new Size(73, 19);
            StructureRadioButton.TabIndex = 42;
            StructureRadioButton.Text = "Structure";
            StructureRadioButton.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(UseCustomColorsRadio);
            panel1.Controls.Add(GrayScaleSymbolRadio);
            panel1.Location = new Point(109, 219);
            panel1.Name = "panel1";
            panel1.Size = new Size(317, 33);
            panel1.TabIndex = 46;
            // 
            // UseCustomColorsRadio
            // 
            UseCustomColorsRadio.AutoSize = true;
            UseCustomColorsRadio.Enabled = false;
            UseCustomColorsRadio.Location = new Point(143, 8);
            UseCustomColorsRadio.Name = "UseCustomColorsRadio";
            UseCustomColorsRadio.Size = new Size(160, 19);
            UseCustomColorsRadio.TabIndex = 41;
            UseCustomColorsRadio.TabStop = true;
            UseCustomColorsRadio.Text = "Paint with Custom Colors";
            UseCustomColorsRadio.UseVisualStyleBackColor = true;
            // 
            // GrayScaleSymbolRadio
            // 
            GrayScaleSymbolRadio.AutoSize = true;
            GrayScaleSymbolRadio.Enabled = false;
            GrayScaleSymbolRadio.Location = new Point(6, 8);
            GrayScaleSymbolRadio.Name = "GrayScaleSymbolRadio";
            GrayScaleSymbolRadio.Size = new Size(118, 19);
            GrayScaleSymbolRadio.TabIndex = 40;
            GrayScaleSymbolRadio.TabStop = true;
            GrayScaleSymbolRadio.Text = "Grayscale Symbol";
            GrayScaleSymbolRadio.UseVisualStyleBackColor = true;
            // 
            // ApplyButton
            // 
            ApplyButton.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ApplyButton.IconChar = FontAwesome.Sharp.IconChar.Check;
            ApplyButton.IconColor = Color.Black;
            ApplyButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            ApplyButton.Location = new Point(263, 268);
            ApplyButton.Name = "ApplyButton";
            ApplyButton.Size = new Size(75, 94);
            ApplyButton.TabIndex = 47;
            ApplyButton.Text = "Apply";
            ApplyButton.TextImageRelation = TextImageRelation.TextAboveImage;
            ApplyButton.UseVisualStyleBackColor = true;
            ApplyButton.Click += ApplyButton_Click;
            // 
            // SymbolInfo
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = CloseButton;
            ClientSize = new Size(584, 462);
            ControlBox = false;
            Controls.Add(ApplyButton);
            Controls.Add(panel1);
            Controls.Add(OtherRadioButton);
            Controls.Add(TerrainRadioButton);
            Controls.Add(VegetationRadioButton);
            Controls.Add(StructureRadioButton);
            Controls.Add(label10);
            Controls.Add(SymbolGuidLabel);
            Controls.Add(label8);
            Controls.Add(label6);
            Controls.Add(SymbolFormatLabel);
            Controls.Add(label4);
            Controls.Add(SymbolPathLabel);
            Controls.Add(label2);
            Controls.Add(CollectionNameLabel);
            Controls.Add(label1);
            Controls.Add(SymbolNameLabel);
            Controls.Add(NewTagTextBox);
            Controls.Add(AddTagButton);
            Controls.Add(CheckedTagsListBox);
            Controls.Add(CloseButton);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "SymbolInfo";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Symbol Information";
            TopMost = true;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button CloseButton;
        private TextBox NewTagTextBox;
        private FontAwesome.Sharp.IconButton AddTagButton;
        private CheckedListBox CheckedTagsListBox;
        private Label SymbolNameLabel;
        private Label label1;
        private Label label2;
        private Label CollectionNameLabel;
        private Label label4;
        private Label SymbolPathLabel;
        private Label label6;
        private Label SymbolFormatLabel;
        private Label label8;
        private Label label10;
        private Label SymbolGuidLabel;
        private RadioButton OtherRadioButton;
        private RadioButton TerrainRadioButton;
        private RadioButton VegetationRadioButton;
        private RadioButton StructureRadioButton;
        private Panel panel1;
        private RadioButton UseCustomColorsRadio;
        private RadioButton GrayScaleSymbolRadio;
        private FontAwesome.Sharp.IconButton ApplyButton;
    }
}