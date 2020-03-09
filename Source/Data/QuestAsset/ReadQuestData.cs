namespace VisualNovelData.Data
{
    public readonly struct ReadQuestData
    {
        private readonly QuestData data;

        public IQuestDictionary Quests
            => this.data.Quests;

        private ReadQuestData(QuestData data)
        {
            this.data = data;
        }

        public QuestRow GetQuest(string id)
            => this.data.GetQuest(id);

        public static implicit operator ReadQuestData(QuestData data)
            => new ReadQuestData(data);
    }
}