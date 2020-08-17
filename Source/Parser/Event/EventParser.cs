using System;
using System.Collections.Generic;
using UnityEngine;

namespace VisualNovelData.Parser
{
    using Data;

    public sealed class EventParser : CsvParser, ICsvParser<EventData>
    {
        private readonly Parser<VseRow> parser;
        private readonly CommandParser commandParser;

        public EventParser()
        {
            this.commandParser = new CommandParser();

            var mapping = new VseRow.Mapping();
            this.parser = Create<VseRow, VseRow.Mapping>(mapping);
        }

        public void Initialize(in Segment<string> languages)
        {
        }

        public EventData Parse(string csvData)
        {
            var data = new EventData();
            Parse(csvData, data);

            return data;
        }

        public void Parse(string csvData, EventData data)
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

            EventRow @event = null;

            while (enumerator.MoveNext())
            {
                if (!enumerator.Current.IsValid)
                {
                    error = enumerator.Current.Error.ToString();
                    break;
                }

                var vseRow = enumerator.Current.Result;
                row = enumerator.Current.RowIndex + 1;

                @event = vseRow.Parse(data, @event, this.commandParser, row);

                if (vseRow.IsError)
                {
                    error = vseRow.Error;
                    break;
                }
            }

            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogError($"Vse row {row}: {error}");
            }
        }
    }
}