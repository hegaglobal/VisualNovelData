using System;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelData.Parser
{
    using Data;

    public sealed class QuestParser : CsvParser, ICsvParser<QuestData>
    {
        private readonly Parser<VsqRow> parser;
        private readonly EventParser eventParser;

        public QuestParser()
        {
            this.eventParser = new EventParser();

            var mapping = new VsqRow.Mapping();
            this.parser = Create<VsqRow, VsqRow.Mapping>(mapping);
        }

        public void Initialize(in Segment<string> languages)
        {
        }

        public QuestData Parse(string csvData)
        {
            var data = new QuestData();
            Parse(csvData, data);

            return data;
        }

        public void Parse(string csvData, QuestData data)
        {
            if (csvData == null)
                throw new ArgumentNullException(nameof(csvData));

            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (string.IsNullOrEmpty(csvData))
                return;

            var row = 0;
            var error = string.Empty;
            var enumerator = this.parser.Parse(csvData).GetEnumerator();

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
            }
        }
    }
}