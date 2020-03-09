using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelData.Data
{
    public sealed class L10nAsset : ScriptableObject, IL10nData
    {
        public const string Extension = "vsl";

        [SerializeField]
        private L10nData data = new L10nData();

        public ReadL10nData Data
            => this.data;

        public ILanguageList Languages
            => this.data.Languages;

        public IL10nTextDictionary L10nTexts
            => this.data.L10nTexts;

        public IContentDictionary Contents
            => this.data.Contents;

        public L10nTextRow GetText(string id)
            => this.data.GetText(id);

        public void AddText(L10nTextRow text)
            => this.data.AddText(text);

        public void ClearTexts()
            => this.data.ClearTexts();

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