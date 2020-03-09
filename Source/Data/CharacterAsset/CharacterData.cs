using System;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelData.Data
{
    [Serializable]
    public sealed class CharacterData : ICharacterData
    {
        [SerializeField]
        private LanguageList languages = new LanguageList();

        public ILanguageList Languages
            => this.languages;

        [SerializeField]
        private CharacterDictionary characters = new CharacterDictionary();

        public ICharacterDictionary Characters
            => this.characters;

        [SerializeField]
        private ContentDictionary contents = new ContentDictionary();

        public IContentDictionary Contents
            => this.contents;

        public CharacterRow GetCharacter(string id)
            => id == null ? null :
               this.characters.ContainsKey(id) ? this.characters[id] : null;

        public void AddCharacter(CharacterRow character)
        {
            if (character == null)
                throw new ArgumentNullException(nameof(character));

            if (character.Id == null)
                return;

            this.characters[character.Id] = character;
        }

        public void ClearCharacters()
            => this.characters.Clear();

        public void AddLanguage(string language)
        {
            if (this.languages.Contains(language))
                return;

            this.languages.Add(language);
        }

        public void AddLanguages(in Segment<string> languages)
        {
            for (var i = 0; i < languages.Count; i++)
            {
                if (this.languages.Contains(languages[i]))
                    continue;

                this.languages.Add(languages[i]);
            }
        }

        public void ClearLanguages()
            => this.languages.Clear();

        public ContentRow GetContent(int id)
            => this.contents.ContainsKey(id) ? this.contents[id] : null;

        public void AddContent(ContentRow content)
        {
            if (content == null)
                throw new ArgumentNullException(nameof(content));

            this.contents[content.Id] = content;
        }

        public void RemoveContent(int id)
        {
            if (!this.contents.ContainsKey(id))
                return;

            this.contents.Remove(id);
        }

        public void ClearContents()
            => this.contents.Clear();

        [Serializable]
        private sealed class CharacterDictionary : Collections.Dictionary<string, CharacterRow>, ICharacterDictionary
        { }
    }
}