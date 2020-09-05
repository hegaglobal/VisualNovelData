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

        public ActorCommand(int actorNumber, string id, string type, int maxConstraint = -1, params string[] parameters)
            : base(id, type, maxConstraint, parameters)
        {
            this.actorNumber = actorNumber;
        }

        public ActorCommand(int actorNumber, string id, string type, int maxConstraint = -1, in Segment<string> parameters = default)
            : base(id, type, maxConstraint, parameters)
        {
            this.actorNumber = actorNumber;
        }

        public ActorCommand(int actorNumber, Command command)
            : this(actorNumber, command.Id, command.Type, command.MaxConstraint, command.Parameters.AsSegment())
        {
        }
    }
}