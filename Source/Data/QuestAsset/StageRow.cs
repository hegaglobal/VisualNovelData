using System;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelData.Data
{
    [Serializable]
    public class StageRow
    {
        [SerializeField]
        private int index = 0;

        public int Index
            => this.index;

        [SerializeField]
        private int maxConstraint = -1;

        /// <summary>
        /// The maximum value at which this stage is still valid to be invoked.
        /// Negative value (less than 0) means no constraint.
        /// </summary>
        public int MaxConstraint
            => this.maxConstraint;

        [SerializeField]
        private EventList events = new EventList();

        public IEventList Events
            => this.events;

        public StageRow(int stage, IReadOnlyList<Event> events, int maxConstraint = -1)
        {
            this.index = stage;

            if (events != null)
            {
                this.events.AddRange(events);
            }

            this.maxConstraint = maxConstraint;
        }
    }
}