using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace VisualNovelData.Parser
{
    using Data;

    public sealed class CharacterParser : CsvParser
    {
        private readonly Parser<VscRow> parser;
        private readonly StringBuilder logger;
        private readonly Segment<string> languages;

        public CharacterParser(in Segment<string> languages)
        {
            this.languages = languages;

            var mapping = new VscRow.Mapping(languages);
            this.parser = Create<VscRow, VscRow.Mapping>(mapping);
            this.logger = new StringBuilder();
        }

        public CharacterData Parse(string csvData)
        {
            var data = new CharacterData();
            Parse(csvData, data);

            return data;
        }

        public void Parse(string csvData, CharacterData data)
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

            CharacterRow character = null;

            while (enumerator.MoveNext())
            {
                if (!enumerator.Current.IsValid)
                {
                    error = enumerator.Current.Error.ToString();
                    break;
                }

                var vscRow = enumerator.Current.Result;
                row = enumerator.Current.RowIndex + 1;

                character = vscRow.Parse(data, character, this.languages, row);

                if (vscRow.IsError)
                {
                    error = vscRow.Error;
                    break;
                }
            }

            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogError($"Vsc row {row}: {error}");
                return;
            }

            if (this.logger.Length > 0)
            {
                Debug.LogError(this.logger);
            }
        }
    }
}