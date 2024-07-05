namespace MapCreator
{
    internal class Cmd_ChangeLabelText : IMapOperation
    {
        private readonly MapLabel Label;
        private readonly string LabelText;
        private readonly string PreviousLabelText;

        public Cmd_ChangeLabelText(MapLabel label, string labelText)
        {
            Label = label;
            LabelText = labelText;

            PreviousLabelText = Label.LabelText;
        }

        public void DoOperation()
        {
            if (Label.LabelPaint != null)
            {
                Label.LabelText = LabelText;
                Label.Width = (uint)Label.LabelPaint.MeasureText(Label.LabelText);
            }
        }

        public void UndoOperation()
        {
            if (Label.LabelPaint != null)
            {
                Label.LabelText = PreviousLabelText;
                Label.Width = (uint)Label.LabelPaint.MeasureText(Label.LabelText);
            }
        }
    }
}
