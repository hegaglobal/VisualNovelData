using System;
using System.Collections.Generic;

namespace VisualNovelData.Collections
{
    public interface IReadOnlyList<T> : System.Collections.Generic.IReadOnlyList<T>
    {
        int BinarySearch(T item);

        int BinarySearch(T item, IComparer<T> comparer);

        int BinarySearch(int index, int count, T item, IComparer<T> comparer);

        bool Contains(T item);

        List<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter);

        void CopyTo(int index, T[] array, int arrayIndex, int count);

        void CopyTo(T[] array, int arrayIndex);

        void CopyTo(T[] array);

        bool Exists(Predicate<T> match);

        T Find(Predicate<T> match);

        List<T> FindAll(Predicate<T> match);

        int FindIndex(int startIndex, int count, Predicate<T> match);

        int FindIndex(int startIndex, Predicate<T> match);

        int FindIndex(Predicate<T> match);

        T FindLast(Predicate<T> match);

        int FindLastIndex(int startIndex, int count, Predicate<T> match);

        int FindLastIndex(int startIndex, Predicate<T> match);

        int FindLastIndex(Predicate<T> match);

        void ForEach(Action<T> action);

        List<T> GetRange(int index, int count);

        int IndexOf(T item, int index, int count);

        int IndexOf(T item, int index);

        int IndexOf(T item);

        int LastIndexOf(T item);

        int LastIndexOf(T item, int index);

        int LastIndexOf(T item, int index, int count);

        T[] ToArray();

        bool TrueForAll(Predicate<T> match);
    }
}