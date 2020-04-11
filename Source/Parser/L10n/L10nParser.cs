using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace VisualNovelData.Parser
{
    using Data;

    public sealed class L10nParser : CsvParser, ICsvParser<L10nData>
    {
        private readonly StringBuilder logger;

        private Parser<VslRow> parser;
        private Segment<string> languages;

        public L10nParser()
        {
            this.logger = new StringBuilder();
        }

        public L10nParser(in Segment<string> languages) : this()
        {
            Initialize(languages);
        }

        public void Initialize(in Segment<string> languages)
        {
            this.languages = languages;

            var mapping = new VslRow.Mapping(languages);
            this.parser = Create<VslRow, VslRow.Mapping>(mapping);
        }

        public L10nData Parse(string csvData)
        {
            var data = new L10nData();
            Parse(csvData, data);

            return data;
        }

        public void Parse(string csvData, L10nData data)
        {
            if (csvData == null)
                throw new ArgumentNullException(nameof(csvData));

            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (string.IsNullOrEmpty(csvData))
                return;

            this.logger.Clear();

            var row = 0;
            var error = string.Empty;
            var enumerator = this.parser.Parse(csvData).GetEnumerator();

            L10nTextRow text = null;

            while (enumerator.MoveNext())
            {
                if (!enumerator.Current.IsValid)
                {
                    error = enumerator.Current.Error.ToString();
                    break;
                }

                var vslRow = enumerator.Current.Result;
                row = enumerator.Current.RowIndex + 1;

                text = vslRow.Parse(data, text, this.languages, row);

                if (vslRow.IsError)
                {
                    error = vslRow.Error;
                    break;
                }
            }

            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogError($"Vsl row {row}: {error}");
                return;
            }

            if (this.logger.Length > 0)
            {
                Debug.LogError(this.logger);
            }
        }
    }
}