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
            OKButton = new Button();
            CancelButton = new Button();
            SuspendLayout();
            // 
            // PresetNameTextBox
            // 
            PresetNameTextBox.Location = new Point(12, 12);
            PresetNameTextBox.Name = "PresetNameTextBox";
            PresetNameTextBox.Size = new Size(214, 23);
            PresetNameTextBox.TabIndex = 0;
            // 
            // OKButton
            // 
            OKButton.DialogResult = DialogResult.OK;
            OKButton.Location = new Point(100, 47);
            OKButton.Name = "OKButton";
            OKButton.Size = new Size(60, 60);
            OKButton.TabIndex = 1;
            OKButton.Text = "&OK";
            OKButton.UseVisualStyleBackColor = true;
            // 
            // CancelButton
            // 
            CancelButton.DialogResult = DialogResult.Cancel;
            CancelButton.Location = new Point(166, 47);
            CancelButton.Name = "CancelButton";
            CancelButton.Size = new Size(60, 60);
            CancelButton.TabIndex = 2;
            CancelButton.Text = "&Cancel";
            CancelButton.UseVisualStyleBackColor = true;
            // 
            // LabelPresetNameEntry
            // 
            AcceptButton = OKButton;
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = CancelButton;
            ClientSize = new Size(238, 119);
            ControlBox = false;
            Controls.Add(CancelButton);
            Controls.Add(OKButton);
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
        private Button OKButton;
        private Button CancelButton;
        public TextBox PresetNameTextBox;
    }
}