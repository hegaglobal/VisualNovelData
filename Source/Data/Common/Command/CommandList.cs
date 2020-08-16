using System;
using System.Collections.Generic;

namespace VisualNovelData.Data
{
    [Serializable]
    public sealed class CommandList : List<Command>, ICommandList { }
}
