namespace MapCreator
{
    public partial class EditLabelForm : Form
    {
        public string GetLabelText()
        {
            return LabelTextBox.Text;
        }

        public void SetLabelText(string labelText)
        {
            LabelTextBox.Text = labelText;
        }

        public EditLabelForm()
        {
            InitializeComponent();
        }
    }
}
