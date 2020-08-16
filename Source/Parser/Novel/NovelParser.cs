using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace VisualNovelData.Parser
{
    using Data;

    public sealed class NovelParser : CsvParser, ICsvParser<NovelData>
    {
        private readonly StringBuilder logger;
        private readonly CommandParser commandParser;
        private readonly IArrayParser<int> intArrayParser;

        private Parser<VsnRow> parser;
        private Segment<string> languages;

        public NovelParser()
        {
            this.logger = new StringBuilder();
            this.commandParser = new CommandParser();
            this.intArrayParser = new ArrayParser<int, IntParser>();
        }

        public NovelParser(in Segment<string> languages) : this()
        {
            Initialize(languages);
        }

        public void Initialize(in Segment<string> languages)
        {
            this.languages = languages;

            var mapping = new VsnRow.Mapping(languages);
            this.parser = Create<VsnRow, VsnRow.Mapping>(mapping);
        }

        public NovelData Parse(string csvData)
        {
            var data = new NovelData();
            Parse(csvData, data);

            return data;
        }

        public void Parse(string csvData, NovelData data)
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
            var goToList = new List<string>();

            ConversationRow conversation = null;
            DialogueRow dialogue = null;

            while (enumerator.MoveNext())
            {
                if (!enumerator.Current.IsValid)
                {
                    error = enumerator.Current.Error.ToString();
                    break;
                }

                var vsnRow = enumerator.Current.Result;
                row = enumerator.Current.RowIndex + 1;

                (conversation, dialogue) = vsnRow.Parse(data, conversation, dialogue, this.languages, row,
                                                        goToList, this.commandParser, this.intArrayParser, this.logger);

                if (vsnRow.IsError)
                {
                    error = vsnRow.Error;
                    break;
                }
            }

            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogError($"Vsn row {row}: {error}");
                return;
            }

            if (this.logger.Length > 0)
            {
                Debug.LogError(this.logger);
            }
        }
    }
}