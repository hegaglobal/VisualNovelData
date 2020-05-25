using System.Collections.Generic;
using TinyCsvParser.Mapping;

namespace VisualNovelData.Parser
{
    public partial class VsnRow : CsvRow
    {
        public string Conversation { get; private set; }

        public string Dialogue { get; private set; }

        public float? Delay { get; private set; }

        public string Actor { get; private set; }

        public string Action { get; private set; }

        public int? Highlight { get; private set; }

        public int? Choice { get; private set; }

        public string GoTo { get; private set; }

        public string EventsOnStart { get; private set; }

        public string EventsOnEnd { get; private set; }

        public string[] Contents { get; private set; }

        public class Mapping : CsvMapping<VsnRow>
        {
            public const int ContentsStartIndex = 11;

            public Mapping(Segment<string> languages) : base()
            {
                var col = 0;

                MapProperty(++col, x => x.Conversation, (x, v) => x.Conversation = v);
                MapProperty(++col, x => x.Dialogue, (x, v) => x.Dialogue = v);
                MapProperty(++col, x => x.Delay, (x, v) => x.Delay = v);
                MapProperty(++col, x => x.Actor, (x, v) => x.Actor = v);
                MapProperty(++col, x => x.Action, (x, v) => x.Action = v);
                MapProperty(++col, x => x.Highlight, (x, v) => x.Highlight = v);
                MapProperty(++col, x => x.Choice, (x, v) => x.Choice = v);
                MapProperty(++col, x => x.GoTo, (x, v) => x.GoTo = v);
                MapProperty(++col, x => x.EventsOnStart, (x, v) => x.EventsOnStart = v);
                MapProperty(++col, x => x.EventsOnEnd, (x, v) => x.EventsOnEnd = v);

                if (languages.Count > 0)
                {
                    MapUsing((entity, values) => {
                        var length = languages.Count;
                        entity.Contents = new string[languages.Count];

                        for (var i = 0; i < length; i++)
                        {
                            var col2 = ContentsStartIndex + i;
                            entity.Contents[i] = values.Tokens[col2];
                        }
                    });
                }
            }
        }
    }
}