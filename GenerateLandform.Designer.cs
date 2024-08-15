namespace MapCreator
{
    partial class GenerateLandform
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
            LandformPictureBox = new PictureBox();
            PlaceLandformButton = new Button();
            CloseButton = new Button();
            GenLandformSplitContainer = new SplitContainer();
            GenerateLandformButton = new FontAwesome.Sharp.IconButton();
            EquirectangularRadio = new RadioButton();
            WorldRadio = new RadioButton();
            AtollRadio = new RadioButton();
            ArchipelagoRadio = new RadioButton();
            ContinentRadio = new RadioButton();
            RandomLandformRadio = new RadioButton();
            ShowHideAdvancedButton = new Button();
            label10 = new Label();
            RoughnessTrack = new TrackBar();
            label9 = new Label();
            RotationTrack = new TrackBar();
            label8 = new Label();
            ScaleTrack = new TrackBar();
            label7 = new Label();
            VariationTrack = new TrackBar();
            label6 = new Label();
            SmoothingTrack = new TrackBar();
            RemoveDisconnectedCellsButton = new Button();
            label5 = new Label();
            DistanceFunctionUpDown = new DomainUpDown();
            CreateBoundaryButton = new Button();
            label4 = new Label();
            SeaLevelUpDown = new NumericUpDown();
            label3 = new Label();
            LerpWeightUpDown = new NumericUpDown();
            label2 = new Label();
            NoiseScaleUpDown = new NumericUpDown();
            AssignHeightButton = new Button();
            GenerateCellsButton = new Button();
            GeneratePointsButton = new Button();
            label1 = new Label();
            GridSizeUpDown = new NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)LandformPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)GenLandformSplitContainer).BeginInit();
            GenLandformSplitContainer.Panel1.SuspendLayout();
            GenLandformSplitContainer.Panel2.SuspendLayout();
            GenLandformSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)RoughnessTrack).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RotationTrack).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ScaleTrack).BeginInit();
            ((System.ComponentModel.ISupportInitialize)VariationTrack).BeginInit();
            ((System.ComponentModel.ISupportInitialize)SmoothingTrack).BeginInit();
            ((System.ComponentModel.ISupportInitialize)SeaLevelUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)LerpWeightUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NoiseScaleUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)GridSizeUpDown).BeginInit();
            SuspendLayout();
            // 
            // LandformPictureBox
            // 
            LandformPictureBox.BackColor = Color.White;
            LandformPictureBox.Location = new Point(254, 10);
            LandformPictureBox.Name = "LandformPictureBox";
            LandformPictureBox.Size = new Size(640, 469);
            LandformPictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
            LandformPictureBox.TabIndex = 0;
            LandformPictureBox.TabStop = false;
            // 
            // PlaceLandformButton
            // 
            PlaceLandformButton.Location = new Point(738, 496);
            PlaceLandformButton.Name = "PlaceLandformButton";
            PlaceLandformButton.Size = new Size(75, 48);
            PlaceLandformButton.TabIndex = 13;
            PlaceLandformButton.Text = "Place";
            PlaceLandformButton.UseVisualStyleBackColor = true;
            PlaceLandformButton.Click += PlaceLandformButton_Click;
            // 
            // CloseButton
            // 
            CloseButton.Location = new Point(819, 496);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new Size(75, 48);
            CloseButton.TabIndex = 14;
            CloseButton.Text = "Close";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += CloseButton_Click;
            // 
            // GenLandformSplitContainer
            // 
            GenLandformSplitContainer.BackColor = SystemColors.ControlDarkDark;
            GenLandformSplitContainer.Location = new Point(12, 10);
            GenLandformSplitContainer.Name = "GenLandformSplitContainer";
            GenLandformSplitContainer.Orientation = Orientation.Horizontal;
            // 
            // GenLandformSplitContainer.Panel1
            // 
            GenLandformSplitContainer.Panel1.BackColor = SystemColors.Control;
            GenLandformSplitContainer.Panel1.Controls.Add(GenerateLandformButton);
            GenLandformSplitContainer.Panel1.Controls.Add(EquirectangularRadio);
            GenLandformSplitContainer.Panel1.Controls.Add(WorldRadio);
            GenLandformSplitContainer.Panel1.Controls.Add(AtollRadio);
            GenLandformSplitContainer.Panel1.Controls.Add(ArchipelagoRadio);
            GenLandformSplitContainer.Panel1.Controls.Add(ContinentRadio);
            GenLandformSplitContainer.Panel1.Controls.Add(RandomLandformRadio);
            GenLandformSplitContainer.Panel1.Controls.Add(ShowHideAdvancedButton);
            GenLandformSplitContainer.Panel1MinSize = 32;
            // 
            // GenLandformSplitContainer.Panel2
            // 
            GenLandformSplitContainer.Panel2.BackColor = SystemColors.Control;
            GenLandformSplitContainer.Panel2.Controls.Add(label10);
            GenLandformSplitContainer.Panel2.Controls.Add(RoughnessTrack);
            GenLandformSplitContainer.Panel2.Controls.Add(label9);
            GenLandformSplitContainer.Panel2.Controls.Add(RotationTrack);
            GenLandformSplitContainer.Panel2.Controls.Add(label8);
            GenLandformSplitContainer.Panel2.Controls.Add(ScaleTrack);
            GenLandformSplitContainer.Panel2.Controls.Add(label7);
            GenLandformSplitContainer.Panel2.Controls.Add(VariationTrack);
            GenLandformSplitContainer.Panel2.Controls.Add(label6);
            GenLandformSplitContainer.Panel2.Controls.Add(SmoothingTrack);
            GenLandformSplitContainer.Panel2.Controls.Add(RemoveDisconnectedCellsButton);
            GenLandformSplitContainer.Panel2.Controls.Add(label5);
            GenLandformSplitContainer.Panel2.Controls.Add(DistanceFunctionUpDown);
            GenLandformSplitContainer.Panel2.Controls.Add(CreateBoundaryButton);
            GenLandformSplitContainer.Panel2.Controls.Add(label4);
            GenLandformSplitContainer.Panel2.Controls.Add(SeaLevelUpDown);
            GenLandformSplitContainer.Panel2.Controls.Add(label3);
            GenLandformSplitContainer.Panel2.Controls.Add(LerpWeightUpDown);
            GenLandformSplitContainer.Panel2.Controls.Add(label2);
            GenLandformSplitContainer.Panel2.Controls.Add(NoiseScaleUpDown);
            GenLandformSplitContainer.Panel2.Controls.Add(AssignHeightButton);
            GenLandformSplitContainer.Panel2.Controls.Add(GenerateCellsButton);
            GenLandformSplitContainer.Panel2.Controls.Add(GeneratePointsButton);
            GenLandformSplitContainer.Panel2.Controls.Add(label1);
            GenLandformSplitContainer.Panel2.Controls.Add(GridSizeUpDown);
            GenLandformSplitContainer.Panel2MinSize = 0;
            GenLandformSplitContainer.Size = new Size(224, 534);
            GenLandformSplitContainer.SplitterDistance = 61;
            GenLandformSplitContainer.TabIndex = 28;
            // 
            // GenerateLandformButton
            // 
            GenerateLandformButton.Font = new Font("Tahoma", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            GenerateLandformButton.IconChar = FontAwesome.Sharp.IconChar.Splotch;
            GenerateLandformButton.IconColor = Color.Black;
            GenerateLandformButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            GenerateLandformButton.Location = new Point(72, 249);
            GenerateLandformButton.Name = "GenerateLandformButton";
            GenerateLandformButton.Size = new Size(80, 80);
            GenerateLandformButton.TabIndex = 7;
            GenerateLandformButton.Text = "Generate";
            GenerateLandformButton.TextImageRelation = TextImageRelation.TextAboveImage;
            GenerateLandformButton.UseVisualStyleBackColor = true;
            GenerateLandformButton.Click += GenerateLandformButton_Click;
            // 
            // EquirectangularRadio
            // 
            EquirectangularRadio.AutoSize = true;
            EquirectangularRadio.Location = new Point(24, 194);
            EquirectangularRadio.Name = "EquirectangularRadio";
            EquirectangularRadio.Size = new Size(114, 20);
            EquirectangularRadio.TabIndex = 6;
            EquirectangularRadio.Text = "Equirectangular";
            EquirectangularRadio.UseVisualStyleBackColor = true;
            EquirectangularRadio.CheckedChanged += EquirectangularRadio_CheckedChanged;
            // 
            // WorldRadio
            // 
            WorldRadio.AutoSize = true;
            WorldRadio.Location = new Point(24, 168);
            WorldRadio.Name = "WorldRadio";
            WorldRadio.Size = new Size(59, 20);
            WorldRadio.TabIndex = 5;
            WorldRadio.Text = "World";
            WorldRadio.UseVisualStyleBackColor = true;
            WorldRadio.CheckedChanged += WorldRadio_CheckedChanged;
            // 
            // AtollRadio
            // 
            AtollRadio.AutoSize = true;
            AtollRadio.Location = new Point(24, 142);
            AtollRadio.Name = "AtollRadio";
            AtollRadio.Size = new Size(50, 20);
            AtollRadio.TabIndex = 4;
            AtollRadio.Text = "Atoll";
            AtollRadio.UseVisualStyleBackColor = true;
            AtollRadio.CheckedChanged += AtollRadio_CheckedChanged;
            // 
            // ArchipelagoRadio
            // 
            ArchipelagoRadio.AutoSize = true;
            ArchipelagoRadio.Location = new Point(24, 116);
            ArchipelagoRadio.Name = "ArchipelagoRadio";
            ArchipelagoRadio.Size = new Size(92, 20);
            ArchipelagoRadio.TabIndex = 3;
            ArchipelagoRadio.Text = "Archipelago";
            ArchipelagoRadio.UseVisualStyleBackColor = true;
            ArchipelagoRadio.CheckedChanged += ArchipelagoRadio_CheckedChanged;
            // 
            // ContinentRadio
            // 
            ContinentRadio.AutoSize = true;
            ContinentRadio.Location = new Point(24, 90);
            ContinentRadio.Name = "ContinentRadio";
            ContinentRadio.Size = new Size(79, 20);
            ContinentRadio.TabIndex = 2;
            ContinentRadio.Text = "Continent";
            ContinentRadio.UseVisualStyleBackColor = true;
            ContinentRadio.CheckedChanged += ContinentRadio_CheckedChanged;
            // 
            // RandomLandformRadio
            // 
            RandomLandformRadio.AutoSize = true;
            RandomLandformRadio.Checked = true;
            RandomLandformRadio.Location = new Point(24, 64);
            RandomLandformRadio.Name = "RandomLandformRadio";
            RandomLandformRadio.Size = new Size(130, 20);
            RandomLandformRadio.TabIndex = 1;
            RandomLandformRadio.TabStop = true;
            RandomLandformRadio.Text = "Random Landform";
            RandomLandformRadio.UseVisualStyleBackColor = true;
            RandomLandformRadio.CheckedChanged += RandomLandformRadio_CheckedChanged;
            // 
            // ShowHideAdvancedButton
            // 
            ShowHideAdvancedButton.Location = new Point(3, 3);
            ShowHideAdvancedButton.Name = "ShowHideAdvancedButton";
            ShowHideAdvancedButton.Size = new Size(217, 28);
            ShowHideAdvancedButton.TabIndex = 0;
            ShowHideAdvancedButton.Text = "Show/Hide Advanced Controls";
            ShowHideAdvancedButton.UseVisualStyleBackColor = true;
            ShowHideAdvancedButton.Click += ShowHideAdvancedButton_Click;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(13, 354);
            label10.Name = "label10";
            label10.Size = new Size(69, 16);
            label10.TabIndex = 52;
            label10.Text = "Roughness";
            // 
            // RoughnessTrack
            // 
            RoughnessTrack.AutoSize = false;
            RoughnessTrack.Location = new Point(79, 354);
            RoughnessTrack.Maximum = 50;
            RoughnessTrack.Minimum = 2;
            RoughnessTrack.Name = "RoughnessTrack";
            RoughnessTrack.Size = new Size(128, 18);
            RoughnessTrack.TabIndex = 51;
            RoughnessTrack.TickStyle = TickStyle.None;
            RoughnessTrack.Value = 20;
            RoughnessTrack.ValueChanged += RoughnessTrack_ValueChanged;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(19, 398);
            label9.Name = "label9";
            label9.Size = new Size(54, 16);
            label9.TabIndex = 50;
            label9.Text = "Rotation";
            // 
            // RotationTrack
            // 
            RotationTrack.AutoSize = false;
            RotationTrack.Location = new Point(79, 398);
            RotationTrack.Maximum = 359;
            RotationTrack.Name = "RotationTrack";
            RotationTrack.Size = new Size(128, 18);
            RotationTrack.TabIndex = 49;
            RotationTrack.TickStyle = TickStyle.None;
            RotationTrack.ValueChanged += RotationTrack_ValueChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(35, 378);
            label8.Name = "label8";
            label8.Size = new Size(38, 16);
            label8.TabIndex = 48;
            label8.Text = "Scale";
            // 
            // ScaleTrack
            // 
            ScaleTrack.AutoSize = false;
            ScaleTrack.Location = new Point(79, 378);
            ScaleTrack.Maximum = 100;
            ScaleTrack.Minimum = 10;
            ScaleTrack.Name = "ScaleTrack";
            ScaleTrack.Size = new Size(128, 18);
            ScaleTrack.TabIndex = 47;
            ScaleTrack.TickStyle = TickStyle.None;
            ScaleTrack.Value = 100;
            ScaleTrack.ValueChanged += ScaleTrack_ValueChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(24, 334);
            label7.Name = "label7";
            label7.Size = new Size(58, 16);
            label7.TabIndex = 46;
            label7.Text = "Variation";
            // 
            // VariationTrack
            // 
            VariationTrack.AutoSize = false;
            VariationTrack.Location = new Point(79, 334);
            VariationTrack.Minimum = 1;
            VariationTrack.Name = "VariationTrack";
            VariationTrack.Size = new Size(128, 18);
            VariationTrack.TabIndex = 45;
            VariationTrack.TickStyle = TickStyle.None;
            VariationTrack.Value = 5;
            VariationTrack.ValueChanged += VariationTrack_ValueChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(14, 314);
            label6.Name = "label6";
            label6.Size = new Size(68, 16);
            label6.TabIndex = 44;
            label6.Text = "Smoothing";
            // 
            // SmoothingTrack
            // 
            SmoothingTrack.AutoSize = false;
            SmoothingTrack.Location = new Point(79, 314);
            SmoothingTrack.Maximum = 200;
            SmoothingTrack.Minimum = 1;
            SmoothingTrack.Name = "SmoothingTrack";
            SmoothingTrack.Size = new Size(128, 18);
            SmoothingTrack.TabIndex = 43;
            SmoothingTrack.TickStyle = TickStyle.None;
            SmoothingTrack.Value = 50;
            SmoothingTrack.ValueChanged += SmoothingTrack_ValueChanged;
            // 
            // RemoveDisconnectedCellsButton
            // 
            RemoveDisconnectedCellsButton.Location = new Point(14, 278);
            RemoveDisconnectedCellsButton.Name = "RemoveDisconnectedCellsButton";
            RemoveDisconnectedCellsButton.Size = new Size(193, 28);
            RemoveDisconnectedCellsButton.TabIndex = 42;
            RemoveDisconnectedCellsButton.Text = "Remove Disconnected Cells";
            RemoveDisconnectedCellsButton.UseVisualStyleBackColor = true;
            RemoveDisconnectedCellsButton.Click += RemoveDisconnectedCellsButton_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(14, 167);
            label5.Name = "label5";
            label5.Size = new Size(107, 16);
            label5.TabIndex = 41;
            label5.Text = "Distance Function";
            // 
            // DistanceFunctionUpDown
            // 
            DistanceFunctionUpDown.Items.Add("Distance Squared");
            DistanceFunctionUpDown.Items.Add("Euclidean Squared");
            DistanceFunctionUpDown.Items.Add("Square Bump");
            DistanceFunctionUpDown.Items.Add("Hyperboloid");
            DistanceFunctionUpDown.Items.Add("Trigonometric Product");
            DistanceFunctionUpDown.Items.Add("Squircle");
            DistanceFunctionUpDown.Location = new Point(14, 186);
            DistanceFunctionUpDown.Name = "DistanceFunctionUpDown";
            DistanceFunctionUpDown.Size = new Size(193, 23);
            DistanceFunctionUpDown.TabIndex = 40;
            DistanceFunctionUpDown.Text = "Distance Squared";
            DistanceFunctionUpDown.TextAlign = HorizontalAlignment.Center;
            // 
            // CreateBoundaryButton
            // 
            CreateBoundaryButton.Location = new Point(14, 422);
            CreateBoundaryButton.Name = "CreateBoundaryButton";
            CreateBoundaryButton.Size = new Size(193, 28);
            CreateBoundaryButton.TabIndex = 39;
            CreateBoundaryButton.Text = "Create Landform Boundary";
            CreateBoundaryButton.UseVisualStyleBackColor = true;
            CreateBoundaryButton.Click += CreateBoundaryButton_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(35, 251);
            label4.Name = "label4";
            label4.Size = new Size(102, 16);
            label4.TabIndex = 38;
            label4.Text = "Sea Level Height";
            // 
            // SeaLevelUpDown
            // 
            SeaLevelUpDown.DecimalPlaces = 2;
            SeaLevelUpDown.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            SeaLevelUpDown.Location = new Point(143, 249);
            SeaLevelUpDown.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            SeaLevelUpDown.Name = "SeaLevelUpDown";
            SeaLevelUpDown.Size = new Size(64, 23);
            SeaLevelUpDown.TabIndex = 37;
            SeaLevelUpDown.TextAlign = HorizontalAlignment.Center;
            SeaLevelUpDown.Value = new decimal(new int[] { 5, 0, 0, 65536 });
            SeaLevelUpDown.ValueChanged += SeaLevelUpDown_ValueChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(14, 143);
            label3.Name = "label3";
            label3.Size = new Size(123, 16);
            label3.TabIndex = 36;
            label3.Text = "Interpolation Weight";
            // 
            // LerpWeightUpDown
            // 
            LerpWeightUpDown.DecimalPlaces = 2;
            LerpWeightUpDown.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            LerpWeightUpDown.Location = new Point(143, 141);
            LerpWeightUpDown.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            LerpWeightUpDown.Name = "LerpWeightUpDown";
            LerpWeightUpDown.Size = new Size(64, 23);
            LerpWeightUpDown.TabIndex = 35;
            LerpWeightUpDown.TextAlign = HorizontalAlignment.Center;
            LerpWeightUpDown.Value = new decimal(new int[] { 5, 0, 0, 65536 });
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(64, 114);
            label2.Name = "label2";
            label2.Size = new Size(73, 16);
            label2.TabIndex = 34;
            label2.Text = "Noise Scale";
            // 
            // NoiseScaleUpDown
            // 
            NoiseScaleUpDown.DecimalPlaces = 2;
            NoiseScaleUpDown.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            NoiseScaleUpDown.Location = new Point(143, 112);
            NoiseScaleUpDown.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            NoiseScaleUpDown.Name = "NoiseScaleUpDown";
            NoiseScaleUpDown.Size = new Size(64, 23);
            NoiseScaleUpDown.TabIndex = 33;
            NoiseScaleUpDown.TextAlign = HorizontalAlignment.Center;
            NoiseScaleUpDown.Value = new decimal(new int[] { 1, 0, 0, 65536 });
            // 
            // AssignHeightButton
            // 
            AssignHeightButton.Location = new Point(14, 215);
            AssignHeightButton.Name = "AssignHeightButton";
            AssignHeightButton.Size = new Size(193, 28);
            AssignHeightButton.TabIndex = 32;
            AssignHeightButton.Text = "Assign Cell Height";
            AssignHeightButton.UseVisualStyleBackColor = true;
            AssignHeightButton.Click += AssignHeightButton_Click;
            // 
            // GenerateCellsButton
            // 
            GenerateCellsButton.Location = new Point(14, 78);
            GenerateCellsButton.Name = "GenerateCellsButton";
            GenerateCellsButton.Size = new Size(193, 28);
            GenerateCellsButton.TabIndex = 31;
            GenerateCellsButton.Text = "Generate Cells";
            GenerateCellsButton.UseVisualStyleBackColor = true;
            GenerateCellsButton.Click += GenerateCellsButton_Click;
            // 
            // GeneratePointsButton
            // 
            GeneratePointsButton.Location = new Point(14, 44);
            GeneratePointsButton.Name = "GeneratePointsButton";
            GeneratePointsButton.Size = new Size(193, 28);
            GeneratePointsButton.TabIndex = 30;
            GeneratePointsButton.Text = "Generate Points";
            GeneratePointsButton.UseVisualStyleBackColor = true;
            GeneratePointsButton.Click += GeneratePointsButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(79, 17);
            label1.Name = "label1";
            label1.Size = new Size(58, 16);
            label1.TabIndex = 29;
            label1.Text = "Grid Size";
            // 
            // GridSizeUpDown
            // 
            GridSizeUpDown.Location = new Point(143, 15);
            GridSizeUpDown.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            GridSizeUpDown.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            GridSizeUpDown.Name = "GridSizeUpDown";
            GridSizeUpDown.Size = new Size(64, 23);
            GridSizeUpDown.TabIndex = 28;
            GridSizeUpDown.TextAlign = HorizontalAlignment.Center;
            GridSizeUpDown.Value = new decimal(new int[] { 25, 0, 0, 0 });
            GridSizeUpDown.ValueChanged += GridSizeUpDown_ValueChanged;
            // 
            // GenerateLandform
            // 
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = CloseButton;
            ClientSize = new Size(907, 566);
            Controls.Add(GenLandformSplitContainer);
            Controls.Add(CloseButton);
            Controls.Add(PlaceLandformButton);
            Controls.Add(LandformPictureBox);
            Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "GenerateLandform";
            Text = "Generate Landform";
            ((System.ComponentModel.ISupportInitialize)LandformPictureBox).EndInit();
            GenLandformSplitContainer.Panel1.ResumeLayout(false);
            GenLandformSplitContainer.Panel1.PerformLayout();
            GenLandformSplitContainer.Panel2.ResumeLayout(false);
            GenLandformSplitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)GenLandformSplitContainer).EndInit();
            GenLandformSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)RoughnessTrack).EndInit();
            ((System.ComponentModel.ISupportInitialize)RotationTrack).EndInit();
            ((System.ComponentModel.ISupportInitialize)ScaleTrack).EndInit();
            ((System.ComponentModel.ISupportInitialize)VariationTrack).EndInit();
            ((System.ComponentModel.ISupportInitialize)SmoothingTrack).EndInit();
            ((System.ComponentModel.ISupportInitialize)SeaLevelUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)LerpWeightUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)NoiseScaleUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)GridSizeUpDown).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private PictureBox LandformPictureBox;
        private Button PlaceLandformButton;
        private Button CloseButton;
        private SplitContainer GenLandformSplitContainer;
        private Label label10;
        private TrackBar RoughnessTrack;
        private Label label9;
        private TrackBar RotationTrack;
        private Label label8;
        private TrackBar ScaleTrack;
        private Label label7;
        private TrackBar VariationTrack;
        private Label label6;
        private TrackBar SmoothingTrack;
        private Button RemoveDisconnectedCellsButton;
        private Label label5;
        private DomainUpDown DistanceFunctionUpDown;
        private Button CreateBoundaryButton;
        private Label label4;
        private NumericUpDown SeaLevelUpDown;
        private Label label3;
        private NumericUpDown LerpWeightUpDown;
        private Label label2;
        private NumericUpDown NoiseScaleUpDown;
        private Button AssignHeightButton;
        private Button GenerateCellsButton;
        private Button GeneratePointsButton;
        private Label label1;
        private NumericUpDown GridSizeUpDown;
        private Button ShowHideAdvancedButton;
        private RadioButton AtollRadio;
        private RadioButton ArchipelagoRadio;
        private RadioButton ContinentRadio;
        private RadioButton RandomLandformRadio;
        private FontAwesome.Sharp.IconButton GenerateLandformButton;
        private RadioButton EquirectangularRadio;
        private RadioButton WorldRadio;
    }
}