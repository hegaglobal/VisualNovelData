using System.Collections.Generic;

namespace VisualNovelData.Data
{
    public interface ICharacterDictionary : IReadOnlyDictionary<string, CharacterRow>
    {
    }
}