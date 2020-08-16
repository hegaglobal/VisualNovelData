using System;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelData.Data
{
    [Serializable]
    public partial class DialogueRow : DataRow
    {
        [SerializeField]
        private string id = string.Empty;

        public string Id
            => this.id;

        [SerializeField]
        private float delay;

        public float Delay
            => this.delay;

        [SerializeField]
        private ChoiceDictionary choices = new ChoiceDictionary();

        public IChoiceDictionary Choices
            => this.choices;

        [SerializeField]
        private string actor1;

        public string Actor1
            => this.actor1;

        [SerializeField]
        private CommandList actions1 = new CommandList();

        public ICommandList Actions1
            => this.actions1;

        [SerializeField]
        private string actor2;

        public string Actor2
            => this.actor2;

        [SerializeField]
        private CommandList actions2 = new CommandList();

        public ICommandList Actions2
            => this.actions2;

        [SerializeField]
        private string actor3;

        public string Actor3
            => this.actor3;

        [SerializeField]
        private CommandList actions3 = new CommandList();

        public ICommandList Actions3
            => this.actions3;

        [SerializeField]
        private string actor4;

        public string Actor4
            => this.actor4;

        [SerializeField]
        private CommandList actions4 = new CommandList();

        public ICommandList Actions4
            => this.actions4;

        [SerializeField]
        private int[] highlight;

        public ReadArray<int> Highlight
            => this.highlight;

        [SerializeField]
        private CommandList commandsOnStart = new CommandList();

        public ICommandList CommandsOnStart
            => this.commandsOnStart;

        [SerializeField]
        private CommandList commandsOnEnd = new CommandList();

        public ICommandList CommandsOnEnd
            => this.commandsOnEnd;

        protected DialogueRow(int row, string id) : base(row)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            this.id = id;
        }

        public DialogueRow(int row, string id, float delay, string actor1, IReadOnlyList<Command> actions1,
                           string actor2, IReadOnlyList<Command> actions2, string actor3, IReadOnlyList<Command> actions3,
                           string actor4, IReadOnlyList<Command> actions4, int[] highlight,
                           IReadOnlyList<Command> commandsOnStart = null, IReadOnlyList<Command> commandsOnEnd = null)
            : this(row, id)
        {
            this.delay = delay;
            this.actor1 = actor1;
            this.actor2 = actor2;
            this.actor3 = actor3;
            this.actor4 = actor4;
            this.highlight = highlight ?? new int[0];

            if (actions1 != null)
            {
                this.actions1.AddRange(actions1);
            }

            if (actions2 != null)
            {
                this.actions1.AddRange(actions2);
            }

            if (actions3 != null)
            {
                this.actions1.AddRange(actions3);
            }

            if (actions4 != null)
            {
                this.actions1.AddRange(actions4);
            }

            if (commandsOnStart != null)
            {
                this.commandsOnStart.AddRange(commandsOnStart);
            }

            if (commandsOnEnd != null)
            {
                this.commandsOnEnd.AddRange(commandsOnEnd);
            }
        }

        public virtual ChoiceRow GetChoice(int id)
            => this.choices.ContainsKey(id) ? this.choices[id] : null;

        public virtual void AddChoice(ChoiceRow choice)
        {
            if (choice == null)
                throw new ArgumentNullException(nameof(choice));

            this.choices[choice.Id] = choice;
        }

        public virtual void ClearChoices()
        {
            this.choices.Clear();
        }

        [Serializable]
        private sealed class ChoiceDictionary : SerializableDictionary<int, ChoiceRow>, IChoiceDictionary
        { }
    }

    [Serializable]
    public sealed class EndDialogueRow : DialogueRow
    {
        public const string Keyword = "[END]";

        public EndDialogueRow(int row, string id) : base(row, id)
        { }

        public EndDialogueRow(int row, string id, IReadOnlyList<Command> commandsOnStart = null,
                              IReadOnlyList<Command> commandsOnEnd = null)
            : base(row, id, 0f, string.Empty, null, string.Empty, null, string.Empty, null, string.Empty, null, null,
                   commandsOnStart, commandsOnEnd)
        { }

        public sealed override void AddChoice(ChoiceRow option) { }

        public sealed override void ClearChoices() { }
    }

    public static class DialogueRowExtensions
    {
        public static bool IsEnd(this DialogueRow dialogue)
            => dialogue != null &&
               (dialogue is EndDialogueRow || string.Equals(dialogue.Id, EndDialogueRow.Keyword));
    }
}