using System.Collections.Generic;

namespace VisualNovelData.Data
{
    public interface IEventDictionary : IReadOnlyDictionary<string, EventRow>
    {
    }
}