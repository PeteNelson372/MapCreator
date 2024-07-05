
namespace MapCreator
{
    internal class Cmd_ChangeBoxColor : IMapOperation
    {
        private PlacedMapBox SelectedBox;
        private Color BoxColor;
        private Color PreviousBoxColor = Color.White;

        public Cmd_ChangeBoxColor(PlacedMapBox selectedBox, Color boxColor)
        {
            SelectedBox = selectedBox;
            PreviousBoxColor = SelectedBox.BoxTint;
            BoxColor = boxColor;
        }

        public void DoOperation()
        {;
            SelectedBox.BoxTint = BoxColor;
        }

        public void UndoOperation()
        {
            SelectedBox.BoxTint = PreviousBoxColor;
        }
    }
}
