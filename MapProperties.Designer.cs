namespace MapCreator
{
    partial class MapProperties
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
            mapName = new TextBox();
            label1 = new Label();
            MapHeight = new NumericUpDown();
            label2 = new Label();
            MapWidth = new NumericUpDown();
            label3 = new Label();
            label4 = new Label();
            applyButton = new Button();
            cancelButton = new Button();
            MapFilePath = new Label();
            groupBox1 = new GroupBox();
            WH4096x2048Radio = new RadioButton();
            MapAspectRatioLabel = new Label();
            WH7016x9921Radio = new RadioButton();
            label5 = new Label();
            WH4691x7016Radio = new RadioButton();
            WH5102x8409Radio = new RadioButton();
            WH5102x6591Radio = new RadioButton();
            WH3840x2160Radio = new RadioButton();
            WH2560x1080Radio = new RadioButton();
            WH1920x1080Radio = new RadioButton();
            WH1600x1200Radio = new RadioButton();
            WH1280x1024Radio = new RadioButton();
            WH1024x768Radio = new RadioButton();
            iconButton1 = new FontAwesome.Sharp.IconButton();
            groupBox2 = new GroupBox();
            listBox1 = new ListBox();
            groupBox3 = new GroupBox();
            InteriorMapButton = new RadioButton();
            OtherMapButton = new RadioButton();
            ShipMapButton = new RadioButton();
            StarMapButton = new RadioButton();
            DungeonMapButton = new RadioButton();
            TownMapButton = new RadioButton();
            CityMapButton = new RadioButton();
            RegionMapButton = new RadioButton();
            WorldMapButton = new RadioButton();
            groupBox4 = new GroupBox();
            MapUnitsCombo = new ComboBox();
            label9 = new Label();
            MapAreaHeightLabel = new Label();
            label8 = new Label();
            MapAreaWidthUpDown = new NumericUpDown();
            label7 = new Label();
            label6 = new Label();
            ((System.ComponentModel.ISupportInitialize)MapHeight).BeginInit();
            ((System.ComponentModel.ISupportInitialize)MapWidth).BeginInit();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)MapAreaWidthUpDown).BeginInit();
            SuspendLayout();
            // 
            // mapName
            // 
            mapName.Font = new Font("Tahoma", 12F);
            mapName.Location = new Point(129, 11);
            mapName.MaxLength = 256;
            mapName.Name = "mapName";
            mapName.Size = new Size(741, 27);
            mapName.TabIndex = 5;
            mapName.WordWrap = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14F);
            label1.Location = new Point(12, 142);
            label1.Name = "label1";
            label1.Size = new Size(111, 25);
            label1.TabIndex = 0;
            label1.Text = "Map Height";
            // 
            // MapHeight
            // 
            MapHeight.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            MapHeight.Location = new Point(129, 141);
            MapHeight.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            MapHeight.Minimum = new decimal(new int[] { 768, 0, 0, 0 });
            MapHeight.Name = "MapHeight";
            MapHeight.Size = new Size(83, 26);
            MapHeight.TabIndex = 1;
            MapHeight.TextAlign = HorizontalAlignment.Center;
            MapHeight.Value = new decimal(new int[] { 768, 0, 0, 0 });
            MapHeight.ValueChanged += MapHeight_ValueChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 14F);
            label2.Location = new Point(12, 106);
            label2.Name = "label2";
            label2.Size = new Size(106, 25);
            label2.TabIndex = 2;
            label2.Text = "Map Width";
            // 
            // MapWidth
            // 
            MapWidth.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            MapWidth.Location = new Point(129, 109);
            MapWidth.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            MapWidth.Minimum = new decimal(new int[] { 768, 0, 0, 0 });
            MapWidth.Name = "MapWidth";
            MapWidth.Size = new Size(83, 26);
            MapWidth.TabIndex = 3;
            MapWidth.TextAlign = HorizontalAlignment.Center;
            MapWidth.Value = new decimal(new int[] { 1024, 0, 0, 0 });
            MapWidth.ValueChanged += MapWidth_ValueChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 14F);
            label3.Location = new Point(12, 9);
            label3.Name = "label3";
            label3.Size = new Size(105, 25);
            label3.TabIndex = 4;
            label3.Text = "Map Name";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 14F);
            label4.Location = new Point(12, 58);
            label4.Name = "label4";
            label4.Size = new Size(84, 25);
            label4.TabIndex = 6;
            label4.Text = "Map File";
            // 
            // applyButton
            // 
            applyButton.BackColor = SystemColors.ActiveCaption;
            applyButton.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            applyButton.Location = new Point(686, 536);
            applyButton.Name = "applyButton";
            applyButton.Size = new Size(89, 41);
            applyButton.TabIndex = 9;
            applyButton.Text = "&Apply";
            applyButton.UseVisualStyleBackColor = false;
            applyButton.Click += applyButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Pixel);
            cancelButton.Location = new Point(781, 536);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(89, 41);
            cancelButton.TabIndex = 10;
            cancelButton.Text = "&Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // MapFilePath
            // 
            MapFilePath.BorderStyle = BorderStyle.FixedSingle;
            MapFilePath.Font = new Font("Tahoma", 12F);
            MapFilePath.Location = new Point(129, 61);
            MapFilePath.Name = "MapFilePath";
            MapFilePath.Size = new Size(741, 27);
            MapFilePath.TabIndex = 11;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(WH4096x2048Radio);
            groupBox1.Controls.Add(MapAspectRatioLabel);
            groupBox1.Controls.Add(WH7016x9921Radio);
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(WH4691x7016Radio);
            groupBox1.Controls.Add(WH5102x8409Radio);
            groupBox1.Controls.Add(WH5102x6591Radio);
            groupBox1.Controls.Add(WH3840x2160Radio);
            groupBox1.Controls.Add(WH2560x1080Radio);
            groupBox1.Controls.Add(WH1920x1080Radio);
            groupBox1.Controls.Add(WH1600x1200Radio);
            groupBox1.Controls.Add(WH1280x1024Radio);
            groupBox1.Controls.Add(WH1024x768Radio);
            groupBox1.Font = new Font("Tahoma", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(218, 106);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(280, 399);
            groupBox1.TabIndex = 12;
            groupBox1.TabStop = false;
            groupBox1.Text = "Map Size Presets (W x H)";
            // 
            // WH4096x2048Radio
            // 
            WH4096x2048Radio.AutoSize = true;
            WH4096x2048Radio.Location = new Point(15, 194);
            WH4096x2048Radio.Name = "WH4096x2048Radio";
            WH4096x2048Radio.Size = new Size(245, 22);
            WH4096x2048Radio.TabIndex = 11;
            WH4096x2048Radio.Text = "4096 x 2048 (Equirectangular 4K)";
            WH4096x2048Radio.UseVisualStyleBackColor = true;
            WH4096x2048Radio.CheckedChanged += WH4096x2048Radio_CheckedChanged;
            // 
            // MapAspectRatioLabel
            // 
            MapAspectRatioLabel.Font = new Font("Tahoma", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            MapAspectRatioLabel.Location = new Point(147, 369);
            MapAspectRatioLabel.Name = "MapAspectRatioLabel";
            MapAspectRatioLabel.Size = new Size(120, 17);
            MapAspectRatioLabel.TabIndex = 16;
            MapAspectRatioLabel.Text = "1.33";
            // 
            // WH7016x9921Radio
            // 
            WH7016x9921Radio.AutoSize = true;
            WH7016x9921Radio.Location = new Point(15, 306);
            WH7016x9921Radio.Name = "WH7016x9921Radio";
            WH7016x9921Radio.Size = new Size(194, 22);
            WH7016x9921Radio.TabIndex = 10;
            WH7016x9921Radio.Text = "7016 x 9921 (A3 600 dpi)";
            WH7016x9921Radio.UseVisualStyleBackColor = true;
            WH7016x9921Radio.CheckedChanged += WH7016x9921Radio_CheckedChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Tahoma", 10F);
            label5.Location = new Point(15, 369);
            label5.Name = "label5";
            label5.Size = new Size(126, 17);
            label5.TabIndex = 15;
            label5.Text = "Aspect Ratio (W/H)";
            // 
            // WH4691x7016Radio
            // 
            WH4691x7016Radio.AutoSize = true;
            WH4691x7016Radio.Location = new Point(15, 222);
            WH4691x7016Radio.Name = "WH4691x7016Radio";
            WH4691x7016Radio.Size = new Size(194, 22);
            WH4691x7016Radio.TabIndex = 9;
            WH4691x7016Radio.Text = "4691 x 7016 (A4 600 dpi)";
            WH4691x7016Radio.UseVisualStyleBackColor = true;
            WH4691x7016Radio.CheckedChanged += WH4691x7016Radio_CheckedChanged;
            // 
            // WH5102x8409Radio
            // 
            WH5102x8409Radio.AutoSize = true;
            WH5102x8409Radio.Location = new Point(15, 278);
            WH5102x8409Radio.Name = "WH5102x8409Radio";
            WH5102x8409Radio.Size = new Size(233, 22);
            WH5102x8409Radio.TabIndex = 8;
            WH5102x8409Radio.Text = "5102 x 8409 (US Legal 600 dpi)";
            WH5102x8409Radio.UseVisualStyleBackColor = true;
            WH5102x8409Radio.CheckedChanged += WH5102x8409Radio_CheckedChanged;
            // 
            // WH5102x6591Radio
            // 
            WH5102x6591Radio.AutoSize = true;
            WH5102x6591Radio.Location = new Point(15, 250);
            WH5102x6591Radio.Name = "WH5102x6591Radio";
            WH5102x6591Radio.Size = new Size(238, 22);
            WH5102x6591Radio.TabIndex = 7;
            WH5102x6591Radio.Text = "5102 x 6591 (US Letter 600 dpi)";
            WH5102x6591Radio.UseVisualStyleBackColor = true;
            WH5102x6591Radio.CheckedChanged += WH5102x6591Radio_CheckedChanged;
            // 
            // WH3840x2160Radio
            // 
            WH3840x2160Radio.AutoSize = true;
            WH3840x2160Radio.Location = new Point(15, 166);
            WH3840x2160Radio.Name = "WH3840x2160Radio";
            WH3840x2160Radio.Size = new Size(202, 22);
            WH3840x2160Radio.TabIndex = 6;
            WH3840x2160Radio.Text = "3840 x 2160 (4K Ultra HD)";
            WH3840x2160Radio.UseVisualStyleBackColor = true;
            WH3840x2160Radio.CheckedChanged += WH3840x2160Radio_CheckedChanged;
            // 
            // WH2560x1080Radio
            // 
            WH2560x1080Radio.AutoSize = true;
            WH2560x1080Radio.Location = new Point(15, 138);
            WH2560x1080Radio.Name = "WH2560x1080Radio";
            WH2560x1080Radio.Size = new Size(142, 22);
            WH2560x1080Radio.TabIndex = 5;
            WH2560x1080Radio.Text = "2560 x 1080 (2K)";
            WH2560x1080Radio.UseVisualStyleBackColor = true;
            WH2560x1080Radio.CheckedChanged += WH2560x1080Radio_CheckedChanged;
            // 
            // WH1920x1080Radio
            // 
            WH1920x1080Radio.AutoSize = true;
            WH1920x1080Radio.Location = new Point(15, 110);
            WH1920x1080Radio.Name = "WH1920x1080Radio";
            WH1920x1080Radio.Size = new Size(170, 22);
            WH1920x1080Radio.TabIndex = 4;
            WH1920x1080Radio.Text = "1920 x 1080 (Full HD)";
            WH1920x1080Radio.UseVisualStyleBackColor = true;
            WH1920x1080Radio.CheckedChanged += WH1920x1080Radio_CheckedChanged;
            // 
            // WH1600x1200Radio
            // 
            WH1600x1200Radio.AutoSize = true;
            WH1600x1200Radio.Location = new Point(15, 82);
            WH1600x1200Radio.Name = "WH1600x1200Radio";
            WH1600x1200Radio.Size = new Size(163, 22);
            WH1600x1200Radio.TabIndex = 3;
            WH1600x1200Radio.Text = "1600 x 1200 (UXGA)";
            WH1600x1200Radio.UseVisualStyleBackColor = true;
            WH1600x1200Radio.CheckedChanged += WH1600x1200Radio_CheckedChanged;
            // 
            // WH1280x1024Radio
            // 
            WH1280x1024Radio.AutoSize = true;
            WH1280x1024Radio.Location = new Point(15, 54);
            WH1280x1024Radio.Name = "WH1280x1024Radio";
            WH1280x1024Radio.Size = new Size(161, 22);
            WH1280x1024Radio.TabIndex = 2;
            WH1280x1024Radio.Text = "1280 x 1024 (SXGA)";
            WH1280x1024Radio.UseVisualStyleBackColor = true;
            WH1280x1024Radio.CheckedChanged += WH1280x1024Radio_CheckedChanged;
            // 
            // WH1024x768Radio
            // 
            WH1024x768Radio.AutoSize = true;
            WH1024x768Radio.Checked = true;
            WH1024x768Radio.Location = new Point(15, 26);
            WH1024x768Radio.Name = "WH1024x768Radio";
            WH1024x768Radio.Size = new Size(145, 22);
            WH1024x768Radio.TabIndex = 1;
            WH1024x768Radio.TabStop = true;
            WH1024x768Radio.Text = "1024 x 768 (XGA)";
            WH1024x768Radio.UseVisualStyleBackColor = true;
            WH1024x768Radio.CheckedChanged += WH1024x768Radio_CheckedChanged;
            // 
            // iconButton1
            // 
            iconButton1.Font = new Font("Tahoma", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            iconButton1.IconChar = FontAwesome.Sharp.IconChar.Exchange;
            iconButton1.IconColor = Color.Black;
            iconButton1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconButton1.IconSize = 24;
            iconButton1.Location = new Point(12, 189);
            iconButton1.Name = "iconButton1";
            iconButton1.Size = new Size(200, 41);
            iconButton1.TabIndex = 13;
            iconButton1.Text = "Swap Resolution";
            iconButton1.TextImageRelation = TextImageRelation.ImageBeforeText;
            iconButton1.UseVisualStyleBackColor = true;
            iconButton1.Click += iconButton1_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(listBox1);
            groupBox2.Font = new Font("Tahoma", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            groupBox2.Location = new Point(504, 393);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(366, 137);
            groupBox2.TabIndex = 14;
            groupBox2.TabStop = false;
            groupBox2.Text = "Map Themes";
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.Items.AddRange(new object[] { "Default Theme", "Theme 1", "Theme 2", "Theme 3", "Theme 4", "Theme 5", "Theme 6" });
            listBox1.Location = new Point(9, 24);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(351, 100);
            listBox1.TabIndex = 0;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(InteriorMapButton);
            groupBox3.Controls.Add(OtherMapButton);
            groupBox3.Controls.Add(ShipMapButton);
            groupBox3.Controls.Add(StarMapButton);
            groupBox3.Controls.Add(DungeonMapButton);
            groupBox3.Controls.Add(TownMapButton);
            groupBox3.Controls.Add(CityMapButton);
            groupBox3.Controls.Add(RegionMapButton);
            groupBox3.Controls.Add(WorldMapButton);
            groupBox3.Font = new Font("Tahoma", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            groupBox3.Location = new Point(504, 106);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(366, 281);
            groupBox3.TabIndex = 17;
            groupBox3.TabStop = false;
            groupBox3.Text = "Map Types";
            // 
            // InteriorMapButton
            // 
            InteriorMapButton.AutoSize = true;
            InteriorMapButton.Location = new Point(9, 137);
            InteriorMapButton.Name = "InteriorMapButton";
            InteriorMapButton.Size = new Size(106, 22);
            InteriorMapButton.TabIndex = 8;
            InteriorMapButton.TabStop = true;
            InteriorMapButton.Text = "Interior Map";
            InteriorMapButton.UseVisualStyleBackColor = true;
            InteriorMapButton.CheckedChanged += InteriorMapButton_CheckedChanged;
            // 
            // OtherMapButton
            // 
            OtherMapButton.AutoSize = true;
            OtherMapButton.Location = new Point(6, 249);
            OtherMapButton.Name = "OtherMapButton";
            OtherMapButton.Size = new Size(150, 22);
            OtherMapButton.TabIndex = 7;
            OtherMapButton.TabStop = true;
            OtherMapButton.Text = "Other/Generic Map";
            OtherMapButton.UseVisualStyleBackColor = true;
            OtherMapButton.CheckedChanged += OtherMapButton_CheckedChanged;
            // 
            // ShipMapButton
            // 
            ShipMapButton.AutoSize = true;
            ShipMapButton.Location = new Point(4, 221);
            ShipMapButton.Name = "ShipMapButton";
            ShipMapButton.Size = new Size(85, 22);
            ShipMapButton.TabIndex = 6;
            ShipMapButton.TabStop = true;
            ShipMapButton.Text = "Ship Map";
            ShipMapButton.UseVisualStyleBackColor = true;
            ShipMapButton.CheckedChanged += ShipMapButton_CheckedChanged;
            // 
            // StarMapButton
            // 
            StarMapButton.AutoSize = true;
            StarMapButton.Location = new Point(6, 193);
            StarMapButton.Name = "StarMapButton";
            StarMapButton.Size = new Size(85, 22);
            StarMapButton.TabIndex = 5;
            StarMapButton.TabStop = true;
            StarMapButton.Text = "Star Map";
            StarMapButton.UseVisualStyleBackColor = true;
            StarMapButton.CheckedChanged += StarMapButton_CheckedChanged;
            // 
            // DungeonMapButton
            // 
            DungeonMapButton.AutoSize = true;
            DungeonMapButton.Location = new Point(6, 165);
            DungeonMapButton.Name = "DungeonMapButton";
            DungeonMapButton.Size = new Size(224, 22);
            DungeonMapButton.TabIndex = 4;
            DungeonMapButton.TabStop = true;
            DungeonMapButton.Text = "Dungeon or Underground Map";
            DungeonMapButton.UseVisualStyleBackColor = true;
            DungeonMapButton.CheckedChanged += DungeonMapButton_CheckedChanged;
            // 
            // TownMapButton
            // 
            TownMapButton.AutoSize = true;
            TownMapButton.Location = new Point(6, 109);
            TownMapButton.Name = "TownMapButton";
            TownMapButton.Size = new Size(157, 22);
            TownMapButton.TabIndex = 3;
            TownMapButton.TabStop = true;
            TownMapButton.Text = "Town or Village Map";
            TownMapButton.UseVisualStyleBackColor = true;
            TownMapButton.CheckedChanged += TownMapButton_CheckedChanged;
            // 
            // CityMapButton
            // 
            CityMapButton.AutoSize = true;
            CityMapButton.Location = new Point(6, 81);
            CityMapButton.Name = "CityMapButton";
            CityMapButton.Size = new Size(83, 22);
            CityMapButton.TabIndex = 2;
            CityMapButton.TabStop = true;
            CityMapButton.Text = "City Map";
            CityMapButton.UseVisualStyleBackColor = true;
            CityMapButton.CheckedChanged += CityMapButton_CheckedChanged;
            // 
            // RegionMapButton
            // 
            RegionMapButton.AutoSize = true;
            RegionMapButton.Location = new Point(6, 53);
            RegionMapButton.Name = "RegionMapButton";
            RegionMapButton.Size = new Size(155, 22);
            RegionMapButton.TabIndex = 1;
            RegionMapButton.TabStop = true;
            RegionMapButton.Text = "Region or Area Map";
            RegionMapButton.UseVisualStyleBackColor = true;
            RegionMapButton.CheckedChanged += RegionMapButton_CheckedChanged;
            // 
            // WorldMapButton
            // 
            WorldMapButton.AutoSize = true;
            WorldMapButton.Location = new Point(6, 25);
            WorldMapButton.Name = "WorldMapButton";
            WorldMapButton.Size = new Size(96, 22);
            WorldMapButton.TabIndex = 0;
            WorldMapButton.TabStop = true;
            WorldMapButton.Text = "World Map";
            WorldMapButton.UseVisualStyleBackColor = true;
            WorldMapButton.CheckedChanged += WorldMapButton_CheckedChanged;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(MapUnitsCombo);
            groupBox4.Controls.Add(label9);
            groupBox4.Controls.Add(MapAreaHeightLabel);
            groupBox4.Controls.Add(label8);
            groupBox4.Controls.Add(MapAreaWidthUpDown);
            groupBox4.Controls.Add(label7);
            groupBox4.Controls.Add(label6);
            groupBox4.Font = new Font("Tahoma", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            groupBox4.Location = new Point(12, 236);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(200, 269);
            groupBox4.TabIndex = 18;
            groupBox4.TabStop = false;
            groupBox4.Text = "Map Area";
            // 
            // MapUnitsCombo
            // 
            MapUnitsCombo.FormattingEnabled = true;
            MapUnitsCombo.Items.AddRange(new object[] { "Centimeters", "Inches", "Feet", "Yards", "Meters", "Kilometers", "Miles", "Astronomical Units (AU)", "Light Years", "Parsecs" });
            MapUnitsCombo.Location = new Point(10, 56);
            MapUnitsCombo.Name = "MapUnitsCombo";
            MapUnitsCombo.Size = new Size(184, 26);
            MapUnitsCombo.TabIndex = 7;
            MapUnitsCombo.Text = "Miles";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label9.Location = new Point(6, 243);
            label9.Name = "label9";
            label9.Size = new Size(159, 13);
            label9.TabIndex = 6;
            label9.Text = "*Height is calculated from width";
            // 
            // MapAreaHeightLabel
            // 
            MapAreaHeightLabel.BorderStyle = BorderStyle.FixedSingle;
            MapAreaHeightLabel.Location = new Point(6, 176);
            MapAreaHeightLabel.Name = "MapAreaHeightLabel";
            MapAreaHeightLabel.Size = new Size(188, 26);
            MapAreaHeightLabel.TabIndex = 5;
            MapAreaHeightLabel.Text = "75";
            MapAreaHeightLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(6, 156);
            label8.Name = "label8";
            label8.Size = new Size(125, 18);
            label8.TabIndex = 4;
            label8.Text = "Map Area Height*";
            // 
            // MapAreaWidthUpDown
            // 
            MapAreaWidthUpDown.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            MapAreaWidthUpDown.Location = new Point(6, 117);
            MapAreaWidthUpDown.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
            MapAreaWidthUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            MapAreaWidthUpDown.Name = "MapAreaWidthUpDown";
            MapAreaWidthUpDown.Size = new Size(188, 26);
            MapAreaWidthUpDown.TabIndex = 3;
            MapAreaWidthUpDown.Value = new decimal(new int[] { 100, 0, 0, 0 });
            MapAreaWidthUpDown.ValueChanged += MapAreaWidthUpDown_ValueChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(6, 96);
            label7.Name = "label7";
            label7.Size = new Size(113, 18);
            label7.TabIndex = 2;
            label7.Text = "Map Area Width";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Tahoma", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(6, 35);
            label6.Name = "label6";
            label6.Size = new Size(75, 18);
            label6.TabIndex = 0;
            label6.Text = "Area Units";
            // 
            // MapProperties
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(882, 589);
            ControlBox = false;
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(iconButton1);
            Controls.Add(groupBox1);
            Controls.Add(MapFilePath);
            Controls.Add(cancelButton);
            Controls.Add(applyButton);
            Controls.Add(label4);
            Controls.Add(mapName);
            Controls.Add(label3);
            Controls.Add(MapWidth);
            Controls.Add(label2);
            Controls.Add(MapHeight);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            HelpButton = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MapProperties";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Map Properties";
            ((System.ComponentModel.ISupportInitialize)MapHeight).EndInit();
            ((System.ComponentModel.ISupportInitialize)MapWidth).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)MapAreaWidthUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private NumericUpDown MapHeight;
        private Label label2;
        private NumericUpDown MapWidth;
        private Label label3;
        private TextBox mapName;
        private Label label4;
        private Button applyButton;
        private Button cancelButton;
        private Label MapFilePath;
        private GroupBox groupBox1;
        private RadioButton WH1280x1024Radio;
        private RadioButton WH1024x768Radio;
        private RadioButton WH1920x1080Radio;
        private RadioButton WH1600x1200Radio;
        private RadioButton WH2560x1080Radio;
        private RadioButton WH5102x6591Radio;
        private RadioButton WH3840x2160Radio;
        private RadioButton WH5102x8409Radio;
        private RadioButton WH4691x7016Radio;
        private RadioButton WH7016x9921Radio;
        private FontAwesome.Sharp.IconButton iconButton1;
        private GroupBox groupBox2;
        private ListBox listBox1;
        private Label label5;
        private Label MapAspectRatioLabel;
        private GroupBox groupBox3;
        private RadioButton WorldMapButton;
        private RadioButton TownMapButton;
        private RadioButton CityMapButton;
        private RadioButton RegionMapButton;
        private RadioButton WH4096x2048Radio;
        private RadioButton DungeonMapButton;
        private RadioButton StarMapButton;
        private RadioButton OtherMapButton;
        private RadioButton ShipMapButton;
        private GroupBox groupBox4;
        private Label label6;
        private NumericUpDown MapAreaWidthUpDown;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label MapAreaHeightLabel;
        private RadioButton InteriorMapButton;
        private ComboBox MapUnitsCombo;
    }
}