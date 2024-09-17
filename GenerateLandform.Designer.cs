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
            GenerationStatusLabel = new Label();
            panel1 = new Panel();
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
            panel1.SuspendLayout();
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
            // GenerationStatusLabel
            // 
            GenerationStatusLabel.BackColor = SystemColors.Control;
            GenerationStatusLabel.Location = new Point(254, 482);
            GenerationStatusLabel.Name = "GenerationStatusLabel";
            GenerationStatusLabel.Size = new Size(397, 30);
            GenerationStatusLabel.TabIndex = 29;
            GenerationStatusLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            panel1.Controls.Add(label10);
            panel1.Controls.Add(RoughnessTrack);
            panel1.Controls.Add(label9);
            panel1.Controls.Add(RotationTrack);
            panel1.Controls.Add(label8);
            panel1.Controls.Add(ScaleTrack);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(VariationTrack);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(SmoothingTrack);
            panel1.Controls.Add(RemoveDisconnectedCellsButton);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(DistanceFunctionUpDown);
            panel1.Controls.Add(CreateBoundaryButton);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(SeaLevelUpDown);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(LerpWeightUpDown);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(NoiseScaleUpDown);
            panel1.Controls.Add(AssignHeightButton);
            panel1.Controls.Add(GenerateCellsButton);
            panel1.Controls.Add(GeneratePointsButton);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(GridSizeUpDown);
            panel1.Location = new Point(12, 12);
            panel1.Name = "panel1";
            panel1.Size = new Size(236, 467);
            panel1.TabIndex = 30;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(17, 353);
            label10.Name = "label10";
            label10.Size = new Size(69, 16);
            label10.TabIndex = 77;
            label10.Text = "Roughness";
            // 
            // RoughnessTrack
            // 
            RoughnessTrack.AutoSize = false;
            RoughnessTrack.Location = new Point(83, 353);
            RoughnessTrack.Maximum = 50;
            RoughnessTrack.Minimum = 2;
            RoughnessTrack.Name = "RoughnessTrack";
            RoughnessTrack.Size = new Size(128, 18);
            RoughnessTrack.TabIndex = 76;
            RoughnessTrack.TickStyle = TickStyle.None;
            RoughnessTrack.Value = 20;
            RoughnessTrack.ValueChanged += RoughnessTrack_ValueChanged;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(23, 397);
            label9.Name = "label9";
            label9.Size = new Size(54, 16);
            label9.TabIndex = 75;
            label9.Text = "Rotation";
            // 
            // RotationTrack
            // 
            RotationTrack.AutoSize = false;
            RotationTrack.Location = new Point(83, 397);
            RotationTrack.Maximum = 359;
            RotationTrack.Name = "RotationTrack";
            RotationTrack.Size = new Size(128, 18);
            RotationTrack.TabIndex = 74;
            RotationTrack.TickStyle = TickStyle.None;
            RotationTrack.ValueChanged += RotationTrack_ValueChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(39, 377);
            label8.Name = "label8";
            label8.Size = new Size(38, 16);
            label8.TabIndex = 73;
            label8.Text = "Scale";
            // 
            // ScaleTrack
            // 
            ScaleTrack.AutoSize = false;
            ScaleTrack.Location = new Point(83, 377);
            ScaleTrack.Maximum = 100;
            ScaleTrack.Minimum = 10;
            ScaleTrack.Name = "ScaleTrack";
            ScaleTrack.Size = new Size(128, 18);
            ScaleTrack.TabIndex = 72;
            ScaleTrack.TickStyle = TickStyle.None;
            ScaleTrack.Value = 100;
            ScaleTrack.ValueChanged += ScaleTrack_ValueChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(28, 333);
            label7.Name = "label7";
            label7.Size = new Size(58, 16);
            label7.TabIndex = 71;
            label7.Text = "Variation";
            // 
            // VariationTrack
            // 
            VariationTrack.AutoSize = false;
            VariationTrack.Location = new Point(83, 333);
            VariationTrack.Minimum = 1;
            VariationTrack.Name = "VariationTrack";
            VariationTrack.Size = new Size(128, 18);
            VariationTrack.TabIndex = 70;
            VariationTrack.TickStyle = TickStyle.None;
            VariationTrack.Value = 5;
            VariationTrack.ValueChanged += VariationTrack_ValueChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(18, 313);
            label6.Name = "label6";
            label6.Size = new Size(68, 16);
            label6.TabIndex = 69;
            label6.Text = "Smoothing";
            // 
            // SmoothingTrack
            // 
            SmoothingTrack.AutoSize = false;
            SmoothingTrack.Location = new Point(83, 313);
            SmoothingTrack.Maximum = 200;
            SmoothingTrack.Minimum = 1;
            SmoothingTrack.Name = "SmoothingTrack";
            SmoothingTrack.Size = new Size(128, 18);
            SmoothingTrack.TabIndex = 68;
            SmoothingTrack.TickStyle = TickStyle.None;
            SmoothingTrack.Value = 50;
            SmoothingTrack.ValueChanged += SmoothingTrack_ValueChanged;
            // 
            // RemoveDisconnectedCellsButton
            // 
            RemoveDisconnectedCellsButton.BackColor = SystemColors.GradientActiveCaption;
            RemoveDisconnectedCellsButton.Location = new Point(18, 277);
            RemoveDisconnectedCellsButton.Name = "RemoveDisconnectedCellsButton";
            RemoveDisconnectedCellsButton.Size = new Size(193, 28);
            RemoveDisconnectedCellsButton.TabIndex = 67;
            RemoveDisconnectedCellsButton.Text = "Remove Disconnected Cells";
            RemoveDisconnectedCellsButton.UseVisualStyleBackColor = false;
            RemoveDisconnectedCellsButton.Click += RemoveDisconnectedCellsButton_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(18, 166);
            label5.Name = "label5";
            label5.Size = new Size(107, 16);
            label5.TabIndex = 66;
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
            DistanceFunctionUpDown.Location = new Point(18, 185);
            DistanceFunctionUpDown.Name = "DistanceFunctionUpDown";
            DistanceFunctionUpDown.Size = new Size(193, 23);
            DistanceFunctionUpDown.TabIndex = 65;
            DistanceFunctionUpDown.Text = "Distance Squared";
            DistanceFunctionUpDown.TextAlign = HorizontalAlignment.Center;
            // 
            // CreateBoundaryButton
            // 
            CreateBoundaryButton.BackColor = SystemColors.GradientActiveCaption;
            CreateBoundaryButton.Location = new Point(18, 421);
            CreateBoundaryButton.Name = "CreateBoundaryButton";
            CreateBoundaryButton.Size = new Size(193, 28);
            CreateBoundaryButton.TabIndex = 64;
            CreateBoundaryButton.Text = "Create Landform Boundary";
            CreateBoundaryButton.UseVisualStyleBackColor = false;
            CreateBoundaryButton.Click += CreateBoundaryButton_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(39, 250);
            label4.Name = "label4";
            label4.Size = new Size(102, 16);
            label4.TabIndex = 63;
            label4.Text = "Sea Level Height";
            // 
            // SeaLevelUpDown
            // 
            SeaLevelUpDown.DecimalPlaces = 2;
            SeaLevelUpDown.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            SeaLevelUpDown.Location = new Point(147, 248);
            SeaLevelUpDown.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            SeaLevelUpDown.Name = "SeaLevelUpDown";
            SeaLevelUpDown.Size = new Size(64, 23);
            SeaLevelUpDown.TabIndex = 62;
            SeaLevelUpDown.TextAlign = HorizontalAlignment.Center;
            SeaLevelUpDown.Value = new decimal(new int[] { 5, 0, 0, 65536 });
            SeaLevelUpDown.ValueChanged += SeaLevelUpDown_ValueChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(18, 142);
            label3.Name = "label3";
            label3.Size = new Size(123, 16);
            label3.TabIndex = 61;
            label3.Text = "Interpolation Weight";
            // 
            // LerpWeightUpDown
            // 
            LerpWeightUpDown.DecimalPlaces = 2;
            LerpWeightUpDown.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            LerpWeightUpDown.Location = new Point(147, 140);
            LerpWeightUpDown.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            LerpWeightUpDown.Name = "LerpWeightUpDown";
            LerpWeightUpDown.Size = new Size(64, 23);
            LerpWeightUpDown.TabIndex = 60;
            LerpWeightUpDown.TextAlign = HorizontalAlignment.Center;
            LerpWeightUpDown.Value = new decimal(new int[] { 5, 0, 0, 65536 });
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(68, 113);
            label2.Name = "label2";
            label2.Size = new Size(73, 16);
            label2.TabIndex = 59;
            label2.Text = "Noise Scale";
            // 
            // NoiseScaleUpDown
            // 
            NoiseScaleUpDown.DecimalPlaces = 2;
            NoiseScaleUpDown.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            NoiseScaleUpDown.Location = new Point(147, 111);
            NoiseScaleUpDown.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            NoiseScaleUpDown.Name = "NoiseScaleUpDown";
            NoiseScaleUpDown.Size = new Size(64, 23);
            NoiseScaleUpDown.TabIndex = 58;
            NoiseScaleUpDown.TextAlign = HorizontalAlignment.Center;
            NoiseScaleUpDown.Value = new decimal(new int[] { 1, 0, 0, 65536 });
            // 
            // AssignHeightButton
            // 
            AssignHeightButton.BackColor = SystemColors.GradientActiveCaption;
            AssignHeightButton.Location = new Point(18, 214);
            AssignHeightButton.Name = "AssignHeightButton";
            AssignHeightButton.Size = new Size(193, 28);
            AssignHeightButton.TabIndex = 57;
            AssignHeightButton.Text = "Assign Cell Height";
            AssignHeightButton.UseVisualStyleBackColor = false;
            AssignHeightButton.Click += AssignHeightButton_Click;
            // 
            // GenerateCellsButton
            // 
            GenerateCellsButton.BackColor = SystemColors.GradientActiveCaption;
            GenerateCellsButton.Location = new Point(18, 77);
            GenerateCellsButton.Name = "GenerateCellsButton";
            GenerateCellsButton.Size = new Size(193, 28);
            GenerateCellsButton.TabIndex = 56;
            GenerateCellsButton.Text = "Generate Cells";
            GenerateCellsButton.UseVisualStyleBackColor = false;
            GenerateCellsButton.Click += GenerateCellsButton_Click;
            // 
            // GeneratePointsButton
            // 
            GeneratePointsButton.BackColor = SystemColors.GradientActiveCaption;
            GeneratePointsButton.Location = new Point(18, 43);
            GeneratePointsButton.Name = "GeneratePointsButton";
            GeneratePointsButton.Size = new Size(193, 28);
            GeneratePointsButton.TabIndex = 55;
            GeneratePointsButton.Text = "Generate Points";
            GeneratePointsButton.UseVisualStyleBackColor = false;
            GeneratePointsButton.Click += GeneratePointsButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(83, 16);
            label1.Name = "label1";
            label1.Size = new Size(58, 16);
            label1.TabIndex = 54;
            label1.Text = "Grid Size";
            // 
            // GridSizeUpDown
            // 
            GridSizeUpDown.Location = new Point(147, 14);
            GridSizeUpDown.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            GridSizeUpDown.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            GridSizeUpDown.Name = "GridSizeUpDown";
            GridSizeUpDown.Size = new Size(64, 23);
            GridSizeUpDown.TabIndex = 53;
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
            Controls.Add(panel1);
            Controls.Add(GenerationStatusLabel);
            Controls.Add(CloseButton);
            Controls.Add(PlaceLandformButton);
            Controls.Add(LandformPictureBox);
            Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "GenerateLandform";
            Text = "Generate Landform";
            ((System.ComponentModel.ISupportInitialize)LandformPictureBox).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
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
        private Label GenerationStatusLabel;
        private Panel panel1;
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
    }
}