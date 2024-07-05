namespace MapCreator
{
    public class UndoStack<MapOperation>(int capacity)
    {
        private readonly LinkedList<MapOperation> items = new();
        public List<MapOperation> Items => items.ToList();
        public int Capacity { get; } = capacity;

        public void Push(MapOperation item)
        {
            // full
            if (items.Count == Capacity)
            {
                // we should remove first, because some times, if we exceeded the size of the internal array
                // the system will allocate new array.
                items.RemoveFirst();
                items.AddLast(item);
            }
            else
            {
                items.AddLast(new LinkedListNode<MapOperation>(item));
            }
        }

        public MapOperation? Pop()
        {
            if (items.Count == 0)
            {
                return default;
            }
            var ls = items.Last;
            items.RemoveLast();
            return ls == null ? default : ls.Value;
        }
    }

}
