using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace VisualNovelData.Parser
{
    using Data;

    public sealed class L10nParser : CsvParser
    {
        private readonly Parser<VslRow> parser;
        private readonly StringBuilder logger;
        private readonly Segment<string> languages;

        public L10nParser(in Segment<string> languages)
        {
            this.languages = languages;

            var mapping = new VslRow.Mapping(languages);
            this.parser = Create<VslRow, VslRow.Mapping>(mapping);
            this.logger = new StringBuilder();
        }

        public L10nData Parse(string csvData)
        {
            this.logger.Clear();

            var row = 0;
            var error = string.Empty;
            var enumerator = this.parser.Parse(csvData).GetEnumerator();
            var data = new L10nData();

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
                return null;
            }

            if (this.logger.Length > 0)
            {
                Debug.LogError(this.logger);
            }

            return data;
        }
    }
}