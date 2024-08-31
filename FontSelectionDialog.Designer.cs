namespace MapCreator
{
    partial class FontSelectionDialog
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
            groupBox1 = new GroupBox();
            FontListView = new ListView();
            columnHeader1 = new ColumnHeader();
            groupBox4 = new GroupBox();
            FontSizeCombo = new ComboBox();
            groupBox5 = new GroupBox();
            ExampleTextLabel = new Label();
            BoldButton = new FontAwesome.Sharp.IconButton();
            ItalicButton = new FontAwesome.Sharp.IconButton();
            UnderlineButton = new FontAwesome.Sharp.IconButton();
            StrikeThroughButton = new FontAwesome.Sharp.IconButton();
            OKButton = new Button();
            CloseButton = new Button();
            groupBox1.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox5.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(FontListView);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(285, 216);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Font Family";
            // 
            // FontListView
            // 
            FontListView.Activation = ItemActivation.OneClick;
            FontListView.AutoArrange = false;
            FontListView.Columns.AddRange(new ColumnHeader[] { columnHeader1 });
            FontListView.FullRowSelect = true;
            FontListView.HeaderStyle = ColumnHeaderStyle.None;
            FontListView.LabelWrap = false;
            FontListView.Location = new Point(6, 20);
            FontListView.MultiSelect = false;
            FontListView.Name = "FontListView";
            FontListView.ShowGroups = false;
            FontListView.Size = new Size(272, 190);
            FontListView.TabIndex = 0;
            FontListView.TileSize = new Size(170, 22);
            FontListView.UseCompatibleStateImageBehavior = false;
            FontListView.View = View.Details;
            FontListView.VirtualListSize = 1;
            FontListView.SelectedIndexChanged += FontListView_SelectedIndexChanged;
            // 
            // columnHeader1
            // 
            columnHeader1.Width = 250;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(FontSizeCombo);
            groupBox4.Location = new Point(312, 12);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(102, 50);
            groupBox4.TabIndex = 3;
            groupBox4.TabStop = false;
            groupBox4.Text = "Font Size";
            // 
            // FontSizeCombo
            // 
            FontSizeCombo.FormattingEnabled = true;
            FontSizeCombo.Items.AddRange(new object[] { "5", "6", "7", "8", "9", "10", "11", "12", "14", "16", "18", "20", "22", "24", "26", "28", "30", "32", "34", "36", "42", "48", "56", "64", "72", "80", "96", "120", "144" });
            FontSizeCombo.Location = new Point(6, 20);
            FontSizeCombo.Name = "FontSizeCombo";
            FontSizeCombo.Size = new Size(90, 24);
            FontSizeCombo.TabIndex = 0;
            FontSizeCombo.TextChanged += FontSizeCombo_TextChanged;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(ExampleTextLabel);
            groupBox5.Location = new Point(312, 68);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(326, 104);
            groupBox5.TabIndex = 6;
            groupBox5.TabStop = false;
            groupBox5.Text = "Sample";
            // 
            // ExampleTextLabel
            // 
            ExampleTextLabel.BackColor = SystemColors.ControlLightLight;
            ExampleTextLabel.BorderStyle = BorderStyle.FixedSingle;
            ExampleTextLabel.Location = new Point(6, 19);
            ExampleTextLabel.Name = "ExampleTextLabel";
            ExampleTextLabel.Size = new Size(314, 74);
            ExampleTextLabel.TabIndex = 0;
            ExampleTextLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // BoldButton
            // 
            BoldButton.IconChar = FontAwesome.Sharp.IconChar.Bold;
            BoldButton.IconColor = Color.Black;
            BoldButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            BoldButton.IconSize = 24;
            BoldButton.Location = new Point(420, 18);
            BoldButton.Name = "BoldButton";
            BoldButton.Size = new Size(50, 44);
            BoldButton.TabIndex = 7;
            BoldButton.TextAlign = ContentAlignment.MiddleLeft;
            BoldButton.UseVisualStyleBackColor = true;
            BoldButton.Click += BoldButton_Click;
            // 
            // ItalicButton
            // 
            ItalicButton.IconChar = FontAwesome.Sharp.IconChar.Italic;
            ItalicButton.IconColor = Color.Black;
            ItalicButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            ItalicButton.IconSize = 24;
            ItalicButton.Location = new Point(476, 18);
            ItalicButton.Name = "ItalicButton";
            ItalicButton.Size = new Size(50, 44);
            ItalicButton.TabIndex = 8;
            ItalicButton.TextAlign = ContentAlignment.MiddleLeft;
            ItalicButton.UseVisualStyleBackColor = true;
            ItalicButton.Click += ItalicButton_Click;
            // 
            // UnderlineButton
            // 
            UnderlineButton.IconChar = FontAwesome.Sharp.IconChar.Underline;
            UnderlineButton.IconColor = Color.Black;
            UnderlineButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            UnderlineButton.IconSize = 24;
            UnderlineButton.Location = new Point(532, 18);
            UnderlineButton.Name = "UnderlineButton";
            UnderlineButton.Size = new Size(50, 44);
            UnderlineButton.TabIndex = 9;
            UnderlineButton.TextAlign = ContentAlignment.MiddleLeft;
            UnderlineButton.UseVisualStyleBackColor = true;
            UnderlineButton.Click += UnderlineButton_Click;
            // 
            // StrikeThroughButton
            // 
            StrikeThroughButton.IconChar = FontAwesome.Sharp.IconChar.Strikethrough;
            StrikeThroughButton.IconColor = Color.Black;
            StrikeThroughButton.IconFont = FontAwesome.Sharp.IconFont.Auto;
            StrikeThroughButton.IconSize = 24;
            StrikeThroughButton.Location = new Point(588, 18);
            StrikeThroughButton.Name = "StrikeThroughButton";
            StrikeThroughButton.Size = new Size(50, 44);
            StrikeThroughButton.TabIndex = 10;
            StrikeThroughButton.TextAlign = ContentAlignment.MiddleLeft;
            StrikeThroughButton.UseVisualStyleBackColor = true;
            StrikeThroughButton.Click += StrikeThroughButton_Click;
            // 
            // OKButton
            // 
            OKButton.DialogResult = DialogResult.OK;
            OKButton.Location = new Point(524, 182);
            OKButton.Name = "OKButton";
            OKButton.Size = new Size(54, 50);
            OKButton.TabIndex = 11;
            OKButton.Text = "O&K";
            OKButton.UseVisualStyleBackColor = true;
            // 
            // CloseButton
            // 
            CloseButton.DialogResult = DialogResult.Cancel;
            CloseButton.Location = new Point(584, 182);
            CloseButton.Name = "CloseButton";
            CloseButton.Size = new Size(54, 50);
            CloseButton.TabIndex = 12;
            CloseButton.Text = "&Cancel";
            CloseButton.UseVisualStyleBackColor = true;
            CloseButton.Click += CancelButton_Click;
            // 
            // FontSelectionDialog
            // 
            AcceptButton = OKButton;
            AutoScaleDimensions = new SizeF(7F, 16F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(649, 244);
            Controls.Add(CloseButton);
            Controls.Add(OKButton);
            Controls.Add(StrikeThroughButton);
            Controls.Add(UnderlineButton);
            Controls.Add(ItalicButton);
            Controls.Add(BoldButton);
            Controls.Add(groupBox5);
            Controls.Add(groupBox4);
            Controls.Add(groupBox1);
            Font = new Font("Tahoma", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FontSelectionDialog";
            ShowIcon = false;
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Select Font";
            TopMost = true;
            Paint += FontSelectionDialog_Paint;
            groupBox1.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox5.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private GroupBox groupBox4;
        private GroupBox groupBox5;
        private ComboBox FontSizeCombo;
        private FontAwesome.Sharp.IconButton BoldButton;
        private FontAwesome.Sharp.IconButton ItalicButton;
        private FontAwesome.Sharp.IconButton UnderlineButton;
        private FontAwesome.Sharp.IconButton StrikeThroughButton;
        private Button OKButton;
        private Button CloseButton;
        private Label ExampleTextLabel;
        private ListView FontListView;
        private ColumnHeader columnHeader1;
    }
}