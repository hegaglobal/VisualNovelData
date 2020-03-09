using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace VisualNovelData.Parser
{
    using Data;

    public sealed class NovelParser : CsvParser
    {
        private readonly Parser<VsnRow> parser;
        private readonly StringBuilder logger;
        private readonly EventParser eventParser;
        private readonly Segment<string> languages;

        public NovelParser(in Segment<string> languages)
        {
            this.languages = languages;
            this.eventParser = new EventParser();
            this.logger = new StringBuilder();

            var mapping = new VsnRow.Mapping(languages);
            this.parser = Create<VsnRow, VsnRow.Mapping>(mapping);
        }

        public NovelData Parse(string csvData)
        {
            this.logger.Clear();

            var row = 0;
            var error = string.Empty;
            var enumerator = this.parser.Parse(csvData).GetEnumerator();
            var data = new NovelData();
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
                                                        goToList, this.eventParser, this.logger);

                if (vsnRow.IsError)
                {
                    error = vsnRow.Error;
                    break;
                }
            }

            if (!string.IsNullOrEmpty(error))
            {
                Debug.LogError($"Vsn row {row}: {error}");
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