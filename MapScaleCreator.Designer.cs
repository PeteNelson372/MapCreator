namespace MapCreator
{
    partial class MapScaleCreator
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
            ScaleWidthUpDown = new NumericUpDown();
            ScaleHeightUpDown = new NumericUpDown();
            ScaleSegmentCountTrack = new TrackBar();
            ScaleLineWidthTrack = new TrackBar();
            ScaleColor3Label = new Label();
            ScaleColor2Label = new Label();
            ScaleColor1Label = new Label();
            ScaleSegmentDistanceUpDown = new NumericUpDown();
            ScaleNumbersDisplayCheckList = new CheckedListBox();
            groupBox12 = new GroupBox();
            ScaleFontColorOpacityTrack = new TrackBar();
            ScaleFontColorOpacityLabel = new Label();
            label69 = new Label();
            label61 = new Label();
            ScaleFontColorSelectLabel = new Label();
            SelectScaleFontButton = new Button();
            groupBox11 = new GroupBox();
            ScaleOutlineColorOpacityTrack = new TrackBar();
            ScaleOutlineColorOpacityLabel = new Label();
            label68 = new Label();
            label63 = new Label();
            ScaleOutlineWidthUpDown = new NumericUpDown();
            label60 = new Label();
            ScaleOutlineColorSelectLabel = new Label();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            ScaleSegmentCountUpDown = new NumericUpDown();
            ScaleLineWidthUpDown = new NumericUpDown();
            label4 = new Label();
            label5 = new Label();
            ResetScaleColorsButton = new FontAwesome.Sharp.IconButton();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            ScaleUnitsTextBox = new TextBox();
            label9 = new Label();
            label10 = new Label();
            CreateScaleButton = new Button();
            DeleteScaleButton = new Button();
            CloseButton = new Button();
            ((System.ComponentModel.ISupportInitialize)ScaleWidthUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ScaleHeightUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ScaleSegmentCountTrack).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ScaleLineWidthTrack).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ScaleSegmentDistanceUpDown).BeginInit();
            groupBox12.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ScaleFontColorOpacityTrack).BeginInit();
            groupBox11.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ScaleOutlineColorOpacityTrack).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ScaleOutlineWidthUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ScaleSegmentCountUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ScaleLineWidthUpDown).BeginInit();
            SuspendLayout();
            // 
            // ScaleWidthUpDown
            // 
            ScaleWidthUpDown.Location = new Point(82, 29);
            ScaleWidthUpDown.Maximum = new decimal(new int[] { 2048, 0, 0, 0 });
            ScaleWidthUpDown.Minimum = new decimal(new int[] { 64, 0, 0, 0 });
            ScaleWidthUpDown.Name = "ScaleWidthUpDown";
            ScaleWidthUpDown.Size = new Size(60, 23);
            ScaleWidthUpDown.TabIndex = 0;
            ScaleWidthUpDown.Value = new decimal(new int[] { 256, 0, 0, 0 });
            // 
            // ScaleHeightUpDown
            // 
            ScaleHeightUpDown.Location = new Point(239, 29);
            ScaleHeightUpDown.Maximum = new decimal(new int[] { 256, 0, 0, 0 });
            ScaleHeightUpDown.Minimum = new decimal(new int[] { 4, 0, 0, 0 });
            ScaleHeightUpDown.Name = "ScaleHeightUpDown";
            ScaleHeightUpDown.Size = new Size(60, 23);
            ScaleHeightUpDown.TabIndex = 1;
            ScaleHeightUpDown.Value = new decimal(new int[] { 16, 0, 0, 0 });
            // 
            // ScaleSegmentCountTrack
            // 
            ScaleSegmentCountTrack.AutoSize = false;
            ScaleSegmentCountTrack.Location = new Point(82, 67);
            ScaleSegmentCountTrack.Maximum = 32;
            ScaleSegmentCountTrack.Minimum = 1;
            ScaleSegmentCountTrack.Name = "ScaleSegmentCountTrack";
            ScaleSegmentCountTrack.Size = new Size(151, 32);
            ScaleSegmentCountTrack.TabIndex = 2;
            ScaleSegmentCountTrack.TickStyle = TickStyle.None;
            ScaleSegmentCountTrack.Value = 5;
            // 
            // ScaleLineWidthTrack
            // 
            ScaleLineWidthTrack.AutoSize = false;
            ScaleLineWidthTrack.Location = new Point(82, 105);
            ScaleLineWidthTrack.Maximum = 8;
            ScaleLineWidthTrack.Minimum = 2;
            ScaleLineWidthTrack.Name = "ScaleLineWidthTrack";
            ScaleLineWidthTrack.Size = new Size(151, 32);
            ScaleLineWidthTrack.TabIndex = 3;
            ScaleLineWidthTrack.TickStyle = TickStyle.None;
            ScaleLineWidthTrack.Value = 3;
            // 
            // ScaleColor3Label
            // 
            ScaleColor3Label.BackColor = Color.Black;
            ScaleColor3Label.BorderStyle = BorderStyle.FixedSingle;
            ScaleColor3Label.Location = new Point(166, 140);
            ScaleColor3Label.Name = "ScaleColor3Label";
            ScaleColor3Label.Size = new Size(26, 28);
            ScaleColor3Label.TabIndex = 80;
            ScaleColor3Label.Click += ScaleColor3Label_Click;
            // 
            // ScaleColor2Label
            // 
            ScaleColor2Label.BackColor = Color.White;
            ScaleColor2Label.BorderStyle = BorderStyle.FixedSingle;
            ScaleColor2Label.Location = new Point(134, 140);
            ScaleColor2Label.Name = "ScaleColor2Label";
            ScaleColor2Label.Size = new Size(26, 28);
            ScaleColor2Label.TabIndex = 79;
            ScaleColor2Label.Click += ScaleColor2Label_Click;
            // 
            // ScaleColor1Label
            // 
            ScaleColor1Label.BackColor = Color.Black;
            ScaleColor1Label.BorderStyle = BorderStyle.FixedSingle;
            ScaleColor1Label.Location = new Point(102, 140);
            ScaleColor1Label.Name = "ScaleColor1Label";
            ScaleColor1Label.Size = new Size(26, 28);
            ScaleColor1Label.TabIndex = 78;
            ScaleColor1Label.Click += ScaleColor1Label_Click;
            // 
            // ScaleSegmentDistanceUpDown
            // 
            ScaleSegmentDistanceUpDown.DecimalPlaces = 1;
            ScaleSegmentDistanceUpDown.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            ScaleSegmentDistanceUpDown.Location = new Point(82, 193);
            ScaleSegmentDistanceUpDown.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
            ScaleSegmentDistanceUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 65536 });
            ScaleSegmentDistanceUpDown.Name = "ScaleSegmentDistanceUpDown";
            ScaleSegmentDistanceUpDown.Size = new Size(60, 23);
            ScaleSegmentDistanceUpDown.TabIndex = 81;
            ScaleSegmentDistanceUpDown.Value = new decimal(new int[] { 100, 0, 0, 0 });
            // 
            // ScaleNumbersDisplayCheckList
            // 
            ScaleNumbersDisplayCheckList.CheckOnClick = true;
            ScaleNumbersDisplayCheckList.FormattingEnabled = true;
            ScaleNumbersDisplayCheckList.Items.AddRange(new object[] { "None", "Ends", "Every Other", "All" });
            ScaleNumbersDisplayCheckList.Location = new Point(82, 274);
            ScaleNumbersDisplayCheckList.Name = "ScaleNumbersDisplayCheckList";
            ScaleNumbersDisplayCheckList.Size = new Size(217, 76);
            ScaleNumbersDisplayCheckList.TabIndex = 83;
            // 
            // groupBox12
            // 
            groupBox12.Controls.Add(ScaleFontColorOpacityTrack);
            groupBox12.Controls.Add(ScaleFontColorOpacityLabel);
            groupBox12.Controls.Add(label69);
            groupBox12.Controls.Add(label61);
            groupBox12.Controls.Add(ScaleFontColorSelectLabel);
            groupBox12.Controls.Add(SelectScaleFontButton);
            groupBox12.Location = new Point(82, 364);
            groupBox12.Name = "groupBox12";
            groupBox12.Size = new Size(104, 199);
            groupBox12.TabIndex = 100;
            groupBox12.TabStop = false;
            groupBox12.Text = "Font";
            // 
            // ScaleFontColorOpacityTrack
            // 
            ScaleFontColorOpacityTrack.AutoSize = false;
            ScaleFontColorOpacityTrack.Location = new Point(6, 165);
            ScaleFontColorOpacityTrack.Maximum = 255;
            ScaleFontColorOpacityTrack.Name = "ScaleFontColorOpacityTrack";
            ScaleFontColorOpacityTrack.Size = new Size(92, 21);
            ScaleFontColorOpacityTrack.TabIndex = 109;
            ScaleFontColorOpacityTrack.TickStyle = TickStyle.None;
            ScaleFontColorOpacityTrack.Value = 255;
            ScaleFontColorOpacityTrack.Scroll += ScaleFontColorOpacityTrack_Scroll;
            // 
            // ScaleFontColorOpacityLabel
            // 
            ScaleFontColorOpacityLabel.CausesValidation = false;
            ScaleFontColorOpacityLabel.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ScaleFontColorOpacityLabel.Location = new Point(68, 148);
            ScaleFontColorOpacityLabel.Name = "ScaleFontColorOpacityLabel";
            ScaleFontColorOpacityLabel.Size = new Size(30, 15);
            ScaleFontColorOpacityLabel.TabIndex = 108;
            ScaleFontColorOpacityLabel.Text = "255";
            ScaleFontColorOpacityLabel.TextAlign = ContentAlignment.MiddleRight;
            ScaleFontColorOpacityLabel.UseMnemonic = false;
            // 
            // label69
            // 
            label69.AutoSize = true;
            label69.CausesValidation = false;
            label69.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label69.Location = new Point(6, 149);
            label69.Name = "label69";
            label69.Size = new Size(44, 13);
            label69.TabIndex = 107;
            label69.Text = "Opacity";
            label69.UseMnemonic = false;
            // 
            // label61
            // 
            label61.AutoSize = true;
            label61.Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label61.Location = new Point(6, 69);
            label61.Name = "label61";
            label61.Size = new Size(37, 16);
            label61.TabIndex = 75;
            label61.Text = "Color";
            // 
            // ScaleFontColorSelectLabel
            // 
            ScaleFontColorSelectLabel.BackColor = Color.White;
            ScaleFontColorSelectLabel.BorderStyle = BorderStyle.FixedSingle;
            ScaleFontColorSelectLabel.Font = new Font("Tahoma", 8F);
            ScaleFontColorSelectLabel.ForeColor = SystemColors.ButtonShadow;
            ScaleFontColorSelectLabel.Location = new Point(6, 94);
            ScaleFontColorSelectLabel.Name = "ScaleFontColorSelectLabel";
            ScaleFontColorSelectLabel.Size = new Size(92, 40);
            ScaleFontColorSelectLabel.TabIndex = 74;
            ScaleFontColorSelectLabel.Text = "Click to Select";
            ScaleFontColorSelectLabel.TextAlign = ContentAlignment.MiddleCenter;
            ScaleFontColorSelectLabel.Click += ScaleFontColorSelectLabel_Click;
            // 
            // SelectScaleFontButton
            // 
            SelectScaleFontButton.Location = new Point(6, 22);
            SelectScaleFontButton.Name = "SelectScaleFontButton";
            SelectScaleFontButton.Size = new Size(92, 41);
            SelectScaleFontButton.TabIndex = 5;
            SelectScaleFontButton.Text = "Select Font";
            SelectScaleFontButton.UseVisualStyleBackColor = true;
            SelectScaleFontButton.Click += SelectScaleFontButton_Click;
            // 
            // groupBox11
            // 
            groupBox11.Controls.Add(ScaleOutlineColorOpacityTrack);
            groupBox11.Controls.Add(ScaleOutlineColorOpacityLabel);
            groupBox11.Controls.Add(label68);
            groupBox11.Controls.Add(label63);
            groupBox11.Controls.Add(ScaleOutlineWidthUpDown);
            groupBox11.Controls.Add(label60);
            groupBox11.Controls.Add(ScaleOutlineColorSelectLabel);
            groupBox11.Location = new Point(195, 364);
            groupBox11.Name = "groupBox11";
            groupBox11.Size = new Size(104, 199);
            groupBox11.TabIndex = 101;
            groupBox11.TabStop = false;
            groupBox11.Text = "Outline";
            // 
            // ScaleOutlineColorOpacityTrack
            // 
            ScaleOutlineColorOpacityTrack.AutoSize = false;
            ScaleOutlineColorOpacityTrack.Location = new Point(6, 165);
            ScaleOutlineColorOpacityTrack.Maximum = 255;
            ScaleOutlineColorOpacityTrack.Name = "ScaleOutlineColorOpacityTrack";
            ScaleOutlineColorOpacityTrack.Size = new Size(92, 21);
            ScaleOutlineColorOpacityTrack.TabIndex = 106;
            ScaleOutlineColorOpacityTrack.TickStyle = TickStyle.None;
            ScaleOutlineColorOpacityTrack.Value = 255;
            ScaleOutlineColorOpacityTrack.Scroll += ScaleOutlineColorOpacityTrack_Scroll;
            // 
            // ScaleOutlineColorOpacityLabel
            // 
            ScaleOutlineColorOpacityLabel.CausesValidation = false;
            ScaleOutlineColorOpacityLabel.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ScaleOutlineColorOpacityLabel.Location = new Point(68, 148);
            ScaleOutlineColorOpacityLabel.Name = "ScaleOutlineColorOpacityLabel";
            ScaleOutlineColorOpacityLabel.Size = new Size(30, 15);
            ScaleOutlineColorOpacityLabel.TabIndex = 105;
            ScaleOutlineColorOpacityLabel.Text = "255";
            ScaleOutlineColorOpacityLabel.TextAlign = ContentAlignment.MiddleRight;
            ScaleOutlineColorOpacityLabel.UseMnemonic = false;
            // 
            // label68
            // 
            label68.AutoSize = true;
            label68.CausesValidation = false;
            label68.Font = new Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label68.Location = new Point(6, 147);
            label68.Name = "label68";
            label68.Size = new Size(44, 13);
            label68.TabIndex = 104;
            label68.Text = "Opacity";
            label68.UseMnemonic = false;
            // 
            // label63
            // 
            label63.AutoSize = true;
            label63.Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label63.Location = new Point(6, 34);
            label63.Name = "label63";
            label63.Size = new Size(40, 16);
            label63.TabIndex = 82;
            label63.Text = "Width";
            // 
            // ScaleOutlineWidthUpDown
            // 
            ScaleOutlineWidthUpDown.Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ScaleOutlineWidthUpDown.Location = new Point(52, 32);
            ScaleOutlineWidthUpDown.Maximum = new decimal(new int[] { 32, 0, 0, 0 });
            ScaleOutlineWidthUpDown.Name = "ScaleOutlineWidthUpDown";
            ScaleOutlineWidthUpDown.Size = new Size(46, 23);
            ScaleOutlineWidthUpDown.TabIndex = 81;
            ScaleOutlineWidthUpDown.TextAlign = HorizontalAlignment.Right;
            ScaleOutlineWidthUpDown.Value = new decimal(new int[] { 2, 0, 0, 0 });
            // 
            // label60
            // 
            label60.AutoSize = true;
            label60.Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label60.Location = new Point(6, 69);
            label60.Name = "label60";
            label60.Size = new Size(37, 16);
            label60.TabIndex = 80;
            label60.Text = "Color";
            // 
            // ScaleOutlineColorSelectLabel
            // 
            ScaleOutlineColorSelectLabel.BackColor = Color.Black;
            ScaleOutlineColorSelectLabel.BorderStyle = BorderStyle.FixedSingle;
            ScaleOutlineColorSelectLabel.Font = new Font("Tahoma", 8F);
            ScaleOutlineColorSelectLabel.ForeColor = SystemColors.ButtonShadow;
            ScaleOutlineColorSelectLabel.Location = new Point(6, 94);
            ScaleOutlineColorSelectLabel.Name = "ScaleOutlineColorSelectLabel";
            ScaleOutlineColorSelectLabel.Size = new Size(92, 40);
            ScaleOutlineColorSelectLabel.TabIndex = 79;
            ScaleOutlineColorSelectLabel.Text = "Click to Select";
            ScaleOutlineColorSelectLabel.TextAlign = ContentAlignment.MiddleCenter;
            ScaleOutlineColorSelectLabel.Click += ScaleOutlineColorSelectLabel_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(36, 31);
            label1.Name = "label1";
            label1.Size = new Size(40, 16);
            label1.TabIndex = 102;
            label1.Text = "Width";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(190, 31);
            label2.Name = "label2";
            label2.Size = new Size(43, 16);
            label2.TabIndex = 103;
            label2.Text = "Height";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 67);
            label3.Name = "label3";
            label3.Size = new Size(64, 16);
            label3.TabIndex = 104;
            label3.Text = "Segments";
            // 
            // ScaleSegmentCountUpDown
            // 
            ScaleSegmentCountUpDown.Location = new Point(239, 65);
            ScaleSegmentCountUpDown.Maximum = new decimal(new int[] { 32, 0, 0, 0 });
            ScaleSegmentCountUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            ScaleSegmentCountUpDown.Name = "ScaleSegmentCountUpDown";
            ScaleSegmentCountUpDown.Size = new Size(60, 23);
            ScaleSegmentCountUpDown.TabIndex = 105;
            ScaleSegmentCountUpDown.Value = new decimal(new int[] { 5, 0, 0, 0 });
            // 
            // ScaleLineWidthUpDown
            // 
            ScaleLineWidthUpDown.Location = new Point(239, 105);
            ScaleLineWidthUpDown.Maximum = new decimal(new int[] { 8, 0, 0, 0 });
            ScaleLineWidthUpDown.Minimum = new decimal(new int[] { 2, 0, 0, 0 });
            ScaleLineWidthUpDown.Name = "ScaleLineWidthUpDown";
            ScaleLineWidthUpDown.Size = new Size(60, 23);
            ScaleLineWidthUpDown.TabIndex = 106;
            ScaleLineWidthUpDown.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(9, 107);
            label4.Name = "label4";
            label4.Size = new Size(67, 16);
            label4.TabIndex = 107;
            label4.Text = "Line Width";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(33, 149);
            label5.Name = "label5";
            label5.Size = new Size(43, 16);
            label5.TabIndex = 108;
            label5.Text = "Colors";
            // 
            // ResetScaleColorsButton
            // 
            ResetScaleColorsButton.FlatStyle = FlatStyle.Flat;
            ResetScaleColorsButton.IconChar = FontAwesome.Sharp.IconChar.RotateBack;
            ResetScaleColorsButton.IconColor = Color.Black;
            ResetScaleColorsButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            ResetScaleColorsButton.IconSize = 18;
            ResetScaleColorsButton.Location = new Point(207, 140);
            ResetScaleColorsButton.Name = "ResetScaleColorsButton";
            ResetScaleColorsButton.Size = new Size(26, 28);
            ResetScaleColorsButton.TabIndex = 109;
            ResetScaleColorsButton.UseVisualStyleBackColor = true;
            ResetScaleColorsButton.Click += ResetScaleColorsButton_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(18, 184);
            label6.Name = "label6";
            label6.Size = new Size(58, 16);
            label6.TabIndex = 110;
            label6.Text = "Segment";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(13, 233);
            label7.Name = "label7";
            label7.Size = new Size(63, 16);
            label7.TabIndex = 111;
            label7.Text = "Unit Label";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(21, 200);
            label8.Name = "label8";
            label8.Size = new Size(55, 16);
            label8.TabIndex = 112;
            label8.Text = "Distance";
            // 
            // ScaleUnitsTextBox
            // 
            ScaleUnitsTextBox.Location = new Point(82, 230);
            ScaleUnitsTextBox.Name = "ScaleUnitsTextBox";
            ScaleUnitsTextBox.Size = new Size(217, 23);
            ScaleUnitsTextBox.TabIndex = 113;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(29, 290);
            label9.Name = "label9";
            label9.Size = new Size(47, 16);
            label9.TabIndex = 115;
            label9.Text = "Display";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(18, 274);
            label10.Name = "label10";
            label10.Size = new Size(58, 16);
            label10.TabIndex = 114;
            label10.Text = "Numbers";
            // 
            // CreateScaleButton
            // 
            CreateScaleButton.Location = new Point(82, 569);
            CreateScaleButton.Name = "CreateScaleButton";
            CreateScaleButton.Size = new Size(60, 60);
            CreateScaleButton.TabIndex = 116;
            CreateScaleButton.Text = "C&reate";
            CreateScaleButton.UseVisualStyleBackColor = true;
            CreateScaleButton.Click += CreateScaleButton_Click;
            // 
            // DeleteScaleButton
            // 
            DeleteScaleButton.Location = new Point(150, 569);
            DeleteScaleButton.Name = "DeleteScaleButton";
            DeleteScaleButton.Size = new Size(60, 60);
            DeleteScaleButton.TabIndex = 117;
            DeleteScaleButton.Text = "&Delete";
            DeleteScaleButton.UseVisualStyleBackColor = true;
            DeleteScaleButton.Click += DeleteScaleButton_Click;
            // 
            // CloseButton
            // 
            CloseButton.Location = new Point(239, 569);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new Size(60, 60);
            CloseButton.TabIndex = 118;
            CloseButton.Text = "&Close";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += CloseButton_Click;
            // 
            // MapScaleCreator
            // 
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(325, 661);
            Controls.Add(CloseButton);
            Controls.Add(DeleteScaleButton);
            Controls.Add(CreateScaleButton);
            Controls.Add(label9);
            Controls.Add(label10);
            Controls.Add(ScaleUnitsTextBox);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(ResetScaleColorsButton);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(ScaleLineWidthUpDown);
            Controls.Add(ScaleSegmentCountUpDown);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(groupBox11);
            Controls.Add(groupBox12);
            Controls.Add(ScaleNumbersDisplayCheckList);
            Controls.Add(ScaleSegmentDistanceUpDown);
            Controls.Add(ScaleColor3Label);
            Controls.Add(ScaleColor2Label);
            Controls.Add(ScaleColor1Label);
            Controls.Add(ScaleLineWidthTrack);
            Controls.Add(ScaleSegmentCountTrack);
            Controls.Add(ScaleHeightUpDown);
            Controls.Add(ScaleWidthUpDown);
            Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            HelpButton = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MapScaleCreator";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.Manual;
            Text = "Map Scale Creator";
            ((System.ComponentModel.ISupportInitialize)ScaleWidthUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)ScaleHeightUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)ScaleSegmentCountTrack).EndInit();
            ((System.ComponentModel.ISupportInitialize)ScaleLineWidthTrack).EndInit();
            ((System.ComponentModel.ISupportInitialize)ScaleSegmentDistanceUpDown).EndInit();
            groupBox12.ResumeLayout(false);
            groupBox12.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ScaleFontColorOpacityTrack).EndInit();
            groupBox11.ResumeLayout(false);
            groupBox11.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ScaleOutlineColorOpacityTrack).EndInit();
            ((System.ComponentModel.ISupportInitialize)ScaleOutlineWidthUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)ScaleSegmentCountUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)ScaleLineWidthUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private NumericUpDown ScaleWidthUpDown;
        private NumericUpDown ScaleHeightUpDown;
        private TrackBar ScaleSegmentCountTrack;
        private TrackBar ScaleLineWidthTrack;
        private Label ScaleColor3Label;
        private Label ScaleColor2Label;
        private Label ScaleColor1Label;
        private NumericUpDown ScaleSegmentDistanceUpDown;
        private CheckedListBox ScaleNumbersDisplayCheckList;
        private GroupBox groupBox12;
        private TrackBar ScaleFontColorOpacityTrack;
        private Label ScaleFontColorOpacityLabel;
        private Label label69;
        private Label label61;
        private Label ScaleFontColorSelectLabel;
        private Button SelectScaleFontButton;
        private GroupBox groupBox11;
        private TrackBar ScaleOutlineColorOpacityTrack;
        private Label ScaleOutlineColorOpacityLabel;
        private Label label68;
        private Label label63;
        private NumericUpDown ScaleOutlineWidthUpDown;
        private Label label60;
        private Label ScaleOutlineColorSelectLabel;
        private Label label1;
        private Label label2;
        private Label label3;
        private NumericUpDown ScaleSegmentCountUpDown;
        private NumericUpDown ScaleLineWidthUpDown;
        private Label label4;
        private Label label5;
        private FontAwesome.Sharp.IconButton ResetScaleColorsButton;
        private Label label6;
        private Label label7;
        private Label label8;
        private TextBox ScaleUnitsTextBox;
        private Label label9;
        private Label label10;
        private Button CreateScaleButton;
        private Button DeleteScaleButton;
        private Button CloseButton;
    }
}