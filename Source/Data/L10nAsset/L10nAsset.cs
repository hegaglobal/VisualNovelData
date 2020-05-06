using System;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelData.Data
{
    public sealed class L10nAsset : ScriptableObject, IL10nData
    {
        public const string Extension = "vsl";

        [SerializeField]
        private LanguageList languages = new LanguageList();

        public ILanguageList Languages
            => this.languages;

        [SerializeField]
        private L10nTextDictionary l10nTexts = new L10nTextDictionary();

        public IL10nTextDictionary L10nTexts
            => this.l10nTexts;

        [SerializeField]
        private ContentDictionary contents = new ContentDictionary();

        public IContentDictionary Contents
            => this.contents;

        public L10nTextRow GetText(string id)
            => id == null ? null :
               this.l10nTexts.ContainsKey(id) ? this.l10nTexts[id] : null;

        public void AddText(L10nTextRow text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            if (text.Id == null)
                return;

            this.l10nTexts[text.Id] = text;
        }

        public void ClearTexts()
            => this.l10nTexts.Clear();

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

        public void Clear()
        {
            ClearLanguages();
            ClearContents();
            ClearTexts();
        }

        [Serializable]
        private sealed class L10nTextDictionary : SerializableDictionary<string, L10nTextRow>, IL10nTextDictionary
        { }
    }
}