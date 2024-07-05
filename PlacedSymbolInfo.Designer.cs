namespace MapCreator
{
    partial class PlacedSymbolInfo
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
            panel1 = new Panel();
            UseCustomColorsRadio = new RadioButton();
            GrayScaleSymbolRadio = new RadioButton();
            OtherRadioButton = new RadioButton();
            TerrainRadioButton = new RadioButton();
            VegetationRadioButton = new RadioButton();
            StructureRadioButton = new RadioButton();
            label10 = new Label();
            SymbolGuidLabel = new Label();
            label8 = new Label();
            label6 = new Label();
            SymbolFormatLabel = new Label();
            label2 = new Label();
            CollectionNameLabel = new Label();
            label1 = new Label();
            SymbolNameLabel = new Label();
            CheckedTagsListBox = new CheckedListBox();
            ResetSymbolColorsButton = new FontAwesome.Sharp.IconButton();
            ColorSymbolsButton = new FontAwesome.Sharp.IconButton();
            SymbolColor4Label = new Label();
            SymbolColor3Label = new Label();
            SymbolColor2Label = new Label();
            SymbolColor1Label = new Label();
            label4 = new Label();
            SymbolPathLabel = new Label();
            label3 = new Label();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // CloseButton
            // 
            CloseButton.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            CloseButton.Location = new Point(477, 317);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new Size(95, 48);
            CloseButton.TabIndex = 27;
            CloseButton.Text = "&Close";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += CloseButton_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(UseCustomColorsRadio);
            panel1.Controls.Add(GrayScaleSymbolRadio);
            panel1.Location = new Point(107, 229);
            panel1.Name = "panel1";
            panel1.Size = new Size(317, 33);
            panel1.TabIndex = 66;
            // 
            // UseCustomColorsRadio
            // 
            UseCustomColorsRadio.AutoSize = true;
            UseCustomColorsRadio.Enabled = false;
            UseCustomColorsRadio.Location = new Point(143, 8);
            UseCustomColorsRadio.Name = "UseCustomColorsRadio";
            UseCustomColorsRadio.Size = new Size(161, 18);
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
            GrayScaleSymbolRadio.Size = new Size(118, 18);
            GrayScaleSymbolRadio.TabIndex = 40;
            GrayScaleSymbolRadio.TabStop = true;
            GrayScaleSymbolRadio.Text = "Grayscale Symbol";
            GrayScaleSymbolRadio.UseVisualStyleBackColor = true;
            // 
            // OtherRadioButton
            // 
            OtherRadioButton.AutoSize = true;
            OtherRadioButton.Enabled = false;
            OtherRadioButton.Location = new Point(362, 204);
            OtherRadioButton.Name = "OtherRadioButton";
            OtherRadioButton.Size = new Size(57, 18);
            OtherRadioButton.TabIndex = 65;
            OtherRadioButton.Text = "Other";
            OtherRadioButton.UseVisualStyleBackColor = true;
            // 
            // TerrainRadioButton
            // 
            TerrainRadioButton.AutoSize = true;
            TerrainRadioButton.Enabled = false;
            TerrainRadioButton.Location = new Point(289, 204);
            TerrainRadioButton.Name = "TerrainRadioButton";
            TerrainRadioButton.Size = new Size(63, 18);
            TerrainRadioButton.TabIndex = 64;
            TerrainRadioButton.Text = "Terrain";
            TerrainRadioButton.UseVisualStyleBackColor = true;
            // 
            // VegetationRadioButton
            // 
            VegetationRadioButton.AutoSize = true;
            VegetationRadioButton.Enabled = false;
            VegetationRadioButton.Location = new Point(197, 204);
            VegetationRadioButton.Name = "VegetationRadioButton";
            VegetationRadioButton.Size = new Size(86, 18);
            VegetationRadioButton.TabIndex = 63;
            VegetationRadioButton.Text = "Vegetation";
            VegetationRadioButton.UseVisualStyleBackColor = true;
            // 
            // StructureRadioButton
            // 
            StructureRadioButton.AutoSize = true;
            StructureRadioButton.Enabled = false;
            StructureRadioButton.Location = new Point(113, 204);
            StructureRadioButton.Name = "StructureRadioButton";
            StructureRadioButton.Size = new Size(77, 18);
            StructureRadioButton.TabIndex = 62;
            StructureRadioButton.Text = "Structure";
            StructureRadioButton.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(32, 132);
            label10.Name = "label10";
            label10.Size = new Size(74, 14);
            label10.TabIndex = 61;
            label10.Text = "Symbol Guid";
            // 
            // SymbolGuidLabel
            // 
            SymbolGuidLabel.BorderStyle = BorderStyle.FixedSingle;
            SymbolGuidLabel.Location = new Point(112, 131);
            SymbolGuidLabel.Name = "SymbolGuidLabel";
            SymbolGuidLabel.Size = new Size(189, 23);
            SymbolGuidLabel.TabIndex = 60;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(33, 204);
            label8.Name = "label8";
            label8.Size = new Size(78, 14);
            label8.TabIndex = 59;
            label8.Text = "Symbol Type";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(18, 168);
            label6.Name = "label6";
            label6.Size = new Size(88, 14);
            label6.TabIndex = 58;
            label6.Text = "Symbol Format";
            // 
            // SymbolFormatLabel
            // 
            SymbolFormatLabel.BorderStyle = BorderStyle.FixedSingle;
            SymbolFormatLabel.Location = new Point(112, 167);
            SymbolFormatLabel.Name = "SymbolFormatLabel";
            SymbolFormatLabel.Size = new Size(60, 23);
            SymbolFormatLabel.TabIndex = 57;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(10, 92);
            label2.Name = "label2";
            label2.Size = new Size(94, 14);
            label2.TabIndex = 54;
            label2.Text = "Collection Name";
            // 
            // CollectionNameLabel
            // 
            CollectionNameLabel.BorderStyle = BorderStyle.FixedSingle;
            CollectionNameLabel.Location = new Point(112, 91);
            CollectionNameLabel.Name = "CollectionNameLabel";
            CollectionNameLabel.Size = new Size(189, 23);
            CollectionNameLabel.TabIndex = 53;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(24, 54);
            label1.Name = "label1";
            label1.Size = new Size(81, 14);
            label1.TabIndex = 52;
            label1.Text = "Symbol Name";
            // 
            // SymbolNameLabel
            // 
            SymbolNameLabel.BorderStyle = BorderStyle.FixedSingle;
            SymbolNameLabel.Location = new Point(112, 53);
            SymbolNameLabel.Name = "SymbolNameLabel";
            SymbolNameLabel.Size = new Size(189, 23);
            SymbolNameLabel.TabIndex = 51;
            // 
            // CheckedTagsListBox
            // 
            CheckedTagsListBox.BorderStyle = BorderStyle.FixedSingle;
            CheckedTagsListBox.CheckOnClick = true;
            CheckedTagsListBox.Location = new Point(107, 278);
            CheckedTagsListBox.Name = "CheckedTagsListBox";
            CheckedTagsListBox.ScrollAlwaysVisible = true;
            CheckedTagsListBox.Size = new Size(148, 87);
            CheckedTagsListBox.Sorted = true;
            CheckedTagsListBox.TabIndex = 48;
            // 
            // ResetSymbolColorsButton
            // 
            ResetSymbolColorsButton.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ResetSymbolColorsButton.IconChar = FontAwesome.Sharp.IconChar.RotateBack;
            ResetSymbolColorsButton.IconColor = Color.Black;
            ResetSymbolColorsButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            ResetSymbolColorsButton.IconSize = 24;
            ResetSymbolColorsButton.Location = new Point(444, 83);
            ResetSymbolColorsButton.Name = "ResetSymbolColorsButton";
            ResetSymbolColorsButton.Size = new Size(60, 60);
            ResetSymbolColorsButton.TabIndex = 87;
            ResetSymbolColorsButton.Text = "Reset";
            ResetSymbolColorsButton.TextImageRelation = TextImageRelation.TextAboveImage;
            ResetSymbolColorsButton.UseVisualStyleBackColor = true;
            ResetSymbolColorsButton.Click += ResetSymbolColorsButton_Click;
            // 
            // ColorSymbolsButton
            // 
            ColorSymbolsButton.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ColorSymbolsButton.IconChar = FontAwesome.Sharp.IconChar.Brush;
            ColorSymbolsButton.IconColor = Color.Black;
            ColorSymbolsButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            ColorSymbolsButton.IconSize = 24;
            ColorSymbolsButton.Location = new Point(510, 83);
            ColorSymbolsButton.Name = "ColorSymbolsButton";
            ColorSymbolsButton.Size = new Size(60, 60);
            ColorSymbolsButton.TabIndex = 86;
            ColorSymbolsButton.Text = "Color";
            ColorSymbolsButton.TextImageRelation = TextImageRelation.TextAboveImage;
            ColorSymbolsButton.UseVisualStyleBackColor = true;
            ColorSymbolsButton.Click += ColorSymbolsButton_Click;
            // 
            // SymbolColor4Label
            // 
            SymbolColor4Label.BackColor = Color.White;
            SymbolColor4Label.BorderStyle = BorderStyle.FixedSingle;
            SymbolColor4Label.Location = new Point(544, 54);
            SymbolColor4Label.Name = "SymbolColor4Label";
            SymbolColor4Label.Size = new Size(26, 26);
            SymbolColor4Label.TabIndex = 85;
            SymbolColor4Label.Click += SymbolColor4Label_Click;
            // 
            // SymbolColor3Label
            // 
            SymbolColor3Label.BackColor = Color.FromArgb(161, 214, 202, 171);
            SymbolColor3Label.BorderStyle = BorderStyle.FixedSingle;
            SymbolColor3Label.Location = new Point(512, 54);
            SymbolColor3Label.Name = "SymbolColor3Label";
            SymbolColor3Label.Size = new Size(26, 26);
            SymbolColor3Label.TabIndex = 84;
            SymbolColor3Label.Click += SymbolColor3Label_Click;
            // 
            // SymbolColor2Label
            // 
            SymbolColor2Label.BackColor = Color.FromArgb(53, 45, 32);
            SymbolColor2Label.BorderStyle = BorderStyle.FixedSingle;
            SymbolColor2Label.Location = new Point(480, 54);
            SymbolColor2Label.Name = "SymbolColor2Label";
            SymbolColor2Label.Size = new Size(26, 26);
            SymbolColor2Label.TabIndex = 83;
            SymbolColor2Label.Click += SymbolColor2Label_Click;
            // 
            // SymbolColor1Label
            // 
            SymbolColor1Label.BackColor = Color.FromArgb(85, 44, 36);
            SymbolColor1Label.BorderStyle = BorderStyle.FixedSingle;
            SymbolColor1Label.Location = new Point(448, 54);
            SymbolColor1Label.Name = "SymbolColor1Label";
            SymbolColor1Label.Size = new Size(26, 26);
            SymbolColor1Label.TabIndex = 82;
            SymbolColor1Label.Click += SymbolColor1Label_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(32, 22);
            label4.Name = "label4";
            label4.Size = new Size(75, 14);
            label4.TabIndex = 89;
            label4.Text = "Symbol Path";
            // 
            // SymbolPathLabel
            // 
            SymbolPathLabel.BorderStyle = BorderStyle.FixedSingle;
            SymbolPathLabel.Location = new Point(112, 21);
            SymbolPathLabel.Name = "SymbolPathLabel";
            SymbolPathLabel.Size = new Size(458, 23);
            SymbolPathLabel.TabIndex = 88;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(360, 54);
            label3.Name = "label3";
            label3.Size = new Size(82, 14);
            label3.TabIndex = 90;
            label3.Text = "Symbol Colors";
            // 
            // PlacedSymbolInfo
            // 
            AutoScaleDimensions = new SizeF(7F, 14F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = CloseButton;
            ClientSize = new Size(584, 377);
            ControlBox = false;
            Controls.Add(label3);
            Controls.Add(label4);
            Controls.Add(SymbolPathLabel);
            Controls.Add(ResetSymbolColorsButton);
            Controls.Add(ColorSymbolsButton);
            Controls.Add(SymbolColor4Label);
            Controls.Add(SymbolColor3Label);
            Controls.Add(SymbolColor2Label);
            Controls.Add(SymbolColor1Label);
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
            Controls.Add(label2);
            Controls.Add(CollectionNameLabel);
            Controls.Add(label1);
            Controls.Add(SymbolNameLabel);
            Controls.Add(CheckedTagsListBox);
            Controls.Add(CloseButton);
            Font = new Font("Tahoma", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "PlacedSymbolInfo";
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Placed Symbol Information";
            TopMost = true;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button CloseButton;
        private Panel panel1;
        private RadioButton UseCustomColorsRadio;
        private RadioButton GrayScaleSymbolRadio;
        private RadioButton OtherRadioButton;
        private RadioButton TerrainRadioButton;
        private RadioButton VegetationRadioButton;
        private RadioButton StructureRadioButton;
        private Label label10;
        private Label SymbolGuidLabel;
        private Label label8;
        private Label label6;
        private Label SymbolFormatLabel;
        private Label label2;
        private Label CollectionNameLabel;
        private Label label1;
        private Label SymbolNameLabel;
        private CheckedListBox CheckedTagsListBox;
        private FontAwesome.Sharp.IconButton ResetSymbolColorsButton;
        private FontAwesome.Sharp.IconButton ColorSymbolsButton;
        private Label SymbolColor4Label;
        private Label SymbolColor3Label;
        private Label SymbolColor2Label;
        private Label SymbolColor1Label;
        private Label label4;
        private Label SymbolPathLabel;
        private Label label3;
    }
}