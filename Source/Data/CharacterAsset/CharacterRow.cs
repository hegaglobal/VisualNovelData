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
        private string p1 = string.Empty;

        public string P1
            => this.p1;

        [SerializeField]
        private string p1Background = string.Empty;

        public string P1Background
            => this.p1Background;

        [SerializeField]
        private string p2 = string.Empty;

        public string P2
            => this.p2;

        [SerializeField]
        private string p2Background = string.Empty;

        public string P2Background
            => this.p2Background;

        [SerializeField]
        private string p3 = string.Empty;

        public string P3
            => this.p3;

        [SerializeField]
        private string p3Background = string.Empty;

        public string P3Background
            => this.p3Background;

        public int ContentId
            => this.Row;

        public CharacterRow(int row, string id, string avatar, string p1, string p1Background,
                            string p2, string p2Background, string p3, string p3Background) : base(row)
        {
            this.id = id ?? string.Empty;
            this.avatar = avatar ?? string.Empty;
            this.p1 = p1 ?? string.Empty;
            this.p1Background = p1Background ?? string.Empty;
            this.p2 = p2 ?? string.Empty;
            this.p2Background = p2Background ?? string.Empty;
            this.p3 = p3 ?? string.Empty;
            this.p3Background = p3Background ?? string.Empty;
        }

        public static CharacterRow None { get; } = new CharacterRow(-1, string.Empty, string.Empty, string.Empty, string.Empty,
                                                                    string.Empty, string.Empty, string.Empty, string.Empty);
    }
}