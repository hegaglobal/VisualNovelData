using System.Runtime.CompilerServices;

namespace VisualNovelData.Data
{
    public readonly struct ReadNovelData
    {
        public bool HasData { get; }

        private readonly NovelData data;

        public ILanguageList Languages
            => this.data.Languages;

        public IConversationDictionary Conversations
            => this.data.Conversations;

        private ReadNovelData(NovelData data)
        {
            this.data = data ?? _empty;
            this.HasData = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private NovelData GetData()
            => this.HasData ? this.data : _empty;

        public ConversationRow GetConversation(string id)
            => GetData().GetConversation(id);

        private static readonly NovelData _empty = new NovelData();

        public static implicit operator ReadNovelData(NovelData data)
            => new ReadNovelData(data);
    }
}