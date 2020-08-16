using System.Collections.Generic;

namespace VisualNovelData.Commands
{
    public interface ICommand
    {
        void Invoke(in Segment<object> parameters);
    }
}