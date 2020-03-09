using System;
using UnityEngine;

namespace VisualNovelData.Collections
{
    [Serializable]
    public class List<T> : System.Collections.Generic.List<T>, IReadOnlyList<T>,
          ISerializationCallbackReceiver
    {
        [SerializeField]
        private T[] values = new T[0];

        public List() : base() { }

        public List(System.Collections.Generic.IEnumerable<T> collection) : base(collection) { }

        public List(int capacity) : base(capacity) { }

        public void OnAfterDeserialize()
        {
            Clear();
            AddRange(this.values);

            Array.Resize(ref this.values, 0);
        }

        public void OnBeforeSerialize()
        {
            Array.Resize(ref this.values, this.Count);

            for (var i = 0; i < this.Count; i++)
            {
                this.values[i] = this[i];
            }
        }

        List<TOutput> IReadOnlyList<T>.ConvertAll<TOutput>(Converter<T, TOutput> converter)
            => new List<TOutput>(ConvertAll(converter));

        List<T> IReadOnlyList<T>.FindAll(Predicate<T> match)
            => new List<T>(FindAll(match));

        List<T> IReadOnlyList<T>.GetRange(int index, int count)
            => new List<T>(GetRange(index, count));
    }
}