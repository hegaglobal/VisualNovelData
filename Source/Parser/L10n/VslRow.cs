using System.Collections.Generic;
using TinyCsvParser.Mapping;

namespace VisualNovelData.Parser
{
    public sealed partial class VslRow : CsvRow
    {
        public string Key { get; private set; }

        public string[] Contents { get; private set; }

        public class Mapping : CsvMapping<VslRow>
        {
            public const int ContentsStartIndex = 2;

            public Mapping(Segment<string> languages) : base()
            {
                var col = 0;

                MapProperty(++col, x => x.Key, (x, v) => x.Key = v);

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