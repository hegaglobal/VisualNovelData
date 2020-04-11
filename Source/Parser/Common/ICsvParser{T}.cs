using System;
using System.Collections.Generic;
using TinyCsvParser;
using TinyCsvParser.Mapping;
using TinyCsvParser.Tokenizer.RFC4180;

namespace VisualNovelData.Parser
{
    public interface ICsvParser<T> where T : class
    {
        void Initialize(in Segment<string> languages);

        T Parse(string csvData);

        void Parse(string csvData, T data);
    }
}