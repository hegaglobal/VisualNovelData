using System;
using System.Text;
using System.Collections.Generic;

namespace VisualNovelData.Parser
{
    public interface IParser<T>
    {
        bool TryParse(string str, out T result);
    }
}