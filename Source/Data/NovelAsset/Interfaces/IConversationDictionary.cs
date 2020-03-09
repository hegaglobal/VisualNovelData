using System.Collections.Generic;

namespace VisualNovelData.Data
{
    public interface IConversationDictionary : IReadOnlyDictionary<string, ConversationRow>
    {
    }
}