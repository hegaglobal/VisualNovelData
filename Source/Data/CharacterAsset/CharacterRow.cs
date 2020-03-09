using System;
using UnityEngine;

namespace VisualNovelData.Data
{
    [Serializable]
    public sealed class CharacterRow : DataRow
    {
        [SerializeField]
        private string id = string.Empty;

        public string Id
            => this.id;

        [SerializeField]
        private string avatar = string.Empty;

        public string Avatar
            => this.avatar;

        [SerializeField]
        private string model = string.Empty;

        public string Model
            => this.model;

        public int ContentId
            => this.Row;

        public CharacterRow(int row, string id, string avatar, string model) : base(row)
        {
            this.id = id ?? string.Empty;
            this.avatar = avatar;
            this.model = model ?? string.Empty;
        }
    }
}