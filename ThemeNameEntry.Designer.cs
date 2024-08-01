namespace MapCreator
{
    partial class ThemeNameEntry
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
            ThemeNameCancelButton = new Button();
            ThemeNameOKButton = new Button();
            ThemeNameTextBox = new TextBox();
            SuspendLayout();
            // 
            // ThemeNameCancelButton
            // 
            ThemeNameCancelButton.DialogResult = DialogResult.Cancel;
            ThemeNameCancelButton.Location = new Point(166, 47);
            ThemeNameCancelButton.Name = "ThemeNameCancelButton";
            ThemeNameCancelButton.Size = new Size(60, 30);
            ThemeNameCancelButton.TabIndex = 5;
            ThemeNameCancelButton.Text = "&Cancel";
            ThemeNameCancelButton.UseVisualStyleBackColor = true;
            // 
            // ThemeNameOKButton
            // 
            ThemeNameOKButton.DialogResult = DialogResult.OK;
            ThemeNameOKButton.Location = new Point(100, 47);
            ThemeNameOKButton.Name = "ThemeNameOKButton";
            ThemeNameOKButton.Size = new Size(60, 30);
            ThemeNameOKButton.TabIndex = 4;
            ThemeNameOKButton.Text = "&OK";
            ThemeNameOKButton.UseVisualStyleBackColor = true;
            // 
            // ThemeNameTextBox
            // 
            ThemeNameTextBox.Location = new Point(12, 12);
            ThemeNameTextBox.Name = "ThemeNameTextBox";
            ThemeNameTextBox.Size = new Size(214, 23);
            ThemeNameTextBox.TabIndex = 3;
            // 
            // ThemeNameEntry
            // 
            AcceptButton = ThemeNameOKButton;
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = ThemeNameCancelButton;
            ClientSize = new Size(238, 91);
            ControlBox = false;
            Controls.Add(ThemeNameCancelButton);
            Controls.Add(ThemeNameOKButton);
            Controls.Add(ThemeNameTextBox);
            Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ThemeNameEntry";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Enter New Theme Name";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button ThemeNameCancelButton;
        private Button ThemeNameOKButton;
        public TextBox ThemeNameTextBox;
    }
}