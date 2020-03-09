using System;
using UnityEngine;

namespace VisualNovelData.Data
{
    [Serializable]
    public abstract class DataRow : IDataRow
    {
        [SerializeField]
        private int row = 0;

        public int Row
            => this.row;

        public DataRow(int row)
        {
            this.row = row;
        }
    }
}