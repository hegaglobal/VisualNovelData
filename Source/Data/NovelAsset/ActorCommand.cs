using System;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelData.Data
{
    [Serializable]
    public class ActorCommand : Command
    {
        [SerializeField]
        private int actorNumber = 0;

        public int ActorNumber
            => this.actorNumber;

        public override Metadata Metadata
            => this.actorNumber.AsMetadata();

        public ActorCommand(int actorNumber, string id, string key, int maxConstraint = -1, params string[] parameters)
            : base(id, key, maxConstraint, parameters)
        {
            this.actorNumber = actorNumber;
        }

        public ActorCommand(int actorNumber, string id, string key, int maxConstraint = -1, in Segment<string> parameters = default)
            : base(id, key, maxConstraint, parameters)
        {
            this.actorNumber = actorNumber;
        }

        public ActorCommand(int actorNumber, Command command)
            : this(actorNumber, command.Id, command.Key, command.MaxConstraint, command.Parameters.AsSegment())
        {
        }
    }
}