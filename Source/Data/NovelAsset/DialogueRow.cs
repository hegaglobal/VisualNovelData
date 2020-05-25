﻿using System;
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
        private string actor;

        public string Actor
            => this.actor;

        [SerializeField]
        private string action;

        public string Action
            => this.action;

        [SerializeField]
        private int highlight;

        public int Highlight
            => this.highlight;

        [SerializeField]
        private EventList eventsOnStart = new EventList();

        public IEventList EventsOnStart
            => this.eventsOnStart;

        [SerializeField]
        private EventList eventsOnEnd = new EventList();

        public IEventList EventsOnEnd
            => this.eventsOnEnd;

        [SerializeField]
        private ChoiceDictionary choices = new ChoiceDictionary();

        public IChoiceDictionary Choices
            => this.choices;

        protected DialogueRow(int row, string id) : base(row)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            this.id = id;
        }

        public DialogueRow(int row, string id, float delay, string actor, string action, int highlight,
                           IReadOnlyList<Event> eventsOnStart = null, IReadOnlyList<Event> eventsOnEnd = null)
            : this(row, id)
        {
            this.delay = delay;
            this.actor = actor;
            this.action = action;
            this.highlight = highlight;

            if (eventsOnStart != null)
            {
                this.eventsOnStart.AddRange(eventsOnStart);
            }

            if (eventsOnEnd != null)
            {
                this.eventsOnEnd.AddRange(eventsOnEnd);
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

        public EndDialogueRow(int row, string id, IReadOnlyList<Event> eventsOnStart = null, IReadOnlyList<Event> eventsOnEnd = null)
            : base(row, id, 0f, string.Empty, string.Empty, -1, eventsOnStart, eventsOnEnd)
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