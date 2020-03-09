using System.Collections.Generic;

namespace VisualNovelData.Data
{
    public partial class Event
    {
        public interface IParameterList : IReadOnlyList<string>
        {
        }
    }
}
