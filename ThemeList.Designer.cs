namespace MapCreator
{
    partial class ThemeList
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
            ThemeListComboBox = new ComboBox();
            label1 = new Label();
            ApplyBackgroundSettingsCheck = new CheckBox();
            label2 = new Label();
            ApplyOceanSettingsCheck = new CheckBox();
            ApplyLandformSettingsCheck = new CheckBox();
            ApplyWaterSettingsCheck = new CheckBox();
            ApplyPathSettingsCheck = new CheckBox();
            ApplySymbolSettingsCheck = new CheckBox();
            CloseThemeDialogButton = new Button();
            ApplyThemeButton = new Button();
            CheckAllCheck = new CheckBox();
            ApplyOceanColorPaletteSettingsCheck = new CheckBox();
            ApplyLandformColorPaletteSettingsCheck = new CheckBox();
            ApplyFreshwaterColorPaletteSettingsCheck = new CheckBox();
            ApplyLabelPresetSettingsCheck = new CheckBox();
            SaveThemeButton = new Button();
            SuspendLayout();
            // 
            // ThemeListComboBox
            // 
            ThemeListComboBox.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ThemeListComboBox.FormattingEnabled = true;
            ThemeListComboBox.Location = new Point(12, 31);
            ThemeListComboBox.Name = "ThemeListComboBox";
            ThemeListComboBox.Size = new Size(254, 27);
            ThemeListComboBox.TabIndex = 0;
            ThemeListComboBox.SelectionChangeCommitted += ThemeListComboBox_SelectionChangeCommitted;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(65, 19);
            label1.TabIndex = 1;
            label1.Text = "Themes";
            // 
            // ApplyBackgroundSettingsCheck
            // 
            ApplyBackgroundSettingsCheck.AutoSize = true;
            ApplyBackgroundSettingsCheck.Checked = true;
            ApplyBackgroundSettingsCheck.CheckState = CheckState.Checked;
            ApplyBackgroundSettingsCheck.Font = new Font("Tahoma", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ApplyBackgroundSettingsCheck.Location = new Point(24, 102);
            ApplyBackgroundSettingsCheck.Name = "ApplyBackgroundSettingsCheck";
            ApplyBackgroundSettingsCheck.Size = new Size(103, 22);
            ApplyBackgroundSettingsCheck.TabIndex = 2;
            ApplyBackgroundSettingsCheck.Text = "Background";
            ApplyBackgroundSettingsCheck.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(45, 74);
            label2.Name = "label2";
            label2.Size = new Size(189, 19);
            label2.TabIndex = 3;
            label2.Text = "Apply Theme Settings To";
            // 
            // ApplyOceanSettingsCheck
            // 
            ApplyOceanSettingsCheck.AutoSize = true;
            ApplyOceanSettingsCheck.Checked = true;
            ApplyOceanSettingsCheck.CheckState = CheckState.Checked;
            ApplyOceanSettingsCheck.Font = new Font("Tahoma", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ApplyOceanSettingsCheck.Location = new Point(24, 130);
            ApplyOceanSettingsCheck.Name = "ApplyOceanSettingsCheck";
            ApplyOceanSettingsCheck.Size = new Size(69, 22);
            ApplyOceanSettingsCheck.TabIndex = 4;
            ApplyOceanSettingsCheck.Text = "Ocean";
            ApplyOceanSettingsCheck.UseVisualStyleBackColor = true;
            // 
            // ApplyLandformSettingsCheck
            // 
            ApplyLandformSettingsCheck.AutoSize = true;
            ApplyLandformSettingsCheck.Checked = true;
            ApplyLandformSettingsCheck.CheckState = CheckState.Checked;
            ApplyLandformSettingsCheck.Font = new Font("Tahoma", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ApplyLandformSettingsCheck.Location = new Point(24, 186);
            ApplyLandformSettingsCheck.Name = "ApplyLandformSettingsCheck";
            ApplyLandformSettingsCheck.Size = new Size(96, 22);
            ApplyLandformSettingsCheck.TabIndex = 5;
            ApplyLandformSettingsCheck.Text = "Landforms";
            ApplyLandformSettingsCheck.UseVisualStyleBackColor = true;
            // 
            // ApplyWaterSettingsCheck
            // 
            ApplyWaterSettingsCheck.AutoSize = true;
            ApplyWaterSettingsCheck.Checked = true;
            ApplyWaterSettingsCheck.CheckState = CheckState.Checked;
            ApplyWaterSettingsCheck.Font = new Font("Tahoma", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ApplyWaterSettingsCheck.Location = new Point(24, 242);
            ApplyWaterSettingsCheck.Name = "ApplyWaterSettingsCheck";
            ApplyWaterSettingsCheck.Size = new Size(99, 22);
            ApplyWaterSettingsCheck.TabIndex = 6;
            ApplyWaterSettingsCheck.Text = "Freshwater";
            ApplyWaterSettingsCheck.UseVisualStyleBackColor = true;
            // 
            // ApplyPathSettingsCheck
            // 
            ApplyPathSettingsCheck.AutoSize = true;
            ApplyPathSettingsCheck.Checked = true;
            ApplyPathSettingsCheck.CheckState = CheckState.Checked;
            ApplyPathSettingsCheck.Font = new Font("Tahoma", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ApplyPathSettingsCheck.Location = new Point(24, 298);
            ApplyPathSettingsCheck.Name = "ApplyPathSettingsCheck";
            ApplyPathSettingsCheck.Size = new Size(63, 22);
            ApplyPathSettingsCheck.TabIndex = 7;
            ApplyPathSettingsCheck.Text = "Paths";
            ApplyPathSettingsCheck.UseVisualStyleBackColor = true;
            // 
            // ApplySymbolSettingsCheck
            // 
            ApplySymbolSettingsCheck.AutoSize = true;
            ApplySymbolSettingsCheck.Checked = true;
            ApplySymbolSettingsCheck.CheckState = CheckState.Checked;
            ApplySymbolSettingsCheck.Font = new Font("Tahoma", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ApplySymbolSettingsCheck.Location = new Point(24, 326);
            ApplySymbolSettingsCheck.Name = "ApplySymbolSettingsCheck";
            ApplySymbolSettingsCheck.Size = new Size(81, 22);
            ApplySymbolSettingsCheck.TabIndex = 8;
            ApplySymbolSettingsCheck.Text = "Symbols";
            ApplySymbolSettingsCheck.UseVisualStyleBackColor = true;
            // 
            // CloseThemeDialogButton
            // 
            CloseThemeDialogButton.DialogResult = DialogResult.Cancel;
            CloseThemeDialogButton.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            CloseThemeDialogButton.Location = new Point(197, 401);
            CloseThemeDialogButton.Name = "CloseThemeDialogButton";
            CloseThemeDialogButton.Size = new Size(75, 48);
            CloseThemeDialogButton.TabIndex = 62;
            CloseThemeDialogButton.Text = "&Close";
            CloseThemeDialogButton.UseVisualStyleBackColor = true;
            // 
            // ApplyThemeButton
            // 
            ApplyThemeButton.DialogResult = DialogResult.OK;
            ApplyThemeButton.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ApplyThemeButton.Location = new Point(12, 401);
            ApplyThemeButton.Name = "ApplyThemeButton";
            ApplyThemeButton.Size = new Size(75, 48);
            ApplyThemeButton.TabIndex = 61;
            ApplyThemeButton.Text = "&Apply";
            ApplyThemeButton.UseVisualStyleBackColor = true;
            // 
            // CheckAllCheck
            // 
            CheckAllCheck.AutoSize = true;
            CheckAllCheck.Checked = true;
            CheckAllCheck.CheckState = CheckState.Checked;
            CheckAllCheck.Font = new Font("Tahoma", 9F, FontStyle.Italic, GraphicsUnit.Point, 0);
            CheckAllCheck.Location = new Point(24, 78);
            CheckAllCheck.Name = "CheckAllCheck";
            CheckAllCheck.Size = new Size(15, 14);
            CheckAllCheck.TabIndex = 63;
            CheckAllCheck.UseVisualStyleBackColor = true;
            CheckAllCheck.CheckStateChanged += CheckAllCheck_CheckStateChanged;
            // 
            // ApplyOceanColorPaletteSettingsCheck
            // 
            ApplyOceanColorPaletteSettingsCheck.AutoSize = true;
            ApplyOceanColorPaletteSettingsCheck.Checked = true;
            ApplyOceanColorPaletteSettingsCheck.CheckState = CheckState.Checked;
            ApplyOceanColorPaletteSettingsCheck.Font = new Font("Tahoma", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ApplyOceanColorPaletteSettingsCheck.Location = new Point(24, 158);
            ApplyOceanColorPaletteSettingsCheck.Name = "ApplyOceanColorPaletteSettingsCheck";
            ApplyOceanColorPaletteSettingsCheck.Size = new Size(155, 22);
            ApplyOceanColorPaletteSettingsCheck.TabIndex = 64;
            ApplyOceanColorPaletteSettingsCheck.Text = "Ocean Color Palette";
            ApplyOceanColorPaletteSettingsCheck.UseVisualStyleBackColor = true;
            // 
            // ApplyLandformColorPaletteSettingsCheck
            // 
            ApplyLandformColorPaletteSettingsCheck.AutoSize = true;
            ApplyLandformColorPaletteSettingsCheck.Checked = true;
            ApplyLandformColorPaletteSettingsCheck.CheckState = CheckState.Checked;
            ApplyLandformColorPaletteSettingsCheck.Font = new Font("Tahoma", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ApplyLandformColorPaletteSettingsCheck.Location = new Point(24, 214);
            ApplyLandformColorPaletteSettingsCheck.Name = "ApplyLandformColorPaletteSettingsCheck";
            ApplyLandformColorPaletteSettingsCheck.Size = new Size(175, 22);
            ApplyLandformColorPaletteSettingsCheck.TabIndex = 65;
            ApplyLandformColorPaletteSettingsCheck.Text = "Landform Color Palette";
            ApplyLandformColorPaletteSettingsCheck.UseVisualStyleBackColor = true;
            // 
            // ApplyFreshwaterColorPaletteSettingsCheck
            // 
            ApplyFreshwaterColorPaletteSettingsCheck.AutoSize = true;
            ApplyFreshwaterColorPaletteSettingsCheck.Checked = true;
            ApplyFreshwaterColorPaletteSettingsCheck.CheckState = CheckState.Checked;
            ApplyFreshwaterColorPaletteSettingsCheck.Font = new Font("Tahoma", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ApplyFreshwaterColorPaletteSettingsCheck.Location = new Point(24, 270);
            ApplyFreshwaterColorPaletteSettingsCheck.Name = "ApplyFreshwaterColorPaletteSettingsCheck";
            ApplyFreshwaterColorPaletteSettingsCheck.Size = new Size(185, 22);
            ApplyFreshwaterColorPaletteSettingsCheck.TabIndex = 66;
            ApplyFreshwaterColorPaletteSettingsCheck.Text = "Freshwater Color Palette";
            ApplyFreshwaterColorPaletteSettingsCheck.UseVisualStyleBackColor = true;
            // 
            // ApplyLabelPresetSettingsCheck
            // 
            ApplyLabelPresetSettingsCheck.AutoSize = true;
            ApplyLabelPresetSettingsCheck.Checked = true;
            ApplyLabelPresetSettingsCheck.CheckState = CheckState.Checked;
            ApplyLabelPresetSettingsCheck.Font = new Font("Tahoma", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ApplyLabelPresetSettingsCheck.Location = new Point(24, 354);
            ApplyLabelPresetSettingsCheck.Name = "ApplyLabelPresetSettingsCheck";
            ApplyLabelPresetSettingsCheck.Size = new Size(113, 22);
            ApplyLabelPresetSettingsCheck.TabIndex = 67;
            ApplyLabelPresetSettingsCheck.Text = "Label Presets";
            ApplyLabelPresetSettingsCheck.UseVisualStyleBackColor = true;
            // 
            // SaveThemeButton
            // 
            SaveThemeButton.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            SaveThemeButton.Location = new Point(104, 401);
            SaveThemeButton.Name = "SaveThemeButton";
            SaveThemeButton.Size = new Size(75, 48);
            SaveThemeButton.TabIndex = 68;
            SaveThemeButton.Text = "&Save As";
            SaveThemeButton.UseVisualStyleBackColor = true;
            SaveThemeButton.Click += SaveThemeButton_Click;
            // 
            // ThemeList
            // 
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(284, 467);
            Controls.Add(SaveThemeButton);
            Controls.Add(ApplyLabelPresetSettingsCheck);
            Controls.Add(ApplyFreshwaterColorPaletteSettingsCheck);
            Controls.Add(ApplyLandformColorPaletteSettingsCheck);
            Controls.Add(ApplyOceanColorPaletteSettingsCheck);
            Controls.Add(CheckAllCheck);
            Controls.Add(CloseThemeDialogButton);
            Controls.Add(ApplyThemeButton);
            Controls.Add(ApplySymbolSettingsCheck);
            Controls.Add(ApplyPathSettingsCheck);
            Controls.Add(ApplyWaterSettingsCheck);
            Controls.Add(ApplyLandformSettingsCheck);
            Controls.Add(ApplyOceanSettingsCheck);
            Controls.Add(label2);
            Controls.Add(ApplyBackgroundSettingsCheck);
            Controls.Add(label1);
            Controls.Add(ThemeListComboBox);
            Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            HelpButton = true;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ThemeList";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Save or Apply Theme";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ComboBox ThemeListComboBox;
        private Label label1;
        private CheckBox ApplyBackgroundSettingsCheck;
        private Label label2;
        private CheckBox ApplyOceanSettingsCheck;
        private CheckBox ApplyLandformSettingsCheck;
        private CheckBox ApplyWaterSettingsCheck;
        private CheckBox ApplyPathSettingsCheck;
        private CheckBox ApplySymbolSettingsCheck;
        private Button CloseThemeDialogButton;
        private Button ApplyThemeButton;
        private CheckBox CheckAllCheck;
        private CheckBox ApplyOceanColorPaletteSettingsCheck;
        private CheckBox ApplyLandformColorPaletteSettingsCheck;
        private CheckBox ApplyFreshwaterColorPaletteSettingsCheck;
        private CheckBox ApplyLabelPresetSettingsCheck;
        private Button SaveThemeButton;
    }
}