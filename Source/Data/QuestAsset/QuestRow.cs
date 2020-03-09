using System;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelData.Data
{
    [Serializable]
    public class QuestRow : DataRow
    {
        private readonly static Segment<StageRow> _empty = Segment<StageRow>.Empty;

        [SerializeField]
        private string id = string.Empty;

        public string Id
            => this.id;

        [SerializeField]
        private QuestProgressType progressType = QuestProgressType.StartToCurrent;

        public QuestProgressType ProgressType
            => this.progressType;

        [SerializeField]
        private QuestProgress progress = new QuestProgress();

        public IQuestProgress Progress
            => this.progress;

        public QuestRow(int row, string id, QuestProgressType progressType) : base(row)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            this.id = id;
            this.progressType = progressType;
        }

        public void AddStage(int stage, IReadOnlyList<Event> events, int maxConstraint = -1)
        {
            if (events == null)
                throw new ArgumentNullException(nameof(events));

            var index = this.progress.Count;

            for (var i = this.progress.Count - 1; i >= 0; i--)
            {
                if (this.progress[i].Index < stage)
                    break;

                index = i;
            }

            this.progress.Insert(index, new StageRow(stage, events, maxConstraint));
        }

        public StageRow GetStage(int stage)
        {
            for (var i = 0; i < this.progress.Count; i++)
            {
                if (this.progress[i].Index == stage)
                    return this.progress[i];
            }

            return null;
        }

        public Segment<StageRow> GetStages(int fromIndex, int toIndex)
        {
            if (fromIndex < 0 || fromIndex >= this.progress.Count)
                return _empty;

            if (toIndex < 0 || toIndex >= this.progress.Count)
                return _empty;

            if (fromIndex > toIndex)
            {
                Debug.LogWarning($"{nameof(fromIndex)} must be less than or equal to {nameof(toIndex)}");
                return _empty;
            }

            return this.progress.Slice(fromIndex, toIndex - fromIndex + 1);
        }

        public int GetCurrentStageIndex(int progress)
        {
            var index = -1;

            for (var i = 0; i < this.progress.Count; i++)
            {
                if (this.progress[i].Index == progress)
                {
                    index = i;
                    break;
                }

                if (this.progress[i].Index > progress)
                    break;

                index = i;
            }

            return index;
        }

        public StageRow GetCurrentStage(int progress)
        {
            var index = GetCurrentStageIndex(progress);
            return index >= 0 ? this.progress[index] : null;
        }

        public Segment<StageRow> GetStagesToCurrent(int progress, int fromIndex = 0)
        {
            if (fromIndex < 0 || fromIndex >= this.progress.Count)
                return _empty;

            var index = GetCurrentStageIndex(progress);
            var count = index - fromIndex + 1;

            return this.progress.Slice(fromIndex, count < 0 ? 0 : count);
        }

        public bool ContainsStage(int stage)
        {
            for (var i = 0; i < this.progress.Count; i++)
            {
                if (this.progress[i].Index == stage)
                    return true;
            }

            return false;
        }

        public void ClearProgress()
            => this.progress.Clear();

        [Serializable]
        private class QuestProgress : Collections.List<StageRow>, IQuestProgress
        { }

        public enum QuestProgressType
        {
            /// <summary>
            /// Should invoke all the stages from start to the current progress
            /// </summary>
            StartToCurrent = 0,

            /// <summary>
            /// Should only invoke the stage at the current progress
            /// </summary>
            OnlyCurrent = 1
        }
    }
}