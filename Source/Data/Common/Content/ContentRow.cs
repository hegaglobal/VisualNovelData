using System;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelData.Data
{
    [Serializable]
    public sealed class ContentRow : DataRow
    {
        public int Id
            => this.Row;

        [SerializeField]
        private LocalizationDictionary localization = new LocalizationDictionary();

        public ILocalizationDictionary Localization
            => this.localization;

        public ContentRow(int id, in Segment<string> languages, in Segment<string> localization) : base(id)
        {
            for (var i = 0; i < languages.Count; i++)
            {
                if (this.localization.ContainsKey(languages[i]))
                    continue;

                var value = !localization.HasSource || i >= localization.Count
                    ? string.Empty
                    : localization[i];

                this.localization.Add(languages[i], value);
            }
        }

        public string GetLocalization(string language, bool @default = true)
        {
            if (language != null &&
                this.localization.ContainsKey(language))
            {
                return this.localization[language];
            }

            if (@default)
            {
                var lang = GetDefaultLanguage();

                if (this.localization.ContainsKey(lang))
                    return this.localization[lang];
            }

            return string.Empty;
        }

        public bool ContainsLanguage(string language)
            => language == null ? false : this.localization.ContainsKey(language);

        public string GetDefaultLanguage()
        {
            var key = string.Empty;

            foreach (var kv in this.localization)
            {
                key = kv.Key;
                break;
            }

            return key;
        }

        [Serializable]
        private sealed class LocalizationDictionary : SerializableDictionary<string, string>, ILocalizationDictionary
        { }
    }
}