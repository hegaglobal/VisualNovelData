using UnityEngine;

namespace VisualNovelData.Data
{
    [System.Serializable]
    public sealed class ChoiceRow : DataRow
    {
        [SerializeField]
        private int id = 0;

        public int Id
            => this.id;

        [SerializeField]
        private string goTo = string.Empty;

        public string GoTo
            => this.goTo;

        public int ContentId
            => this.Row;

        public ChoiceRow(int row, int id, string goTo) : base(row)
        {
            this.id = id;
            this.goTo = goTo ?? string.Empty;
        }

        public static ChoiceRow None { get; } = new ChoiceRow(-1, 0, string.Empty);
    }
}