using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelData.Data
{
    public sealed class CharacterAsset : ScriptableObject, ICharacterData
    {
        public const string Extension = "vsc";

        [SerializeField]
        private CharacterData data = new CharacterData();

        public ReadCharacterData Data
            => this.data;

        public ILanguageList Languages
            => this.data.Languages;

        public ICharacterDictionary Characters
            => this.data.Characters;

        public IContentDictionary Contents
            => this.data.Contents;

        public CharacterRow GetCharacter(string id)
            => this.data.GetCharacter(id);

        public void AddCharacter(CharacterRow character)
            => this.data.AddCharacter(character);

        public void ClearCharacters()
            => this.data.ClearCharacters();

        public void AddLanguage(string language)
            => this.data.AddLanguage(language);

        public void AddLanguages(in Segment<string> languages)
            => this.data.AddLanguages(languages);

        public void ClearLanguages()
            => this.data.ClearLanguages();

        public ContentRow GetContent(int id)
            => this.data.GetContent(id);

        public void AddContent(ContentRow content)
            => this.data.AddContent(content);

        public void RemoveContent(int id)
            => this.data.RemoveContent(id);

        public void ClearContents()
            => this.data.ClearContents();

        public void Clear()
            => this.data.Clear();
    }
}