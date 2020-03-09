using System.Collections.Generic;

namespace VisualNovelData.EventSystems
{
    public interface IEvent
    {
        void Invoke(in Segment<object> parameters);
    }
}