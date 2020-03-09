namespace VisualNovelData.Data
{
    public interface IQuestData
    {
        IQuestDictionary Quests { get; }

        QuestRow GetQuest(string id);

        void AddQuest(QuestRow quest);

        void Clear();
    }
}