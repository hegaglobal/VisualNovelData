using System;
using UnityEngine;

namespace VisualNovelData.Data
{
    using Collections;

    [Serializable]
    public sealed class QuestData : IQuestData
    {
        [SerializeField]
        private QuestDictionary quest = new QuestDictionary();

        public IQuestDictionary Quests
            => this.quest;

        public QuestRow GetQuest(string id)
            => this.quest.ContainsKey(id) ? this.quest[id] : null;

        public void AddQuest(QuestRow quest)
        {
            if (quest == null)
                throw new ArgumentNullException(nameof(quest));

            if (quest.Id == null || this.quest.ContainsKey(quest.Id))
                return;

            this.quest.Add(quest.Id, quest);
        }

        public void ClearQuests()
        {
            this.quest.Clear();
        }

        [Serializable]
        private sealed class QuestDictionary : Dictionary<string, QuestRow>, IQuestDictionary
        { }
    }
}