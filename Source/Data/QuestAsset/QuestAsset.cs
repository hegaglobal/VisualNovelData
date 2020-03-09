using UnityEngine;

namespace VisualNovelData.Data
{
    public sealed class QuestAsset : ScriptableObject, IQuestData
    {
        public const string Extension = "vsq";

        [SerializeField]
        private QuestData data = new QuestData();

        public ReadQuestData Data
            => this.data;

        public IQuestDictionary Quests
            => this.data.Quests;

        public QuestRow GetQuest(string id)
            => this.data.GetQuest(id);

        public void AddQuest(QuestRow quest)
            => this.data.AddQuest(quest);

        public void Clear()
            => this.data.Clear();
    }
}