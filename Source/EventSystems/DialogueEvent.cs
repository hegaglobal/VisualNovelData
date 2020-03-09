using System;
using System.Collections.Generic;
using VisualNovelData.Data;

namespace VisualNovelData.EventSystems
{
    public abstract class DialogueEvent : BaseEvent
    {
        public sealed override void Invoke(in Segment<object> parameters)
        {
            if (parameters.Count <= 0)
                throw new ArgumentException($"Must have at least 1 parameter. The first parameter must be an instance of {typeof(DialogueRow).FullName}");

            if (!(parameters[0] is DialogueRow dialogue))
                throw new ArgumentException($"The first parameter must be an instance of {typeof(DialogueRow).FullName}");

            Invoke(dialogue, SegmentExtensions.Slice(parameters, 1));
        }

        protected abstract void Invoke(DialogueRow dialogue, in Segment<object> parameters);
    }
}