using System.Collections.Generic;
using TinyCsvParser.Mapping;

namespace VisualNovelData.Parser
{
    public sealed partial class VscRow : CsvRow
    {
        public string Character { get; private set; }

        public string Avatar { get; private set; }

        public string P1 { get; private set; }

        public string P1Background { get; private set; }

        public string P2 { get; private set; }

        public string P2Background { get; private set; }

        public string P3 { get; private set; }

        public string P3Background { get; private set; }

        public string[] Contents { get; private set; }

        public class Mapping : CsvMapping<VscRow>
        {
            public const int ContentsStartIndex = 5;

            public Mapping(Segment<string> languages) : base()
            {
                var col = 0;

                MapProperty(++col, x => x.Character, (x, v) => x.Character = v);
                MapProperty(++col, x => x.Avatar, (x, v) => x.Avatar = v);
                MapProperty(++col, x => x.P1, (x, v) => x.P1 = v);
                MapProperty(++col, x => x.P1Background, (x, v) => x.P1Background = v);
                MapProperty(++col, x => x.P2, (x, v) => x.P2 = v);
                MapProperty(++col, x => x.P2Background, (x, v) => x.P2Background = v);
                MapProperty(++col, x => x.P3, (x, v) => x.P3 = v);
                MapProperty(++col, x => x.P3Background, (x, v) => x.P3Background = v);

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