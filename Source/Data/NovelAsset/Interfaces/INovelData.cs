using System.Collections.Generic;

namespace VisualNovelData.Data
{
    public interface INovelData
    {
        ILanguageList Languages { get; }

        IConversationDictionary Conversations { get; }

        void AddLanguage(string language);

        void AddLanguages(in Segment<string> languages);

        void ClearLanguages();

        ConversationRow GetConversation(string id);

        void AddConversation(ConversationRow conversation);

        void ClearConversations();
    }
}