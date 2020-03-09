namespace VisualNovelData.Collections
{
    public interface IReadOnlyDictionary<TKey, TValue> : System.Collections.Generic.IReadOnlyDictionary<TKey, TValue>
    {
        bool ContainsValue(TValue value);
    }
}