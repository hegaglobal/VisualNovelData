using System;
using UnityEngine;

namespace VisualNovelData.Data
{
    [Serializable]
    public sealed class L10nTextRow : DataRow
    {
        [SerializeField]
        private string id = string.Empty;

        public string Id
            => this.id;

        public int ContentId
            => this.Row;

        public L10nTextRow(int row, string id) : base(row)
        {
            this.id = id ?? string.Empty;
        }

        public static L10nTextRow None { get; } = new L10nTextRow(-1, string.Empty);
    }

    public static class L10nTextRowExtensions
    {
        public static bool IsNullOrNone(this L10nTextRow self)
            => self == null || self == L10nTextRow.None;
    }
}