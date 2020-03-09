using System;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelData.Data
{
    [Serializable]
    public sealed class ConversationRow : DataRow
    {
        [SerializeField]
        private string id = string.Empty;

        public string Id
            => this.id;

        [SerializeField]
        private string startingDialogue = string.Empty;

        public string StartingDialogue
            => this.startingDialogue;

        [SerializeField]
        private DialogueDictionary dialogues = new DialogueDictionary();

        public IDialogueDictionary Dialogues
            => this.dialogues;

        [SerializeField]
        private ContentDictionary contents = new ContentDictionary();

        public IContentDictionary Contents
            => this.contents;

        public ConversationRow(int row, string id) : base(row)
        {
            this.id = id;
        }

        public DialogueRow GetDialogue(string id)
            => id == null ? null :
               this.dialogues.ContainsKey(id) ? this.dialogues[id] : null;

        public void AddDialogue(DialogueRow dialogue)
        {
            if (dialogue == null)
                throw new ArgumentNullException(nameof(dialogue));

            if (dialogue.Id == null)
                return;

            this.dialogues[dialogue.Id] = dialogue;

            if (this.dialogues.Count == 1)
            {
                this.startingDialogue = dialogue.Id;
            }
        }

        public void ClearDialogues()
        {
            this.startingDialogue = string.Empty;
            this.dialogues.Clear();
        }

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
        {
            this.contents.Clear();
        }

        [Serializable]
        private sealed class DialogueDictionary : SerializableDictionary<string, DialogueRow>, IDialogueDictionary
        { }
    }
}