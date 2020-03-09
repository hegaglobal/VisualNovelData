using System;
using System.Collections.Generic;

namespace VisualNovelData.Data
{
    [Serializable]
    public sealed class EventList : List<Event>, IEventList { }
}
