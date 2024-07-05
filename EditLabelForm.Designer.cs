namespace MapCreator
{
    partial class EditLabelForm
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
            LabelTextBox = new TextBox();
            OKButton = new Button();
            CancelEditButton = new Button();
            SuspendLayout();
            // 
            // LabelTextBox
            // 
            LabelTextBox.Location = new Point(12, 12);
            LabelTextBox.Name = "LabelTextBox";
            LabelTextBox.Size = new Size(318, 26);
            LabelTextBox.TabIndex = 0;
            // 
            // OKButton
            // 
            OKButton.DialogResult = DialogResult.OK;
            OKButton.Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            OKButton.Location = new Point(174, 57);
            OKButton.Name = "OKButton";
            OKButton.Size = new Size(75, 23);
            OKButton.TabIndex = 1;
            OKButton.Text = "O&k";
            OKButton.UseVisualStyleBackColor = true;
            // 
            // CancelEditButton
            // 
            CancelEditButton.DialogResult = DialogResult.Cancel;
            CancelEditButton.Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            CancelEditButton.Location = new Point(255, 57);
            CancelEditButton.Name = "CancelEditButton";
            CancelEditButton.Size = new Size(75, 23);
            CancelEditButton.TabIndex = 2;
            CancelEditButton.Text = "&Cancel";
            CancelEditButton.UseVisualStyleBackColor = true;
            // 
            // EditLabelForm
            // 
            AcceptButton = OKButton;
            AutoScaleDimensions = new SizeF(8F, 18F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = CancelEditButton;
            ClientSize = new Size(342, 92);
            Controls.Add(CancelEditButton);
            Controls.Add(OKButton);
            Controls.Add(LabelTextBox);
            Font = new Font("Tahoma", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "EditLabelForm";
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Edit Label Text";
            TopMost = true;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox LabelTextBox;
        private Button OKButton;
        private Button CancelEditButton;
    }
}