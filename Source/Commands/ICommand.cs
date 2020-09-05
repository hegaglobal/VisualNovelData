using System.Collections.Generic;

namespace VisualNovelData.Commands
{
    public interface ICommand
    {
        void Invoke(in Metadata metadata, in Segment<object> parameters);
    }
}