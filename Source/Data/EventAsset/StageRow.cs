using System;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelData.Data
{
    [Serializable]
    public class StageRow
    {
        [SerializeField]
        private int index = 0;

        public int Index
            => this.index;

        [SerializeField]
        private int maxConstraint = -1;

        /// <summary>
        /// The maximum value at which this stage is still valid to be invoked.
        /// Negative value (less than 0) means no constraint.
        /// </summary>
        public int MaxConstraint
            => this.maxConstraint;

        [SerializeField]
        private CommandList commands = new CommandList();

        public ICommandList Commands
            => this.commands;

        public StageRow(int stage, IReadOnlyList<Command> commands, int maxConstraint = -1)
        {
            this.index = stage;

            if (commands != null)
            {
                this.commands.AddRange(commands);
            }

            this.maxConstraint = maxConstraint;
        }

        public static StageRow None { get; } = new StageRow(-1, null);
    }

    public static class StageRowExtensions
    {
        public static bool IsNullOrNone(this StageRow self)
            => self == null || self == StageRow.None;
    }
}