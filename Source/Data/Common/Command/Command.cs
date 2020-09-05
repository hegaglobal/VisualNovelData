using System;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelData.Data
{
    [Serializable]
    public partial class Command
    {
        [SerializeField]
        private string id = string.Empty;

        public string Id
            => this.id;

        [SerializeField]
        private string key = string.Empty;

        public string Key
            => this.key;

        [SerializeField]
        private int maxConstraint = -1;

        public int MaxConstraint
            => this.maxConstraint;

        [SerializeField]
        private ParameterList parameters = new ParameterList();

        public IParameterList Parameters
            => this.parameters;

        public virtual Metadata Metadata
            => Metadata.None;

        private IReadOnlyList<object> cachedObjectList;
        private Segment<object> cachedObjectSegment;

        public Segment<object> ObjectParameters
        {
            get
            {
                if (this.cachedObjectList == null)
                {
                    this.cachedObjectList = this.parameters;
                    this.cachedObjectSegment = this.cachedObjectList.AsSegment();
                }

                return this.cachedObjectSegment;
            }
        }

        public Command(string id, string key, int maxConstraint = -1, params string[] parameters)
        {
            Initialize(id, key, maxConstraint, parameters);
        }

        public Command(string id, string key, int maxConstraint = -1, in Segment<string> parameters = default)
        {
            Initialize(id, key, maxConstraint, parameters);
        }

        private void Initialize(string id, string key, int maxConstraint, in Segment<string> parameters)
        {
            this.id = id;
            this.key = key;
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
