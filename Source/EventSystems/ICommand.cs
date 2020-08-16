using System.Collections.Generic;

namespace VisualNovelData.CommandSystems
{
    public interface ICommand
    {
        void Invoke(in Segment<object> parameters);
    }
}