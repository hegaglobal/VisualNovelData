using System;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelData.Data
{
    public interface IContentDictionary : IReadOnlyDictionary<int, ContentRow>
    { }

    [Serializable]
    public sealed class ContentDictionary : SerializableDictionary<int, ContentRow>, IContentDictionary
    { }
}