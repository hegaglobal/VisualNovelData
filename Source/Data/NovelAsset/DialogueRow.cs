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
        private float delay = 0f;

        public float Delay
            => this.delay;

        [SerializeField]
        private ChoiceDictionary choices = new ChoiceDictionary();

        public IChoiceDictionary Choices
            => this.choices;

        [SerializeField]
        private string speaker = string.Empty;

        public string Speaker
            => this.speaker;

        [SerializeField]
        private string actor1 = string.Empty;

        public string Actor1
            => this.actor1;

        [SerializeField]
        private ActorCommandList actions1 = new ActorCommandList();

        public IActorCommandList Actions1
            => this.actions1;

        [SerializeField]
        private string actor2 = string.Empty;

        public string Actor2
            => this.actor2;

        [SerializeField]
        private ActorCommandList actions2 = new ActorCommandList();

        public IActorCommandList Actions2
            => this.actions2;

        [SerializeField]
        private string actor3 = string.Empty;

        public string Actor3
            => this.actor3;

        [SerializeField]
        private ActorCommandList actions3 = new ActorCommandList();

        public IActorCommandList Actions3
            => this.actions3;

        [SerializeField]
        private string actor4 = string.Empty;

        public string Actor4
            => this.actor4;

        [SerializeField]
        private ActorCommandList actions4 = new ActorCommandList();

        public IActorCommandList Actions4
            => this.actions4;

        [SerializeField]
        private int[] highlight = new int[0];

        public ReadArray1<int> Highlight
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
            this.id = id ?? string.Empty;
        }

        public DialogueRow(int row, string id, float delay, string speaker, string actor1, IReadOnlyList<Command> actions1,
                           string actor2, IReadOnlyList<Command> actions2, string actor3, IReadOnlyList<Command> actions3,
                           string actor4, IReadOnlyList<Command> actions4, int[] highlight,
                           IReadOnlyList<Command> commandsOnStart = null, IReadOnlyList<Command> commandsOnEnd = null)
            : this(row, id)
        {
            this.delay = delay;
            this.speaker = speaker ?? string.Empty;
            this.actor1 = actor1 ?? string.Empty;
            this.actor2 = actor2 ?? string.Empty;
            this.actor3 = actor3 ?? string.Empty;
            this.actor4 = actor4 ?? string.Empty;
            this.highlight = highlight ?? new int[0];

            AddRange(this.actions1, actions1, 1);
            AddRange(this.actions2, actions2, 2);
            AddRange(this.actions3, actions3, 3);
            AddRange(this.actions4, actions4, 4);
            AddRange(this.commandsOnStart, commandsOnStart);
            AddRange(this.commandsOnEnd, commandsOnEnd);
        }

        private void AddRange(CommandList list, IReadOnlyList<Command> commands)
        {
            if (commands == null)
                return;

            foreach (var command in commands)
            {
                if (command != null)
                    list.Add(command);
            }
        }

        private void AddRange(ActorCommandList list, IReadOnlyList<Command> commands, int actorNumber)
        {
            if (commands == null)
                return;

            foreach (var command in commands)
            {
                if (command != null)
                    list.Add(new ActorCommand(actorNumber, command));
            }
        }

        public virtual ChoiceRow GetChoice(int id)
            => this.choices.ContainsKey(id) ? this.choices[id] : ChoiceRow.None;

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

        public static DialogueRow None { get; } = new DialogueRow(-1, string.Empty);

        [Serializable]
        private sealed class ChoiceDictionary : SerializableDictionary<int, ChoiceRow>, IChoiceDictionary
        { }

        [Serializable]
        private sealed class ActorCommandList : List<ActorCommand>, IActorCommandList
        {
            Command IReadOnlyList<Command>.this[int index]
                => this[index];

            IEnumerator<Command> IEnumerable<Command>.GetEnumerator()
                => GetEnumerator();
        }
    }

    [Serializable]
    public sealed class EndDialogueRow : DialogueRow
    {
        public const string Keyword = "[END]";

        public EndDialogueRow(int row, string id) : base(row, id)
        { }

        public EndDialogueRow(int row, string id, IReadOnlyList<Command> commandsOnStart = null,
                              IReadOnlyList<Command> commandsOnEnd = null)
            : base(row, id, 0f, string.Empty, string.Empty, null, string.Empty, null, string.Empty, null, string.Empty, null,
                   null, commandsOnStart, commandsOnEnd)
        { }

        public sealed override void AddChoice(ChoiceRow option) { }

        public sealed override void ClearChoices() { }
    }

    public static class DialogueRowExtensions
    {
        public static bool IsEnd(this DialogueRow self)
            => self != null &&
               (self is EndDialogueRow || string.Equals(self.Id, EndDialogueRow.Keyword));

        public static bool IsNullOrNone(this DialogueRow self)
            => self == null || self == DialogueRow.None;
    }
}