using System;
using System.Collections.Generic;

namespace VisualNovelData.Data
{
    public interface ILanguageList : IReadOnlyList<string>
    { }

    [Serializable]
    public sealed class LanguageList : List<string>, ILanguageList
    { }
}