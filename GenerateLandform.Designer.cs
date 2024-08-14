﻿namespace MapCreator
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
            GridSizeUpDown = new NumericUpDown();
            label1 = new Label();
            GeneratePointsButton = new Button();
            GenerateCellsButton = new Button();
            AssignHeightButton = new Button();
            label2 = new Label();
            NoiseScaleUpDown = new NumericUpDown();
            label3 = new Label();
            LerpWeightUpDown = new NumericUpDown();
            label4 = new Label();
            SeaLevelUpDown = new NumericUpDown();
            CreateBoundaryButton = new Button();
            PlaceLandformButton = new Button();
            CloseButton = new Button();
            DistanceFunctionUpDown = new DomainUpDown();
            label5 = new Label();
            RemoveDisconnectedCellsButton = new Button();
            SmoothingTrack = new TrackBar();
            label6 = new Label();
            label7 = new Label();
            VariationTrack = new TrackBar();
            label8 = new Label();
            ScaleTrack = new TrackBar();
            label9 = new Label();
            RotationTrack = new TrackBar();
            label10 = new Label();
            RoughnessTrack = new TrackBar();
            ((System.ComponentModel.ISupportInitialize)LandformPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)GridSizeUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)NoiseScaleUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)LerpWeightUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)SeaLevelUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)SmoothingTrack).BeginInit();
            ((System.ComponentModel.ISupportInitialize)VariationTrack).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ScaleTrack).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RotationTrack).BeginInit();
            ((System.ComponentModel.ISupportInitialize)RoughnessTrack).BeginInit();
            SuspendLayout();
            // 
            // LandformPictureBox
            // 
            LandformPictureBox.BackColor = Color.White;
            LandformPictureBox.Location = new Point(215, 10);
            LandformPictureBox.Name = "LandformPictureBox";
            LandformPictureBox.Size = new Size(640, 480);
            LandformPictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
            LandformPictureBox.TabIndex = 0;
            LandformPictureBox.TabStop = false;
            // 
            // GridSizeUpDown
            // 
            GridSizeUpDown.Location = new Point(141, 10);
            GridSizeUpDown.Maximum = new decimal(new int[] { 50, 0, 0, 0 });
            GridSizeUpDown.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            GridSizeUpDown.Name = "GridSizeUpDown";
            GridSizeUpDown.Size = new Size(64, 23);
            GridSizeUpDown.TabIndex = 1;
            GridSizeUpDown.TextAlign = HorizontalAlignment.Center;
            GridSizeUpDown.Value = new decimal(new int[] { 25, 0, 0, 0 });
            GridSizeUpDown.ValueChanged += GridSizeUpDown_ValueChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(77, 12);
            label1.Name = "label1";
            label1.Size = new Size(58, 16);
            label1.TabIndex = 2;
            label1.Text = "Grid Size";
            // 
            // GeneratePointsButton
            // 
            GeneratePointsButton.Location = new Point(12, 39);
            GeneratePointsButton.Name = "GeneratePointsButton";
            GeneratePointsButton.Size = new Size(193, 28);
            GeneratePointsButton.TabIndex = 3;
            GeneratePointsButton.Text = "Generate Points";
            GeneratePointsButton.UseVisualStyleBackColor = true;
            GeneratePointsButton.Click += GeneratePointsButton_Click;
            // 
            // GenerateCellsButton
            // 
            GenerateCellsButton.Location = new Point(12, 73);
            GenerateCellsButton.Name = "GenerateCellsButton";
            GenerateCellsButton.Size = new Size(193, 28);
            GenerateCellsButton.TabIndex = 4;
            GenerateCellsButton.Text = "Generate Cells";
            GenerateCellsButton.UseVisualStyleBackColor = true;
            GenerateCellsButton.Click += GenerateCellsButton_Click;
            // 
            // AssignHeightButton
            // 
            AssignHeightButton.Location = new Point(12, 210);
            AssignHeightButton.Name = "AssignHeightButton";
            AssignHeightButton.Size = new Size(193, 28);
            AssignHeightButton.TabIndex = 5;
            AssignHeightButton.Text = "Assign Cell Height";
            AssignHeightButton.UseVisualStyleBackColor = true;
            AssignHeightButton.Click += AssignHeightButton_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(62, 109);
            label2.Name = "label2";
            label2.Size = new Size(73, 16);
            label2.TabIndex = 7;
            label2.Text = "Noise Scale";
            // 
            // NoiseScaleUpDown
            // 
            NoiseScaleUpDown.DecimalPlaces = 2;
            NoiseScaleUpDown.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            NoiseScaleUpDown.Location = new Point(141, 107);
            NoiseScaleUpDown.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            NoiseScaleUpDown.Name = "NoiseScaleUpDown";
            NoiseScaleUpDown.Size = new Size(64, 23);
            NoiseScaleUpDown.TabIndex = 6;
            NoiseScaleUpDown.TextAlign = HorizontalAlignment.Center;
            NoiseScaleUpDown.Value = new decimal(new int[] { 1, 0, 0, 65536 });
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 138);
            label3.Name = "label3";
            label3.Size = new Size(123, 16);
            label3.TabIndex = 9;
            label3.Text = "Interpolation Weight";
            // 
            // LerpWeightUpDown
            // 
            LerpWeightUpDown.DecimalPlaces = 2;
            LerpWeightUpDown.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            LerpWeightUpDown.Location = new Point(141, 136);
            LerpWeightUpDown.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            LerpWeightUpDown.Name = "LerpWeightUpDown";
            LerpWeightUpDown.Size = new Size(64, 23);
            LerpWeightUpDown.TabIndex = 8;
            LerpWeightUpDown.TextAlign = HorizontalAlignment.Center;
            LerpWeightUpDown.Value = new decimal(new int[] { 5, 0, 0, 65536 });
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(33, 246);
            label4.Name = "label4";
            label4.Size = new Size(102, 16);
            label4.TabIndex = 11;
            label4.Text = "Sea Level Height";
            // 
            // SeaLevelUpDown
            // 
            SeaLevelUpDown.DecimalPlaces = 2;
            SeaLevelUpDown.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            SeaLevelUpDown.Location = new Point(141, 244);
            SeaLevelUpDown.Maximum = new decimal(new int[] { 1, 0, 0, 0 });
            SeaLevelUpDown.Name = "SeaLevelUpDown";
            SeaLevelUpDown.Size = new Size(64, 23);
            SeaLevelUpDown.TabIndex = 10;
            SeaLevelUpDown.TextAlign = HorizontalAlignment.Center;
            SeaLevelUpDown.Value = new decimal(new int[] { 5, 0, 0, 65536 });
            SeaLevelUpDown.ValueChanged += SeaLevelUpDown_ValueChanged;
            // 
            // CreateBoundaryButton
            // 
            CreateBoundaryButton.Location = new Point(12, 417);
            CreateBoundaryButton.Name = "CreateBoundaryButton";
            CreateBoundaryButton.Size = new Size(193, 28);
            CreateBoundaryButton.TabIndex = 12;
            CreateBoundaryButton.Text = "Create Landform Boundary";
            CreateBoundaryButton.UseVisualStyleBackColor = true;
            CreateBoundaryButton.Click += CreateBoundaryButton_Click;
            // 
            // PlaceLandformButton
            // 
            PlaceLandformButton.Location = new Point(12, 468);
            PlaceLandformButton.Name = "PlaceLandformButton";
            PlaceLandformButton.Size = new Size(75, 48);
            PlaceLandformButton.TabIndex = 13;
            PlaceLandformButton.Text = "Place";
            PlaceLandformButton.UseVisualStyleBackColor = true;
            // 
            // CloseButton
            // 
            CloseButton.Location = new Point(130, 468);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new Size(75, 48);
            CloseButton.TabIndex = 14;
            CloseButton.Text = "Close";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += CloseButton_Click;
            // 
            // DistanceFunctionUpDown
            // 
            DistanceFunctionUpDown.Items.Add("Distance Squared");
            DistanceFunctionUpDown.Items.Add("Euclidean Squared");
            DistanceFunctionUpDown.Items.Add("Square Bump");
            DistanceFunctionUpDown.Items.Add("Hyperboloid");
            DistanceFunctionUpDown.Items.Add("Trigonometric Product");
            DistanceFunctionUpDown.Items.Add("Squircle");
            DistanceFunctionUpDown.Items.Add("Smooth Minimum");
            DistanceFunctionUpDown.Location = new Point(12, 181);
            DistanceFunctionUpDown.Name = "DistanceFunctionUpDown";
            DistanceFunctionUpDown.Size = new Size(193, 23);
            DistanceFunctionUpDown.TabIndex = 15;
            DistanceFunctionUpDown.Text = "Distance Squared";
            DistanceFunctionUpDown.TextAlign = HorizontalAlignment.Center;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 162);
            label5.Name = "label5";
            label5.Size = new Size(107, 16);
            label5.TabIndex = 16;
            label5.Text = "Distance Function";
            // 
            // RemoveDisconnectedCellsButton
            // 
            RemoveDisconnectedCellsButton.Location = new Point(12, 273);
            RemoveDisconnectedCellsButton.Name = "RemoveDisconnectedCellsButton";
            RemoveDisconnectedCellsButton.Size = new Size(193, 28);
            RemoveDisconnectedCellsButton.TabIndex = 17;
            RemoveDisconnectedCellsButton.Text = "Remove Disconnected Cells";
            RemoveDisconnectedCellsButton.UseVisualStyleBackColor = true;
            RemoveDisconnectedCellsButton.Click += RemoveDisconnectedCellsButton_Click;
            // 
            // SmoothingTrack
            // 
            SmoothingTrack.AutoSize = false;
            SmoothingTrack.Location = new Point(77, 309);
            SmoothingTrack.Maximum = 200;
            SmoothingTrack.Minimum = 1;
            SmoothingTrack.Name = "SmoothingTrack";
            SmoothingTrack.Size = new Size(128, 18);
            SmoothingTrack.TabIndex = 18;
            SmoothingTrack.TickStyle = TickStyle.None;
            SmoothingTrack.Value = 50;
            SmoothingTrack.ValueChanged += SmoothingTrack_ValueChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(12, 309);
            label6.Name = "label6";
            label6.Size = new Size(68, 16);
            label6.TabIndex = 19;
            label6.Text = "Smoothing";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(22, 329);
            label7.Name = "label7";
            label7.Size = new Size(58, 16);
            label7.TabIndex = 21;
            label7.Text = "Variation";
            // 
            // VariationTrack
            // 
            VariationTrack.AutoSize = false;
            VariationTrack.Location = new Point(77, 329);
            VariationTrack.Minimum = 1;
            VariationTrack.Name = "VariationTrack";
            VariationTrack.Size = new Size(128, 18);
            VariationTrack.TabIndex = 20;
            VariationTrack.TickStyle = TickStyle.None;
            VariationTrack.Value = 5;
            VariationTrack.ValueChanged += VariationTrack_ValueChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(33, 373);
            label8.Name = "label8";
            label8.Size = new Size(38, 16);
            label8.TabIndex = 23;
            label8.Text = "Scale";
            // 
            // ScaleTrack
            // 
            ScaleTrack.AutoSize = false;
            ScaleTrack.Location = new Point(77, 373);
            ScaleTrack.Maximum = 100;
            ScaleTrack.Minimum = 10;
            ScaleTrack.Name = "ScaleTrack";
            ScaleTrack.Size = new Size(128, 18);
            ScaleTrack.TabIndex = 22;
            ScaleTrack.TickStyle = TickStyle.None;
            ScaleTrack.Value = 100;
            ScaleTrack.ValueChanged += ScaleTrack_ValueChanged;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(17, 393);
            label9.Name = "label9";
            label9.Size = new Size(54, 16);
            label9.TabIndex = 25;
            label9.Text = "Rotation";
            // 
            // RotationTrack
            // 
            RotationTrack.AutoSize = false;
            RotationTrack.Location = new Point(77, 393);
            RotationTrack.Maximum = 359;
            RotationTrack.Name = "RotationTrack";
            RotationTrack.Size = new Size(128, 18);
            RotationTrack.TabIndex = 24;
            RotationTrack.TickStyle = TickStyle.None;
            RotationTrack.ValueChanged += RotationTrack_ValueChanged;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(11, 349);
            label10.Name = "label10";
            label10.Size = new Size(69, 16);
            label10.TabIndex = 27;
            label10.Text = "Roughness";
            // 
            // RoughnessTrack
            // 
            RoughnessTrack.AutoSize = false;
            RoughnessTrack.Location = new Point(77, 349);
            RoughnessTrack.Maximum = 50;
            RoughnessTrack.Minimum = 2;
            RoughnessTrack.Name = "RoughnessTrack";
            RoughnessTrack.Size = new Size(128, 18);
            RoughnessTrack.TabIndex = 26;
            RoughnessTrack.TickStyle = TickStyle.None;
            RoughnessTrack.Value = 20;
            RoughnessTrack.ValueChanged += RoughnessTrack_ValueChanged;
            // 
            // GenerateLandform
            // 
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = CloseButton;
            ClientSize = new Size(867, 528);
            Controls.Add(label10);
            Controls.Add(RoughnessTrack);
            Controls.Add(label9);
            Controls.Add(RotationTrack);
            Controls.Add(label8);
            Controls.Add(ScaleTrack);
            Controls.Add(label7);
            Controls.Add(VariationTrack);
            Controls.Add(label6);
            Controls.Add(SmoothingTrack);
            Controls.Add(RemoveDisconnectedCellsButton);
            Controls.Add(label5);
            Controls.Add(DistanceFunctionUpDown);
            Controls.Add(CloseButton);
            Controls.Add(PlaceLandformButton);
            Controls.Add(CreateBoundaryButton);
            Controls.Add(label4);
            Controls.Add(SeaLevelUpDown);
            Controls.Add(label3);
            Controls.Add(LerpWeightUpDown);
            Controls.Add(label2);
            Controls.Add(NoiseScaleUpDown);
            Controls.Add(AssignHeightButton);
            Controls.Add(GenerateCellsButton);
            Controls.Add(GeneratePointsButton);
            Controls.Add(label1);
            Controls.Add(GridSizeUpDown);
            Controls.Add(LandformPictureBox);
            Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "GenerateLandform";
            Text = "Generate Landform";
            ((System.ComponentModel.ISupportInitialize)LandformPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)GridSizeUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)NoiseScaleUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)LerpWeightUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)SeaLevelUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)SmoothingTrack).EndInit();
            ((System.ComponentModel.ISupportInitialize)VariationTrack).EndInit();
            ((System.ComponentModel.ISupportInitialize)ScaleTrack).EndInit();
            ((System.ComponentModel.ISupportInitialize)RotationTrack).EndInit();
            ((System.ComponentModel.ISupportInitialize)RoughnessTrack).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox LandformPictureBox;
        private NumericUpDown GridSizeUpDown;
        private Label label1;
        private Button GeneratePointsButton;
        private Button GenerateCellsButton;
        private Button AssignHeightButton;
        private Label label2;
        private NumericUpDown NoiseScaleUpDown;
        private Label label3;
        private NumericUpDown LerpWeightUpDown;
        private Label label4;
        private NumericUpDown SeaLevelUpDown;
        private Button CreateBoundaryButton;
        private Button PlaceLandformButton;
        private Button CloseButton;
        private DomainUpDown DistanceFunctionUpDown;
        private Label label5;
        private Button RemoveDisconnectedCellsButton;
        private TrackBar SmoothingTrack;
        private Label label6;
        private Label label7;
        private TrackBar VariationTrack;
        private Label label8;
        private TrackBar ScaleTrack;
        private Label label9;
        private TrackBar RotationTrack;
        private Label label10;
        private TrackBar RoughnessTrack;
    }
}