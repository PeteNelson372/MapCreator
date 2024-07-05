namespace MapCreator
{
    internal class Cmd_CreateMapFrame(MapCreatorMap map, MapFrame? frame, Color frameTint, int frameTintOpacity, float frameScale) : IMapOperation
    {
        private MapCreatorMap Map = map;
        private MapFrame? Frame = frame;
        private MapFrame? StoredFrame = null;
        private Color FrameTint = frameTint;
        private int FrameTintOpacity = frameTintOpacity;
        private float FrameScale = frameScale;

        public void DoOperation()
        {
            StoredFrame = Frame;

            OverlayMethods.CreateFrame(Map, Frame, FrameTint, FrameTintOpacity, FrameScale);
        }

        public void UndoOperation()
        {
            if (StoredFrame != null)
            {
                Frame = StoredFrame;

                OverlayMethods.CreateFrame(Map, Frame, FrameTint, FrameTintOpacity, FrameScale);
            }
            else
            {
                OverlayMethods.RemoveFrame(Map);
            }
        }
    }
}
