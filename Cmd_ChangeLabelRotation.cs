namespace MapCreator
{
    internal class Cmd_ChangeLabelRotation : IMapOperation
    {
        private MapLabel Label;
        private float RotationValue;
        private MainForm Form;

        public Cmd_ChangeLabelRotation(MapLabel label, float rotationValue, MainForm mainForm)
        {
            Label = label;
            RotationValue = rotationValue;
            Form = mainForm;
        }

        public void DoOperation()
        {
            Label.LabelRotationDegrees = RotationValue;
        }

        public void UndoOperation()
        {
            Label.LabelRotationDegrees = 0;
            Form.LabelRotationUpDown.Value = 0;
        }
    }
}
