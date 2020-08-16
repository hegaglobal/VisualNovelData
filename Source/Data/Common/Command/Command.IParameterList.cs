using System.Collections.Generic;

namespace VisualNovelData.Data
{
    public partial class Command
    {
        public interface IParameterList : IReadOnlyList<string>
        {
        }
    }
}
