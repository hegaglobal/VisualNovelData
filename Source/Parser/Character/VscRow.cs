﻿using System.Collections.Generic;
using TinyCsvParser.Mapping;

namespace VisualNovelData.Parser
{
    public sealed partial class VscRow : CsvRow
    {
        public string Character { get; private set; }

        public string Avatar { get; private set; }

        public string Model { get; private set; }

        public string[] Contents { get; private set; }

        public class Mapping : CsvMapping<VscRow>
        {
            public const int ContentsStartIndex = 4;

            public Mapping(Segment<string> languages) : base()
            {
                var col = 0;

                MapProperty(++col, x => x.Character, (x, v) => x.Character = v);
                MapProperty(++col, x => x.Avatar, (x, v) => x.Avatar = v);
                MapProperty(++col, x => x.Model, (x, v) => x.Model = v);

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