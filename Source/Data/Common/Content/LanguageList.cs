using System;

namespace VisualNovelData.Data
{
    using Collections;

    public interface ILanguageList : IReadOnlyList<string>
    { }

    [Serializable]
    public sealed class LanguageList : List<string>, ILanguageList
    { }
}