namespace MapCreator
{
    partial class PaletteColorSelector
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
            colorGrid1 = new Cyotek.Windows.Forms.ColorGrid();
            colorEditor1 = new Cyotek.Windows.Forms.ColorEditor();
            colorWheel1 = new Cyotek.Windows.Forms.ColorWheel();
            SuspendLayout();
            // 
            // colorGrid1
            // 
            colorGrid1.Location = new Point(259, 12);
            colorGrid1.Name = "colorGrid1";
            colorGrid1.Size = new Size(319, 273);
            colorGrid1.TabIndex = 0;
            // 
            // colorEditor1
            // 
            colorEditor1.Location = new Point(585, 12);
            colorEditor1.Margin = new Padding(4, 3, 4, 3);
            colorEditor1.Name = "colorEditor1";
            colorEditor1.Size = new Size(202, 273);
            colorEditor1.TabIndex = 1;
            // 
            // colorWheel1
            // 
            colorWheel1.Alpha = 1D;
            colorWheel1.Location = new Point(12, 12);
            colorWheel1.Name = "colorWheel1";
            colorWheel1.Size = new Size(241, 273);
            colorWheel1.TabIndex = 2;
            // 
            // PaletteColorSelector
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(colorWheel1);
            Controls.Add(colorEditor1);
            Controls.Add(colorGrid1);
            Name = "PaletteColorSelector";
            Text = "PaletteColorSelector";
            ResumeLayout(false);
        }

        #endregion

        private Cyotek.Windows.Forms.ColorGrid colorGrid1;
        private Cyotek.Windows.Forms.ColorEditor colorEditor1;
        private Cyotek.Windows.Forms.ColorWheel colorWheel1;
    }
}