using System;

namespace VisualNovelData.Data
{
    using Collections;

    public interface IContentDictionary : IReadOnlyDictionary<int, ContentRow>
    { }

    [Serializable]
    public sealed class ContentDictionary : Dictionary<int, ContentRow>, IContentDictionary
    { }
}