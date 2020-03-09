namespace VisualNovelData.Data
{
    using Collections;

    public interface ICharacterDictionary : IReadOnlyDictionary<string, CharacterRow>
    {
    }
}