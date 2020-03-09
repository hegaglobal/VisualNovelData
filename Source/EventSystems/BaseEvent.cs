using System;
using System.Collections.Generic;

namespace VisualNovelData.EventSystems
{
    public abstract class BaseEvent : IEvent
    {
        protected Converter converter { get; } = new Converter();

        public abstract void Invoke(in Segment<object> parameters);
    }
}