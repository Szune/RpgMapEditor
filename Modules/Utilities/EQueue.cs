using System.Collections.Generic;

namespace RpgMapEditor.Modules.Utilities
{
    public enum PeekAt
    {
        FirstAdded,
        LastAdded
    }
    public class EQueue<T>
    {
        private List<T> _queue = new List<T>();

        public void Add(T item)
        {
            _queue.Add(item);
        }

        public int Count { get { return _queue.Count; } }

        public void Clear()
        {
            _queue.Clear();
        }

        public T Peek(PeekAt dir = PeekAt.FirstAdded)
        {
            if (_queue.Count == 0)
            {
                throw new KeyNotFoundException("Index out of bounds, queue is empty.");
            }
            if (dir == PeekAt.FirstAdded)
            {
                return _queue[0];
            }
            else
            {
                return _queue[_queue.Count - 1];
            }
        }

        public T Pop(PeekAt dir = PeekAt.FirstAdded)
        {
            if (_queue.Count == 0)
            {
                throw new KeyNotFoundException("Index out of bounds, queue is empty.");
            }
            if (dir == PeekAt.FirstAdded)
            {
                T val = _queue[0];
                _queue.RemoveAt(0);
                return val;
            }
            else
            {
                T val = _queue[Count - 1];
                _queue.RemoveAt(Count - 1);
                return val;
            }
        }
    }
}
