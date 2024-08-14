namespace MapCreator
{
    partial class MapCreatorAboutBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapCreatorAboutBox));
            logoPictureBox = new PictureBox();
            VersionLabel = new Label();
            labelCompanyName = new Label();
            okButton = new Button();
            pictureBox1 = new PictureBox();
            label2 = new Label();
            richTextBox1 = new RichTextBox();
            panel1 = new Panel();
            textBox1 = new TextBox();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)logoPictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // logoPictureBox
            // 
            logoPictureBox.BackColor = Color.White;
            logoPictureBox.Image = (Image)resources.GetObject("logoPictureBox.Image");
            logoPictureBox.InitialImage = (Image)resources.GetObject("logoPictureBox.InitialImage");
            logoPictureBox.Location = new Point(11, 3);
            logoPictureBox.Margin = new Padding(4, 3, 4, 3);
            logoPictureBox.Name = "logoPictureBox";
            logoPictureBox.Size = new Size(170, 134);
            logoPictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            logoPictureBox.TabIndex = 26;
            logoPictureBox.TabStop = false;
            // 
            // VersionLabel
            // 
            VersionLabel.Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            VersionLabel.Location = new Point(214, 35);
            VersionLabel.Margin = new Padding(7, 0, 4, 0);
            VersionLabel.MaximumSize = new Size(0, 20);
            VersionLabel.Name = "VersionLabel";
            VersionLabel.Size = new Size(252, 20);
            VersionLabel.TabIndex = 25;
            VersionLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // labelCompanyName
            // 
            labelCompanyName.Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            labelCompanyName.Location = new Point(214, 61);
            labelCompanyName.Margin = new Padding(7, 0, 4, 0);
            labelCompanyName.MaximumSize = new Size(0, 20);
            labelCompanyName.Name = "labelCompanyName";
            labelCompanyName.Size = new Size(252, 20);
            labelCompanyName.TabIndex = 29;
            labelCompanyName.Text = "The Brookmonte Group: Zero Sum Games";
            labelCompanyName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // okButton
            // 
            okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            okButton.DialogResult = DialogResult.Cancel;
            okButton.Location = new Point(469, 346);
            okButton.Margin = new Padding(4, 3, 4, 3);
            okButton.Name = "okButton";
            okButton.Size = new Size(88, 27);
            okButton.TabIndex = 31;
            okButton.Text = "&OK";
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.White;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(11, 203);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(170, 170);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 32;
            pictureBox1.TabStop = false;
            // 
            // label2
            // 
            label2.Font = new Font("Tahoma", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(214, 10);
            label2.Name = "label2";
            label2.Size = new Size(252, 23);
            label2.TabIndex = 34;
            label2.Text = "Map Creator";
            // 
            // richTextBox1
            // 
            richTextBox1.BorderStyle = BorderStyle.None;
            richTextBox1.Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            richTextBox1.Location = new Point(214, 217);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.ReadOnly = true;
            richTextBox1.ScrollBars = RichTextBoxScrollBars.None;
            richTextBox1.Size = new Size(343, 112);
            richTextBox1.TabIndex = 35;
            richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(logoPictureBox);
            panel1.Controls.Add(pictureBox1);
            panel1.Location = new Point(-3, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(191, 384);
            panel1.TabIndex = 36;
            // 
            // textBox1
            // 
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            textBox1.Location = new Point(214, 107);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.ScrollBars = ScrollBars.Vertical;
            textBox1.Size = new Size(343, 90);
            textBox1.TabIndex = 37;
            textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(214, 89);
            label1.Name = "label1";
            label1.Size = new Size(123, 15);
            label1.TabIndex = 38;
            label1.Text = "Licensing Information";
            // 
            // MapCreatorAboutBox
            // 
            AcceptButton = okButton;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(578, 382);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(panel1);
            Controls.Add(richTextBox1);
            Controls.Add(label2);
            Controls.Add(VersionLabel);
            Controls.Add(labelCompanyName);
            Controls.Add(okButton);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MapCreatorAboutBox";
            Padding = new Padding(10);
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "About Map Creator";
            ((System.ComponentModel.ISupportInitialize)logoPictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox logoPictureBox;
        private Label VersionLabel;
        private Label labelCompanyName;
        private Button okButton;
        private PictureBox pictureBox1;
        private Label label2;
        private RichTextBox richTextBox1;
        private Panel panel1;
        private TextBox textBox1;
        private Label label1;
    }
}
