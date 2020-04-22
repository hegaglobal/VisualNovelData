using System;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelData.EventSystems
{
    using Data;

    public sealed class QuestEventSystem
    {
        public IQuestData QuestData { get; private set; }

        public EventSystem EventSystem { get; private set; }

        public QuestEventSystem() { }

        public QuestEventSystem(EventSystem eventSystem)
            => Set(eventSystem);

        public QuestEventSystem(IQuestData questData)
            => Set(questData);

        public QuestEventSystem(IQuestData questData, EventSystem eventSystem)
        {
            Set(questData);
            Set(eventSystem);
        }

        public void Set(IQuestData questData)
            => this.QuestData = questData ?? throw new ArgumentNullException(nameof(questData));

        public void Set(EventSystem eventSystem)
            => this.EventSystem = eventSystem ?? throw new ArgumentNullException(nameof(eventSystem));

        private bool TryGetQuest(string questId, out QuestRow quest)
        {
            if (questId == null)
                throw new ArgumentNullException(nameof(questId));

            quest = default;

            if (this.QuestData == null)
            {
                Debug.LogWarning("Cannot get quest. No QuestAsset is assigned");
                return false;
            }

            quest = this.QuestData.GetQuest(questId);

            if (quest == null)
            {
                Debug.LogWarning($"Cannot find any quest by id={questId}");
                return false;
            }

            return true;
        }

        public void Invoke(string questId, int progress)
        {
            if (!TryGetQuest(questId, out var quest))
                return;

            ISegment<StageRow> stages;

            switch (quest.ProgressType)
            {
                case QuestRow.QuestProgressType.All:
                    stages = quest.GetStages();
                    break;

                case QuestRow.QuestProgressType.StartToCurrent:
                    stages = quest.GetStagesToCurrent(progress);
                    break;

                case QuestRow.QuestProgressType.OnlyCurrent:
                    stages = quest.GetCurrentStage(progress).AsSegment1();
                    break;

                default:
                    stages = quest.GetEmptyStages();
                    break;
            }

            foreach (var stage in stages)
            {
                if (CanSkip(stage, progress))
                    continue;

                Invoke(stage.Events.AsSegment(), progress);
            }
        }

        private void Invoke(in Segment<Event> events, int progress)
        {
            if (events.Count <= 0)
                return;

            if (this.EventSystem == null)
            {
                Debug.LogWarning("Cannot invoke events. No EventSystem is assigned");
                return;
            }

            this.EventSystem.Invoke(events, progress);
        }

        private static bool CanSkip(StageRow stage, int progress)
            => stage == null ||
               (stage.MaxConstraint >= 0 && progress > stage.MaxConstraint);
    }
}