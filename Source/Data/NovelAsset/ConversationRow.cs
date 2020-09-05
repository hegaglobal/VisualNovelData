using System;
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
            this.id = id ?? string.Empty;
        }

        public DialogueRow GetDialogue(string id)
            => id == null ? DialogueRow.None :
               this.dialogues.ContainsKey(id) ? this.dialogues[id] : DialogueRow.None;

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
            => this.contents.ContainsKey(id) ? this.contents[id] : ContentRow.None;

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

        public static ConversationRow None { get; } = new ConversationRow(-1, string.Empty);

        [Serializable]
        private sealed class DialogueDictionary : SerializableDictionary<string, DialogueRow>, IDialogueDictionary
        { }
    }

    public static class ConversationRowExtensions
    {
        public static bool IsNullOrNone(this ConversationRow self)
            => self == null || self == ConversationRow.None;
    }
}