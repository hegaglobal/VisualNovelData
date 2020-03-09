using System;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelData.Data
{
    [Serializable]
    public sealed partial class Event
    {
        [SerializeField]
        private string id = string.Empty;

        public string Id
            => this.id;

        [SerializeField]
        private string type = string.Empty;

        public string Type
            => this.type;

        [SerializeField]
        private int maxConstraint = -1;

        public int MaxConstraint
            => this.maxConstraint;

        [SerializeField]
        private ParameterList parameters = new ParameterList();

        public IParameterList Parameters
            => this.parameters;

        private IReadOnlyList<object> cachedObjectList;
        private Segment<object> cachedObjectSegment;

        public Segment<object> ObjectParameters
        {
            get
            {
                if (this.cachedObjectList == null)
                {
                    this.cachedObjectList = this.parameters as IReadOnlyList<object>;
                    this.cachedObjectSegment = this.cachedObjectList.AsSegment();
                }

                return this.cachedObjectSegment;
            }
        }

        public Event(string id, string type, int maxConstraint = -1, params string[] parameters)
        {
            Initialize(id, type, maxConstraint, parameters);
        }

        public Event(string id, string type, int maxConstraint = -1, in Segment<string> parameters = default)
        {
            Initialize(id, type, maxConstraint, parameters);
        }

        private void Initialize(string id, string type, int maxConstraint, in Segment<string> parameters)
        {
            this.id = id;
            this.type = type;
            this.maxConstraint = maxConstraint;
            this.parameters.AddRange(parameters);
        }

        public override string ToString()
            => this.id;

        [Serializable]
        private sealed class ParameterList : List<string>, IParameterList
        { }
    }
}
