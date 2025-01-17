﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelData.Data
{
    public sealed class NovelAsset : ScriptableObject, INovelData
    {
        public const string Extension = "vsn";

        [SerializeField]
        private LanguageList languages = new LanguageList();

        public ILanguageList Languages
            => this.languages;

        [SerializeField]
        private ConversationDictionary conversations = new ConversationDictionary();

        public IConversationDictionary Conversations
            => this.conversations;

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

        public ConversationRow GetConversation(string id)
            => this.conversations.ContainsKey(id) ? this.conversations[id] : ConversationRow.None;

        public void AddConversation(ConversationRow conversation)
        {
            if (conversation == null)
                throw new ArgumentNullException(nameof(conversation));

            if (conversation.Id == null)
                return;

            this.conversations[conversation.Id] = conversation;
        }

        public void ClearConversations()
            => this.conversations.Clear();

        public void Clear()
        {
            ClearLanguages();
            ClearConversations();
        }

        [Serializable]
        private sealed class ConversationDictionary : SerializableDictionary<string, ConversationRow>, IConversationDictionary
        { }
    }
}