using System;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelData.Data
{
    [Serializable]
    public class EventRow : DataRow
    {
        [SerializeField]
        private string id = string.Empty;

        public string Id
            => this.id;

        [SerializeField]
        private EventInvokeType invokeType = EventInvokeType.All;

        public EventInvokeType InvokeType
            => this.invokeType;

        [SerializeField]
        private StageList stages = new StageList();

        public IStageList Stages
            => this.stages;

        public EventRow(int row, string id, EventInvokeType invokeType) : base(row)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            this.id = id;
            this.invokeType = invokeType;
        }

        public void AddStage(int stage, IReadOnlyList<Command> commands, int maxConstraint = -1)
        {
            if (commands == null)
                throw new ArgumentNullException(nameof(commands));

            var index = this.stages.Count;

            for (var i = this.stages.Count - 1; i >= 0; i--)
            {
                if (this.stages[i].Index < stage)
                    break;

                index = i;
            }

            this.stages.Insert(index, new StageRow(stage, commands, maxConstraint));
        }

        public StageRow GetStage(int stage)
        {
            for (var i = 0; i < this.stages.Count; i++)
            {
                if (this.stages[i].Index == stage)
                    return this.stages[i];
            }

            return StageRow.None;
        }

        public Segment<StageRow> GetEmptyStages()
            => this.stages.Slice(0, 0);

        public Segment<StageRow> GetStages()
            => this.stages;

        public Segment<StageRow> GetStages(int fromIndex, int toIndex)
        {
            if (fromIndex < 0 || fromIndex >= this.stages.Count)
                return GetEmptyStages();

            if (toIndex < 0 || toIndex >= this.stages.Count)
                return GetEmptyStages();

            if (fromIndex > toIndex)
            {
                Debug.LogWarning($"{nameof(fromIndex)} must be less than or equal to {nameof(toIndex)}");
                return GetEmptyStages();
            }

            if (fromIndex == toIndex)
                return GetEmptyStages();

            return this.stages.Slice(fromIndex, toIndex - fromIndex + 1);
        }

        public int GetCurrentStageIndex(int stage)
        {
            var index = -1;

            for (var i = 0; i < this.stages.Count; i++)
            {
                if (this.stages[i].Index == stage)
                {
                    index = i;
                    break;
                }

                if (this.stages[i].Index > stage)
                    break;

                index = i;
            }

            return index;
        }

        public StageRow GetCurrentStage(int stage)
        {
            var index = GetCurrentStageIndex(stage);
            return index >= 0 ? this.stages[index] : StageRow.None;
        }

        public Segment<StageRow> GetStagesToCurrent(int stage, int fromIndex = 0)
        {
            if (fromIndex < 0 || fromIndex >= this.stages.Count)
                return GetEmptyStages();

            var index = GetCurrentStageIndex(stage);
            var count = index - fromIndex + 1;

            return this.stages.Slice(fromIndex, count < 0 ? 0 : count);
        }

        public bool ContainsStage(int stage)
        {
            for (var i = 0; i < this.stages.Count; i++)
            {
                if (this.stages[i].Index == stage)
                    return true;
            }

            return false;
        }

        public void ClearStages()
            => this.stages.Clear();

        public static EventRow None { get; } = new EventRow(-1, string.Empty, EventInvokeType.All);

        [Serializable]
        private sealed class StageList : List<StageRow>, IStageList
        { }

        public enum EventInvokeType
        {
            /// <summary>
            /// Should invoke all stages
            /// </summary>
            All,

            /// <summary>
            /// Should invoke all the stages from start to the current stage
            /// </summary>
            StartToCurrent,

            /// <summary>
            /// Should only invoke the stage at the current stage
            /// </summary>
            OnlyCurrent
        }
    }
}