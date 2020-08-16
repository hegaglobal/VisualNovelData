using System;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelData.CommandSystems
{
    using Data;

    public sealed class QuestCommandSystem
    {
        public IQuestData QuestData { get; private set; }

        public CommandSystem CommandSystem { get; private set; }

        public QuestCommandSystem() { }

        public QuestCommandSystem(CommandSystem commandSystem)
            => Set(commandSystem);

        public QuestCommandSystem(IQuestData questData)
            => Set(questData);

        public QuestCommandSystem(IQuestData questData, CommandSystem commandSystem)
        {
            Set(questData);
            Set(commandSystem);
        }

        public void Set(IQuestData questData)
            => this.QuestData = questData ?? throw new ArgumentNullException(nameof(questData));

        public void Set(CommandSystem commandSystem)
            => this.CommandSystem = commandSystem ?? throw new ArgumentNullException(nameof(commandSystem));

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

                Invoke(stage.Commands.AsSegment(), progress);
            }
        }

        private void Invoke(in Segment<Command> commands, int progress)
        {
            if (commands.Count <= 0)
                return;

            if (this.CommandSystem == null)
            {
                Debug.LogWarning($"Cannot invoke commands. No {nameof(this.CommandSystem)} is assigned");
                return;
            }

            this.CommandSystem.Invoke(commands, progress);
        }

        private static bool CanSkip(StageRow stage, int progress)
            => stage == null ||
               (stage.MaxConstraint >= 0 && progress > stage.MaxConstraint);
    }
}