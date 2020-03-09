namespace VisualNovelData.Data
{
    public readonly struct ReadNovelData
    {
        private readonly NovelData data;

        public ILanguageList Languages
            => this.data.Languages;

        public IConversationDictionary Conversations
            => this.data.Conversations;

        private ReadNovelData(NovelData data)
        {
            this.data = data;
        }

        public ConversationRow GetConversation(string id)
            => this.data.GetConversation(id);

        public static implicit operator ReadNovelData(NovelData data)
            => new ReadNovelData(data);
    }
}