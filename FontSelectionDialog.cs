using System.Drawing.Text;

namespace MapCreator
{
    public partial class FontSelectionDialog : Form
    {
        private bool isBold = false;
        private bool isItalic = false;
        private bool isUnderline = false;
        private bool isStrikeout = false;

        public Font? SelectedFont = null;

        public FontSelectionDialog()
        {
            InitializeComponent();

            InstalledFontCollection installedFontCollection = new();

            // Get the array of FontFamily objects.
            foreach (var t in installedFontCollection.Families.Where(t => t.IsStyleAvailable(FontStyle.Regular)))
            {
                ListViewItem lvi = new()
                {
                    Text = t.Name,
                    Font = new Font(t, 12),
                    ToolTipText = t.Name
                };

                FontListView.Items.Add(lvi);
            }

            FontSizeCombo.SelectedIndex = 7; // 12 points
            FontSizeCombo.Text = "12";

            SelectedFont = Font;

            SetFont();
            SetExampleText();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void BoldButton_Click(object sender, EventArgs e)
        {
            isBold = !isBold;

            if (isBold)
            {
                BoldButton.BackColor = ColorTranslator.FromHtml("#D2F1C1");
            }
            else
            {
                BoldButton.BackColor = Color.White;
            }

            SetFont();
            SetExampleText();
        }

        private void ItalicButton_Click(object sender, EventArgs e)
        {
            isItalic = !isItalic;

            if (isItalic)
            {
                ItalicButton.BackColor = ColorTranslator.FromHtml("#D2F1C1");
            }
            else
            {
                ItalicButton.BackColor = Color.White;
            }

            SetFont();
            SetExampleText();
        }

        private void UnderlineButton_Click(object sender, EventArgs e)
        {
            isUnderline = !isUnderline;

            if (isUnderline)
            {
                UnderlineButton.BackColor = ColorTranslator.FromHtml("#D2F1C1");
            }
            else
            {
                UnderlineButton.BackColor = Color.White;
            }

            SetFont();
            SetExampleText();
        }

        private void StrikeThroughButton_Click(object sender, EventArgs e)
        {
            isStrikeout = !isStrikeout;

            if (isStrikeout)
            {
                StrikeThroughButton.BackColor = ColorTranslator.FromHtml("#D2F1C1");
            }
            else
            {
                StrikeThroughButton.BackColor = Color.White;
            }

            SetFont();
            SetExampleText();
        }

        private void FontSizeCombo_TextChanged(object sender, EventArgs e)
        {
            SetFont();
            SetExampleText();
        }

        private void FontListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetFont();
            SetExampleText();
        }

        private void SetExampleText()
        {
            ExampleTextLabel.Font = SelectedFont;
            ExampleTextLabel.Text = "The quick brown fox";
        }

        private void SetFont()
        {
            ListView.SelectedListViewItemCollection selectedItems = FontListView.SelectedItems;

            if (selectedItems.Count > 0)
            {
                ListViewItem lvi = selectedItems[0];

                if (lvi != null)
                {
                    FontFamily ff = lvi.Font.FontFamily;

                    if (ff != null)
                    {

                        if (float.TryParse(FontSizeCombo.Text, out float fontSize))
                        {
                            FontStyle fs = FontStyle.Regular;

                            if (isBold)
                            {
                                fs |= FontStyle.Bold;
                            }

                            if (isItalic)
                            {
                                fs |= FontStyle.Italic;
                            }

                            if (isUnderline)
                            {
                                fs |= FontStyle.Underline;
                            }

                            if (isStrikeout)
                            {
                                fs |= FontStyle.Strikeout;
                            }

                            try
                            {
                                SelectedFont = new Font(ff, fontSize, fs);
                            }
                            catch { }
                        }
                    }
                }
            }
        }

        private void FontSelectionDialog_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, SystemColors.Control, ButtonBorderStyle.Solid);
        }
    }
}
