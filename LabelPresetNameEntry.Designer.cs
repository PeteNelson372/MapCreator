namespace MapCreator
{
    partial class LabelPresetNameEntry
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
            PresetNameTextBox = new TextBox();
            PresetOKButton = new Button();
            PresetCancelButton = new Button();
            SuspendLayout();
            // 
            // PresetNameTextBox
            // 
            PresetNameTextBox.Location = new Point(12, 12);
            PresetNameTextBox.Name = "PresetNameTextBox";
            PresetNameTextBox.Size = new Size(214, 23);
            PresetNameTextBox.TabIndex = 0;
            // 
            // PresetOKButton
            // 
            PresetOKButton.DialogResult = DialogResult.OK;
            PresetOKButton.Location = new Point(100, 47);
            PresetOKButton.Name = "PresetOKButton";
            PresetOKButton.Size = new Size(60, 30);
            PresetOKButton.TabIndex = 1;
            PresetOKButton.Text = "&OK";
            PresetOKButton.UseVisualStyleBackColor = true;
            // 
            // PresetCancelButton
            // 
            PresetCancelButton.DialogResult = DialogResult.Cancel;
            PresetCancelButton.Location = new Point(166, 47);
            PresetCancelButton.Name = "PresetCancelButton";
            PresetCancelButton.Size = new Size(60, 30);
            PresetCancelButton.TabIndex = 2;
            PresetCancelButton.Text = "&Cancel";
            PresetCancelButton.UseVisualStyleBackColor = true;
            // 
            // LabelPresetNameEntry
            // 
            AcceptButton = PresetOKButton;
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(238, 91);
            ControlBox = false;
            Controls.Add(PresetCancelButton);
            Controls.Add(PresetOKButton);
            Controls.Add(PresetNameTextBox);
            Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "LabelPresetNameEntry";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Enter Label Preset Name";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button PresetOKButton;
        private Button PresetCancelButton;
        public TextBox PresetNameTextBox;
    }
}