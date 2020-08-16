using System.Text.RegularExpressions;
using UnityEngine;

namespace VisualNovelData.Parser
{
    using Data;

    public partial class VsqRow
    {
        private static readonly Regex _digitsRegex = new Regex(@"\d+", RegexOptions.Compiled);

        public override bool IsEmpty
            => !(this.HasQuest || this.HasStage || this.HasCommands);

        public bool IsQuestStart
            => this.HasQuest && !this.Quest.StartsWith('[') && !this.Quest.EndsWith(']') &&
               !(this.HasStage || this.HasCommands);

        public bool IsQuestEnd
            => this.HasQuest && this.Quest.StartsWith('[') && this.Quest.EndsWith(']') &&
               !(this.HasStage || this.HasCommands);

        public bool HasQuest
            => !string.IsNullOrEmpty(this.Quest);

        public bool HasStage
            => !string.IsNullOrEmpty(this.Stage) && _digitsRegex.IsMatch(this.Stage);

        public bool HasCommands
            => !string.IsNullOrEmpty(this.Commands);

        public override string ToString()
        {
            this.stringBuilder.Clear();
            this.stringBuilder.Append($"{this.Quest} - {this.Stage} - {this.Commands}");

            return this.stringBuilder.ToString();
        }

        public QuestRow Parse(IQuestData data, QuestRow quest, CommandParser commandParser, int row)
        {
            this.error.Clear();

            if (!this.IsEmpty)
            {
                if (this.IsQuestStart)
                    return ParseQuestStart(data, quest, row);

                if (this.IsQuestEnd)
                    return ParseQuestEnd(quest, data);

                if (this.HasStage)
                    ParseStage(quest, commandParser);
            }

            return quest;
        }

        private QuestRow ParseQuestStart(IQuestData data, QuestRow quest, int row)
        {
            var id = this.Quest;

            if (!this.idRegex.IsMatch(id))
            {
                this.error.AppendLine($"Quest id must only contain characters in [a-zA-Z0-9_]. Current value: {this.Quest}");
                return null;
            }

            if (quest != null && !quest.Id.Equals(id))
            {
                this.error.AppendLine($"Quest `{quest.Id}` must end before starting a new one");
                return quest;
            }

            if (!this.Stage.TryConvertProgressType(out var progressType))
            {
                this.error.AppendLine(string.Format("Invoke type can only be either empty, {0}, {1}, or {2}",
                                      ProgressTypeExtensions.ALL,
                                      ProgressTypeExtensions.START_TO_CURRENT,
                                      ProgressTypeExtensions.ONLY_CURRENT));
                return null;
            }

            if (data.Quests.ContainsKey(id))
            {
                Debug.LogWarning($"Vsq row {row}: Quest id has already existed");
            }

            return new QuestRow(row, id, progressType);
        }

        private QuestRow ParseQuestEnd(QuestRow quest, IQuestData data)
        {
            if (quest == null)
            {
                this.error.AppendLine("No quest");
                return quest;
            }

            if (string.IsNullOrEmpty(quest.Id))
            {
                this.error.AppendLine("Invalid quest id");
                return quest;
            }

            var start = $"[{quest.Id}]";
            var end = this.Quest;

            if (!start.Equals(end))
            {
                this.error.AppendLine("Mismatched quest start and end tokens");
                return quest;
            }

            data.AddQuest(quest);
            return null;
        }

        private void ParseStage(QuestRow quest, CommandParser commandParser)
        {
            var stage = 0;

            if (this.HasStage &&
                !int.TryParse(this.Stage, out stage))
            {
                this.error.AppendLine($"Cannot convert stage value to integer. Current value: {this.Stage}");
                return;
            }

            if (stage < 0)
            {
                this.error.AppendLine($"State value must be positive (stage >= 0). Current value: {stage}");
                return;
            }

            if (quest.ContainsStage(stage))
            {
                this.error.AppendLine($"Stage {stage} has already existed");
                return;
            }

            var maxConstraint = this.MaxConstraint ?? -1;
            var command = commandParser.Parse(this.Commands, this.error);

            if (this.IsError)
                return;

            quest.AddStage(stage, command, maxConstraint);
        }
    }
}