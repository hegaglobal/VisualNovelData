using System;

namespace VisualNovelData.Data
{
    using Collections;

    [Serializable]
    public sealed class EventList : List<Event>, IEventList { }
}
