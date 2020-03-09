using UnityEngine;

namespace VisualNovelData.Parser
{
    using Data;

    public sealed class QuestParser : CsvParser
    {
        private readonly Parser<VsqRow> parser;
        private readonly EventParser eventParser;

        public QuestParser()
        {
            this.eventParser = new EventParser();

            var mapping = new VsqRow.Mapping();
            this.parser = Create<VsqRow, VsqRow.Mapping>(mapping);
        }

        public QuestData Parse(string csvData)
        {
            var row = 0;
            var error = string.Empty;
            var enumerator = this.parser.Parse(csvData).GetEnumerator();
            var data = new QuestData();

            QuestRow quest = null;

            while (enumerator.MoveNext())
            {
                if (!enumerator.Current.IsValid)
                {
                    error = enumerator.Current.Error.ToString();
                    break;
                }

                var vsqRow = enumerator.Current.Result;
                row = enumerator.Current.RowIndex + 1;

                quest = vsqRow.Parse(data, quest, this.eventParser, row);

                if (vsqRow.IsError)
                {
                    error = vsqRow.Error;
                    break;
                }
            }

            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogError($"Vsq row {row}: {error}");
                return null;
            }

            return data;
        }
    }
}