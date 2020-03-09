namespace VisualNovelData.Data
{
    using Collections;

    public interface IConversationDictionary : IReadOnlyDictionary<string, ConversationRow>
    {
    }
}