using System;
using UnityEngine;

namespace VisualNovelData.Data
{
    public sealed class EventAsset : ScriptableObject, IEventData
    {
        public const string Extension = "vse";

        [SerializeField]
        private EventDictionary events = new EventDictionary();

        public IEventDictionary Events
            => this.events;

        public EventRow GetEvent(string id)
            => this.events.ContainsKey(id) ? this.events[id] : EventRow.None;

        public void AddEvent(EventRow @event)
        {
            if (@event == null)
                throw new ArgumentNullException(nameof(@event));

            if (@event.Id == null || this.events.ContainsKey(@event.Id))
                return;

            this.events.Add(@event.Id, @event);
        }

        public void Clear()
            => this.events.Clear();

        [Serializable]
        private sealed class EventDictionary : SerializableDictionary<string, EventRow>, IEventDictionary
        { }
    }
}