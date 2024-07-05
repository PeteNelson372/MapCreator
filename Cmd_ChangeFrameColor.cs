namespace MapCreator
{
    internal class Cmd_ChangeFrameColor : IMapOperation
    {
        private PlacedMapFrame SelectedFrame;
        private Color FrameTint;
        private Color PreviousFrameTint = Color.White;

        public Cmd_ChangeFrameColor(PlacedMapFrame selectedFrame, Color frameTint)
        {
            SelectedFrame = selectedFrame;
            PreviousFrameTint = SelectedFrame.FrameTint;
            FrameTint = frameTint;
        }

        public void DoOperation()
        {
            SelectedFrame.FrameTint = FrameTint;
        }

        public void UndoOperation()
        {
            SelectedFrame.FrameTint = PreviousFrameTint;
        }
    }
}
