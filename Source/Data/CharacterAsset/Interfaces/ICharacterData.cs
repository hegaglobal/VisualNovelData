using System.Collections.Generic;

namespace VisualNovelData.Data
{
    public interface ICharacterData
    {
        ILanguageList Languages { get; }

        ICharacterDictionary Characters { get; }

        IContentDictionary Contents { get; }

        CharacterRow GetCharacter(string id);

        void AddCharacter(CharacterRow character);

        void ClearCharacters();

        void AddLanguage(string language);

        void AddLanguages(in Segment<string> languages);

        void ClearLanguages();

        ContentRow GetContent(int id);

        void AddContent(ContentRow content);

        void RemoveContent(int id);

        void ClearContents();
    }
}