using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GMTK25
{
    public class LoopQueue<T> : IEnumerable<T>
    {
        private readonly int capacity;
        private readonly Queue<T> items;

        public int Count => items.Count;

        public LoopQueue(int capacity)
        {
            this.capacity = capacity;
            items = new Queue<T>(capacity);
        }

        public T? Peek()
        {
            return items.FirstOrDefault();
        }

        public void Enqueue(T item)
        {
            if (Count == capacity) _ = items.Dequeue();
            items.Enqueue(item);
        }

        public void Dequeue()
        {
            _ = items.TryDequeue(out _);
        }

        public void Cycle()
        {
            if (items.Count == 0) return;
            items.Enqueue(items.Dequeue());
        }

        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}