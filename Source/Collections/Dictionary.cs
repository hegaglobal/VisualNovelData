using System;
using UnityEngine;

namespace VisualNovelData.Collections
{
    [Serializable]
    public abstract class Dictionary<TKey, TValue> : SerializableDictionary<TKey, TValue>,
          IReadOnlyDictionary<TKey, TValue>
    {
    }
}