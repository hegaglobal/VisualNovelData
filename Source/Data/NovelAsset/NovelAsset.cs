using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelData.Data
{
    public sealed class NovelAsset : ScriptableObject, INovelData
    {
        public const string Extension = "vsn";

        [SerializeField]
        private NovelData data = new NovelData();

        public ReadNovelData Data
            => this.data;

        public ILanguageList Languages
            => this.data.Languages;

        public IConversationDictionary Conversations
            => this.data.Conversations;

        public void AddLanguage(string language)
            => this.data.AddLanguage(language);

        public void AddLanguages(in Segment<string> languages)
            => this.data.AddLanguages(languages);

        public void ClearLanguages()
            => this.data.ClearLanguages();

        public ConversationRow GetConversation(string id)
            => this.data.GetConversation(id);

        public void AddConversation(ConversationRow conversation)
            => this.data.AddConversation(conversation);

        public void ClearConversations()
            => this.data.ClearConversations();
    }
}