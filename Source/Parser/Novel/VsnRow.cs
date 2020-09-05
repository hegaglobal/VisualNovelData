using System.Collections.Generic;
using TinyCsvParser.Mapping;

namespace VisualNovelData.Parser
{
    public partial class VsnRow : CsvRow
    {
        public string Conversation { get; private set; }

        public string Dialogue { get; private set; }

        public float? Delay { get; private set; }

        public int? Choice { get; private set; }

        public string GoTo { get; private set; }

        public string Speaker { get; private set; }

        public string Actor1 { get; private set; }

        public string Actions1 { get; private set; }

        public string Actor2 { get; private set; }

        public string Actions2 { get; private set; }

        public string Actor3 { get; private set; }

        public string Actions3 { get; private set; }

        public string Actor4 { get; private set; }

        public string Actions4 { get; private set; }

        public string Highlight { get; private set; }

        public string CommandsOnStart { get; private set; }

        public string CommandsOnEnd { get; private set; }

        public string[] Contents { get; private set; }

        public class Mapping : CsvMapping<VsnRow>
        {
            public const int ContentsStartIndex = 18;

            public Mapping(Segment<string> languages) : base()
            {
                var col = 0;

                MapProperty(++col, x => x.Conversation, (x, v) => x.Conversation = v);
                MapProperty(++col, x => x.Dialogue, (x, v) => x.Dialogue = v);
                MapProperty(++col, x => x.Delay, (x, v) => x.Delay = v);
                MapProperty(++col, x => x.Choice, (x, v) => x.Choice = v);
                MapProperty(++col, x => x.GoTo, (x, v) => x.GoTo = v);
                MapProperty(++col, x => x.Speaker, (x, v) => x.Speaker = v);
                MapProperty(++col, x => x.Actor1, (x, v) => x.Actor1 = v);
                MapProperty(++col, x => x.Actions1, (x, v) => x.Actions1 = v);
                MapProperty(++col, x => x.Actor2, (x, v) => x.Actor2 = v);
                MapProperty(++col, x => x.Actions2, (x, v) => x.Actions2 = v);
                MapProperty(++col, x => x.Actor3, (x, v) => x.Actor3 = v);
                MapProperty(++col, x => x.Actions3, (x, v) => x.Actions3 = v);
                MapProperty(++col, x => x.Actor4, (x, v) => x.Actor4 = v);
                MapProperty(++col, x => x.Actions4, (x, v) => x.Actions4 = v);
                MapProperty(++col, x => x.Highlight, (x, v) => x.Highlight = v);
                MapProperty(++col, x => x.CommandsOnStart, (x, v) => x.CommandsOnStart = v);
                MapProperty(++col, x => x.CommandsOnEnd, (x, v) => x.CommandsOnEnd = v);

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